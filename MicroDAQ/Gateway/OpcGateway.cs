using System;
using System.Collections.Generic;
using System.Text;
using JonLibrary.OPC;
using JonLibrary.Automatic;
using System.Threading;
using System.Data;
using MicroDAQ.Database;
using MicroDAQ.DataItem;
using log4net;
using MicroDAQ.Specifical;

namespace MicroDAQ.Gateway
{
    public class OpcGateway : GatewayBase
    {
        ILog log;
        public override void Dispose()
        {
            UpdateCycle.Quit();
            RemoteCtrlCycle.Quit();
        }

        /// <summary>
        /// 使用多个ItemManage创建OpcGateway实例
        /// </summary>
        /// <param name="itemManagers"></param>
        public OpcGateway(IList<MicroDAQ.DataItem.IDataItemManage> itemManagers, IList<IDatabaseManage> databaseManagers)
        {
            log = LogManager.GetLogger(this.GetType());
            this.ItemManagers = itemManagers;
            this.DatabaseManagers = databaseManagers;

            UpdateCycle = new CycleTask();
            RemoteCtrlCycle = new CycleTask();
            UpdateCycle.WorkStateChanged += new CycleTask.WorkStateChangeEventHandle(UpdateCycle_WorkStateChanged);
            RemoteCtrlCycle.WorkStateChanged += new CycleTask.WorkStateChangeEventHandle(RemoteCtrlCycle_WorkStateChanged);
        }


        void UpdateCycle_WorkStateChanged(JonLibrary.Automatic.RunningState state)
        {
            if ((UpdateCycle.State == JonLibrary.Automatic.RunningState.Running) || (RemoteCtrlCycle.State == JonLibrary.Automatic.RunningState.Running))
            {
                this.RunningState = Gateway.RunningState.Running;
            }
            else
            {
                this.RunningState = Gateway.RunningState.Stopped;
            }
        }
        void RemoteCtrlCycle_WorkStateChanged(JonLibrary.Automatic.RunningState state)
        {
            if ((UpdateCycle.State == JonLibrary.Automatic.RunningState.Running) || (RemoteCtrlCycle.State == JonLibrary.Automatic.RunningState.Running))
            {
                this.RunningState = Gateway.RunningState.Running;
            }
            else
            {
                this.RunningState = Gateway.RunningState.Stopped;
            }
        }
        /// <summary>
        /// 数据项管理器
        /// </summary>
        public IList<IDataItemManage> ItemManagers { get; private set; }
        /// <summary>
        /// 数据库管理器
        /// </summary>
        public IList<IDatabaseManage> DatabaseManagers { get; set; }
        public CycleTask UpdateCycle { get; private set; }
        public CycleTask RemoteCtrlCycle { get; private set; }


        protected virtual void Update()
        {
            try
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
            catch (Exception ex)
            {
                log.Error(new Exception("运行期间出现一个错误！", ex));
            }
        }


        private void update2()
        {
            foreach (var item in Program.M.Items)
            {
                Program.DatabaseManager.UpdateMeterValue(item.ID, (int)item.Type, (int)item.State, (float)item.Value, 0.0f, 0.0f, item.Quality);
            }
            foreach (var item in Program.M_flowAlert.Items)
            {
                float t = 0.0f;
                if ((item.Value == 0) && ((item.State == DataState.正常) || (item.State == DataState.已启动)))
                    t = 28.3f;
                if (item.Value == 2)
                    t = 0.0f;
                Program.DatabaseManager.UpdateMeterValue(item.ID + 10000, (int)16, (int)item.State, t, 0.0f, 0.0f, item.Quality);
                Console.WriteLine(item.ToString());
            }

        }
        int running;
        public void remoteCtrl()
        {
            try
            {
                DataRow[] Rows = this.DatabaseManagers[0].GetRemoteControl();
                if (Rows != null)
                    foreach (var row in Rows)
                    {
                        //MessageBox.Show((row["cycle"].ToString() != null).ToString());
                        foreach (var mt in Program.MeterManager.CTMeters.Values)
                            mt.SetCommand(++running % ushort.MaxValue,
                                          int.Parse(row["id"].ToString()),
                                          int.Parse(row["command"].ToString()),
                                          int.Parse((row["cycle"] != null) ? (row["cycle"].ToString()) : ("0"))
                                          );
                        Thread.Sleep(500);
                    }
                System.Threading.Thread.Sleep(500);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                System.Threading.Thread.Sleep(3000);
            }
        }

        #region Start()

        public override void Start()
        {
            UpdateCycle.Run(this.Update, System.Threading.ThreadPriority.BelowNormal);
            RemoteCtrlCycle.Run(this.remoteCtrl, System.Threading.ThreadPriority.BelowNormal);
        }

        /// <summary>
        /// 启动
        /// </summary>
        public void Start(string pid)
        {
            foreach (var manager in this.ItemManagers)
                manager.Connect(pid, "127.0.0.1");
            Start();
        }
        #endregion

        #region Pasue()
        /// <summary>
        /// 暂停更新和控制
        /// </summary>
        public override void Pause()
        {
            this.Pause(this.UpdateCycle);
            this.Pause(this.RemoteCtrlCycle);
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
            this.Continue(this.RemoteCtrlCycle);
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
            this.Stop(this.RemoteCtrlCycle);
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
