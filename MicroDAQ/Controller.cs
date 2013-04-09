using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using JonLibrary.Automatic;

namespace MicroDAQ
{
    class Controller : JonLibrary.OPC.Machine
    {
        public Controller(string Name, string[] Ctrl, string[] State)
        {
            this.Name = Name;
            ItemCtrl = Ctrl;
            ItemStatus = State;
        }
        public bool SetCommand(int runningNumber, int MeterID, int Cmd, int cmdValue)
        {
            ushort[] value = new ushort[5];
            value[0] = (ushort)(runningNumber & 0xFFFF);
            value[1] = (ushort)(MeterID & 0xFFFF);
            value[2] = (ushort)(Cmd & 0xFFFF);
            value[3] = (ushort)(cmdValue & 0xFFFF);
            value[4] = (ushort)(10& 0xFFFF);

            return PLC.Write(GROUP_NAME_CTRL, new object[] { value });
        }

        protected override void PLC_DataChange(string groupName, int[] item, object[] value, short[] Qualities)
        {

            switch (groupName)
            {
                case GROUP_NAME_CTRL:
                    break;
                case GROUP_NAME_STATE:
                    if (value[0] != null)
                    {
                        ushort[] val = (ushort[])value[0];

                        RunningNumber = (ushort)val[0];
                        MeterID = (ushort)val[1];

                        TaskState = (TaskState)(ushort)val[3];
                        DataTime = DateTime.Now;

                        bool r = true;
                        foreach (short q in Qualities)
                        {
                            r &= (q >= 192) ? (true) : (false);
                        }
                        ConnectionState = (r) ? (ConnectionState.Open) : (ConnectionState.Closed);

                    }
                    break;
            }
            DataTime = DateTime.Now;
            OnStatusChannge();
        }
        public int RunningNumber { get; private set; }
        public int MeterID { get; private set; }
        public TaskState TaskState { get; private set; }
        public int DataTick { get; private set; }
        public int SyncTick { get; set; }
        public DateTime DataTime { get; private set; }
        public DateTime SyncTime { get; private set; }
    }
}
