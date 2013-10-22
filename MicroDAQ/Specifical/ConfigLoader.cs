using System;
using System.Collections.Generic;
using System.Text;
using MicroDAQ.DBUtility;
using System.Data;
using MicroDAQ.Gateways.Modbus2;
using MicroDAQ.Configuration;

namespace MicroDAQ.Specifical
{
    public static class ConfigLoader
    {

        static string dbFile = "sqlite.db";

        static SQLiteHelper sqlite ;

       public static ModbusGatewayInfo[] LoadConfig()
        {
            sqlite = new SQLiteHelper(dbFile);

            string sql = string.Empty;
            sql = "SELECT * FROM modbusgateway;" +
                    "SELECT * FROM modbusMaster;" +
                    "SELECT * FROM modbusSlave;" +
                    "SELECT * FROM modbusVaribale;";

            DataSet ds = sqlite.ExecuteQuery("sql");

            ModbusGatewayInfo[] gatewayInfo = new ModbusGatewayInfo[10];
            gatewayInfo[0] = new ModbusGatewayInfo(1, ds);

            return gatewayInfo;


        }


        //private static void GetModbusMasterAgent()
        //{
        //    //从站与变量关系
        //    foreach (var slave in slaveList)
        //    {
        //        List<ModbusVariableInfo> listVariable = new List<ModbusVariableInfo>();
        //        foreach (var variableInfo in Variablelist)
        //        {
        //            if (slave.serialID == variableInfo.serialID)
        //            {
        //                listVariable.Add(variableInfo);
        //                Variablelist.Remove(variableInfo);
        //            }

        //        }
        //        slave.modbusVariables = listVariable;
        //    }
        //    //主从站关系
        //    foreach (var master in masterList)
        //    {
        //        List<ModbusSlaveInfo> list = new List<ModbusSlaveInfo>();
        //        foreach (var slave in slaveList)
        //        {
        //            if (master.serialID == slave.serialID)
        //            {
        //                list.Add(slave);
        //                slaveList.Remove(slave);
        //            }//字段不对
        //        }
        //        master.modbusSlaves = list;
        //    }
        //    for (int i = 0; i < masterList.Count; i++)
        //    {
        //        ModbusMasterAgent agent = new ModbusMasterAgent(masterList[i]);
        //        agentlist.Add(agent);
        //    }

        //}

    }
}
