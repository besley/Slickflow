using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Serialization;

namespace Slickflow.Designer
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                        name: "defaultApi",
                        routeTemplate: "api/{controller}/{action}/{id}",
                        defaults: new
                        {
                //action = RouteParameter.Optional,
                id = RouteParameter.Optional
                        });

            config.Routes.MapHttpRoute(
                name: "ActionApi",
                routeTemplate: "api/{controller}/{action}/{skip}/{take}/{id}",
                defaults: new
                {
                    id = RouteParameter.Optional
                });
        }
    }
}
