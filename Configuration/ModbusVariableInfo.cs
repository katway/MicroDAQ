// File:    ModbusVariable.cs
// Author:  John
// Created: 2013年9月23日 16:51:16
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
        /// 寄存器地址
        /// NModbus库要求的参数类型，必须为ushort
        /// </summary>
        public ushort regesiterAddress;
        /// <summary>
        /// 读取长度
        /// 可选的，一般由数据类型决定,不可手动填写。
        /// NModbus库要求的参数类型，必须为ushort
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