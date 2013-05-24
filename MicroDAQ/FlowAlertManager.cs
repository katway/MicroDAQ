using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace MicroDAQ
{
    class FlowAlertManager : JonLibrary.OPC.Machine
    {
        public List<DataItem> Items = null;
        public Dictionary<int, DataItem> ItemPair = null;
        //public FlowAlertManager(string name, string[] dataHead, string[] data)
        //    : base(name, dataHead, data)
        //{ }

        public FlowAlertManager(string name, string[] dataHead, string[] data)
            : base()
        {
            this.Name = Name;
            ItemCtrl = dataHead;
            ItemStatus = data;
            int count = (dataHead.Length < data.Length) ? (dataHead.Length) : (data.Length);
            Items = new List<DataItem>();
            ItemPair = new Dictionary<int, DataItem>();
            for (int i = 0; i < count; i++)
                Items.Add(new DataItem());
        }



        internal protected override bool Connect(string OPCServerIP)
        {
            bool success = true;
            success &= PLC.Connect("OPC.SimaticNET", OPCServerIP);
            success &= PLC.AddGroup(GROUP_NAME_CTRL, 1, 0);
            success &= PLC.AddItems(GROUP_NAME_CTRL, ItemCtrl);
            success &= PLC.AddGroup(GROUP_NAME_STATE, 1, 0);
            success &= PLC.AddItems(GROUP_NAME_STATE, ItemStatus);
            PLC.SetState(GROUP_NAME_CTRL, true);
            PLC.SetState(GROUP_NAME_STATE, true);
            ConnectionState = (success) ? (ConnectionState.Open) : (ConnectionState.Closed);
            return success;
        }



        protected void UpdateItemPair(int key, DataItem item)
        {
            if (!ItemPair.ContainsKey(key))
            { ItemPair.Add(key, item); }
            ItemPair[key] = item;
        }
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
