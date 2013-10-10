using System;
using System.Collections.Generic;
using System.Text;

namespace MicroDAQ.Common
{
    public interface IItem
    {

        int ID { get; set; }
        ItemType Type { get; set; }
        ItemState State { get; set; }
        int DataTick { get; set; }
        int SyncTick { get; set; }
        DateTime DataTime { get; set; }
        DateTime SyncTime { get; set; }
        object Value { get; set; }
        short Quality { get; set; }

    }
}
