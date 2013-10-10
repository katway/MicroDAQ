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
    public partial class ProjectPropertyForm : Form
    {
        ProjectViewModel model;
        public ProjectPropertyForm()
        {
            InitializeComponent();

         
        }

        private void ProjectLocation_TextChanged(object sender, EventArgs e)
        {


        }

        private void ProjectPropertyForm_Load(object sender, EventArgs e)
        {
            ProjectLocation.Text = "G:\\Work\\Github\\MicroDAQ\\ConfigEditor\\bin\\Debug";
            model = ConfigEditor.Core.IO.ProjectReader.Read();
            this.txtSerialNum.Text = model.SerialPorts.Count.ToString();
            this.txtDeviceNum.Text = model.AllDevices.Count.ToString();

            int count=0;
            foreach (DeviceViewModel device in model.AllDevices)
            {
                foreach (ItemViewModel item in device.Items)
                {
                    count++;
                }
            }

            this.txtItemNum.Text = count.ToString();
        }

        private void Ok_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

      
    }
}
