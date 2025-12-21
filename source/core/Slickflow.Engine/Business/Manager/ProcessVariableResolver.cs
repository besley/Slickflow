using System.Data;
using System.Linq;
using System.Collections.Generic;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Xpdl.Entity;

namespace Slickflow.Engine.Business.Manager
{
    internal class ProcessVariableResolver
    {
        /// <summary>
        /// Verify if the required variable satisfies
        /// 检查必填变量是否存在
        /// </summary>
        internal bool CheckVariableRequired(IDbConnection conn, TaskViewEntity taskView, Activity activity, IDictionary<string, string> conditions, IDbTransaction trans)
        {
            var isCheckPassed = true;
            var variableList = activity.VariableList;
            if (variableList != null)
            {
                var outputVariableList = variableList.Where(v => v.DirectionType == VariableDirectionTypeEnum.Output).ToList();
                foreach (var ov in outputVariableList)
                {
                    if (ov.IsRequired == true)
                    {
                        var pvm = new ProcessVariableManager();
                        var ovDbEntity = pvm.GetVariableByActivity(conn, taskView.ProcessInstanceId, taskView.ActivityInstanceId, ov.Name, trans);
                        if (ovDbEntity == null)
                        {
                            isCheckPassed = false;
                            break;
                        }
                        else
                        {
                            if (!conditions.ContainsKey(ovDbEntity.Name))
                            {
                                conditions.Add(ovDbEntity.Name, ovDbEntity.Value);
                            }
                        }
                    }
                }
            }
            return isCheckPassed;
        }
    }
}
