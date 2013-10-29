// File:    IPSetting.cs
// Author:  John
// Created: 2013年9月23日 16:51:16
// Purpose: Definition of Class IPSetting

using System;
using System.Data;
namespace MicroDAQ.Configuration
{
    public class IPSettingInfo
    {
        public long serialID;
        public string iP;
        public int port;
        public string enable;

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
        }

       

    }
}