namespace ConfigEditor.Forms
{
    partial class SplashScreenForm
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
            this.prgLoad = new System.Windows.Forms.ProgressBar();
            this.lblShowTip = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // prgLoad
            // 
            this.prgLoad.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.prgLoad.Location = new System.Drawing.Point(0, 277);
            this.prgLoad.MarqueeAnimationSpeed = 50;
            this.prgLoad.Name = "prgLoad";
            this.prgLoad.Size = new System.Drawing.Size(400, 23);
            this.prgLoad.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.prgLoad.TabIndex = 0;
            // 
            // lblShowTip
            // 
            this.lblShowTip.AutoSize = true;
            this.lblShowTip.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblShowTip.ForeColor = System.Drawing.Color.Green;
            this.lblShowTip.Location = new System.Drawing.Point(0, 260);
            this.lblShowTip.Name = "lblShowTip";
            this.lblShowTip.Size = new System.Drawing.Size(55, 14);
            this.lblShowTip.TabIndex = 1;
            this.lblShowTip.Text = "正在启动";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = global::ConfigEditor.Properties.Resources.splash;
            this.pictureBox1.InitialImage = global::ConfigEditor.Properties.Resources.splash;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(400, 277);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // SplashScreenForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Azure;
            this.ClientSize = new System.Drawing.Size(400, 300);
            this.Controls.Add(this.lblShowTip);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.prgLoad);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SplashScreenForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "启动画面";
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.Transparent;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SplashForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar prgLoad;
        private System.Windows.Forms.Label lblShowTip;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}