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
using MicroDAQ.Common;
using MicroDAQ.Gateways.Modbus;
using MicroDAQ.DBUtility;
using MicroDAQ.Gateways.Modbus2;
using MicroDAQ.Configuration;

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
        List<ModbusMasterInfo> masterList;
        List<ModbusSlaveInfo> slaveList;
        List<ModbusVariableInfo> Variablelist;
        List<ModbusMasterAgent> agentlist;
        public MainForm()
        {
            log = LogManager.GetLogger(this.GetType());
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
                    string[] data = new string[plc.ItemsData.Count];
                    plc.ItemsHead.CopyTo(head, 0);
                    plc.ItemsData.CopyTo(data, 0);
                    IDataItemManage dim = new OpcDataItemManager("ItemData", head, data);
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


        private IList<IDatabase> createDBManagers()
        {
            bool success = false;
            IList<IDatabase> listDatabaseManger = new List<IDatabase>();
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


            Program.MobusGateway = new ModbusGateway(createDBManagers());
            Program.MobusGateway.Start();

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

        private void btnStart_Click(object sender, EventArgs e)
        {
            this.btnStart.Enabled = false;
            this.Start();
        }

        private void btnPC_Click(object sender, EventArgs e)
        {
            Program.opcGateway.Pause();
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
                //if (UpdateCycle != null) UpdateCycle.SetExit = true;
                //if (RemoteCtrl != null) RemoteCtrl.SetExit = true;
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
        private void ModbusMasterTable()
        {

            string strSql = "select * from ModbusMaster";
            SQLiteHelper ite = new SQLiteHelper("Project");
            DataTable dt = ite.ExecuteQuery(strSql);
            masterList = new List<ModbusMasterInfo>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ModbusMasterInfo masterInfo = new ModbusMasterInfo();
                masterInfo.allias = dt.Rows[i]["allias"].ToString();
                masterInfo.name = dt.Rows[i]["name"].ToString();
                masterInfo.type = dt.Rows[i][""].ToString();
                masterInfo.enable = dt.Rows[i][""].ToString();
                masterInfo.port = dt.Rows[i][""].ToString();
                masterList.Add(masterInfo);
            }





        }
        private void ModbusSlaveTable()
        {

            string strSql = "select * from ModbusSlave";
            SQLiteHelper ite = new SQLiteHelper("Project");
            DataTable dt = ite.ExecuteQuery(strSql);
            slaveList = new List<ModbusSlaveInfo>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ModbusSlaveInfo slaveInfo = new ModbusSlaveInfo();
                slaveInfo.serialID = Convert.ToInt64(dt.Rows[i][""]);
                slaveInfo.name = dt.Rows[i][""].ToString();
                slaveInfo.serialID = Convert.ToInt64(dt.Rows[i][""]);
                slaveInfo.type = dt.Rows[i][""].ToString();
                slaveInfo.slave = Convert.ToByte(dt.Rows[i][""]);
                slaveInfo.enable = dt.Rows[i][""].ToString();
                slaveList.Add(slaveInfo);
            }
        }
        private DataTable ModbusVariableTable()
        {

            string strSql = "select * from ModbusRegister";
            SQLiteHelper ite = new SQLiteHelper("Project");
            DataTable dt = ite.ExecuteQuery(strSql);
            Variablelist = new List<ModbusVariableInfo>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ModbusVariableInfo variableInfo = new ModbusVariableInfo();
                //字段赋值

                Variablelist.Add(variableInfo);
            }
            return dt;

        }
        private void GetModbusMasterAgent()
        {
            //从站与变量关系
            foreach (var slave in slaveList)
            {
                List<ModbusVariableInfo> listVariable = new List<ModbusVariableInfo>();
                foreach (var variableInfo in Variablelist)
                {
                    if (slave.serialID == variableInfo.serialID)
                    {
                        listVariable.Add(variableInfo);
                        Variablelist.Remove(variableInfo);
                    }

                }
                slave.modbusVariables = listVariable;
            }
            //主从站关系
            foreach (var master in masterList)
            {
                List<ModbusSlaveInfo> list = new List<ModbusSlaveInfo>();
                foreach (var slave in slaveList)
                {
                    if (master.serialID == slave.serialID)
                    {
                        list.Add(slave);
                        slaveList.Remove(slave);
                    }//字段不对
                }
                master.modbusSlaves = list;
            }
            for (int i = 0; i < masterList.Count; i++)
            {
                ModbusMasterAgent agent = new ModbusMasterAgent(masterList[i]);
                agentlist.Add(agent);
            }

        }



    }

}
