/**
 * 文件名：Item.cs
 * 说明：监测参数类
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
    public class Item
    {
        //监测参数编号
        private long _serialID;

        //名称
        private string _name;

        //别名
        private string _allias;

        //启用
        private string _enable;

        //识别码
        private int _code;

        /// <summary>
        /// 监测参数编号
        /// </summary>
        public long SerialID
        {
            get { return _serialID; }
            set { _serialID = value; }
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
        public string Allias
        {
            get { return _allias; }
            set { _allias = value; }
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
        /// 识别码
        /// </summary>
        public int Code
        {
            get { return _code; }
            set { _code = value; }
        }
    }
}
