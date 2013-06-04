using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace MicroDAQ
{
    public enum DataType
    {
        温度表 = 1,
        湿度表 = 2,
        压差表 = 4,
        风速仪 = 5,
        压力表 = 8,
        电导率 = 9,
        总有机碳 = 10,
        流量表 = 16,
        尘埃粒子 = 32,
        浮游菌 = 64,
        真空泵 = 65,
        设备状态 = 66,
        灭菌时间累加 = 67,
        F0值 = 68,
        灭菌时间 = 69,
        脉动次数 = 70,
        干燥时间 = 71,
        干燥间隔 = 72,
        脉动上限 = 73,
        脉动下限 = 74,
        压力回差 = 75,
        干燥回压 = 76,
        补气间隔 = 77,
        前门关到位 = 78,
        后门关到位 = 79,
        真空泵过热 = 80,
        夹层进气阀 = 81,
        内层进气阀 = 82,
        内层真空阀 = 83,
        内层补气阀 = 84,
        内层排气阀 = 85,
        运行灯 = 86,
        结束报警灯 = 87,
        注水阀 = 88,
        真空度 = 89,
        速度 = 90,
        时间间隔 = 91,
        生产总量 = 92,
        运行时间小时数 = 93,
        当前装量号 = 94,
        产量 = 95,
        运行时间分钟数 = 96,
        设备 = 128
    }
    public enum RunningState
    {
        已读取 = 1,
        执行中 = 2,
        完成 = 4,
        错误 = 8,
        未完成 = 16
    }
    public enum DataState
    {
        正常 = 1,
        仪表故障 = 2,
        仪表掉线 = 4,
        已启动 = 8,
        已停止 = 16
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

    }

    public class Meter : JonLibrary.OPC.Machine
    {

        public Meter(string Name, string[] Ctrl, string[] State)
        {
            this.Name = Name;
            ItemCtrl = Ctrl;
            ItemStatus = State;
            Particle = new Dictionary<DateTime, Particle>();
            ParticleCount = new Particle();
        }

        protected override void PLC_DataChange(string groupName, int[] item, object[] value, short[] Qualities)
        {
            base.PLC_DataChange(groupName, item, value, Qualities);
            switch (groupName)
            {
                case GROUP_NAME_CTRL:
                    break;
                case GROUP_NAME_STATE:
                    for (int i = 0; i < item.Length; i++)
                    {
                        if (value[i] != null)
                            switch (item[i])
                            {
                                case 0:
                                    ID = (ushort)value[i];
                                    break;
                                case 1:
                                    this.Type = (DataType)(ushort)value[i];
                                    break;
                                case 2:
                                    this.State = (DataState)(ushort)value[i];
                                    break;
                                case 3:                                   
                                    this.DataTick = (int)value[i];
                                    break;
                                case 4:
                                    this.Value1 = (float)value[i];
                                    break;
                                case 5:
                                    this.Value2 = (float)value[i];
                                    break;
                                case 6:
                                    ushort val = (ushort)value[i];
                                    this.Warning = ((val & 11) > 0) ? (true) : (false);
                                    break;
                            }
                    }
                    break;
            }
            DataTime = DateTime.Now;
            OnStatusChannge();
        }
        private DateTime CutOffMinute(DateTime dt)
        {
            return new DateTime(dt.Ticks - (dt.Ticks % TimeSpan.TicksPerMinute), dt.Kind);
        }
        public int ID { get; protected set; }
        public DataType Type { get; protected set; }
        public DataState State { get; protected set; }
        public int DataTick { get; protected set; }
        public int SyncTick { get; set; }
        public DateTime DataTime { get; protected set; }
        public DateTime SyncTime { get; protected set; }
        public float Value1 { get; protected set; }
        public float Value2 { get; protected set; }
        public float Value3 { get; protected set; }
        public bool Warning;

        public Dictionary<DateTime, Particle> Particle;
        public Particle ParticleCount;

        public void CalcPaticleCount()
        {
            if (this.Type == DataType.尘埃粒子)
            {

                DateTime minuteTick = CutOffMinute(DateTime.Now);

                List<DateTime> overdue = new List<DateTime>();
                //ParticleCount.Clear();
                foreach (var p in Particle)
                {
                    if (minuteTick - p.Key > TimeSpan.FromMinutes(35))
                    {
                        overdue.Add(p.Key);
                    }
                }

                foreach (var o in overdue)
                {
                    if (Particle.ContainsKey(o))
                        Particle.Remove(o);
                }
                Console.Write(minuteTick.ToString());
                if (!Particle.ContainsKey(minuteTick))
                //{
                //    Particle[minuteTick].Value1 = Value1;
                //    Particle[minuteTick].Value2 = Value2;
                //}
                //else
                {
                    Particle.Add(minuteTick, new Particle(minuteTick, Value1, Value2, Value3));
                }
                ParticleCount.Clear();
                foreach (var p in Particle)
                {
                    ParticleCount.Value1 += p.Value.Value1;
                    ParticleCount.Value2 += p.Value.Value2;
                }
            }
        }
        public override string ToString()
        {
            return string.Format("ID:{0}\nType:{1}\nState:{2}\nDataTime:{3}\nValue1:{4}\nValue2:{5}\nValue3:{6}\nConnectionState:{7}"
                , ID, this.Type, State, DataTick, Value1, Value2, Value3, ConnectionState);
        }
    }
}
