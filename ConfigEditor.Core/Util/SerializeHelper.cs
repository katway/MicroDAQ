/**
 * 文件名：SerializeHelper.cs
 * 说明：序列号和反序列号辅助类
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

namespace ConfigEditor.Core.Util
{
    /// <summary>
    /// 序列号和反序列号辅助类
    /// </summary>
    public class SerializeHelper
    {
        /// <summary>
        /// 将16进制字符串转化为字节数组 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] HexSerialize(string value)
        {
            value = value.Replace(" ", "");
            int len = value.Length / 2;
            byte[] ret = new byte[len];
            for (int i = 0; i < len; i++)
                ret[i] = (byte)(Convert.ToInt32(value.Substring(i * 2, 2), 16));
            return ret;
        }

        /// <summary>
        /// 将字节数组转化为16进制字符串 
        /// </summary>
        /// <param name="arrByte"></param>
        /// <param name="reverse"></param>
        /// <returns></returns>
        public static string HexDeserialize(byte[] arrByte)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in arrByte)
                sb.AppendFormat("{0:X2}", b);

            return sb.ToString();
        }

        /// <summary>
        /// 将字符串转换为UTF-8编码的字节数组
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] UTF8Serialize(string value)
        {
            return Encoding.UTF8.GetBytes(value);
        }

        /// <summary>
        /// 将UTF-8编码的字节数组转换为字符串
        /// </summary>
        /// <param name="arrByte"></param>
        /// <returns></returns>
        public static string UTF8Deserialize(byte[] arrByte)
        {
            return Encoding.UTF8.GetString(arrByte);
        }

    }
}
