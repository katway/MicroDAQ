using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using Modbus.Device;
using System.IO.Ports;
using System.Windows.Forms;


namespace ModbusLibrary
{
    public partial class ModMdiaC2000 :Button , IModbusOperate
    {
        IModbusSerialMaster master;
        ModMdiaC2000 button;

        public void ConnectionPort(SerialPort ports)
        {
            master = ModbusSerialMaster.CreateRtu(ports);
        }
        public ModMdiaC2000()
        {
            button = this;
            button.Font = new System.Drawing.Font("宋体", 10F);
            button.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            button.UseVisualStyleBackColor = true;
            button.Size = new System.Drawing.Size(110, 30);
            InitializeComponent();
        }

        public ModMdiaC2000(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
        #region 离子地址字段
        /// <summary>
        /// 离子器地址编号
        /// </summary>
        byte slaveAdress;
        [Description("离子器地址编号")]
        [Browsable(true)]
        public byte SlaveAdress
        {
            set
            {
                slaveAdress = value;

            }

            get { return slaveAdress; }
        }
        #endregion

        #region 通道输入的原始值0~7
        private ushort[] channelRawData;
        
        private ushort channelRawData0;
        public ushort ChannelRawData0
        {
            get { return channelRawData0; }
        }
        private ushort channelRawData1;
        public ushort ChannelRawData1
        {
            get { return channelRawData1; }
        }
        private ushort channelRawData2;
        public ushort ChannelRawData2
        {
            get { return channelRawData2; }
        }
        private ushort channelRawData3;
        public ushort ChannelRawData3
        {
            get { return channelRawData3; }
        }
        private ushort channelRawData4;
        public ushort ChannelRawData4
        {
            get { return channelRawData4; }
        }
        private ushort channelRawData5;
        public ushort ChannelRawData5
        {
            get { return channelRawData5; }
        }
        private ushort channelRawData6;
        public ushort ChannelRawData6
        {
            get { return channelRawData6; }
        }
        private ushort channelRawData7;
        public ushort ChannelRawData7
        {
            get { return channelRawData7; }
        }
        #endregion

        #region 通道值0~7
        /// <summary>
        /// 通道值(索引数对应的通道数)
        /// </summary>
        private float[] channelFloatData = new float[8];

        private float channelFloatData0;
        public float ChannelFloatData0
        {
            get { return channelFloatData0; }
        }
        private float channelFloatData1;
        public float ChannelFloatData1
        {
            get { return channelFloatData1; }
        }
        private float channelFloatData2;
        public float ChannelFloatData2
        {
            get { return channelFloatData2; }
        }
        private float channelFloatData3;
        public float ChannelFloatData3
        {
            get { return channelFloatData3; }
        }
        private float channelFloatData4;
        public float ChannelFloatData4
        {
            get { return channelFloatData4; }
        }
        private float channelFloatData5;
        public float ChannelFloatData5
        {
            get { return channelFloatData5; }
        }
        private float channelFloatData6;
        public float ChannelFloatData6
        {
            get { return channelFloatData6; }
        }
        private float channelFloatData7;
        public float ChannelFloatData7
        {
            get { return channelFloatData7; }
        }
        #endregion




        public void ReadData()
        {
            // 测试设备虚拟器代码
            //channelRawData = master.ReadHoldingRegisters(1, 0, 8);
            master.Transport.ReadTimeout = 300;
           
            //读取通道初始值
            channelRawData = master.ReadHoldingRegisters(slaveAdress, 0, 8);
            channelRawData0=channelRawData[0];
            channelRawData1=channelRawData[1];
            channelRawData2=channelRawData[2];
            channelRawData3=channelRawData[3];
            channelRawData4=channelRawData[4];
            channelRawData5=channelRawData[5];
            channelRawData6=channelRawData[6];
            channelRawData7=channelRawData[7];
            //通道高低位的值
            ushort[] channelData = master.ReadHoldingRegisters(slaveAdress, 0x0501, 16);
            ushort[] high = new ushort[8];
            ushort[] low = new ushort[8];
            int m = 0;
            int n = 0;
            for (int i = 0; i < channelData.Length; i = i + 2)
            {
                high[m] = channelData[i];
                m++;
            }
            for (int i = 1; i < channelData.Length; i = i + 2)
            {
                low[n] = channelData[i];
                n++;
            }
            //把高低位组合转为float
            for (int i = 0; i < 8; i++)
            {
                byte[] byHigh = BitConverter.GetBytes(high[i]);
                byte[] byLow = BitConverter.GetBytes(low[i]);
                byte[] allByte = new byte[4];
                allByte[0] = byHigh[0];
                allByte[1] = byHigh[1];
                allByte[2] = byLow[0];
                allByte[3] = byLow[1];
                channelFloatData[i] = BitConverter.ToSingle(allByte, 0);
            }

            channelFloatData0=channelFloatData[0];
            channelFloatData1=channelFloatData[1];
            channelFloatData2=channelFloatData[2];
            channelFloatData3=channelFloatData[3];
            channelFloatData4=channelFloatData[4];
            channelFloatData5=channelFloatData[5];
            channelFloatData6=channelFloatData[6];
            channelFloatData7=channelFloatData[7];
         }
    }
}
