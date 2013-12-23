/**
 * 文件名：OPCGateway.cs
 * 说明：OPC网关类
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

namespace ConfigEditor.Core.Models
{
   public class OPCGateway
    {
        //OPC网关编号
        private long _serialID;

        //名称
        private string _name;

        //别名
        private string _allias;

        //启用
        private string _enable;

        /// <summary>
        /// OPC网关编号
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
    }
}
