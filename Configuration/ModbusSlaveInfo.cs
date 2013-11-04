// File:    ModbusSlave.cs
// Author:  John
// Created: 2013年9月23日 16:51:16
// Purpose: Definition of Class ModbusSlave

using System;
using System.Data;
using System.Collections.Generic;
using System.Net.Sockets;

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
        public TcpClient tcpClient;

        public System.Collections.Generic.List<ModbusVariableInfo> modbusVariables;

        public IPSettingInfo iPSetting;

        public ModbusSlaveInfo(long serialID, DataSet config)
        {
            ///初始化
            this.modbusVariables = new List<ModbusVariableInfo>();
            this.serialID = serialID;

            ///查找指定seiralID的纪录
            string filter = "serialid = " + this.serialID;
            DataRow[] dt = config.Tables["ModbusSlave"].Select(filter);

            ///使用各字段中的值为属性赋值
            this.serialID = (long)dt[0]["serialID"];
            this.name = dt[0]["name"].ToString();
            this.allias = dt[0]["allias"].ToString();
            this.type = dt[0]["type"].ToString();
            this.slave = Convert.ToByte(dt[0]["slave"]);
            this.enable = dt[0]["enable"].ToString();

            ///查找下属的Variable纪录
            filter = "ModbusSlave_SerialID = " + this.serialID;
            DataRow[] dtSlave = config.Tables["ModbusRegister"].Select(filter);
            for (int i = 0; i < dtSlave.Length; i++)
            {
                long serial = (long)dtSlave[i]["serialid"];
                ModbusVariableInfo VariableInfo = new ModbusVariableInfo(serial, config);
                this.modbusVariables.Add(VariableInfo);
            }
            //加载IP配置信息
            if (dt[0]["IPSetting_SerialID"].ToString() != "0")
            {
                long ipSettingID = Convert.ToInt64(dt[0]["IPSetting_SerialID"]);
                this.iPSetting = new IPSettingInfo(ipSettingID, config);
                this.tcpClient = iPSetting.tcpClient;
            }
        }

      

    }
}