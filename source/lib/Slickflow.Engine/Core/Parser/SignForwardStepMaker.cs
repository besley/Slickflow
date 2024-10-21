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
    internal class SignForwardStepMaker : ManagerBase
    {
        /// <summary>
        /// 流程下一步信息获取
        /// </summary>
        /// <param name="runner">当前运行用户</param>
        /// <returns>下一步信息</returns>
        internal SignForwardStepInfo GetSignForwardStepInfo(WfAppRunner runner)
        {
            var signForwardStepInfo = new SignForwardStepInfo();
            var signResult = GetSignForwardRoleUserTree(runner);
            signForwardStepInfo.SignForwardRoleUserTree = signResult.StepList;
            return signForwardStepInfo;
        }

        /// <summary>
        /// 根据应用获取流程下一步节点列表，包含角色用户
        /// </summary>
        /// <param name="runner">应用执行人</param>
        /// <returns>节点列表</returns>
        internal NextActivityTreeResult GetSignForwardRoleUserTree(WfAppRunner runner)
        {
            var nextTreeResult = new NextActivityTreeResult();

            //判断应用数据是否缺失
            if (string.IsNullOrEmpty(runner.AppInstanceID)
                || string.IsNullOrEmpty(runner.ProcessGUID))
            {
                throw new WorkflowException(LocalizeHelper.GetEngineMessage("nextstepparser.getnextactivityroleusertree.error"));
            }

            using (var session = SessionFactory.CreateSession())
            {
                //运行状态的流程实例
                var tm = new TaskManager();
                TaskViewEntity taskView = tm.GetTaskOfMine(session.Connection, runner, session.Transaction);

                var isRunningTask = tm.CheckTaskStateInRunningState(taskView);
                if (isRunningTask == false)
                {
                    throw new WorkflowException(LocalizeHelper.GetEngineMessage("nextstepparser.getnextactivityroleusertree.notrunning.error"));
                }

                var treeNodeList = new List<NodeView>();
                //var processModel = ProcessModelFactory.Create(runner.ProcessGUID, runner.Version);
                var processModel = ProcessModelFactory.CreateByTask(session.Connection, taskView, session.Transaction);
                var currentActivity = processModel.GetActivity(taskView.ActivityGUID);
                treeNodeList.Add(new NodeView
                {
                    ActivityGUID = currentActivity.ActivityGUID,
                    ActivityName = currentActivity.ActivityName,
                    ActivityCode = currentActivity.ActivityCode,
                    ActivityUrl = currentActivity.ActivityUrl,
                    MyProperties = currentActivity.MyProperties,
                    ActivityType = currentActivity.ActivityType,
                    Roles = processModel.GetActivityRoles(currentActivity.ActivityGUID)
                });
                nextTreeResult.StepList = treeNodeList;
            }
            return nextTreeResult;
        }
     }
}
