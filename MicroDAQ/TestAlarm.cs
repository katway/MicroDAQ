using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MicroDAQ
{
    public partial class TestAlarm : Form
    {
        private int initSlave = 200;
        private int alarmIndex = 0;

        private List<AlarmControl> alarms;
        public TestAlarm()
        {
            InitializeComponent();
            alarms = new List<AlarmControl>();
        }

        private AlarmControl AddAlarm(int slave, byte alertCode)
        {
            AlarmControl alarm = new AlarmControl(slave, alertCode);
            alarms.Add(alarm);
            this.Controls.Add(alarm);
            return alarm;
        }
        private void RemoveAlarm()
        {
            AlarmControl alarm = alarms[alarms.Count - 1];
            this.Controls.Remove(alarm);
            alarms.RemoveAt(alarms.Count - 1);
            alarm.Dispose();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AlarmControl alarm = AddAlarm(alarmIndex++ + initSlave, 0);
            alarm.Location = new Point(30, 40 + 29 * alarmIndex);

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            int slave;
            int.TryParse(this.textBox1.Text, out slave);
            this.initSlave = slave;
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {

            RemoveAlarm();
            alarmIndex--;
        }

        private void TestAlarm_Load(object sender, EventArgs e)
        {
            if (Program.RemoteCycle != null)
                Program.RemoteCycle.SetPause = true;
        }
        int runningNum = 0;

        int ctrlIndex = 0;
        private void tmrRemoteCtrl_Tick(object sender, EventArgs e)
        {
            if (this.alarms.Count > 0)
            {
                AlarmControl alarm = this.alarms[ctrlIndex % this.alarms.Count];
                ctrlIndex++;
                foreach (var mt in Program.MeterManager.CTMeters.Values)
                {
                    runningNum = ++runningNum % ushort.MaxValue;
                    mt.SetCommand(runningNum, alarm.Slave, 1, (int)alarm.AlertCode);
                }
            }
        }

        private void TestAlarm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Program.RemoteCycle != null)
                Program.RemoteCycle.SetPause = false;
        }

    }
}
