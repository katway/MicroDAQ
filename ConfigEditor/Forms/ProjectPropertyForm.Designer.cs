namespace ConfigEditor.Forms
{
    partial class ProjectPropertyForm
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.ProjectLocation = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtItemNum = new System.Windows.Forms.TextBox();
            this.txtDeviceNum = new System.Windows.Forms.TextBox();
            this.txtSerialNum = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(333, 265);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click_1);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(185, 265);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "确定";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // ProjectLocation
            // 
            this.ProjectLocation.Location = new System.Drawing.Point(108, 25);
            this.ProjectLocation.Name = "ProjectLocation";
            this.ProjectLocation.Size = new System.Drawing.Size(300, 21);
            this.ProjectLocation.TabIndex = 1;
            this.ProjectLocation.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(36, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "项目位置：";
            // 
            // groupBox3
            // 
            this.groupBox3.Location = new System.Drawing.Point(23, 235);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(403, 10);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtItemNum);
            this.groupBox2.Controls.Add(this.txtDeviceNum);
            this.groupBox2.Controls.Add(this.txtSerialNum);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(38, 58);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(370, 171);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "项目统计";
            // 
            // txtItemNum
            // 
            this.txtItemNum.Location = new System.Drawing.Point(104, 109);
            this.txtItemNum.Name = "txtItemNum";
            this.txtItemNum.Size = new System.Drawing.Size(163, 21);
            this.txtItemNum.TabIndex = 5;
            this.txtItemNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtDeviceNum
            // 
            this.txtDeviceNum.Location = new System.Drawing.Point(104, 77);
            this.txtDeviceNum.Name = "txtDeviceNum";
            this.txtDeviceNum.Size = new System.Drawing.Size(163, 21);
            this.txtDeviceNum.TabIndex = 3;
            this.txtDeviceNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtSerialNum
            // 
            this.txtSerialNum.Location = new System.Drawing.Point(104, 38);
            this.txtSerialNum.Name = "txtSerialNum";
            this.txtSerialNum.Size = new System.Drawing.Size(163, 21);
            this.txtSerialNum.TabIndex = 1;
            this.txtSerialNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 112);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "变量数：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 77);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "设备数：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "串口数：";
            // 
            // ProjectPropertyForm
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(441, 300);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.ProjectLocation);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProjectPropertyForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "项目属性";
            this.Load += new System.EventHandler(this.ProjectPropertyForm_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.TextBox ProjectLocation;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtItemNum;
        private System.Windows.Forms.TextBox txtDeviceNum;
        private System.Windows.Forms.TextBox txtSerialNum;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;



    }
}