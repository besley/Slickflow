using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
//using System.Diagnostics;
using Slickflow.Data;
using Slickflow.Module.Form;
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
    /// Process Model Interface
    /// </summary>
    public interface IProcessModel 
    {
        Process Process { get; }
        ProcessEntity ProcessEntity { get; set; }
        Entity.Activity GetStartActivity();
        Entity.Activity GetFirstActivity();
        NextActivityMatchedResult GetFirstActivityList(Entity.Activity startActivity, IDictionary<string, string> conditionKeyValuePair);
        Entity.Activity GetEndActivity();
        Entity.Activity GetActivity(string activityID);
        IList<Entity.Activity> GetActivityList();
        Entity.Activity GetNextActivity(string activityID);
        IList<NodeView> GetNextActivityTree(string currentActivityID);
        IList<NodeView> GetNextActivityTree(string currentActivityID, IDictionary<string, string> conditions);
        IList<NodeView> GetFirstActivityTree(Entity.Activity startActivity, IDictionary<string, string> conditions);
        NextActivityTreeResult GetNextActivityTree(string currentActivityID,
            Nullable<int> taskID,
            IDictionary<string, string> conditions,
            IDbSession session);
        NextActivityMatchedResult GetNextActivityList(string currentActivityID,
            Nullable<int> taskID,
            IDictionary<string, string> conditionKeyValuePair,
            ActivityResource activityResource,
            Expression<Func<ActivityResource, Entity.Activity, bool>> expression,
            IDbSession session);
        IList<Entity.Activity> GetNextActivityListWithoutCondition(string activityID);

        IList<NodeView> GetPreviousActivityTree(string currentActivityID);
        IList<Entity.Activity> GetPreviousActivityList(string currentActivityID);
        IList<Entity.Activity> GetPreviousActivityList(string currentActivityID, out bool hasGatewayPassed);
        IList<Entity.Activity> GetFromActivityList(string toActivityID);

        Entity.Activity GetBackwardGatewayActivity(Entity.Activity gatewayActivity, 
            ref int joinCount, ref int splitCount);
        Int32 GetBackwardTransitionListCount(string activityID);
        Transition GetForwardTransition(string fromActivityID, string toActivityID);
        IList<Transition> GetForwardTransitionList(string activityID);
        IList<Transition> GetForwardTransitionList(string activityID, IDictionary<string, string> conditionKeyValuePair);
        IList<Transition> GetBackwardTransitionList(string toActivityID);
        Boolean IsValidTransition(Transition transition, IDictionary<string, string> conditionValuePair);
        IList<Entity.Activity> GetTaskActivityList();
        IList<Entity.Activity> GetAllTaskActivityList();
        IList<Entity.Activity> GetAllTaskActivityList(Entity.Activity splitActivity, Entity.Activity joinActivity);

        int GetForcedBranchesCountBeforeEOrJoin(Entity.Activity gatewayActivity, out IList<Transition> forcedTransitionList);

        //Activity type
        Boolean IsMIParallel(Entity.Activity activity);
        Boolean IsMISequence(Entity.Activity activity);
        Boolean IsMINode(Entity.Activity activity);
        Boolean IsMINode(ActivityInstanceEntity activityInstance);
        Boolean IsTaskNode(Entity.Activity activity);
        Boolean IsTaskNode(ActivityInstanceEntity activityInstance);
        Boolean IsAndSplitMI(Entity.Activity activity);

        //Resource
        IList<Role> GetRoles();
        IList<Role> GetActivityRoles(string activityID);
        IDictionary<string, PerformerList> GetActivityPerformers(string activityID);
        IDictionary<string, PerformerList> GetActivityPerformers(IList<NodeView> nextActivityTree);
        IList<Form> GetFormList();

        //Notification
        IList<User> GetActivityNotifications(string activityID);
    }
}
