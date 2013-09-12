using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace MicroDAQ.Common
{
    public interface IDatabaseManage
    {
        SqlConnection UpdateConnection { set; get; }
        SqlConnection GetdataConnection { set; get; }
        bool UpdateItem(Item item);
        DataRow[] GetRemoteControl();
    }
}
