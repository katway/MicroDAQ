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
using MicroDAQ.DataItem;
using MicroDAQ.Database;
using MicroDAQ.Gateway;


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

        private string wordItemFormat;
        private string wordArrayItemFormat;
        private string realItemFormat;

        private List<PLCStationInformation> Plcs;
        private SysncOpcOperate.OPCServer SyncOpc;
        IniFile ini = null;

        bool boolReadConfig = false;
        bool boolCreateItems = false;
        bool boolCreateItemManger = false;

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

                opcServerType = ini.GetValue("OpcServer", "Type").Trim();
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

        /// <summary>
        /// 读取配置
        /// </summary>
        private void ReadConfig()
        {
            try
            {
                SyncOpc.Connect(ini.GetValue(opcServerType, "ProgramID"), "127.0.0.1");
                //读取Item地址格式           
                string[] getPairsConfigItems = new string[Plcs.Count];
                wordItemFormat = ini.GetValue(opcServerType, "WordItemFormat");
                wordArrayItemFormat = ini.GetValue(opcServerType, "WordItemFormat");
                realItemFormat = ini.GetValue(opcServerType, "RealItemFormat");

                if (SyncOpc.AddGroup("Groups"))
                {
                    #region 是否多组,有几组
                    //生成读取是否多组,有几组的Item地址
                    for (int i = 0; i < Plcs.Count; i++)
                    {
                        getPairsConfigItems[i] = string.Format(wordArrayItemFormat, 1, 26, 2);
                    }
                    //获取是否多组的数据,并转存到PlcStation列表里
                    object[] values = new object[Plcs.Count];
                    if (SyncOpc.AddItems("Groups", getPairsConfigItems))
                    {
                        SyncOpc.SyncRead("Groups", values);
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
                        values = new object[Plcs[i].PairsNumber];

                        if (SyncOpc.AddItems("Groups", getItemsNumber[i]))
                        {
                            SyncOpc.SyncRead("Groups", values);
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

                    boolReadConfig = true;
                }
            }
            catch (Exception ex)
            {
                boolReadConfig = false;
            }
        }
        /// <summary>
        /// 创建Items项
        /// </summary>
        private void CreateItems()
        {
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
            catch (Exception ex)
            {

            }

        }
        /// <summary>
        /// 创建数据项管理器
        /// </summary>
        private void CreateItemsManger()
        {
            try
            {
                IList<IDataItemManage> listDataItemManger = new List<IDataItemManage>();
                IList<IDatabaseManage> listDatabaseManger = new List<IDatabaseManage>();
                foreach (var plc in Plcs)
                {
                    string[] head = new string[plc.ItemsHead.Count];
                    string[] data = new string[plc.ItemsHead.Count];
                    plc.ItemsHead.CopyTo(head, 0);
                    plc.ItemsData.CopyTo(data, 0);
                    IDataItemManage dim = new DataItemManager("ItemData", head, data);
                    DatabaseManage dbm = new DatabaseManage();
                    listDataItemManger.Add(dim);
                    listDatabaseManger.Add(dbm);
                }
                Program.opcGateway = new OpcGateway(listDataItemManger, listDatabaseManger);
                Program.opcGateway.Start();
                boolCreateItemManger = true;
            }
            catch (Exception ex)
            {
                boolCreateItemManger = false;
            }
        }

        public void Start()
        {
            if (!boolReadConfig)
            {
                //等OPCSERVER启动
                Thread.Sleep(Program.waitMillionSecond);
                ReadConfig();
            }

            if (boolReadConfig && !boolCreateItems)
            {
                CreateItems();
            }

            if (boolReadConfig && boolCreateItems && !boolCreateItemManger)
            {
                CreateItemsManger();

                StartWork.SetExit = true;
            }
            Thread.Sleep(200);
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

        CycleTask StartWork;
        private void btnStart_Click(object sender, EventArgs e)
        {
            this.btnStart.Enabled = false;
            StartWork = new CycleTask();
            StartWork.Run(Start, ThreadPriority.Lowest);
        }

        private void btnPC_Click(object sender, EventArgs e)
        {


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
