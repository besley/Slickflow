using System;
using System.Web.UI;


using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Service;


namespace Slickflow.WebDemoV2._0.Slickflows
{
    public partial class FlowCanvas : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                InitFlowXmlInfo();
            }
        }


        protected void InitFlowXmlInfo()
        {
            String ProcessGUID = Request.QueryString["ProcessGUID"] == null ? String.Empty : Request.QueryString["ProcessGUID"].ToString();
            if (ProcessGUID != String.Empty)
            {
                IWorkflowService wfService = new WorkflowService();
                ProcessFileEntity processFileEntity = wfService.GetProcessFile(ProcessGUID);
                if (processFileEntity != null)
                {
                    this.txtProcessGUID.Value = processFileEntity.ProcessGUID.ToString();
                    this.wfxml.InnerHtml = processFileEntity.XmlContent;
                }
            }
        }
    }
}