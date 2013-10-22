// File:    ModbusGateway.cs
// Author:  John
// Created: 2013年9月29日 16:31:00
// Purpose: Definition of Class ModbusGateway

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace MicroDAQ.Configuration
{
    public class ModbusGatewayInfo
    {
        public long serialID;
        public string name;
        public string allias;
        public string enable;

        public IList<ModbusMasterInfo> modbusMaster;

        public ModbusGatewayInfo(long serialID, DataSet config)
        {
            ///初始化
            this.modbusMaster = new List<ModbusMasterInfo>();
            this.serialID = serialID;

            ///查找指定seiralID的纪录
            string filter = "serialid = " + this.serialID;
            DataRow[] dt = config.Tables["ModbusGateway"].Select(filter);
            ///使用各字段中的值为属性赋值
            this.serialID = (long)dt[0]["serialID"];
            this.name = dt[0]["name"].ToString();
            this.allias = dt[0]["allias"].ToString();
            this.enable = dt[0]["enable"].ToString();



            ///查找下属的Modbus纪录
            filter = "serialid = " + this.serialID;
            DataRow[] dtMaster = config.Tables["ModbusMaster"].Select(filter);
            for (int i = 0; i < dt.Length; i++)
            {
                long serial = (long)dtMaster[i]["serialid"];
                ModbusMasterInfo masterInfo = new ModbusMasterInfo(serial, config);
                this.modbusMaster.Add(masterInfo);
            }
        }

    }
}