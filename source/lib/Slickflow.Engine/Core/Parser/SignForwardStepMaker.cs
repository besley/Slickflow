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
using static IronPython.Runtime.Profiler;
using System.Reflection;

namespace Slickflow.Engine.Core.Parser
{
    /// <summary>
    /// Sign Forward Step Maker
    /// 加签步骤生成器
    /// </summary>
    internal class SignForwardStepMaker : ManagerBase
    {
        /// <summary>
        /// Obtain information on the steps for adding a signature
        /// 获取加签步骤信息
        /// </summary>
        /// <param name="runner"></param>
        /// <returns></returns>
        internal SignForwardStepInfo GetSignForwardStepInfo(WfAppRunner runner)
        {
            var signForwardStepInfo = new SignForwardStepInfo();
            var signResult = GetSignForwardRoleUserTree(runner);
            signForwardStepInfo.SignForwardRoleUserTree = signResult.StepList;
            return signForwardStepInfo;
        }

        /// <summary>
        /// According to the application, obtain the next node list of the process, including role users
        /// 根据应用获取流程下一步节点列表，包含角色用户
        /// </summary>
        /// <param name="runner"></param>
        /// <returns></returns>
        internal NextActivityTreeResult GetSignForwardRoleUserTree(WfAppRunner runner)
        {
            var nextTreeResult = new NextActivityTreeResult();

            //判断应用数据是否缺失
            //Determine whether the application data is missing
            if (string.IsNullOrEmpty(runner.AppInstanceID)
                || string.IsNullOrEmpty(runner.ProcessGUID))
            {
                throw new WorkflowException(LocalizeHelper.GetEngineMessage("nextstepparser.getnextactivityroleusertree.error"));
            }

            using (var session = SessionFactory.CreateSession())
            {
                //运行状态的流程实例
                //Process instance of running status
                var tm = new TaskManager();
                TaskViewEntity taskView = tm.GetTaskOfMine(session.Connection, runner, session.Transaction);

                var isRunningTask = tm.CheckTaskStateInRunningState(taskView);
                if (isRunningTask == false)
                {
                    throw new WorkflowException(LocalizeHelper.GetEngineMessage("nextstepparser.getnextactivityroleusertree.notrunning.error"));
                }

                var treeNodeList = new List<NodeView>();
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
