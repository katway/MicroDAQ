/**
 * 文件名：EnumHelper.cs
 * 说明：枚举类型转换
 * 作者：林安城
 * 更改记录： 
 * -------------------------------------------------------
 * 改动人 	时间 			原因
 * -------------------------------------------------------
 * 林安城 	2013-09-30		创建文件
 * -------------------------------------------------------
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConfigEditor.Core.Models;

namespace ConfigEditor.Core.Util
{
    /// <summary>
    /// 枚举类型转换
    /// </summary>
    public class EnumHelper
    {
        /// <summary>
        /// 枚举类型转换，数据类型
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string EnumToCaption(DataTypes enumValue)
        {
            switch (enumValue)
            {
                case DataTypes.Integer:
                    return "整型";

                case DataTypes.Real:
                    return "实型";

                case DataTypes.Discrete:
                    return "离散型";

                case DataTypes.String:
                    return "字符串";

                default:
                  return "null";
            }
        }

        /// <summary>
        /// 枚举类型转换，读写属性
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string EnumToCaption(AccessRights enumValue)
        {
            switch (enumValue)
            {
                case AccessRights.ReadWrite:
                    return "可读可写";

                case AccessRights.ReadOnly:
                    return "只读";

                case AccessRights.WriteOnly:
                    return "只写";

                default:
                    return "null";
            }
        }

        /// <summary>
        /// 枚举类型转换，数据模型
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string EnumToCaption(ModbusDataModels enumValue)
        {
            switch (enumValue)
            {
                case ModbusDataModels.Coils:
                    return "线圈";

                case ModbusDataModels.DiscretesInput:
                    return "离散量输入";

                case ModbusDataModels.InputRegisters:
                    return "输入寄存器";

                case ModbusDataModels.HoldingRegisters:
                    return "保持寄存器";

                default:
                    return "null";
            }
        }

        /// <summary>
        /// 字符串转换为枚举类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static T StringToEnum<T>(string str)
        {
            T enumValue = (T)Enum.Parse(typeof(T), str);
            return enumValue;
        }
    }
}
