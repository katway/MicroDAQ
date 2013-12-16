/**
 * 文件名：DBConfigService.cs
 * 说明：DB块业务服务类
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
using ConfigEditor.Core.ViewModels;
using ConfigEditor.Core.Database;
using ConfigEditor.Core.Models;

namespace ConfigEditor.Core.Services
{
    /// <summary>
    /// DB块业务服务类
    /// </summary>
    public class DBConfigService
    {
        public DBConfigService()
        {
        }

        /// <summary>
        /// 添加DB块
        /// </summary>
        /// <param name="model"></param>
        public void AddDB(DBConfigViewModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("输入的参数为空。");
            }

            //创建OPCGateway对象
            OPCGatewayDao opcDao = new OPCGatewayDao();
            IList<OPCGateway> opcList = opcDao.GetAll();
            OPCGateway opc = null;
            if (opcList != null)
            {
                opc = opcList.SingleOrDefault(obj => obj.Name == "OPC");
                if (opc == null)
                {
                    opc = new OPCGateway()
                    {
                        Name = "OPC",
                        Allias = "OPC",
                        Enable = "True"
                    };

                    opcDao.Insert(opc);

                    opc.SerialID = opcDao.GetLastSerialID();
                }
            }

            DBConfig db = new DBConfig()
            {
                DB = model.DB,
                Connection = model.Connection,
                DBType = model.DBType,
                StartAddress = model.StartAddress,
                Length = model.Length,
                Accessibility = model.Accessibility.ToString(),
                Address = model.Address,
                Code = model.Code.HasValue ? Convert.ToInt32(model.Code) : 0,
                Enable = model.IsEnable.ToString()
            };


            DBConfigDao dao = new DBConfigDao();
            dao.Insert(db);

            model.SerialID = dao.GetLastSerialID();

           
        }

        /// <summary>
        /// 编辑DB块
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public void EditDB(DBConfigViewModel model)
        {
            if (model == null || model.SerialID == 0)
            {
                throw new ArgumentNullException("输入的参数为空。");
            }

            DBConfig db = new DBConfig()
            {
                SerialID = model.SerialID,
                DB = model.DB,
                Connection = model.Connection,
                DBType = model.DBType,
                Accessibility = model.Accessibility.ToString(),
                Address = model.Address,
                Code = model.Code.HasValue ? Convert.ToInt32(model.Code) : 0,
                Enable = model.IsEnable.ToString()
            };

            DBConfigDao dao = new DBConfigDao();
            dao.Update(db);
        }

        /// <summary>
        /// 删除DB块
        /// </summary>
        /// <param name="id"></param>
        public void DeleteDB(int SerialID)
        {
            if (SerialID == 0)
            {
                throw new ArgumentNullException("输入的参数为空。");
            }

            DBConfigDao dao = new DBConfigDao();
            dao.Delete(SerialID);
        }
    }
}
