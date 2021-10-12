using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Engine;
using Slickflow.Module.Resource;


namespace Slickflow.Engine.Common
{
    /// <summary>
    /// 活动上的资源类
    /// </summary>
    public class ActivityResource
    {
        #region 属性、构造函数
        /// <summary>
        /// 当前流程执行用户
        /// </summary>
        public WfAppRunner AppRunner
        {
            get;
            set;
        }

        /// <summary>
        /// 带有执行人员信息的下一步节点列表
        /// </summary>
        public IDictionary<string, PerformerList> NextActivityPerformers
        {
            get;
            set;
        }

        /// <summary>
        /// 条件Key-Value对
        /// </summary>
        public IDictionary<string, string> ConditionKeyValuePair
        {
            get;
            set;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="runner">运行者</param>
        /// <param name="nextActivityPerformers">下一步步骤人员列表</param>
        /// <param name="conditionKeyValuePair">条件参数</param>
        internal ActivityResource(WfAppRunner runner,
            IDictionary<string, PerformerList> nextActivityPerformers,
            IDictionary<string, string> conditionKeyValuePair = null)
        {
            AppRunner = runner;
            NextActivityPerformers = nextActivityPerformers;
            ConditionKeyValuePair = conditionKeyValuePair;
        }

        /// <summary>
        /// 获取特定步骤的办理人员列表
        /// </summary>
        /// <param name="previousActivityPerformers">上一步办理人员列表</param>
        /// <param name="activityGUID">节点GUID</param>
        /// <returns>办理人员列表</returns>
        internal PerformerList GetPreviousPerformerList(IList<KeyValuePairWrapper> previousActivityPerformers,
            string activityGUID)
        {
            PerformerList performerList = null;
            foreach (var kvp in previousActivityPerformers)
            {
                if (kvp.ActivityGUID == activityGUID)
                {
                    performerList = kvp.PerformerList;
                    break;
                }
            }
            return performerList;
        }
        #endregion

        #region 创建下一步活动执行者列表
        /// <summary>
        /// 创建下一步活动执行者列表
        /// </summary>
        /// <param name="activityGUID">活动节点GUID</param>
        /// <param name="userID">用户ID</param>
        /// <param name="userName">用户名称</param>
        /// <returns>步骤执行者列表</returns>
        internal static IDictionary<string, PerformerList> CreateNextActivityPerformers(string activityGUID,
            string userID,
            string userName)
        {
            var nextActivityPerformers = new Dictionary<string, PerformerList>();
            var performerList = PerformerBuilder.CreatePerformerList(userID, userName);
            nextActivityPerformers.Add(activityGUID, performerList);

            return nextActivityPerformers;
        }

        /// <summary>
        /// 创建下一步活动执行者列表
        /// </summary>
        /// <param name="nextActivityTree">活动节点列表</param>
        /// <param name="userID">用户ID</param>
        /// <param name="userName">用户名称</param>
        /// <returns>步骤执行者列表</returns>
        internal static IDictionary<string, PerformerList> CreateNextActivityPerformers(IList<NodeView> nextActivityTree,
            string userID,
            string userName)
        {
            var nextActivityPerformers = new Dictionary<string, PerformerList>();
            var performList = PerformerBuilder.CreatePerformerList(userID, userName);
            foreach (var node in nextActivityTree)
            {
                nextActivityPerformers.Add(node.ActivityGUID, performList);
            }
            return nextActivityPerformers;
        }

        /// <summary>
        /// 创建下一步活动执行者列表
        /// </summary>
        /// <param name="activityGUID">活动节点GUID</param>
        /// <param name="roleList">角色列表</param>
        /// <returns>步骤执行者列表</returns>
        internal static IDictionary<string, PerformerList> CreateNextActivityPerformers(string activityGUID,
            IList<Role> roleList)
        {
            var performerList = PerformerBuilder.CreatePerformerList(roleList);
            var nextActivityPerformers = new Dictionary<string, PerformerList>();

            nextActivityPerformers.Add(activityGUID, performerList);

            return nextActivityPerformers;
        }

        /// <summary>
        /// 创建下一步活动执行者列表
        /// </summary>
        /// <param name="nextActivityPerformers">下一步活动人员列表</param>
        /// <param name="activityGUID">活动节点GUID</param>
        /// <param name="roleList">角色列表</param>
        internal static void CreateNextActivityPerformers(IDictionary<string, PerformerList> nextActivityPerformers,
            string activityGUID,
            IList<Role> roleList)
        {
            var performerList = PerformerBuilder.CreatePerformerList(roleList);
            nextActivityPerformers.Add(activityGUID, performerList);
        }

        /// <summary>
        /// 创建下一步活动执行者列表
        /// </summary>
        /// <param name="activityGUID">活动节点GUID</param>
        /// <param name="performerList">执行者列表</param>
        /// <returns>步骤执行者列表</returns>
        internal static IDictionary<string, PerformerList> CreateNextActivityPerformers(string activityGUID, 
            PerformerList performerList)
        {
            var nextActivityPerformers = new Dictionary<string, PerformerList>();
            nextActivityPerformers.Add(activityGUID, performerList);

            return nextActivityPerformers;
        }
        #endregion
    }
}
