using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using MicroDAQ.Specifical;

namespace MicroDAQ.UI
{
    public partial class AlarmControl : UserControl
    {
        public AlarmControl()
        {
            InitializeComponent();
            Slave = 200;
        }

        public AlarmControl(int slave, byte alertCode)
            : this()
        {
            Slave = slave;
            AlertCode = (AlertCode)alertCode;
        }

        void AlarmRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rd = null;
            if (sender is RadioButton)
                rd = sender as RadioButton;
            if (rd.Equals(this.rdoBuzzRed)) AlertCode = AlertCode.BuzzRed;
            if (rd.Equals(this.rdoRed)) AlertCode = AlertCode.Red;
            if (rd.Equals(this.rdoYellow)) AlertCode = AlertCode.Yellow;
            if (rd.Equals(this.rdoGreen)) AlertCode = AlertCode.Green;
        }


        public int Slave { get; set; }
        public AlertCode AlertCode { get; private set; }



        private void AlarmControl_Load(object sender, EventArgs e)
        {
            this.mtxtSlave.Text = this.Slave.ToString();
            this.rdoBuzzRed.CheckedChanged += new EventHandler(AlarmRadioButton_CheckedChanged);
            this.rdoRed.CheckedChanged += new EventHandler(AlarmRadioButton_CheckedChanged);
            this.rdoYellow.CheckedChanged += new EventHandler(AlarmRadioButton_CheckedChanged);
            this.rdoGreen.CheckedChanged += new EventHandler(AlarmRadioButton_CheckedChanged);

        }

        private void mtxtSlave_TextChanged(object sender, EventArgs e)
        {
            int slave;
            int.TryParse(mtxtSlave.Text, out slave);
            this.Slave = slave;
        }

        public bool Highlight
        {
            set
            {
                this.highlight = value;
                if (value == true)
                    this.mtxtSlave.BackColor = Color.DeepSkyBlue;
                if (value == false)
                    this.mtxtSlave.BackColor = SystemColors.Window;
            }
            get
            { return highlight; }
        }
        private bool highlight;
    }
}
