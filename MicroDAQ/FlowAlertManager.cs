using System;
using System.Collections.Generic;
using System.Text;

namespace MicroDAQ
{
    class FlowAlertManager : DataItemManager
    {
        public FlowAlertManager(string name, string[] dataHead, string[] data)
            : base(name, dataHead, data)
        { }
        protected override void PLC_DataChange(string groupName, int[] item, object[] value, short[] Qualities)
        {
            base.PLC_DataChange(groupName, item, value, Qualities);
            switch (groupName)
            {
                case GROUP_NAME_CTRL:
                    for (int i = 0; i < item.Length; i++)
                    {
                        ushort[] val = null;
                        if (value[i] != null)
                        {
                            val = (ushort[])value[i];
                            this.Items[item[i]].ID = val[0];
                            this.Items[item[i]].Type = (DataType)val[1];
                            this.Items[item[i]].State = (DataState)val[2];
                            this.Items[item[i]].Quality = Qualities[i];
                            UpdateItemPair(this.Items[item[i]].ID, this.Items[item[i]]);
                        }
                    }
                    break;
                case GROUP_NAME_STATE:
                    for (int i = 0; i < item.Length; i++)
                    {
                        if (value[i] != null)
                        {
                            this.Items[item[i]].Value = ((ushort)value[i] & (ushort)2);
                            this.Items[item[i]].Quality = Qualities[i];
                        }
                    }
                    break;
            }
            OnStatusChannge();
        }
    }
}
