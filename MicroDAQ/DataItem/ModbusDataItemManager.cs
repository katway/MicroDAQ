using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ModbusOperate;
using System.IO.Ports;
using System.Reflection;
using Modbus.Device;

namespace MicroDAQ.DataItem
{
   public class ModbusDataItemManager:IDataItemManage
    {

        public IList<Item> Items { get; set; }
        public ConnectionState ConnectionState { get; set; }
        IModbus Imodbus;
        Dictionary<int, string> modbusProperty;  //要读取的属性名字和ID
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ModbusType">组件类型</param>
        /// <param name="slave">地址</param>
        /// <param name="dic">id和属性名称</param>
       public ModbusDataItemManager(string ModbusType, int slave, Dictionary<int, string> dic, SerialPort port)
        {
            modbusProperty = dic;
            Imodbus = (IModbus)Assembly.Load("MicroDAQ").CreateInstance("ModbusOperate." + ModbusType);
            Imodbus.ConnectionPort(port);
            Imodbus.GetType().GetProperty("SlaveAdress").SetValue(Imodbus, slave, null);
        }
       public void ModbusReadData()
        {
            Imodbus.ReadData();                      //读取属性值
            foreach (var dic in modbusProperty)      //遍历要读取的属性
            {
                Item item = new Item();
                PropertyInfo propertyInfo = Imodbus.GetType().GetProperty(dic.Value);
                item.Value =(float)propertyInfo.GetValue(Imodbus, null);
                item.ID = dic.Key;
                           
            }

        }

        public bool Connect(string OpcServerProgramID, string OPCServerIP)
        {
            throw new NotImplementedException();
        }
    }
}
