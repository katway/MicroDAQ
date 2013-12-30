using System;
using System.Collections.Generic;
using System.Text;
using JonLibrary.Common;
using JonLibrary.Automatic;
using log4net;
using MicroDAQ.DataItem;
using OpcOperate.Sync;
using OpcOperate.ASync;
using MicroDAQ.Gateway;
using MicroDAQ.Database;


namespace MicroDAQ.Specifical
{
    /// <summary>
    /// 用于正式运行前，装载运行所需信息
    /// </summary>
    internal class Loader
    {

        /// <summary>
        /// 使用哪个OPCServer
        /// </summary>
        string opcServerType = "SimaticNet";

        private string wordItemFormat;
        private string wordArrayItemFormat;
        private string realItemFormat;

        internal List<PLCStationInformation> Plcs;
        private OpcOperate.Sync.OPCServer SyncOpc;
        IniFile ini = null;

        ILog log;


        internal Loader()
        {
            log = LogManager.GetLogger(this.GetType());
            Plcs = new List<PLCStationInformation>();

            ini = new IniFile(AppDomain.CurrentDomain.BaseDirectory + "MicroDAQ.ini");

            int plcCount = int.Parse(ini.GetValue("PLCConfig", "Amount"));
            opcServerType = ini.GetValue("OpcServer", "Type").Trim();
            for (int i = 0; i < plcCount; i++)
            {
                PLCStationInformation plc = new PLCStationInformation();
                Plcs.Add(plc);
                plc.Connection = string.Format(ini.GetValue(opcServerType, "ConnectionString"), i + 1);

            }
        }



        /// <summary>
        /// 读取配置
        /// </summary>
        private bool ReadConfig()
        {
            bool success = false;
            try
            {
                //读取Item地址格式           
                string[] getPairsConfigItems = new string[Plcs.Count];
                wordItemFormat = ini.GetValue(opcServerType, "WordItemFormat");
                wordArrayItemFormat = ini.GetValue(opcServerType, "WordArrayItemFormat");
                realItemFormat = ini.GetValue(opcServerType, "RealItemFormat");
                if (SyncOpc.AddGroup("ConfigGroups"))
                {
                    #region 是否多组,有几组
                    //生成读取是否多组,有几组的Item地址
                    for (int i = 0; i < Plcs.Count; i++)
                    {
                        getPairsConfigItems[i] = Plcs[i].Connection + string.Format(wordArrayItemFormat, 1, 26, 2);
                    }
                    //获取是否多组的数据,并转存到PlcStation列表里
                    int[] itemHandle = new int[Plcs.Count];
                    object[] values = new object[Plcs.Count];
                    if (SyncOpc.AddItems("ConfigGroups", getPairsConfigItems, itemHandle))
                    {
                        foreach (string item in getPairsConfigItems)
                            log.Info(item);

                        SyncOpc.SyncRead("ConfigGroups", values, itemHandle);
                        for (int i = 0; i < Plcs.Count; i++)
                        {
                            ushort[] value = (ushort[])values[i];
                            Plcs[i].MorePair = (value[0] == 2) ? (true) : false;
                            Plcs[i].PairsNumber = value[1];
                        }
                    }
                    #endregion

                    #region 每个PLC中每个DB组有多少监测点
                    //读取配置监测点数量的Item,每个PLC的DB1,W30,W32|W34,W36|W38....
                    //生成读取配置监测点数量的Items
                    string[][] getItemsNumber = null;   //第一维为PlcStation编号,第二维为数量组的编号
                    getItemsNumber = new string[Plcs.Count][];
                    for (int i = 0; i < Plcs.Count; i++)
                    {
                        getItemsNumber[i] = new string[Plcs[i].PairsNumber];
                        for (int j = 0; j < Plcs[i].PairsNumber; j++)
                        {
                            getItemsNumber[i][j] = Plcs[i].Connection + string.Format(wordArrayItemFormat, 1, 30 + j * 4, 2);
                        }
                    }
                    //获取每个PLC中,每个DB组中存放的监测点数量
                    for (int i = 0; i < Plcs.Count; i++)
                    {
                        itemHandle = new int[Plcs[i].PairsNumber];
                        values = new object[Plcs[i].PairsNumber];

                        foreach (string item in getItemsNumber[i])
                            log.Info(item);
                        if (SyncOpc.AddItems("ConfigGroups", getItemsNumber[i], itemHandle))
                        {

                            SyncOpc.SyncRead("ConfigGroups", values, itemHandle);
                            //取1个PLC中的每个DB组中存放的监测点数量
                            for (int j = 0; j < Plcs[i].PairsNumber; j++)
                            {
                                ushort[] value = (ushort[])values[j];
                                Plcs[i].ItemsNumber[j].SmallItems = value[0];
                                Plcs[i].ItemsNumber[j].BigItems = value[1];
                            }
                        }
                    }
                    #endregion
                    success = true;
                }

            }
            catch (Exception ex)
            {
                log.Error(ex);
                success = false;
            }
            return success;
        }
        /// <summary>
        /// 创建Items项
        /// </summary>
        private bool CreateItems()
        {
            bool success = false;
            try
            {
                //遍历所有PLC
                for (int i = 0; i < Plcs.Count; i++)
                {
                    PLCStationInformation plc = Plcs[i];
                    //遍历PLC中所有DB组
                    for (int j = 0; j < plc.ItemsNumber.Length; j++)
                    {
                        PLCStationInformation.ConfigItemsNumber num = plc.ItemsNumber[j];

                        //根据20字节监测点数量生成Item地址
                        for (int k = 0; k < num.BigItems; k++)
                        {
                            plc.ItemsHead.Add(plc.Connection + string.Format(wordArrayItemFormat, 4 + j * 2, 20 * k, 3));
                            plc.ItemsData.Add(plc.Connection + string.Format(realItemFormat, 4 + j * 2, 20 * k + 10));
                        }

                        //根据10字节监测点数量生成Item地址
                        for (int k = 0; k < num.SmallItems; k++)
                        {
                            plc.ItemsHead.Add(plc.Connection + string.Format(wordArrayItemFormat, 3 + j * 2, 10 * k, 3));
                            plc.ItemsData.Add(plc.Connection + string.Format(realItemFormat, 3 + j * 2, 10 * k + 6));
                        }

                    }
                }
                success = true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return success;
        }
        /// <summary>
        /// 创建数据项管理器
        /// </summary>
        private IList<IDataItemManage> createItemsMangers()
        {
            bool success = false;
            IList<IDataItemManage> listDataItemManger = new List<IDataItemManage>();
            try
            {
                foreach (var plc in Plcs)
                {
                    string[] head = new string[plc.ItemsHead.Count];
                    string[] data = new string[plc.ItemsHead.Count];
                    plc.ItemsHead.CopyTo(head, 0);
                    plc.ItemsData.CopyTo(data, 0);
                    IDataItemManage dim = new DataItemManager("ItemData", head, data);
                    listDataItemManger.Add(dim);
                }

                success = true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            if (success)
                return listDataItemManger;
            else
                return null;
        }


        private void createCtrl()
        {
            string pid = ini.GetValue(opcServerType, "ProgramID");
            int i = 0;
            foreach (var plc in Plcs)
            {
                Controller MetersCtrl = new Controller("MetersCtrl",

                                        new string[] { plc.Connection + string.Format(wordArrayItemFormat, 2, 100, 5) },
                                        new string[] { plc.Connection + string.Format(wordArrayItemFormat, 2, 120, 5) }
                                      );
                Program.MeterManager.CTMeters.Add(90 + i++, MetersCtrl);
                MetersCtrl.Connect(pid, "127.0.0.1");
            }
        }


        private IList<IDatabaseManage> createDBManagers()
        {
            bool success = false;
            IList<IDatabaseManage> listDatabaseManger = new List<IDatabaseManage>();
            string[] dbs = ini.GetValue("Database", "Members").Trim().Split(',');
            try
            {
                foreach (string dbName in dbs)
                {
                    DatabaseManage dbm = new DatabaseManage(ini.GetValue(dbName, "Address"),
                                                                 ini.GetValue(dbName, "PersistSecurityInfo"),
                                                                 ini.GetValue(dbName, "Database"),
                                                                 ini.GetValue(dbName, "Username"),
                                                                 ini.GetValue(dbName, "Password"));

                    listDatabaseManger.Add(dbm);
                    success = true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            if (success)
                return listDatabaseManger;
            else
                return null;

        }
        public bool Start()
        {
            bool success = false;
            SyncOpc = new OPCServer();
            string pid = ini.GetValue(opcServerType, "ProgramID");
            if (SyncOpc.Connect(pid, "127.0.0.1"))
            {
                if (ReadConfig())
                    if (CreateItems())
                    {
                        createCtrl();
                        Program.opcGateway = new OpcGateway(createItemsMangers(), createDBManagers());
                        Program.opcGateway.Start(pid);
                        ///增加可配置的间隔时间
                        int interval = 1000;
                        int.TryParse(ini.GetValue("Database", "UpdateInterval"), out interval);
                        Program.opcGateway.UpdateInterval = interval;
                        success = true;
                    }
            }
            else
            {
                success = false;
            }
            return success;
        }
    }
}
