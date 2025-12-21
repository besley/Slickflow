
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Slickflow.Data;
using Slickflow.Module.Localize;
using Slickflow.Engine.Common;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Utility;

namespace Slickflow.Engine.Business.Manager
{
    /// <summary>
    /// Transition manager
    /// 节点转移管理类
    /// </summary>
    internal class TransitionInstanceManager : ManagerBase
    {
        #region Create method 实例创建方法
        /// <summary>
        /// Create transfer instance data
        /// 创建转移实例数据
        /// </summary>
        internal TransitionInstanceEntity CreateTransitionInstanceObject(ProcessInstanceEntity processInstance,
            String transitionId,
            ActivityInstanceEntity fromActivityInstance,
            ActivityInstanceEntity toActivityInstance,
            TransitionTypeEnum transitionType,
            TransitionFlyingTypeEnum flyingType,
            WfAppRunner runner,
            byte conditionParseResult)
        {
            var entity = new TransitionInstanceEntity();
            entity.AppName = processInstance.AppName;
            entity.AppInstanceId = processInstance.AppInstanceId;
            entity.ProcessId = processInstance.ProcessId;
            entity.ProcessInstanceId = processInstance.Id;
            entity.TransitionId = transitionId;
            entity.TransitionType = (byte)transitionType;
            entity.FlyingType = (byte)flyingType;

            //构造活动节点数据
            entity.FromActivityId = fromActivityInstance.ActivityId;
            entity.FromActivityInstanceId = fromActivityInstance.Id;
            entity.FromActivityType = fromActivityInstance.ActivityType;
            entity.FromActivityName = fromActivityInstance.ActivityName;
            entity.ToActivityId = toActivityInstance.ActivityId;
            entity.ToActivityInstanceId = toActivityInstance.Id;
            entity.ToActivityType = toActivityInstance.ActivityType;
            entity.ToActivityName = toActivityInstance.ActivityName;

            entity.ConditionParsedResult = conditionParseResult;
            entity.CreatedUserId = runner.UserId;
            entity.CreatedUserName = runner.UserName;
            entity.CreatedDateTime = System.DateTime.UtcNow;

            return entity;
        }
        #endregion

        #region Insert 数据插入
        /// <summary>
        /// Insert transition
        /// 插入方法
        /// </summary>
        internal int Insert(IDbConnection conn,
            TransitionInstanceEntity entity,
            IDbTransaction trans)
        {
            int newId = Repository.Insert(conn, entity, trans);
            entity.Id = newId;

            return entity.Id;
        }

        /// <summary>
        /// Delete transition
        /// 删除转移实例
        /// </summary>
        internal void Delete(IDbConnection conn,
            int transitionInstanceId,
            IDbTransaction trans)
        {
            Repository.Delete<TransitionInstanceEntity>(conn, transitionInstanceId, trans);
        }
        #endregion

        #region Query task 数据查询
        /// <summary>
        /// Retrieve instance data based on Id
        /// 根据ID获取实例数据
        /// </summary>
        internal TransitionInstanceEntity GetById(int transitionInstanceId)
        {
            return Repository.GetById<TransitionInstanceEntity>(transitionInstanceId);
        }

        /// <summary>
        /// Get End transition Data
        /// 获取结束转移数据
        /// </summary>
        internal TransitionInstanceEntity GetEndTransition(string appName, 
            string appInstanceId, 
            string processId)
        {
            var nodeList = GetTransitonInstance(appInstanceId, processId, ActivityTypeEnum.EndNode).ToList();

            if (nodeList == null || nodeList.Count == 0)
            {
                throw new WorkflowException(LocalizeHelper.GetEngineMessage("transitioninstancemanager.entrust.error"));
            }

            return nodeList[0];
        }

        /// <summary>
        /// Obtain the final transition entity data
        /// 获取最后的转移实体数据
        /// </summary>
        internal TransitionInstanceEntity GetLastTaskTransition(string appName, 
            string appInstanceId, 
            string processId)
        {
            var nodeList = GetWorkItemTransitonInstance(appInstanceId, processId).ToList();

            if (nodeList.Count == 0)
            {
                throw new WorkflowException(LocalizeHelper.GetEngineMessage("transitioninstancemanager.getlasttasktransition.error"));
            }

            return nodeList[0];
        }

        /// <summary>
        /// Obtain a transition list of WorkItem type destination nodes
        /// 获得去向节点是WorkItem类型的转移列表
        /// </summary>
        internal IEnumerable<TransitionInstanceEntity> GetWorkItemTransitonInstance(string appInstanceId,
            String processId)
        {
            //2015.09.11 besley
            //需考虑后期节点类型增加目前支持TaskNode, SubProcessNode, MultipleInstanceNode
            //以上都是WorkItemType为1类型，保留ToActivityType是为了版本兼容，后期版本去掉ToActivity类型的判断。
            //It is necessary to consider adding node types in the later stage. Currently, TaskNode is supported, SubProcessNode, MultipleInstanceNode
            //The above are all WorkItemType 1 types. Keeping ToActiveType is for version compatibility, and removing the judgment of ToActivity type in later versions.
            var sql = @"SELECT 
                            T.*
                        FROM wf_transition_instance T
                        INNER JOIN wf_activity_instance A
                            ON T.to_activity_instance_id = A.Id
                        WHERE T.app_instance_id=@appInstanceId 
                            AND T.process_id=@processId 
                            AND (T.to_activity_type=4 OR T.to_activity_type=5 OR T.to_activity_type=6 OR A.work_item_type=1)          
                        ORDER BY T.created_datetime DESC";

            var list = Repository.Query<TransitionInstanceEntity>(sql,
                new
                {
                    appInstanceId = appInstanceId,
                    processId = processId
                });
            return list;
        }

        /// <summary>
        /// Select data transition based on destination node type
        /// 根据去向节点类型选择转移数据
        /// </summary>
        internal IEnumerable<TransitionInstanceEntity> GetTransitonInstance(string appInstanceId,
            String processId,
            ActivityTypeEnum toActivityType)
        {
            //var list = Repository.GetAll<TransitionInstanceEntity>()
            //            .Where<TransitionInstanceEntity>(
            //                t => t.AppInstanceId == appInstanceId &&
            //                t.ProcessId == processId &&
            //                t.ToActivityType == (int)toActivityType
            //            )
            //            .OrderByDescending(t => t.Id)
            //            .ToList();
            var sql = @"SELECT 
                            * 
                        FROM wf_transition_instance 
                        WHERE app_instance_id=@appInstanceId 
                            AND process_id=@processId 
                            AND to_activity_type=@toActivityType 
                        ORDER BY id DESC";

            var list = Repository.Query<TransitionInstanceEntity>(sql,
                new
                {
                    appInstanceId = appInstanceId,
                    processId = processId,
                    toActivityType = (int)toActivityType
                });

            return list;
        }

        /// <summary>
        /// Obtain the list of transition data
        /// 获取转移数据列表
        /// </summary>
        internal IEnumerable<TransitionInstanceEntity> GetTransitionInstanceList(string appInstanceId,
            string processId,
            string version)
        {
            IEnumerable<TransitionInstanceEntity> list = new List<TransitionInstanceEntity>();
            var pim = new ProcessInstanceManager();
            var pi = pim.GetProcessInstanceCurrent(appInstanceId, processId, version);
            if (pi != null)
            {
                list = GetTransitionInstanceList(appInstanceId, processId, pi.Id);
            }
            return list;
        }

        /// <summary>
        /// Get transition list
        /// 获取转移数据列表
        /// </summary>
        internal IEnumerable<TransitionInstanceEntity> GetTransitionInstanceList(string appInstanceId,
            string processId,
            int processInstanceId)
        {
            //var list = Repository.GetAll<TransitionInstanceEntity>()
            //            .Where<TransitionInstanceEntity>(
            //                t => t.AppInstanceId == appInstanceId &&
            //                t.ProcessId == processId &&
            //                t.ProcessInstanceId == processInstanceId
            //            )
            //            .OrderByDescending(t => t.Id)
            //            .ToList();

            var sql = @"SELECT 
                            * 
                        FROM wf_transition_instance 
                        WHERE app_instance_id=@appInstanceId 
                            AND process_id=@processId 
                            AND process_instance_id=@processInstanceId
                        ORDER BY id DESC";

            var list = Repository.Query<TransitionInstanceEntity>(sql,
                new
                {
                    appInstanceId = appInstanceId,
                    processId = processId,
                    processInstanceId = processInstanceId
                });
            return list;
        }

        /// <summary>
        /// Get the list of activity instances that have been issued for the next step of the current node
        /// 获取当前节点的下一步已经发出的活动实例列表
        /// </summary>
        /// <param name="fromActivityInstanceId"></param>
        /// <returns></returns>
        internal IList<ActivityInstanceEntity> GetTargetActivityInstanceList(int fromActivityInstanceId)
        {
            var session = SessionFactory.CreateSession();
            try
            {
                return GetTargetActivityInstanceList(fromActivityInstanceId, session);
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
        /// Get the list of activity instances that have been issued for the next step of the current node
        /// 获取当前节点的下一步已经发出的活动实例列表
        /// </summary>
        internal IList<ActivityInstanceEntity> GetTargetActivityInstanceList(int fromActivityInstanceId, 
            IDbSession session)
        {
            IList<dynamic> targetIDs = GetTargetActivityInstanceIDs(fromActivityInstanceId, session);
            var nextActivityInstanceList = Repository.GetByIds<ActivityInstanceEntity>(session.Connection, targetIDs).ToList<ActivityInstanceEntity>();

            return nextActivityInstanceList;
        }

        /// <summary>
        /// Traverse the Id of the next activity instance
        /// 遍历下一步活动实例的Id
        /// </summary>
        private IList<dynamic> GetTargetActivityInstanceIDs(int fromActivityInstanceId,
            IDbSession session)
        {
            //var transitionList = Repository.GetAll<TransitionInstanceEntity>(session.Connection, session.Transaction)
            //    .Where<TransitionInstanceEntity>(t => t.FromActivityInstanceId == fromActivityInstanceId)
            //    .ToList();

            var sql = @"SELECT 
                            * 
                        FROM wf_transition_instance 
                        WHERE from_activity_instance_id=@fromActivityInstanceId";

            var transitionList = Repository.Query<TransitionInstanceEntity>(session.Connection,
                sql,
                new
                {
                    fromActivityInstanceId = fromActivityInstanceId,
                });

            var targetIDs = new List<dynamic>();
            foreach (var trans in transitionList)
            {
                if (trans.ToActivityType == (int)ActivityTypeEnum.GatewayNode)
                {
                    targetIDs.AddRange(GetTargetActivityInstanceIDs(trans.ToActivityInstanceId, session));       //遍历Gateway包含的子节点 
                }
                else
                {
                    targetIDs.Add(trans.ToActivityInstanceId);
                }
            }
            return targetIDs;
        }

        /// <summary>
        /// Determine whether the defined Transition has been instantiated and executed
        /// 判读定义的Transition是否已经被实例化执行
        /// </summary>
        internal bool IsTransiionInstancedAndConditionParsedOK(String transitionId,
            IList<TransitionInstanceEntity> transitionInstanceList)
        {
            bool isConainedAndCompletedOK = false;
            foreach (TransitionInstanceEntity transitionInstance in transitionInstanceList)
            {
                //判断连线是否被实例化，并且条件是否满足
                //Determine whether the connection has been instantiated and whether the conditions are met
                if (transitionId == transitionInstance.TransitionId)
                {
                    if (transitionInstance.ConditionParsedResult == (byte)ConditionParseResultEnum.Passed)
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
