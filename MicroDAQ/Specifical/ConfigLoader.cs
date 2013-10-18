using System;
using System.Collections.Generic;
using System.Text;
using MicroDAQ.DBUtility;
using System.Data;

namespace MicroDAQ.Specifical
{
    public static class ConfigLoader
    {
        static string dbFile = "sqlite.db";

        static SQLiteHelper sqlite = new SQLiteHelper(dbFile);

        static void LoadConfig()
        {
            DataSet ds = sqlite.ExecuteQuery("SELECT * FROM modbusgateway");



        }

    }
}
