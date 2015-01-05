using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;


namespace Slickflow.WebApi
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);


            //use Json format 
            SetJsonFormat();
        }

        /// <summary>
        /// 注册容器类
        /// </summary>
        private void RegisterContainer()
        {
            //var builder = new ContainerBuilder();
            //var catalog = new DirectoryCatalog("Plugin");
            //builder.RegisterComposablePartCatalog(catalog);
            //WebAppContainer = builder.Build();
        }

        /// <summary>
        /// 设置JSon格式
        /// </summary>
        private void SetJsonFormat()
        {
            var config = GlobalConfiguration.Configuration;
            config.Formatters.Remove(config.Formatters.XmlFormatter);
        }
    }
}