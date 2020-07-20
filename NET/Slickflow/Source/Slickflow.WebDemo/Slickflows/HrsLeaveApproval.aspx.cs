using System;
using System.Collections.Generic;
using System.Web.UI;


using Slickflow.WebDemo.Business;
using Slickflow.WebDemo.Common;
using Slickflow.WebDemo.Entity;

using Slickflow.Engine.Common;
using Slickflow.Engine.Core.Result;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Service;
using Slickflow.Engine.Xpdl;



namespace Slickflow.WebDemo.Slickflows
{
    public partial class HrsLeaveApproval : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                InitFlowInfo();
            }
        }

        protected void InitFlowInfo()
        {
            string AppInstanceID = Request.QueryString["AppInstanceID"] == null ? string.Empty : Request.QueryString["AppInstanceID"].ToString();
            string ProcessGUID = Request.QueryString["ProcessGUID"] == null ? string.Empty : Request.QueryString["ProcessGUID"].ToString();
            int ActivityInstanceID = Request.QueryString["ActivityInstanceID"] == null ? 0 : Helper.ConverToInt32(Request.QueryString["ActivityInstanceID"].ToString());
            if (!string.IsNullOrEmpty(AppInstanceID))
            {
                int leaveID = int.Parse(AppInstanceID);
                HrsLeaveEntity hrsLeaveEntity = WorkFlows.GetHrsLeaveModel(leaveID);
                if (hrsLeaveEntity != null && hrsLeaveEntity.ID > 0)
                {
                    selectLeaveType.Value = hrsLeaveEntity.LeaveType.ToString();
                    selectLeaveType.Disabled = true;
                    txtDays.Value = hrsLeaveEntity.Days.ToString();
                    txtFromDate.Value = hrsLeaveEntity.FromDate.ToString("yyyy-MM-dd");
                    txtToDate.Value = hrsLeaveEntity.ToDate.ToString("yyyy-MM-dd");
                    hiddenInstanceId.Value = hrsLeaveEntity.ID.ToString();
                    hiddenActivityInstanceID.Value = ActivityInstanceID.ToString();

                    this.txtDepmanagerRemark.Value = hrsLeaveEntity.DepManagerRemark;
                    this.txtDirectorRemark.Value = hrsLeaveEntity.DirectorRemark;//主管总监
                    this.txtDeputyGeneralRemark.Value = hrsLeaveEntity.DeputyGeneralRemark;//副总经理
                    this.txtGeneralManagerRemark.Value = hrsLeaveEntity.GeneralManagerRemark;//总经理


                    //权限设置
                    IWorkflowService service = new WorkflowService();
                    ActivityInstanceEntity activityInstanceEntity = service.GetActivityInstance(ActivityInstanceID);
                    ProcessInstanceEntity processInstanceEntity = service.GetProcessInstance(activityInstanceEntity.ProcessInstanceID);

                    this.txtProcessGUID.Value = activityInstanceEntity.ProcessGUID;
                    if (activityInstanceEntity != null)
                    {
                        ActivityEntity activityEntity = service.GetActivityEntity(processInstanceEntity.ProcessGUID,
                            processInstanceEntity.Version,
                            activityInstanceEntity.ActivityGUID);

                        var roles = service.GetActivityRoles(processInstanceEntity.ProcessGUID,
                            processInstanceEntity.Version,
                            activityInstanceEntity.ActivityGUID);

                        if (activityEntity != null && roles != null && roles.Count > 0)
                        {
                            foreach (var role in roles)
                            {
                                if (role.ID == LoginRoleID.ToString())
                                {
                                    switch (role.ID)
                                    {
                                        case "2"://部门经理
                                            this.txtDepmanagerRemark.Disabled = false;
                                            hiddenPerformField.Value = "DepManager";
                                            break;

                                        case "4"://主管总监
                                            this.txtDirectorRemark.Disabled = false;
                                            hiddenPerformField.Value = "Director";
                                            break;

                                        case "7"://副总经理
                                            this.txtDeputyGeneralRemark.Disabled = false;
                                            hiddenPerformField.Value = "Deputy";
                                            break;

                                        case "8"://总经理
                                            this.txtGeneralManagerRemark.Disabled = false;
                                            hiddenPerformField.Value = "General";
                                            break;
                                    }
                                }
                            }
                        }
                    }

                }
            }
        }


        //送往下一步
        protected void btnSendNext_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime now = DateTime.Now;
                string CurrentActivityText = string.Empty;
                string currentOpinionRemark = string.Empty;
                string processGUID = this.txtProcessGUID.Value.ToString();
                decimal days = Helper.ConverToDecimal(this.txtDays.Value);
                string instanceId = this.hiddenInstanceId.Value;
                string DepManagerRemark = this.txtDepmanagerRemark.Value;
                string DirectorRemark = this.txtDirectorRemark.Value;
                string DeputyGeneralRemark = this.txtDeputyGeneralRemark.Value;
                string GeneralManagerRemark = this.txtGeneralManagerRemark.Value;
                int activityInstanceID = Helper.ConverToInt32(hiddenActivityInstanceID.Value);
                string strNextActivityPerformers = hiddenNextActivityPerformers.Value.ToString().Trim();
                IDictionary<string, PerformerList> nextActivityPerformers = NextActivityPerformers(strNextActivityPerformers);
                if (nextActivityPerformers == null)
                {
                    base.RegisterStartupScript("", "<script>alert('请选择办理步骤或办理人员--Please select next step and user');</script>");
                    return;
                }
                switch (hiddenPerformField.Value.ToString())
                {
                    case "DepManager"://部门经理
                        currentOpinionRemark = this.txtDepmanagerRemark.Value;
                        break;

                    case "Director"://主管总监
                        currentOpinionRemark = this.txtDirectorRemark.Value;
                        break;
                    case "Deputy"://副总经理
                        currentOpinionRemark = this.txtDeputyGeneralRemark.Value;
                        break;
                    case "General"://总经理
                        currentOpinionRemark = this.txtGeneralManagerRemark.Value;
                        break;
                }

                if (!string.IsNullOrEmpty(instanceId))
                {
                    //调用流程
                    IWorkflowService service = new WorkflowService();

                    WfAppRunner initiator = new WfAppRunner();
                    initiator.AppName = "请假流程--AskforLeaveProcess";
                    initiator.AppInstanceID = instanceId;
                    initiator.ProcessGUID = processGUID;
                    initiator.UserID = LoginUserID.ToString();
                    initiator.UserName = LoginUserName;
                    initiator.Conditions = GetCondition(string.Format("days-{0}", days));
                    initiator.NextActivityPerformers = nextActivityPerformers;
                    WfExecutedResult runAppResult = service.RunProcessApp(initiator);
                    if (runAppResult.Status != WfExecutedStatus.Success)
                    {
                        base.RegisterStartupScript("", "<script>alert('" + runAppResult.Message + "');</script>");
                        return;
                    }

                    ActivityInstanceEntity activityInstanceEntity = service.GetActivityInstance(activityInstanceID);
                    if (activityInstanceEntity != null)
                    {
                        CurrentActivityText = activityInstanceEntity.ActivityName;
                    }
                    try
                    {
                        //保存业务数据
                        //BizAppFlowEntity AppFlowEntity = new Entity.BizAppFlowEntity();
                        //AppFlowEntity.AppName = "请假流程";
                        //AppFlowEntity.AppInstanceID = instanceId.ToString();
                        //AppFlowEntity.ActivityName = CurrentActivityText;
                        //AppFlowEntity.Remark = string.Format("{0}(ID:{1}) {2}", LoginUserName, LoginUserID, currentOpinionRemark);
                        //AppFlowEntity.ChangedTime = now;
                        //AppFlowEntity.ChangedUserID = LoginUserID.ToString();
                        //AppFlowEntity.ChangedUserName = LoginUserName;


                        HrsLeaveOpinionEntity hrsleaveOpinionEntity = new HrsLeaveOpinionEntity();
                        hrsleaveOpinionEntity.AppInstanceID = instanceId.ToString();
                        hrsleaveOpinionEntity.ActivityID = activityInstanceEntity.ActivityGUID.ToString();
                        hrsleaveOpinionEntity.ActivityName = CurrentActivityText;
                        hrsleaveOpinionEntity.Remark = string.Format("{0}(ID:{1}) {2}", LoginUserName, LoginUserID, currentOpinionRemark);
                        hrsleaveOpinionEntity.ChangedTime = now;
                        hrsleaveOpinionEntity.ChangedUserID = LoginUserID.ToString();
                        hrsleaveOpinionEntity.ChangedUserName = LoginUserName;
                        WorkFlows.AddHrsLeaveOpinion(hrsleaveOpinionEntity);

                    }
                    catch (Exception ex)
                    { }

                    try
                    {
                        HrsLeaveEntity hrsLeaveEntity = new Entity.HrsLeaveEntity();
                        hrsLeaveEntity.ID = Helper.ConverToInt32(instanceId);
                        hrsLeaveEntity.DepManagerRemark = DepManagerRemark;
                        hrsLeaveEntity.DirectorRemark = DirectorRemark;
                        hrsLeaveEntity.DeputyGeneralRemark = DeputyGeneralRemark;
                        hrsLeaveEntity.GeneralManagerRemark = GeneralManagerRemark;
                        hrsLeaveEntity.CurrentActivityText = CurrentActivityText;
                        WorkFlows.UpdateHrsLeave(hrsLeaveEntity);
                    }
                    catch (Exception ex)
                    { }

                    base.RegisterStartupScript("", "<script>alert('办理成功--Successed');window.location.href='FlowList.aspx';</script>");

                }
            }
            catch (Exception ex)
            {
                base.ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('流程发起出现异常--EXCEPTION:" + ex.ToString() + "');</script>");
            }
        }


    }
}