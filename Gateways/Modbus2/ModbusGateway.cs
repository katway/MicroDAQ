using System;
using System.Collections.Generic;
using System.Text;
using MicroDAQ.Common;
using Modbus.Device;

namespace MicroDAQ.Gateways.Modbus2
{
    public class ModbusGateway : GatewayBase
    {

        public ModbusGateway()
        {
            throw new System.NotImplementedException();
        }
        /// <param name="ItemManage">Item管理器对象</param>
        /// <param name="DatabaseManager">Database管理器对象</param>
        public ModbusGateway(IList<MicroDAQ.Common.IDataItemManage> ItemManagers, IDatabaseManage DatabaseManager)
            : base(ItemManagers, DatabaseManager)
        {

        }


        public System.Collections.Generic.IList<ModbusMasterAgent> ModbusMasters { get; set; }


    }
}
