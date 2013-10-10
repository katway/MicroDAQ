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
using ConfigEditor.Core.Util;

namespace ConfigEditor.Forms
{
    /// <summary>
    /// 更新变量到 EMS 系统
    /// </summary>
    public partial class UpdateEmsForm : Form
    {
        //日志
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //获取数据库列表脚本
        private const string SQL_GET_DATABASES = "SELECT name FROM sys.databases ORDER BY name";

        public UpdateEmsForm()
        {
            InitializeComponent();
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
                int port = 1433;
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

                //保存数据库连接信息到配置文件

            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
