/**
 * 文件名：XmlFileGenerator.cs
 * 说明：生成设备变量模板
 * 作者：林安城
 * 更改记录： 
 * -------------------------------------------------------
 * 改动人 	时间 			原因
 * -------------------------------------------------------
 * 林安城 	2013-10-12		创建文件
 * -------------------------------------------------------
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using ConfigEditor.Core.Models;

namespace ConfigEditor.Core.Xml
{
    /// <summary>
    /// 生成设备变量模板
    /// </summary>
    public class XmlFileGenerator
    {
        /// <summary>
        /// 康耐德C2000 MD44
        /// </summary>
        public static void CreateMD44File()
        {
            List<XmlItem> items = new List<XmlItem>();
            items.Add(new XmlItem() { Name = "DO0", Address = "0x0300", Length = 1, DataModel = ModbusDataModels.HoldingRegisters.ToString(), DataType = DataTypes.Discrete.ToString(), Access = AccessRights.ReadWrite.ToString() });
            items.Add(new XmlItem() { Name = "DO1", Address = "0x0301", Length = 1, DataModel = ModbusDataModels.HoldingRegisters.ToString(), DataType = DataTypes.Discrete.ToString(), Access = AccessRights.ReadWrite.ToString() });
            items.Add(new XmlItem() { Name = "DO2", Address = "0x0302", Length = 1, DataModel = ModbusDataModels.HoldingRegisters.ToString(), DataType = DataTypes.Discrete.ToString(), Access = AccessRights.ReadWrite.ToString() });
            items.Add(new XmlItem() { Name = "DO3", Address = "0x0303", Length = 1, DataModel = ModbusDataModels.HoldingRegisters.ToString(), DataType = DataTypes.Discrete.ToString(), Access = AccessRights.ReadWrite.ToString() });

            items.Add(new XmlItem() { Name = "DI0", Address = "0x0308", Length = 1, DataModel = ModbusDataModels.HoldingRegisters.ToString(), DataType = DataTypes.Discrete.ToString(), Access = AccessRights.ReadOnly.ToString() });
            items.Add(new XmlItem() { Name = "DI1", Address = "0x0309", Length = 1, DataModel = ModbusDataModels.HoldingRegisters.ToString(), DataType = DataTypes.Discrete.ToString(), Access = AccessRights.ReadOnly.ToString() });
            items.Add(new XmlItem() { Name = "DI2", Address = "0x030A", Length = 1, DataModel = ModbusDataModels.HoldingRegisters.ToString(), DataType = DataTypes.Discrete.ToString(), Access = AccessRights.ReadOnly.ToString() });
            items.Add(new XmlItem() { Name = "DI3", Address = "0x030B", Length = 1, DataModel = ModbusDataModels.HoldingRegisters.ToString(), DataType = DataTypes.Discrete.ToString(), Access = AccessRights.ReadOnly.ToString() });

            string name = "康耐德C2000 MD44";
            XmlDevice device = new XmlDevice(name);
            device.Protocols = new string[] { "ModbusRTU" };
            device.Items = items.ToArray();

            string xmlFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"Devices\{0}.xml", name));
            XmlSerializeHelper.Serialize(device, xmlFile);
        }

        /// <summary>
        /// 康耐德C2000 MDIA
        /// </summary>
        public static void CreateMDIAFile()
        {
            List<XmlItem> items = new List<XmlItem>();
            items.Add(new XmlItem() { Name = "DI0", Address = "0x0319", Length = 1, DataModel = ModbusDataModels.HoldingRegisters.ToString(), DataType = DataTypes.Discrete.ToString(), Access = AccessRights.ReadOnly.ToString() });
            items.Add(new XmlItem() { Name = "DI1", Address = "0x031A", Length = 1, DataModel = ModbusDataModels.HoldingRegisters.ToString(), DataType = DataTypes.Discrete.ToString(), Access = AccessRights.ReadOnly.ToString() });

            items.Add(new XmlItem() { Name = "AI0", Address = "0x0511", Length = 1, DataModel = ModbusDataModels.HoldingRegisters.ToString(), DataType = DataTypes.Integer.ToString(), Access = AccessRights.ReadOnly.ToString() });
            items.Add(new XmlItem() { Name = "AI1", Address = "0x0512", Length = 1, DataModel = ModbusDataModels.HoldingRegisters.ToString(), DataType = DataTypes.Integer.ToString(), Access = AccessRights.ReadOnly.ToString() });
            items.Add(new XmlItem() { Name = "AI2", Address = "0x0513", Length = 1, DataModel = ModbusDataModels.HoldingRegisters.ToString(), DataType = DataTypes.Integer.ToString(), Access = AccessRights.ReadOnly.ToString() });
            items.Add(new XmlItem() { Name = "AI3", Address = "0x0514", Length = 1, DataModel = ModbusDataModels.HoldingRegisters.ToString(), DataType = DataTypes.Integer.ToString(), Access = AccessRights.ReadOnly.ToString() });
            items.Add(new XmlItem() { Name = "AI4", Address = "0x0515", Length = 1, DataModel = ModbusDataModels.HoldingRegisters.ToString(), DataType = DataTypes.Integer.ToString(), Access = AccessRights.ReadOnly.ToString() });
            items.Add(new XmlItem() { Name = "AI5", Address = "0x0516", Length = 1, DataModel = ModbusDataModels.HoldingRegisters.ToString(), DataType = DataTypes.Integer.ToString(), Access = AccessRights.ReadOnly.ToString() });
            items.Add(new XmlItem() { Name = "AI6", Address = "0x0517", Length = 1, DataModel = ModbusDataModels.HoldingRegisters.ToString(), DataType = DataTypes.Integer.ToString(), Access = AccessRights.ReadOnly.ToString() });
            items.Add(new XmlItem() { Name = "AI7", Address = "0x0518", Length = 1, DataModel = ModbusDataModels.HoldingRegisters.ToString(), DataType = DataTypes.Integer.ToString(), Access = AccessRights.ReadOnly.ToString() });

            string name = "康耐德C2000 MDIA";
            XmlDevice device = new XmlDevice(name);
            device.Protocols = new string[] { "ModbusRTU" };
            device.Items = items.ToArray();

            string xmlFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"Devices\{0}.xml", name));
            XmlSerializeHelper.Serialize(device, xmlFile);
        }

        /// <summary>
        /// Lighthouse R5104
        /// </summary>
        public static void CreateR5104File()
        {
            List<XmlItem> items = new List<XmlItem>();
            items.Add(new XmlItem() { Name = "Status", Address = string.Format("0x{0:X4}", 30007 - 30001), Length = 2, DataModel = ModbusDataModels.InputRegisters.ToString(), DataType = DataTypes.Integer.ToString(), Access = AccessRights.ReadOnly.ToString() });
            items.Add(new XmlItem() { Name = "Channel1", Address = string.Format("0x{0:X4}", 30009 - 30001), Length = 2, DataModel = ModbusDataModels.InputRegisters.ToString(), DataType = DataTypes.Integer.ToString(), Access = AccessRights.ReadOnly.ToString() });
            items.Add(new XmlItem() { Name = "Channel2", Address = string.Format("0x{0:X4}", 30011 - 30001), Length = 2, DataModel = ModbusDataModels.InputRegisters.ToString(), DataType = DataTypes.Integer.ToString(), Access = AccessRights.ReadOnly.ToString() });

            string name = "Lighthouse R5104";
            XmlDevice device = new XmlDevice(name);
            device.Protocols = new string[] { "ModbusASCII" };
            device.Items = items.ToArray();

            string xmlFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"Devices\{0}.xml", name));
            XmlSerializeHelper.Serialize(device, xmlFile);
        }

        /// <summary>
        /// Lighthouse R5104V
        /// </summary>
        public static void CreateR5104VFile()
        {
            List<XmlItem> items = new List<XmlItem>();
            items.Add(new XmlItem() { Name = "Status", Address = string.Format("0x{0:X4}", 30007 - 30001), Length = 2, DataModel = ModbusDataModels.InputRegisters.ToString(), DataType = DataTypes.Integer.ToString(), Access = AccessRights.ReadOnly.ToString() });
            items.Add(new XmlItem() { Name = "Channel1", Address = string.Format("0x{0:X4}", 30009 - 30001), Length = 2, DataModel = ModbusDataModels.InputRegisters.ToString(), DataType = DataTypes.Integer.ToString(), Access = AccessRights.ReadOnly.ToString() });
            items.Add(new XmlItem() { Name = "Channel2", Address = string.Format("0x{0:X4}", 30011 - 30001), Length = 2, DataModel = ModbusDataModels.InputRegisters.ToString(), DataType = DataTypes.Integer.ToString(), Access = AccessRights.ReadOnly.ToString() });

            string name = "Lighthouse R5104V";
            XmlDevice device = new XmlDevice(name);
            device.Protocols = new string[] { "ModbusASCII" };
            device.Items = items.ToArray();

            string xmlFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"Devices\{0}.xml", name));
            XmlSerializeHelper.Serialize(device, xmlFile);
        }
    }
}
