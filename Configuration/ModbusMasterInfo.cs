// File:    ModbusMaster.cs
// Author:  John
// Created: 2013年9月23日 16:51:16
// Purpose: Definition of Class ModbusMaster

using System;
using System.Data;
using System.Collections.Generic;
namespace MicroDAQ.Configuration
{
    public class ModbusMasterInfo
    {
        public long serialID;
        public string name;
        public string allias;
        public string type;
        public string enable;

        public List<ModbusSlaveInfo> modbusSlaves;


        public SerialPortInfo serialPort;

        public ModbusMasterInfo(long serialID, System.Data.DataSet config)
        {
            ///初始化
            this.modbusSlaves = new List<ModbusSlaveInfo>();
            this.serialID = serialID;

            ///查找指定seiralID的纪录
            string filter = "serialid = " + this.serialID;
            DataRow[] dt = config.Tables["ModbusMaster"].Select(filter);

            ///使用各字段中的值为属性赋值
            this.serialID = (long)dt[0]["serialID"];
            this.name = dt[0]["name"].ToString();
            this.allias = dt[0]["allias"].ToString();
            this.enable = dt[0]["enable"].ToString();

            ///查找下属的Slave纪录
            filter = "serialid = " + this.serialID;
            DataRow[] dtSlave = config.Tables["ModbusSlave"].Select(filter);
            for (int i = 0; i < dt.Length; i++)
            {
                long serial = (long)dtSlave[i]["serialid"];
                ModbusSlaveInfo slaveInfo = new ModbusSlaveInfo(serial, config);
                this.modbusSlaves.Add(slaveInfo);
            }

            ///SerialPortInfo
            //this.serialPort = new SerialPortInfo(



        }



    }
}