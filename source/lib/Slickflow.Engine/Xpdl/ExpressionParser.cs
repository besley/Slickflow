using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Slickflow.Engine.Xpdl
{
    /// <summary>
    /// 条件表达式解析器
    /// </summary>
    internal class ExpressionParser
    {
        /// <summary>
        /// 解析条件表达式
        /// </summary>
        /// <param name="expressionReplaced">字符串</param>
        /// <returns>解析结果</returns>
        internal static bool Parse(string expressionReplaced)
        {
            Expression e = System.Linq.Dynamic.DynamicExpression.Parse(typeof(Boolean), expressionReplaced);
            LambdaExpression LE = Expression.Lambda(e);
            Func<bool> testMe = (Func<bool>)LE.Compile();
            var result = testMe();
            return result;
        }

        /// <summary>
        /// 取代条件表达式中的参数值
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <param name="dictoinary">列表</param>
        /// <returns>表达式</returns>
        internal static string ReplaceParameterToValue(string expression, IDictionary<string, string> dictoinary)
        {
            foreach (KeyValuePair<string, string> p in dictoinary)
            {
                if (p.Value == string.Empty /* hacked by shiyonglin 2018-4-24*/
                    || !ExpressionParser.IsNumeric(p.Value))
                {
                    //字符串类型的变量处理，加上双引号。
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
        /// 判断字符串是否是数字类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNumeric(string value)
        {
            return Regex.IsMatch(value, @"^[+-]?\d*[.]?\d*$");
        }
    }
}
