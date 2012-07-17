using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using JonLibrary.OPC;
using JonLibrary.Automatic;
using JonLibrary.Common;
using System.Threading;

namespace MicroDAQ
{
    public partial class MainForm : Form
    {
        System.Data.ConnectionState ConnectionState;
        int plcCount;
        int[] meters;
        int[] dataItems;
        byte[] projectCode = new byte[4];
        byte[] version = new byte[2];
        int plcTick;
        int ctMeter;
        uint[] ctMeterID;
        string[] plcConnection;//= string.Empty;
        public MainForm()
        {
            InitializeComponent();
            PLC = new AsyncPLC4();
            //PLC.DataChange += new AsyncPLC4.dgtDataChange(PLC_DataChange);
            PLC.ReadComplete += new AsyncPLC4.dgtReadComplete(PLC_ReadComplete);

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            ni.Icon = this.Icon;
            ni.Text = this.Text;

            bool autoStart = false; ;
            try
            {
                IniFile ini = new IniFile(AppDomain.CurrentDomain.BaseDirectory + "MicroDAQ.ini");
                this.Text = ini.GetValue("General", "Title");
                this.tsslProject.Text = "项目代码：" + ini.GetValue("General", "ProjetCode");
                this.tsslVersion.Text = "接口版本：" + ini.GetValue("General", "VersionCode");
                autoStart = bool.Parse(ini.GetValue("AutoRun", "AutoStart"));
                Duty = ini.GetValue("General", "Duty");
                plcCount = int.Parse(ini.GetValue("PLCConfig", "Amount"));
                plcConnection = new string[plcCount];
                meters = new int[plcCount];
                dataItems = new int[plcCount];
                for (int i = 0; i < plcCount; i++)
                    plcConnection[i] = ini.GetValue(string.Format("PLC{0}", i),
                                                    "Connection");

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
                case "CtMeters":
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
                    this.BeginInvoke((Action)delegate
                                    {
                                        //tsslProject.Text = string.Format("项目代码：{0}{1}{2}{3}", (char)projectCode[0], (char)projectCode[1], (char)projectCode[2], (char)projectCode[3]);
                                        //this.tsslVersion.Text = string.Format("接口版本：{0}.{1}", version[0], version[1]);
                                        this.tsslMeters.Text = "采集点：";
                                        //switch (Duty)
                                        //{
                                        //    case "E":
                                        foreach (int ms in meters)
                                            this.tsslMeters.Text += ms.ToString() + " ";
                                        //    break;
                                        //case "M":
                                        foreach (int ds in dataItems)
                                            this.tsslMeters.Text += ds.ToString() + " ";
                                        //        break;
                                        //}
                                    });
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
        AsyncPLC4 PLC;

        string Duty = string.Empty;
        private void ReadConfig()
        {
            PLC.Connect("OPC.SimaticNET", "127.0.0.1");
            //配置

            string[] items = null;
            //switch (Duty)
            //{
            //    case "E":
            items = new string[plcCount];
            for (int i = 0; i < plcCount; i++)
            {
                items[i] = plcConnection[i] + "DB1,W22";
            }
            PLC.AddGroup("Cfg", 1, 0);
            PLC.AddItems("Cfg", items);
            PLC.Read("Cfg");
            //    break;
            //case "M":
            items = new string[plcCount];
            for (int i = 0; i < plcCount; i++)
            {
                items[i] = plcConnection[i] + "DB1,W42";
            }
            PLC.AddGroup("Cfg-DataItem", 1, 0);
            PLC.AddItems("Cfg-DataItem", items);

            PLC.Read("Cfg-DataItem");
            //        break;
            //}
            //meters[0] = 43; meters[1] = 45; meters[2] = 21; meters[3] = 0;
            //dataItems[0] = 138; dataItems[1] = 164; dataItems[2] = 0; dataItems[3] = 30;
            //getConfig = true;
        }
        Meter meter;
        Controller MetersCtrl;
        MachineData M;
        private void CreateMeters()
        {
            int count = dataItems.Sum();
            List<string> h = new List<string>();
            List<string> d = new List<string>();

            for (int plcIndex = 0; plcIndex < plcCount; plcIndex++)
            {

                for (int meterIndex = 0; meterIndex < meters[plcIndex]; meterIndex++)
                {
                    h.Add(string.Format(plcConnection[plcIndex] + "DB3,W{0},3", meterIndex * 20 + 0));
                    d.Add(string.Format(plcConnection[plcIndex] + "DB3,REAL{0}", meterIndex * 20 + 10));
                }
                if (meters[plcIndex] > 0)
                {
                    MetersCtrl = new Controller("MetersCtrl",
                                  new string[] { string.Format(plcConnection[plcIndex] + "DB4,W0,5") },
                                    new string[] { string.Format(plcConnection[plcIndex] + "DB5,W0,5") });
                    Program.MeterManager.CTMeters.Add(plcIndex * 10000 + 99, MetersCtrl);
                    MetersCtrl.Connect("127.0.0.1");
                }
            }

            for (int plcIndex = 0; plcIndex < plcCount; plcIndex++)
            {
                for (int itemIndex = 0; itemIndex < dataItems[plcIndex]; itemIndex++)
                {
                    h.Add(string.Format(plcConnection[plcIndex] + "DB6,W{0},3", itemIndex * 10));
                    d.Add(string.Format(plcConnection[plcIndex] + "DB6,REAL{0}", itemIndex * 10 + 6));
                }
            }

            M = new MachineData("MachineData", h.ToArray(), d.ToArray());
            M.Connect("127.0.0.1");



        }


        int updateMeters;
        int remoteMeters;
        private void update()
        {
            try
            {
                updateMeters = 0;
                foreach (var meter in Program.MeterManager.Meters.Values)
                {
                    if (meter.GetType() == typeof(Meter) || meter.GetType() == typeof(MachineDataItem))
                    {
                        Meter mt = meter as Meter;
                        mt.SyncTick = plcTick;
                        //if (mt.State != MeterState.通讯中断)
#warning 临时应对OPCMES和PLC仪表组结方式不一致
                        if (mt.Type == DataType.尘埃粒子)
                        {
                            mt.CalcPaticleCount();
                            Program.DatabaseManager.UpdateMeterValue(mt.ID, (int)mt.Type, (int)mt.State, mt.Value1, 0, 0);
                            Program.DatabaseManager.UpdateMeterValue(mt.ID + 10000, (int)mt.Type, (int)mt.State, mt.Value2, 0, 0);
                            Program.DatabaseManager.UpdateMeterValue(mt.ID + 11000, (int)mt.Type, (int)mt.State, mt.ParticleCount.Value1, 0, 0);
                            Program.DatabaseManager.UpdateMeterValue(mt.ID + 12000, (int)mt.Type, (int)mt.State, mt.ParticleCount.Value2, 0, 0);
                            Program.DatabaseManager.UpdateMeterValue(mt.ID + 13000, (int)mt.Type + 1, (int)mt.State, (mt.Warning) ? (2) : (1), 0, 0);
                        }
                        else
                        {
                            Program.DatabaseManager.UpdateMeterValue(mt.ID, (int)mt.Type, (int)mt.State, mt.Value1, mt.Value2, (float)(int)mt.ConnectionState);
                        }
                    }
                }
                System.Threading.Thread.Sleep(900);
            }
            catch (Exception ex)
            {
                System.Threading.Thread.Sleep(3000);
            }

        }

        private void update2()
        {
            foreach (var item in M.Items)
            {
                Program.DatabaseManager.UpdateMeterValue(item.ID, (int)item.Type, (int)item.State, (float)item.Value, 0.0f, 0.0f);
            }
        }
        int running;
        public void remoteCtrl()
        {
            try
            {
                DataRow[] Rows = Program.DatabaseManager.GetRemoteControl();
                if (Rows != null)
                    foreach (var row in Rows)
                    {
                        //MessageBox.Show((row["cycle"].ToString() != null).ToString());
                        foreach (var mt in Program.MeterManager.CTMeters.Values)
                            mt.SetCommand(++running,
                                                      int.Parse(row["id"].ToString()),
                                                      int.Parse(row["command"].ToString()),
                                                      int.Parse((row["cycle"] != null) ? (row["cycle"].ToString()) : ("0"))
                                                  );
                        Thread.Sleep(400);
                    }
                System.Threading.Thread.Sleep(200);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                System.Threading.Thread.Sleep(3000);
            }
        }

        bool readConfig = false;
        bool getConfig = false;
        bool createMeters = false;
        bool metersCreated = false;
        bool started = false;
        public void start()
        {
            if (!readConfig)
            {
                Thread.Sleep(180000);
                ReadConfig();
                readConfig = true;
            }

            if (getConfig && !started)
            {

                CreateMeters();

                started = true;
                UpdateCycle = new CycleTask();
                RemoteCtrl = new CycleTask();

                UpdateCycle.WorkStateChanged += new CycleTask.dgtWorkStateChange(UpdateCycle_WorkStateChanged);
                RemoteCtrl.WorkStateChanged += new CycleTask.dgtWorkStateChange(RemoteCtrl_WorkStateChanged);
                UpdateCycle.Run(update, System.Threading.ThreadPriority.BelowNormal);
                RemoteCtrl.Run(remoteCtrl, System.Threading.ThreadPriority.BelowNormal);
                Start.SetExit = true;
            }
            Thread.Sleep(200);
        }

        void RemoteCtrl_WorkStateChanged(JonLibrary.Automatic.RunningState state)
        {
            this.BeginInvoke((Action)delegate
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
            });
        }

        void UpdateCycle_WorkStateChanged(JonLibrary.Automatic.RunningState state)
        {
            this.BeginInvoke((Action)delegate
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

                             });
        }



        CycleTask UpdateCycle;
        CycleTask RemoteCtrl;
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

    }

}
