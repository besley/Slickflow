/*
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
using Slickflow.Engine.Common;
using Slickflow.Data;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Xpdl.Node;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;

namespace Slickflow.Engine.Business.Manager
{
    /// <summary>
    /// 节点转移管理类
    /// </summary>
    internal class TransitionInstanceManager : ManagerBase
    {
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

        #region 数据增删改查
        internal TransitionInstanceEntity GetById(int transitionInstanceID)
        {
            return Repository.GetById<TransitionInstanceEntity>(transitionInstanceID);
        }

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

        internal TransitionInstanceEntity GetLastTaskTransition(string appName, string appInstanceID, string processGUID)
        {
            var nodeList = GetTransitonInstance(appInstanceID, processGUID, ActivityTypeEnum.TaskNode).ToList();

            if (nodeList.Count == 0)
            {
                throw new WorkflowException("没有符合条件的最后流转任务的实例数据，请查看流程其它信息！");
            }

            return nodeList[0];
        }

        /// <summary>
        /// 根据去向节点类型选择转移数据
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="appInstanceID"></param>
        /// <param name="processGUID"></param>
        /// <param name="toActivityType"></param>
        /// <returns></returns>
        internal IEnumerable<TransitionInstanceEntity> GetTransitonInstance(string appInstanceID, 
            String processGUID, 
            ActivityTypeEnum toActivityType)
        {
            var sql = @"SELECT * FROM WfTransitionInstance 
                        WHERE AppInstanceID=@appInstanceID 
                            AND ProcessGUID=@processGUID 
                            AND ToActivityType=@toActivityType 
                        ORDER BY CreatedDateTime DESC";

            var transitionList = Repository.Query<TransitionInstanceEntity>(sql,
                new
                {
                    appInstanceID = appInstanceID,
                    processGUID = processGUID,
                    toActivityType = toActivityType
                });

            return transitionList;
        }

        internal IEnumerable<TransitionInstanceEntity> GetTransitionInstanceList(string appInstanceID,
            string processGUID,
            int processInstanceID)
        {
            var whereSql = @"SELECT * FROM WfTransitionInstance 
                        WHERE AppInstanceID=@appInstanceID 
                            AND ProcessGUID=@processGUID 
                            AND ProcessInstanceID=@processInstanceID
                        ORDER BY CreatedDateTime DESC";

            var transitionList = Repository.Query<TransitionInstanceEntity>(whereSql,
                new
                {
                    appInstanceID = appInstanceID,
                    processGUID = processGUID.ToString(),
                    processInstanceID = processInstanceID
                });

            return transitionList;
        }

        /// <summary>
        /// 读取节点的上一步节点信息
        /// </summary>
        /// <param name="runningNode">当前节点</param>
        /// <param name="hasPassedGatewayNode">是否经由路由节点</param>
        /// <returns></returns>
        internal IList<ActivityInstanceEntity> GetPreviousActivityInstance(ActivityInstanceEntity runningNode,
            bool isSendback,
            out bool hasPassedGatewayNode)
        {
            hasPassedGatewayNode = false;
            var transitionList = GetTransitionInstanceList(runningNode.AppInstanceID, 
                runningNode.ProcessGUID, 
                runningNode.ProcessInstanceID).ToList();

            var backSrcActivityInstanceId = 0;
            if (isSendback == true && runningNode.BackSrcActivityInstanceID != null)
            {
                //节点时曾经发生退回的节点
                backSrcActivityInstanceId = runningNode.BackSrcActivityInstanceID.Value;
            }
            else
                backSrcActivityInstanceId = runningNode.ID;

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
                if (entity.FromActivityType == (short)ActivityTypeEnum.TaskNode 
                    || entity.FromActivityType == (short)ActivityTypeEnum.PluginNode
                    || entity.FromActivityType == (short)ActivityTypeEnum.ScriptNode
                    || entity.FromActivityType == (short)ActivityTypeEnum.StartNode)
                {
                    previousActivityInstanceList.Add(aim.GetById(entity.FromActivityInstanceID));
                }
                else if (entity.FromActivityType == (short)ActivityTypeEnum.GatewayNode)
                {
                    GetPreviousOfGatewayActivityInstance(transitionList, entity.FromActivityInstanceID, previousActivityInstanceList);
                }
            }
        }
            
        internal void Insert(IDbConnection conn,
            TransitionInstanceEntity entity,
            IDbTransaction trans)
        {
            int newID = Repository.Insert(conn, entity, trans);
            entity.ID = newID;

            Debug.WriteLine(string.Format("transition instance inserted, time:{0}", System.DateTime.Now.ToString()));
        }



        /// <summary>
        /// 删除转移实例
        /// </summary>
        /// <param name="transitionInstanceID"></param>
        /// <param name="wfLinqDataContext"></param>
        internal void Delete(IDbConnection conn, 
            int transitionInstanceID,
            IDbTransaction trans)
        {
            Repository.Delete<TransitionInstanceEntity>(conn, transitionInstanceID, trans);
        }
        #endregion

        /// <summary>
        /// 判读定义的Transition是否已经被实例化执行
        /// </summary>
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
    }
}
