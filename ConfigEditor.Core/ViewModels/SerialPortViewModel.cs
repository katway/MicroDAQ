/**
 * 文件名：SerialPortViewModel.cs
 * 说明：串口实体模型
 * 作者：林安城
 * 更改记录： 
 * -------------------------------------------------------
 * 改动人 	时间 			原因
 * -------------------------------------------------------
 * 林安城 	2013-09-20		创建文件
 * -------------------------------------------------------
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConfigEditor.Core.Models;

namespace ConfigEditor.Core.ViewModels
{
    /// <summary>
    /// 串口视图模型
    /// </summary>
    public class SerialPortViewModel
    {
        //唯一标识
        private int _id;

        //串口号
        private string _portName;

        //波特率
        private int _baudRate;

        //数据位
        private int _dataBits;

        //奇偶校验
        private string _parity;

        //停止位
        private int _stopBits;

        //Modbus通讯协议
        private ModbusProtocols _protocol;

        /// <summary>
        /// 唯一标识
        /// </summary>
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// 串口号
        /// </summary>
        public string PortName
        {
            get { return _portName; }
            set { _portName = value; }
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
        /// 数据位
        /// </summary>
        public int DataBits
        {
            get { return _dataBits; }
            set { _dataBits = value; }
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
        /// 停止位
        /// </summary>
        public int StopBits
        {
            get { return _stopBits; }
            set { _stopBits = value; }
        }

        /// <summary>
        /// Modbus通讯协议
        /// </summary>
        public ModbusProtocols Protocol
        {
            get { return _protocol; }
            set { _protocol = value; }
        }

        public SerialPortViewModel()
        {
        }

    }
}
