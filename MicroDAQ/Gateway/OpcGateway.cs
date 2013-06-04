using System;
using System.Collections.Generic;
using System.Text;
using JonLibrary.OPC;
using JonLibrary.Automatic;
using JonLibrary.Common;
using System.Threading;
using System.Data;
using MicroDAQ.Database;
using MicroDAQ.DataItem;

namespace MicroDAQ.Gateway
{
    public class OpcGateway : GatewayBase
    {


        /// <summary>
        /// 使用多个ItemManage创建OpcGateway实例
        /// </summary>
        /// <param name="itemManager"></param>
        public OpcGateway(IList<MicroDAQ.DataItem.IDataItemManage> itemManager, IList<IDatabaseManage> databaseManager)
        {
            ItemManagers = itemManager;

            UpdateCycle = new CycleTask();
            RemoteCtrlCycle = new CycleTask();
            UpdateCycle.WorkStateChanged += new CycleTask.dgtWorkStateChange(UpdateCycle_WorkStateChanged);
            RemoteCtrlCycle.WorkStateChanged += new CycleTask.dgtWorkStateChange(RemoteCtrl_WorkStateChanged);
        }

        void UpdateCycle_WorkStateChanged(JonLibrary.Automatic.RunningState state)
        {
            this.RunningState = Enum.GetName(Gateway.RunningState, state.ToString());
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



        Meter meter;
        Controller MetersCtrl;


        protected virtual void Update()
        {
            foreach (IDatabaseManage dbMgr in this.DatabaseManagers)
            {
                foreach (DataItemManager mgr in this.ItemManagers)
                {
                    foreach (Item item in mgr.Items)
                    {
                        dbMgr.UpdateItem(item);
                    }
                }
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
                DataRow[] Rows = Program.DatabaseManager.GetRemoteControl();
                if (Rows != null)
                    foreach (var row in Rows)
                    {
                        //MessageBox.Show((row["cycle"].ToString() != null).ToString());
                        foreach (var mt in Program.MeterManager.CTMeters.Values)
                            mt.SetCommand(++running,
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

        /// <summary>
        /// 启动
        /// </summary>
        public override void Start()
        {
            UpdateCycle.Run(this.Update, System.Threading.ThreadPriority.BelowNormal);
            RemoteCtrlCycle.Run(this.remoteCtrl, System.Threading.ThreadPriority.BelowNormal);
        }
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
        /// 根据参数暂停更新任务或控制任务
        /// </summary>
        /// <param name="pauseUpdate">是否暂停更新</param>
        /// <param name="pauseRemoteCtrl">是否暂停控制</param>
        public void Pasue(bool pauseUpdate, bool pauseRemoteCtrl)
        {
            this.UpdateCycle.SetPause = pauseUpdate;
            this.RemoteCtrlCycle.SetPause = pauseRemoteCtrl;
        }
        /// <summary>
        /// 暂停参数指定的任务对象
        /// </summary>
        /// <param name="task">要暂停的任务对象</param>
        public void Pause(CycleTask task)
        {
            task.SetPause = true;
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
            task.SetPause = false;
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
        /// 根据参数暂停更新任务或控制任务
        /// </summary>
        /// <param name="ExitUpdate">是否暂停更新</param>
        /// <param name="ExitRemoteCtrl">是否暂停控制</param>
        private void Stop(bool ExitUpdate, bool ExitRemoteCtrl)
        {
            this.UpdateCycle.SetExit = ExitUpdate;
            this.RemoteCtrlCycle.SetExit = ExitRemoteCtrl;
        }
        /// <summary>
        /// 暂停参数指定的任务对象
        /// </summary>
        /// <param name="task">要暂停的任务对象</param>
        private void Stop(CycleTask task)
        {
            task.SetExit = true;
        }
        #endregion

        public override void Dispose()
        {

        }
    }
}
