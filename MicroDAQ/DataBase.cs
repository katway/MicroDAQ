#define DEBUG       //用于程序调试，软件发布时请关闭此声明。
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
//using System.Data.SqlClient;
//using Oracle.DataAccess.Client;
//using System.Data;
using System.Data.OracleClient;
using JonLibrary.Runtime;

#if DEBUG
using System.Windows.Forms;
#endif
namespace Linfen_WareHouse
{
    class DataBase
    {
        public delegate void StateChangeHandler(string DBState);
        public event StateChangeHandler DBStateChange;

        public string oprationState = string.Empty;
        private string connectionString;

        public OracleConnection Connection = null;
        OracleCommand cmd = null;
        OracleDataAdapter dataAdapter;
        OracleDataReader dataReader;
        public string ConnectionDebugMessage;
        public static string GetTaskSQlCommand = "select task_id,task_type,status,from_addr,to_addr,create_time,product_id,pack_unit,pack_num,pallet_id,product_name ,channel_id ,TASK_PRIORITY from  v_wms_task ";
        public string GetTaskSQLCommand2 = "select task_id,task_type,status,from_addr,to_addr,create_time,product_id,pack_unit,pack_num,pallet_id,product_name ,channel_id ,TASK_PRIORITY from  v_wms_task WHERE channel_id ={0}";
        public DataBase(string host_ip, int port, string sid, string user_id, string password)
        {
            connectionString = string.Format("Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={0})(PORT={1})))(CONNECT_DATA=(SID = {2})));User Id={3};Password={4};", host_ip, port, sid, user_id, password);
            ConnectionDebugMessage = string.Format("HOST={0};PORT={1};SID={2};User Id={3}.", host_ip, port, sid, user_id);
        }
        /// <summary>
        /// 连接到数据库
        /// </summary>
        public bool Connect()
        {
            bool sucess;
            try
            {
                Connection = new OracleConnection(connectionString);
                Connection.Open();
                sucess = true;
            }
            catch (Exception ex)
            {
                sucess = false;
            }
            return sucess;
        }
        /// <summary>
        /// 断开到数据库的连接
        /// </summary>
        public void Disconnect()
        {
            if (Connection.State != ConnectionState.Closed)
            { Connection.Close(); }
        }

        public CWarehouseTask GetTask2(int channel)
        {
            if (channel == 4) channel = 0;
            bool sucess = false;
            CWarehouseTask newTask = null;
            if (Connection.State != ConnectionState.Open) { return null; }
            try
            {
                using (OracleCommand cmd = new OracleCommand())
                {
                    cmd.Connection = Connection;
                    //MessageBox.Show(GetTaskSQLCommand2);
                    cmd.CommandText = string.Format(GetTaskSQLCommand2, channel);
                    cmd.CommandType = CommandType.Text;
                    using (dataReader = cmd.ExecuteReader())
                    {
                        if (dataReader.Read())
                        {//如果有任务
                            OprationState = "正在读取任务";
                            #region 读取
                            //#region task_id,task_type,status,from_addr,to_addr,create_time,product_id,pack_unit,pack_num,pallet_id
                            CWarehouseTask tmpTask = new CWarehouseTask();
                            tmpTask.TaskID = dataReader.GetInt32(0);
                            tmpTask.TaskType = (CWarehouseTask.ETaskType)int.Parse(dataReader.GetString(1));
                            tmpTask.State = int.Parse(dataReader.GetString(2));
                            
                            //调整地址格式
                            string tmp = dataReader.GetString(3);
                            tmp = tmp.Replace("LF00", "WH01");
                            tmpTask.SourceAddress = tmp;
                            tmp = dataReader.GetString(4);
                            tmp = tmp.Replace("LF00", "WH01");
                            tmp = tmp.Replace("CR0115", "CR0105");
                            tmpTask.DestinationAddress = tmp;
                            
                            //调整空托盘直出任务类型
                            if ((tmpTask.TaskType == CWarehouseTask.ETaskType.空托盘返库) && (tmpTask.DestinationAddress == "CR0105"))
                            { tmpTask.TaskType = CWarehouseTask.ETaskType.空托盘直出; }

                            tmpTask.CreateTime = dataReader.GetDateTime(5);
                            tmpTask.ProductID = dataReader.GetString(6);
                            tmpTask.PackingUnit = dataReader.GetString(7);
                            tmpTask.PackingNumber = dataReader.GetInt32(8);
                            tmpTask.PalletID = dataReader.GetString(9);
                            tmpTask.ProductName = dataReader.GetString(10);
                            if (dataReader[11] != null)
                            {
                                int ch = dataReader.GetInt32(11);
                                tmpTask.ChannelID = (ch == 0) ? (4) : (ch);
                            }
                            else
                            { tmpTask.ChannelID = 4; }
                            tmpTask.Priority = dataReader.GetInt32(12);
                            //tmpTask.RunningState = CWarehouseTask.EState.创建;
                            #endregion
                            #region 创建
                            try
                            {
                                newTask = new CWarehouseTask(tmpTask.TaskID,
                                                            tmpTask.Req_ID,
                                                            (int)tmpTask.TaskType,
                                                            tmpTask.State,
                                                            tmpTask.SourceAddress,
                                                            tmpTask.DestinationAddress,
                                                            tmpTask.CreateTime,
                                                            tmpTask.ProductID,
                                                            tmpTask.PackingUnit,
                                                            tmpTask.PackingNumber,
                                                            tmpTask.Num,
                                                            tmpTask.Priority,
                                                            tmpTask.PalletID,
                                                            tmpTask.ProductName,
                                                            tmpTask.ChannelID);
                                sucess = true;
                            }
                            catch (Exception ex1)
                            {
                                sucess = false;
                                Program.ErrorManager.Add(new RuntimeError(oprationState, RuntimeError.EVisibility.Programer, new Exception("生成任务实例出现异常,此异常并不会影响程序执行结果。", ex1)));
                            }
                            #endregion
                        }
                        else
                        {//无任务
                            sucess = false;
                        }
                    }

                }
            }
            catch (Exception ex) // catches any other error
            {
                sucess = false;
                OprationState = "读取任务过程中出现数据库相关的操作错误";
                Program.ErrorManager.Add(new RuntimeError(oprationState, RuntimeError.EVisibility.Programer, ex));

            }
            //如果无错误，返回newTask,否则返回null
            return ((sucess) && newTask != null) ? (newTask) : (null);

        }


        //SPPG_WMS.sp_pallet_task_init
        //SPPG_WMS.sp_pallet_task_init(pallet_num in number,pi_Task_id out number,ri_ret out number, rs_ret out varchar2).

        /// <summary>
        /// 空托盘返库请求
        /// </summary>
        /// <param name="palletNumber"></param>
        /// <returns></returns>
        public bool RunProcedure(int palletNumber)
        {
            bool sucess = false;
            OracleHelper.LocalConnectionString = connectionString;

            if (!OracleHelper.CanConnectDb(true))
            {
                OprationState = string.Format("空托盘返库执行存储过程出错。\r\n错误原因：数据库未连接！");
                Program.ErrorManager.Add(new RuntimeError(oprationState, RuntimeError.EVisibility.User));
                return false;
            }
            OracleTransaction tran = OracleHelper.BeginTransaction();

            OracleParameter[] param = new OracleParameter[] {new OracleParameter("pallet_num", OracleType.Int32),
                                                            new OracleParameter("pi_Task_id", OracleType.Int32),
                                                            new OracleParameter("ri_ret", OracleType.Int32),
                                                            new OracleParameter("rs_ret", OracleType.VarChar,512)
                                                                        };
            param[0].Value = palletNumber;

            param[0].Direction = ParameterDirection.Input;
            param[1].Direction = ParameterDirection.Output;
            param[2].Direction = ParameterDirection.Output;
            param[3].Direction = ParameterDirection.Output;

            try
            {
                OracleHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "SPPG_WMS.sp_pallet_task_init", param);

                if ((int)param[2].Value != 0)
                {
                    OracleHelper.RollbackTransaction(tran);
                    sucess = false;
                    OprationState = string.Format("空托盘返库执行存储过程出错。托盘数量：{0}\r\n可能是由于：{1}", palletNumber, param[3].Value);
                    Program.ErrorManager.Add(new RuntimeError(oprationState, RuntimeError.EVisibility.User));
                }
                else
                {
                    sucess = true;
                }
            }
            catch (System.Exception ex)
            {
                OracleHelper.RollbackTransaction(tran);
                sucess = false;
                OprationState = string.Format("空托盘返库执行存储过程出错，托盘数量：{0}\r\n", palletNumber);
                Program.ErrorManager.Add(new RuntimeError(oprationState, RuntimeError.EVisibility.Programer, ex));
            }
            finally
            {
                if (sucess) { OracleHelper.CommitTransaction(tran); }
            }

            return sucess;
        }

        /// <summary>
        /// 执行立库任务的存储过程
        /// </summary>
        /// <param name="taskID">任务ID</param>
        /// <param name="taskState">任务状态</param>
        /// <returns></returns>
        public bool RunProcedure(int taskID, int taskState)
        {
            bool sucess = false;

            OracleHelper.LocalConnectionString = connectionString;

            if (!OracleHelper.CanConnectDb(true))
            {
                OprationState = string.Format("任务{0}执行存储过程出错，任务状态码：{1}\r\n错误原因：数据库未连接！", taskID, taskState);
                Program.ErrorManager.Add(new RuntimeError(oprationState, RuntimeError.EVisibility.User));
                return false;
            }
            OracleTransaction tran = OracleHelper.BeginTransaction();

            OracleParameter[] param = new OracleParameter[] {new OracleParameter("pi_Task_id", OracleType.Int32),
                                                                        new OracleParameter("ps_Status", OracleType.VarChar  ),
                                                                        new OracleParameter("ri_ret", OracleType.Int32),
                                                                        new OracleParameter("rs_ret", OracleType.VarChar,512)
                                                                        };
            param[0].Value = taskID;
            param[1].Value = taskState.ToString();//.ToCharArray ()[0];

            param[0].Direction = ParameterDirection.Input;
            param[1].Direction = ParameterDirection.Input;
            param[2].Direction = ParameterDirection.Output;
            param[3].Direction = ParameterDirection.Output;

            try
            {
                OracleHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "SPPG_WMS.SP_TASK", param);

                if ((int)param[2].Value != 0)
                {
                    OracleHelper.RollbackTransaction(tran);
                    sucess = false;
                    OprationState = string.Format("任务{0}执行存储过程出错，任务状态码：{1}\r\n可能是由于：{2}", taskID, taskState, param[3].Value);
                    Program.ErrorManager.Add(new RuntimeError(oprationState, RuntimeError.EVisibility.User));
                }
                else
                {
                    sucess = true;
                }
            }
            catch (System.Exception ex)
            {
                OracleHelper.RollbackTransaction(tran);//.Rollback();
                sucess = false;
                OprationState = string.Format("任务{0}执行存储过程出错，任务状态码：{1}\r\n", taskID, taskState);
                Program.ErrorManager.Add(new RuntimeError(oprationState, RuntimeError.EVisibility.Programer, ex));
            }
            finally
            {
                if (sucess) { OracleHelper.CommitTransaction(tran); }
            }

            return sucess;
        }

        public bool UpdateAlarm(string deviceCode, string deviceName, string faultMessage, string faultType, TimeSpan? faultDuration)
        {

            //Program.ErrorManager.Add(new RuntimeError(deviceCode + deviceName + faultMessage + faultType + faultDuration.ToString(), RuntimeError.EVisibility.Programer));
            if (faultDuration.HasValue)
            { return UpdateAlarm(deviceCode, deviceName, faultMessage, faultType, faultDuration.Value); }
            else
            { return false; }

        }
        public bool UpdateAlarm(string deviceCode, string deviceName, string faultMessage, string faultType, TimeSpan faultDuration)
        {
            //SPPG_WMS.sp_error_log(ls_device_id in varchar2,ls_device_name in varchar2,ls_error_inf in varchar2, ri_ret out number, rs_ret out varchar2)
            //SPPG_WMS.sp_error_log(ls_device_id in varchar2,ls_device_name in varchar2,ls_error_inf in varchar2,li_Error_time in number,ls_Error_type in varchar2,ri_ret out number, rs_ret out varchar2).
            bool sucess = false;

            OracleHelper.LocalConnectionString = connectionString;

            if (!OracleHelper.CanConnectDb(true))
            {
                OprationState = "上传设备错误信息失败,数据库未连接！";
                Program.ErrorManager.Add(new RuntimeError(oprationState, RuntimeError.EVisibility.User));
                return false;
            }
            OracleTransaction tran = OracleHelper.BeginTransaction();

            OracleParameter[] param = new OracleParameter[] {new OracleParameter("ls_device_id", OracleType.VarChar ),
                                                                        new OracleParameter("ls_device_name", OracleType.VarChar  ),
                                                                        new OracleParameter("ls_error_inf", OracleType.VarChar ),
                                                                        new OracleParameter ("li_Error_time", OracleType.Int32),
                                                                        new OracleParameter ("ls_Error_type",OracleType.VarChar ),
                                                                        new OracleParameter ("ri_ret",OracleType.Int32    ),
                                                                        new OracleParameter("rs_ret", OracleType.VarChar,512)
                                                                        };
            param[0].Value = deviceCode;
            param[1].Value = deviceName;
            param[2].Value = faultMessage;
            param[3].Value = (int)(faultDuration.Seconds);
            param[0].Direction = ParameterDirection.Input;
            param[1].Direction = ParameterDirection.Input;
            param[2].Direction = ParameterDirection.Input;
            param[3].Direction = ParameterDirection.Input;
            param[4].Direction = ParameterDirection.Input;
            param[5].Direction = ParameterDirection.Output;
            param[6].Direction = ParameterDirection.Output;

            try
            {
                OracleHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "SPPG_WMS.sp_error_log", param);

                if ((int)param[5].Value != 0)
                {
                    OracleHelper.RollbackTransaction(tran);
                    sucess = false;
                    OprationState = string.Format("上传设备错误信息失败!代码:{0}可能是由于：{1}", param[5].Value, param[6].Value);
                    Program.ErrorManager.Add(new RuntimeError(oprationState, RuntimeError.EVisibility.User));
                }
                else
                {
                    sucess = true;
                }
            }
            catch (System.Exception ex)
            {
                OracleHelper.RollbackTransaction(tran);//.Rollback();
                sucess = false;
                OprationState = string.Format("上传设备错误信息失败!数据库操作错误");
                Program.ErrorManager.Add(new RuntimeError(oprationState, RuntimeError.EVisibility.Programer, ex));
            }
            finally
            {
                if (sucess)
                {
                    OracleHelper.CommitTransaction(tran);
                    //Program.ErrorManager.Add(new RuntimeError("上传设备报警信息-成功", RuntimeError.EVisibility.Programer));
                }
            }

            return sucess;
        }






        /// <summary>
        /// 数据库操作状态
        /// </summary>
        public string OprationState
        {
            get { return oprationState; }
            set
            {
                oprationState = value;

                if (DBStateChange != null)
                {
                    DBStateChange(oprationState);          //触发数据库操作状态变化事件
                }
            }
        }
    }

}
