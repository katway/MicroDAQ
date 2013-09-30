/**
 * 文件名：DeviceViewModel.cs
 * 说明：设备实体模型
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
    /// 设备实体模型
    /// </summary>
    public class DeviceViewModel
    {
        //唯一标识
        private int _id;

        //名称
        private string _name;

        //别名
        private string _alias;

        //从站号
        private int _slave;

        //IP地址
        private string _ipAddress;

        //IP端口
        private int _ipPort;

        //是否启用
        private bool _isEnable;

        //传输通道
        private ChannelTypes _channelType;

        //Modbus通讯协议
        private ModbusProtocols _protocol;

        //变量列表
        private List<ItemViewModel> _items;

        //所属通道
        private ChannelBase _channel;

        /// <summary>
        /// 唯一标识
        /// </summary>
        public int Id
        {
            get { return _id; }
            set { _id = value; }
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
        public string Alias
        {
            get { return _alias; }
            set { _alias = value; }
        }

        /// <summary>
        /// 从站号
        /// </summary>
        public int Slave
        {
            get { return _slave; }
            set { _slave = value; }
        }

        /// <summary>
        /// IP地址
        /// </summary>
        public string IpAddress
        {
            get { return _ipAddress; }
            set { _ipAddress = value; }
        }

        /// <summary>
        /// IP端口
        /// </summary>
        public int IpPort
        {
            get { return _ipPort; }
            set { _ipPort = value; }
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
        /// 传输通道
        /// </summary>
        public ChannelTypes ChannelType
        {
            get { return _channelType; }
            set { _channelType = value; }
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
        /// 变量列表
        /// </summary>
        public List<ItemViewModel> Items
        {
            get { return _items; }
            set { _items = value; }
        }

        /// <summary>
        /// 所属通道
        /// </summary>
        public ChannelBase Channel
        {
            get { return _channel; }
            set { _channel = value; }
        }

        public DeviceViewModel()
        {
            _items = new List<ItemViewModel>();
        }
    }
}
