using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using Slickflow.Module.Resource;
using Slickflow.Engine.Common;
using Slickflow.Data;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Engine.Core.Parser;
using Slickflow.Engine.Business.Result;

namespace Slickflow.Engine.Service
{
    /// <summary>
    /// Workflow Service - Query Methods
    /// 工作流服务 - 查询方法
    /// </summary>
    public partial class WorkflowService : IWorkflowService
    {
        #region Get Next Activity Info
        /// <summary>
        /// Obtain the first feasible node of the process
        /// 获取流程的第一个可办理节点
        /// </summary>
        public Activity GetFirstActivity(string processId, string version)
        {
            var processModel = ProcessModelFactory.CreateByProcess(processId, version);
            var firstActivity = processModel.GetFirstActivity();
            return firstActivity;
        }

        /// <summary>
        /// Get the node list of task types
        /// 获取任务类型的节点列表
        /// </summary>
        public IList<Activity> GetTaskActivityList(string processId, string version)
        {
            var processModel = ProcessModelFactory.CreateByProcess(processId, version);
            var activityList = processModel.GetTaskActivityList();

            return activityList;
        }

        /// <summary>
        /// Get the node list of task types
        /// 获取任务类型的节点列表
        /// </summary>
        public IList<Activity> GetTaskActivityList(int processId)
        {
            var pm = new ProcessManager();
            var entity = pm.GetById(processId);
            var processModel = ProcessModelFactory.CreateByProcess(entity);
            var activityList = processModel.GetTaskActivityList();

            return activityList;
        }

        /// <summary>
        /// Get a list of nodes for all task types (including countersignature and subprocesses)
        /// 获取全部任务类型的节点列表（包含会签和子流程）
        /// </summary>
        public IList<Activity> GetAllTaskActivityList(string processId, string version)
        {
            var processModel = ProcessModelFactory.CreateByProcess(processId, version);
            var activityList = processModel.GetAllTaskActivityList();

            return activityList;
        }

        /// <summary>
        /// Get the next node information of the current node
        /// 获取当前节点的下一个节点信息
        /// </summary>
        public Activity GetNextActivity(string processId, 
            string version, 
            string activityId)
        {
            var processModel = ProcessModelFactory.CreateByProcess(processId, version);
            var nextActivity = processModel.GetNextActivity(activityId);
            return nextActivity;
        }

        /// <summary>
        /// Get the next node information of the current node
        /// 获取当前节点的下一个节点信息
        /// </summary>
        public IList<NodeView> GetNextActivityTree(String processId,
            String version,
            String activityId,
            IDictionary<string, string> condition)
        {
            var processModel = ProcessModelFactory.CreateByProcess(processId, version);
            using (var session = SessionFactory.CreateSession())
            {
                var nextResult = processModel.GetNextActivityListView(activityId, null, condition, session);

                return nextResult.StepList;
            }
        }

        /// <summary>
        /// Simple mode: Obtain the next node of the process based on the application 
        /// (without considering the situation of multiple subsequent nodes)
        /// 简单模式：根据应用获取流程下一步节点(不考虑有多个后续节点的情况）
        /// </summary>
        public NodeView GetNextActivity(WfAppRunner runner,
            IDictionary<string, string> condition = null)
        {
            var info = GetNextStepInfo(runner, condition);
            return info?.NextActivityRoleUserTree?.FirstOrDefault();
        }

        /// <summary>
        /// Simple mode: Obtain the next node of the process based on the application 
        /// (without considering the situation of multiple subsequent nodes)
        /// 简单模式：根据应用获取流程下一步节点(不考虑有多个后续节点的情况）
        /// </summary>
        public NodeView GetNextActivity(int taskId,
            IDictionary<string, string> condition = null)
        {
            var runner = new WfAppRunner { TaskId = taskId };
            var info = GetNextStepInfo(runner, condition);
            return info?.NextActivityRoleUserTree?.FirstOrDefault();
        }

        /// <summary>
        /// Obtain the next node list according to the application process
        /// 根据应用获取流程下一步节点列表
        /// </summary>
        public IList<NodeView> GetNextActivityTree(WfAppRunner runner, 
            IDictionary<string, string> condition = null)
        {
            var info = GetNextStepInfo(runner, condition);
            return info?.NextActivityRoleUserTree ?? new List<NodeView>();
        }

        /// <summary>
        /// Obtain the next node list according to the application process
        /// 根据应用获取流程下一步节点列表
        /// </summary>
        public async Task<IList<NodeView>> GetNextActivityTreeAsync(WfAppRunner runner, 
            IDictionary<string, string> condition = null)
        {
            var info = await GetNextStepInfoAsync(runner, condition);
            return info?.NextActivityRoleUserTree ?? new List<NodeView>();
        }

        /// <summary>
        /// Obtain the next node list according to the application process
        /// 根据应用获取流程下一步节点列表
        /// </summary>
        public IList<NodeView> GetNextActivityTree(IDbConnection conn, 
            WfAppRunner runner,
            IDictionary<string, string> condition,
            IDbTransaction trans)
        {
            // Connection/transaction parameters are currently ignored; kept for signature compatibility.
            var info = GetNextStepInfo(runner, condition);
            return info?.NextActivityRoleUserTree ?? new List<NodeView>();
        }

        /// <summary>
        /// Obtain the next node list according to the application process
        /// 根据应用获取流程下一步节点列表
        /// </summary>
        public async Task<IList<NodeView>> GetNextActivityTreeAsync(IDbConnection conn, 
            WfAppRunner runner,
            IDictionary<string, string> condition, 
            IDbTransaction trans)
        {
            var info = await GetNextStepInfoAsync(runner, condition);
            return info?.NextActivityRoleUserTree ?? new List<NodeView>();
        }

        /// <summary>
        /// Obtain the next node list according to the application process
        /// 获取下一步活动列表树
        /// </summary>
        public IList<NodeView> GetNextActivityTree(int taskId,
            IDictionary<string, string> condition = null)
        {
            var runner = new WfAppRunner { TaskId = taskId };
            var info = GetNextStepInfo(runner, condition);
            return info?.NextActivityRoleUserTree ?? new List<NodeView>();
        }

        /// <summary>
        /// Obtain the next node list according to the application process
        /// 获取下一步活动列表树
        /// </summary>
        public async Task<IList<NodeView>> GetNextActivityTreeAsync(int taskId, 
            IDictionary<string, string> condition = null)
        {
            var runner = new WfAppRunner { TaskId = taskId };
            var info = await GetNextStepInfoAsync(runner, condition);
            return info?.NextActivityRoleUserTree ?? new List<NodeView>();
        }

        /// <summary>
        /// Obtain the next activity list according to the application process
        /// 根据应用获取流程下一步活动列表
        /// </summary>
        public IList<Activity> GetNextActivityList(IDbConnection conn, 
            WfAppRunner runner,
            IDictionary<string, string> condition,
            IDbTransaction trans)
        {
            // Connection/transaction parameters are currently ignored; kept for signature compatibility.
            var info = GetNextStepInfo(runner, condition);
            return ConvertNodeViewListToActivityList(info?.NextActivityRoleUserTree, runner.ProcessId, runner.Version);
        }
        
        /// <summary>
        /// Obtain the next activity list according to the application process
        /// 根据应用获取流程下一步活动列表
        /// </summary>
        public async Task<IList<Activity>> GetNextActivityListAsync(IDbConnection conn, 
            WfAppRunner runner,
            IDictionary<string, string> condition, 
            IDbTransaction trans)
        {
            var info = await GetNextStepInfoAsync(runner, condition);
            return ConvertNodeViewListToActivityList(info?.NextActivityRoleUserTree, runner.ProcessId, runner.Version);
        }
        
        /// <summary>
        /// Obtain the next activity list according to the application process
        /// 根据应用获取流程下一步活动列表
        /// </summary>
        public IList<Activity> GetNextActivityList(WfAppRunner runner,
            IDictionary<string, string> condition = null)
        {
            var info = GetNextStepInfo(runner, condition);
            return ConvertNodeViewListToActivityList(info?.NextActivityRoleUserTree, runner.ProcessId, runner.Version);
        }

        /// <summary>
        /// Obtain the next activity list according to the application process
        /// 根据应用获取流程下一步活动列表
        /// </summary>
        public async Task<IList<Activity>> GetNextActivityListAsync(WfAppRunner runner,
            IDictionary<string, string> condition = null)
        {
            var info = await GetNextStepInfoAsync(runner, condition);
            return ConvertNodeViewListToActivityList(info?.NextActivityRoleUserTree, runner.ProcessId, runner.Version);
        }

        /// <summary>
        /// Obtain the next activity list according to the application process
        /// 获取下一步活动列表
        /// </summary>
        public IList<Activity> GetNextActivityList(int taskId,
            IDictionary<string, string> condition = null)
        {
            var runner = new WfAppRunner { TaskId = taskId };
            var info = GetNextStepInfo(runner, condition);
            return ConvertNodeViewListToActivityList(info?.NextActivityRoleUserTree, runner.ProcessId, runner.Version);
        }
        
        /// <summary>
        /// Obtain the next activity list according to the application process
        /// 获取下一步活动列表
        /// </summary>
        public async Task<IList<Activity>> GetNextActivityListAsync(int taskId, 
            IDictionary<string, string> condition = null)
        {
            var runner = new WfAppRunner { TaskId = taskId };
            var info = await GetNextStepInfoAsync(runner, condition);
            return ConvertNodeViewListToActivityList(info?.NextActivityRoleUserTree, runner.ProcessId, runner.Version);
        }
        
        /// <summary>
        /// Convert NodeView list to Activity list
        /// 将 NodeView 列表转换为 Activity 列表
        /// </summary>
        private IList<Activity> ConvertNodeViewListToActivityList(IList<NodeView> nodeViewList, string processId, string version)
        {
            if (nodeViewList == null || nodeViewList.Count == 0)
            {
                return new List<Activity>();
            }
            
            if (string.IsNullOrEmpty(processId) || string.IsNullOrEmpty(version))
            {
                return new List<Activity>();
            }
            
            var activityList = new List<Activity>();
            
            try
            {
                var processModel = ProcessModelFactory.CreateByProcess(processId, version);
                
                // Extract unique ActivityId from NodeView list
                var activityIds = nodeViewList.Select(nv => nv.ActivityId)
                    .Where(id => !string.IsNullOrEmpty(id))
                    .Distinct()
                    .ToList();
                
                // Convert each ActivityId to Activity object
                foreach (var activityId in activityIds)
                {
                    try
                    {
                        var activity = processModel.GetActivity(activityId);
                        if (activity != null)
                        {
                            activityList.Add(activity);
                        }
                    }
                    catch
                    {
                        // Skip invalid activity IDs
                    }
                }
            }
            catch
            {
                // Return empty list if ProcessModel creation fails
            }
            
            return activityList;
        }

        /// <summary>
        /// According to the application, obtain the next node list of the process, including role users
        /// 根据应用获取流程下一步节点列表，包含角色用户
        /// </summary>
        public IList<NodeView> GetNextActivityRoleUserTree(WfAppRunner runner, 
            IDictionary<string, string> condition = null)
        {
            var info = GetNextStepInfo(runner, condition);
            return info?.NextActivityRoleUserTree ?? new List<NodeView>();
        }

        /// <summary>
        /// According to the application, obtain the next node list of the process, including role users
        /// 根据应用获取流程下一步节点列表，包含角色用户
        /// </summary>
        public async Task<IList<NodeView>> GetNextActivityRoleUserTreeAsync(WfAppRunner runner, 
            IDictionary<string, string> condition = null)
        {
            var info = await GetNextStepInfoAsync(runner, condition);
            return info?.NextActivityRoleUserTree ?? new List<NodeView>();
        }

        /// <summary>
        /// According to the application, obtain the next node list of the process, including role users
        /// Includes:
        /// 1) The next step for the gateway is to add a pre loaded user list of personnel;
        /// 2) The next step in the co signing mode is to add a pre loaded user list of personnel;
        /// 根据应用获取流程下一步节点列表，包含角色用户
        /// 包含：
        /// 1) 网关下一步添加人员的预加载用户列表；
        /// 2) 会签模式的下一步添加人员的预加载用户列表；
        /// </summary>
        public NextStepInfo GetNextStepInfo(WfAppRunner runner, 
            IDictionary<string, string> condition = null)
        {
            var nsp = new NextStepParser();
            var nextStepInfo = nsp.GetNextStepInfo(ResourceService, runner, condition);

            return nextStepInfo;
        }

        /// <summary>
        /// Asynchronous retrieval of next step list
        /// 异步获取下一步列表
        /// </summary>
        public async Task<NextStepInfo> GetNextStepInfoAsync(WfAppRunner runner, 
            IDictionary<string, string> condition = null)
        {
            var task = await Task.Run<NextStepInfo>(() =>
            {
                return GetNextStepInfo(runner, condition);
            });
            return task;
        }

        /// <summary>
        /// Get the list of node roles and personnel for the next step of the starting node when the process starts
        /// Unified integration into: GetNextActivityRoleUserTree()
        /// 流程启动时获取开始节点下一步的节点角色人员列表
        /// 统一整合到: GetNextActivityRoleUserTree()
        /// </summary>
        public IList<NodeView> GetFirstActivityRoleUserTree(WfAppRunner runner, 
            IDictionary<string, string> condition = null)
        {
            using (var session = SessionFactory.CreateSession())
            {
                var processModel = ProcessModelFactory.CreateByProcess(runner.ProcessId, runner.Version);
                var firstActivity = processModel.GetFirstActivity();
                var nextResult = processModel.GetNextActivityListView(firstActivity.ActivityId,
                    null,
                    condition,
                    session);

                foreach (var ns in nextResult.StepList)
                {
                    var roleIDs = ns.Roles.Select(x => x.Id).ToArray();
                    ns.Users = ResourceService.GetUserListByRoleReceiverType(roleIDs, runner.UserId, (int)ns.ReceiverType);     //增加转移前置过滤条件
                }
                return nextResult.StepList;
            }
        }

        /// <summary>
        /// Get the list of node roles and personnel for the next step of the starting node when the process starts
        /// Unified integration into: GetNextActivityRoleUserTree()
        /// 流程启动时获取开始节点下一步的节点角色人员列表
        /// 统一整合到: GetNextActivityRoleUserTree()
        /// </summary>
        public async Task<IList<NodeView>> GetFirstActivityRoleUserTreeAsync(WfAppRunner runner, 
            IDictionary<string, string> condition = null)
        {
            var task = await Task.Run<IList<NodeView>>(() =>
            {
                return GetFirstActivityRoleUserTree(runner, condition);
            });
            return task;
        }

        /// <summary>
        /// Get activity instance completed
        /// 获取已经完成的节点
        /// </summary>
        public IList<NodeImage> GetActivityInstanceCompleted(int taskId)
        {
            IList<NodeImage> imageList = new List<NodeImage>();

            var tm = new TaskManager();
            var task = tm.GetTaskView(taskId);

            var am = new ActivityInstanceManager();
            var list = am.GetActivityInstanceListCompleted(task.AppInstanceId, task.ProcessId);

            foreach (ActivityInstanceEntity a in list)
            {
                imageList.Add(new NodeImage
                {
                    Id = a.Id,
                    ActivityName = a.ActivityName
                });
            }
            return imageList;
        }

        /// <summary>
        /// Get activity instance completed
        /// 获取已经完成的节点记录
        /// </summary>
        public IList<NodeImage> GetActivityInstanceCompleted(WfAppRunner runner)
        {
            IList<NodeImage> imageList = new List<NodeImage>();
            var am = new ActivityInstanceManager();
            var list = am.GetActivityInstanceListCompleted(runner.AppInstanceId, runner.ProcessId);

            foreach (ActivityInstanceEntity a in list)
            {
                imageList.Add(new NodeImage
                { 
                    Id = a.Id, 
                    ActivityName = a.ActivityName 
                });
            }
            return imageList;
        }

        /// <summary>
        /// Get transition instance list
        /// 获取转移实例记录
        /// </summary>
        public IList<TransitionImage> GetTransitionInstanceList(TransitionInstanceQuery query)
        {
            IList<TransitionImage> imageList = new List<TransitionImage>();
            var tm = new TransitionInstanceManager();
            var list = tm.GetTransitionInstanceList(query.AppInstanceId, query.ProcessId, query.Version).ToList();

            foreach (TransitionInstanceEntity t in list)
            {
                imageList.Add(new TransitionImage
                {
                    Id = t.Id,
                    TransitionId = t.TransitionId
                });
            }

            return imageList;
        }

        /// <summary>
        /// Get activity entity
        /// 获取当前活动实体
        /// </summary>
        public Activity GetActivityEntity(string processId, string version, string activityId)
        {
            var processModel = ProcessModelFactory.CreateByProcess(processId, version);
            return processModel.GetActivity(activityId);
        }

        /// <summary>
        /// Get activity roles
        /// 获取活动节点下的角色数据
        /// </summary>
        public IList<Role> GetActivityRoles(string processId, string version, string activityId)
        {
            var processModel = ProcessModelFactory.CreateByProcess(processId, version);
            return processModel.GetActivityRoles(activityId);
        }
        #endregion

        #region Get Previous Activity Info
        /// <summary>
        /// Get previous activity list
        /// 获取上一步节点信息
        /// </summary>
        public IList<NodeView> GetPreviousActivityTree(WfAppRunner runner)
        {
            var psc = new PreviousStepChecker();
            var hasGatewayPassed = false;
            var nodeList = psc.GetPreviousActivityTree(runner, out hasGatewayPassed);

            return nodeList;
        }

        /// <summary>
        /// Get previous activity list
        /// 获取上一步节点信息
        /// </summary>
        public async Task<IList<NodeView>> GetPreviousActivityTreeAsync(WfAppRunner runner)
        {
            var task = await Task.Run<IList<NodeView>>(() =>
            {
                return GetPreviousActivityTree(runner);
            });
            return task;
        }

        /// <summary>
        /// Get previous activity list
        /// 获取上一步节点信息
        /// </summary>
        public PreviousStepInfo GetPreviousStepInfo(WfAppRunner runner)
        {             
            var hasGatewayPassed = false;
            var psc = new PreviousStepChecker();
            var nodeList = psc.GetPreviousActivityTree(runner, out hasGatewayPassed);
            var psi = new PreviousStepInfo();
            psi.PreviousActivityRoleUserTree = nodeList;
            psi.HasGatewayPassed = hasGatewayPassed;

            return psi;
        }

        /// <summary>
        /// Get previous activity list
        /// 异步获取上一步节点信息
        /// </summary>
        public async Task<PreviousStepInfo> GetPreviousStepInfoAsync(WfAppRunner runner)
        {
            var task = await Task.Run<PreviousStepInfo>(() =>
            {
                return GetPreviousStepInfo(runner);
            });
            return task;
        }
        #endregion

        #region Get Sign Froward Step Info
        /// <summary>
        /// Obtain performer information for the signing process
        /// 获取加签步骤人员信息
        /// </summary>
        public SignForwardStepInfo GetSignForwardStepInfo(WfAppRunner runner)
        {
            var ssp = new SignForwardStepMaker();
            var stepInfo = ssp.GetSignForwardStepInfo(runner);

            return stepInfo;
        }
        #endregion

        #region Process Instance Query
        /// <summary>
        /// Get process instance by id
        /// 获取流程实例数据
        /// </summary>
        public ProcessInstanceEntity GetProcessInstance(int processInstanceId)
        {
            var pim = new ProcessInstanceManager();
            var instance = pim.GetById(processInstanceId);
            return instance;
        }

        /// <summary>
        /// Get process instance by id
        /// 获取流程实例数据
        /// </summary>
        public ProcessInstanceEntity GetProcessInstance(IDbConnection conn, 
            int processInstanceId, 
            IDbTransaction trans)
        {
            var pim = new ProcessInstanceManager();
            var instance = pim.GetById(conn, processInstanceId, trans);
            return instance;
        }

        /// <summary>
        /// Get process instance by app instance id
        /// 获取流程实例数据
        /// </summary>
        public IList<ProcessInstanceEntity> GetProcessInstance(string appInstanceId)
        {
            var pim = new ProcessInstanceManager();
            var list = pim.GetProcessInstance(appInstanceId).ToList();
            return list;
        }

        /// <summary>
        /// Get process instance by activity instance id
        /// 获取流程实例数据
        /// </summary>
        public ProcessInstanceEntity GetProcessInstanceByActivity(int activityInstanceId)
        {
            using (var session = SessionFactory.CreateSession())
            {
                var pim = new ProcessInstanceManager();
                var instance = pim.GetByActivity(session.Connection, activityInstanceId, session.Transaction);
                return instance;
            }
        }

        /// <summary>
        /// Get process instance by runner
        /// 获取运行中的流程实例
        /// </summary>
        public ProcessInstanceEntity GetRunningProcessInstance(WfAppRunner runner)
        {
            var pim = new ProcessInstanceManager();
            var entity = pim.GetRunningProcessInstance(runner.AppInstanceId, runner.ProcessId);
            return entity;
        }

        /// <summary>
        /// Get process instance count
        /// 判断流程实例是否存在
        /// </summary>
        public Int32 GetProcessInstanceCount(string processId, string version)
        {
            var pim = new ProcessInstanceManager();
            return pim.GetProcessInstanceCount(processId, version);
        }

        /// <summary>
        /// Get process initiator
        /// 获取流程发起人信息
        /// </summary>
        public User GetProcessInitiator(int processInstanceId)
        {
            User initiator = null;
            try
            {
                var pim = new ProcessInstanceManager();
                initiator = pim.GetProcessInitiator(processInstanceId);
            }
            catch
            {
                throw;
            }
            return initiator;
        }

        /// <summary>
        /// Get activity instance
        /// 获取活动实例数据
        /// </summary>
        public ActivityInstanceEntity GetActivityInstance(int activityInstanceId)
        {
            var aim = new ActivityInstanceManager();
            var instance = aim.GetById(activityInstanceId);
            return instance;
        }

        /// <summary>
        /// Get activity instance
        /// 获取一个流程实例下的所有活动实例
        /// </summary>
        public IList<ActivityInstanceEntity> GetActivityInstanceList(int processInstanceId)
        {
            var aim = new ActivityInstanceManager();
            var session = SessionFactory.CreateSession();
            try
            {
                return aim.GetActivityInstanceList(processInstanceId, session);
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
        /// Get target activity instance list
        /// 获取目标活动实例列表
        /// </summary>
        public IList<ActivityInstanceEntity> GetTargetActivityInstanceList(int fromActivityInstanceId)
        {
            var tim = new TransitionInstanceManager();
            return tim.GetTargetActivityInstanceList(fromActivityInstanceId);
        }

        /// <summary>
        /// Get task performers
        /// 获取当前等待办理节点的任务分配人列表
        /// </summary>
        public IList<Performer> GetTaskPerformers(WfAppRunner runner)
        {
            var tm = new TaskManager();
            var tasks = tm.GetReadyTaskOfApp(runner).ToList();

            Performer performer;
            IList<Performer> performerList = new List<Performer>();
            foreach (var task in tasks)
            {
                performer = new Performer(task.AssignedUserId, task.AssignedUserName);
                performerList.Add(performer);
            }
            return performerList;
        }

        /// <summary>
        /// Determine whether the current task is the last task
        /// (Suitable for scenarios with simple nodes or multiple instance nodes)
        /// 判断当前任务是否是最后一个任务
        /// (适应于简单节点或者多实例节点的场景)
        /// </summary>
        public Boolean IsLastTask(int taskId)
        {
            var tm = new TaskManager();
            return tm.IsLastTask(taskId);
        }

        /// <summary>
        /// Entrust task
        /// 创建新的委托任务
        /// </summary>
        public WfDataManagedResult EntrustTask(TaskEntrustedEntity entrusted, bool cancalOriginalTask = true)
        {
            var tm = new TaskManager();
            return tm.Entrust(entrusted, cancalOriginalTask);
        }

        public void CancelEntrustedTask(int id)
        {
            var tm = new TaskManager();
            tm.CancelEntrustedTask(id);
        }

        /// <summary>
        /// Replace task
        /// 取代任务
        /// </summary>
        public Boolean ReplaceTask(int taskId, List<TaskReplacedEntity> replaced, WfAppRunner runner)
        {
            var tm = new TaskManager();
            return tm.Replace(taskId, replaced, runner);
        }

        /// <summary>
        /// Set process overdue
        /// 设置流程实例的过期时间
        /// </summary>
        public Boolean SetProcessOverdue(int processInstanceId, DateTime overdueDateTime, WfAppRunner runner)
        {
            var pim = new ProcessInstanceManager();
            return pim.SetOverdue(processInstanceId, overdueDateTime, runner);
        }

        /// <summary>
        /// Set the scheduled job of the process instance to completion status
        /// (Used for HangFire backend polling task)
        /// 设置活动实例的定时作业为完成状态
        /// (用于HangFire后台轮询任务)
        /// </summary>
        public void SetProcessJobTimerCompleted(IDbConnection conn, int processInstanceId, IDbTransaction trans)
        {
            var pim = new ProcessInstanceManager();
            var processInstance = pim.GetById(conn, processInstanceId, trans);
            processInstance.JobTimerStatus = (short)JobTimerStatusEnum.Completed;
            processInstance.JobTimerTreatedDateTime = System.DateTime.UtcNow;
            pim.Update(conn, processInstance, trans);
        }

        /// <summary>
        /// Set the scheduled job of the activity instance to completion status
        /// (Used for HangFire backend polling task)
        /// 设置活动实例的定时作业为完成状态
        /// (用于HangFire后台轮询任务)
        /// </summary>
        public void SetActivityJobTimerCompleted(IDbConnection conn, int activityInstanceId, IDbTransaction trans)
        {
            var aim = new ActivityInstanceManager();
            var activityInstance = aim.GetById(conn, activityInstanceId, trans);
            activityInstance.JobTimerStatus = (short)JobTimerStatusEnum.Completed;
            activityInstance.JobTimerTreatedDateTime = System.DateTime.UtcNow;
            aim.Update(conn, activityInstance, trans);
        }

        /// <summary>
        /// Get running node
        /// 获取流程当前运行节点信息
        /// </summary>
        public ActivityInstanceEntity GetRunningNode(WfAppRunner runner)
        {
            var aim = new ActivityInstanceManager();
            var entity = aim.GetRunningNode(runner);

            return entity;
        }

        /// <summary>
        /// Determine if it is my task
        /// 判断是否是我的任务
        /// </summary>
        public bool IsMineTask(ActivityInstanceEntity entity, string userId)
        {
            var aim = new ActivityInstanceManager();
            bool isMine = aim.IsMineTask(entity, userId);
            return isMine;
        }

        /// <summary>
        /// Get running activity instance
        /// 获取正在运行中的活动实例
        /// </summary>
        public IList<ActivityInstanceEntity> GetRunningActivityInstance(TaskQuery query)
        {
            var aim = new ActivityInstanceManager();
            var list = aim.GetRunningActivityInstanceList(query.AppInstanceId, query.ProcessId, query.Version).ToList();
            return list;
        }

        /// <summary>
        /// Get process variable
        /// 获取流程变量
        /// </summary>
        public ProcessVariableEntity GetProcessVariable(int variableId)
        {
            var pvm = new ProcessVariableManager();
            var entity = pvm.GetVariableById(variableId);

            return entity;
        }

        /// <summary>
        /// Get process variable
        /// 获取流程变量
        /// </summary>
        public ProcessVariableEntity GetProcessVariable(ProcessVariableQuery query)
        {
            var pvm = new ProcessVariableManager();
            var entity = pvm.GetVariableEntity(query);
            return entity;
        }

        /// <summary>
        /// Get process variable list
        /// 获取变量列表
        /// </summary>
        public IList<ProcessVariableEntity> GetProcessVariableList(ProcessVariableQuery query)
        {
            var pvm = new ProcessVariableManager();
            var list = pvm.GetVariableList(query.ProcessInstanceId);
            return list;
        }

        /// <summary>
        /// Delete process variable
        /// 删除流程变量
        /// </summary>
        public void DeleteProcessVariable(int variableId)
        {
            var pvm = new ProcessVariableManager();
            pvm.DeleteVariable(variableId);
        }

        /// <summary>
        /// Validate process variable
        /// 验证触发表达式是否满足
        /// </summary>
        public Boolean ValidateProcessVariable(int processInstanceId, string expression)
        {
            var isValidated = false;
            using (var conn = SessionFactory.CreateConnection())
            {
                var pvm = new ProcessVariableManager();
                isValidated = pvm.ValidateProcessVariable(conn, processInstanceId, expression, null);
            }
            return isValidated;
        }

        /// <summary>
        /// Update process instance
        /// 更新流程实例实体
        /// </summary>
        public void UpdateProcessInstance(ProcessInstanceEntity entity)
        {
            var pim = new ProcessInstanceManager();
            pim.Update(entity);
        }

        /// <summary>
        /// Save process variable
        /// 保存流程变量
        /// </summary>
        public int SaveProcessVariable(ProcessVariableEntity entity)
        {
            var pvm = new ProcessVariableManager();
            var entityId = pvm.SaveVariable(entity);
            return entityId;
        }
        #endregion

        #region Task Read
        /// <summary>
        /// Set task to read status (retrieve task based on task Id)
        /// 设置任务为已读状态(根据任务ID获取任务)
        /// </summary>
        public bool SetTaskRead(WfAppRunner runner)
        {
            bool isRead = false;
            try
            {
                var taskManager = new TaskManager();
                taskManager.SetTaskRead(runner);
                isRead = true;
            }
            catch (System.Exception)
            {
                throw;
            }

            return isRead;
        }

        /// <summary>
        /// Set task email send status
        /// 更新任务邮件发送状态
        /// </summary>
        public Boolean SetTaskEMailSent(int taskId)
        {
            bool isSetOK = false;
            try
            {
                var taskManager = new TaskManager();
                taskManager.SetTaskEMailSent(taskId);
                isSetOK = true;
            }
            catch (System.Exception)
            {
                throw;
            }

            return isSetOK;
        }

        /// <summary>
        /// Get task view
        /// 获取任务视图
        /// </summary>
        public TaskViewEntity GetTaskView(int taskId)
        {
            var tm = new TaskManager();
            var entity = tm.GetTaskView(taskId);
            return entity;
        }

        /// <summary>
        /// Get task view
        /// 获取任务视图
        /// </summary>
        public TaskViewEntity GetTaskView(int processInstanceId, int activityInstanceId)
        {
            var tm = new TaskManager();
            var entity = tm.GetTaskViewByActivity(processInstanceId, activityInstanceId);
            return entity;
        }

        /// <summary>
        /// Get task view
        /// 获取任务视图
        /// </summary>
        public TaskViewEntity GetTaskView(IDbConnection conn, string appInstanceId, string processId, string userId, IDbTransaction trans)
        {
            var tm = new TaskManager();
            var entity = tm.GetTaskOfMine(conn, appInstanceId, processId, userId, trans);
            return entity;
        }

        /// <summary>
        /// Get running tasks
        /// 获取运行中的任务
        /// </summary>
        public IList<TaskViewEntity> GetRunningTasks(TaskQuery query)
        {
            var taskManager = new TaskManager();
            var taskList = taskManager.GetRunningTasks(query);
            if (taskList != null)
                return taskList.ToList();
            else
                return null;
        }

        /// <summary>
        /// Get first runnint task of activity instance
        /// 获取活动实例下的第一个任务记录
        /// </summary>
        public TaskViewEntity GetFirstRunningTask(int activityInstanceId)
        {
            var taskManager = new TaskManager();
            var taskView = taskManager.GetFirstRunningTask(activityInstanceId);
            return taskView;
        }

        /// <summary>
        /// Get tasks in ready status
        /// 获取待办任务
        /// </summary>
        public IList<TaskViewEntity> GetReadyTasks(TaskQuery query)
        {
            var taskManager = new TaskManager();
            var taskList = taskManager.GetReadyTasks(query);

            if (taskList != null)
                return taskList.ToList();
            else
                return null;
        }

        /// <summary>
        /// Get tasks in completed status
        /// 获取办结任务列表
        /// </summary>
        public IList<TaskViewEntity> GetCompletedTasks(TaskQuery query)
        {
            var taskManager = new TaskManager();
            var taskList = taskManager.GetCompletedTasks(query);

            if (taskList != null)
                return taskList.ToList();
            else
                return null;
        }

        /// <summary>
        /// Get task list about email unsent
        /// 获取未发送邮件通知的待办任务列表
        /// </summary>
        public IList<TaskViewEntity> GetTaskListEMailUnSent()
        {
            var taskManager = new TaskManager();
            var taskList = taskManager.GetTaskListEMailUnSent();
            return taskList;
        }
        #endregion
    }
}
