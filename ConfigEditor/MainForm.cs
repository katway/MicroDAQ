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
using ConfigEditor.Util;
using ConfigEditor.Core.Util;
using ConfigEditor.Core.IO;

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
                this.LoadProject();
                this.naviTreeView.Nodes[1].Tag = this._project.Ethernet;
                this.naviTreeView.ExpandAll();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 加载项目并初始化界面
        /// </summary>
        private void LoadProject()
        {
            this._project = ProjectReader.Read();
            foreach (SerialPortViewModel spvm in this._project.SerialPorts)
            {
                TreeNode spNode = new TreeNode(spvm.PortName);
                spNode.Tag = spvm;
                spNode.ImageKey = "channel.bmp";
                spNode.SelectedImageKey = "channel.bmp";

                this.naviTreeView.Nodes[0].Nodes.Add(spNode);

                foreach (DeviceViewModel dvm in spvm.Devices)
                {
                    TreeNode deviceNode = new TreeNode(dvm.Name);
                    deviceNode.Tag = dvm;
                    deviceNode.ImageKey = "device.bmp";
                    deviceNode.SelectedImageKey = "device.bmp";

                    spNode.Nodes.Add(deviceNode);
                }
            }

            foreach (DeviceViewModel dvm in this._project.Ethernet.Devices)
            {
                TreeNode deviceNode = new TreeNode(dvm.Name);
                deviceNode.Tag = dvm;
                deviceNode.ImageKey = "device.bmp";
                deviceNode.SelectedImageKey = "device.bmp";

                this.naviTreeView.Nodes[1].Nodes.Add(deviceNode);
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

                    SerialPortService service = new SerialPortService();
                    service.AddSerialPort(model);
                    
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
                TreeNode parentNode = this.naviTreeView.SelectedNode;
                ChannelBase channel = parentNode.Tag as ChannelBase;

                DeviceEditForm frm = new DeviceEditForm(this, channel);
                DialogResult dr = frm.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    DeviceViewModel model = frm.Model;

                    DeviceService service = new DeviceService();
                    service.AddDevice(model);

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
                TreeNode parentNode = this.naviTreeView.SelectedNode;
                DeviceViewModel device = parentNode.Tag as DeviceViewModel;
                if (device == null)
                {
                    return;
                }

                ItemEditForm frm = new ItemEditForm(this, device);
                DialogResult dr = frm.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    ItemViewModel model = frm.Model;

                    ItemService service = new ItemService();
                    service.AddItem(model);

                    string[] items = new string[] 
                    { 
                        model.Name,
                        model.Alias,
                        model.Code.ToString(),
                        EnumHelper.EnumToCaption(model.DataType),
                        EnumHelper.EnumToCaption(model.TableName),
                        model.Address,
                        model.Length.ToString(),
                        model.ScanPeriod.ToString()
                    };

                    ListViewItem lvi = new ListViewItem(items);
                    lvi.ImageKey = "tag.bmp";
                    lvi.Tag = model;

                    this.itemListView.Items.Add(lvi);

                    this.itemPropertyGrid.SelectedObject = model;
                }
            }
            catch (Exception ex) 
            {
                log.Error(ex);
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 刷新变量列表窗体
        /// </summary>
        public void SaveAndRefreshItemListView(ItemViewModel model)
        {
            ItemService service = new ItemService();
            service.AddItem(model);

            string[] items = new string[] 
            { 
                model.Name,
                model.Alias,
                model.Code.ToString(),
                EnumHelper.EnumToCaption(model.DataType),
                EnumHelper.EnumToCaption(model.TableName),
                model.Address,
                model.Length.ToString(),
                model.ScanPeriod.ToString()
            };

            ListViewItem lvi = new ListViewItem(items);
            lvi.ImageKey = "tag.bmp";
            lvi.Tag = model;

            this.itemListView.Items.Add(lvi);

            this.itemPropertyGrid.SelectedObject = model;
        }

        /// <summary>
        /// 刷新变量列表窗体
        /// </summary>
        public void RefreshItemListView(DeviceViewModel device)
        {
            if (device == null)
            {
                return;
            }

            foreach (ItemViewModel model in device.Items)
            {
                string[] items = new string[] 
                { 
                    model.Name,
                    model.Alias,
                    model.Code.ToString(),
                    EnumHelper.EnumToCaption(model.DataType),
                    EnumHelper.EnumToCaption(model.TableName),
                    model.Address,
                    model.Length.ToString(),
                    model.ScanPeriod.ToString()
                };

                ListViewItem lvi = new ListViewItem(items);
                lvi.ImageKey = "tag.bmp";
                lvi.Tag = model;

                this.itemListView.Items.Add(lvi);
            }

            if (this.itemListView.Items.Count > 0)
            {
                this.itemListView.Items[0].Selected = true;
                ItemViewModel model = this.itemListView.Items[0].Tag as ItemViewModel;
                this.itemPropertyGrid.SelectedObject = model;
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
                TreeNode parentNode = this.naviTreeView.SelectedNode;
                DeviceViewModel device = parentNode.Tag as DeviceViewModel;
                if (device == null)
                {
                    return;
                }

                ItemBatchAddForm frm = new ItemBatchAddForm(this, device);
                DialogResult dr = frm.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    List<ItemViewModel> models = frm.Models;
                    ItemService service = new ItemService();
                    foreach (ItemViewModel model in models)
                    {
                        service.AddItem(model);
                    }

                    this.RefreshItemListView(device);
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
                    DeviceViewModel device = model.Device;

                    ItemEditForm frm = new ItemEditForm(this, device, model);
                    DialogResult dr = frm.ShowDialog();
                    if (dr == DialogResult.OK)
                    {
                        ItemService service = new ItemService();
                        service.EditItem(model);

                        //更新变量列表的对应行
                        lvi.SubItems[0].Text = model.Name;
                        lvi.SubItems[1].Text = model.Alias;
                        lvi.SubItems[2].Text = model.Code.ToString();
                        lvi.SubItems[3].Text = EnumHelper.EnumToCaption(model.DataType);
                        lvi.SubItems[4].Text = EnumHelper.EnumToCaption(model.TableName);
                        lvi.SubItems[5].Text = model.Address;
                        lvi.SubItems[6].Text = model.Length.ToString();
                        lvi.SubItems[7].Text = model.ScanPeriod.ToString();

                        this.itemPropertyGrid.SelectedObject = model;
                    }
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
                        DialogResult dr = frm.ShowDialog();
                        if (dr == DialogResult.OK)
                        {
                            SerialPortService service = new SerialPortService();
                            service.EditSerialPort(model);
                        }
                    }
                    //设备节点
                    else if (tag.GetType() == typeof(DeviceViewModel))
                    {
                        DeviceViewModel model = node.Tag as DeviceViewModel;
                        ChannelBase channel = node.Parent.Tag as ChannelBase;

                        DeviceEditForm frm = new DeviceEditForm(this, channel, model);
                        DialogResult dr = frm.ShowDialog();
                        if (dr == DialogResult.OK)
                        {
                            DeviceService service = new DeviceService();
                            service.EditDevice(model);
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

                    ItemService service = new ItemService();
                    ListView.SelectedIndexCollection indexList = this.itemListView.SelectedIndices;
                    for (int i = indexList.Count; i > 0; i--)
                    {
                        int index = indexList[i - 1];
                        ListViewItem lvi = this.itemListView.Items[index];
                        ItemViewModel model = lvi.Tag as ItemViewModel;

                        service.DeleteItem(model.Id);
                        model.Device.Items.Remove(model);
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

                        SerialPortService service = new SerialPortService();
                        service.DeleteSerialPort(model.Id);

                        this._project.SerialPorts.Remove(model);
                    }
                    //设备节点
                    else if (tag.GetType() == typeof(DeviceViewModel))
                    {
                        if (MessageBox.Show("确定删除设备吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        {
                            return;
                        }

                        DeviceViewModel model = node.Tag as DeviceViewModel;

                        DeviceService service = new DeviceService();
                        service.DeleteDevice(model.Id);
                        
                        if (node.Level == 1)
                        {
                            this._project.Ethernet.Devices.Remove(model);
                        }
                        else if (node.Level == 2)
                        {
                            SerialPortViewModel parentModel = node.Parent.Tag as SerialPortViewModel;
                            parentModel.Devices.Remove(model);
                        }
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

                        this.tsmiAddSerialPort.Enabled = true;
                        this.tsmiAddDevice.Enabled = false;
                        this.tsmiAddItem.Enabled = false;
                        this.tsmiBatchAddItem.Enabled = false;
                        this.tsmiEdit.Enabled = false;
                        this.tsmiDelete.Enabled = false;
                    }
                    else
                    {
                        this.tsbAddSerialPort.Enabled = false;
                        this.tsbAddDevice.Enabled = true;
                        this.tsbAddItem.Enabled = false;
                        this.tsbBatchAddItem.Enabled = false;
                        this.tsbEdit.Enabled = false;
                        this.tsbDelete.Enabled = false;

                        this.tsmiAddSerialPort.Enabled = false;
                        this.tsmiAddDevice.Enabled = true;
                        this.tsmiAddItem.Enabled = false;
                        this.tsmiBatchAddItem.Enabled = false;
                        this.tsmiEdit.Enabled = false;
                        this.tsmiDelete.Enabled = false;
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

                        this.tsmiAddSerialPort.Enabled = true;
                        this.tsmiAddDevice.Enabled = true;
                        this.tsmiAddItem.Enabled = false;
                        this.tsmiBatchAddItem.Enabled = false;
                        this.tsmiEdit.Enabled = true;
                        this.tsmiDelete.Enabled = true;
                    }
                    else
                    {
                        this.tsbAddSerialPort.Enabled = false;
                        this.tsbAddDevice.Enabled = true;
                        this.tsbAddItem.Enabled = true;
                        this.tsbBatchAddItem.Enabled = true;
                        this.tsbEdit.Enabled = true;
                        this.tsbDelete.Enabled = true;

                        this.tsmiAddSerialPort.Enabled = false;
                        this.tsmiAddDevice.Enabled = true;
                        this.tsmiAddItem.Enabled = true;
                        this.tsmiBatchAddItem.Enabled = true;
                        this.tsmiEdit.Enabled = true;
                        this.tsmiDelete.Enabled = true;

                        DeviceViewModel device = node.Tag as DeviceViewModel;
                        this.RefreshItemListView(device);
                        return;
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

                    this.tsmiAddSerialPort.Enabled = false;
                    this.tsmiAddDevice.Enabled = false;
                    this.tsmiAddItem.Enabled = true;
                    this.tsmiBatchAddItem.Enabled = true;
                    this.tsmiEdit.Enabled = true;
                    this.tsmiDelete.Enabled = true;

                    DeviceViewModel device = node.Tag as DeviceViewModel;
                    this.RefreshItemListView(device);
                    return;
                }

                this.itemListView.Items.Clear();
                this.itemPropertyGrid.SelectedObject = null;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 选择变量对象
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void itemListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.itemListView.SelectedItems.Count == 0)
                {
                    this.itemPropertyGrid.SelectedObject = null;
                    return;
                }

                ListViewItem lvi = this.itemListView.SelectedItems[0];
                ItemViewModel model = lvi.Tag as ItemViewModel;

                this.itemPropertyGrid.SelectedObject = model;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Del键删除选中行
        /// </summary>
        private void itemListView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                this.tsmiDelete_Click(this, null);
            }
        }

        /// <summary>
        /// 鼠标双击事件编辑变量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void itemListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.tsmiEdit_Click(this, null);
        }
    }
}
