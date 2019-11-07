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
    /// 串行序列流程测试流程
    /// </summary>
    public class WfEDevController : Controller
    {
        [HttpGet]
        public string Create()
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
    }
}