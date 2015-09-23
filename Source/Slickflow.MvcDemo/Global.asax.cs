
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Configuration;

namespace Slickflow.MvcDemo
{
    public class Global : System.Web.HttpApplication
    {
        private static string _webApiHostUrl = string.Empty;
        internal static string WebAPIHostUrl
        {
            get
            {
                if (_webApiHostUrl == string.Empty)
                    _webApiHostUrl = WebConfigurationManager.AppSettings["WebApiHostUrl"].ToString();

                return _webApiHostUrl;
            }
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}