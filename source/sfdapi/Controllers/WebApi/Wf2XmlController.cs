﻿using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc;
using Slickflow.Data;
using Slickflow.Module.Localize;
using Slickflow.Module.Resource;
using Slickflow.Engine.Common;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Service;
using Slickflow.Engine.Utility;
using Slickflow.Graph;
using Slickflow.Graph.Roslyn;
using Slickflow.Engine.Xpdl.Common;
using Slickflow.Graph.Common;

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
                if (string.IsNullOrEmpty(fileEntity.ProcessName.Trim())
                    || string.IsNullOrEmpty(fileEntity.ProcessCode.Trim())
                    || string.IsNullOrEmpty(fileEntity.Version.Trim()))
                {
                    result = ResponseResult<ProcessEntity>.Error(LocalizeHelper.GetDesignerMessage("wf2xmlcontroller.crateprocess.warning"));
                    return result;
                }

                //创建新流程,ProcessGUID默认赋值
                if (string.IsNullOrEmpty(fileEntity.ProcessGUID))
                {
                    fileEntity.ProcessGUID = Guid.NewGuid().ToString();
                }

                if (string.IsNullOrEmpty(fileEntity.Version))
                {
                    fileEntity.Version = "1";
                }

                //根据模板类型来创建流程
                ProcessEntity entity = new ProcessEntity
                {
                    ProcessGUID = fileEntity.ProcessGUID,
                    ProcessName = fileEntity.ProcessName,
                    ProcessCode = fileEntity.ProcessCode,
                    Version = fileEntity.Version,
                    IsUsing = fileEntity.IsUsing,
                    Description = fileEntity.Description,
                };

                //生成XML内容
                fileEntity = ProcessXmlBuilder.InitNewBPMNFileBlank(fileEntity);
                entity.XmlContent = fileEntity.XmlContent;

                //插入XML文档
                var wfService = new WorkflowService();
                var processID = wfService.CreateProcess(entity);
                entity.ID = processID;

                result = ResponseResult<ProcessEntity>.Success(entity,
                    LocalizeHelper.GetDesignerMessage("wf2xmlcontroller.crateprocess.success")
                );
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<ProcessEntity>.Error(LocalizeHelper.GetDesignerMessage("wf2xmlcontroller.crateprocess.error", ex.Message));
            }
            return result;
        }

        /// <summary>
        /// 根据业务模板名称, 创建流程定义
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult<ProcessEntity> CreateProcessByTemplateName([FromBody] ProcessTemplate processTemplate)
        {
            var result = ResponseResult<ProcessEntity>.Default();
            try
            {
                if (string.IsNullOrEmpty(processTemplate.TemplateName.Trim()))
                {
                    result = ResponseResult<ProcessEntity>.Error(LocalizeHelper.GetDesignerMessage("wf2xmlcontroller.crateprocess.warning"));
                    return result;
                }

                var entity = ProcessFactory.CreateProcessByTemplateName(processTemplate);
                result = ResponseResult<ProcessEntity>.Success(entity,
                    LocalizeHelper.GetDesignerMessage("wf2xmlcontroller.crateprocess.success")
                );
            }
            catch(System.Exception ex)
            {
                result = ResponseResult<ProcessEntity>.Error(LocalizeHelper.GetDesignerMessage("wf2xmlcontroller.crateprocess.error", ex.Message));
            }
            return result;
        }

        /// <summary>
        /// 代码创建流程图
        /// </summary>
        /// <param name="graph">代码文本</param>
        /// <returns>创建结果</returns>
        [HttpPost]
        public ResponseResult<ProcessEntity> ExecuteProcessGraph([FromBody] ProcessGraph graph)
        {
            var result = ResponseResult<ProcessEntity>.Default();
            try
            {
                var roslynBuilder = new RoslynBuilder();
                var roslynResult = roslynBuilder.Execute(graph);
                if (roslynResult.Status == 1)
                {
                    result = ResponseResult<ProcessEntity>.Success(roslynResult.Process,
                        LocalizeHelper.GetDesignerMessage("wf2xmlcontroller.executeprocessgraph.success")
                    );
                }
                else
                {
                    result = ResponseResult<ProcessEntity>.Error(roslynResult.Message);
                }
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<ProcessEntity>.Error(LocalizeHelper.GetDesignerMessage("wf2xmlcontroller.executeprocessgraph.error", ex.Message));
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
                processEntity.IsUsing = entity.IsUsing;
                if (!string.IsNullOrEmpty(entity.XmlContent))
                {
                    processEntity.XmlContent = PaddingContentWithRightSpace(entity.XmlContent);
                }
                wfService.UpdateProcess(processEntity);

                result = ResponseResult.Success(LocalizeHelper.GetDesignerMessage("wf2xmlcontroller.updateprocess.success")
                );
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(LocalizeHelper.GetDesignerMessage("wf2xmlcontroller.updateprocess.error", ex.Message));
            }
            return result;
        }

        /// <summary>
        /// 更新流程使用状态
        /// </summary>
        /// <param name="entity">流程实体</param>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult UpdateProcessUsingState([FromBody] ProcessEntity entity)
        {
            var result = ResponseResult.Default();
            try
            {
                var wfService = new WorkflowService();
                wfService.UpdateProcessUsingState(entity.ProcessGUID, entity.Version, entity.IsUsing);

                result = ResponseResult.Success(LocalizeHelper.GetDesignerMessage("wf2xmlcontroller.updateprocessusingstate.success")
                );
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(LocalizeHelper.GetDesignerMessage("wf2xmlcontroller.updateprocessusingstate.error", ex.Message));
            }
            return result;
        }



        /// <summary>
        /// 更新流程数据
        /// </summary>
        /// <param name="entity">流程实体</param>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult UpgradeProcess([FromBody] ProcessEntity entity)
        {
            var result = ResponseResult.Default();
            try
            {
                var wfService = new WorkflowService();
                var process = wfService.GetProcessByID(entity.ID);
                int newVersion = 1;
                var parsed = int.TryParse(process.Version, out newVersion);
                if (parsed == true) newVersion = newVersion + 1;
                wfService.UpgradeProcess(process.ProcessGUID, process.Version, newVersion.ToString());
               
                result = ResponseResult.Success(LocalizeHelper.GetDesignerMessage("wf2xmlcontroller.upgradeprocess.success"));
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(LocalizeHelper.GetDesignerMessage("wf2xmlcontroller.upgradeprocess.error", ex.Message));
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

                result = ResponseResult.Success(LocalizeHelper.GetDesignerMessage("wf2xmlcontroller.deleteprocess.success")
                );
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(LocalizeHelper.GetDesignerMessage("wf2xmlcontroller.deleteprocess.error", ex.Message));
            }
            return result;
        }

        /// <summary>
        /// 加载流程模板
        /// </summary>
        /// <param name="id">模板类型</param>
        /// <returns>模板内容</returns>
        [HttpGet]
        public ResponseResult<ProcessTemplate> LoadProcessTemplate(string id)
        {
            var result = ResponseResult<ProcessTemplate>.Default();
            try
            {
                ProcessTemplate entity = null;
                ProcessTemplateType templateType = ProcessTemplateType.Blank;
                var isOK = Enum.TryParse<ProcessTemplateType>(id, out templateType);
                if (isOK)
                {
                    entity = ProcessTemplateFactory.LoadTemplateContent(templateType);
                }
                result = ResponseResult<ProcessTemplate>.Success(entity, LocalizeHelper.GetDesignerMessage("wf2xmlcontroller.loadProcesstemplate.success")
                );
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<ProcessTemplate>.Error(LocalizeHelper.GetDesignerMessage("wf2xmlcontroller.loadProcesstemplate.error", ex.Message)
                );
            }
            return result;
        }
        
        #endregion

        #region 读取流程XML文件数据处理
        /// <summary>
        /// 根据流程名称获取流程实体
        /// </summary>
        /// <param name="query">查询实体</param>
        /// <returns>流程实体</returns>
        [HttpPost]
        public ResponseResult<ProcessEntity> QueryProcessByName([FromBody] ProcessQuery query)
        {
            var result = ResponseResult<ProcessEntity>.Default();
            try
            {
                var wfService = new WorkflowService();
                var entity = wfService.GetProcessByName(query.ProcessName, query.Version);

                result = ResponseResult<ProcessEntity>.Success(entity);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<ProcessEntity>.Error(LocalizeHelper.GetDesignerMessage("wf2xmlcontroller.queryprocess.error", ex.Message)
                );
            }
            return result;
        }

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
                result = ResponseResult<ProcessFileEntity>.Error(LocalizeHelper.GetDesignerMessage("wf2xmlcontroller.queryprocessfile.error", ex.Message)
                );
            }
            return result;
        }

        /// <summary>
        /// 检查流程文件是否重复
        /// </summary>
        /// <param name="query">查询实体</param>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult<ProcessEntity> CheckProcessFile([FromBody] ProcessEntity query)
        {
            var result = ResponseResult<ProcessEntity>.Default();
            try
            {
                var wfService = new WorkflowService();
                var entity = wfService.GetProcessByName(query.ProcessName, query.Version);
                if (entity == null)
                {
                    entity = wfService.GetProcessByCode(query.ProcessCode, query.Version);
                }
                result = ResponseResult<ProcessEntity>.Success(entity);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<ProcessEntity>.Error(LocalizeHelper.GetDesignerMessage("wf2xmlcontroller.queryprocessfile.error", ex.Message)
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
                result = ResponseResult<ProcessFileEntity>.Error(LocalizeHelper.GetDesignerMessage("wf2xmlcontroller.queryprocessfile.error", ex.Message));
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
                result = ResponseResult<ProcessEntity>.Error(LocalizeHelper.GetDesignerMessage("wf2xmlcontroller.queryprocess.error", ex.Message));
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
                result = ResponseResult<List<ProcessEntity>>.Error(LocalizeHelper.GetDesignerMessage("wf2xmlcontroller.getprocesslist.error", ex.Message));
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
                var entity = wfService.GetProcessUsing(query.ProcessGUID);

                result = ResponseResult<ProcessEntity>.Success(entity);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<ProcessEntity>.Error(LocalizeHelper.GetDesignerMessage("wf2xmlcontroller.queryprocess.error", ex.Message));
            }
            return result;
        }

        /// <summary>
        /// 初始化流程XML文件
        /// </summary>
        /// <returns>流程XML</returns>
        [HttpGet]
        public ResponseResult<ProcessFileEntity> InitNewBPMNFile()
        {
            var result = new ResponseResult<ProcessFileEntity>();
            try
            {
                var entity = ProcessXmlBuilder.InitNewBPMNFile();
                result = ResponseResult<ProcessFileEntity>.Success(entity);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<ProcessFileEntity>.Error(LocalizeHelper.GetDesignerMessage("wf2xmlcontroller.initnewbpmnfile.error", ex.Message));
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

                result = ResponseResult.Success(LocalizeHelper.GetDesignerMessage("wf2xmlcontroller.saveprocessfile.success"));
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(LocalizeHelper.GetDesignerMessage("wf2xmlcontroller.saveprocessfile.error", ex.Message));
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
        /// 校验流程有效性
        /// </summary>
        /// <param name="entity">校验实体</param>
        /// <returns>校验结果对象</returns>
        [HttpPost]
        public ResponseResult<ProcessValidateResult> ValidateProcess([FromBody] ProcessEntity entity)
        {
            var result = ResponseResult<ProcessValidateResult>.Default();
            try
            {
                var wfService = new WorkflowService();
                var validateResult = wfService.ValidateProcess(entity);

                result = ResponseResult<ProcessValidateResult>.Success(validateResult, LocalizeHelper.GetDesignerMessage("wf2xmlcontroller.validateprocess.success"));
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<ProcessValidateResult>.Error(LocalizeHelper.GetDesignerMessage("wf2xmlcontroller.validateprocess.error", ex.Message));
            }
            return result;
        }
        #endregion

        #region 任务数据记录
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
                result = ResponseResult<List<ActivityInstanceEntity>>.Error(LocalizeHelper.GetDesignerMessage("wf2xmlcontroller.queryreadyactivityinstance.error", ex.Message));
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
                result = ResponseResult<List<TransitionImage>>.Error(LocalizeHelper.GetDesignerMessage("wf2xmlcontroller.querycompletedtransitioninstance.error", ex.Message));
            }
            return result;
        }

        /// <summary>
        /// 获取完成状态的任务
        /// </summary>
        /// <param name="query">任务查询实体</param>
        /// <returns>任务列表</returns>
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
                result = ResponseResult<List<TaskViewEntity>>.Error(LocalizeHelper.GetDesignerMessage("wf2xmlcontroller.querycompletedtasks.error", ex.Message));
            }
            return result;
        }

        /// <summary>
        /// Get Task todo list Top 10
        /// </summary>
        /// <returns>Task List</returns>
        [HttpGet]
        public ResponseResult<List<TaskViewEntity>> GetTaskToDoListTop()
        {
            var result = ResponseResult<List<TaskViewEntity>>.Default();
            try
            {
                var tm = new TaskManager();
                var taskList = tm.GetTaskToDoListTop();
                result = ResponseResult<List<TaskViewEntity>>.Success(taskList);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<List<TaskViewEntity>>.Error(LocalizeHelper.GetDesignerMessage("wf2xmlcontroller.gettasktodolisttop.error", ex.Message));
            }
            return result;
        }

        /// <summary>
        /// Get Task Done List top 10
        /// </summary>
        /// <returns>task list</returns>
        [HttpGet]
        public ResponseResult<List<TaskViewEntity>> GetTaskDoneListTop()
        {
            var result = ResponseResult<List<TaskViewEntity>>.Default();
            try
            {
                var tm = new TaskManager();
                var taskList = tm.GetTaskDoneListTop();
                result = ResponseResult<List<TaskViewEntity>>.Success(taskList);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<List<TaskViewEntity>>.Error(LocalizeHelper.GetDesignerMessage("wf2xmlcontroller.gettaskdonelisttop.error", ex.Message));
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
                result = ResponseResult<List<Role>>.Error(LocalizeHelper.GetDesignerMessage("wf2xmlcontroller.getroleall.error", ex.Message));
            }
            return result;
        }

        /// <summary>
        /// 获取所有用户数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult<List<User>> GetUserAll()
        {
            var result = ResponseResult<List<User>>.Default();
            try
            {
                var wfService = new WorkflowService();
                var entity = wfService.GetUserAll().ToList();

                result = ResponseResult<List<User>>.Success(entity);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<List<User>>.Error(LocalizeHelper.GetDesignerMessage("wf2xmlcontroller.getuserall.error", ex.Message));
            }
            return result; 
        }
        #endregion
    }
}