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

        public int ID { get; set; }
        public ItemType Type { get; set; }
        public ItemState State { get; set; }
        public int DataTick { get; set; }
        public int SyncTick { get; set; }
        public DateTime DataTime { get; set; }
        public DateTime SyncTime { get; set; }
        public object Value
        {
            get
            { return value; }
            set
            {
                throw new NotImplementedException("还没写如何转为ushort数组哇");
                switch (this.VariableInfo.dataType)
                {

                }
            }
        }
        private float value;
        public short Quality { get; set; }
        /// <summary>
        /// 原始数据
        /// </summary>
        public ushort[] OriginalValue { get { return originalValue; } }
        private ushort[] originalValue;

        public MicroDAQ.Configuration.ModbusVariableInfo VariableInfo
        {
            get;
            set;
        }

    }
}
