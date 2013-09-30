using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConfigEditor.Core.Models;

namespace ConfigEditor.Core.ViewModels
{
    public abstract class ChannelBase
    {
        //Modbus通讯协议
        private ModbusProtocols _protocol;

        //通道类型
        private ChannelTypes _type;

        /// <summary>
        /// 通道类型
        /// </summary>
        public ChannelTypes Type
        {
            get { return _type; }
            set { _type = value; }
        }

        /// <summary>
        /// Modbus通讯协议
        /// </summary>
        public ModbusProtocols Protocol
        {
            get { return _protocol; }
            set { _protocol = value; }
        }

        /// <summary>
        /// 设备列表
        /// </summary>
        public List<DeviceViewModel> Devices { get; set; }

        public ChannelBase()
        {

        }
    }
}
