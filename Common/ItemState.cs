using System;
using System.Collections.Generic;
using System.Text;

namespace MicroDAQ.Common
{
    public enum ItemState
    {
        正常 = 1,
        仪表故障 = 2,
        仪表掉线 = 4,
        已启动 = 8,
        已停止 = 16
    }
}
