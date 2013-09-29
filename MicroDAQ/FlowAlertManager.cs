using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MicroDAQ.DataItem;
using JonLibrary.OPC;
using MicroDAQ.Common;


namespace MicroDAQ
{
    class FlowAlertManager : IDataItemManage
    {
        public IList<Item> Items { get; set; }
        public Dictionary<int, Item> ItemPair = null;
        //public FlowAlertManager(string name, string[] dataHead, string[] data)
        //    : base(name, dataHead, data)
        //{ }
        Machine machine = null;

        public ConnectionState ConnectionState { get; set; }
        public FlowAlertManager(string name, string[] dataHead, string[] data)
            : base()
        {
            machine = new FlowAlert(this, name, dataHead, data);
            //this.Name = Name;
            //ItemCtrl = dataHead;
            //ItemStatus = data;
            int count = (dataHead.Length < data.Length) ? (dataHead.Length) : (data.Length);
            Items = new List<Item>();
            ItemPair = new Dictionary<int, Item>();
            for (int i = 0; i < count; i++)
                Items.Add(new Item());
        }
        public bool Connect(string OpcServerProgramID, string OpcServerAddress)
        {
            return machine.Connect(OpcServerProgramID, OpcServerAddress);
        }



        protected void UpdateItemPair(int key, Item item)
        {
            if (!ItemPair.ContainsKey(key))
            { ItemPair.Add(key, item); }
            ItemPair[key] = item;
        }
        public void StartSynchronize()
        {
            throw new NotImplementedException();
        }
        public void StopSynchronize()
        {
            throw new System.NotImplementedException();
        }
        public void Dispose()
        { throw new NotImplementedException(); }
        /// <summary>
        /// 用于实现特定业务功能的内部类 
        /// </summary>
        class FlowAlert : JonLibrary.OPC.Machine
        {
            FlowAlertManager Manager = null;
            public FlowAlert(FlowAlertManager manager, string name, string[] dataHead, string[] data)
                : base()
            {
                this.Manager = manager;
                this.Name = Name;
                ItemCtrl = dataHead;
                ItemStatus = data;
                int count = (dataHead.Length < data.Length) ? (dataHead.Length) : (data.Length);
                //Items = new List<Item>();
                //ItemPair = new Dictionary<int, Item>();
                //for (int i = 0; i < count; i++)
                //    Items.Add(new Item());
            }



            internal protected override bool Connect(string OpcServerProgreamID, string OPCServerIP)
            {
                bool success = true;
                success &= PLC.Connect(OpcServerProgreamID, OPCServerIP);
                success &= PLC.AddGroup(GROUP_NAME_CTRL, 1, 0);
                success &= PLC.AddItems(GROUP_NAME_CTRL, ItemCtrl);
                success &= PLC.AddGroup(GROUP_NAME_STATE, 1, 0);
                success &= PLC.AddItems(GROUP_NAME_STATE, ItemStatus);
                PLC.SetState(GROUP_NAME_CTRL, true);
                PLC.SetState(GROUP_NAME_STATE, true);
                ConnectionState = (success) ? (ConnectionState.Open) : (ConnectionState.Closed);
                return success;
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
                                Manager.Items[item[i]].ID = val[0];
                                Manager.Items[item[i]].Type = (ItemType)val[1];
                                Manager.Items[item[i]].State = (ItemState)val[2];
                                Manager.Items[item[i]].Quality = Qualities[i];
                                Manager.UpdateItemPair(Manager.Items[item[i]].ID, Manager.Items[item[i]]);
                            }
                        }
                        break;
                    case GROUP_NAME_STATE:
                        for (int i = 0; i < item.Length; i++)
                        {
                            if (value[i] != null)
                            {
                                Manager.Items[item[i]].Value = ((ushort)value[i] & (ushort)2);
                                Manager.Items[item[i]].Quality = Qualities[i];
                            }
                        }
                        break;
                }
                OnStatusChannge();
            }


        }
    }
}
