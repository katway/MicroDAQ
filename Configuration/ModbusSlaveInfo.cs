// File:    ModbusSlave.cs
// Author:  John
// Created: 2013��9��23�� 16:51:16
// Purpose: Definition of Class ModbusSlave

using System;
using System.Data;
using System.Collections.Generic;

namespace MicroDAQ.Configuration
{
    public class ModbusSlaveInfo
    {
        public long serialID;
        public string name;
        public string allias;
        public string type;
        public byte slave;
        public string enable;

        public System.Collections.Generic.List<ModbusVariableInfo> modbusVariables;

        public IPSettingInfo iPSetting;

        public ModbusSlaveInfo(long serialID, DataSet config)
        {
            ///��ʼ��
            this.modbusVariables = new List<ModbusVariableInfo>();
            this.serialID = serialID;

            ///����ָ��seiralID�ļ�¼
            string filter = "serialid = " + this.serialID;
            DataRow[] dt = config.Tables["ModbusSlave"].Select(filter);

            ///ʹ�ø��ֶ��е�ֵΪ���Ը�ֵ
            this.serialID = (long)dt[0]["serialID"];
            this.name = dt[0]["name"].ToString();
            this.allias = dt[0]["allias"].ToString();
            this.type = dt[0]["type"].ToString();
            this.slave = Convert.ToByte(dt[0]["slave"]);
            this.enable = dt[0]["enable"].ToString();

            ///����������Variable��¼
            filter = "serialid = " + this.serialID;
            DataRow[] dtSlave = config.Tables["ModbusVariable"].Select(filter);
            for (int i = 0; i < dt.Length; i++)
            {
                long serial = (long)dtSlave[i]["serialid"];
                ModbusVariableInfo VariableInfo = new ModbusVariableInfo(serial, config);
                this.modbusVariables.Add(VariableInfo);
            }
        }

      

    }
}