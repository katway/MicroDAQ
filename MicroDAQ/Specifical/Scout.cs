using System;
using System.Collections.Generic;
using System.Text;
using OpcOperate.Sync;

namespace MicroDAQ.Specifical
{

    /// <summary>
    /// 用于系统正式运行前，检测各项前提条件是否满足运行需要
    /// </summary>
    internal class Scout
    {
        internal Scout(OPCServer syncOPC)
        {
            this.SyncOpc = syncOPC;
        }


        internal OPCServer SyncOpc { get; private set; }
    }
}
