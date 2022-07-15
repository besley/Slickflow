using System;
using System.Collections.Generic;
using System.Linq;
using Slickflow.Data;
using Slickflow.Module.Localize;
using Slickflow.Module.Resource;
using Slickflow.Engine.Common;
using Slickflow.Engine.Utility;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;

namespace Slickflow.Engine.Core.Parser
{
    /// <summary>
    /// 上一步活动节点检查器
    /// </summary>
    internal class PreviousStepChecker : ManagerBase
    {
        #region 上一步解析
        /// <summary>
        /// 获取上一步节点树
        /// </summary>
        /// <param name="taskID">任务ID</param>
        /// <param name="hasGatewayPassed">是否经过网关</param>
        /// <returns>上一步步骤列表</returns>
        internal IList<NodeView> GetPreviousActivityTree(int taskID, out Boolean hasGatewayPassed)
        {
            hasGatewayPassed = false;
            //首先获取当前运行节点信息
            var aim = new ActivityInstanceManager();
            var runningNode = aim.GetByTask(taskID);
            var nodeList = GetPreviousActivityTree(runningNode, out hasGatewayPassed);
            return nodeList;

        }

        /// <summary>
        /// 获取上一步节点树
        /// </summary>
        /// <param name="runner">运行者</param>
        /// <param name="hasGatewayPassed">是否经过网关</param>
        /// <returns>上一步步骤列表</returns>
        internal IList<NodeView> GetPreviousActivityTree(WfAppRunner runner, out Boolean hasGatewayPassed)
        {
            hasGatewayPassed = false;
            //首先获取当前运行节点信息
            var aim = new ActivityInstanceManager();
            var runningNode = aim.GetRunningNode(runner);
            var nodeList = GetPreviousActivityTree(runningNode, out hasGatewayPassed);
            return nodeList;
        }

        /// <summary>
        /// 获取上一步节点树
        /// </summary>
        /// <param name="runningNode">运行活动</param>
        /// <param name="hasGatewayPassed">是否经过网关</param>
        /// <returns>上一步步骤列表</returns>
        private IList<NodeView> GetPreviousActivityTree(ActivityInstanceEntity runningNode, out Boolean hasGatewayPassed)
        {
            var aim = new ActivityInstanceManager();

            //获取前置节点列表
            var processInstance = (new ProcessInstanceManager()).GetById(runningNode.ProcessInstanceID);
            var processModel = ProcessModelFactory.Create(processInstance.ProcessGUID, processInstance.Version);
            var previousActivityList = GetPreviousActivityList(runningNode, processModel, out hasGatewayPassed);

            //封装返回结果集合
            var nodeList = new List<NodeView>();
            foreach (var activity in previousActivityList)
            {
                //判断是否是会签节点
                if (processModel.IsMINode(activity) == true)
                {
                    //获取上一步节点的运行记录
                    if (processModel.IsMIParallel(activity) == true)
                    {
                        //并行会签节点读取所有完成的子节点
                        var activityInstanceList = aim.GetPreviousParallelMultipleInstanceListCompleted(runningNode, activity.ActivityGUID);
                        foreach (var ai in activityInstanceList)
                        {
                            AppendNodeViewList(nodeList, activity, ai.EndedByUserID, ai.EndedByUserName);
                        }
                    }
                    else if (processModel.IsMISequence(activity) == true)
                    {
                        if (processModel.IsTaskNode(runningNode) == true)
                        {
                            //当前节点是任务节点
                            //退回到会签节点的最后一步
                            var activityInstance = aim.GetPreviousActivityInstanceSimple(runningNode, activity.ActivityGUID);
                            if (activityInstance != null)
                            {
                                AppendNodeViewList(nodeList, activity, activityInstance.EndedByUserID, activityInstance.EndedByUserName);
                            }
                        }
                        else if (processModel.IsMISequence(activity) == true)
                        {
                            //串行会签节点按照CompleteOrder顺序递减读取上一步
                            var previousAdjacentBrotherNode = GetPreviousOfMultipleInstanceNode(
                                runningNode.MIHostActivityInstanceID.Value,
                                runningNode.ID,
                                runningNode.CompleteOrder.Value);
                            if (previousAdjacentBrotherNode != null)
                            {
                                AppendNodeViewList(nodeList, activity, previousAdjacentBrotherNode.EndedByUserID, previousAdjacentBrotherNode.EndedByUserName);
                            }
                        }
                    }
                }
                else
                {
                    if (hasGatewayPassed == true)
                    {
                        //跨越网关类型
                        var activityInstanceList = aim.GetActivityInstanceListCompletedSimple(runningNode.ProcessInstanceID, activity.ActivityGUID);
                        foreach (var a in activityInstanceList)
                        {
                            AppendNodeViewList(nodeList, activity, a.EndedByUserID, a.EndedByUserName);
                        }
                    }
                    else
                    {
                        //普通任务节点
                        var activityInstance = aim.GetPreviousActivityInstanceSimple(runningNode, activity.ActivityGUID);
                        if (activityInstance != null)
                        {
                            AppendNodeViewList(nodeList, activity, activityInstance.EndedByUserID, activityInstance.EndedByUserName);
                        }
                    }
                }
            }
            return nodeList;
        }

        /// <summary>
        /// 封装节点列表
        /// </summary>
        /// <param name="nodeList">节点列表</param>
        /// <param name="activity">活动</param>
        /// <param name="userID">用户ID</param>
        /// <param name="userName">用户名称</param>
        private void AppendNodeViewList(IList<NodeView> nodeList, 
            Activity activity, 
            string userID, 
            string userName)
        {
            NodeView nodeView = null;
            if (nodeList.Count > 0)
            {
                nodeView = nodeList.FirstOrDefault(n => n.ActivityGUID == activity.ActivityGUID);
            }
            
            if (nodeView == null)
            {
                nodeView = new NodeView
                {
                    ActivityGUID = activity.ActivityGUID,
                    ActivityName = activity.ActivityName,
                    ActivityCode = activity.ActivityCode,
                    ActivityUrl = activity.ActivityUrl,
                    MyProperties = activity.MyProperties,
                    ActivityType = activity.ActivityType
                };
                nodeList.Add(nodeView);
            }

            //添加用户列表
            nodeView.Users = AppendUserList(nodeView.Users, new User
            {
                UserID = userID,
                UserName = userName
            });
        }

        /// <summary>
        /// 增加单个用户
        /// </summary>
        /// <param name="existUserList">用户列表</param>
        /// <param name="user">追加用户</param>
        /// <returns>用户列表</returns>
        private IList<User> AppendUserList(IList<User> existUserList, User user)
        {
            if (existUserList == null)
            {
                existUserList = new List<User>();
            }

            //检验用户是否已经存在
            if (existUserList.Any(u=>u.UserID == user.UserID) == false)
            {
                existUserList.Add(new User { UserID = user.UserID, UserName = user.UserName });
            }
            return existUserList;
        }

        /// <summary>
        /// 获取当前运行节点的上一步节点列表
        /// （包括多实例节点类型）
        /// 1. 多实例节点内部使用CompleteOrder数值递减；
        ///    (其中涉及到多实例节点的主节点跨越模式)
        /// 2. 普通节点模式按照是否有Gateway节点递归回溯；
        /// </summary>
        /// <param name="runningNode">运行节点</param>
        /// <param name="processModel">流程模型</param>
        /// <param name="hasGatewayPassed">是否经过网关节点</param>
        /// <returns>上一步活动列表</returns>
        internal IList<Activity> GetPreviousActivityList(ActivityInstanceEntity runningNode,
            IProcessModel processModel,
            out Boolean hasGatewayPassed)
        {
            var isOfMultipleInstanceNode = false;

            IList<Activity> activityList = new List<Activity>();
            //判断当前节点是否是多实例节点
            if (runningNode.MIHostActivityInstanceID != null)
            {
                if (runningNode.CompleteOrder > 1)
                {
                    //多实例串行节点的中间节点，其上一步就是completeorder-1的节点
                    isOfMultipleInstanceNode = true;
                }
                else if (runningNode.CompleteOrder == 1
                    || runningNode.CompleteOrder == -1)
                {
                    //第一种条件：只有串行模式下有CompleteOrder的值为 1
                    //串行模式多实例的第一个执行节点，此时可退回的节点是主节点的上一步
                    //第一种条件：只有并行模式下有CompleteOrder的值为 -1
                    //并行节点，此时可退回的节点是主节点的上一步
                    ;
                }
                else
                {
                    throw new ApplicationException(LocalizeHelper.GetEngineMessage("previousstepchecker.getpreviousactivitylist.error"));
                }
            }

            //返回前置节点列表
            hasGatewayPassed = false;
            if (isOfMultipleInstanceNode == true)
            {
                //已经是中间节点，只能退回到上一步多实例子节点
                var entity = GetPreviousOfMultipleInstanceNode(runningNode.MIHostActivityInstanceID.Value,
                    runningNode.ID,
                    runningNode.CompleteOrder.Value);
                var activity = processModel.GetActivity(entity.ActivityGUID);

                activityList.Add(activity);
            }
            else
            {
                activityList = processModel.GetPreviousActivityList(runningNode.ActivityGUID, out hasGatewayPassed);
            }
            return activityList;
        }

        /// <summary>
        /// 查询实例节点的前置节点
        /// </summary>
        /// <param name="mainActivityInstanceID">主节点实例ID</param>
        /// <param name="activityInstanceID">活动实例ID</param>
        /// <param name="completeOrder">完成顺序</param>
        /// <returns>活动实例</returns>
        internal ActivityInstanceEntity GetPreviousOfMultipleInstanceNode(int mainActivityInstanceID,
            int activityInstanceID,
            float completeOrder)
        {
            var whereSql = @"SELECT * FROM WfActivityInstance
                            WHERE MIHostActivityInstanceID = @mainActivityInstanceID
                                AND CompleteOrder = @completeOrder-1
                                AND ActivityState=@activityState
                            ORDER BY ID DESC
                            ";
            var entity = Repository.GetFirst<ActivityInstanceEntity>(whereSql,
                new
                {
                    mainActivityInstanceID = mainActivityInstanceID,
                    completeOrder = completeOrder,
                    activityInstanceID = activityInstanceID,
                    activityState = (short)ActivityStateEnum.Completed
                });

            return entity;
        }

        /// <summary>
        /// 读取节点的上一步节点信息
        /// </summary>
        /// <param name="runningNode">当前节点</param>
        /// <param name="isLookUpBackSource">是否退回</param>
        /// <param name="hasPassedGatewayNode">是否经由路由节点</param>
        /// <returns>活动实例列表</returns>
        internal IList<ActivityInstanceEntity> GetPreviousActivityInstanceList(ActivityInstanceEntity runningNode,
            bool isLookUpBackSource,
            out bool hasPassedGatewayNode)
        {
            hasPassedGatewayNode = false;
            var tim = new TransitionInstanceManager();
            var transitionList = tim.GetTransitionInstanceList(runningNode.AppInstanceID,
                runningNode.ProcessGUID,
                runningNode.ProcessInstanceID).ToList();

            var backSrcActivityInstanceId = 0;
            if (isLookUpBackSource == true)
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
        #endregion
    }
}
