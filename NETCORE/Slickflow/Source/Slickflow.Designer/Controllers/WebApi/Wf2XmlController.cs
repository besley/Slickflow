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
using System.IO;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc;
using SlickOne.WebUtility;
using Slickflow.Module.Resource;
using Slickflow.Engine.Common;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Service;
using Slickflow.Graph;


namespace Slickflow.Designer.Controllers.WebApi
{
    /// <summary>
    /// 流程定义XML操作控制器
    /// </summary>
    public class Wf2XmlController : Controller
    {
        #region Test
        [HttpGet]
        public string Hello()
        {
            return "Hello World!";
        }

        /// <summary>
        /// 节点属性HttpGet调用测试
        /// </summary>
        /// <param name="id">id参数</param>
        /// <returns></returns>
        [HttpGet] 
        public string MultipleG(string id)
        {
            return id;
        }

        /// <summary>
        /// 节点属性HttpPost调用测试
        /// 两个变量是JSON格式：
        /// {
        ///     "runner": {"UserName": "Jack"}, 
        ///     "role":{"RoleName": "Admin"}
        /// }
        /// 备注：可以在WfProcessVariable表中存入测试数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public string MultipleJ([FromBody] JObject data)
        {
            var runner = data["runner"].ToObject<WfAppRunner>().UserName;
            var role = data["role"].ToObject<Role>().RoleName;
            var result = string.Format("runner:{0}, role:{1}", runner, role);

            return result;
        }

        /// <summary>
        /// 节点属性HttpPost调用测试
        /// 两个变量是单纯格式：
        /// {
        ///     "runner": "jack",
        ///     "role": "admin"
        /// }
        /// 备注：可以在WfProcessVariable表中存入测试数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public string MultipleR([FromBody] dynamic data)
        {
            var runner = data.runner.ToString();
            var role = data.role.ToString();

            var result = string.Format("runner:{0}, role:{1}", runner, role);
            return result;
        }
        #endregion

        #region 流程定义数据
        /// <summary>
        /// 创建流程定义
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult<ProcessEntity> CreateProcess([FromBody] ProcessFileEntity fileEntity)
        {
            var result = ResponseResult<ProcessEntity>.Default();
            try
            {
                ProcessEntity entity = null;
                //根据模板类型来创建流程
                if (fileEntity.TemplateType == ProcessTemplateType.Blank)
                {
                    entity = new ProcessEntity
                    {
                        ProcessGUID = fileEntity.ProcessGUID,
                        ProcessName = fileEntity.ProcessName,
                        ProcessCode = fileEntity.ProcessCode,
                        Version = fileEntity.Version,
                        IsUsing = fileEntity.IsUsing,
                        Description = fileEntity.Description,
                    };
                    var wfService = new WorkflowService();
                    var processID = wfService.CreateProcess(entity);
                    entity.ID = processID;
                }
                else
                {
                    //模板默认生成XML内容
                    entity = ProcessGraphFactory.CreateProcess(fileEntity);
                }
                result = ResponseResult<ProcessEntity>.Success(entity);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<ProcessEntity>.Error(string.Format("创建流程记录失败,错误:{0}", ex.Message));
            }
            return result;
        }

        /// <summary>
        /// 更新流程数据
        /// </summary>
        /// <param name="entity">流程实体</param>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult UpdateProcess([FromBody] ProcessEntity entity)
        {
            var result = ResponseResult.Default();
            try
            {
                var wfService = new WorkflowService();
                var processEntity = wfService.GetProcessByVersion(entity.ProcessGUID, entity.Version);
                processEntity.ProcessName = entity.ProcessName;
                processEntity.ProcessCode = entity.ProcessCode;
                processEntity.XmlFileName = entity.XmlFileName;
                processEntity.AppType = entity.AppType;
                processEntity.Description = entity.Description;
                if (!string.IsNullOrEmpty(entity.XmlContent))
                {
                    processEntity.XmlContent = PaddingContentWithRightSpace(entity.XmlContent);
                }
                wfService.UpdateProcess(processEntity);

                result = ResponseResult.Success();
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(string.Format("更新流程记录失败,错误:{0}", ex.Message));
            }
            return result;
        }

        /// <summary>
        /// 删除琉璃厂数据
        /// </summary>
        /// <param name="entity">流程实体</param>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult DeleteProcess([FromBody] ProcessEntity entity)
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
        /// <param name="query">查询实体</param>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult<ProcessFileEntity> QueryProcessFile([FromBody] ProcessFileQuery query)
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
        /// 查询流程文件
        /// </summary>
        /// <param name="query">查询实体</param>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult<ProcessFileEntity> QueryProcessFileByID([FromBody] ProcessFileQuery query)
        {
            var result = ResponseResult<ProcessFileEntity>.Default();
            try
            {
                var wfService = new WorkflowService();
                var entity = wfService.GetProcessFileByID(query.ID);

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
        public ResponseResult<ProcessEntity> GetProcessByVersion([FromBody] ProcessEntity obj)
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
        [HttpPost]
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
        [HttpPost]
        public ResponseResult<ProcessEntity> GetProcess([FromBody] ProcessQuery query)
        {
            var result = ResponseResult<ProcessEntity>.Default();
            try
            {
                var wfService = new WorkflowService();
                var entity = wfService.GetProcess(query.ProcessGUID);

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
        /// <param name="entity">流程文件实体</param>
        /// <returns>响应结果</returns>
        [HttpPost]
        public ResponseResult SaveProcessFile([FromBody] ProcessFileEntity entity)
        {
            var result = ResponseResult.Default();
            try
            {
                var wfService = new WorkflowService();
                entity.XmlContent = PaddingContentWithRightSpace(entity.XmlContent);
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
        /// 补足XmlContent内容（Oracle-01483-error）
        /// ORA-01483: invalid length for DATE or NUMBER
        /// </summary>
        /// <param name="content">原始文本内容</param>
        /// <returns>处理后的文本内容</returns>
        private string PaddingContentWithRightSpace(string content)
        {
            var newContent = content;
            int len = content.Length;
            if (4096 - len > 0)
            {
                newContent = content.PadRight(4096);
            }
            return newContent;
        }

        /// <summary>
        /// 获取待办状态的节点
        /// </summary>
        /// <param name="query">查询实体</param>
        /// <returns>活动实例数据列表</returns>
        [HttpPost]
        public ResponseResult<List<ActivityInstanceEntity>> QueryReadyActivityInstance([FromBody] TaskQuery query)
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
        public ResponseResult<List<TransitionImage>> QueryCompletedTransitionInstance([FromBody] TransitionInstanceQuery query)
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
        public ResponseResult<List<TaskViewEntity>> QueryCompletedTasks([FromBody] TaskQuery query)
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
        [HttpPost]
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