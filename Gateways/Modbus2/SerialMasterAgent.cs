using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using Modbus.Device;

namespace MicroDAQ.Gateways
{
    public class ModbusMasterAgent
    {
        public SerialPort SerialPort { get; set; }

        public IModbusMaster ModbusMaster
        {
            get;
            set;
        }

    }
}
