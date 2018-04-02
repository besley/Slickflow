using System;
using System.Web.UI;
using System.Data;
using Slickflow.WebDemo.Business;
using Slickflow.WebDemo.Common;


namespace Slickflow.WebDemo.Slickflows
{
    public partial class FlowOpinion : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                InitFlowOpinion();
            }
        }

        protected void InitFlowOpinion()
        {
            string AppInstanceID = Request.QueryString["AppInstanceID"] == null ? string.Empty : Request.QueryString["AppInstanceID"].ToString();
            if (!string.IsNullOrEmpty(AppInstanceID))
            {
                DataTable dt = WorkFlows.GetHrsLeaveOpinionListByAppInstanceID(AppInstanceID);
                Repeater1.DataSource = dt;
                Repeater1.DataBind();
            }
            else
            {
                Repeater1.DataSource = null;
                Repeater1.DataBind();
            }
        }

    }
}