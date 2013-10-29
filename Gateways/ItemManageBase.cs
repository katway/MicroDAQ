using System;
using System.Collections.Generic;
using System.Text;

namespace MicroDAQ.Gateways
{
    public abstract class ItemManageBase : MicroDAQ.Common.IDataItemManage
    {
        public List<Common.IItem> Items
        {
            get;
            set;
        }
       
        

        /// <summary>
        /// 用于数据采集的CycleTask对象（线程）
        /// </summary>
        public JonLibrary.Automatic.CycleTask SynchronizeCycle
        {
            get;
            set;
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
            if (this.SynchronizeCycle != null)
                this.SynchronizeCycle.Run(new System.Threading.ThreadStart(this.ReadWrite), System.Threading.ThreadPriority.BelowNormal);
        }

        /// <summary>
        /// 停止数据采集和同步
        /// </summary>
        public void StopSynchronize()
        {
            if (this.SynchronizeCycle != null)
                this.SynchronizeCycle.Quit();
        }

        /// <summary>
        /// 暂停数据采集和同步
        /// </summary>
        public void PauseSynchronize()
        {
            if (this.SynchronizeCycle != null)
                this.SynchronizeCycle.Pause();
        }

        /// <summary>
        /// 读取和写入数据
        /// </summary>
        public abstract void ReadWrite();
    }
}
