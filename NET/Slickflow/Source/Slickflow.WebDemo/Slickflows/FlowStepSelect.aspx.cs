using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

using Slickflow.Module.Resource;
using Slickflow.Engine.Common;
using Slickflow.Engine.Service;
using Slickflow.Engine.Xpdl;

using Slickflow.WebDemo.Business;
using Slickflow.WebDemo.Entity;
using Slickflow.WebDemo.Common;


namespace Slickflow.WebDemo.Slickflows
{
    public partial class FlowStepSelect : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (LoginUserID > 0)
                {
                    if (Request.QueryString["Action"] != null && Request.QueryString["Action"].ToString() == "InitStep")
                    {
                        InitStepMember();
                    }
                }
                else
                {
                    LiteralMSG.Text = "您可能尚未登录或登录信息已失效，请重新登录后再操作--Please login firstly!";
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected void InitStepMember()
        {
            WfAppRunner runner = new WfAppRunner();
            string strNodes = string.Empty;

            //流程定义的GUID
            string flowGuid = Request.QueryString["ProcessGUID"] == null ? "" : Request.QueryString["ProcessGUID"].ToString();
            string Step = Request.QueryString["Step"] == null ? "" : Request.QueryString["Step"].ToString();
            if (string.IsNullOrEmpty(flowGuid) || string.IsNullOrEmpty(Step))
            {
                base.RegisterStartupScript("", "<script>alert('流程GUID为空--ProcessGUID is null');</script>");
            }
            else
            {
                List<ZTreeEntity> zTreeEntityList = new List<ZTreeEntity>();

                String processGUID = flowGuid;
                IWorkflowService service = new WorkflowService();
                switch (Step)
                {
                    case "start"://流程第一步选择
                        ActivityEntity firstActivity = service.GetFirstActivity(processGUID, string.Empty);
                        String firstActivityGUID = firstActivity.ActivityGUID;
                        string conditions = Request.QueryString["condition"] == null ? "" : Request.QueryString["condition"].ToString();
                        
                        runner = new WfAppRunner
                        {
                            ProcessGUID = flowGuid,
                            Version = "1",
                            UserID = this.LoginUserID.ToString()
                        };

                        IList<NodeView> nextNodes = service.GetFirstActivityRoleUserTree(runner, GetCondition(conditions));

                        if (nextNodes != null)
                        {
                            if (nextNodes != null)
                            {
                                ZTreeEntity zTreeEntity = null;
                                foreach (NodeView item in nextNodes)
                                {
                                    zTreeEntity = new ZTreeEntity();
                                    zTreeEntity.id = string.Format("step[{0}]", item.ActivityGUID);
                                    zTreeEntity.pId = "0";
                                    zTreeEntity.name = item.ActivityName;
                                    zTreeEntity.nocheck = false;
                                    zTreeEntityList.Add(zTreeEntity);

                                    foreach (var user in item.Users)
                                    {
                                        zTreeEntity = new ZTreeEntity();
                                        zTreeEntity.id = string.Format("step[{0}]member[{1}]", item.ActivityGUID, user.UserID);
                                        zTreeEntity.pId = string.Format("step[{0}]", item.ActivityGUID);
                                        zTreeEntity.name = user.UserName;
                                        zTreeEntity.nocheck = false;
                                        zTreeEntityList.Add(zTreeEntity);
                                    }
                                }
                            }
                        }
                        else
                        {
                            LiteralMSG.Text = "当前没有需要您办理的步骤--none steps choosed";
                        }
                        break;
                    case "task":
                        try
                        {
                            if (LoginUserID > 0)
                            {
                                string condition = Request.QueryString["condition"] == null ? "" : Request.QueryString["condition"].ToString();
                                string instanceId = Request.QueryString["instanceId"] == null ? string.Empty : Request.QueryString["instanceId"].ToString();

                                runner.AppInstanceID = instanceId;
                                runner.ProcessGUID = processGUID;
                                runner.UserID = this.LoginUserID.ToString();
                                hiddenIsSelectMember.Value = "true";
                                //IList<NodeView> NodeViewList = service.GetNextActivityTree(runner, GetCondition(condition));
                                IList<NodeView> NodeViewList = service.GetNextActivityRoleUserTree(runner, GetCondition(condition));


                                if (NodeViewList != null)
                                {
                                    ZTreeEntity zTreeEntity = null;
                                    foreach (NodeView item in NodeViewList)
                                    {
                                        zTreeEntity = new ZTreeEntity();
                                        zTreeEntity.id = string.Format("step[{0}]", item.ActivityGUID);
                                        zTreeEntity.pId = "0";
                                        zTreeEntity.name = item.ActivityName;
                                        zTreeEntityList.Add(zTreeEntity);

                                        foreach (var user in item.Users)
                                        {
                                            zTreeEntity = new ZTreeEntity();
                                            zTreeEntity.id = string.Format("step[{0}]member[{1}]", item.ActivityGUID, user.UserID);
                                            zTreeEntity.pId = string.Format("step[{0}]", item.ActivityGUID);
                                            zTreeEntity.name = user.UserName;
                                            zTreeEntity.nocheck = false;
                                            zTreeEntityList.Add(zTreeEntity);
                                        }
                                    }
                                }
                                else
                                {
                                    LiteralMSG.Text = "当前没有需要您办理的步骤--none steps choosed";
                                }
                            }
                        }
                        catch (Exception ex)
                        { }
                        break;
                }
                strNodes = JsonHelper.ObjectToJson(zTreeEntityList);
            }

            Response.Write(strNodes);
            Response.End();
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



    }
}