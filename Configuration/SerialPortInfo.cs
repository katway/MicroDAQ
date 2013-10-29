// File:    SerialPort.cs
// Author:  John
// Created: 2013��9��23�� 16:51:16
// Purpose: Definition of Class SerialPort

using System;
using System.Data;
using System.Collections;

namespace MicroDAQ.Configuration
{
    public class SerialPortInfo
    {
        public long serialID;
        public string port;
        public int baudRate;
        public string parity;
        public int databits;
        public string stopbits;
        public string enable;

        public SerialPortInfo(long serialID, DataSet config)
        {
            ///��ʼ��
            this.serialID = serialID;

            ///����ָ��seiralID�ļ�¼
            string filter = "serialid = " + this.serialID;
            DataRow[] dt = config.Tables["SerialPort"].Select(filter);

            ///ʹ�ø��ֶ��е�ֵΪ���Ը�ֵ
            this.serialID = (long)dt[0]["serialID"];
            this.port = dt[0]["port"].ToString();
            this.baudRate = Convert.ToInt32(dt[0]["baudRate"]);
            this.parity = dt[0]["parity"].ToString();
            this.databits = Convert.ToInt32(dt[0]["databits"]); ;
            this.stopbits = dt[0]["stopbits"].ToString();
            this.enable = dt[0]["enable"].ToString();
        }



    }
}