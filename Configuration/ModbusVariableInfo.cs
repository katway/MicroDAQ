// File:    ModbusVariable.cs
// Author:  John
// Created: 2013��9��23�� 16:51:16
// Purpose: Definition of Class ModbusVariable

using System;
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

    }
}