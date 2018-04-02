using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Slickflow.Engine.Utility;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Common;

namespace Slickflow.Engine.Core.Pattern
{
    /// <summary>
    /// Action 执行器类
    /// </summary>
    internal class ActionExecutor
    {
        /// <summary>
        /// Action 的执行方法
        /// </summary>
        /// <param name="actionList">操作列表</param>
        /// <param name="actionMethodParameters">参数列表</param>
        internal void ExecteActionList(IList<ActionEntity> actionList, IDictionary<string, ActionParameterInternal> actionMethodParameters)
        {
            if (actionList != null && actionList.Count > 0)
            {
                foreach (var action in actionList)
                {
                    if (!string.IsNullOrEmpty(action.ActionName))
                    {
                        if (actionMethodParameters.ContainsKey(action.ActionName))
                            Execute(action, actionMethodParameters[action.ActionName]);
                        else
                            Execute(action, null);
                    }
                }
            }
        }

        /// <summary>
        /// 调用外部事件方法的实现过程
        /// <param name="action">操作</param>
        /// <param name="parameters">参数</param>
        /// </summary>
        internal void Execute(ActionEntity action, ActionParameterInternal parameters)
        {
            if (action.ActionType == ActionTypeEnum.ExternalMethod)
            {
                //取出当前应用程序执行路径
                var executingPath = ConfigHelper.GetExecutingDirectory();
                var pluginAssemblyName = string.Format("{0}\\{1}\\{2}.dll", executingPath, "plugin", action.AssemblyFullName);
                var pluginAssemblyTypes = Assembly.LoadFile(pluginAssemblyName).GetTypes();
                Type outerInfterface = pluginAssemblyTypes
                                            .Single(t => t.Name == action.InterfaceFullName && t.IsInterface);
                Type outerClass = pluginAssemblyTypes
                                        .Single(t => !t.IsInterface && outerInfterface.IsAssignableFrom(t));
                object instance = Activator.CreateInstance(outerClass, parameters.ConstructorParameters);
                MethodInfo mi = outerClass.GetMethod(action.MethodName);
                var result = mi.Invoke(instance, parameters.MethodParameters);
            }
            else if (action.ActionType == ActionTypeEnum.WebApi)
            {
                throw new ApplicationException("调用WebAPI功能暂未实现！");
            }
        }
    }
}
