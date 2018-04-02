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
        /// 动态变量
        /// </summary>
        public IDictionary<string, string> DynamicVariables
        {
            get;
            set;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="runner"></param>
        /// <param name="nextActivityPerformers"></param>
        /// <param name="conditionKeyValuePair"></param>
        /// <param name="dynamicVariables"></param>
        internal ActivityResource(WfAppRunner runner,
            IDictionary<string, PerformerList> nextActivityPerformers,
            IDictionary<string, string> conditionKeyValuePair = null,
            IDictionary<string, string> dynamicVariables = null)
        {
            AppRunner = runner;
            NextActivityPerformers = nextActivityPerformers;
            ConditionKeyValuePair = conditionKeyValuePair;
            DynamicVariables = dynamicVariables;
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
            var performerList = PerformerBuilder.CreatePerformerList(userID, userName);
            var nextActivityPerformers = new Dictionary<string, PerformerList>();

            nextActivityPerformers.Add(activityGUID, performerList);

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
