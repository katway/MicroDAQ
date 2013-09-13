using System;
using System.Collections.Generic;
using System.Text;
using Modbus.Device;
using Modbus.Utility;
using System.Data;
using System.Data.SqlClient;
using MicroDAQ.Common;
namespace MicroDAQ.Gateways.Modbus
{
    public class SerialPortMasterManager
    {
        public IList<Item> Items { get; set; }
        IModbusMaster SerialMaster;
        DataTable dtMeta;
        DataTable dtCommands;
        DataTable dtWriteData;
        byte slaveAddress;
        public SqlConnection Connection { get; set; }
        /// <summary>
        /// 构造函数 初始化变量
        /// </summary>
        /// <param name="master"></param>
        /// <param name="slave"></param>
        /// <param name="commandsData"></param>
        /// <param name="metaData"></param>
        public SerialPortMasterManager(IModbusMaster master, int slave, DataTable commandsData, DataTable metaData, DataTable writeData)
        {
            string ConnectionString = "server=.\\sqlexpress;database=Modbusdb;uid=sa;pwd=sa";
            Connection = new SqlConnection(ConnectionString);
            SerialMaster = master;
            master.Transport.ReadTimeout = 300;
            slaveAddress = Convert.ToByte(slave);
            dtCommands = commandsData;
            dtMeta = metaData;
            dtWriteData = writeData;
            Items = new List<Item>();
            for (int i = 0; i < metaData.Rows.Count; i++)
            {
                Items.Add(new Item());
            }
        }
        /// <summary>
        /// 读数据
        /// </summary>
        public void Read()
        {
            int flag = 0;//items索引

            for (int i = 0; i < dtCommands.Rows.Count; i++)
            {

                string regesiter = dtCommands.Rows[i]["RegisterName"].ToString();
                ushort adress = Convert.ToUInt16(dtCommands.Rows[i]["RegesiterAddress"]);
                ushort length = Convert.ToUInt16(dtCommands.Rows[i]["Length"]);
                string serialID = dtCommands.Rows[i]["SerialID"].ToString();
                DataRow[] rows = dtMeta.Select("ModbusCommands_SerialID=" + "'" + serialID + "'", "Address ASC");//一条命令所对应的原始数据表数据
                ushort[] values = new ushort[length];
                int index = 0;//values 索引
                try
                {
                    if (regesiter.ToLower() == "holdingregister")
                    { values = SerialMaster.ReadHoldingRegisters(slaveAddress, adress, length); }
                    else
                    { values = SerialMaster.ReadInputRegisters(slaveAddress, adress, length); }
                    //存储过程
                    // ProCommandState(serialID, "true");
                }
                catch
                {
                    //存储过程
                    // ProCommandState(serialID, "false");
                    continue;
                }
                for (int j = 0; j < rows.Length; j++)
                {
                    if (rows[j]["Final"].ToString() == "1")
                    {
                        Items[flag].Value = Convert.ToSingle(values[index]);
                        Items[flag].ID = Convert.ToInt32(rows[j]["Code"]);
                        index += 1;
                    }
                    else
                    {
                        ushort high;
                        ushort low;
                        if (rows[j]["Arithmetic"].ToString().ToLower() == "getfloatmsb")
                        {
                            high = values[index];
                            low = values[index + 1];
                        }
                        else
                        {
                            low = values[index];
                            high = values[index + 1];
                        }
                        float value = ushortToFloat(high, low);
                        Items[flag].Value = value;
                        Items[flag].ID = Convert.ToInt32(rows[j]["Code"]);
                        index += 2;
                    }
                    flag += 1;
                }
            }
        }
        public void Write()
        {
            for (int i = 0; i < dtWriteData.Rows.Count; i++)
            {
                string type = dtWriteData.Rows[i]["type"].ToString();
                string regesiter = dtWriteData.Rows[i]["RegisterName"].ToString();
                ushort adress = Convert.ToUInt16(dtWriteData.Rows[i]["RegesiterAddress"]);
                ushort value = Convert.ToUInt16(dtWriteData.Rows[i]["cycle"]);
                try
                {
                    ushort[] shorts;
                    if (type == "WLT")
                    {

                        if (value == 1)
                        {
                            shorts = new ushort[4] { 1, 0, 0, 0 };
                        }
                        else if (value == 2)
                        {
                            shorts = new ushort[4] { 0, 1, 0, 0 };
                        }
                        else if (value == 4)
                        {
                            shorts = new ushort[4] { 0, 0, 1, 0 };
                        }
                        else
                        {
                            shorts = new ushort[4] { 1, 0, 0, 1 };
                        }
                        SerialMaster.WriteMultipleRegisters(slaveAddress, adress, shorts);
                    }
                    else
                    {
                        shorts = new ushort[1] { value };
                        SerialMaster.WriteMultipleRegisters(slaveAddress, adress, shorts);
                    }
                }
                catch
                { }
            }
        }
        public void ReadWriteData()
        {
            Read();
            Write();
        }
        /// <summary>
        /// 两个ushort转为float
        /// </summary>
        /// <param name="highNumber"></param>
        /// <param name="lowNumber"></param>
        /// <returns></returns>
        private float ushortToFloat(ushort highNumber, ushort lowNumber)
        {
            return ModbusUtility.GetSingle(highNumber, lowNumber);
        }
        /// <summary>
        /// 日志存储过程
        /// </summary>
        /// <param name="serialID"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        private int ProCommandState(int serialID, string state)
        {
            SqlCommand command = new SqlCommand("proc_RecordCommandLog", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@serialID ", SqlDbType.Int));
            command.Parameters.Add(new SqlParameter("@State ", SqlDbType.VarChar, 10));
            command.UpdatedRowSource = UpdateRowSource.None;
            command.Parameters["@serialID "].Value = serialID;
            command.Parameters["@State "].Value = state;
            Connection.Open();
            int i = command.ExecuteNonQuery();
            Connection.Close();
            return i;
        }
    }
}
