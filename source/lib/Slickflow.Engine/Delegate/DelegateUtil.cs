
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;

namespace Slickflow.Engine.Delegate
{
    /// <summary>
    /// Delegate Utility
    /// </summary>
    internal class DelegateUtil
    {
        /// <summary>
        /// Construct JSON string for the final object
        /// 构造最终对象的Json字符串
        /// </summary>
        /// <param name="arguments"></param>
        /// <param name="delegateService"></param>
        /// <returns></returns>
        internal static string CompositeJsonValue(string arguments, IDelegateService delegateService)
        {
            var jsonValue = string.Empty;
            var arguValue = string.Empty;
            var arguList = arguments.Split(',');

            var strBuilder = new StringBuilder(256);
            foreach (var name in arguList)
            {
                if (strBuilder.ToString() != string.Empty) strBuilder.Append(",");

                arguValue = delegateService.GetVariableThroughly(name);
                arguValue = FormatJsonStringIfSimple(arguValue);
                strBuilder.Append(string.Format("{0}:{1}", name, arguValue));
            }

            if (strBuilder.ToString() != string.Empty)
            {
                jsonValue = string.Format("{{{0}}}", strBuilder.ToString());
            }
            return jsonValue;
        }

        /// <summary>
        /// If it is a simple string, add double quotation marks
        /// 如果是简单字符串, 加双引号
        /// jack => "jack"
        /// </summary>
        /// <param name="jsonValue"></param>
        /// <returns></returns>
        internal static string FormatJsonStringIfSimple(string jsonValue)
        {
            jsonValue = jsonValue.TrimStart().TrimEnd();
            if ((jsonValue.StartsWith('{') && jsonValue.EndsWith('}'))
                || (jsonValue.StartsWith('"') && jsonValue.EndsWith('"')))
            {
                return jsonValue;
            }
            else
            {
                return string.Format("\"{0}\"", jsonValue);
            }
        }

        /// <summary>
        /// Construct JSON string for the final object
        /// 构造最终对象的Json字符串
        /// </summary>
        /// <param name="arguments"></param>
        /// <param name="delegateService"></param>
        /// <returns></returns>
        internal static DynamicParameters CompositeSqlParametersValue(string arguments, IDelegateService delegateService)
        {
            DynamicParameters parameters = new DynamicParameters();

            var arguValue = string.Empty;
            var arguList = arguments.Split(',');
            foreach (var name in arguList)
            {
                arguValue = delegateService.GetVariableThroughly(name);
                parameters.Add(string.Format("@{0}", name), arguValue);
            }
            return parameters;
        }

        /// <summary>
        /// Construct JSON string for the final object
        /// 构造最终对象的Json字符串
        /// </summary>
        /// <param name="arguments"></param>
        /// <param name="delegateService"></param>
        /// <returns></returns>
        internal static IDictionary<string, string> CompositeKeyValue(string arguments, IDelegateService delegateService)
        {
            var dictionary = new Dictionary<string, string>();
            var arguValue = string.Empty;
            var arguList = arguments.Split(',');
            foreach (var name in arguList)
            {
                arguValue = delegateService.GetVariableThroughly(name);
                dictionary.Add(name, arguValue);
            }
            return dictionary;
        }

        /// <summary>
        /// Construct a variable value list
        /// 构造可变数值列表
        /// </summary>
        /// <param name="arguments"></param>
        /// <param name="delegateService"></param>
        /// <returns></returns>
        internal static object[] CompositeParameterValues(string arguments, IDelegateService delegateService)
        {
            var arguList = arguments.Split(',');
            object[] valueArray = new object[arguList.Length];
            for (var i = 0; i < arguList.Length; i++)
            {
                valueArray[i] = delegateService.GetVariableThroughly(arguList[i]);
            }
            return valueArray;
        }
    }
}
