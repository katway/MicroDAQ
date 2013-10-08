/**
 * 文件名：IPSettingDao.cs
 * 说明：IP设置DAO类
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
    public class IPSettingDao
    {
        public IPSettingDao()
        { 
        }
        /// <summary>
        /// 插入新记录
        /// </summary>
        /// <param name="slave"></param>
        /// <returns></returns>
        public bool Insert(IPSetting ipsetting)
        {
            bool result = false;

            try
            {
                DbDaoHelper dao = new DbDaoHelper(DataSources.PROJECT);
                string sql = @" INSERT INTO IPSetting
                               ( IP, Port,  Enable)
                                VALUES ('{0}','{1}','{2}')  ";

                object[] objs = new object[]
                {
                   
                    ipsetting.IP,
                    ipsetting.Port,                        
                    ipsetting.Enable
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
        public bool Update(IPSetting ipsetting)
        {
            bool result = false;

            try
            {
                DbDaoHelper dao = new DbDaoHelper(DataSources.PROJECT);
                string sql = @"
                                UPDATE IPSetting
                                SET   IP ='{1}', Port ='{2}',Enable ='{3}'
                                WHERE (SerialID = '{0}')
                              ";
                object[] objs = new object[]
                {
                    ipsetting.SerialID,
                    ipsetting.IP,
                    ipsetting.Port,                         
                    ipsetting.Enable
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
                string sql = @"DELETE FROM IPSetting WHERE SerialID = '{0}' ";
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
                string sql = "DELETE FROM IPSetting";
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
        public IList<IPSetting> GetAll()
        {
            IList<IPSetting> list = new List<IPSetting>();

            try
            {
                DbDaoHelper dao = new DbDaoHelper(DataSources.PROJECT);
                string sql = "SELECT * FROM IPSetting";
                DataTable dt = dao.ExecuteQuery(sql);

                foreach (DataRow row in dt.Rows)
                {
                    IPSetting ipsetting = new IPSetting()
                    {
                        SerialID = Convert.ToInt32(row["SerialID"]),
                        IP = Convert.ToString(row["IP"]),
                        Port = Convert.ToInt32(row["Port"]),
                        Enable = Convert.ToString(row["Enable"])
                    };

                    list.Add(ipsetting);
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
        public IList<IPSetting> Query(string where)
        {
            IList<IPSetting> list = new List<IPSetting>();

            try
            {
                DbDaoHelper dao = new DbDaoHelper(DataSources.PROJECT);
                string sql = "SELECT * FROM IPSetting where 1=1 " + where;
                DataTable dt = dao.ExecuteQuery(sql);

                foreach (DataRow row in dt.Rows)
                {
                    IPSetting ipsetting = new IPSetting()
                    {
                        SerialID = Convert.ToInt32(row["SerialID"]),
                        IP = Convert.ToString(row["IP"]),
                        Port = Convert.ToInt32(row["Port"]),
                        Enable = Convert.ToString(row["Enable"])
                    };

                    list.Add(ipsetting);
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
        /// <param name="SerialID"></param>
        /// <returns></returns>
        public IPSetting GetByID(int SerialID)
        {
            IPSetting item = new IPSetting();
            try
            {
                DbDaoHelper dao = new DbDaoHelper(DataSources.PROJECT);
                string sql = string.Format("SELECT * FROM IPSetting where SerialID = '{0}'", SerialID);
                DataTable dt = dao.ExecuteQuery(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    item.SerialID = Convert.ToInt32(row["SerialID"]);
                    item.IP = Convert.ToString(row["IP"]);
                    item.Port = Convert.ToInt32(row["Port"]);
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
                lastSerialID = Convert.ToInt32(dao.ExecuteScalar("select SerialID from [IPSetting] order by SerialID desc limit 1"));
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
            string sql = "select count(1) from [IPSetting] where Name='" + name + "'";
            int count = Convert.ToInt32(dao.ExecuteScalar(sql));
            if (count > 0)
            {
                isExist = true;
            }
            return isExist;
        }
    }
}
