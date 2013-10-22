// File:    ModbusMaster.cs
// Author:  John
// Created: 2013��9��23�� 16:51:16
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
            ///��ʼ��
            this.modbusSlaves = new List<ModbusSlaveInfo>();
            this.serialID = serialID;

            ///����ָ��seiralID�ļ�¼
            string filter = "serialid = " + this.serialID;
            DataRow[] dt = config.Tables["ModbusMaster"].Select(filter);

            ///ʹ�ø��ֶ��е�ֵΪ���Ը�ֵ
            this.serialID = (long)dt[0]["serialID"];
            this.name = dt[0]["name"].ToString();
            this.allias = dt[0]["allias"].ToString();
            this.enable = dt[0]["enable"].ToString();

            ///����������Slave��¼
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