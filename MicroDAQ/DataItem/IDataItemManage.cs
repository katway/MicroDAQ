using System;
using System.Collections.Generic;
using System.Text;

namespace MicroDAQ.DataItem
{
    public interface IDataItemManage
    {
        List<Item> Items { get; set; }
    }
}
