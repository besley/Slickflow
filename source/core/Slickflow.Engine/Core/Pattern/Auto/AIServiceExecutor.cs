using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Slickflow.Engine.Common;
using Slickflow.Engine.Event;
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
    internal class AiServiceExecutor
    {
        /// <summary>
        /// Execute service list
        /// </summary>
        internal static void ExecuteAIServiceList(Activity fromActivity,
            ActivityInstanceEntity fromActivityInstance,
            Activity currentActivity,
            ActivityInstanceEntity currentActivityInstance,
            ActivityForwardContext activityForwardContext,
            IEventService eventService)
        {
            IList<AiServiceDetail> aiServiceList = currentActivity.AIServiceList;
            if (aiServiceList != null && aiServiceList.Count > 0)
            {
                foreach (var service in aiServiceList)
                {
                    if (service.AIServiceType != AiServiceTypeEnum.None)
                    {
                        Execute(fromActivity, fromActivityInstance, currentActivity, service, currentActivityInstance, activityForwardContext, eventService).GetAwaiter().GetResult();
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
            AiServiceDetail service,
            ActivityInstanceEntity currentActivityInstance,
            ActivityForwardContext activityForwardContext,
            IEventService eventService)
        {
            if (service.AIServiceType == AiServiceTypeEnum.LLM)
            {
                await ExecuteLlmService(fromActivity, fromActivityInstance, currentActivity, service, currentActivityInstance, activityForwardContext, eventService);
            }
            else if (service.AIServiceType == AiServiceTypeEnum.RAG)
            {
                await ExecuteLlmService(fromActivity, fromActivityInstance, currentActivity, service, currentActivityInstance, activityForwardContext, eventService);
            }
            else if (service.AIServiceType == AiServiceTypeEnum.Agent)
            {
                await ExecuteAgentService(fromActivity, fromActivityInstance, currentActivity, service, currentActivityInstance, activityForwardContext, eventService);
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
            AiServiceDetail service,
            ActivityInstanceEntity currentActivityInstance,
            ActivityForwardContext activityForwardContext,
            IEventService eventService)
        {
            try
            {
                var session = eventService.GetSession();
                var processInstanceId = eventService.GetProcessInstanceId();

                //��ȡ�������
                //Getting input variable
                var pvm = new ProcessVariableManager();
                List<MultiMediaFile> mediaFileList = null;

                //��һ����������������ǵ�ǰ�ڵ���������
                //The output variable of the previous step is the input variable of the current node
                var outputVarialbeNameList = activityForwardContext.ProcessModel.GetActivityOutputVarialbeNameList(fromActivity);
                if (outputVarialbeNameList != null)
                {
                    //ִ�д�ģ������ʱ����Ҫ�õ������������Ϊ��ģ�͵������ж�����
                    //When performing large model operations, input variables are needed as input judgment conditions for the large model
                    var inputVarialbeValueList = pvm.GetVariableListByActivity(session.Connection, processInstanceId, fromActivityInstance.Id, outputVarialbeNameList, session.Transaction);

                    //��ģ̬�ļ�ͬ����Ҫ�����ģ�͵������������ȥ
                    //Multimodal files also need to be added to the input parameters of the large model
                    mediaFileList = inputVarialbeValueList.Select<ProcessVariableEntity, MultiMediaFile>(a => new MultiMediaFile
                    {
                        Name = a.Name,
                        MeidaType = EnumHelper.TryParseEnum<MultiMediaTypeEnum>(a.MediaType),
                        Content = a.Value
                    }).ToList();
                }

                //��ȡ����ʵ����Ϣ
                //Getting process instance info
                var processInstance = (new ProcessInstanceManager()).GetById(session.Connection, processInstanceId, session.Transaction);

                //��ȡ��ģ��������Ϣ
                //Reading AxConfig Setting info
                var aiModelDataService = new AiModelDataService();
                var axConfigEntity = aiModelDataService.GetAiActivityConfigByProcessVersionActivity(processInstance.ProcessId, processInstance.Version, currentActivity.ActivityId);

                //���ô�ģ�ͷ���
                //Calling llm executing service
                var aiFastCallingService = new AiFastCallingService();
                var aiResponseMessage = await aiFastCallingService.InvokeAIServiceAsync(axConfigEntity, mediaFileList);

                //����ڵ����
                //Saving output variable

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
        private static async Task ExecuteRagService(Activity fromActivity,
            ActivityInstanceEntity fromActivityInstance,
            Activity currentActivity,
            AiServiceDetail service,
            ActivityInstanceEntity activityInstance,
            ActivityForwardContext activityForwardContext,
            IEventService eventService)
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

        /// <summary>
        /// Execute Agent Service: runs a full ReAct loop via AgentMultiTurnService.
        /// Resolves tools from AgentToolRegistry by ActivityId, builds context from
        /// workflow variables, and writes the final answer back as an output variable.
        /// </summary>
        private static async Task ExecuteAgentService(Activity fromActivity,
            ActivityInstanceEntity fromActivityInstance,
            Activity currentActivity,
            AiServiceDetail service,
            ActivityInstanceEntity currentActivityInstance,
            ActivityForwardContext activityForwardContext,
            IEventService eventService)
        {
            try
            {
                var session           = eventService.GetSession();
                var processInstanceId = eventService.GetProcessInstanceId();

                // Collect input variables from the previous activity's output variables
                var pvm           = new ProcessVariableManager();
                List<MultiMediaFile> mediaFileList = null;

                var outputVariableNameList = activityForwardContext.ProcessModel
                    .GetActivityOutputVarialbeNameList(fromActivity);
                if (outputVariableNameList != null && fromActivityInstance != null)
                {
                    var inputVariableValueList = pvm.GetVariableListByActivity(
                        session.Connection,
                        processInstanceId,
                        fromActivityInstance.Id,
                        outputVariableNameList,
                        session.Transaction);

                    mediaFileList = inputVariableValueList
                        .Select<ProcessVariableEntity, MultiMediaFile>(a => new MultiMediaFile
                        {
                            Name      = a.Name,
                            MeidaType = EnumHelper.TryParseEnum<MultiMediaTypeEnum>(a.MediaType),
                            Content   = a.Value
                        })
                        .ToList();
                }

                // Resolve ProcessId/Version from context (avoids DB round-trip and null-ref when
                // the process instance is not yet committed within the same transaction)
                var processId = activityForwardContext.ProcessModel?.ProcessEntity?.ProcessId
                    ?? activityForwardContext.ProcessInstance?.ProcessId;
                var processVersion = activityForwardContext.ProcessModel?.ProcessEntity?.Version
                    ?? activityForwardContext.ProcessInstance?.Version
                    ?? "1";

                if (string.IsNullOrEmpty(processId))
                {
                    // Fallback: query DB only if context doesn't have the ID
                    var processInstance = (new ProcessInstanceManager())
                        .GetById(session.Connection, processInstanceId, session.Transaction);
                    if (processInstance == null)
                        throw new WorkflowException($"Process instance not found: id={processInstanceId}");
                    processId      = processInstance.ProcessId;
                    processVersion = processInstance.Version ?? "1";
                }

                // Load AI activity config
                var aiModelDataService = new AiModelDataService();
                var axConfigEntity     = aiModelDataService.GetAiActivityConfigByProcessVersionActivity(
                    processId,
                    processVersion,
                    currentActivity.ActivityId);

                if (axConfigEntity == null)
                    throw new WorkflowException(
                        $"AI activity config not found for ProcessId={processId}, Version={processVersion}, ActivityId={currentActivity.ActivityId}");

                // Run the ReAct agent loop
                var agentService    = new AgentMultiTurnService();
                var aiResponseMessage = await agentService.InvokeWithHistoryAsync(
                    axConfigEntity,
                    mediaFileList,
                    null);   // Agent manages its own history per-run

                // Persist result as output variable
                var currentVariableList = currentActivity.VariableList;
                var currentOutputVariable = currentVariableList?.FirstOrDefault(
                    a => a.DirectionType == VariableDirectionTypeEnum.Output);

                var appInstanceId = activityForwardContext.ProcessInstance?.AppInstanceId ?? string.Empty;
                var processVariable = new ProcessVariableEntity
                {
                    ProcessInstanceId  = processInstanceId,
                    ProcessId          = processId,
                    AppInstanceId      = appInstanceId,
                    ActivityInstanceId = currentActivityInstance.Id,
                    ActivityId         = currentActivityInstance.ActivityId,
                    ActivityName       = currentActivityInstance.ActivityName,
                    Name               = currentOutputVariable?.Name ?? "AgentResult",
                    VariableScope      = ProcessVariableScopeEnum.Activity.ToString(),
                    Value              = aiResponseMessage,
                    MediaType          = MultiMediaTypeEnum.Text.ToString()
                };

                pvm.SaveVariable(session.Connection, processVariable, session.Transaction);
            }
            catch (Exception ex)
            {
                throw new WorkflowException(LocalizeHelper.GetEngineMessage(
                    "actionexecutor.ExecuteLocalService.exception", ex.Message));
            }
        }
    }
}
