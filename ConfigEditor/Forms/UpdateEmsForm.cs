/**
 * 文件名：UpdateEmsForm.cs
 * 说明：更新变量到 EMS 系统
 * 作者：林安城
 * 更改记录： 
 * -------------------------------------------------------
 * 改动人 	时间 			原因
 * -------------------------------------------------------
 * 林安城 	2013-10-09		创建文件
 * -------------------------------------------------------
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using ConfigEditor.Core.Util;
using ConfigEditor.Core.ViewModels;
using JonLibrary.Common;
using ConfigEditor.Util;

namespace ConfigEditor.Forms
{
    /// <summary>
    /// 更新变量到 EMS 系统
    /// </summary>
    public partial class UpdateEmsForm : Form
    {
        //日志
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //主窗体
        private MainForm _parentForm;

        //获取数据库列表脚本
        private const string SQL_GET_DATABASES = "select name from master..sysdatabases";

        //更新变量脚本
        private const string SQL_INSERT_ITEMS_FORMAT = @"IF NOT EXISTS(SELECT * FROM ProcessItem WHERE slave = '{3}') INSERT INTO ProcessItem (id, name, code, slave, type, updateRate, createTime) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}',GETDATE());";
        private const string SQL_SELECT_ITEM_FORMAT = @"SELECT COUNT(*) FROM ProcessItem WHERE slave = '{0}'";

        //连接名称
        public const string DefaultConnection = "DefaultConnection";

        private UpdateEmsForm()
        {
            InitializeComponent();
        }

        public UpdateEmsForm(MainForm parentForm)
            : this()
        {
            this._parentForm = parentForm;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateEmsForm_Load(object sender, EventArgs e)
        {
            try
            {
                this.cmbAuth.SelectedIndex = 1;

                //读取配置文件并显示
                string connectionString = ConfigurationManager.ConnectionStrings[DefaultConnection].ConnectionString;
                if (!string.IsNullOrEmpty(connectionString))
                {
                    SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder(connectionString);
                    this.txtServer.Text = sqlBuilder.DataSource;

                    if (sqlBuilder.IntegratedSecurity)
                    {
                        this.cmbAuth.SelectedIndex = 0;
                    }
                    else
                    {
                        this.cmbAuth.SelectedIndex = 1;
                        this.txtUser.Text = sqlBuilder.UserID;
                        this.txtPassword.Text = sqlBuilder.Password;
                    }

                    this.cmbDatabase.Text = sqlBuilder.InitialCatalog;
                }

                string updateTime = ConfigurationManager.AppSettings["LAST_UPDATE_TIME"];
                if (!string.IsNullOrEmpty(updateTime))
                {
                    this.txtUpdateTime.Text = updateTime;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 连接测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTest_Click(object sender, EventArgs e)
        {
            try
            {
                string connectionString = string.Empty;
                if (this.cmbAuth.SelectedIndex == 0)
                {
                    connectionString = "Integrated Security=True;Server={0};Database={1}";
                    object[] objs = new object[]
                    {
                        this.txtServer.Text,
                        string.IsNullOrEmpty(this.cmbDatabase.Text) ? "master" : this.cmbDatabase.Text
                    };

                    connectionString = string.Format(connectionString, objs);
                }
                else
                {
                    connectionString = "Server={0};Database={1};User={2};Password={3}";
                    object[] objs = new object[]
                    {
                        this.txtServer.Text,
                        string.IsNullOrEmpty(this.cmbDatabase.Text) ? "master" : this.cmbDatabase.Text,
                        this.txtUser.Text,
                        this.txtPassword.Text
                    };

                    connectionString = string.Format(connectionString, objs);
                }

                using (DbConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                }

                //写入应用程序配置文件
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                ConnectionStringSettings css = new ConnectionStringSettings(DefaultConnection, connectionString, "System.Data.SqlClient");
                config.ConnectionStrings.ConnectionStrings.Clear();
                config.ConnectionStrings.ConnectionStrings.Add(css);

                config.Save(ConfigurationSaveMode.Full);
                ConfigurationManager.RefreshSection("connectionStrings");

                MessageBox.Show("测试数据库连接成功。", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show("测试数据库连接失败。\n" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 开始更新操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                string server = this.txtServer.Text;
                string user = this.txtUser.Text;
                string password = this.txtPassword.Text;
                string database = string.Empty;

                if (string.IsNullOrEmpty(this.txtServer.Text))
                {
                    MessageBox.Show("服务器不能为空。", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (this.cmbAuth.Text == "SQL Server 身份验证")
                {
                    if (string.IsNullOrEmpty(this.txtUser.Text))
                    {
                        MessageBox.Show("用户名不能为空。", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }

                if (string.IsNullOrEmpty(this.cmbDatabase.Text))
                {
                    MessageBox.Show("选择数据库不能为空。", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                database = this.cmbDatabase.Text;

                //生成数据库连接字符串
                string connectionString = string.Empty;
                if (this.cmbAuth.SelectedIndex == 0)
                {
                    connectionString = "Integrated Security=True;Server={0};Database={1}";
                    object[] objs = new object[]
                    {
                        server,
                        database
                    };

                    connectionString = string.Format(connectionString, objs);
                }
                else
                {
                    connectionString = "Server={0};Database={1};User={2};Password={3}";
                    object[] objs = new object[]
                    {
                        server,
                        database,
                        user,
                        password
                    };

                    connectionString = string.Format(connectionString, objs);
                }

                //写入应用程序配置文件
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                ConnectionStringSettings css = new ConnectionStringSettings(DefaultConnection, connectionString, "System.Data.SqlClient");
                config.ConnectionStrings.ConnectionStrings.Clear();
                config.ConnectionStrings.ConnectionStrings.Add(css);

                config.Save(ConfigurationSaveMode.Full);
                ConfigurationManager.RefreshSection("connectionStrings");

                this.txtLog.Clear();

                //写入INI配置文件
                this.WriteIniFile(server, user, password, database);

                //更新服务器
                this.UpdateEmsItems(connectionString);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 写入INI配置文件
        /// </summary>
        /// <param name="server"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="database"></param>
        private void WriteIniFile(string server, string user, string password, string database)
        {
            string iniFile = Path.Combine(Application.StartupPath, "MicroDAQ.ini");
            if (!File.Exists(iniFile))
            {
                this.txtLog.AppendText("MicroDAQ.ini 配置文件不存在。" + Environment.NewLine);
                return;
            }

            IniFile ini = new IniFile(iniFile);
            string[] dbs = ini.GetValue("Database", "Members").Trim().Split(',');
            if (dbs.Length > 0)
            {
                ini.SetValue(dbs[0], "Address", server);
                ini.SetValue(dbs[0], "PersistSecurityInfo", "True");
                ini.SetValue(dbs[0], "Database", database);
                ini.SetValue(dbs[0], "Username", user);
                ini.SetValue(dbs[0], "Password", password);
            }
        }

        /// <summary>
        /// 更新数据库的变量
        /// </summary>
        /// <param name="connectionString"></param>
        private void UpdateEmsItems(string connectionString)
        {
            StringBuilder sb = new StringBuilder();
            ProjectViewModel project = this._parentForm.Project;

            int allCount = 0;
            int insertCount = 0;

            List<ItemViewModel> successUpdateItems = new List<ItemViewModel>();

            foreach (DeviceViewModel device in project.AllDevices)
            {
                allCount += device.Items.Count;
                if (!device.Channel.IsEnable || !device.IsEnable)
                {
                    continue;
                }

                foreach (ItemViewModel item in device.Items)
                {
                    if (item.IsEnable && item.Code.HasValue && item.Code > 0)
                    {
                        Dictionary<string, object> pars1 = new Dictionary<string, object>();
                        pars1["slave"] = item.Code;

                        int result = Convert.ToInt32(SQLServerHelper.ExecuteScalar(connectionString, CommandType.Text, SqlMapHelper.Format(SqlMapHelper.CountItems, pars1), null));
                        if (result > 0)
                        {
                            continue;
                        }

                        insertCount++;

                        string id = Guid.NewGuid().ToString().Replace("-", string.Empty);
                        Dictionary<string, object> pars2 = new Dictionary<string, object>();
                        pars2["id"] = id;
                        pars2["name"] = item.Name;
                        pars2["code"] = item.Code;
                        pars2["slave"] = item.Code;
                        pars2["type"] = item.DataType.ToString();
                        pars2["updateRate"] = item.ScanPeriod;

                        string sql = SqlMapHelper.Format(SqlMapHelper.CreateItems, pars2);
                        sb.AppendLine(sql);

                        successUpdateItems.Add(item);
                    }
                }
            }

            if (sb.Length > 0)
            {
                SQLServerHelper.ExecuteNonQuery(connectionString, CommandType.Text, sb.ToString(), null);
            }

            //写入应用程序配置文件
            DateTime now = DateTime.Now;
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["LAST_UPDATE_TIME"].Value = now.ToString("yyyy-MM-dd HH:mm:ss");
            config.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection("appSettings");

            this.txtUpdateTime.Text = now.ToString("yyyy-MM-dd HH:mm:ss");
            this.txtLog.AppendText(string.Format("项目的变量总数：{0} 个；" + Environment.NewLine, allCount));
            this.txtLog.AppendText(string.Format("更新的变量个数：{0} 个；" + Environment.NewLine, insertCount));

            this.txtLog.AppendText("-".PadRight(45, '-') + Environment.NewLine);
            this.txtLog.AppendText("设备名称".PadRightEx(20, ' ') + "变量名称".PadRightEx(20, ' ') + "标识码" + Environment.NewLine);
            foreach (ItemViewModel item in successUpdateItems)
            {
                string line = item.Device.Name.PadRightEx(20, ' ') + item.Name.PadRightEx(20, ' ') + item.Code.Value.ToString() + Environment.NewLine;
                this.txtLog.AppendText(line);
            }

            this.txtLog.AppendText("-".PadRight(45, '-') + Environment.NewLine);
            this.txtLog.AppendText("更新完成。");

        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 选择身份验证事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbAuth_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbAuth.Text == "SQL Server 身份验证")
            {
                this.txtUser.Enabled = true;
                this.txtPassword.Enabled = true;
            }
            else
            {
                this.txtUser.Enabled = false;
                this.txtPassword.Enabled = false;
            }
        }

        /// <summary>
        /// 获取数据库列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbDatabase_DropDown(object sender, EventArgs e)
        {
            try
            {
                this.cmbDatabase.Items.Clear();

                string connectionString = BuildMssqlMasterConnectionString();
                List<object> result = SQLServerHelper.ExecuteSingleColumn(connectionString, CommandType.Text, SQL_GET_DATABASES, null);
                string[] tables = result.Select(obj => obj as string).ToArray();

                this.cmbDatabase.Items.AddRange(tables);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show("获取所有数据库名失败。\n" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 构建连接字符串
        /// </summary>
        /// <returns></returns>
        private string BuildMssqlConnectionString()
        {
            string connectionString = string.Empty;

            if (this.cmbAuth.SelectedIndex == 0)
            {
                connectionString = "Integrated Security=True;Server={0};Database={1}";
                object[] objs = new object[]
                {
                    this.txtServer.Text,
                    this.cmbDatabase.Text
                };

                connectionString = string.Format(connectionString, objs);
            }
            else
            {
                connectionString = "Server={0};Database={1};User={2};Password={3}";
                object[] objs = new object[]
                {
                    this.txtServer.Text,
                    this.cmbDatabase.Text,
                    this.txtUser.Text,
                    this.txtPassword.Text
                };

                connectionString = string.Format(connectionString, objs);
            }

            return connectionString;
        }

        /// <summary>
        /// 构建连接字符串
        /// </summary>
        /// <returns></returns>
        private string BuildMssqlMasterConnectionString()
        {
            string connectionString = string.Empty;

            if (this.cmbAuth.SelectedIndex == 0)
            {
                connectionString = "Integrated Security=True;Server={0};Database={1}";
                object[] objs = new object[]
                {
                    this.txtServer.Text,
                    "master"
                };

                connectionString = string.Format(connectionString, objs);
            }
            else
            {
                connectionString = "Server={0};Database={1};User={2};Password={3}";
                object[] objs = new object[]
                {
                    this.txtServer.Text,
                    "master",
                    this.txtUser.Text,
                    this.txtPassword.Text
                };

                connectionString = string.Format(connectionString, objs);
            }

            return connectionString;
        }


    }
}
