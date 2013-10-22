/**
 * 文件名：LogHelper.cs
 * 说明：Log4net日志
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
using System.Diagnostics;
using log4net;

namespace ConfigEditor.Core.Util
{
    /// <summary>
    /// Log4net日志 
    /// </summary>
    public class LogHelper
    {
        //日志
        public readonly static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public readonly static ILog logio = LogManager.GetLogger("IOLogger");
        public readonly static ILog logker = LogManager.GetLogger("KernelLogger");
        public readonly static ILog logsql = LogManager.GetLogger("SQLLogger");
        public readonly static ILog logda = LogManager.GetLogger("DALogger");
        public readonly static ILog logopen = LogManager.GetLogger("OpenLogger");

        public const string Unknown = "Unknown";

        public static void Debug(object message)
        {
            log.Debug(message);
        }

        public static void Info(object message)
        {
            log.Info(message);
        }

        public static void Warn(object message)
        {
            log.Warn(message);
        }

        public static void Error(object message)
        {
            log.Error(message);
        }

        public static void Fatal(object message)
        {
            log.Fatal(message);
        }

        /// <summary>
        /// IO输出
        /// </summary>
        /// <param name="message"></param>
        public static void Output(object message)
        {
            logio.Info(">> " + message);
        }

        /// <summary>
        /// IO输出
        /// </summary>
        /// <param name="device"></param>
        /// <param name="tag"></param>
        /// <param name="message"></param>
        public static void Output(string channel, string device, string tag, object message)
        {
            logio.InfoFormat("Channel({0}) Device({1}) Tag({2}) >> {3}", channel, device, tag, message);
        }

        /// <summary>
        /// IO输入
        /// </summary>
        /// <param name="message"></param>
        public static void Input(object message)
        {
            logio.Info("<< " + message);
        }

        /// <summary>
        /// IO输入
        /// </summary>
        /// <param name="device"></param>
        /// <param name="tag"></param>
        /// <param name="message"></param>
        public static void Input(string channel, string device, string tag, object message)
        {
            logio.InfoFormat("Channel({0}) Device({1}) Tag({2}) << {3}", channel, device, tag, message);
        }

        /// <summary>
        /// IO输入
        /// </summary>
        /// <param name="device"></param>
        /// <param name="tag"></param>
        /// <param name="message"></param>
        public static void InputOfTimeout(string channel, string device, string tag, object message)
        {
            logio.InfoFormat("Channel({0}) Device({1}) Tag({2}) << [Timeout] {3}", channel, device, tag, message);
        }

        /// <summary>
        /// 输出内核数据
        /// </summary>
        /// <param name="message"></param>
        public static void Kernel(string message)
        {
            logker.Info(message);
        }

        /// <summary>
        /// 输出内核数据
        /// </summary>
        /// <param name="message"></param>
        public static void Kernel(string message, params object[] args)
        {
            logker.Info(string.Format(message, args));
        }

        /// <summary>
        /// 输出SQL语句
        /// </summary>
        /// <param name="message"></param>
        public static void Sql(string message)
        {
            logsql.Info(message);
        }

        /// <summary>
        /// 客户端数据项访问
        /// </summary>
        /// <param name="message"></param>
        public static void Da(string message)
        {
            logda.Info(message);
        }

        /// <summary>
        /// 服务器读取消息
        /// </summary>
        /// <param name="message"></param>
        public static void OpenRead(string message)
        {
            logopen.InfoFormat("Recv << {0}", message);
        }

        /// <summary>
        /// 服务器写入消息
        /// </summary>
        /// <param name="message"></param>
        public static void OpenWrite(string message)
        {
            logopen.InfoFormat("Send >> {0}", message);
        }
    }
}
