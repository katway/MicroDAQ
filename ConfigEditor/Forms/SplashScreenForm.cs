/**
 * 文件名：SplashScreenForm.cs
 * 说明：启动画面
 * 作者：林安城
 * 更改记录： 
 * -------------------------------------------------------
 * 改动人 	时间 			原因
 * -------------------------------------------------------
 * 林安城 	2013-10-10		创建文件
 * -------------------------------------------------------
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ConfigEditor.Forms
{
    /// <summary>
    /// 启动画面
    /// </summary>
    public partial class SplashScreenForm : Form
    {
        delegate void StringParameterDelegate(string Text);
        delegate void StringParameterWithStatusDelegate(string Text, TypeOfMessage tom);
        delegate void SplashShowCloseDelegate();

        /// <summary>
        /// To ensure splash screen is closed using the API and not by keyboard or any other things
        /// </summary>
        bool CloseSplashScreenFlag = false;

        /// <summary>
        /// Base constructor
        /// </summary>
        public SplashScreenForm()
        {
            InitializeComponent();
            this.lblShowTip.Parent = this.pictureBox1;
            this.lblShowTip.BackColor = Color.Transparent;
            lblShowTip.ForeColor = Color.Green;

            prgLoad.Show();
        }

        /// <summary>
        /// Displays the splashscreen
        /// </summary>
        public void ShowSplashScreen()
        {
            if (InvokeRequired)
            {
                // We're not in the UI thread, so we need to call BeginInvoke
                BeginInvoke(new SplashShowCloseDelegate(ShowSplashScreen));
                return;
            }
            this.Show();
            Application.Run(this);
        }

        /// <summary>
        /// Closes the SplashScreen
        /// </summary>
        public void CloseSplashScreen()
        {
            if (InvokeRequired)
            {
                // We're not in the UI thread, so we need to call BeginInvoke
                BeginInvoke(new SplashShowCloseDelegate(CloseSplashScreen));
                return;
            }
            CloseSplashScreenFlag = true;
            this.Close();
        }

        /// <summary>
        /// Update text in default green color of success message
        /// </summary>
        /// <param name="Text">Message</param>
        public void UdpateStatusText(string Text)
        {
            if (InvokeRequired)
            {
                // We're not in the UI thread, so we need to call BeginInvoke
                BeginInvoke(new StringParameterDelegate(UdpateStatusText), new object[] { Text });
                return;
            }
            // Must be on the UI thread if we've got this far
            lblShowTip.ForeColor = Color.Green;
            lblShowTip.Text = Text;
        }


        /// <summary>
        /// Update text with message color defined as green/yellow/red/ for success/warning/failure
        /// </summary>
        /// <param name="Text">Message</param>
        /// <param name="tom">Type of Message</param>
        public void UdpateStatusTextWithStatus(string Text, TypeOfMessage tom)
        {
            if (InvokeRequired)
            {
                // We're not in the UI thread, so we need to call BeginInvoke
                BeginInvoke(new StringParameterWithStatusDelegate(UdpateStatusTextWithStatus), new object[] { Text, tom });
                return;
            }
            // Must be on the UI thread if we've got this far
            switch (tom)
            {
                case TypeOfMessage.Error:
                    lblShowTip.ForeColor = Color.Red;
                    break;
                case TypeOfMessage.Warning:
                    lblShowTip.ForeColor = Color.Yellow;
                    break;
                case TypeOfMessage.Success:
                    lblShowTip.ForeColor = Color.Green;
                    break;
            }
            lblShowTip.Text = Text;

        }

        /// <summary>
        /// Prevents the closing of form other than by calling the CloseSplashScreen function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SplashForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CloseSplashScreenFlag == false)
                e.Cancel = true;
        }
    }

    /// <summary>
    /// Defined types of messages: Success/Warning/Error.
    /// </summary>
    public enum TypeOfMessage
    {
        Success,
        Warning,
        Error,
    }
    /// <summary>
    /// Initiate instance of SplashScreen
    /// </summary>
    public static class SplashScreen
    {
        static SplashScreenForm sf = null;

        /// <summary>
        /// Displays the splashscreen
        /// </summary>
        public static void ShowSplashScreen()
        {
            if (sf == null)
            {
                sf = new SplashScreenForm();
                sf.ShowSplashScreen();
            }
        }

        /// <summary>
        /// Closes the SplashScreen
        /// </summary>
        public static void CloseSplashScreen()
        {
            if (sf != null)
            {
                sf.CloseSplashScreen();
                sf = null;
            }
        }

        /// <summary>
        /// Update text in default green color of success message
        /// </summary>
        /// <param name="Text">Message</param>
        public static void UdpateStatusText(string Text)
        {
            if (sf != null)
                sf.UdpateStatusText(Text);

        }

        /// <summary>
        /// Update text with message color defined as green/yellow/red/ for success/warning/failure
        /// </summary>
        /// <param name="Text">Message</param>
        /// <param name="tom">Type of Message</param>
        public static void UdpateStatusTextWithStatus(string Text, TypeOfMessage tom)
        {

            if (sf != null)
                sf.UdpateStatusTextWithStatus(Text, tom);
        }
    }

}
