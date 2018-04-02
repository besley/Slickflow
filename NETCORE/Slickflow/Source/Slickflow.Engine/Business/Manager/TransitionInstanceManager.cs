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
using System.Diagnostics;
using Slickflow.Data;
using Slickflow.Engine.Common;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Business.Data;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Utility;

namespace Slickflow.Engine.Business.Manager
{
    /// <summary>
    /// 节点转移管理类
    /// </summary>
    internal class TransitionInstanceManager
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
        /// <param name="entity">实体</param>
        /// <param name="session">会话</param>
        /// <returns>新实例ID</returns>
        internal int Insert(TransitionInstanceEntity entity,
            IDbSession session)
        {
            var repository = session.GetRepository<TransitionInstanceEntity>();
            var newEntity = repository.Insert(entity);
            session.SaveChanges();

            return newEntity.ID;
        }

        /// <summary>
        /// 删除转移实例
        /// </summary>
        /// <param name="transitionInstanceID">转移实例ID</param>
        /// <param name="session">数据上下文</param>
        internal void Delete(int transitionInstanceID,
            IDbSession session)
        {
            var repository = session.GetRepository<TransitionInstanceEntity>();
            repository.Delete(transitionInstanceID);
            session.SaveChanges();
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
            using (var session = DbFactory.CreateSession())
            {
                var entity = session.GetRepository<TransitionInstanceEntity>().GetByID(transitionInstanceID);
                return entity;
            }
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
                throw new WorkflowException("没有流程结束的流转记录！");
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
                throw new WorkflowException("没有符合条件的最后流转任务的实例数据，请查看流程其它信息！");
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
            using (var session = DbFactory.CreateSession())
            {
                var repositoryTI = session.GetRepository<TransitionInstanceEntity>();
                var repositoryAI = session.GetRepository<ActivityInstanceEntity>();
                var transitonList = (from t in repositoryTI.GetDbSet()
                                     join a in repositoryAI.GetDbSet()
                                        on t.ToActivityInstanceID equals a.ID
                                     where t.AppInstanceID == appInstanceID
                                        && t.ProcessGUID == processGUID
                                        && (t.ToActivityType == 4 || t.ToActivityType == 5 || t.ToActivityType == 6 || a.WorkItemType == 1)
                                     select t)
                                     .OrderByDescending(e => e.CreatedDateTime)
                                     .ToList();
                return transitonList;
            }
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
            using (var session = DbFactory.CreateSession())
            {
                var instanceList = session.GetRepository<TransitionInstanceEntity>().Query(e => e.AppInstanceID == appInstanceID
                    && e.ProcessGUID == processGUID
                    && e.ToActivityType == (short)toActivityType)
                    .OrderByDescending(e => e.CreatedDateTime)
                    .ToList();
                return instanceList;
            }
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
            //var sql = @"SELECT 
            //                    * 
            //                FROM WfTransitionInstance 
            //                WHERE AppInstanceID=@appInstanceID 
            //                    AND ProcessGUID=@processGUID 
            //                    AND ProcessInstanceID=@processInstanceID
            //                ORDER BY CreatedDateTime DESC";
            using (var session = DbFactory.CreateSession())
            {
                var instanceList = session.GetRepository<TransitionInstanceEntity>().Query(e => e.AppInstanceID == appInstanceID
                    && e.ProcessGUID == processGUID
                    && e.ProcessInstanceID == processInstanceID)
                    .OrderByDescending(e => e.CreatedDateTime)
                    .ToList();
                return instanceList;
            }
        }

        /// <summary>
        /// 读取节点的上一步节点信息
        /// </summary>
        /// <param name="runningNode">当前节点</param>
        /// <param name="isSendback">是否退回</param>
        /// <param name="hasPassedGatewayNode">是否经由路由节点</param>
        /// <returns>活动实例列表</returns>
        internal IList<ActivityInstanceEntity> GetPreviousActivityInstance(ActivityInstanceEntity runningNode,
            bool isSendback,
            out bool hasPassedGatewayNode)
        {
            hasPassedGatewayNode = false;
            var transitionList = GetTransitionInstanceList(runningNode.AppInstanceID, 
                runningNode.ProcessGUID, 
                runningNode.ProcessInstanceID).ToList();

            var backSrcActivityInstanceId = 0;
            if (isSendback == true)
            {
                //退回情况下的处理
                if (runningNode.MIHostActivityInstanceID != null && runningNode.CompleteOrder.Value == 1)
                {
                    //多实例的第一个子节点，先找到主节点，再到transition记录表中找到上一步节点
                    backSrcActivityInstanceId = runningNode.MIHostActivityInstanceID.Value;
                }
                else if (runningNode.BackSrcActivityInstanceID != null)
                {
                    //节点时曾经发生退回的节点
                    backSrcActivityInstanceId = runningNode.BackSrcActivityInstanceID.Value;
                }
                else
                {
                    backSrcActivityInstanceId = runningNode.ID;
                }
            }
            else
            {
                backSrcActivityInstanceId = runningNode.ID;
            }

            var aim = new ActivityInstanceManager();
            var runningTransitionList = transitionList
                .Where(o => o.ToActivityInstanceID == backSrcActivityInstanceId)
                .ToList();

            IList<ActivityInstanceEntity> previousActivityInstanceList = new List<ActivityInstanceEntity>();
            foreach (var entity in runningTransitionList)
            {
                //如果是逻辑节点，则继续查找
                if (entity.FromActivityType == (short)ActivityTypeEnum.GatewayNode)
                {
                    GetPreviousOfGatewayActivityInstance(transitionList, entity.FromActivityInstanceID, previousActivityInstanceList);
                    hasPassedGatewayNode = true;
                }
                else
                {
                    previousActivityInstanceList.Add(aim.GetById(entity.FromActivityInstanceID));
                }
            }
            return previousActivityInstanceList;
        }

        /// <summary>
        /// 获取网关节点前的节点
        /// </summary>
        /// <param name="transitionList">转移列表</param>
        /// <param name="toActivityInstanceID">流转到的活动实例ID</param>
        /// <param name="previousActivityInstanceList">前节点实例列表</param>
        private void GetPreviousOfGatewayActivityInstance(IList<TransitionInstanceEntity> transitionList,
            int toActivityInstanceID,
            IList<ActivityInstanceEntity> previousActivityInstanceList)
        {
            var previousTransitionList = transitionList
                .Where(o => o.ToActivityInstanceID == toActivityInstanceID)
                .ToList();

            var aim = new ActivityInstanceManager();
            foreach (var entity in previousTransitionList)
            {
                var activityType = EnumHelper.ParseEnum<ActivityTypeEnum>(entity.FromActivityType.ToString());
                if (XPDLHelper.IsSimpleComponentNode(activityType) == true)
                {
                    previousActivityInstanceList.Add(aim.GetById(entity.FromActivityInstanceID));
                }
                else if (entity.FromActivityType == (short)ActivityTypeEnum.GatewayNode)
                {
                    GetPreviousOfGatewayActivityInstance(transitionList, entity.FromActivityInstanceID, previousActivityInstanceList);
                }
            }
        }

        /// <summary>
        /// 获取当前节点的下一步已经发出的活动实例列表
        /// </summary>
        /// <param name="fromActivityInstanceID">起始活动实例ID</param>
        /// <returns>下一步活动实例列表</returns>
        internal IList<ActivityInstanceEntity> GetNextActivityInstanceList(int fromActivityInstanceID)
        {
            IList<int> nextIDs = GetNextActivityInstanceIDs(fromActivityInstanceID);
            using (var session = DbFactory.CreateSession())
            {
                var nextList = session.GetRepository<ActivityInstanceEntity>().Query(x => nextIDs.Contains(x.ID)).ToList();
                return nextList;
            }
        }

        /// <summary>
        /// 遍历下一步活动实例的ID
        /// </summary>
        /// <param name="fromActivityInstanceID">起始活动实例ID</param>
        /// <returns>下一步活动实例ID列表</returns>
        private IList<int> GetNextActivityInstanceIDs(int fromActivityInstanceID)
        {
            //var sql = @"SELECT 
            //                * 
            //            FROM WfTransitionInstance 
            //            WHERE FromActivityInstanceID=@fromActivityInstanceID";

            using (var session = DbFactory.CreateSession())
            {
                var transitionList = session.GetRepository<TransitionInstanceEntity>().Query(e => e.FromActivityInstanceID == fromActivityInstanceID);
                List<int> nextIDs = new List<int>();
                foreach (var trans in transitionList)
                {
                    if (trans.ToActivityType == (int)ActivityTypeEnum.GatewayNode)
                    {
                        nextIDs.AddRange(GetNextActivityInstanceIDs(trans.ToActivityInstanceID));       //遍历Gateway包含的子节点 
                    }
                    else
                    {
                        nextIDs.Add(trans.ToActivityInstanceID);
                    }
                }
                return nextIDs;
            }
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

        /// <summary>
        /// 获取转移OUT出去的实例数目
        /// </summary>
        /// <param name="appInstanceID">应用实例ID</param>
        /// <param name="processInstanceID">流程实例ID</param>
        /// <param name="fromActivityGUID">起始活动节点GUID</param>
        /// <returns></returns>
        internal int GetTransitionListCountOfOut(string appInstanceID, 
            int processInstanceID, 
            string fromActivityGUID)
        {
            using (var session = DbFactory.CreateSession())
            {
                //var sql = @"SELECT 
                //                * 
                //            FROM WfTransitionInstance 
                //            WHERE AppInstanceID=@appInstanceID
                //                AND ProcessInstanceID=@processInstanceID
                //                AND FromActivityGUID=@fromActivityGUID
                //                ORDER BY FromActivityInstanceID";
                int outCount = 0;
                var repository = session.GetRepository<TransitionInstanceEntity>();
                var transitionList = repository.Query(e => e.AppInstanceID == appInstanceID
                    && e.ProcessInstanceID == processInstanceID
                    && e.FromActivityGUID == fromActivityGUID)
                    .OrderByDescending(e => e.FromActivityInstanceID)
                    .ToList();
                if (transitionList.Count() > 0)
                {
                    var fromActivityInstanceID = transitionList[0].FromActivityInstanceID;
                    //var countSql = @"SELECT 
                    //                COUNT(1) 
                    //            FROM WfTransitionInstance 
                    //            WHERE AppInstanceID=@appInstanceID
                    //                AND ProcessInstanceID=@processInstanceID
                    //                AND FromActivityInstanceID=@fromActivityInstanceID";
                    outCount = repository.Count(e => e.AppInstanceID == appInstanceID
                        && e.ProcessInstanceID == processInstanceID
                        && e.FromActivityInstanceID == fromActivityInstanceID);
                }
                return outCount;
            }
        }
        #endregion
    }
}
