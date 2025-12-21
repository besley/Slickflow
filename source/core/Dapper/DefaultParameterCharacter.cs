using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;

namespace Dapper
{
    /// <summary>
    /// 参数前缀
    /// </summary>
    public class DefaultParameterCharacter
    {
        /// <summary>
        /// 根据不同数据库加前缀
        /// </summary>
        /// <param name="cnn">连接</param>
        /// <param name="sql">sql语句</param>
        /// <returns>替换后的sql</returns>
        public static string ReplacePrefixParameter(IDbConnection cnn, string sql)
        {
            if (!string.IsNullOrEmpty(sql))
            {
                if (cnn.GetType().ToString().ToLowerInvariant().Contains("oracle"))
                {
                    sql = sql.Replace('@', ':');
                }
            }
            return sql;
        }
    }
}
