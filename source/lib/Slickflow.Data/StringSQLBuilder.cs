using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Data
{
    public enum Operator
    {
        Eq = 1,

        Ge = 2,

        Gt = 3,

        Le = 4,

        Lt = 5,

        Nq = 6,

        Like = 7
    }

    public class StringSQLBuilder
    {
        private StringBuilder stringSQLBuilder = new StringBuilder(1024);

        public StringSQLBuilder(string sql)
        {
            stringSQLBuilder.Append(sql);
        }

        private string GetStringOperator(Operator op)
        {
            var strOperator = string.Empty;
            if (op == Operator.Eq)
            {
                strOperator = "=";
            }
            else if (op == Operator.Le)
            {
                strOperator = "<=";
            }
            else if(op == Operator.Lt)
            {
                strOperator = "<";
            }
            else if(op == Operator.Ge)
            {
                strOperator = ">=";
            }
            else if(op == Operator.Gt)
            {
                strOperator = ">";
            }
            else if (op == Operator.Nq)
            {
                strOperator = "<>";
            }
            else if (op == Operator.Like)
            {
                strOperator = "like";
            }
            return strOperator;
        }
        public void And(string field, Operator op, dynamic variable)
        {
            var strOperator = GetStringOperator(op);
            var condition = string.Empty;
            if (op != Operator.Like)
            {
                condition = string.Format(" AND {0}{1}{2}", field, strOperator, variable);
            }
            else
            {
                string.Format(" AND {0} {1} %{2}% ", field, strOperator, variable);
            }
            
            stringSQLBuilder.Append(condition);
        }

        public void OrderBy(string field, bool isDesc = false)
        {
            var strOrderBy = string.Empty;
            if (isDesc == false)
            {
                strOrderBy = string.Format(" ORDER BY {0} ", field);
            }
            else
            {
                strOrderBy = string.Format(" ORDER BY {0} DESC", field);
            }
            stringSQLBuilder.Append(strOrderBy);
        }

        public string GetSQL()
        {
            return stringSQLBuilder.ToString();
        }
             
    }
}
