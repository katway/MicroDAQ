/**
 * 文件名：ArrayHelper.cs
 * 说明：数组辅助类
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
using System.Collections;

namespace ConfigEditor.Core.Util
{
    /// <summary>
    /// 数组辅助类
    /// </summary>
    public class ArrayHelper
    {
        /// <summary>
        /// 获取基本类型或者数值类型的值字符串
        /// </summary>
        /// <param name="orginalValue"></param>
        /// <returns></returns>
        public static string GetItemValue(object orginalValue)
        {
            string value = null;
            if (orginalValue != null)
            {
                if (orginalValue.GetType().IsArray)
                {
                    Array a = orginalValue as Array;
                    foreach (object o in a)
                    {
                        string oStr = string.Empty;
                        if (o != null)
                        {
                            oStr = o.ToString();
                        }
                        else
                        {
                            oStr = "null";
                        }

                        if (value == null)
                        {
                            value = oStr;
                        }
                        else
                        {
                            value += "," + oStr;
                        }
                    }
                }
                else
                {
                    value = orginalValue.ToString();
                }
            }
            else
            {
                value = string.Empty;
            }

            return value;
        }

        /// <summary>
        /// 逗号分隔字符串
        /// </summary>
        /// <param name="commaValue"></param>
        /// <returns></returns>
        public static Array ToArray(string commaValue)
        {
            if (string.IsNullOrEmpty(commaValue))
            {
                return null;
            }

            string[] values = commaValue.Split(',');
            return values;
        }
    }

}