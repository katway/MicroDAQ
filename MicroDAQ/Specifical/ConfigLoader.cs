using System;
using System.Collections.Generic;
using System.Text;
using MicroDAQ.DBUtility;

namespace MicroDAQ.Specifical
{
    public static class ConfigLoader
    {
        static string dbFile = "sqlite.db";

        static SQLiteHelper sqlite = new SQLiteHelper(dbFile);

        static void LoadConfig()
        {
           sqlite.ExecuteQuery



        }

    }
}
