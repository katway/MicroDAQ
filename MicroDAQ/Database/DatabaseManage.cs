using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace MicroDAQ.Database
{
    public class DatabaseManage : IDatabaseManage
    {
        static bool instanceFlag = false;
        public DatabaseManage()
            : this(@"VWINTECH-201\SQL2000", "3306", "opcmes2_bdgk_test", "sa", "")
        { }
        public SqlConnection GetdataConnection { get; set; }
        public SqlConnection UpdateConnection { get; set; }
        public string ConnectionString;
        public DatabaseManage(string svrAddress, string port, string dbName, string dbUser, string dbUserPassword)
        {
            if (instanceFlag)
            { throw new Exception("不允许创建多实例"); }
            else
            {
                ConnectionString = string.Format("Data Source={0};Initial Catalog={2};User Id={3};Password={4};", svrAddress, port, dbName, dbUser, dbUserPassword);
                //ConnectionString = string.Format("server={0};port={1};database={2};uid={3};pwd={4};charset=utf8;", svrAddress, port, dbName, dbUser, dbUserPassword);
                GetdataConnection = new SqlConnection(ConnectionString);
                UpdateConnection = new SqlConnection(ConnectionString);
                getRemoteAdapter = new SqlDataAdapter("SELECT * FROM v_remotecontrol WITH(nolock) WHERE cmdstate=1 AND ID IS NOT NULL AND cycle is not null", GetdataConnection);
                getRemoteControl = new SqlCommand();

                GetdataConnection.StateChange += new StateChangeEventHandler(Connection_StateChange);
                instanceFlag = true;
            }
        }
        SqlCommand getRemoteControl;
        void Connection_StateChange(object sender, StateChangeEventArgs e)
        {
            Console.Write("Database connection ");
            switch (GetdataConnection.State)
            {
                case ConnectionState.Broken:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case ConnectionState.Closed:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case ConnectionState.Executing:
                case ConnectionState.Connecting:
                case ConnectionState.Fetching:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case ConnectionState.Open:
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    break;
            }
            Console.WriteLine(GetdataConnection.State);
            Console.ResetColor();
        }

        DataTable tblResult = new DataTable("Unname");
        SqlDataAdapter getRemoteAdapter;
        DataRow[] result = null;
        public DataRow[] GetRemoteControl()
        {
            try
            {
                switch (GetdataConnection.State)
                {
                    case ConnectionState.Broken:
                        GetdataConnection.Close();
                        break;
                    case ConnectionState.Closed:
                        GetdataConnection.Open();
                        break;
                    case ConnectionState.Open:
                        tblResult.Rows.Clear();
                        getRemoteAdapter.Fill(tblResult);
                        result = new DataRow[tblResult.Rows.Count];
                        tblResult.Rows.CopyTo(result, 0);


                        foreach (var row in result)
                        {
                            string sql = string.Format("Update remotecontrol SET cmdstate= {0} WHERE slave= {1}", 2, row["id"].ToString());//, Connection);
                            SqlCommand Command = new SqlCommand(sql, GetdataConnection);
                            Command.ExecuteNonQuery();
                        }


                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                GetdataConnection.Close();
            }
            return result;
        }
        private void RunProcedure(SqlCommand command, string storedProcName, SqlParameter[] parameters)
        {
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = storedProcName;
            command.Parameters.AddRange(parameters);
            command.ExecuteNonQuery();
        }



        public bool UpdateItem(DataItem.Item item)
        {
            bool success = false;
            try
            {
                lock (this)
                {
                    if (UpdateConnection.State == ConnectionState.Open)
                    {
                        SqlCommand Command = new SqlCommand();
                        Command.Connection = UpdateConnection;
                        Command.CommandType = CommandType.StoredProcedure;
                        Command.CommandText = "UpdateMeterValue";


                        SqlParameter param0 = new SqlParameter("@MeterID", SqlDbType.Int);
                        SqlParameter param1 = new SqlParameter("@MeterType", SqlDbType.Int);
                        SqlParameter param2 = new SqlParameter("@MeterState", SqlDbType.Int);
                        SqlParameter param3 = new SqlParameter("@Value1", SqlDbType.Float);
                        SqlParameter param4 = new SqlParameter("@Value2", SqlDbType.Float);
                        SqlParameter param5 = new SqlParameter("@Value3", SqlDbType.Float);
                        SqlParameter param6 = new SqlParameter("@ResultState", SqlDbType.Int);
                        SqlParameter param7 = new SqlParameter("@RestultMessage", SqlDbType.VarChar, 60);
                        SqlParameter param8 = new SqlParameter("@Quality", SqlDbType.Int);

                        param0.Direction = ParameterDirection.Input;
                        param1.Direction = ParameterDirection.Input;
                        param2.Direction = ParameterDirection.Input;
                        param3.Direction = ParameterDirection.Input;
                        param4.Direction = ParameterDirection.Input;
                        param5.Direction = ParameterDirection.Input;
                        param6.Direction = ParameterDirection.Output;
                        param7.Direction = ParameterDirection.Output;
                        param8.Direction = ParameterDirection.Input;


                        param0.Value = item.ID;
                        param1.Value = item.Type;// MeterType;
                        param2.Value = item.State;// MeterState;
                        param3.Value = item.Value;//
                        param4.Value = 0.0f;
                        param5.Value = 0.0f;
                        param8.Value = item.Quality;

                        Command.Parameters.Add(param0);
                        Command.Parameters.Add(param1);
                        Command.Parameters.Add(param2);
                        Command.Parameters.Add(param3);
                        Command.Parameters.Add(param4);
                        Command.Parameters.Add(param5);
                        Command.Parameters.Add(param8);
                        Command.Parameters.Add(param6);
                        Command.Parameters.Add(param7);

                        try
                        {
                            Command.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                            success = false;
                            Command.Dispose();
                        }
                    }
                    else
                    {
                        switch (UpdateConnection.State)
                        {
                            case ConnectionState.Broken:
                                UpdateConnection.Close();
                                break;
                            case ConnectionState.Closed:
                                UpdateConnection.Open();
                                break;
                        }
                    }
                }
            }
            catch
            { success = false; }
            return success;



        }
        /// <summary>
        /// 使用存储过程UpdateMeterValue更新仪表参数
        /// </summary>
        /// <param name="MeterId">仪表参数项ID</param>
        /// <param name="MeterType">仪表类型</param>
        /// <param name="MeterState">仪表状态</param>
        /// <param name="Value1">仪表数值1</param>
        /// <param name="Value2">仪表数值2</param>
        /// <param name="Value3">仪表数值3</param>
        /// <returns></returns>
        public bool UpdateMeterValue(int MeterId, int MeterType, int MeterState, float Value1, float Value2, float Value3, int Quality)
        {
            bool success = false;
            try
            {
                lock (this)
                {
                    if (UpdateConnection.State == ConnectionState.Open)
                    {
                        SqlCommand Command = new SqlCommand();
                        Command.Connection = UpdateConnection;
                        Command.CommandType = CommandType.StoredProcedure;
                        Command.CommandText = "UpdateMeterValue";
                        SqlParameter param0 = new SqlParameter("@MeterID", SqlDbType.Int);
                        SqlParameter param1 = new SqlParameter("@MeterType", SqlDbType.Int);
                        SqlParameter param2 = new SqlParameter("@MeterState", SqlDbType.Int);
                        SqlParameter param3 = new SqlParameter("@Value1", SqlDbType.Float);
                        SqlParameter param4 = new SqlParameter("@Value2", SqlDbType.Float);
                        SqlParameter param5 = new SqlParameter("@Value3", SqlDbType.Float);
                        SqlParameter param6 = new SqlParameter("@ResultState", SqlDbType.Int);
                        SqlParameter param7 = new SqlParameter("@RestultMessage", SqlDbType.VarChar, 60);
                        SqlParameter param8 = new SqlParameter("@Quality", SqlDbType.Int);

                        param0.Direction = ParameterDirection.Input;
                        param1.Direction = ParameterDirection.Input;
                        param2.Direction = ParameterDirection.Input;
                        param3.Direction = ParameterDirection.Input;
                        param4.Direction = ParameterDirection.Input;
                        param5.Direction = ParameterDirection.Input;
                        param6.Direction = ParameterDirection.Output;
                        param7.Direction = ParameterDirection.Output;
                        param8.Direction = ParameterDirection.Input;


                        param0.Value = MeterId;
                        param1.Value = MeterType;
                        param2.Value = MeterState;
                        param3.Value = Value1;
                        param4.Value = Value2;
                        param5.Value = Value3;
                        param8.Value = Quality;

                        Command.Parameters.Add(param0);
                        Command.Parameters.Add(param1);
                        Command.Parameters.Add(param2);
                        Command.Parameters.Add(param3);
                        Command.Parameters.Add(param4);
                        Command.Parameters.Add(param5);
                        Command.Parameters.Add(param8);
                        Command.Parameters.Add(param6);
                        Command.Parameters.Add(param7);

                        try
                        {
                            Command.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                            success = false;
                            Command.Dispose();
                        }
                    }
                    else
                    {
                        switch (UpdateConnection.State)
                        {
                            case ConnectionState.Broken:
                                UpdateConnection.Close();
                                break;
                            case ConnectionState.Closed:
                                UpdateConnection.Open();
                                break;
                        }
                    }
                }
            }
            catch
            { success = false; }
            return success;
        }
    }
}
