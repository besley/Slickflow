/*
* Slickflow 工作流引擎遵循LGPL协议，也可联系作者商业授权并获取技术支持；
* 除此之外的使用则视为不正当使用，请您务必避免由此带来的商业版权纠纷。
* 
The Slickflow project.
Copyright (C) 2014  .NET Workflow Engine Library

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, you can access the official
web page about lgpl: https://www.gnu.org/licenses/lgpl.html
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Slickflow.Data;
using Slickflow.Module.Resource;
using Slickflow.Module.Resource.Service;
using Slickflow.Engine.Common;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Business.Data;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Core.Result;
using Slickflow.Engine.Core.Event;
using Slickflow.Engine.Core.Runtime;
//using Slickflow.Data.OracleProvider;

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
        /// 获取流程定义文件
        /// </summary>
        /// <param name="id">流程ID</param>
        /// <returns></returns>
        public ProcessFileEntity GetProcessFileByID(int id)
        {
            var pm = new ProcessManager();
            var entity = pm.GetProcessFileByID(id);

            return entity;
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
        /// 流程定义的XML文件获取
        /// </summary>
        /// <param name="processGUID">流程GUID</param>
        /// <param name="version">版本</param>
        /// <returns>流程文件</returns>
        public ProcessFileEntity GetProcessFile(string processGUID, string version)
        {
            var pm = new ProcessManager();
            var entity = pm.GetProcessFile(processGUID, version);

            return entity;
        }

        /// <summary>
        /// 保存流程定义的xml文件
        /// </summary>
        /// <param name="entity">流程文件实体</param>
        public void SaveProcessFile(ProcessFileEntity entity)
        {
            var pm = new ProcessManager();
            pm.SaveProcessFile(entity);
        }

        /// <summary>
        /// 创建流程定义记录
        /// </summary>
        /// <param name="entity">流程定义实体</param>
        /// <returns>新ID</returns>
        public int CreateProcess(ProcessEntity entity)
        {
            var pm = new ProcessManager();
            var processID = pm.CreateProcess(entity);

            return processID;
        }

        /// <summary>
        /// 创建流程定义记录新版本
        /// </summary>
        /// <param name="entity">流程</param>
        public int CreateProcessVersion(ProcessEntity entity)
        {
            var pm = new ProcessManager();
            return pm.CreateProcessVersion(entity);
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
            pm.Delete(processGUID, version);
        }

		/// <summary>
        /// 删除流程定义记录
        /// </summary>
        /// <param name="processGUID">流程GUID</param>
        public void DeleteProcess(string processGUID)
        {
            var processManager = new ProcessManager();
            processManager.Delete(processGUID);
        }

        /// <summary>
        /// 导入流程XML文件，并生成新流程
        /// </summary>
        /// <param name="entity">流程实体</param>
        /// <returns>新流程ID</returns>
        public int ImportProcess(ProcessEntity entity)
        {
            var pm = new ProcessManager();
            var processID = pm.Insert(entity);

            return processID;
        }
        #endregion

        #region 获取节点信息
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
        /// 获取任务类型的节点列表
        /// </summary>
        /// <param name="processGUID">流程定义GUID</param>
        /// <param name="version">版本</param>
        /// <returns>活动列表</returns>
        public IList<ActivityEntity> GetTaskActivityList(string processGUID, string version)
        {
            var processModel = ProcessModelFactory.Create(processGUID, version);
            var activityList = processModel.GetTaskActivityList();

            return activityList;
        }

        /// <summary>
        /// 获取全部任务类型的节点列表（包含会签和子流程）
        /// </summary>
        /// <param name="processGUID">流程GUID</param>
        /// <param name="version">版本</param>
        /// <returns>活动列表</returns>
        public IList<ActivityEntity> GetAllTaskActivityList(string processGUID, string version)
        {
            var processModel = ProcessModelFactory.Create(processGUID, version);
            var activityList = processModel.GetAllTaskActivityList();

            return activityList;
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
            var tm = new TaskManager();
            var taskView = tm.GetTaskOfMine(runner.AppInstanceID, runner.ProcessGUID, runner.UserID);
            var processModel = ProcessModelFactory.Create(taskView.ProcessGUID, taskView.Version);
            var nextSteps = processModel.GetNextActivityTree(taskView.ProcessInstanceID,
                taskView.ActivityGUID,
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
            var nextSteps = processModel.GetNextActivityTree(taskView.ProcessInstanceID,
                taskView.ActivityGUID,
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
            var nextSteps = processModel.GetNextActivityTree(taskView.ProcessInstanceID, 
                taskView.ActivityGUID, 
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
        private AutoResetEvent waitHandler = new AutoResetEvent(false);
        private WfExecutedResult _startedResult = null;
        private WfExecutedResult _runAppResult = null;
        private WfExecutedResult _withdrawedResult = null;
        private WfExecutedResult _sendbackResult = null;
        private WfExecutedResult _reversedResult = null;
        private WfExecutedResult _jumpResult = null;
        private WfExecutedResult _signforwardResult = null;
        /// <summary>
        /// 启动流程
        /// </summary>
        /// <param name="starter">启动人</param>
        /// <returns>启动结果</returns>
        public WfExecutedResult StartProcess(WfAppRunner starter)
        {
            using (var session = DbFactory.CreateSession())
            {
                var transaction = session.DbContext.Database.BeginTransaction();
                var result = StartProcess(starter, session);

                if (result.Status == WfExecutedStatus.Success)
                {
                    transaction.Commit();
                }
                else
                {
                    transaction.Rollback();
                }
                return result;
            }
        }

        /// <summary>
        /// 启动流程
        /// </summary>
        /// <param name="starter">启动人</param>
        /// <param name="session">会话</param>
        /// <returns>启动结果</returns>
        public WfExecutedResult StartProcess(WfAppRunner starter, 
            IDbSession session)
        {
            try
            {
                var runtimeInstance = WfRuntimeManagerFactory.CreateRuntimeInstanceStartup(starter, ref _startedResult);

                if (_startedResult.Status == WfExecutedStatus.Exception)
                {
                    return _startedResult;
                }

                runtimeInstance.OnWfProcessExecuted += runtimeInstance_OnWfProcessStarted;
                runtimeInstance.Execute(session);

                //do something else here...

                waitHandler.WaitOne();
            }
            catch (System.Exception e)
            {
                _startedResult.Status = WfExecutedStatus.Failed;
                _startedResult.Message = string.Format("流程启动发生错误，内部异常:{0}", e.Message);
                LogManager.RecordLog(WfDefine.WF_PROCESS_START_ERROR, LogEventType.Error, LogPriority.High, starter, e);
            }
            return _startedResult;
        }

        /// <summary>
        /// 流程启动返回结果对象
        /// </summary>
        /// <param name="sender">返回对象</param>
        /// <param name="args">事件参数</param>
        private void runtimeInstance_OnWfProcessStarted(object sender, WfEventArgs args)
        {
            _startedResult = args.WfExecutedResult;
            waitHandler.Set();
        }
        #endregion

        #region 运行流程
        /// <summary>
        /// 运行流程(业务处理)
        /// </summary>
        /// <param name="runner">运行人</param>
        /// <returns>运行结果</returns>
        public WfExecutedResult RunProcessApp(WfAppRunner runner)
        {
            using (var session = DbFactory.CreateSession())
            {
                var transaction = session.DbContext.Database.BeginTransaction();
                var result = RunProcessApp(runner, session);

                if (result.Status == WfExecutedStatus.Success)
                {
                    transaction.Commit();
                }
                else
                {
                    transaction.Rollback();
                }
                return result;
            }
        }

        /// <summary>
        /// 运行流程
        /// </summary>
        /// <param name="runner">运行人</param>
        /// <param name="session">会话</param>
        /// <returns>运行结果</returns>
        public WfExecutedResult RunProcessApp(WfAppRunner runner, 
            IDbSession session)
        {
            try
            {
                var runtimeInstance = WfRuntimeManagerFactory.CreateRuntimeInstanceAppRunning(runner, ref _runAppResult);

                if (_runAppResult.Status == WfExecutedStatus.Exception)
                {
                    return _runAppResult;
                }

                runtimeInstance.OnWfProcessExecuted += runtimeInstance_OnWfProcessContinued;
                bool isRunning = runtimeInstance.Execute(session);

                waitHandler.WaitOne();

                return _runAppResult;
            }
            catch (System.Exception e)
            {
                _runAppResult.Status = WfExecutedStatus.Failed;
                var error = e.InnerException != null ? e.InnerException.Message : e.Message;
                _runAppResult.Message = string.Format("流程运行时发生异常！，详细错误：{0}", error);
                LogManager.RecordLog(WfDefine.WF_PROCESS_RUN_ERROR, LogEventType.Error, LogPriority.High, runner, e);
            }

            return _runAppResult;
        }

        /// <summary>
        /// 流程运行返回结果对象
        /// </summary>
        /// <param name="sender">返回对象</param>
        /// <param name="args">事件参数</param>
        private void runtimeInstance_OnWfProcessContinued(object sender, WfEventArgs args)
        {
            _runAppResult = args.WfExecutedResult;
            waitHandler.Set();
        }
        #endregion

        #region 流程跳转
        /// <summary>
        /// 流程跳转
        /// </summary>
        /// <param name="runner">执行操作人</param>
        /// <returns>跳转结果</returns>
        public WfExecutedResult JumpProcess(WfAppRunner runner)
        {
            using (var session = DbFactory.CreateSession())
            {
                var result = JumpProcess(runner, session);
                if (result.Status == WfExecutedStatus.Success)
                {
                    session.SaveChanges();
                }
                return result;
            }
        }

        /// <summary>
        /// 流程跳转
        /// </summary>
        /// <param name="runner">执行操作人</param>
        /// <param name="session">会话</param>
        /// <returns>跳转结果</returns>
        public WfExecutedResult JumpProcess(WfAppRunner runner, 
            IDbSession session)
        {
            try
            {
                var runtimeInstance = WfRuntimeManagerFactory.CreateRuntimeInstanceJump(runner, ref _jumpResult);

                if (_jumpResult.Status == WfExecutedStatus.Exception)
                {
                    return _jumpResult;
                }

                runtimeInstance.OnWfProcessExecuted += runtimeInstance_OnWfProcessJump;
                bool isJumping = runtimeInstance.Execute(session);

                waitHandler.WaitOne();

                return _jumpResult;
            }
            catch (System.Exception e)
            {
                _jumpResult.ExceptionType = WfExceptionType.Jump_OtherError;
                _jumpResult.Message = string.Format("流程跳转时发生异常！，详细错误：{0}", e.Message);
                LogManager.RecordLog(WfDefine.WF_PROCESS_JUMP_ERROR, LogEventType.Error, LogPriority.High, runner, e);
            }

            return _jumpResult;
        }

        private void runtimeInstance_OnWfProcessJump(object sender, WfEventArgs args)
        {
            _jumpResult = args.WfExecutedResult;
            waitHandler.Set();
        }
        #endregion

        #region 流程撤销、回退和返签（已经结束的流程可以被复活）
        /// <summary>
        /// 流程撤销
        /// </summary>
        /// <param name="runner">撤销人</param>
        /// <returns>撤销结果</returns>
        public WfExecutedResult WithdrawProcess(WfAppRunner runner)
        {
            using (var session = DbFactory.CreateSession())
            {
                var result = WithdrawProcess(runner, session);
                if (result.Status == WfExecutedStatus.Success)
                {
                    session.SaveChanges();
                }
                return result;
            }
        }

        /// <summary>
        /// 撤销流程
        /// </summary>
        /// <param name="withdrawer">撤销人</param>
        /// <param name="session">会话</param>
        /// <returns>撤销结果</returns>
        public WfExecutedResult WithdrawProcess(WfAppRunner withdrawer, 
            IDbSession session)
        {
            try
            {
                var runtimeInstance = WfRuntimeManagerFactory.CreateRuntimeInstanceWithdraw(withdrawer, ref _withdrawedResult);

                //不满足撤销操作，返回异常结果信息
                if (_withdrawedResult.Status == WfExecutedStatus.Exception)
                {
                    return _withdrawedResult;
                }

                runtimeInstance.OnWfProcessExecuted += runtimeInstance_OnWfProcessWithdrawed;
                bool isWithdrawed = runtimeInstance.Execute(session);

                waitHandler.WaitOne();

                return _withdrawedResult;
            }
            catch (System.Exception e)
            {
                _withdrawedResult.Status = WfExecutedStatus.Failed;
                _withdrawedResult.Message = string.Format("流程撤销发生异常！，详细错误：{0}", e.Message);
                LogManager.RecordLog(WfDefine.WF_PROCESS_WITHDRAW_ERROR, LogEventType.Error, LogPriority.High, withdrawer, e);
            }
            return _withdrawedResult;
        }

        /// <summary>
        /// 流程撤销返回对象
        /// </summary>
        /// <param name="sender">返回对象</param>
        /// <param name="args">事件参数</param>
        private void runtimeInstance_OnWfProcessWithdrawed(object sender, WfEventArgs args)
        {
            _withdrawedResult = args.WfExecutedResult;
            waitHandler.Set();
        }

        /// <summary>
        /// 退回到上一步
        /// </summary>
        /// <param name="runner">退回操作人</param>
        /// <returns>退回结果</returns>
        public WfExecutedResult SendBackProcess(WfAppRunner runner)
        {
            using (var session = DbFactory.CreateSession())
            {
                var result = SendBackProcess(runner, session);
                if (result.Status == WfExecutedStatus.Success)
                {
                    session.SaveChanges();
                }
                return result;
            }
        }

        /// <summary>
        /// 退回到上一步
        /// </summary>
        /// <param name="sender">退回人</param>
        /// <param name="session">会话</param>
        /// <returns>退回结果</returns>
        public WfExecutedResult SendBackProcess(WfAppRunner sender, 
            IDbSession session)
        {
            try
            {
                var runtimeInstance = WfRuntimeManagerFactory.CreateRuntimeInstanceSendBack(sender, ref _sendbackResult);

                if (_sendbackResult.Status == WfExecutedStatus.Exception)
                {
                    return _sendbackResult;
                }

                runtimeInstance.OnWfProcessExecuted += runtimeInstance_OnWfProcessSentBack;
                bool isRejected = runtimeInstance.Execute(session);

                waitHandler.WaitOne();

                return _sendbackResult;
            }
            catch (System.Exception e)
            {
                _sendbackResult.Status = WfExecutedStatus.Failed;
                _sendbackResult.Message = string.Format("流程退回发生异常！，详细错误：{0}", e.Message);
                LogManager.RecordLog(WfDefine.WF_PROCESS_SENDBACK_ERROR, LogEventType.Error, LogPriority.High, sender, e);
            }
            return _sendbackResult;
        }

        /// <summary>
        /// 流程退回返回结果对象
        /// </summary>
        /// <param name="sender">返回对象</param>
        /// <param name="args">事件参数</param>
        private void runtimeInstance_OnWfProcessSentBack(object sender, WfEventArgs args)
        {
            _sendbackResult = args.WfExecutedResult;
            waitHandler.Set();
        }

        /// <summary>
        /// 流程返签
        /// </summary>
        /// <param name="runner">结束人</param>
        /// <returns>返签结果</returns>
        public WfExecutedResult ReverseProcess(WfAppRunner runner)
        {
            using (var session = DbFactory.CreateSession())
            {
                var result = ReverseProcess(runner, session);
                if (result.Status == WfExecutedStatus.Success)
                {
                    session.SaveChanges();
                }
                return result;
            }
        }

        /// <summary>
        /// 流程返签
        /// </summary>
        /// <param name="ender">结束人</param>
        /// <param name="session">会话</param>
        /// <returns>返签结果</returns>
        public WfExecutedResult ReverseProcess(WfAppRunner ender, 
            IDbSession session)
        {
            try
            {
                var runtimeInstance = WfRuntimeManagerFactory.CreateRuntimeInstanceReverse(ender, ref _reversedResult);

                if (_reversedResult.Status == WfExecutedStatus.Exception)
                {
                    return _reversedResult;
                }

                runtimeInstance.OnWfProcessExecuted += runtimeInstance_OnWfProcessReversed;
                bool isReversed = runtimeInstance.Execute(session);

                waitHandler.WaitOne();

                //return _wfExecutedResult;
                return _reversedResult;
            }
            catch (System.Exception e)
            {
                _reversedResult.Status = WfExecutedStatus.Failed;
                _reversedResult.Message = string.Format("流程返签发生异常！，详细错误：{0}", e.Message);
                LogManager.RecordLog(WfDefine.WF_PROCESS_REVERSE_ERROR, LogEventType.Error, LogPriority.High, ender, e);
            }
            return _reversedResult;
        }

        /// <summary>
        /// 恢复流程实例（只针对挂起操作）
        /// </summary>
        /// <param name="processInstanceId">挂起操作的实例ID</param>
        /// <param name="runner">执行者</param>
        public bool ResumeProcess(int processInstanceId, WfAppRunner runner)
        {
            var pim = new ProcessInstanceManager();
            return pim.Resume(processInstanceId, runner);
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
                pim.Suspend(taskentity.ProcessInstanceID, runner);
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// 流程返签结果对象
        /// </summary>
        /// <param name="sender">返回对象</param>
        /// <param name="args">事件参数</param>
        private void runtimeInstance_OnWfProcessReversed(object sender, WfEventArgs args)
        {
            _reversedResult = args.WfExecutedResult;
            waitHandler.Set();
        }
        #endregion

        #region 取消（运行的）流程、废弃执行中或执行完的流程
        /// <summary>
        /// 取消流程
        /// </summary>
        /// <param name="processInstanceID">流程实例ID</param>
        /// <param name="runner">执行操作的用户</param>
        /// <returns>执行结果的标志</returns>
        public bool CancelProcess(int processInstanceID, WfAppRunner runner)
        {
            var pim = new ProcessInstanceManager();
            return pim.Cancel(processInstanceID, runner);
        }

        /// <summary>
        /// 废弃流程
        /// </summary>
        /// <param name="runner">执行操作的用户</param>
        /// <returns>执行结果的标志</returns>
        public bool DiscardProcess(int processInstanceID, WfAppRunner runner)
        {
            var pim = new ProcessInstanceManager();
            return pim.Discard(processInstanceID, runner);
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
            var taskManager = new TaskManager();
            var taskList = taskManager.GetRunningTasks(query, out int allRowsCount);
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
            var taskManager = new TaskManager();
            var taskList = taskManager.GetCompletedTasks(query, out int allRowsCount);

            if (taskList != null)
                return taskList.ToList();
            else
                return null;
        }
        #endregion 
    }
}