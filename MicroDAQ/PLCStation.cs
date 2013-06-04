using System;
using System.Collections.Generic;
using System.Text;

namespace MicroDAQ
{
    public class PLCStation
    {
        public string ProjectCode { get; set; }
        public string Version { get; set; }
        public int PlcTick { get; set; }
        public string plcConnection { get; set; }

        public string[] ItemsHead { get; set; }
        public string[] ItemsData { get; set; }
    }

}
