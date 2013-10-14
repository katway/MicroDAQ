using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace MicroDAQ.Common
{
    public interface IDataItemManage : IDisposable
    {
        IList<IItem> Items { get; set; }
        void StartSynchronize();
        void StopSynchronize();
    }
}
