/**
 * 文件名：SqlMapHelper.cs
 * 说明：SQL语句映射类
 * 作者：林安城
 * 更改记录： 
 * -------------------------------------------------------
 * 改动人 	时间 			原因
 * -------------------------------------------------------
 * 林安城 	2013-09-29		创建文件
 * -------------------------------------------------------
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Xml.Linq;

namespace ConfigEditor.Util
{
    /// <summary>
    /// SQL语句映射类
    /// </summary>
    public class SqlMapHelper
    {
        public const string CountItems = "count_items";

        public const string CreateItems = "create_items";

        public static readonly Dictionary<string, string> SqlMaps = new Dictionary<string,string>();

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static SqlMapHelper()
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string fileName = Path.Combine(path, "slqmap.xml");
            if (!File.Exists(fileName))
            {
                throw new Exception("slqmap.xml 映射文件不存在。");
            }

            XDocument doc = XDocument.Load(fileName);
            var query = from item in doc.Element("sqlMap").Element("statements").Elements()
                        select item;
            foreach (var item in query)
            {
                string id = item.Attribute("id").Value;
                string sql = item.Value;

                SqlMaps[id] = sql;
            }
        }

        /// <summary>
        /// 生成SQL语句
        /// </summary>
        /// <param name="sqlId"></param>
        /// <param name="pars"></param>
        /// <returns></returns>
        public static string Format(string sqlId, Dictionary<string, object> pars)
        {
            if (SqlMaps == null || !SqlMaps.ContainsKey(sqlId))
            {
                throw new Exception("映射文件找不到对应的SQL语句。");
            }

            StringBuilder sql = new StringBuilder(SqlMaps[sqlId]);
            foreach (var pair in pars)
            {
                sql.Replace(string.Format("#{{{0}}}", pair.Key), string.Format(pair.Value.GetType().IsValueType ? "{0}" : "'{0}'", pair.Value));
            }

            return sql.ToString();
        }
    }
}
