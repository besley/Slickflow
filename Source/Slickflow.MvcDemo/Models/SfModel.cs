using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using SlickOne.WebUtility;
using Slickflow.Engine.Common;


namespace Slickflow.MvcDemo.Models
{
    /// <summary>
    /// EAV模型
    /// </summary>
    public class SfModel
    {
        public List<Role> GetRolesByProcess()
        {
            var query = new RoleQuery();
            query.ProcessGUID = "5c5041fc-ab7f-46c0-85a5-6250c3aea375";
            query.Version = "1";

            var hostUrl = ConfigurationManager.AppSettings["WebApiHostUrl"];
            var url = string.Format("http://{0}/sfmvc/api/wf/QueryProcessRoleUserList", hostUrl);
            var clientHelper = HttpClientHelper.CreateHelper(url);
            var roleList = clientHelper.Query<RoleQuery, Role>(query);

            return roleList;
        }
    }
}