﻿/**
 * 文件名：XmlDevice.cs
 * 说明：Xml设备节点类
 * 作者：林安城
 * 更改记录： 
 * -------------------------------------------------------
 * 改动人 	时间 			原因
 * -------------------------------------------------------
 * 林安城 	2013-10-12		创建文件
 * -------------------------------------------------------
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ConfigEditor.Core.Xml
{
    /// <summary>
    /// Xml设备节点类
    /// </summary>
    [XmlRootAttribute("Device", IsNullable = false)]
    public class XmlDevice
    {
        public XmlDevice()
        {
        }

        public XmlDevice(string name)
        {
            Name = name;
        }

        /// <summary>
        /// 名称
        /// </summary>
        [XmlAttribute]
        public string Name { get; set; }

        /// <summary>
        /// 支持协议
        /// </summary>
        [XmlAttribute]
        public string[] Protocols { get; set; }

        /// <summary>
        /// 变量列表
        /// </summary>
        [XmlArray]
        [XmlArrayItem(ElementName= "Item")]
        public XmlItem[] Items { get; set; }
    }
}
