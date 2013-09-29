/**
 * 文件名：SerialPortDao.cs
 * 说明：串行端口DAO类
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
    public class SerialPortDao
    {
        public SerialPortDao()
        { 
        }
        /// <summary>
        /// 插入新记录
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public bool Insert(SerialPort port)
        {
            bool result = false;

            try
            {
                DbDaoHelper dao = new DbDaoHelper(DataSources.PROJECT);
                string sql = @" INSERT INTO SerialPort
                               ( Port, BaudRate, Parity, Databits, Stopbits,Enable)
                                VALUES ('{0}','{1}','{2}','{3}','{4}','{5}')  ";

                object[] objs = new object[]
                {
                   
                    port.Port,
                    port.BaudRate,
                    port.Parity,
                    port.Databits,
                    port.Stopbits,                        
                    port.Enable
                };

                sql = string.Format(sql, objs);
                int rowCount = dao.ExecuteNonQuery(sql, true);
                if (rowCount > 0)
                {
                    result = true;
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return result;
        }

        /// <summary>
        /// 修改记录
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public bool Update(SerialPort port)
        {
            bool result = false;

            try
            {
                DbDaoHelper dao = new DbDaoHelper(DataSources.PROJECT);
                string sql = @"
                                UPDATE SerialPort
                                SET  Port ='{1}', BaudRate ='{2}', Parity ='{3}', Databits ='{4}', Stopbits ='{5}',
                                      Enable ='{6}'
                                WHERE (SerialID = '{0}')
                              ";
                object[] objs = new object[]
                {
                   port.SerialID,
                    port.Port,
                    port.BaudRate,
                    port.Parity,
                    port.Databits,
                    port.Stopbits,                        
                    port.Enable
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
                string sql = @"DELETE FROM SerialPort WHERE SerialID = '{0}' ";
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
                string sql = "DELETE FROM SerialPort";
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
        public IList<SerialPort> GetAll()
        {
            IList<SerialPort> list = new List<SerialPort>();

            try
            {
                DbDaoHelper dao = new DbDaoHelper(DataSources.PROJECT);
                string sql = "SELECT * FROM SerialPort";
                DataTable dt = dao.ExecuteQuery(sql);

                foreach (DataRow row in dt.Rows)
                {
                    SerialPort port = new SerialPort()
                    {
                        SerialID = Convert.ToInt32(row["SerialID"]),
                        Port = Convert.ToString(row["Port"]),
                        BaudRate = Convert.ToInt32(row["BaudRate"]),
                        Databits = Convert.ToInt32(row["Databits"]),
                        Parity = Convert.ToString(row["Parity"]),
                        Stopbits = Convert.ToInt32(row["Stopbits"]),
                        Enable = Convert.ToString(row["Enable"])
                    };

                    list.Add(port);
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
        public IList<SerialPort> Query(string where)
        {
            IList<SerialPort> list = new List<SerialPort>();

            try
            {
                DbDaoHelper dao = new DbDaoHelper(DataSources.PROJECT);
                string sql = "SELECT * FROM SerialPort where 1=1 " + where;
                DataTable dt = dao.ExecuteQuery(sql);

                foreach (DataRow row in dt.Rows)
                {
                    SerialPort port = new SerialPort()
                    {

                        SerialID = Convert.ToInt32(row["SerialID"]),
                        Port = Convert.ToString(row["Port"]),
                        BaudRate = Convert.ToInt32(row["BaudRate"]),
                        Databits = Convert.ToInt32(row["Databits"]),
                        Parity = Convert.ToString(row["Parity"]),
                        Stopbits = Convert.ToInt32(row["Stopbits"]),
                        Enable = Convert.ToString(row["Enable"])
                    };

                    list.Add(port);
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
        public SerialPort GetByID(int SerialID)
        {
            SerialPort item = new SerialPort();
            try
            {
                DbDaoHelper dao = new DbDaoHelper(DataSources.PROJECT);
                string sql = string.Format("SELECT * FROM SerialPort where SerialID = '{0}'", SerialID);
                DataTable dt = dao.ExecuteQuery(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                   
                        item.SerialID = Convert.ToInt32(row["SerialID"]);
                         item.Port = Convert.ToString(row["Port"]);
                         item.BaudRate = Convert.ToInt32(row["BaudRate"]);
                         item.Databits = Convert.ToInt32(row["Databits"]);
                         item.Parity = Convert.ToString(row["Parity"]);
                         item.Stopbits = Convert.ToInt32(row["Stopbits"]);
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
                lastSerialID = Convert.ToInt32(dao.ExecuteScalar("select SerialID from [SerialPort] order by SerialID desc limit 1"));
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
            string sql = "select count(1) from [SerialPort] where Name='" + name + "'";
            int count = Convert.ToInt32(dao.ExecuteScalar(sql));
            if (count > 0)
            {
                isExist = true;
            }
            return isExist;
        }
    }
}
