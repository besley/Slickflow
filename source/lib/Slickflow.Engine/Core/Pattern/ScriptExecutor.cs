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
    internal class ScriptExecutor
    {
        /// <summary>
        /// Action 的执行方法
        /// </summary>
        /// <param name="scriptList">操作列表</param>
        /// <param name="delegateService">参数列表</param>
        internal static void ExecuteScriptList(IList<Xpdl.Entity.ScriptDetail> scriptList,
            IDelegateService delegateService)
        {
            if (scriptList != null && scriptList.Count > 0)
            {
                foreach (var script in scriptList)
                {
                    if (script.Method != ScriptMethodEnum.None)
                    {
                        Execute(script, delegateService);
                    }
                }
            }
        }

        /// <summary>
        /// 执行外部服务实现类
        /// </summary>
        /// <param name="script">操作</param>
        /// <param name="delegateService">委托服务类</param>
        private static void Execute(Xpdl.Entity.ScriptDetail script, IDelegateService delegateService)
        {
            if (script.Method == ScriptMethodEnum.SQL)
            {
                ExecuteSQLMethod(script, delegateService);
            }
            else if (script.Method == ScriptMethodEnum.Python)
            {
                ExecutePythonMethod(script, delegateService);
            }
            else
            {
                throw new WorkflowException(LocalizeHelper.GetEngineMessage("actionexecutor.Execute.exception", script.Method.ToString()));
            }
        }


        /// <summary>
        /// 执行外部方法
        /// SetVariable:
        /// https://stackoverflow.com/questions/26426955/setting-and-getting-variables-in-net-hosted-ironpython-script/45734097
        /// </summary>
        /// <param name="script">Action实体</param>
        /// <param name="delegateService">委托服务</param>
        private static void ExecutePythonMethod(Xpdl.Entity.ScriptDetail script, IDelegateService delegateService)
        {
            try
            {
                if (!string.IsNullOrEmpty(script.ScriptText))
                {
                    // var pythonScript = action.Expression;
                    var pythonScript = script.ScriptText;         //modified by Besley in 12/26/2019, body is nodetext rather than attribute
                    var engine = Python.CreateEngine();
                    var scope = engine.CreateScope();
                    var dictionary = DelegateUtil.CompositeKeyValue(script.Arguments, delegateService);
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
        /// 执行外部方法
        /// </summary>
        /// <param name="script">Action实体</param>
        /// <param name="delegateService">委托服务</param>
        private static void ExecuteSQLMethod(Xpdl.Entity.ScriptDetail script, IDelegateService delegateService)
        {
            try
            {
                var parameters = DelegateUtil.CompositeSqlParametersValue(script.Arguments, delegateService);
                if (!string.IsNullOrEmpty(script.ScriptText))
                {
                    //var sqlScript = action.Expression;
                    var sqlScript = script.ScriptText;        //modified by Besley in 12/26/2019, body is nodetext rather than attribute
                    var session = delegateService.GetSession();
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
    }
}
