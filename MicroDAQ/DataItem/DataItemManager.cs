using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MicroDAQ.DataItem;
using JonLibrary.OPC;
using MicroDAQ.Specifical;


namespace MicroDAQ.DataItem
{
    /// <summary>
    /// 数量项管理器
    /// </summary>
    public class DataItemManager : IDataItemManage
    {
        public IList<Item> Items { get; set; }
        public ConnectionState ConnectionState { get; set; }
        public Dictionary<int, Item> ItemPair = null;
        /// <summary>
        /// 使用由指定的xx建立管理器
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dataHead"></param>
        /// <param name="data"></param>
        public DataItemManager(string name, int[] id, string[] data)
            : base()
        {
            machine = new DataItem(this, name, id, data);
            int count = (id.Length < data.Length) ? (id.Length) : (data.Length);
            Items = new List<Item>();
            ItemPair = new Dictionary<int, Item>();
            for (int i = 0; i < count; i++)
                Items.Add(new Item(id[i]));
        }
        Machine machine;
        public bool Connect(string OpcServerProgramID, string OPCServerAddress)
        {
            return machine.Connect(OpcServerProgramID, OPCServerAddress);
        }
        protected void UpdateItemPair(int key, Item item)
        {
            if (!ItemPair.ContainsKey(key))
            { ItemPair.Add(key, item); }
            ItemPair[key] = item;
        }

        class DataItem : Machine
        {
            DataItemManager Manager;
            int[] ID;
            public DataItem(DataItemManager manager, string name, int[] id, string[] data)
                : base()
            {
                this.Manager = manager;
                this.Name = Name;
                this.ID = id;
                ItemStatus = data;
            }

            internal protected override bool Connect(string OpcServerProgramID, string OPCServerIP)
            {
                bool success = true;
                success &= PLC.Connect(OpcServerProgramID, OPCServerIP);
                success &= PLC.AddGroup(GROUP_NAME_STATE, 1, 0);
                success &= PLC.AddItems(GROUP_NAME_STATE, ItemStatus);

                PLC.SetState(GROUP_NAME_STATE, true);
                ConnectionState = (success) ? (ConnectionState.Open) : (ConnectionState.Closed);
                return success;
            }


            protected override void PLC_DataChange(string groupName, int[] item, object[] value, short[] Qualities)
            {
                base.PLC_DataChange(groupName, item, value, Qualities);
                switch (groupName)
                {

                    case GROUP_NAME_STATE:
                        for (int i = 0; i < item.Length; i++)
                        {
                            if (value[i] != null)
                            {
                                Manager.Items[item[i]].ID = ID[item[i]];
                                Manager.Items[item[i]].Value = value[i];
                                Manager.Items[item[i]].Quality = Qualities[i];
                                Manager.Items[item[i]].DataTime = DateTime.Now;
                                Manager.Items[item[i]].State = DataState.正常;
                            }
                        }
                        break;
                }
                OnStatusChannge();
            }
        }

    }
}
