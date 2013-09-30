// File:    ModbusGateway.cs
// Author:  John
// Created: 2013Äê9ÔÂ29ÈÕ 16:31:00
// Purpose: Definition of Class ModbusGateway

using System;
using System.Collections.Generic;
namespace MicroDAQ.Configuration
{
    public class ModbusGatewayInfo : DataLoader
    {
        public long serialID;
        public string name;
        public string allias;
        public string enable;

        public IList<ModbusMasterInfo> modbusMaster;


    }
}