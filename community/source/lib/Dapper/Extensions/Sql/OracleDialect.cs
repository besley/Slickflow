using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DapperExtensions;

namespace DapperExtensions.Sql
{
    /// <summary>
    /// Oracle SQL Dialect 
    /// </summary>
    public class OracleDialect : SqlDialectBase
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

        //from Simple.Data.Oracle implementation https://github.com/flq/Simple.Data.Oracle/blob/master/Simple.Data.Oracle/OraclePager.cs
        public override string GetPagingSql(string sql, int page, int resultsPerPage, IDictionary<string, object> parameters)
        {
            var toSkip = page * resultsPerPage;
            var topLimit = (page + 1) * resultsPerPage;

            var sb = new StringBuilder();
            sb.AppendLine("SELECT * FROM (");
            sb.AppendLine("SELECT \"_ss_dapper_1_\".*, ROWNUM RNUM FROM (");
            sb.Append(sql);
            sb.AppendLine(") \"_ss_dapper_1_\"");
            sb.AppendLine("WHERE ROWNUM <= :topLimit) \"_ss_dapper_2_\" ");
            sb.AppendLine("WHERE \"_ss_dapper_2_\".RNUM > :toSkip");

            parameters.Add(":topLimit", topLimit);
            parameters.Add(":toSkip", toSkip);

            return sb.ToString();
        }

        public override string GetSetSql(string sql, int firstResult, int maxResults, IDictionary<string, object> parameters)
        {
            var sb = new StringBuilder();
            sb.AppendLine("SELECT * FROM (");
            sb.AppendLine("SELECT \"_ss_dapper_1_\".*, ROWNUM RNUM FROM (");
            sb.Append(sql);
            sb.AppendLine(") \"_ss_dapper_1_\"");
            sb.AppendLine("WHERE ROWNUM <= :topLimit) \"_ss_dapper_2_\" ");
            sb.AppendLine("WHERE \"_ss_dapper_2_\".RNUM > :toSkip");

            parameters.Add(":topLimit", maxResults + firstResult);
            parameters.Add(":toSkip", firstResult);

            return sb.ToString();
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
