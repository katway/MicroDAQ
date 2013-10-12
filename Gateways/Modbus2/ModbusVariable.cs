using System;
using System.Collections.Generic;
using System.Text;
using MicroDAQ.Common;
using MicroDAQ.Configuration;

namespace MicroDAQ.Gateways.Modbus2
{
    public class ModbusVariable : IItem
    {
        public ModbusVariable(ModbusVariableInfo variableInfo)
        {
            VariableInfo = variableInfo;
            this.ID = variableInfo.code;
            this.originalValue = new ushort[variableInfo.length];
        }

        /// <summary>
        /// 变量ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public ItemType Type { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public ItemState State { get; set; }
        /// <summary>
        /// 时钟戳
        /// </summary>
        public int DataTick { get; set; }
        /// <summary>
        /// 同步时间戳
        /// </summary>
        public int SyncTick { get; set; }
        /// <summary>
        /// 获取时间
        /// </summary>
        public DateTime DataTime { get; set; }
        /// <summary>
        /// 同步时间
        /// </summary>
        public DateTime SyncTime { get; set; }
        /// <summary>
        /// 数据值
        /// </summary>
        public object Value
        {
            get
            { return value; }
            set
            {
                this.value = value;
                throw new NotImplementedException("还没写如何转为ushort数组哇");
                switch (this.VariableInfo.dataType.ToLower())
                {
                    case "ushort":
                    case "short":
                        break;
                    case "uint":
                    case "int":
                        break;
                    case "float":
                        break;
                    case "double":
                        break;
                    default:
                        break;
                }
            }
        }
        private object value;
        /// <summary>
        /// 通信质量
        /// </summary>
        public short Quality { get; set; }
        /// <summary>
        /// 原始数据
        /// </summary>
        public ushort[] OriginalValue { get { return originalValue; } }
        private ushort[] originalValue;

        /// <summary>
        /// 变量配置信息
        /// </summary>
        public MicroDAQ.Configuration.ModbusVariableInfo VariableInfo
        {
            get;
            set;
        }

    }
}
