﻿
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
using Slickflow.Engine.Delegate;

namespace Slickflow.Engine.Core.Pattern
{
    /// <summary>
    /// Service Executor
    /// 自动服务方法类
    /// </summary>
    internal class ServiceExecutor
    {
        /// <summary>
        /// Execute service list
        /// </summary>
        internal static void ExecuteServiceList(IList<Xpdl.Entity.ServiceDetail> serviceList,
            IDelegateService delegateService)
        {
            if (serviceList != null && serviceList.Count > 0)
            {
                foreach (var service in serviceList)
                {
                    if (service.Method != ServiceMethodEnum.None)
                    {
                        Execute(service, delegateService);
                    }
                }
            }
        }

        /// <summary>
        /// Execute
        /// </summary>
        private static void Execute(Xpdl.Entity.ServiceDetail service, IDelegateService delegateService)
        {
            if (service.Method == ServiceMethodEnum.LocalService)
            {
                ExecuteLocalService(service, delegateService);
            }
            else if (service.Method == ServiceMethodEnum.CSharpLibrary)
            {
                ExecuteCSharpLibraryMethod(service, delegateService);
            }
            else if (service.Method == ServiceMethodEnum.WebApi)
            {
                ExecuteWebApiMethod(service, delegateService);
            }
            else if (service.Method == ServiceMethodEnum.StoreProcedure)
            {
                ExecuteStoreProcedureMethod(service, delegateService);
            }
            else
            {
                throw new WorkflowException(LocalizeHelper.GetEngineMessage("actionexecutor.Execute.exception", service.Method.ToString()));
            }
        }

        /// <summary>
        /// Execute Local Service
        /// </summary>
        private static void ExecuteLocalService(Xpdl.Entity.ServiceDetail service, IDelegateService delegateService)
        {
            try
            {
                //先获取具体实现类
                //First, obtain the specific implementation class
                var instance = ReflectionHelper.GetSpecialInstance<IExternalService>(service.Expression);
                //再调用基类可执行方法
                //Call the base class executable method again
                var exterableInstance = instance as IExternable;
                exterableInstance.Executable(delegateService);
            }
            catch (System.Exception ex)
            {
                throw new WorkflowException(LocalizeHelper.GetEngineMessage("actionexecutor.ExecuteLocalService.exception", ex.Message));
            }
        }

        /// <summary>
        /// Execute WebApi Method
        /// </summary>
        private static void ExecuteWebApiMethod(Xpdl.Entity.ServiceDetail service, IDelegateService delegateService)
        {
            try
            {
                object result = null;
                if (service.SubMethod == SubMethodEnum.HttpGet)
                {
                    var jsonGetValue = delegateService.GetVariableThroughly(service.Arguments);
                    var url = string.Format("{0}/{1}", service.Expression, jsonGetValue);
                    var httpGetClient = HttpClientHelper.CreateHelper(url);
                    result = httpGetClient.Get();
                }
                else if (service.SubMethod == SubMethodEnum.HttpPost)
                {
                    string url = service.Expression;
                    var httpClientHelper = HttpClientHelper.CreateHelper(url);
                    var jsonValue = DelegateUtil.CompositeJsonValue(service.Arguments, delegateService);
                    var httpPostClient = HttpClientHelper.CreateHelper(url);
                    result = httpClientHelper.Post(jsonValue);
                }
                else if (service.SubMethod == SubMethodEnum.HttpPut)
                {
                    string url = service.Expression;
                    var httpClientHelper = HttpClientHelper.CreateHelper(url);
                    var jsonValue = DelegateUtil.CompositeJsonValue(service.Arguments, delegateService);
                    var httpPostClient = HttpClientHelper.CreateHelper(url);
                    result = httpClientHelper.Put(jsonValue);
                }
                else if (service.SubMethod == SubMethodEnum.HttpDelete)
                {
                    var jsonGetValue = delegateService.GetVariableThroughly(service.Arguments);
                    var url = string.Format("{0}/{1}", service.Expression, jsonGetValue);
                    var httpGetClient = HttpClientHelper.CreateHelper(url);
                    result = httpGetClient.Delete();
                }
            }
            catch (System.Exception ex)
            {
                throw new WorkflowException(LocalizeHelper.GetEngineMessage("actionexecutor.ExecuteWebApi.exception", ex.Message));
            }
        }

        /// <summary>
        /// Execute Store procedure
        /// </summary>
        private static void ExecuteStoreProcedureMethod(Xpdl.Entity.ServiceDetail service, IDelegateService delegateService)
        {
            try
            {
                var parameters = DelegateUtil.CompositeSqlParametersValue(service.Arguments, delegateService);
                var procedureName = service.Expression;
                var session = delegateService.GetSession();
                var repository = new Repository();
                repository.ExecuteProc(session.Connection, procedureName, parameters, session.Transaction);
            }
            catch (System.Exception ex)
            {
                throw new WorkflowException(LocalizeHelper.GetEngineMessage("actionexecutor.ExecuteStoreProcedureMethod.exception", ex.Message));
            }
        }

        /// <summary>
        /// Execute library
        /// </summary>
        private static void ExecuteCSharpLibraryMethod(Xpdl.Entity.ServiceDetail service, IDelegateService delegateService)
        {
            try
            {
                //取出当前应用程序执行路径
                //Retrieve the current application execution path
                var methodInfo = service.MethodInfo;
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
                if (!string.IsNullOrEmpty(service.Arguments.Trim()))
                {
                    methodParams = DelegateUtil.CompositeParameterValues(service.Arguments, delegateService);
                }
                var result = mi.Invoke(instance, methodParams);
            }
            catch (System.Exception ex)
            {
                throw new WorkflowException(LocalizeHelper.GetEngineMessage("actionexecutor.ExecuteCSharpLibraryMethod.exception", ex.Message));
            }
        }
    }
}
