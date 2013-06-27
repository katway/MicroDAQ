using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace a000000
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ArrayList lib = new ArrayList();
            lib.Add("张三");
            lib.Add("李四");
            lib.Add("王五");
            lib.Add("大忽悠");
            lib.Add("老陈"); 
            foreach(string a in lib)
            {
                this.label1.Text = a;
                //Console.WriteLine(a);
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
              //int[] fibarray = new int[] { 0, 1, 2, 3, 5, 6 };
              //foreach (int i in fibarray)
              //{
              //   // Console.WriteLine(i);
              //    this.label1.Text = i.ToString();
              //    Console.ReadLine();
              //}
             
        }
    }
}
