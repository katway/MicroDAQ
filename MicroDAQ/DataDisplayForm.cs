using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace MicroDAQ
{
    public partial class DataDisplayForm : Form
    {
        public DataDisplayForm()
        {
            InitializeComponent();
        }
        SqlConnection connection = new SqlConnection(Program.DatabaseManager.ConnectionString);
        DataTable dt;
        private void DataDisplayForm_Load(object sender, EventArgs e)
        {
            bkwConnect.DoWork += new DoWorkEventHandler(bkwConnect_DoWork);
            bkwConnect.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bkwConnect_RunWorkerCompleted);
            bkwConnect.RunWorkerAsync();
        }

        void bkwConnect_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ShowItems();
            ShowDB();
        }

        void bkwConnect_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                connection.Open();
            }
            catch
            { }
        }
        private void ShowItems()
        {
            this.btnRefreshData.Enabled = false;
            try
            {
                int i = 0;
                if (Program.M == null)
                {
                    MessageBox.Show("尚未加载PLC数据，请点击‘运行’并稍后重试。");
                    this.btnRefreshData.Enabled = true;
                    return;
                }

                foreach (var item in Program.M.Items)
                {
                    DataItem meter = item as DataItem;
                    i++;
                    if (lsvItems.Items.Count < i)
                        lsvItems.Items.Add(new ListViewItem(new string[]{
                                            meter.ID.ToString()                                            ,
                                            meter.Value .ToString ()                                            ,
                                            string.Empty,
                                            meter.Type.ToString (),
                                            meter.State .ToString (),
                                            meter.Quality.ToString ()}));
                    else
                    {
                        lsvItems.Items[i - 1].SubItems[0].Text = meter.ID.ToString();
                        lsvItems.Items[i - 1].SubItems[1].Text = meter.Value.ToString();
                        lsvItems.Items[i - 1].SubItems[3].Text = meter.Type.ToString();
                        lsvItems.Items[i - 1].SubItems[4].Text = meter.State.ToString();
                        lsvItems.Items[i - 1].SubItems[5].Text = meter.Quality.ToString();
                    }

                    if (meter.State != DataState.正常 && meter.State != DataState.已启动)
                        lsvItems.Items[i - 1].BackColor = Color.Gold;
                    //Console.WriteLine((item as Meter).ID);
                }

                if (Program.M_flowAlert.Items != null)
                    foreach (var item in Program.M_flowAlert.Items)
                    {
                        DataItem meter = item as DataItem;
                        i++;
                        if (lsvItems.Items.Count < i)
                            lsvItems.Items.Add(new ListViewItem(new string[]{
                                            meter.ID.ToString()  ,
                                            meter.Value .ToString () ,
                                            string.Empty,
                                            "流量",
                                            meter.State .ToString (),
                                            meter.Quality.ToString ()}));
                        else
                        {
                            lsvItems.Items[i - 1].SubItems[0].Text = meter.ID.ToString();
                            lsvItems.Items[i - 1].SubItems[1].Text = meter.Value.ToString();
                            lsvItems.Items[i - 1].SubItems[3].Text = "流量";
                            lsvItems.Items[i - 1].SubItems[4].Text = meter.State.ToString();
                            lsvItems.Items[i - 1].SubItems[5].Text = meter.Quality.ToString();
                        }
                    }


                switch (Program.M.ConnectionState)
                {
                    case ConnectionState.Closed:
                        this.labOPCState.BackColor = Color.Red;
                        this.labOPCState.ForeColor = Color.Yellow;
                        this.labOPCState.Text = "通信错误";
                        break;
                    case ConnectionState.Open:
                        this.labOPCState.BackColor = Color.Green;
                        this.labOPCState.ForeColor = Color.White;
                        this.labOPCState.Text = "通信正常";
                        break;

                }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.ToString()); }
            this.btnRefreshData.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ShowItems();
        }

        private void ShowDB()
        {
            this.btnRefreshDB.Enabled = false;
            try
            {
                string sql = @"SELECT   m.id AS 参数ID,
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
                this.dgvDB.DataSource = dt;

                switch (connection.State)
                {
                    case ConnectionState.Closed:
                        this.labDBState.BackColor = Color.Red;
                        this.labDBState.ForeColor = Color.Yellow;
                        this.labDBState.Text = "通信错误";
                        break;
                    case ConnectionState.Open:
                        this.labDBState.BackColor = Color.Green;
                        this.labDBState.ForeColor = Color.White;
                        this.labDBState.Text = "通信正常";
                        break;

                }

            }
            catch
            { };

            this.btnRefreshDB.Enabled = true;
        }

        private void btnRefreshDB_Click(object sender, EventArgs e)
        {
            ShowDB();
        }

        private void DataDisplayForm_SizeChanged(object sender, EventArgs e)
        {
            //this.btnRefreshData.Location = new Point(this.Width - 150, 50);
            //this.btnRefreshDB.Location = new Point(this.btnRefreshData.Location.X, this.Height / 2 + 50);
            //this.labOPCState.Location = new Point(this.btnRefreshData.Location.X, this.btnRefreshData.Location.Y + 80);
            //this.labDBState.Location = new Point(this.btnRefreshDB.Location.X, this.btnRefreshDB.Location.Y + 80);
            //this.label1.Location = new Point(this.labOPCState.Location.X, this.labOPCState.Location.Y - 20);
            //this.label3.Location = new Point(this.labDBState.Location.X, this.labDBState.Location.Y - 20);


            this.lsvItems.Size = new Size(this.Width - 200, this.Height / 2 - 40);
            this.dgvDB.Size = new Size(this.Width - 200, this.Height / 2 - 40);
            this.lsvItems.Location = new Point(10, 10);
            this.dgvDB.Location = new Point(10, this.Height / 2 - 10);

            this.grpItem.Location = new Point(this.Width - 180, this.lsvItems.Location.Y);
            this.grpDB.Location = new Point(this.Width - 180, this.dgvDB.Location.Y);
            this.grpItem.Size = new Size(this.grpItem.Width, this.lsvItems.Height);
            this.grpDB.Size = new Size(this.grpDB.Width, this.dgvDB.Height);

        }

        private void DataDisplayForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                connection.Close();
            }
            catch
            { }
        }

        private void btnInstant_Click(object sender, EventArgs e)
        {
            this.btnInstant.Enabled = false;
            try
            {
                string sql = @"SELECT   v.id AS 参数ID,
                                    p.name AS 参数名称,
                                    t.name AS 参数类型,
                                    v.value1 AS 采集值1,
                                    v.value2 AS 采集值2,
                                    v.value3 AS 采集值3,
                                    p.unit AS 单位,
                                    v.time AS 刷新时间,
                                    v.zztime AS 存储点
                        FROM ProcessItem p 
                        LEFT JOIN meter_uuid m ON p.id = m.uuid 
                        LEFT JOIN meter_type t ON p.protocolType = t.protocol 
                        RIGHT JOIN meters_value v ON m.id = v.id 
                        ORDER BY v.id ";
                SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                dt = new DataTable();
                adapter.Fill(dt);
                this.dgvDB.DataSource = dt;
                dgvDB.Columns["刷新时间"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss.fff";
                dgvDB.Columns["存储点"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss.fff";

                switch (connection.State)
                {
                    case ConnectionState.Closed:
                        this.labDBState.BackColor = Color.Red;
                        this.labDBState.ForeColor = Color.Yellow;
                        this.labDBState.Text = "通信错误";
                        break;
                    case ConnectionState.Open:
                        this.labDBState.BackColor = Color.Green;
                        this.labDBState.ForeColor = Color.White;
                        this.labDBState.Text = "通信正常";
                        break;
                }

            }
            catch
            { };

            this.btnInstant.Enabled = true;
        }
        Form testAlarm = null;
        private void btnTestAlarm_Click(object sender, EventArgs e)
        {
            if (testAlarm != null && !testAlarm.IsDisposed)
                testAlarm.Show();
            else
                (testAlarm = new TestAlarm()).Show();
        }
    }
}
