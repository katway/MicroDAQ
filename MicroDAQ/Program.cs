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

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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

            #region 处理来自参数的调整模式请求，不添加错误捕获和重新启动
            foreach (string arg in args)
            {
                if (arg.Contains("debug"))
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);

                    Form MainForm = null;
                    while (!BeQuit)
                    {
                        MainForm = new MainForm();
                        Application.Run(MainForm);
                        if (MainForm != null) MainForm.Dispose();
                    }
                    Environment.Exit(Environment.ExitCode);
                    break;
                }

            }
            #endregion
            bool createNew;
            using (System.Threading.Mutex m = new System.Threading.Mutex(true, "Global\\" + Application.ProductName, out createNew))
            {
                if (createNew)
                {

                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);

                    Form MainForm = null;
                    while (!BeQuit)
                    {
                        try
                        {
                            MainForm = new MainForm();
                            Application.Run(MainForm);
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                        }
                        finally
                        {
                            if (MainForm != null) MainForm.Dispose();
                        }
                    }
                    Environment.Exit(Environment.ExitCode);
                }
                else
                {
                    MessageBox.Show("程序已经在运行，无法再次启动。", "已启动", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
        }


        public static OpcGateway opcGateway = null;
        public static MachineManager MeterManager = new MachineManager();
        public static DatabaseManage DatabaseManager;// = new DatabaseManager();
        public static DataItemManager M;
        public static FlowAlertManager M_flowAlert;

    }
}
