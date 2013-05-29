using System;
using System.Collections.Generic;
using System.Text;

namespace MicroDAQ
{
    class PLCStation
    {
        public string ProjectCode { get; set; }
        public string Version { get; set; }
        public int PlcTick { get; set; }
        public string plcConnection { get; set; }

        public Item smallItem { get; set; }
        public Item bigItem { get; set; }

    }
    struct Item
    {
        public static int Count { get; set; }
    }
}
