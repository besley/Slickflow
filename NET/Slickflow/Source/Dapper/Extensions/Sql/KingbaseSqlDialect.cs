using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DapperExtensions;

namespace DapperExtensions.Sql
{
    public class KingbaseSqlDialect: SqlDialectBase
    {
        public override char OpenQuote
        {
            get { return ' '; }
        }

        public override char CloseQuote
        {
            get { return ' '; }
        }

        public override char ParameterPrefix
        {
            get
            {
                return ':';
            }
        }

        public override string BatchSeperator
        {
            get { return string.Empty; }
        }

        public override string GetIdentitySql(string tableName)
        {
            var sql = string.Format("SELECT TO_NUMBER(nvl({0}_ID_SEQ.CURRVAL, 0)) AS ID FROM DUAL", tableName.Trim());
            return sql;
        }

        public override bool SupportsMultipleStatements
        {
            get { return false; }
        }

        public override string GetPagingSql(string sql, int page, int resultsPerPage, IDictionary<string, object> parameters)
        {
            int startValue = page * resultsPerPage;
            return GetSetSql(sql, startValue, resultsPerPage, parameters);
        }

        public override string GetSetSql(string sql, int firstResult, int maxResults, IDictionary<string, object> parameters)
        {
            string result = string.Format("{0} LIMIT @firstResult OFFSET @pageStartRowNbr", sql);
            parameters.Add("@firstResult", firstResult);
            parameters.Add("@maxResults", maxResults);
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
