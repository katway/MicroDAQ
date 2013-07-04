using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;

namespace ModbusOperate
{
  public  interface IModbus
    {
         void ConnectionPort(SerialPort ports);
         void ReadData();
    }
}
