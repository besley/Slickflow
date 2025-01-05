using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Slickflow.Data;
using Slickflow.Module.Form;
using Slickflow.Module.Localize;
using Slickflow.Module.Resource;
using Slickflow.Engine.Utility;
using Slickflow.Engine.Common;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Xpdl.Common;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Engine.Xpdl.Schedule;
using Slickflow.Engine.Xpdl.Node;
using Slickflow.Engine.Config;

namespace Slickflow.Engine.Xpdl
{
    /// <summary>
    /// BPMN2 Process Model
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
                var processGUID = ProcessEntity.ProcessGUID;
                var version = ProcessEntity.Version;

                if (WfConfig.EXPIRED_DAYS_ENABLED == true)
                {
                    //Get Process content from cache
                    if (XPDLMemoryCachedHelper.GetXpdlCache(processGUID, version) == null)
                    {
                        var process = ConvertProcessModelFromXML(ProcessEntity);
                        XPDLMemoryCachedHelper.SetXpdlCache(processGUID, version, process);
                    }
                    return XPDLMemoryCachedHelper.GetXpdlCache(processGUID, version);
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
            if (root.Name == XPDLDefinition.BPMN2_ElementName_Definitions)
            {
                if (processEntity.PackageType == null)
                {
                    //单一流程
                    //Simple Process
                    xmlNodeProcess = root.SelectSingleNode(XPDLDefinition.BPMN2_StrXmlPath_Process, 
                        XPDLHelper.GetSlickflowXmlNamespaceManager(xmlDoc));
                }
                else
                {
                    //泳道流程
                    //Pool Process
                    var xPath = string.Format("{0}[@sf:guid='{1}']", XPDLDefinition.BPMN2_StrXmlPath_Process, processEntity.ProcessGUID);
                    xmlNodeProcess = root.SelectSingleNode(xPath, XPDLHelper.GetSlickflowXmlNamespaceManager(xmlDoc));
                }
            }
            else if (root.Name == XPDLDefinition.BPMN2_ElementName_SubProcess)
            {
                //子流程
                //Sub Process
                xmlNodeProcess = root;
            }
            else
            {
                throw new NotImplementedException("NOT supported process xml content");
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
        /// <param name="activityGUID"></param>
        /// <returns></returns>
        public Activity GetActivity(string activityGUID)
        {
            var activity = ProcessModelHelper.GetActivity(this.Process, activityGUID);
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
            var startActivityGUID = startNode.ActivityGUID;

            activityList.Add(startNode);

            return TranverseTransitionList(activityList, startActivityGUID);
        }

        /// <summary>
        /// Recursive traversal of predecessor nodes on transfer
        /// 递归遍历转移上的前置节点
        /// </summary>
        /// <param name="activityList"></param>
        /// <param name="fromActivityGUID"></param>
        /// <returns></returns>
        private IList<Activity> TranverseTransitionList(List<Activity> activityList, string fromActivityGUID)
        {
            Activity toActivity = null;
            var transitionList = GetForwardTransitionList(fromActivityGUID);
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
                TranverseTransitionList(activityList, toActivity.ActivityGUID);
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
            var startActivityGUID = startNode.ActivityGUID;

            return TranverseTaskTransitionList(activityList, startActivityGUID);
        }

        /// <summary>
        /// Recursive traversal of predecessor nodes on transfer
        /// 递归遍历转移上的前置节点
        /// </summary>
        private IList<Activity> TranverseTaskTransitionList(List<Activity> activityList, string fromActivityGUID)
        {
            Activity toActivity = null;
            var transitionList = GetForwardTransitionList(fromActivityGUID);
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
                TranverseTaskTransitionList(activityList, toActivity.ActivityGUID);
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
            return TranverseTransitionListBetweenSplitJoin(taskActivityList, splitActivity.ActivityGUID, joinActivity.ActivityGUID);
        }

        /// <summary>
        /// Recursive traversal of predecessor nodes on transfer
        /// 递归遍历转移上的前置节点
        /// </summary>
        private IList<Activity> TranverseTransitionListBetweenSplitJoin(IList<Activity> activityList,
            string fromActivityGUID,
            string finalActivityGUID)
        {
            Activity toActivity = null;
            var transitionNodeList = GetForwardTransitionList(fromActivityGUID);
            foreach (var transition in transitionNodeList)
            {
                toActivity = transition.ToActivity;
                if (toActivity.ActivityType == ActivityTypeEnum.GatewayNode
                    && toActivity.ActivityGUID == finalActivityGUID)
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
                TranverseTransitionListBetweenSplitJoin(activityList, toActivity.ActivityGUID, finalActivityGUID);
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
                if (a.ActivityGUID == toActivity.ActivityGUID)
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
                    return GetNextActivityList(startActivity.ActivityGUID, null, conditionKeyValuePair, session);
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
            var transitionList = GetBackwardTransitionList(gatewayActivity.ActivityGUID);
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
        public Transition GetForwardTransition(string fromActivityGUID, string toActivityGUID)
        {
            var transition = ProcessModelHelper.GetForwardTransition(this.Process, fromActivityGUID, toActivityGUID);
            return transition;
        }

        /// <summary>
        /// Get the collection of subsequent connections for the current node
        /// 获取当前节点的后续连线的集合
        /// </summary>
        public IList<Transition> GetForwardTransitionList(string fromActivityGUID)
        {
            var transitionList = ProcessModelHelper.GetForwardTransitionList(this.Process, fromActivityGUID);
            return transitionList;
        }

        /// <summary>
        /// Retrieve the set of subsequent connections to the current node (using conditional filtering)
        /// 获取当前节点的后续连线的集合（使用条件过滤）
        /// </summary>
        public IList<Transition> GetForwardTransitionList(string fromActivityGUID,
            IDictionary<string, string> conditionKeyValuePair)
        {
            var validTransitionList = new List<Transition>();
            var transitionList = ProcessModelHelper.GetForwardTransitionList(this.Process, fromActivityGUID);
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
        public IList<Activity> GetFromActivityList(string toActivityGUID)
        {
            var activityList = new List<Activity>();
            var transitionList = GetBackwardTransitionList(toActivityGUID);
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
        public Activity GetNextActivity(string activityGUID)
        {
            var nextActivity = ProcessModelHelper.GetNextActivity(this.Process, activityGUID);
            return nextActivity;
        }

        #region Route Parser, Rules
        /// <summary>
        /// Obtain the next activity node tree for use in the flow interface
        /// 获取下一步活动节点树，供流转界面使用
        /// </summary>
        public IList<NodeView> GetFirstActivityTree(Activity startActivity, IDictionary<string, string> conditions)
        {
            using (var session = SessionFactory.CreateSession())
            {
                var nextResult = GetFirstActivityTree(startActivity, conditions, session);
                return nextResult.StepList;
            }
        }

        /// <summary>
        /// Obtain the next activity node tree for use in the flow interface
        /// 获取下一步活动节点树，供流转界面使用
        /// </summary>
        private NextActivityTreeResult GetFirstActivityTree(Activity startActivity,
            IDictionary<string, string> conditions,
            IDbSession session)
        {
            var firstTreeResult = new NextActivityTreeResult();
            var treeNodeList = new List<NodeView>();
            var nextStepResult = GetFirstActivityList(startActivity, conditions, session);
            firstTreeResult.Message = nextStepResult.Message;

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
                        ActivityGUID = child.Activity.ActivityGUID,
                        ActivityName = child.Activity.ActivityName,
                        ActivityCode = child.Activity.ActivityCode,
                        ActivityUrl = child.Activity.ActivityUrl,
                        MyProperties = child.Activity.MyProperties,
                        ActivityType = child.Activity.ActivityType,
                        Roles = GetActivityRoles(child.Activity.ActivityGUID),
                        Partakers = GetActivityPartakers(child.Activity.ActivityGUID),
                        ReceiverType = child.Transition.Receiver != null ? child.Transition.Receiver.ReceiverType
                        : ReceiverTypeEnum.Default
                    });
                }
            }
            firstTreeResult.StepList = treeNodeList;
            return firstTreeResult;
        }

        /// <summary>
        /// Obtain the next node list accompanied by runtime condition information
        /// 获取下一步节点列表，伴随运行时条件信息
        /// </summary>
        private NextActivityMatchedResult GetFirstActivityList(Activity startEntity,
            IDictionary<string, string> conditionKeyValuePair,
            IDbSession session)
        {
            try
            {
                NextActivityMatchedResult result = null;
                var resultType = NextActivityMatchedType.Unknown;
                var root = NextActivityComponentFactory.CreateNextActivityComponent();

                var transitionList = GetForwardTransitionList(startEntity.ActivityGUID, conditionKeyValuePair).ToList();
                if (transitionList.Count > 0)
                {
                    //遍历连线，获取下一步节点的列表
                    //Traverse the connection to obtain the list of next nodes
                    NextActivityComponent child = null;
                    foreach (Transition transition in transitionList)
                    {
                        if (XPDLHelper.IsSimpleComponentNode(transition.ToActivity.ActivityType) == true)        
                        {
                            child = NextActivityComponentFactory.CreateNextActivityComponent(transition, transition.ToActivity);
                        }
                        else if (XPDLHelper.IsGatewayComponentNode(transition.ToActivity.ActivityType) == true)
                        {
                            NextActivityScheduleBase activitySchedule = NextActivityScheduleFactory.CreateActivitySchedule(this as IProcessModel,
                                transition.ToActivity.GatewayDetail.SplitJoinType,
                                null);

                            //获取网关后面的节点
                            //Get the nodes behind the gateway
                            child = activitySchedule.GetNextActivityListFromGateway(transition,
                                transition.ToActivity,
                                conditionKeyValuePair,
                                session,
                                out resultType);
                        }
                        else if (XPDLHelper.IsCrossOverComponentNode(transition.ToActivity.ActivityType) == true)
                        {
                            //事件类型的特殊节点处理，跟网关类似
                            //Special node handling for event types, similar to gateways
                            NextActivityScheduleBase activitySchedule = NextActivityScheduleFactory.CreateActivityScheduleIntermediate(this as IProcessModel);
                            child = activitySchedule.GetNextActivityListFromGateway(transition,
                                transition.ToActivity,
                                conditionKeyValuePair,
                                session,
                                out resultType);
                        }
                        else
                        {
                            var errMsg = string.Format("Unknown node type：{0}", transition.ToActivity.ActivityType.ToString());
                            LogManager.RecordLog(LocalizeHelper.GetEngineMessage("processmodel.getnextactivitylist.error"),
                                LogEventType.Exception,
                                LogPriority.Normal,
                                null,
                                new WfXpdlException(errMsg));
                            throw new XmlDefinitionException(errMsg);
                        }

                        if (child != null)
                        {
                            root.Add(child);
                            resultType = NextActivityMatchedType.Successed;
                        }
                    }
                }
                else
                {
                    resultType = NextActivityMatchedType.NoneTransitionFilteredByCondition;
                }
                result = NextActivityMatchedResult.CreateNextActivityMatchedResultObject(resultType, root);
                return result;
            }
            catch (System.Exception e)
            {
                LogManager.RecordLog(LocalizeHelper.GetEngineMessage("processmodel.getnextactivitylist.error"),
                    LogEventType.Exception,
                    LogPriority.Normal,
                    null,
                    new WfXpdlException(e.Message));
                throw new WfXpdlException(LocalizeHelper.GetEngineMessage("processmodel.getnextactivitylist.error", e.Message),
                    e);
            }
        }

        /// <summary>
        /// Obtain the next activity node tree for use in the flow interface
        /// 获取下一步活动节点树，供流转界面使用
        /// </summary>
        public IList<NodeView> GetNextActivityTree(string currentActivityGUID)
        {
            using (var session = SessionFactory.CreateSession())
            {
                var nextResult = GetNextActivityTree(currentActivityGUID, null, null, session);
                return nextResult.StepList;
            }
        }

        /// <summary>
        /// Obtain the next activity node tree for use in the flow interface
        /// 获取下一步活动节点树，供流转界面使用
        /// </summary>
        public IList<NodeView> GetNextActivityTree(string currentActivityGUID, IDictionary<string, string> conditions)
        {
            using (var session = SessionFactory.CreateSession())
            {
                var nextResult = GetNextActivityTree(currentActivityGUID, null, conditions, session);
                return nextResult.StepList;
            }
        }
        /// <summary>
        /// Obtain the next activity node tree for use in the flow interface
        /// 获取下一步活动节点树，供流转界面使用
        /// </summary>
        public NextActivityTreeResult GetNextActivityTree(string currentActivityGUID,
            Nullable<int> taskID,
            IDictionary<string, string> conditions,
            IDbSession session)
        {
            var nextTreeResult = new NextActivityTreeResult();
            var treeNodeList = new List<NodeView>();
            var currentActivity = GetActivity(currentActivityGUID);

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
                    ActivityGUID = skiptoActivity.ActivityGUID,
                    ActivityName = skiptoActivity.ActivityName,
                    ActivityCode = skiptoActivity.ActivityCode,
                    ActivityUrl = skiptoActivity.ActivityUrl,
                    MyProperties = skiptoActivity.MyProperties,
                    ActivityType = skiptoActivity.ActivityType,
                    Roles = GetActivityRoles(skiptoActivity.ActivityGUID),
                    Partakers = GetActivityPartakers(skiptoActivity.ActivityGUID),
                    IsSkipTo = true
                });
            }
            else
            {
                //Transiton方式的流转定义
                //Definition of Transition Mode Flow
                var nextStepResult = GetNextActivityList(currentActivity.ActivityGUID, taskID, conditions, session);
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
                            ActivityGUID = child.Activity.ActivityGUID,
                            ActivityName = child.Activity.ActivityName,
                            ActivityCode = child.Activity.ActivityCode,
                            ActivityUrl = child.Activity.ActivityUrl,
                            MyProperties = child.Activity.MyProperties,
                            ActivityType = child.Activity.ActivityType,
                            Roles = GetActivityRoles(child.Activity.ActivityGUID),
                            Partakers = GetActivityPartakers(child.Activity.ActivityGUID),
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
                        ActivityGUID = child.Activity.ActivityGUID,
                        ActivityName = child.Activity.ActivityName,
                        ActivityCode = child.Activity.ActivityCode,
                        ActivityUrl = child.Activity.ActivityUrl,
                        MyProperties = child.Activity.MyProperties,
                        ActivityType = child.Activity.ActivityType,
                        Roles = GetActivityRoles(child.Activity.ActivityGUID),
                        ReceiverType = child.Transition.Receiver != null ? child.Transition.Receiver.ReceiverType
                            : ReceiverTypeEnum.Default
                    });
                }
            }
        }


        /// <summary>
        /// Obtain the next node list accompanied by runtime condition information
        /// 获取下一步节点列表，伴随运行时条件信息
        /// </summary>
        private NextActivityMatchedResult GetNextActivityList(string currentActivityGUID,
            Nullable<int> taskID,
            IDictionary<string, string> conditionKeyValuePair,
            IDbSession session)
        {
            try
            {
                NextActivityMatchedResult result = null;
                NextActivityMatchedType resultType = NextActivityMatchedType.Unknown;

                //创建“下一步节点”的根节点
                //Create the root node for the 'next step node'
                NextActivityComponent root = NextActivityComponentFactory.CreateNextActivityComponent();

                //开始正常情况下的路径查找
                //Start normal path search
                List<Transition> transitionList = GetForwardTransitionList(currentActivityGUID,
                    conditionKeyValuePair).ToList();

                if (transitionList.Count > 0)
                {
                    //遍历连线，获取下一步节点的列表
                    //Traverse the connection to obtain the list of next nodes
                    NextActivityComponent child = null;
                    foreach (Transition transition in transitionList)
                    {
                        if (XPDLHelper.IsSimpleComponentNode(transition.ToActivity.ActivityType) == true)       
                        {
                            child = NextActivityComponentFactory.CreateNextActivityComponent(transition, transition.ToActivity);
                        }
                        else if (XPDLHelper.IsGatewayComponentNode(transition.ToActivity.ActivityType) == true)
                        {
                            NextActivityScheduleBase activitySchedule = NextActivityScheduleFactory.CreateActivitySchedule(this as IProcessModel,
                                transition.ToActivity.GatewayDetail.SplitJoinType,
                                taskID);

                            //获取网关后面的节点
                            //Get the nodes behind the gateway
                            child = activitySchedule.GetNextActivityListFromGateway(transition,
                                transition.ToActivity,
                                conditionKeyValuePair,
                                session,
                                out resultType);
                        }
                        else if (XPDLHelper.IsCrossOverComponentNode(transition.ToActivity.ActivityType) == true)
                        {
                            //事件类型的特殊节点处理，跟网关类似
                            //Special node handling for event types, similar to gateways
                            NextActivityScheduleBase activitySchedule = NextActivityScheduleFactory.CreateActivityScheduleIntermediate(this as IProcessModel);
                            child = activitySchedule.GetNextActivityListFromGateway(transition,
                                transition.ToActivity,
                                conditionKeyValuePair,
                                session,
                                out resultType);
                        }
                        else
                        {
                            var errMsg = string.Format("Unknown node type：{0}", transition.ToActivity.ActivityType.ToString());
                            LogManager.RecordLog(LocalizeHelper.GetEngineMessage("processmodel.getnextactivitylist.error"),
                                LogEventType.Exception,
                                LogPriority.Normal,
                                null,
                                new WfXpdlException(errMsg));
                            throw new XmlDefinitionException(errMsg);
                        }

                        if (child != null)
                        {
                            root.Add(child);
                            resultType = NextActivityMatchedType.Successed;
                        }
                    }
                }
                else
                {
                    resultType = NextActivityMatchedType.NoneTransitionFilteredByCondition;
                }
                result = NextActivityMatchedResult.CreateNextActivityMatchedResultObject(resultType, root);
                return result;
            }
            catch (System.Exception e)
            {
                LogManager.RecordLog(LocalizeHelper.GetEngineMessage("processmodel.getnextactivitylist.error"),
                    LogEventType.Exception,
                    LogPriority.Normal,
                    null,
                    new WfXpdlException(e.Message));
                throw new WfXpdlException(LocalizeHelper.GetEngineMessage("processmodel.getnextactivitylist.error", e.Message),
                    e);
            }
        }

        /// <summary>
        ///  Obtain the next node list accompanied by runtime condition information
        /// 获取下一步节点列表（伴随条件和资源）
        /// </summary>
        public NextActivityMatchedResult GetNextActivityList(string currentActivityGUID,
            Nullable<int> taskID,
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
            NextActivityMatchedResult result = GetNextActivityList(currentActivityGUID,
                taskID,
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
                var routerNode = router.NextActivityList[0];

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
        public IList<Activity> GetNextActivityListWithoutCondition(string activityGUID)
        {
            IList<Activity> activityList = new List<Activity>();
            GetNextActivityListWithoutConditionRecurily(activityList, activityGUID);

            return activityList;
        }

        /// <summary>
        /// Recursive retrieval of the next node list
        /// 递归获取下一步节点列表
        /// </summary>
        private void GetNextActivityListWithoutConditionRecurily(IList<Activity> activityList,
            string activityGUID)
        {
            var transitionList = GetForwardTransitionList(activityGUID);
            foreach (var transition in transitionList)
            {
                var toActivityGUID = transition.ToActivityGUID;
                if (XPDLHelper.IsGatewayComponentNode(transition.ToActivity.ActivityType) == true)
                {
                    GetNextActivityListWithoutConditionRecurily(activityList, transition.ToActivityGUID);
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
        public IList<Activity> GetPreviousActivityList(string currentActivityGUID)
        {
            var activityList = new List<Activity>();
            var transitionList = GetBackwardTransitionList(currentActivityGUID);
            foreach (var transition in transitionList)
            {
                if (XPDLHelper.IsSimpleComponentNode(transition.FromActivity.ActivityType) == true
                    || transition.FromActivity.ActivityType == ActivityTypeEnum.SubProcessNode)
                {
                    activityList.Add(transition.FromActivity);
                }
                else if (transition.FromActivity.ActivityType == ActivityTypeEnum.GatewayNode)
                {
                    var nodeList = GetPreviousActivityList(transition.FromActivity.ActivityGUID);
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
        public IList<Activity> GetPreviousActivityList(string currentActivityGUID, out bool hasGatewayPassed)
        {
            var hasGatewayPassedInternal = false;
            IList<Activity> previousActivityList = new List<Activity>();
            GetPreviousActivityList(currentActivityGUID, previousActivityList, ref hasGatewayPassedInternal);
            hasGatewayPassed = hasGatewayPassedInternal;

            return previousActivityList;
        }

        /// <summary>
        /// Retrieve the list of predecessor nodes (excluding internal judgments of multi instance nodes 
        /// as there are no corresponding transition records)
        /// 获取前置节点列表（不包含多实例节点内部的判断，因为没有相应的Transition记录）
        /// </summary>
        private void GetPreviousActivityList(string currentActivityGUID,
            IList<Activity> previousActivityList,
            ref bool hasGatewayPassed)
        {
            var transitionList = GetBackwardTransitionList(currentActivityGUID);
            foreach (var transition in transitionList)
            {
                var fromActivity = GetActivity(transition.FromActivityGUID);
                if (fromActivity.ActivityType == ActivityTypeEnum.GatewayNode)
                {
                    hasGatewayPassed = true;
                    GetPreviousActivityList(fromActivity.ActivityGUID, previousActivityList, ref hasGatewayPassed);
                }
                else
                {
                    previousActivityList.Add(GetActivity(fromActivity.ActivityGUID));
                }
            }
        }

        /// <summary>
        /// Get Previous Activity Tree
        /// 获取上一步
        /// </summary>
        public IList<NodeView> GetPreviousActivityTree(string currentActivityGUID)
        {
            var hasGatewayPassed = false;
            var activityList = GetPreviousActivityList(currentActivityGUID, out hasGatewayPassed);
            var treeNodeList = new List<NodeView>();

            foreach (var activity in activityList)
            {
                treeNodeList.Add(new NodeView
                {
                    ActivityGUID = activity.ActivityGUID,
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
                foreach (var partaker in partakerList)
                {
                    if (partaker.OuterType == PartakerTypeEnum.Role.ToString())
                    {
                        if (roles.Find(role => role.ID == partaker.OuterID) == null)
                        {
                            var role = new Role { ID = partaker.OuterID, RoleCode = partaker.OuterCode, RoleName = partaker.Name };
                            roles.Add(role);
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
        public IDictionary<string, PerformerList> GetActivityPerformers(string activityGUID)
        {
            var roleList = GetActivityRoles(activityGUID);
            var performers = ActivityResource.CreateNextActivityPerformers(activityGUID, roleList);

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
                var roleList = GetActivityRoles(node.ActivityGUID);
                ActivityResource.CreateNextActivityPerformers(nextActivityPerformers, node.ActivityGUID, roleList);
            }
            return nextActivityPerformers;
        }

        /// <summary>
        /// Retrieve the set of role defined on the node
        /// 获取节点上定义的角色集合
        /// </summary>
        public IList<Role> GetActivityRoles(string activityGUID)
        {
            IList<Role> roles = new List<Role>();
            var activity = ProcessModelHelper.GetActivity(this.Process, activityGUID);
            var partakerList = activity.PartakerList;
            if (partakerList != null)
            {
                foreach (var partaker in partakerList)
                {
                    if (partaker.OuterType == PartakerTypeEnum.Role.ToString())
                    {
                        var role = new Role { ID = partaker.OuterID, RoleCode = partaker.OuterCode, RoleName = partaker.Name };
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
        internal IList<Partaker> GetActivityPartakers(string activityGUID)
        {
            var activity = ProcessModelHelper.GetActivity(this.Process, activityGUID);
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
                    FormID = item.OuterID,
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
        public IList<User> GetActivityNotifications(string activityGUID)
        {
            IList<User> users = new List<User>();
            var activity = ProcessModelHelper.GetActivity(this.Process, activityGUID);
            var partakerList = activity.NotificationList;
            if (partakerList != null)
            {
                foreach (var partaker in partakerList)
                {
                    if (partaker.OuterType == PartakerTypeEnum.User.ToString())
                    {
                        var user = new User { UserID = partaker.OuterID, UserName = partaker.Name };
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
        public IList<Transition> GetBackwardTransitionList(string toActivityGUID)
        {
            var transitionList = ProcessModelHelper.GetBackwardTransitionList(this.Process, toActivityGUID);
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
                IList<Transition> backwardTrans = GetBackwardTransitionList(fromActivity.ActivityGUID);
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
                    IList<Transition> backwardTrans = GetBackwardTransitionList(fromActivity.ActivityGUID);
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
                IList<Transition> backwardTrans = GetBackwardTransitionList(fromActivity.ActivityGUID);
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
        internal IList<Transition> GetBackwardTransitionList(string activityGUID,
            Expression<Func<Transition, bool>> expression)
        {
            IList<Transition> transitionList = GetBackwardTransitionList(activityGUID);
            return GetBackwardTransitionList(activityGUID, expression);
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
        internal IList<Transition> GetBackworkTransitionListWithCondition(string toActivityGUID)
        {
            return GetBackwardTransitionList(toActivityGUID,
                (t => t.Condition != null && !string.IsNullOrEmpty(t.Condition.ConditionText)));
        }

        /// <summary>
        /// Obtain the number of pre node connections
        /// 获取节点前驱连线的数目
        /// </summary>
        public int GetBackwardTransitionListCount(string toActivityGUID)
        {
            IList<Transition> backwardList = GetBackwardTransitionList(toActivityGUID);
            return backwardList.Count;
        }
        #endregion
    }
}
