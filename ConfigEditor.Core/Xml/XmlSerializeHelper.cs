/**
 * 文件名：XmlSerializeHelper.cs
 * 说明：Xml序列化类
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
using System.IO;

namespace ConfigEditor.Core.Xml
{
    /// <summary>
    /// Xml序列化类
    /// </summary>
    public class XmlSerializeHelper
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="device"></param>
        /// <param name="xmlFile"></param>
        public static void Serialize(XmlDevice device, string xmlFile)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(XmlDevice));
            StreamWriter sw = new StreamWriter(xmlFile);
            serializer.Serialize(sw, device);
            sw.Close();
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="xmlFile"></param>
        /// <returns></returns>
        public static XmlDevice Deserialize(string xmlFile)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(XmlDevice));
            FileStream fs = new FileStream(xmlFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            XmlDevice device = serializer.Deserialize(fs) as XmlDevice;
            fs.Close();

            return device;
        }
    }
}
