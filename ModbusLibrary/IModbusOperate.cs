using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;

namespace ModbusLibrary
{
  public  interface IModbusOperate
    {
         void ConnectionPort(SerialPort ports);
         void ReadData();
    }
}
