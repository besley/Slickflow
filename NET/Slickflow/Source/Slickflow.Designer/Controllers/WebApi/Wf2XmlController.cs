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
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using System.Xml;
using Slickflow.Module.Resource;
using Slickflow.Engine.Common;
using Slickflow.Engine.Utility;
using Slickflow.Engine.Core;
using Slickflow.Engine.Core.Result;
using Slickflow.Data;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Service;
using Slickflow.Engine.Parser;
using SlickOne.WebUtility;

namespace Slickflow.Designer.Controllers.WebApi
{
    /// <summary>
    /// 流程定义XML操作控制器
    /// </summary>
    public class Wf2XmlController : ApiController
    {
        #region Test
        [HttpGet]
        public string Hello()
        {
            return "Hello World!";
        }
        #endregion

        #region 流程定义数据
        /// <summary>
        /// 创建流程定义
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult<ProcessEntity> CreateProcess(ProcessEntity entity)
        {
            var result = ResponseResult<ProcessEntity>.Default();
            try
            {
                var wfService = new WorkflowService();
                var processID = wfService.CreateProcess(entity);

                entity.ID = processID;

                result = ResponseResult<ProcessEntity>.Success(entity);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<ProcessEntity>.Error(string.Format("创建流程记录失败,错误:{0}", ex.Message));
            }
            return result;
        }

        [HttpPost]
        public ResponseResult UpdateProcess(ProcessEntity entity)
        {
            var result = ResponseResult.Default();
            try
            {
                var wfService = new WorkflowService();
                var processEntity = wfService.GetProcessByVersion(entity.ProcessGUID, entity.Version);
                processEntity.ProcessName = entity.ProcessName;
                processEntity.XmlFileName = entity.XmlFileName;
                processEntity.AppType = entity.AppType;
                processEntity.Description = entity.Description;

                wfService.UpdateProcess(processEntity);

                result = ResponseResult.Success();
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(string.Format("更新流程记录失败,错误:{0}", ex.Message));
            }
            return result;
        }

        [HttpPost]
        public ResponseResult DeleteProcess(ProcessEntity entity)
        {
            var result = ResponseResult.Default();
            try
            {
                var wfService = new WorkflowService();
                wfService.DeleteProcess(entity.ProcessGUID, entity.Version);

                result = ResponseResult.Success();
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(string.Format("删除流程记录失败,错误:{0}", ex.Message));
            }
            return result;
        }
        #endregion

        #region 读取流程XML文件数据处理
        /// <summary>
        /// 读取XML文件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult<ProcessFileEntity> QueryProcessFile(ProcessFileQuery query)
        {
            var result = ResponseResult<ProcessFileEntity>.Default();
            try
            {
                var wfService = new WorkflowService();
                var entity = wfService.GetProcessFile(query.ProcessGUID, query.Version);

                result = ResponseResult<ProcessFileEntity>.Success(entity);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<ProcessFileEntity>.Error(
                    string.Format("获取流程XML文件失败！{0}", ex.Message)
                );
            }
            return result;
        }

        /// <summary>
        /// 根据版本获取流程记录
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult<ProcessEntity> GetProcessByVersion(ProcessEntity obj)
        {
            var result = ResponseResult<ProcessEntity>.Default();
            try
            {
                var wfService = new WorkflowService();
                var entity = wfService.GetProcessByVersion(obj.ProcessGUID, obj.Version);

                result = ResponseResult<ProcessEntity>.Success(entity);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<ProcessEntity>.Error(
                    string.Format("获取流程基本信息失败！{0}", ex.Message)
                );
            }
            return result;
        } 

        /// <summary>
        /// 获取流程记录列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ResponseResult<List<ProcessEntity>> GetProcessListSimple()
        {
            var result = ResponseResult<List<ProcessEntity>>.Default();
            try
            {
                var wfService = new WorkflowService();
                var entity = wfService.GetProcessListSimple().ToList();

                result = ResponseResult<List<ProcessEntity>>.Success(entity);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<List<ProcessEntity>>.Error(
                    string.Format("获取流程基本信息失败！{0}", ex.Message)
                );
            }
            return result;
        }

        /// <summary>
        /// 获取当前使用版本的流程记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ResponseResult<ProcessEntity> GetProcess(string id)
        {
            var result = ResponseResult<ProcessEntity>.Default();
            try
            {
                var wfService = new WorkflowService();
                var entity = wfService.GetProcess(id);

                result = ResponseResult<ProcessEntity>.Success(entity);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<ProcessEntity>.Error(
                    string.Format("获取流程基本信息失败！{0}", ex.Message)
                );
            }
            return result;
        }

        /// <summary>
        /// 保存XML文件
        /// </summary>
        /// <param name="package"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult SaveProcessFile(ProcessFileEntity entity)
        {
            var result = ResponseResult.Default();
            try
            {
                var wfService = new WorkflowService();
                wfService.SaveProcessFile(entity);

                result = ResponseResult.Success();
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(
                    string.Format("保存流程XML文件失败！{0}", ex.Message)
                );
            }
            return result;
        }

        /// <summary>
        /// 获取待办状态的节点
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult<List<ActivityInstanceEntity>> QueryReadyActivityInstance(TaskQuery query)
        {
            var result = ResponseResult<List<ActivityInstanceEntity>>.Default();
            try
            {
                var wfService = new WorkflowService();
                var itemList = wfService.GetRunningActivityInstance(query).ToList();


                result = ResponseResult<List<ActivityInstanceEntity>>.Success(itemList);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<List<ActivityInstanceEntity>>.Error(string.Format(
                    "获取待办任务数据失败, 异常信息:{0}",
                    ex.Message));
            }
            return result;
        }

        /// <summary>
        /// 查询已完成转移数据
        /// </summary>
        /// <param name="query">查询实体</param>
        /// <returns>转移实例列表</returns>
        [HttpPost]
        public ResponseResult<List<TransitionImage>> QueryCompletedTransitionInstance(TransitionInstanceQuery query)
        {
            var result = ResponseResult<List<TransitionImage>>.Default();
            try
            {
                var wfService = new WorkflowService();
                var itemList = wfService.GetTransitionInstanceList(query).ToList();

                result = ResponseResult<List<TransitionImage>>.Success(itemList);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<List<TransitionImage>>.Error(string.Format(
                    "获取已完成转移数据失败, 异常信息:{0}",
                    ex.Message));
            }
            return result;
        }

        /// <summary>
        /// 获取完成状态的任务
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult<List<TaskViewEntity>> QueryCompletedTasks(TaskQuery query)
        {
            var result = ResponseResult<List<TaskViewEntity>>.Default();
            try
            {
                var taskList = new List<TaskViewEntity>();
                var wfService = new WorkflowService();
                var itemList = wfService.GetCompletedTasks(query);

                if (itemList != null)
                {
                    taskList = itemList.ToList();
                }
                result = ResponseResult<List<TaskViewEntity>>.Success(taskList);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<List<TaskViewEntity>>.Error(string.Format(
                    "获取已办任务数据失败, 异常信息:{0}",
                    ex.Message));
            }
            return result;
        }
        #endregion

        #region 角色资源数据获取
        /// <summary>
        /// 获取所有角色数据集
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ResponseResult<List<Role>> GetRoleAll()
        {
            var result = ResponseResult<List<Role>>.Default();
            try
            {
                var wfService = new WorkflowService();
                var entity = wfService.GetRoleAll().ToList();

                result = ResponseResult<List<Role>>.Success(entity);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<List<Role>>.Error(
                    string.Format("获取角色数据失败！{0}", ex.Message)
                );
            }
            return result;
        }
        #endregion

    }
}