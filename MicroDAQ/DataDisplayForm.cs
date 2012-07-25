using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MicroDAQ
{
    public partial class DataDisplayForm : Form
    {
        public DataDisplayForm()
        {
            InitializeComponent();
        }

        private void DataDisplayForm_Load(object sender, EventArgs e)
        {
            ShowItems();
        }
        private void ShowItems()
        {

            int i = 0;
            foreach (var item in Program.M.Items)
            {
                DataItem meter = item as DataItem;
                i++;
                if (lsvItems.Items.Count < i)
                    lsvItems.Items.Add(new ListViewItem(new string[]{
                                            meter.ID.ToString()                                            ,
                                            meter.Value .ToString (),
                                            meter.Type.ToString (),
                                            meter.State .ToString ()}));
                else
                {
                    lsvItems.Items[i - 1].SubItems[0].Text = meter.ID.ToString();
                    lsvItems.Items[i - 1].SubItems[1].Text = meter.Value.ToString();
                    lsvItems.Items[i - 1].SubItems[3].Text = meter.Type.ToString();
                    lsvItems.Items[i - 1].SubItems[4].Text = meter.State.ToString(); 
                }
                //Console.WriteLine((item as Meter).ID);

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ShowItems();
        }
    }
}
