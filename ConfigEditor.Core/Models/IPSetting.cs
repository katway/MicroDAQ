/**
 * 文件名：IPSetting.cs
 * 说明：IP设置类
 * 作者：刘风彬
 * 更改记录： 
 * -------------------------------------------------------
 * 改动人 	时间 			原因
 * -------------------------------------------------------
 * 刘风彬 	2013-09-29		创建文件
 * -------------------------------------------------------
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConfigEditor.Core.Models;
using System.Data;

namespace ConfigEditor.Core.Models
{
    public class IPSetting
    {
        //IP设置编号
        private long _serialID;

        //启用
        private string _enable;

        //端口
        private int _port;

        //网络地址
        private string _ip;

        /// <summary>
        /// 监测参数编号
        /// </summary>
        public long SerialID
        {
            get { return _serialID; }
            set { _serialID = value; }
        }

        /// <summary>
        /// 启用
        /// </summary>
        public string Enable
        {
            get { return _enable; }
            set { _enable = value; }
        }

        /// <summary>
        /// 端口
        /// </summary>
        public int Port
        {
            get { return _port; }
            set { _port = value; }
        }

        /// <summary>
        /// 网络地址
        /// </summary>
        public string IP
        {
            get { return _ip; }
            set { _ip = value; }
        }
    }
}
