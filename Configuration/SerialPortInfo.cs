// File:    SerialPort.cs
// Author:  John
// Created: 2013年9月23日 16:51:16
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
            ///初始化
            this.serialID = serialID;

            ///查找指定seiralID的纪录
            string filter = "serialid = " + this.serialID;
            DataRow[] dt = config.Tables["SerialPort"].Select(filter);

            ///使用各字段中的值为属性赋值
            this.serialID = (long)dt[0]["serialID"];
            string port = dt[0]["port"].ToString();
            int baudRate = Convert.ToInt32(dt[0]["baudRate"]);
            string parity = dt[0]["parity"].ToString();
            int databits = Convert.ToInt32(dt[0]["databits"]); ;
            string stopbits = dt[0]["stopbits"].ToString();
            this.enable = dt[0]["enable"].ToString();
        }



    }
}