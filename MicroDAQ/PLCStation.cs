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
        public string Connection { get; set; }

        public bool MorePair { get; set; }
        public int PairsNumber { get; set; }
        internal ConfigItemsNumber[] ItemsNumber { get; set; }

        public string[] ItemsHead { get; set; }
        public string[] ItemsData { get; set; }



        internal struct ConfigItemsNumber
        {
            internal int BigItems;
            internal int SmallItems;
        }

        
    }

}
