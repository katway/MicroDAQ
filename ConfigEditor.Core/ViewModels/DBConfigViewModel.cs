/**
 * 文件名：DBConfigViewModel.cs
 * 说明：DB块实体模型
 * 作者：刘风彬
 * 更改记录： 
 * -------------------------------------------------------
 * 改动人 	时间 			原因
 * -------------------------------------------------------
 * 刘风彬 	2013-12-04		创建文件
 * -------------------------------------------------------
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConfigEditor.Core.Models;

namespace ConfigEditor.Core.ViewModels
{
    public class DBConfigViewModel : ChannelBase
    {


        //变量编号
        private int _serialID;

        //地址
        private string _address;

        //DB块
        private string _db;

        //连接名
        private string _connection;

        //起始地址
        private string _startaddress;

        //读取DB块数据类型
        private string _dbtype;

        //读取长度
        private string _length;

        //识别码
        private int? _code;

        //读写
        private AccessRights _accessibility;

        //是否启用
        private bool _isEnable;

        //变量列表
        private List<ItemViewModel> _items;


        /// <summary>
        /// 变量编号
        /// </summary>
        public int SerialID
        {
            get { return _serialID; }
            set { _serialID = value; }
        }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address
        {
            get { return _address; }
            set { _address = value; }
        }

        /// <summary>
        /// 连接名
        /// </summary>
        public string Connection
        {
            get { return _connection; }
            set { _connection = value; }
        }

        /// <summary>
        /// DB块
        /// </summary>
        public string DB
        {
            get { return _db; }
            set { _db = value; }
        }

        /// <summary>
        /// 起始地址
        /// </summary>
        public string StartAddress
        {
            get { return _startaddress; }
            set { _startaddress = value; }
        }

        /// <summary>
        ///读取DB块数据类型
        /// </summary>
        public string DBType
        {
            get { return _dbtype; }
            set { _dbtype = value; }
        }

        /// <summary>
        /// 识别码
        /// </summary>
        public int? Code
        {
            get { return _code; }
            set { _code = value; }
        }

        /// <summary>
        /// 读写
        /// </summary>
        public AccessRights Accessibility
        {
            get { return _accessibility; }
            set { _accessibility = value; }
        }

        /// <summary>
        /// 读取长度
        /// </summary>
        public string Length
        {
            get { return _length; }
            set { _length = value; }
        }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable
        {
            get { return _isEnable; }
            set { _isEnable = value; }
        }

        /// <summary>
        /// 变量列表
        /// </summary>
        public List<ItemViewModel> Items
        {
            get { return _items; }
            set { _items = value; }
        }

        public DBConfigViewModel()
        {
            Type = ChannelTypes.OpcItems;
            DBConfig = new List<DBConfigViewModel>();
            IsEnable = true;
        }

        public void Add(DBConfigViewModel model)
        {
            throw new NotImplementedException();
        }
    }
}
