using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Reflection;
using System.IO;

namespace SfDemo.WebApi.Controllers
{
    public class TestController : ApiControllerBase
    {
        [HttpGet]
        public string GetAppPath()
        {
            //var applicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            //return applicationBase;
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            var directory = Path.GetDirectoryName(path);
            return directory;
        }
    }
}
