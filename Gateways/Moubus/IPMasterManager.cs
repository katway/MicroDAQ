using System;
using System.Collections.Generic;
using System.Text;
using Modbus.Device;
using System.Data;
using System.Data.SqlClient;
using MicroDAQ.Common;
using Modbus.Utility;

namespace MicroDAQ.Gateways.Modbus
{
    public class IPMasterManager : IDataItemManage
    {
        public IList<Item> Items { get; set; }
        ModbusIpMaster IpMaster;
        DataTable dtMeta;
        DataTable dtCommands;
        string device_ID;
        byte slaveAddress;
        public SqlConnection Connection { get; set; }
        public IPMasterManager(ModbusIpMaster master, int slave, DataTable commandsData, DataTable metaData,  string deviceID)
        {
            string ConnectionString = "server=VWINTECH-201\\SQL2000;database=opcmes3;uid=sa;pwd= ";
            Connection = new SqlConnection(ConnectionString);
            IpMaster = master;
            master.Transport.ReadTimeout = 300;
            slaveAddress = Convert.ToByte(slave);
            dtCommands = commandsData;
            dtMeta = metaData;
            device_ID = deviceID;
            Items = new List<Item>();
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
                string serialID = dtCommands.Rows[i]["SerialID"].ToString();
                DataRow[] rows = dtMeta.Select("ModbusCommands_SerialID='" + serialID + "'", "Address ASC");
                ushort[] values = new ushort[length];
                int index = 0;//values 索引
                try
                {
                    if (regesiter.ToLower() == "holdingregister")
                    { values = IpMaster.ReadHoldingRegisters(slaveAddress, adress, length); }
                    else
                    { values = IpMaster.ReadInputRegisters(slaveAddress, adress, length); }
                    //存储过程
                    // ProCommandState(serialID, "true");
                }
                catch
                {
                    //存储过程
                    // ProCommandState(serialID, "false");
                    for (int j = 0; j < rows.Length; j++)
                    {
                        Items[flag].ID = Convert.ToInt32(rows[j]["Code"]);
                        Items[flag].DataTime = DateTime.Now;
                        Items[flag].State = ItemState.仪表掉线;
                        flag = flag + 1;
                    }
                    continue;

                }
                for (int j = 0; j < rows.Length; j++)
                {
                    if (rows[j]["Final"].ToString() == "1")
                    {
                        Items[flag].Value = Convert.ToSingle(values[index]);
                        Items[flag].ID = Convert.ToInt32(rows[j]["Code"]);
                        Items[flag].DataTime = DateTime.Now;
                        Items[flag].State =ItemState.正常;
                        index += 1;
                    }
                    else
                    {
                        ushort high;
                        ushort low;
                        float value;
                        string type = rows[j]["Arithmetic"].ToString().ToLower();
                        switch (type)
                        {
                            case "getfloatmsb":
                                high = values[index];
                                low = values[index + 1];
                                value = ModbusUtility.GetSingle(high, low);
                                break;

                            case "getfloatlsb":
                                low = values[index];
                                high = values[index + 1];
                                value = ModbusUtility.GetSingle(high, low);
                                break;

                            case "getuintmsb":
                                high = values[index];
                                low = values[index + 1];
                                value = ModbusUtility.GetUInt32(high, low);
                                break;

                            case "getuintlsb":
                                low = values[index];
                                high = values[index + 1];
                                value = ModbusUtility.GetUInt32(high, low);
                                break;
                            default:
                                value = 0;
                                break;

                        }
                       
                        Items[flag].Value = value;
                        Items[flag].ID = Convert.ToInt32(rows[j]["Code"]);
                        Items[flag].DataTime = DateTime.Now;
                        Items[flag].State = ItemState.正常;
                        index += 2;
                    }
                    flag += 1;
                }
            }
        }
        public void Write()
        {
            DataTable dtWriteData = GetWriteCommandsByID(device_ID);
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
                        IpMaster.WriteMultipleRegisters(slaveAddress, adress, shorts);
                    }
                    else
                    {
                        shorts = new ushort[1] { value };
                        IpMaster.WriteMultipleRegisters(slaveAddress, adress, shorts);

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
        /// 查询写命令
        /// </summary>
        /// <param name="deviceID"></param>
        /// <returns></returns>

        private DataTable GetWriteCommandsByID(string deviceID)
        {
            string sqlStr = "select * from modubs_control a where a.SerialID=" + "'" + deviceID + "'";
            Connection.Open();
            SqlDataAdapter da = new SqlDataAdapter(sqlStr, Connection);
            DataSet ds = new DataSet();
            da.Fill(ds);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                if (row["type"].ToString() == "CTR")
                {
                    string sql = string.Format("Update remotecontrol SET cmdstate= {0} WHERE slave= {1}", 2, row["code"].ToString());//, Connection);
                    SqlCommand Command = new SqlCommand(sql, Connection);
                    Command.ExecuteNonQuery();
                }
            }

            Connection.Close();
            return ds.Tables[0];
        }
        private int ProCommandState(int serialID, string state)
        {
            SqlCommand command = new SqlCommand("proc_RecordCommandLog", Connection);
            Connection.Open();
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@serialID ", SqlDbType.Int));
            command.Parameters.Add(new SqlParameter("@State ", SqlDbType.VarChar, 10));
            command.UpdatedRowSource = UpdateRowSource.None;
            command.Parameters["@serialID "].Value = serialID;
            command.Parameters["@State "].Value = state;
            int i = command.ExecuteNonQuery();
            Connection.Close();
            return i;

        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
