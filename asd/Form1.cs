using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace asd
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] arr = new string[] {"a1","b","c","d"}; 
            List<string> b=new List<string>();
            foreach(string a in arr)
            {              
                b.Add(a);               
            }
            this.listBox1.DataSource = b;          
            //for (int i = 0; i < b.Count; i++)
            //{
            //    this.textBox1.Text = b[0].ToString();
            //}
            

        }
    }
}
