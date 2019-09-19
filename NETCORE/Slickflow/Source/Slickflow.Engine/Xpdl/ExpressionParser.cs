using System;
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
        /// <param name="filterString"></param>
        /// <returns></returns>
        public static bool Parse(string filterString)
        {
            //string filterString = "(12 % 3 == 0) and (true == true)";

            //Expression e = System.Linq.Dynamic.DynamicExpression.Parse(typeof(Boolean), filterString);
            Expression e = null;
            LambdaExpression LE = Expression.Lambda(e);
            Func<bool> testMe = (Func<bool>)LE.Compile();
            bool result = testMe();
            return result;
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
