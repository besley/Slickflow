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

namespace Slickflow.Engine.Core.Pattern.Auto
{
    /// <summary>
    /// Script Executor
    /// 脚本执行器
    /// </summary>
    internal class ScriptExecutor
    {
        /// <summary>
        /// Execute script list
        /// </summary>
        internal static void ExecuteScriptList(IList<ScriptDetail> scriptList,
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
        /// Execute
        /// </summary>
        private static void Execute(ScriptDetail script, IDelegateService delegateService)
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
        /// Execute Python script
        /// 执行外部方法
        /// SetVariable:
        /// https://stackoverflow.com/questions/26426955/setting-and-getting-variables-in-net-hosted-ironpython-script/45734097
        /// </summary>
        private static void ExecutePythonMethod(ScriptDetail script, IDelegateService delegateService)
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
            catch (Exception ex)
            {
                throw new WorkflowException(LocalizeHelper.GetEngineMessage("actionexecutor.ExecutePythonMethod.exception", ex.Message));
            }
        }

        /// <summary>
        /// Excute sql method
        /// </summary>
        private static void ExecuteSQLMethod(ScriptDetail script, IDelegateService delegateService)
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
            catch (Exception ex)
            {
                throw new WorkflowException(LocalizeHelper.GetEngineMessage("actionexecutor.ExecuteSQLMethod.exception", ex.Message));
            }
        }
    }
}
