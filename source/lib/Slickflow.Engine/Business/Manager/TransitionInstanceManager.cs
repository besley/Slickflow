
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
        /// <param name="processInstance"></param>
        /// <param name="transitionGUID"></param>
        /// <param name="fromActivityInstance"></param>
        /// <param name="toActivityInstance"></param>
        /// <param name="transitionType"></param>
        /// <param name="flyingType"></param>
        /// <param name="runner"></param>
        /// <param name="conditionParseResult"></param>
        /// <returns></returns>
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

        #region Insert 数据插入
        /// <summary>
        /// Insert transition
        /// 插入方法
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="entity"></param>
        /// <param name="trans"></param>
        internal int Insert(IDbConnection conn,
            TransitionInstanceEntity entity,
            IDbTransaction trans)
        {
            int newID = Repository.Insert(conn, entity, trans);
            entity.ID = newID;

            return entity.ID;
        }

        /// <summary>
        /// Delete transition
        /// 删除转移实例
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="transitionInstanceID"></param>
        /// <param name="trans"></param>
        internal void Delete(IDbConnection conn,
            int transitionInstanceID,
            IDbTransaction trans)
        {
            Repository.Delete<TransitionInstanceEntity>(conn, transitionInstanceID, trans);
        }
        #endregion

        #region Query task 数据查询
        /// <summary>
        /// Retrieve instance data based on ID
        /// 根据ID获取实例数据
        /// </summary>
        /// <param name="transitionInstanceID"></param>
        /// <returns></returns>
        internal TransitionInstanceEntity GetById(int transitionInstanceID)
        {
            return Repository.GetById<TransitionInstanceEntity>(transitionInstanceID);
        }

        /// <summary>
        /// Get End transition Data
        /// 获取结束转移数据
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="appInstanceID"></param>
        /// <param name="processGUID"></param>
        /// <returns></returns>
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
        /// Obtain the final transition entity data
        /// 获取最后的转移实体数据
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="appInstanceID"></param>
        /// <param name="processGUID"></param>
        /// <returns></returns>
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
        /// Obtain a transition list of WorkItem type destination nodes
        /// 获得去向节点是WorkItem类型的转移列表
        /// </summary>
        /// <param name="appInstanceID"></param>
        /// <param name="processGUID"></param>
        /// <returns></returns>
        internal IEnumerable<TransitionInstanceEntity> GetWorkItemTransitonInstance(string appInstanceID,
            String processGUID)
        {
            //2015.09.11 besley
            //需考虑后期节点类型增加目前支持TaskNode, SubProcessNode, MultipleInstanceNode
            //以上都是WorkItemType为1类型，保留ToActivityType是为了版本兼容，后期版本去掉ToActivity类型的判断。
            //It is necessary to consider adding node types in the later stage. Currently, TaskNode is supported, SubProcessNode, MultipleInstanceNode
            //The above are all WorkItemType 1 types. Keeping ToActiveType is for version compatibility, and removing the judgment of ToActivity type in later versions.
            var sql = @"SELECT 
                            T.* 
                        FROM WfTransitionInstance T
                        INNER JOIN WfActivityInstance A
                            ON T.ToActivityInstanceID = A.ID
                        WHERE T.AppInstanceID=@appInstanceID 
                            AND T.ProcessGUID=@processGUID 
                            AND (T.ToActivityType=4 OR T.ToActivityType=5 OR T.ToActivityType=6 OR A.WorkItemType=1)          
                        ORDER BY T.CreatedDateTime DESC";

            var list = Repository.Query<TransitionInstanceEntity>(sql,
                new
                {
                    appInstanceID = appInstanceID,
                    processGUID = processGUID
                });
            return list;
        }

        /// <summary>
        /// Select data transition based on destination node type
        /// 根据去向节点类型选择转移数据
        /// </summary>
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

            var list = Repository.Query<TransitionInstanceEntity>(sql,
                new
                {
                    appInstanceID = appInstanceID,
                    processGUID = processGUID,
                    toActivityType = (int)toActivityType
                });
            return list;
        }

        /// <summary>
        /// Obtain the list of transition data
        /// 获取转移数据列表
        /// </summary>
        /// <param name="appInstanceID"></param>
        /// <param name="processGUID"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        internal IEnumerable<TransitionInstanceEntity> GetTransitionInstanceList(string appInstanceID,
            string processGUID,
            string version)
        {
            IEnumerable<TransitionInstanceEntity> list = new List<TransitionInstanceEntity>();
            var pim = new ProcessInstanceManager();
            var pi = pim.GetProcessInstanceCurrent(appInstanceID, processGUID, version);
            if (pi != null)
            {
                list = GetTransitionInstanceList(appInstanceID, processGUID, pi.ID);
            }
            return list;
        }

        /// <summary>
        /// Get transition list
        /// 获取转移数据列表
        /// </summary>
        /// <param name="appInstanceID"></param>
        /// <param name="processGUID"></param>
        /// <param name="processInstanceID"></param>
        /// <returns></returns>
        internal IEnumerable<TransitionInstanceEntity> GetTransitionInstanceList(string appInstanceID,
            string processGUID,
            int processInstanceID)
        {
            var sql = @"SELECT * FROM WfTransitionInstance 
                        WHERE AppInstanceID=@appInstanceID 
                            AND ProcessGUID=@processGUID 
                            AND ProcessInstanceID=@processInstanceID
                        ORDER BY CreatedDateTime DESC";

            var list = Repository.Query<TransitionInstanceEntity>(sql,
                new
                {
                    appInstanceID = appInstanceID,
                    processGUID = processGUID.ToString(),
                    processInstanceID = processInstanceID
                });
            return list;
        }

        /// <summary>
        /// Get the list of activity instances that have been issued for the next step of the current node
        /// 获取当前节点的下一步已经发出的活动实例列表
        /// </summary>
        /// <param name="fromActivityInstanceID"></param>
        /// <returns></returns>
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
        /// Get the list of activity instances that have been issued for the next step of the current node
        /// 获取当前节点的下一步已经发出的活动实例列表
        /// </summary>
        /// <param name="fromActivityInstanceID"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        internal IList<ActivityInstanceEntity> GetTargetActivityInstanceList(int fromActivityInstanceID, 
            IDbSession session)
        {
            IList<dynamic> targetIDs = GetTargetActivityInstanceIDs(fromActivityInstanceID, session);
            var nextActivityInstanceList = Repository.GetByIds<ActivityInstanceEntity>(session.Connection, targetIDs).ToList<ActivityInstanceEntity>();

            return nextActivityInstanceList;
        }

        /// <summary>
        /// Traverse the ID of the next activity instance
        /// 遍历下一步活动实例的ID
        /// </summary>
        /// <param name="fromActivityInstanceID"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        private IList<dynamic> GetTargetActivityInstanceIDs(int fromActivityInstanceID,
            IDbSession session)
        {
            var sql = @"SELECT * FROM WfTransitionInstance 
                        WHERE FromActivityInstanceID=@fromActivityInstanceID";

            var transitionList = Repository.Query<TransitionInstanceEntity>(session.Connection,
                sql,
                new
                {
                    fromActivityInstanceID = fromActivityInstanceID,
                });

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
        /// Determine whether the defined Transition has been instantiated and executed
        /// 判读定义的Transition是否已经被实例化执行
        /// </summary>
        /// <param name="transitionGUID"></param>
        /// <param name="transitionInstanceList"></param>
        /// <returns></returns>
        internal bool IsTransiionInstancedAndConditionParsedOK(String transitionGUID,
            IList<TransitionInstanceEntity> transitionInstanceList)
        {
            bool isConainedAndCompletedOK = false;
            foreach (TransitionInstanceEntity transitionInstance in transitionInstanceList)
            {
                //判断连线是否被实例化，并且条件是否满足
                //Determine whether the connection has been instantiated and whether the conditions are met
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
