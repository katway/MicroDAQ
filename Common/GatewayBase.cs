﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;


namespace MicroDAQ.Common
{
    public class GatewayBase : IGateway
    {

        /// <summary>
        /// 数据库管理器对象
        /// </summary>
        public IDatabaseManage DatabaseManage { get; set; }

        /// <summary>
        /// Item管理器对象
        /// </summary>
        public IList<MicroDAQ.Common.IDataItemManage> ItemManage { get; set; }

        /// <summary>
        /// 运行状态
        /// </summary>
        public GatewayState GatewayState
        {
            get { return state; }
            set
            {
                OnStateChanging();
                state = value;
                OnStateChanged();
            }
        }

        private GatewayState state;

        /// <summary>
        /// 启动
        /// </summary>
        public virtual void Start()
        { }

        /// <summary>
        /// 暂停
        /// </summary>
        public virtual void Pause()
        { }

        /// <summary>
        /// 继续
        /// </summary>
        public virtual void Continue()
        { }

        /// <summary>
        /// 停止
        /// </summary>
        public virtual void Stop()
        { }

        /// <summary>
        /// 销毁并释放资源
        /// </summary>
        public virtual void Dispose()
        { }

        protected void OnStateChanging()
        {
            if (StateChanging != null)
            { StateChanging(this, null); }
        }
        protected void OnStateChanged()
        {
            if (StateChanged != null)
            { StateChanged(this, null); }
        }

        /// <summary>
        /// 向指定目标推送数据
        /// </summary>
        public virtual void Push()
        {
            foreach (IDatabase db in this.DatabaseManage.DatabaseList)
                foreach (IDataItemManage itemManage in this.ItemManage)
                    foreach (Item item in itemManage.Items)
                    {
                        db.UpdateItem(item);
                    }

        }
        /// <summary>
        /// 运行状态将要发生变化的通知事件
        /// </summary>
        /// <returns></returns>
        public event EventHandler StateChanging;
        /// <summary>
        /// 运行状态已经发生变化的通知事件 
        /// </summary>
        /// <returns></returns>
        public event EventHandler StateChanged;


    }
}
