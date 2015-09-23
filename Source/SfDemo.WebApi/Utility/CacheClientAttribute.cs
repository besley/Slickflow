using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;

namespace SfDemo.WebApi.Utility
{
    /// <summary>
    /// 缓存处理
    /// </summary>
    public class CacheClientAttribute : ActionFilterAttribute
    {
        public int Duration { get; set; }
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            actionExecutedContext.Response.Headers.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue
            {
                MaxAge = TimeSpan.FromHours(Duration),
                MustRevalidate = true,
                Public = true
            };
            base.OnActionExecuted(actionExecutedContext);
        }
    }
}