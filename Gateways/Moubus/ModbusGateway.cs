using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Modbus.Device;
using System.Net;
using System.Net.Sockets;
using System.IO.Ports;
using JonLibrary.Automatic;
using MicroDAQ.Common;


namespace MicroDAQ.Gateways.Modbus
{
    public class ModbusGateway : GatewayBase
    {

        public SqlConnection Connection { get; set; }
        public DataTable IPMasterDevice;     //通过ip通讯的设备信息表
        public DataTable SerialMasterDevice;//通过Com口通讯的设备信息表
        public DataTable IPMasterGroup;
        public DataTable SerialMasterGroup;
        /// <summary>
        /// 数据项管理器
        /// </summary>
        public List<SerialPortMasterManager> SerialManagers = new List<SerialPortMasterManager>();
        public List<IPMasterManager> IPManagers = new List<IPMasterManager>();

        /// <summary>
        /// 数据库管理器
        /// </summary>
        public IList<IDatabaseManage> DatabaseManagers { get; set; }
        public CycleTask UpdateCycle { get; private set; }
        public CycleTask ModbusCycle { get; private set; }
        public ModbusGateway(IList<IDatabaseManage> databaseManagers)
        {
            //string ConnectionString = "server=.\\SQLEXPRESS;database=opcmes3;uid=microdaq;pwd=microdaq";
            string ConnectionString = @"server=192.168.1.201\SQL2000;database=opcmes3;uid=microdaq;pwd=microdaq";
            Connection = new SqlConnection(ConnectionString);
            this.DatabaseManagers = databaseManagers;
            UpdateCycle = new CycleTask();
            UpdateCycle.WorkStateChanged += new CycleTask.WorkStateChangeEventHandle(UpdateCycle_WorkStateChanged);
            ModbusCycle = new CycleTask();
            ModbusCycle.WorkStateChanged += new CycleTask.WorkStateChangeEventHandle(ModbusCycle_WorkStateChanged);

            SetTable();
            CreateIPDevice();
            CreatePortDevice();

        }

        #region SQL查询,全局变量DataTable赋值
        /// <summary>
        /// 获得所有IP通讯方式的设备信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetIPMasterDevice()
        {
            string sqlStr = "select a.SerialID,b.IP,b.Port,a.TransferType,a.slave from ModbusSlave a left join IPSetting b on a.IPSetting_SerialID=b.SerialID where a.Type='IP' order by a.TransferType,a.IPSetting_SerialID ";
            Connection.Open();
            SqlDataAdapter da = new SqlDataAdapter(sqlStr, Connection);
            DataSet ds = new DataSet();
            da.Fill(ds);
            Connection.Close();
            return ds.Tables[0];

        }
        /// <summary>
        /// 获得所有串口通讯的设备信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetSerialMasterDevice()
        {

            string sqlStr = "select a.SerialID,b.BaudRate,b.Databits,b.Parity,b.Stopbits,c.PortName,a.TransferType,a.slave from ModbusSlave a left join SerialPortSetting b on a.SerialPortSetting_SerialID=b.SerialID left join SerialPort c on a.SerialPort_SerialID=c.SerialID where a.Type='serialPort' order by a.TransferType,a.SerialPortSetting_SerialID,c.SerialID  ";
            Connection.Open();
            SqlDataAdapter da = new SqlDataAdapter(sqlStr, Connection);
            DataSet ds = new DataSet();
            da.Fill(ds);
            Connection.Close();
            return ds.Tables[0];
        }
        /// <summary>
        /// 每个Ip设置对应的设备个数
        /// </summary>
        /// <returns></returns>
        public DataTable IPMasterGroupCount()
        {
            string sqlStr = "select COUNT(*) count from ModbusSlave a left join IPSetting b on a.IPSetting_SerialID=b.SerialID where a.Type='IP' group by a.TransferType,a.IPSetting_SerialID order by  a.TransferType,a.IPSetting_SerialID ";
            Connection.Open();
            SqlDataAdapter da = new SqlDataAdapter(sqlStr, Connection);
            DataSet ds = new DataSet();
            da.Fill(ds);
            Connection.Close();
            return ds.Tables[0];
        }
        /// <summary>
        /// 每个serialport设置对应的个数
        /// </summary>
        /// <returns></returns>
        public DataTable SerialMasterGroupCount()
        {
            string sqlStr = "select COUNT(*) count from ModbusSlave a left join SerialPortSetting b on a.SerialPortSetting_SerialID=b.SerialID left join SerialPort c on a.SerialPort_SerialID=c.SerialID where a.Type='SerialPort' group by a.TransferType,a.SerialPortSetting_SerialID,c.SerialID order by  a.TransferType,a.SerialPortSetting_SerialID,c.SerialID ";
            Connection.Open();
            SqlDataAdapter da = new SqlDataAdapter(sqlStr, Connection);
            DataSet ds = new DataSet();
            da.Fill(ds);
            Connection.Close();
            return ds.Tables[0];

        }
        private void SetTable()
        {
            IPMasterDevice = GetIPMasterDevice();
            SerialMasterDevice = GetSerialMasterDevice();
            IPMasterGroup = IPMasterGroupCount();
            SerialMasterGroup = SerialMasterGroupCount();
        }
        /// <summary>
        /// 每个设备的读命令
        /// </summary>
        /// <param name="deviceID"></param>
        /// <returns></returns>
        private DataTable GetReadCommandsByID(string deviceID)
        {
            string sqlStr = "select * from  ModbusCommands a left join RegisterType b on a.RegisterType=b.SerialID  where a.ModbusSlave_SerialID=" + "'" + deviceID + "'and b.registerName='HoldinGregister' or b.registerName='InputRegisters' ";
            Connection.Open();
            SqlDataAdapter da = new SqlDataAdapter(sqlStr, Connection);
            DataSet ds = new DataSet();
            da.Fill(ds);
            Connection.Close();
            return ds.Tables[0];
        }
        /// <summary>
        /// 每个设备读命令的唯一编号
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private DataTable GetMetaByID(DataTable dt)
        {
            string sqlStr = "select * from MetaData a where a.ModbusCommands_SerialID in( ";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i != dt.Rows.Count - 1)
                {
                    sqlStr += "'" + dt.Rows[i]["SerialID"].ToString() + "',";
                }
                else
                {
                    sqlStr += "'" + dt.Rows[i]["SerialID"].ToString() + "')";
                }
            }
            Connection.Open();
            SqlDataAdapter da = new SqlDataAdapter(sqlStr, Connection);
            DataSet ds = new DataSet();
            da.Fill(ds);
            Connection.Close();
            return ds.Tables[0];
        }
        private DataTable GetWriteCommandsByID(string deviceID)
        {
            string sqlStr = "select * from modubs_control a where a.SerialID=" + "'" + deviceID + "'";
            Connection.Open();
            SqlDataAdapter da = new SqlDataAdapter(sqlStr, Connection);
            DataSet ds = new DataSet();
            da.Fill(ds);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                if (row["type"].ToString() == "CTR")
                {
                    string sql = string.Format("Update remotecontrol SET cmdstate= {0} WHERE slave= {1}", 2, row["code"].ToString());//, Connection);
                    SqlCommand Command = new SqlCommand(sql, Connection);
                    Command.ExecuteNonQuery();
                }
            }

            Connection.Close();
            return ds.Tables[0];
        }
        #endregion

        #region 创建Master
        /// <summary>
        /// 创建IpMaster
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private ModbusIpMaster CreateModbusIpMaster(string ip, int port, string type)
        {
            string[] strIP = ip.Split('.');
            byte[] byteIP = new byte[4];
            for (int i = 0; i < strIP.Length; i++)
            {
                byteIP[i] = Convert.ToByte(strIP[i]);
            }
            IPAddress address = new IPAddress(byteIP);
            try
            {
                if (type.ToLower() == "tcp")
                {

                    TcpClient TcpClient = new TcpClient(address.ToString(), port);
                    ModbusIpMaster master = ModbusIpMaster.CreateIp(TcpClient);
                    return master;
                }
                else //UDP
                {
                    UdpClient UdpClient = new UdpClient();
                    IPEndPoint endPoint = new IPEndPoint(address, port);
                    UdpClient.Connect(endPoint);
                    ModbusIpMaster master = ModbusIpMaster.CreateIp(UdpClient);
                    return master;
                }
            }
            catch
            {
                // 连接失败
                return null;
            }

        }
        /// <summary>
        /// 创建SerialMaster(TcpClient)
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private ModbusSerialMaster CreateSerialMaster(string ip, int port, string type)
        {
            string[] strIP = ip.Split('.');
            byte[] byteIP = new byte[4];
            for (int i = 0; i < strIP.Length; i++)
            {
                byteIP[i] = Convert.ToByte(strIP[i]);
            }
            IPAddress address = new IPAddress(byteIP);
            try
            {
                TcpClient masterTcpClient = new TcpClient(address.ToString(), port);
                IModbusMaster master;
                if (type.ToLower() == "rtu")
                {
                    master = ModbusSerialMaster.CreateRtu(masterTcpClient);

                }
                else
                {
                    master = ModbusSerialMaster.CreateAscii(masterTcpClient);

                }
                return (ModbusSerialMaster)master;
            }
            catch
            {
                // 连接失败
                return null;
            }



        }
        private ModbusSerialMaster CreateSerialMaster(DataRow row)
        {
            try
            {
                string com = row["PortName"].ToString();
                SerialPort port = new SerialPort(com);

                port.BaudRate = Convert.ToInt32(row["BaudRate"]);
                port.DataBits = Convert.ToInt32(row["DataBits"]);
                port.Parity = (Parity)Enum.Parse(typeof(Parity), row["Parity"].ToString());
                port.StopBits = (StopBits)Enum.Parse(typeof(Parity), row["StopBits"].ToString());
                port.Open();//打开串口
                IModbusMaster master;

                if (row["TransferType"].ToString().ToLower() == "rtu")
                {
                    master = ModbusSerialMaster.CreateRtu(port);
                }
                else
                {
                    master = ModbusSerialMaster.CreateAscii(port);
                }

                return (ModbusSerialMaster)master;
            }
            catch
            {
                return null;
            }
        }



        #endregion

        #region 创建设备类
        /// <summary>
        /// IP通讯的设备处理
        /// </summary>
        private void CreateIPDevice()
        {
            if (IPMasterDevice.Rows.Count != 0)
            {
                int row = 0;   //记录行的索引
                for (int i = 0; i < IPMasterGroup.Rows.Count; i++)
                {


                    int count = Convert.ToInt32(IPMasterGroup.Rows[i][0]);
                    string type = IPMasterDevice.Rows[row]["TransferType"].ToString();
                    string ip = IPMasterDevice.Rows[row]["IP"].ToString();
                    int port = Convert.ToInt32(IPMasterDevice.Rows[row]["Port"]);
                    if (type.ToLower() == "udp" || type.ToLower() == "tcp")
                    {
                        ModbusIpMaster master = CreateModbusIpMaster(ip, port, type);
                        if (master != null)
                        {
                            for (int j = 0; j < count; j++)
                            {
                                string deviceID = IPMasterDevice.Rows[row]["SerialID"].ToString();
                                int slave = Convert.ToInt32(IPMasterDevice.Rows[row]["Slave"]);
                                DataTable commandData = GetReadCommandsByID(deviceID);
                                DataTable metaData = new DataTable();
                                DataTable writeData = GetWriteCommandsByID(deviceID);
                                if (commandData.Rows.Count != 0)
                                {
                                    metaData = GetMetaByID(commandData);
                                }
                                IPMasterManager manager = new IPMasterManager(master, slave, commandData, metaData, writeData);
                                IPManagers.Add(manager);


                                row = row + 1;
                            }
                        }
                        else
                        {
                            for (int j = 0; j < count; j++)
                            {
                                string deviceID = IPMasterDevice.Rows[row]["SerialID"].ToString();
                                // ProCommandState(deviceID);
                                // row=row+1;

                            }
                            row += count;
                        }
                    }
                    else //rtu,Ascii
                    {
                        IModbusMaster master = CreateSerialMaster(ip, port, type);
                        if (master != null)
                        {
                            for (int j = 0; j < count; j++)
                            {
                                string deviceID = IPMasterDevice.Rows[row]["SerialID"].ToString();
                                int slave = Convert.ToInt32(IPMasterDevice.Rows[row]["Slave"]);
                                DataTable commandData = GetReadCommandsByID(deviceID);
                                DataTable metaData = new DataTable();
                                DataTable writeData = GetWriteCommandsByID(deviceID);
                                if (commandData.Rows.Count != 0)
                                {
                                    metaData = GetMetaByID(commandData);
                                }
                                SerialPortMasterManager manager = new SerialPortMasterManager(master, slave, commandData, metaData, writeData);
                                SerialManagers.Add(manager);

                                row = row + 1;
                            }
                        }
                        else
                        {

                            for (int j = 0; j < count; j++)
                            {
                                string deviceID = IPMasterDevice.Rows[row]["SerialID"].ToString();
                                // ProCommandState(deviceID);
                            }
                            row += count;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Port通讯的设备处理
        /// </summary>
        private void CreatePortDevice()
        {
            int row = 0;   //记录行的索引
            for (int i = 0; i < SerialMasterGroup.Rows.Count; i++)
            {
                int count = Convert.ToInt32(SerialMasterGroup.Rows[i][0]);//每组的个数
                string type = SerialMasterDevice.Rows[row]["TransferType"].ToString();
                DataRow rowData = SerialMasterDevice.Rows[row];
                IModbusMaster master = CreateSerialMaster(rowData);
                if (master != null)
                {
                    for (int j = 0; j < count; j++) //同一个master，创建设备类
                    {
                        string deviceID = SerialMasterDevice.Rows[row]["SerialID"].ToString();
                        int slave = Convert.ToInt32(SerialMasterDevice.Rows[row]["Slave"]);
                        DataTable commandData = GetReadCommandsByID(deviceID);
                        DataTable metaData = new DataTable();
                        DataTable writeData = GetWriteCommandsByID(deviceID);
                        if (commandData.Rows.Count != 0)
                        {
                            metaData = GetMetaByID(commandData);
                        }
                        SerialPortMasterManager manager = new SerialPortMasterManager(master, slave, commandData, metaData, writeData);
                        SerialManagers.Add(manager);

                        row = row + 1;
                    }
                }
                else
                {
                    row += count;
                }

            }
        }
        #endregion

        #region 状态改变
        void UpdateCycle_WorkStateChanged(JonLibrary.Automatic.RunningState state)
        {
            this.RunningState = (GatewayState)((int)state);
        }
        void ModbusCycle_WorkStateChanged(JonLibrary.Automatic.RunningState state)
        {
            this.RunningState = (GatewayState)((int)state);
        }
        #endregion

        #region 数据库提交

        protected virtual void Update()
        {
            foreach (IDatabaseManage dbMgr in this.DatabaseManagers)
            {
                foreach (SerialPortMasterManager mgr in this.SerialManagers)
                {
                    foreach (Item item in mgr.Items)
                    {
                        dbMgr.UpdateItem(item);
                    }
                }
            }
            foreach (IDatabaseManage dbMgr in this.DatabaseManagers)
            {
                foreach (IPMasterManager mgr in this.IPManagers)
                {
                    foreach (Item item in mgr.Items)
                    {
                        dbMgr.UpdateItem(item);
                    }
                }
            }
        }
        #endregion

        #region 遍历数据项管理器
        public void ErgodicManagers()
        {
            foreach (SerialPortMasterManager manager in this.SerialManagers)
                manager.ReadWriteData();

            foreach (IPMasterManager manager in this.IPManagers)
                manager.Read();
        }
        #endregion

        #region Start()
        /// <summary>
        /// 启动
        /// </summary>
        public override void Start()
        {
            //ErgodicManagers();
            //Update();
            ModbusCycle.Run(this.ErgodicManagers, System.Threading.ThreadPriority.BelowNormal);
            UpdateCycle.Run(this.Update, System.Threading.ThreadPriority.BelowNormal);

        }
        #endregion

        #region Pasue()
        /// <summary>
        /// 暂停更新和控制
        /// </summary>
        public override void Pause()
        {
            this.Pause(this.UpdateCycle);
        }

        /// <summary>
        /// 暂停参数指定的任务对象
        /// </summary>
        /// <param name="task">要暂停的任务对象</param>
        public void Pause(CycleTask task)
        {
            if (task != null)
                task.Pause();
        }
        #endregion

        #region Continue()
        /// <summary>
        /// 暂停更新和控制
        /// </summary>
        public override void Continue()
        {
            this.Continue(this.UpdateCycle);
        }

        /// <summary>
        /// 继续参数指定的任务对象
        /// </summary>
        /// <param name="task">要继续的任务对象</param>
        public void Continue(CycleTask task)
        {
            if (task != null)
                task.Continue();
        }

        #endregion

        #region Stop()
        /// <summary>
        /// 暂停更新和控制
        /// </summary>
        public override void Stop()
        {
            this.Stop(this.UpdateCycle);
        }

        /// <summary>
        /// 暂停参数指定的任务对象
        /// </summary>
        /// <param name="task">要暂停的任务对象</param>
        private void Stop(CycleTask task)
        {
            if (task != null)
                task.Quit();
        }
        #endregion

        #region 日志存储过程
        private int ProCommandState(int serialID)
        {
            SqlCommand command = new SqlCommand("RecordCommandLogByDeviceID", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@serialID ", SqlDbType.Int));
            command.UpdatedRowSource = UpdateRowSource.None;
            command.Parameters["@serialID "].Value = serialID;
            Connection.Open();
            int i = command.ExecuteNonQuery();
            Connection.Close();
            return i;

        }
        #endregion
    }
}
