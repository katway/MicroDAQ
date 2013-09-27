using System;
using System.Collections.Generic;
using System.Text;

namespace MicroDAQ.Common
{
    public interface IDatabaseManage
    {
        IList<MicroDAQ.Common.IDatabase> DatabaseList
        {
            get;
            set;
        }
    }
}
