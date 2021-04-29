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
using System.Text;
using System.Data;
using Dapper;
using DapperExtensions;
using Slickflow.Data;
using Slickflow.Module.Localize;
using Slickflow.Engine.Common;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Utility;

namespace Slickflow.Engine.Business.Manager
{
    /// <summary>
    /// 节点转移管理类
    /// </summary>
    internal class TransitionInstanceManager : ManagerBase
    {
        #region 实例创建方法
        /// <summary>
        /// 创建转移实例数据
        /// </summary>
        /// <param name="processInstance">流程实例</param>
        /// <param name="transitionGUID">转移GUID</param>
        /// <param name="fromActivityInstance">来源活动实例</param>
        /// <param name="toActivityInstance">目的活动实例</param>
        /// <param name="transitionType">转移类型</param>
        /// <param name="flyingType">飞越类型</param>
        /// <param name="runner">执行者</param>
        /// <param name="conditionParseResult">条件解析结果</param>
        /// <returns>转移实例</returns>
        internal TransitionInstanceEntity CreateTransitionInstanceObject(ProcessInstanceEntity processInstance,
            String transitionGUID,
            ActivityInstanceEntity fromActivityInstance,
            ActivityInstanceEntity toActivityInstance,
            TransitionTypeEnum transitionType,
            TransitionFlyingTypeEnum flyingType,
            WfAppRunner runner,
            byte conditionParseResult)
        {
            var entity = new TransitionInstanceEntity();
            entity.AppName = processInstance.AppName;
            entity.AppInstanceID = processInstance.AppInstanceID;
            entity.ProcessGUID = processInstance.ProcessGUID;
            entity.ProcessInstanceID = processInstance.ID;
            entity.TransitionGUID = transitionGUID;
            entity.TransitionType = (byte)transitionType;
            entity.FlyingType = (byte)flyingType;

            //构造活动节点数据
            entity.FromActivityGUID = fromActivityInstance.ActivityGUID;
            entity.FromActivityInstanceID = fromActivityInstance.ID;
            entity.FromActivityType = fromActivityInstance.ActivityType;
            entity.FromActivityName = fromActivityInstance.ActivityName;
            entity.ToActivityGUID = toActivityInstance.ActivityGUID;
            entity.ToActivityInstanceID = toActivityInstance.ID;
            entity.ToActivityType = toActivityInstance.ActivityType;
            entity.ToActivityName = toActivityInstance.ActivityName;

            entity.ConditionParseResult = conditionParseResult;
            entity.CreatedByUserID = runner.UserID;
            entity.CreatedByUserName = runner.UserName;
            entity.CreatedDateTime = System.DateTime.Now;

            return entity;
        }
        #endregion

        #region 数据插入
        /// <summary>
        /// 插入方法
        /// </summary>
        /// <param name="conn">数据库连接</param>
        /// <param name="entity">实体</param>
        /// <param name="trans">事务</param>
        internal int Insert(IDbConnection conn,
            TransitionInstanceEntity entity,
            IDbTransaction trans)
        {
            int newID = Repository.Insert(conn, entity, trans);
            entity.ID = newID;

            return entity.ID;
        }

        /// <summary>
        /// 删除转移实例
        /// </summary>
        /// <param name="conn">数据库连接</param>
        /// <param name="transitionInstanceID">转移实例ID</param>
        /// <param name="trans">事务</param>
        internal void Delete(IDbConnection conn,
            int transitionInstanceID,
            IDbTransaction trans)
        {
            Repository.Delete<TransitionInstanceEntity>(conn, transitionInstanceID, trans);
        }
        #endregion

        #region 数据查询
        /// <summary>
        /// 根据ID获取实例数据
        /// </summary>
        /// <param name="transitionInstanceID">转移ID</param>
        /// <returns>转移实例</returns>
        internal TransitionInstanceEntity GetById(int transitionInstanceID)
        {
            return Repository.GetById<TransitionInstanceEntity>(transitionInstanceID);
        }

        /// <summary>
        /// 获取结束转移数据
        /// </summary>
        /// <param name="appName">应用名称</param>
        /// <param name="appInstanceID">应用实例ID</param>
        /// <param name="processGUID">流程GUID</param>
        /// <returns>转移实体</returns>
        internal TransitionInstanceEntity GetEndTransition(string appName, 
            string appInstanceID, 
            string processGUID)
        {
            var nodeList = GetTransitonInstance(appInstanceID, processGUID, ActivityTypeEnum.EndNode).ToList();

            if (nodeList == null || nodeList.Count == 0)
            {
                throw new WorkflowException(LocalizeHelper.GetEngineMessage("transitioninstancemanager.entrust.error"));
            }

            return nodeList[0];
        }

        /// <summary>
        /// 获取最后的转移实体数据
        /// </summary>
        /// <param name="appName">应用名称</param>
        /// <param name="appInstanceID">应用实例ID</param>
        /// <param name="processGUID">流程GUID</param>
        /// <returns>转移实体数据</returns>
        internal TransitionInstanceEntity GetLastTaskTransition(string appName, string appInstanceID, string processGUID)
        {
            var nodeList = GetWorkItemTransitonInstance(appInstanceID, processGUID).ToList();

            if (nodeList.Count == 0)
            {
                throw new WorkflowException(LocalizeHelper.GetEngineMessage("transitioninstancemanager.getlasttasktransition.error"));
            }

            return nodeList[0];
        }

        /// <summary>
        /// 获得去向节点是WorkItem类型的转移列表
        /// </summary>
        /// <param name="appInstanceID">应用ID</param>
        /// <param name="processGUID">流程GUID</param>
        /// <returns>转移实例列表</returns>
        internal IEnumerable<TransitionInstanceEntity> GetWorkItemTransitonInstance(string appInstanceID,
            String processGUID)
        {
            //2015.09.11 besley
            //需考虑后期节点类型增加目前支持TaskNode, SubProcessNode, MultipleInstanceNode
            //以上都是WorkItemType为1类型，保留ToActivityType是为了版本兼容，后期版本去掉ToActivity类型的判断。
            //var sql = @"SELECT 
            //                T.* 
            //            FROM WfTransitionInstance T
            //            INNER JOIN WfActivityInstance A
            //                ON T.ToActivityInstanceID = A.ID
            //            WHERE T.AppInstanceID=@appInstanceID 
            //                AND T.ProcessGUID=@processGUID 
            //                AND (T.ToActivityType=4 OR T.ToActivityType=5 OR T.ToActivityType=6 OR A.WorkItemType=1)          
            //            ORDER BY T.CreatedDateTime DESC";

            //var transitionList = Repository.Query<TransitionInstanceEntity>(sql,
            //    new
            //    {
            //        appInstanceID = appInstanceID,
            //        processGUID = processGUID
            //    });
            //return transitionList;
            var sqlQuery = (from t in Repository.GetAll<TransitionInstanceEntity>()
                            join a in Repository.GetAll<ActivityInstanceEntity>()
                                on t.ToActivityInstanceID equals a.ID 
                            where t.AppInstanceID == appInstanceID
                                && t.ProcessGUID == processGUID
                                && (t.ToActivityType == 4 || t.ToActivityType == 5 || t.ToActivityType == 6 || a.WorkItemType == 1)
                            select t
                            );
            var list = sqlQuery.OrderByDescending(t => t.CreatedDateTime).ToList<TransitionInstanceEntity>();
            return list;
        }

        /// <summary>
        /// 根据去向节点类型选择转移数据
        /// </summary>
        /// <param name="appInstanceID">应用实例ID</param>
        /// <param name="processGUID">流程GUID</param>
        /// <param name="toActivityType">到的活动节点类型</param>
        /// <returns>转移实体列表</returns>
        internal IEnumerable<TransitionInstanceEntity> GetTransitonInstance(string appInstanceID,
            String processGUID,
            ActivityTypeEnum toActivityType)
        {
            //var sql = @"SELECT * FROM WfTransitionInstance 
            //            WHERE AppInstanceID=@appInstanceID 
            //                AND ProcessGUID=@processGUID 
            //                AND ToActivityType=@toActivityType 
            //            ORDER BY CreatedDateTime DESC";

            //var transitionList = Repository.Query<TransitionInstanceEntity>(sql,
            //    new
            //    {
            //        appInstanceID = appInstanceID,
            //        processGUID = processGUID,
            //        toActivityType = (int)toActivityType
            //    });
            //return transitionList;

            var sqlQuery = (from t in Repository.GetAll<TransitionInstanceEntity>()
                            where t.AppInstanceID == appInstanceID
                                && t.ProcessGUID == processGUID
                                && t.ToActivityType == (short)toActivityType
                            select t
                            );
            var list = sqlQuery.OrderByDescending(t => t.CreatedDateTime).ToList<TransitionInstanceEntity>();
            return list;
        }

        /// <summary>
        /// 获取转移数据列表
        /// </summary>
        /// <param name="appInstanceID">应用实例ID</param>
        /// <param name="processGUID">流程GUID</param>
        /// <returns>转移实例列表</returns>
        internal IEnumerable<TransitionInstanceEntity> GetTransitionInstanceList(string appInstanceID,
            string processGUID)
        {
            IEnumerable<TransitionInstanceEntity> list = new List<TransitionInstanceEntity>();
            var pim = new ProcessInstanceManager();
            var pi = pim.GetProcessInstanceCurrent(appInstanceID, processGUID);
            if (pi != null)
            {
                list = GetTransitionInstanceList(appInstanceID, processGUID, pi.ID);
            }
            return list;
        }

        /// <summary>
        /// 获取转移数据列表
        /// </summary>
        /// <param name="appInstanceID">应用实例ID</param>
        /// <param name="processGUID">流程GUID</param>
        /// <param name="processInstanceID">流程实例ID</param>
        /// <returns>转移实例列表</returns>
        internal IEnumerable<TransitionInstanceEntity> GetTransitionInstanceList(string appInstanceID,
            string processGUID,
            int processInstanceID)
        {
            //var whereSql = @"SELECT * FROM WfTransitionInstance 
            //            WHERE AppInstanceID=@appInstanceID 
            //                AND ProcessGUID=@processGUID 
            //                AND ProcessInstanceID=@processInstanceID
            //            ORDER BY CreatedDateTime DESC";

            //var transitionList = Repository.Query<TransitionInstanceEntity>(whereSql,
            //    new
            //    {
            //        appInstanceID = appInstanceID,
            //        processGUID = processGUID.ToString(),
            //        processInstanceID = processInstanceID
            //    });
            //return transitionList;

            var sqlQuery = (from t in Repository.GetAll<TransitionInstanceEntity>()
                            where t.AppInstanceID == appInstanceID
                                && t.ProcessGUID == processGUID
                                && t.ProcessInstanceID == processInstanceID
                            select t
                            );
            var list = sqlQuery.OrderByDescending(t => t.CreatedDateTime).ToList<TransitionInstanceEntity>();
            return list;
        }

        /// <summary>
        /// 获取当前节点的下一步已经发出的活动实例列表
        /// </summary>
        /// <param name="fromActivityInstanceID">起始活动实例ID</param>
        /// <returns>下一步活动实例列表</returns>
        internal IList<ActivityInstanceEntity> GetTargetActivityInstanceList(int fromActivityInstanceID)
        {
            var session = SessionFactory.CreateSession();
            try
            {
                return GetTargetActivityInstanceList(fromActivityInstanceID, session);
            }
            catch(System.Exception ex)
            {
                throw;
            }
            finally
            {
                session.Dispose();
            }
        }

        /// <summary>
        /// 获取当前节点的下一步已经发出的活动实例列表
        /// </summary>
        /// <param name="fromActivityInstanceID">起始活动实例ID</param>
        /// <param name="session">数据会话</param>
        /// <returns>下一步活动实例列表</returns>
        internal IList<ActivityInstanceEntity> GetTargetActivityInstanceList(int fromActivityInstanceID, 
            IDbSession session)
        {
            IList<dynamic> targetIDs = GetTargetActivityInstanceIDs(fromActivityInstanceID, session);
            var nextActivityInstanceList = Repository.GetByIds<ActivityInstanceEntity>(session.Connection, targetIDs).ToList<ActivityInstanceEntity>();

            return nextActivityInstanceList;
        }

        /// <summary>
        /// 遍历下一步活动实例的ID
        /// </summary>
        /// <param name="fromActivityInstanceID">起始活动实例ID</param>
        /// <param name="session">数据会话</param>
        /// <returns>下一步活动实例ID列表</returns>
        private IList<dynamic> GetTargetActivityInstanceIDs(int fromActivityInstanceID,
            IDbSession session)
        {
            //var sql = @"SELECT * FROM WfTransitionInstance 
            //            WHERE FromActivityInstanceID=@fromActivityInstanceID";

            //var transitionList = Repository.Query<TransitionInstanceEntity>(session.Connection,
            //    sql,
            //    new
            //    {
            //        fromActivityInstanceID = fromActivityInstanceID,
            //    });
            var sqlQuery = (from t in Repository.GetAll<TransitionInstanceEntity>()
                            where t.FromActivityInstanceID == fromActivityInstanceID
                            select t
                            );
            var transitionList = sqlQuery.ToList<TransitionInstanceEntity>();

            List<dynamic> targetIDs = new List<dynamic>();
            foreach (var trans in transitionList)
            {
                if (trans.ToActivityType == (int)ActivityTypeEnum.GatewayNode)
                {
                    targetIDs.AddRange(GetTargetActivityInstanceIDs(trans.ToActivityInstanceID, session));       //遍历Gateway包含的子节点 
                }
                else
                {
                    targetIDs.Add(trans.ToActivityInstanceID);
                }
            }
            return targetIDs;
        }

        /// <summary>
        /// 判读定义的Transition是否已经被实例化执行
        /// </summary>
        /// <param name="transitionGUID">转移GUID</param>
        /// <param name="transitionInstanceList">转移实例列表</param>
        /// <returns>布尔值</returns>
        internal bool IsTransiionInstancedAndConditionParsedOK(String transitionGUID,
            IList<TransitionInstanceEntity> transitionInstanceList)
        {
            bool isConainedAndCompletedOK = false;
            foreach (TransitionInstanceEntity transitionInstance in transitionInstanceList)
            {
                //判断连线是否被实例化，并且条件是否满足
                if (transitionGUID == transitionInstance.TransitionGUID)
                {
                    if (transitionInstance.ConditionParseResult == (byte)ConditionParseResultEnum.Passed)
                    {
                        isConainedAndCompletedOK = true;
                        break;
                    }
                }
            }
            return isConainedAndCompletedOK;
        }
        #endregion
    }
}
