/*
* Slickflow 工作流引擎遵循LGPL协议，也可联系作者商业授权并获取技术支持；
* 除此之外的使用则视为不正当使用，请您务必避免由此带来的商业版权纠纷。
* 
The Slickflow project.
Copyright (C) 2014  .NET Workflow Engine Library

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, you can access the official
web page about lgpl: https://www.gnu.org/licenses/lgpl.html
*/

using System.Collections.Generic;
using System.Linq;
using System.Dynamic;
using System.Text;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using Newtonsoft.Json;
using Dapper;
using DapperExtensions;
using Slickflow.Data;
using Slickflow.Engine.Common;
using Slickflow.Engine.Utility;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Delegate;
using Slickflow.Module.Resource;

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
        /// <param name="delegateService">参数列表</param>
        internal static void ExecteActionList(IList<ActionEntity> actionList, 
            IDelegateService delegateService)
        {
            if (actionList != null && actionList.Count > 0)
            {
                foreach (var action in actionList)
                {
                    if (action.FireType != FireTypeEnum.None
                        && !string.IsNullOrEmpty(action.Expression))
                    {
                        Execute(action, delegateService);
                    }
                }
            }
        }

        /// <summary>
        /// 触发前执行外部操作的方法
        /// </summary>
        /// <param name="actionList">操作列表</param>
        /// <param name="delegateService">委托服务</param>
        internal static void ExecteActionListBefore(IList<ActionEntity> actionList,
            IDelegateService delegateService)
        {
            if (actionList != null && actionList.Count > 0)
            {
                var list = actionList.Where(a => a.FireType == FireTypeEnum.Before).ToList();
                if (list != null && list.Count > 0)
                {
                    ActionExecutor.ExecteActionList(list, delegateService);
                }
            }
        }

        /// <summary>
        /// 触发后执行外部操作的方法
        /// </summary>
        /// <param name="actionList">操作列表</param>
        /// <param name="delegateService">委托服务</param>
        internal static void ExecteActionListAfter(IList<ActionEntity> actionList,
            IDelegateService delegateService)
        {
            if (actionList != null && actionList.Count > 0)
            {
                var list = actionList.Where(a => a.FireType == FireTypeEnum.After).ToList();
                if (list != null && list.Count > 0)
                {
                    ActionExecutor.ExecteActionList(list, delegateService);
                }
            }
        }

        /// <summary>
        /// 执行外部服务实现类
        /// </summary>
        /// <param name="action">操作</param>
        /// <param name="delegateService">委托服务类</param>
        private static void Execute(ActionEntity action, IDelegateService delegateService)
        {
            if (action.ActionType == ActionTypeEnum.Event)
            {
                if (action.ActionMethod == ActionMethodEnum.LocalMethod)
                {
                    ExecuteLocalMethod(action, delegateService);
                }
                else
                {
                    throw new WorkflowException(string.Format("社区版暂不支持的选项:{0}", action.ActionMethod.ToString()));
                }
            }
        }

        /// <summary>
        /// 执行外部方法
        /// </summary>
        /// <param name="action">Action实体</param>
        /// <param name="delegateService">委托服务</param>
        private static void ExecuteLocalMethod(ActionEntity action, IDelegateService delegateService)
        {
            try
            {
                //先获取具体实现类
                var instance = ReflectionHelper.GetSpecialInstance<IExternalService>(action.Expression);
                //再调用基类可执行方法
                var exterableInstance = instance as IExternable;
                exterableInstance.Executable(delegateService);
            }
            catch (System.Exception ex)
            {
                throw new WorkflowException(string.Format("执行LocalMethod出错:{0}", ex.Message));
            }
        }
    }
}
