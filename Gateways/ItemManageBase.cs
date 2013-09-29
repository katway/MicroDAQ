using System;
using System.Collections.Generic;
using System.Text;

namespace MicroDAQ.Gateways
{
    public class ItemManageBase : MicroDAQ.Common.IDataItemManage
    {
        public IList<Common.Item> Items
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 开始数据采集和同步
        /// </summary>
        public void StartSynchronize()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 停止数据采集和同步
        /// </summary>
        public void StopSynchronize()
        {
            throw new System.NotImplementedException();
        }
    }
}
