/**
 * 文件名：ItemViewMdoel.cs
 * 说明：变量实体模型
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

namespace ConfigEditor.Core.ViewModels
{
    /// <summary>
    /// 变量实体模型
    /// </summary>
    public class ItemViewModel
    {
        //唯一标识
        private int _id;

        //名称
        private string _name;

        //别名
        private string _alias;

        //标识码
        private int _code;

        //功能区
        private ModbusDataModels _tableName;

        //访问属性
        private AccessRights _access;

        //数据类型
        private DataTypes _dataType;

        //寄存器地址
        private string _address;

        //寄存器长度
        private int _length;

        //刷新周期
        private int _scanPeriod;

        //最大值
        private double _maximum;

        //最小值
        private double _minimum;

        //是否启用
        private bool _isEnable;

        /// <summary>
        /// 唯一标识
        /// </summary>
        public int Id
        {
            get { return _id; }
            set { _id = value; }
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
        /// 别名
        /// </summary>
        public string Alias
        {
            get { return _alias; }
            set { _alias = value; }
        }

        /// <summary>
        /// 标识码
        /// </summary>
        public int Code
        {
            get { return _code; }
            set { _code = value; }
        }

        /// <summary>
        /// 功能区
        /// </summary>
        public ModbusDataModels TableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }

        /// <summary>
        /// 访问属性
        /// </summary>
        public AccessRights Access
        {
            get { return _access; }
            set { _access = value; }
        }

        /// <summary>
        /// 数据类型
        /// </summary>
        public DataTypes DataType
        {
            get { return _dataType; }
            set { _dataType = value; }
        }

        /// <summary>
        /// 寄存器地址
        /// </summary>
        public string Address
        {
            get { return _address; }
            set { _address = value; }
        }

        /// <summary>
        /// 寄存器长度
        /// </summary>
        public int Length
        {
            get { return _length; }
            set { _length = value; }
        }

        /// <summary>
        /// 刷新周期
        /// </summary>
        public int ScanPeriod
        {
            get { return _scanPeriod; }
            set { _scanPeriod = value; }
        }
        //小数精度
        private int _precision;

        /// <summary>
        /// 小数精度
        /// </summary>
        public int Precision
        {
            get { return _precision; }
            set { _precision = value; }
        }

        /// <summary>
        /// 最大值
        /// </summary>
        public double Maximum
        {
            get { return _maximum; }
            set { _maximum = value; }
        }

        /// <summary>
        /// 最小值
        /// </summary>
        public double Minimum
        {
            get { return _minimum; }
            set { _minimum = value; }
        }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable
        {
            get { return _isEnable; }
            set { _isEnable = value; }
        }
        
        public ItemViewModel()
        {
        }
    }
}
