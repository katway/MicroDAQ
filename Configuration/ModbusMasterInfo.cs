// File:    ModbusMaster.cs
// Author:  John
// Created: 2013Äê9ÔÂ23ÈÕ 16:51:16
// Purpose: Definition of Class ModbusMaster

using System;
namespace MicroDAQ.Configuration
{
    public class ModbusMasterInfo
    {
        public long serialID;
        public string name;
        public string allias;
        public string type;
        public string enable;

        public System.Collections.Generic.List<ModbusSlaveInfo> modbusSlave;


        public SerialPortInfo serialPort;

    }
}