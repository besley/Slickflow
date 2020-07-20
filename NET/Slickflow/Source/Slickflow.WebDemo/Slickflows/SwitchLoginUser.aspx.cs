using Slickflow.WebDemo.Business;
using Slickflow.WebDemo.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Slickflow.WebDemo.Slickflows
{
    public partial class SwitchLoginUser : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                HttpContext.Current.Session.Clear();
                HttpContext.Current.Session.Remove("UserId");
                HttpContext.Current.Session.Remove("UserName");
                HttpContext.Current.Session.Remove("RoleId");
                HttpContext.Current.Session.Remove("RoleName");
                Helper.BindDropDownList(this.ddlRole, WorkFlows.GetSysRole(), "RoleName", "ID", true, "请选择系统角色(SelectRole)", "0");
            }
        }

        protected void ddlRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            int rId = Convert.ToInt32(ddlRole.SelectedValue.ToString());
            Helper.BindDropDownList(this.ddlUser, WorkFlows.GetSysUser(rId), "UserName", "ID", true, "请选择系统用户(SelectUser)", "0");
        }


        //用户切换
        protected void btnSwitchUser_Click(object sender, EventArgs e)
        {
            int RoleId = Helper.ConverToInt32(this.ddlRole.SelectedValue.ToString());
            int UserId = Helper.ConverToInt32(this.ddlUser.SelectedValue.ToString());
            if (RoleId > 0)
            {
                if (UserId > 0)
                {
                    try
                    {
                        HttpContext.Current.Session["UserId"] = UserId.ToString();
                        HttpContext.Current.Session["UserName"] = this.ddlUser.SelectedItem.Text.ToString();
                        HttpContext.Current.Session["RoleId"] = RoleId.ToString();
                        HttpContext.Current.Session["RoleName"] = this.ddlRole.SelectedItem.Text.ToString();
                        HttpContext.Current.Session.Timeout = 60;
                        base.RegisterStartupScript("", "<script>alert('切换成功--Successed'); window.parent.location.href = window.parent.location.href;</script>");
                    }
                    catch (Exception ex)
                    {
                        base.RegisterStartupScript("", "<script>alert('切换失败--Failed');</script>");
                    }
                }
                else
                {
                    base.RegisterStartupScript("", "<script>alert('请选择系统用户--Please select user');</script>");
                }
            }
            else
            {
                base.RegisterStartupScript("", "<script>alert('请选择系统角色--Please select role');</script>");
            }
        }
    }
}