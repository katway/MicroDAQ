namespace MicroDAQ
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.btnStart = new System.Windows.Forms.Button();
            this.pcb = new System.Windows.Forms.PictureBox();
            this.tmr = new System.Windows.Forms.Timer(this.components);
            this.btnPC = new System.Windows.Forms.Button();
            this.stspMain = new System.Windows.Forms.StatusStrip();
            this.tsslVersion = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslProject = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslMeters = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsddbPLC = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsslUpdate = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslRemote = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslDB = new System.Windows.Forms.ToolStripStatusLabel();
            this.ni = new System.Windows.Forms.NotifyIcon(this.components);
            this.cmsNI = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.退出EToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.splitter2 = new System.Windows.Forms.Splitter();
            ((System.ComponentModel.ISupportInitialize)(this.pcb)).BeginInit();
            this.stspMain.SuspendLayout();
            this.cmsNI.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(345, 149);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(79, 33);
            this.btnStart.TabIndex = 2;
            this.btnStart.Text = "启动";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // pcb
            // 
            this.pcb.Image = global::MicroDAQ.Properties.Resources.bkg;
            this.pcb.Location = new System.Drawing.Point(12, 12);
            this.pcb.Name = "pcb";
            this.pcb.Size = new System.Drawing.Size(316, 240);
            this.pcb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pcb.TabIndex = 3;
            this.pcb.TabStop = false;
            // 
            // btnPC
            // 
            this.btnPC.Enabled = false;
            this.btnPC.Location = new System.Drawing.Point(345, 202);
            this.btnPC.Name = "btnPC";
            this.btnPC.Size = new System.Drawing.Size(79, 33);
            this.btnPC.TabIndex = 5;
            this.btnPC.Text = "暂停";
            this.btnPC.UseVisualStyleBackColor = true;
            this.btnPC.Click += new System.EventHandler(this.btnPC_Click);
            // 
            // stspMain
            // 
            this.stspMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslVersion,
            this.tsslProject,
            this.tsslMeters,
            this.tsddbPLC,
            this.tsslUpdate,
            this.tsslRemote,
            this.tsslDB});
            this.stspMain.Location = new System.Drawing.Point(0, 253);
            this.stspMain.Name = "stspMain";
            this.stspMain.Size = new System.Drawing.Size(476, 26);
            this.stspMain.TabIndex = 6;
            this.stspMain.Text = "statusStrip1";
            // 
            // tsslVersion
            // 
            this.tsslVersion.AutoSize = false;
            this.tsslVersion.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.tsslVersion.Name = "tsslVersion";
            this.tsslVersion.Size = new System.Drawing.Size(120, 21);
            this.tsslVersion.Text = "程序版本：";
            this.tsslVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tsslProject
            // 
            this.tsslProject.AutoSize = false;
            this.tsslProject.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.tsslProject.Name = "tsslProject";
            this.tsslProject.Size = new System.Drawing.Size(110, 21);
            this.tsslProject.Text = "项目代码：";
            this.tsslProject.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tsslMeters
            // 
            this.tsslMeters.DoubleClickEnabled = true;
            this.tsslMeters.Name = "tsslMeters";
            this.tsslMeters.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.tsslMeters.Size = new System.Drawing.Size(56, 21);
            this.tsslMeters.Text = "采集点：";
            this.tsslMeters.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tsslMeters.Click += new System.EventHandler(this.tsslMeters_Click);
            // 
            // tsddbPLC
            // 
            this.tsddbPLC.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsddbPLC.Image = ((System.Drawing.Image)(resources.GetObject("tsddbPLC.Image")));
            this.tsddbPLC.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsddbPLC.Name = "tsddbPLC";
            this.tsddbPLC.Size = new System.Drawing.Size(29, 24);
            this.tsddbPLC.Text = "PLC中声明的监测点数量";
            // 
            // tsslUpdate
            // 
            this.tsslUpdate.AutoSize = false;
            this.tsslUpdate.BackColor = System.Drawing.SystemColors.Control;
            this.tsslUpdate.Name = "tsslUpdate";
            this.tsslUpdate.Size = new System.Drawing.Size(15, 21);
            this.tsslUpdate.Text = "S";
            // 
            // tsslRemote
            // 
            this.tsslRemote.AutoSize = false;
            this.tsslRemote.BackColor = System.Drawing.SystemColors.Control;
            this.tsslRemote.Name = "tsslRemote";
            this.tsslRemote.Size = new System.Drawing.Size(15, 21);
            this.tsslRemote.Text = "S";
            // 
            // tsslDB
            // 
            this.tsslDB.BackColor = System.Drawing.Color.LimeGreen;
            this.tsslDB.Name = "tsslDB";
            this.tsslDB.Size = new System.Drawing.Size(43, 21);
            this.tsslDB.Text = "tssldb";
            this.tsslDB.Visible = false;
            // 
            // ni
            // 
            this.ni.ContextMenuStrip = this.cmsNI;
            this.ni.Text = "notifyIcon1";
            this.ni.Visible = true;
            this.ni.DoubleClick += new System.EventHandler(this.ni_DoubleClick);
            // 
            // cmsNI
            // 
            this.cmsNI.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.退出EToolStripMenuItem});
            this.cmsNI.Name = "cmsNI";
            this.cmsNI.Size = new System.Drawing.Size(117, 26);
            // 
            // 退出EToolStripMenuItem
            // 
            this.退出EToolStripMenuItem.Name = "退出EToolStripMenuItem";
            this.退出EToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.退出EToolStripMenuItem.Text = "退出(&X)";
            this.退出EToolStripMenuItem.Click += new System.EventHandler(this.退出EToolStripMenuItem_Click);
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(0, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 253);
            this.splitter1.TabIndex = 8;
            this.splitter1.TabStop = false;
            // 
            // splitter2
            // 
            this.splitter2.Location = new System.Drawing.Point(3, 0);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(3, 253);
            this.splitter2.TabIndex = 9;
            this.splitter2.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(476, 279);
            this.Controls.Add(this.splitter2);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.stspMain);
            this.Controls.Add(this.btnPC);
            this.Controls.Add(this.pcb);
            this.Controls.Add(this.btnStart);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "数据采集系统";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form2_FormClosing);
            this.Load += new System.EventHandler(this.Form2_Load);
            this.SizeChanged += new System.EventHandler(this.MainForm_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.pcb)).EndInit();
            this.stspMain.ResumeLayout(false);
            this.stspMain.PerformLayout();
            this.cmsNI.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.PictureBox pcb;
        
        private System.Windows.Forms.Timer tmr;
        private System.Windows.Forms.Button btnPC;
        private System.Windows.Forms.StatusStrip stspMain;
        private System.Windows.Forms.ToolStripStatusLabel tsslProject;
        private System.Windows.Forms.ToolStripStatusLabel tsslVersion;
        private System.Windows.Forms.ToolStripStatusLabel tsslMeters;
        private System.Windows.Forms.ToolStripStatusLabel tsslUpdate;
        private System.Windows.Forms.NotifyIcon ni;
        private System.Windows.Forms.ContextMenuStrip cmsNI;
        private System.Windows.Forms.ToolStripMenuItem 退出EToolStripMenuItem;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.ToolStripStatusLabel tsslRemote;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.ToolStripDropDownButton tsddbPLC;
        private System.Windows.Forms.ToolStripStatusLabel tsslDB;
    }
}