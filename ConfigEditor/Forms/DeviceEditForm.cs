/**
 * 文件名：DeviceEditForm.cs
 * 说明：设备添加或编辑窗体类
 * 作者：林安城
 * 更改记录： 
 * -------------------------------------------------------
 * 改动人 	时间 			原因
 * -------------------------------------------------------
 * 林安城 	2013-09-29		创建文件
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
using ConfigEditor.Core.Models;
using ConfigEditor.Core.ViewModels;
using System.Text.RegularExpressions;

namespace ConfigEditor.Forms
{
    /// <summary>
    /// 设备添加或编辑窗体类
    /// </summary>
    public partial class DeviceEditForm : Form
    {
        //日志
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //主窗体
        private MainForm _parentForm;

        //用户操作
        private UserActions _action;

        //视图模型
        private DeviceViewModel _model;

        /// <summary>
        /// 实体模型
        /// </summary>
        public DeviceViewModel Model { get { return _model; } }

        public DeviceEditForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 添加设备
        /// </summary>
        /// <param name="parentForm"></param>
        /// <param name="action"></param>
        public DeviceEditForm(MainForm parentForm)
            : this()
        {
            this.Text = "添加设备";
            this._parentForm = parentForm;
            this._action = UserActions.Add;
        }

        /// <summary>
        /// 编辑设备
        /// </summary>
        /// <param name="parentForm"></param>
        /// <param name="model"></param>
        public DeviceEditForm(MainForm parentForm, DeviceViewModel model)
            : this()
        {
            this.Text = "编辑设备";
            this._parentForm = parentForm;
            this._action = UserActions.Edit;
            this._model = model;
        }

        private void DeviceEditForm_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 保存操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.txtName.Text))
                {
                    MessageBox.Show("名称不能为空。", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (!Regex.IsMatch(this.txtSlave.Text, @"^[0-9]+$") || Convert.ToInt32(this.txtSlave.Text) > 0 || Convert.ToInt32(this.txtSlave.Text) < 256)
                {
                    MessageBox.Show("从站号必须为1-255的值。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var query = this._parentForm.Project.SerialPorts.Where(obj => obj.PortName.Equals(this.txtName.Text) && obj != this._model);
                if (query.Any())
                {
                    MessageBox.Show("名称不能重复。", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (this._action == UserActions.Add)
                {
                    this._model = new DeviceViewModel();

                    this._model.Name = this.txtName.Text;
                    this._model.Alias = this.txtAlias.Text;
                    this._model.Slave = Convert.ToInt32(this.txtSlave.Text);
                    this._model.IpAddress = this.txtIp.Text;
                    this._model.IpPort = Convert.ToInt32(this.txtPort.Text);
                    this._model.IsEnable = this.chkIsEnable.Enabled;
                }
                else
                {

                }

                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 取消操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                this.DialogResult = DialogResult.Cancel;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
