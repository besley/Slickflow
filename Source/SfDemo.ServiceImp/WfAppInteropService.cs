using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slickflow.Data;
using Slickflow.Engine.Common;
using Slickflow.Engine.Core.Result;
using Slickflow.Engine.Service;

namespace SfDemo.ServiceImp
{
    /// <summary>
    /// 引擎和应用端的交互服务类
    /// 示例代码，请勿直接作为生产项目代码使用。
    /// </summary>
    public class WfAppInteropService
    {
        /// <summary>
        /// 启动流程
        /// </summary>
        public WfExecutedResult StartProcess(WfAppRunner runner)
        {
            //启动流程
            var wfService = new WorkflowService();
            var result = wfService.StartProcess(runner);

            return result;
        }

        /// <summary>
        /// 工作流运行
        /// </summary>
        /// <param name="session"></param>
        /// <param name="runner"></param>
        /// <returns></returns>
        public WfExecutedResult RunProcess(IDbSession session, WfAppRunner runner, IDictionary<string, string> conditions = null)
        {
            var result = new WfExecutedResult();
            var wfService = new WorkflowService();
            var nodeViewList = wfService.GetNextActivityTree(runner, conditions).ToList<NodeView>();

            foreach (var node in nodeViewList)
            {
                var performerList = wfService.GetPerformerList(node);       //根据节点角色定义，读取执行者列表
                Dictionary<string, PerformerList> dict = new Dictionary<string, PerformerList>();
                dict.Add(node.ActivityGUID, performerList);
                runner.NextActivityPerformers = dict;

                if (node.IsSkipTo == true)
                {
                    result = wfService.JumpProcess(session.Connection, runner, session.Transaction);
                }
                else
                {
                    result = wfService.RunProcessApp(session.Connection, runner, session.Transaction);
                }
            }
            return result;
        }

        /// <summary>
        /// 检查流程是否已经运行
        /// </summary>
        /// <param name="runner">流程查询属性</param>
        /// <returns></returns>
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
    }
}
