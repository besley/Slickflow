using Slickflow.Data;
using Slickflow.AI.Configuration;
using Slickflow.AI.Entity;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Common;
using Slickflow.Engine.Service;
using Slickflow.Engine.Utility;
using Slickflow.Engine.Xpdl.Common;
using Slickflow.Engine.Xpdl.Template;
using System.Text;
using System.Text.Json;
using Slickflow.WebUtility;
using sfdapi.Models;

namespace sfdapi.Services
{
    public class SfBpmnServiceBuilder
    {
        #region Property and Constructor
        private AiAppConfigProviderOptions _aiOpitons;
        private readonly HttpClient _httpClient;
        
        public SfBpmnServiceBuilder(AiAppConfigProviderOptions aiOptions) 
        {
            _aiOpitons = aiOptions;
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(120); 
        }
        #endregion

        public async Task<ProcessFileEntity>CreateBpmnProcessByTemplate(string templateName)
        {
            try
            {
                //create bpmn process
                var xmlContent = BpmnFileSampleDefine.RebuildBpmnPorcessXmlContentByTemplate(templateName);
                var workflowService = new WorkflowService();
                var processFileEntity = workflowService.CreateProcessByXML(xmlContent);

                return processFileEntity;
            }
            catch (Exception ex)
            {
                LogManager.RecordLog("An error occurred when generating process by AI",
                LogEventType.Exception,
                LogPriority.Normal,
                templateName,
                ex);
                throw new InvalidOperationException($"An error occurred when creating the process by template, detail:{ex.Message}");
            }
        }

        #region Load Process Template Content
        /// <summary>
        /// Load template content
        /// 加载流程模板
        /// </summary>
        public ProcessTemplate LoadTemplateContent(ProcessTemplateType templateType)
        {
            var content = string.Empty;
            if (templateType == ProcessTemplateType.Default)
            {
                content = string.Format(@"using Slickflow.Graph;
using Slickflow.Engine.Common;

//firstly, create a process model builder
var pmb = ProcessModelBuilder.CreateProcess(""HelloWorldProcess_{0}"", ""HelloWorldProcess_Code_{0}""); 
var process = pmb.Start(""Start"")
    .Task(""Hello"", ""003"")       //task name, code
    .Task(""World"", ""005"")       //task name, code
    .End(""End"")
    .Store();",
RandomSequenceGenerator.GetRandomInt4());
            }
            else if (templateType == ProcessTemplateType.Sequence)
            {
                content = string.Format(@"using Slickflow.Graph;
using Slickflow.Engine.Common;

//firstly, create a process model builder
var pmb = ProcessModelBuilder.CreateProcess(""BookSellerProcess_{0}"", ""BookSellerProcess_Code_{0}""); 
var process = pmb.Start(""Start"")
    .Task(""Package Books"", ""003"")       //task name, code
    .Task(""Weight Books"", ""005"")       //task name, code
    .Task(""Deliver Books"", ""007"")       //task name, code
    .Task(""Print Notes"", ""009"")       //task name, code
    .End(""End"")
    .Store();",
    RandomSequenceGenerator.GetRandomInt4());
            }
            else if (templateType == ProcessTemplateType.Parallel)
            {
                content = string.Format(@"using Slickflow.Graph;
using Slickflow.Engine.Common;

//firstly, create a process model builder
var pmb = ProcessModelBuilder.CreateProcess(""LargeOrderProcess_{0}"", ""LargeOrderProcess_Code_{0}"");
var process = pmb.Start(""Start"")
        .Task(""Large Order Received"", ""001"")
        .AndSplit(""AndSplit"", ""AS002"")
        .Parallels(
            () => pmb.Branch(
                () => pmb.Task(""Engineering Review"", ""0011"")
            )
            , () => pmb.Branch(
                () => pmb.Task(""Design Review"", ""0012"")
            )
            , () => pmb.Branch(
                () => pmb.Task(""QA Review"", ""0013"")
            )
        )
        .AndJoin(""AndJoin"", ""AJ002"")
        .Task(""Management Approve"", ""007"")
        .End(""End"")
        .Store();",
        RandomSequenceGenerator.GetRandomInt4());
            }
            else if (templateType == ProcessTemplateType.Conditional)
            {
                content = string.Format(@"using Slickflow.Graph;
using Slickflow.Engine.Common;

//firstly, create a process model builder
var pmb = ProcessModelBuilder.CreateProcess(""ConditionalProcess_{0}"", ""ConditionalProcess_Code_{0}"");
var process = pmb.Start(""Start"")
		.Task(""Reimbursement Submit"", ""task001"")
		.Task(""Finalcial Approval"", ""task002"")
		.XOrSplit(""XOr-Split"", ""orsplit001"")
		.Parallels(
			() => pmb.Branch(
				() => pmb.Task(
					 VertexBuilder.CreateTask(""Approved by the Director in Charge"", ""task010""),
					 LinkBuilder.CreateTransition(""money<10000"")
								.AddCondition(ConditionTypeEnum.Expression, ""money<10000"")
					)
			)
			, () => pmb.Branch(
					() => pmb.Task(
						VertexBuilder.CreateTask(""CEO Approval"", ""task020""),
						LinkBuilder.CreateTransition(""money>=10000"")
							   .AddCondition(ConditionTypeEnum.Expression, ""money>=10000"")
				)
			)
		)
		.XOrJoin(""XOr-Join"", ""orjoin001"")
		.End(""end"")
        .Store();",
         RandomSequenceGenerator.GetRandomInt4());
            }
            else if (templateType == ProcessTemplateType.ProcessModify)
            {
                content = string.Format(@"using Slickflow.Graph;
using Slickflow.Engine.Common;

//firstly, create a process model builder
var pmb = ProcessModelBuilder.CreateProcess(""BookSellerProcess_{0}"", ""BookSellerProcess_Code_{0}""); 
var process = pmb.Start(""Start"")
    .Task(""Package Books"", ""003"")       //task name, code
    .Task(""Deliver Books"", ""005"")       //task name, code
    .End(""End"")
    .Store();

//secondly load a process model builder
var pmb2 = ProcessModelBuilder.LoadProcess(""BookSellerProcess_Code_{0}"", ""1"");
//execute deffrient task operation once together
pmb2.Add(""003"", ActivityTypeEnum.TaskNode, ""zzz"", ""zzz-code"")
   .Insert(""003"", ActivityTypeEnum.TaskNode, ""task004"", ""004"")
   .Set(""003"", (a) => pmb2.GetBuilder(a).SetUrl(""slickflow.com"").SetName(""mer-sss-ryxmas""))
   .Replace(""004"", ActivityTypeEnum.TaskNode, ""task222"", ""222"")
   .Exchange(""222"", ""zzz-code"")
   .Fork(""zzz-code"", ActivityTypeEnum.TaskNode, ""yyy"", ""555"")
   .Remove(""222"", true)
   .Update();",
   RandomSequenceGenerator.GetRandomInt4());
            }
            else if (templateType == ProcessTemplateType.RunProcess)
            {
                content = string.Format(@"using Slickflow.Graph;
using Slickflow.Engine.Common;
using Slickflow.Engine.Service;

//firstly, create a process model builder
var pmb = ProcessModelBuilder.CreateProcess(""BookSellerProcess_{0}"", ""BookSellerProcess_Code_{0}""); 
var process = pmb.Start(""Start"")
    .Task(""Package Books"", ""003"")       //task name, code
    .Task(""Deliver Books"", ""005"")       //task name, code
    .End(""End"")
    .Store();

//initialize a runner object
WfAppRunner runner = new WfAppRunner();
runner.ProcessCode = ""BookSellerProcess_Code_{0}"";
runner.Version = ""1"";
runner.UserId = ""10"";
runner.UserName = ""Jack"";
runner.AppName = ""Test APP"";
runner.AppInstanceId = ""tx001"";
runner.AppInstanceCode = ""TX002"";

//startup a new process instance
//the first task ""Package Books"" will be in ready status
IWorkflowService wfService = new WorkflowService();
var wfResultStart = wfService.CreateRunner(runner.UserId, runner.UserName)
    .UseApp(runner.AppInstanceId, runner.AppName, runner.AppInstanceCode)
    .UseProcess(runner.ProcessCode, runner.Version)
    .Start();

//continue to run the process instance
//finished the first task ""Package Books""
//the second task ""Deliver Books"" will be in ready status
var wfResultRun = wfService.CreateRunner(runner.UserId, runner.UserName)
    .UseApp(runner.AppInstanceId, runner.AppName, runner.AppInstanceCode)
    .UseProcess(runner.ProcessCode, runner.Version)
    .NextStepInt(""10"", ""Jack"")      //this method is only for tutorial, OnTask() should be called normally before using it.
    .Run();
",
    RandomSequenceGenerator.GetRandomInt4());
            }
            var template = new ProcessTemplate { TemplateType = templateType, Content = content };
            return template;
        }
        #endregion
    }
}
