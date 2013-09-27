/**
 * 文件名：MainForm.cs
 * 说明：配置工具主窗体类
 * 作者：林安城
 * 更改记录： 
 * -------------------------------------------------------
 * 改动人 	时间 			原因
 * -------------------------------------------------------
 * 林安城 	2013-09-20		创建文件
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
using ConfigEditor.Forms;
using ConfigEditor.Core.ViewModels;

namespace ConfigEditor
{
    /// <summary>
    /// 配置工具主窗体类
    /// </summary>
    public partial class MainForm : Form
    {
        //日志
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //同步对象
        private static object sync = new object();

        //项目实体对象
        private ProjectViewModel _project;

        /// <summary>
        /// 项目实体对象
        /// </summary>
        public ProjectViewModel Project { get { return _project; } }


        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 界面初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        /// <summary>
        /// 添加串口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiAddSerialPort_Click(object sender, EventArgs e)
        {
            try
            {
                SerialPortEditForm frm = new SerialPortEditForm();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 添加设备
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiAddDevice_Click(object sender, EventArgs e)
        {
            try
            {
                DeviceEditForm frm = new DeviceEditForm();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 添加变量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiAddItem_Click(object sender, EventArgs e)
        {
            try
            {
                ItemEditForm frm = new ItemEditForm();
                frm.ShowDialog();
            }
            catch (Exception ex) 
            {
                log.Error(ex);
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 批量添加变量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiBatchAddItem_Click(object sender, EventArgs e)
        {
            try
            {
                ItemBatchAddForm frm = new ItemBatchAddForm();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 启用/禁用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiEnable_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 编辑，根据选择的实体类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiEdit_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 删除，根据选择的实体类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiDelete_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 项目属性
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiProjectProperty_Click(object sender, EventArgs e)
        {
            try
            {
                ProjectPropertyForm frm = new ProjectPropertyForm();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 清空项目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiClearProject_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 选项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiOptions_Click(object sender, EventArgs e)
        {
            try
            {
                OptionsForm frm = new OptionsForm();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiTest_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 关于
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiAbout_Click(object sender, EventArgs e)
        {
            try
            {
                AboutForm frm = new AboutForm();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
