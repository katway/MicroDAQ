/**
 * 文件名：Device.cs
 * 说明：设备类
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
    public class Device
    {
        //设备编号
        private long _serialID;
               
        //名称
        private string _name;

        //别名
        private string _allias;

        /// <summary>
        /// 设备编号
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

      
    }
}
