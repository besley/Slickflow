using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DapperExtensions.Sql
{
    public class PostgreSqlDialect : SqlDialectBase
    {
        public override string GetIdentitySql(string tableName)
        {
            return "SELECT LASTVAL() AS Id";
        }

        public override string GetPagingSql(string sql, int page, int resultsPerPage, IDictionary<string, object> parameters)
        {
            int startValue = page * resultsPerPage;
            return GetSetSql(sql, startValue, resultsPerPage, parameters);
        }

        public override string GetSetSql(string sql, int firstResult, int maxResults, IDictionary<string, object> parameters)
        {
            // 修正1：交换参数位置，使含义更清晰
            // LIMIT 应该对应 maxResults（每页数量）
            // OFFSET 应该对应 firstResult（跳过数量）
            string result = $"{sql} LIMIT @pageSize OFFSET @skipRows";

            // 修正2：参数名与 SQL 中的占位符一致
            parameters.Add("@pageSize", maxResults);     // 每页显示多少条
            parameters.Add("@skipRows", firstResult);    // 跳过多少条

            return result;
        }

        public override string GetColumnName(string prefix, string columnName, string alias)
        {
            return base.GetColumnName(null, columnName, alias).ToLower();
        }

        public override string GetTableName(string schemaName, string tableName, string alias)
        {
            return base.GetTableName(schemaName, tableName, alias).ToLower();
        }
    }

}