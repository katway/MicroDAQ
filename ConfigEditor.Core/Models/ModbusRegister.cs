/**
 * 文件名：ModbusRegister.cs
 * 说明：监测变量类
 * 作者：刘风彬
 * 更改记录： 
 * -------------------------------------------------------
 * 改动人 	时间 			原因
 * -------------------------------------------------------
 * 刘风彬 	2013-09-29		创建文件
 * -------------------------------------------------------
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfigEditor.Core.Models
{
    public class ModbusRegister
    {
        //监测变量编号
        private long _serialID;

        //Modbus从机编号
        private long _modbusSlave_SerialID;

        //别名
        private string _allias;

        //名称
        private string _name;

        //功能区
        private int _regesiterType;

        //地址
        private int _regesiterAddress;

        // 数据类型
        private string _dataType;

        //长度
        private int _length;

        //读写
        private string _accessibility;

        //数值
        private decimal _value;

        //启用
        private string _enable;

        //刷新周期
        private int _scanPeriod;

        //最小有效值
        private decimal _minimum;

        //最大有效值
        private decimal _maximum;

        //原始数值
        private decimal _originalValue;

        //小数位数
        private int _decimalPlaces;

        //启用
        private string _enable2;

        /// <summary>
        /// 监测变量编号
        /// </summary>
        public long SerialID
        {
            get { return _serialID; }
            set { _serialID = value; }
        }

        /// <summary>
        /// Modbus从机编号
        /// </summary>
        public long ModbusSlave_SerialID
        {
            get { return _modbusSlave_SerialID; }
            set { _modbusSlave_SerialID = value; }
        }
        /// <summary>
        /// 别名
        /// </summary>
        public string Allias
        {
            get { return _allias; }
            set { _allias = value; }
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        ///  功能区
        /// </summary>
        public int RegesiterType
        {
            get { return _regesiterType; }
            set { _regesiterType = value; }
        }

        /// <summary>
        ///  地址
        /// </summary>
        public int RegesiterAddress
        {
            get { return _regesiterAddress; }
            set { _regesiterAddress = value; }
        }

        /// <summary>
        /// 数据类型
        /// </summary>
        public string DataType
        {
            get { return _dataType; }
            set { _dataType = value; }
        }

        /// <summary>
        /// 长度
        /// </summary>
        public int Length
        {
            get { return _length; }
            set { _length = value; }
        }

        /// <summary>
        /// 读写
        /// </summary>
        public string Accessibility
        {
            get { return _accessibility; }
            set { _accessibility = value; }
        }

        /// <summary>
        /// 数值
        /// </summary>
        public decimal Value
        {
            get { return _value; }
            set { _value = value; }
        }

        /// <summary>
        /// 启用
        /// </summary>
        public string Enable
        {
            get { return _enable; }
            set { _enable = value; }
        }

        /// <summary>
        /// 刷新周期
        /// </summary>
        public int ScanPeriod
        {
            get { return _scanPeriod; }
            set { _scanPeriod = value; }
        }

        /// <summary>
        /// 最小有效值
        /// </summary>
        public decimal Minimum
        {
            get { return _minimum; }
            set { _minimum = value; }
        }

        /// <summary>
        ///  最大有效值
        /// </summary>
        public decimal Maximum
        {
            get { return _maximum; }
            set { _maximum = value; }
        }

        /// <summary>
        /// 原始数值
        /// </summary>
        public decimal OriginalValue
        {
            get { return _originalValue; }
            set { _originalValue = value; }
        }

        /// <summary>
        /// 小数位数
        /// </summary>
        public int DecimalPlaces
        {
            get { return _decimalPlaces; }
            set { _decimalPlaces = value; }
        }

        /// <summary>
        /// 启用
        /// </summary>
        public string Enable2
        {
            get { return _enable2; }
            set { _enable2 = value; }
        }

    }
}
