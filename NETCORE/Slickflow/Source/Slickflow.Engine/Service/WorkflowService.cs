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
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Slickflow.Data;
using Slickflow.Module.Resource;
using Slickflow.Engine.Common;
using Slickflow.Engine.Storage;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Core.Result;
using Slickflow.Engine.Core.Event;
using Slickflow.Engine.Core.Runtime;
using Slickflow.Engine.Core.Parser;
using Slickflow.Engine.Utility;

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
        /// 流程定义数据读取
        /// </summary>
        /// <param name="processName">流程名称</param>
        /// <param name="version">版本号</param>
        /// <returns>流程</returns>
        public ProcessEntity GetProcessByName(string processName, string version = null)
        {
            var pm = new ProcessManager();
            var entity = pm.GetByName(processName, version);

            return entity;
        }

        /// <summary>
        /// 流程定义数据读取
        /// </summary>
        /// <param name="processCode">流程代码</param>
        /// <param name="version">版本号</param>
        /// <returns>流程</returns>
        public ProcessEntity GetProcessByCode(string processCode, string version = null)
        {
            var pm = new ProcessManager();
            var entity = pm.GetByCode(processCode, version);

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
        /// 获取流程定义记录
        /// </summary>
        /// <param name="processID">流程主键ID</param>
        /// <returns>流程</returns>
        public ProcessEntity GetProcessByID(int processID)
        {
            var pm = new ProcessManager();
            var entity = pm.GetByID(processID);
            return entity;
        }

        /// <summary>
        /// 获取流程定义文件
        /// </summary>
        /// <param name="id">流程ID</param>
        /// <returns></returns>
        public ProcessFileEntity GetProcessFileByID(int id)
        {
            var pm = new ProcessManager();
            var entity = pm.GetProcessFileByID(id, XPDLStorageFactory.CreateXPDLStorage());

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
        public int InsertProcess(ProcessEntity entity)
        {
            var pm = new ProcessManager();
            var processID = pm.Insert(entity);

            return processID;
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
            var processManager = new ProcessManager();
            entity.CreatedDateTime = DateTime.Now;
            entity.IsUsing = 1;

            var session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();
                processManager.UpdateUsingState(session.Connection, entity.ProcessGUID, session.Transaction);
                int processId = processManager.Insert(session.Connection, entity, session.Transaction);
                session.Commit();

                return processId;
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
        /// 升级流程记录
        /// </summary>
        /// <param name="processGUID">流程GUID</param>
        /// <param name="version">流程版本</param>
        /// <param name="newVersion">新版本编号</param>
        public void UpgradeProcess(string processGUID, string version, string newVersion)
        {
            var processManager = new ProcessManager();
            processManager.Upgrade(processGUID, version, newVersion);
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

        /// <summary>
        /// 删除流程实例包括其关联数据
        /// </summary>
        /// <param name="processGUID">流程GUID</param>
        /// <param name="version">版本</param>
        /// <returns>是否删除</returns>
        public bool DeleteInstanceInt(string processGUID, string version)
        {
            var pim = new ProcessInstanceManager();
            return pim.Delete(processGUID, version);
        }

        /// <summary>
        /// 导入流程XML文件，并生成新流程
        /// </summary>
        /// <param name="entity">流程实体</param>
        /// <returns>新流程ID</returns>
        public int ImportProcess(ProcessEntity entity)
        {
            string xmlContent = entity.XmlContent;

            var pm = new ProcessManager();
            var processID = pm.CreateProcess(entity);

            var fileEntity = new ProcessFileEntity
            {
                ProcessGUID = entity.ProcessGUID,
                Version = entity.Version,
                ProcessName = entity.ProcessName,
                XmlContent = xmlContent
            };
            pm.SaveProcessFile(fileEntity);

            return processID;
        }

        /// <summary>
        /// 重置缓存中的流程定义信息
        /// </summary>
        /// <param name="processGUID">流程Guid编号</param>
        /// <param name="version">流程版本</param>
        public void ResetCache(string processGUID, string version = null)
        {
            //获取流程信息
            var process = GetProcessByVersion(processGUID, version);
            var pm = new ProcessManager();
            var xmlDoc = pm.GetProcessXmlDocument(process.ProcessGUID, process.Version,
                XPDLStorageFactory.CreateXPDLStorage());    //xml文件读取方式，数据库或外部文件
            if (MemoryCachedHelper.GetXpdlCache(process.ProcessGUID, process.Version) == null)
            {
                MemoryCachedHelper.SetXpdlCache(process.ProcessGUID, process.Version, xmlDoc);
            }
            else
            {
                MemoryCachedHelper.TryUpdate(process.ProcessGUID, process.Version, xmlDoc);
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
        /// 获取任务类型的节点列表
        /// </summary>
        /// <param name="processID">流程主键ID</param>
        /// <returns>活动列表</returns>
        public IList<ActivityEntity> GetTaskActivityList(int processID)
        {
            var pm = new ProcessManager();
            var entity = pm.GetByID(processID);
            var processModel = ProcessModelFactory.Create(entity.ProcessGUID, entity.Version);
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
            var nextSteps = processModel.GetNextActivityTree(taskView.ActivityGUID,
                condition);

            return nextSteps;
        }

        /// <summary>
        /// 根据应用获取流程下一步节点列表
        /// </summary>
        /// <param name="runner">应用执行人</param>
        /// <param name="condition">条件</param>
        /// <returns>节点列表</returns>
        public async Task<IList<NodeView>> GetNextActivityTreeAsync(WfAppRunner runner, 
            IDictionary<string, string> condition = null)
        {
            var task = await Task.Run<IList<NodeView>>(() =>
            {
                return GetNextActivityTree(runner, condition);
            });
            return task;
        }

        /// <summary>
        /// 根据应用获取流程下一步节点列表
        /// </summary>
        /// <param name="conn">数据库链接</param>
        /// <param name="runner">应用执行人</param>
        /// <param name="condition">条件</param>
        /// <param name="trans">事务或交易</param>
        /// <returns>节点列表</returns>
        public IList<NodeView> GetNextActivityTree(IDbConnection conn, 
            WfAppRunner runner,
            IDictionary<string, string> condition,
            IDbTransaction trans)
        {
            var tm = new TaskManager();
            var taskView = tm.GetTaskOfMine(conn, runner.AppInstanceID, runner.ProcessGUID, runner.UserID, runner.TaskID, trans);
            var processModel = ProcessModelFactory.Create(taskView.ProcessGUID, taskView.Version);
            var nextSteps = processModel.GetNextActivityTree(taskView.ActivityGUID,
                condition);

            return nextSteps;
        }


        /// <summary>
        /// 根据应用获取流程下一步节点列表
        /// </summary>
        /// <param name="conn">数据库链接</param>
        /// <param name="runner">应用执行人</param>
        /// <param name="condition">条件</param>
        /// <param name="trans">事务或交易</param>
        /// <returns>节点列表</returns>
        public async Task<IList<NodeView>> GetNextActivityTreeAsync(IDbConnection conn, 
            WfAppRunner runner,
            IDictionary<string, string> condition, 
            IDbTransaction trans)
        {
            var task = await Task.Run<IList<NodeView>>(() =>
            {
                return GetNextActivityTree(conn, runner, condition, trans);
            });
            return task;
        }

        /// <summary>
        /// 获取下一步活动列表树
        /// </summary>
        /// <param name="taskID">任务ID</param>
        /// <param name="condition">条件</param>
        /// <returns>下一步列表</returns>
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
        /// 获取下一步活动列表树
        /// </summary>
        /// <param name="taskID">任务ID</param>
        /// <param name="condition">条件</param>
        /// <returns>下一步列表</returns>
        public async Task<IList<NodeView>> GetNextActivityTreeAsync(int taskID, 
            IDictionary<string, string> condition = null)
        {
            var task = await Task.Run<IList<NodeView>>(() => 
            {
                return GetNextActivityTree(taskID, condition);
            });
            return task;
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
            var nsp = new NextStepParser();
            var nextSteps = nsp.GetNextActivityRoleUserTree(ResourceService, runner, condition);

            return nextSteps;
        }


        /// <summary>
        /// 根据应用获取流程下一步节点列表，包含角色用户
        /// </summary>
        /// <param name="runner">应用执行人</param>
        /// <param name="condition">条件</param>
        /// <returns>节点列表</returns>
        public async Task<IList<NodeView>> GetNextActivityRoleUserTreeAsync(WfAppRunner runner, 
            IDictionary<string, string> condition = null)
        {
            var task = await Task.Run<IList<NodeView>>(() =>
            {
                return GetNextActivityRoleUserTree(runner, condition);
            });
            return task;
        }

        /// <summary>
        /// 根据应用获取流程下一步节点列表，包含角色用户
        /// 包含：
        /// 1) 网关下一步添加人员的预加载用户列表；
        /// 2) 会签模式的下一步添加人员的预加载用户列表；
        /// </summary>
        /// <param name="runner">应用执行人</param>
        /// <param name="condition">条件</param>
        /// <returns>节点列表</returns>
        public NextStepInfo GetNextStepInfo(WfAppRunner runner, 
            IDictionary<string, string> condition = null)
        {
            var nsp = new NextStepParser();
            var nextStepInfo = nsp.GetNextStepInfo(ResourceService, runner, condition);

            return nextStepInfo;
        }

        /// <summary>
        /// 异步获取下一步列表
        /// </summary>
        /// <param name="runner">应用执行人</param>
        /// <param name="condition">条件</param>
        /// <returns>节点列表</returns>
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
        /// 流程启动时获取开始节点下一步的节点角色人员列表
        /// 统一整合到: GetNextActivityRoleUserTree()
        /// </summary>
        /// <param name="runner">应用执行人</param>
        /// <param name="condition">条件</param>
        /// <returns>节点列表</returns>
        public IList<NodeView> GetFirstActivityRoleUserTree(WfAppRunner runner, 
            IDictionary<string, string> condition = null)
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
        /// 流程启动时获取开始节点下一步的节点角色人员列表
        /// 统一整合到: GetNextActivityRoleUserTree()
        /// </summary>
        /// <param name="runner">应用执行人</param>
        /// <param name="condition">条件</param>
        /// <returns>节点列表</returns>
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
            var list = am.GetActivityInstanceListCompleted(task.AppInstanceID, task.ProcessGUID);

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
            var list = am.GetActivityInstanceListCompleted(runner.AppInstanceID, runner.ProcessGUID);

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

        #region 获取要退回的(上一步)节点信息(退回模式)
        /// <summary>
        /// 获取上一步节点信息
        /// </summary>
        /// <param name="runner">当前运行用户</param>
        /// <returns>上一步步骤列表</returns>
        public IList<NodeView> GetPreviousActivityTree(WfAppRunner runner)
        {
            var psc = new PreviousStepChecker();
            var hasGatewayPassed = false;
            var nodeList = psc.GetPreviousActivityTree(runner, out hasGatewayPassed);

            return nodeList;
        }

        /// <summary>
        /// 获取上一步节点信息
        /// </summary>
        /// <param name="runner">当前运行用户</param>
        /// <returns>上一步步骤列表</returns>
        public async Task<IList<NodeView>> GetPreviousActivityTreeAsync(WfAppRunner runner)
        {
            var task = await Task.Run<IList<NodeView>>(() =>
            {
                return GetPreviousActivityTree(runner);
            });
            return task;
        }

        /// <summary>
        /// 获取上一步节点信息
        /// </summary>
        /// <param name="runner">运行者</param>
        /// <returns>上一步信息</returns>
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
        /// 异步获取上一步节点信息
        /// </summary>
        /// <param name="runner">运行者</param>
        /// <returns>上一步信息</returns>
        public async Task<PreviousStepInfo> GetPreviousStepInfoAsync(WfAppRunner runner)
        {
            var task = await Task.Run<PreviousStepInfo>(() =>
            {
                return GetPreviousStepInfo(runner);
            });
            return task;
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
                WfRuntimeManagerFactory.RegisterEvent(runtimeInstance, 
                    runtimeInstance_OnWfProcessStarting, 
                    runtimeInstance_OnWfProcessStarted);
                bool isStarted = runtimeInstance.Execute(session);

                //do some thing else here...
                //...
                waitHandler.WaitOne();
            }
            catch (System.Exception e)
            {
                startedResult.Status = WfExecutedStatus.Failed;
                startedResult.Message = string.Format("流程启动发生异常！详细错误:{0}", e.Message);
                LogManager.RecordLog(WfDefine.WF_PROCESS_START_ERROR, LogEventType.Error, LogPriority.High, runner, e);
            }
            finally
            {
                //卸载事件
                WfRuntimeManagerFactory.UnregisterEvent(runtimeInstance, 
                    runtimeInstance_OnWfProcessStarting, 
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
        /// <param name="runner">启动人</param>
        /// <returns>启动结果</returns>
        public WfExecutedResult StartProcess(WfAppRunner runner)
        {
            string time = string.Format("engine time:{0}", System.DateTime.Now.ToString());
            System.Diagnostics.Debug.WriteLine(time);

            _wfAppRunner = runner;
            var result = Start();

            return result;
        }

        /// <summary>
        /// 异步启动流程
        /// </summary>
        /// <param name="runner">启动人</param>
        /// <returns>执行结果</returns>
        public async Task<WfExecutedResult> StartProcessAsync(WfAppRunner runner)
        {
            var task = await Task.Run<WfExecutedResult>(() => 
            {
                return StartProcess(runner);
            });
            return task;
        }

        /// <summary>
        /// 流程启动
        /// </summary>
        /// <param name="conn">连接</param>
        /// <param name="runner">启动人</param>
        /// <param name="trans">事务</param>
        /// <returns>启动结果</returns>
        public WfExecutedResult StartProcess(IDbConnection conn, WfAppRunner runner, IDbTransaction trans)
        {
            _wfAppRunner = runner;
            var result = Start(conn, trans);

            return result;
        }

        /// <summary>
        /// 异步流程启动
        /// </summary>
        /// <param name="conn">连接</param>
        /// <param name="runner">启动人</param>
        /// <param name="trans">事务</param>
        /// <returns>启动结果</returns>
        public async Task<WfExecutedResult> StartProcessAsync(IDbConnection conn, WfAppRunner runner, IDbTransaction trans)
        {
            var task = await Task.Run<WfExecutedResult>(() =>
            {
                return StartProcess(conn, runner, trans);
            });
            return task;
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
                runtimeInstance = WfRuntimeManagerFactory.CreateRuntimeInstanceAppRunning(runner, session, ref runAppResult);
                if (runAppResult.Status == WfExecutedStatus.Exception)
                {
                    return runAppResult;
                }

                //注册事件并运行
                WfRuntimeManagerFactory.RegisterEvent(runtimeInstance, 
                    runtimeInstance_OnWfProcessRunning,
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
                runAppResult.Message = string.Format("流程运行时发生异常！详细错误：{0}", error);
                LogManager.RecordLog(WfDefine.WF_PROCESS_RUN_ERROR, LogEventType.Error, LogPriority.High, runner, e);
            }
            finally
            {
                //卸载事件
                WfRuntimeManagerFactory.UnregisterEvent(runtimeInstance, 
                    runtimeInstance_OnWfProcessRunning,
                    runtimeInstance_OnWfProcessContinued);
                waitHandler.Dispose();
            }
            return runAppResult;

            void runtimeInstance_OnWfProcessRunning(object sender, WfEventArgs args)
            {
                Delegate.DelegateExecutor.InvokeExternalDelegate(session,
                    Delegate.EventFireTypeEnum.OnProcessRunning,
                    runner.DelegateEventList,
                    runtimeInstance.ProcessInstanceID); 
            }

            void runtimeInstance_OnWfProcessContinued(object sender, WfEventArgs args)
            {
                runAppResult = args.WfExecutedResult;
                if (runAppResult.Status == WfExecutedStatus.Success)
                {
                    Delegate.DelegateExecutor.InvokeExternalDelegate(session,
                        Delegate.EventFireTypeEnum.OnProcessContinued,
                        runner.DelegateEventList,
                        runtimeInstance.ProcessInstanceID);
                }
                waitHandler.Set();
            }
        }

        /// <summary>
        /// 流程流转
        /// 说明：新方法统一调用RunProcess()
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
        /// 说明：新方法统一调用RunProcess()
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

        /// <summary>
        /// 流程流转
        /// </summary>
        /// <returns>执行结果</returns>
        public WfExecutedResult RunProcess(WfAppRunner runner)
        {
            _wfAppRunner = runner;
            var result = Run();

            return result;
        }

        /// <summary>
        /// 异步执行流程
        /// </summary>
        /// <param name="runner">执行人</param>
        /// <returns>执行结果</returns>
        public async Task<WfExecutedResult> RunProcessAsync(WfAppRunner runner)
        {
            var task = await Task.Run<WfExecutedResult>(() =>
            {
                return RunProcess(runner);
            });
            return task;
        }

        /// <summary>
        /// 流程流转
        /// </summary>
        /// <param name="conn">连接</param>
        /// <param name="runner">运行人</param>
        /// <param name="trans">事务</param>
        /// <returns>执行结果</returns>
        public WfExecutedResult RunProcess(IDbConnection conn, WfAppRunner runner, IDbTransaction trans)
        {
            _wfAppRunner = runner;
            var result = Run(conn, trans);

            return result;
        }

        /// <summary>
        /// 异步流程流转
        /// </summary>
        /// <param name="conn">连接</param>
        /// <param name="runner">运行人</param>
        /// <param name="trans">事务</param>
        /// <returns>执行结果</returns>
        public async Task<WfExecutedResult> RunProcessAsync(IDbConnection conn, WfAppRunner runner, IDbTransaction trans)
        {
            var task = await Task.Run<WfExecutedResult>(() =>
            {
                return RunProcessApp(conn, runner, trans);
            });
            return task;
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
                WfRuntimeManagerFactory.RegisterEvent(runtimeInstance,
                    runtimeInstance_OnWfProcessWithdrawing,
                    runtimeInstance_OnWfProcessWithdrawn);
                bool isWithdrawn = runtimeInstance.Execute(session);

                waitHandler.WaitOne();
            }
            catch (System.Exception e)
            {
                withdrawedResult.Status = WfExecutedStatus.Failed;
                var error = e.InnerException != null ? e.InnerException.Message : e.Message;
                withdrawedResult.Message = string.Format("流程撤销发生异常！详细错误：{0}", error);
                LogManager.RecordLog(WfDefine.WF_PROCESS_WITHDRAW_ERROR, LogEventType.Error, LogPriority.High, runner, e);
            }
            finally
            {
                //卸载事件
                WfRuntimeManagerFactory.UnregisterEvent(runtimeInstance, 
                    runtimeInstance_OnWfProcessWithdrawing,
                    runtimeInstance_OnWfProcessWithdrawn);
                waitHandler.Dispose();
            }
            return withdrawedResult;

            void runtimeInstance_OnWfProcessWithdrawing(object sender, WfEventArgs args)
            {
                Delegate.DelegateExecutor.InvokeExternalDelegate(session,
                    Delegate.EventFireTypeEnum.OnProcessWithdrawing,
                    runner.DelegateEventList,
                    runtimeInstance.ProcessInstanceID);
            }

            void runtimeInstance_OnWfProcessWithdrawn(object sender, WfEventArgs args)
            {
                withdrawedResult = args.WfExecutedResult;
                if (withdrawedResult.Status == WfExecutedStatus.Success)
                {
                    Delegate.DelegateExecutor.InvokeExternalDelegate(session,
                        Delegate.EventFireTypeEnum.OnProcessWithdrawn,
                        runner.DelegateEventList,
                        runtimeInstance.ProcessInstanceID);
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
        /// 异步撤销流程
        /// </summary>
        /// <param name="runner">执行人</param>
        /// <returns>执行结果</returns>
        public async Task<WfExecutedResult> WithdrawProcessAsync(WfAppRunner runner)
        {
            var task = await Task.Run<WfExecutedResult>(() =>
            {
                return WithdrawProcess(runner);
            });
            return task;
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

        /// <summary>
        /// 异步撤销流程
        /// </summary>
        /// <param name="conn">连接</param>
        /// <param name="runner">运行人</param>
        /// <param name="trans">事务</param>
        /// <returns>执行结果</returns>
        public async Task<WfExecutedResult> WithdrawProcessAsync(IDbConnection conn, WfAppRunner runner, IDbTransaction trans)
        {
            var task = await Task.Run<WfExecutedResult>(() =>
            {
                return WithdrawProcess(conn, runner, trans);
            });
            return task;
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
                WfRuntimeManagerFactory.RegisterEvent(runtimeInstance, 
                    runtimeInstance_OnWfProcessSendBacking,
                    runtimeInstance_OnWfProcessSendBacked);
                bool isSendBacked = runtimeInstance.Execute(session);

                waitHandler.WaitOne();
            }
            catch (System.Exception e)
            {
                sendbackResult.Status = WfExecutedStatus.Failed;
                var error = e.InnerException != null ? e.InnerException.Message : e.Message;
                sendbackResult.Message = string.Format("流程退回发生异常！详细错误：{0}", error);
                LogManager.RecordLog(WfDefine.WF_PROCESS_SENDBACK_ERROR, LogEventType.Error, LogPriority.High, runner, e);
            }
            finally
            {
                //卸载事件
                WfRuntimeManagerFactory.UnregisterEvent(runtimeInstance, 
                    runtimeInstance_OnWfProcessSendBacking,
                    runtimeInstance_OnWfProcessSendBacked);
                waitHandler.Dispose();
            }
            return sendbackResult;


            void runtimeInstance_OnWfProcessSendBacking(object sender, WfEventArgs args)
            {
                Delegate.DelegateExecutor.InvokeExternalDelegate(session,
                        Delegate.EventFireTypeEnum.OnProcessSendBacking,
                        runner.DelegateEventList,
                        runtimeInstance.ProcessInstanceID);
            }

            void runtimeInstance_OnWfProcessSendBacked(object sender, WfEventArgs args)
            {
                sendbackResult = args.WfExecutedResult;
                if (sendbackResult.Status == WfExecutedStatus.Success)
                {
                    Delegate.DelegateExecutor.InvokeExternalDelegate(session,
                        Delegate.EventFireTypeEnum.OnProcessSendBacked,
                        runner.DelegateEventList,
                        runtimeInstance.ProcessInstanceID);
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
        /// 异步退回流程
        /// </summary>
        /// <param name="runner">执行人</param>
        /// <returns>执行结果</returns>
        public async Task<WfExecutedResult> SendBackProcessAsync(WfAppRunner runner)
        {
            var task = await Task.Run<WfExecutedResult>(() =>
            {
                return SendBackProcess(runner);
            });
            return task;
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

        /// <summary>
        /// 异步退回流程
        /// </summary>
        /// <param name="conn">连接</param>
        /// <param name="runner">运行人</param>
        /// <param name="trans">事务</param>
        /// <returns>执行结果</returns>
        public async Task<WfExecutedResult> SendBackProcessAsync(IDbConnection conn, WfAppRunner runner, IDbTransaction trans)
        {
            var task = await Task.Run<WfExecutedResult>(() =>
            {
                return SendBackProcess(conn, runner, trans);
            });
            return task;
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
                WfRuntimeManagerFactory.RegisterEvent(runtimeInstance, 
                    runtimeInstance_OnWfProcessReversing,
                    runtimeInstance_OnWfProcessReversed);
                bool isReversed = runtimeInstance.Execute(session);

                waitHandler.WaitOne();
            }
            catch (System.Exception e)
            {
                reversedResult.Status = WfExecutedStatus.Failed;
                var error = e.InnerException != null ? e.InnerException.Message : e.Message;
                reversedResult.Message = string.Format("流程返签时发生异常！详细错误：{0}", error);
                LogManager.RecordLog(WfDefine.WF_PROCESS_REVERSE_ERROR, LogEventType.Error, LogPriority.High, runner, e);
            }
            finally
            {
                //卸载事件
                WfRuntimeManagerFactory.UnregisterEvent(runtimeInstance, 
                    runtimeInstance_OnWfProcessReversing,
                    runtimeInstance_OnWfProcessReversed);
                waitHandler.Dispose();
            }
            return reversedResult;

            void runtimeInstance_OnWfProcessReversing(object sender, WfEventArgs args)
            {
                Delegate.DelegateExecutor.InvokeExternalDelegate(session,
                    Delegate.EventFireTypeEnum.OnProcessReversing,
                    runner.DelegateEventList,
                    runtimeInstance.ProcessInstanceID);
            }

            void runtimeInstance_OnWfProcessReversed(object sender, WfEventArgs args)
            {
                reversedResult = args.WfExecutedResult;
                if (reversedResult.Status == WfExecutedStatus.Success)
                {
                    Delegate.DelegateExecutor.InvokeExternalDelegate(session,
                        Delegate.EventFireTypeEnum.OnProcessReversed,
                        runner.DelegateEventList,
                        runtimeInstance.ProcessInstanceID);
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
        /// 异步返签流程
        /// </summary>
        /// <param name="runner">执行人</param>
        /// <returns>执行结果</returns>
        public async Task<WfExecutedResult> ReverseProcessAsync(WfAppRunner runner)
        {
            var task = await Task.Run<WfExecutedResult>(() =>
            {
                return ReverseProcess(runner);
            });
            return task;
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

        /// <summary>
        /// 异步返签流程
        /// </summary>
        /// <param name="conn">连接</param>
        /// <param name="runner">运行人</param>
        /// <param name="trans">事务</param>
        /// <returns>执行结果</returns>
        public async Task<WfExecutedResult> ReverseProcessAsync(IDbConnection conn, WfAppRunner runner, IDbTransaction trans)
        {
            var task = await Task.Run<WfExecutedResult>(() =>
            {
                return ReverseProcess(conn, runner, trans);
            });
            return task;
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
                WfRuntimeManagerFactory.RegisterEvent(runtimeInstance, 
                    runtimeInstance_OnWfProcessJumping,
                    runtimeInstance_OnWfProcessJumped);          
                bool isJumped = runtimeInstance.Execute(session);

                waitHandler.WaitOne();
            }
            catch (System.Exception e)
            {
                jumpResult.ExceptionType = WfExceptionType.Jump_OtherError;
                var error = e.InnerException != null ? e.InnerException.Message : e.Message;
                jumpResult.Message = string.Format("流程跳转时发生异常！详细错误：{0}", error);
                LogManager.RecordLog(WfDefine.WF_PROCESS_JUMP_ERROR, LogEventType.Error, LogPriority.High, runner, e);
            }
            finally
            {
                //卸载事件
                WfRuntimeManagerFactory.UnregisterEvent(runtimeInstance, 
                    runtimeInstance_OnWfProcessJumping,
                    runtimeInstance_OnWfProcessJumped);
                waitHandler.Dispose();
            }
            return jumpResult;

            void runtimeInstance_OnWfProcessJumping(object sender, WfEventArgs args)
            {
                Delegate.DelegateExecutor.InvokeExternalDelegate(session,
                    Delegate.EventFireTypeEnum.OnProcessJumping,
                    runner.DelegateEventList,
                    runtimeInstance.ProcessInstanceID);
            }

            void runtimeInstance_OnWfProcessJumped(object sender, WfEventArgs args)
            {
                jumpResult = args.WfExecutedResult;
                if (jumpResult.Status == WfExecutedStatus.Success)
                {
                    Delegate.DelegateExecutor.InvokeExternalDelegate(session,
                        Delegate.EventFireTypeEnum.OnProcessJumped,
                        runner.DelegateEventList,
                        runtimeInstance.ProcessInstanceID);
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
        /// 异步流程跳转
        /// </summary>
        /// <param name="runner">执行操作人</param>
        /// <param name="jumpOption">跳转选项</param>
        /// <returns>跳转结果</returns>
        public async Task<WfExecutedResult> JumpProcessAsync(WfAppRunner runner, JumpOptionEnum jumpOption = JumpOptionEnum.Default)
        {
            var task = await Task.Run<WfExecutedResult>(() =>
            {
                return JumpProcess(runner, jumpOption);
            });
            return task;
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
        /// 异步流程跳转
        /// </summary>
        /// <param name="conn">连接</param>
        /// <param name="runner">执行操作人</param>
        /// <param name="trans">事务</param>
        /// <param name="jumpOption">跳转选项</param>
        /// <returns>跳转结果</returns>
        public async Task<WfExecutedResult> JumpProcessAsync(IDbConnection conn, WfAppRunner runner, IDbTransaction trans,
            JumpOptionEnum jumpOption = JumpOptionEnum.Default)
        {
            var task = await Task.Run<WfExecutedResult>(() =>
            {
                return JumpProcess(conn, runner, trans, jumpOption);
            });
            return task;
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
        /// 更新任务邮件发送状态
        /// </summary>
        /// <param name="taskID">任务ID</param>
        public Boolean SetTaskEMailSent(int taskID)
        {
            bool isSetOK = false;
            try
            {
                var taskManager = new TaskManager();
                taskManager.SetTaskEMailSent(taskID);
                isSetOK = true;
            }
            catch (System.Exception)
            {
                throw;
            }

            return isSetOK;
        }

        /// <summary>
        /// 获取任务视图
        /// </summary>
        /// <param name="taskID">任务ID</param>
        /// <returns>任务视图</returns>
        public TaskViewEntity GetTaskView(int taskID)
        {
            var tm = new TaskManager();
            var entity = tm.GetTaskView(taskID);
            return entity;
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
        /// 获取活动实例下的第一个任务记录
        /// </summary>
        /// <param name="activityInstanceID">活动实例</param>
        /// <returns>任务视图</returns>
        public TaskViewEntity GetFirstRunningTask(int activityInstanceID)
        {
            var taskManager = new TaskManager();
            var taskView = taskManager.GetFirstRunningTask(activityInstanceID);
            return taskView;
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

        /// <summary>
        /// 获取未发送邮件通知的待办任务列表
        /// </summary>
        /// <returns></returns>
        public IList<TaskViewEntity> GetTaskListEMailUnSent()
        {
            var taskManager = new TaskManager();
            var taskList = taskManager.GetTaskListEMailUnSent();
            return taskList;
        }
        #endregion 
    }
}