using System;
using System.Collections.Generic;
using System.Text;
using Modbus.Device;
using System.Data;
using System.Data.SqlClient;
namespace MicroDAQ.DataItem
{
    class SerialPortMasterManager
    {
        public IList<Item> Items { get; set; }
        IModbusMaster SerialMaster;
        DataTable dtMeta;
        DataTable dtCommands;
        byte slaveAddress;
        public SqlConnection Connection { get; set; }
        public SerialPortMasterManager(IModbusMaster master,int slave, DataTable commandsData, DataTable metaData)
        {
            string ConnectionString = "server=.\\sqlexpress;database=Modbusdb;uid=sa;pwd=sa";
            Connection = new SqlConnection(ConnectionString);
            SerialMaster = master;
            master.Transport.ReadTimeout = 300;
            slaveAddress = Convert.ToByte(slave);
            dtCommands = commandsData;
            dtMeta = metaData;
            Items=new List<Item>();
            for (int i = 0; i < metaData.Rows.Count; i++)
            {
                Items.Add(new Item());
            }
           
        }

        public void Read()
        {
            int flag = 0;//items索引

            for (int i = 0; i < dtCommands.Rows.Count; i++)
            {

                string regesiter = dtCommands.Rows[i]["RegisterName"].ToString();
                ushort adress = Convert.ToUInt16(dtCommands.Rows[i]["RegesiterAddress"]);
                ushort length = Convert.ToUInt16(dtCommands.Rows[i]["Length"]);
                int serialID = Convert.ToInt32(dtCommands.Rows[i]["SerialID"]);
                DataRow[] rows = dtMeta.Select("ModbusCommands_SerialID=" + serialID,"Address ASC");
                ushort[] values = new ushort[length];
                int index=0;//values 索引
                try
                {
                    if (regesiter.ToLower() == "holdingregister")
                    { values = SerialMaster.ReadHoldingRegisters(slaveAddress, adress, length); }
                    else
                    { values = SerialMaster.ReadInputRegisters(slaveAddress, adress, length); }
                    //存储过程
                    ProCommandState(serialID, "true");
                }
                catch
                {
                    //存储过程
                    ProCommandState(serialID, "false");
                    return;
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
                         if (rows[j]["Arithmetic"].ToString() == "高")
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

        private float ushortToFloat(ushort highNumber,ushort lowNumber)
        {
            byte[] high = BitConverter.GetBytes(highNumber);
            byte[] low = BitConverter.GetBytes(lowNumber);
            byte[] allByte = new byte[4];
            allByte[0] = high[0];
            allByte[1] = high[1];
            allByte[2] = low[0];
            allByte[3] = low[1];
           return  BitConverter.ToSingle(allByte, 0);
        }
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
