/*
* Slickflow 软件遵循自有项目开源协议，也可联系作者获取企业版商业授权和技术支持；
* 除此之外的使用则视为不正当使用，请您务必避免由此带来的一切商业版权纠纷和损失。
* 
The Slickflow Open License (SfPL 1.0)
Copyright (C) 2014  .NET Workflow Engine Library

1. Slickflow software must be legally used, and should not be used in violation of law, 
   morality and other acts that endanger social interests;
2. Non-transferable, non-transferable and indivisible authorization of this software;
3. The source code can be modified to apply Slickflow components in their own projects 
   or products, but Slickflow source code can not be separately encapsulated for sale or 
   distributed to third-party users;
4. The intellectual property rights of Slickflow software shall be protected by law, and
   no documents such as technical data shall be made public or sold.
5. The enterprise, ultimate and universe version can be provided with commercial license, 
   technical support and upgrade service.
*/

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using System.Threading;
using Slickflow.Data;
using Slickflow.Module.Resource;
using Slickflow.Engine.Core;
using Slickflow.Engine.Common;
using Slickflow.Engine.Storage;
using Slickflow.Engine.Utility;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Core.Result;
using Slickflow.Engine.Core.Event;
using Slickflow.Engine.Core.Runtime;

namespace Slickflow.Engine.Service
{
    /// <summary>
    /// 工作流服务(执行部分)
    /// </summary>
    public partial class WorkflowService : IWorkflowService
    {
        #region 基本属性
        /// <summary>
        /// 资源服务接口
        /// </summary>
        protected IResourceService ResourceService { get; private set; }
        #endregion

        #region 构造方法
        /// <summary>
        /// 构造方法
        /// </summary>
        public WorkflowService()
        {
            //设置当前数据为 ORACLE 
            //DBTypeExtenstions.SetDBType(DBTypeEnum.ORACLE, new OracleWfDataProvider());

            //资源接口组件
            ResourceService = ResourceServiceFactory.Create();
        }
      
        #endregion

        #region 流程定义数据
        /// <summary>
        /// 流程定义数据读取
        /// </summary>
        /// <param name="processGUID">流程定义GUID</param>
        /// <param name="version">版本号</param>
        /// <returns>流程</returns>
        public ProcessEntity GetProcessByVersion(string processGUID, string version = null)
        {
            var pm = new ProcessManager();
            var entity = pm.GetByVersion(processGUID, version);

            return entity;
        }

        /// <summary>
        /// 获取当前版本的流程定义记录
        /// </summary>
        /// <param name="processGUID">流程GUID</param>
        /// <returns>流程</returns>
        public ProcessEntity GetProcess(string processGUID)
        {
            return GetProcessByVersion(processGUID);
        }

        /// <summary>
        /// 获取流程定义数据
        /// </summary>
        /// <returns></returns>
        public IList<ProcessEntity> GetProcessList()
        {
            var pm = new ProcessManager();
            var list = pm.GetAll();

            return list;
        }

        /// <summary>
        /// 获取流程定义数据（只包括基本属性）
        /// </summary>
        /// <returns></returns>
        public IList<ProcessEntity> GetProcessListSimple()
        {
            var pm = new ProcessManager();
            var list = pm.GetListSimple();
            return list;
        }

        /// <summary>
        /// 流程定义的XML文件获取和保存
        /// </summary>
        /// <param name="processGUID">流程GUID</param>
        /// <param name="version">版本</param>
        /// <returns>流程文件</returns>
        public ProcessFileEntity GetProcessFile(string processGUID, string version)
        {
            var pm = new ProcessManager();
            var entity = pm.GetProcessFile(processGUID, version, XPDLStorageFactory.CreateXPDLStorage());

            return entity;
        }

        /// <summary>
        /// 保存流程定义的xml文件
        /// </summary>
        /// <param name="entity">流程文件实体</param>
        public void SaveProcessFile(ProcessFileEntity entity)
        {
            var pm = new ProcessManager();
            pm.SaveProcessFile(entity, XPDLStorageFactory.CreateXPDLStorage());
        }

        /// <summary>
        /// 创建流程定义记录
        /// </summary>
        /// <param name="entity">流程定义实体</param>
        /// <returns>新ID</returns>
        public int CreateProcess(ProcessEntity entity)
        {
            var pm = new ProcessManager();
            var processID = pm.CreateProcess(entity, XPDLStorageFactory.CreateXPDLStorage());

            return processID;
        }

        /// <summary>
        /// 创建流程定义记录新版本
        /// </summary>
        /// <param name="entity">流程</param>
        public int CreateProcessVersion(ProcessEntity entity)
        {
            var pm = new ProcessManager();
            return pm.CreateProcess(entity);
        }
        /// <summary>
        /// 更新流程定义记录
        /// </summary>
        /// <param name="entity">流程</param>
        public void UpdateProcess(ProcessEntity entity)
        {
            var processManager = new ProcessManager();
            processManager.Update(entity);
        }

        /// <summary>
        /// 删除流程定义记录
        /// </summary>
        /// <param name="processGUID">流程GUID</param>
        /// <param name="version">版本</param>
        public void DeleteProcess(string processGUID, string version)
        {
            var pm = new ProcessManager();
            pm.DeleteProcess(processGUID, version, XPDLStorageFactory.CreateXPDLStorage());
        }

		/// <summary>
        /// 删除流程定义记录
        /// </summary>
        /// <param name="processGUID">流程GUID</param>
        public void DeleteProcess(string processGUID)
        {
            
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();
                var processManager = new ProcessManager();
                processManager.Delete(session.Connection, processGUID, session.Transaction);
                session.Commit();
            }
            catch (System.Exception ex)
            {
                session.Rollback();
                throw;
            }
            finally
            {
                session.Dispose();
            }
        }
        #endregion

        #region 获取要运行节点(下一步)节点信息(正常流转运行)
        /// <summary>
        /// 获取流程的第一个可办理节点
        /// </summary>
        /// <param name="processGUID">流程定义GUID</param>
        /// <param name="version">版本</param>
        /// <returns>活动</returns>
        public ActivityEntity GetFirstActivity(string processGUID, string version)
        {
            var processModel = ProcessModelFactory.Create(processGUID, version);
            var firstActivity = processModel.GetFirstActivity();
            return firstActivity;
        }

        /// <summary>
        /// 获取当前节点的下一个节点信息
        /// </summary>
        /// <param name="processGUID">流程GUID</param>
        /// <param name="version">版本</param>
        /// <param name="activityGUID">活动GUID</param>
        /// <returns>活动</returns>
        public ActivityEntity GetNextActivity(string processGUID, 
            string version, 
            string activityGUID)
        {
            var processModel = ProcessModelFactory.Create(processGUID, version);
            var nextActivity = processModel.GetNextActivity(activityGUID);
            return nextActivity;
        }

        /// <summary>
        /// 获取当前节点的下一个节点信息
        /// </summary>
        /// <param name="processGUID">流程定义GUID</param>
        /// <param name="version">版本</param>
        /// <param name="activityGUID">活动定义GUID</param>
        /// <param name="condition">条件</param>
        /// <returns>节点视图</returns>
        public IList<NodeView> GetNextActivity(String processGUID,
            String version,
            String activityGUID,
            IDictionary<string, string> condition)
        {
            var processModel = ProcessModelFactory.Create(processGUID, version);
            var nextSteps = processModel.GetNextActivityTree(activityGUID, condition);
            return nextSteps;
        }

        /// <summary>
        /// 简单模式：根据应用获取流程下一步节点（不考虑有多个后续节点的情况）
        /// </summary>
        /// <param name="runner">运行者</param>
        /// <param name="condition">条件</param>
        /// <returns>节点视图</returns>
        public NodeView GetNextActivity(WfAppRunner runner,
            IDictionary<string, string> condition = null)
        {
            NodeView nodeView = null;
            var list = GetNextActivityTree(runner, condition).ToList();
            if (list != null && list.Count() > 0)
            {
                nodeView = list[0];
            }
            return nodeView;
        }

        /// <summary>
        /// 简单模式：根据应用获取流程下一步节点（不考虑有多个后续节点的情况）
        /// </summary>
        /// <param name="taskID">任务ID</param>
        /// <param name="condition">条件</param>
        /// <returns>获取下一步节点</returns>
        public NodeView GetNextActivity(int taskID,
            IDictionary<string, string> condition = null)
        {
            NodeView nodeView = null;
            var list = GetNextActivityTree(taskID, condition).ToList();
            if (list != null && list.Count() > 0)
            {
                nodeView = list[0];
            }
            return nodeView;
        }

        /// <summary>
        /// 根据应用获取流程下一步节点列表
        /// </summary>
        /// <param name="runner">应用执行人</param>
        /// <param name="condition">条件</param>
        /// <returns>节点列表</returns>
        public IList<NodeView> GetNextActivityTree(WfAppRunner runner, 
            IDictionary<string, string> condition = null)
        {
            var aim = new ActivityInstanceManager();
            var activityInstance = aim.GetActivityInstanceOfMine(runner.AppInstanceID, runner.ProcessGUID, runner.UserID);
            var processModel = ProcessModelFactory.Create(activityInstance.ProcessGUID, runner.Version);
            var nextSteps = processModel.GetNextActivityTree(activityInstance.ActivityGUID,
                condition);

            return nextSteps;
        }

        /// <summary>
        /// 根据应用获取流程下一步节点列表，包含角色用户
        /// </summary>
        /// <param name="runner">应用执行人</param>
        /// <param name="condition">条件</param>
        /// <returns>节点列表</returns>
        public IList<NodeView> GetNextActivityRoleUserTree(WfAppRunner runner, 
            IDictionary<string, string> condition = null)
        {
            var tm = new TaskManager();
            var taskView = tm.GetTaskOfMine(runner.AppInstanceID, runner.ProcessGUID, runner.UserID);
            var processModel = ProcessModelFactory.Create(taskView.ProcessGUID, taskView.Version);
            var nextSteps = processModel.GetNextActivityTree(taskView.ActivityGUID,
                condition);

            foreach (var ns in nextSteps)
            {
                if (ns.ReceiverType == ReceiverTypeEnum.ProcessInitiator)       //下一步执行人为流程发起人
                {
                    var pim = new ProcessInstanceManager();
                    ns.Users = AppendUserList(ns.Users, pim.GetProcessInitiator(taskView.ProcessInstanceID));   //获取流程发起人
                }
                else
                {
                    var roleIDs = ns.Roles.Select(x => x.ID).ToArray();
                    ns.Users = ResourceService.GetUserListByRoleReceiverType(roleIDs, runner.UserID, (int)ns.ReceiverType);     //增加转移前置过滤条件
                }
            }
            return nextSteps;
        }

        /// <summary>
        /// 流程启动时获取开始节点下一步的节点角色人员列表
        /// </summary>
        /// <param name="runner">应用执行人</param>
        /// <param name="condition">条件</param>
        /// <returns>节点列表</returns>
        public IList<NodeView> GetFirstActivityRoleUserTree(WfAppRunner runner, IDictionary<string, string> condition = null)
        {
            var processModel = ProcessModelFactory.Create(runner.ProcessGUID, runner.Version);
            var firstActivity = processModel.GetFirstActivity();
            var nextSteps = processModel.GetNextActivityTree(firstActivity.ActivityGUID,
                condition);

            foreach (var ns in nextSteps)
            {
                var roleIDs = ns.Roles.Select(x => x.ID).ToArray();
                ns.Users = ResourceService.GetUserListByRoleReceiverType(roleIDs, runner.UserID, (int)ns.ReceiverType);     //增加转移前置过滤条件
            }
            return nextSteps;
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
        /// 获取下一步活动列表树
        /// </summary>
        /// <param name="taskID">任务ID</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public IList<NodeView> GetNextActivityTree(int taskID, 
            IDictionary<string, string> condition = null)
        {
            var taskView = (new TaskManager()).GetTaskView(taskID);
            var processModel = ProcessModelFactory.Create(taskView.ProcessGUID, taskView.Version);
            var nextSteps = processModel.GetNextActivityTree(taskView.ActivityGUID, 
                condition);

            return nextSteps;
        }

        /// <summary>
        /// 获取已经完成的节点
        /// </summary>
        /// <param name="taskID">任务ID</param>
        /// <returns>节点列表</returns>
        public IList<NodeImage> GetActivityInstanceCompleted(int taskID)
        {
            IList<NodeImage> imageList = new List<NodeImage>();

            var tm = new TaskManager();
            var task = tm.GetTaskView(taskID);

            var am = new ActivityInstanceManager();
            var list = am.GetCompletedActivityInstanceList(task.AppInstanceID, task.ProcessGUID);

            foreach (ActivityInstanceEntity a in list)
            {
                imageList.Add(new NodeImage
                {
                    ID = a.ID,
                    ActivityName = a.ActivityName
                });
            }
            return imageList;
        }

        /// <summary>
        /// 获取已经完成的节点记录
        /// </summary>
        /// <param name="runner">运行者</param>
        /// <returns>节点列表</returns>
        public IList<NodeImage> GetActivityInstanceCompleted(WfAppRunner runner)
        {
            IList<NodeImage> imageList = new List<NodeImage>();
            var am = new ActivityInstanceManager();
            var list = am.GetCompletedActivityInstanceList(runner.AppInstanceID, runner.ProcessGUID);

            foreach (ActivityInstanceEntity a in list)
            {
                imageList.Add(new NodeImage 
                { 
                    ID = a.ID, 
                    ActivityName = a.ActivityName 
                });
            }
            return imageList;
        }

        /// <summary>
        /// 获取转移实例记录
        /// </summary>
        /// <param name="query">查询实体</param>
        /// <returns>转移列表</returns>
        public IList<TransitionImage> GetTransitionInstanceList(TransitionInstanceQuery query)
        {
            IList<TransitionImage> imageList = new List<TransitionImage>();
            var tm = new TransitionInstanceManager();
            var list = tm.GetTransitionInstanceList(query.AppInstanceID, query.ProcessGUID).ToList();

            foreach (TransitionInstanceEntity t in list)
            {
                imageList.Add(new TransitionImage
                {
                    ID = t.ID,
                    TransitionGUID = t.TransitionGUID
                });
            }

            return imageList;
        }

        /// <summary>
        /// 获取当前活动实体
        /// </summary>
        /// <param name="processGUID">流程定义GUID</param>
        /// <param name="version">版本</param>
        /// <param name="activityGUID">活动GUID</param>
        /// <returns>活动</returns>
        public ActivityEntity GetActivityEntity(string processGUID, string version, string activityGUID)
        {
            var processModel = ProcessModelFactory.Create(processGUID, version);
            return processModel.GetActivity(activityGUID);
        }

        /// <summary>
        /// 获取活动节点下的角色数据
        /// </summary>
        /// <param name="processGUID">流程GUID</param>
        /// <param name="version">版本</param>
        /// <param name="activityGUID">活动GUID</param>
        /// <returns>角色列表</returns>
        public IList<Role> GetActivityRoles(string processGUID, string version, string activityGUID)
        {
            var processModel = ProcessModelFactory.Create(processGUID, version);
            return processModel.GetActivityRoles(activityGUID);
        }
        #endregion

        #region 流程启动
        /// <summary>
        /// 流程启动
        /// 编码示例：
        /// var wfResult = wfService.CreateRunner(runner.UserID, runner.UserName)
        ///                 .UseApp(runner.AppInstanceID, runner.AppName, runner.AppInstanceCode)
        ///                 .UseProcess(runner.ProcessGUID, runner.Version)
        ///                 .Start();
        /// </summary>
        /// <returns>执行结果</returns>
        public WfExecutedResult Start()
        {
            IDbConnection conn = SessionFactory.CreateConnection();
            IDbTransaction trans = null;
            try
            {
                trans = conn.BeginTransaction();
                var result = Start(conn, trans);
                if (result.Status == WfExecutedStatus.Success)
                {
                    trans.Commit();
                }
                else
                    trans.Rollback();

                return result;
            }
            catch
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        /// <summary>
        /// 启动流程
        /// 编码示例：
        /// var wfResult = wfService.CreateRunner(runner.UserID, runner.UserName)
        ///                 .UseApp(runner.AppInstanceID, runner.AppName, runner.AppInstanceCode)
        ///                 .UseProcess(runner.ProcessGUID, runner.Version)
        ///                 .Start(conn, trans);
        /// </summary>
        /// <param name="conn">连接</param>
        /// <param name="trans">事务</param>
        /// <returns>启动结果</returns>
        public WfExecutedResult Start(IDbConnection conn, IDbTransaction trans)
        {
            var runner = _wfAppRunner;
            WfExecutedResult startedResult = WfExecutedResult.Default();
            AutoResetEvent waitHandler = new AutoResetEvent(false);

            //启动方法执行
            WfRuntimeManager runtimeInstance = null;
            IDbSession session = null;
            try
            {
                session = SessionFactory.CreateSession(conn, trans);
                runtimeInstance = WfRuntimeManagerFactory.CreateRuntimeInstanceStartup(runner, ref startedResult);

                if (startedResult.Status == WfExecutedStatus.Exception)
                {
                    return startedResult;
                }

                //绑定事件
                runtimeInstance.RegisterEvent(runtimeInstance_OnWfProcessStarting, runtimeInstance_OnWfProcessStarted);
                bool isStarted = runtimeInstance.Execute(session);

                //do some thing else here...
                //...
               waitHandler.WaitOne();
            }
            catch (System.Exception e)
            {
                startedResult.Status = WfExecutedStatus.Failed;
                startedResult.Message = string.Format("流程启动发生错误，内部异常:{0}", e.Message);
                LogManager.RecordLog(WfDefine.WF_PROCESS_START_ERROR, LogEventType.Error, LogPriority.High, runner, e);
            }
            finally
            {
                //卸载事件
                runtimeInstance.UnRegiesterEvent(runtimeInstance_OnWfProcessStarting, 
                   runtimeInstance_OnWfProcessStarted);
                waitHandler.Dispose();
            }
            return startedResult;

            void runtimeInstance_OnWfProcessStarting(object sender, WfEventArgs args)
            {
                Delegate.DelegateExecutor.InvokeExternalDelegate(session,
                        Delegate.EventFireTypeEnum.OnProcessStarting,
                        runner.DelegateEventList,
                        startedResult.ProcessInstanceIDStarted);
            }

            void runtimeInstance_OnWfProcessStarted(object sender, WfEventArgs args)
            {
                startedResult = args.WfExecutedResult;
                if (startedResult.Status == WfExecutedStatus.Success)
                {
                    Delegate.DelegateExecutor.InvokeExternalDelegate(session, 
                        Delegate.EventFireTypeEnum.OnProcessStarted,
                        runner.DelegateEventList,
                        startedResult.ProcessInstanceIDStarted);
                }
                waitHandler.Set();
            }
        }

        /// <summary>
        /// 启动流程
        /// </summary>
        /// <param name="starter">启动人</param>
        /// <returns>启动结果</returns>
        public WfExecutedResult StartProcess(WfAppRunner starter)
        {
            _wfAppRunner = starter;
            var result = Start();

            return result;
        }

        /// <summary>
        /// 流程启动
        /// </summary>
        /// <param name="conn">连接</param>
        /// <param name="starter">启动人</param>
        /// <param name="trans">事务</param>
        /// <returns>启动结果</returns>
        public WfExecutedResult StartProcess(IDbConnection conn, WfAppRunner starter, IDbTransaction trans)
        {
            _wfAppRunner = starter;
            var result = Start(conn, trans);

            return result;
        }
        #endregion

        #region 运行流程
        /// <summary>
        /// 运行流程(业务处理)
        /// 编码示例：
        /// var wfResult = wfService.CreateRunner(runner.UserID, runner.UserName)
        ///                 .UseApp(runner.AppInstanceID, runner.AppName, runner.AppInstanceCode)
        ///                 .UseProcess(runner.ProcessGUID, runner.Version)
        ///                 .Run();
        /// </summary>
        /// <param name="runner">运行人</param>
        /// <returns>运行结果</returns>
        public WfExecutedResult Run()
        {
            IDbConnection conn = SessionFactory.CreateConnection();
            IDbTransaction trans = null;
            try
            {
                trans = conn.BeginTransaction();
                var result = Run(conn, trans);
                if (result.Status == WfExecutedStatus.Success)
                    trans.Commit();
                else
                    trans.Rollback();

                return result;
            }
            catch (System.Exception ex)
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        /// <summary>
        /// 运行流程
        /// 编码示例：
        /// var wfResult = wfService.CreateRunner(runner.UserID, runner.UserName)
        ///                 .UseApp(runner.AppInstanceID, runner.AppName, runner.AppInstanceCode)
        ///                 .UseProcess(runner.ProcessGUID, runner.Version)
        ///                 .Run(conn, trans);
        /// </summary>
        /// <param name="conn">连接</param>
        /// <param name="trans">事务</param>
        /// <returns>运行结果</returns>
        public WfExecutedResult Run(IDbConnection conn, 
            IDbTransaction trans)
        {
            var runner = _wfAppRunner;
            WfExecutedResult runAppResult = WfExecutedResult.Default();
            AutoResetEvent waitHandler = new AutoResetEvent(false);

            //运行方法开始执行
            WfRuntimeManager runtimeInstance = null;
            IDbSession session = null;
            try
            {
                session = SessionFactory.CreateSession(conn, trans);
                runtimeInstance = WfRuntimeManagerFactory.CreateRuntimeInstanceAppRunning(runner, ref runAppResult);
                if (runAppResult.Status == WfExecutedStatus.Exception)
                {
                    return runAppResult;
                }

                //注册事件并运行
                runtimeInstance.RegisterEvent(runtimeInstance_OnWfProcessRunning,
                    runtimeInstance_OnWfProcessContinued);
                bool isRun = runtimeInstance.Execute(session);

                //do some thing else here...
                //...

                waitHandler.WaitOne();
            }
            catch (System.Exception e)
            {
                runAppResult.Status = WfExecutedStatus.Failed;
                var error = e.InnerException != null ? e.InnerException.Message : e.Message;
                runAppResult.Message = string.Format("流程运行时发生异常！，详细错误：{0}", error);
                LogManager.RecordLog(WfDefine.WF_PROCESS_RUN_ERROR, LogEventType.Error, LogPriority.High, runner, e);
            }
            finally
            {
                //卸载事件
                if (runtimeInstance != null)
                {
                    runtimeInstance.UnRegiesterEvent(runtimeInstance_OnWfProcessRunning,
                        runtimeInstance_OnWfProcessContinued);
                }

                waitHandler.Dispose();
            }
            return runAppResult;

            void runtimeInstance_OnWfProcessRunning(object sender, WfEventArgs args)
            {
                Delegate.DelegateExecutor.InvokeExternalDelegate(session,
                    Delegate.EventFireTypeEnum.OnProcessRunning,
                    runner.DelegateEventList,
                    runtimeInstance.RunningActivityInstance.ProcessInstanceID); 
            }

            void runtimeInstance_OnWfProcessContinued(object sender, WfEventArgs args)
            {
                runAppResult = args.WfExecutedResult;
                if (runAppResult.Status == WfExecutedStatus.Success)
                {
                    Delegate.DelegateExecutor.InvokeExternalDelegate(session,
                        Delegate.EventFireTypeEnum.OnProcessContinued,
                        runner.DelegateEventList,
                        runtimeInstance.RunningActivityInstance.ProcessInstanceID);
                }
                waitHandler.Set();
            }
        }

        /// <summary>
        /// 流程流转
        /// </summary>
        /// <returns>执行结果</returns>
        public WfExecutedResult RunProcessApp(WfAppRunner runner)
        {
            _wfAppRunner = runner;
            var result = Run();

            return result;
        }

        /// <summary>
        /// 流程流转
        /// </summary>
        /// <param name="conn">连接</param>
        /// <param name="runner">运行人</param>
        /// <param name="trans">事务</param>
        /// <returns>执行结果</returns>
        public WfExecutedResult RunProcessApp(IDbConnection conn, WfAppRunner runner, IDbTransaction trans)
        {
            _wfAppRunner = runner;
            var result = Run(conn, trans);

            return result;
        }
        #endregion

        #region 流程撤销
        /// <summary>
        /// 流程撤销
        /// </summary>
        /// <returns>执行结果</returns>
        public WfExecutedResult Withdraw()
        {
            IDbConnection conn = SessionFactory.CreateConnection();
            IDbTransaction trans = null;
            try
            {
                trans = conn.BeginTransaction();
                var result = Withdraw(conn, trans);

                if (result.Status == WfExecutedStatus.Success)
                    trans.Commit();
                else
                    trans.Rollback();

                return result;
            }
            catch
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        /// <summary>
        /// 流程撤销
        /// </summary>
        /// <param name="conn">连接</param>
        /// <param name="trans">事务</param>
        /// <returns>执行结果</returns>
        public WfExecutedResult Withdraw(IDbConnection conn, IDbTransaction trans)
        {
            var runner = _wfAppRunner;
            WfExecutedResult withdrawedResult = WfExecutedResult.Default();
            AutoResetEvent waitHandler = new AutoResetEvent(false);

            //撤销方法开始执行
            WfRuntimeManager runtimeInstance = null;
            IDbSession session = null;
            try
            {
                session = SessionFactory.CreateSession(conn, trans);
                runtimeInstance = WfRuntimeManagerFactory.CreateRuntimeInstanceWithdraw(runner, ref withdrawedResult);

                //不满足撤销操作，返回异常结果信息
                if (withdrawedResult.Status == WfExecutedStatus.Exception)
                {
                    return withdrawedResult;
                }
                //注册事件并运行
                runtimeInstance.RegisterEvent(runtimeInstance_OnWfProcessWithdrawing,
                    runtimeInstance_OnWfProcessWithdrawn);
                bool isWithdrawn = runtimeInstance.Execute(session);

                waitHandler.WaitOne();
            }
            catch (System.Exception e)
            {
                withdrawedResult.Status = WfExecutedStatus.Failed;
                var error = e.InnerException != null ? e.InnerException.Message : e.Message;
                withdrawedResult.Message = string.Format("流程撤销发生异常！，详细错误：{0}", error);
                LogManager.RecordLog(WfDefine.WF_PROCESS_WITHDRAW_ERROR, LogEventType.Error, LogPriority.High, runner, e);
            }
            finally
            {
                //卸载事件
                runtimeInstance.UnRegiesterEvent(runtimeInstance_OnWfProcessWithdrawing,
                    runtimeInstance_OnWfProcessWithdrawn);
                waitHandler.Dispose();
            }
            return withdrawedResult;

            void runtimeInstance_OnWfProcessWithdrawing(object sender, WfEventArgs args)
            {
                Delegate.DelegateExecutor.InvokeExternalDelegate(session,
                    Delegate.EventFireTypeEnum.OnProcessWithdrawing,
                    runner.DelegateEventList,
                    runtimeInstance.RunningActivityInstance.ProcessInstanceID);
            }

            void runtimeInstance_OnWfProcessWithdrawn(object sender, WfEventArgs args)
            {
                withdrawedResult = args.WfExecutedResult;
                if (withdrawedResult.Status == WfExecutedStatus.Success)
                {
                    Delegate.DelegateExecutor.InvokeExternalDelegate(session,
                        Delegate.EventFireTypeEnum.OnProcessWithdrawn,
                        runner.DelegateEventList,
                        runtimeInstance.RunningActivityInstance.ProcessInstanceID);
                }
                waitHandler.Set();
            }
        }

        /// <summary>
        /// 流程撤销
        /// </summary>
        /// <param name="runner">撤销人</param>
        /// <returns>撤销结果</returns>
        public WfExecutedResult WithdrawProcess(WfAppRunner runner)
        {
            _wfAppRunner = runner;
            var result = Withdraw();

            return result;
        }

        /// <summary>
        /// 撤销流程
        /// </summary>
        /// <param name="conn">连接</param>
        /// <param name="runner">撤销人</param>
        /// <param name="trans">事务</param>
        /// <returns>撤销结果</returns>
        public WfExecutedResult WithdrawProcess(IDbConnection conn, 
            WfAppRunner runner, 
            IDbTransaction trans)
        {
            _wfAppRunner = runner;
            var result = Withdraw(conn, trans);

            return result;
        }
        #endregion

        #region 流程回退
        /// <summary>
        /// 流程退回
        /// </summary>
        /// <returns>执行结果</returns>
        public WfExecutedResult SendBack()
        {
            IDbConnection conn = SessionFactory.CreateConnection();
            IDbTransaction trans = null;
            try
            {
                trans = conn.BeginTransaction();
                var result = SendBack(conn, trans);
                if (result.Status == WfExecutedStatus.Success)
                    trans.Commit();
                else
                    trans.Rollback();

                return result;
            }
            catch
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        /// <summary>
        /// 流程退回
        /// </summary>
        /// <param name="conn">连接</param>
        /// <param name="trans">事务</param>
        /// <returns>执行结果</returns>
        public WfExecutedResult SendBack(IDbConnection conn, 
            IDbTransaction trans)
        {
            var runner = _wfAppRunner;
            WfExecutedResult sendbackResult = WfExecutedResult.Default();
            AutoResetEvent waitHandler = new AutoResetEvent(false);

            //退回开始
            WfRuntimeManager runtimeInstance = null;
            IDbSession session = null;
            try
            {
                session = SessionFactory.CreateSession(conn, trans);
                runtimeInstance = WfRuntimeManagerFactory.CreateRuntimeInstanceSendBack(runner, ref sendbackResult);

                if (sendbackResult.Status == WfExecutedStatus.Exception)
                {
                    return sendbackResult;
                }

                //注册事件并运行
                runtimeInstance.RegisterEvent(runtimeInstance_OnWfProcessSendBacking,
                    runtimeInstance_OnWfProcessSendBacked);
                bool isSendBacked = runtimeInstance.Execute(session);

                waitHandler.WaitOne();
            }
            catch (System.Exception e)
            {
                sendbackResult.Status = WfExecutedStatus.Failed;
                var error = e.InnerException != null ? e.InnerException.Message : e.Message;
                sendbackResult.Message = string.Format("流程退回发生异常！，详细错误：{0}", error);
                LogManager.RecordLog(WfDefine.WF_PROCESS_SENDBACK_ERROR, LogEventType.Error, LogPriority.High, runner, e);
            }
            finally
            {
                //卸载事件
                runtimeInstance.UnRegiesterEvent(runtimeInstance_OnWfProcessSendBacking,
                    runtimeInstance_OnWfProcessSendBacked);
                waitHandler.Dispose();
            }
            return sendbackResult;


            void runtimeInstance_OnWfProcessSendBacking(object sender, WfEventArgs args)
            {
                Delegate.DelegateExecutor.InvokeExternalDelegate(session,
                        Delegate.EventFireTypeEnum.OnProcessSendBacking,
                        runner.DelegateEventList,
                        runtimeInstance.RunningActivityInstance.ProcessInstanceID);
            }

            void runtimeInstance_OnWfProcessSendBacked(object sender, WfEventArgs args)
            {
                sendbackResult = args.WfExecutedResult;
                if (sendbackResult.Status == WfExecutedStatus.Success)
                {
                    Delegate.DelegateExecutor.InvokeExternalDelegate(session,
                        Delegate.EventFireTypeEnum.OnProcessSendBacked,
                        runner.DelegateEventList,
                        runtimeInstance.RunningActivityInstance.ProcessInstanceID);
                }
                waitHandler.Set();
            }
        }

        /// <summary>
        /// 退回到上一步
        /// </summary>
        /// <param name="runner">退回操作人</param>
        /// <returns>退回结果</returns>
        public WfExecutedResult SendBackProcess(WfAppRunner runner)
        {
            _wfAppRunner = runner;
            var result = SendBack();

            return result;
        }

        /// <summary>
        /// 退回到上一步
        /// </summary>
        /// <param name="conn">连接</param>
        /// <param name="runner">退回人</param>
        /// <param name="trans">事务</param>
        /// <returns>退回结果</returns>
        public WfExecutedResult SendBackProcess(IDbConnection conn, 
            WfAppRunner runner, 
            IDbTransaction trans)
        {
            _wfAppRunner = runner;
            var result = SendBack(conn, trans);

            return result;
        }
        #endregion

        #region 流程返签（已经结束的流程可以被复活）
        /// <summary>
        /// 流程返签
        /// </summary>
        /// <returns>执行结果</returns>
        public WfExecutedResult Reverse()
        {
            IDbConnection conn = SessionFactory.CreateConnection();
            IDbTransaction trans = null;
            try
            {
                trans = conn.BeginTransaction();
                var result = Reverse(conn, trans);
                if (result.Status == WfExecutedStatus.Success)
                    trans.Commit();
                else
                    trans.Rollback();

                return result;
            }
            catch
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        /// <summary>
        /// 流程返签
        /// </summary>
        /// <param name="conn">连接</param>
        /// <param name="trans">事务</param>
        /// <returns>执行结果</returns>
        public WfExecutedResult Reverse(IDbConnection conn, IDbTransaction trans)
        {
            var runner = _wfAppRunner;
            WfExecutedResult reversedResult = WfExecutedResult.Default();
            AutoResetEvent waitHandler = new AutoResetEvent(false);

            //返签方法开始执行
            WfRuntimeManager runtimeInstance = null;
            IDbSession session = null;
            try
            {
                session = SessionFactory.CreateSession(conn, trans);
                runtimeInstance = WfRuntimeManagerFactory.CreateRuntimeInstanceReverse(runner, ref reversedResult);

                if (reversedResult.Status == WfExecutedStatus.Exception)
                {
                    return reversedResult;
                }

                //注册事件并运行
                runtimeInstance.RegisterEvent(runtimeInstance_OnWfProcessReversing,
                    runtimeInstance_OnWfProcessReversed);
                bool isReversed = runtimeInstance.Execute(session);

                waitHandler.WaitOne();
            }
            catch (System.Exception e)
            {
                reversedResult.Status = WfExecutedStatus.Failed;
                var error = e.InnerException != null ? e.InnerException.Message : e.Message;
                reversedResult.Message = string.Format("流程返签时发生异常！，详细错误：{0}", error);
                LogManager.RecordLog(WfDefine.WF_PROCESS_REVERSE_ERROR, LogEventType.Error, LogPriority.High, runner, e);
            }
            finally
            {
                //卸载事件
                runtimeInstance.UnRegiesterEvent(runtimeInstance_OnWfProcessReversing,
                    runtimeInstance_OnWfProcessReversed);
                waitHandler.Dispose();
            }
            return reversedResult;

            void runtimeInstance_OnWfProcessReversing(object sender, WfEventArgs args)
            {
                Delegate.DelegateExecutor.InvokeExternalDelegate(session,
                    Delegate.EventFireTypeEnum.OnProcessReversing,
                    runner.DelegateEventList,
                    runtimeInstance.RunningActivityInstance.ProcessInstanceID);
            }

            void runtimeInstance_OnWfProcessReversed(object sender, WfEventArgs args)
            {
                reversedResult = args.WfExecutedResult;
                if (reversedResult.Status == WfExecutedStatus.Success)
                {
                    Delegate.DelegateExecutor.InvokeExternalDelegate(session,
                        Delegate.EventFireTypeEnum.OnProcessReversed,
                        runner.DelegateEventList,
                        runtimeInstance.RunningActivityInstance.ProcessInstanceID);
                }
                waitHandler.Set();
            }
        }

        /// <summary>
        /// 流程返签
        /// </summary>
        /// <param name="runner">结束人</param>
        /// <returns>返签结果</returns>
        public WfExecutedResult ReverseProcess(WfAppRunner runner)
        {
            _wfAppRunner = runner;
            var result = Reverse();

            return result;
        }

        /// <summary>
        /// 流程返签
        /// </summary>
        /// <param name="conn">连接</param>
        /// <param name="runner">结束人</param>
        /// <param name="trans">事务</param>
        /// <returns>返签结果</returns>
        public WfExecutedResult ReverseProcess(IDbConnection conn, 
            WfAppRunner runner, 
            IDbTransaction trans)
        {
            _wfAppRunner = runner;
            var result = Reverse(conn, trans);

            return result;
        }
        #endregion

        #region 流程跳转
        /// <summary>
        /// 流程跳转
        /// </summary>
        /// <param name="jumpOption">跳转类型</param>
        /// <returns>执行结果</returns>
        public WfExecutedResult Jump(JumpOptionEnum jumpOption = JumpOptionEnum.Default)
        {
            IDbConnection conn = SessionFactory.CreateConnection();
            IDbTransaction trans = null;
            try
            {
                trans = conn.BeginTransaction();
                var result = Jump(conn, trans, jumpOption);
                if (result.Status == WfExecutedStatus.Success)
                    trans.Commit();
                else
                    trans.Rollback();

                return result;
            }
            catch
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        /// <summary>
        /// 流程跳转
        /// </summary>
        /// <param name="conn">连接</param>
        /// <param name="trans">事务</param>
        /// <param name="jumpOption">跳转类型</param>
        /// <returns>执行结果</returns>
        public WfExecutedResult Jump(IDbConnection conn,
            IDbTransaction trans,
            JumpOptionEnum jumpOption = JumpOptionEnum.Default)
        {
            var runner = _wfAppRunner;
            WfExecutedResult jumpResult = WfExecutedResult.Default();
            AutoResetEvent waitHandler = new AutoResetEvent(false);

            //跳转方法开始执行
            WfRuntimeManager runtimeInstance = null;
            IDbSession session = null;
            try
            {
                runner.NextActivityPerformers = FillNextActivityPerforms(runner, jumpOption);
                session = SessionFactory.CreateSession(conn, trans);
                runtimeInstance = WfRuntimeManagerFactory.CreateRuntimeInstanceJump(runner, ref jumpResult);

                if (jumpResult.Status == WfExecutedStatus.Exception)
                {
                    return jumpResult;
                }

                //注册事件并运行
                runtimeInstance.RegisterEvent(runtimeInstance_OnWfProcessJumping,
                    runtimeInstance_OnWfProcessJumped);          
                bool isJumped = runtimeInstance.Execute(session);

                waitHandler.WaitOne();
            }
            catch (System.Exception e)
            {
                jumpResult.ExceptionType = WfExceptionType.Jump_OtherError;
                var error = e.InnerException != null ? e.InnerException.Message : e.Message;
                jumpResult.Message = string.Format("流程跳转时发生异常！，详细错误：{0}", error);
                LogManager.RecordLog(WfDefine.WF_PROCESS_JUMP_ERROR, LogEventType.Error, LogPriority.High, runner, e);
            }
            finally
            {
                //卸载事件
                runtimeInstance.UnRegiesterEvent(runtimeInstance_OnWfProcessJumping,
                    runtimeInstance_OnWfProcessJumped);
                waitHandler.Dispose();
            }
            return jumpResult;

            void runtimeInstance_OnWfProcessJumping(object sender, WfEventArgs args)
            {
                Delegate.DelegateExecutor.InvokeExternalDelegate(session,
                    Delegate.EventFireTypeEnum.OnProcessJumping,
                    runner.DelegateEventList,
                    runtimeInstance.RunningActivityInstance.ProcessInstanceID);
            }

            void runtimeInstance_OnWfProcessJumped(object sender, WfEventArgs args)
            {
                jumpResult = args.WfExecutedResult;
                if (jumpResult.Status == WfExecutedStatus.Success)
                {
                    Delegate.DelegateExecutor.InvokeExternalDelegate(session,
                        Delegate.EventFireTypeEnum.OnProcessJumped,
                        runner.DelegateEventList,
                        runtimeInstance.RunningActivityInstance.ProcessInstanceID);
                }
                waitHandler.Set();
            }
        }

        /// <summary>
        /// 流程跳转
        /// </summary>
        /// <param name="runner">执行操作人</param>
        /// <param name="jumpOption">跳转选项</param>
        /// <returns>跳转结果</returns>
        public WfExecutedResult JumpProcess(WfAppRunner runner, 
            JumpOptionEnum jumpOption = JumpOptionEnum.Default)
        {
            _wfAppRunner = runner;
            var result = Jump(jumpOption);

            return result;
        }

        /// <summary>
        /// 流程跳转
        /// </summary>
        /// <param name="conn">连接</param>
        /// <param name="runner">执行操作人</param>
        /// <param name="trans">事务</param>
        /// <param name="jumpOption">跳转选项</param>
        /// <returns>跳转结果</returns>
        public WfExecutedResult JumpProcess(IDbConnection conn,
            WfAppRunner runner,
            IDbTransaction trans,
            JumpOptionEnum jumpOption = JumpOptionEnum.Default)
        {
            _wfAppRunner = runner;
            var result = Jump(conn, trans, jumpOption);

            return result;
        }

        /// <summary>
        /// 重新生成跳转活动的执行人员列表
        /// </summary>
        /// <param name="runner">当前运行用户</param>
        /// <param name="jumpOption">跳转选项</param>
        /// <returns>执行人员列表</returns>
        private IDictionary<string, PerformerList> FillNextActivityPerforms(WfAppRunner runner,
            JumpOptionEnum jumpOption)
        {
            IDictionary<string, PerformerList> nextActivityPerformers = null;
            var pm = ProcessModelFactory.Create(runner.ProcessGUID, runner.Version);
            if (jumpOption == JumpOptionEnum.Default)
            {
                nextActivityPerformers = runner.NextActivityPerformers;
            }
            else if (jumpOption == JumpOptionEnum.Startup)
            {
                var firstActivity = pm.GetFirstActivity();
                var aim = new ActivityInstanceManager();
                var firstActivityInstance = aim.GetActivityInstanceLatest(runner.AppInstanceID, runner.ProcessGUID, firstActivity.ActivityGUID);
                nextActivityPerformers = new Dictionary<string, PerformerList>();
                var performer = new Performer(firstActivityInstance.CreatedByUserID, firstActivityInstance.CreatedByUserName);
                var performerList = new PerformerList();
                performerList.Add(performer);
                nextActivityPerformers.Add(firstActivity.ActivityGUID, performerList);
            }
            else if (jumpOption == JumpOptionEnum.End)
            {
                var endActivity = pm.GetEndActivity();
                nextActivityPerformers = new Dictionary<string, PerformerList>();
                var performer = new Performer(runner.UserID, runner.UserName);
                var performerList = new PerformerList();
                performerList.Add(performer);
                nextActivityPerformers.Add(endActivity.ActivityGUID, performerList);
            }
            return nextActivityPerformers;
        }
        #endregion

        #region 挂起（恢复）流程成、取消（运行的）流程、废弃执行中或执行完的流程
        /// <summary>
        /// 恢复流程实例（只针对挂起操作）
        /// </summary>
        /// <param name="processInstanceId">挂起操作的实例ID</param>
        /// <param name="runner">执行者</param>
        /// <returns></returns>
        public bool ResumeProcess(int processInstanceId, WfAppRunner runner)
        {
            bool result = true;
            try
            {
                var pim = new ProcessInstanceManager();
                pim.Resume(processInstanceId, runner, SessionFactory.CreateSession());
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }
        /// <summary>
        /// 挂起流程实例
        /// </summary>
        /// <param name="taskId">任务ID</param>
        /// <param name="runner">执行者</param>
        /// <returns></returns>
        public bool SuspendProcess(int taskId, WfAppRunner runner)
        {
            bool result = true;
            try
            {
                var pim = new ProcessInstanceManager();
                var taskMng = new TaskManager();
                TaskEntity taskentity = taskMng.GetTask(taskId);
                pim.Suspend(taskentity.ProcessInstanceID, runner, SessionFactory.CreateSession());
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// 取消流程
        /// </summary>
        /// <param name="runner">执行操作的用户</param>
        /// <returns>执行结果的标志</returns>
        public bool CancelProcess(WfAppRunner runner)
        {
            var pim = new ProcessInstanceManager();
            return pim.Cancel(runner);
        }

        /// <summary>
        /// 废弃流程
        /// </summary>
        /// <param name="runner">执行操作的用户</param>
        /// <returns>执行结果的标志</returns>
        public bool DiscardProcess(WfAppRunner runner)
        {
            var pim = new ProcessInstanceManager();
            return pim.Discard(runner);
        }

        /// <summary>
        /// 终结流程
        /// </summary>
        /// <param name="runner">执行操作的用户</param>
        /// <returns>执行结果的标志</returns>
        public bool TerminateProcess(WfAppRunner terminator)
        {
            var pim = new ProcessInstanceManager();
            return pim.Terminate(terminator);
        }
        #endregion

        #region 任务读取和处理
        /// <summary>
        /// 设置任务为已读状态(根据任务ID获取任务)
        /// </summary>
        /// <param name="runner">执行人</param>
        /// <returns>任务读取的标志</returns>
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
        /// 获取运行中的任务
        /// </summary>
        /// <param name="query">查询实体</param>
        /// <returns>任务列表</returns>
        public IList<TaskViewEntity> GetRunningTasks(TaskQuery query)
        {
            int allRowsCount = 0;
            var taskManager = new TaskManager();
            var taskList = taskManager.GetRunningTasks(query, out allRowsCount);
            if (taskList != null)
                return taskList.ToList();
            else
                return null;
        }

        /// <summary>
        /// 获取待办任务
        /// </summary>
        /// <param name="query">查询对象</param>
        /// <returns>任务列表</returns>
        public IList<TaskViewEntity> GetReadyTasks(TaskQuery query)
        {
            int allRowsCount = 0;
            var taskManager = new TaskManager();
            var taskList = taskManager.GetReadyTasks(query, out allRowsCount);

            if (taskList != null)
                return taskList.ToList();
            else
                return null;
        }

        /// <summary>
        /// 获取办结任务列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<TaskViewEntity> GetCompletedTasks(TaskQuery query)
        {
            int allRowsCount = 0;
            var taskManager = new TaskManager();
            var taskList = taskManager.GetCompletedTasks(query, out allRowsCount);

            if (taskList != null)
                return taskList.ToList();
            else
                return null;
        }
        #endregion 
    }
}