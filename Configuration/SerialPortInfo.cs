// File:    SerialPort.cs
// Author:  John
// Created: 2013Äê9ÔÂ23ÈÕ 16:51:16
// Purpose: Definition of Class SerialPort

using System;
namespace MicroDAQ.Configuration
{
    public class SerialPortInfo
    {
        public long serialID;
        public string port;
        public int baudRate;
        public string parity;
        public int databits;
        public int stopbits;
        public string enable;

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