using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Slickflow.WebDemo.Slickflows
{
    public partial class Tool : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnCreateGUID_Click(object sender, EventArgs e)
        {
            this.txtNewGuid.Value = Guid.NewGuid().ToString();
        }
    }
}