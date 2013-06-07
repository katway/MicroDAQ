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
using SysncOpcOperate;


namespace MicroDAQ
{
    public partial class MainForm : Form
    {


        /// <summary>
        /// 使用哪个OPCServer
        /// </summary>
        string opcServerType = "SimaticNet";
        /// <summary>
        /// OPCServer名称
        /// </summary>
        string opcServerPorgramID = "OPC.SimaticNet";


        private List<PLCStationInformation> Plcs;
        private SysncOpcOperate.OPCServer SyncOpc;
        IniFile ini = null;

        public MainForm()
        {
            InitializeComponent();
            Plcs = new List<PLCStationInformation>();

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            ni.Icon = this.Icon;
            ni.Text = this.Text;

            bool autoStart = false;
            try
            {
                ini = new IniFile(AppDomain.CurrentDomain.BaseDirectory + "MicroDAQ.ini");
                this.Text = ini.GetValue("General", "Title");
                this.tsslProject.Text = "项目代码：" + ini.GetValue("General", "ProjetCode");
                this.tsslVersion.Text = "接口版本：" + ini.GetValue("General", "VersionCode");
                autoStart = bool.Parse(ini.GetValue("AutoRun", "AutoStart"));
                int plcCount = int.Parse(ini.GetValue("PLCConfig", "Amount"));

                opcServeType = ini.GetValue("OpcServer", "Type").Trim();
                for (int i = 0; i < plcCount; i++)
                {
                    PLCStationInformation plc = new PLCStationInformation();
                    Plcs.Add(plc);
                    plc.Connection = ini.GetValue(string.Format("PLC{0}", i), "Connection");
                }

            }
            catch
            { }
            finally
            {
                if (autoStart)
                    btnStart_Click(null, null);
            }
            ni.Text = this.Text;
        }

        private string opcServeType;
        private string wordItemFormat;
        private string wordArrayItemFormat;
        private string realItemFormat;

        /// <summary>
        /// 读取配置
        /// </summary>
        private void ReadConfig()
        {

            SyncOpc.Connect(ini.GetValue(opcServeType, "ProgramID"), "127.0.0.1");
            //读取Item地址格式           
            string[] getPairsConfigItems = new string[Plcs.Count];
            wordItemFormat = ini.GetValue(opcServeType, "WordItemFormat");
            wordArrayItemFormat = ini.GetValue(opcServeType, "WordItemFormat");
            realItemFormat = ini.GetValue(opcServeType, "RealItemFormat");

            if (SyncOpc.AddGroup())
            {
                #region 是否多组,有几组
                //生成读取是否多组,有几组的Item地址
                for (int i = 0; i < Plcs.Count; i++)
                {
                    getPairsConfigItems[i] = string.Format(wordArrayItemFormat, 1, 26, 2);
                }
                //获取是否多组的数据,并转存到PlcStation列表里
                int[] itemsHandle = new int[Plcs.Count];
                object[] values = new object[Plcs.Count];
                if (SyncOpc.AddItems(getPairsConfigItems, itemsHandle))
                {
                    SyncOpc.SyncRead(values, itemsHandle);
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
                for (int i = 0; i < Plcs.Count; i++)
                {
                    getItemsNumber[i] = new string[Plcs[i].PairsNumber];
                    for (int j = 0; j < Plcs[i].PairsNumber; j++)
                    {
                        getItemsNumber[i][j] = Plcs[i].Connection + string.Format(wordArrayItemFormat, 1, 30 + j * 2, 2);
                    }
                }
                //获取每个PLC中,每个DB组中存放的监测点数量
                for (int i = 0; i < Plcs.Count; i++)
                {
                    itemsHandle = new int[Plcs[i].PairsNumber];
                    values = new object[Plcs[i].PairsNumber];

                    if (SyncOpc.AddItems(getItemsNumber[i], itemsHandle))
                    {
                        SyncOpc.SyncRead(values, itemsHandle);
                        //取1个PLC中的每个DB组中存放的监测点数量
                        for (int j = 0; j < Plcs[i].PairsNumber; j++)
                        {
                            ushort[] value = (ushort[])values[j];
                            Plcs[i].ItemsNumber[j].BigItems = value[0];
                            Plcs[i].ItemsNumber[j].SmallItems = value[1];
                        }
                    }
                }

                #endregion
            }
        }
        /// <summary>
        /// 创建Items项
        /// </summary>
        private void CreateItems()
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
                        plc.ItemsHead.Add(plc.Connection + string.Format(wordArrayItemFormat, 3 + j, 20 * k, 3));
                        plc.ItemsData.Add(plc.Connection + string.Format(realItemFormat, 3 + j, 20 * k + 10));
                    }

                    //根据10字节监测点数量生成Item地址
                    for (int k = 0; k < num.SmallItems; k++)
                    {
                        plc.ItemsHead.Add(plc.Connection + string.Format(wordArrayItemFormat, 3 + j, 10 * k, 3));
                        plc.ItemsData.Add(plc.Connection + string.Format(realItemFormat, 3 + j, 10 * k + 6));
                    }

                }
            }
        }



        int updateMeters;
        int remoteMeters;

        bool readConfig = false;
        bool getConfig = false;
        bool createMeters = false;
        bool metersCreated = false;
        bool started = false;
        public void start()
        {
            if (!readConfig)
            {
                //等OPCSERVER启动
                Thread.Sleep(Program.waitMillionSecond);
                ReadConfig();
                readConfig = true;
            }

            if (getConfig && !started)
            {

                // CreateMeters(); 1

                started = true;
                CycleTask UpdateCycle = new CycleTask();
                CycleTask RemoteCtrl = new CycleTask();
                Program.RemoteCycle = RemoteCtrl;
                UpdateCycle.WorkStateChanged += new CycleTask.dgtWorkStateChange(UpdateCycle_WorkStateChanged);
                RemoteCtrl.WorkStateChanged += new CycleTask.dgtWorkStateChange(RemoteCtrl_WorkStateChanged);

                Start.SetExit = true;
            }
            Thread.Sleep(200);
        }



        private void CreateMeters()
        {
            int count = dataItems.Length;



            List<string> h = new List<string>();
            List<string> d = new List<string>();

            List<string> h_flow = new List<string>();
            List<string> d_flow = new List<string>();

            int meterIndex = 0;
            for (int plcIndex = 0; plcIndex < plcCount; plcIndex++)
            {
                for (int i = 0; i < groups[plcIndex]; i++)
                {
                    for (int j = 0; j < meters[meterIndex]; j++)
                    {
                        h.Add(string.Format("{0}DB{1},W{2},3", 3 + (i * 2), plcConnection[plcIndex], j * 10 + 0));
                        d.Add(string.Format("{0}DB{1},REAL{2}", 3 + (i * 2), plcConnection[plcIndex], j * 10 + 6));
                    }
                    if (meters[meterIndex] > 0)
                    {
                        MetersCtrl = new Controller("MetersCtrl",
                                     new string[] { string.Format(plcConnection[plcIndex] + "DB2,W100,5") },
                                     new string[] { string.Format(plcConnection[plcIndex] + "DB2,W120,5") });
                        Program.MeterManager.CTMeters.Add(plcIndex * 10000 + 99, MetersCtrl);
                        MetersCtrl.Connect("127.0.0.1");
                    }
                    meterIndex++;
                }
            }

            int itemIndex = 0;
            for (int plcIndex = 0; plcIndex < plcCount; plcIndex++)
            {
                for (int i = 0; i < groups[plcIndex]; i++)
                {
                    for (int j = 0; j < dataItems[itemIndex]; j++)
                    {
                        h.Add(string.Format("{0}DB{1},W{2},3", 4 + (i * 2), plcConnection[plcIndex], j * 20));
                        d.Add(string.Format("{0}DB{1},REAL{2}", 4 + (i * 2), plcConnection[plcIndex], j * 20 + 10));
                        h_flow.Add(string.Format("{0}DB{1},W{2},3", 4 + (i * 2), plcConnection[plcIndex], j * 20));
                        d_flow.Add(string.Format("{0}DB{1},W{2}", 4 + (i * 2), plcConnection[plcIndex], j * 20 + 18));
                    }
                    itemIndex++;
                }
            }

            Program.M = new DataItemManager("MachineData", h.ToArray(), d.ToArray());
            Program.M.Connect("127.0.0.1");
            Program.M_flowAlert = new FlowAlertManager("FlowAlert", h_flow.ToArray(), d_flow.ToArray());
            Program.M_flowAlert.Connect("127.0.0.1");
        }


        void RemoteCtrl_WorkStateChanged(JonLibrary.Automatic.RunningState state)
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

        CycleTask Start;
        private void btnStart_Click(object sender, EventArgs e)
        {
            this.btnStart.Enabled = false;
            Start = new CycleTask();
            Start.Run(start, ThreadPriority.Lowest);
        }

        private void btnPC_Click(object sender, EventArgs e)
        {
            UpdateCycle.SetPause = !UpdateCycle.SetPause;
            RemoteCtrl.SetPause = !RemoteCtrl.SetPause;

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
                if (UpdateCycle != null) UpdateCycle.SetExit = true;
                if (RemoteCtrl != null) RemoteCtrl.SetExit = true;
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
                (frmDataDisplay = new DataDisplayForm()).Show();
        }

    }

}
