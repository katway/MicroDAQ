using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfigEditor.Core.Models
{
    public class ModbusSlave
    {
        //Modbus从机编号
        private long _serialID;

        //Modbus主机编号
        private long _modbusMaster_SerialID;

        //IP设置编号
        private long _iPSetting_SerialID;

        //名称
        private string _name;

        //别名
        private string _allias;

        //协议类型
        private string _type;

        //从机地址
        private int _slave;

        //启用
        private string _enable;

        /// <summary>
        /// Modbus从机编号
        /// </summary>
        public long SerialID 
        {
            get { return _serialID; }
            set { _serialID = value;}
        }

        /// <summary>
        /// Modbus主机编号
        /// </summary>
        public long ModbusMaster_SerialID
        {
            get { return _modbusMaster_SerialID; }
            set { _modbusMaster_SerialID = value; }
        }

        /// <summary>
        /// IP设置编号
        /// </summary>
        public long IPSetting_SerialID
        {
            get { return _iPSetting_SerialID; }
            set { _iPSetting_SerialID = value; }

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
        /// 协议类型
        /// </summary>
        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        /// <summary>
        /// 从机地址
        /// </summary>
        public int Slave
        {
            get { return _slave; }
            set { _slave = value; }
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
