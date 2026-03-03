using Slickflow.AI.Entity;
using Slickflow.AI.Service;
using Slickflow.Data;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Common;
using Slickflow.Engine.Core;
using Slickflow.Engine.Core.Result;
using Slickflow.Engine.Event;
using Slickflow.Engine.External;
using Slickflow.Engine.Utility;
using Slickflow.Engine.Xpdl.Common;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Module.Localize;
using Slickflow.WebUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using Dapper;

namespace Slickflow.Engine.Executor
{
    /// <summary>
    /// Executes a single workflow activity: service, AI service, script, end.
    /// </summary>
    public class WorkflowActivityExecutor : IWorkflowActivityExecutor
    {
        private readonly string _notifyClientSessionId;
        private readonly Action<string, object> _notifyClientCallback;

        public WorkflowActivityExecutor(string notifyClientSessionId = null, Action<string, object> notifyClientCallback = null)
        {
            _notifyClientSessionId = notifyClientSessionId;
            _notifyClientCallback = notifyClientCallback;
        }

        /// <summary>
        /// Execute a single activity (service, AI, script, or end node).
        /// </summary>
        public void ExecuteActivity(Activity currentActivity, AutoExecutionContext context, WfExecutedResult result)
        {
            try
            {
                if (currentActivity.ActivityType == ActivityTypeEnum.ServiceNode)
                {
                    var serviceList = currentActivity.ServiceList;
                    if (serviceList != null && serviceList.Count > 0)
                        ExecuteServiceNode(currentActivity, serviceList, context);
                }

                if (currentActivity.ActivityType == ActivityTypeEnum.AIServiceNode)
                {
                    var aiServiceList = currentActivity.AIServiceList;
                    if (aiServiceList != null && aiServiceList.Count > 0)
                        ExecuteAIServiceNode(currentActivity, aiServiceList, context);
                }

                if (currentActivity.ScriptList != null && currentActivity.ScriptList.Count > 0)
                {
                    var scriptList = currentActivity.ScriptList;
                    if (scriptList != null && scriptList.Count > 0)
                        ExecuteScriptNode(currentActivity, scriptList, context);
                }
            }
            catch (Exception ex)
            {
                result.Status = WfExecutedStatus.Failed;
                result.Message = $"Failed to execute activity {currentActivity.ActivityId}: {ex.Message}";
            }

            Console.WriteLine($"The activity:{currentActivity.ActivityName} has been executed successed");
        }

        /// <summary>
        /// Execute service list (aligned with Engine Core.Pattern.Auto.ServiceExecutor.ExecuteServiceList).
        /// </summary>
        public void ExecuteServiceNode(Activity activity, List<ServiceDetail> serviceDetail, AutoExecutionContext context)
        {
            if (serviceDetail == null || serviceDetail.Count == 0)
                return;

            foreach (var service in serviceDetail)
            {
                if (service.Method == ServiceMethodEnum.None)
                    continue;
                ExecuteService(service, context);
            }
        }

        public void ExecuteService(ServiceDetail service, AutoExecutionContext context)
        {
            if (service.Method == ServiceMethodEnum.LocalService)
                ExecuteLocalServiceInExecutor(service, context);
            else if (service.Method == ServiceMethodEnum.CSharpLibrary)
                ExecuteCSharpLibraryInExecutor(service, context);
            else if (service.Method == ServiceMethodEnum.WebApi)
                ExecuteWebApiInExecutor(service, context);
            else if (service.Method == ServiceMethodEnum.StoreProcedure)
                ExecuteStoreProcedureInExecutor(service, context);
            else if (service.Method == ServiceMethodEnum.LocalMethod)
                ExecuteLocalMethodInExecutor(service, context);
            else
                throw new WorkflowException(LocalizeHelper.GetEngineMessage("actionexecutor.Execute.exception", service.Method.ToString()));
        }

        public string GetVariableFromContext(AutoExecutionContext context, string name)
        {
            if (context?.Variables == null || string.IsNullOrWhiteSpace(name))
                return string.Empty;
            return context.Variables.TryGetValue(name.Trim(), out var v) ? (v ?? string.Empty) : string.Empty;
        }

        public string CompositeJsonValueFromContext(string arguments, AutoExecutionContext context)
        {
            if (string.IsNullOrWhiteSpace(arguments))
                return "{}";
            var sb = new StringBuilder(256);
            var arguList = arguments.Split(',');
            foreach (var name in arguList)
            {
                var n = name?.Trim();
                if (string.IsNullOrEmpty(n)) continue;
                if (sb.Length > 0) sb.Append(",");
                var arguValue = GetVariableFromContext(context, n);
                arguValue = FormatJsonStringIfSimple(arguValue);
                sb.AppendFormat("{0}:{1}", n, arguValue);
            }
            return sb.Length > 0 ? "{" + sb + "}" : "{}";
        }

        public string FormatJsonStringIfSimple(string jsonValue)
        {
            if (string.IsNullOrEmpty(jsonValue)) return "\"\"";
            jsonValue = jsonValue.Trim();
            if ((jsonValue.StartsWith("{") && jsonValue.EndsWith("}")) || (jsonValue.StartsWith("\"") && jsonValue.EndsWith("\"")))
                return jsonValue;
            return "\"" + jsonValue + "\"";
        }

        public object[] CompositeParameterValuesFromContext(string arguments, AutoExecutionContext context)
        {
            if (string.IsNullOrWhiteSpace(arguments))
                return null;
            var arguList = arguments.Split(',');
            var valueArray = new object[arguList.Length];
            for (var i = 0; i < arguList.Length; i++)
                valueArray[i] = GetVariableFromContext(context, arguList[i]);
            return valueArray;
        }

        public DynamicParameters CompositeSqlParametersValueFromContext(string arguments, AutoExecutionContext context)
        {
            var parameters = new DynamicParameters();
            if (string.IsNullOrWhiteSpace(arguments))
                return parameters;
            var arguList = arguments.Split(',');
            foreach (var name in arguList)
            {
                var n = name?.Trim();
                if (string.IsNullOrEmpty(n)) continue;
                var arguValue = GetVariableFromContext(context, n);
                parameters.Add("@" + n, arguValue);
            }
            return parameters;
        }

        public void ExecuteLocalServiceInExecutor(ServiceDetail service, AutoExecutionContext context)
        {
            try
            {
                var adapter = new AutoRunEventServiceAdapter(context);
                var instance = ReflectionHelper.GetSpecialInstance<IExternalService>(service.Expression);
                if (instance is IExecutable executableInstance)
                    executableInstance.Executable(adapter);
            }
            catch (Exception ex)
            {
                throw new WorkflowException(LocalizeHelper.GetEngineMessage("actionexecutor.ExecuteLocalService.exception", ex.Message));
            }
        }

        /// <summary>
        /// Execute LocalMethod ServiceTask: resolve delegate from registry by key and invoke.
        /// </summary>
        public void ExecuteLocalMethodInExecutor(ServiceDetail service, AutoExecutionContext context)
        {
            var key = service.Expression;
            if (string.IsNullOrWhiteSpace(key))
                throw new WorkflowException("LocalMethod requires Expression (registry key).");

            var registry = context.DelegateRegistry ?? ServiceTaskDelegateRegistry.Global;
            if (!registry.TryResolve(key, out var d))
                throw new WorkflowException($"Delegate not found for key: {key}. Register before Run with ServiceTaskDelegateRegistry.Global.Register(key, delegate).");

            var adapter = new AutoRunEventServiceAdapter(context);
            if (d is Action<IEventService> action)
            {
                action(adapter);
            }
            else if (d is Func<IEventService, object> func)
            {
                var result = func(adapter);
                if (result == null || context.Variables == null) return;

                string varName;
                string varValue;
                if (result is ServiceTaskResult sr)
                {
                    varName = sr.VariableName;
                    varValue = sr.Value?.ToString() ?? "";
                }
                else
                {
                    varName = ServiceTaskDelegateRegistry.DefaultResultVariableName;
                    varValue = result.ToString();
                }
                if (!string.IsNullOrEmpty(varName))
                    context.Variables[varName] = varValue;
            }
            else
            {
                throw new WorkflowException($"Delegate for key '{key}' must be Action<IEventService> or Func<IEventService, object>.");
            }
        }

        public void ExecuteWebApiInExecutor(ServiceDetail service, AutoExecutionContext context)
        {
            try
            {
                if (service.SubMethod == SubMethodEnum.HttpGet)
                {
                    var jsonGetValue = GetVariableFromContext(context, service.Arguments ?? "");
                    var url = string.Format("{0}/{1}", service.Expression, jsonGetValue);
                    var httpGetClient = HttpClientHelper.CreateHelper(url);
                    httpGetClient.Get();
                }
                else if (service.SubMethod == SubMethodEnum.HttpPost)
                {
                    var url = service.Expression;
                    var httpClientHelper = HttpClientHelper.CreateHelper(url);
                    var jsonValue = CompositeJsonValueFromContext(service.Arguments ?? "", context);
                    httpClientHelper.Post(jsonValue);
                }
                else if (service.SubMethod == SubMethodEnum.HttpPut)
                {
                    var url = service.Expression;
                    var httpClientHelper = HttpClientHelper.CreateHelper(url);
                    var jsonValue = CompositeJsonValueFromContext(service.Arguments ?? "", context);
                    httpClientHelper.Put(jsonValue);
                }
                else if (service.SubMethod == SubMethodEnum.HttpDelete)
                {
                    var jsonGetValue = GetVariableFromContext(context, service.Arguments ?? "");
                    var url = string.Format("{0}/{1}", service.Expression, jsonGetValue);
                    var httpGetClient = HttpClientHelper.CreateHelper(url);
                    httpGetClient.Delete();
                }
            }
            catch (Exception ex)
            {
                throw new WorkflowException(LocalizeHelper.GetEngineMessage("actionexecutor.ExecuteWebApi.exception", ex.Message));
            }
        }

        public void ExecuteCSharpLibraryInExecutor(ServiceDetail service, AutoExecutionContext context)
        {
            try
            {
                var methodInfo = service.MethodInfo;
                if (methodInfo == null)
                    throw new WorkflowException("Service MethodInfo is not configured.");
                var assemblyFullName = methodInfo.AssemblyFullName;
                var methodName = methodInfo.MethodName;
                var executingPath = ConfigHelper.GetExecutingDirectory();
                var pluginAssemblyName = string.Format("{0}\\{1}\\{2}.dll", executingPath, "plugin", assemblyFullName);
                var pluginAssemblyTypes = Assembly.LoadFile(pluginAssemblyName).GetTypes();
                var outerClass = pluginAssemblyTypes.Single(t => !t.IsInterface && t.FullName == methodInfo.TypeFullName);
                var instance = Activator.CreateInstance(outerClass);
                var mi = outerClass.GetMethod(methodName);
                if (mi == null)
                    throw new WorkflowException($"Method {methodName} not found on type {methodInfo.TypeFullName}.");
                object[] methodParams = null;
                if (!string.IsNullOrWhiteSpace(service.Arguments))
                    methodParams = CompositeParameterValuesFromContext(service.Arguments, context);
                mi.Invoke(instance, methodParams);
            }
            catch (Exception ex)
            {
                throw new WorkflowException(LocalizeHelper.GetEngineMessage("actionexecutor.ExecuteCSharpLibraryMethod.exception", ex.Message));
            }
        }

        public void ExecuteStoreProcedureInExecutor(ServiceDetail service, AutoExecutionContext context)
        {
            try
            {
                var parameters = CompositeSqlParametersValueFromContext(service.Arguments ?? "", context);
                var procedureName = service.Expression;
                using var session = SessionFactory.CreateSession();
                var repository = new Repository();
                repository.ExecuteProc(session.Connection, procedureName, parameters, session.Transaction);
            }
            catch (Exception ex)
            {
                throw new WorkflowException(LocalizeHelper.GetEngineMessage("actionexecutor.ExecuteStoreProcedureMethod.exception", ex.Message));
            }
        }

        public void ExecuteAIServiceNode(Activity currentActivity, List<AiServiceDetail> aiServiceList, AutoExecutionContext context)
        {
            if (aiServiceList == null || aiServiceList.Count == 0)
                return;

            foreach (var service in aiServiceList)
            {
                if (service.AIServiceType == AiServiceTypeEnum.None)
                    continue;

                if (service.AIServiceType == AiServiceTypeEnum.LLM)
                    ExecuteLlmService(currentActivity, service, context);
                else if (service.AIServiceType == AiServiceTypeEnum.RAG)
                    ExecuteLlmService(currentActivity, service, context);
                else
                    throw new WorkflowException($"Unsupported AI service type: {service.AIServiceType}");
            }
        }

        public void ExecuteLlmService(Activity currentActivity, AiServiceDetail service, AutoExecutionContext context)
        {
            string processId = context.Runner?.ProcessId;
            string version = context.Runner?.Version;
            if (string.IsNullOrEmpty(processId) || string.IsNullOrEmpty(version))
                throw new WorkflowException("ProcessId and Version are required for AI service execution.");

            List<MultiMediaFile> mediaFileList = context.Variables
                .Select(kv => new MultiMediaFile
                {
                    Name = kv.Key,
                    Content = kv.Value ?? string.Empty,
                    MeidaType = MultiMediaTypeEnum.Text
                })
                .ToList();

            var aiModelDataService = new AiModelDataService();
            var axConfigEntity = aiModelDataService.GetAiActivityConfigByProcessVersionActivity(processId, version, currentActivity.ActivityId);
            if (axConfigEntity == null)
                throw new WorkflowException($"AI activity config not found for ProcessId={processId}, Version={version}, ActivityId={currentActivity.ActivityId}.");

            var historyChatMessageList = GetChatHistoryFromContext(context);

            string aiResponseMessage;
            if (service.AIServiceType == AiServiceTypeEnum.RAG)
            {
                var ragMultiTurnService = new RagMultiTurnService();
                aiResponseMessage = ragMultiTurnService
                    .InvokeWithHistoryAsync(axConfigEntity, mediaFileList, historyChatMessageList)
                    .GetAwaiter()
                    .GetResult();
            }
            else if (service.AIServiceType == AiServiceTypeEnum.LLM)
            {
                var llmMultiTurnService = new LlmMultiTurnService();
                aiResponseMessage = llmMultiTurnService
                    .InvokeWithHistoryAsync(axConfigEntity, mediaFileList, historyChatMessageList)
                    .GetAwaiter()
                    .GetResult();
            }
            else
            {
                var aiFastCallingService = new AiFastCallingService();
                aiResponseMessage = aiFastCallingService.InvokeAIServiceAsync(axConfigEntity, mediaFileList).GetAwaiter().GetResult();
            }

            // After obtaining the AI reply, append current turn (user + assistant) into chat_history
            // so that downstream AI nodes (LLM or RAG) see the latest multi-turn context.
            // For non user-facing AI nodes (such as JSON contact extractors), we only record the user message
            // and avoid injecting raw JSON into chat_history.
            try
            {
                var userMessage = GetVariableFromContext(context, "user_message");
                var responseForHistory = service.AIServiceType == AiServiceTypeEnum.RAG
                    ? aiResponseMessage
                    : null;
                UpdateChatHistoryInContext(context, userMessage, responseForHistory);
            }
            catch
            {
                // Do not break workflow if history serialization fails
            }

            var currentVariableList = currentActivity.VariableList;
            var currentOutputVariable = currentVariableList?.FirstOrDefault(a => a.DirectionType == VariableDirectionTypeEnum.Output);
            string outputVarName = currentOutputVariable?.Name;
            if (!string.IsNullOrEmpty(outputVarName))
                context.Variables[outputVarName] = aiResponseMessage;
            else
                context.Variables["AIServiceResult"] = aiResponseMessage;

            // Sync to ai_response for downstream plugins (MessageService, CustomerSaveService) to persist the conversation.
            // Only user-facing RAG nodes should update ai_response; internal LLM nodes that output JSON (e.g. contact_json)
            // must NOT overwrite the last natural-language reply shown to the end user.
            if (service.AIServiceType == AiServiceTypeEnum.RAG)
            {
                context.Variables["ai_response"] = aiResponseMessage ?? string.Empty;
            }

            if (service.AIServiceType == AiServiceTypeEnum.RAG && IsNotifyClientEnabled(currentActivity) && _notifyClientCallback != null && !string.IsNullOrEmpty(_notifyClientSessionId))
            {
                _notifyClientCallback(_notifyClientSessionId, new
                {
                    type = "node_response",
                    data = new
                    {
                        NodeId = currentActivity.ActivityId,
                        NodeName = currentActivity.ActivityName,
                        NodeType = "RAG",
                        ResponseData = new { reply = aiResponseMessage },
                        Timestamp = DateTime.UtcNow,
                        Message = "RAG node execution completed"
                    }
                });
            }
        }

        /// <summary>
        /// Reads chat_history (JSON array) from process variables and deserializes to history message list for multi-turn RAG.
        /// Example format: [{"role":"user","content":"..."},{"role":"assistant","content":"..."}]
        /// </summary>
        private static IList<ChatHistoryMessage> GetChatHistoryFromContext(AutoExecutionContext context)
        {
            if (context?.Variables == null) return null;
            if (!context.Variables.TryGetValue("chat_history", out var raw) || raw == null)
                return null;
            var json = raw.ToString();
            if (string.IsNullOrWhiteSpace(json)) return null;
            try
            {
                var list = JsonSerializer.Deserialize<List<ChatHistoryMessage>>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return list?.Count > 0 ? list : null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Appends the current turn (user message + AI response) to chat_history
        /// and writes it back to process variables as JSON.
        /// </summary>
        private static void UpdateChatHistoryInContext(AutoExecutionContext context, string userMessage, string aiResponse)
        {
            if (context?.Variables == null)
                return;

            var history = GetChatHistoryFromContext(context)?.ToList() ?? new List<ChatHistoryMessage>();

            if (!string.IsNullOrWhiteSpace(userMessage))
            {
                history.Add(new ChatHistoryMessage
                {
                    Role = "user",
                    Content = userMessage
                });
            }

            if (!string.IsNullOrWhiteSpace(aiResponse))
            {
                history.Add(new ChatHistoryMessage
                {
                    Role = "assistant",
                    Content = aiResponse
                });
            }

            if (history.Count == 0)
                return;

            var json = JsonSerializer.Serialize(history, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            context.Variables["chat_history"] = json;
        }

        public bool IsNotifyClientEnabled(Activity activity)
        {
            try
            {
                var section = activity?.SectionList?.FirstOrDefault(s => string.Equals(s?.Name, "myProperties", StringComparison.Ordinal));
                if (string.IsNullOrWhiteSpace(section?.Value)) return false;
                using var doc = JsonDocument.Parse(section.Value);
                if (doc.RootElement.TryGetProperty("isNotifyClient", out var prop))
                    return prop.ValueKind == JsonValueKind.True;
                return false;
            }
            catch
            {
                return false;
            }
        }

        public void ExecuteScriptNode(Activity activity, List<ScriptDetail> scriptDetail, AutoExecutionContext context)
        {
            // Execute script logic here
        }

        /// <summary>
        /// Execute end activity (end node).
        /// </summary>
        public void ExecuteEndActivity(Activity endActivity, AutoExecutionContext context)
        {
            Console.WriteLine($"The end activity has been executed successed!");
        }
    }
}
