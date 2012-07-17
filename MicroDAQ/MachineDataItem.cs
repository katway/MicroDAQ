using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MicroDAQ
{
    internal class MachineDataItem : Meter
    {
        public MachineDataItem(string Name, string[] Ctrl, string[] State)
            : base(Name, Ctrl, State)
        { }


        protected override void PLC_DataChange(string groupName, int[] item, object[] value, short[] Qualities)
        {
            switch (groupName)
            {
                case GROUP_NAME_CTRL:
                    break;
                case GROUP_NAME_STATE:
                    for (int i = 0; i < item.Length; i++)
                    {
                        if (value[i] != null)
                            switch (item[i])
                            {
                                case 0:
                                    ID = (ushort)value[i];
                                    break;
                                case 1:
                                    this.Type = (DataType)(ushort)value[i];
                                    break;
                                case 2:
                                    this.State = (DataState)(ushort)value[i];
                                    break;
                                case 3:
                                    this.Value1 = (float)value[i];
                                    break;                             
                            }
                    }
                    bool r = true;
                    foreach (short q in Qualities)
                    {
                        r &= (q >= 192) ? (true) : (false);
                    }
                    ConnectionState = (r) ? (ConnectionState.Open) : (ConnectionState.Closed);
                    break;
            }
            DataTime = DateTime.Now;
            OnStatusChannge();
        }
    }
}
