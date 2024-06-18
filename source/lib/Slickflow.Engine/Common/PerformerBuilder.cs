using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slickflow.Engine.Common;
using Slickflow.Module.Resource;

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
            PerformerList performerList = new PerformerList();
            if (roleList.Count() > 0)
            {
                var roleIDs = roleList.Select(x => x.ID).ToArray();
                var resourceService = ResourceServiceFactory.Create();
                var userList = resourceService.GetUserListByRoles(roleIDs);
                performerList = new PerformerList();

                foreach (var user in userList)
                {
                    var performer = new Performer(user.UserID.ToString(), user.UserName);

                    performerList.Add(performer);
                }
            }
            return performerList;
        }

        /// <summary>
        /// 生成任务办理人ID字符串列表
        /// </summary>
        /// <param name="performerList">操作者列表</param>
        /// <returns>ID字符串列表</returns>
        internal static string GenerateActivityAssignedUserIDs(PerformerList performerList)
        {
            StringBuilder strBuilder = new StringBuilder(1024);
            foreach (var performer in performerList)
            {
                if (strBuilder.ToString() != "")
                    strBuilder.Append(",");
                strBuilder.Append(performer.UserID);
            }
            return strBuilder.ToString();
        }

        /// <summary>
        /// 生成办理人名称的字符串列表
        /// </summary>
        /// <param name="performerList">操作者列表</param>
        /// <returns>ID字符串列表</returns>
        internal static string GenerateActivityAssignedUserNames(PerformerList performerList)
        {
            StringBuilder strBuilder = new StringBuilder(1024);
            foreach (var performer in performerList)
            {
                if (strBuilder.ToString() != "")
                    strBuilder.Append(",");
                strBuilder.Append(performer.UserName);
            }
            return strBuilder.ToString();
        }
    }
}
