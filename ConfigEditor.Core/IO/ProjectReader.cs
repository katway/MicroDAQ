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
using ConfigEditor.Core.Util;

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
                ModbusMasterDao mmDao = new ModbusMasterDao();
                ModbusSlaveDao msDao = new ModbusSlaveDao();
                ModbusRegisterDao mrDao = new ModbusRegisterDao();
                IPSettingDao ipsDao = new IPSettingDao();
                DeviceDao deviceDao = new DeviceDao();
                ItemDao itemDao = new ItemDao();

                IList<SerialPort> spList = spDao.GetAll().OrderBy(obj => obj.Port).ToList();
                IList<ModbusMaster> mmList = mmDao.GetAll();
                IList<ModbusSlave> msList = msDao.GetAll().OrderBy(obj => obj.Name).ToList();;
                IList<ModbusRegister> mrList = mrDao.GetAll().OrderBy(obj => obj.Name).ToList();
                IList<IPSetting> ipsList = ipsDao.GetAll();
                IList<Device> deviceList = deviceDao.GetAll();
                IList<Item> itemList = itemDao.GetAll();

                #region 串口通道设备
                foreach (SerialPort sp in spList)
                {
                    SerialPortViewModel spvm = new SerialPortViewModel()
                    {
                        Id = (int)sp.SerialID,
                        PortName = sp.Port,
                        BaudRate = sp.BaudRate,
                        DataBits = sp.Databits,
                        Parity = sp.Parity,
                        StopBits = sp.Stopbits.ToString(),
                        Type = ChannelTypes.SerialPort,
                        Protocol = ModbusProtocols.ModbusRTU,
                        IsEnable = Convert.ToBoolean(sp.Enable)
                    };

                    ModbusMaster mm = mmList.SingleOrDefault(obj => obj.SerialPort_SerialID == sp.SerialID);
                    if (mm == null)
                    {
                        continue;
                    }

                    project.SerialPorts.Add(spvm);
                    var query1 = from ms in msList
                                 where ms.ModbusMaster_SerialID == mm.SerialID
                                 select ms;
                    foreach (var item in query1)
                    {
                        DeviceViewModel dvm = new DeviceViewModel()
                        {
                            Id = (int)item.SerialID,
                            Name = item.Name,
                            Alias = item.Allias,
                            ChannelType = ChannelTypes.SerialPort,
                            Protocol = ModbusProtocols.ModbusRTU,
                            Slave = item.Slave,
                            IpAddress = string.Empty,
                            IpPort = 0,
                            IsEnable = Convert.ToBoolean(item.Enable)
                        };

                        dvm.Channel = spvm;
                        spvm.Devices.Add(dvm);
                        var query2 = from mr in mrList
                                     where mr.ModbusSlave_SerialID == item.SerialID
                                     select mr;
                        foreach (var obj in query2)
                        {
                            ItemViewModel ivm = new ItemViewModel()
                            {
                                Id = (int)obj.SerialID,
                                Name = obj.Name,
                                Code = null,
                                Alias = obj.Allias,
                                TableName = (ModbusDataModels)obj.RegesiterType,
                                Access = EnumHelper.StringToEnum<AccessRights>(obj.Accessibility),
                                DataType = EnumHelper.StringToEnum<DataTypes>(obj.DataType),
                                Precision = obj.DecimalPlaces,
                                Address = string.Format("0x{0:X4}", obj.RegesiterAddress),
                                Length = obj.Length,
                                Minimum = (double?)obj.Minimum,
                                Maximum = (double?)obj.Maximum,
                                ScanPeriod = obj.ScanPeriod,
                                IsEnable = Convert.ToBoolean(obj.Enable)
                            };

                            ivm.Device = dvm;
                            dvm.Items.Add(ivm);
                        }
                    }
                }
                #endregion

                #region 以太网通道设备
                var query3 = from ips in ipsList
                             join ms in msList on ips.SerialID equals ms.IPSetting_SerialID
                             select new
                             {
                                 ips,
                                 ms
                             };

                foreach (var item in query3)
                {
                    DeviceViewModel dvm = new DeviceViewModel()
                    {
                        Id = (int)item.ms.SerialID,
                        Name = item.ms.Name,
                        Alias = item.ms.Allias,
                        ChannelType = ChannelTypes.Ethernet,
                        Protocol = ModbusProtocols.ModbusTCP,
                        Slave = item.ms.Slave,
                        IpAddress = item.ips.IP,
                        IpPort = item.ips.Port,
                        IsEnable = Convert.ToBoolean(item.ms.Enable)
                    };

                    dvm.Channel = project.Ethernet;
                    project.Ethernet.Devices.Add(dvm);
                    var query2 = from mr in mrList
                                 where mr.ModbusSlave_SerialID == item.ms.SerialID
                                 select mr;
                    foreach (var obj in query2)
                    {
                        ItemViewModel ivm = new ItemViewModel()
                        {
                            Id = (int)obj.SerialID,
                            Name = obj.Name,
                            Code = null,
                            Alias = obj.Allias,
                            TableName = (ModbusDataModels)obj.RegesiterType,
                            Access = EnumHelper.StringToEnum<AccessRights>(obj.Accessibility),
                            DataType = EnumHelper.StringToEnum<DataTypes>(obj.DataType),
                            Precision = obj.DecimalPlaces,
                            Address = string.Format("0x{0:X4}", obj.RegesiterAddress),
                            Length = obj.Length,
                            Minimum = (double?)obj.Minimum,
                            Maximum = (double?)obj.Maximum,
                            ScanPeriod = obj.ScanPeriod,
                            IsEnable = Convert.ToBoolean(obj.Enable)
                        };

                        ivm.Device = dvm;
                        dvm.Items.Add(ivm);
                    }
                }

                #endregion

                return project;
            }
            catch (Exception ex)
            {
                throw new Exception("读取项目数据库过程出现错误，请联系管理员。", ex);
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
