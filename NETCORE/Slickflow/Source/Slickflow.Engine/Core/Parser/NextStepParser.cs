using System;
using System.Collections.Generic;
using System.Linq;
using ServiceStack.Text;
using Slickflow.Data;
using Slickflow.Module.Resource;
using Slickflow.Engine.Common;
using Slickflow.Engine.Utility;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;

namespace Slickflow.Engine.Core.Parser
{
    /// <summary>
    /// 下一步的步骤列表读取类
    /// </summary>
    internal class NextStepParser
    {
        /// <summary>
        /// 流程下一步信息获取
        /// </summary>
        /// <param name="resourceService">资源服务</param>
        /// <param name="runner">当前运行用户</param>
        /// <param name="condition">条件</param>
        /// <returns>下一步信息</returns>
        internal NextStepInfo GetNextStepInfo(IResourceService resourceService,
            WfAppRunner runner, 
            IDictionary<string, string> condition = null)
        {
            NextStepInfo nextStepInfo = new NextStepInfo();
            var nextActivityTree = GetNextActivityRoleUserTree(resourceService, runner, condition);
            nextStepInfo.NextActivityRoleUserTree = nextActivityTree;
            nextStepInfo.NextActivityPerformers = GetNextActivityPerformers(nextActivityTree, runner);

            return nextStepInfo;
        }

        /// <summary>
        /// 获取预选步骤人员列表
        /// </summary>
        /// <param name="nextActivityTree">下一步活动节点树</param>
        /// <param name="runner">当前运行用户</param>
        /// <returns>步骤预选人员列表</returns>
        private IDictionary<string, PerformerList> GetNextActivityPerformers(IList<NodeView> nextActivityTree,
            WfAppRunner runner)
        {
            IDictionary<string, PerformerList> nextSteps = null;

            var tm = new TaskManager();
            TaskViewEntity taskView = tm.GetTaskOfMine(runner);

            //读取活动实例中记录的步骤预选数据
            var aim = new ActivityInstanceManager();
            if (taskView.MIHostActivityInstanceID != null)
            {
                var mainActivityInstanceID = taskView.MIHostActivityInstanceID.Value;
                var mainActivityInstance = aim.GetById(mainActivityInstanceID);
                if (mainActivityInstance != null)
                {
                    nextSteps = NextStepUtility.DeserializeNextStepPerformers(mainActivityInstance.NextStepPerformers);
                }
            }
            
            //获取网关节点信息
            IProcessModel processModel = ProcessModelFactory.Create(runner.ProcessGUID, runner.Version);
            var nextActivity = processModel.GetNextActivity(taskView.ActivityGUID);
            if (nextActivity.ActivityType == ActivityTypeEnum.GatewayNode)
            {
                var gatewayActivityInstance = aim.GetActivityInstanceLatest(taskView.ProcessInstanceID, nextActivity.ActivityGUID);
                if (gatewayActivityInstance != null
                    && !string.IsNullOrEmpty(gatewayActivityInstance.NextStepPerformers))
                {
                    nextSteps = NextStepUtility.DeserializeNextStepPerformers(gatewayActivityInstance.NextStepPerformers);
                }
            }
            return nextSteps;
        }

        /// <summary>
        /// 根据应用获取流程下一步节点列表，包含角色用户
        /// </summary>
        /// <param name="resourceService">资源服务</param>
        /// <param name="runner">应用执行人</param>
        /// <param name="condition">条件</param>
        /// <returns>节点列表</returns>
        internal IList<NodeView> GetNextActivityRoleUserTree(IResourceService resourceService, 
            WfAppRunner runner,
            IDictionary<string, string> condition = null)
        {
            //判断应用数据是否缺失
            if (string.IsNullOrEmpty(runner.AppInstanceID)
                || string.IsNullOrEmpty(runner.ProcessGUID))
            {
                throw new WorkflowException("应用(AppInstanceID)或流程(ProcessGUID)参数为空，请重新传入正确参数！");
            }

            //条件参数一致
            if (condition == null && runner.Conditions != null)
            {
                condition = runner.Conditions;
            }

            IList<NodeView> nextSteps = new List<NodeView>();
            IProcessModel processModel = ProcessModelFactory.Create(runner.ProcessGUID, runner.Version);

            using (var session = SessionFactory.CreateSession())
            {
                var pim = new ProcessInstanceManager();
                var processInstanceList = pim.GetProcessInstance(session.Connection, 
                    runner.AppInstanceID, 
                    runner.ProcessGUID,
                    session.Transaction).ToList();
                var processInstanceEntity = EnumHelper.GetFirst<ProcessInstanceEntity>(processInstanceList);

                //判断流程是否创建还是已经运行
                if (processInstanceEntity != null
                    && processInstanceEntity.ProcessState == (short)ProcessStateEnum.Running)
                {
                    //运行状态的流程实例
                    var tm = new TaskManager();
                    TaskViewEntity taskView = tm.GetTaskOfMine(session.Connection, runner, session.Transaction);

                    //获取下一步列表
                    nextSteps = processModel.GetNextActivityTree(taskView.ActivityGUID, condition);

                    foreach (var ns in nextSteps)
                    {
                        if (ns.ReceiverType == ReceiverTypeEnum.ProcessInitiator)       //下一步执行人为流程发起人
                        {
                            ns.Users = AppendUserList(ns.Users, pim.GetProcessInitiator(session.Connection,
                                taskView.ProcessInstanceID,
                                session.Transaction));   //获取流程发起人
                        }
                        else
                        {
                            var roleIDs = ns.Roles.Select(x => x.ID).ToArray();
                            if (roleIDs.Count() > 0)
                            {
                                ns.Users = resourceService.GetUserListByRoleReceiverType(roleIDs, runner.UserID, (int)ns.ReceiverType);     //增加转移前置过滤条件
                            }
                        }
                    }
                }
                else
                {
                    //流程准备启动，获取第一个办理节点的用户列表
                    var firstActivity = processModel.GetFirstActivity();
                    nextSteps = processModel.GetNextActivityTree(firstActivity.ActivityGUID,
                        condition);

                    foreach (var ns in nextSteps)
                    {
                        var roleIDs = ns.Roles.Select(x => x.ID).ToArray();
                        ns.Users = resourceService.GetUserListByRoleReceiverType(roleIDs, runner.UserID, (int)ns.ReceiverType);     //增加转移前置过滤条件
                    }
                }
            }

            return nextSteps;
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
            existUserList.Add(new User { UserID = user.UserID, UserName = user.UserName });
            return existUserList;
        }


        /// <summary>
        /// 构造用户列表
        /// </summary>
        /// <param name="existUserList">用户列表</param>
        /// <param name="newUserList">追加用户列表</param>
        /// <returns>用户列表</returns>
        private IList<User> AppendUserList(IList<User> existUserList, IList<User> newUserList)
        {
            if (existUserList == null)
            {
                existUserList = new List<User>();
            }

            foreach (var user in newUserList)
            {
                if (existUserList.Select(r => r.UserName == user.UserID).ToList() != null)
                {
                    existUserList.Add(new User { UserID = user.UserID, UserName = user.UserName });
                }
            }
            return existUserList;
        }
    }
}
