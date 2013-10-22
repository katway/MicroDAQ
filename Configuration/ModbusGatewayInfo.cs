// File:    ModbusGateway.cs
// Author:  John
// Created: 2013��9��29�� 16:31:00
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
            ///��ʼ��
            this.modbusMaster = new List<ModbusMasterInfo>();
            this.serialID = serialID;

            ///����ָ��seiralID�ļ�¼
            string filter = "serialid = " + this.serialID;
            DataRow[] dt = config.Tables["ModbusGateway"].Select(filter);
            ///ʹ�ø��ֶ��е�ֵΪ���Ը�ֵ
            this.serialID = (long)dt[0]["serialID"];
            this.name = dt[0]["name"].ToString();
            this.allias = dt[0]["allias"].ToString();
            this.enable = dt[0]["enable"].ToString();



            ///����������Modbus��¼
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