/**
 * 文件名：ProjectReader.cs
 * 说明：项目工程读取类
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
using ConfigEditor.Core.ViewModels;
using ConfigEditor.Core.Database;
using ConfigEditor.Core.Models;


namespace ConfigEditor.Core.IO
{
    /// <summary>
    /// 项目工程读取类
    /// </summary>
    public class ProjectReader
    {
        public ProjectReader()
        {
        }

        /// <summary>
        /// 读取项目数据库文件
        /// </summary>
        /// <returns></returns>
        public static ProjectViewModel Read()
        {
            try
            {
                ProjectViewModel project = new ProjectViewModel();

                return project;
            }
            catch (Exception ex)
            {
                throw new Exception("读取项目数据库过程中发生异常。", ex);
            }
        }

        /// <summary>
        /// 清空项目
        /// </summary>
        public static bool ClearProject()
        {
            bool result = true;

            try
            {
                DbDaoHelper dao = new DbDaoHelper(DataSources.PROJECT);
                string sql1 = "DELETE FROM Device";
                string sql2 = "DELETE FROM IPSetting";
                string sql3 = "DELETE FROM Item";
                string sql4 = "DELETE FROM ModbusGateway";
                string sql5 = "DELETE FROM ModbusMaster";
                string sql6 = "DELETE FROM ModbusRegister";
                string sql7 = "DELETE FROM ModbusSlave";
                string sql8 = "DELETE FROM SerialPort";
                string sql9 = "UPDATE sqlite_sequence SET seq = 0;VACUUM Database;";
                dao.ExecuteNonQuery(sql1);
                dao.ExecuteNonQuery(sql2); 
                dao.ExecuteNonQuery(sql3);
                dao.ExecuteNonQuery(sql4);
                dao.ExecuteNonQuery(sql5);
                dao.ExecuteNonQuery(sql6);
                dao.ExecuteNonQuery(sql7);
                dao.ExecuteNonQuery(sql8);
                dao.ExecuteNonQuery(sql9);
            }
            catch
            {
                throw;
            }


            return result;
        }
    }
}
