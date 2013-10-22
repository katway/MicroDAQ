using System;
using System.Collections.Generic;
using System.Text;
using MicroDAQ.Common;
using Modbus.Device;
using System.Data;
using MicroDAQ.Configuration;

namespace MicroDAQ.Gateways.Modbus2
{
    public class ModbusGateway : GatewayBase
    {

       
        public ModbusGateway(ModbusGatewayInfo config)
        {
            this.GatewayInfo = config;

            ///创建下属的ModbusMasterAgent对象
            foreach (ModbusMasterInfo masterInfo in gatewayInfo.modbusMaster)
            {
                ModbusMasterAgent newMaster = new ModbusMasterAgent(masterInfo);
                this.ModbusMasters.Add(newMaster);
            }

        }


        /// <param name="ItemManage">Item管理器对象</param>
        /// <param name="DatabaseManager">Database管理器对象</param>
        public ModbusGateway(IList<IDataItemManage> ItemManagers, IDatabaseManage DatabaseManager)
            : base(ItemManagers, DatabaseManager)
        {

        }


        public System.Collections.Generic.IList<ModbusMasterAgent> ModbusMasters { get; set; }

        /// <summary>
        /// 配置信息
        /// </summary>
        public ModbusGatewayInfo GatewayInfo
        {
            get { return gatewayInfo; }
            set { gatewayInfo = value; }
        }
        private ModbusGatewayInfo gatewayInfo;

    }
}
