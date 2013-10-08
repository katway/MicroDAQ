/**
 * 文件名：ItemBatchAddForm.cs
 * 说明：批量添加变量窗体类
 * 作者：林安城
 * 更改记录： 
 * -------------------------------------------------------
 * 改动人 	时间 			原因
 * -------------------------------------------------------
 * 林安城 	2013-09-30		创建文件
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
using ConfigEditor.Core.ViewModels;
using ConfigEditor.Core.Models;
using System.Text.RegularExpressions;

namespace ConfigEditor.Forms
{
    /// <summary>
    /// 批量添加变量窗体类
    /// </summary>
    public partial class ItemBatchAddForm : Form
    {
        //日志
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //主窗体
        private MainForm _parentForm;

        //视图模型
        private ItemViewModel _model;
        
        //所属设备
        private DeviceViewModel _device;

        //变量列表
        private List<ItemViewModel> _models;

        public List<ItemViewModel> Models { get { return _models; } }


        public ItemBatchAddForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 添加变量
        /// </summary>
        /// <param name="parentForm"></param>
        /// <param name="action"></param>
        public ItemBatchAddForm(MainForm parentForm, DeviceViewModel device)
            : this()
        {
            this.Text = "批量添加变量";
            this._parentForm = parentForm;
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
                this.cmbTableName.SelectedIndex = 0;
                this.cmbAccess.SelectedIndex = 0;
                this.cmbDataType.SelectedIndex = 0;

                this._models = new List<ItemViewModel>();
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
            if (string.IsNullOrEmpty(this.txtNamePrefix.Text))
            {
                MessageBox.Show("名称前缀不能为空。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (!string.IsNullOrEmpty(this.txtStartCode.Text) && !Regex.IsMatch(this.txtStartCode.Text, @"^[0-9]+$"))
            {
                MessageBox.Show("起始识别码必须为整数。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

            if (!Regex.IsMatch(this.txtLength.Text, @"^[0-9]+$") && Convert.ToInt32(this.txtLength.Text) > 0)
            {
                MessageBox.Show("寄存器长度必须为正整数。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (!Regex.IsMatch(this.txtScanPeriod.Text, @"^[0-9]+$"))
            {
                MessageBox.Show("刷新周期必须为整数。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                StringBuilder sb = new StringBuilder();
                int count = Convert.ToInt32(this.nupdItemCount.Value);
                int address = Convert.ToInt32(this.txtAddress.Text.Replace("0x", string.Empty), 16);
                int length = Convert.ToInt32(this.txtLength.Text);

                for(int i = 0; i < count; i++)
                {
                    string name = string.Format("{0}{1:0#}", this.txtNamePrefix.Text, i + 1);
                    string alias = string.IsNullOrEmpty(this.txtAliasPrefix.Text) ? string.Empty : string.Format("{0}{1:0#}", this.txtAliasPrefix.Text, i + 1);

                    var query = this._device.Items.Where(obj => obj.Name.Equals(name) && obj != this._model);
                    if (query.Any())
                    {
                        sb.AppendLine(string.Format("名称为{0}的变量已存在；", name));
                        continue;
                    }

                    this._model = new ItemViewModel();
                    this._model.Device = this._device;

                    this._model.Name = name;
                    this._model.Alias = alias;
                    this._model.Code = string.IsNullOrEmpty(this.txtStartCode.Text) ? null : (int?)(Convert.ToInt32(this.txtStartCode.Text) + i);

                    this._model.TableName = (ModbusDataModels)this.cmbTableName.SelectedIndex;
                    this._model.Access = (AccessRights)this.cmbAccess.SelectedIndex;
                    this._model.DataType = (DataTypes)this.cmbDataType.SelectedIndex;
                    this._model.Precision = (!this.cmbPrecision.Enabled) || string.IsNullOrEmpty(this.cmbPrecision.Text) ? null : (int?)Convert.ToInt32(this.cmbPrecision.Text);

                    this._model.Address = string.Format("0x{0:X4}", address + length * i);
                    this._model.Length = Convert.ToInt32(this.txtLength.Text);
                    this._model.Maximum = (!this.txtMaximum.Enabled) || string.IsNullOrEmpty(this.txtMaximum.Text) ? null : (int?)Convert.ToInt32(this.txtMaximum.Text);
                    this._model.Minimum = (!this.txtMinimum.Enabled) || string.IsNullOrEmpty(this.txtMinimum.Text) ? null : (int?)Convert.ToInt32(this.txtMinimum.Text);

                    this._model.ScanPeriod = Convert.ToInt32(this.txtScanPeriod.Text);
                    this._model.IsEnable = this.chkIsEnable.Checked;

                    //更新主界面变量列表
                    this._device.Items.Add(this._model);
                    this._models.Add(this._model);
                }

                if (sb.Length > 0)
                {
                    MessageBox.Show(sb.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                        break;

                    case "实型":
                        this.cmbPrecision.Enabled = true;
                        this.txtMaximum.Enabled = true;
                        this.txtMinimum.Enabled = true;
                        break;

                    case "离散型":
                    case "字符串":
                        this.cmbPrecision.Enabled = false;
                        this.txtMaximum.Enabled = false;
                        this.txtMinimum.Enabled = false;
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
