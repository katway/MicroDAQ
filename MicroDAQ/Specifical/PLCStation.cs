﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MicroDAQ.Specifical
{
    public class PLCStationInformation
    {
        /// <summary>
        /// 项目代码
        /// </summary>
        public string ProjectCode { get; set; }
        /// <summary>
        /// 协议版本
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// PLC运行时间戳
        /// </summary>
        public int PlcTick { get; set; }
        /// <summary>
        /// 连接名称
        /// </summary>
        public string Connection { get; set; }

        /// <summary>
        /// 配置中声明的是否使用多个DB组
        /// </summary>
        public bool MorePair { get; set; }
        /// <summary>
        /// 配置中声明的DB组数量
        /// </summary>
        public int PairsNumber
        {
            get { return pairsNumber; }
            set
            {
                if (MorePair)
                    pairsNumber = value;
                else
                    pairsNumber = 1;

                ItemsNumber = new ConfigItemsNumber[value];
            }
        }
        private int pairsNumber;
        /// <summary>
        /// 配置中声明的监测点数量
        /// </summary>
        internal ConfigItemsNumber[] ItemsNumber { get; set; }
        /// <summary>
        /// 监测点数据头地址
        /// </summary>
        public IList<string> ItemsHead { get; set; }
        /// <summary>
        /// 监测点数据内容地址
        /// </summary>
        public IList<string> ItemsData { get; set; }

        public PLCStationInformation()
        {
            ItemsHead = new List<string>();
            ItemsData = new List<string>();
        }

        /// <summary>
        /// 监测点数量结构
        /// </summary>
        internal struct ConfigItemsNumber
        {
            /// <summary>
            /// 20字节格式的监测点数量
            /// </summary>
            internal int BigItems;
            /// <summary>
            /// 10字节格式的监测点数量
            /// </summary>
            internal int SmallItems;

            internal ConfigItemsNumber(int bigItems, int smallItems)
            {
                this.BigItems = bigItems;
                this.SmallItems = smallItems;
            }
        }


    }

}