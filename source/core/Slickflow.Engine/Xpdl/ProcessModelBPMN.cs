using MySqlX.XDevAPI;
using Slickflow.Data;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Common;
using Slickflow.Engine.Config;
using Slickflow.Engine.Utility;
using Slickflow.Engine.Xpdl.Common;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Engine.Xpdl.Node;
using Slickflow.Engine.Xpdl.Schedule;
using Slickflow.Module.Form;
using Slickflow.Module.Localize;
using Slickflow.Module.Resource;
using Slickflow.WebUtility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Xml;

namespace Slickflow.Engine.Xpdl
{
    /// <summary>
    /// BPMN Process Model
    /// </summary>
    public class ProcessModelBPMN : IProcessModel
    {
        #region Property and Constructor
        /// <summary>
        /// Process 
        /// </summary>
        public Process Process
        {
            get
            {
                var processId = ProcessEntity.ProcessId;
                var version = ProcessEntity.Version;

                if (WfConfig.EXPIRED_DAYS_ENABLED == true)
                {
                    //Get Process content from cache
                    if (XPDLMemoryCachedHelper.GetXpdlCache(processId, version) == null)
                    {
                        var process = ConvertProcessModelFromXML(ProcessEntity);
                        XPDLMemoryCachedHelper.SetXpdlCache(processId, version, process);
                    }
                    return XPDLMemoryCachedHelper.GetXpdlCache(processId, version);
                }
                else
                {
                    //Get Process content from database
                    var processDB = ConvertProcessModelFromXML(ProcessEntity);
                    return processDB;
                }
            }
        }
        /// <summary>
        /// Process Entity
        /// </summary>
        public ProcessEntity ProcessEntity { get; set; }
       
        /// <summary>
        /// Convert process model from xml
        /// </summary>
        /// <returns></returns>
        private Process ConvertProcessModelFromXML(ProcessEntity processEntity)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(this.ProcessEntity.XmlContent);
            var root = xmlDoc.DocumentElement;

            //按照是否有子流程来构建流程模型
            //Build a process model based on whether there are sub processes
            XmlNode xmlNodeProcess = null;
            if (root.Name == XPDLDefinition.BPMN_ElementName_Definitions)
            {
                if (processEntity.PackageType == null)
                {
                    //单一流程
                    //Simple Process
                    xmlNodeProcess = root.SelectSingleNode(XPDLDefinition.BPMN_StrXmlPath_Process, 
                        XPDLHelper.GetSlickflowXmlNamespaceManager(xmlDoc));
                }
                else
                {
                    //泳道流程
                    //Pool Process
                    var xPath = string.Format("{0}[@id='{1}']", XPDLDefinition.BPMN_StrXmlPath_Process, processEntity.ProcessId);
                    xmlNodeProcess = root.SelectSingleNode(xPath, XPDLHelper.GetSlickflowXmlNamespaceManager(xmlDoc));
                }
            }
            else if (root.Name == XPDLDefinition.BPMN_ElementName_SubProcess)
            {
                //子流程
                //Sub Process
                xmlNodeProcess = root;
            }
            else
            {
                throw new NotImplementedException(
                    string.Format("NOT supported process xml content, element type:{0}", root.Name)
                    );
            }
            Process process = ProcessModelConvertor.ConvertProcessModelFromXML(xmlNodeProcess);
            return process;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="entity"></param>
        public ProcessModelBPMN(ProcessEntity entity)
        {
            ProcessEntity = entity;
        }
        #endregion

        /// <summary>
        /// Get Activity
        /// </summary>
        public Activity GetActivity(string activityId)
        {
            var activity = ProcessModelHelper.GetActivity(this.Process, activityId);
            return activity;
        }

        /// <summary>
        /// Get Activity List
        /// </summary>
        /// <returns></returns>
        public IList<Activity> GetActivityList()
        {
            List<Activity> activityList = new List<Activity>();
            var startNode = GetStartActivity();
            var startActivityId = startNode.ActivityId;

            activityList.Add(startNode);

            return TranverseTransitionList(activityList, startActivityId);
        }

        /// <summary>
        /// Get Activity Output Variable Name List
        /// </summary>
        /// <returns></returns>
        public IList<string> GetActivityOutputVarialbeNameList(Entity.Activity activity)
        {
            if (activity?.VariableList == null)
            {
                return new List<string>();
            }

            return activity.VariableList
                .Where(v => v.DirectionType == VariableDirectionTypeEnum.Output)
                .Select(v => v.Name)
                .ToList();
        }

        /// <summary>
        /// Recursive traversal of predecessor nodes on transfer
        /// 递归遍历转移上的前置节点
        /// </summary>
        private IList<Activity> TranverseTransitionList(List<Activity> activityList, string fromActivityId)
        {
            Activity toActivity = null;
            var transitionList = GetForwardTransitionList(fromActivityId);
            foreach (var transition in transitionList)
            {
                toActivity = transition.ToActivity;
                if (toActivity != null)
                {
                    AppendActivity(activityList, toActivity);
                }
                else
                {
                    break;
                }
                TranverseTransitionList(activityList, toActivity.ActivityId);
            }
            return activityList;
        }

        /// <summary>
        /// Obtain the nodes of task types (including countersignature nodes and subprocess nodes) 
        /// and form a sequence in Transition order
        /// 获取任务类型的节点(包含会签节点和子流程节点)，按照Transition顺序组成序列
        /// </summary>
        /// <returns></returns>
        public IList<Activity> GetAllTaskActivityList()
        {
            List<Activity> activityList = new List<Activity>();
            var startNode = GetStartActivity();
            var startActivityId = startNode.ActivityId;

            return TranverseTaskTransitionList(activityList, startActivityId);
        }

        /// <summary>
        /// Recursive traversal of predecessor nodes on transfer
        /// 递归遍历转移上的前置节点
        /// </summary>
        private IList<Activity> TranverseTaskTransitionList(List<Activity> activityList, string fromActivityId)
        {
            Activity toActivity = null;
            var transitionList = GetForwardTransitionList(fromActivityId);
            foreach (var transition in transitionList)
            {
                toActivity = transition.ToActivity;
                if (toActivity.ActivityType == ActivityTypeEnum.EndNode)
                {
                    break;
                }
                else if (toActivity.ActivityType == ActivityTypeEnum.TaskNode
                    || toActivity.ActivityType == ActivityTypeEnum.MultiSignNode
                    || toActivity.ActivityType == ActivityTypeEnum.SubProcessNode)
                {
                    AppendActivity(activityList, toActivity);
                }
                //递归遍历转移数据
                //Recursive traversal to transition data
                TranverseTaskTransitionList(activityList, toActivity.ActivityId);
            }
            return activityList;
        }

        /// <summary>
        /// Get Task activity list
        /// </summary>
        public IList<Activity> GetTaskActivityList()
        {
            List<Activity> taskList = new List<Activity>();
            var activityList = this.Process.ActivityList;
            foreach (var activity in activityList)
            {
                if (activity.ActivityType == ActivityTypeEnum.TaskNode)
                {
                    AppendActivity(taskList, activity);
                }
            }
            return taskList;
        }

        /// <summary>
        /// Get a list of task nodes between branches and merges
        /// 获取分支和合并之间的任务节点列表
        /// </summary>
        public IList<Activity> GetAllTaskActivityList(Activity splitActivity,
            Activity joinActivity)
        {
            IList<Activity> taskActivityList = new List<Activity>();
            return TranverseTransitionListBetweenSplitJoin(taskActivityList, splitActivity.ActivityId, joinActivity.ActivityId);
        }

        /// <summary>
        /// Recursive traversal of predecessor nodes on transfer
        /// 递归遍历转移上的前置节点
        /// </summary>
        private IList<Activity> TranverseTransitionListBetweenSplitJoin(IList<Activity> activityList,
            string fromActivityId,
            string finalActivityId)
        {
            Activity toActivity = null;
            var transitionNodeList = GetForwardTransitionList(fromActivityId);
            foreach (var transition in transitionNodeList)
            {
                toActivity = transition.ToActivity;
                if (toActivity.ActivityType == ActivityTypeEnum.GatewayNode
                    && toActivity.ActivityId == finalActivityId)
                {
                    break;
                }
                else if (toActivity.ActivityType == ActivityTypeEnum.TaskNode
                    || toActivity.ActivityType == ActivityTypeEnum.MultiSignNode
                    || toActivity.ActivityType == ActivityTypeEnum.SubProcessNode)
                {
                    AppendActivity(activityList, toActivity);
                }
                //递归遍历转移数据
                //Recursive traversal to transfer data
                TranverseTransitionListBetweenSplitJoin(activityList, toActivity.ActivityId, finalActivityId);
            }
            return activityList;
        }

        /// <summary>
        /// Add nodes to the node list and remove duplicate nodes
        /// 添加节点到节点列表中，去掉有重复的节点
        /// </summary>
        private void AppendActivity(IList<Activity> activityList, Activity toActivity)
        {
            var isExist = false;
            foreach (var a in activityList)
            {
                if (a.ActivityId == toActivity.ActivityId)
                {
                    isExist = true;
                    break;
                }
            }

            if (isExist == false) activityList.Add(toActivity);
        }

        /// <summary>
        /// Get Start Activity
        /// </summary>
        public Activity GetStartActivity()
        {
            var startNode = ProcessModelHelper.GetStartActivity(this.Process);
            if (startNode == null)
            {
                throw new ApplicationException(LocalizeHelper.GetEngineMessage("processmodel.getstartactivity.error"));
            }
            return startNode;
        }

        /// <summary>
        /// Get First Activity
        /// </summary>
        public Activity GetFirstActivity()
        {
            var activity = ProcessModelHelper.GetFirstActivity(this.Process);
            return activity;
        }

        /// <summary>
        /// Get the list of activity nodes at the beginning of the process
        /// (there may be multiple nodes after the start node)
        /// 获取流程起始的活动节点列表(开始节点之后，可能有多个节点)
        /// </summary>
        public NextActivityMatchedResult GetFirstActivityList(Activity startActivity, IDictionary<string, string> conditionKeyValuePair)
        {
            try
            {
                using (var session = SessionFactory.CreateSession())
                {
                    var processModelBPMNCore = new ProcessModelBPMNCore();
                    return processModelBPMNCore.GetNextActivityTreeListCore(this, startActivity.ActivityId, null, conditionKeyValuePair, session);
                }
            }
            catch (System.Exception e)
            {
                throw new WfXpdlException(LocalizeHelper.GetEngineMessage("processmodel.getfirstactivitylist.error"),
                    e);
            }
        }

        /// <summary>
        /// Get End Activity
        /// </summary>
        public Activity GetEndActivity()
        {
            var activity = ProcessModelHelper.GetEndActivity(this.Process);
            return activity;
        }

        /// <summary>
        /// Obtain the mandatory number of branches for properties on the pre branch connection of the merged node
        /// 获取合并节点的前置分支连线上的属性的强制分支数目
        /// </summary>
        public int GetForcedBranchesCountBeforeEOrJoin(Activity gatewayActivity, out IList<Transition> forcedTransitionList)
        {
            var tokensCount = 0;
            forcedTransitionList = new List<Transition>();
            var transitionList = GetBackwardTransitionList(gatewayActivity.ActivityId);
            foreach (var transition in transitionList)
            {
                if (transition.GroupBehaviours != null && transition.GroupBehaviours.Forced)
                {
                    tokensCount += 1;
                    forcedTransitionList.Add(transition);
                }
            }
            return tokensCount;
        }

        /// <summary>
        /// Get Forward Transition
        /// </summary>
        public Transition GetForwardTransition(string fromActivityId, string toActivityId)
        {
            var transition = ProcessModelHelper.GetForwardTransition(this.Process, fromActivityId, toActivityId);
            return transition;
        }

        /// <summary>
        /// Get the collection of subsequent connections for the current node
        /// 获取当前节点的后续连线的集合
        /// </summary>
        public IList<Transition> GetForwardTransitionList(string fromActivityId)
        {
            var transitionList = ProcessModelHelper.GetForwardTransitionList(this.Process, fromActivityId);
            return transitionList;
        }

        /// <summary>
        /// Retrieve the set of subsequent connections to the current node (using conditional filtering)
        /// 获取当前节点的后续连线的集合（使用条件过滤）
        /// </summary>
        public IList<Transition> GetForwardTransitionList(string fromActivityId,
            IDictionary<string, string> conditionKeyValuePair)
        {
            var validTransitionList = new List<Transition>();
            var transitionList = ProcessModelHelper.GetForwardTransitionList(this.Process, fromActivityId);
            foreach (var transition in transitionList)
            {
                bool isValidTranstion = IsValidTransition(transition, conditionKeyValuePair);
                if (isValidTranstion) validTransitionList.Add(transition);
            }
            return validTransitionList;
        }

        /// <summary>
        /// Retrieve the source node list based on the transfer on XML
        /// 根据XML上的转移获取来源节点列表
        /// </summary>
        public IList<Activity> GetFromActivityList(string toActivityId)
        {
            var activityList = new List<Activity>();
            var transitionList = GetBackwardTransitionList(toActivityId);
            foreach (var transition in transitionList)
            {
                activityList.Add(transition.FromActivity);
            }
            return activityList;
        }

        /// <summary>
        /// Get the next node information of the current node
        /// 获取当前节点的下一个节点信息
        /// </summary>
        public Activity GetNextActivity(string activityId)
        {
            var nextActivity = ProcessModelHelper.GetNextActivity(this.Process, activityId);
            return nextActivity;
        }

        #region Route Parser, Rules
        /// <summary>
        /// Obtain the next activity node tree at the beginning of the process, for use in the flow UI.
        /// This is effectively a convenience wrapper over the generic next-activity tree logic,
        /// using the start activity as the current node and without a taskId.
        /// 获取流程起始处的下一步活动节点树，供流转界面使用（起点为开始活动，taskId 为空）
        /// </summary>
        public IList<NodeView> GetFirstActivityTreeView(Activity startActivity, IDictionary<string, string> conditions)
        {
            using (var session = SessionFactory.CreateSession())
            {
                var nextResult = GetNextActivityListView(startActivity.ActivityId, null, conditions, session);
                return nextResult.StepList;
            }
        }

        // Note: First-activity tree previously had its own implementation (GetFirstActivityTree / GetFirstActivityList).
        // To avoid duplicated routing logic, it now delegates to the generic GetNextActivityTree starting
        // from the start activity, which internally uses the unified GetNextActivityList core.

        /// <summary>
        /// Obtain the next activity node tree for use in the flow interface (convenience overload).
        /// This overload creates its own session and does not bind to any specific taskId.
        /// For task-aware routing (e.g. multi-instance gateways), use the overload with taskId and session.
        /// 获取下一步活动节点树，供流转界面使用（便捷方法：内部创建 Session，taskId 为 null）。
        /// </summary>
        public IList<NodeView> GetNextActivityTreeView(string currentActivityId)
        {
            using (var session = SessionFactory.CreateSession())
            {
                var nextResult = GetNextActivityListView(currentActivityId, null, null, session);
                return nextResult.StepList;
            }
        }

        /// <summary>
        /// Obtain the next activity node tree for use in the flow interface (convenience overload with conditions).
        /// This overload creates its own session and does not bind to any specific taskId.
        /// For task-aware routing (e.g. multi-instance gateways), use the overload with taskId and session.
        /// 获取下一步活动节点树，供流转界面使用（便捷方法：内部创建 Session，taskId 为 null，可带条件）。
        /// </summary>
        public IList<NodeView> GetNextActivityTreeView(string currentActivityId, IDictionary<string, string> conditions)
        {
            using (var session = SessionFactory.CreateSession())
            {
                var nextResult = GetNextActivityListView(currentActivityId, null, conditions, session);
                return nextResult.StepList;
            }
        }
        /// <summary>
        /// Obtain the next activity node tree for use in the flow interface (core overload).
        /// This is the routing core used by engine internals, supporting task-aware routing and
        /// SkipDetail jump logic. All convenience overloads delegate to this method.
        /// 获取下一步活动节点树的核心方法，支持 taskId、多实例网关及 SkipDetail 跳转逻辑，
        /// 其它便捷重载最终都委托到此方法。
        /// </summary>
        public NextActivityListViewResult GetNextActivityListView(string currentActivityId,
            Nullable<int> activityInstanceId,
            IDictionary<string, string> conditions,
            IDbSession session)
        {
            var nextTreeResult = new NextActivityListViewResult();
            var treeNodeList = new List<NodeView>();
            var currentActivity = GetActivity(currentActivityId);

            //判断有没有指定的跳转节点信息
            //Determine if there is specified jump node information
            if (currentActivity.SkipDetail != null
                && currentActivity.SkipDetail.IsSkip == true)
            {
                //获取跳转节点信息
                //Obtain jump node information
                var skipto = currentActivity.SkipDetail.Skipto;
                var skiptoActivity = GetActivity(skipto);

                treeNodeList.Add(new NodeView
                {
                    ActivityId = skiptoActivity.ActivityId,
                    ActivityName = skiptoActivity.ActivityName,
                    ActivityCode = skiptoActivity.ActivityCode,
                    ActivityUrl = skiptoActivity.ActivityUrl,
                    MyProperties = skiptoActivity.MyProperties,
                    ActivityType = skiptoActivity.ActivityType,
                    Roles = GetActivityRoles(skiptoActivity.ActivityId),
                    Partakers = GetActivityPartakers(skiptoActivity.ActivityId),
                    IsSkipTo = true
                });
            }
            else
            {
                //Transiton方式的流转定义
                //Definition of Transition Mode Flow
                var processModelBPMNCore = new ProcessModelBPMNCore();
                var nextStepResult = processModelBPMNCore.GetNextActivityTreeListCore(this, currentActivity.ActivityId, activityInstanceId, conditions, session);
                nextTreeResult.Message = nextStepResult.Message;

                foreach (var child in nextStepResult.Root)
                {
                    if (child.HasChildren)
                    {
                        Tranverse(child, treeNodeList);
                    }
                    else
                    {
                        treeNodeList.Add(new NodeView
                        {
                            ActivityId = child.Activity.ActivityId,
                            ActivityName = child.Activity.ActivityName,
                            ActivityCode = child.Activity.ActivityCode,
                            ActivityUrl = child.Activity.ActivityUrl,
                            MyProperties = child.Activity.MyProperties,
                            ActivityType = child.Activity.ActivityType,
                            Roles = GetActivityRoles(child.Activity.ActivityId),
                            Partakers = GetActivityPartakers(child.Activity.ActivityId),
                            ReceiverType = child.Transition.Receiver != null ? child.Transition.Receiver.ReceiverType
                            : ReceiverTypeEnum.Default
                        });
                    }
                }
            }
            nextTreeResult.StepList = treeNodeList;
            return nextTreeResult;
        }

        /// <summary>
        /// Tranverse
        /// </summary>
        private void Tranverse(NextActivityComponent root, IList<NodeView> treeNodeList)
        {
            foreach (var child in root)
            {
                if (child.HasChildren)
                {
                    Tranverse(child, treeNodeList);
                }
                else
                {
                    treeNodeList.Add(new NodeView
                    {
                        ActivityId = child.Activity.ActivityId,
                        ActivityName = child.Activity.ActivityName,
                        ActivityCode = child.Activity.ActivityCode,
                        ActivityUrl = child.Activity.ActivityUrl,
                        MyProperties = child.Activity.MyProperties,
                        ActivityType = child.Activity.ActivityType,
                        Roles = GetActivityRoles(child.Activity.ActivityId),
                        ReceiverType = child.Transition.Receiver != null ? child.Transition.Receiver.ReceiverType
                            : ReceiverTypeEnum.Default
                    });
                }
            }
        }

        /// <summary>
        /// Obtain the next node list accompanied by runtime condition information and resource-based filtering.
        /// 获取下一步节点列表（伴随条件和资源过滤）。
        ///
        /// 说明：
        /// 1) 本方法在内部首先调用私有的 GetNextActivityListCore(currentActivityId, taskId, conditions, session)
        ///    计算“纯结构层”的下一步节点树；
        /// 2) 然后再结合 ActivityResource + 表达式，对结果树做二次过滤；
        /// 3) 该方法主要作为引擎内部使用接口（如特殊测试或扩展场景），
        ///    常规业务/界面层应通过 WorkflowService.GetNextStepInfo / NextStepParser 统一获取下一步信息。
        /// </summary>
        public NextActivityMatchedResult GetNextActivityTreeListRuntime(string currentActivityId,
            Nullable<int> activityInstanceId,
            IDictionary<string, string> conditionKeyValuePair,
            ActivityResource activityResource,
            Expression<Func<ActivityResource, Activity, bool>> expression,
            IDbSession session)
        {
            #region AndSplit Multiple Instance
            Boolean isNotConditionCheck = false;
            NextActivityMatchedResult newResult = null;
            NextActivityComponent newRoot = NextActivityComponentFactory.CreateNextActivityComponent();

            //First, obtain the next node list without runtime expression added (to filter the steps selected by front-end users)
            //The condition variables during definition need to be passed in,
            //and the returned list is the next activity list after parsing
            //先获取未加运行时表达式(为了过滤前端用户选择的步骤)的下一步节点列表
            //定义时的条件变量需要传入，返回的是解析后的下一步活动列表
            var processModelBPMNCore = new ProcessModelBPMNCore();
            NextActivityMatchedResult result = processModelBPMNCore.GetNextActivityTreeListCore(this, currentActivityId,
                activityInstanceId,
                conditionKeyValuePair,
                session);

            //下一步节点列表为空
            //The next step is to leave the node list empty
            if (result.Root.HasChildren == false)
            {
                newResult = NextActivityMatchedResult.CreateNextActivityMatchedResultObject(NextActivityMatchedType.NoneTransitionFilteredByCondition,
                    result.Root);
                return newResult;
            }

            //下一步节点列表封装
            //Next step: Node List Encapsulation
            if (result.Root is NextActivityRouter)
            {
                var router = (NextActivityRouter)result.Root;
                var routerNode = router.ChildActivityList[0];

                //并行与分支(多实例)--不需要判断条件，直接返回下一步列表
                //Parallel and Branch (Multi Instance) - No need to determine conditions,
                //return directly to the next step list
                if (routerNode.Activity.GatewayDetail != null
                    && routerNode.Activity.GatewayDetail.SplitJoinType == GatewaySplitJoinTypeEnum.Split)
                {
                    if (routerNode.Activity.GatewayDetail.DirectionType == GatewayDirectionEnum.AndSplit
                        || routerNode.Activity.GatewayDetail.DirectionType == GatewayDirectionEnum.AndSplitMI)
                    {
                        newRoot = result.Root;
                        isNotConditionCheck = true;
                    }
                }
            }

            if (isNotConditionCheck == false)
            {
                //套入条件表达式
                //Insert conditional expression
                foreach (NextActivityComponent c in result.Root)
                {
                    if (c.HasChildren)
                    {
                        NextActivityComponent child = GetNextActivityListByExpressionRecurisivly(c, activityResource, expression);
                        if (child != null)
                        {
                            newRoot.Add(child);
                        }
                    }
                    else
                    {
                        //前端用户明确指定下一步的执行用户列表
                        //Front end users clearly specify the list of executing users for the next step
                        if (activityResource.AppRunner.NextPerformerType == NextPerformerIntTypeEnum.Specific
                            && activityResource.NextActivityPerformers != null)
                        {
                            if (expression.Compile().Invoke(activityResource, c.Activity))
                            {
                                newRoot.Add(c);
                            }
                        }
                        else if (IsInternalNextPerformerType(activityResource.AppRunner.NextPerformerType) == true)
                        {
                            //系统内部定义的测试使用方式，直接添加下一步步骤到下一步列表
                            //The testing usage method defined within the system can be directly added to the next step list
                            newRoot.Add(c);
                        }
                        else
                        {
                            throw new ApplicationException(LocalizeHelper.GetEngineMessage("processmodel.getnextactivitylist.nonenextstepperformer.error"));
                        }
                    }
                }
            }

            //返回下一步列表
            //Return to the next step list
            if (newRoot.HasChildren)
            {
                newResult = NextActivityMatchedResult.CreateNextActivityMatchedResultObject(result.MatchedType, newRoot);
            }
            else
            {
                newResult = NextActivityMatchedResult.CreateNextActivityMatchedResultObject(NextActivityMatchedType.NoneTransitionFilteredByCondition,
                    newRoot);
            }
            return newResult;
            #endregion
        }

        /// <summary>
        /// Recursively obtain the list of next nodes that meet the conditions
        /// 递归获取满足条件的下一步节点列表
        /// </summary>
        private NextActivityComponent GetNextActivityListByExpressionRecurisivly(NextActivityComponent parent,
           ActivityResource activityResource,
           Expression<Func<ActivityResource, Activity, bool>> expression)
        {
            //创建新父节点
            //Create a new parent node
            NextActivityComponent r1 = NextActivityComponentFactory.CreateNextActivityComponent(parent);

            //递归遍历
            //Traverse
            foreach (NextActivityComponent c in parent)
            {
                if (c.HasChildren)
                {
                    NextActivityComponent child = GetNextActivityListByExpressionRecurisivly(c, activityResource, expression);
                    r1 = AddChildToNewGatewayComponent(r1, c, child);
                }
                else
                {
                    if (expression.Compile().Invoke(activityResource, c.Activity))
                    {
                        r1 = AddChildToNewGatewayComponent(r1, parent, c);
                    }
                }
            }
            return r1;
        }

        /// <summary>
        /// Add child nodes to gateway nodes
        /// 添加子节点到网关节点
        /// </summary>
        private NextActivityComponent AddChildToNewGatewayComponent(NextActivityComponent newParent,
            NextActivityComponent parent,
            NextActivityComponent child)
        {
            if ((newParent != null) && (child != null))
                newParent.Add(child);
            return newParent;
        }

        /// <summary>
        /// Determine whether it is an internally defined executor type
        /// 判断是否是内部定义的执行者类型
        /// </summary>
        private bool IsInternalNextPerformerType(NextPerformerIntTypeEnum performerType)
        {
            bool isInternal = false;
            if (performerType == NextPerformerIntTypeEnum.Definition
                    || performerType == NextPerformerIntTypeEnum.Single)
            {
                isInternal = true;
            }
            return isInternal;
        }

        /// <summary>
        /// Obtain the next node list without adding conditions
        /// 不加条件获取下一步节点列表
        /// </summary>
        public IList<Activity> GetNextActivityListWithoutCondition(string activityId)
        {
            IList<Activity> activityList = new List<Activity>();
            GetNextActivityListWithoutConditionRecurily(activityList, activityId);

            return activityList;
        }

        /// <summary>
        /// Recursive retrieval of the next node list
        /// 递归获取下一步节点列表
        /// </summary>
        private void GetNextActivityListWithoutConditionRecurily(IList<Activity> activityList,
            string activityId)
        {
            var transitionList = GetForwardTransitionList(activityId);
            foreach (var transition in transitionList)
            {
                if (XPDLHelper.IsGatewayComponentNode(transition.ToActivity.ActivityType) == true)
                {
                    GetNextActivityListWithoutConditionRecurily(activityList, transition.ToActivityId);
                }
                else
                {
                    activityList.Add(transition.ToActivity);
                }
            }
        }

        /// <summary>
        /// Obtain the next node list accompanied by runtime condition information
        /// (Excluding judgments within multiple instance nodes, as there are no corresponding transition records)
        /// 获取下一步节点列表，伴随运行时条件信息
        /// （不包含多实例节点内部的判断，因为没有相应的Transition记录）
        /// </summary>
        public IList<Activity> GetPreviousActivityList(string currentActivityId)
        {
            var activityList = new List<Activity>();
            var transitionList = GetBackwardTransitionList(currentActivityId);
            foreach (var transition in transitionList)
            {
                if (XPDLHelper.IsSimpleComponentNode(transition.FromActivity.ActivityType) == true
                    || transition.FromActivity.ActivityType == ActivityTypeEnum.SubProcessNode)
                {
                    activityList.Add(transition.FromActivity);
                }
                else if (transition.FromActivity.ActivityType == ActivityTypeEnum.GatewayNode)
                {
                    var nodeList = GetPreviousActivityList(transition.FromActivity.ActivityId);
                    foreach (var node in nodeList)
                    {
                        activityList.Add(node);
                    }
                }
                else
                {
                    throw new XmlDefinitionException(LocalizeHelper.GetEngineMessage("processmodel.getpreviousactivitylist.error",
                        transition.FromActivity.ActivityType.ToString()));
                }
            }
            return activityList;
        }

        /// <summary>
        /// Retrieve the list of predecessor nodes (excluding internal judgments of multi instance nodes 
        /// as there are no corresponding transition records)
        /// 获取前置节点列表（不包含多实例节点内部的判断，因为没有相应的Transition记录）
        /// </summary>
        public IList<Activity> GetPreviousActivityList(string currentActivityId, out bool hasGatewayPassed)
        {
            var hasGatewayPassedInternal = false;
            IList<Activity> previousActivityList = new List<Activity>();
            GetPreviousActivityList(currentActivityId, previousActivityList, ref hasGatewayPassedInternal);
            hasGatewayPassed = hasGatewayPassedInternal;

            return previousActivityList;
        }

        /// <summary>
        /// Retrieve the list of predecessor nodes (excluding internal judgments of multi instance nodes 
        /// as there are no corresponding transition records)
        /// 获取前置节点列表（不包含多实例节点内部的判断，因为没有相应的Transition记录）
        /// </summary>
        private void GetPreviousActivityList(string currentActivityId,
            IList<Activity> previousActivityList,
            ref bool hasGatewayPassed)
        {
            var transitionList = GetBackwardTransitionList(currentActivityId);
            foreach (var transition in transitionList)
            {
                var fromActivity = GetActivity(transition.FromActivityId);
                if (fromActivity.ActivityType == ActivityTypeEnum.GatewayNode)
                {
                    hasGatewayPassed = true;
                    GetPreviousActivityList(fromActivity.ActivityId, previousActivityList, ref hasGatewayPassed);
                }
                else
                {
                    previousActivityList.Add(GetActivity(fromActivity.ActivityId));
                }
            }
        }

        /// <summary>
        /// Get Previous Activity Tree
        /// 获取上一步
        /// </summary>
        public IList<NodeView> GetPreviousActivityTree(string currentActivityId)
        {
            var hasGatewayPassed = false;
            var activityList = GetPreviousActivityList(currentActivityId, out hasGatewayPassed);
            var treeNodeList = new List<NodeView>();

            foreach (var activity in activityList)
            {
                treeNodeList.Add(new NodeView
                {
                    ActivityId = activity.ActivityId,
                    ActivityName = activity.ActivityName,
                    ActivityCode = activity.ActivityCode,
                    ActivityUrl = activity.ActivityUrl,
                    MyProperties = activity.MyProperties,
                    ActivityType = activity.ActivityType
                });
            }
            return treeNodeList;
        }
        #endregion

        #region Activity Role
        /// <summary>
        /// Retrieve the list of roles under the process
        /// 获取流程下的角色列表
        /// </summary>
        public IList<Role> GetRoles()
        {
            List<Role> roles = new List<Role>();
            var activityList = GetAllTaskActivityList();
            foreach (var activity in activityList)
            {
                var partakerList = activity.PartakerList;
                if (partakerList != null)
                {
                    foreach (var partaker in partakerList)
                    {
                        if (partaker.OuterType == PartakerTypeEnum.Role.ToString())
                        {
                            if (roles.Find(role => role.Id == partaker.OuterId) == null)
                            {
                                var role = new Role { Id = partaker.OuterId, RoleCode = partaker.OuterCode, RoleName = partaker.Name };
                                roles.Add(role);
                            }
                        }
                    }
                }
            }
            return roles;
        }

        /// <summary>
        /// Obtain a list of handling personnel according to the activity definition
        /// 根据活动定义获取办理人员列表
        /// </summary>
        public IDictionary<string, PerformerList> GetActivityPerformers(string activityId)
        {
            var roleList = GetActivityRoles(activityId);
            var performers = ActivityResource.CreateNextActivityPerformers(activityId, roleList);

            return performers;
        }

        /// <summary>
        /// Obtain a list of handling personnel according to the activity definition
        /// 根据活动定义获取办理人员列表
        /// </summary>
        public IDictionary<string, PerformerList> GetActivityPerformers(IList<NodeView> nextActivityTree)
        {
            var nextActivityPerformers = new Dictionary<string, PerformerList>();
            foreach (var node in nextActivityTree)
            {
                var roleList = GetActivityRoles(node.ActivityId);
                ActivityResource.CreateNextActivityPerformers(nextActivityPerformers, node.ActivityId, roleList);
            }
            return nextActivityPerformers;
        }

        /// <summary>
        /// Retrieve the set of role defined on the node
        /// 获取节点上定义的角色集合
        /// </summary>
        public IList<Role> GetActivityRoles(string activityId)
        {
            IList<Role> roles = new List<Role>();
            var activity = ProcessModelHelper.GetActivity(this.Process, activityId);
            var partakerList = activity.PartakerList;
            if (partakerList != null)
            {
                foreach (var partaker in partakerList)
                {
                    if (partaker.OuterType == PartakerTypeEnum.Role.ToString())
                    {
                        var role = new Role { Id = partaker.OuterId, RoleCode = partaker.OuterCode, RoleName = partaker.Name };
                        roles.Add(role);
                    }
                }
            }
            return roles;
        }

        /// <summary>
        /// Retrieve the set of roles and personnel defined on the node
        /// 获取节点上定义的角色及人员集合
        /// </summary>
        internal IList<Partaker> GetActivityPartakers(string activityId)
        {
            var activity = ProcessModelHelper.GetActivity(this.Process, activityId);
            return activity.PartakerList;
        }
        #endregion

        #region Forms
        /// <summary>
        /// Get form list
        /// 获取流程下的表单列表
        /// </summary>
        public IList<Form> GetFormList()
        {
            var forms = new List<Form>();
            foreach (var item in this.Process.FormList)
            {
                var form = new Form
                {
                    FormId = item.OuterId,
                    FormName = item.Name,
                    FormCode = item.OuterCode
                };
                forms.Add(form);
            }
            return forms;
        }
        #endregion

        #region Notifications
        /// <summary>
        /// Retrieve the list of notification users defined on the node
        /// 获取节点上定义的通知用户列表
        /// </summary>
        public IList<User> GetActivityNotifications(string activityId)
        {
            IList<User> users = new List<User>();
            var activity = ProcessModelHelper.GetActivity(this.Process, activityId);
            var partakerList = activity.NotificationList;
            if (partakerList != null)
            {
                foreach (var partaker in partakerList)
                {
                    if (partaker.OuterType == PartakerTypeEnum.User.ToString())
                    {
                        var user = new User { UserId = partaker.OuterId, UserName = partaker.Name };
                        users.Add(user);
                    }
                }
            }
            return users;
        }
        #endregion

        #region Parse Expression
        /// <summary>
        /// Parse Condition by LINQ
        /// 用LINQ解析条件表达式
        /// </summary>
        private bool ParseCondition(Transition transition, IDictionary<string, string> conditionKeyValuePair)
        {
            Boolean result = false;
            try
            {
                string expression = transition.Condition.ConditionText;
                string expressionReplaced = ExpressionParser.ReplaceParameterToValue(expression, conditionKeyValuePair);
                result = ExpressionParser.Parse(expressionReplaced);
            }
            catch (System.Exception ex)
            {
                //throw new WfXpdlException(string.Format("条件表达式解析错误，请确认是否传入所有变量参数！内部错误描述：{0}", ex.Message),
                //    ex);
                ;
            }
            return result;
        }

        /// <summary>
        /// Is it a transition that meets the conditions? If the condition is empty, it defaults to being valid.
        /// 是否是满足条件的Transition，如果条件为空，默认是有效的。
        /// </summary>
        public bool IsValidTransition(Transition transition,
           IDictionary<string, string> conditionKeyValuePair)
        {
            bool isValid = false;

            if (transition.Condition != null && !string.IsNullOrEmpty(transition.Condition.ConditionText))
            {
                if (conditionKeyValuePair != null && conditionKeyValuePair.Count != 0)
                {
                    isValid = ParseCondition(transition, conditionKeyValuePair);
                }
            }
            else
            {
                //流程节点上定义的条件为空，则认为连线是可到达的
                //If the condition defined on the process node is empty, it is considered that the connection is reachable
                isValid = true;
            }
            return isValid;
        }
        #endregion

        #region Activity Type
        /// <summary>
        /// Whether is multiple instance parallel
        /// 是否并行会签节点
        /// </summary>
        public Boolean IsMIParallel(Activity activity)
        {
            var isParallel = false;
            if (activity.ActivityType == ActivityTypeEnum.MultiSignNode)
            {
                if (activity.MultiSignDetail.ComplexType == ComplexTypeEnum.SignTogether
                    || activity.MultiSignDetail.ComplexType == ComplexTypeEnum.SignForward)
                {
                    if (activity.MultiSignDetail.MergeType == MergeTypeEnum.Parallel)
                    {
                        isParallel = true;
                    }
                }
            }
            return isParallel;
        }

        /// <summary>
        /// Whether is multiple instance sequence
        /// 是否串行会签节点
        /// </summary>
        public Boolean IsMISequence(Activity activity)
        {
            var isSequence = false;
            if (activity.ActivityType == ActivityTypeEnum.MultiSignNode)
            {
                if (activity.MultiSignDetail.ComplexType == ComplexTypeEnum.SignTogether
                    || activity.MultiSignDetail.ComplexType == ComplexTypeEnum.SignForward)
                {
                    if (activity.MultiSignDetail.MergeType == MergeTypeEnum.Sequence)
                    {
                        isSequence = true;
                    }
                }
            }
            return isSequence;
        }

        /// <summary>
        /// Whether is multiple instance ndoe
        /// 是否会签节点
        /// </summary>
        public Boolean IsMINode(Activity activity)
        {
            var isMI = false;
            if (activity.ActivityType == ActivityTypeEnum.MultiSignNode)
            {
                isMI = true;
            }
            return isMI;
        }

        /// <summary>
        /// Whether is multiple instance ndoe
        /// 判断是否会签节点
        /// </summary>
        public Boolean IsMINode(ActivityInstanceEntity activityInstance)
        {
            var isMI = false;
            if (activityInstance.ActivityType == (short)ActivityTypeEnum.MultiSignNode)
            {
                isMI = true;
            }
            return isMI;
        }

        /// <summary>
        /// Whehter is task node
        /// 是否任务节点
        /// </summary>
        public Boolean IsTaskNode(Activity activity)
        {
            var isTask = false;
            if (activity.ActivityType == ActivityTypeEnum.TaskNode)
            {
                isTask = true;
            }
            return isTask;
        }

        /// <summary>
        /// Whether is task node
        /// 是否任务节点
        /// </summary>
        public Boolean IsTaskNode(ActivityInstanceEntity activityInstance)
        {
            var isTask = false;
            if (activityInstance.ActivityType == (short)ActivityTypeEnum.TaskNode)
            {
                isTask = true;
            }
            return isTask;
        }

        /// <summary>
        /// Whehter is and split mi container
        /// 是否并行容器节点
        /// </summary>
        public Boolean IsAndSplitMI(Activity activity)
        {
            var isAndSplitMI = false;
            if (activity.GatewayDetail.SplitJoinType == GatewaySplitJoinTypeEnum.Split
                    && activity.GatewayDetail.DirectionType == GatewayDirectionEnum.AndSplitMI)
            {
                isAndSplitMI = true;
            }
            return isAndSplitMI;
        }
        #endregion

        #region Backward Node
        /// <summary>
        /// Obtain the predecessor connection of the node
        /// 获取节点的前驱连线
        /// </summary>
        public IList<Transition> GetBackwardTransitionList(string toActivityId)
        {
            var transitionList = ProcessModelHelper.GetBackwardTransitionList(this.Process, toActivityId);
            return transitionList;
        }

        /// <summary>
        /// Obtain the branch nodes corresponding to the merged nodes
        /// 获取与合并节点相对应的分支节点
        /// </summary>
        public Activity GetBackwardGatewayActivity(Activity fromActivity,
            ref int joinCount,
            ref int splitCount)
        {
            if (fromActivity.ActivityType == ActivityTypeEnum.GatewayNode
                && fromActivity.GatewayDetail.SplitJoinType == GatewaySplitJoinTypeEnum.Join)
            {
                joinCount++;
                IList<Transition> backwardTrans = GetBackwardTransitionList(fromActivity.ActivityId);
                if (backwardTrans.Count > 0)
                {
                    return GetBackwardGatewayActivity(backwardTrans[0].FromActivity, ref joinCount, ref splitCount);
                }
            }
            else if (fromActivity.ActivityType == ActivityTypeEnum.GatewayNode
                && fromActivity.GatewayDetail.SplitJoinType == GatewaySplitJoinTypeEnum.Split)
            {
                splitCount++;
                if (splitCount == joinCount)
                {
                    return fromActivity;
                }
                else
                {
                    IList<Transition> backwardTrans = GetBackwardTransitionList(fromActivity.ActivityId);
                    if (backwardTrans.Count > 0)
                    {
                        return GetBackwardGatewayActivity(backwardTrans[0].FromActivity, ref joinCount, ref splitCount);
                    }
                }
            }
            else if (fromActivity.ActivityType == ActivityTypeEnum.StartNode
                || fromActivity.ActivityType == ActivityTypeEnum.EndNode)
            {
                return null;
            }
            else
            {
                IList<Transition> backwardTrans = GetBackwardTransitionList(fromActivity.ActivityId);
                if (backwardTrans.Count > 0)
                {
                    return GetBackwardGatewayActivity(backwardTrans[0].FromActivity, ref joinCount, ref splitCount);
                }
            }
            return null;
        }

        /// <summary>
        /// Get the predecessor node list of the node (Lambda expression)
        /// 获取节点的前驱节点列表(Lambda表达式)
        /// </summary>
        internal IList<Transition> GetBackwardTransitionList(string activityId,
            Expression<Func<Transition, bool>> expression)
        {
            IList<Transition> transitionList = GetBackwardTransitionList(activityId);
            return GetBackwardTransitionList(activityId, expression);
        }

        /// <summary>
        /// Get the predecessor node list of the node (Lambda expression)
        /// 获取节点的前驱节点列表(Lambda表达式)
        /// </summary>
        internal IList<Transition> GetBackwardTransitionList(IList<Transition> transitionList,
            Expression<Func<Transition, bool>> expression)
        {
            IList<Transition> newTransitionList = new List<Transition>();
            foreach (Transition transition in transitionList)
            {
                if (expression.Compile().Invoke(transition))
                {
                    newTransitionList.Add(transition);
                }
            }
            return newTransitionList;
        }

        /// <summary>
        /// According to the process definition file, obtain a list of conditional node precursor connections
        /// (with conditions that can be reconstructed using Lambda expressions)
        /// 根据流程定义文件，获取带有条件的节点前驱连线列表，（带有条件，可以用Lambda表达式重构）
        /// </summary>
        internal IList<Transition> GetBackworkTransitionListWithCondition(string toActivityId)
        {
            return GetBackwardTransitionList(toActivityId,
                (t => t.Condition != null && !string.IsNullOrEmpty(t.Condition.ConditionText)));
        }

        /// <summary>
        /// Obtain the number of pre node connections
        /// 获取节点前驱连线的数目
        /// </summary>
        public int GetBackwardTransitionListCount(string toActivityId)
        {
            IList<Transition> backwardList = GetBackwardTransitionList(toActivityId);
            return backwardList.Count;
        }
        #endregion
    }
}
