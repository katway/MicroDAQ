/**
 * 文件名：DbDaoHelper.cs
 * 说明：SQLite数据库访问对象辅助类
 * 作者：林安城
 * 更改记录： 
 * -------------------------------------------------------
 * 改动人 	时间 			原因
 * -------------------------------------------------------
 * 林安城 	2013-09-20		创建文件
 * -------------------------------------------------------
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using log4net;



namespace MicroDAQ.DBUtility
{
    /// <summary>
    /// SQLite数据库访问对象辅助类
    /// </summary>
    public class SQLiteHelper
    {
        //日志
        private static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        //数据库连接
        private SQLiteConnection con = null;

        public SQLiteConnection Connection
        {
            get { return con; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbFile"></param>
        public SQLiteHelper(string dbFile)
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string db = string.Empty;

            if (File.Exists(dbFile))
            {
                db = dbFile;
            }
            else
            {
                db = Path.Combine(path, dbFile);
            }


            //是否存在文件
            if (!File.Exists(db))
            {
                throw new FileNotFoundException(string.Format("文件({0})不存在。", db));
            }

            this.con = new SQLiteConnection();
            this.con.ConnectionString = string.Format("Data Source={0};Pooling=true;FailIfMissing=true", db);
        }

        /// <summary>
        /// 执行SQL语句，无返回值
        /// </summary>
        /// <param name="sql"></param>
        public int ExecuteNonQuery(string sql)
        {
            int result = 0;

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                SQLiteCommand cmd = con.CreateCommand();
                cmd.CommandText = sql;
                result = cmd.ExecuteNonQuery();

                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Dispose();
                }
            }

            return result;
        }

        /// <summary>
        /// 执行SQL语句，无返回值,启用事务
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="isTran">是否启用事务</param>
        public int ExecuteNonQuery(string sql, bool isTran)
        {
            int result = 0;
            if (isTran == true)
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SQLiteCommand cmd = con.CreateCommand();
                DbTransaction trans = con.BeginTransaction(); //启动事务
                try
                {
                    cmd.CommandText = sql;
                    result = cmd.ExecuteNonQuery();
                    trans.Commit(); //提交事务
                    con.Close();
                }
                catch (Exception ex)
                {
                    trans.Rollback(); //事务回滚
                    throw ex;
                }
                finally
                {
                    if (con.State != ConnectionState.Closed)
                    {
                        con.Dispose();
                    }
                }
            }
            else
            {
                result = ExecuteNonQuery(sql);
            }
            return result;
        }

        /// <summary>
        /// 执行插入SQL语句，返回自增长字段的值
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int ExecuteRowId(string sql)
        {
            sql += "; select last_insert_rowid() newid;";

            int newid = Convert.ToInt32(this.ExecuteScalar(sql));

            return newid;
        }

        /// <summary>
        /// 查询单个值
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public object ExecuteScalar(string sql)
        {
            object result = null;
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                SQLiteCommand cmd = con.CreateCommand();
                cmd.CommandText = sql;
                result = cmd.ExecuteScalar();

                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Dispose();
                }
            }

            return result;
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable ExecuteQuery(string sql)
        {
            DataSet ds = new DataSet();

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                SQLiteDataAdapter da = new SQLiteDataAdapter(sql, con);
                da.Fill(ds);
                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Dispose();
                }
            }

            return ds.Tables[0];
        }
    }
}
