// File:    IPSetting.cs
// Author:  John
// Created: 2013年9月23日 16:51:16
// Purpose: Definition of Class IPSetting

using System;
using System.Data;
using System.Net;
using System.Net.Sockets;
namespace MicroDAQ.Configuration
{
    public class IPSettingInfo
    {
        public long serialID;
        public string iP;
        public int port;
        public string enable;
        public TcpClient tcpClient;

        public IPSettingInfo(long serialID, DataSet config)
        {
            ///初始化
            this.serialID = serialID;

            ///查找指定seiralID的纪录
            string filter = "serialid = " + this.serialID;
            DataRow[] dt = config.Tables["IPSetting"].Select(filter);

            ///使用各字段中的值为属性赋值
            this.serialID = (long)dt[0]["serialID"];
            this.iP = dt[0]["ip"].ToString();
            this.port = Convert.ToInt32(dt[0]["port"]);
            this.enable = dt[0]["enable"].ToString();
       
            string[] strIP = iP.Split('.');
            byte[] byteIP = new byte[4];
            for (int i = 0; i < strIP.Length; i++)
            {
                byteIP[i] = Convert.ToByte(strIP[i]);
            }
            IPAddress address = new IPAddress(byteIP);
            this.tcpClient  = new TcpClient(address.ToString(), port);
           
        }

       

    }
}