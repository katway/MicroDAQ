// File:    SerialPort.cs
// Author:  John
// Created: 2013��9��23�� 16:51:16
// Purpose: Definition of Class SerialPort

using System;
namespace MicroDAQ.Configuration
{
    public class SerialPort
    {
        public long serialID;
        public string port;
        public int baudRate;
        public string parity;
        public int databits;
        public int stopbits;
        public string enable;

    }
}