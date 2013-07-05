using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using Modbus.Device;
using System.IO.Ports;


namespace ModbusLibrary
{
    public partial class ModMdiaC2000 : Component, IModbusOperate
    {
        IModbusSerialMaster master;

        public void ConnectionPort(SerialPort ports)
        {
            master = ModbusSerialMaster.CreateRtu(ports);
        }
        public ModMdiaC2000()
        {
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
        private ushort[] channelRawData;
        /// <summary>
        /// 通道原始值(索引数对应的通道数)
        /// </summary>
        public ushort[] ChannelRawData
        {
            get { return channelRawData; }
        }
        /// <summary>
        /// 通道值(索引数对应的通道数)
        /// </summary>
        public float[] channelFloatData = new float[8];

        //测试
        private ushort name;

        public ushort Name
        {
            get { return name; }
            set { name = value; }
        }

        public void ReadData()
        {
            //设备虚拟器代码
            channelRawData = master.ReadHoldingRegisters(1, 0, 8);
            name = channelRawData[0];

            ////通道初始值的读取
            //channelRawData = master.ReadHoldingRegisters(1, 0x0511, 8);
            ////通道高低位的值
            //ushort[] channelData = master.ReadHoldingRegisters(1, 0x0501, 16);
            //ushort[] high = new ushort[8];
            //ushort[] low = new ushort[8];
            //int m = 0;
            //int n = 0;
            //for (int i = 0; i < channelData.Length; i = i + 2)
            //{
            //    high[m] = channelData[i];
            //    m++;
            //}
            //for (int i = 1; i < channelData.Length; i = i + 2)
            //{
            //    low[n] = channelData[i];
            //    n++;
            //}
            ////把高低位组合转为float
            //for (int i = 0; i < 8; i++)
            //{
            //    byte[] byHigh = BitConverter.GetBytes(high[i]);
            //    byte[] byLow = BitConverter.GetBytes(low[i]);
            //    byte[] allByte = new byte[4];
            //    allByte[0] = byHigh[0];
            //    allByte[1] = byHigh[1];
            //    allByte[2] = byLow[0];
            //    allByte[3] = byLow[1];
            //    channelFloatData[i] = BitConverter.ToSingle(allByte, 0);
            //}

        }
    }
}
