/**
 * 文件名：SerialPortEditForm.cs
 * 说明：变量添加或编辑窗体类
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
using System.Text.RegularExpressions;
using ConfigEditor.Core.ViewModels;
using ConfigEditor.Core.Models;

namespace ConfigEditor.Forms
{
    /// <summary>
    /// 变量添加或编辑窗体类
    /// </summary>
    public partial class ItemEditForm : Form
    {
        //日志
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //主窗体
        private MainForm _parentForm;

        //用户操作
        private UserActions _action;

        //视图模型
        private ItemViewModel _model;
        
        //所属设备
        private DeviceViewModel _device;

        /// <summary>
        /// 实体模型
        /// </summary>
        public ItemViewModel Model { get { return _model; } }

        public ItemEditForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 添加变量
        /// </summary>
        /// <param name="parentForm"></param>
        /// <param name="action"></param>
        public ItemEditForm(MainForm parentForm, DeviceViewModel device)
            : this()
        {
            this.Text = "添加变量";
            this._parentForm = parentForm;
            this._action = UserActions.Add;
            this._device = device;
        }

        /// <summary>
        /// 编辑变量
        /// </summary>
        /// <param name="parentForm"></param>
        /// <param name="model"></param>
        public ItemEditForm(MainForm parentForm, DeviceViewModel device, ItemViewModel model)
            : this()
        {
            this.Text = "编辑变量";
            this._parentForm = parentForm;
            this._action = UserActions.Edit;
            this._model = model;
            this._device = device;
        }

        /// <summary>
        /// 窗体初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemEditForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (this._action == UserActions.Add)
                {
                    this.txtName.Text = this.GetNewItemName();

                    this.cmbTableName.SelectedIndex = 0;
                    this.cmbAccess.SelectedIndex = 0;
                    this.cmbDataType.SelectedIndex = 0;
                }
                else if (this._action == UserActions.Edit)
                {
                    this.btnAddNext.Enabled = false;

                    this.txtName.Text = this._model.Name;
                    this.txtAlias.Text = this._model.Alias;
                    this.txtCode.Text = this._model.Code.ToString();
                    this.cmbTableName.SelectedIndex = (int)this._model.TableName;
                    this.cmbAccess.SelectedIndex = (int)this._model.Access;
                    this.cmbDataType.SelectedIndex = (int)this._model.DataType;
                    this.cmbPrecision.Text = this._model.Precision.ToString();
                    this.txtAddress.Text = this._model.Address;
                    this.txtLength.Text = this._model.Length.ToString();

                    this.txtMaximum.Text = this._model.Maximum.ToString();
                    this.txtMinimum.Text = this._model.Minimum.ToString();
                    this.txtScanPeriod.Text = this._model.ScanPeriod.ToString();
                    this.chkIsEnable.Checked = this._model.IsEnable;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 校验用户输入
        /// </summary>
        /// <returns></returns>
        private bool CheckUserInputs()
        {
            if (string.IsNullOrEmpty(this.txtName.Text))
            {
                MessageBox.Show("名称不能为空。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            var query = this._device.Items.Where(obj => obj.Name.Equals(this.txtName.Text) && obj != this._model);
            if (query.Any())
            {
                MessageBox.Show("名称不能重复。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (string.IsNullOrEmpty(this.txtCode.Text))
            {
                MessageBox.Show("识别码不能为空。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (!string.IsNullOrEmpty(this.txtCode.Text) && !Regex.IsMatch(this.txtCode.Text, @"^[0-9]+$"))
            {
                MessageBox.Show("识别码必须为整数。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (string.IsNullOrEmpty(this.txtAddress.Text))
            {
                MessageBox.Show("寄存器地址不能为空。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (!Regex.IsMatch(this.txtAddress.Text, @"^(0x){1}[0-9a-fA-F]{4}$"))
            {
                MessageBox.Show("寄存器地址格式错误，正确格式如0x0101。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (string.IsNullOrEmpty(this.txtLength.Text))
            {
                MessageBox.Show("寄存器长度不能为空。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (!Regex.IsMatch(this.txtLength.Text, @"^[0-9]+$"))
            {
                MessageBox.Show("寄存器长度必须为正整数。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (!Regex.IsMatch(this.txtScanPeriod.Text, @"^[0-9]+$"))
            {
                MessageBox.Show("刷新周期必须为整数。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (!string.IsNullOrEmpty(this.txtMaximum.Text) && !Regex.IsMatch(this.txtMaximum.Text, @"^[0-9]+$"))
            {
                MessageBox.Show("最大有效值必须为整数。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (!string.IsNullOrEmpty(this.txtMinimum.Text) && !Regex.IsMatch(this.txtMinimum.Text, @"^[0-9]+$"))
            {
                MessageBox.Show("最小有效值必须为整数。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return true;
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
                if (!this.CheckUserInputs())
                {
                    return;
                }

                if (this._action == UserActions.Add)
                {
                    this._model = new ItemViewModel();
                    this._model.Device = this._device;

                    this._model.Name = this.txtName.Text;
                    this._model.Alias = this.txtAlias.Text;
                    this._model.Code = string.IsNullOrEmpty(this.txtCode.Text) ? null : (int?)Convert.ToInt32(this.txtCode.Text);

                    this._model.TableName = (ModbusDataModels)this.cmbTableName.SelectedIndex;
                    this._model.Access = (AccessRights)this.cmbAccess.SelectedIndex;
                    this._model.DataType = (DataTypes)this.cmbDataType.SelectedIndex;
                    this._model.Precision = (!this.cmbPrecision.Enabled) || string.IsNullOrEmpty(this.cmbPrecision.Text) ? null : (int?)Convert.ToInt32(this.cmbPrecision.Text);

                    this._model.Address = this.txtAddress.Text;
                    this._model.Length = Convert.ToInt32(this.txtLength.Text);
                    this._model.Maximum = (!this.txtMaximum.Enabled) || string.IsNullOrEmpty(this.txtMaximum.Text) ? null : (int?)Convert.ToInt32(this.txtMaximum.Text);
                    this._model.Minimum = (!this.txtMinimum.Enabled) || string.IsNullOrEmpty(this.txtMinimum.Text) ? null : (int?)Convert.ToInt32(this.txtMinimum.Text);

                    this._model.ScanPeriod = Convert.ToInt32(this.txtScanPeriod.Text);
                    this._model.IsEnable = this.chkIsEnable.Checked;

                    this._device.Items.Add(this._model);
                }
                else
                {
                    this._model.Name = this.txtName.Text;
                    this._model.Alias = this.txtAlias.Text;
                    this._model.Code = string.IsNullOrEmpty(this.txtCode.Text) ? null : (int?)Convert.ToInt32(this.txtCode.Text);

                    this._model.TableName = (ModbusDataModels)this.cmbTableName.SelectedIndex;
                    this._model.Access = (AccessRights)this.cmbAccess.SelectedIndex;
                    this._model.DataType = (DataTypes)this.cmbDataType.SelectedIndex;
                    this._model.Precision = (!this.cmbPrecision.Enabled) || string.IsNullOrEmpty(this.cmbPrecision.Text) ? null : (int?)Convert.ToInt32(this.cmbPrecision.Text);

                    this._model.Address = this.txtAddress.Text;
                    this._model.Length = Convert.ToInt32(this.txtLength.Text);
                    this._model.Maximum = (!this.txtMaximum.Enabled) || string.IsNullOrEmpty(this.txtMaximum.Text) ? null : (int?)Convert.ToInt32(this.txtMaximum.Text);
                    this._model.Minimum = (!this.txtMinimum.Enabled) || string.IsNullOrEmpty(this.txtMinimum.Text) ? null : (int?)Convert.ToInt32(this.txtMinimum.Text);

                    this._model.ScanPeriod = Convert.ToInt32(this.txtScanPeriod.Text);
                    this._model.IsEnable = this.chkIsEnable.Checked;
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
        /// 继续添加操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddNext_Click(object sender, EventArgs e)
        {
            try
            {
                if (!this.CheckUserInputs())
                {
                    return;
                }

                this._model = new ItemViewModel();
                this._model.Device = this._device;

                this._model.Name = this.txtName.Text;
                this._model.Alias = this.txtAlias.Text;
                this._model.Code = string.IsNullOrEmpty(this.txtCode.Text) ? null : (int?)Convert.ToInt32(this.txtCode.Text);

                this._model.TableName = (ModbusDataModels)this.cmbTableName.SelectedIndex;
                this._model.Access = (AccessRights)this.cmbAccess.SelectedIndex;
                this._model.DataType = (DataTypes)this.cmbDataType.SelectedIndex;
                this._model.Precision = (!this.cmbPrecision.Enabled) || string.IsNullOrEmpty(this.cmbPrecision.Text) ? null : (int?)Convert.ToInt32(this.cmbPrecision.Text);

                this._model.Address = this.txtAddress.Text;
                this._model.Length = Convert.ToInt32(this.txtLength.Text);
                this._model.Maximum = (!this.txtMaximum.Enabled) || string.IsNullOrEmpty(this.txtMaximum.Text) ? null : (int?)Convert.ToInt32(this.txtMaximum.Text);
                this._model.Minimum = (!this.txtMinimum.Enabled) || string.IsNullOrEmpty(this.txtMinimum.Text) ? null : (int?)Convert.ToInt32(this.txtMinimum.Text);

                this._model.ScanPeriod = Convert.ToInt32(this.txtScanPeriod.Text);
                this._model.IsEnable = this.chkIsEnable.Checked;

                this._device.Items.Add(this._model);

                //初始化下一个变量
                this.txtName.Text = this.GetNewItemName();
                if (!string.IsNullOrEmpty(this.txtCode.Text))
                {
                    int code = Convert.ToInt32(this.txtCode.Text);
                    this.txtCode.Text = string.Format("{0}", code + 1);
                }

                if (!string.IsNullOrEmpty(this.txtAddress.Text) && !string.IsNullOrEmpty(this.txtLength.Text))
                {
                    int address = Convert.ToInt32(this.txtAddress.Text.Replace("0x", string.Empty), 16);
                    int length = Convert.ToInt32(this.txtLength.Text);

                    this.txtAddress.Text = string.Format("0x{0:X4}", address + length);
                }

                //更新主界面变量列表
                this._parentForm.SaveAndRefreshItemListView(this._model);

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

        /// <summary>
        /// 获取最新名称
        /// </summary>
        /// <returns></returns>
        private string GetNewItemName()
        {
            var query = from item in this._device.Items
                        where item.Name.StartsWith("Item") || item.Name.StartsWith("item")
                        select item;

            if (query.Count() == 0)
            {
                return "Item1";
            }

            int n;
            int xx = query.Where(obj => obj.Name.Length > 4 && Int32.TryParse(obj.Name.Substring(4), out n)).Select(obj => Convert.ToInt32(obj.Name.Substring(4))).Max();

            return "Item" + (++xx).ToString();
        }

        /// <summary>
        /// 选择数据类型事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbDataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string dataType = this.cmbDataType.Text;
                switch (dataType)
                {
                    case "整型":
                        this.cmbPrecision.Enabled = false;
                        this.txtMaximum.Enabled = true;
                        this.txtMinimum.Enabled = true;
                        this.cmbPrecision.Text = null;
                        break;

                    case "实型":
                        this.cmbPrecision.Enabled = true;
                        this.txtMaximum.Enabled = true;
                        this.txtMinimum.Enabled = true;
                        this.cmbPrecision.Text = "2";
                        break;

                    case "离散型":
                    case "字符串":
                        this.cmbPrecision.Enabled = false;
                        this.txtMaximum.Enabled = false;
                        this.txtMinimum.Enabled = false;
                        this.cmbPrecision.Text = null;
                        break;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 寄存器地址，活动焦点事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtAddress_Enter(object sender, EventArgs e)
        {
            try
            {
                this.txtAddress.TextAlign = HorizontalAlignment.Left;

                if(string.IsNullOrEmpty(this.txtAddress.Text))
                {
                    this.txtAddress.AppendText("0x");
                    this.txtAddress.SelectionStart = this.txtAddress.TextLength;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 寄存器地址，失去焦点事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtAddress_Leave(object sender, EventArgs e)
        {
            try
            {
                this.txtAddress.TextAlign = HorizontalAlignment.Right;

                if (this.txtAddress.Text.Equals("0x", StringComparison.CurrentCultureIgnoreCase))
                {
                    this.txtAddress.Clear();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
