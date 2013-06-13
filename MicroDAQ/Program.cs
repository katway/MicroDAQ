using System;
using System.Collections.Generic;
using System.Windows.Forms;
using JonLibrary.Common;
using JonLibrary.Automatic;
using MicroDAQ.Database;
using MicroDAQ.Gateway;
using log4net;
using MicroDAQ.DataItem;
namespace MicroDAQ
{
    static class Program
    {
        public static int waitMillionSecond = 180000;
        public static bool BeQuit;
        public static CycleTask RemoteCycle;
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            //记录错误日志
            log.Error("error", new Exception("发生了一个异常"));
            //记录严重错误
            log.Fatal("fatal", new Exception("发生了一个致命错误"));
            //记录一般信息
            log.Info("info");
            //记录调试信息
            log.Debug("debug");
            //记录警告信息
            log.Warn("warn");
            Console.WriteLine("日志记录完毕。");
            Console.Read();



            #region 处理来自参数的快速启动请求，跳过对OPCSERVER的三分钟等待
            foreach (string arg in args)
            {
                if (arg.Contains("fast"))
                {
                    waitMillionSecond = 1000;
                    break;
                }

            }
            #endregion
            bool createNew;
            //try
            //{
            //Console.WriteLine(Application.ProductName);
            using (System.Threading.Mutex m = new System.Threading.Mutex(true, "Global\\" + Application.ProductName, out createNew))
            {
                if (createNew)
                {

                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);

                    Form MainForm = null;
                    while (!BeQuit)
                        try
                        {
                            MainForm = new MainForm();
                            //frmMain = new TestAlarm();
                            Application.Run(MainForm);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("OH. NO!" + ex.ToString());
                        }
                        finally
                        {
                            if (MainForm != null) MainForm.Dispose();
                        }
                    Environment.Exit(Environment.ExitCode);
                }
                else
                {
                    MessageBox.Show("程序已经在运行，无法再次启动。", "已启动", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            //}
            //catch
            //{
            //    MessageBox.Show("Only one instance of this application is allowed!");
            //}



        }


        public static OpcGateway opcGateway = null;
        public static MachineManager MeterManager = new MachineManager();
        public static DatabaseManage DatabaseManager;// = new DatabaseManager();
        public static DataItemManager M;
        public static FlowAlertManager M_flowAlert;

    }
}
