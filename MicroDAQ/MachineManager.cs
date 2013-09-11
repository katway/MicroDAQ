using System;
using System.Collections.Generic;
using System.Text;
using JonLibrary.OPC;
using MicroDAQ.Specifical;

namespace MicroDAQ
{
    class MachineManager
    {
        private bool instanceFlag = false;
        public Dictionary<int, Machine> Meters;
        public Dictionary<int, Controller> CTMeters;
        public MachineManager()
        {
            if (!instanceFlag)
            {
                Meters = new Dictionary<int, Machine>();
                CTMeters = new Dictionary<int, Controller>();
                instanceFlag = true;
            }
            else
            {
                throw new Exception("不允许创建多实例");
            }
        }
    }
}
