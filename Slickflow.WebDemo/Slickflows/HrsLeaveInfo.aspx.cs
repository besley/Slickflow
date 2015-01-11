using System;
using System.Collections.Generic;
using System.Web.UI;


using Slickflow.WebDemoV2._0.Business;
using Slickflow.WebDemoV2._0.Common;
using Slickflow.WebDemoV2._0.Entity;

using Slickflow.Engine.Common;
using Slickflow.Engine.Core.Result;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Service;
using Slickflow.Engine.Xpdl;


namespace Slickflow.WebDemoV2._0.Slickflows
{
    public partial class HrsLeaveInfo : BasePage
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
            int AppInstanceID = Request.QueryString["AppInstanceID"] == null ? 0 : Helper.ConverToInt32(Request.QueryString["AppInstanceID"].ToString());
            if (AppInstanceID > 0)
            {
                HrsLeaveEntity hrsLeaveEntity = WorkFlows.GetHrsLeaveModel(AppInstanceID);
                if (hrsLeaveEntity != null && hrsLeaveEntity.ID > 0)
                {
                    selectLeaveType.Value = hrsLeaveEntity.LeaveType.ToString();
                    selectLeaveType.Disabled = true;
                    txtDays.Value = hrsLeaveEntity.Days.ToString();
                    txtFromDate.Value = hrsLeaveEntity.FromDate.ToString("yyyy-MM-dd");
                    txtToDate.Value = hrsLeaveEntity.ToDate.ToString("yyyy-MM-dd");

                    this.txtDepmanagerRemark.Value = hrsLeaveEntity.DepManagerRemark;
                    this.txtDirectorRemark.Value = hrsLeaveEntity.DirectorRemark;//主管总监
                    this.txtDeputyGeneralRemark.Value = hrsLeaveEntity.DeputyGeneralRemark;//副总经理
                    this.txtGeneralManagerRemark.Value = hrsLeaveEntity.GeneralManagerRemark;//总经理

                    txtCreatedByUserName.Value = hrsLeaveEntity.CreatedUserName;
                    txtCreatedDateTime.Value = hrsLeaveEntity.CreatedDate.ToString("yyyy-MM-dd");
                }
            }
        }



    }
}