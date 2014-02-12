using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using JonLibrary.Automatic;
using JonLibrary.Common;
using System.Threading;
using MicroDAQ.Gateway;
using MicroDAQ.Specifical;
using log4net;


namespace MicroDAQ.UI
{
    public partial class MainForm : Form
    {
        IniFile ini = null;
        ILog log;

        Loader Loader = null;
        public MainForm()
        {
            InitializeComponent();
            ini = new IniFile(AppDomain.CurrentDomain.BaseDirectory + "MicroDAQ.ini");
            log = LogManager.GetLogger(this.GetType());
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            ni.Icon = this.Icon;
            ni.Text = this.Text;

            bool autoStart = false;
            try
            {
                this.Text = ini.GetValue("General", "Title");
                this.tsslProject.Text = "项目代码：" + ini.GetValue("General", "ProjetCode");
                this.tsslVersion.Text = "程序版本：" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
                autoStart = bool.Parse(ini.GetValue("AutoRun", "AutoStart"));
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                if (autoStart)
                    this.BeginInvoke(new MethodInvoker(
                                        delegate
                                        { btnStart_Click(null, null); })
                                    );
            }
            ni.Text = this.Text;
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
                        foreach (PLCStationInformation plc in Loader.Configurator.PlcsInfo)
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

            System.Threading.Thread.Sleep(Program.waitMillionSecond);
            Loader = new Loader();
            bool success = this.Loader.Start();
            if (success)
            {
                Program.opcGateway.StateChanged += new EventHandler(opcGateway_StateChanged);
                Program.opcGateway.UpdateCycle.WorkStateChanged += new CycleTask.WorkStateChangeEventHandle(UpdateCycle_WorkStateChanged);
                Program.opcGateway.RemoteCtrlCycle.WorkStateChanged += new CycleTask.WorkStateChangeEventHandle(RemoteCtrlCycle_WorkStateChanged);
            }
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
