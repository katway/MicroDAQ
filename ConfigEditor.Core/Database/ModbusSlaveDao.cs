/**
 * 文件名：ModbusSlaveDao.cs
 * 说明：Modbus从机DAO类
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
        /// <param name="device"></param>
        /// <returns></returns>
        public bool Insert(ModbusSlave slave)
        {
            bool result = false;

            try
            {
                DbDaoHelper dao = new DbDaoHelper(DataSources.PROJECT);
                string sql = @"
                                INSERT INTO ModbusSlave
                                      (Name, ProtocolId, IsSimulate, IsTagLog, Timeout, Reconnect, ChannelId, 
                                      IpAddress, IpPort, Slave, IsEnabled)
                                VALUES ('{0}',{1},'{2}','{3}',{4},{5},{6},'{7}',{8},'{9}','{10}')
                              ";

                object[] objs = new object[]
                {
                    //device.Name,
                    //device.ProtocolId,
                    //device.IsSimulate.ToString(),
                    //device.IsTagLog.ToString(),
                    //device.Timeout,
                    //device.Reconnect,
                    //device.ChannelId,
                    //device.IpAddress,
                    //device.IpPort,
                    //device.Slave,
                    //device.IsEnabled.ToString()
                };

                int rowCount = dao.ExecuteNonQuery(string.Format(sql, objs), true);
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
        /// <param name="device"></param>
        /// <returns></returns>
        public bool Update(ModbusSlave slave)
        {
            bool result = false;

            try
            {
                DbDaoHelper dao = new DbDaoHelper(DataSources.PROJECT);
                string sql = @"
                                UPDATE Device
                                SET Name ='{1}', ProtocolId ={2}, IsSimulate ='{3}', IsTagLog ='{4}', Timeout ={5}, Reconnect ={6}, ChannelId ={7}, 
                                      IpAddress ='{8}', IpPort ={9}, Slave ='{10}', IsEnabled ='{11}'
                                WHERE (Id = {0})
                              ";
                object[] objs = new object[]
                {
                    //device.Id,
                    //device.Name,
                    //device.ProtocolId,
                    //device.IsSimulate.ToString(),
                    //device.IsTagLog.ToString(),
                    //device.Timeout,
                    //device.Reconnect,
                    //device.ChannelId,
                    //device.IpAddress,
                    //device.IpPort,
                    //device.Slave,
                    //device.IsEnabled.ToString()
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
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            bool result = false;

            try
            {
                DbDaoHelper dao = new DbDaoHelper(DataSources.PROJECT);
                //删除变量

                //删除设备

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
                        //Id = Convert.ToInt32(row["Id"]),
                        //Name = Convert.ToString(row["Name"]),
                        //ProtocolId = Convert.ToInt32(row["ProtocolId"]),
                        //IsSimulate = Convert.ToBoolean(row["IsSimulate"]),
                        //IsTagLog = Convert.ToBoolean(row["IsTagLog"]),
                        //Timeout = Convert.ToInt32(row["Timeout"]),
                        //Reconnect = Convert.ToInt32(row["Reconnect"]),
                        //ChannelId = Convert.ToInt32(row["ChannelId"]),
                        //IpAddress = (row["IpAddress"] == DBNull.Value) ? null : Convert.ToString(row["IpAddress"]),
                        //IpPort = (row["IpPort"] == DBNull.Value) ? 0 : Convert.ToInt32(row["IpPort"]),
                        //Slave = Convert.ToString(row["Slave"]),
                        //IsEnabled = Convert.ToBoolean(row["IsEnabled"])
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
                        //Id = Convert.ToInt32(row["Id"]),
                        //Name = Convert.ToString(row["Name"]),
                        //ProtocolId = Convert.ToInt32(row["ProtocolId"]),
                        //IsSimulate = Convert.ToBoolean(row["IsSimulate"]),
                        //IsTagLog = Convert.ToBoolean(row["IsTagLog"]),
                        //Timeout = Convert.ToInt32(row["Timeout"]),
                        //Reconnect = Convert.ToInt32(row["Reconnect"]),
                        //ChannelId = Convert.ToInt32(row["ChannelId"]),
                        //IpAddress = (row["IpAddress"] == DBNull.Value) ? null : Convert.ToString(row["IpAddress"]),
                        //IpPort = (row["IpPort"] == DBNull.Value) ? 0 : Convert.ToInt32(row["IpPort"]),
                        //Slave = Convert.ToString(row["Slave"]),
                        //IsEnabled = Convert.ToBoolean(row["IsEnabled"])
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
        public ModbusSlave GetByID(int id)
        {
            ModbusSlave item = new ModbusSlave();
            try
            {
                DbDaoHelper dao = new DbDaoHelper(DataSources.PROJECT);
                string sql = string.Format("SELECT * FROM ModbusSlave where ModbusSlaveID = {0}", id);
                DataTable dt = dao.ExecuteQuery(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    //item.Id = Convert.ToInt32(row["Id"]);
                    //item.Name = Convert.ToString(row["Name"]);
                    //item.ProtocolId = Convert.ToInt32(row["ProtocolId"]);
                    //item.IsSimulate = Convert.ToBoolean(row["IsSimulate"]);
                    //item.IsTagLog = Convert.ToBoolean(row["IsTagLog"]);
                    //item.Timeout = Convert.ToInt32(row["Timeout"]);
                    //item.Reconnect = Convert.ToInt32(row["Reconnect"]);
                    //item.ChannelId = Convert.ToInt32(row["ChannelId"]);
                    //item.IpAddress = (row["IpAddress"] == DBNull.Value) ? null : Convert.ToString(row["IpAddress"]);
                    //item.IpPort = (row["IpPort"] == DBNull.Value) ? 0 : Convert.ToInt32(row["IpPort"]);
                    //item.Slave = Convert.ToString(row["Slave"]);
                    //item.IsEnabled = Convert.ToBoolean(row["IsEnabled"]);
                }
            }
            catch
            {
                throw;
            }
            return item;
        }
        /// <summary>
        /// 获取最新ID
        /// </summary>
        /// <returns></returns>
        public int GetLastID()
        {
            int lastID = 0;
            DbDaoHelper dao = new DbDaoHelper(DataSources.PROJECT);
            try
            {
                lastID = Convert.ToInt32(dao.ExecuteScalar("select id from [ModbusSlave] order by id desc limit 1"));
            }
            catch
            {
                throw;
            }

            return lastID;
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
