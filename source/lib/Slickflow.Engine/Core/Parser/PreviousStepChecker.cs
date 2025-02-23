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
    /// Previous Activity Node Parser
    /// 上一步活动节点解析器
    /// </summary>
    internal class PreviousStepChecker : ManagerBase
    {
        /// <summary>
        /// Retrieve the previous node tree
        /// 获取上一步节点树
        /// </summary>
        /// <param name="taskID"></param>
        /// <param name="hasGatewayPassed"></param>
        /// <returns></returns>
        internal IList<NodeView> GetPreviousActivityTree(int taskID, out Boolean hasGatewayPassed)
        {
            hasGatewayPassed = false;
            var aim = new ActivityInstanceManager();
            var runningNode = aim.GetByTask(taskID);
            var nodeList = GetPreviousActivityTree(runningNode, out hasGatewayPassed);
            return nodeList;

        }

        /// <summary>
        /// Retrieve the previous node tree
        /// 获取上一步节点树
        /// </summary>
        /// <param name="runner"></param>
        /// <param name="hasGatewayPassed"></param>
        /// <returns></returns>
        internal IList<NodeView> GetPreviousActivityTree(WfAppRunner runner, out Boolean hasGatewayPassed)
        {
            hasGatewayPassed = false;
            var aim = new ActivityInstanceManager();
            var runningNode = aim.GetRunningNode(runner);
            var nodeList = GetPreviousActivityTree(runningNode, out hasGatewayPassed);
            return nodeList;
        }

        /// <summary>
        /// Retrieve the previous node tree
        /// 获取上一步节点树
        /// </summary>
        /// <param name="runningNode"></param>
        /// <param name="hasGatewayPassed"></param>
        /// <returns></returns>
        private IList<NodeView> GetPreviousActivityTree(ActivityInstanceEntity runningNode, out Boolean hasGatewayPassed)
        {
            var aim = new ActivityInstanceManager();

            //获取前置节点列表
            //Get the list of previous nodes
            var processInstance = (new ProcessInstanceManager()).GetById(runningNode.ProcessInstanceID);
            var processModel = ProcessModelFactory.CreateByProcessInstance(processInstance);
            var previousActivityList = GetPreviousActivityList(runningNode, processModel, out hasGatewayPassed);

            //封装返回结果集合
            //Encapsulate the return result set
            var nodeList = new List<NodeView>();
            foreach (var activity in previousActivityList)
            {
                //判断上一步是否是会签节点
                //Determine whether the previous step was a signing together node
                if (processModel.IsMINode(activity) == true)
                {
                    //获取上一步节点的运行记录
                    //Get the running record of the previous node
                    if (processModel.IsMIParallel(activity) == true)
                    {
                        //并行会签节点读取所有完成的子节点
                        //Parallel row signing node reads all completed child nodes
                        var activityInstanceList = aim.GetPreviousParallelMultipleInstanceListCompleted(runningNode, activity.ActivityID);
                        foreach (var ai in activityInstanceList)
                        {
                            AppendNodeViewList(nodeList, activity, ai.EndedByUserID, ai.EndedByUserName);
                        }
                    }
                    else if (processModel.IsMISequence(activity) == true)
                    {
                        //是两个不同节点之间的退回处理，取前一节点的最后一个完成的多实例节点
                        //It is a return process between two different nodes, taking the last completed multi instance node from the previous node
                        if (activity.ActivityID != runningNode.ActivityID)
                        {
                            //退回到会签节点的最后一步
                            //Return to the final step of the co signing node
                            var activityInstance = aim.GetPreviousActivityInstanceSimple(runningNode, activity.ActivityID);
                            if (activityInstance != null)
                            {
                                AppendNodeViewList(nodeList, activity, activityInstance.EndedByUserID, activityInstance.EndedByUserName);
                            }
                        }
                        else
                        {
                            //多实例节点的内部退回处理
                            //串行会签节点按照CompleteOrder顺序递减读取上一步
                            //Internal rollback processing of multiple instance nodes
                            //Serial countersignature nodes read the previous step in descending order of Completed Order
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
                        //Cross gateway type
                        var activityInstanceList = aim.GetActivityInstanceListCompletedSimple(runningNode.ProcessInstanceID, activity.ActivityID);
                        foreach (var a in activityInstanceList)
                        {
                            AppendNodeViewList(nodeList, activity, a.EndedByUserID, a.EndedByUserName);
                        }
                    }
                    else
                    {
                        //普通任务节点
                        //Normal task node
                        var activityInstance = aim.GetPreviousActivityInstanceSimple(runningNode, activity.ActivityID);
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
        /// Append Node View List
        /// 追加节点列表
        /// </summary>
        /// <param name="nodeList"></param>
        /// <param name="activity"></param>
        /// <param name="userID"></param>
        /// <param name="userName"></param>
        private void AppendNodeViewList(IList<NodeView> nodeList, 
            Activity activity, 
            string userID, 
            string userName)
        {
            NodeView nodeView = null;
            if (nodeList.Count > 0)
            {
                nodeView = nodeList.FirstOrDefault(n => n.ActivityID == activity.ActivityID);
            }
            
            if (nodeView == null)
            {
                nodeView = new NodeView
                {
                    ActivityID = activity.ActivityID,
                    ActivityName = activity.ActivityName,
                    ActivityCode = activity.ActivityCode,
                    ActivityUrl = activity.ActivityUrl,
                    MyProperties = activity.MyProperties,
                    ActivityType = activity.ActivityType
                };
                nodeList.Add(nodeView);
            }

            //添加用户列表
            //Append user list
            nodeView.Users = AppendUserList(nodeView.Users, new User
            {
                UserID = userID,
                UserName = userName
            });
        }

        /// <summary>
        /// Append Single User
        /// 增加单个用户
        /// </summary>
        /// <param name="existUserList"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        private IList<User> AppendUserList(IList<User> existUserList, User user)
        {
            if (existUserList == null)
            {
                existUserList = new List<User>();
            }

            //检验用户是否已经存在
            //Verify if the user already exists
            if (existUserList.Any(u=>u.UserID == user.UserID) == false)
            {
                existUserList.Add(new User { UserID = user.UserID, UserName = user.UserName });
            }
            return existUserList;
        }

        /// <summary>
        /// Retrieve the previous node list of the current running node
        /// (including multiple instance node types)
        ///  1.  Multiple instance nodes use a decreasing value of CompleteOrder internally;
        /// (This involves the master node crossing mode with multiple instance nodes)
        ///  2.  The normal node mode recursively backtracks based on whether there is a Gateway node;
        /// 获取当前运行节点的上一步节点列表
        /// （包括多实例节点类型）
        /// 1. 多实例节点内部使用CompleteOrder数值递减；
        ///    (其中涉及到多实例节点的主节点跨越模式)
        /// 2. 普通节点模式按照是否有Gateway节点递归回溯；
        /// </summary>
        /// <param name="runningNode"></param>
        /// <param name="processModel"></param>
        /// <param name="hasGatewayPassed"></param>
        /// <returns></returns>
        internal IList<Activity> GetPreviousActivityList(ActivityInstanceEntity runningNode,
            IProcessModel processModel,
            out Boolean hasGatewayPassed)
        {
            var isOfMultipleInstanceNode = false;

            IList<Activity> activityList = new List<Activity>();
            //判断当前节点是否是多实例节点
            //Determine whether the current node is a multi instance node
            if (runningNode.MIHostActivityInstanceID != null)
            {
                if (runningNode.CompleteOrder > 1)
                {
                    //多实例串行节点的中间节点，其上一步就是completeorder-1的节点
                    //The intermediate node of a multi instance serial node, whose previous step is the node of compleorder-1
                    isOfMultipleInstanceNode = true;
                }
                else if (runningNode.CompleteOrder == 1
                    || runningNode.CompleteOrder == -1)
                {
                    //第一种条件：只有串行模式下有CompleteOrder的值为 1
                    //串行模式多实例的第一个执行节点，此时可退回的节点是主节点的上一步
                    //第一种条件：只有并行模式下有CompleteOrder的值为 -1
                    //并行节点，此时可退回的节点是主节点的上一步
                    //The first condition: Only in serial mode, there is a value of 1 for CompleteOrder
                    //The first executing node of multiple instances in serial mode, and the node that can be returned at this time is the previous step of the master node
                    //The first condition: Only in parallel mode, there is a value of -1 for CompleteOrder
                    //Parallel nodes, the node that can be returned at this time is the previous step of the master node
                    ;
                }
                else
                {
                    throw new ApplicationException(LocalizeHelper.GetEngineMessage("previousstepchecker.getpreviousactivitylist.error"));
                }
            }

            //返回前置节点列表
            //Return the list of predecessor nodes
            hasGatewayPassed = false;
            if (isOfMultipleInstanceNode == true)
            {
                //已经是中间节点，只能退回到上一步多实例子节点
                //It is already an intermediate node and can only be reverted back to the previous step of the multi instance node
                var entity = GetPreviousOfMultipleInstanceNode(runningNode.MIHostActivityInstanceID.Value,
                    runningNode.ID,
                    runningNode.CompleteOrder.Value);
                var activity = processModel.GetActivity(entity.ActivityID);

                activityList.Add(activity);
            }
            else
            {
                activityList = processModel.GetPreviousActivityList(runningNode.ActivityID, out hasGatewayPassed);
            }
            return activityList;
        }

        /// <summary>
        /// Query the predecessor nodes of the instance node
        /// 查询实例节点的前置节点
        /// </summary>
        /// <param name="mainActivityInstanceID"></param>
        /// <param name="activityInstanceID"></param>
        /// <param name="completeOrder"></param>
        /// <returns></returns>
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
        /// Read the previous node information of the node
        /// 读取节点的上一步节点信息
        /// </summary>
        /// <param name="runningNode"></param>
        /// <param name="isLookUpBackSource"></param>
        /// <param name="hasPassedGatewayNode"></param>
        /// <returns></returns>
        internal IList<ActivityInstanceEntity> GetPreviousActivityInstanceList(ActivityInstanceEntity runningNode,
            bool isLookUpBackSource,
            out bool hasPassedGatewayNode)
        {
            hasPassedGatewayNode = false;
            var tim = new TransitionInstanceManager();
            var transitionList = tim.GetTransitionInstanceList(runningNode.AppInstanceID,
                runningNode.ProcessID,
                runningNode.ProcessInstanceID).ToList();

            var backSrcActivityInstanceId = 0;
            if (isLookUpBackSource == true)
            {
                //退回情况下的处理
                //Handling in case of return
                if (runningNode.MIHostActivityInstanceID != null && runningNode.CompleteOrder.Value == 1)
                {
                    //多实例的第一个子节点，先找到主节点，再到transition记录表中找到上一步节点
                    //The first child node of multiple instances, first find the main node,
                    //and then find the previous node in the transition record table
                    backSrcActivityInstanceId = runningNode.MIHostActivityInstanceID.Value;
                }
                else if (runningNode.BackSrcActivityInstanceID != null)
                {
                    //节点时曾经发生退回的节点
                    //Nodes that have experienced a sendback in the past
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
                //如果是网关节点，则继续查找
                //If it is a gateway node, continue searching
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
        /// Obtain the nodes before the gateway node
        /// 获取网关节点前的节点
        /// </summary>
        /// <param name="transitionList"></param>
        /// <param name="toActivityInstanceID"></param>
        /// <param name="previousActivityInstanceList"></param>
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
    }
}
