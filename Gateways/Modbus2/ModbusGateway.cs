using System;
using System.Collections.Generic;
using System.Text;
using MicroDAQ.Common;

namespace MicroDAQ.Gateways.Modbus2
{
    public class ModbusGateway : GatewayBase
    {

        /// <param name="ItemManage">Item管理器对象</param>
        /// <param name="DatabaseManager">Database管理器对象</param>
        public ModbusGateway(IDataItemManage ItemManager, IDatabase DatabaseManager)
        {
            throw new System.NotImplementedException();
        }


        /// <summary>
        /// 启动
        /// </summary>
        public override void Start()
        { }

        /// <summary>
        /// 暂停
        /// </summary>
        public override void Pause()
        { }

        /// <summary>
        /// 继续
        /// </summary>
        public override void Continue()
        { }

        /// <summary>
        /// 停止
        /// </summary>
        public override void Stop()
        { }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
