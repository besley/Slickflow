using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
//using System.Diagnostics;
using Slickflow.Data;
using Slickflow.Module.Resource;
using Slickflow.Engine.Utility;
using Slickflow.Engine.Common;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Engine.Xpdl.Node;
using Slickflow.Engine.Xpdl.Schedule;

namespace Slickflow.Engine.Xpdl
{
    /// <summary>
    /// 流程模型解析
    /// </summary>
    public interface IProcessModel 
    {
        Process Process { get; }

        ProcessEntity ProcessEntity { get; set; }
        public string SubProcessGUID { get; set; }

        //方法列表
        Entity.Activity GetStartActivity();
        Entity.Activity GetFirstActivity();
        NextActivityMatchedResult GetFirstActivityList(Entity.Activity startActivity, IDictionary<string, string> conditionKeyValuePair);
        Entity.Activity GetEndActivity();
        Entity.Activity GetActivity(string activityGUID);
        IList<Entity.Activity> GetActivityList();
        Entity.Activity GetNextActivity(string activityGUID);
        IList<NodeView> GetNextActivityTree(string currentActivityGUID);
        IList<NodeView> GetNextActivityTree(string currentActivityGUID, IDictionary<string, string> conditions);
        IList<NodeView> GetFirstActivityTree(Entity.Activity startActivity, IDictionary<string, string> conditions);
        NextActivityTreeResult GetNextActivityTree(string currentActivityGUID,
            Nullable<int> taskID,
            IDictionary<string, string> conditions,
            IDbSession session);
        NextActivityMatchedResult GetNextActivityList(string currentActivityGUID,
            Nullable<int> taskID,
            IDictionary<string, string> conditionKeyValuePair,
            ActivityResource activityResource,
            Expression<Func<ActivityResource, Entity.Activity, bool>> expression,
            IDbSession session);
        IList<Entity.Activity> GetNextActivityListWithoutCondition(string activityGUID);

        IList<NodeView> GetPreviousActivityTree(string currentActivityGUID);
        IList<Entity.Activity> GetPreviousActivityList(string currentActivityGUID);
        IList<Entity.Activity> GetPreviousActivityList(string currentActivityGUID, out bool hasGatewayPassed);
        IList<Entity.Activity> GetFromActivityList(string toActivityGUID);

        Entity.Activity GetBackwardGatewayActivity(Entity.Activity gatewayActivity, 
            ref int joinCount, ref int splitCount);
        Int32 GetBackwardTransitionListCount(string activityGUID);
        Transition GetForwardTransition(string fromActivityGUID, string toActivityGUID);
        IList<Transition> GetForwardTransitionList(string activityGUID);
        IList<Transition> GetForwardTransitionList(string activityGUID, IDictionary<string, string> conditionKeyValuePair);
        IList<Transition> GetBackwardTransitionList(string toActivityGUID);
        Boolean IsValidTransition(Transition transition, IDictionary<string, string> conditionValuePair);
        IList<Entity.Activity> GetTaskActivityList();
        IList<Entity.Activity> GetAllTaskActivityList();
        IList<Entity.Activity> GetAllTaskActivityList(Entity.Activity splitActivity, Entity.Activity joinActivity);

        int GetForcedBranchesCountBeforeEOrJoin(Entity.Activity gatewayActivity, out IList<Transition> forcedTransitionList);

        //节点类型判断
        Boolean IsMIParallel(Entity.Activity activity);
        Boolean IsMISequence(Entity.Activity activity);
        Boolean IsMINode(Entity.Activity activity);
        Boolean IsTaskNode(Entity.Activity activity);
        Boolean IsTaskNode(ActivityInstanceEntity activityInstance);
        Boolean IsAndSplitMI(Entity.Activity activity);

        //资源
        IList<Role> GetRoles();
        IList<Role> GetActivityRoles(string activityGUID);
        IDictionary<string, PerformerList> GetActivityPerformers(string activityGUID);
        IDictionary<string, PerformerList> GetActivityPerformers(IList<NodeView> nextActivityTree);
    }
}
