using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using Modbus.Device;
using MicroDAQ.Configuration;
using MicroDAQ.Common;

namespace MicroDAQ.Gateways.Modbus2
{
    public class ModbusMasterAgent : MicroDAQ.Gateways.ItemManageBase, IDataItemManage
    {
        public ModbusMasterAgent(ModbusMasterInfo masterInfo)
        {
            this.MasterInfo = masterInfo;       

            ///建立SerialPort对象
            this.SerialPort = new SerialPort(
                        this.MasterInfo.serialPort.port,
                        this.MasterInfo.serialPort.baudRate,
                        (Parity)Enum.Parse(typeof(Parity), this.MasterInfo.serialPort.parity),
                        this.MasterInfo.serialPort.databits,
                        (StopBits)Enum.Parse(typeof(StopBits), this.MasterInfo.serialPort.stopbits));
            this.SerialPort.Open();

            ///创建下属的SerialPortSlaveAgent对象
            this.SerialPortSlaves = new List<SerialPortSlaveAgent>();
            foreach (ModbusSlaveInfo slaveInfo in masterInfo.modbusSlaves)
            {
                SerialPortSlaveAgent newSlave = new SerialPortSlaveAgent(this, slaveInfo);
                this.SerialPortSlaves.Add(newSlave);

                ///将各个Slave的变量添加到Items列表
                this.Items =new List<IItem>();
                foreach (ModbusVariable variable in newSlave.Variables)
                {
                    this.Items.Add(variable);
                }
            }
            ///创建ModbusMaster对象




        }

        public ModbusMasterAgent()
        {
            throw new System.NotImplementedException();
        }

        public SerialPort SerialPort { get; set; }

        public IModbusMaster ModbusMaster { get; set; }

        public IList<SerialPortSlaveAgent> SerialPortSlaves { get; set; }

        public MicroDAQ.Configuration.ModbusMasterInfo MasterInfo { get; set; }


        public override void ReadWrite()
        {
            foreach (SerialPortSlaveAgent slave in this.SerialPortSlaves)
            {
                slave.ReadWrite();
            }
        }
    }
}
