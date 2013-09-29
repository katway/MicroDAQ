/**
 * 文件名：SerialPort.cs
 * 说明：串行端口类
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

namespace ConfigEditor.Core.Models
{
    public class SerialPort
    {
        //串行端口编号
        private long _serialID;

        //端口
        private string _port;

        //奇偶校验
        private string _parity;

        //启用
        private string _enable;

        //波特率
        private int _baudRate;

        //数据位
        private int _databits;

        //停止位
        private int _stopbits;

        /// <summary>
        /// 串行端口编号
        /// </summary>
        public long SerialID
        {
            get { return _serialID; }
            set { _serialID = value; }
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
        /// 奇偶校验
        /// </summary>
        public string Parity
        {
            get { return _parity; }
            set { _parity = value; }
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
        /// 波特率
        /// </summary>
        public int BaudRate
        {
            get { return _baudRate; }
            set { _baudRate = value; }
        }

        /// <summary>
        /// 停止位
        /// </summary>
        public int Stopbits
        {
            get { return _stopbits; }
            set { _stopbits = value; }
        }

        /// <summary>
        /// 数据位
        /// </summary>
        public int Databits
        {
            get { return _databits; }
            set { _databits = value; }
        }
    }
}
