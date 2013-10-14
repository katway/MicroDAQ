/**
 * 文件名：ChannelBase.cs
 * 说明：通道抽象类
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
    /// 通道抽象类
    /// </summary>
    public abstract class ChannelBase
    {
        //Modbus通讯协议
        private ModbusProtocols _protocol;

        //通道类型
        private ChannelTypes _type;

        //是否启用
        private bool _isEnable;

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
        /// 是否启用
        /// </summary>
        public bool IsEnable
        {
            get { return _isEnable; }
            set { _isEnable = value; }
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
