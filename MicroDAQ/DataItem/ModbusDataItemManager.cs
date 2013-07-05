using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ModbusLibrary;
using System.IO.Ports;
using System.Reflection;
using Modbus.Device;

namespace MicroDAQ.DataItem
{
   public class ModbusDataItemManager:IDataItemManage
    {

        public IList<Item> Items { get; set; }
        public ConnectionState ConnectionState { get; set; }
        IModbusOperate Imodbus;
        Dictionary<int, string> modbusProperty;  //要读取的属性名字和ID
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ModbusType">组件类型</param>
        /// <param name="slave">地址</param>
        /// <param name="dic">id和属性名称</param>
       public ModbusDataItemManager(string ModbusType, byte slave, Dictionary<int, string> dic, SerialPort port)
        {
            #region 添加属性item
            Items = new List<Item>();
            for (int i = 0; i < dic.Count; i++)
            {
                Items.Add(new Item());
            }
                modbusProperty = dic;
            #endregion

            #region 设置波特率
                if (ModbusType == "ModMote5104")
                { port.BaudRate = 19200; }
                else
                { port.BaudRate = 9600; }
                #endregion

            #region 反射
                Imodbus = (IModbusOperate)Assembly.Load("ModbusLibrary").CreateInstance("ModbusLibrary." + ModbusType);
            Imodbus.ConnectionPort(port);
            Imodbus.GetType().GetProperty("SlaveAdress").SetValue(Imodbus, slave, null);
            #endregion
        }
       /// <summary>
       /// 读取属性值到Item
       /// </summary>
       public void ModbusReadData()
        {
            Imodbus.ReadData();   //读取属性值
            int i = 0;
            foreach (var  dic in modbusProperty)      //遍历要读取的属性
            {
                PropertyInfo propertyInfo = Imodbus.GetType().GetProperty(dic.Value);
                //测试

                Items[i].Value =Convert.ToSingle(propertyInfo.GetValue(Imodbus, null));
                Items[i].ID = dic.Key;
                i++;
                           
            }

        }

        public bool Connect(string OpcServerProgramID, string OPCServerIP)
        {
            throw new NotImplementedException();
        }
    }
}
