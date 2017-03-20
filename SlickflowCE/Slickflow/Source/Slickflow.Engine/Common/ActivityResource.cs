using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Engine;

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

        public IDictionary<string, string> ConditionKeyValuePair
        {
            get;
            set;
        }

        public IDictionary<string, string> DynamicVariables
        {
            get;
            set;
        }

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

        internal static IDictionary<string, PerformerList> CreateNextActivityPerformers(string activityGUID,
            string userID,
            string userName)
        {
            var performerList = new PerformerList();
            performerList.Add(new Performer(userID, userName));
            IDictionary<string, PerformerList> nextActivityPerformers = new Dictionary<string, PerformerList>();
            nextActivityPerformers.Add(activityGUID, performerList);

            return nextActivityPerformers;
        }
        #endregion
    }
}
