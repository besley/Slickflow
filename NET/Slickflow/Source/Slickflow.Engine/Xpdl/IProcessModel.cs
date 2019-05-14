using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Diagnostics;
using Slickflow.Module.Resource;
using Slickflow.Engine.Utility;
using Slickflow.Engine.Common;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Xpdl.Schedule;

namespace Slickflow.Engine.Xpdl
{
    /// <summary>
    /// 流程模型解析
    /// </summary>
    internal interface IProcessModel
    {
        ProcessEntity ProcessEntity { get; set; }
        XmlDocument XmlProcessDefinition { get; }
        ActivityEntity GetFirstActivity();
        ActivityEntity GetStartActivity();
        ActivityEntity GetEndActivity();
        ActivityEntity GetActivity(string activityGUID);
        ActivityEntity GetNextActivity(string activityGUID);
        IList<NodeView> GetNextActivityTree(string currentActivityGUID,
            IDictionary<string, string> condition = null);
        IList<NodeView> GetPreviousActivityTree(string currentActivityGUID);
        IList<ActivityEntity> GetPreviousActivityList(string currentActivityGUID);
        IList<ActivityEntity> GetPreviousActivityList(string toActivityGUID, out bool hasGatewayPassed);

        ActivityEntity GetBackwardGatewayActivity(ActivityEntity gatewayActivity, 
            ref int joinCount, ref int splitCount);
        Int32 GetBackwardTransitionListCount(string activityGUID);
        IList<TransitionEntity> GetForwardTransitionList(string activityGUID);
		IList<TransitionEntity> GetForwardTransitionList(string activityGUID, IDictionary<string, string> conditionKeyValuePair);
        Boolean CheckAndSplitOccurrenceCondition(IList<TransitionEntity> transitionList, 
            IDictionary<string, string> conditionValuePair);
        Boolean IsValidTransition(TransitionEntity transition, IDictionary<string, string> conditionValuePair);
        NextActivityMatchedResult GetNextActivityList(string currentActivityGUID, 
            IDictionary<string, string> conditionKeyValuePair = null);
        NextActivityMatchedResult GetNextActivityList(string currentActivityGUID,
            IDictionary<string, string> conditionKeyValuePair,
            ActivityResource activityResource,
            Expression<Func<ActivityResource, ActivityEntity, bool>> expression);

        //资源
        IList<Role> GetRoles();
        IList<Role> GetActivityRoles(string activityGUID);
        IDictionary<string, PerformerList> GetActivityPerformers(string activityGUID);
    }
}
