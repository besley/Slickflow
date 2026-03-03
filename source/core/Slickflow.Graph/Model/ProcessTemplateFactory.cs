using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Common;
using Slickflow.Engine.Xpdl.Common;
using Slickflow.Engine.Xpdl.Template;


namespace Slickflow.Graph.Model
{
    /// <summary>
    /// Process graphic code creation factory class
    /// 流程图形代码创建工厂类
    /// </summary>
    public class ProcessTemplateFactory
    {
        /// <summary>
        /// Load template content
        /// 加载流程模板
        /// </summary>
        public static ProcessTemplate LoadTemplateContent(ProcessTemplateType templateType)
        {
            var content = string.Empty;
            if (templateType == ProcessTemplateType.Default)
            {
                content = string.Format(@"using Slickflow.Graph.Model;
using Slickflow.Engine.Common;

//firstly, create a process model builder
var wf = new Workflow(""HelloWorldProcess_{0}"", ""HelloWorldProcess_Code_{0}""); 
var process = wf.Start(""Start"")
    .Task(""Hello"", ""003"")       //task name, code
    .Task(""World"", ""005"")       //task name, code
    .End(""End"")
    .Build();",
Utility.GetRandomInt());
            }
            else if (templateType == ProcessTemplateType.Sequence)
            {
                content = string.Format(@"using Slickflow.Graph.Model;
using Slickflow.Engine.Common;

//firstly, create a process model builder
var wf = new Workflow(""BookSellerProcess_{0}"", ""BookSellerProcess_Code_{0}""); 
var process = wf.Start(""Start"")
    .Task(""Package Books"", ""003"")       //task name, code
    .Task(""Weight Books"", ""005"")       //task name, code
    .Task(""Deliver Books"", ""007"")       //task name, code
    .Task(""Print Notes"", ""009"")       //task name, code
    .End(""End"")
    .Build();", 
    Utility.GetRandomInt());
            }
            else if (templateType == ProcessTemplateType.Parallel)
            {
                content = string.Format(@"using Slickflow.Graph.Model;
using Slickflow.Engine.Common;

//firstly, create a process model builder
var wf = new Workflow(""LargeOrderProcess_{0}"", ""LargeOrderProcess_Code_{0}"");
var process = wf.Start(""Start"")
        .Task(""Large Order Received"", ""001"")
        .AndSplit(""AndSplit"", ""AS002"")
        .Parallels(
            (""Engineering Review"", ""0011""),
            (""Design Review"", ""0012""),
            (""QA Review"", ""0013"")
        )
        .AndJoin(""AndJoin"", ""AJ002"")
        .Task(""Management Approve"", ""007"")
        .Connect(""007"", ""001"")
        .End(""End"")
        .Build();",
        Utility.GetRandomInt());
            }
            else if(templateType == ProcessTemplateType.Conditional)
            {
                content = string.Format(@"using Slickflow.Graph.Model;
using Slickflow.Engine.Common;

//firstly, create a process model builder
var wf = new Workflow(""ConditionalProcess_{0}"", ""ConditionalProcess_Code_{0}"");
var process = wf.Start(""Start"")
		.Task(""Reimbursement Submit"", ""task001"")
		.Task(""Finalcial Approval"", ""task002"")
		.XOrSplit(""XOr-Split"", ""orsplit001"")
		.Parallels(
			() => wf.Branch(() => wf.Task(
                    NodeBuilder.CreateTask(""Approved by the Director in Charge"", ""task010""),
					EdgeBuilder.CreateEdge(""money<10000"")
                               .AddCondition(ConditionTypeEnum.Expression, ""money<10000""))), 
            () => wf.Branch(() => wf.Task(
                    NodeBuilder.CreateTask(""CEO Approval"", ""task020""),
					EdgeBuilder.CreateEdge(""money>=10000"")
                               .AddCondition(ConditionTypeEnum.Expression, ""money>=10000"")))
		)
		.XOrJoin(""XOr-Join"", ""orjoin001"")
		.End(""end"")
        .Build();",
         Utility.GetRandomInt());
            }
            else if (templateType == ProcessTemplateType.ProcessModify)
            {
                content = string.Format(@"using Slickflow.Graph.Model;
using Slickflow.Engine.Common;

//firstly, create a process model builder
var wf = new Workflow(""BookSellerProcess_{0}"", ""BookSellerProcess_Code_{0}""); 
var process = wf.Start(""Start"")
    .Task(""Package Books"", ""003"")       //task name, code
    .Task(""Deliver Books"", ""005"")       //task name, code
    .End(""End"")
    .Build();

//secondly load a process model builder
var wf2 = Workflow.LoadProcess(""BookSellerProcess_Code_{0}"", ""1"");
//execute deffrient task operation once together
wf2.Add(""003"", ActivityTypeEnum.TaskNode, ""zzz"", ""zzz-code"")
   .Insert(""003"", ActivityTypeEnum.TaskNode, ""task004"", ""004"")
   .Set(""003"", (a) => wf2.GetBuilder(a).SetUrl(""slickflow.com"").SetName(""mer-sss-ryxmas""))
   .Replace(""004"", ActivityTypeEnum.TaskNode, ""task222"", ""222"")
   .Exchange(""222"", ""zzz-code"")
   .Fork(""zzz-code"", ActivityTypeEnum.TaskNode, ""yyy"", ""555"")
   .Remove(""222"", true)
   .Update();", 
   Utility.GetRandomInt());
            }
            else if (templateType == ProcessTemplateType.RunProcess)
            {
                content = string.Format(@"using Slickflow.Graph.Model;
using Slickflow.Engine.Common;
using Slickflow.Engine.Service;

//firstly, create a process model builder
var wf = new Workflow(""BookSellerProcess_{0}"", ""BookSellerProcess_Code_{0}""); 
var process = wf.Start(""Start"")
    .Task(""Package Books"", ""003"")       //task name, code
    .Task(""Deliver Books"", ""005"")       //task name, code
    .End(""End"")
    .Build();

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
    Utility.GetRandomInt());
            }
            var template = new ProcessTemplate { TemplateType = templateType, Content = content };
            return template;
        }

        /// <summary>
        /// Create process by template name
        /// 创建流程
        /// </summary>
        public static string CreateProcessByTemplate(string templateName, out ProcessFileEntity fileEntity)
        {
            var xmlContent = string.Empty;
            var strRandomInt01 = Utility.GetRandomInt().ToString();
            var strRandomInt02 = Utility.GetRandomInt().ToString();
            fileEntity = new ProcessFileEntity
            {
                ProcessId = $"Process_{strRandomInt01}_{strRandomInt02}",
                ProcessName = $"{templateName}_{strRandomInt01}_{strRandomInt02}",
                ProcessCode = $"{templateName}_Code_{strRandomInt01}_{strRandomInt02}",
                Version = "1",
                Status = 1
            };

            if (templateName == ProcessTemplateDefine.WF_TEMPLATE_STANDARD_BLANK)
            {
                xmlContent = CreateFlowBlank(fileEntity);
            }
            else if (templateName == ProcessTemplateDefine.WF_TEMPLATE_STANDARD_DEFAULT)
            {
                xmlContent = CreateFlowDefault(fileEntity);
            }
            else if (templateName == ProcessTemplateDefine.WF_TEMPLATE_STANDARD_SIMPLE)
            {
                xmlContent = CreateFlowSimple(fileEntity);
            }
            else if (templateName == ProcessTemplateDefine.WF_TEMPLATE_STANDARD_SEQUENCE)
            {
                xmlContent = CreateFlowSequence(fileEntity);
            }
            else if (templateName == ProcessTemplateDefine.WF_TEMPLATE_STANDARD_GATEWAY)
            {
                xmlContent = CreateFlowGateway(fileEntity);
            }
            else if (templateName == ProcessTemplateDefine.WF_TEMPLATE_STANDARD_PARALLEL)
            {
                xmlContent = CreateFlowGateway(fileEntity);
            }
            else if (templateName == ProcessTemplateDefine.WF_TEMPLATE_STANDARD_SUBPROCESS)
            {
                xmlContent = CreateFlowSubProcess(fileEntity);
            }
            else if (templateName == ProcessTemplateDefine.WF_TEMPLATE_STANDARD_MULTIPLEINSTANCE)
            {
                xmlContent = CreateFlowMI(fileEntity);
            }
            else if (templateName == ProcessTemplateDefine.WF_TEMPLATE_STANDARD_ANDSPLITMI)
            {
                xmlContent = CreateFlowAndSplitMI(fileEntity);
            }
            else if (templateName == ProcessTemplateDefine.WF_TEMPLATE_STANDARD_COMPLEX)
            {
                xmlContent = CreateFlowComplex(fileEntity);
            }
            else if (templateName == ProcessTemplateDefine.WF_TEMPLATE_STANDARD_CONDITIONAL)
            {
                xmlContent = CreateFlowConditional(fileEntity);
            }
            else if (templateName == ProcessTemplateDefine.WF_TEMPLATE_BUSINESS_ASKFORLEAVE)
            {
                xmlContent = CreateFlowAskforLeave(fileEntity);
            }
            else if (templateName == ProcessTemplateDefine.WF_TEMPLATE_BUSINESS_EORDER)
            {
                xmlContent = CreateFlowEOrder(fileEntity);
            }
            else if (templateName == ProcessTemplateDefine.WF_TEMPLATE_BUSINESS_REIMBURSEMENT)
            {
                xmlContent = CreateFlowReimbursement(fileEntity);
            }
            else if (templateName == ProcessTemplateDefine.WF_TEMPLATE_BUSINESS_WAREHOUSING)
            {
                xmlContent = CreateFlowWarehousing(fileEntity);
            }
            else if (templateName == ProcessTemplateDefine.WF_TEMPLATE_BUSINESS_OFFICEIN)
            {
                xmlContent = CreateFlowOfficeIn(fileEntity);
            }
            else if (templateName == ProcessTemplateDefine.WF_TEMPLATE_BUSINESS_CONTRACT)
            {
                xmlContent = CreateFlowContract(fileEntity);
            }
            fileEntity.XmlContent = xmlContent; 

            return xmlContent;
        }

        /// <summary>
        /// Create a blonk flow
        /// 创建空白流程
        /// </summary>
        private static string CreateFlowBlank(ProcessFileEntity fileEntity)
        {
            var wf = Workflow.Create(fileEntity.ProcessName, fileEntity.ProcessCode, fileEntity.ProcessId, fileEntity.Version);
            var xmlContent = wf.Start()
               .Serialize();

            return xmlContent;
        }

        /// <summary>
        /// Create a default simple flow
        /// 创建简单流程
        /// </summary>
        private static string CreateFlowDefault(ProcessFileEntity fileEntity)
        {
            var wf = Workflow.Create(fileEntity.ProcessName, fileEntity.ProcessCode, fileEntity.ProcessId, fileEntity.Version);
            var xmlContent = wf.Start()
               .Task("Task", "Hello")
               .Task("Task", "World")
               .End()
               .Serialize();

            return xmlContent;
        }

        /// <summary>
        /// Create a simple flow
        /// 创建简单流程
        /// </summary>
        private static string CreateFlowSimple(ProcessFileEntity fileEntity)
        {
            var wf = Workflow.Create(fileEntity.ProcessName, fileEntity.ProcessCode, fileEntity.ProcessId, fileEntity.Version);
            var xmlContent = wf.Start()
               .Task("Task", "task001")
               .End()
               .Serialize();

            return xmlContent;
        }

        /// <summary>
        /// Create flow sequence
        /// 创建活动和边界的流程
        /// </summary>
        private static string CreateFlowSequence(ProcessFileEntity fileEntity)
        {
            var wf = Workflow.Create(fileEntity.ProcessName, fileEntity.ProcessCode, fileEntity.ProcessId, fileEntity.Version);
            var xmlContent =
                wf.Start()
                   .Task(NodeBuilder.CreateTask("Task-001", "task001").SetUrl("http://www.slickflow.com"))
                   .Task(NodeBuilder.CreateTask("Task-002", "task002"), EdgeBuilder.CreateEdge("t-001"))
                   .Task(NodeBuilder.CreateTask("Task-003", "task003"))
                   .End()
                   .Serialize();

            return xmlContent;
        }

        /// <summary>
        /// Create sub process
        /// 创建子流程
        /// </summary>
        private static string CreateFlowSubProcess(ProcessFileEntity fileEntity)
        {
            var wf = Workflow.Create(fileEntity.ProcessName, fileEntity.ProcessCode, fileEntity.ProcessId, fileEntity.Version);
            var xmlContent =
                wf.Start()
                   .Task("Task-001", "task001")
                   .Task(NodeBuilder.CreateSubProcess("InterSubProcess", "subname", "subcode"))
                   .Task("Task-003", "task003")
                   .End()
                   .Serialize();

            return xmlContent;
        }

        /// <summary>
        /// Create flow gateway
        /// 创建分支流程
        /// </summary>
        private static string CreateFlowGateway(ProcessFileEntity fileEntity)
        {
            var wf = Workflow.Create(fileEntity.ProcessName, fileEntity.ProcessCode, fileEntity.ProcessId, fileEntity.Version);
            var xmlContent = 
              wf.Start("start")
                .Task("Task-001", "task001")
                .AndSplit("and-split", "andsplit001")
                .Parallels(
                    ("task-010", "task010"),
                    ("task-020", "task020")
                )
                .AndJoin("and-join", "andjoin001")
                .Task("task-100", "task100")
                .End("end")
                .Serialize();

            return xmlContent;
        }

        /// <summary>
        /// Create flow multiple instance
        /// 创建并行分支容器流程
        /// </summary>
        private static string CreateFlowMI(ProcessFileEntity fileEntity)
        {
            var wf = Workflow.Create(fileEntity.ProcessName, fileEntity.ProcessCode, fileEntity.ProcessId, fileEntity.Version);
            var xmlContent =
                wf.Start()
                   .Task(NodeBuilder.CreateTask("Task-001", "task001").SetUrl("http://www.slickflow.com"))
                   .Task(NodeBuilder.CreateMultipleInstance("Sign Together", "MI001"))
                   .Task(NodeBuilder.CreateTask("Task-003", "task003"))
                   .End()
                   .Serialize();

            return xmlContent;
        }

        /// <summary>
        /// Creating parallel branch containers
        /// 创建并行分支容器流程
        /// </summary>
        private static string CreateFlowSequenceAdvanced(ProcessFileEntity fileEntity)
        {
            var wf = Workflow.Create(fileEntity.ProcessName, fileEntity.ProcessCode, fileEntity.ProcessId, fileEntity.Version);
            var xmlContent =
                wf.Start()
                   .Task(
                        NodeBuilder.CreateTask("Task-001", "task001")
                                   .SetUrl("http://www.slickflow.com")
                                   .AddRole("TestRole")
                    //.AddAction(
                    //     NodeBuilder.CreateActionLocalService(FireTypeEnum.Before,
                    //         string.Empty,
                    //         "Slickflow.Module.External.OrderSubmitService"
                    //     ))
                    )
                   .Task(
                        NodeBuilder.CreateTask("Task-002", "task002"),
                        EdgeBuilder.CreateEdge("t-001")
                    //.AddCondition(ConditionTypeEnum.Expression, "a>2")
                    )
                  .Task(
                        NodeBuilder.CreateSubProcess("Task-003", "task003")
                    // .AddBoundary(
                    //    NodeBuilder.CreateBoundary(EventTriggerEnum.Timer,
                    //        "P2M10D"
                    //))
                    )
                   .End()
                   .Serialize();

            return xmlContent;
        }

        /// <summary>
        /// Create gateway advanced
        /// 创建分支流程
        /// </summary>
        private static string CreateFlowGatewayAdvanced(ProcessFileEntity fileEntity)
        {
            var wf = Workflow.Create(fileEntity.ProcessName, fileEntity.ProcessCode, fileEntity.ProcessId, fileEntity.Version);
            var xmlContent =
                wf.Start("start")
                  .Task(
                        NodeBuilder.CreateTask("Task-001", "task001")
                                   .AddAction(
                                        NodeBuilder.CreateActionLocalService(FireTypeEnum.Before,
                                            string.Empty,
                                            "Slickflow.Module.External.OrderSubmitService"
                                    ))
                  )
                  .AndSplit("and-split", "andsplit001")
                  .Parallels(
                        () => wf.Branch(
                            () => wf.Task("task-010", "task010"),
                            () => wf.Task("task-011", "task011")
                        )
                        , () => wf.Branch(
                             () => wf.Task("task-020", "task020"),
                             () => wf.Task("task-021", "task021")
                         )
                        , () => wf.Branch(
                             () => wf.Task("task-030", "task030"),
                             () => wf.Task("task-031", "task031")
                         )
                  )
                  .AndJoin("and-join")
                  .Task("task-100")
                  .End("end")
                  .Serialize();

            return xmlContent;
        }

        /// <summary>
        /// Create and split multiple instance container
        /// 创建并行分支容器流程
        /// </summary>
        private static string CreateFlowAndSplitMI(ProcessFileEntity fileEntity)
        {
            var wf = Workflow.Create(fileEntity.ProcessName, fileEntity.ProcessCode, fileEntity.ProcessId, fileEntity.Version);
            var xmlContent =
                wf.Start("start")
                  .Task("Task-001", "task001")
                  .AndSplitMI("and-split", "andsplit001")
                  .Parallels(
                        ("task-010", "task010"),
                        ("task-011", "task011")
                  )
                  .AndJoinMI("and-join", "andjoin001")
                  .Task("task-100", "task100")
                  .End("end")
                  .Serialize();

            return xmlContent;
        }

        /// <summary>
        /// Create parallel flow
        /// 创建并行分支容器流程
        /// </summary>
        private static string CreateFlowComplex(ProcessFileEntity fileEntity)
        {
            var wf = Workflow.Create(fileEntity.ProcessName, fileEntity.ProcessCode, fileEntity.ProcessId, fileEntity.Version);
            var xmlContent =
                wf.Start("start")
                  .Task("Task-001", "task001")
                  .OrSplit("or-split", "orsplit001")
                  .Parallels(
                        () => wf.Branch(
                            () => wf.Task("task-010", "task010"),
                            () => wf.Task(NodeBuilder.CreateMultipleInstance("MI-011", "mi011"))
                        ),
                        () => wf.Branch(
                             () => wf.Task("task-020", "task020"),
                             () => wf.Task(NodeBuilder.CreateSubProcess("Sub-021", "subname021", "subcode021"))
                        ), 
                        () => wf.Branch(
                             () => wf.Task("task-030", "task030"),
                             () => wf.Task("task-031", "task031")
                        )
                  )
                  .OrJoin("or-join", "orjoin001")
                  .Task("task-100", "task100")
                  .End("end")
                  .Serialize();

            return xmlContent;
        }

        /// <summary>
        /// Create conditional flow
        /// 创建中间条件节点流程
        /// </summary>
        private static string CreateFlowConditional(ProcessFileEntity fileEntity)
        {
            var wf = Workflow.Create(fileEntity.ProcessName, fileEntity.ProcessCode, fileEntity.ProcessId, fileEntity.Version);
            var xmlContent =
                wf.Start()
                   .Task(NodeBuilder.CreateTask("Task-001", "task001").SetUrl("http://www.slickflow.com"))
                   .Intermediate(
                        NodeBuilder.CreateIntermediate("Intermediate", "inter-002", TriggerTypeEnum.Conditional),
                        EdgeBuilder.CreateEdge("link-001")
                    )
                   .Task(NodeBuilder.CreateTask("Task-003", "task003"))
                   .End()
                   .Serialize();

            return xmlContent;
        }

        /// <summary>
        /// Create ask for leave process
        /// 程序代码创建请假流程
        /// </summary>
        private static string CreateFlowAskforLeave(ProcessFileEntity fileEntity)
        {
            var wf = Workflow.Create(fileEntity.ProcessName, fileEntity.ProcessCode, fileEntity.ProcessId, fileEntity.Version);
            var xmlContent =
                  wf.Start("start")
                    .Task("Apply Submit", "task001")
                    .XOrSplit("XOr-Split", "xorsplit001")
                    .Parallels(
                        () => wf.Branch(
                            () => wf.Task(
                                NodeBuilder.CreateTask("Dept Manager Approval", "task010"),
                                EdgeBuilder.CreateEdge("days<3").AddCondition(ConditionTypeEnum.Expression, "days<3")
                            )
                        ),
                        () => wf.Branch(
                            () => wf.Task(
                                NodeBuilder.CreateTask("CEO Approval", "task020"),
                                EdgeBuilder.CreateEdge("days>=3").AddCondition(ConditionTypeEnum.Expression, "days>=3")
                            )
                        )
                    )
                    .XOrJoin("XOr-Join", "xorjoin001")
                    .Task("HR Approval", "task100")
                    .End("end")
                    .Serialize();

            return xmlContent;
        }

        /// <summary>
        /// Create e-order process
        /// 程序代码创建订单流程
        /// </summary>
        private static string CreateFlowEOrder(ProcessFileEntity fileEntity)
        {
            var wf = Workflow.Create(fileEntity.ProcessName, fileEntity.ProcessCode, fileEntity.ProcessId, fileEntity.Version);
            var xmlContent =
                  wf.Start("start")
                    .Task("Dispatch Order", "task001")
                    .OrSplit("Or-Split", "orsplit001")
                    .Parallels(
                        () => wf.Branch(
                            () => wf.Task(
                                 NodeBuilder.CreateTask("Print Delivery Note", "task010"),
                                 EdgeBuilder.CreateEdge("HasInventory=\"Y\"").AddCondition(ConditionTypeEnum.Expression, "HasInventory=\"Y\"")
                            )
                        ),
                        () => wf.Branch(
                            () => wf.Task(
                                NodeBuilder.CreateTask("Sample Making", "task020"),
                                EdgeBuilder.CreateEdge("HasInventory=\"N\"").AddCondition(ConditionTypeEnum.Expression, "HasInventory=\"N\"")
                            ),
                            () => wf.Task("Produce", "task030"),
                            () => wf.Task("QA", "task040"),
                            () => wf.Task("Weight", "task050")
                        )
                    )
                    .OrJoin("Or-Join", "orjoin001")
                    .End("end")
                    .Serialize();

            return xmlContent;
        }

        /// <summary>
        /// Create reimbursement flow
        /// 程序代码创建报销流程
        /// </summary>
        private static string CreateFlowReimbursement(ProcessFileEntity fileEntity)
        {
            var wf = Workflow.Create(fileEntity.ProcessName, fileEntity.ProcessCode, fileEntity.ProcessId, fileEntity.Version);
            var xmlContent =
                  wf.Start("start")
                    .Task("Reimbursement Submit", "task001")
                    .Task("Finalcial Approval", "task002")
                    .XOrSplit("XOr-Split", "orsplit001")
                    .Parallels(
                        () => wf.Branch(
                            () => wf.Task(
                                NodeBuilder.CreateTask("Approved by the Director in Charge", "task010"),
                                EdgeBuilder.CreateEdge("money<10000").AddCondition(ConditionTypeEnum.Expression, "money<10000")
                            )
                        ),
                        () => wf.Branch(
                            () => wf.Task(
                                NodeBuilder.CreateTask("CEO Approval", "task020"),
                                EdgeBuilder.CreateEdge("money>=10000").AddCondition(ConditionTypeEnum.Expression, "money>=10000")
                            )
                        )
                    )
                    .XOrJoin("XOr-Join", "orjoin001")
                    .End("end")
                    .Serialize();

            return xmlContent;
        }

        /// <summary>
        /// Create warehousing flow
        /// 程序代码创建入库流程
        /// </summary>
        private static string CreateFlowWarehousing(ProcessFileEntity fileEntity)
        {
            var wf = Workflow.Create(fileEntity.ProcessName, fileEntity.ProcessCode, fileEntity.ProcessId, fileEntity.Version);
            var xmlContent =
                  wf.Start("start")
                    .Task("Warehouse Signature", "task001")
                    .XOrSplit("And-Split", "andsplit001")
                    .Parallels(
                        () => wf.Branch(() => wf.Task("Signature of the Comprehensive Department", "task010")),
                        () => wf.Branch(() => wf.Task("Financial Signature", "task020"))
                    )
                    .AndJoin("And-Join", "andjoin001")
                    .Task("CEO Signature", "task007")
                    .End("end")
                    .Serialize();

            return xmlContent;
        }

        /// <summary>
        /// Create office in flow
        /// 程序代码创建办公室领用流程
        /// </summary>
        private static string CreateFlowOfficeIn(ProcessFileEntity fileEntity)
        {
            var wf = Workflow.Create(fileEntity.ProcessName, fileEntity.ProcessCode, fileEntity.ProcessId, fileEntity.Version);
            var xmlContent =
                  wf.Start("start")
                    .Task("Warehouse Signature", "task001")
                    .OrSplit("Or-Split", "orsplit001")
                    .Parallels(
                        () => wf.Branch(
                            () => wf.Task(
                                NodeBuilder.CreateTask("Signature of Administrative Department", "task010"),
                                EdgeBuilder.CreateEdge("surplus = \"normal\"").AddCondition(ConditionTypeEnum.Expression, "surplus = \"normal\"")
                            )
                        ),
                        () => wf.Branch(
                            () => wf.Task(
                                NodeBuilder.CreateTask("Financial Signature", "task020"),
                                EdgeBuilder.CreateEdge("surplus = \"normal\"").AddCondition(ConditionTypeEnum.Expression, "surplus = \"normal\"")
                            )
                        ),
                        () => wf.Branch(
                            () => wf.Task(
                                NodeBuilder.CreateTask("CEO Signature", "task030"),
                                EdgeBuilder.CreateEdge("surplus = \"overamount\"").AddCondition(ConditionTypeEnum.Expression, "surplus = \"overamount\"")
                            )
                        )
                    )
                    .OrJoin("Or-Join", "orjoin001")
                    .Task("Finance Signature", "task007")
                    .End("end")
                    .Serialize();

            return xmlContent;
        }

        /// <summary>
        /// Create contract flow
        /// 程序代码创建合同签订流程
        /// </summary>
        private static string CreateFlowContract(ProcessFileEntity fileEntity)
        {
            var wf = Workflow.Create(fileEntity.ProcessName, fileEntity.ProcessCode, fileEntity.ProcessId, fileEntity.Version);
            var xmlContent =
                wf.Start("start")
                    .Task("Contract Draft", "task001")
                    .Task("Approved by BA Manager", "task002")
                    .AndSplit("And-Split", "andsplit001")
                    .Parallels(
                        () => wf.Branch(() => wf.Task("Contract Department Review", "task010")),
                        () => wf.Branch(() => wf.Task("Financial Department Review", "task020")),
                        () => wf.Branch(() => wf.Task("Group Headquarters Review", "task030"))
                    )
                    .AndJoin("And-Join", "andjoin001")
                    .Task("Contract Archived", "task007")
                    .End("end")
                    .Serialize();

            return xmlContent;
        }
    }
}
