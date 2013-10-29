using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MicroDAQ.DataItem;
using JonLibrary.OPC;
using MicroDAQ.Common;


namespace MicroDAQ.DataItem
{
    /// <summary>
    /// OPC数量项管理器
    /// </summary>
    public class OpcDataItemManager : IDataItemManage
    {
        public List<IItem> Items { get; set; }
        public ConnectionState ConnectionState { get; set; }
        public Dictionary<int, IItem> ItemPair = null;
        /// <summary>
        /// 使用由指定的xx建立管理器
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dataHead"></param>
        /// <param name="data"></param>
        public OpcDataItemManager(string name, string[] dataHead, string[] data)
            : base()
        {
            machine = new DataItem(this, name, dataHead, data);
            int count = (dataHead.Length < data.Length) ? (dataHead.Length) : (data.Length);
            Items = new List<IItem>();
            ItemPair = new Dictionary<int, IItem>();
            for (int i = 0; i < count; i++)
                Items.Add(new Item());
        }
        Machine machine;
        public bool Connect(string OpcServerProgramID, string OPCServerAddress)
        {
            return machine.Connect(OpcServerProgramID, OPCServerAddress);
        }

        class DataItem : Machine
        {
            OpcDataItemManager Manager;

            public DataItem(OpcDataItemManager manager, string name, string[] dataHead, string[] data)
                : base()
            {
                this.Manager = manager;
                this.Name = Name;
                ItemCtrl = dataHead;
                ItemStatus = data;

            }

            internal protected override bool Connect(string OpcServerProgramID, string OPCServerIP)
            {
                bool success = true;
                success &= PLC.Connect(OpcServerProgramID, OPCServerIP);
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
                                Manager.Items[item[i]].Value = (float)value[i];
                                Manager.Items[item[i]].Quality = Qualities[i];
                            }
                        }
                        break;
                }
                OnStatusChannge();
            }


        }
        protected void UpdateItemPair(int key, IItem item)
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
        {
            throw new NotImplementedException();
        }
    }
}
