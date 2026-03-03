using Slickflow.Engine.Common;
using Slickflow.Engine.Core.Result;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Engine.Xpdl.Schedule;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Slickflow.Engine.Executor
{
    /// <summary>
    /// Runs the workflow graph: traverses activities, handles gateways, delegates single-activity execution.
    /// 工作流图执行器：遍历活动节点、处理网关、委托单节点执行。
    /// </summary>
    public class WorkflowNodeRunner
    {
        private readonly IProcessModel _processModel;
        private readonly IWorkflowActivityExecutor _activityExecutor;

        public WorkflowNodeRunner(IProcessModel processModel, IWorkflowActivityExecutor activityExecutor)
        {
            _processModel = processModel ?? throw new ArgumentNullException(nameof(processModel));
            _activityExecutor = activityExecutor ?? throw new ArgumentNullException(nameof(activityExecutor));
        }

        /// <summary>
        /// Run from the given start activity through the graph.
        /// 从给定开始节点沿图执行
        /// </summary>
        public void Run(Activity startActivity, AutoExecutionContext context, WfExecutedResult result)
        {
            RunThroughlyCurrentNode(startActivity, context, result);
        }

        /// <summary>
        /// Recursively execute activities
        /// 递归执行活动节点
        /// </summary>
        public void RunThroughlyCurrentNode(Activity currentActivity,
            AutoExecutionContext context,
            WfExecutedResult result)
        {
            if (result.Status != WfExecutedStatus.Success)
                return;

            int incomingCount = _processModel.GetBackwardTransitionListCount(currentActivity.ActivityId);
            if (incomingCount > 1)
            {
                if (!context.JoinReachedCounts.TryGetValue(currentActivity.ActivityId, out int reached))
                    reached = 0;
                context.JoinReachedCounts[currentActivity.ActivityId] = reached + 1;
                if (reached + 1 < incomingCount)
                    return;
            }

            if (context.ExecutedActivityIds.Contains(currentActivity.ActivityId))
                return;

            context.ExecutedActivityIds.Add(currentActivity.ActivityId);

            if (currentActivity.ActivityType == ActivityTypeEnum.EndNode)
            {
                _activityExecutor.ExecuteEndActivity(currentActivity, context);
                return;
            }

            _activityExecutor.ExecuteActivity(currentActivity, context, result);

            if (result.Status != WfExecutedStatus.Success)
                return;

            var nextMatchedResult = GetNextActivityTree(currentActivity, context);

            if (nextMatchedResult == null || nextMatchedResult.MatchedType != NextActivityMatchedType.Successed
                || nextMatchedResult.Root == null || !nextMatchedResult.Root.HasChildren)
            {
                if (currentActivity.ActivityType != ActivityTypeEnum.EndNode)
                {
                    result.Status = WfExecutedStatus.Failed;
                    result.Message = $"No next activities found for activity: {currentActivity.ActivityId}";
                }
                return;
            }

            ContinueRunForwardRecursively(currentActivity, nextMatchedResult.Root, context, result);
        }

        /// <summary>
        /// Traverse next activity tree recursively
        /// 按树递归遍历下一节点
        /// </summary>
        public void ContinueRunForwardRecursively(Activity fromActivity,
            NextActivityComponent root,
            AutoExecutionContext context,
            WfExecutedResult result)
        {
            if (result.Status != WfExecutedStatus.Success)
                return;

            var nextActivities = GetNextActivitiesFromRoot(root);
            if (nextActivities == null || nextActivities.Count == 0)
                return;

            if (IsGatewayNode(fromActivity) && fromActivity.GatewayDetail != null)
            {
                HandleGatewayNode(fromActivity, nextActivities, context, result);
            }
            else
            {
                foreach (var nextActivity in nextActivities)
                {
                    RunThroughlyCurrentNode(nextActivity, context, result);
                    if (result.Status != WfExecutedStatus.Success)
                        break;
                }
            }
        }

        /// <summary>
        /// Get next activity tree (same API as Engine WorkflowService / NodeMediator).
        /// </summary>
        public NextActivityMatchedResult GetNextActivityTree(Activity currentActivity, AutoExecutionContext context)
        {
            var activityResource = new ActivityResource(context.Runner, null);
            return _processModel.GetNextActivityTreeListRuntime(
                currentActivity.ActivityId,
                0,
                context.Variables,
                activityResource,
                (a, b) => true,
                null);
        }

        /// <summary>
        /// Collect immediate children activities from root
        /// </summary>
        public IList<Activity> GetNextActivitiesFromRoot(NextActivityComponent root)
        {
            var activities = new List<Activity>();
            if (root == null)
                return activities;
            if (!root.HasChildren)
            {
                if (root.Activity != null)
                    activities.Add(root.Activity);
                return activities;
            }
            foreach (NextActivityComponent comp in root)
            {
                if (comp?.Activity != null)
                    activities.Add(comp.Activity);
            }
            return activities;
        }

        /// <summary>
        /// Handle gateway node by DirectionType
        /// </summary>
        public void HandleGatewayNode(Activity gatewayNode,
            IList<Activity> nextActivities,
            AutoExecutionContext context,
            WfExecutedResult result)
        {
            if (gatewayNode == null || gatewayNode.GatewayDetail == null)
            {
                result.Status = WfExecutedStatus.Failed;
                result.Message = $"Invalid gateway node: {gatewayNode?.ActivityId}";
                return;
            }

            var directionType = gatewayNode.GatewayDetail.DirectionType;

            switch (directionType)
            {
                case GatewayDirectionEnum.AndSplit:
                case GatewayDirectionEnum.AndJoin:
                case GatewayDirectionEnum.AndSplitMI:
                case GatewayDirectionEnum.AndJoinMI:
                    HandleAndGatewayNode(nextActivities, context, result);
                    break;

                case GatewayDirectionEnum.XOrSplit:
                case GatewayDirectionEnum.XOrJoin:
                    HandleXOrGatewayNode(gatewayNode, nextActivities, context, result);
                    break;

                case GatewayDirectionEnum.OrSplit:
                case GatewayDirectionEnum.OrJoin:
                case GatewayDirectionEnum.ApprovalOrSplit:
                case GatewayDirectionEnum.EOrJoin:
                    HandleOrGatewayNode(gatewayNode, nextActivities, context, result);
                    break;

                case GatewayDirectionEnum.None:
                default:
                    foreach (var nextActivity in nextActivities)
                    {
                        RunThroughlyCurrentNode(nextActivity, context, result);
                        if (result.Status != WfExecutedStatus.Success)
                            break;
                    }
                    break;
            }
        }

        public void HandleAndGatewayNode(IList<Activity> nextActivities,
            AutoExecutionContext context,
            WfExecutedResult result)
        {
            foreach (var nextActivity in nextActivities)
            {
                RunThroughlyCurrentNode(nextActivity, context, result);
                if (result.Status != WfExecutedStatus.Success)
                    break;
            }
        }

        public void HandleXOrGatewayNode(Activity gatewayNode,
            IList<Activity> nextActivities,
            AutoExecutionContext context,
            WfExecutedResult result)
        {
            Activity matchedActivity = null;

            foreach (var nextActivity in nextActivities)
            {
                var transition = _processModel.GetForwardTransition(gatewayNode.ActivityId, nextActivity.ActivityId);
                if (transition != null)
                {
                    if (EvaluateCondition(transition, context.Variables))
                    {
                        matchedActivity = nextActivity;
                        break;
                    }
                }
                else
                {
                    if (matchedActivity == null)
                        matchedActivity = nextActivity;
                }
            }

            if (matchedActivity != null)
            {
                RunThroughlyCurrentNode(matchedActivity, context, result);
            }
            else
            {
                result.Status = WfExecutedStatus.Failed;
                result.Message = $"No matching branch found for XOR gateway: {gatewayNode.ActivityId}";
            }
        }

        public void HandleOrGatewayNode(Activity gatewayNode,
            IList<Activity> nextActivities,
            AutoExecutionContext context,
            WfExecutedResult result)
        {
            bool hasMatch = false;

            foreach (var nextActivity in nextActivities)
            {
                var transition = _processModel.GetForwardTransition(gatewayNode.ActivityId, nextActivity.ActivityId);
                if (transition != null)
                {
                    if (EvaluateCondition(transition, context.Variables))
                    {
                        hasMatch = true;
                        RunThroughlyCurrentNode(nextActivity, context, result);
                        if (result.Status != WfExecutedStatus.Success)
                            break;
                    }
                }
                else
                {
                    hasMatch = true;
                    RunThroughlyCurrentNode(nextActivity, context, result);
                    if (result.Status != WfExecutedStatus.Success)
                        break;
                }
            }

            if (!hasMatch)
            {
                result.Status = WfExecutedStatus.Failed;
                result.Message = $"No matching branch found for OR gateway: {gatewayNode.ActivityId}";
            }
        }

        public bool EvaluateCondition(Transition transition, IDictionary<string, string> variables)
        {
            if (transition == null || transition.Condition == null)
                return true;

            var condition = transition.Condition.ConditionText;
            if (string.IsNullOrEmpty(condition))
                return true;

            try
            {
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsGatewayNode(Activity activity)
        {
            return activity.ActivityType == ActivityTypeEnum.GatewayNode;
        }
    }
}
