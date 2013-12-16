/**
 * 文件名：DBConfig.cs
 * 说明：DB块配置
 * 作者：刘风彬
 * 更改记录： 
 * -------------------------------------------------------
 * 改动人 	时间 			原因
 * -------------------------------------------------------
 * 刘风彬 	2013-12-04		创建文件
 * -------------------------------------------------------
 */

namespace ConfigEditor.Core.Models
{
   public class DBConfig
    {

        //变量编号
        private long _serialID;

        //地址
        private string _address;

       //DB块
        private string _db;

       //连接名
        private string _connection;

       //读取DB块数据类型
        private string _dbtype;

       //起始地址
        private string _startaddress;

        //识别码
        private int _code;

        //读写
        private string _accessibility;

        //启用
        private string _enable;

       //读取长度
        private string _length;

        /// <summary>
        /// 变量编号
        /// </summary>
        public long SerialID
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
        ///读取DB块数据类型
        /// </summary>
        public string DBType
        {
            get { return _dbtype; }
            set { _dbtype = value; }
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
        /// 识别码
        /// </summary>
        public int Code
        {
            get { return _code; }
            set { _code = value; }
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
        /// 启用
        /// </summary>
        public string Enable
        {
            get { return _enable; }
            set { _enable = value; }
        }

        /// <summary>
        /// 读取长度
        /// </summary>
        public string Length
        {
            get { return _length; }
            set { _length = value; }
        }
    }
}
