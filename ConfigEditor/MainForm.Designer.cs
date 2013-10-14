namespace ConfigEditor
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("串口");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("以太网");
            this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.文件FToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiProjectProperty = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiExit = new System.Windows.Forms.ToolStripMenuItem();
            this.编辑EToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAddSerialPort = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAddDevice = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAddItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiBatchAddItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiEnable = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.工具TToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiClearProject = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiUpdateEms = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.帮助HToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.mainToolStrip = new System.Windows.Forms.ToolStrip();
            this.tsbAddSerialPort = new System.Windows.Forms.ToolStripButton();
            this.tsbAddDevice = new System.Windows.Forms.ToolStripButton();
            this.tsbAddItem = new System.Windows.Forms.ToolStripButton();
            this.tsbBatchAddItem = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbEdit = new System.Windows.Forms.ToolStripButton();
            this.tsbDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbHelp = new System.Windows.Forms.ToolStripButton();
            this.mainStatusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.mainSplitContainer = new System.Windows.Forms.SplitContainer();
            this.naviTreeView = new System.Windows.Forms.TreeView();
            this.commonImageList = new System.Windows.Forms.ImageList(this.components);
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.itemListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.itemPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.cmsNavi = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.添加串口ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.添加设备ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.添加变量ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.批量添加变量ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.编辑ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsItem = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.编辑ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.删除ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenuStrip.SuspendLayout();
            this.mainToolStrip.SuspendLayout();
            this.mainStatusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).BeginInit();
            this.mainSplitContainer.Panel1.SuspendLayout();
            this.mainSplitContainer.Panel2.SuspendLayout();
            this.mainSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.cmsNavi.SuspendLayout();
            this.cmsItem.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenuStrip
            // 
            this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件FToolStripMenuItem,
            this.编辑EToolStripMenuItem,
            this.工具TToolStripMenuItem,
            this.帮助HToolStripMenuItem});
            this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mainMenuStrip.Name = "mainMenuStrip";
            this.mainMenuStrip.Size = new System.Drawing.Size(1008, 25);
            this.mainMenuStrip.TabIndex = 0;
            this.mainMenuStrip.Text = "menuStrip1";
            // 
            // 文件FToolStripMenuItem
            // 
            this.文件FToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiProjectProperty,
            this.toolStripSeparator2,
            this.tsmiExit});
            this.文件FToolStripMenuItem.Name = "文件FToolStripMenuItem";
            this.文件FToolStripMenuItem.Size = new System.Drawing.Size(58, 21);
            this.文件FToolStripMenuItem.Text = "文件(&F)";
            // 
            // tsmiProjectProperty
            // 
            this.tsmiProjectProperty.Name = "tsmiProjectProperty";
            this.tsmiProjectProperty.Size = new System.Drawing.Size(139, 22);
            this.tsmiProjectProperty.Text = "项目属性(&P)";
            this.tsmiProjectProperty.Click += new System.EventHandler(this.tsmiProjectProperty_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(136, 6);
            // 
            // tsmiExit
            // 
            this.tsmiExit.Name = "tsmiExit";
            this.tsmiExit.Size = new System.Drawing.Size(139, 22);
            this.tsmiExit.Text = "退出(&X)";
            this.tsmiExit.Click += new System.EventHandler(this.tsmiExit_Click);
            // 
            // 编辑EToolStripMenuItem
            // 
            this.编辑EToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiAddSerialPort,
            this.tsmiAddDevice,
            this.tsmiAddItem,
            this.tsmiBatchAddItem,
            this.toolStripSeparator3,
            this.tsmiEnable,
            this.toolStripSeparator1,
            this.tsmiEdit,
            this.tsmiDelete});
            this.编辑EToolStripMenuItem.Name = "编辑EToolStripMenuItem";
            this.编辑EToolStripMenuItem.Size = new System.Drawing.Size(59, 21);
            this.编辑EToolStripMenuItem.Text = "编辑(&E)";
            // 
            // tsmiAddSerialPort
            // 
            this.tsmiAddSerialPort.Image = global::ConfigEditor.Properties.Resources.new_port_16;
            this.tsmiAddSerialPort.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsmiAddSerialPort.Name = "tsmiAddSerialPort";
            this.tsmiAddSerialPort.Size = new System.Drawing.Size(164, 22);
            this.tsmiAddSerialPort.Text = "添加串口(&C)";
            this.tsmiAddSerialPort.Click += new System.EventHandler(this.tsmiAddSerialPort_Click);
            // 
            // tsmiAddDevice
            // 
            this.tsmiAddDevice.Image = global::ConfigEditor.Properties.Resources.new_device;
            this.tsmiAddDevice.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsmiAddDevice.Name = "tsmiAddDevice";
            this.tsmiAddDevice.Size = new System.Drawing.Size(164, 22);
            this.tsmiAddDevice.Text = "添加设备(&I)";
            this.tsmiAddDevice.Click += new System.EventHandler(this.tsmiAddDevice_Click);
            // 
            // tsmiAddItem
            // 
            this.tsmiAddItem.Image = global::ConfigEditor.Properties.Resources.new_tag;
            this.tsmiAddItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsmiAddItem.Name = "tsmiAddItem";
            this.tsmiAddItem.Size = new System.Drawing.Size(164, 22);
            this.tsmiAddItem.Text = "添加变量(&V)";
            this.tsmiAddItem.Click += new System.EventHandler(this.tsmiAddItem_Click);
            // 
            // tsmiBatchAddItem
            // 
            this.tsmiBatchAddItem.Image = global::ConfigEditor.Properties.Resources.new_group;
            this.tsmiBatchAddItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsmiBatchAddItem.Name = "tsmiBatchAddItem";
            this.tsmiBatchAddItem.Size = new System.Drawing.Size(164, 22);
            this.tsmiBatchAddItem.Text = "批量添加变量(&B)";
            this.tsmiBatchAddItem.Click += new System.EventHandler(this.tsmiBatchAddItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(161, 6);
            // 
            // tsmiEnable
            // 
            this.tsmiEnable.Name = "tsmiEnable";
            this.tsmiEnable.Size = new System.Drawing.Size(164, 22);
            this.tsmiEnable.Text = "启用/禁用(&N)";
            this.tsmiEnable.Click += new System.EventHandler(this.tsmiEnable_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(161, 6);
            // 
            // tsmiEdit
            // 
            this.tsmiEdit.Image = global::ConfigEditor.Properties.Resources.property;
            this.tsmiEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsmiEdit.Name = "tsmiEdit";
            this.tsmiEdit.Size = new System.Drawing.Size(164, 22);
            this.tsmiEdit.Text = "编辑(&E)";
            this.tsmiEdit.Click += new System.EventHandler(this.tsmiEdit_Click);
            // 
            // tsmiDelete
            // 
            this.tsmiDelete.Image = global::ConfigEditor.Properties.Resources.delete_16;
            this.tsmiDelete.Name = "tsmiDelete";
            this.tsmiDelete.Size = new System.Drawing.Size(164, 22);
            this.tsmiDelete.Text = "删除(&D)";
            this.tsmiDelete.Click += new System.EventHandler(this.tsmiDelete_Click);
            // 
            // 工具TToolStripMenuItem
            // 
            this.工具TToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiClearProject,
            this.tsmiUpdateEms,
            this.tsmiOptions});
            this.工具TToolStripMenuItem.Name = "工具TToolStripMenuItem";
            this.工具TToolStripMenuItem.Size = new System.Drawing.Size(59, 21);
            this.工具TToolStripMenuItem.Text = "工具(&T)";
            // 
            // tsmiClearProject
            // 
            this.tsmiClearProject.Name = "tsmiClearProject";
            this.tsmiClearProject.Size = new System.Drawing.Size(163, 22);
            this.tsmiClearProject.Text = "清空项目(&C)";
            this.tsmiClearProject.Click += new System.EventHandler(this.tsmiClearProject_Click);
            // 
            // tsmiUpdateEms
            // 
            this.tsmiUpdateEms.Name = "tsmiUpdateEms";
            this.tsmiUpdateEms.Size = new System.Drawing.Size(163, 22);
            this.tsmiUpdateEms.Text = "更新到 EMS (&U)";
            this.tsmiUpdateEms.ToolTipText = "更新变量到 EMS 系统";
            this.tsmiUpdateEms.Click += new System.EventHandler(this.tsmiUpdateEms_Click);
            // 
            // tsmiOptions
            // 
            this.tsmiOptions.Name = "tsmiOptions";
            this.tsmiOptions.Size = new System.Drawing.Size(163, 22);
            this.tsmiOptions.Text = "选项(&O)";
            this.tsmiOptions.Visible = false;
            this.tsmiOptions.Click += new System.EventHandler(this.tsmiOptions_Click);
            // 
            // 帮助HToolStripMenuItem
            // 
            this.帮助HToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiAbout});
            this.帮助HToolStripMenuItem.Name = "帮助HToolStripMenuItem";
            this.帮助HToolStripMenuItem.Size = new System.Drawing.Size(61, 21);
            this.帮助HToolStripMenuItem.Text = "帮助(&H)";
            // 
            // tsmiAbout
            // 
            this.tsmiAbout.Name = "tsmiAbout";
            this.tsmiAbout.Size = new System.Drawing.Size(125, 22);
            this.tsmiAbout.Text = "关于(&A)...";
            this.tsmiAbout.Click += new System.EventHandler(this.tsmiAbout_Click);
            // 
            // mainToolStrip
            // 
            this.mainToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbAddSerialPort,
            this.tsbAddDevice,
            this.tsbAddItem,
            this.tsbBatchAddItem,
            this.toolStripSeparator4,
            this.tsbEdit,
            this.tsbDelete,
            this.toolStripSeparator7,
            this.toolStripButton1,
            this.toolStripSeparator6,
            this.tsbHelp});
            this.mainToolStrip.Location = new System.Drawing.Point(0, 25);
            this.mainToolStrip.Name = "mainToolStrip";
            this.mainToolStrip.Size = new System.Drawing.Size(1008, 25);
            this.mainToolStrip.TabIndex = 1;
            this.mainToolStrip.Text = "toolStrip1";
            // 
            // tsbAddSerialPort
            // 
            this.tsbAddSerialPort.Image = global::ConfigEditor.Properties.Resources.new_port_16;
            this.tsbAddSerialPort.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAddSerialPort.Name = "tsbAddSerialPort";
            this.tsbAddSerialPort.Size = new System.Drawing.Size(76, 22);
            this.tsbAddSerialPort.Text = "添加串口";
            this.tsbAddSerialPort.ToolTipText = "添加串口";
            this.tsbAddSerialPort.Click += new System.EventHandler(this.tsmiAddSerialPort_Click);
            // 
            // tsbAddDevice
            // 
            this.tsbAddDevice.Image = ((System.Drawing.Image)(resources.GetObject("tsbAddDevice.Image")));
            this.tsbAddDevice.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAddDevice.Name = "tsbAddDevice";
            this.tsbAddDevice.Size = new System.Drawing.Size(76, 22);
            this.tsbAddDevice.Text = "添加设备";
            this.tsbAddDevice.ToolTipText = "添加设备";
            this.tsbAddDevice.Click += new System.EventHandler(this.tsmiAddDevice_Click);
            // 
            // tsbAddItem
            // 
            this.tsbAddItem.Image = ((System.Drawing.Image)(resources.GetObject("tsbAddItem.Image")));
            this.tsbAddItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAddItem.Name = "tsbAddItem";
            this.tsbAddItem.Size = new System.Drawing.Size(76, 22);
            this.tsbAddItem.Text = "添加变量";
            this.tsbAddItem.ToolTipText = "添加变量";
            this.tsbAddItem.Click += new System.EventHandler(this.tsmiAddItem_Click);
            // 
            // tsbBatchAddItem
            // 
            this.tsbBatchAddItem.Image = ((System.Drawing.Image)(resources.GetObject("tsbBatchAddItem.Image")));
            this.tsbBatchAddItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbBatchAddItem.Name = "tsbBatchAddItem";
            this.tsbBatchAddItem.Size = new System.Drawing.Size(76, 22);
            this.tsbBatchAddItem.Text = "批量添加";
            this.tsbBatchAddItem.ToolTipText = "批量添加变量";
            this.tsbBatchAddItem.Click += new System.EventHandler(this.tsmiBatchAddItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbEdit
            // 
            this.tsbEdit.Image = ((System.Drawing.Image)(resources.GetObject("tsbEdit.Image")));
            this.tsbEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbEdit.Name = "tsbEdit";
            this.tsbEdit.Size = new System.Drawing.Size(52, 22);
            this.tsbEdit.Text = "编辑";
            this.tsbEdit.ToolTipText = "编辑";
            this.tsbEdit.Click += new System.EventHandler(this.tsmiEdit_Click);
            // 
            // tsbDelete
            // 
            this.tsbDelete.Image = global::ConfigEditor.Properties.Resources.delete_16;
            this.tsbDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDelete.Name = "tsbDelete";
            this.tsbDelete.Size = new System.Drawing.Size(52, 22);
            this.tsbDelete.Text = "删除";
            this.tsbDelete.Click += new System.EventHandler(this.tsmiDelete_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Image = global::ConfigEditor.Properties.Resources.update_database;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(94, 22);
            this.toolStripButton1.Text = "更新到 EMS";
            this.toolStripButton1.ToolTipText = "更新变量到 EMS 系统";
            this.toolStripButton1.Click += new System.EventHandler(this.tsmiUpdateEms_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbHelp
            // 
            this.tsbHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbHelp.Image = ((System.Drawing.Image)(resources.GetObject("tsbHelp.Image")));
            this.tsbHelp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbHelp.Name = "tsbHelp";
            this.tsbHelp.Size = new System.Drawing.Size(23, 22);
            this.tsbHelp.Text = "关于";
            this.tsbHelp.ToolTipText = "关于";
            this.tsbHelp.Click += new System.EventHandler(this.tsmiAbout_Click);
            // 
            // mainStatusStrip
            // 
            this.mainStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.mainStatusStrip.Location = new System.Drawing.Point(0, 660);
            this.mainStatusStrip.Name = "mainStatusStrip";
            this.mainStatusStrip.Size = new System.Drawing.Size(1008, 22);
            this.mainStatusStrip.TabIndex = 2;
            this.mainStatusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(32, 17);
            this.toolStripStatusLabel1.Text = "就绪";
            // 
            // mainSplitContainer
            // 
            this.mainSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainSplitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.mainSplitContainer.Location = new System.Drawing.Point(0, 50);
            this.mainSplitContainer.Name = "mainSplitContainer";
            // 
            // mainSplitContainer.Panel1
            // 
            this.mainSplitContainer.Panel1.Controls.Add(this.naviTreeView);
            // 
            // mainSplitContainer.Panel2
            // 
            this.mainSplitContainer.Panel2.Controls.Add(this.splitContainer2);
            this.mainSplitContainer.Size = new System.Drawing.Size(1008, 610);
            this.mainSplitContainer.SplitterDistance = 220;
            this.mainSplitContainer.TabIndex = 3;
            // 
            // naviTreeView
            // 
            this.naviTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.naviTreeView.HideSelection = false;
            this.naviTreeView.ImageKey = "device.bmp";
            this.naviTreeView.ImageList = this.commonImageList;
            this.naviTreeView.Location = new System.Drawing.Point(0, 0);
            this.naviTreeView.Name = "naviTreeView";
            treeNode1.ImageKey = "port.png";
            treeNode1.Name = "节点0";
            treeNode1.SelectedImageKey = "port.png";
            treeNode1.Tag = "SerialPorts";
            treeNode1.Text = "串口";
            treeNode2.ImageKey = "ethernet.png";
            treeNode2.Name = "节点1";
            treeNode2.SelectedImageKey = "ethernet.png";
            treeNode2.Tag = "Ethernet";
            treeNode2.Text = "以太网";
            this.naviTreeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2});
            this.naviTreeView.SelectedImageKey = "device.bmp";
            this.naviTreeView.Size = new System.Drawing.Size(220, 610);
            this.naviTreeView.TabIndex = 0;
            this.naviTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.naviTreeView_AfterSelect);
            // 
            // commonImageList
            // 
            this.commonImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("commonImageList.ImageStream")));
            this.commonImageList.TransparentColor = System.Drawing.Color.Magenta;
            this.commonImageList.Images.SetKeyName(0, "channel.bmp");
            this.commonImageList.Images.SetKeyName(1, "delete_channel.bmp");
            this.commonImageList.Images.SetKeyName(2, "delete_tag.bmp");
            this.commonImageList.Images.SetKeyName(3, "device.bmp");
            this.commonImageList.Images.SetKeyName(4, "disable_device.bmp");
            this.commonImageList.Images.SetKeyName(5, "edit_tag.bmp");
            this.commonImageList.Images.SetKeyName(6, "group.bmp");
            this.commonImageList.Images.SetKeyName(7, "new_channel.bmp");
            this.commonImageList.Images.SetKeyName(8, "new_device.bmp");
            this.commonImageList.Images.SetKeyName(9, "new_group.bmp");
            this.commonImageList.Images.SetKeyName(10, "new_tag.bmp");
            this.commonImageList.Images.SetKeyName(11, "qc.bmp");
            this.commonImageList.Images.SetKeyName(12, "tag.bmp");
            this.commonImageList.Images.SetKeyName(13, "property.bmp");
            this.commonImageList.Images.SetKeyName(14, "information.png");
            this.commonImageList.Images.SetKeyName(15, "warning.png");
            this.commonImageList.Images.SetKeyName(16, "error.png");
            this.commonImageList.Images.SetKeyName(17, "view.png");
            this.commonImageList.Images.SetKeyName(18, "ethernet.png");
            this.commonImageList.Images.SetKeyName(19, "port.png");
            this.commonImageList.Images.SetKeyName(20, "disable_port.png");
            this.commonImageList.Images.SetKeyName(21, "disable_tag.png");
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.itemListView);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.itemPropertyGrid);
            this.splitContainer2.Size = new System.Drawing.Size(784, 610);
            this.splitContainer2.SplitterDistance = 574;
            this.splitContainer2.TabIndex = 0;
            // 
            // itemListView
            // 
            this.itemListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8});
            this.itemListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.itemListView.FullRowSelect = true;
            this.itemListView.Location = new System.Drawing.Point(0, 0);
            this.itemListView.Name = "itemListView";
            this.itemListView.Size = new System.Drawing.Size(574, 610);
            this.itemListView.SmallImageList = this.commonImageList;
            this.itemListView.StateImageList = this.commonImageList;
            this.itemListView.TabIndex = 0;
            this.itemListView.UseCompatibleStateImageBehavior = false;
            this.itemListView.View = System.Windows.Forms.View.Details;
            this.itemListView.SelectedIndexChanged += new System.EventHandler(this.itemListView_SelectedIndexChanged);
            this.itemListView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.itemListView_KeyDown);
            this.itemListView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.itemListView_MouseDoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "名称";
            this.columnHeader1.Width = 140;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "别名";
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "识别码";
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "数据类型";
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "功能区";
            this.columnHeader5.Width = 75;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "寄存器地址";
            this.columnHeader6.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader6.Width = 80;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "长度";
            this.columnHeader7.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader7.Width = 40;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "刷新周期(秒)";
            this.columnHeader8.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader8.Width = 85;
            // 
            // itemPropertyGrid
            // 
            this.itemPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.itemPropertyGrid.Location = new System.Drawing.Point(0, 0);
            this.itemPropertyGrid.Name = "itemPropertyGrid";
            this.itemPropertyGrid.Size = new System.Drawing.Size(206, 610);
            this.itemPropertyGrid.TabIndex = 0;
            this.itemPropertyGrid.ToolbarVisible = false;
            // 
            // cmsNavi
            // 
            this.cmsNavi.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.添加串口ToolStripMenuItem,
            this.添加设备ToolStripMenuItem,
            this.添加变量ToolStripMenuItem,
            this.批量添加变量ToolStripMenuItem,
            this.toolStripSeparator5,
            this.编辑ToolStripMenuItem,
            this.删除ToolStripMenuItem});
            this.cmsNavi.Name = "cmsNavi";
            this.cmsNavi.Size = new System.Drawing.Size(149, 142);
            // 
            // 添加串口ToolStripMenuItem
            // 
            this.添加串口ToolStripMenuItem.Name = "添加串口ToolStripMenuItem";
            this.添加串口ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.添加串口ToolStripMenuItem.Text = "添加串口";
            // 
            // 添加设备ToolStripMenuItem
            // 
            this.添加设备ToolStripMenuItem.Name = "添加设备ToolStripMenuItem";
            this.添加设备ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.添加设备ToolStripMenuItem.Text = "添加设备";
            // 
            // 添加变量ToolStripMenuItem
            // 
            this.添加变量ToolStripMenuItem.Name = "添加变量ToolStripMenuItem";
            this.添加变量ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.添加变量ToolStripMenuItem.Text = "添加变量";
            // 
            // 批量添加变量ToolStripMenuItem
            // 
            this.批量添加变量ToolStripMenuItem.Name = "批量添加变量ToolStripMenuItem";
            this.批量添加变量ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.批量添加变量ToolStripMenuItem.Text = "批量添加变量";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(145, 6);
            // 
            // 编辑ToolStripMenuItem
            // 
            this.编辑ToolStripMenuItem.Name = "编辑ToolStripMenuItem";
            this.编辑ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.编辑ToolStripMenuItem.Text = "编辑";
            // 
            // 删除ToolStripMenuItem
            // 
            this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
            this.删除ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.删除ToolStripMenuItem.Text = "删除";
            // 
            // cmsItem
            // 
            this.cmsItem.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.编辑ToolStripMenuItem1,
            this.删除ToolStripMenuItem1});
            this.cmsItem.Name = "cmsItem";
            this.cmsItem.Size = new System.Drawing.Size(101, 48);
            // 
            // 编辑ToolStripMenuItem1
            // 
            this.编辑ToolStripMenuItem1.Name = "编辑ToolStripMenuItem1";
            this.编辑ToolStripMenuItem1.Size = new System.Drawing.Size(100, 22);
            this.编辑ToolStripMenuItem1.Text = "编辑";
            // 
            // 删除ToolStripMenuItem1
            // 
            this.删除ToolStripMenuItem1.Name = "删除ToolStripMenuItem1";
            this.删除ToolStripMenuItem1.Size = new System.Drawing.Size(100, 22);
            this.删除ToolStripMenuItem1.Text = "删除";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 682);
            this.Controls.Add(this.mainSplitContainer);
            this.Controls.Add(this.mainStatusStrip);
            this.Controls.Add(this.mainToolStrip);
            this.Controls.Add(this.mainMenuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mainMenuStrip;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "配置工具";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.mainMenuStrip.ResumeLayout(false);
            this.mainMenuStrip.PerformLayout();
            this.mainToolStrip.ResumeLayout(false);
            this.mainToolStrip.PerformLayout();
            this.mainStatusStrip.ResumeLayout(false);
            this.mainStatusStrip.PerformLayout();
            this.mainSplitContainer.Panel1.ResumeLayout(false);
            this.mainSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).EndInit();
            this.mainSplitContainer.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.cmsNavi.ResumeLayout(false);
            this.cmsItem.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem 文件FToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem tsmiExit;
        private System.Windows.Forms.ToolStripMenuItem 工具TToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmiOptions;
        private System.Windows.Forms.ToolStripMenuItem 帮助HToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmiAbout;
        private System.Windows.Forms.ToolStrip mainToolStrip;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripButton tsbHelp;
        private System.Windows.Forms.StatusStrip mainStatusStrip;
        private System.Windows.Forms.SplitContainer mainSplitContainer;
        private System.Windows.Forms.TreeView naviTreeView;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.PropertyGrid itemPropertyGrid;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ListView itemListView;
        private System.Windows.Forms.ToolStripMenuItem tsmiProjectProperty;
        private System.Windows.Forms.ToolStripMenuItem 编辑EToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmiAddSerialPort;
        private System.Windows.Forms.ToolStripMenuItem tsmiAddDevice;
        private System.Windows.Forms.ToolStripMenuItem tsmiAddItem;
        private System.Windows.Forms.ToolStripMenuItem tsmiBatchAddItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem tsmiEdit;
        private System.Windows.Forms.ToolStripMenuItem tsmiDelete;
        private System.Windows.Forms.ToolStripMenuItem tsmiEnable;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem tsmiClearProject;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ImageList commonImageList;
        private System.Windows.Forms.ToolStripButton tsbAddSerialPort;
        private System.Windows.Forms.ToolStripButton tsbAddDevice;
        private System.Windows.Forms.ToolStripButton tsbAddItem;
        private System.Windows.Forms.ToolStripButton tsbBatchAddItem;
        private System.Windows.Forms.ToolStripButton tsbDelete;
        private System.Windows.Forms.ContextMenuStrip cmsNavi;
        private System.Windows.Forms.ToolStripMenuItem 添加串口ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 添加设备ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 添加变量ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 批量添加变量ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem 编辑ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cmsItem;
        private System.Windows.Forms.ToolStripMenuItem 编辑ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripButton tsbEdit;
        private System.Windows.Forms.ToolStripMenuItem tsmiUpdateEms;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
    }
}

