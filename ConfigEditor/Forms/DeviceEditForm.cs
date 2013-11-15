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
using System.Text.RegularExpressions;
using ConfigEditor.Core.Models;
using ConfigEditor.Core.ViewModels;
using ConfigEditor.Util;
using System.IO;
using ConfigEditor.Core.Xml;
using ConfigEditor.Core.Util;

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

        //所属通道
        private ChannelBase _channel;

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
        public DeviceEditForm(MainForm parentForm, ChannelBase channel)
            : this()
        {
            this.Text = "添加设备";
            this._parentForm = parentForm;
            this._action = UserActions.Add;
            this._channel = channel;
        }

        /// <summary>
        /// 编辑设备
        /// </summary>
        /// <param name="parentForm"></param>
        /// <param name="model"></param>
        public DeviceEditForm(MainForm parentForm, ChannelBase channel, DeviceViewModel model)
            : this()
        {
            this.Text = "编辑设备";
            this._parentForm = parentForm;
            this._action = UserActions.Edit;
            this._channel = channel;
            this._model = model;
        }

        /// <summary>
        /// 初始化窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeviceEditForm_Load(object sender, EventArgs e)
        {
            try
            {
                this.cmbDeviceModels.Items.AddRange(EditHelper.SUPPORT_DEVICES);

                if (this._action == UserActions.Add)
                {
                    this.cmbDeviceModels.SelectedIndex = 0;
                    this.cmbProtocol.SelectedIndex = (int)this._channel.Protocol;
                    this.txtName.Text = this.GetNewDeviceName();
                    this.txtSlave.Text = this.GetNewSlave().ToString();

                    if (this._channel.Protocol == ModbusProtocols.ModbusTCP)
                    {
                        this.txtIp.Enabled = true;
                        this.txtPort.Enabled = true;
                        this.panel1.Visible = true;
                    }
                    else
                    {
                        this.txtIp.Enabled = false;
                        this.txtPort.Enabled = false;
                        this.panel1.Visible = false;
                    }
                }
                else if (this._action == UserActions.Edit)
                {
                    this.cmbDeviceModels.Enabled = false;
                    this.cmbProtocol.SelectedIndex = (int)this._model.Protocol;
                    this.txtName.Text = this._model.Name;
                    this.txtAlias.Text = this._model.Alias;
                    this.txtSlave.Text = this._model.Slave.ToString();
                    this.txtIp.Text = this._model.IpAddress;
                    this.txtPort.Text = this._model.IpPort.ToString();
                    this.chkIsEnable.Checked = this._model.IsEnable;

                    if (this._channel.Protocol == ModbusProtocols.ModbusTCP)
                    {
                        this.txtIp.Enabled = true;
                        this.txtPort.Enabled = true;
                        this.panel1.Visible = true;
                    }
                    else
                    {
                        this.txtIp.Enabled = false;
                        this.txtPort.Enabled = false;
                        this.panel1.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                    MessageBox.Show("名称不能为空。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (!Regex.IsMatch(this.txtSlave.Text, @"^[0-9]+$") || Convert.ToInt32(this.txtSlave.Text) <= 0 || Convert.ToInt32(this.txtSlave.Text) >= 256)
                {
                    MessageBox.Show("从站号为1-255范围的数值。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var query = this._parentForm.Project.AllDevices.Where(obj => obj.Name.Equals(this.txtName.Text) && obj != this._model);
                if (query.Any())
                {
                    MessageBox.Show("名称不能重复。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var query2 = this._channel.Devices.Where(obj => obj.Slave == Convert.ToInt32(this.txtSlave.Text) && obj != this._model);
                if (query2.Any())
                {
                    MessageBox.Show("从站号不能重复。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (this._action == UserActions.Add)
                {
                    this._model = new DeviceViewModel();
                    this._model.Channel = this._channel;
                    this._model.ChannelType = this._channel.Type;
                    this._model.Protocol = (this._channel.Protocol != ModbusProtocols.ModbusTCP) ? this._channel.Protocol : (ModbusProtocols)this.cmbProtocol.SelectedIndex;

                    this._model.Name = this.txtName.Text;
                    this._model.Alias = this.txtAlias.Text;
                    this._model.Slave = Convert.ToInt32(this.txtSlave.Text);
                    this._model.IpAddress = this.txtIp.Text;
                    this._model.IpPort = Convert.ToInt32(this.txtPort.Text);
                    this._model.IsEnable = this.chkIsEnable.Checked;

                    //创建具体设备型号的变量
                    if (this.cmbDeviceModels.Text != EditHelper.SUPPORT_DEVICES[0])
                    {
                        if (!this.CreateDefaultItems(this.cmbDeviceModels.Text))
                        {
                            return;
                        }
                    }
                }
                else
                {
                    this._model.Protocol = (this._model.Protocol != ModbusProtocols.ModbusTCP) ? this._model.Protocol : (ModbusProtocols)this.cmbProtocol.SelectedIndex;
                    
                    this._model.Name = this.txtName.Text;
                    this._model.Alias = this.txtAlias.Text;
                    this._model.Slave = Convert.ToInt32(this.txtSlave.Text);
                    this._model.IpAddress = this.txtIp.Text;
                    this._model.IpPort = Convert.ToInt32(this.txtPort.Text);
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
        /// 创建默认变量
        /// </summary>
        /// <param name="deviceName"></param>
        private bool CreateDefaultItems(string deviceName)
        {
            string xmlFile = Path.Combine(Application.StartupPath , string.Format(@"Devices\{0}.xml", deviceName));
            if (!File.Exists(xmlFile))
            {
                MessageBox.Show("找不到设备驱动模板文件。", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            XmlDevice device = XmlSerializeHelper.Deserialize(xmlFile);
            if (device == null)
            {
                MessageBox.Show("设备驱动模板文件的格式错误。", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (device.Protocols == null || !device.Protocols.Contains(this._channel.Protocol.ToString()))
            {
                MessageBox.Show("设备不支持当前通讯协议。", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            foreach (XmlItem item in device.Items)
            {
                ItemViewModel ivm = new ItemViewModel()
                {
                    Name = item.Name,
                    TableName = EnumHelper.StringToEnum<ModbusDataModels>(item.DataModel),
                    Address = item.Address,
                    Length = item.Length,
                    DataType = EnumHelper.StringToEnum<DataTypes>(item.DataType),
                    Access = EnumHelper.StringToEnum<AccessRights>(item.Access),
                    Alias = string.Empty,
                    ScanPeriod = 1000,
                    IsEnable = true,
                    Device = this._model
                };

                this._model.Items.Add(ivm);
            }

            return true;
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
        private string GetNewDeviceName()
        {
            var query = from device in this._parentForm.Project.AllDevices
                        where device.Name.StartsWith("Device") || device.Name.StartsWith("device")
                        select device;

            if (query.Count() == 0)
            {
                return "Device1";
            }

            int n;
            int xx = query.Where(obj => obj.Name.Length > 6 && Int32.TryParse(obj.Name.Substring(6), out n)).Select(obj => Convert.ToInt32(obj.Name.Substring(6))).Max();

            return "Device" + (++xx).ToString();
        }

        /// <summary>
        /// 获取最新从站号
        /// </summary>
        /// <returns></returns>
        private int GetNewSlave()
        {
            var query = from device in this._channel.Devices
                        select device;

            if (query.Count() == 0)
            {
                return 1;
            }

            int val = query.Max(obj => Convert.ToInt32(obj.Slave));

            return ++val;
        }

        private void txtAlias_TextChanged(object sender, EventArgs e)
        {

        }

      
    }
}
