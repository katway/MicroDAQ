using System;
using System.Collections.Generic;
using System.Text;

namespace MicroDAQ.UI
{
    /// <summary>
    /// 读取的10字节或者20字节数据地址
    /// </summary>
    public struct ItemsAddress
    {
        /// <summary>
        /// 一般是前3个Word
        /// </summary>
        public string[] Head;
        /// <summary>
        /// 一般是1个Real
        /// </summary>
        public string[] Data;
    }
}
