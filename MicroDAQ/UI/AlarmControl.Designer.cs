namespace MicroDAQ.UI
{
    partial class AlarmControl
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.mtxtSlave = new System.Windows.Forms.MaskedTextBox();
            this.rdoBuzzRed = new System.Windows.Forms.RadioButton();
            this.rdoRed = new System.Windows.Forms.RadioButton();
            this.rdoYellow = new System.Windows.Forms.RadioButton();
            this.rdoGreen = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // mtxtSlave
            // 
            this.mtxtSlave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.mtxtSlave.Location = new System.Drawing.Point(6, 2);
            this.mtxtSlave.Mask = "000";
            this.mtxtSlave.Name = "mtxtSlave";
            this.mtxtSlave.Size = new System.Drawing.Size(49, 21);
            this.mtxtSlave.TabIndex = 0;
            this.mtxtSlave.TextChanged += new System.EventHandler(this.mtxtSlave_TextChanged);
            // 
            // rdoBuzzRed
            // 
            this.rdoBuzzRed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.rdoBuzzRed.AutoSize = true;
            this.rdoBuzzRed.Location = new System.Drawing.Point(83, 5);
            this.rdoBuzzRed.Name = "rdoBuzzRed";
            this.rdoBuzzRed.Size = new System.Drawing.Size(65, 16);
            this.rdoBuzzRed.TabIndex = 1;
            this.rdoBuzzRed.Text = "红+蜂鸣";
            this.rdoBuzzRed.UseVisualStyleBackColor = true;
            // 
            // rdoRed
            // 
            this.rdoRed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.rdoRed.AutoSize = true;
            this.rdoRed.Location = new System.Drawing.Point(171, 5);
            this.rdoRed.Name = "rdoRed";
            this.rdoRed.Size = new System.Drawing.Size(35, 16);
            this.rdoRed.TabIndex = 2;
            this.rdoRed.Text = "红";
            this.rdoRed.UseVisualStyleBackColor = true;
            // 
            // rdoYellow
            // 
            this.rdoYellow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.rdoYellow.AutoSize = true;
            this.rdoYellow.Location = new System.Drawing.Point(231, 5);
            this.rdoYellow.Name = "rdoYellow";
            this.rdoYellow.Size = new System.Drawing.Size(35, 16);
            this.rdoYellow.TabIndex = 3;
            this.rdoYellow.Text = "黄";
            this.rdoYellow.UseVisualStyleBackColor = true;
            // 
            // rdoGreen
            // 
            this.rdoGreen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.rdoGreen.AutoSize = true;
            this.rdoGreen.Location = new System.Drawing.Point(289, 5);
            this.rdoGreen.Name = "rdoGreen";
            this.rdoGreen.Size = new System.Drawing.Size(35, 16);
            this.rdoGreen.TabIndex = 4;
            this.rdoGreen.Text = "绿";
            this.rdoGreen.UseVisualStyleBackColor = true;
            // 
            // AlarmControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.rdoGreen);
            this.Controls.Add(this.rdoYellow);
            this.Controls.Add(this.rdoRed);
            this.Controls.Add(this.rdoBuzzRed);
            this.Controls.Add(this.mtxtSlave);
            this.MaximumSize = new System.Drawing.Size(360, 25);
            this.MinimumSize = new System.Drawing.Size(360, 25);
            this.Name = "AlarmControl";
            this.Size = new System.Drawing.Size(360, 25);
            this.Load += new System.EventHandler(this.AlarmControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MaskedTextBox mtxtSlave;
        private System.Windows.Forms.RadioButton rdoBuzzRed;
        private System.Windows.Forms.RadioButton rdoRed;
        private System.Windows.Forms.RadioButton rdoYellow;
        private System.Windows.Forms.RadioButton rdoGreen;
    }
}
