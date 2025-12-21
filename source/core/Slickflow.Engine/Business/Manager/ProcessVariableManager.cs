
using MongoDB.Driver;
using Slickflow.Data;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Common;
using Slickflow.Engine.Delegate;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Module.Localize;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace Slickflow.Engine.Business.Manager
{
    /// <summary>
    /// Process Variable Manager
    /// 流程变量管理类
    /// </summary>
    internal class ProcessVariableManager : ManagerBase
    {
        /// <summary>
        /// Retrieve process variable entity
        /// 获取流程变量实体
        /// </summary>
        internal ProcessVariableEntity GetVariableById(int variableId)
        {
            var entity = Repository.GetById<ProcessVariableEntity>(variableId);
            return entity;
        }

        internal ProcessVariableEntity GetVariableById(IDbConnection conn, int variableId, IDbTransaction trans)
        {
            var entity = Repository.GetById<ProcessVariableEntity>(conn, variableId, trans);
            return entity;
        }

        internal string GetVariable(IDbConnection conn, ProcessVariableScopeEnum variableType, string name,
            DelegateContext delegateContext, IDbTransaction trans)
        {
            var variableValue = string.Empty;
            if (variableType == ProcessVariableScopeEnum.Activity)
            {
                var activityVariableEntity = GetVariableByActivity(conn, delegateContext.ProcessInstanceId, delegateContext.ActivityInstanceId, name, trans);
                if (activityVariableEntity != null) variableValue = activityVariableEntity.Value;
            }
            else
            {
                var processVariableEntity = GetVariableByProcess(conn, delegateContext.ProcessInstanceId, name, trans);
                if (processVariableEntity != null)  variableValue = processVariableEntity.Value;
            }
            return variableValue;
        }

        internal string GetVariableByScopePriority(IDbConnection conn, string name,
                DelegateContext delegateContext, IDbTransaction trans)
        {
            var variableValue = string.Empty;
            var activityVariableEntity = GetVariableByActivity(conn, delegateContext.ProcessInstanceId, delegateContext.ActivityInstanceId, name, trans);
            if (activityVariableEntity != null)
            {
                variableValue = activityVariableEntity.Value;
                return variableValue;
            }
            else
            {
                var processVariableEntity = GetVariableByProcess(conn, delegateContext.ProcessInstanceId, name, trans);
                if (processVariableEntity != null) variableValue = processVariableEntity.Value;
                return variableValue;
            }
        }

        internal IDictionary<string, string> GetVariableByTask(IDbConnection conn, TaskViewEntity taskView, IDictionary<string, string> condition, 
            IDbTransaction trans)
        {
            var variableList = GetVariableList(conn, taskView.ProcessInstanceId, taskView.ActivityInstanceId, trans);
            if (variableList != null && variableList.Count > 0)
            {
                foreach (var variable in variableList )
                {
                    if (condition.ContainsKey(variable.Name)) 
                    {
                        ;//condition[variable.Name] = variable.Value;
                    }
                    else
                    {
                        condition.Add(variable.Name, variable.Value);
                    }
                }
            }
            return condition;
        }

        /// <summary>
        /// Retrieve the list of process variables
        /// 获取流程变量列表
        /// </summary>
        internal IList<ProcessVariableEntity> GetVariableList(int processInstanceId)
        {
            //var list = Repository.GetAll<ProcessVariableEntity>()
            //            .Where<ProcessVariableEntity>(v => v.ProcessInstanceId == processInstanceId)
            //            .OrderBy(v => v.ActivityId)
            //            .ToList();

            var sql = @"SELECT 
                            * 
                        FROM wf_process_variable
                        WHERE process_instance_id=@processInstanceId
                        ORDER BY activity_id";
            var list = Repository.Query<ProcessVariableEntity>(sql,
                        new
                        {
                            processInstanceId = processInstanceId,
                        }).ToList();
            return list;
        }

        internal IList<ProcessVariableEntity> GetVariableList(IDbConnection conn, int processInstanceId, int activityInstanceId, IDbTransaction trans)
        {
            //var list = Repository.GetAll<ProcessVariableEntity>()
            //            .Where<ProcessVariableEntity>(v => v.ProcessInstanceId == processInstanceId && v.ActivityInstanceId == activityInstanceId)
            //            .OrderBy(v => v.ActivityInstanceId)
            //            .ToList();

            var sql = @"SELECT 
                            * 
                        FROM wf_process_variable
                        WHERE process_instance_id=@processInstanceId
                            AND activity_instance_id=@activityInstanceId
                        ORDER BY activity_id";
            var list = Repository.Query<ProcessVariableEntity>(sql,
                        new
                        {
                            processInstanceId = processInstanceId,
                            activityInstanceId = activityInstanceId
                        }).ToList();
            return list;
        }

        /// <summary>
        /// Retrieve process variable entity
        /// 获取流程变量实体
        /// </summary>
        internal ProcessVariableEntity GetVariableEntity(ProcessVariableQuery query)
        {
            using (var session = SessionFactory.CreateSession())
            {
                var entity = Query(session.Connection, query, session.Transaction);
                return entity;
            }
        }

        /// <summary>
        /// Retrieve process variable value
        /// 获取变量数值
        /// </summary>
        internal string GetVariableValue(ProcessVariableQuery query)
        {
            using (var session = SessionFactory.CreateSession())
            {
                var entity = Query(session.Connection, query, session.Transaction);
                string value = entity == null ? null : entity.Value;

                return value;
            }
        }

        /// <summary>
        /// Retrieve process variable value
        /// 获取变量数值
        /// </summary>
        internal string GetVariableValue(IDbConnection conn, ProcessVariableQuery query, IDbTransaction trans)
        {
            var entity = Query(conn, query, trans);
            string value = entity == null ? null : entity.Value;

            return value;
        }

        /// <summary>
        /// Query process variable
        /// 流程变量查询
        /// </summary>
        private ProcessVariableEntity Query(IDbConnection conn, 
            ProcessVariableQuery query,
            IDbTransaction trans)
        {
            ProcessVariableEntity entity = null;
            string sql = string.Empty;
            if (query.VariableSceope == ProcessVariableScopeEnum.Process)
            {
                entity = GetVariableByProcess(conn, query.ProcessInstanceId, query.Name, trans);
            }
            else if (query.VariableSceope == ProcessVariableScopeEnum.Activity)
            {
                entity = GetVariableByActivity(conn, query.ProcessInstanceId, query.ActivityInstanceId, query.Name, trans);
            }
            return entity;
        }

        /// <summary>
        /// Query process variable
        /// 流程变量查询
        /// </summary>
        internal ProcessVariableEntity GetVariableByProcess(IDbConnection conn,
            int processInstanceId,
            string name,
            IDbTransaction trans)
        {
            ProcessVariableEntity entity = null;
            //var list = Repository.GetAll<ProcessVariableEntity>(conn, trans)
            //    .Where<ProcessVariableEntity>(
            //        v => v.VariableType == ProcessVariableTypeEnum.Process.ToString() &&
            //            v.Name == name &&
            //            v.ProcessInstanceId == processInstanceId
            //    )
            //    .OrderByDescending(v => v.Id)
            //    .ToList();

            var sql = @"SELECT 
                            * 
                        FROM wf_process_variable 
                        WHERE variable_scope=@variableScope 
                            AND process_instance_id=@processInstanceId 
                            AND name=@name 
                        ORDER BY activity_id";
            var list = Repository.Query<ProcessVariableEntity>(sql,
                new
                {
                    variableScope = ProcessVariableScopeEnum.Process.ToString(),
                    processInstanceId = processInstanceId,
                    name = name
                }).ToList();

            if (list != null && list.Count() > 0)
                entity = list[0];

            return entity;

        }

        /// <summary>
        /// Query activity variable
        /// 活动变量查询
        /// </summary>
        internal ProcessVariableEntity GetVariableByActivity(IDbConnection conn,
            int processInstanceId,
            int activityInstanceId,
            string name,
            IDbTransaction trans)
        {
            ProcessVariableEntity entity = null;
            //var list = Repository.GetAll<ProcessVariableEntity>(conn, trans)
            //    .Where<ProcessVariableEntity>(
            //        v => v.VariableType == ProcessVariableTypeEnum.Activity.ToString() &&
            //            v.ProcessInstanceId == processInstanceId &&
            //            v.ActivityInstanceId == activityInstanceId &&
            //            v.Name == name
            //    )
            //    .OrderByDescending(v => v.Id)
            //    .ToList();

            var sql = @"SELECT 
                            * 
                        FROM wf_process_variable
                        WHERE variable_scope=@variableScope
                            AND process_instance_id=@processInstanceId
                            AND activity_instance_id=@activityInstanceId 
                            AND name=@name
                        ORDER BY name";

            var list = Repository.Query<ProcessVariableEntity>(sql,
                new
                {
                    variableScope = ProcessVariableScopeEnum.Activity.ToString(),
                    processInstanceId = processInstanceId,
                    activityInstanceId = activityInstanceId,
                    name = name
                }).ToList();

            if (list != null && list.Count() > 0)
                entity = list[0];

            return entity;
        }

        /// <summary>
        /// Query activity variable
        /// 活动变量查询
        /// </summary>
        internal IList<ProcessVariableEntity> GetVariableListByActivity(IDbConnection conn,
            int processInstanceId,
            int activityInstanceId,
            IList<string> variableNameList,
            IDbTransaction trans)
        {
            //var list = Repository.GetAll<ProcessVariableEntity>(conn, trans)
            //    .Where<ProcessVariableEntity>(
            //        v => v.VariableType == ProcessVariableTypeEnum.Activity.ToString() &&
            //            v.ProcessInstanceId == processInstanceId &&
            //            v.ActivityInstanceId == activityInstanceId &&
            //    )
            //    .OrderByDescending(v => v.Id)
            //    .ToList();

            List<ProcessVariableEntity> list = null;
            var sql = string.Empty;
            if (variableNameList.Count == 1)
            {
                sql = @"SELECT 
                          * 
                      FROM wf_process_variable
                      WHERE variable_scope=@variableScope
                          AND process_instance_id=@processInstanceId
                          AND activity_instance_id=@activityInstanceId
                          AND name=@name
                      ORDER BY name";
                list = Repository.Query<ProcessVariableEntity>(conn,
                    sql,
                    new
                    {
                        variableScope = ProcessVariableScopeEnum.Activity.ToString(),
                        processInstanceId = processInstanceId,
                        activityInstanceId = activityInstanceId,
                        name = variableNameList[0]  // 转换为数组
                    }, trans).ToList();
            }
            else
            {
                sql = @"SELECT 
                          * 
                      FROM wf_process_variable
                      WHERE variable_scope=@variableScope
                          AND process_instance_id=@processInstanceId
                          AND activity_instance_id=@activityInstanceId
                          AND name IN @variableNames
                      ORDER BY name";

                list = Repository.Query<ProcessVariableEntity>(conn,
                    sql,
                    new
                    {
                        variableScope = ProcessVariableScopeEnum.Activity.ToString(),
                        processInstanceId = processInstanceId,
                        activityInstanceId = activityInstanceId,
                        variableNames = variableNameList.ToArray()  // 转换为数组
                    }, trans).ToList();
            }
                

            return list;
        }

        /// <summary>
        /// Save process variable
        /// 保存变量
        /// </summary>
        internal int SaveVariable(ProcessVariableEntity entity)
        {
            var entityId = 0;
            using (var session = SessionFactory.CreateSession())
            {
                entityId = SaveVariable(session.Connection, entity, session.Transaction);
            }
            return entityId;
        }

        /// <summary>
        /// Save process variable
        /// 保存变量
        /// </summary>
        public int SaveVariable(IDbConnection conn, ProcessVariableEntity entity, IDbTransaction trans)
        {
            var variableId = 0;
            if (string.IsNullOrEmpty(entity.Name) || string.IsNullOrEmpty(entity.Value))
            {
                throw new WorkflowException(LocalizeHelper.GetEngineMessage("processvariablemanager.savevariable.error"));
            }

            ProcessVariableEntity existProcessVariable = null;
            var pvm = new ProcessVariableManager();
            if (entity.Id != 0)
            {
                existProcessVariable = GetVariableById(conn, entity.Id, trans);
            }
            else
            {
                existProcessVariable = GetVariableByActivity(conn, entity.ProcessInstanceId, entity.ActivityInstanceId, entity.Name, trans);
            }
                
            if (existProcessVariable != null)
            {
                variableId = existProcessVariable.Id;
                existProcessVariable.Value = entity.Value;
                existProcessVariable.UpdatedDateTime = DateTime.UtcNow;
                Update(conn, existProcessVariable, trans);
            }
            else
            {
                entity.UpdatedDateTime = DateTime.UtcNow;
                variableId = Insert(conn, entity, trans);
            }
            return variableId;
        }

        /// <summary>
        /// Verify if the trigger expression satisfies
        /// 验证触发表达式是否满足
        /// </summary>
        internal bool ValidateProcessVariable(IDbConnection conn, int processInstanceId, string expression, IDbTransaction trans)
        {
            var parsed = false;
            var keyValuePair = new Dictionary<string, string>();
            var regex = new Regex("(?<=@)\\w+", RegexOptions.Compiled);
            var matches = regex.Matches(expression);
            foreach (var v in matches)
            {
                var value = GetVariableByProcess(conn, processInstanceId, v.ToString(), trans);
                if (value != null)
                {
                    keyValuePair.Add(string.Format("{0}{1}", '@', v.ToString()), value.Value);
                }
            }

            if (keyValuePair.Count > 0)
            {
                var replaced = ExpressionParser.ReplaceParameterToValue(expression, keyValuePair);
                parsed = ExpressionParser.Parse(replaced);
            }
            return parsed;
        }

        /// <summary>
        /// Insert process variable
        /// 流程变量数据插入
        /// </summary>
        private Int32 Insert(IDbConnection conn, ProcessVariableEntity entity, IDbTransaction trans)
        {
            int newId = Repository.Insert(conn, entity, trans);
            entity.Id = newId;

            return newId;
        }

        /// <summary>
        /// Update process variable
        /// 流程变量更新
        /// </summary>
        private void Update(IDbConnection conn, ProcessVariableEntity entity,
            IDbTransaction trans)
        {
            Repository.Update(conn, entity, trans);
        }

        /// <summary>
        /// Delete process variable
        /// 删除变量
        /// </summary>
        internal void DeleteVariable(int variableId)
        {
            Repository.Delete<ProcessVariableEntity>(variableId);
        }
    }
}
