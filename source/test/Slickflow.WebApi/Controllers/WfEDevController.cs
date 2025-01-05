using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SlickOne.WebUtility;
using Slickflow.Data;
using Slickflow.Engine.Common;
using Slickflow.Engine.Core.Result;
using Slickflow.Graph;
using Slickflow.Engine.Service;

namespace Slickflow.WebApi.Controllers
{
    /// <summary>
    /// EDev Controller
    /// </summary>
    public class WfEDevController : Controller
    {
        /// <summary>
        /// example"
        /// using Slickflow.Graph;

        //var pmb = ProcessModelBuilder.CreateProcess("BookSellerProcess", "BookSellerProcessCode");
        //var process = pmb.Start("Start")
        //                 .Task("Package Books")
        //                 .Task("Deliver Books")
        //                 .End("End")
        //                 .Store();
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        [HttpPost]
        public string CreateByText([FromBody] ProcessGraph graph)
        {
            var message = "OK";
            try
            {
                if (graph != null && !string.IsNullOrEmpty(graph.Body))
                {
                    var roslyResult = RoslynHotSpot.Execute(graph.Body);
                }
                else
                {
                    message = "The code is empty!";
                }
            }
            catch (System.Exception ex)
            {
                message = ex.Message;
            }

            return message;
        }

        [HttpGet]
        public string Create()
        {
            string message = "OK";
            try
            {
                var pmb = ProcessModelBuilder.CreateProcess("EDev", "EDevCode");
                var process = pmb.Start("Start")
                                 .Task("Task1")
                                 .OrSplit("OrSplit")
                                     .Parallels(
                                            () => pmb.Branch(
                                                () => pmb.Task("Task2")
                                            ),
                                            () => pmb.Branch(
                                                ()=>pmb.Task("Task3")
                                            )
                                      )
                                .OrJoin("OrJoin")
                                .Task("Task4")
                                .End("End")
                                .Store();
            }
            catch (System.Exception ex)
            {
                message = ex.Message;
            }
            return message;
        }

        [HttpGet]
        public string CreateSimple()
        {
            string message = "OK";
            try
            {
                var pmb = ProcessModelBuilder.CreateProcess("EDevTrainingApproval", "EDevTrainingApprovalFlow");
                var process = pmb.Start("Start")
                                 .Task("Approvers in Progress")//1st Approval In progress
                                 .OrSplit("Clainms?")
                                    .Parallels(
                                        false,
                                        (a)=>pmb.Branch(
                                            a,
                                            () => pmb.End(
                                                      VertexBuilder.CreateEnd("End"),
                                                      LinkBuilder.CreateTransition("Completed Enrollment w/o Claim")
                                                           .AddCondition(ConditionTypeEnum.Expression, "Claims = 0")
                                                  )),
                                        (a)=>pmb.Branch(
                                            a,
                                            ()=>pmb.OrSplit("Pre Claims?")
                                                .Parallels(
                                                () => pmb.Branch(
                                                    () => pmb.Dumb()
                                                    //()=>pmb.Task("Task1001")
                                                ),
                                                () => pmb.Branch(
                                                    () => pmb.Task(
                                                            VertexBuilder.CreateTask("Pending for Reimbursement"),
                                                            LinkBuilder.CreateTransition("Completed Enrollment w/o Claim")
                                                                .AddCondition(ConditionTypeEnum.Expression, "Norminated = 4")
                                                    )
                                                )
                                            )
                                            .Task("Pending for Finance Reimbursement", "PendingforFinaceReimbursementCode", null, true)
                                            .End("End Workflow")
                                      )
                                   )
                                .Store();
            }
            catch (System.Exception ex)
            {
                message = ex.Message;
            }
            return message;
        }

        [HttpGet]
        public string CreateEDev()
        {
            string message = "OK";
            try
            {
                var pmb = ProcessModelBuilder.CreateProcess("EDevTrainingApproval", "EDevTrainingApprovalFlow");
                var process = pmb.Start("Start")
                                 .Task("Approvers in Progress")//1st Approval In progress
                                 .Task("Approvers in Progress")//2nd Approval In progress
                                 .OrSplit("3rd Approval Need?")
                                     .Parallels(
                                             () => pmb.Branch(
                                                () => pmb.Task(
                                                        VertexBuilder.CreateTask("Approvers in Progress"),//3rd Approval In progress
                                                        LinkBuilder.CreateTransition("Required 3rd Approval")
                                                            .AddCondition(ConditionTypeEnum.Expression, "Norminated = 4")//Training Type (Attend duing office Hour)
                                                 )
                                             ),
                                             () => pmb.Branch(
                                                 () => pmb.Dumb()
                                             )
                                     )
                                 .Task("SND in Process", "SNDInProcessCode", null, true)//SND in Process
                                 .OrSplit("Claims?")
                                     .Parallels(
                                            false,
                                            (a) => pmb.Branch(
                                                a,
                                                () => pmb.Task(
                                                      VertexBuilder.CreateEnd("End"),
                                                      LinkBuilder.CreateTransition("Completed Enrollment w/o Claim")
                                                           .AddCondition(ConditionTypeEnum.Expression, "Claims = 0")
                                                  )
                                            ),

                                            //Claim = 1
                                            (a) => pmb.Branch(
                                                 a,
                                                 () => pmb.OrSplit("Pre Approval Claim ?")
                                                    .Parallels(
                                                        () => pmb.Branch(
                                                            () => pmb.Dumb()
                                                        ),
                                                        () => pmb.Branch(
                                                            () => pmb.Task(
                                                                    VertexBuilder.CreateTask("Pending for Reimbursement"),
                                                                    LinkBuilder.CreateTransition("Completed Enrollment w/o Claim")
                                                                        .AddCondition(ConditionTypeEnum.Expression, "Norminated = 4")
                                                            )
                                                        )
                                                    )
                                                 .Task("Pending for Finance Reimbursement", "PendingforFinaceReimbursementCode", null, true)
                                                 .End("End Workflow")
                                            )
                                    )
                                .Store();
            }
            catch (System.Exception ex)
            {
                message = ex.Message;
            }
            return message;
        }

        [HttpGet]
        public string Start()
        {
            IWorkflowService wfService = new WorkflowService();
            var wfResult = wfService.CreateRunner("10", "jack")
                     .UseApp("BS-100", "Delivery-Books", "BS-100-LX")
                     .UseProcess("BookSellerProcessCode")
                     .Start();
            return wfResult.Status.ToString();
        }

        [HttpGet]
        public string Run(int id)
        {
            IWorkflowService wfService = new WorkflowService();
            var wfResult = wfService.CreateRunner("10", "jack")
                     .UseApp("BS-100", "Delivery-Books", "BS-100-LX")
                     .UseProcess("BookSellerProcessCode")
                     .OnTask(id)        //TaskID
                     .NextStepInt("20", "Alice")
                     .Run();
            return wfResult.Status.ToString();
        }

        [HttpGet]
        public string Withdraw(int id)
        {
            IWorkflowService wfService = new WorkflowService();
            var wfResult = wfService.CreateRunner("10", "Jack")
                     .UseApp("BS-100", "Delivery-Books", "BS-100-LX")
                     .UseProcess("BookSellerProcessCode")
                     .OnTask(id)             //TaskID
                     .Withdraw();
            return wfResult.Status.ToString();
        }

        [HttpGet]
        public string SendBack(int id)
        {
            IWorkflowService wfService = new WorkflowService();
            var wfResult = wfService.CreateRunner("20", "Alice")
                     .UseApp("BS-100", "Delivery-Books", "BS-100-LX")
                     .UseProcess("BookSellerProcessCode")
                     .PrevStepInt()
                     .OnTask(id)             //TaskID
                     .SendBack();
            return wfResult.Status.ToString();
        }
    }
}