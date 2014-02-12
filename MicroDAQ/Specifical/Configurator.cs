using System;
using System.Collections.Generic;
using System.Text;
using JonLibrary.Common;
using JonLibrary.Automatic;
using log4net;


namespace MicroDAQ.Specifical
{
    /// <summary>
    /// 用于加载配置文件内关于PLC的信息
    /// </summary>
    internal class Configurator
    {
        /// <summary>
        /// 使用哪个OPCServer
        /// </summary>
        internal string opcServerType = "SimaticNet";
        internal string OpcServerProgramID = string.Empty;

        internal string wordItemFormat;
        internal string wordArrayItemFormat;
        internal string realItemFormat;

        internal List<PLCStationInformation> PlcsInfo;
        internal IniFile ini = null;

        ILog log;


        internal Configurator()
        {
            log = LogManager.GetLogger(this.GetType());
            PlcsInfo = new List<PLCStationInformation>();
        }
        internal void ReadConfigFromFile()
        {
            ini = new IniFile(AppDomain.CurrentDomain.BaseDirectory + "MicroDAQ.ini");

            int plcCount = int.Parse(ini.GetValue("PLCConfig", "Amount"));
            this.opcServerType = ini.GetValue("OpcServer", "Type").Trim();
            this.OpcServerProgramID = ini.GetValue(opcServerType, "ProgramID");
            for (int i = 0; i < plcCount; i++)
            {
                PLCStationInformation plc = new PLCStationInformation();
                PlcsInfo.Add(plc);
                plc.Connection = string.Format(ini.GetValue(opcServerType, "ConnectionString"), i + 1);
                plc.ConnectionStateItem = string.Format(ini.GetValue(opcServerType, "ConnectionState"), plc.Connection);
            }
            //读取Item地址格式           
            wordItemFormat = ini.GetValue(opcServerType, "WordItemFormat");
            wordArrayItemFormat = ini.GetValue(opcServerType, "WordArrayItemFormat");
            realItemFormat = ini.GetValue(opcServerType, "RealItemFormat");
        }


    }
}
