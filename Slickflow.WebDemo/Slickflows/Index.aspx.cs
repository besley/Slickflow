using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Slickflow.WebDemoV2._0.Slickflows
{
    public partial class Index : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                try
                {
                    Litera_USER.Text = string.Format("您好，{0}:{1}", HttpContext.Current.Session["RoleName"].ToString(), HttpContext.Current.Session["UserName"].ToString());
                }
                catch (Exception ex)
                {
                    Litera_USER.Text = "未登录";
                }
            }
        }
    }
}