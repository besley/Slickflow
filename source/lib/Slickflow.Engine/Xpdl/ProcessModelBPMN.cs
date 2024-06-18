/*
The Slickflow project.
Copyright (C) 2014  .NET Workflow Engine Library

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, you can access the official
web page about lgpl: https://www.gnu.org/licenses/lgpl.html
*/

using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Slickflow.Data;
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

namespace Slickflow.Engine.Xpdl
{
    /// <summary>
    /// BPMN2流程模型类
    /// </summary>
    public class ProcessModelBPMN : IProcessModel
    {
        #region 属性与构造函数
        /// <summary>
        /// 流程实体
        /// </summary>
        public Process Process
        {
            get
            {
                var processGUID = string.IsNullOrEmpty(this.SubProcessGUID) ? ProcessEntity.ProcessGUID : this.SubProcessGUID;
                var version = ProcessEntity.Version;

                if (XPDLMemoryCachedHelper.GetXpdlCache(processGUID, version) == null)
                {
                    var process = ConvertProcessModelFromXML();
                    XPDLMemoryCachedHelper.SetXpdlCache(processGUID, version, process);
                }
                return XPDLMemoryCachedHelper.GetXpdlCache(processGUID, version);
            }
        }
        /// <summary>
        /// 流程实体
        /// </summary>
        public ProcessEntity ProcessEntity { get; set; }

        /// <summary>
        /// 子流程ID
        /// </summary>
        public string SubProcessGUID { get; set; }
        
        /// <summary>
        /// 从XML转换流程实体
        /// </summary>
        /// <returns></returns>
        private Process ConvertProcessModelFromXML()
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(this.ProcessEntity.XmlContent);
            var root = xmlDoc.DocumentElement;

            //按照是否有子流程来构建流程模型
            Process process = null;
            if (!string.IsNullOrEmpty(this.SubProcessGUID))
            {
                var xmlNodeSubProcess = XMLHelper.GetXmlNodeByXpath(xmlDoc,
                    string.Format("{0}[@sf:guid='" + this.SubProcessGUID + "']", XPDLDefinition.BPMN2_StrXmlPath_Process_Sub), 
                    XPDLHelper.GetSlickflowXmlNamespaceManager(xmlDoc));
                process = ProcessModelConvertor.ConvertProcessModelFromXML(xmlNodeSubProcess);
            }
            else
            {
                var xmlNodeProcess = root.SelectSingleNode(XPDLDefinition.BPMN2_StrXmlPath_Process,
                    XPDLHelper.GetSlickflowXmlNamespaceManager(xmlDoc));
                process = ProcessModelConvertor.ConvertProcessModelFromXML(xmlNodeProcess);
            }
            return process;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="entity">流程实体</param>
        public ProcessModelBPMN(ProcessEntity entity)
        {
            ProcessEntity = entity;
        }
        #endregion

        #region Get Activity from XML
        /// <summary>
        /// 获取XML的节点信息
        /// </summary>
        /// <param name="activityGUID">节点GUID</param>
        /// <returns>Xml节点</returns>
        //private XmlNode GetXmlActivityNodeFromXmlFile(string activityGUID)
        //{
        //    var xmlNode = ConvertHelper.GetXmlActivityNodeFromXmlFile(XMLProcessDefinition,
        //        XMLProcessNamespaceManager,
        //        activityGUID,
        //        this.IsSubProcess);
        //    return xmlNode;
        //}

        /// <summary>
        /// 获取活动节点的类型信息
        /// </summary>
        /// <param name="nodeType">节点类型</param>
        /// <returns>Xml节点</returns>
        //private XmlNode GetXmlActivityTypeSingleNodeFromXmlFile(ActivityTypeEnum nodeType)
        //{
        //    var xmlDoc = XMLProcessDefinition;
        //    XmlNode typeNode = null;
        //    if (nodeType == ActivityTypeEnum.StartNode)
        //    {
        //        typeNode = XMLHelper.GetXmlNodeByXpath(xmlDoc, XPDLHelper.GetXmlPathByActivityType(ActivityTypeEnum.StartNode, this.IsSubProcess),
        //            XMLProcessNamespaceManager);
        //    }
        //    else if (nodeType == ActivityTypeEnum.EndNode)
        //    {
        //        typeNode = XMLHelper.GetXmlNodeByXpath(xmlDoc, XPDLHelper.GetXmlPathByActivityType(ActivityTypeEnum.EndNode, this.IsSubProcess),
        //            XMLProcessNamespaceManager);
        //    }
        //    else
        //    {
        //        throw new ApplicationException(String.Format("Not supported node type:{0}", nodeType.ToString()));
        //    }
        //    return typeNode;
        //}

        ///// <summary>
        ///// 获取特定类型的活动节点
        ///// </summary>
        ///// <param name="nodeType">节点类型</param>
        ///// <returns>Xml节点列表</returns>
        //private XmlNodeList GetXmlActivityListByTypeFromXmlFile(ActivityTypeEnum nodeType)
        //{
        //    XmlNodeList nodeList = XMLHelper.GetXmlNodeListByXpath(XMLProcessDefinition,
        //        string.Format("{0}/ActivityType[@type='" + nodeType.ToString() + "']", XPDLDefinition.StrXmlActivityPath));
        //    return nodeList;
        //}

        ///// <summary>
        ///// 获取参与者信息
        ///// </summary>
        ///// <param name="participantGUID">参与者GUID</param>
        ///// <returns>XML节点</returns>
        //private XmlNode GetXmlParticipantNodeFromXmlFile(string participantGUID)
        //{
        //    XmlNode participantNode = XMLHelper.GetXmlNodeByXpath(XMLProcessDefinition,
        //        string.Format("{0}[@id='" + participantGUID + "']", XPDLDefinition.StrXmlSingleParticipantPath));
        //    return participantNode;
        //}
        #endregion

        /// <summary>
        /// 获取当前节点信息
        /// </summary>
        /// <param name="activityGUID">活动节点GUID</param>
        /// <returns>活动实体</returns>
        public Activity GetActivity(string activityGUID)
        {
            var activity = ProcessModelHelper.GetActivity(this.Process, activityGUID);
            return activity;
        }

        public IList<Activity> GetActivityList()
        {
            List<Activity> activityList = new List<Activity>();
            var startNode = GetStartActivity();
            var startActivityGUID = startNode.ActivityGUID;

            activityList.Add(startNode);

            return TranverseTransitionList(activityList, startActivityGUID);
        }

        /// <summary>
        /// 递归遍历转移上的前置节点
        /// </summary>
        /// <param name="activityList">活动列表</param>
        /// <param name="fromActivityGUID">起始活动GUID</param>
        /// <returns>节点实体列表</returns>
        private IList<Activity> TranverseTransitionList(List<Activity> activityList, string fromActivityGUID)
        {
            Activity toActivity = null;
            var transitionList = GetForwardTransitionList(fromActivityGUID);
            foreach (var transition in transitionList)
            {
                AppendActivity(activityList, transition.ToActivity);
                if (toActivity.ActivityType == ActivityTypeEnum.EndNode)
                {
                    break;
                }
                TranverseTransitionList(activityList, toActivity.ActivityGUID);
            }
            return activityList;
        }

        /// <summary>
        /// 获取任务类型的节点(包含会签节点和子流程节点)，按照Transition顺序组成序列
        /// </summary>
        /// <returns>节点实体列表</returns>
        public IList<Activity> GetAllTaskActivityList()
        {
            List<Activity> activityList = new List<Activity>();
            var startNode = GetStartActivity();
            var startActivityGUID = startNode.ActivityGUID;

            return TranverseTaskTransitionList(activityList, startActivityGUID);
        }

        /// <summary>
        /// 递归遍历转移上的前置节点
        /// </summary>
        /// <param name="activityList">活动列表</param>
        /// <param name="fromActivityGUID">起始活动GUID</param>
        /// <returns>节点实体列表</returns>
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
                TranverseTaskTransitionList(activityList, toActivity.ActivityGUID);
            }
            return activityList;
        }

        /// <summary>
        /// 获取任务类型的节点
        /// </summary>
        /// <returns>节点实体列表</returns>
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
        /// 获取分支和合并之间的任务节点列表
        /// </summary>
        /// <param name="splitActivity">分支节点</param>
        /// <param name="joinActivity">合并节点</param>
        /// <returns>节点列表</returns>
        public IList<Activity> GetAllTaskActivityList(Activity splitActivity,
            Activity joinActivity)
        {
            IList<Activity> taskActivityList = new List<Activity>();
            return TranverseTransitionListBetweenSplitJoin(taskActivityList, splitActivity.ActivityGUID, joinActivity.ActivityGUID);
        }

        /// <summary>
        /// 递归遍历转移上的前置节点
        /// </summary>
        /// <param name="activityList">活动列表</param>
        /// <param name="fromActivityGUID">起始活动GUID</param>
        /// <param name="finalActivityGUID">截止活动GUID</param>
        /// <returns>节点实体列表</returns>
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
                TranverseTransitionListBetweenSplitJoin(activityList, toActivity.ActivityGUID, finalActivityGUID);
            }
            return activityList;
        }

        /// <summary>
        /// 添加节点到节点列表中，去掉有重复的节点
        /// </summary>
        /// <param name="activityList">节点列表</param>
        /// <param name="toActivity">节点</param>
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
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
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
        /// 获取第一个节点
        /// </summary>
        /// <returns>活动节点</returns>
        public Activity GetFirstActivity()
        {
            var activity = ProcessModelHelper.GetFirstActivity(this.Process);
            return activity;
        }

        /// <summary>
        /// 获取流程起始的活动节点列表(开始节点之后，可能有多个节点)
        /// </summary>
        /// <param name="startActivity">开始节点</param>
        /// <param name="conditionKeyValuePair">条件表达式的参数名称-参数值的集合</param>
        /// <returns></returns>
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
        /// 获取结束节点
        /// </summary>
        /// <returns>结束节点</returns>
        public Activity GetEndActivity()
        {
            var activity = ProcessModelHelper.GetEndActivity(this.Process);
            return activity;
        }

        /// <summary>
        /// 获取合并节点的前置分支连线上的属性的强制分支数目
        /// </summary>
        /// <param name="gatewayActivity">合并节点</param>
        /// <param name="forcedTransitionList">强制合并分支</param>
        /// <returns>强制分支数目</returns>
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
        /// 获取连线实体
        /// </summary>
        /// <param name="fromActivityGUID">起始节点GUID</param>
        /// <param name="toActivityGUID">到达节点GUID</param>
        /// <returns>转移实体</returns>
        public Transition GetForwardTransition(string fromActivityGUID, string toActivityGUID)
        {
            var transition = ProcessModelHelper.GetForwardTransition(this.Process, fromActivityGUID, toActivityGUID);
            return transition;
        }

        /// <summary>
        /// 获取当前节点的后续连线的集合
        /// </summary>
        /// <param name="fromActivityGUID">起始节点ID</param>
        /// <returns>转移节点列表</returns>
        public IList<Transition> GetForwardTransitionList(string fromActivityGUID)
        {
            var transitionList = ProcessModelHelper.GetForwardTransitionList(this.Process, fromActivityGUID);
            return transitionList;
        }

        /// <summary>
        /// 获取当前节点的后续连线的集合（使用条件过滤）
        /// </summary>
        /// <param name="fromActivityGUID">起始节点ID</param>
        /// <param name="conditionKeyValuePair">条件</param>
        /// <returns>转移实体列表</returns>
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
        /// 根据XML上的转移获取来源节点列表
        /// </summary>
        /// <param name="toActivityGUID">目标节点GUID</param>
        /// <returns>来源节点列表</returns>
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
        /// 获取当前节点的下一个节点信息
        /// </summary>
        /// <param name="activityGUID">活动GUID</param>
        /// <returns>节点实体</returns>
        public Activity GetNextActivity(string activityGUID)
        {
            var nextActivity = ProcessModelHelper.GetNextActivity(this.Process, activityGUID);
            return nextActivity;
        }

        #region 流程流转解析，处理流程下一步流转条件等规则
        /// <summary>
        /// 获取下一步活动节点树，供流转界面使用
        /// </summary>
        /// <param name="startActivity">起始节点</param>
        /// <param name="conditions">条件参数</param>
        /// <returns>下一步列表</returns>
        public IList<NodeView> GetFirstActivityTree(Activity startActivity, IDictionary<string, string> conditions)
        {
            using (var session = SessionFactory.CreateSession())
            {
                var nextResult = GetFirstActivityTree(startActivity, conditions, session);
                return nextResult.StepList;
            }
        }

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
                        Participants = GetActivityParticipants(child.Activity.ActivityGUID),
                        ReceiverType = child.Transition.Receiver != null ? child.Transition.Receiver.ReceiverType
                        : ReceiverTypeEnum.Default
                    });
                }
            }
            firstTreeResult.StepList = treeNodeList;
            return firstTreeResult;
        }

        /// <summary>
        /// 获取下一步节点列表，伴随运行时条件信息
        /// </summary>
        /// <param name="startEntity">开始节点</param>
        /// <param name="conditionKeyValuePair">条件对</param>
        /// <param name="session">会话</param>
        /// <returns>下一步匹配结果</returns>
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
                    NextActivityComponent child = null;
                    foreach (Transition transition in transitionList)
                    {
                        if (XPDLHelper.IsSimpleComponentNode(transition.ToActivity.ActivityType) == true)        //可流转简单类型节点 || 子流程节点
                        {
                            child = NextActivityComponentFactory.CreateNextActivityComponent(transition, transition.ToActivity);
                        }
                        else if (XPDLHelper.IsGatewayComponentNode(transition.ToActivity.ActivityType) == true)
                        {
                            NextActivityScheduleBase activitySchedule = NextActivityScheduleFactory.CreateActivitySchedule(this as IProcessModel,
                                transition.ToActivity.GatewayDetail.SplitJoinType,
                                null);

                            //获取网关后面的节点
                            child = activitySchedule.GetNextActivityListFromGateway(transition,
                                transition.ToActivity,
                                conditionKeyValuePair,
                                session,
                                out resultType);
                        }
                        else if (XPDLHelper.IsCrossOverComponentNode(transition.ToActivity.ActivityType) == true)
                        {
                            //事件类型的特殊节点处理，跟网关类似
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
        /// 获取下一步活动节点树，供流转界面使用
        /// </summary>
        /// <param name="currentActivityGUID">活动GUID</param>
        /// <returns>下一步列表</returns>
        public IList<NodeView> GetNextActivityTree(string currentActivityGUID)
        {
            using (var session = SessionFactory.CreateSession())
            {
                var nextResult = GetNextActivityTree(currentActivityGUID, null, null, session);
                return nextResult.StepList;
            }
        }

        /// <summary>
        /// 获取下一步活动节点树，供流转界面使用
        /// </summary>
        /// <param name="currentActivityGUID">活动GUID</param>
        /// <param name="conditions">条件参数</param>
        /// <returns>下一步列表</returns>
        public IList<NodeView> GetNextActivityTree(string currentActivityGUID, IDictionary<string, string> conditions)
        {
            using (var session = SessionFactory.CreateSession())
            {
                var nextResult = GetNextActivityTree(currentActivityGUID, null, conditions, session);
                return nextResult.StepList;
            }
        }
        /// <summary>
        /// 获取下一步活动节点树，供流转界面使用
        /// </summary>
        /// <param name="currentActivityGUID">活动GUID</param>
        /// <param name="taskID">任务ID</param>
        /// <param name="conditions">条件</param>
        /// <param name="session">会话</param>
        /// <returns>下一步列表</returns>
        public NextActivityTreeResult GetNextActivityTree(string currentActivityGUID,
            Nullable<int> taskID,
            IDictionary<string, string> conditions,
            IDbSession session)
        {
            var nextTreeResult = new NextActivityTreeResult();
            var treeNodeList = new List<NodeView>();
            var currentActivity = GetActivity(currentActivityGUID);

            //判断有没有指定的跳转节点信息
            if (currentActivity.ActivityTypeDetail != null 
                && currentActivity.SkipDetail != null
                && currentActivity.SkipDetail.IsSkip == true)
            {
                //获取跳转节点信息
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
                    Participants = GetActivityParticipants(skiptoActivity.ActivityGUID),
                    IsSkipTo = true
                });
            }
            else
            {
                //Transiton方式的流转定义
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
                            Participants = GetActivityParticipants(child.Activity.ActivityGUID),
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
        /// 迭代遍历
        /// </summary>
        /// <param name="root">根节点</param>
        /// <param name="treeNodeList">树节点信息</param>
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
        /// 获取下一步节点列表，伴随运行时条件信息
        /// </summary>
        /// <param name="currentActivityGUID">当前节点GUID</param>
        /// <param name="taskID">任务ID</param>
        /// <param name="conditionKeyValuePair">条件对</param>
        /// <param name="session">会话</param>
        /// <returns>下一步匹配结果</returns>
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
                NextActivityComponent root = NextActivityComponentFactory.CreateNextActivityComponent();

                //开始正常情况下的路径查找
                List<Transition> transitionList = GetForwardTransitionList(currentActivityGUID,
                    conditionKeyValuePair).ToList();

                if (transitionList.Count > 0)
                {
                    //遍历连线，获取下一步节点的列表
                    NextActivityComponent child = null;
                    foreach (Transition transition in transitionList)
                    {
                        if (XPDLHelper.IsSimpleComponentNode(transition.ToActivity.ActivityType) == true)        //可流转简单类型节点 || 子流程节点
                        {
                            child = NextActivityComponentFactory.CreateNextActivityComponent(transition, transition.ToActivity);
                        }
                        else if (XPDLHelper.IsGatewayComponentNode(transition.ToActivity.ActivityType) == true)
                        {
                            NextActivityScheduleBase activitySchedule = NextActivityScheduleFactory.CreateActivitySchedule(this as IProcessModel,
                                transition.ToActivity.GatewayDetail.SplitJoinType,
                                taskID);

                            //获取网关后面的节点
                            child = activitySchedule.GetNextActivityListFromGateway(transition,
                                transition.ToActivity,
                                conditionKeyValuePair,
                                session,
                                out resultType);
                        }
                        else if (XPDLHelper.IsCrossOverComponentNode(transition.ToActivity.ActivityType) == true)
                        {
                            //事件类型的特殊节点处理，跟网关类似
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
        /// 获取下一步节点列表（伴随条件和资源）
        /// </summary>
        /// <param name="currentActivityGUID">当前节点GUID</param>
        /// <param name="taskID">任务ID</param>
        /// <param name="conditionKeyValuePair">条件对</param>
        /// <param name="activityResource">活动资源</param>
        /// <param name="expression">表达式</param>
        /// <param name="session">数据会话</param>
        /// <returns>下一步匹配结果</returns>
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

            //先获取未加运行时表达式(为了过滤前端用户选择的步骤)的下一步节点列表
            //定义时的条件变量需要传入，返回的是解析后的下一步活动列表
            NextActivityMatchedResult result = GetNextActivityList(currentActivityGUID,
                taskID,
                conditionKeyValuePair,
                session);

            //下一步节点列表为空
            if (result.Root.HasChildren == false)
            {
                newResult = NextActivityMatchedResult.CreateNextActivityMatchedResultObject(NextActivityMatchedType.NoneTransitionFilteredByCondition,
                    result.Root);
                return newResult;
            }

            //下一步节点列表封装
            if (result.Root is NextActivityRouter)
            {
                var router = (NextActivityRouter)result.Root;
                var routerNode = router.NextActivityList[0];

                //并行与分支(多实例)--不需要判断条件，直接返回下一步列表
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
        /// 递归获取满足条件的下一步节点列表
        /// </summary>
        /// <param name="parent">父节点</param>
        /// <param name="activityResource">活动资源</param>
        /// <param name="expression">表达式</param>
        /// <returns>下一步节点</returns>
        private NextActivityComponent GetNextActivityListByExpressionRecurisivly(NextActivityComponent parent,
           ActivityResource activityResource,
           Expression<Func<ActivityResource, Activity, bool>> expression)
        {
            //创建新父节点
            NextActivityComponent r1 = NextActivityComponentFactory.CreateNextActivityComponent(parent);

            //递归遍历
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
        /// 添加子节点到网关节点
        /// </summary>
        /// <param name="newParent">新父节点</param>
        /// <param name="parent">父节点</param>
        /// <param name="child">子节点</param>
        /// <returns>下一步活动节点</returns>
        private NextActivityComponent AddChildToNewGatewayComponent(NextActivityComponent newParent,
            NextActivityComponent parent,
            NextActivityComponent child)
        {
            if ((newParent != null) && (child != null))
                newParent.Add(child);
            return newParent;
        }

        /// <summary>
        /// 判断是否是内部定义的执行者类型
        /// </summary>
        /// <param name="performerType">执行者类型</param>
        /// <returns>是否</returns>
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
        /// 不加条件获取下一步节点列表
        /// </summary>
        /// <param name="activityGUID">当前节点GUID</param>
        /// <returns>节点列表</returns>
        public IList<Activity> GetNextActivityListWithoutCondition(string activityGUID)
        {
            IList<Activity> activityList = new List<Activity>();
            GetNextActivityListWithoutConditionRecurily(activityList, activityGUID);

            return activityList;
        }

        /// <summary>
        /// 递归获取下一步节点列表
        /// </summary>
        /// <param name="activityList">节点列表</param>
        /// <param name="activityGUID">当前节点GUID</param>
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
        /// 获取下一步节点列表，伴随运行时条件信息
        /// （不包含多实例节点内部的判断，因为没有相应的Transition记录）
        /// </summary>
        /// <param name="currentActivityGUID">当前节点信息</param>
        /// <returns>活动列表</returns>
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
        /// 获取前置节点列表（不包含多实例节点内部的判断，因为没有相应的Transition记录）
        /// </summary>
        /// <param name="currentActivityGUID">当前运行节点GUID</param>
        /// <param name="hasGatewayPassed">是否经过网关</param>
        /// <returns>前置节点列表</returns>
        public IList<Activity> GetPreviousActivityList(string currentActivityGUID, out bool hasGatewayPassed)
        {
            var hasGatewayPassedInternal = false;
            IList<Activity> previousActivityList = new List<Activity>();
            GetPreviousActivityList(currentActivityGUID, previousActivityList, ref hasGatewayPassedInternal);
            hasGatewayPassed = hasGatewayPassedInternal;

            return previousActivityList;
        }

        /// <summary>
        /// 获取前置节点列表（不包含多实例节点内部的判断，因为没有相应的Transition记录）
        /// </summary>
        /// <param name="currentActivityGUID">当前运行节点GUID</param>
        /// <param name="previousActivityList">前置节点列表</param>
        /// <param name="hasGatewayPassed">是否经过网关</param>
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
        /// 获取上一步
        /// </summary>
        /// <param name="currentActivityGUID">当前节点列表</param>
        /// <returns>步骤列表</returns>
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

        #region 获取节点上的角色信息
        /// <summary>
        /// 根据活动定义获取办理人员列表
        /// </summary>
        /// <param name="activityGUID">活动GUID</param>
        /// <returns>下一步办理人员列表</returns>
        public IDictionary<string, PerformerList> GetActivityPerformers(string activityGUID)
        {
            var roleList = GetActivityRoles(activityGUID);
            var performers = ActivityResource.CreateNextActivityPerformers(activityGUID, roleList);

            return performers;
        }

        /// <summary>
        /// 根据活动定义获取办理人员列表
        /// </summary>
        /// <param name="nextActivityTree">活动列表</param>
        /// <returns>下一步办理人员列表</returns>
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
        /// 获取节点上定义的角色code集合
        /// </summary>
        /// <param name="activityGUID">活动GUID</param>
        /// <returns>角色列表</returns>
        public IList<Role> GetActivityRoles(string activityGUID)
        {
            IList<Role> roles = new List<Role>();
            var activity = ProcessModelHelper.GetActivity(this.Process, activityGUID);
            var participantList = activity.ParticipantList;
            if (participantList != null)
            {
                foreach (var participant in participantList)
                {
                    if (participant.OuterType == ParticipantTypeEnum.Role.ToString())
                    {
                        var role = new Role { ID = participant.OuterID, RoleCode = participant.OuterCode, RoleName = participant.Name };
                        roles.Add(role);
                    }
                }
            }
            return roles;
        }

        /// <summary>
        /// 获取节点上定义的角色及人员集合
        /// </summary>
        /// <param name="activityGUID">活动GUID</param>
        /// <returns>参与者列表</returns>
        internal IList<Participant> GetActivityParticipants(string activityGUID)
        {
            var activity = ProcessModelHelper.GetActivity(this.Process, activityGUID);
            return activity.ParticipantList;
        }
        #endregion

        /// <summary>
        /// 获取流程下的角色列表
        /// </summary>
        /// <returns></returns>
        public IList<Role> GetRoles()
        {
            List<Role> roles = new List<Role>();
            var activityList = GetAllTaskActivityList();
            foreach (var activity in activityList)
            {
                var participantList = activity.ParticipantList;
                foreach (var participant in participantList)
                {
                    if (participant.OuterType == ParticipantTypeEnum.Role.ToString())
                    {
                        if (roles.Find(role => role.ID == participant.OuterID) == null)
                        {
                            var role = new Role { ID = participant.OuterID, RoleCode = participant.OuterCode, RoleName = participant.Name };
                            roles.Add(role);
                        }
                    }
                }
            }
            return roles;
        }


        /// <summary>
        /// 根据流程GUID读取单一流程记录
        /// </summary>
        /// <param name="xmlContent">XML内容</param>
        /// <param name="processGUID">流程实体</param>
        /// <returns>XML文档对象</returns>
        private XmlDocument GetXmlDocumentByProcess(string xmlContent, string processGUID)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlContent);
            var processNodeList = xmlDoc.DocumentElement.SelectNodes(XPDLDefinition.BPMN2_ElementName_Process,
                XPDLHelper.GetBPMN2XmlNamespaceManager(xmlDoc));

            return xmlDoc;
        }

        #region 解析条件表达式
        /// <summary>
        /// 用LINQ解析条件表达式
        /// </summary>
        /// <param name="transition">转移</param>
        /// <param name="conditionKeyValuePair">条件参数</param>
        /// <returns>解析结果</returns>
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
        /// 是否是满足条件的Transition，如果条件为空，默认是有效的。
        /// </summary>
        /// <param name="transition">转移</param>
        /// <param name="conditionKeyValuePair">条件对</param>
        /// <returns>解析结果</returns>
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
                isValid = true;
            }
            return isValid;
        }
        #endregion

        #region 节点类型判断
        /// <summary>
        /// 是否并行会签节点
        /// </summary>
        /// <param name="activity">节点</param>
        /// <returns>是否并行</returns>
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
        /// 是否串行会签节点
        /// </summary>
        /// <param name="activity">节点</param>
        /// <returns>是否串行</returns>
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
        /// 判断是否会签节点
        /// </summary>
        /// <param name="activity">节点</param>
        /// <returns>是否会签</returns>
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
        /// 是否任务节点
        /// </summary>
        /// <param name="activity">节点</param>
        /// <returns>是否任务</returns>
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
        /// 是否任务节点
        /// </summary>
        /// <param name="activityInstance">节点</param>
        /// <returns>是否任务</returns>
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
        /// 是否并行容器节点
        /// </summary>
        /// <param name="activity">节点</param>
        /// <returns>是否</returns>
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


        #region 退回节点处理
        /// <summary>
        /// 获取节点的前驱连线
        /// </summary>
        /// <param name="toActivityGUID">目标节点GUID</param>
        /// <returns>转移列表</returns>
        public IList<Transition> GetBackwardTransitionList(string toActivityGUID)
        {
            var transitionList = ProcessModelHelper.GetBackwardTransitionList(this.Process, toActivityGUID);
            return transitionList;
        }

        /// <summary>
        /// 获取与合并节点相对应的分支节点
        /// </summary>
        /// <param name="fromActivity">合并节点</param>
        /// <param name="joinCount">经过的合并节点个数</param>
        /// <param name="splitCount">经过的分支节点个数</param>
        /// <returns>对应的分支节点</returns>
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
        /// 获取节点的前驱节点列表(Lambda表达式)
        /// </summary>
        /// <param name="activityGUID">活动GUID</param>
        /// <param name="expression">表达式</param>
        /// <returns>转移列表</returns>
        internal IList<Transition> GetBackwardTransitionList(string activityGUID,
            Expression<Func<Transition, bool>> expression)
        {
            IList<Transition> transitionList = GetBackwardTransitionList(activityGUID);
            return GetBackwardTransitionList(activityGUID, expression);
        }

        /// <summary>
        /// 获取节点的前驱节点列表(Lambda表达式)
        /// </summary>
        /// <param name="transitionList">转移列表</param>
        /// <param name="expression">表达式</param>
        /// <returns>转移实体列表</returns>
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
        /// 根据流程定义文件，获取带有条件的节点前驱连线列表，（带有条件，可以用Lambda表达式重构）
        /// </summary>
        /// <param name="toActivityGUID">目标节点GUID</param>
        /// <returns>转移列表</returns>
        internal IList<Transition> GetBackworkTransitionListWithCondition(string toActivityGUID)
        {
            return GetBackwardTransitionList(toActivityGUID,
                (t => t.Condition != null && !string.IsNullOrEmpty(t.Condition.ConditionText)));
        }

        /// <summary>
        /// 获取节点前驱连线的数目
        /// </summary>
        /// <param name="toActivityGUID">目标节点GUID</param>
        /// <returns>个数</returns>
        public int GetBackwardTransitionListCount(string toActivityGUID)
        {
            IList<Transition> backwardList = GetBackwardTransitionList(toActivityGUID);
            return backwardList.Count;
        }
        #endregion
    }
}
