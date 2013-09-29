/**
 * 文件名：SerialPortHelper.cs
 * 说明：串口辅助类
 * 作者：林安城
 * 更改记录： 
 * -------------------------------------------------------
 * 改动人 	时间 			原因
 * -------------------------------------------------------
 * 林安城 	2013-09-29		创建文件
 * -------------------------------------------------------
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfigEditor.Util
{
    /// <summary>
    /// 串口辅助类
    /// </summary>
    public class EditHelper
    {
        //常用波特率
        public static readonly int[] COMMON_BAUD_RATE = { 110, 300, 600, 1200, 2400, 4800, 9600, 14400, 19200, 38400, 56000, 57600, 115200 };

        public static readonly string[] DEVICES = new string[]
        {
            "标准",
        };


        public EditHelper()
        {
        }
    }
}
