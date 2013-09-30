/**
 * 文件名：ProjectViewModel.cs
 * 说明：项目实体模型
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

namespace ConfigEditor.Core.ViewModels
{
    /// <summary>
    /// 项目实体模型
    /// </summary>
    public class ProjectViewModel
    {
        public ProjectViewModel()
        {
            SerialPorts = new List<SerialPortViewModel>();
            Ethernet = new EthernetViewModel();
        }

        /// <summary>
        /// 串口列表
        /// </summary>
        public List<SerialPortViewModel> SerialPorts { get; set; }

        /// <summary>
        /// 以太网
        /// </summary>
        public EthernetViewModel Ethernet { get; set; }

        /// <summary>
        /// 全部设备
        /// </summary>
        public List<DeviceViewModel> AllDevices
        {
            get
            {
                return SerialPorts.SelectMany(obj => obj.Devices).Concat(Ethernet.Devices).ToList();
            }
        }
    }
}
