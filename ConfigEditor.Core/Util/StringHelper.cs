/**
 * 文件名：StringHelper.cs
 * 说明：字符串辅助类
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
using System.Text.RegularExpressions;

namespace ConfigEditor.Core.Util
{
    /// <summary>
    /// 字符串辅助类
    /// </summary>
    public class StringHelper
    {
        public static string EncloseQuote(string msg)
        {
            if (string.IsNullOrEmpty(msg))
            {
                return "NULL";
            }
            else
            {
                return string.Format("'{0}'", msg);
            }
        }

        /// <summary>
        /// 整数或者小数字符串转换整型字符串
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static int ConvetToInteger(object o)
        {
            int result = 0;

            if (o == null)
            {
                return result;
            }

            string temp = o.ToString();
            if (IsNumeric(temp))
            {
                int a = (int)Convert.ToDouble(temp);
                result = a;
            }

            return result;
        }


        /// <summary>
        /// 判断字符串是否为数字
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNumeric(string value)
        {
            return Regex.IsMatch(value, @"^[+-]?\d*[.]?\d*$");
        }

        /// <summary>
        /// 判断字符串是否为整数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsInt(string value)
        {
            return Regex.IsMatch(value, @"^[+-]?\d*$");
        }
    }
}
