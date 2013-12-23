﻿/**
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
                OPCGatewayDao cpcDao = new OPCGatewayDao();
                DBConfigDao dbconfigDao = new DBConfigDao();


                IList<SerialPort> spList = spDao.GetAll().OrderBy(obj => obj.Port).ToList();
                IList<ModbusMaster> mmList = mmDao.GetAll();
                IList<ModbusSlave> msList = msDao.GetAll().OrderBy(obj => obj.Name).ToList(); ;
                IList<ModbusRegister> mrList = mrDao.GetAll().OrderBy(obj => obj.Name).ToList();
                IList<IPSetting> ipsList = ipsDao.GetAll();
                IList<Device> deviceList = deviceDao.GetAll();
                IList<Item> itemList = itemDao.GetAll();
                IList<DBConfig> dbconfigList = dbconfigDao.GetAll();

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
                            Protocol = EnumHelper.StringToEnum<ModbusProtocols>(item.Type),
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
                                Code = obj.Code != 0 ? (int?)obj.Code : null,
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

                    //设置串口通道的默认协议
                    if (spvm.Devices.Count > 0)
                    {
                        spvm.Protocol = spvm.Devices[0].Protocol;
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
                        Protocol = EnumHelper.StringToEnum<ModbusProtocols>(item.ms.Type),
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
                            Code = obj.Code != 0 ? (int?)obj.Code : null,
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

                #region OpcItems设备变量

                var query4 = from db in dbconfigList
                             where db.SerialID == db.SerialID
                             select db;

                foreach (var item in query4)
                {
                    DBConfigViewModel dbfvm = new DBConfigViewModel()
                    {
                        SerialID = (int)item.SerialID,
                        Connection = item.Connection,
                        DB = item.DB,
                        DBType = item.DBType,
                        Accessibility = EnumHelper.StringToEnum<AccessRights>(item.Accessibility),
                        Address = item.Address ,
                        Type = ChannelTypes.OpcItems,
                        Code = item.Code != 0 ? (int?)item.Code : null,
                        IsEnable = Convert.ToBoolean(item.Enable)
                    };

                    
                    project.OpcItems.Add(dbfvm);

                 
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

        public static bool Clear()
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
                string sql9 = "DELETE FROM OPCGateWay";
                string sql10 = "DELETE FROM DBConfig";
                string sql11 = "UPDATE sqlite_sequence SET seq = 0;VACUUM Database;";
               
                
                dao.ExecuteNonQuery(sql1);
                dao.ExecuteNonQuery(sql2);
                dao.ExecuteNonQuery(sql3);
                dao.ExecuteNonQuery(sql4);
                dao.ExecuteNonQuery(sql5);
                dao.ExecuteNonQuery(sql6);
                dao.ExecuteNonQuery(sql7);
                dao.ExecuteNonQuery(sql8);
                dao.ExecuteNonQuery(sql9);
                dao.ExecuteNonQuery(sql10);
                dao.ExecuteNonQuery(sql11);
            }
            catch
            {
                throw;
            }


            return result;

        }
    }
}