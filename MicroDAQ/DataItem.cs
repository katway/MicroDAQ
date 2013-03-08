using System;
using System.Collections.Generic;
using System.Text;

namespace MicroDAQ
{
    public class DataItem
    {
        public int ID { get; internal protected set; }
        public DataType Type { get; internal protected set; }
        public DataState State { get; internal protected set; }
        public int DataTick { get; internal protected set; }
        public int SyncTick { get; set; }
        public DateTime DataTime { get; internal protected set; }
        public DateTime SyncTime { get; internal protected set; }
        public float Value { get; internal protected set; }
        public short Quality { get; internal protected set; }
    }
}
