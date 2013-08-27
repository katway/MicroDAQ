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
    /// <summary>
    /// 使用方法：ConnectionPort()连接-->slaveAdress离子器地址赋值-->ReadData()-->读取属性值
    /// </summary>
    public partial class ModMote5104 :Button, IModbusOperate
    {
        #region 全局变量和构造函数
        ModbusSerialMaster master;
        ModMote5104 button;
        public void ConnectionPort(SerialPort ports)
        {
            master = ModbusSerialMaster.CreateAscii(ports);
        }
        public ModMote5104()
        {
            button = this;
            button.Font = new System.Drawing.Font("宋体", 10F);
            button.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            button.UseVisualStyleBackColor = true;
            button.Size = new System.Drawing.Size(110, 30);
            InitializeComponent();
        }
        public ModMote5104(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }
        #endregion

        #region 数据寄存器字段 3***系列
        /// <summary>
        /// 时间戳（高）
        /// </summary>
        ushort timeStampHigh;
        [Description("时间戳（高）")]
        [Browsable(true)]
        public ushort TimeStampHigh
        {
            get { return timeStampHigh; }
            set { timeStampHigh = value; }
        }
        /// <summary>
        /// 时间戳（低）
        /// </summary>
        ushort timeStampLow;
        [Description("时间戳（低）")]
        [Browsable(true)]
        public ushort TimeStampLow
        {
            get { return timeStampLow; }
            set { timeStampLow = value; }
        }
        /// <summary>
        /// 取样时间（高）
        /// </summary>
        ushort simplingTimeHigh;
        [Description("取样时间（高）")]
        [Browsable(true)]
        public ushort SimplingTimeHigh
        {
            get { return simplingTimeHigh; }
            set { simplingTimeHigh = value; }
        }
        /// <summary>
        /// 取样时间（低）
        /// </summary>
        ushort simplingTimeLow;
        [Description("取样时间（低）")]
        [Browsable(true)]
        public ushort SimplingTimeLow
        {
            get
            {
                return simplingTimeLow;
            }
            set
            {
                simplingTimeLow = value;
            }
        }
        /// <summary>
        /// 位置（高）
        /// </summary>
        ushort positionHigh;
        [Description("位置（高）")]
        [Browsable(true)]
        public ushort PositionHigh
        {
            get { return positionHigh; }
            set { positionHigh = value; }
        }
        /// <summary>
        /// 位置（低）
        /// </summary>
        ushort positionLow;
        [Description("位置（低）")]
        [Browsable(true)]
        public ushort PositionLow
        {
            get { return positionLow; }
            set { positionLow = value; }
        }
        /// <summary>
        /// 设备状态（高）
        /// </summary>
        ushort deviceStateHigh;
        [Description("设备状态（高）")]
        [Browsable(true)]
        public ushort DeviceStateHigh
        {
            get { return deviceStateHigh; }
            set { deviceStateHigh = value; }
        }
        /// <summary>
        /// 设备状态（低）
        /// </summary>
        ushort deviceStateLow;
        [Description("设备状态（低）")]
        [Browsable(true)]
        public ushort DeviceStateLow
        {
            get { return deviceStateLow; }
            set { deviceStateLow = value; }
        }
        /// <summary>
        /// 基底电压（低）
        /// </summary>
        ushort baseVoltageLow;
        [Description("基底电压（低）")]
        [Browsable(true)]
        public ushort BaseVoltageLow
        {
            get { return baseVoltageLow; }
            set { baseVoltageLow = value; }
        }
        /// <summary>
        /// 基底电压（高）
        /// </summary>
        ushort baseVoltageHigh;
        [Description("基底电压（高）")]
        [Browsable(true)]
        public ushort BaseVoltageHigh
        {
            get { return baseVoltageHigh; }
            set { baseVoltageHigh = value; }
        }
        /// <summary>
        /// 流量值（低）
        /// </summary>
        ushort flowValueHigh;
        [Description("流量值（低）")]
        [Browsable(true)]
        public ushort FlowValueHigh
        {
            get { return flowValueHigh; }
            set { flowValueHigh = value; }
        }
        /// <summary>
        /// 流量值（高）
        /// </summary>
        ushort flowValueLow;
        [Description("流量值（高）")]
        [Browsable(true)]
        public ushort FlowValueLow
        {
            get { return flowValueLow; }
            set { flowValueLow = value; }
        }
        /// <summary>
        /// 激光电压（低）
        /// </summary>
        ushort laserVoltageLow;
        [Description("激光电压（低）")]
        [Browsable(true)]
        public ushort LaserVoltageLow
        {
            get { return laserVoltageLow; }
            set { laserVoltageLow = value; }
        }
        /// <summary>
        /// 激光电压（高）
        /// </summary>
        ushort laserVoltageHigh;
        [Description("激光电压（高）")]
        [Browsable(true)]
        public ushort LaserVoltageHigh
        {
            get { return laserVoltageHigh; }
            set { laserVoltageHigh = value; }
        }
        /// <summary>
        /// 离子信道1高位
        /// </summary>
        ushort firstChannelHigh;

        public ushort FirstChannelHigh
        {
            get { return firstChannelHigh; }
            set { firstChannelHigh = value; }
        }
        /// <summary>
        /// 离子信道1低位
        /// </summary>
        ushort firstChannelLow;

        public ushort FirstChannelLow
        {
            get { return firstChannelLow; }
            set { firstChannelLow = value; }
        }
        /// <summary>
        /// 离子信道2高位
        /// </summary>
        ushort secondChannelHigh;

        public ushort SecondChannelHigh
        {
            get { return secondChannelHigh; }
            set { secondChannelHigh = value; }
        }
        /// <summary>
        /// 离子信道2低位
        /// </summary>
        ushort secondChannelLow;

        public ushort SecondChannelLow
        {
            get { return secondChannelLow; }
            set { secondChannelLow = value; }
        } 
        /// <summary>
        /// 离子信道（高）
        /// </summary>
        //  public  ushort[] slaveChannelsHigh = new ushort[28];
        /// <summary>
        /// 离子信道（低）
        /// </summary>
        // public  ushort[] slavaChannelsLow = new ushort[28];
        #endregion

        #region 保持存储器字段 4***系列
        /// <summary>
        /// 版本号
        /// </summary>
        ushort versionId;
        [Description("版本号）")]
        [Browsable(true)]
        public ushort VersionId
        {
            get { return versionId; }
            set { versionId = value; }
        }
        /// <summary>
        /// 寄存器
        /// </summary>
        ushort command;
        [Description("寄存器")]
        [Browsable(true)]
        public ushort Command
        {
            get { return command; }
            set { command = value; }
        }
        /// <summary>
        /// 设备状态
        /// </summary>
        ushort deviceState;
        [Description("设备状态")]
        [Browsable(true)]
        public ushort DeviceState
        {
            get { return deviceState; }
            set { deviceState = value; }
        }
        /// <summary>
        /// 固件版本
        /// </summary>
        ushort firmwareVersion;
        [Description("固件版本")]
        [Browsable(true)]
        public ushort FirmwareVersion
        {
            get { return firmwareVersion; }
            set { firmwareVersion = value; }
        }
        /// <summary>
        /// 序号（高）
        /// </summary>
        ushort serialHigh;
        [Description("序号（高）")]
        [Browsable(true)]
        public ushort SerialHigh
        {
            get { return serialHigh; }
            set { serialHigh = value; }
        }
        /// <summary>
        /// 序号（低）
        /// </summary>
        ushort serialLow;
        [Description("序号（低）")]
        [Browsable(true)]
        public ushort SerialLow
        {
            get { return serialLow; }
            set { serialLow = value; }
        }
        /// <summary>
        /// 流量
        /// </summary>
        ushort flow;
        [Description("流量")]
        [Browsable(true)]
        public ushort Flow
        {
            get { return flow; }
            set { flow = value; }
        }
        /// <summary>
        /// 记录数
        /// </summary>
        ushort recordCount;
        [Description("记录数")]
        [Browsable(true)]
        public ushort RecordCount
        {
            get { return recordCount; }
            set { recordCount = value; }
        }
        /// <summary>
        /// 记录索引
        /// </summary>
        ushort recordIndex;
        [Description("记录索引")]
        [Browsable(true)]
        public ushort RecordIndex
        {
            get { return recordIndex; }
            set { recordIndex = value; }
        }
        /// <summary>
        /// 位置数
        /// </summary>
        ushort locationNumber;
        [Description("位置数")]
        [Browsable(true)]
        public ushort LocationNumber
        {
            get { return locationNumber; }
            set { locationNumber = value; }
        }
        /// <summary>
        /// 实时时钟（高）
        /// </summary>
        ushort rtcHigh;
        [Description("实时时钟（高）")]
        [Browsable(true)]
        public ushort RtcHigh
        {
            get { return rtcHigh; }
            set { rtcHigh = value; }
        }
        /// <summary>
        /// 实时时钟（低）
        /// </summary>
        ushort rtcLow;
        [Description("实时时钟（低）")]
        [Browsable(true)]
        public ushort RtcLow
        {
            get { return rtcLow; }
            set { rtcLow = value; }
        }
        /// <summary>
        /// 起始延迟（高）
        /// </summary>
        ushort startDelayHigh;
        [Description("起始延迟（高）")]
        [Browsable(true)]
        public ushort StartDelayHigh
        {
            get { return startDelayHigh; }
            set { startDelayHigh = value; }
        }
        /// <summary>
        /// 起始延迟（低）
        /// </summary>
        ushort startDelayLow;
        [Description("起始延迟（低）")]
        [Browsable(true)]
        public ushort StartDelayLow
        {
            get { return startDelayLow; }
            set { startDelayLow = value; }
        }
        /// <summary>
        /// 保持时间（高）
        /// </summary>
        ushort keepTimeHigh;
        [Description("保持时间（高）")]
        [Browsable(true)]
        public ushort KeepTimeHigh
        {
            get { return keepTimeHigh; }
            set { keepTimeHigh = value; }
        }
        /// <summary>
        /// 保持时间（低）
        /// </summary>
        ushort keepTimeLow;
        [Description("保持时间（低）")]
        [Browsable(true)]
        public ushort KeepTimeLow
        {
            get { return keepTimeLow; }
            set { keepTimeLow = value; }
        }
        /// <summary>
        /// 采样时间（高）
        /// </summary>
        ushort simpleTimeHigh;
        [Description("采样时间（高）")]
        [Browsable(true)]
        public ushort SimpleTimeHigh
        {
            get { return simpleTimeHigh; }
            set { simpleTimeHigh = value; }
        }
        /// <summary>
        /// 采样时间（低）
        /// </summary>
        ushort simpleTimeLow;
        [Description("采样时间（低）")]
        [Browsable(true)]
        public ushort SimpleTimeLow
        {
            get { return simpleTimeLow; }
            set { simpleTimeLow = value; }
        }
        /// <summary>
        /// 数据设置（高）
        /// </summary>
        ushort dataSettingHigh;
        [Description("数据设置（高）")]
        [Browsable(true)]
        public ushort DataSettingHigh
        {
            get { return dataSettingHigh; }
            set { dataSettingHigh = value; }
        }
        /// <summary>
        /// 数据设置（低）
        /// </summary>
        ushort dataSettingLow;
        [Description("数据设置（低）")]
        [Browsable(true)]
        public ushort DataSettingLow
        {
            get { return dataSettingLow; }
            set { dataSettingLow = value; }
        }
        /// <summary>
        /// 产品名称
        /// </summary>
        ushort[] productsName = new ushort[16];

        #endregion

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

        #region 读取存储器数据 
        public void ReadData()
        {
            ReadHoldingRegisters();
            ReadInputRegisters();
        }
        /// <summary>
        /// 读3字段
        /// </summary>
        private void ReadInputRegisters()
        {
            int j = 0;
            int g = 0;
            master.Transport.Retries = 1;
            master.Transport.ReadTimeout = 300;
            master.Transport.WriteTimeout = 300;
            ushort[] register = master.ReadInputRegisters(slaveAdress, 0, 70);
            timeStampHigh = register[0];
            timeStampLow = register[1];
            simplingTimeHigh = register[2];
            simplingTimeLow = register[3];
            positionHigh = register[4];
            positionLow = register[5];
            deviceStateHigh = register[6];
            deviceStateLow = register[7];
            firstChannelHigh = register[8];
            firstChannelLow = register[9];
            secondChannelHigh = register[10];
            secondChannelLow = register[11];

            //for (int i = 8; i < 64; i=i+2)
            //{ 
            //    slaveChannelsHigh[j] = register[i];
            //    j++;
            //}
            //for (int i = 9; i < 64; i = i + 2)
            //{
            //    slavaChannelsLow[g] = register[i];
            //    g++;
            //}
            baseVoltageLow = register[64];
            baseVoltageHigh = register[65];
            flowValueLow = register[66];
            flowValueHigh = register[67];
            laserVoltageLow = register[68];
            laserVoltageHigh = register[69];

        }
        /// <summary>
        /// 读4字段
        /// </summary>
        private void ReadHoldingRegisters()
        {

            int j = 0;
            master.Transport.ReadTimeout = 300;
            master.Transport.WriteTimeout = 300;
            master.Transport.Retries = 1;
            ushort[] holdRegister = master.ReadHoldingRegisters(slaveAdress, 0, 36);
            versionId = holdRegister[0];
            command = holdRegister[1];
            deviceState = holdRegister[2];
            firmwareVersion = holdRegister[3];
            serialHigh = holdRegister[4];
            serialLow = holdRegister[5];
            for (int i = 6; i < 22; i++)
            {
                productsName[j] = holdRegister[i];
                j++;
            }//产品名称
            flow = holdRegister[22];
            recordCount = holdRegister[23];
            recordIndex = holdRegister[24];
            locationNumber = holdRegister[25];
            rtcHigh = holdRegister[26];
            rtcLow = holdRegister[27];
            startDelayHigh = holdRegister[28];
            startDelayLow = holdRegister[29];
            keepTimeHigh = holdRegister[30];
            keepTimeLow = holdRegister[31];
            simpleTimeHigh = holdRegister[32];
            simpleTimeLow = holdRegister[33];
            dataSettingHigh = holdRegister[34];
            dataSettingLow = holdRegister[35];


        }
        #endregion
    }

}
