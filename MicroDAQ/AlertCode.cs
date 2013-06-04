using System;
using System.Collections.Generic;
using System.Text;

namespace MicroDAQ
{
    public enum AlertCode : byte
    {
        
        Red =1,
        Yellow = 2,
        Green = 4,
        Buzz = 8,
        BuzzRed = 9
}
}
