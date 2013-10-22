/**
 * 文件名：ModbusMaster.cs
 * 说明：Modbus主机类
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
    public class ModbusMaster
    {
        //Modbus主机编号
        private long _serialID;

        //串行端口编号
        private long _serialPort_SerialID;

        //Modbus网关编号
        private long _modbusGateway_SerialID;

        //名称
        private string _name;

        //别名
        private string _allias;

        //启用
        private string _enable;

        /// <summary>
        /// Modbus主机编号
        /// </summary>
        public long SerialID
        {
            get { return _serialID; }
            set { _serialID = value; }
        }


        public long SerialPort_SerialID
        {
            get { return _serialPort_SerialID; }
            set { _serialPort_SerialID = value; }
        }


        public long ModbusGateway_SerialID
        {
            get { return _modbusGateway_SerialID; }
            set { _modbusGateway_SerialID = value; }
        }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// 别名
        /// </summary>
        public string Allias
        {
            get { return _allias; }
            set { _allias = value; }
        }

        /// <summary>
        /// 启用
        /// </summary>
        public string Enable
        {
            get { return _enable; }
            set { _enable = value; }
        }
    }
}
