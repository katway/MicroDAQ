using System;
using System.Collections.Generic;
using System.Text;

namespace MicroDAQ.Common
{
    public class Item : IItem
    {
        public int ID { get; set; }
        public ItemType Type { get; set; }
        public ItemState State { get; set; }
        public int DataTick { get; set; }
        public int SyncTick { get; set; }
        public DateTime DataTime { get; set; }
        public DateTime SyncTime { get; set; }
        public object Value { get; set; }
        public short Quality { get; set; }
    }
}
