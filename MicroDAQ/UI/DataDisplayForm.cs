using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using MicroDAQ.DataItem;
using MicroDAQ.Database;
using MicroDAQ.UI;
namespace MicroDAQ.Specifical
{
    public partial class DataDisplayForm : Form
    {
        public DataDisplayForm()
        {
            InitializeComponent();

        }

        SqlConnection connection = null;
        #region PLC与OPCMES即时数据的显示
        private void btnInstant_Click(object sender, EventArgs e)
        {
            //显示即时数据
            ShowItems();
        }
        /// <summary>
        /// PLC与OPCMES即时数据的显示
        /// </summary>     
        DataTable NewTable = null;
        public void ShowItems()
        {
            //PLC关闭的情况

            if (Program.opcGateway.ItemManagers == null)
            {
                try
                {
                    if (connection.State == ConnectionState.Closed)
                    {

                        this.labOPCState.BackColor = Color.Red;
                        this.labOPCState.ForeColor = Color.Yellow;
                        this.labOPCState.Text = "通信错误";

                        this.labDBState.BackColor = Color.Red;
                        this.labDBState.ForeColor = Color.Yellow;
                        this.labDBState.Text = "通信错误";
                        return;

                    }
                    else
                    {
                        this.labOPCState.BackColor = Color.Red;
                        this.labOPCState.ForeColor = Color.Yellow;
                        this.labOPCState.Text = "通信错误";

                        this.labDBState.BackColor = Color.Green;
                        this.labDBState.ForeColor = Color.White;
                        this.labDBState.Text = "通信正常";
                        return;

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }


            else
            {
                //plc打开成功，数据库连接成功的情况 
                if (connection.State == ConnectionState.Open)
                {
                    this.labDBState.BackColor = Color.Green;
                    this.labDBState.ForeColor = Color.White;
                    this.labDBState.Text = "通信正常";

                    this.labOPCState.BackColor = Color.Green;
                    this.labOPCState.ForeColor = Color.White;
                    this.labOPCState.Text = "通信正常";


                    string sql = @"SELECT v.id AS 参数ID,
                                     p.name AS 参数名称,
                                     v.value1 AS 采集值1,
                                     v.value2 AS 采集值2,
                                     p.unit AS 单位,
                                     v.time AS 刷新时间,
                                     v.zztime AS 存储点
                        FROM ProcessItem p 
                        LEFT JOIN meter_uuid m ON p.id = m.uuid 
                        LEFT JOIN meter_type t ON p.protocolType = t.protocol 
                        RIGHT JOIN meters_value v ON m.id = v.id 
                        ORDER BY v.id ";
                    SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    NewTable = new DataTable();
                    NewTable.Columns.AddRange(new DataColumn[]{
                        new DataColumn("参数ID"),
                        new DataColumn("参数名称"),
                        new DataColumn("数据采集值1"),                      
                        new DataColumn("数据采集值2"),
                        new DataColumn("单位"),
                        new DataColumn("刷新时间"),
                        new DataColumn("存储点"), 
                        new DataColumn("PLC数据值1"),
                        new DataColumn("PLC设备类型"),
                        new DataColumn("PLC状态"),
                        new DataColumn("PLC可信度") });
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow tmp = dt.Rows[i];
                        foreach (IDataItemManage mgr in Program.opcGateway.ItemManagers)
                        {
                            foreach (Item meter in mgr.Items)
                            {
                                if (tmp[0].ToString() == meter.ID.ToString())
                                {
                                    DataRow row = NewTable.NewRow();
                                    row["参数ID"] = tmp[0].ToString();
                                    row["参数名称"] = tmp[1].ToString();

                                    row["数据采集值1"] = tmp[2].ToString();
                                    row["数据采集值2"] = tmp[3].ToString();

                                    row["单位"] = tmp[4].ToString();
                                    row["刷新时间"] = tmp[5].ToString();
                                    row["存储点"] = tmp[6].ToString();

                                    row["PLC数据值1"] = meter.Value.ToString();
                                    row["PLC设备类型"] = meter.Type.ToString();
                                    row["PLC状态"] = meter.State.ToString();
                                    row["PLC可信度"] = meter.Quality.ToString();
                                    NewTable.Rows.Add(row);
                                }
                            }

                        }

                    }

                    this.dgvDB.DataSource = NewTable;
                    //dgvDB.Columns["刷新时间"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss.fff";
                    //dgvDB.Columns["存储点"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss.fff";

                    //比较数据是否相等，如果不相等数据背景色改变
                    for (int j = 0; j < dgvDB.Rows.Count; j++)
                    {
                        string a = this.dgvDB.Rows[j].Cells[2].Value.ToString();
                        string b = this.dgvDB.Rows[j].Cells[7].Value.ToString();
                        if (a != b)
                        {
                            this.dgvDB.Rows[j].Cells[2].Style.BackColor = Color.Red;
                            this.dgvDB.Rows[j].Cells[7].Style.BackColor = Color.Red;
                        }

                    }
                }

                else
                {//plc打开成功，数据库连接失败的情况

                    this.labOPCState.BackColor = Color.Green;
                    this.labOPCState.ForeColor = Color.White;
                    this.labOPCState.Text = "通信正常";

                    this.labDBState.BackColor = Color.Red;
                    this.labDBState.ForeColor = Color.Yellow;
                    this.labDBState.Text = "通信错误";

                    DataTable table = new DataTable();
                    table.Columns.AddRange(new DataColumn[]{
                            new DataColumn("PLC编号"),
                            new DataColumn("PLC数据值1"),
                            new DataColumn("PLC设备类型"),
                            new DataColumn("PLC状态"),
                            new DataColumn("PLC可信度")});
                    if (Program.opcGateway.ItemManagers == null)
                    {
                        MessageBox.Show("尚未加载plc数据！");
                        return;
                    }
                    else
                    {
                        foreach (IDataItemManage mgr in Program.opcGateway.ItemManagers)
                        {
                            foreach (Item item in mgr.Items)
                            {
                                DataRow row = table.NewRow();
                                Item meter = item;
                                row["plc编号"] = meter.ID.ToString();
                                row["plc数据值1"] = meter.Value.ToString();
                                row["plc设备类型"] = meter.Type.ToString();
                                row["plc状态"] = meter.State.ToString();
                                row["plc可信度"] = meter.Quality.ToString();
                                table.Rows.Add(row);
                            }
                        }
                        this.dgvDB.DataSource = table;
                    }
                }
            }
        }
        #endregion

        #region 加载form窗体
        /// <summary>
        /// 加载form窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>        
        private void FormDemo_Load(object sender, EventArgs e)
        {
            if (Program.opcGateway != null && Program.opcGateway.DatabaseManagers != null)
            {
                //循环遍历数据库
                List<SqlConnection> sqlcon = new List<SqlConnection>();
                foreach (IDatabaseManage a in Program.opcGateway.DatabaseManagers)
                {
                    SqlConnection conn = new SqlConnection(a.UpdateConnection.ConnectionString);
                    sqlcon.Add(conn);
                }
                for (int i = 0; i < sqlcon.Count; i++)
                {
                    connection = sqlcon[0];

                }

                bkwConnect.DoWork += new DoWorkEventHandler(bkwConnect_DoWork);
                bkwConnect.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bkwConnect_RunWorkerCompleted);
                bkwConnect.RunWorkerAsync();
            }
        }
        //打开数据库连接
        void bkwConnect_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                connection.Open();
            }
            catch
            { }
        }

        void bkwConnect_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            ShowItems();
            ShowDB();
            GetOder();

        }
        #endregion
        #region 指令查询
        /// <summary>
        /// 指令查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>          
        private void button1_Click(object sender, EventArgs e)
        {
            GetOder();
        }
        /// <summary>
        /// 指令查询
        /// </summary>
        public void GetOder()
        {
            if (connection.State == ConnectionState.Open)
            {
                this.labDBState2.BackColor = Color.Green;
                this.labDBState2.ForeColor = Color.White;
                this.labDBState2.Text = "通信正常";
                string selectRemoteControl = "select id as 编号,cycle,command as 命令,cmdstate as 命令状态 from v_remoteControl";
                SqlDataAdapter adapter = new SqlDataAdapter(selectRemoteControl, connection);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                this.dataGridView3.DataSource = dt;
            }
            else
            {
                this.labDBState2.BackColor = Color.Red;
                this.labDBState2.ForeColor = Color.Yellow;
                this.labDBState2.Text = "通信错误";
            }

        }
        #endregion
        #region 测试报警灯
        /// <summary>
        /// 测试报警灯
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private int initSlave = 201;
        private int alarmIndex = 0;
        private List<AlarmControl> alarms = new List<AlarmControl>();

        private AlarmControl AddAlarm(int slave, byte alertCode)
        {
            AlarmControl alarm = new AlarmControl(slave, alertCode);
            alarms.Add(alarm);
            this.tabPage2.Controls.Add(alarm);
            return alarm;
        }
        private void RemoveAlarm()
        {
            AlarmControl alarm = alarms[alarms.Count - 1];
            this.tabPage2.Controls.Remove(alarm);
            alarms.RemoveAt(alarms.Count - 1);
            alarm.Dispose();
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            AlarmControl alarm = AddAlarm(alarmIndex++ + initSlave, 0);
            alarm.Location = new Point(30, 60 + 29 * alarmIndex);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            RemoveAlarm();
            alarmIndex--;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            int slave;
            int.TryParse(this.textBox1.Text, out slave);
            this.initSlave = slave;
        }
        int runningNum = 0;
        int ctrlIndex = 0;
        private void tmrRemoteCtrl_Tick(object sender, EventArgs e)
        {
            if (this.alarms.Count > 0)
            {
                if (ctrlIndex > 0)
                    this.alarms[(ctrlIndex - 1) % this.alarms.Count].Highlight = false;

                AlarmControl alarm = this.alarms[ctrlIndex % this.alarms.Count];
                alarm.Highlight = true;

                foreach (var mt in Program.MeterManager.CTMeters.Values)
                {
                    runningNum = ++runningNum % ushort.MaxValue;
                    mt.SetCommand(runningNum, alarm.Slave, 1, (int)alarm.AlertCode);
                }
                Console.WriteLine(ctrlIndex);
                ctrlIndex = ++ctrlIndex % short.MaxValue;
            }
        }
        #endregion
        private void FormDemo_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #region 配置情况
        /// <summary>
        /// 配置情况
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        DataTable dt;
        private void ShowDB()
        {
            try
            {
                if (connection.State == ConnectionState.Open)
                {
                    this.labDBState1.BackColor = Color.Green;
                    this.labDBState1.ForeColor = Color.White;
                    this.labDBState1.Text = "通信正常";
                    string sql = @"SELECT  m.id AS 参数ID,
                                    p.name AS 参数名称,
                                    t.name AS 参数类型,
                                    p.unit AS 单位,
                                    p.updateRate AS 存储频率,
                                    p.minimum AS 下限,
                                    p.maximum AS 上限,
                                    p.yellowMin AS 低警告,
                                    p.yellowMax AS 高警告  
                        FROM ProcessItem p 
                        LEFT JOIN meter_uuid m ON p.id = m.uuid 
                        LEFT JOIN meter_type t ON p.protocolType = t.protocol 
                        LEFT JOIN meters_value v ON m.id = v.id 
                        ORDER BY m.id ";
                    SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                    dt = new DataTable();
                    adapter.Fill(dt);
                    this.dataGridView2.DataSource = dt;
                }

                else
                {
                    this.labDBState1.BackColor = Color.Red;
                    this.labDBState1.ForeColor = Color.Yellow;
                    this.labDBState1.Text = "通信错误";

                }


            }
            catch
            { };

        }
        private void btnRefreshDB_Click(object sender, EventArgs e)
        {
            ShowDB();
        }
        #endregion

        private void button2_Click(object sender, EventArgs e)
        {
            ShowItems();
        }
        //清除meter-value里面的数据：
        private void btnDelete_Click(object sender, EventArgs e)
        {
            string deleteData = "delete from meters_value";
            if (connection.State == ConnectionState.Open)
            {
                SqlDataAdapter adapter = new SqlDataAdapter(deleteData, connection);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                MessageBox.Show("数据清除成功！");
            }
            else
            {
                this.btnDelete.Enabled = false;
                MessageBox.Show("数据库连接不成功，没有数据可清除！");
            }
        }



        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (this.tabControl1.SelectedIndex == 1)
            {
                this.tmrRemoteCtrl.Enabled = true;
                if (Program.opcGateway != null)
                    Program.opcGateway.Pause(Program.opcGateway.RemoteCtrlCycle);
            }
            else
            {
                this.tmrRemoteCtrl.Enabled = false;
                if (Program.opcGateway != null)
                    Program.opcGateway.Continue(Program.opcGateway.RemoteCtrlCycle);
            }
        }
    }
}
