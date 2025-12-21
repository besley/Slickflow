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
    /// Performer Builder
    /// 执行者构造类
    /// </summary>
    internal class PerformerBuilder
    {
        /// <summary>
        /// Create a list of activity node performers
        /// 创建活动节点执行者列表
        /// </summary>
        internal static PerformerList CreatePerformerList(string userId, string userName)
        {
            var performerList = new PerformerList();
            performerList.Add(new Performer(userId, userName));

            return performerList;
        }


        /// <summary>
        /// Create a list of activity node performers
        /// 创建活动节点执行者列表
        /// </summary>
        /// <param name="roleList"></param>
        /// <returns></returns>
        internal static PerformerList CreatePerformerList(IList<Role> roleList)
        {
            PerformerList performerList = new PerformerList();
            if (roleList.Count() > 0)
            {
                var roleIDs = roleList.Select(x => x.Id).ToArray();
                var resourceService = ResourceServiceFactory.Create();
                var userList = resourceService.GetUserListByRoles(roleIDs);
                performerList = new PerformerList();

                foreach (var user in userList)
                {
                    var performer = new Performer(user.UserId.ToString(), user.UserName);

                    performerList.Add(performer);
                }
            }
            return performerList;
        }

        /// <summary>
        /// Generate a list of task performers Id strings
        /// 生成任务办理人ID字符串列表
        /// </summary>
        /// <param name="performerList"></param>
        /// <returns></returns>
        internal static string GenerateActivityAssignedUserIDs(PerformerList performerList)
        {
            StringBuilder strBuilder = new StringBuilder(1024);
            foreach (var performer in performerList)
            {
                if (strBuilder.ToString() != "")
                    strBuilder.Append(",");
                strBuilder.Append(performer.UserId);
            }
            return strBuilder.ToString();
        }

        /// <summary>
        /// Generate a list of task performer name strings
        /// 生成办理人名称的字符串列表
        /// </summary>
        /// <param name="performerList"></param>
        /// <returns></returns>
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
