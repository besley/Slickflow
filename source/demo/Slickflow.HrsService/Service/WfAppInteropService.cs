using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slickflow.Data;
using Slickflow.Engine.Common;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Engine.Core.Result;
using Slickflow.Engine.Service;

namespace Slickflow.HrsService.Service
{
    /// <summary>
    /// Interactive service class between engine and application side
    /// Example code for developers' reference
    /// 引擎和应用端的交互服务类
    /// 示例代码，供开发人员参考
    /// </summary>
    public class WfAppInteropService
    {
        /// <summary>
        /// Start Process
        /// </summary>
        public WfExecutedResult StartProcess(WfAppRunner runner)
        {
            var wfService = new WorkflowService();
            var result = wfService.StartProcess(runner);

            return result;
        }

        /// <summary>
        /// Run Process
        /// </summary>
        public WfExecutedResult RunProcess(IDbSession session, WfAppRunner runner, IDictionary<string, string> conditions = null)
        {
            var result = new WfExecutedResult();
            var wfService = new WorkflowService();
            var nodeViewList = wfService.GetNextActivityTree(runner, conditions).ToList<NodeView>();

            foreach (var node in nodeViewList)
            {
                var performerList = wfService.GetPerformerList(node);      
                Dictionary<string, PerformerList> dict = new Dictionary<string, PerformerList>();
                dict.Add(node.ActivityGUID, performerList);
                runner.NextActivityPerformers = dict;

                result = wfService.RunProcessApp(session.Connection, runner, session.Transaction);
            }
            return result;
        }

        /// <summary>
        /// Check Process Instance Running status
        /// </summary>
        public Boolean CheckProcessInstanceRunning(WfAppRunner runner)
        {
            var isRunning = false;
            var wfService = new WorkflowService();
            var instance = wfService.GetRunningProcessInstance(runner);
            if (instance != null)
            {
                isRunning = true;
            }
            return isRunning;
        }

        /// <summary>
        /// Get Activity
        /// </summary>
        public Activity GetActivity(string processGUID, string version, string activityGUID)
        {
            var wfService = new WorkflowService();
            var activityEntity = wfService.GetActivityEntity(processGUID, version, activityGUID);

            return activityEntity;
        }
    }
}
