using System;
using System.Collections.Generic;
using System.Text;
using MicroDAQ.DataItem;
using JonLibrary.Automatic;
using MicroDAQ.Database;
using System.Xml;
using System.IO.Ports;
namespace MicroDAQ.Gateway
{
    class ModbusGateway : Gateway.GatewayBase
    {
        #region 全局变量，构造函数
        /// <summary>
        /// 数据项管理器
        /// </summary>
        public List<ModbusDataItemManager> ItemManagers;
        /// <summary>
        /// 数据库管理器
        /// </summary>
        public IList<IDatabaseManage> DatabaseManagers { get; set; }
        public CycleTask UpdateCycle { get; private set; }
        public CycleTask ModbusCycle { get; private set; }
        int count;
        List<string> modbusName;
        List<byte> slaveID;
        Dictionary<byte, Dictionary<int, string>> dic;
        Dictionary<string, string> SerialPort = new Dictionary<string, string>();
        public ModbusGateway(IList<IDatabaseManage> databaseManagers)
        {
            #region 初始化全局变量
            modbusName = new List<string>();
            slaveID = new List<byte>();
            dic = new Dictionary<byte, Dictionary<int, string>>();
            ItemManagers = new List<ModbusDataItemManager>();
            #endregion
            ReadXml();       //读xml文件
            CreateItemsMangers(CreatePort());
            this.DatabaseManagers = databaseManagers;
            UpdateCycle = new CycleTask();
            UpdateCycle.WorkStateChanged += new CycleTask.WorkStateChangeEventHandle(UpdateCycle_WorkStateChanged);
            ModbusCycle = new CycleTask();
            ModbusCycle.WorkStateChanged+=new CycleTask.WorkStateChangeEventHandle(ModbusCycle_WorkStateChanged);
        }
        #endregion

        #region 状态改变
        void UpdateCycle_WorkStateChanged(JonLibrary.Automatic.RunningState state)
        {
            this.RunningState = (Gateway.RunningState)((int)state);
        }
        void ModbusCycle_WorkStateChanged(JonLibrary.Automatic.RunningState state)
        {
            this.RunningState = (Gateway.RunningState)((int)state);
        }
        #endregion

        #region 创建数据项管理器
        /// <summary>
        /// 创建数据项管理器
        /// </summary>
        private void CreateItemsMangers(SerialPort port)
        {  
           
            for (int i = 0; i < count; i++)
            {
                ModbusDataItemManager data = new ModbusDataItemManager(modbusName[i], slaveID[i], dic[slaveID[i]], port);
                ItemManagers.Add(data);
            }
           
        }
        private void ReadXml()
        {
            string path = System.AppDomain.CurrentDomain.BaseDirectory + "温湿度采集.xml";
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            //port参数
            XmlNode portNode = doc.SelectSingleNode("/Session/SerialMaster");
            SerialPort.Add("Port", portNode.Attributes[0].Value);
            SerialPort.Add("BaudRate", portNode.Attributes[1].Value);
            SerialPort.Add("Parity", portNode.Attributes[2].Value);
            SerialPort.Add("DataBits", portNode.Attributes[3].Value);
            SerialPort.Add("StopBits", portNode.Attributes[4].Value);
            //以下为maser参数
            XmlNode node = doc.SelectSingleNode("/Session/SerialMaster");
            XmlNodeList list = node.ChildNodes;
            count = list.Count;   //设备个数
            foreach (XmlNode xmlNode in list)// 遍历设备
            {
                modbusName.Add(xmlNode.Attributes[0].Value);
                slaveID.Add(Convert.ToByte(xmlNode.Attributes[1].Value));
                XmlNodeList nodelist = xmlNode.ChildNodes;

                Dictionary<int, string> pro = new Dictionary<int, string>();
                foreach (XmlNode childs in nodelist)
                {

                    pro.Add(Convert.ToInt32(childs.Attributes[1].Value), childs.Attributes[0].Value);

                }
                dic.Add(Convert.ToByte(xmlNode.Attributes[1].Value), pro);
            }
            
        }
        private SerialPort CreatePort()
        {
            SerialPort port = new SerialPort(SerialPort["Port"]);
            port.BaudRate =Convert.ToInt32(SerialPort["BaudRate"]);
            port.DataBits =Convert.ToInt32(SerialPort["DataBits"]);
            port.Parity = (Parity)Enum.Parse(typeof(Parity),SerialPort["Parity"]);
            port.StopBits = (StopBits)Enum.Parse(typeof(Parity), SerialPort["StopBits"]);
            port.Open();//打开串口
            return port;
        }
        #endregion

        #region 遍历数据项管理器
        public void ErgodicManagers()
            {
            foreach (ModbusDataItemManager manager in this.ItemManagers)
                manager.ModbusReadData();     
        }
        #endregion

        #region 数据库提交

        protected virtual void Update()
        {
            foreach (IDatabaseManage dbMgr in this.DatabaseManagers)
            {
                foreach (IDataItemManage mgr in this.ItemManagers)
                {
                    foreach (Item item in mgr.Items)
                    {
                        dbMgr.UpdateItem(item);
                    }
                }
            }
        }
        #endregion

        #region Start()
        /// <summary>
        /// 启动
        /// </summary>
        public override void Start()
        {

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

    }
}
