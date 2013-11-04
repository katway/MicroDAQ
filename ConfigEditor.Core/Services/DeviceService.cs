/**
 * 文件名：DeviceService.cs
 * 说明：设备业务服务类
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
using ConfigEditor.Core.Models;
using ConfigEditor.Core.Database;

namespace ConfigEditor.Core.Services
{
    /// <summary>
    /// 设备业务服务类
    /// </summary>
    public class DeviceService
    {
        /// <summary>
        /// 添加设备
        /// </summary>
        /// <param name="model"></param>
        public void AddDevice(DeviceViewModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("输入的参数为空。");
            }

            if (model.ChannelType == ChannelTypes.SerialPort)
            {
                //串口通道的从站
                SerialPortViewModel sp = model.Channel as SerialPortViewModel;

                ModbusMasterDao mmDao = new ModbusMasterDao();
                ModbusMaster mm = mmDao.GetBySerialPortID(sp.Id);

                ModbusSlave ms = new ModbusSlave()
                {
                    Name = model.Name,
                    Allias = model.Alias,
                    Type = model.Protocol.ToString(),
                    Slave = model.Slave,
                    ModbusMaster_SerialID = mm.SerialID,
                    IPSetting_SerialID = 0,
                    Enable = model.IsEnable.ToString()
                };

                ModbusSlaveDao dao = new ModbusSlaveDao();
                dao.Insert(ms);

                model.Id = dao.GetLastSerialID();
            }
            else
            {
                //以太网通道的从站
                ModbusMasterDao mmDao = new ModbusMasterDao();
                ModbusMaster mm = mmDao.GetBySerialPortID(0);
                if (mm == null)
                {
                    //插入记录
                    mm = new ModbusMaster()
                    {
                        SerialPort_SerialID = 0,
                        Name = "TCP",
                        Allias = "TCP",
                        Enable = model.IsEnable.ToString()
                    };

                    mmDao.Insert(mm);
                    mm.SerialID = mmDao.GetLastSerialID();
                }

                IPSetting ips = new IPSetting()
                {
                    IP = model.IpAddress,
                    Port = model.IpPort,
                    Enable = model.IsEnable.ToString()
                };

                IPSettingDao ipsDao = new IPSettingDao();
                ipsDao.Insert(ips);

                ModbusSlave ms = new ModbusSlave()
                {
                    Name = model.Name,
                    Allias = model.Alias,
                    Type = model.Protocol.ToString(),
                    Slave = model.Slave,
                    ModbusMaster_SerialID = (mm != null) ? mm.SerialID : 0,
                    IPSetting_SerialID = ipsDao.GetLastSerialID(),
                    Enable = model.IsEnable.ToString()
                };

                ModbusSlaveDao dao = new ModbusSlaveDao();
                dao.Insert(ms);

                model.Id = dao.GetLastSerialID();
            }

        }

        /// <summary>
        /// 编辑设备
        /// </summary>
        /// <param name="model"></param>
        public void EditDevice(DeviceViewModel model)
        {
            if (model == null || model.Id == 0)
            {
                throw new ArgumentNullException("输入的参数为空。");
            }

            if (model.ChannelType == ChannelTypes.SerialPort)
            {
                SerialPortViewModel sp = model.Channel as SerialPortViewModel;

                ModbusMasterDao mmDao = new ModbusMasterDao();
                ModbusMaster mm = mmDao.GetBySerialPortID(sp.Id);

                ModbusSlave ms = new ModbusSlave()
                {
                    SerialID = model.Id,
                    Name = model.Name,
                    Allias = model.Alias,
                    Type = model.Protocol.ToString(),
                    Slave = model.Slave,
                    ModbusMaster_SerialID = mm.SerialID,
                    IPSetting_SerialID = 0,
                    Enable = model.IsEnable.ToString()
                };

                ModbusSlaveDao dao = new ModbusSlaveDao();
                dao.Update(ms);
            }
            else
            {
                ModbusSlaveDao dao = new ModbusSlaveDao();
                ModbusSlave ms = dao.GetByID(model.Id);
                ms.Name = model.Name;
                ms.Allias = model.Alias;
                ms.Type = model.Protocol.ToString();
                ms.Slave = model.Slave;
                ms.Enable = model.IsEnable.ToString();
                dao.Update(ms);

                IPSettingDao ipsDao = new IPSettingDao();
                IPSetting ips = ipsDao.GetByID((int)ms.IPSetting_SerialID);
                ips.IP = model.IpAddress;
                ips.Port = model.IpPort;
                ips.Enable = model.IsEnable.ToString();
                ipsDao.Update(ips);
            }
        }

        /// <summary>
        /// 删除设备
        /// </summary>
        /// <param name="id"></param>
        public void DeleteDevice(DeviceViewModel model)
        {
            if (model == null || model.Id == 0)
            {
                throw new ArgumentNullException("输入的参数为空。");
            }

            //删除变量
            ModbusRegisterDao mrDao = new ModbusRegisterDao();
            foreach (var item in model.Items)
            {
                mrDao.Delete(item.Id);
            }

            //删除从机
            ModbusSlaveDao msDao = new ModbusSlaveDao();
            ModbusSlave ms = msDao.GetByID(model.Id);
            msDao.Delete(model.Id);

            //删除IP设置
            IPSettingDao ipsDao = new IPSettingDao();
            IList<IPSetting> ipsList = ipsDao.GetAll();
            foreach (IPSetting ips in ipsList)
            {
                if (ips.SerialID == ms.IPSetting_SerialID)
                {
                    ipsDao.Delete((int)ips.SerialID);
                }
            }
        }

        /// <summary>
        /// 删除设备
        /// </summary>
        /// <param name="id"></param>
        public void DeleteDevice(int id)
        {
            if (id == 0)
            {
                throw new ArgumentNullException("输入的参数为空。");
            }

            //Todo：删除变量

            ModbusSlaveDao dao = new ModbusSlaveDao();
            ModbusSlave model = dao.GetByID(id);
            dao.Delete(id);

            IPSettingDao ipsDao = new IPSettingDao();
            IList<IPSetting> ipsList = ipsDao.GetAll();
            foreach (IPSetting ips in ipsList)
            {
                if (ips.SerialID == model.IPSetting_SerialID)
                {
                    ipsDao.Delete((int)ips.SerialID);
                }
            }
        }
    }
}
