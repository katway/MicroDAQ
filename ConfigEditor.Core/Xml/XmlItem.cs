/**
 * 文件名：XmlItem.cs
 * 说明：Xml变量节点类
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
    /// Xml变量节点类
    /// </summary>
    public class XmlItem
    {
        /// <summary>
        /// 变量名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 数据模型
        /// </summary>
        public string DataModel { get; set; }

        /// <summary>
        /// 寄存器地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 寄存器个数
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// 访问类型
        /// </summary>
        public string Access { get; set; }
    }
}
