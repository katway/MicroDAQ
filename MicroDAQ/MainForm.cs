using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using JonLibrary.OPC;
using JonLibrary.Automatic;
using JonLibrary.Common;
using System.Threading;
using MicroDAQ.UI;
using OpcOperate.Sync;
using MicroDAQ.DataItem;
using MicroDAQ.Database;
using MicroDAQ.Gateway;
using log4net;
using System.Data.SqlClient;
using MicroDAQ.Specifical;

namespace MicroDAQ
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// 使用哪个OPCServer
        /// </summary>
        string opcServerType = "SimaticNet";

        private string wordItemFormat;
        private string wordArrayItemFormat;
        private string realItemFormat;

        private List<PLCStationInformation> Plcs;
        private OpcOperate.Sync.OPCServer SyncOpc;
        IniFile ini = null;

        ILog log;
        public MainForm()
        {
            log = LogManager.GetLogger(this.GetType());
            InitializeComponent();
            Plcs = new List<PLCStationInformation>();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            Console.WriteLine(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());
            ni.Icon = this.Icon;
            ni.Text = this.Text;

            bool autoStart = false;
            try
            {
                ini = new IniFile(AppDomain.CurrentDomain.BaseDirectory + "MicroDAQ.ini");
                this.Text = ini.GetValue("General", "Title");
                this.tsslProject.Text = "项目代码：" + ini.GetValue("General", "ProjetCode");
                this.tsslVersion.Text = "程序版本：" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
                autoStart = bool.Parse(ini.GetValue("AutoRun", "AutoStart"));
                int plcCount = int.Parse(ini.GetValue("PLCConfig", "Amount"));
                opcServerType = ini.GetValue("OpcServer", "Type").Trim();
                for (int i = 0; i < plcCount; i++)
                {
                    PLCStationInformation plc = new PLCStationInformation();
                    Plcs.Add(plc);
                    plc.Connection = string.Format(ini.GetValue(opcServerType, "ConnectionString"), i + 1);

                }


            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                if (autoStart)
                    btnStart_Click(null, null);
            }
            ni.Text = this.Text;
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
        public void Start()
        {
            //Thread.Sleep(Program.waitMillionSecond);
            SyncOpc = new OPCServer();
            string pid = ini.GetValue(opcServerType, "ProgramID");
            if (SyncOpc.Connect(pid, "127.0.0.1"))
                if (ReadConfig())
                    if (CreateItems())
                    {
                        createCtrl();
                        Program.opcGateway = new OpcGateway(createItemsMangers(), createDBManagers());
                        Program.opcGateway.StateChanged += new EventHandler(opcGateway_StateChanged);
                        Program.opcGateway.UpdateCycle.WorkStateChanged += new CycleTask.WorkStateChangeEventHandle(UpdateCycle_WorkStateChanged);
                        Program.opcGateway.RemoteCtrlCycle.WorkStateChanged += new CycleTask.WorkStateChangeEventHandle(RemoteCtrlCycle_WorkStateChanged);
                        Program.opcGateway.Start(pid);
                    }

        }

        void opcGateway_StateChanged(object sender, EventArgs e)
        {
            Console.WriteLine((sender as OpcGateway).RunningState);
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(delegate
                {
                    if (Program.opcGateway.RunningState == Gateway.RunningState.Running)
                    {
                        //添加获取采集点的数量
                        this.tsddbPLC.DropDownItems.Clear();
                        foreach (PLCStationInformation plc in Plcs)
                        {
                            ToolStripMenuItem tsiPLC = new ToolStripMenuItem(plc.Connection);
                            this.tsddbPLC.DropDownItems.Add(tsiPLC);
                            for (int i = 0; i < plc.ItemsNumber.Length; i++)
                            {
                                ToolStripMenuItem tsiItemGrop = new ToolStripMenuItem(string.Format("第{0}对DB块", i + 1));
                                tsiPLC.DropDownItems.Add(tsiItemGrop);

                                tsiItemGrop.DropDownItems.Add(string.Format("10字节监测点数：{0}", plc.ItemsNumber[i].SmallItems));
                                tsiItemGrop.DropDownItems.Add(string.Format("20字节监测点数：{0}", plc.ItemsNumber[i].BigItems));
                            }
                        }
                    }
                }));
            }
        }

        void RemoteCtrlCycle_WorkStateChanged(JonLibrary.Automatic.RunningState state)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                switch (state)
                {
                    case JonLibrary.Automatic.RunningState.Paused:
                        this.tsslRemote.Text = "P";
                        this.btnPC.Text = "继续";
                        break;
                    case JonLibrary.Automatic.RunningState.Running:
                        this.tsslRemote.Text = "R";
                        this.btnPC.Enabled = true;
                        this.btnPC.Text = "暂停";
                        break;
                    case JonLibrary.Automatic.RunningState.Stopped:
                        this.tsslRemote.Text = "S";
                        break;
                }
            }));
        }




        void UpdateCycle_WorkStateChanged(JonLibrary.Automatic.RunningState state)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                switch (state)
                {
                    case JonLibrary.Automatic.RunningState.Paused:
                        this.tsslUpdate.Text = "P";
                        this.btnPC.Text = "继续";
                        break;
                    case JonLibrary.Automatic.RunningState.Running:
                        this.tsslUpdate.Text = "R";
                        this.btnPC.Enabled = true;
                        this.btnPC.Text = "暂停";
                        break;
                    case JonLibrary.Automatic.RunningState.Stopped:
                        this.tsslUpdate.Text = "S";
                        break;
                }
            }));
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            this.btnStart.Enabled = false;
            this.Start();
        }

        private void btnPC_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(Program.opcGateway.RunningState.ToString());
            if (Program.opcGateway.RunningState == Gateway.RunningState.Running)
                Program.opcGateway.Pause();
            else
                Program.opcGateway.Continue();
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!Program.BeQuit)
            {
                this.WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false;
                e.Cancel = true;
            }
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            switch (this.WindowState)
            {
                case FormWindowState.Normal:
                    this.ShowInTaskbar = true;
                    break;
                case FormWindowState.Minimized:
                    this.ShowInTaskbar = false;
                    break;
            }
        }

        private void 退出EToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("这将使数据采集系统退出运行状态，确定要退出吗？", "退出", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                    == System.Windows.Forms.DialogResult.Yes)
            {
                this.Hide();
                Program.BeQuit = true;
                Thread.Sleep(200);
                this.Close();
            }
        }

        private void ni_DoubleClick(object sender, EventArgs e)
        {
            switch (this.WindowState)
            {
                case FormWindowState.Normal:
                    this.WindowState = FormWindowState.Minimized;
                    break;
                case FormWindowState.Minimized:
                    this.WindowState = FormWindowState.Normal;
                    break;
            }
        }

        Form frmDataDisplay = null;
        private void tsslMeters_Click(object sender, EventArgs e)
        {
            if (frmDataDisplay != null && !frmDataDisplay.IsDisposed)
                frmDataDisplay.Show();
            else
            {
                (frmDataDisplay = new DataDisplayForm()).Show();
            }
        }
    }
}
