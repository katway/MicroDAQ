/**
 * 文件名：KernelHelper.cs
 * 说明：内核性能优化辅助类
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
using System.Threading;
using System.Reflection;

namespace ConfigEditor.Core.Util
{
    /// <summary>
    /// 内核性能优化辅助类
    /// </summary>
    public class KernelHelper
    {
        /// <summary>
        /// 计算耗时
        /// </summary>
        /// <param name="phase">阶段</param>
        /// <param name="interval">间隔，单位毫秒</param>
        /// <returns></returns>
        public static string ExecTimeSpan(string phase, ref int tick)
        {
            int interval = Environment.TickCount - tick;
            tick = Environment.TickCount;
            return string.Format("{0}, 耗时{1:00}:{2:00}:{3:00}.{4:000}", phase, interval / 1000 / 60 / 60, interval / 1000 / 60 % 60, interval / 1000 % 60 % 60, interval % 1000 % 60 % 60);
        }

        /// <summary>
        /// 计算耗时
        /// </summary>
        /// <param name="method">阶段</param>
        /// <param name="interval">间隔，单位毫秒</param>
        /// <returns></returns>
        public static string ExecTimeSpan(MethodBase method, ref int tick)
        {
            int interval = Environment.TickCount - tick;
            tick = Environment.TickCount;
            return string.Format("{0}.{1}, ThreadId:{2}, 耗时{3:00}:{4:00}:{5:00}.{6:000}", method.ReflectedType.Name, method.Name, Thread.CurrentThread.ManagedThreadId, interval / 1000 / 60 / 60, interval / 1000 / 60 % 60, interval / 1000 % 60 % 60, interval % 1000 % 60 % 60);
        }


        /// <summary>
        /// 记录线程信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public static string ExecThread(MethodBase method)
        {
            return string.Format("{0}.{1}, ThreadId:{2}, Priority:{3}, IsBackground:{4}", method.ReflectedType.Name, method.Name, Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.Priority, Thread.CurrentThread.IsBackground);
        }

        /// <summary>
        /// 记录异步读取请求
        /// </summary>
        /// <param name="requestHandle"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public static string ExecAsyncReadRequest(object requestHandle, string[] items)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("异步读取[请求-{0}]", requestHandle));
            foreach (string item in items)
            {
                sb.AppendLine(item);
            }

            return sb.ToString();
        }

        /// <summary>
        /// 记录异步读取响应
        /// </summary>
        /// <param name="requestHandle"></param>
        /// <param name="results"></param>
        /// <returns></returns>
        public static string ExecAsyncReadResponse(object requestHandle, IDictionary<string, string> results)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("异步读取[响应-{0}]", requestHandle));
            foreach (var pair in results)
            {
                sb.AppendLine(string.Format("{0}{1}", pair.Key.PadRight(50, ' '), pair.Value));
            }

            return sb.ToString();
        }

        /// <summary>
        /// 记录异步写入请求
        /// </summary>
        /// <param name="requestHandle"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public static string ExecAsyncWriteRequest(object requestHandle, Dictionary<string, object> items)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("异步写入[请求-{0}]", requestHandle));
            foreach (var item in items)
            {
                sb.AppendLine(item.Key + " = " + ArrayHelper.GetItemValue(item.Value));
            }

            return sb.ToString();
        }

        /// <summary>
        /// 记录异步写入响应
        /// </summary>
        /// <param name="requestHandle"></param>
        /// <param name="results"></param>
        /// <returns></returns>
        public static string ExecAsyncWriteResponse(object requestHandle, IDictionary<string, string> results)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("异步写入[响应-{0}]", requestHandle));
            foreach (var pair in results)
            {                
                sb.AppendLine(string.Format("{0}{1}", pair.Key.PadRight(50, ' '), pair.Value));
            }

            return sb.ToString();
        }

        /// <summary>
        /// 记录异步订阅通知
        /// </summary>
        /// <param name="requestHandle"></param>
        /// <param name="results"></param>
        /// <returns></returns>
        public static string ExecAsyncSubscribeNotification(IDictionary<string, string> results)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("异步订阅[通知]");
            foreach (var pair in results)
            {
                sb.AppendLine(string.Format("{0}{1}", pair.Key.PadRight(50, ' '), pair.Value));
            }

            return sb.ToString();
        }
    }
}
