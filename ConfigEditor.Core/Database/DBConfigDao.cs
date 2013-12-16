/**
 * 文件名：DBConfigDao.cs
 * 说明：DB块配置DAO类
 * 作者：刘风彬
 * 更改记录： 
 * -------------------------------------------------------
 * 改动人 	时间 			原因
 * -------------------------------------------------------
 * 刘风彬 	2013-12-04		创建文件
 * -------------------------------------------------------
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConfigEditor.Core.Models;
using System.Data;

namespace ConfigEditor.Core.Database
{
    public class DBConfigDao
    {

        public DBConfigDao()
        {
        }

        /// <summary>
        /// 插入新记录
        /// </summary>
        /// <param name="slave"></param>
        /// <returns></returns>
        public bool Insert(DBConfig config)
        {
            bool result = false;

            try
            {
                DbDaoHelper dao = new DbDaoHelper(DataSources.PROJECT);
                string sql = @" INSERT INTO DBConfig ( Address, Accessibility, Enable, Code)
                                VALUES ('{0}','{1}','{2}','{3}')  ";

                object[] objs = new object[]
                {
                    
                    config.Address =  config.Connection + config.DB + "," + config.DBType + config.StartAddress + "," + config.Length,
                    //config.Connection,
                    //config.DB,
                    //config.DBType,
                    //config.StartAddress,
                    config.Accessibility,                        
                    config.Enable,
                    config.Code
                  
                   
                };

                sql = string.Format(sql, objs);
                int rowCount = dao.ExecuteNonQuery(sql, true);
                if (rowCount > 0)
                {
                    result = true;
                }
            }
            catch
            {
                throw;
            }

            return result;
        }

        /// <summary>
        /// 修改记录
        /// </summary>
        /// <param name="master"></param>
        /// <returns></returns>
        public bool Update(DBConfig config)
        {
            bool result = false;

            try
            {
                DbDaoHelper dao = new DbDaoHelper(DataSources.PROJECT);
                string sql = @"
                                UPDATE DBConfig
                                SET   Address ='{1}', Enable ='{2}',
                                      Accessibility ='{3}',Code = '{4}'
                                WHERE (SerialID = '{0}')
                              ";
                object[] objs = new object[]
                {
                    config.SerialID,
                    config.Address,
                    config.Enable,                         
                    config.Accessibility,
                    config.Code
                };
                string tmp = string.Format(sql, objs);
                int rowCount = dao.ExecuteNonQuery(tmp);
                if (rowCount > 0)
                {
                    result = true;   
                }
            }
            catch
            { 
                throw;
            }

            return result;
        }

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="SerialID"></param>
        /// <returns></returns>
        public bool Delete(int SerialID)
        {
            bool result = false;

            try
            {
                DbDaoHelper dao = new DbDaoHelper(DataSources.PROJECT);
                string sql = @"DELETE FROM DBConfig WHERE SerialID = '{0}' ";
                object[] objs = new object[]
                {
                    SerialID
                    
                };

                sql = string.Format(sql, objs);
                int rowCount = dao.ExecuteNonQuery(sql);
                if (rowCount > 0)
                {
                    result = true;
                }
            }
            catch
            {
                throw;
            }

            return result;
        }


        /// <summary>
        /// 清空记录
        /// </summary>
        /// <returns></returns>
        public bool Clear()
        {
            bool result = true;

            try
            {
                DbDaoHelper dao = new DbDaoHelper(DataSources.PROJECT);
                string sql = "DELETE FROM DBConfig";
                dao.ExecuteNonQuery(sql);
            }
            catch
            {
                throw;
            }

            return result;
        }

        /// <summary>
        /// 获取所有记录
        /// </summary>
        /// <returns></returns>
        public IList<DBConfig> GetAll()
        {
            IList<DBConfig> list = new List<DBConfig>();

            try
            {
                DbDaoHelper dao = new DbDaoHelper(DataSources.PROJECT);
                string sql = "SELECT * FROM DBConfig";
                DataTable dt = dao.ExecuteQuery(sql);

                foreach (DataRow row in dt.Rows)
                {
                    DBConfig config = new DBConfig()
                    {
                        SerialID = Convert.ToInt32(row["SerialID"]),
                        Address = Convert.ToString(row["Address"]),
                        Accessibility = Convert.ToString(row["Accessibility"]),
                        Enable = Convert.ToString(row["Enable"]),
                        Code = row["Code"] != DBNull.Value ? Convert.ToInt32(row["Code"]) : 0
                    };

                    list.Add(config);
                }
            }
            catch
            {
                throw;
            }

            return list;
        }
        /// <summary>
        /// 按条件查询
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        public IList<DBConfig> Query(string where)
        {
            IList<DBConfig> list = new List<DBConfig>();

            try
            {
                DbDaoHelper dao = new DbDaoHelper(DataSources.PROJECT);
                string sql = "SELECT * FROM DBConfig where 1=1 " + where;
                DataTable dt = dao.ExecuteQuery(sql);

                foreach (DataRow row in dt.Rows)
                {
                    DBConfig config = new DBConfig()
                    {

                        SerialID = Convert.ToInt32(row["SerialID"]),
                        Address = Convert.ToString(row["Address"]),
                        Accessibility = Convert.ToString(row["Accessibility"]),
                        Enable = Convert.ToString(row["Enable"]),
                        Code = row["Code"] != DBNull.Value ? Convert.ToInt32(row["Code"]) : 0
                    };

                    list.Add(config);
                }
            }
            catch
            {
                throw;
            }

            return list;
        }
        /// <summary>
        /// 按编号查询
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public DBConfig GetByID(int SerialID)
        {
            DBConfig dbg = new DBConfig();
            try
            {
                DbDaoHelper dao = new DbDaoHelper(DataSources.PROJECT);
                string sql = string.Format("SELECT * FROM DBConfig where SerialID = '{0}'", SerialID);
                DataTable dt = dao.ExecuteQuery(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    dbg.SerialID = Convert.ToInt32(row["SerialID"]);
                    dbg.Address = Convert.ToString(row["Address"]);
                    dbg.Accessibility = Convert.ToString(row["Accessibility"]);
                    dbg.Enable = Convert.ToString(row["Enable"]);
                  
                }
            }
            catch
            {
                throw;
            }
            return dbg;
        }
        /// <summary>
        /// 获取最新SerialID
        /// </summary>
        /// <returns></returns>
        public int GetLastSerialID()
        {
            int lastSerialID = 0;
            DbDaoHelper dao = new DbDaoHelper(DataSources.PROJECT);
            try
            {
                lastSerialID = Convert.ToInt32(dao.ExecuteScalar("select SerialID from [DBConfig] order by SerialID desc limit 1"));
            }
            catch
            {
                throw;
            }

            return lastSerialID;
        }

        /// <summary>
        ///判断名称是否存在
        /// </summary>
        /// <param name="deviceName"></param>
        /// <returns></returns>
        //public bool IsExist(string name)
        //{
        //    bool isExist = false;
        //    DbDaoHelper dao = new DbDaoHelper(DataSources.PROJECT);
        //    string sql = "select count(1) from [DBConfig] where Name='" + name + "'";
        //    int count = Convert.ToInt32(dao.ExecuteScalar(sql));
        //    if (count > 0)
        //    {
        //        isExist = true;
        //    }
        //    return isExist;
        //}

        //public string Accessibility { get; set; }
    }
}
