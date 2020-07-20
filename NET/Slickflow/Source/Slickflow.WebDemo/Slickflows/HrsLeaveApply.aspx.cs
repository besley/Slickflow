using System;
using System.Collections.Generic;
using System.Web.UI;

using Slickflow.WebDemo.Business;
using Slickflow.WebDemo.Common;
using Slickflow.WebDemo.Entity;

using Slickflow.Engine.Common;
using Slickflow.Engine.Core.Result;
using Slickflow.Engine.Service;



namespace Slickflow.WebDemo.Slickflows
{
    public partial class HrsLeaveApply : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

            }
        }

        //提交请假信息
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string processGUID = this.txtProcessGUID.Value.ToString();
                decimal days = Helper.ConverToDecimal(this.txtDays.Value);
                int leaveType = Helper.ConverToInt32(selectLeaveType.Value);
                string strNextActivityPerformers = hiddenNextActivityPerformers.Value.ToString().Trim();

                IDictionary<string, PerformerList> nextActivityPerformers = NextActivityPerformers(strNextActivityPerformers);
                if (nextActivityPerformers == null)
                {
                    base.RegisterStartupScript("", "<script>alert('请选择办理步骤或办理人员--Please select next step and user');</script>");
                    return;
                }

                DateTime now = DateTime.Now;

                //请假业务数据
                HrsLeaveEntity hrsLeaveEntity = new HrsLeaveEntity();
                hrsLeaveEntity.LeaveType = leaveType;
                hrsLeaveEntity.Days = days;
                try
                {
                    hrsLeaveEntity.FromDate = Helper.ConvertToDateTime(this.txtFromDate.Value, now);
                }
                catch (Exception ex)
                {
                    hrsLeaveEntity.FromDate = now;
                }
                try
                {
                    hrsLeaveEntity.ToDate = Helper.ConvertToDateTime(this.txtToDate.Value, now);
                }
                catch (Exception ex)
                {
                    hrsLeaveEntity.ToDate = now;
                }
                hrsLeaveEntity.CurrentActivityText = string.Empty;
                hrsLeaveEntity.Status = 0;
                hrsLeaveEntity.CreatedUserID = LoginUserID;
                hrsLeaveEntity.CreatedUserName = this.LoginUserName;
                hrsLeaveEntity.CreatedDate = now;

                int instanceId = WorkFlows.AddHrsLeave(hrsLeaveEntity);
                if (instanceId > 0)
                {
                    //调用流程
                    IWorkflowService service = new WorkflowService();

                    WfAppRunner initiator = new WfAppRunner();
                    initiator.AppName = "请假流程(AskforLeaveProcess)";
                    initiator.AppInstanceID = instanceId.ToString();
                    initiator.ProcessGUID = processGUID;
                    initiator.UserID = LoginUserID.ToString();
                    initiator.UserName = LoginUserName;
                    initiator.Conditions = GetCondition(string.Format("days-{0}", days));
                    WfExecutedResult startedResult = service.StartProcess(initiator);
                    if (startedResult.Status != WfExecutedStatus.Success)
                    {
                        base.RegisterStartupScript("", "<script>alert('" + startedResult.Message + "');</script>");
                        return;
                    }

                    //送往下一步
                    /*
                    PerformerList pList = new PerformerList();
                    pList.Add(new Performer(nextUserID.ToString(), nextUserName));

                    initiator.NextActivityPerformers = new Dictionary<String, PerformerList>();
                    initiator.NextActivityPerformers.Add(stepGuid, pList);
                    */
                    initiator.NextActivityPerformers = nextActivityPerformers;
                    WfExecutedResult runAppResult = service.RunProcessApp(initiator);
                    if (runAppResult.Status != WfExecutedStatus.Success)
                    {
                        base.RegisterStartupScript("", "<script>alert('" + runAppResult.Message + "');</script>");
                        return;
                    }
                    //保存业务数据
                    //BizAppFlowEntity AppFlowEntity = new Entity.BizAppFlowEntity();
                    //AppFlowEntity.AppName = "流程发起";
                    //AppFlowEntity.AppInstanceID = instanceId.ToString();
                    //AppFlowEntity.ActivityName = "流程发起";
                    //AppFlowEntity.Remark = string.Format("申请人:{0}-{1}", LoginUserID, LoginUserName);
                    //AppFlowEntity.ChangedTime = now;
                    //AppFlowEntity.ChangedUserID = LoginUserID.ToString();
                    //AppFlowEntity.ChangedUserName = LoginUserName;

                    //WorkFlows.AddBizAppFlow(AppFlowEntity);

                    HrsLeaveOpinionEntity hrsleaveOpinionEntity = new HrsLeaveOpinionEntity();
                    hrsleaveOpinionEntity.AppInstanceID = instanceId.ToString();
                    hrsleaveOpinionEntity.ActivityID = System.Guid.Empty.ToString();
                    hrsleaveOpinionEntity.ActivityName = "流程发起(Apply)";
                    hrsleaveOpinionEntity.Remark = string.Format("申请人(Applicant):{0}-{1}", LoginUserID, LoginUserName);
                    hrsleaveOpinionEntity.ChangedTime = now;
                    hrsleaveOpinionEntity.ChangedUserID = LoginUserID.ToString();
                    hrsleaveOpinionEntity.ChangedUserName = LoginUserName;
                    WorkFlows.AddHrsLeaveOpinion(hrsleaveOpinionEntity);


                    base.RegisterStartupScript("", "<script>alert('流程发起成功--Successed');window.location.href='FlowList.aspx';</script>");

                }
            }
            catch (Exception ex)
            {
                base.RegisterStartupScript("", "<script>alert('流程发起出现异常--EXCEPTION:" + ex.ToString() + "');</script>");
            }
        }
    }
}
