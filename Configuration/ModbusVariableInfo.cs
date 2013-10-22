// File:    ModbusVariable.cs
// Author:  John
// Created: 2013��9��23�� 16:51:16
// Purpose: Definition of Class ModbusVariable

using System;
using System.Data;
using System.Collections;
namespace MicroDAQ.Configuration
{
    public class ModbusVariableInfo
    {
        public long serialID;
        public int code;
        public string allias;
        public string name;
        public string dataType;
        public int regesiterType;
        /// <summary>
        /// �Ĵ�����ַ
        /// NModbus��Ҫ��Ĳ������ͣ�����Ϊushort
        /// </summary>
        public ushort regesiterAddress;
        /// <summary>
        /// ��ȡ����
        /// ��ѡ�ģ�һ�����������;���,�����ֶ���д��
        /// NModbus��Ҫ��Ĳ������ͣ�����Ϊushort
        /// </summary> 
        public ushort length;
        public string accessibility;
        public decimal value;
        public string enable;
        public int scanPeriod;
        public decimal minimum;
        public decimal maximum;
        public decimal originalValue;
        public int decimalPlaces;

        public ModbusVariableInfo(long serialID, DataSet config)
        {
            ///��ʼ��
            this.serialID = serialID;

            ///����ָ��seiralID�ļ�¼
            string filter = "serialid = " + this.serialID;
            DataRow[] dt = config.Tables["ModbusSlave"].Select(filter);

            ///ʹ�ø��ֶ��е�ֵΪ���Ը�ֵ
            this.serialID = (long)dt[0]["serialID"];
            this.name = dt[0]["name"].ToString();
            this.allias = dt[0]["allias"].ToString();
            this.dataType = dt[0]["datatype"].ToString();
            this.regesiterType = Convert.ToByte(dt[0]["regesiterType"]);
            this.regesiterAddress = Convert.ToUInt16(dt[0]["regesiterAddress"]);
            this.length = Convert.ToUInt16(dt[0]["length"]);
            this.accessibility = dt[0]["accessibility"].ToString();
            //this.value = Convert.ToDecimal(dt[0]["slave"]);
            this.scanPeriod = Convert.ToInt16(dt[0]["scanPeriod"]);
            this.minimum = Convert.ToDecimal(dt[0]["minimum"]);
            this.maximum = Convert.ToDecimal(dt[0]["maximum"]);
            //this.originalValue = Convert.ToByte(dt[0]["slave"]);
            this.decimalPlaces = Convert.ToByte(dt[0]["decimalPlaces"]);
            this.enable = dt[0]["enable"].ToString();
        }

        public ModbusVariableInfo()
        {
            throw new System.NotImplementedException();
        }

    }
}