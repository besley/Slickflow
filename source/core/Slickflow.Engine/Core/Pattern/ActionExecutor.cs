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
using Slickflow.Engine.Event;
using Slickflow.WebUtility;
using Slickflow.Engine.External;

namespace Slickflow.Engine.Core.Pattern
{
    /// <summary>
    /// Action Executor
    /// Action 执行器
    /// </summary>
    internal class ActionExecutor
    {
        /// <summary>
        /// Execute Action List
        /// Action 列表执行方法
        /// </summary>
        /// <param name="actionList"></param>
        /// <param name="eventService"></param>
        internal static void ExecuteActionList(IList<Xpdl.Entity.Action> actionList, 
            IEventService eventService)
        {
            if (actionList != null && actionList.Count > 0)
            {
                foreach (var action in actionList)
                {
                    if (action.FireType != FireTypeEnum.None
                        && (action.ActionMethod != ActionMethodEnum.None))
                    {
                        Execute(action, eventService);
                    }
                }
            }
        }

        /// <summary>
        /// Method for executing external operations before triggering
        /// 触发前执行外部操作的方法
        /// </summary>
        /// <param name="actionList"></param>
        /// <param name="eventService"></param>
        internal static void ExecuteActionListBefore(IList<Xpdl.Entity.Action> actionList,
            IEventService eventService)
        {
            if (actionList != null && actionList.Count > 0)
            {
                var list = actionList.Where(a => a.FireType == FireTypeEnum.Before).ToList();
                if (list != null && list.Count > 0)
                {
                    ExecuteActionList(list, eventService);
                }
            }
        }

        /// <summary>
        /// Method for executing external operations after triggering
        /// 触发后执行外部操作的方法
        /// </summary>
        /// <param name="actionList"></param>
        /// <param name="eventService"></param>
        internal static void ExecuteActionListAfter(IList<Xpdl.Entity.Action> actionList,
            IEventService eventService)
        {
            if (actionList != null && actionList.Count > 0)
            {
                var list = actionList.Where(a => a.FireType == FireTypeEnum.After).ToList();
                if (list != null && list.Count > 0)
                {
                    ExecuteActionList(list, eventService);
                }
            }
        }

        /// <summary>
        /// Execute external service implementation class
        /// 执行外部服务实现类
        /// </summary>
        /// <param name="action"></param>
        /// <param name="eventService"></param>
        private static void Execute(Xpdl.Entity.Action action, IEventService eventService)
        {
            if (action.ActionType == ActionTypeEnum.Event)
            {
                if (action.ActionMethod == ActionMethodEnum.LocalService)
                {
                    ExecuteLocalService(action, eventService);
                }
                else if (action.ActionMethod == ActionMethodEnum.CSharpLibrary)
                {
                    ExecuteCSharpLibraryMethod(action, eventService);
                }
                else if (action.ActionMethod == ActionMethodEnum.WebApi)
                {
                    ExecuteWebApiMethod(action, eventService);
                }
                else if (action.ActionMethod == ActionMethodEnum.SQL)
                {
                    ExecuteSQLMethod(action, eventService);
                }
                else if (action.ActionMethod == ActionMethodEnum.StoreProcedure)
                {
                    ExecuteStoreProcedureMethod(action, eventService);
                }
                else if (action.ActionMethod == ActionMethodEnum.Python)
                {
                    ExecutePythonMethod(action, eventService);
                }
                else
                {
                    throw new WorkflowException(LocalizeHelper.GetEngineMessage("actionexecutor.Execute.exception", action.ActionMethod.ToString()));
                }
            }
        }

        /// <summary>
        /// Execute local service
        /// 执行本地服务
        /// </summary>
        /// <param name="action"></param>
        /// <param name="eventService"></param>
        private static void ExecuteLocalService(Xpdl.Entity.Action action, IEventService eventService)
        {
            try
            {
                // 先获取具体实现类
                // First, obtain the specific implementation class
                var instance = ReflectionHelper.GetSpecialInstance<IExternalService>(action.Expression);
                // 再调用基类可执行方法
                // Call the base class executable method again
                var executableInstance = instance as IExecutable;
                executableInstance.Executable(eventService);
            }
            catch (System.Exception ex)
            {
                throw new WorkflowException(LocalizeHelper.GetEngineMessage("actionexecutor.ExecuteLocalService.exception", ex.Message));
            }
        }

        /// <summary>
        /// Execute WebApi Method
        /// 执行 WebAPI 方法
        /// </summary>
        /// <param name="action"></param>
        /// <param name="eventService"></param>
        private static void ExecuteWebApiMethod(Xpdl.Entity.Action action, IEventService eventService)
        {
            try
            {
                object result = null;
                if (action.SubMethod == SubMethodEnum.HttpGet)
                {
                    var jsonGetValue = eventService.GetVariableByScopePriority(action.Arguments);
                    var url = string.Format("{0}/{1}", action.Expression, jsonGetValue);
                    var httpGetClient = HttpClientHelper.CreateHelper(url);
                    result = httpGetClient.Get();
                }
                else if (action.SubMethod == SubMethodEnum.HttpPost)
                {
                    string url = action.Expression;
                    var httpClientHelper = HttpClientHelper.CreateHelper(url);
                    var jsonValue = CompositeJsonValue(action.Arguments, eventService);
                    result = httpClientHelper.Post(jsonValue);
                }
                else if (action.SubMethod == SubMethodEnum.HttpPut)
                {
                    string url = action.Expression;
                    var httpClientHelper = HttpClientHelper.CreateHelper(url);
                    var jsonValue = CompositeJsonValue(action.Arguments, eventService);
                    result = httpClientHelper.Put(jsonValue);
                }
                else if (action.SubMethod == SubMethodEnum.HttpDelete)
                {
                    var jsonGetValue = eventService.GetVariableByScopePriority(action.Arguments);
                    var url = string.Format("{0}/{1}", action.Expression, jsonGetValue);
                    var httpClientHelper = HttpClientHelper.CreateHelper(url);
                    result = httpClientHelper.Delete();
                }
            }
            catch (System.Exception ex)
            {
                throw new WorkflowException(LocalizeHelper.GetEngineMessage("actionexecutor.ExecuteWebApi.exception", ex.Message));
            }
        }

        /// <summary>
        /// Execute SQL Method
        /// 执行 SQL 方法
        /// </summary>
        /// <param name="action"></param>
        /// <param name="eventService"></param>
        private static void ExecuteSQLMethod(Xpdl.Entity.Action action, IEventService eventService)
        {
            try
            {
                var parameters = CompositeSqlParametersValue(action.Arguments, eventService);
                if (action.CodeInfo != null 
                    && !string.IsNullOrEmpty(action.CodeInfo.CodeText))
                {
                    var sqlScript = action.CodeInfo.CodeText;        //modified by Besley in 12/26/2019, body is nodetext rather than attribute
                    var session = eventService.GetSession();
                    var repository = new Repository();
                    repository.Execute(session.Connection, sqlScript, parameters, session.Transaction);
                }
                else
                {
                    throw new WorkflowException(LocalizeHelper.GetEngineMessage("actionexecutor.ExecuteSQLMethod.warn"));
                }
            }
            catch (System.Exception ex)
            {
                throw new WorkflowException(LocalizeHelper.GetEngineMessage("actionexecutor.ExecuteSQLMethod.exception", ex.Message));
            }
        }

        /// <summary>
        /// Execute sql store procedure
        /// 执行 SQL 存储过程
        /// </summary>
        /// <param name="action"></param>
        /// <param name="eventService"></param>
        private static void ExecuteStoreProcedureMethod(Xpdl.Entity.Action action, IEventService eventService)
        {
            try
            {
                var parameters = CompositeSqlParametersValue(action.Arguments, eventService);
                var procedureName = action.Expression;
                var session = eventService.GetSession();
                var repository = new Repository();
                repository.ExecuteProc(session.Connection, procedureName, parameters, session.Transaction);
            }
            catch (System.Exception ex)
            {
                throw new WorkflowException(LocalizeHelper.GetEngineMessage("actionexecutor.ExecuteStoreProcedureMethod.exception", ex.Message));
            }
        }

        /// <summary>
        /// Execute python script
        /// 执行 Python 脚本
        /// SetVariable:
        /// https://stackoverflow.com/questions/26426955/setting-and-getting-variables-in-net-hosted-ironpython-script/45734097
        /// </summary>
        /// <param name="action"></param>
        /// <param name="eventService"></param>
        private static void ExecutePythonMethod(Xpdl.Entity.Action action, IEventService eventService)
        {
            try
            {
                if (action.CodeInfo != null
                    && !string.IsNullOrEmpty(action.CodeInfo.CodeText))
                {
                    // var pythonScript = action.Expression;
                    var pythonScript = action.CodeInfo.CodeText;         //modified by Besley in 12/26/2019, body is nodetext rather than attribute
                    var engine = Python.CreateEngine();
                    var scope = engine.CreateScope();
                    var dictionary = CompositeKeyValue(action.Arguments, eventService);
                    foreach (var item in dictionary)
                    {
                        scope.SetVariable(item.Key, item.Value);
                    }
                    var source = engine.CreateScriptSourceFromString(pythonScript);
                    source.Execute(scope);
                }
                else
                {
                    throw new WorkflowException(LocalizeHelper.GetEngineMessage("actionexecutor.ExecutePythonMethod.warn"));
                }
            }
            catch (System.Exception ex)
            {
                throw new WorkflowException(LocalizeHelper.GetEngineMessage("actionexecutor.ExecutePythonMethod.exception", ex.Message));
            }
        }

        /// <summary>
        /// Execute c# library
        /// 执行 C# 库方法
        /// </summary>
        /// <param name="action"></param>
        /// <param name="eventService"></param>
        private static void ExecuteCSharpLibraryMethod(Xpdl.Entity.Action action, IEventService eventService)
        {
            try
            {
                // 获取当前应用程序执行路径
                // Retrieve the current application execution path
                var methodInfo = action.MethodInfo;
                var assemblyFullName = methodInfo.AssemblyFullName;
                var methodName = methodInfo.MethodName;
                var executingPath = ConfigHelper.GetExecutingDirectory();
                var pluginAssemblyName = string.Format("{0}\\{1}\\{2}.dll", executingPath, "plugin", assemblyFullName);
                var pluginAssemblyTypes = Assembly.LoadFile(pluginAssemblyName).GetTypes();
                Type outerClass = pluginAssemblyTypes
                                        .Single(t => !t.IsInterface && t.FullName == methodInfo.TypeFullName);
                //object instance = Activator.CreateInstance(outerClass, parameters.ConstructorParameters);
                object instance = Activator.CreateInstance(outerClass);
                System.Reflection.MethodInfo mi = outerClass.GetMethod(methodName);

                object[] methodParams = null;
                if (!string.IsNullOrEmpty(action.Arguments.Trim()))
                {
                    methodParams = CompositeParameterValues(action.Arguments, eventService);
                }
                var result = mi.Invoke(instance, methodParams);
            }
            catch(System.Exception ex)
            {
                throw new WorkflowException(LocalizeHelper.GetEngineMessage("actionexecutor.ExecuteCSharpLibraryMethod.exception", ex.Message));
            }
        }

        /// <summary>
        /// Construct JSON string for the final object
        /// 构造最终对象的 Json 字符串
        /// </summary>
        /// <param name="arguments"></param>
        /// <param name="eventService"></param>
        /// <returns></returns>
        private static string CompositeJsonValue(string arguments, IEventService eventService)
        {
            var jsonValue = string.Empty;
            var arguValue = string.Empty;
            var arguList = arguments.Split(',');

            var strBuilder = new StringBuilder(256);
            foreach (var name in arguList)
            {
                if (strBuilder.ToString() != string.Empty) strBuilder.Append(",");

                arguValue = eventService.GetVariableByScopePriority(name);
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
        /// 如果是简单字符串，加双引号
        /// jack => "jack"
        /// </summary>
        /// <param name="jsonValue"></param>
        /// <returns></returns>
        private static string FormatJsonStringIfSimple(string jsonValue)
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
        /// Construct SQL parameters value
        /// 构造 SQL 参数值
        /// </summary>
        /// <param name="arguments"></param>
        /// <param name="eventService"></param>
        /// <returns></returns>
        private static DynamicParameters CompositeSqlParametersValue(string arguments, IEventService eventService)
        {
            DynamicParameters parameters = new DynamicParameters();

            var arguValue = string.Empty;
            var arguList = arguments.Split(',');
            foreach (var name in arguList)
            {
                arguValue = eventService.GetVariableByScopePriority(name);
                parameters.Add(string.Format("@{0}",name), arguValue);
            }
            return parameters;
        }

        /// <summary>
        /// Construct key-value dictionary
        /// 构造键值对字典
        /// </summary>
        /// <param name="arguments"></param>
        /// <param name="eventService"></param>
        /// <returns></returns>
        private static IDictionary<string, string> CompositeKeyValue(string arguments, IEventService eventService)
        {
            var dictionary = new Dictionary<string, string>();
            var arguValue = string.Empty;
            var arguList = arguments.Split(',');
            foreach (var name in arguList)
            {
                arguValue = eventService.GetVariableByScopePriority(name);
                dictionary.Add(name, arguValue);
            }
            return dictionary;
        }

        /// <summary>
        /// Construct Parameters value
        /// 构造参数值列表
        /// </summary>
        /// <param name="arguments"></param>
        /// <param name="eventService"></param>
        /// <returns></returns>
        private static object[] CompositeParameterValues(string arguments, IEventService eventService)
        {
            var arguList = arguments.Split(',');
            object[] valueArray = new object[arguList.Length];
            for (var i = 0; i < arguList.Length; i++)
            {
                valueArray[i] = eventService.GetVariableByScopePriority(arguList[i]);
            }
            return valueArray;
        }
    }
}
