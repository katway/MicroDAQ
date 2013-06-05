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

namespace MicroDAQ
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// PLC数量
        /// </summary>
        int plcCount;
        /// <summary>
        /// 每个PLC中10字节数据项数量
        /// </summary>
        int[] meters;
        /// <summary>
        /// 每个PLC20字节数据项数量
        /// </summary>
        int[] dataItems;

        uint[] ctMeterID;
        /// <summary>
        /// OPCServer连接数组
        /// </summary>
        string[] plcConnection;
        /// <summary>
        /// 使用哪个OPCServer
        /// </summary>
        string opcServer = "SimaticNet";
        /// <summary>
        /// OPCServer名称
        /// </summary>
        string opcServerPorgramID = "OPC.SimaticNet";
        /// <summary>
        /// OPCServer地址连接方式
        /// </summary>
        string opcServerConnection = "S7:[S7 connection_{0}]";
        /// <summary>
        /// 是否读取了扩展组数
        /// </summary>
        bool readGroupCount = false;
        /// <summary>
        /// 扩展组数
        /// </summary>
        int[] groups;
        /// <summary>
        /// 监控数据项
        /// </summary>
        public ItemsAddress DataItem { get; set; }
        /// <summary>
        /// 粒子流量报警
        /// </summary>
        public ItemsAddress FlowAlert { get; set; }

        AsyncPLC4 PLC;

        public MainForm()
        {
            InitializeComponent();
            PLC = new AsyncPLC4();
            PLC.ReadComplete += new AsyncPLC4.dgtReadComplete(PLC_ReadComplete);
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            ni.Icon = this.Icon;
            ni.Text = this.Text;

            bool autoStart = false;
            try
            {
                IniFile ini = new IniFile(AppDomain.CurrentDomain.BaseDirectory + "MicroDAQ.ini");
                this.Text = ini.GetValue("General", "Title");
                this.tsslProject.Text = "项目代码：" + ini.GetValue("General", "ProjetCode");
                this.tsslVersion.Text = "接口版本：" + ini.GetValue("General", "VersionCode");
                autoStart = bool.Parse(ini.GetValue("AutoRun", "AutoStart"));

                plcCount = int.Parse(ini.GetValue("PLCConfig", "Amount"));

                plcConnection = new string[plcCount];
                meters = new int[plcCount];
                dataItems = new int[plcCount];

                opcServer = ini.GetValue("OPCServer", "Type").Trim();
                opcServerConnection = ini.GetValue(opcServer, "ConnectionString").Trim();
                opcServerPorgramID = ini.GetValue(opcServer, "PorgramID").Trim();
                for (int i = 0; i < plcCount; i++)
                {
                    plcConnection[i] = string.Format(opcServerConnection, i);
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

        void PLC_DataChange(string groupName, int[] item, object[] value, short[] Qualities)
        {
            bool r = true;
            foreach (short q in Qualities)
            {
                r &= (q >= 192) ? (true) : (false);
            }
            ConnectionState = (r) ? (ConnectionState.Open) : (ConnectionState.Closed);

            switch (groupName)
            {
                case "Cfg":
                    for (int i = 0; i < item.Length; i++)
                    {
                        if (value[i] == null) continue;
                        meters[item[i]] = (ushort)value[i];
                    }
                    break;
                case "Cfg-DataItem":
                    for (int i = 0; i < item.Length; i++)
                    {
                        if (value[i] == null) continue;
                        dataItems[item[i]] = (ushort)value[i];
                    }
                    getConfig = true;
                    break;
                case "GroupCount":
                    for (int i = 0; i < item.Length; i++)
                    {
                        if (value[i] == null) continue;
                        groups[item[i]] = (ushort)value[i];
                    }
                    readGroupCount = true;
                    break;
            }
        }

        void PLC_ReadComplete(string groupName, int[] item, object[] value, short[] Qualities)
        {
            this.PLC_DataChange(groupName, item, value, Qualities);
            switch (groupName)
            {
                case "Cfg":
                case "Cfg-DataItem":
                    this.BeginInvoke(new MethodInvoker(delegate
                    {
                        this.tsslMeters.Text = "采集点：";
                        foreach (int ms in meters)
                            this.tsslMeters.Text += ms.ToString() + " ";

                        foreach (int ds in dataItems)
                            this.tsslMeters.Text += ds.ToString() + " ";
                    }));
                    break;
                case "CtMeters":
                    ctMeterID = (uint[])value[0];
                    for (int i = 0; i < ctMeterID.Length; i++)
                    {
                        ctMeterID[i] = ctMeterID[i] >> 16;
                    }
                    break;
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

                CreateMeters();

                started = true;
                CycleTask UpdateCycle = new CycleTask();
                CycleTask RemoteCtrl = new CycleTask();
                Program.RemoteCycle = RemoteCtrl;
                UpdateCycle.WorkStateChanged += new CycleTask.dgtWorkStateChange(UpdateCycle_WorkStateChanged);
                RemoteCtrl.WorkStateChanged += new CycleTask.dgtWorkStateChange(RemoteCtrl_WorkStateChanged);
                UpdateCycle.Run(update2, System.Threading.ThreadPriority.BelowNormal);
                RemoteCtrl.Run(remoteCtrl, System.Threading.ThreadPriority.BelowNormal);
                Start.SetExit = true;
            }
            Thread.Sleep(200);
        }

        private void ReadConfig()
        {
            List<string> listItems = new List<string>();
            PLC.Connect(opcServerPorgramID, "127.0.0.1");

            # region 读取扩展组数
            for (int i = 0; i < plcCount; i++)
            {
                listItems.Add(plcConnection[i] + "DB1,W28");
            }
            PLC.AddGroup("GroupCount", 1, 0);
            PLC.AddItems("GroupCount", listItems.ToArray());
            PLC.Read("GroupCount");

            #endregion

            if (readGroupCount)
            {
                listItems = new List<string>();
                for (int i = 0; i < plcCount; i++)
                {
                    for (int j = 0; j < groups[i]; j++)
                    {
                        listItems.Add(plcConnection[i] + string.Format("DB{0},W{1}", 1, 30 + (j * 4)));
                    }
                }
                PLC.AddGroup("Cfg", 1, 0);
                PLC.AddItems("Cfg", listItems.ToArray());
                PLC.Read("Cfg");

                listItems = new List<string>();
                for (int i = 0; i < plcCount; i++)
                {
                    for (int j = 0; j < groups[i]; j++)
                    {
                        listItems.Add(plcConnection[i] + string.Format("DB{0},W{1}", 1, 32 + (j * 4)));
                    }
                }
                PLC.AddGroup("Cfg-DataItem", 1, 0);
                PLC.AddItems("Cfg-DataItem", listItems.ToArray());
                PLC.Read("Cfg-DataItem");

                readGroupCount = false;
            }
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
                        h.Add(string.Format("{0}DB{1},W{2},3",3+(i*2) ,plcConnection[plcIndex], j * 10 + 0));
                        d.Add(string.Format("{0}DB{1},REAL{2}",3+(i*2) ,plcConnection[plcIndex],j * 10 + 6));
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
                        h.Add(string.Format("{0}DB{1},W{2},3", 4+(i*2),plcConnection[plcIndex], j * 20));
                        d.Add(string.Format("{0}DB{1},REAL{2}", 4+(i*2),plcConnection[plcIndex], j * 20 + 10));
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
