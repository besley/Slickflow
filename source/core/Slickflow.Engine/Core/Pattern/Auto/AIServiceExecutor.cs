using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Slickflow.Engine.Common;
using Slickflow.Engine.Delegate;
using Slickflow.Engine.Xpdl.Common;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Utility;
using Slickflow.Module.Localize;
using Slickflow.AI.Service;
using Slickflow.WebUtility;

namespace Slickflow.Engine.Core.Pattern.Auto
{
    internal class AIServiceExecutor
    {
        /// <summary>
        /// Execute service list
        /// </summary>
        internal static void ExecuteAIServiceList(Activity fromActivity,
            ActivityInstanceEntity fromActivityInstance,
            Activity currentActivity,
            ActivityInstanceEntity currentActivityInstance,
            ActivityForwardContext activityForwardContext,
            IDelegateService delegateService)
        {
            IList<AIServiceDetail> aiServiceList = currentActivity.AIServiceList;
            if (aiServiceList != null && aiServiceList.Count > 0)
            {
                foreach (var service in aiServiceList)
                {
                    if (service.AIServiceType != AIServiceTypeEnum.None)
                    {
                        Execute(fromActivity, fromActivityInstance, currentActivity, service, currentActivityInstance, activityForwardContext, delegateService).GetAwaiter().GetResult();
                    }
                }
            }
        }

        /// <summary>
        /// Execute
        /// </summary>
        private static async Task Execute(Activity fromActivity,
            ActivityInstanceEntity fromActivityInstance,
            Activity currentActivity,
            AIServiceDetail service,
            ActivityInstanceEntity currentActivityInstance,
            ActivityForwardContext activityForwardContext,
            IDelegateService delegateService)
        {
            if (service.AIServiceType == AIServiceTypeEnum.LLM)
            {
                await ExecuteLlmService(fromActivity, fromActivityInstance, currentActivity, service, currentActivityInstance, activityForwardContext, delegateService);
            }
            else if (service.AIServiceType == AIServiceTypeEnum.PlugIn)
            {
                await ExecutePlugInService(fromActivity, fromActivityInstance, currentActivity, service, currentActivityInstance, activityForwardContext, delegateService);
            }
            else
            {
                throw new WorkflowException(LocalizeHelper.GetEngineMessage("actionexecutor.Execute.exception", service.AIServiceType.ToString()));
            }
        }

        /// <summary>
        /// Execute Local Service
        /// </summary>
        private static async Task ExecuteLlmService(Activity fromActivity,
            ActivityInstanceEntity fromActivityInstance,
            Activity currentActivity,
            AIServiceDetail service,
            ActivityInstanceEntity currentActivityInstance,
            ActivityForwardContext activityForwardContext,
            IDelegateService delegateService)
        {
            try
            {
                var session = delegateService.GetSession();
                var processInstanceId = delegateService.GetProcessInstanceId();

                //获取输入参数
                //Getting input variable
                var pvm = new ProcessVariableManager();
                List<MultiMediaFile> mediaFileList = null;

                //上一步的输出变量，就是当前节点的输入变量
                //The output variable of the previous step is the input variable of the current node
                var outputVarialbeNameList = activityForwardContext.ProcessModel.GetActivityOutputVarialbeNameList(fromActivity);
                if (outputVarialbeNameList != null)
                {
                    //执行大模型运算时候，需要用到输入变量，作为大模型的输入判断条件
                    //When performing large model operations, input variables are needed as input judgment conditions for the large model
                    var inputVarialbeValueList = pvm.GetVariableListByActivity(session.Connection, processInstanceId, fromActivityInstance.Id, outputVarialbeNameList, session.Transaction);

                    //多模态文件同样需要加入大模型的输入参数里面去
                    //Multimodal files also need to be added to the input parameters of the large model
                    mediaFileList = inputVarialbeValueList.Select<ProcessVariableEntity, MultiMediaFile>(a => new MultiMediaFile
                    {
                        Name = a.Name,
                        MeidaType = EnumHelper.TryParseEnum<MultiMediaTypeEnum>(a.MediaType),
                        base64Content = a.Value
                    }).ToList();
                }

                //读取大模型配置信息
                //Reading AxConfig Setting info
                var aiModelDataService = new AiModelDataService();
                var axConfigEntity = aiModelDataService.GetAiActivityConfigByUUID(service.ConfigUUID);

                //调用大模型服务
                //Calling llm executing service
                var aiFastCallingService = new AiFastCallingService();
                var aiResponseMessage = await aiFastCallingService.InvokeAIServiceAsync(axConfigEntity, mediaFileList);

                //保存节点变量
                //Saving output variable
                var processInstance = (new ProcessInstanceManager()).GetById(session.Connection, processInstanceId, session.Transaction);

                var currentVariableList = currentActivity.VariableList;
                var currentOutputVariable = currentVariableList.FirstOrDefault(
                    a => a.DirectionType == VariableDirectionTypeEnum.Output);

                var processVariable = new ProcessVariableEntity();
                processVariable.ProcessInstanceId = processInstanceId;
                processVariable.ProcessId = processInstance.ProcessId;
                processVariable.AppInstanceId = processInstance.AppInstanceId;
                processVariable.ActivityInstanceId = currentActivityInstance.Id;
                processVariable.ActivityId = currentActivityInstance.ActivityId;
                processVariable.ActivityName = currentActivityInstance.ActivityName;
                processVariable.Name = currentOutputVariable.Name;
                processVariable.VariableScope = ProcessVariableScopeEnum.Activity.ToString();
                processVariable.Value = aiResponseMessage;
                processVariable.MediaType = MultiMediaTypeEnum.Text.ToString();

                pvm.SaveVariable(session.Connection, processVariable, session.Transaction);
            }
            catch (Exception ex)
            {
                throw new WorkflowException(LocalizeHelper.GetEngineMessage("actionexecutor.ExecuteLocalService.exception", ex.Message));
            }
        }

        /// <summary>
        /// Execute Local Service
        /// </summary>
        private static async Task ExecutePlugInService(Activity fromActivity,
            ActivityInstanceEntity fromActivityInstance,
            Activity currentActivity,
            AIServiceDetail service,
            ActivityInstanceEntity activityInstance,
            ActivityForwardContext activityForwardContext,
            IDelegateService delegateService)
        {
            try
            {
                ;
            }
            catch (Exception ex)
            {
                throw new WorkflowException(LocalizeHelper.GetEngineMessage("actionexecutor.ExecuteLocalService.exception", ex.Message));
            }
        }
    }
}
