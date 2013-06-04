namespace MicroDAQ
{
    partial class DataDisplayForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lsvItems = new System.Windows.Forms.ListView();
            this.参数ID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.数据值1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.数据值2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.设备类型 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.状态 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.数据质量 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.dgvDB = new System.Windows.Forms.DataGridView();
            this.grpDB = new System.Windows.Forms.GroupBox();
            this.labDBState = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnInstant = new System.Windows.Forms.Button();
            this.btnRefreshDB = new System.Windows.Forms.Button();
            this.grpItem = new System.Windows.Forms.GroupBox();
            this.btnTestAlarm = new System.Windows.Forms.Button();
            this.labOPCState = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnRefreshData = new System.Windows.Forms.Button();
            this.bkwConnect = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDB)).BeginInit();
            this.grpDB.SuspendLayout();
            this.grpItem.SuspendLayout();
            this.SuspendLayout();
            // 
            // lsvItems
            // 
            this.lsvItems.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.参数ID,
            this.数据值1,
            this.数据值2,
            this.设备类型,
            this.状态,
            this.数据质量});
            this.lsvItems.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lsvItems.FullRowSelect = true;
            this.lsvItems.GridLines = true;
            this.lsvItems.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lsvItems.Location = new System.Drawing.Point(12, 12);
            this.lsvItems.MultiSelect = false;
            this.lsvItems.Name = "lsvItems";
            this.lsvItems.Size = new System.Drawing.Size(607, 255);
            this.lsvItems.TabIndex = 0;
            this.lsvItems.UseCompatibleStateImageBehavior = false;
            this.lsvItems.View = System.Windows.Forms.View.Details;
            // 
            // 参数ID
            // 
            this.参数ID.Text = "参数ID";
            this.参数ID.Width = 85;
            // 
            // 数据值1
            // 
            this.数据值1.Text = "数据值1";
            this.数据值1.Width = 100;
            // 
            // 数据值2
            // 
            this.数据值2.Text = "数据值2";
            this.数据值2.Width = 100;
            // 
            // 设备类型
            // 
            this.设备类型.Text = "设备类型";
            this.设备类型.Width = 120;
            // 
            // 状态
            // 
            this.状态.Text = "状态";
            this.状态.Width = 80;
            // 
            // 数据质量
            // 
            this.数据质量.Text = "可信度";
            this.数据质量.Width = 80;
            // 
            // dgvDB
            // 
            this.dgvDB.AllowUserToAddRows = false;
            this.dgvDB.AllowUserToDeleteRows = false;
            this.dgvDB.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgvDB.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvDB.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvDB.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            this.dgvDB.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvDB.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleVertical;
            this.dgvDB.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvDB.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDB.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvDB.Location = new System.Drawing.Point(12, 320);
            this.dgvDB.Name = "dgvDB";
            this.dgvDB.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvDB.RowHeadersVisible = false;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dgvDB.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvDB.RowTemplate.Height = 23;
            this.dgvDB.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDB.Size = new System.Drawing.Size(607, 255);
            this.dgvDB.TabIndex = 2;
            // 
            // grpDB
            // 
            this.grpDB.Controls.Add(this.labDBState);
            this.grpDB.Controls.Add(this.label3);
            this.grpDB.Controls.Add(this.btnInstant);
            this.grpDB.Controls.Add(this.btnRefreshDB);
            this.grpDB.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.grpDB.Location = new System.Drawing.Point(625, 320);
            this.grpDB.Name = "grpDB";
            this.grpDB.Size = new System.Drawing.Size(162, 255);
            this.grpDB.TabIndex = 27;
            this.grpDB.TabStop = false;
            this.grpDB.Text = "OPCMES数据参数";
            // 
            // labDBState
            // 
            this.labDBState.AutoSize = true;
            this.labDBState.BackColor = System.Drawing.Color.Silver;
            this.labDBState.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labDBState.Location = new System.Drawing.Point(30, 205);
            this.labDBState.Name = "labDBState";
            this.labDBState.Size = new System.Drawing.Size(65, 20);
            this.labDBState.TabIndex = 30;
            this.labDBState.Text = "状态未知";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(30, 172);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 20);
            this.label3.TabIndex = 29;
            this.label3.Text = "数据库通信状态";
            // 
            // btnInstant
            // 
            this.btnInstant.Location = new System.Drawing.Point(23, 99);
            this.btnInstant.Name = "btnInstant";
            this.btnInstant.Size = new System.Drawing.Size(107, 34);
            this.btnInstant.TabIndex = 28;
            this.btnInstant.Text = "即时数据";
            this.btnInstant.UseVisualStyleBackColor = true;
            this.btnInstant.Click += new System.EventHandler(this.btnInstant_Click);
            // 
            // btnRefreshDB
            // 
            this.btnRefreshDB.Location = new System.Drawing.Point(23, 49);
            this.btnRefreshDB.Name = "btnRefreshDB";
            this.btnRefreshDB.Size = new System.Drawing.Size(107, 34);
            this.btnRefreshDB.TabIndex = 27;
            this.btnRefreshDB.Text = "配置情况";
            this.btnRefreshDB.UseVisualStyleBackColor = true;
            this.btnRefreshDB.Click += new System.EventHandler(this.btnRefreshDB_Click);
            // 
            // grpItem
            // 
            this.grpItem.Controls.Add(this.btnTestAlarm);
            this.grpItem.Controls.Add(this.labOPCState);
            this.grpItem.Controls.Add(this.label1);
            this.grpItem.Controls.Add(this.btnRefreshData);
            this.grpItem.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.grpItem.Location = new System.Drawing.Point(625, 12);
            this.grpItem.Name = "grpItem";
            this.grpItem.Size = new System.Drawing.Size(161, 255);
            this.grpItem.TabIndex = 28;
            this.grpItem.TabStop = false;
            this.grpItem.Text = "PLC数据项";
            // 
            // btnTestAlarm
            // 
            this.btnTestAlarm.Location = new System.Drawing.Point(23, 204);
            this.btnTestAlarm.Name = "btnTestAlarm";
            this.btnTestAlarm.Size = new System.Drawing.Size(107, 34);
            this.btnTestAlarm.TabIndex = 27;
            this.btnTestAlarm.Text = "测试报警灯";
            this.btnTestAlarm.UseVisualStyleBackColor = true;
            this.btnTestAlarm.Click += new System.EventHandler(this.btnTestAlarm_Click);
            // 
            // labOPCState
            // 
            this.labOPCState.AutoSize = true;
            this.labOPCState.BackColor = System.Drawing.Color.Silver;
            this.labOPCState.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labOPCState.Location = new System.Drawing.Point(30, 145);
            this.labOPCState.Name = "labOPCState";
            this.labOPCState.Size = new System.Drawing.Size(65, 20);
            this.labOPCState.TabIndex = 26;
            this.labOPCState.Text = "状态未知";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(30, 115);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 20);
            this.label1.TabIndex = 25;
            this.label1.Text = "OPC通信状态";
            // 
            // btnRefreshData
            // 
            this.btnRefreshData.Location = new System.Drawing.Point(23, 35);
            this.btnRefreshData.Name = "btnRefreshData";
            this.btnRefreshData.Size = new System.Drawing.Size(107, 34);
            this.btnRefreshData.TabIndex = 24;
            this.btnRefreshData.Text = "刷新数据";
            this.btnRefreshData.UseVisualStyleBackColor = true;
            this.btnRefreshData.Click += new System.EventHandler(this.button1_Click);
            // 
            // DataDisplayForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(789, 587);
            this.Controls.Add(this.grpItem);
            this.Controls.Add(this.grpDB);
            this.Controls.Add(this.dgvDB);
            this.Controls.Add(this.lsvItems);
            this.Name = "DataDisplayForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "数据查询";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DataDisplayForm_FormClosing);
            this.Load += new System.EventHandler(this.DataDisplayForm_Load);
            this.SizeChanged += new System.EventHandler(this.DataDisplayForm_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDB)).EndInit();
            this.grpDB.ResumeLayout(false);
            this.grpDB.PerformLayout();
            this.grpItem.ResumeLayout(false);
            this.grpItem.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lsvItems;
        private System.Windows.Forms.ColumnHeader 参数ID;
        private System.Windows.Forms.ColumnHeader 设备类型;
        private System.Windows.Forms.ColumnHeader 状态;
        private System.Windows.Forms.ColumnHeader 数据值1;
        private System.Windows.Forms.ColumnHeader 数据值2;
        private System.Windows.Forms.DataGridView dgvDB;
        private System.Windows.Forms.ColumnHeader 数据质量;
        private System.Windows.Forms.GroupBox grpDB;
        private System.Windows.Forms.Label labDBState;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnInstant;
        private System.Windows.Forms.Button btnRefreshDB;
        private System.Windows.Forms.GroupBox grpItem;
        private System.Windows.Forms.Label labOPCState;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnRefreshData;
        private System.ComponentModel.BackgroundWorker bkwConnect;
        private System.Windows.Forms.Button btnTestAlarm;
    }
}