using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Slickflow.Engine.Xpdl
{
    /// <summary>
    /// Expression Parser
    /// </summary>
    internal class ExpressionParser
    {
        /// <summary>
        /// Parse expresson
        /// </summary>
        internal static bool Parse(string expressionReplaced)
        {
            Expression e = System.Linq.Dynamic.DynamicExpression.Parse(typeof(Boolean), expressionReplaced);
            LambdaExpression LE = Expression.Lambda(e);
            Func<bool> testMe = (Func<bool>)LE.Compile();
            var result = testMe();
            return result;
        }

        /// <summary>
        /// Replace parameter values in conditional expressions
        /// 取代条件表达式中的参数值
        /// </summary>
        internal static string ReplaceParameterToValue(string expression, IDictionary<string, string> dictoinary)
        {
            foreach (KeyValuePair<string, string> p in dictoinary)
            {
                if (p.Value == string.Empty /* hacked by shiyonglin 2018-4-24*/
                    || !ExpressionParser.IsNumeric(p.Value))
                {
                    //字符串类型的变量处理，加上双引号。
                    //Handling string type variables with double quotation marks.
                    string s = "\"" + p.Value.Trim('\"') + "\"";
                    expression = expression.Replace(p.Key, s);
                }
                else
                {
                    expression = expression.Replace(p.Key, p.Value);
                }
            }
            return expression;
        }

        /// <summary>
        /// Determine whether the string is of numeric type
        /// 判断字符串是否是数字类型
        /// </summary>
        public static bool IsNumeric(string value)
        {
            return Regex.IsMatch(value, @"^[+-]?\d*[.]?\d*$");
        }
    }
}
