using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace MicroDAQ.Gateway
{
    public abstract class GatewayBase : IGateway
    {
        /// <summary>
        /// 运行状态
        /// </summary>
        public Gateway.RunningState RunningState
        {
            get { return state; }
            set
            {
                OnStateChanging();
                state = value;
                OnStateChanged();
            }
        }
        private RunningState state;
        /// <summary>
        /// 启动
        /// </summary>
        public abstract void Start();
        /// <summary>
        /// 暂停
        /// </summary>
        public abstract void Pause();
        /// <summary>
        /// 继续
        /// </summary>
        public abstract void Continue();
        /// <summary>
        /// 停止
        /// </summary>
        public abstract void Stop();
        /// <summary>
        /// 销毁并释放资源
        /// </summary>
        public abstract void Dispose();

        protected void OnStateChanging()
        {
            if (StateChanging() != null)
            { StateChanging(); }
        }
        protected void OnStateChanged()
        {
            if (StateChanged() != null)
            { StateChanged(); }
        }
        /// <summary>
        /// 运行状态将要发生变化的通知事件
        /// </summary>
        /// <returns></returns>
        public abstract EventHandler StateChanging();
        /// <summary>
        /// 运行状态已经发生变化的通知事件 
        /// </summary>
        /// <returns></returns>
        public abstract EventHandler StateChanged();
    }
}
