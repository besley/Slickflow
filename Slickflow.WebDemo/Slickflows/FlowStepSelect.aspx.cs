using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

using Slickflow.Engine.Common;
using Slickflow.Engine.Service;
using Slickflow.Engine.Xpdl;

using Slickflow.WebDemoV2._0.Business;


namespace Slickflow.WebDemoV2._0.Slickflows
{
    public partial class FlowStepSelect : BasePage
    {
        public string stepList = string.Empty;
        public string userList = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (LoginUserID > 0)
                {
                    InitStepMember();
                }
                else
                {
                    LiteralMSG.Text = "您可能尚未登录或登录信息已失效，请重新登录后再操作";
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected void InitStepMember()
        {
            //流程定义的GUID
            string flowGuid = Request.QueryString["ProcessGUID"] == null ? "" : Request.QueryString["ProcessGUID"].ToString();
            string Step = Request.QueryString["Step"] == null ? "" : Request.QueryString["Step"].ToString();
            if (string.IsNullOrEmpty(flowGuid) || string.IsNullOrEmpty(Step))
            {
                base.RegisterStartupScript("", "<script>alert('流程GUID为空');</script>");
            }
            else
            {
                String processGUID = flowGuid;
                IWorkflowService service = new WorkflowService();
                switch (Step)
                {
                    case "start"://流程第一步选择
                        ActivityEntity firstActivity = service.GetFirstActivity(processGUID);
                        String firstActivityGUID = firstActivity.ActivityGUID;

                        string conditions = Request.QueryString["condition"] == null ? "" : Request.QueryString["condition"].ToString();
                        
                        IList<NodeView> nextNodes = service.GetNextActivity(processGUID, firstActivityGUID, GetCondition(conditions));
                        if (nextNodes != null)
                        {
                            Repeater1.DataSource = nextNodes;
                            Repeater1.DataBind();
                        }
                        else
                        {
                            LiteralMSG.Text = "当前没有需要您办理的步骤";
                        }
                        break;
                    case "task":
                        try
                        {
                            if (LoginUserID > 0)
                            {
                                string condition = Request.QueryString["condition"] == null ? "" : Request.QueryString["condition"].ToString();
                                string instanceId = Request.QueryString["instanceId"] == null ? string.Empty : Request.QueryString["instanceId"].ToString();
                                WfAppRunner runner = new WfAppRunner();
                                runner.AppInstanceID = instanceId;
                                runner.ProcessGUID = processGUID;
                                runner.UserID = this.LoginUserID.ToString();
                                hiddenIsSelectMember.Value = "true";
                                IList<NodeView> NodeViewList = service.GetNextActivityTree(runner, GetCondition(condition));
                                if (NodeViewList != null)
                                {
                                    Repeater1.DataSource = NodeViewList;
                                    Repeater1.DataBind();
                                    if (NodeViewList.Count == 1)
                                    {
                                        string nextActivityGuid = NodeViewList[0].ActivityGUID;
                                        /*
                                        ActivityEntity activityEntity = service.GetActivityInstance(processGUID, nextActivityGuid);
                                        if (activityEntity.ActivityType == ActivityTypeEnum.EndNode)
                                        {
                                            hiddenIsSelectMember.Value = "false";
                                        }*/
                                    }
                                }
                                else
                                {
                                    LiteralMSG.Text = "当前没有需要您办理的步骤";
                                }
                            }
                        }
                        catch (Exception ex)
                        { }
                        break;
                }
            }
        }



        /// <summary>
        /// 得到人员列表
        /// </summary>
        /// <param name="roleList"></param>
        protected DataTable GetUsers(IList<Role> roleList)
        {
            DataTable dt = new DataTable();
            int i = 0;
            StringBuilder sb = new StringBuilder("");
            foreach (var role in roleList)
            {
                if (i > 0)
                    sb.Append(",");
                sb.Append(role.ID);
                i++;
            }
            string roleIdList = sb.ToString();
            if (!string.IsNullOrEmpty(roleIdList))
            {
                dt = WorkFlows.GetSysUserByRoleIdList(roleIdList);
            }
            return dt;
        }


        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Repeater rep = e.Item.FindControl("Repeater2") as Repeater;//找到里层的repeater对象
                NodeView rowv = (NodeView)e.Item.DataItem;//找到分类Repeater关联的数据项 
                if (rowv.Roles != null)
                {
                    DataTable dt = GetUsers(rowv.Roles);
                    if (dt != null)
                    {
                        rep.DataSource = dt;
                        rep.DataBind();
                    }
                }
            }
        }

    }
}