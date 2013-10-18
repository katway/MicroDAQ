using System;
using System.Collections.Generic;
using System.Text;

namespace MicroDAQ.Specifical
{
    public enum AlertCode : byte
    {

        Green = 1,
        Yellow = 2,
        Red = 4,
        Buzz = 8,
        BuzzRed = 12
}
}
