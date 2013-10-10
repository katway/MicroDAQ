using System;
using System.Collections.Generic;
using System.Text;
using Modbus.Device;
using Modbus.Utility;
using System.Data;
using System.Data.SqlClient;
using MicroDAQ.Common;
using MicroDAQ.Configuration;
namespace MicroDAQ.Gateways.Modbus2
{
    public class SerialPortSlave : MicroDAQ.Gateways.ItemManageBase, IDataItemManage
    {
        DataTable dtMeta;
        DataTable dtCommands;
        DataTable dtWriteData;
        byte slaveAddress;
        public SqlConnection Connection { get; set; }
        /// <summary>
        /// 根据Master对象信息和Salve配置信息生成对象
        /// </summary>
        /// <param name="masterAgent">所属Master对象</param>
        /// <param name="slaveInfo">配置信息</param>
        public SerialPortSlave(ModbusMasterAgent masterAgent, ModbusSlaveInfo slaveInfo)
        {
            ///上属Master相关
            this.ModbusMasterAgent = masterAgent;
            switch (slaveInfo.type.ToUpper())
            {
                case "RTU":
                    this.ModbusMasterAgent.ModbusMaster
                        = ModbusSerialMaster.CreateRtu(this.ModbusMasterAgent.SerialPort);
                    break;
                case "ASCII":
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

                ///取出数据
                switch (variable.VariableInfo.regesiterType)
                {
                    //TODO:需要修改以下判断值
                    case 1:
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
                    case 3:
                        break;
                    case 4:
                        break;
                    default:
                        break;
                }

                ///转化数据
                switch (variable.VariableInfo.dataType.ToLower())
                {
                    case "int":
                    case "short":
                        variable.Value = tmpVal[0];
                        break;
                    case "float":
                        variable.Value = ModbusUtility.GetSingle(tmpVal[0], tmpVal[1]);
                        break;
                    case "long":
                        variable.Value = ModbusUtility.GetUInt32(tmpVal[0], tmpVal[1]);
                        break;
                    default:
                        throw new NotImplementedException(string.Format("无法识别的数据类型-{0}", variable.VariableInfo.dataType));
                }
            }
        }
        public void Write()
        {
            foreach (ModbusVariable variable in this.Variables)
            {
                this.ModbusMasterAgent.ModbusMaster.
                                 WriteMultipleRegisters(
                                     this.ModbusSlaveInfo.slave,
                                     variable.VariableInfo.regesiterAddress,
                                     (ushort[])variable.OriginalValue);
            }
        }
        public void ReadWriteData()
        {
            Read();
            Write();
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

        public void StartSynchronize()
        {
            new JonLibrary.Automatic.CycleTask().Run(new System.Threading.ThreadStart(ReadWriteData), System.Threading.ThreadPriority.BelowNormal);
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void StopSynchronize()
        {
            throw new System.NotImplementedException();
        }

        public MicroDAQ.Configuration.ModbusSlaveInfo ModbusSlaveInfo
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public IList<MicroDAQ.Gateways.Modbus2.ModbusVariable> Variables
        {
            get;
            set;
        }

        public MicroDAQ.Gateways.ModbusMasterAgent ModbusMasterAgent
        {
            get;
            set;
        }
    }
}
