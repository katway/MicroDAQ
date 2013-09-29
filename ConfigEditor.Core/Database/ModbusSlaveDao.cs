/**
 * 文件名：ModbusSlaveDao.cs
 * 说明：Modbus从机DAO类
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
    /// Modbus从机数据访问对象类
    /// </summary>
    public class ModbusSlaveDao
    {
        public ModbusSlaveDao()
        {
        }

        /// <summary>
        /// 插入新记录
        /// </summary>
        /// <param name="slave"></param>
        /// <returns></returns>
        public bool Insert(ModbusSlave slave)
        {
            bool result = false;

            try
            {
                DbDaoHelper dao = new DbDaoHelper(DataSources.PROJECT);
                string sql = @" INSERT INTO ModbusSlave
                               ( ModbusMaster_SerialID, IPSetting_SerialID, Name, Allias, Type,Slave, Enable)
                                VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}')  ";

                object[] objs = new object[]
                {
                    slave.SerialID,
                    slave.ModbusMaster_SerialID,
                    slave.IPSetting_SerialID,
                    slave.Name,
                    slave.Allias,
                    slave.Type,
                    slave.Slave,                         
                    slave.Enable
                };

                 sql = string.Format(sql, objs);
                int rowCount = dao.ExecuteNonQuery(sql, true);
                if (rowCount > 0)
                {
                    result = true;
                }
            }
            catch(Exception e)
            {
                throw;
            }

            return result;
        }

        /// <summary>
        /// 修改记录
        /// </summary>
        /// <param name="slave"></param>
        /// <returns></returns>
        public bool Update(ModbusSlave slave)
        {
            bool result = false;

            try
            {
                DbDaoHelper dao = new DbDaoHelper(DataSources.PROJECT);
                string sql = @"
                                UPDATE ModbusSlave
                                SET  ModbusMaster_SerialID ='{1}', IPSetting_SerialID ='{2}', Name ='{3}', Allias ='{4}', Type ='{5}', Slave ='{6}', 
                                      Enable ='{7}'
                                WHERE (SerialID = '{0}')
                              ";
                object[] objs = new object[]
                {
                    slave.SerialID,
                    slave.ModbusMaster_SerialID,
                    slave.IPSetting_SerialID,
                    slave.Name,
                    slave.Allias,
                    slave.Type,
                    slave.Slave,                         
                    slave.Enable
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
                string sql =@"DELETE FROM ModbusSlave WHERE SerialID = '{0}' ";
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
                string sql = "DELETE FROM ModbusSlave";
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
        public IList<ModbusSlave> GetAll()
        {
            IList<ModbusSlave> list = new List<ModbusSlave>();

            try
            {
                DbDaoHelper dao = new DbDaoHelper(DataSources.PROJECT);
                string sql = "SELECT * FROM ModbusSlave";
                DataTable dt = dao.ExecuteQuery(sql);

                foreach (DataRow row in dt.Rows)
                {
                    ModbusSlave slave = new ModbusSlave()
                    {
                        SerialID = Convert.ToInt32(row["SerialID"]),
                        Name = Convert.ToString(row["Name"]),
                        ModbusMaster_SerialID = Convert.ToInt32(row["ModbusMaster_SerialID"]),
                        IPSetting_SerialID = Convert.ToInt32(row["IPSetting_SerialID"]),
                        Allias = Convert.ToString(row["Allias"]),
                        Type = Convert.ToString(row["Type"]),
                        Slave = Convert.ToInt32(row["Slave"]),
                        Enable = Convert.ToString(row["Enable"])
                    };

                    list.Add(slave);
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
        public IList<ModbusSlave> Query(string where)
        {
            IList<ModbusSlave> list = new List<ModbusSlave>();

            try
            {
                DbDaoHelper dao = new DbDaoHelper(DataSources.PROJECT);
                string sql = "SELECT * FROM ModbusSlave where 1=1 " + where;
                DataTable dt = dao.ExecuteQuery(sql);

                foreach (DataRow row in dt.Rows)
                {
                    ModbusSlave slave = new ModbusSlave()
                    {
                       
                        SerialID = Convert.ToInt32(row["SerialID"]),
                        Name = Convert.ToString(row["Name"]),
                        ModbusMaster_SerialID = Convert.ToInt32(row["ModbusMaster_SerialID"]),
                        IPSetting_SerialID = Convert.ToInt32(row["IPSetting_SerialID"]),
                        Allias = Convert.ToString(row["Allias"]),
                        Type = Convert.ToString(row["Type"]),
                        Slave = Convert.ToInt32(row["Slave"]),
                        Enable = Convert.ToString(row["Enable"])
                    };

                    list.Add(slave);
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
        public ModbusSlave GetByID(int SerialID)
        {
            ModbusSlave item = new ModbusSlave();
            try
            {
                DbDaoHelper dao = new DbDaoHelper(DataSources.PROJECT);
                string sql = string.Format("SELECT * FROM ModbusSlave where SerialID = '{0}'", SerialID);
                DataTable dt = dao.ExecuteQuery(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                   
                    item.SerialID = Convert.ToInt32(row["SerialID"]);
                    item.Name = Convert.ToString(row["Name"]);
                    item.ModbusMaster_SerialID = Convert.ToInt32(row["ModbusMaster_SerialID"]);
                    item.IPSetting_SerialID = Convert.ToInt32(row["IPSetting_SerialID"]);
                    item.Allias = Convert.ToString(row["Allias"]);
                    item.Type = Convert.ToString(row["Type"]);
                    item.Slave = Convert.ToInt32(row["Slave"]);
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
                lastSerialID = Convert.ToInt32(dao.ExecuteScalar("select SerialID from [ModbusSlave] order by SerialID desc limit 1"));
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
            string sql = "select count(1) from [ModbusSlave] where Name='" + name + "'";
            int count = Convert.ToInt32(dao.ExecuteScalar(sql));
            if (count > 0)
            {
                isExist = true;
            }
            return isExist;
        }
    }
}
