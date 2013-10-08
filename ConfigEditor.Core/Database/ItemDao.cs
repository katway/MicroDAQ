/**
 * 文件名：ItemDao.cs
 * 说明：监测参数DAO类
 * 作者：刘风彬
 * 更改记录： 
 * -------------------------------------------------------
 * 改动人 	时间 			原因
 * -------------------------------------------------------
 * 刘风彬 	2013-09-29		创建文件
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
    /// <summary>
    /// 
    /// </summary>
    public class ItemDao
    {
        public ItemDao()
        { 
        }
        /// <summary>
        /// 插入新记录
        /// </summary>
        /// <param name="slave"></param>
        /// <returns></returns>
        public bool Insert(Item item)
        {
            bool result = false;

            try
            {
                DbDaoHelper dao = new DbDaoHelper(DataSources.PROJECT);
                string sql = @" INSERT INTO Item
                               ( Name, Allias, Code, Enable)
                                VALUES ('{0}','{1}','{2}','{3}')  ";

                object[] objs = new object[]
                {
                    
                    item.Name,
                    item.Allias,
                    item.Code,    
                    item.Enable
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
        public bool Update(Item item)
        {
            bool result = false;

            try
            {
                DbDaoHelper dao = new DbDaoHelper(DataSources.PROJECT);
                string sql = @"
                                UPDATE Item
                                SET   Name ='{1}', Allias ='{2}',Code = '{3}',
                                      Enable ='{4}'
                                WHERE (SerialID = '{0}')
                              ";
                object[] objs = new object[]
                {
                    item.SerialID,
                    item.Name,
                    item.Allias,
                    item.Code,                        
                    item.Enable
                };

                int rowCount = dao.ExecuteNonQuery(string.Format(sql, objs));
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
                string sql = @"DELETE FROM Item WHERE SerialID = '{0}' ";
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
                string sql = "DELETE FROM Item";
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
        public IList<Item> GetAll()
        {
            IList<Item> list = new List<Item>();

            try
            {
                DbDaoHelper dao = new DbDaoHelper(DataSources.PROJECT);
                string sql = "SELECT * FROM Item";
                DataTable dt = dao.ExecuteQuery(sql);

                foreach (DataRow row in dt.Rows)
                {
                    Item item = new Item()
                    {
                        SerialID = Convert.ToInt32(row["SerialID"]),
                        Name = Convert.ToString(row["Name"]),
                        Allias = Convert.ToString(row["Allias"]),
                        Code = Convert.ToInt32(row["Code"]),
                        Enable = Convert.ToString(row["Enable"])
                    };

                    list.Add(item);
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
        public IList<Item> Query(string where)
        {
            IList<Item> list = new List<Item>();

            try
            {
                DbDaoHelper dao = new DbDaoHelper(DataSources.PROJECT);
                string sql = "SELECT * FROM Item where 1=1 " + where;
                DataTable dt = dao.ExecuteQuery(sql);

                foreach (DataRow row in dt.Rows)
                {
                    Item item = new Item()
                    {

                        SerialID = Convert.ToInt32(row["SerialID"]),
                        Name = Convert.ToString(row["Name"]),
                        Allias = Convert.ToString(row["Allias"]),
                        Code = Convert.ToInt32(row["Code"]),
                        Enable = Convert.ToString(row["Enable"])
                    };

                    list.Add(item);
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
        public Item GetByID(int SerialID)
        {
            Item item = new Item();
            try
            {
                DbDaoHelper dao = new DbDaoHelper(DataSources.PROJECT);
                string sql = string.Format("SELECT * FROM Item where SerialID = '{0}'", SerialID);
                DataTable dt = dao.ExecuteQuery(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    item.SerialID = Convert.ToInt32(row["SerialID"]);
                    item.Name = Convert.ToString(row["Name"]);
                    item.Allias = Convert.ToString(row["Allias"]);
                    item.Code = Convert.ToInt32(row["Code"]);
                    item.Enable = Convert.ToString(row["Enable"]);
                }
            }
            catch
            {
                throw;
            }
            return item;
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
                lastSerialID = Convert.ToInt32(dao.ExecuteScalar("select SerialID from [Item] order by SerialID desc limit 1"));
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
        public bool IsExist(string name)
        {
            bool isExist = false;
            DbDaoHelper dao = new DbDaoHelper(DataSources.PROJECT);
            string sql = "select count(1) from [Item] where Name='" + name + "'";
            int count = Convert.ToInt32(dao.ExecuteScalar(sql));
            if (count > 0)
            {
                isExist = true;
            }
            return isExist;
        }
    }
}
