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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Data;
using Dapper;
using DapperExtensions;
using Slickflow.Data;
using Slickflow.Module.Localize;
using Slickflow.Engine.Common;
using Slickflow.Engine.Utility;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Business.Entity;

namespace Slickflow.Engine.Business.Manager
{
    /// <summary>
    /// 流程变量管理类
    /// </summary>
    internal class ProcessVariableManager : ManagerBase
    {
        /// <summary>
        /// 流程变量数据插入
        /// </summary>
        /// <param name="conn">链接</param>
        /// <param name="entity">流程变量实体</param>
        /// <param name="trans">事务</param>
        /// <returns>新实例ID</returns>
        private Int32 Insert(IDbConnection conn, ProcessVariableEntity entity, IDbTransaction trans)
        {
            int newID = Repository.Insert(conn, entity, trans);
            entity.ID = newID;

            return newID;
        }

        /// <summary>
        /// 流程实例更新
        /// </summary>
        /// <param name="conn">链接</param>
        /// <param name="entity">流程变量实体</param>
        /// <param name="trans">事务</param>
        /// <returns>新实例ID</returns>
        private void Update(IDbConnection conn, ProcessVariableEntity entity,
            IDbTransaction trans)
        {
            Repository.Update(conn, entity, trans);
        }

        /// <summary>
        /// 删除变量
        /// </summary>
        /// <param name="variableID">变量ID</param>
        internal void DeleteVariable(int variableID)
        {
            Repository.Delete<ProcessVariableEntity>(variableID);
        }

        /// <summary>
        /// 设置变量
        /// </summary>
        /// <param name="entity">流程变量实体</param>
        /// <returns>实体ID</returns>
        internal int SaveVariable(ProcessVariableEntity entity)
        {
            var entityID = 0;
            using (var session = SessionFactory.CreateSession())
            {
                entityID = SaveVariable(session.Connection, entity, session.Transaction);
            }
            return entityID;
        }

        /// <summary>
        /// 设置变量
        /// </summary>
        /// <param name="conn">链接</param>
        /// <param name="entity">流程变量实体</param>
        /// <param name="trans">事务</param>
        /// <returns>实体ID</returns>
        internal int SaveVariable(IDbConnection conn, ProcessVariableEntity entity, IDbTransaction trans)
        {
            if (string.IsNullOrEmpty(entity.AppInstanceID) || entity.ProcessInstanceID == 0)
            {
                throw new WorkflowException(LocalizeHelper.GetEngineMessage("processvariablemanager.savevariable.error"));
            }

            int entityID = 0;
            var query = new ProcessVariableQuery
            {
                VariableType = EnumHelper.ParseEnum<ProcessVariableTypeEnum>(entity.VariableType.ToString()),
                ProcessInstanceID = entity.ProcessInstanceID,
                ActivityGUID = entity.ActivityGUID,
                Name = entity.Name
            };

            var item = Query(conn, query, trans);
            if (item == null)
            {
                entityID = Insert(conn, entity, trans);
            }
            else
            {
                entityID = item.ID;
                Update(conn, entity, trans);
            }
            return entityID;
        }

        /// <summary>
        /// 获取流程变量列表
        /// </summary>
        /// <param name="processInstanceID">流程实例ID</param>
        /// <returns>流程变量列表</returns>
        internal IList<ProcessVariableEntity> GetVariableList(int processInstanceID)
        {
            //string sql = @"SELECT * FROM WfProcessVariable
            //            WHERE ProcessInstanceID=@processInstanceID
            //            ORDER BY ActivityGUID";
            //var list = Repository.Query<ProcessVariableEntity>(sql,
            //    new
            //    {
            //        processInstanceID = processInstanceID,
            //    }).ToList();

            var sqlQuery = (from pv in Repository.GetAll<ProcessVariableEntity>()
                            where pv.ProcessInstanceID == processInstanceID
                            select pv
                            );
            var list = sqlQuery.OrderBy(pv => pv.ActivityGUID).ToList<ProcessVariableEntity>();
            return list;
        }

        /// <summary>
        /// 获取流程变量实体
        /// </summary>
        /// <param name="query">变量查询</param>
        /// <returns>实体</returns>
        internal ProcessVariableEntity GetVariableEntity(ProcessVariableQuery query)
        {
            using (var session = SessionFactory.CreateSession())
            {
                var entity = Query(session.Connection, query, session.Transaction);
                return entity;
            }
        }

        /// <summary>
        /// 获取流程变量实体
        /// </summary>
        /// <param name="variableID">流程变量ID</param>
        /// <returns>实体</returns>
        internal ProcessVariableEntity GetVariableEntity(int variableID)
        {
            var entity = Repository.GetById<ProcessVariableEntity>(variableID);
            return entity;
        }

        /// <summary>
        /// 获取变量数值
        /// </summary>
        /// <param name="query">查询</param>
        /// <returns>变量的值</returns>
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
        /// 获取变量数值
        /// </summary>
        /// <param name="conn">链接串</param>
        /// <param name="query">查询</param>
        /// <param name="trans">事务</param>
        /// <returns>变量的值</returns>
        internal string GetVariableValue(IDbConnection conn, ProcessVariableQuery query, IDbTransaction trans)
        {
            var entity = Query(conn, query, trans);
            string value = entity == null ? null : entity.Value;

            return value;
        }

        /// <summary>
        /// 流程变量查询
        /// </summary>
        /// <param name="conn">链接</param>
        /// <param name="query">查询对象</param>
        /// <param name="trans">事务</param>
        /// <returns>流程变量实体</returns>
        private ProcessVariableEntity Query(IDbConnection conn, 
            ProcessVariableQuery query,
            IDbTransaction trans)
        {
            ProcessVariableEntity entity = null;
            string sql = string.Empty;
            if (query.VariableType == ProcessVariableTypeEnum.Process)
            {
                entity = GetVariable(conn, query.ProcessInstanceID, query.Name, trans);
            }
            else if (query.VariableType == ProcessVariableTypeEnum.Activity)
            {
                if (string.IsNullOrEmpty(query.ActivityGUID))
                {
                    throw new WorkflowException(LocalizeHelper.GetEngineMessage("processvariablemanager.query.error"));
                }
                entity = GetVariable(conn, query.ProcessInstanceID, query.ActivityGUID, query.Name, trans);
            }
            return entity;
        }

        /// <summary>
        /// 流程变量查询
        /// </summary>
        /// <param name="conn">链接</param>
        /// <param name="processInstanceID">流程实例ID</param>
        /// <param name="name">变量名称</param>
        /// <param name="trans">事务</param>
        /// <returns>流程变量实体</returns>
        private ProcessVariableEntity GetVariable(IDbConnection conn,
            int processInstanceID, 
            string name,
            IDbTransaction trans)
        {
            ProcessVariableEntity entity = null;
            //string sql = @"SELECT * FROM WfProcessVariable
            //               WHERE VariableType=@variableType
            //                    AND ProcessInstanceID=@newProcessInstanceID
            //                    AND Name=@newName
            //               ORDER BY ID DESC";

            //var list = Repository.Query<ProcessVariableEntity>(conn, sql,
            //    new
            //    {
            //        variableType = ProcessVariableTypeEnum.Process.ToString(),
            //        newProcessInstanceID = processInstanceID,
            //        newName = name
            //    },
            //    trans
            //    ).ToList();
            var sqlQuery = (from pv in Repository.GetAll<ProcessVariableEntity>(conn, trans)
                            where pv.VariableType == ProcessVariableTypeEnum.Process.ToString()
                                && pv.ProcessInstanceID == processInstanceID
                                && pv.Name == name
                            select pv
                            );
            var list = sqlQuery.OrderByDescending(pv => pv.ID).ToList<ProcessVariableEntity>();

            if (list.Count() > 0)
                entity = list[0];

            return entity;
        }

        /// <summary>
        /// 流程变量查询
        /// </summary>
        /// <param name="conn">链接</param>
        /// <param name="processInstanceID">流程实例ID</param>
        /// <param name="activityGUID">活动GUID</param>
        /// <param name="name">变量名称</param>
        /// <param name="trans">事务</param>
        /// <returns>流程变量实体</returns>
        private ProcessVariableEntity GetVariable(IDbConnection conn,
            int processInstanceID,
            string activityGUID,
            string name,
            IDbTransaction trans)
        {
            if (!string.IsNullOrEmpty(activityGUID))
            {
                return GetVariableOfActivity(conn, processInstanceID, activityGUID, name, trans);
            }
            else
            {
                return GetVariableOfProcess(conn, processInstanceID, name, trans);
            }
        }

        /// <summary>
        /// 流程变量查询
        /// </summary>
        /// <param name="conn">链接</param>
        /// <param name="processInstanceID">流程实例ID</param>
        /// <param name="name">变量名称</param>
        /// <param name="trans">事务</param>
        /// <returns>流程变量实体</returns>
        private ProcessVariableEntity GetVariableOfProcess(IDbConnection conn,
            int processInstanceID,
            string name,
            IDbTransaction trans)
        {
            ProcessVariableEntity entity = null;
            var sqlQuery = (from pv in Repository.GetAll<ProcessVariableEntity>()
                            where pv.VariableType == ProcessVariableTypeEnum.Process.ToString()
                                && pv.ProcessInstanceID == processInstanceID
                                && pv.Name == name
                            select pv
                            );
            var list = sqlQuery.OrderByDescending(pv => pv.ID).ToList<ProcessVariableEntity>();
            if (list != null && list.Count() > 0)
                entity = list[0];

            return entity;

        }

        /// <summary>
        /// 活动变量查询
        /// </summary>
        /// <param name="conn">链接</param>
        /// <param name="processInstanceID">流程实例ID</param>
        /// <param name="activityGUID">活动GUID</param>
        /// <param name="name">变量名称</param>
        /// <param name="trans">事务</param>
        /// <returns>流程变量实体</returns>
        private ProcessVariableEntity GetVariableOfActivity(IDbConnection conn,
            int processInstanceID,
            string activityGUID,
            string name,
            IDbTransaction trans)
        {
            ProcessVariableEntity entity = null;
            var sqlQuery = (from pv in Repository.GetAll<ProcessVariableEntity>()
                            where pv.VariableType == ProcessVariableTypeEnum.Activity.ToString()
                                && pv.ProcessInstanceID == processInstanceID
                                && pv.Name == name
                                && pv.ActivityGUID == activityGUID
                            select pv
                            );
            var list = sqlQuery.OrderByDescending(pv => pv.ID).ToList<ProcessVariableEntity>();
            if (list != null && list.Count() > 0)
                entity = list[0];

            return entity;
        }

        /// <summary>
        /// 验证触发表达式是否满足
        /// </summary>
        /// <param name="conn">链接</param>
        /// <param name="processInstanceID">流程实例ID</param>
        /// <param name="expression">表达式</param>
        /// <param name="trans">事务</param>
        /// <returns></returns>
        internal bool ValidateProcessVariable(IDbConnection conn, int processInstanceID, string expression, IDbTransaction trans)
        {
            var keyValuePair = new Dictionary<string, string>();
            var regex = new Regex("(?<=@)\\w+", RegexOptions.Compiled);
            var matches = regex.Matches(expression);
            foreach (var v in matches)
            {
                var value = GetVariableOfProcess(conn, processInstanceID, v.ToString(), trans);
                keyValuePair.Add(string.Format("{0}{1}", '@', v.ToString()), value.Value);
            }

            var replaced = ExpressionParser.ReplaceParameterToValue(expression, keyValuePair);
            var parsed = ExpressionParser.Parse(replaced);
            return parsed;
        }
    }
}
