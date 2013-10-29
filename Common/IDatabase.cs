using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace MicroDAQ.Common
{
    public interface IDatabase : IDisposable
    {
        SqlConnection UpdateConnection { set; get; }
        SqlConnection GetdataConnection { set; get; }
        bool UpdateItem(IItem item);
        DataRow[] GetRemoteControl();
    }
}
