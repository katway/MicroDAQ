using System;
using System.Collections.Generic;
using System.Text;
using Modbus.Device;
using Modbus.Utility;
using System.Data;
using System.Data.SqlClient;
using MicroDAQ.Common;
using MicroDAQ.Configuration;
using System.Data;
namespace MicroDAQ.Gateways.Modbus2
{
    public class SerialPortSlaveAgent
    {

        /// <summary>
        /// 根据Master对象信息和Salve配置信息生成对象
        /// </summary>
        /// <param name="masterAgent">所属Master对象</param>
        /// <param name="slaveInfo">配置信息</param>
        public SerialPortSlaveAgent(ModbusMasterAgent masterAgent, ModbusSlaveInfo slaveInfo)
        {
            ///上属Master相关
            this.ModbusMasterAgent = masterAgent;
            switch (slaveInfo.type.ToUpper())
            {
                case "MODBUSRTU":
                    this.ModbusMasterAgent.ModbusMaster
                        = ModbusSerialMaster.CreateRtu(this.ModbusMasterAgent.SerialPort);
                    break;
                case "MODBUSASCII":
                    this.ModbusMasterAgent.ModbusMaster
                       = ModbusSerialMaster.CreateAscii(this.ModbusMasterAgent.SerialPort);
                    break;
                default:
                    throw new InvalidOperationException(string.Format("无法识别的Modbus从机类型-{0}", slaveInfo.type));

            }

            ///自身Slave相关
            this.ModbusSlaveInfo = slaveInfo;

            ///下属Variable相关
            this.Variables = new List<ModbusVariable>();
            foreach (var var in this.ModbusSlaveInfo.modbusVariables)
            {
                this.Variables.Add(new ModbusVariable(var));
            }
        }

        /// <summary>
        /// 读数据
        /// </summary>
        public void Read()
        {
            for (int i = 0; i < this.Variables.Count; i++)
            {
                ModbusVariable variable = Variables[i];

                ushort[] tmpVal = new ushort[variable.VariableInfo.length];
                if (variable.VariableInfo.accessibility != "WriteOnly")
                {
                    ///取出数据
                    switch (variable.VariableInfo.regesiterType)
                    {
                        //TODO:需要修改以下判断值
                        case 3:
                            tmpVal = this.ModbusMasterAgent.ModbusMaster.
                                                            ReadHoldingRegisters(
                                                                     this.ModbusSlaveInfo.slave,
                                                                     variable.VariableInfo.regesiterAddress,
                                                                     variable.VariableInfo.length);
                            break;
                        case 2:
                            tmpVal = this.ModbusMasterAgent.ModbusMaster.
                                                            ReadInputRegisters(
                                                                     this.ModbusSlaveInfo.slave,
                                                                     variable.VariableInfo.regesiterAddress,
                                                                     variable.VariableInfo.length);
                            break;
                        case 1:
                            break;
                        case 0:
                            break;
                        default:
                            break;
                    }

                    ///转化数据
                    switch (variable.VariableInfo.dataType.ToLower())
                    {
                        case "integer":
                            switch (variable.VariableInfo.length)
                            {
                                case  1:
                                    variable.Value = tmpVal[0];
                                    break;
                                case  2:
                                    variable.Value = ModbusUtility.GetUInt32(tmpVal[0], tmpVal[1]);
                                    break;
                                case  4:
                                    byte[] byte1 = BitConverter.GetBytes(tmpVal[0]);
                                    byte[] byte2 = BitConverter.GetBytes(tmpVal[1]);
                                    byte[] byte3 = BitConverter.GetBytes(tmpVal[2]);
                                    byte[] byte4 = BitConverter.GetBytes(tmpVal[3]);
                                    byte[] bytes = new byte[8] { byte1[0], byte1[1], byte2[0], byte2[1], byte3[0], byte3[1], byte4[0], byte4[1]};
                                    variable.Value = BitConverter.ToInt64(bytes, 0);
                                    break;
                                default:
                                throw new NotImplementedException(string.Format("无法识别的数据类型-{0}", variable.VariableInfo.dataType));
                            }
                            break;
                        case "real":
                           variable.Value = ModbusUtility.GetSingle(tmpVal[0], tmpVal[1]);
                           break;
                        case "bool":
                           variable.Value = tmpVal[0];
                           break;
                        default:
                       throw new NotImplementedException(string.Format("无法识别的数据类型-{0}", variable.VariableInfo.dataType));
                    }
                }
     
               
            }
        }
        /// <summary>
        /// 写数据
        /// </summary>
        //public void Write()
        //{
        //    foreach (ModbusVariable variable in this.Variables)
        //    {
        //        this.ModbusMasterAgent.ModbusMaster.
        //                         WriteMultipleRegisters(
        //                             this.ModbusSlaveInfo.slave,
        //                             variable.VariableInfo.regesiterAddress,
        //                             (ushort[])variable.OriginalValue);
        //    }
        //}
        public void Write()
        {
            foreach (ModbusVariable variable in this.Variables)
            {
                if (variable.VariableInfo.accessibility != "ReadOnly")
                {
                    DataRow dr = SelectControl(variable.VariableInfo.code);
                    ushort[] shortValues;
                    if (dr["type"].ToString().ToUpper() == "WLT")
                    {
                        switch (Convert.ToInt32(dr["command"]))
                        {
                            case 1:
                                shortValues = new ushort[4] { 0, 0, 0, 1 };
                                break;
                            case 2:
                                shortValues = new ushort[4] { 0, 0, 1, 0 };
                                break;
                            case 4:
                                shortValues = new ushort[4] { 0, 1, 0, 0 };
                                break;
                            case 8:
                                shortValues = new ushort[4] { 1, 0, 0, 0 };
                                break;
                            case 12:
                                shortValues = new ushort[4] { 1, 1, 0, 0 };
                                break;
                            default:
                                shortValues = new ushort[4] { 0, 0, 0, 0 };
                                throw new NotImplementedException(string.Format("无法识别的指令-{0}", Convert.ToInt32(dr["conmmand"])));
                        }
                        variable.originalValue = shortValues;
                    }
                    else
                    {
                        variable.originalValue[0] = Convert.ToUInt16(dr["conmmand"]);
                    }
                     this.ModbusMasterAgent.ModbusMaster.
                                WriteMultipleRegisters(
                                    this.ModbusSlaveInfo.slave,
                                    variable.VariableInfo.regesiterAddress,
                                     (ushort[])variable.OriginalValue);
                   
                }
            }
        }
        public void ReadWrite()
        {     
            Write();
            Read();
        }

        public DataRow SelectControl(int code)
        {
            string ConnectionString = "server=192.168.1.179;database=opcmes3;uid=microdaq;pwd=microdaq";
            SqlConnection con = new SqlConnection(ConnectionString);
            con.Open();
            string sqlStr = "select * from v_remoteControl a where a.id= " + code;
            SqlDataAdapter da = new SqlDataAdapter(sqlStr, con);
            DataSet ds = new System.Data.DataSet();
            da.Fill(ds);
            return ds.Tables[0].Rows[0];
 
        }

        public MicroDAQ.Configuration.ModbusSlaveInfo ModbusSlaveInfo { get; set; }

        public IList<MicroDAQ.Gateways.Modbus2.ModbusVariable> Variables { get; set; }

        public MicroDAQ.Gateways.Modbus2.ModbusMasterAgent ModbusMasterAgent { get; set; }
    }
}
