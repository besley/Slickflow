using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Diagnostics;
using Slickflow.Data;
using Slickflow.Module.Resource;
using Slickflow.Engine.Utility;
using Slickflow.Engine.Common;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Engine.Xpdl.Schedule;

namespace Slickflow.Engine.Xpdl
{
    /// <summary>
    /// 流程模型解析
    /// </summary>
    public interface IProcessModel 
    {
        ProcessEntity ProcessEntity { get; set; }
        XmlDocument XmlProcessDefinition { get; }

        //方法列表
        ActivityEntity GetStartActivity();
        ActivityEntity GetFirstActivity();
        NextActivityMatchedResult GetFirstActivityList(ActivityEntity startActivity, IDictionary<string, string> conditionKeyValuePair);
        ActivityEntity GetActivityByType(string xmlContent, string processGUID,  ActivityTypeEnum activityType);
        ActivityEntity GetEndActivity();
        ActivityEntity GetActivity(string activityGUID);
        IList<ActivityEntity> GetActivityList();
        ActivityEntity GetNextActivity(string activityGUID);
        IList<NodeView> GetNextActivityTree(string currentActivityGUID);
        IList<NodeView> GetNextActivityTree(string currentActivityGUID, IDictionary<string, string> conditions);
        NextActivityTreeResult GetNextActivityTree(string currentActivityGUID,
            Nullable<int> taskID,
            IDictionary<string, string> conditions,
            IDbSession session);
        NextActivityMatchedResult GetNextActivityList(string currentActivityGUID,
            Nullable<int> taskID,
            IDictionary<string, string> conditionKeyValuePair,
            ActivityResource activityResource,
            Expression<Func<ActivityResource, ActivityEntity, bool>> expression,
            IDbSession session);
        IList<ActivityEntity> GetNextActivityListWithoutCondition(string activityGUID);

        IList<NodeView> GetPreviousActivityTree(string currentActivityGUID);
        IList<ActivityEntity> GetPreviousActivityList(string currentActivityGUID);
        IList<ActivityEntity> GetPreviousActivityList(string toActivityGUID, out bool hasGatewayPassed);
        IList<ActivityEntity> GetFromActivityList(string toActivityGUID);

        ActivityEntity GetBackwardGatewayActivity(ActivityEntity gatewayActivity, 
            ref int joinCount, ref int splitCount);
        Int32 GetBackwardTransitionListCount(string activityGUID);
        TransitionEntity GetForwardTransition(string fromActivityGUID, string toActivityGUID);
        IList<TransitionEntity> GetForwardTransitionList(string activityGUID);
        IList<TransitionEntity> GetForwardTransitionList(string activityGUID, IDictionary<string, string> conditionKeyValuePair);
        IList<TransitionEntity> GetBackwardTransitionList(string toActivityGUID);
        Boolean IsValidTransition(TransitionEntity transition, IDictionary<string, string> conditionValuePair);
        IList<ActivityEntity> GetTaskActivityList();
        IList<ActivityEntity> GetAllTaskActivityList();
        IList<ActivityEntity> GetAllTaskActivityList(ActivityEntity splitActivity, ActivityEntity joinActivity);

        int GetForcedBranchesCountBeforeEOrJoin(ActivityEntity gatewayActivity, out IList<TransitionEntity> forcedTransitionList);

        //节点类型判断
        Boolean IsMIParallel(ActivityEntity activity);
        Boolean IsMISequence(ActivityEntity activity);
        Boolean IsMINode(ActivityEntity activity);
        Boolean IsTaskNode(ActivityEntity activity);
        Boolean IsTaskNode(ActivityInstanceEntity activityInstance);
        Boolean IsAndSplitMI(ActivityEntity activity);

        //资源
        IList<Role> GetRoles();
        IList<Role> GetActivityRoles(string activityGUID);
        IDictionary<string, PerformerList> GetActivityPerformers(string activityGUID);
        IDictionary<string, PerformerList> GetActivityPerformers(IList<NodeView> nextActivityTree);
    }
}
