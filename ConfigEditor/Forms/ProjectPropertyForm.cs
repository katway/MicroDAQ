/**
 * 文件名：ProjectPropertyForm.cs
 * 说明：项目属性窗体
 * 作者：刘风彬
 * 更改记录： 
 * -------------------------------------------------------
 * 改动人 	时间 			原因
 * -------------------------------------------------------
 * 刘风彬 	2013-10-10		创建文件
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
using ConfigEditor.Core.ViewModels;

namespace ConfigEditor.Forms
{
    /// <summary>
    /// 项目属性窗体实现
    /// </summary>
    public partial class ProjectPropertyForm : Form
    {
        ProjectViewModel model;
        public ProjectPropertyForm(ProjectViewModel model)
        {
            InitializeComponent();
            this.model = model;    
        }

        private void ProjectLocation_TextChanged(object sender, EventArgs e)
        {
        }
      
        private void ProjectPropertyForm_Load(object sender, EventArgs e)
        {
            try
            {

                //显示项目所在位置路径
                string path = System.IO.Directory.GetCurrentDirectory().ToString();
                string db = System.Configuration.ConfigurationManager.AppSettings["PROJECT_FILE"].ToString();
                string file = path + "\\" + db;
                ProjectLocation.Text = file;

                //显示串口数量
                this.txtSerialNum.Text = model.SerialPorts.Count.ToString();

                //显示设备数量
                this.txtDeviceNum.Text = model.AllDevices.Count.ToString();

                //显示变量数
                int count = 0;
                foreach (DeviceViewModel device in model.AllDevices)
                {
                    foreach (ItemViewModel item in device.Items)
                    {
                        count++;
                    }
                }

                this.txtItemNum.Text = count.ToString();
            }
            catch
            {
                throw ;
            }
        }

        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;

        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

      
    }
}
