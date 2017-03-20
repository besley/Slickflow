using System;
using System.Web.UI;
using System.Data;

using Slickflow.WebDemoV2._0.Business;
using Slickflow.WebDemoV2._0.Common;



namespace Slickflow.WebDemoV2._0.Slickflows
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
                DataTable dt = WorkFlows.GetBizAppFlow(string.Format(" and AppInstanceID='{0}'", AppInstanceID));
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