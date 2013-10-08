/**
 * 文件名：ModbusRegisterDao.cs
 * 说明：监测变量DAO类
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
    public class ModbusRegisterDao
    {
        public ModbusRegisterDao()
        { 
        }
        /// <summary>
        /// 插入新记录
        /// </summary>
        /// <param name="register"></param>
        /// <returns></returns>
        public bool Insert(ModbusRegister register)
        {
            bool result = false;

            try
            {
                DbDaoHelper dao = new DbDaoHelper(DataSources.PROJECT);
                string sql = @" INSERT INTO ModbusRegister
                               ( ModbusSlave_SerialID, Allias, Name, RegesiterType, RegesiterAddress,DataType,Length,Accessibility,Value,Enable,ScanPeriod,Minimum,Maximum,OriginalValue,DecimalPlaces,Enable2)
                                VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}')  ";

                object[] objs = new object[]
                {
                    
                    register.ModbusSlave_SerialID,
                    register.Allias,
                    register.Name,
                    register.RegesiterType,                        
                    register.RegesiterAddress,
                    register.DataType,
                    register.Length,
                    register.Accessibility,
                    register.Value,
                    register.Enable,
                    register.ScanPeriod,
                    register.Minimum,
                    register.Maximum,
                    register.OriginalValue,
                    register.DecimalPlaces,
                    register.Enable2
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
        /// <param name="register"></param>
        /// <returns></returns>
        public bool Update(ModbusRegister register)
        {
            bool result = false;

            try
            {
                DbDaoHelper dao = new DbDaoHelper(DataSources.PROJECT);
                string sql = @"
                                UPDATE ModbusRegister
                                SET  ModbusSlave_SerialID='{1}', Allias='{2}', Name='{3}', RegesiterType='{4}', RegesiterAddress='{5}',DataType='{6}',Length='{7}',Accessibility='{8}',Value='{9}',Enable='{10}',ScanPeriod='{11}',Minimum='{12}',Maximum='{13}',OriginalValue='{14}',DecimalPlaces='{15}',Enable2='{16}'
                                    
                                WHERE (SerialID = '{0}')
                              ";
                object[] objs = new object[]
                {
                   register.SerialID,
                    register.ModbusSlave_SerialID,
                    register.Allias,
                    register.Name,
                    register.RegesiterType,                        
                    register.RegesiterAddress,
                    register.DataType,
                    register.Length,
                    register.Accessibility,
                    register.Value,
                    register.Enable,
                    register.ScanPeriod,
                    register.Minimum,
                    register.Maximum,
                    register.OriginalValue,
                    register.DecimalPlaces,
                    register.Enable2
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
                string sql = @"DELETE FROM ModbusRegister WHERE SerialID = '{0}' ";
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
                string sql = "DELETE FROM ModbusRegister";
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
        public IList<ModbusRegister> GetAll()
        {
            IList<ModbusRegister> list = new List<ModbusRegister>();

            try
            {
                DbDaoHelper dao = new DbDaoHelper(DataSources.PROJECT);
                string sql = "SELECT * FROM ModbusRegister";
                DataTable dt = dao.ExecuteQuery(sql);

                foreach (DataRow row in dt.Rows)
                {
                    ModbusRegister register = new ModbusRegister()
                    {
                        SerialID = Convert.ToInt32(row["SerialID"]),
                        Name = Convert.ToString(row["Name"]),
                        ModbusSlave_SerialID = Convert.ToInt32(row["ModbusSlave_SerialID"]),
                        RegesiterType = Convert.ToInt32(row["RegesiterType"]),
                        RegesiterAddress = Convert.ToInt32(row["RegesiterAddress"]),
                        Allias = Convert.ToString(row["Allias"]),
                        DataType = Convert.ToString(row["DataType"]),
                        Length = Convert.ToInt32(row["Length"]),
                        Accessibility = Convert.ToString(row["Accessibility"]),
                        Value = Convert.ToDecimal(row["Value"]),
                        Enable = Convert.ToString(row["Enable"]),
                        ScanPeriod = Convert.ToInt32(row["ScanPeriod"]),
                        Minimum = Convert.ToDecimal(row["Minimum"]),
                        Maximum = Convert.ToDecimal(row["Maximum"]),
                        OriginalValue = Convert.ToDecimal(row["OriginalValue"]),
                        DecimalPlaces = Convert.ToInt32(row["DecimalPlaces"]),
                        Enable2 = Convert.ToString(row["Enable2"])
                    };

                    list.Add(register);
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
        public IList<ModbusRegister> Query(string where)
        {
            IList<ModbusRegister> list = new List<ModbusRegister>();

            try
            {
                DbDaoHelper dao = new DbDaoHelper(DataSources.PROJECT);
                string sql = "SELECT * FROM ModbusRegister where 1=1 " + where;
                DataTable dt = dao.ExecuteQuery(sql);

                foreach (DataRow row in dt.Rows)
                {
                    ModbusRegister register = new ModbusRegister()
                    {
                        SerialID = Convert.ToInt32(row["SerialID"]),
                        Name = Convert.ToString(row["Name"]),
                        ModbusSlave_SerialID = Convert.ToInt32(row["ModbusSlave_SerialID"]),
                        RegesiterType = Convert.ToInt32(row["RegesiterType"]),
                        RegesiterAddress = Convert.ToInt32(row["RegesiterAddress"]),
                        Allias = Convert.ToString(row["Allias"]),
                        DataType = Convert.ToString(row["DataType"]),
                        Length = Convert.ToInt32(row["Length"]),
                        Accessibility = Convert.ToString(row["Accessibility"]),
                        Value = Convert.ToDecimal(row["Value"]),
                        Enable = Convert.ToString(row["Enable"]),
                        ScanPeriod = Convert.ToInt32(row["ScanPeriod"]),
                        Minimum = Convert.ToDecimal(row["Minimum"]),
                        Maximum = Convert.ToDecimal(row["Maximum"]),
                        OriginalValue = Convert.ToDecimal(row["OriginalValue"]),
                        DecimalPlaces = Convert.ToInt32(row["DecimalPlaces"]),
                        Enable2 = Convert.ToString(row["Enable2"])
                    };

                    list.Add(register);
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
        public ModbusRegister GetByID(int SerialID)
        {
            ModbusRegister item = new ModbusRegister();
            try
            {
                DbDaoHelper dao = new DbDaoHelper(DataSources.PROJECT);
                string sql = string.Format("SELECT * FROM ModbusRegister where SerialID = '{0}'", SerialID);
                DataTable dt = dao.ExecuteQuery(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];


                     item.SerialID = Convert.ToInt32(row["SerialID"]);
                        item.Name = Convert.ToString(row["Name"]);
                        item.ModbusSlave_SerialID = Convert.ToInt32(row["ModbusSlave_SerialID"]);
                       item.RegesiterType = Convert.ToInt32(row["RegesiterType"]);
                        item.RegesiterAddress = Convert.ToInt32(row["RegesiterAddress"]);
                        item.Allias = Convert.ToString(row["Allias"]);
                        item.DataType = Convert.ToString(row["DataType"]);
                        item.Length = Convert.ToInt32(row["Length"]);
                        item.Accessibility = Convert.ToString(row["Accessibility"]);
                        item.Value = Convert.ToDecimal(row["Value"]);
                        item.Enable = Convert.ToString(row["Enable"]);
                        item.ScanPeriod = Convert.ToInt32(row["ScanPeriod"]);
                        item.Minimum = Convert.ToDecimal(row["Minimum"]);
                        item.Maximum = Convert.ToDecimal(row["Maximum"]);
                        item.OriginalValue = Convert.ToDecimal(row["OriginalValue"]);
                        item.DecimalPlaces = Convert.ToInt32(row["DecimalPlaces"]);
                        item.Enable2 = Convert.ToString(row["Enable2"]);
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
                lastSerialID = Convert.ToInt32(dao.ExecuteScalar("select SerialID from [ModbusRegister] order by SerialID desc limit 1"));
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
            string sql = "select count(1) from [ModbusRegister] where Name='" + name + "'";
            int count = Convert.ToInt32(dao.ExecuteScalar(sql));
            if (count > 0)
            {
                isExist = true;
            }
            return isExist;
        }
    }
}
