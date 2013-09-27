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
using System.ComponentModel;
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

        //小数精度
        private int _precision;

        //最大值
        private double _maximum;

        //最小值
        private double _minimum;

        //是否启用
        private bool _isEnable;

        /// <summary>
        /// 唯一标识
        /// </summary>
        [Browsable(false)]
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// 名称
        /// </summary>
        [Browsable(true)]
        [Category("\t\t基本")]
        [DisplayName("名称")]
        [Description("变量名称")]
        [ReadOnly(true)]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// 别名
        /// </summary>
        [Browsable(true)]
        [Category("\t\t基本")]
        [DisplayName("别名")]
        [Description("数据的别名")]
        [ReadOnly(true)]
        public string Alias
        {
            get { return _alias; }
            set { _alias = value; }
        }

        /// <summary>
        /// 标识码
        /// </summary>
        [Browsable(true)]
        [Category("\t\t基本")]
        [DisplayName("标识码")]
        [Description("与EMS系统的仪表参数对应的标识码")]
        [ReadOnly(true)]
        public int Code
        {
            get { return _code; }
            set { _code = value; }
        }

        /// <summary>
        /// 功能区
        /// </summary>
        [Browsable(true)]
        [Category("\t\t数据")]
        [DisplayName("功能区")]
        [Description("Modbus 功能区")]
        [ReadOnly(true)]
        public ModbusDataModels TableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }

        /// <summary>
        /// 访问属性
        /// </summary>
        [Browsable(true)]
        [Category("\t\t数据")]
        [DisplayName("访问属性")]
        [Description("访问属性")]
        [ReadOnly(true)]
        public AccessRights Access
        {
            get { return _access; }
            set { _access = value; }
        }

        /// <summary>
        /// 数据类型
        /// </summary>
        [Browsable(true)]
        [Category("\t\t数据")]
        [DisplayName("数据类型")]
        [Description("数据类型")]
        [ReadOnly(true)]
        public DataTypes DataType
        {
            get { return _dataType; }
            set { _dataType = value; }
        }

        /// <summary>
        /// 寄存器地址
        /// </summary>
        [Browsable(true)]
        [Category("\t\t数据")]
        [DisplayName("寄存器地址")]
        [Description("寄存器地址，十六进制表示")]
        [ReadOnly(true)]
        public string Address
        {
            get { return _address; }
            set { _address = value; }
        }

        /// <summary>
        /// 寄存器长度
        /// </summary>
        [Browsable(true)]
        [Category("\t\t数据")]
        [DisplayName("寄存器长度")]
        [Description("寄存器长度")]
        [ReadOnly(true)]
        public int Length
        {
            get { return _length; }
            set { _length = value; }
        }

        /// <summary>
        /// 刷新周期
        /// </summary>
        [Browsable(true)]
        [Category("\t\t数据")]
        [DisplayName("刷新周期")]
        [Description("刷新周期")]
        [ReadOnly(true)]
        public int ScanPeriod
        {
            get { return _scanPeriod; }
            set { _scanPeriod = value; }
        }

        /// <summary>
        /// 小数精度
        /// </summary>
        [Browsable(true)]
        [Category("\t\t高级")]
        [DisplayName("小数精度")]
        [Description("小数精度")]
        [ReadOnly(true)]
        public int Precision
        {
            get { return _precision; }
            set { _precision = value; }
        }

        /// <summary>
        /// 最大值
        /// </summary>
        [Browsable(true)]
        [Category("\t\t高级")]
        [DisplayName("最大有效值")]
        [Description("最大有效值")]
        [ReadOnly(true)]
        public double Maximum
        {
            get { return _maximum; }
            set { _maximum = value; }
        }

        /// <summary>
        /// 最小值
        /// </summary>
        [Browsable(true)]
        [Category("\t\t高级")]
        [DisplayName("最小有效值")]
        [Description("最小有效值")]
        [ReadOnly(true)]
        public double Minimum
        {
            get { return _minimum; }
            set { _minimum = value; }
        }

        /// <summary>
        /// 是否启用
        /// </summary>
        [Browsable(true)]
        [Category("\t\t高级")]
        [DisplayName("启用")]
        [Description("启用")]
        [ReadOnly(true)]
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
