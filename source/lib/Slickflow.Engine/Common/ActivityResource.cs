using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Engine;
using Slickflow.Module.Resource;


namespace Slickflow.Engine.Common
{
    /// <summary>
    /// Resources related to activity
    /// 活动上的资源类
    /// </summary>
    public class ActivityResource
    {
        #region Properties, Constructors 属性、构造函数
        /// <summary>
        /// Current process execution user
        /// 当前流程执行用户
        /// </summary>
        public WfAppRunner AppRunner
        {
            get;
            set;
        }

        /// <summary>
        /// Next activity list with execution personnel information
        /// 带有执行人员信息的下一步节点列表
        /// </summary>
        public IDictionary<string, PerformerList> NextActivityPerformers
        {
            get;
            set;
        }

        /// <summary>
        /// Key value pair
        /// 条件Key-Value对
        /// </summary>
        public IDictionary<string, string> ConditionKeyValuePair
        {
            get;
            set;
        }

        /// <summary>
        /// Construct function
        /// 构造函数
        /// </summary>
        /// <param name="runner"></param>
        /// <param name="nextActivityPerformers"></param>
        /// <param name="conditionKeyValuePair"></param>
        internal ActivityResource(WfAppRunner runner,
            IDictionary<string, PerformerList> nextActivityPerformers,
            IDictionary<string, string> conditionKeyValuePair = null)
        {
            AppRunner = runner;
            NextActivityPerformers = nextActivityPerformers;
            ConditionKeyValuePair = conditionKeyValuePair;
        }

        /// <summary>
        /// Obtain the list of performers handling the previous step
        /// 获取上一步骤的办理人员列表
        /// </summary>
        internal PerformerList GetPreviousPerformerList(IList<KeyValuePairWrapper> previousActivityPerformers,
            string activityID)
        {
            PerformerList performerList = null;
            foreach (var kvp in previousActivityPerformers)
            {
                if (kvp.ActivityID == activityID)
                {
                    performerList = kvp.PerformerList;
                    break;
                }
            }
            return performerList;
        }
        #endregion

        #region Create a list of next activity performers 创建下一步活动执行者列表
        /// <summary>、
        /// Create a list of next activity performers
        /// 创建下一步活动执行者列表
        /// </summary>
        internal static IDictionary<string, PerformerList> CreateNextActivityPerformers(string activityID,
            string userID,
            string userName)
        {
            var nextActivityPerformers = new Dictionary<string, PerformerList>();
            var performerList = PerformerBuilder.CreatePerformerList(userID, userName);
            nextActivityPerformers.Add(activityID, performerList);

            return nextActivityPerformers;
        }

        /// <summary>
        /// Create a list of next activity performers
        /// 创建下一步活动执行者列表
        /// </summary>
        /// <param name="nextActivityTree"></param>
        /// <param name="userID"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        internal static IDictionary<string, PerformerList> CreateNextActivityPerformers(IList<NodeView> nextActivityTree,
            string userID,
            string userName)
        {
            var nextActivityPerformers = new Dictionary<string, PerformerList>();
            var performList = PerformerBuilder.CreatePerformerList(userID, userName);
            foreach (var node in nextActivityTree)
            {
                nextActivityPerformers.Add(node.ActivityID, performList);
            }
            return nextActivityPerformers;
        }

        /// <summary>
        /// Create a list of next activity performers
        /// 创建下一步活动执行者列表
        /// </summary>
        internal static IDictionary<string, PerformerList> CreateNextActivityPerformers(string activityID,
            IList<Role> roleList)
        {
            var performerList = PerformerBuilder.CreatePerformerList(roleList);
            var nextActivityPerformers = new Dictionary<string, PerformerList>();

            nextActivityPerformers.Add(activityID, performerList);

            return nextActivityPerformers;
        }

        /// <summary>
        /// Create a list of next activity performers
        /// 创建下一步活动执行者列表
        /// </summary>
        internal static void CreateNextActivityPerformers(IDictionary<string, PerformerList> nextActivityPerformers,
            string activityID,
            IList<Role> roleList)
        {
            var performerList = PerformerBuilder.CreatePerformerList(roleList);
            nextActivityPerformers.Add(activityID, performerList);
        }

        /// <summary>
        /// Create a list of next activity performers
        /// 创建下一步活动执行者列表
        /// </summary>
        internal static IDictionary<string, PerformerList> CreateNextActivityPerformers(string activityID, 
            PerformerList performerList)
        {
            var nextActivityPerformers = new Dictionary<string, PerformerList>();
            nextActivityPerformers.Add(activityID, performerList);

            return nextActivityPerformers;
        }
        #endregion
    }
}
