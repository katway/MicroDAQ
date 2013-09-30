// File:    ModbusSlave.cs
// Author:  John
// Created: 2013Äê9ÔÂ23ÈÕ 16:51:16
// Purpose: Definition of Class ModbusSlave

using System;
namespace MicroDAQ.Configuration
{
    public class ModbusSlaveInfo
    {
        public long serialID;
        public string name;
        public string allias;
        public string type;
        public int slave;
        public string enable;

        public System.Collections.Generic.List<ModbusVariable> modbusVariable;

        public IPSettingInfo iPSetting;

        internal DataLoader ConfigLoader
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }



    }
}