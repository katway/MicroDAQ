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
using ConfigEditor.Core.Models;
using ConfigEditor.Core.ViewModels;
using ConfigEditor.Core.Database;

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

                SerialPortDao spDao = new SerialPortDao();
                IList<SerialPort> spList = spDao.GetAll();
                foreach (SerialPort sp in spList)
                {
                    SerialPortViewModel spvm = new SerialPortViewModel()
                    {
                        Id = (int)sp.SerialID,
                        PortName = sp.Port,
                        DataBits = sp.Databits,
                        Parity = sp.Parity
                    };

                    project.SerialPorts.Add(spvm);
                }

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
        public static void Clear()
        {

        }
    }
}
