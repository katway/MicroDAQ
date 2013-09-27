/**
 * 文件名：EntityHelper.cs
 * 说明：实体拷贝辅助类
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
using System.Reflection;

namespace ConfigEditor.Core.Util
{
    /// <summary>
    /// 实体拷贝辅助类
    /// </summary>
    public class EntityHelper
    {
        /// <summary>
        /// 把源对象里的各个字段的内容直接赋值给目标对象
        /// 只是字段复制，两个对象的字段名和类型都必须一致
        /// </summary>
        /// <param name="dest">目标对象</param>
        /// <param name="src">源对象</param>
        public static T CopyObject<T>(object src)
        {
            if (src == null)
            {
                return default(T);
            }

            Type srcType = src.GetType();
            Type destType = typeof(T);

            T dest = Activator.CreateInstance<T>();

            PropertyInfo[] srcInfo = srcType.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            PropertyInfo[] destInfo = destType.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

            for (int i = 0; i < srcInfo.Length; i++)
            {
                string name = srcInfo[i].Name;
                object val = srcInfo[i].GetValue(src, null);

                for (int j = 0; j < destInfo.Length; j++)
                {
                    string targetName = destInfo[j].Name;

                    //是否为基本类型
                    if (destInfo[j].PropertyType.IsPrimitive)
                    {
                        if (name.ToLower().Equals(targetName.ToLower()))
                        {
                            destInfo[j].SetValue(dest, val, null);
                            break;
                        }
                    }
                    else
                    {
                        if (name.ToLower().Equals(targetName.ToLower()) && destInfo[j].PropertyType == srcInfo[i].PropertyType)
                        {
                            destInfo[j].SetValue(dest, val, null);
                            break;
                        }
                    }
                }
            }

            return dest;
        }

        /// <summary>
        /// 复制列表对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <param name="dest"></param>
        public static IList<T2> CopyList<T1, T2>(IList<T1> src)
        {
            if (src == null)
            {
                return null;
            }

            IList<T2> dest = new List<T2>();

            foreach (T1 t1 in src)
            {
                T2 item = CopyObject<T2>(t1);
                dest.Add(item);
            }

            return dest;
        }
    }
}
