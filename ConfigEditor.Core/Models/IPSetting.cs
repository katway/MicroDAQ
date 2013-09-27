using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfigEditor.Core.Models
{
    class IPSetting
    {
        //IP设置编号
        private long _serialID;

        //启用
        private string _enable;

        //端口
        private string _port;

        //网络地址
        private int _ip;

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
        public string Port
        {
            get { return _port; }
            set { _port = value; }
        }

        /// <summary>
        /// 网络地址
        /// </summary>
        public int IP
        {
            get { return _ip; }
            set { _ip = value; }
        }
    }
}
