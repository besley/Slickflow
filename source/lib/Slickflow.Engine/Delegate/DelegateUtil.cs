
using System;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using IronPython.Hosting;
using Dapper;
using Slickflow.Data;
using Slickflow.Module.Localize;
using Slickflow.Engine.Common;
using Slickflow.Engine.Utility;
using Slickflow.Engine.Xpdl.Common;
using Slickflow.Engine.Xpdl.Entity;

namespace Slickflow.Engine.Delegate
{
    internal class DelegateUtil
    {
        /// <summary>
        /// 构造最终对象的Json字符串
        /// </summary>
        /// <param name="arguments">参数列表</param>
        /// <param name="delegateService">委托服务</param>
        /// <returns>Json字符串</returns>
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
        /// 如果是简单字符串, 加双引号
        /// jack => "jack"
        /// </summary>
        /// <param name="jsonValue">字符串</param>
        /// <returns>变换格式后的字符串</returns>
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
        /// 构造最终对象的Json字符串
        /// </summary>
        /// <param name="arguments">参数列表</param>
        /// <param name="delegateService">委托服务</param>
        /// <returns>动态参数列表</returns>
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
        /// 构造最终对象的Json字符串
        /// </summary>
        /// <param name="arguments">参数列表</param>
        /// <param name="delegateService">委托服务</param>
        /// <returns>字典列表</returns>
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
        /// 构造可变数值列表
        /// </summary>
        /// <param name="arguments">参数列表</param>
        /// <param name="delegateService">委托服务</param>
        /// <returns>参数列表</returns>
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
