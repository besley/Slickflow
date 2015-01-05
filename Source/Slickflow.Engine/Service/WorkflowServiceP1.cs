using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading;
using Slickflow.Engine.Common;
using Slickflow.Engine.Utility;
using Slickflow.Data;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;

namespace Slickflow.Engine.Service
{
    /// <summary>
    /// 工作流服务（数据查询）
    /// </summary>
    public partial class WorkflowService : IWorkflowService
    {
        /// <summary>
        /// 获取流程实例数据
        /// </summary>
        /// <param name="processInstanceID"></param>
        /// <returns></returns>
        public ProcessInstanceEntity GetProcessInstance(int processInstanceID)
        {
            var pim = new ProcessInstanceManager();
            var instance = pim.GetById(processInstanceID);
            return instance;
        }

        /// <summary>
        /// 获取流程正常实例数据
        /// </summary>
        /// <param name="runner"></param>
        /// <returns></returns>
        public ProcessInstanceEntity GetProcessInstance(WfAppRunner runner, IDbConnection conn = null)
        {
            if (conn == null)
            {
                conn = SessionFactory.CreateConnection();
            }

            try
            {
                var pim = new ProcessInstanceManager();
                var processInstance = pim.GetProcessInstanceLatest(runner.AppName, 
                    runner.AppInstanceID, runner.ProcessGUID);
                return processInstance;
            }
            catch
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// 获取运行中的流程实例
        /// </summary>
        /// <param name="runner"></param>
        /// <returns></returns>
        public ProcessInstanceEntity GetRunningProcessInstance(WfAppRunner runner)
        {
            ProcessInstanceEntity entity = null;
            IDbConnection conn = SessionFactory.CreateConnection();
            try
            {
                var pim = new ProcessInstanceManager();
                entity = pim.GetRunningProcessInstance(conn, runner.AppName, 
                    runner.AppInstanceID, runner.ProcessGUID);
            }
            catch
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
            return entity;
        }

        /// <summary>
        /// 获取流程发起人信息
        /// </summary>
        /// <param name="processInstanceID"></param>
        /// <returns></returns>
        public Performer GetProcessInitiator(int processInstanceID)
        {
            Performer performer = null;
            try
            {
                var pim = new ProcessInstanceManager();
                performer = pim.GetProcessInitiator(processInstanceID);
            }
            catch
            {
                throw;
            }
            return performer;
        }

        /// <summary>
        /// 获取活动实例数据
        /// </summary>
        /// <param name="activityInstanceID"></param>
        /// <returns></returns>
        public ActivityInstanceEntity GetActivityInstance(int activityInstanceID)
        {
            var aim = new ActivityInstanceManager();
            var instance = aim.GetById(activityInstanceID);
            return instance;
        }

        /// <summary>
        /// 获取一个流程实例下的所有活动实例
        /// </summary>
        /// <param name="processInstanceID"></param>
        /// <returns></returns>
        public IList<ActivityInstanceEntity> GetActivityInstances(int processInstanceID)
        {
            var aim = new ActivityInstanceManager();
            var session = SessionFactory.CreateSession();
            try
            {
                return aim.GetActivityInstances(processInstanceID, session);
            }
            catch
            {
                throw;
            }
            finally
            {
                session.Dispose();
            }
        }

        /// <summary>
        /// 获取当前等待办理节点的任务分配人列表
        /// </summary>
        /// <param name="runner"></param>
        /// <returns></returns>
        public IList<Performer> GetTaskPerformers(WfAppRunner runner)
        {
            var tm = new TaskManager();
            var tasks = tm.GetReadyTaskOfApp(runner).ToList();

            Performer performer;
            IList<Performer> performerList = new List<Performer>();
            foreach (var task in tasks)
            {
                performer = new Performer(task.AssignedToUserID, task.AssignedToUserName);
                performerList.Add(performer);
            }
            return performerList;
        }

        public void EntrustTask(TaskEntrustedEntity entrusted)
        {
            var tm = new TaskManager();
            tm.Entrust(entrusted);
        }

        /// <summary>
        /// 获取流程当前运行节点信息
        /// </summary>
        /// <param name="runner"></param>
        /// <returns></returns>
        public ActivityInstanceEntity GetRunningNode(WfAppRunner runner)
        {
            var aim = new ActivityInstanceManager();
            var entity = aim.GetRunningNodeOfMine(runner);

            return entity;
        }

        public bool IsMineTask(ActivityInstanceEntity entity, string userID)
        {
            var aim = new ActivityInstanceManager();
            bool isMine = aim.IsMineTask(entity, userID);
            return isMine;
        }
    }
}
