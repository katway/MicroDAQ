using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace MicroDAQ.Database
{
    public interface IDatabaseManage
    {
        SqlConnection UpdateConnection { set; get; }
        SqlConnection GetdataConnection { set; get; }
        bool UpdateItem(MicroDAQ.DataItem.Item item);
    }
}
