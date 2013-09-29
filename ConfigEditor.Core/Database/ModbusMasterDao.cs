/**
 * 文件名：ModbusMasterDao.cs
 * 说明：Modbus主机DAO类
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
    public class ModbusMasterDao
    {
        public ModbusMasterDao()
        { 
        }

        /// <summary>
        /// 插入新记录
        /// </summary>
        /// <param name="slave"></param>
        /// <returns></returns>
        public bool Insert(ModbusMaster master)
        {
            bool result = false;

            try
            {
                DbDaoHelper dao = new DbDaoHelper(DataSources.PROJECT);
                string sql = @" INSERT INTO ModbusMaster
                               ( SerialPort_SerialID, ModbusGateway_SerialID, Name, Allias,  Enable)
                                VALUES ('{0}','{1}','{2}','{3}','{4}')  ";

                object[] objs = new object[]
                {
                   
                    master.SerialPort_SerialID,
                    master.ModbusGateway_SerialID,
                    master.Name,
                    master.Allias,                        
                    master.Enable
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
        /// <param name="master"></param>
        /// <returns></returns>
        public bool Update(ModbusMaster master)
        {
            bool result = false;

            try
            {
                DbDaoHelper dao = new DbDaoHelper(DataSources.PROJECT);
                string sql = @"
                                UPDATE ModbusMaster
                                SET  SerialPort_SerialID ='{1}', ModbusGateway_SerialID ='{2}', Name ='{3}', Allias ='{4}',
                                      Enable ='{5}'
                                WHERE (SerialID = '{0}')
                              ";
                object[] objs = new object[]
                {
                    master.SerialID,
                    master.SerialPort_SerialID,
                    master.ModbusGateway_SerialID,
                    master.Name,
                    master.Allias,                      
                    master.Enable
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
                string sql = @"DELETE FROM ModbusMaster WHERE SerialID = '{0}' ";
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
                string sql = "DELETE FROM ModbusMaster";
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
        public IList<ModbusMaster> GetAll()
        {
            IList<ModbusMaster> list = new List<ModbusMaster>();

            try
            {
                DbDaoHelper dao = new DbDaoHelper(DataSources.PROJECT);
                string sql = "SELECT * FROM ModbusMaster";
                DataTable dt = dao.ExecuteQuery(sql);

                foreach (DataRow row in dt.Rows)
                {
                    ModbusMaster master = new ModbusMaster()
                    {
                        SerialID = Convert.ToInt32(row["SerialID"]),
                        Name = Convert.ToString(row["Name"]),
                        SerialPort_SerialID = Convert.ToInt32(row["SerialPort_SerialID"]),
                        ModbusGateway_SerialID = Convert.ToInt32(row["ModbusGateway_SerialID"]),
                        Allias = Convert.ToString(row["Allias"]),
                        Enable = Convert.ToString(row["Enable"])
                    };

                    list.Add(master);
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
        public IList<ModbusMaster> Query(string where)
        {
            IList<ModbusMaster> list = new List<ModbusMaster>();

            try
            {
                DbDaoHelper dao = new DbDaoHelper(DataSources.PROJECT);
                string sql = "SELECT * FROM ModbusMaster where 1=1 " + where;
                DataTable dt = dao.ExecuteQuery(sql);

                foreach (DataRow row in dt.Rows)
                {
                    ModbusMaster master = new ModbusMaster()
                    {

                        SerialID = Convert.ToInt32(row["SerialID"]),
                        Name = Convert.ToString(row["Name"]),
                        SerialPort_SerialID = Convert.ToInt32(row["SerialPort_SerialID"]),
                        ModbusGateway_SerialID = Convert.ToInt32(row["ModbusGateway_SerialID"]),
                        Allias = Convert.ToString(row["Allias"]),
                        Enable = Convert.ToString(row["Enable"])
                    };

                    list.Add(master);
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
        public ModbusMaster GetByID(int SerialID)
        {
            ModbusMaster item = new ModbusMaster();
            try
            {
                DbDaoHelper dao = new DbDaoHelper(DataSources.PROJECT);
                string sql = string.Format("SELECT * FROM ModbusMaster where SerialID = '{0}'", SerialID);
                DataTable dt = dao.ExecuteQuery(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                     item.SerialID = Convert.ToInt32(row["SerialID"]);
                         item.Name = Convert.ToString(row["Name"]);
                         item.SerialPort_SerialID = Convert.ToInt32(row["SerialPort_SerialID"]);
                         item.ModbusGateway_SerialID = Convert.ToInt32(row["ModbusGateway_SerialID"]);
                         item.Allias = Convert.ToString(row["Allias"]);
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
                lastSerialID = Convert.ToInt32(dao.ExecuteScalar("select SerialID from [ModbusMaster] order by SerialID desc limit 1"));
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
            string sql = "select count(1) from [ModbusMaster] where Name='" + name + "'";
            int count = Convert.ToInt32(dao.ExecuteScalar(sql));
            if (count > 0)
            {
                isExist = true;
            }
            return isExist;
        }
    }
}
