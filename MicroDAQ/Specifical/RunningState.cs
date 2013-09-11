using System;
using System.Collections.Generic;
using System.Text;

namespace MicroDAQ.Specifical
{
    public enum RunningState
    {
        已读取 = 1,
        执行中 = 2,
        完成 = 4,
        错误 = 8,
        未完成 = 16
    }
}
