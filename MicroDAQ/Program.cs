using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using JonLibrary.Common;
namespace MicroDAQ
{
    static class Program
    {
        public static bool BeQuit;
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool createNew;
            //try
            //{
            //Console.WriteLine(Application.ProductName);
            using (System.Threading.Mutex m = new System.Threading.Mutex(true, "Global\\" + Application.ProductName, out createNew))
            {
                if (createNew)
                {
                    IniFile ini = new IniFile(AppDomain.CurrentDomain.BaseDirectory + "MicroDAQ.ini");

                    DatabaseManager = new DatabaseManager(ini.GetValue("Database", "Address"),
                                                        ini.GetValue("Database", "Port"),
                                                        ini.GetValue("Database", "Database"),
                                                        ini.GetValue("Database", "Username"),
                                                        ini.GetValue("Database", "Password"));
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    MainForm frmMain = null;

                    while (!BeQuit)
                        try
                        {
                            frmMain = new MainForm();
                            Application.Run(frmMain);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("OH. NO!" + ex.ToString());
                        }
                        finally
                        {
                            if (frmMain != null) frmMain.Dispose();
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

        public static MachineManager MeterManager = new MachineManager();
        public static DatabaseManager DatabaseManager;// = new DatabaseManager();
    }
}
