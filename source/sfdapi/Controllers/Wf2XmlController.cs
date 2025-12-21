using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using sfdapi.Services;
using Slickflow.Data;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Common;
using Slickflow.Engine.Core.Result;
using Slickflow.Engine.Service;
using Slickflow.Engine.Utility;
using Slickflow.Engine.Xpdl.Common;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Engine.Xpdl.Template;
using Slickflow.Module.Form;
using Slickflow.Module.Localize;
using Slickflow.Module.Resource;
using Slickflow.WebUtility;

namespace sfdapi.Controllers
{
    /// <summary>
    /// Workflow xml controller
    /// 流程定义XML操作控制器
    /// </summary>
    public class Wf2XmlController : Controller
    {
        #region Propery and Construct
        private IWorkflowService _workflowService;
        private SfBpmnServiceBuilder _bpmnServiceBuilder;
        public Wf2XmlController(IWorkflowService workflowService,
            SfBpmnServiceBuilder builder)
        {
            _workflowService = workflowService;
            _bpmnServiceBuilder = builder;
        }
        #endregion

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

        #region Process Definition
        /// <summary>
        /// Create Process
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

                //创建新流程,ProcessID默认赋值
                ////Create a new process, with ProcessId assigned by default
                if (string.IsNullOrEmpty(fileEntity.ProcessId))
                {
                    fileEntity.ProcessId = Guid.NewGuid().ToString();
                }

                if (string.IsNullOrEmpty(fileEntity.Version))
                {
                    fileEntity.Version = "1";
                }

                //根据模板类型来创建流程
                //Create processes based on template types
                ProcessEntity entity = new ProcessEntity
                {
                    ProcessId = fileEntity.ProcessId,
                    ProcessName = fileEntity.ProcessName,
                    ProcessCode = fileEntity.ProcessCode,
                    Version = fileEntity.Version,
                    Status = fileEntity.Status,
                    Description = fileEntity.Description,
                };

                //生成XML内容
                //Generate XML content
                fileEntity = ProcessXmlInitializer.InitNewBPMNFileBlank(fileEntity);
                entity.XmlContent = fileEntity.XmlContent;

                //插入XML文档
                //Insert XML document
                var processId = _workflowService.CreateProcess(entity);
                entity.Id = processId;

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
        /// Update process
        /// 更新流程数据
        /// </summary>
        [HttpPost]
        public ResponseResult UpdateProcess([FromBody] ProcessEntity entity)
        {
            var result = ResponseResult.Default();
            try
            {
                var wfService = new WorkflowService();
                var processEntity = wfService.GetProcessByVersion(entity.ProcessId, entity.Version);
                processEntity.ProcessName = entity.ProcessName;
                processEntity.ProcessCode = entity.ProcessCode;
                processEntity.XmlFileName = entity.XmlFileName;
                processEntity.AppType = entity.AppType;
                processEntity.Description = entity.Description;
                processEntity.Status = entity.Status;
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
        /// Update process using state
        /// 更新流程使用状态
        /// </summary>
        [HttpPost]
        public ResponseResult UpdateProcessUsingState([FromBody] ProcessEntity entity)
        {
            var result = ResponseResult.Default();
            try
            {
                _workflowService.UpdateProcessUsingState(entity.ProcessId, entity.Version, entity.Status);

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
        /// Upgrade process
        /// 更新流程数据
        /// </summary>
        [HttpPost]
        public ResponseResult UpgradeProcess([FromBody] ProcessEntity entity)
        {
            var result = ResponseResult.Default();
            try
            {
                var process = _workflowService.GetProcessById(entity.Id);
                int newVersion = 1;
                var parsed = int.TryParse(process.Version, out newVersion);
                if (parsed == true) newVersion = newVersion + 1;
                _workflowService.UpgradeProcess(process.ProcessId, process.Version, newVersion.ToString());
               
                result = ResponseResult.Success(LocalizeHelper.GetDesignerMessage("wf2xmlcontroller.upgradeprocess.success"));
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(LocalizeHelper.GetDesignerMessage("wf2xmlcontroller.upgradeprocess.error", ex.Message));
            }
            return result;
        }

        /// <summary>
        /// Delete process
        /// 删除流程数据
        /// </summary>
        [HttpPost]
        public ResponseResult DeleteProcess([FromBody] ProcessEntity entity)
        {
            var result = ResponseResult.Default();
            try
            {
                var wfService = new WorkflowService();
                wfService.DeleteProcess(entity.ProcessId, entity.Version);

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
        /// Delete process
        /// 删除流程数据
        /// </summary>
        [HttpGet]
        public ResponseResult DeleteProcessById(int id)
        {
            var result = ResponseResult.Default();
            try
            {
                _workflowService.DeleteProcess(id);
                result = ResponseResult.Success(LocalizeHelper.GetDesignerMessage("wf2xmlcontroller.deleteprocess.success")
                );
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(LocalizeHelper.GetDesignerMessage("wf2xmlcontroller.deleteprocess.error", ex.Message));
            }
            return result;
        }

        #endregion

        #region Read process data
        /// <summary>
        /// Query process by name
        /// 根据流程名称获取流程实体
        /// </summary>
        [HttpPost]
        public ResponseResult<ProcessEntity> QueryProcessByName([FromBody] ProcessQuery query)
        {
            var result = ResponseResult<ProcessEntity>.Default();
            try
            {
                var entity = _workflowService.GetProcessByName(query.ProcessName, query.Version);

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
        /// Query process file
        /// 读取XML文件
        /// </summary>
        [HttpPost]
        public ResponseResult<ProcessFileEntity> QueryProcessFile([FromBody] ProcessFileQuery query)
        {
            var result = ResponseResult<ProcessFileEntity>.Default();
            try
            {
                var entity = _workflowService.GetProcessFile(query.ProcessId, query.Version);

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
        /// Query process file
        /// 读取XML文件
        /// </summary>
        [HttpPost]
        public ResponseResult<Activity> GetFirstActivity([FromBody] ProcessFileQuery query)
        {
            var result = ResponseResult<Activity>.Default();
            try
            {
                var entity = _workflowService.GetFirstActivity(query.ProcessId, query.Version);

                result = ResponseResult<Activity>.Success(entity);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<Activity>.Error(LocalizeHelper.GetDesignerMessage("wf2xmlcontroller.getfirstactivity.error", ex.Message)
                );
            }
            return result;
        }

        /// <summary>
        /// Check if the process file exists
        /// 检查流程文件是否重复
        /// </summary>
        [HttpPost]
        public ResponseResult<ProcessEntity> CheckProcessFile([FromBody] ProcessEntity query)
        {
            var result = ResponseResult<ProcessEntity>.Default();
            try
            {
                var entity = _workflowService.GetProcessByName(query.ProcessName, query.Version);
                if (entity == null)
                {
                    entity = _workflowService.GetProcessByCode(query.ProcessCode, query.Version);
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
        /// Query process file by id
        /// 查询流程文件
        /// </summary>
        [HttpGet]
        public ResponseResult<ProcessFileEntity> GetProcessFile(int id)
        {
            var result = ResponseResult<ProcessFileEntity>.Default();
            try
            {
                var entity = _workflowService.GetProcessFileById(id);

                result = ResponseResult<ProcessFileEntity>.Success(entity);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<ProcessFileEntity>.Error(LocalizeHelper.GetDesignerMessage("wf2xmlcontroller.queryprocessfile.error", ex.Message));
            }
            return result;
        }

        /// <summary>
        /// Get process by version
        /// 根据版本获取流程记录
        /// </summary>
        [HttpPost]
        public ResponseResult<ProcessEntity> GetProcessByVersion([FromBody] ProcessEntity obj)
        {
            var result = ResponseResult<ProcessEntity>.Default();
            try
            {
                var entity = _workflowService.GetProcessByVersion(obj.ProcessId, obj.Version);

                result = ResponseResult<ProcessEntity>.Success(entity);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<ProcessEntity>.Error(LocalizeHelper.GetDesignerMessage("wf2xmlcontroller.queryprocess.error", ex.Message));
            }
            return result;
        } 

        /// <summary>
        /// Get process list with simple info
        /// 获取流程记录列表
        /// </summary>
        [HttpGet]
        public ResponseResult<List<ProcessEntity>> GetProcessListSimple()
        {
            var result = ResponseResult<List<ProcessEntity>>.Default();
            try
            {
                var entity = _workflowService.GetProcessListSimple().ToList();

                result = ResponseResult<List<ProcessEntity>>.Success(entity);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<List<ProcessEntity>>.Error(LocalizeHelper.GetDesignerMessage("wf2xmlcontroller.getprocesslist.error", ex.Message));
            }
            return result;
        }

        /// <summary>
        /// Get process
        /// 获取当前使用版本的流程记录
        /// </summary>
        [HttpPost]
        public ResponseResult<ProcessEntity> GetProcess([FromBody] ProcessQuery query)
        {
            var result = ResponseResult<ProcessEntity>.Default();
            try
            {
                var entity = _workflowService.GetProcessUsing(query.ProcessId);

                result = ResponseResult<ProcessEntity>.Success(entity);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<ProcessEntity>.Error(LocalizeHelper.GetDesignerMessage("wf2xmlcontroller.queryprocess.error", ex.Message));
            }
            return result;
        }

        /// <summary>
        /// Initialize bpmn file
        /// 初始化流程XML文件
        /// </summary>
        [HttpGet]
        public ResponseResult<ProcessFileEntity> InitNewBPMNFile()
        {
            var result = new ResponseResult<ProcessFileEntity>();
            try
            {
                var entity = ProcessXmlInitializer.InitNewBPMNFile();
                result = ResponseResult<ProcessFileEntity>.Success(entity);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<ProcessFileEntity>.Error(LocalizeHelper.GetDesignerMessage("wf2xmlcontroller.initnewbpmnfile.error", ex.Message));
            }
            return result;
        }

        /// <summary>
        /// Save process file
        /// 保存XML文件
        /// </summary>
        [HttpPost]
        public ResponseResult SaveProcessFile([FromBody] ProcessFileEntity entity)
        {
            var result = ResponseResult.Default();
            try
            {
                entity.XmlContent = PaddingContentWithRightSpace(entity.XmlContent);
                _workflowService.SaveProcessFile(entity);

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
        /// Validate process
        /// 校验流程有效性
        /// </summary>
        [HttpPost]
        public ResponseResult<ProcessValidateResult> ValidateProcess([FromBody] ProcessEntity entity)
        {
            var result = ResponseResult<ProcessValidateResult>.Default();
            try
            {
                var validateResult = _workflowService.ValidateProcess(entity);

                result = ResponseResult<ProcessValidateResult>.Success(validateResult, LocalizeHelper.GetDesignerMessage("wf2xmlcontroller.validateprocess.success"));
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<ProcessValidateResult>.Error(LocalizeHelper.GetDesignerMessage("wf2xmlcontroller.validateprocess.error", ex.Message));
            }
            return result;
        }
        #endregion

        #region Task Data
        /// <summary>
        /// Query ready activity instance
        /// 获取待办状态的节点
        /// </summary>
        [HttpPost]
        public ResponseResult<List<ActivityInstanceEntity>> QueryReadyActivityInstance([FromBody] TaskQuery query)
        {
            var result = ResponseResult<List<ActivityInstanceEntity>>.Default();
            try
            {
                var itemList = _workflowService.GetRunningActivityInstance(query).ToList();


                result = ResponseResult<List<ActivityInstanceEntity>>.Success(itemList);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<List<ActivityInstanceEntity>>.Error(LocalizeHelper.GetDesignerMessage("wf2xmlcontroller.queryreadyactivityinstance.error", ex.Message));
            }
            return result;
        }

        /// <summary>
        /// Query completed transiton instance
        /// 查询已完成转移数据
        /// </summary>
        [HttpPost]
        public ResponseResult<List<TransitionImage>> QueryCompletedTransitionInstance([FromBody] TransitionInstanceQuery query)
        {
            var result = ResponseResult<List<TransitionImage>>.Default();
            try
            {
                var itemList = _workflowService.GetTransitionInstanceList(query).ToList();

                result = ResponseResult<List<TransitionImage>>.Success(itemList);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<List<TransitionImage>>.Error(LocalizeHelper.GetDesignerMessage("wf2xmlcontroller.querycompletedtransitioninstance.error", ex.Message));
            }
            return result;
        }

        /// <summary>
        /// Query completed tasks
        /// 获取完成状态的任务
        /// </summary>
        [HttpPost]
        public ResponseResult<List<TaskViewEntity>> QueryCompletedTasks([FromBody] TaskQuery query)
        {
            var result = ResponseResult<List<TaskViewEntity>>.Default();
            try
            {
                var taskList = new List<TaskViewEntity>();
                var itemList = _workflowService.GetCompletedTasks(query);

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

        #region Role User Resource Data
        /// <summary>
        /// Get Role all
        /// 获取所有角色数据集
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult<List<Role>> GetRoleAll()
        {
            var result = ResponseResult<List<Role>>.Default();
            try
            {
                var entity = _workflowService.GetRoleAll().ToList();

                result = ResponseResult<List<Role>>.Success(entity);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<List<Role>>.Error(LocalizeHelper.GetDesignerMessage("wf2xmlcontroller.getroleall.error", ex.Message));
            }
            return result;
        }

        /// <summary>
        /// Get user all
        /// 获取所有用户数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult<List<User>> GetUserAll()
        {
            var result = ResponseResult<List<User>>.Default();
            try
            {
                var entity = _workflowService.GetUserAll().ToList();

                result = ResponseResult<List<User>>.Success(entity);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<List<User>>.Error(LocalizeHelper.GetDesignerMessage("wf2xmlcontroller.getuserall.error", ex.Message));
            }
            return result; 
        }
        #endregion

        #region Form Resource Data
        /// <summary>
        /// Get Form All
        /// 表单数据获取
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult<List<Form>> GetFormAll()
        {
            var result = ResponseResult<List<Form>>.Default();
            try
            {
                var entity = _workflowService.GetFormAll().ToList();

                result = ResponseResult<List<Form>>.Success(entity);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<List<Form>>.Error(LocalizeHelper.GetDesignerMessage("wf2xmlcontroller.getformall.error", ex.Message));
            }
            return result;
        }

        /// <summary>
        /// Field node editing permission data acquisition
        /// 字段节点编辑权限数据获取
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult<List<Form>> GetFieldActivityEditInfo([FromBody] FieldQuery query)
        {
            var result = ResponseResult<List<Form>>.Default();
            try
            {
                var entity = _workflowService.GetFormFieldEditComp(query).ToList();

                result = ResponseResult<List<Form>>.Success(entity);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<List<Form>>.Error(LocalizeHelper.GetDesignerMessage("wf2xmlcontroller.getfieldactivityeditbyform.error", ex.Message));
            }
            return result;
        }

        /// <summary>
        /// Edit and save node field permissions
        /// 节点字段权限编辑保存
        /// </summary>
        [HttpPost]
        public ResponseResult SaveFieldActivityEditInfo([FromBody] FieldActivityEditInfo info)
        {
            var result = ResponseResult.Default();
            try
            {
                var formService = new FormService();
                formService.SaveFieldActivityEditInfo(info);

                result = ResponseResult.Success(LocalizeHelper.GetDesignerMessage("wf2xmlcontroller.savefieldactivityeditiinfo.success"));
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(LocalizeHelper.GetDesignerMessage("wf2xmlcontroller.savefieldactivityeditiinfo.error", ex.Message));
            }
            return result;
        }
        #endregion

        #region Process Execution Method
        /// <summary>
        /// Start Process
        /// </summary>
        [HttpPost]
        public async Task<ResponseResult> StartProcess([FromBody] WfAppRunner runner)
        {
            var result = ResponseResult.Default();

            try
            {
                var wfResult = await _workflowService.StartProcessAsync(runner);
                if (wfResult.Status == WfExecutedStatus.Success)
                {
                    result = ResponseResult.Success(wfResult.Message);
                }
                else
                {
                    result = ResponseResult.Error(wfResult.Message);
                }
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(ex.Message);
            }
            return result;
        }


        /// <summary>
        /// Run Process
        /// </summary>
        [HttpPost]
        public async Task<ResponseResult> RunProcess([FromBody] WfAppRunner runner)
        {
            var result = ResponseResult.Default();

            try
            {
                var wfResult = await _workflowService.RunProcessAsync(runner);
                if (wfResult.Status == WfExecutedStatus.Success)
                {
                    result = ResponseResult.Success(wfResult.Message);
                }
                else
                {
                    result = ResponseResult.Error(wfResult.Message);
                }
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// Sign forward process
        /// </summary>
        [HttpPost]
        public async Task<ResponseResult> SignForwardProcess([FromBody] WfAppRunner runner)
        {
            var result = ResponseResult.Default();

            try
            {
                var wfResult = await _workflowService.SignForwardProcessAsync(runner);
                if (wfResult.Status == WfExecutedStatus.Success)
                {
                    result = ResponseResult.Success(wfResult.Message);
                }
                else
                {
                    result = ResponseResult.Error(wfResult.Message);
                }
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// Jump process
        /// </summary>
        [HttpPost]
        public async Task<ResponseResult> JumpProcess([FromBody] WfAppRunner runner)
        {
            var result = ResponseResult.Default();

            try
            {
                var wfResult = await _workflowService.JumpProcessAsync(runner);
                if (wfResult.Status == WfExecutedStatus.Success)
                {
                    result = ResponseResult.Success(wfResult.Message);
                }
                else
                {
                    result = ResponseResult.Error(wfResult.Message);
                }
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// Revise process
        /// </summary>
        [HttpPost]
        public async Task<ResponseResult> ReviseProcess([FromBody] WfAppRunner runner)
        {
            var result = ResponseResult.Default();

            try
            {
                var wfResult = await _workflowService.ReviseProcessAsync(runner);
                if (wfResult.Status == WfExecutedStatus.Success)
                {
                    result = ResponseResult.Success(wfResult.Message);
                }
                else
                {
                    result = ResponseResult.Error(wfResult.Message);
                }
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// Sendback process
        /// </summary>
        [HttpPost]
        public async Task<ResponseResult> SendBackProcess([FromBody] WfAppRunner runner)
        {
            var result = ResponseResult.Default();
            try
            {
                var wfResult = await _workflowService.SendBackProcessAsync(runner);

                if (wfResult.Status == WfExecutedStatus.Success)
                {
                    result = ResponseResult.Success(wfResult.Message);
                }
                else
                {
                    result = ResponseResult.Error(wfResult.Message);
                }
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// Withdraw process
        /// </summary>
        [HttpPost]
        public async Task<ResponseResult> WithdrawProcess([FromBody] WfAppRunner runner)
        {
            var result = ResponseResult.Default();
            try
            {
                var wfResult = await _workflowService.WithdrawProcessAsync(runner);

                if (wfResult.Status == WfExecutedStatus.Success)
                {
                    result = ResponseResult.Success(wfResult.Message);
                }
                else
                {
                    result = ResponseResult.Error(wfResult.Message);
                }
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// Resend process
        /// </summary>
        [HttpPost]
        public async Task<ResponseResult> ResendProcess([FromBody] WfAppRunner runner)
        {
            var result = ResponseResult.Default();

            try
            {
                var wfResult = await _workflowService.ResendProcessAsync(runner);
                if (wfResult.Status == WfExecutedStatus.Success)
                {
                    result = ResponseResult.Success(wfResult.Message);
                }
                else
                {
                    result = ResponseResult.Error(wfResult.Message);
                }
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// Reject process
        /// </summary>
        [HttpPost]
        public async Task<ResponseResult> RejectProcess([FromBody] WfAppRunner runner)
        {
            var result = ResponseResult.Default();

            try
            {
                var wfResult = await _workflowService.RejectProcessAsync(runner);
                if (wfResult.Status == WfExecutedStatus.Success)
                {
                    result = ResponseResult.Success(wfResult.Message);
                }
                else
                {
                    result = ResponseResult.Error(wfResult.Message);
                }
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// Close process
        /// </summary>
        [HttpPost]
        public async Task<ResponseResult> CloseProcess([FromBody] WfAppRunner runner)
        {
            var result = ResponseResult.Default();

            try
            {
                var wfResult = await _workflowService.CloseProcessAsync(runner);
                if (wfResult.Status == WfExecutedStatus.Success)
                {
                    result = ResponseResult.Success(wfResult.Message);
                }
                else
                {
                    result = ResponseResult.Error(wfResult.Message);
                }
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// Delete process instance
        /// </summary>
        [HttpPost]
        public ResponseResult DeleteInstance([FromBody] ProcessQuery query)
        {
            var result = ResponseResult.Default();

            try
            {
                var isOK = _workflowService.DeleteInstanceInt(query.ProcessId, query.Version);

                if (isOK == true)
                {
                    result = ResponseResult.Success(LocalizeHelper.GetWebMessage("wf2xmlcontroller.DeleteInstance.success"));
                }
                else
                {
                    result = ResponseResult.Error(LocalizeHelper.GetWebMessage("wf2xmlcontroller.DeleteInstance.error"));
                }
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(ex.Message);
            }
            return result;
        }
        #endregion

        #region Process Template Service
        /// <summary>
        /// Create a process definition based on the business template name
        /// 根据业务模板名称, 创建流程定义
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResponseResult<ProcessFileEntity>> CreateProcessByTemplate([FromBody] ProcessTemplate processTemplate)
        {
            var result = ResponseResult<ProcessFileEntity>.Default();
            try
            {
                if (string.IsNullOrEmpty(processTemplate.TemplateName.Trim()))
                {
                    result = ResponseResult<ProcessFileEntity>.Error("Please input the template name, it cant be empty");
                    return result;
                }

                var entity = await _bpmnServiceBuilder.CreateBpmnProcessByTemplate(processTemplate.TemplateName);
                result = ResponseResult<ProcessFileEntity>.Success(entity);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<ProcessFileEntity>.Error(LocalizeHelper.GetDesignerMessage("wf2xmlcontroller.crateprocess.error", ex.Message));
            }
            return result;
        }

        /// <summary>
        /// Load process template
        /// 加载流程模板
        /// </summary>
        [HttpGet]
        public ResponseResult<ProcessTemplate> LoadProcessTemplateCode(string id)
        {
            var result = ResponseResult<ProcessTemplate>.Default();
            try
            {
                ProcessTemplate entity = null;
                ProcessTemplateType templateType = ProcessTemplateType.Blank;
                var isOK = Enum.TryParse<ProcessTemplateType>(id, out templateType);
                if (isOK)
                {
                    entity = _bpmnServiceBuilder.LoadTemplateContent(templateType);
                }
                result = ResponseResult<ProcessTemplate>.Success(entity);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<ProcessTemplate>.Error(LocalizeHelper.GetDesignerMessage("wf2xmlcontroller.loadProcesstemplate.error", ex.Message)
                );
            }
            return result;
        }
        #endregion
    }
}