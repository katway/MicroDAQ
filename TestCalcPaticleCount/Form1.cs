using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestCalcPaticleCount
{
    
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Particle = new Dictionary<DateTime, Particle>();
            ParticleCount = new Particle();
        }

        public Dictionary<DateTime, Particle> Particle;
        public Particle ParticleCount;

        private DateTime CutOffMinute(DateTime dt)
        {
            return new DateTime(dt.Ticks - (dt.Ticks % TimeSpan.TicksPerMinute), dt.Kind);
        }

        public void CalcPaticleCount(DateTime time)
        {
            DateTime minuteTick = CutOffMinute(time);
            //if (this.Type == MeterType.尘埃粒子)
            //{
            //lock (Particle)
            //{
            //// DateTime minuteTick = CutOffMinute(DateTime.Now);
            //if (Particle.ContainsKey(minuteTick))
            //{

            //}
            //else
            //{
            //    Particle.Add(minuteTick, new Particle(minuteTick, Value1, Value2, Value3));
            //}
            List<DateTime> overdue = new List<DateTime>();
            //ParticleCount.Clear();
            foreach (var p in Particle)
            {
                if (minuteTick - p.Key > TimeSpan.FromMinutes(35))
                {
                    overdue.Add(p.Key);
                }
                //TimeSpan x = minuteTick - p.Key;
                //TimeSpan y = TimeSpan.FromMinutes(35);
            }

            foreach (var o in overdue)
            {
                if (Particle.ContainsKey(o))
                    Particle.Remove(o);
            }
            Console.Write(minuteTick.ToString());
            lock (Particle)
            {
                if (Particle.ContainsKey(minuteTick))
                {
                    Particle[minuteTick].Value1 = Value1;
                    Particle[minuteTick].Value2 = Value2;
                }
                else
                {
                    Particle.Add(minuteTick, new Particle(minuteTick, Value1, Value2, Value3));
                }
            }
            ParticleCount.Clear();
            foreach (var p in Particle)
            {
                //if (minuteTick - p.Key < TimeSpan.FromMinutes(35))
                //{
                ParticleCount.Value1 += p.Value.Value1;
                ParticleCount.Value2 += p.Value.Value2;
                //ParticleCount.Value3 += Value3;
                //}
            }
        }

        public float Value1 = 35f;
        public float Value2 { get; private set; }
        public float Value3 { get; private set; }
        DateTime time = DateTime.Now;

        private void button1_Click(object sender, EventArgs e)
        {
            time = time + TimeSpan.FromMinutes(1);
            Value1++;
            Value2 = 2;
            Value3 = 3;

            CalcPaticleCount(time);
            Console.Write("\t");
            Console.Write(ParticleCount.ToString());

            //int sum = 0;
            //int val = Convert.ToInt32( Value1 - 35);
            //for (int i = val; i <= Value1; i++)
            //{
            //    sum += i;
            //}

            //Console.Write("\t");

            //Console.Write(sum);
            Console.WriteLine();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            int sum = 0;
            int val = Convert.ToInt32(Value1 - 35);
            for (int i = val; i <= Value1; i++)
            {
                sum += i;
            }

            Console.Write("\t");

            Console.Write(sum);
        }
    }
    public class Particle
    {
        public Particle()
        {
            Value1 = Value2 = Value3 = 0;
        }
        public Particle(DateTime time, float value1, float value2, float value3)
        {
            Time = time;
            Value1 = value1;
            Value2 = value2;
            Value3 = value3;
        }
        public DateTime Time;
        public float Value1;
        public float Value2;
        public float Value3;
        public void Clear()
        {
            Value1 = Value2 = Value3 = 0;
        }
        public override string ToString()
        {
            return string.Format("{0},{1},{2}", Value1, Value2, Value3);
        }

    }
}
