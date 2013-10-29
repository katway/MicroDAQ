using System;
using System.Collections.Generic;
using System.Text;
using MicroDAQ.Common;
using Modbus.Device;
using System.Data;
using MicroDAQ.Configuration;
using JonLibrary.Automatic;

namespace MicroDAQ.Gateways.Modbus2
{
    public class ModbusGateway : GatewayBase
    {

        public CycleTask UpdateCycle { get; private set; }
        public CycleTask ModbusCycle { get; private set; }
        public ModbusGateway(ModbusGatewayInfo config, IList<IDatabase> DatabaseManagers)
        {
            this.GatewayInfo = config;
            this.DatabaseManagers = DatabaseManagers;
            UpdateCycle = new CycleTask();
            UpdateCycle.WorkStateChanged += new CycleTask.WorkStateChangeEventHandle(UpdateCycle_WorkStateChanged);
            ModbusCycle = new CycleTask();
            ModbusCycle.WorkStateChanged += new CycleTask.WorkStateChangeEventHandle(ModbusCycle_WorkStateChanged);
            ///创建下属的ModbusMasterAgent对象
            this.ItemManagers = new List<IDataItemManage>();
            foreach (ModbusMasterInfo masterInfo in gatewayInfo.modbusMaster)
            {
                ModbusMasterAgent newMaster = new ModbusMasterAgent(masterInfo);
                this.ItemManagers.Add(newMaster);
            }

        }


        /// <param name="ItemManage">Item管理器对象</param>
        /// <param name="DatabaseManager">Database管理器对象</param>
        //public ModbusGateway(IList<IDataItemManage> ItemManagers, IDatabaseManage DatabaseManager)
        //    : base(ItemManagers, DatabaseManager)
        //{

        //}


        //public System.Collections.Generic.IList<IDataItemManage> ModbusMasters { get; set; }

        /// <summary>
        /// 配置信息
        /// </summary>
        public ModbusGatewayInfo GatewayInfo
        {
            get { return gatewayInfo; }
            set { gatewayInfo = value; }
        }
        private ModbusGatewayInfo gatewayInfo;

         public void Start()
         {
             // 读写

             ModbusCycle.Run(this.ReadWrite, System.Threading.ThreadPriority.BelowNormal);
             
             //数据提交
             UpdateCycle.Run(this.Push, System.Threading.ThreadPriority.BelowNormal);

         }
         #region 读写
         private void ReadWrite()
         {
             foreach (ModbusMasterAgent modbusMaster in ItemManagers)
             {
                 modbusMaster.ReadWrite();
             }
         }
#endregion
         #region 状态改变
         void UpdateCycle_WorkStateChanged(JonLibrary.Automatic.RunningState state)
         {
             this.GatewayState = (GatewayState)((int)state);
         }
         void ModbusCycle_WorkStateChanged(JonLibrary.Automatic.RunningState state)
         {
             this.GatewayState = (GatewayState)((int)state);
         }
         #endregion


    }  
}
