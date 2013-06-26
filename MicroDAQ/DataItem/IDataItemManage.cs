using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
namespace MicroDAQ.DataItem
{
    public interface IDataItemManage 
    {
        IList<Item> Items { get; set; }
        ConnectionState ConnectionStates { get;set;}
    }
}
