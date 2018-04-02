using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slickflow.Engine.Common;
using Slickflow.Module.Resource;
using Slickflow.Module.Resource.Service;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// 执行者构造类
    /// </summary>
    internal class PerformerBuilder
    {
        /// <summary>
        /// 创建活动节点执行者列表
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="userName">用户名称</param>
        /// <returns></returns>
        internal static PerformerList CreatePerformerList(string userID, string userName)
        {
            var performerList = new PerformerList();
            performerList.Add(new Performer(userID, userName));

            return performerList;
        }


        /// <summary>
        /// 创建活动节点执行者列表
        /// </summary>
        /// <param name="roleList">角色列表</param>
        /// <returns></returns>
        internal static PerformerList CreatePerformerList(IList<Role> roleList)
        {
            var roleIDs = roleList.Select(x => x.ID).ToArray();
            var resourceService = ResourceServiceFactory.Create();
            var userList = resourceService.GetUserListByRoles(roleIDs);
            var performerList = new PerformerList();

            foreach (var user in userList)
            {
                var performer = new Performer(user.UserID.ToString(), user.UserName);

                performerList.Add(performer);
            }
            return performerList;
        }
    }
}
