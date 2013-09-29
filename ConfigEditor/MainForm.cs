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
using ConfigEditor.Core.Services;

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
            try
            {
                this._project = new ProjectViewModel();

                this.naviTreeView.ExpandAll();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                SerialPortEditForm frm = new SerialPortEditForm(this);
                DialogResult dr = frm.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    SerialPortViewModel model = frm.Model;
                    
                    TreeNode node = new TreeNode(model.PortName);
                    node.Tag = model;
                    node.ImageKey = "channel.bmp";
                    node.SelectedImageKey = "channel.bmp";

                    this._project.SerialPorts.Add(model);
                    this.naviTreeView.Nodes[0].Nodes.Add(node);
                    this.naviTreeView.SelectedNode = node;
                    if (!this.naviTreeView.Nodes[0].IsExpanded)
                    {
                        this.naviTreeView.Nodes[0].Expand();
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
        /// 添加设备
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiAddDevice_Click(object sender, EventArgs e)
        {
            try
            {
                DeviceEditForm frm = new DeviceEditForm();
                DialogResult dr = frm.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    DeviceViewModel model = frm.Model;

                    TreeNode node = new TreeNode(model.Name);
                    node.Tag = model;

                    if (model.IsEnable)
                    {
                        node.ImageKey = "device.bmp";
                        node.SelectedImageKey = "device.bmp";
                    }
                    else
                    {
                        node.ImageKey = "disable_device.bmp";
                        node.SelectedImageKey = "disable_device.bmp";
                    }

                    TreeNode parentNode = this.naviTreeView.SelectedNode;
                    //以太网通道设备
                    if (parentNode.Level == 0)
                    {
                        this._project.Ethernet.Devices.Add(model);
                        this.naviTreeView.Nodes[1].Nodes.Add(node);
                        this.naviTreeView.SelectedNode = node;
                        if (!this.naviTreeView.Nodes[1].IsExpanded)
                        {
                            this.naviTreeView.Nodes[1].Expand();
                        }
                    }
                    //串口通道设备
                    else
                    {
                        SerialPortViewModel parentModel = parentNode.Tag as SerialPortViewModel;
                        parentModel.Devices.Add(model);
                        parentNode.Nodes.Add(node);
                        this.naviTreeView.SelectedNode = node;
                        if (!parentNode.IsExpanded)
                        {
                            parentNode.Expand();
                        }
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
        /// 添加变量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiAddItem_Click(object sender, EventArgs e)
        {
            try
            {
                ItemEditForm frm = new ItemEditForm();
                DialogResult dr = frm.ShowDialog();
                if (dr == DialogResult.OK)
                {

                }
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
                DialogResult dr = frm.ShowDialog();
                if (dr == DialogResult.OK)
                {

                }
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
            try
            {
                //编辑变量
                if (this.itemListView.Focused && this.itemListView.SelectedItems.Count > 0)
                {
                    ListViewItem lvi = this.itemListView.SelectedItems[0];
                    ItemViewModel model = lvi.Tag as ItemViewModel;

                    ItemEditForm frm = new ItemEditForm(this, model);
                    frm.ShowDialog();
                }
                //编辑其他
                else if (this.naviTreeView.Focused && this.naviTreeView.SelectedNode != null)
                {
                    TreeNode node = this.naviTreeView.SelectedNode;
                    object tag = node.Tag;
                    if (tag == null)
                    {
                        return;
                    }

                    //串口节点
                    if (tag.GetType() == typeof(SerialPortViewModel))
                    {
                        SerialPortViewModel model = node.Tag as SerialPortViewModel;

                        SerialPortEditForm frm = new SerialPortEditForm(this, model);
                        frm.ShowDialog();
                    }
                    //设备节点
                    else if (tag.GetType() == typeof(DeviceViewModel))
                    {
                        DeviceViewModel model = node.Tag as DeviceViewModel;

                        DeviceEditForm frm = new DeviceEditForm(this, model);
                        frm.ShowDialog();
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
        /// 删除，根据选择的实体类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiDelete_Click(object sender, EventArgs e)
        {
            try
            {
                //删除变量
                if (this.itemListView.Focused  && this.itemListView.SelectedItems.Count > 0)
                {
                    if (MessageBox.Show("确定删除变量吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return;
                    }

                    ListView.SelectedIndexCollection indexList = this.itemListView.SelectedIndices;
                    for (int i = indexList.Count; i > 0; i--)
                    {
                        int index = indexList[i - 1];
                        this.itemListView.Items.RemoveAt(index);
                    }
                }
                //删除其他
                else if (this.naviTreeView.Focused && this.naviTreeView.SelectedNode != null)
                {
                    TreeNode node = this.naviTreeView.SelectedNode;
                    object tag = node.Tag;
                    if (tag == null)
                    {
                        return;
                    }

                    //串口节点
                    if (tag.GetType() == typeof(SerialPortViewModel))
                    {
                        if (MessageBox.Show("确定删除串口吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        {
                            return;
                        }

                        SerialPortViewModel model = node.Tag as SerialPortViewModel;
                        this._project.SerialPorts.Remove(model);

                        SerialPortService service = new SerialPortService();
                        service.DeleteSerialPort(model.Id);
                    }
                    //设备节点
                    else if (tag.GetType() == typeof(DeviceViewModel))
                    {
                        if (MessageBox.Show("确定删除设备吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        {
                            return;
                        }

                        DeviceViewModel model = node.Tag as DeviceViewModel;
                        if (node.Level == 1)
                        {
                            this._project.Ethernet.Devices.Remove(model);
                        }
                        else if (node.Level == 2)
                        {
                            SerialPortViewModel parentModel = node.Parent.Tag as SerialPortViewModel;
                            parentModel.Devices.Remove(model);
                        }

                        DeviceService service = new DeviceService();
                        service.DeleteDevice(model.Id);
                    }

                    this.itemListView.Items.Clear();
                    node.Parent.Nodes.Remove(node);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            try
            {
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        /// <summary>
        /// 选择串口、设备节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void naviTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                this.itemListView.Items.Clear();

                TreeNode node = e.Node;
                object tag = node.Tag;

                if (node.Level == 0)
                {
                    //1级节点
                    string type = tag.ToString();
                    if (type == "SerialPorts")
                    {
                        this.tsbAddSerialPort.Enabled = true;
                        this.tsbAddDevice.Enabled = false;
                        this.tsbAddItem.Enabled = false;
                        this.tsbBatchAddItem.Enabled = false;
                        this.tsbEdit.Enabled = false;
                        this.tsbDelete.Enabled = false;
                    }
                    else if(type == "Ethernet")
                    {
                        this.tsbAddSerialPort.Enabled = false;
                        this.tsbAddDevice.Enabled = true;
                        this.tsbAddItem.Enabled = false;
                        this.tsbBatchAddItem.Enabled = false;
                        this.tsbEdit.Enabled = false;
                        this.tsbDelete.Enabled = false;
                    }
                }
                else if (node.Level == 1)
                {
                    //2级节点
                    string type = node.Parent.Tag.ToString();
                    if (type == "SerialPorts")
                    {
                        this.tsbAddSerialPort.Enabled = true;
                        this.tsbAddDevice.Enabled = true;
                        this.tsbAddItem.Enabled = false;
                        this.tsbBatchAddItem.Enabled = false;
                        this.tsbEdit.Enabled = true;
                        this.tsbDelete.Enabled = true;
                    }
                    else if (type == "Ethernet")
                    {
                        this.tsbAddSerialPort.Enabled = false;
                        this.tsbAddDevice.Enabled = true;
                        this.tsbAddItem.Enabled = true;
                        this.tsbBatchAddItem.Enabled = true;
                        this.tsbEdit.Enabled = true;
                        this.tsbDelete.Enabled = true;
                    }
                }
                else
                {
                    //3级节点
                    this.tsbAddSerialPort.Enabled = false;
                    this.tsbAddDevice.Enabled = false;
                    this.tsbAddItem.Enabled = true;
                    this.tsbBatchAddItem.Enabled = true;
                    this.tsbEdit.Enabled = true;
                    this.tsbDelete.Enabled = true;
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
