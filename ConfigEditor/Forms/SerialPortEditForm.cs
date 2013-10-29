/**
 * 文件名：SerialPortEditForm.cs
 * 说明：串口添加或编辑窗体类
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
using System.IO.Ports;
using System.Text.RegularExpressions;
using ConfigEditor.Util;
using ConfigEditor.Core.Models;
using ConfigEditor.Core.ViewModels;

namespace ConfigEditor.Forms
{
    /// <summary>
    /// 串口添加或编辑窗体类
    /// </summary>
    public partial class SerialPortEditForm : Form
    {
        //日志
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //主窗体
        private MainForm _parentForm;

        //用户操作
        private UserActions _action;

        //视图模型
        private SerialPortViewModel _model;

        /// <summary>
        /// 实体模型
        /// </summary>
        public SerialPortViewModel Model { get { return _model; } }


        public SerialPortEditForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 添加串口
        /// </summary>
        /// <param name="parentForm"></param>
        /// <param name="action"></param>
        public SerialPortEditForm(MainForm parentForm)
            : this()
        {
            this.Text = "添加串口";
            this._parentForm = parentForm;
            this._action = UserActions.Add;
        }

        /// <summary>
        /// 编辑串口
        /// </summary>
        /// <param name="parentForm"></param>
        /// <param name="model"></param>
        public SerialPortEditForm(MainForm parentForm, SerialPortViewModel model)
            : this()
        {
            this.Text = "编辑串口";
            this._parentForm = parentForm;
            this._action = UserActions.Edit;
            this._model = model;
        }

        /// <summary>
        /// 窗体初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SerialPortEditForm_Load(object sender, EventArgs e)
        {
            try
            {
                string[] portNames = System.IO.Ports.SerialPort.GetPortNames();

                this.cmbPortName.Items.AddRange(portNames);
                this.cmbBaudRate.Items.AddRange(EditHelper.COMMON_BAUD_RATE.Select(obj => obj.ToString()).ToArray());

                if (this._action == UserActions.Add)
                {
                    if (this.cmbPortName.Items.Count > 0)
                    {
                        this.cmbPortName.SelectedIndex = 0;
                    }

                    this.cmbBaudRate.Text = "9600";
                    this.cmbDataBits.Text = "8";
                    this.cmbParity.Text = "无";
                    this.cmbStopBits.Text = "1";
                }
                else if (this._action == UserActions.Edit)
                {
                    Parity parity = Parity.None;
                    string parityShow = "无";
                    bool success = Enum.TryParse<Parity>(this._model.Parity, out parity);
                    if (success)
                    {
                        switch (parity)
                        {
                            case Parity.Even:
                                parityShow = "偶";
                                break;

                            case Parity.Odd:
                                parityShow = "奇";
                                break;

                            case Parity.None:
                                parityShow = "无";
                                break;

                            case Parity.Mark:
                                parityShow = "标志";
                                break;

                            case Parity.Space:
                                parityShow = "空格";
                                break;
                        }
                    }

                    this.cmbPortName.Text = this._model.PortName;
                    this.cmbBaudRate.Text = this._model.BaudRate.ToString();
                    this.cmbDataBits.Text = this._model.DataBits.ToString();
                    this.cmbParity.Text = parityShow;
                    this.cmbStopBits.Text = (this._model.StopBits == "3" ) ? "1.5" : this._model.StopBits;
                    this.chkIsEnable.Checked = this._model.IsEnable;

                    if (this._model.Protocol == ModbusProtocols.ModbusRTU)
                    {
                        this.rdoRtu.Checked = true;
                    }
                    else if(this._model.Protocol == ModbusProtocols.ModbusASCII)
                    {
                        this.rdoAscii.Checked = true;
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
                if (!Regex.IsMatch(this.cmbPortName.Text, @"^COM[0-9]+$"))
                {
                    MessageBox.Show("串口号格式不正确,正确格式如：COM1", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var query = this._parentForm.Project.SerialPorts.Where(obj => obj.PortName.Equals(this.cmbPortName.Text) && obj != this._model);
                if (query.Any())
                {
                    MessageBox.Show("串口号不能重复。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (string.IsNullOrEmpty(this.cmbDataBits.Text))
                {
                    MessageBox.Show("波特率不能为空。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                Parity parity = Parity.None;
                switch (this.cmbParity.Text)
                {
                    case "偶":
                        parity = Parity.Even;
                        break;

                    case "奇":
                        parity = Parity.Odd;
                        break;

                    case "无":
                        parity = Parity.None;
                        break;

                    case "标志":
                        parity = Parity.Mark;
                        break;

                    case "空格":
                        parity = Parity.Space;
                        break;
                }

                if (this._action == UserActions.Add)
                {
                    this._model = new SerialPortViewModel();

                    this._model.PortName = this.cmbPortName.Text;
                    this._model.BaudRate = Convert.ToInt32(this.cmbBaudRate.Text);
                    this._model.DataBits = Convert.ToInt32(this.cmbDataBits.Text);
                    this._model.Parity = parity.ToString();
                    this._model.StopBits = this.cmbStopBits.Text;
                    this._model.IsEnable = this.chkIsEnable.Checked;

                    if (this.rdoRtu.Checked)
                    {
                        this._model.Protocol = ModbusProtocols.ModbusRTU;
                    }
                    else
                    {
                        this._model.Protocol = ModbusProtocols.ModbusASCII;
                    }

                }
                else
                {
                    this._model.PortName = this.cmbPortName.Text;
                    this._model.BaudRate = Convert.ToInt32(this.cmbBaudRate.Text);
                    this._model.DataBits = Convert.ToInt32(this.cmbDataBits.Text);
                    this._model.Parity = parity.ToString();
                    this._model.StopBits = this.cmbStopBits.Text;
                    this._model.IsEnable = this.chkIsEnable.Checked;

                    if (this.rdoRtu.Checked)
                    {
                        this._model.Protocol = ModbusProtocols.ModbusRTU;
                    }
                    else
                    {
                        this._model.Protocol = ModbusProtocols.ModbusASCII;
                    }
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
