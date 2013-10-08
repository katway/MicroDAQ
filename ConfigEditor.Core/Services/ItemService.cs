/**
 * 文件名：ItemService.cs
 * 说明：变量业务服务类
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

namespace ConfigEditor.Core.Services
{
    /// <summary>
    /// 变量业务服务类
    /// </summary>
    public class ItemService
    {
        public ItemService()
        {
        }

        /// <summary>
        /// 添加变量
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public void AddItem(ItemViewModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("输入的参数为空。");
            }

            ModbusRegister mr = new ModbusRegister()
            {
                ModbusSlave_SerialID = model.Device.Id,
                Name = model.Name,
                Allias = model.Alias,
                RegesiterType = (int)model.TableName,
                RegesiterAddress = Convert.ToInt32(model.Address, 16),
                Length = model.Length,
                DataType = model.DataType.ToString(),
                DecimalPlaces = model.Precision.HasValue ? Convert.ToInt32(model.Precision) : 0,
                Accessibility = model.Access.ToString(),
                ScanPeriod = model.ScanPeriod,
                Minimum = model.Minimum.HasValue ? Convert.ToDecimal(model.Minimum) : 0,
                Maximum = model.Maximum.HasValue ? Convert.ToDecimal(model.Maximum) : 0,
                Enable = model.IsEnable.ToString()
            };

            ModbusRegisterDao dao = new ModbusRegisterDao();
            dao.Insert(mr);

            model.Id = dao.GetLastSerialID();

            Item item = new Item()
            {
                Name = model.Name,
                Code = model.Code.HasValue ? Convert.ToInt32(model.Code) : 0,
                Allias = model.Alias,
                Enable = model.IsEnable.ToString()
            };

            ItemDao iDao = new ItemDao();
            iDao.Insert(item);
        }

        /// <summary>
        /// 编辑变量
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public void EditItem(ItemViewModel model)
        {
            if (model == null || model.Id == 0)
            {
                throw new ArgumentNullException("输入的参数为空。");
            }

            ModbusRegister mr = new ModbusRegister()
            {
                SerialID = model.Id,
                ModbusSlave_SerialID = model.Device.Id,
                Name = model.Name,
                Allias = model.Alias,
                RegesiterType = (int)model.TableName,
                RegesiterAddress = Convert.ToInt32(model.Address, 16),
                Length = model.Length,
                DataType = model.DataType.ToString(),
                DecimalPlaces = model.Precision.HasValue ? Convert.ToInt32(model.Precision) : 0,
                Accessibility = model.Access.ToString(),
                ScanPeriod = model.ScanPeriod,
                Minimum = model.Minimum.HasValue ? Convert.ToDecimal(model.Minimum) : 0,
                Maximum = model.Maximum.HasValue ? Convert.ToDecimal(model.Maximum) : 0,
                Enable = model.IsEnable.ToString()
            };

            ModbusRegisterDao dao = new ModbusRegisterDao();
            dao.Update(mr);
        }

        /// <summary>
        /// 删除变量
        /// </summary>
        /// <param name="id"></param>
        public void DeleteItem(int id)
        {
            if (id == 0)
            {
                throw new ArgumentNullException("输入的参数为空。");
            }

            ModbusRegisterDao dao = new ModbusRegisterDao();
            dao.Delete(id);
        }
    }
}
