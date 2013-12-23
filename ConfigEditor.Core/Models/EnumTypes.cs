/**
 * 文件名：Enums.cs
 * 说明：常用枚举类型
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

namespace ConfigEditor.Core.Models
{
    /// <summary>
    /// 传输通道类型
    /// </summary>
    public enum ChannelTypes
    {
        //串口
        SerialPort,
        //以太网
        Ethernet,
        //DB块
        OpcItems
    }

    /// <summary>
    /// 数据类型
    /// </summary>
    public enum DataTypes
    {
        //整形
        Integer,
        //实型
        Real,
        //离散型
        Discrete,
        //字符串
        String
    }

    /// <summary>
    /// Modbus应用协议
    /// </summary>
    public enum ModbusProtocols
    {
        //Modbus RTU
        ModbusRTU,
        //Modbus ASCII
        ModbusASCII,
        //Modbus TCP
        ModbusTCP
    }

    /// <summary>
    /// Modbus数据模型
    /// </summary>
    public enum ModbusDataModels
    {
        //线圈
        Coils,
        //离散量输入
        DiscretesInput,
        //输入寄存器
        InputRegisters,
        //保持寄存器
        HoldingRegisters
    }

    /// <summary>
    /// 访问属性
    /// </summary>
    public enum AccessRights
    {
        //可读可写
        ReadWrite,
        //只读
        ReadOnly,
        //只写
        WriteOnly,
    }

    /// <summary>
    /// 数据源枚举类
    /// </summary>
    public enum DataSources
    {
        PROJECT
    }

    /// <summary>
    /// 用户操作
    /// </summary>
    public enum UserActions
    {
        //添加
        Add,
        //编辑
        Edit,
        //删除
        Delete
    }

   
}
