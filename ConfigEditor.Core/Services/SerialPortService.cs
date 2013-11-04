/**
 * 文件名：SerialPortService.cs
 * 说明：串口业务服务类
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
    /// 串口业务服务类
    /// </summary>
    public class SerialPortService
    {
        public SerialPortService()
        {
        }

        /// <summary>
        /// 添加串口
        /// </summary>
        /// <param name="model"></param>
        public void AddSerialPort(SerialPortViewModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("输入的参数为空。");
            }

            //创建ModbusGateway对象
            ModbusGatewayDao mgDao = new ModbusGatewayDao();
            IList<ModbusGateway> mgList = mgDao.GetAll();
            ModbusGateway mg = null;
            if (mgList != null)
            {
                mg = mgList.SingleOrDefault(obj => obj.Name == "Modbus");
                if (mg == null)
                {
                    mg = new ModbusGateway()
                    {
                        Name = "Modbus",
                        Allias = "Modbus",
                        Enable = "True"
                    };

                    mgDao.Insert(mg);

                    mg.SerialID = mgDao.GetLastSerialID();
                }
            }

            SerialPort sp = new SerialPort()
            {
                Port = model.PortName,
                BaudRate = model.BaudRate,
                Databits = model.DataBits,
                Stopbits = (model.StopBits != "1.5") ? Convert.ToInt32(model.StopBits) : 3,
                Parity = model.Parity,
                Enable = model.IsEnable.ToString()
            };

            SerialPortDao dao = new SerialPortDao();
            dao.Insert(sp);

            model.Id = dao.GetLastSerialID();

            ModbusMaster mm = new ModbusMaster()
            {
                SerialPort_SerialID = model.Id,
                ModbusGateway_SerialID = (mg != null) ? mg.SerialID : 0,
                Name = model.PortName,
                Allias = model.PortName,
                Enable = model.IsEnable.ToString()
            };

            ModbusMasterDao mmDao = new ModbusMasterDao();
            mmDao.Insert(mm);
        }

        /// <summary>
        /// 编辑串口
        /// </summary>
        /// <param name="model"></param>
        public void EditSerialPort(SerialPortViewModel model)
        {
            if (model == null || model.Id == 0)
            {
                throw new ArgumentNullException("输入的参数为空。");
            }

            SerialPort sp = new SerialPort()
            {
                SerialID = model.Id,
                Port = model.PortName,
                BaudRate = model.BaudRate,
                Databits = model.DataBits,
                Stopbits = (model.StopBits != "1.5") ? Convert.ToInt32(model.StopBits) : 3,
                Parity = model.Parity,
                Enable = model.IsEnable.ToString()
            };

            SerialPortDao dao = new SerialPortDao();
            dao.Update(sp);
        }

        /// <summary>
        /// 删除串口
        /// </summary>
        /// <param name="id"></param>
        public void DeleteSerialPort(SerialPortViewModel model)
        {
            if (model == null || model.Id == 0)
            {
                throw new ArgumentNullException("输入的参数为空。");
            }

            //删除设备和变量
            DeviceService service = new DeviceService();
            foreach (var device in model.Devices)
            {
                service.DeleteDevice(device);
            }

            //删除主机
            ModbusMasterDao mmDao = new ModbusMasterDao();
            ModbusMaster mm = mmDao.GetBySerialPortID(model.Id);
            mmDao.Delete((int)mm.SerialID);

            SerialPortDao dao = new SerialPortDao();
            dao.Delete(model.Id);
        }

        /// <summary>
        /// 删除串口
        /// </summary>
        /// <param name="id"></param>
        public void DeleteSerialPort(int id)
        {
            if (id == 0)
            {
                throw new ArgumentNullException("输入的参数为空。");
            }

            //Todo:删除设备和变量

            //删除主机
            ModbusMasterDao mmDao = new ModbusMasterDao();
            ModbusMaster mm = mmDao.GetBySerialPortID(id);
            mmDao.Delete((int)mm.SerialID);

            SerialPortDao dao = new SerialPortDao();
            dao.Delete(id);
        }
    }
}
