/*
* Slickflow 工作流引擎遵循LGPL协议，也可联系作者商业授权并获取技术支持；
* 除此之外的使用则视为不正当使用，请您务必避免由此带来的商业版权纠纷。
* 
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
using System.Text;
using System.Diagnostics;
using Slickflow.Module.Resource;
using Slickflow.Engine.Utility;
using Slickflow.Engine.Common;
using Slickflow.Engine.Storage;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Xpdl.Node;
using Slickflow.Engine.Xpdl.Schedule;

namespace Slickflow.Engine.Xpdl
{
    /// <summary>
    /// 流程定义模型类
    /// </summary>
    public class ProcessModel : IProcessModel
    {
        #region 属性和构造函数
        /// <summary>
        /// 流程定义实体
        /// </summary>
        public ProcessEntity ProcessEntity { get; set; }

        /// <summary>
        /// 流程xml文档
        /// </summary>
        public XmlDocument XmlProcessDefinition
        {
            get
            {
                if (MemoryCachedHelper.GetXpdlCache(ProcessEntity.ProcessGUID, ProcessEntity.Version) == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(ProcessEntity.XmlContent);

                    MemoryCachedHelper.SetXpdlCache(ProcessEntity.ProcessGUID, ProcessEntity.Version, xmlDoc);
                }
                return MemoryCachedHelper.GetXpdlCache(ProcessEntity.ProcessGUID, ProcessEntity.Version);
            }
        }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="entity">流程实体</param>
        public ProcessModel(ProcessEntity entity)
        {
            ProcessEntity = entity;
        }
        #endregion

        #region 流程定义数据获取
        /// <summary>
        /// 获取角色列表
        /// </summary> 
        /// <returns>角色列表</returns>
        public IList<Role> GetRoles()
        {
            IList<Role> roleList = new List<Role>();
            XmlNodeList participantNodeList = XMLHelper.GetXmlNodeListByXpath(this.XmlProcessDefinition,
                string.Format("{0}[@type='Role']", XPDLDefinition.StrXmlSingleParticipantPath));

            //转为角色实体
            foreach (XmlNode node in participantNodeList)
            {
                var role = new Role
                {
                    ID = XMLHelper.GetXmlAttribute(node, "outerId"),
                    RoleCode = XMLHelper.GetXmlAttribute(node, "code"),
                    RoleName = XMLHelper.GetXmlAttribute(node, "name")
                };
                roleList.Add(role);
            }
            return roleList;
        }
        #endregion

        #region 活动节点基本方法和流转规则处理

        #region 活动节点基本方法
        /// <summary>
        /// 获取开始节点信息
        /// </summary>
        /// <returns>节点实体</returns>
        public ActivityEntity GetStartActivity()
        {
            string nodeType = "StartNode";

            XmlNode startTypeNode = GetXmlActivityTypeNodeFromXmlFile(nodeType);
            if (startTypeNode != null)
                return ConvertXmlActivityNodeToActivityEntity(startTypeNode.ParentNode);
            throw new Exception("未检测到流程配置节点信息，请先设计流程后在进行办理！");
        }

        /// <summary>
        /// 获取结束节点
        /// </summary>
        /// <returns>节点实体</returns>
        public ActivityEntity GetEndActivity()
        {
            string nodeType = "EndNode";
            XmlNode endTypeNode = GetXmlActivityTypeNodeFromXmlFile(nodeType);
            return ConvertXmlActivityNodeToActivityEntity(endTypeNode.ParentNode);
        }

        /// <summary>
        /// 获取任务类型的节点
        /// </summary>
        /// <returns>节点实体列表</returns>
        public IList<ActivityEntity> GetTaskActivityList()
        {
            string nodeType = "TaskNode";
            XmlNodeList nodeList = GetXmlActivityListByTypeFromXmlFile(nodeType);

            List<ActivityEntity> activityList = new List<ActivityEntity>();
            ActivityEntity entity = null;

            foreach (XmlNode node in nodeList)
            {
                entity = ConvertXmlActivityNodeToActivityEntity(node.ParentNode);
                activityList.Add(entity);
            }

            return activityList;
        }

        /// <summary>
        /// 获取任务类型的节点(包含会签节点和子流程节点)，按照Transition顺序组成序列
        /// </summary>
        /// <returns>节点实体列表</returns>
        public IList<ActivityEntity> GetAllTaskActivityList()
        {
            List<ActivityEntity> activityList = new List<ActivityEntity>();
            var startNode = GetStartActivity();
            var startActivityGUID = startNode.ActivityGUID;

            return TranverseTransitionList(activityList, startActivityGUID);
        }

       
        /// <summary>
        /// 递归遍历转移上的前置节点
        /// </summary>
        /// <param name="activityList">活动列表</param>
        /// <param name="fromActivityGUID">起始活动GUID</param>
        /// <returns>节点实体列表</returns>
        private IList<ActivityEntity> TranverseTransitionList(List<ActivityEntity> activityList, string fromActivityGUID)
        {
            ActivityEntity toActivity = null;
            var transitionNodeList = GetForwardXmlTransitionNodeList(fromActivityGUID);
            foreach (XmlNode transition in transitionNodeList)
            {
                toActivity = GetActivityFromTransitionTo(transition);
                if (toActivity.ActivityType == ActivityTypeEnum.EndNode)
                {
                    break;
                }
                else if (toActivity.ActivityType == ActivityTypeEnum.TaskNode
                    || toActivity.ActivityType == ActivityTypeEnum.MultipleInstanceNode
                    || toActivity.ActivityType == ActivityTypeEnum.SubProcessNode)
                {
                    AppendActivity(activityList, toActivity);
                }
                //递归遍历转移数据
                TranverseTransitionList(activityList, toActivity.ActivityGUID);
            }

            return activityList;
        }

        /// <summary>
        /// 添加节点到节点列表中，去掉有重复的节点
        /// </summary>
        /// <param name="activityList">节点列表</param>
        /// <param name="toActivity">节点</param>
        private void AppendActivity(IList<ActivityEntity> activityList, ActivityEntity toActivity)
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
        /// 获取分支和合并之间的任务节点列表
        /// </summary>
        /// <param name="splitActivity">分支节点</param>
        /// <param name="joinActivity">合并节点</param>
        /// <returns>节点列表</returns>
        public IList<ActivityEntity> GetAllTaskActivityList(ActivityEntity splitActivity,
            ActivityEntity joinActivity)
        {
            IList<ActivityEntity> taskActivityList = new List<ActivityEntity>();
            return TranverseTransitionListBetweenSplitJoin(taskActivityList, splitActivity.ActivityGUID, joinActivity.ActivityGUID);
        }

        /// <summary>
        /// 递归遍历转移上的前置节点
        /// </summary>
        /// <param name="activityList">活动列表</param>
        /// <param name="fromActivityGUID">起始活动GUID</param>
        /// <param name="finalActivityGUID">截止活动GUID</param>
        /// <returns>节点实体列表</returns>
        private IList<ActivityEntity> TranverseTransitionListBetweenSplitJoin(IList<ActivityEntity> activityList, 
            string fromActivityGUID,
            string finalActivityGUID)
        {
            ActivityEntity toActivity = null;
            var transitionNodeList = GetForwardXmlTransitionNodeList(fromActivityGUID);
            foreach (XmlNode transition in transitionNodeList)
            {
                toActivity = GetActivityFromTransitionTo(transition);
                if (toActivity.ActivityType == ActivityTypeEnum.GatewayNode
                    && toActivity.ActivityGUID == finalActivityGUID)
                {
                    break;
                }
                else if (toActivity.ActivityType == ActivityTypeEnum.TaskNode
                    || toActivity.ActivityType == ActivityTypeEnum.MultipleInstanceNode
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
        /// 获取流程的第一个可办理节点
        /// </summary>
        /// <returns>节点实体</returns>
        public ActivityEntity GetFirstActivity()
        {
            ActivityEntity startActivity = GetStartActivity();
            ActivityEntity firstActivity = GetNextActivity(startActivity.ActivityGUID);
            return firstActivity;
        }

        /// <summary>
        /// 获取当前节点的下一个节点信息
        /// </summary>
        /// <param name="activityGUID">活动GUID</param>
        /// <returns>节点实体</returns>
        public ActivityEntity GetNextActivity(string activityGUID)
        {
            XmlNode transitionNode = GetForwardXmlTransitionNode(activityGUID);

            return GetActivityFromTransitionTo(transitionNode);
        }

        /// <summary>
        /// 获取流程起始的活动节点列表(开始节点之后，可能有多个节点)
        /// </summary>
        /// <param name="conditionKeyValuePair">条件表达式的参数名称-参数值的集合</param>
        /// <returns></returns>
        public NextActivityMatchedResult GetFirstActivityList(IDictionary<string, string> conditionKeyValuePair)
        {
            try
            {
                ActivityEntity startActivity = GetStartActivity();
                return GetNextActivityList(startActivity.ActivityGUID, conditionKeyValuePair);
            }
            catch (System.Exception e)
            {
                throw new WfXpdlException(string.Format("解析流程定义文件发生异常，异常描述：{0}", e.Message), e);
            }
        }


        /// <summary>
        /// 获取合并节点的前置分支连线上的属性的强制分支数目
        /// </summary>
        /// <param name="gatewayActivity">合并节点</param>
        /// <param name="forcedTransitionList">强制合并分支</param>
        /// <returns>强制分支数目</returns>
        public int GetForcedBranchesCountBeforeEOrJoin(ActivityEntity gatewayActivity, out IList<TransitionEntity> forcedTransitionList)
        {
            var tokensCount = 0;
            forcedTransitionList = new List<TransitionEntity>();
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
        #endregion

        #region 流程流转解析，处理流程下一步流转条件等规则
        /// <summary>
        /// 获取下一步活动节点树，供流转界面使用
        /// </summary>
        /// <param name="currentActivityGUID">活动GUID</param>
        /// <param name="condition">条件</param>
        /// <returns>下一步列表</returns>
        public IList<NodeView> GetNextActivityTree(string currentActivityGUID, 
            IDictionary<string, string> condition = null)
        {
            var treeNodeList = new List<NodeView>();
            var activity = GetActivity(currentActivityGUID);

            //判断有没有指定的跳转节点信息
            if (activity.ActivityTypeDetail.SkipInfo != null 
                && activity.ActivityTypeDetail.SkipInfo.IsSkip == true)
            {
                //获取跳转节点信息
                var skipto = activity.ActivityTypeDetail.SkipInfo.Skipto;
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
                var nextSteps = GetNextActivityList(activity.ActivityGUID, condition);

                foreach (var child in nextSteps.Root)
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
            return treeNodeList;
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
        /// <param name="conditionKeyValuePair">条件对</param>
        /// <returns>下一步匹配结果</returns>
        public NextActivityMatchedResult GetNextActivityList(string currentActivityGUID,
            IDictionary<string, string> conditionKeyValuePair = null)
        {
            try
            {
                NextActivityMatchedResult result = null;
                NextActivityMatchedType resultType = NextActivityMatchedType.Unknown;

                //创建“下一步节点”的根节点
                NextActivityComponent root = NextActivityComponentFactory.CreateNextActivityComponent();
                NextActivityComponent child = null;
                List<TransitionEntity> transitionList = GetForwardTransitionList(currentActivityGUID,
                    conditionKeyValuePair).ToList();

                if (transitionList.Count > 0)
                {
                    //遍历连线，获取下一步节点的列表
                    foreach (TransitionEntity transition in transitionList)
                    {
                        if (XPDLHelper.IsSimpleComponentNode(transition.ToActivity.ActivityType) == true
                            || transition.ToActivity.ActivityType == ActivityTypeEnum.SubProcessNode)        //可流转简单类型节点 || 子流程节点
                        {
                            child = NextActivityComponentFactory.CreateNextActivityComponent(transition, transition.ToActivity);
                        }
                        else if (XPDLHelper.IsGatewayComponentNode(transition.ToActivity.ActivityType) == true)
                        {
                            NextActivityScheduleBase activitySchedule = NextActivityScheduleFactory.CreateActivitySchedule(this as IProcessModel,
                                transition.ToActivity.GatewaySplitJoinType);

                            child = activitySchedule.GetNextActivityListFromGateway(transition,
                                transition.ToActivity,
                                conditionKeyValuePair,
                                out resultType);
                        }
                        else if (XPDLHelper.IsIntermediateEventComponentNode(transition.ToActivity.ActivityType) == true)
                        {
                            NextActivityScheduleBase activitySchedule = NextActivityScheduleFactory.CreateActivityScheduleIntermediate(this as IProcessModel);
                            child = activitySchedule.GetNextActivityListFromGateway(transition,
                                transition.ToActivity,
                                conditionKeyValuePair,
                                out resultType);
                        }
                        else
                        {
                            var errMsg = string.Format("未知的节点类型：{0}", transition.ToActivity.ActivityType.ToString());
                            LogManager.RecordLog("流程获取下一步发生异常",
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
                LogManager.RecordLog("流程获取下一步发生异常",
                    LogEventType.Exception,
                    LogPriority.Normal,
                    null,
                    new WfXpdlException(e.Message));
                throw new WfXpdlException(string.Format("解析流程定义文件发生异常，异常描述：{0}", e.Message), e);
            }
        }

        /// <summary>
        /// 获取下一步节点列表（伴随条件和资源）
        /// </summary>
        /// <param name="currentActivityGUID">当前节点GUID</param>
        /// <param name="conditionKeyValuePair">条件对</param>
        /// <param name="activityResource">活动资源</param>
        /// <param name="expression">表达式</param>
        /// <returns>下一步匹配结果</returns>
        public NextActivityMatchedResult GetNextActivityList(string currentActivityGUID,
            IDictionary<string, string> conditionKeyValuePair,
            ActivityResource activityResource,
            Expression<Func<ActivityResource, ActivityEntity, bool>> expression)
        {
            #region AndSplit Multiple Instance
            Boolean isNotConditionCheck = false;
            NextActivityMatchedResult newResult = null;
            NextActivityComponent newRoot = NextActivityComponentFactory.CreateNextActivityComponent();

            //先获取未加运行时表达式(为了过滤前端用户选择的步骤)的下一步节点列表
            //定义时的条件变量需要传入，返回的是解析后的下一步活动列表
            NextActivityMatchedResult result = GetNextActivityList(currentActivityGUID,
                conditionKeyValuePair);

            //下一步节点列表为空
            if (result.Root.HasChildren == false)
            {
                newResult = NextActivityMatchedResult.CreateNextActivityMatchedResultObject(NextActivityMatchedType.NoneTransitionFilteredByCondition,
                    result.Root);
                return newResult;
            }

            //下一步节点列表封装
            if (result.Root is NextActivityGateway)
            {
                var gateway = (NextActivityGateway)result.Root;
                var gatewayNode = gateway.NextActivityList[0];

                //并行与分支(多实例)--不需要判断条件，直接返回下一步列表
                if (gatewayNode.Activity.GatewaySplitJoinType == GatewaySplitJoinTypeEnum.Split
                    && gatewayNode.Activity.GatewayDirectionType == GatewayDirectionEnum.AndSplitMI)
                {
                    newRoot = result.Root;
                    isNotConditionCheck = true;
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
                            throw new ApplicationException("未能正确获取下一步的节点信息！");
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
           Expression<Func<ActivityResource, ActivityEntity, bool>> expression)
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
        public IList<ActivityEntity> GetNextActivityListWithoutCondition(string activityGUID)
        {
            IList<ActivityEntity> activityList = new List<ActivityEntity>();
            GetNextActivityListWithoutConditionRecurily(activityList, activityGUID);

            return activityList;
        }

        /// <summary>
        /// 递归获取下一步节点列表
        /// </summary>
        /// <param name="activityList">节点列表</param>
        /// <param name="activityGUID">当前节点GUID</param>
        private void GetNextActivityListWithoutConditionRecurily(IList<ActivityEntity> activityList, 
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
        #endregion

        #region 流程流转解析，处理流程上一步退回步骤列表
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

        /// <summary>
        /// 获取下一步节点列表，伴随运行时条件信息
        /// （不包含多实例节点内部的判断，因为没有相应的Transition记录）
        /// </summary>
        /// <param name="currentActivityGUID">当前节点信息</param>
        /// <returns>活动列表</returns>
        public IList<ActivityEntity> GetPreviousActivityList(string currentActivityGUID)
        {
            var activityList = new List<ActivityEntity>();
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
                    throw new XmlDefinitionException(string.Format("未知的节点类型：{0}", transition.FromActivity.ActivityType.ToString()));
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
        public IList<ActivityEntity> GetPreviousActivityList(string currentActivityGUID, out bool hasGatewayPassed)
        {
            var hasGatewayPassedInternal = false;
            IList<ActivityEntity> previousActivityList = new List<ActivityEntity>();
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
            IList<ActivityEntity> previousActivityList, 
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
        /// 根据XML上的转移获取来源节点列表
        /// </summary>
        /// <param name="toActivityGUID">目标节点GUID</param>
        /// <returns>来源节点列表</returns>
        public IList<ActivityEntity> GetFromActivityList(string toActivityGUID)
        {
            var activityList = new List<ActivityEntity>();
            var transitionList = GetBackwardTransitionList(toActivityGUID);
            foreach (var transition in transitionList)
            {
                activityList.Add(transition.FromActivity);
            }
            return activityList;
        }
        #endregion

        #endregion


        #region Xml活动节点读取操作
        /// <summary>
        /// 获取XML的节点信息
        /// </summary>
        /// <param name="activityGUID">节点GUID</param>
        /// <returns>Xml节点</returns>
        private XmlNode GetXmlActivityNodeFromXmlFile(string activityGUID)
        {
            XmlNode xmlNode = XMLHelper.GetXmlNodeByXpath(XmlProcessDefinition,
                    string.Format("{0}[@id='" + activityGUID + "']", XPDLDefinition.StrXmlActivityPath));
            return xmlNode;
        }

        /// <summary>
        /// 获取活动节点的类型信息
        /// </summary>
        /// <param name="nodeType">节点类型</param>
        /// <returns>Xml节点</returns>
        private XmlNode GetXmlActivityTypeNodeFromXmlFile(string nodeType)
        {
            XmlNode typeNode = XMLHelper.GetXmlNodeByXpath(XmlProcessDefinition,
                string.Format("{0}/ActivityType[@type='" + nodeType + "']", XPDLDefinition.StrXmlActivityPath));
            return typeNode;
        }

        /// <summary>
        /// 获取特定类型的活动节点
        /// </summary>
        /// <param name="nodeType">节点类型</param>
        /// <returns>Xml节点列表</returns>
        private XmlNodeList GetXmlActivityListByTypeFromXmlFile(string nodeType)
        {
            XmlNodeList nodeList = XMLHelper.GetXmlNodeListByXpath(XmlProcessDefinition,
                string.Format("{0}/ActivityType[@type='" + nodeType + "']", XPDLDefinition.StrXmlActivityPath));
            return nodeList;
        }

        /// <summary>
        /// 获取参与者信息
        /// </summary>
        /// <param name="participantGUID">参与者GUID</param>
        /// <returns>XML节点</returns>
        private XmlNode GetXmlParticipantNodeFromXmlFile(string participantGUID)
        {
            XmlNode participantNode = XMLHelper.GetXmlNodeByXpath(XmlProcessDefinition,
                string.Format("{0}[@id='" + participantGUID + "']", XPDLDefinition.StrXmlSingleParticipantPath));
            return participantNode;
        }

        /// <summary>
        /// 获取当前节点信息
        /// </summary>
        /// <returns>活动实体</returns>
        public ActivityEntity GetActivity(string activityGUID)
        {
            XmlNode activityNode = GetXmlActivityNodeFromXmlFile(activityGUID);

            ActivityEntity entity = ConvertXmlActivityNodeToActivityEntity(activityNode);
            return entity;
        }

        /// <summary>
        /// 获取转移上的To节点的对象
        /// </summary>
        /// <param name="transitionNode">转移的xml节点</param>
        /// <returns>活动实体</returns>
        private ActivityEntity GetActivityFromTransitionTo(XmlNode transitionNode)
        {
            string nextActivityGuid = XMLHelper.GetXmlAttribute(transitionNode, "to");
            XmlNode activityNode = GetXmlActivityNodeFromXmlFile(nextActivityGuid);

            ActivityEntity entity = ConvertXmlActivityNodeToActivityEntity(activityNode);
            return entity;
        }
        #endregion
        
        #region 获取节点上的角色信息
        /// <summary>
        /// 获取角色编码信息
        /// </summary>
        /// <param name="performerGUID">执行用户GUID</param>
        /// <returns>角色</returns>
        private Role GetRoleFromXmlFile(string performerGUID)
        {
            XmlNode performerNode = GetXmlParticipantNodeFromXmlFile(performerGUID);
            if (performerNode != null)
            {
                var roleCode = XMLHelper.GetXmlAttribute(performerNode, "code");
                var roleName = XMLHelper.GetXmlAttribute(performerNode, "name");
                string roleId = XMLHelper.GetXmlAttribute(performerNode, "outerId") ?? "0";
                var role = new Role { ID = roleId, RoleCode = roleCode, RoleName = roleName };
                return role;
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// 获取执行者信息
        /// </summary>
        /// <param name="performerGUID">执行用户GUID</param>
        /// <returns>参与者</returns>
        private Participant GetParticipantFromXmlFile(string performerGUID)
        {
            XmlNode participantNode = GetXmlParticipantNodeFromXmlFile(performerGUID);
            if (participantNode != null)
            {
                var id = XMLHelper.GetXmlAttribute(participantNode, "id");
                var type = XMLHelper.GetXmlAttribute(participantNode, "type");
                var code = XMLHelper.GetXmlAttribute(participantNode, "code");
                var name = XMLHelper.GetXmlAttribute(participantNode, "name");

                var outerId = XMLHelper.GetXmlAttribute(participantNode, "outerId");
                var participant = new Participant { ID = id, Type = type, Code = code, Name = name, OuterID = outerId };

                return participant;
            }
            else
            {
                return null;
            }
        }

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
        /// 获取节点上定义的角色code集合
        /// </summary>
        /// <param name="activityGUID">活动GUID</param>
        /// <returns>角色列表</returns>
        public IList<Role> GetActivityRoles(string activityGUID)
        {
            IList<Role> roles = new List<Role>();
            XmlNode xmlNode = GetXmlActivityNodeFromXmlFile(activityGUID);
            XmlNode performersNode = xmlNode.SelectSingleNode("Performers");
            
            if (performersNode != null)
            {
                foreach (XmlNode performer in performersNode.ChildNodes)
                {
                    string performerGUID = XMLHelper.GetXmlAttribute(performer, "id");  //xml 文件对应的角色id
                    Role role = GetRoleFromXmlFile(performerGUID);
                    if (role != null) roles.Add(role);
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
            IList<Participant> participants = new List<Participant>();
            XmlNode xmlNode = GetXmlActivityNodeFromXmlFile(activityGUID);
            XmlNode performersNode = xmlNode.SelectSingleNode("Performers");
            if (performersNode != null)
            {
                foreach (XmlNode performer in performersNode.ChildNodes)
                {
                    string performerGUID = XMLHelper.GetXmlAttribute(performer, "id");
                    participants.Add(GetParticipantFromXmlFile(performerGUID));
                }
            }
            return participants;
        }
        #endregion

        #region 获取节点数据项信息
        /// <summary>
        /// 获取当前节点所要求的数据项，即数据项必须填写，才可以触发后续流程
        /// </summary>
        /// <param name="activityGUID"></param>
        /// <returns></returns>
        internal IList<string> GetActivityDataItemsRequired(Guid activityGUID)
        {
            XmlNode requiredNode = XMLHelper.GetXmlNodeByXpath(XmlProcessDefinition,
               string.Format("{0}/DataItemsRequired", XPDLDefinition.StrXmlActivityPath));

            IList<string> itemList = new List<string>();
            foreach (XmlNode dataItemNode in requiredNode.ChildNodes)
            {
                string dataItemID = XMLHelper.GetXmlAttribute(dataItemNode, "id");
                XmlNode srcDataItemNode = XMLHelper.GetXmlNodeByXpath(XmlProcessDefinition,
                    string.Format("{0}[@id='" + dataItemID + "']", XPDLDefinition.StrXmlSingleDataItems));

                string dataItemCode = XMLHelper.GetXmlAttribute(srcDataItemNode, "code");
                itemList.Add(dataItemCode);
            }
            return itemList;
        }
        #endregion

        #region Xml节点转换信息
        /// <summary>
        /// 把XML节点转换为ActivityEntity实体对象
        /// </summary>
        /// <param name="node">节点</param>
        /// <returns>活动实体</returns>
        internal ActivityEntity ConvertXmlActivityNodeToActivityEntity(XmlNode node)
        {
            ActivityEntity entity = new ActivityEntity();
            entity.ActivityGUID = XMLHelper.GetXmlAttribute(node, "id");
            entity.ActivityName = XMLHelper.GetXmlAttribute(node, "name");
            entity.ActivityCode = XMLHelper.GetXmlAttribute(node, "code");
            entity.ActivityUrl = XMLHelper.GetXmlAttribute(node, "url");
            entity.ProcessGUID = ProcessEntity.ProcessGUID;

            //描述信息
            XmlNode descNode = node.SelectSingleNode("Description");
            entity.Description = (descNode != null) ? descNode.InnerText : string.Empty;

            //节点类型信息
            XmlNode typeNode = node.SelectSingleNode("ActivityType");
            entity.ActivityType = (ActivityTypeEnum)Enum.Parse(typeof(ActivityTypeEnum), XMLHelper.GetXmlAttribute(typeNode, "type"));
            entity.ActivityTypeDetail = ConvertXmlNodeToActivityTypeDetail(typeNode);
            entity.WorkItemType = XPDLHelper.GetWorkItemType(entity.ActivityType);

            if (entity.ActivityType == ActivityTypeEnum.SubProcessNode)             //sub process node
            {
                //子流程节点
                var subProcessNode = new SubProcessNode(entity);
                subProcessNode.SubProcessGUID = XMLHelper.GetXmlAttribute(typeNode, "subId");
                entity.Node = subProcessNode;
            }
            else if (entity.ActivityType == ActivityTypeEnum.MultipleInstanceNode)      //multiple instance node
            {
                var multipleInstanceNode = new MultipleInstanceNode(entity);
                entity.Node = multipleInstanceNode;
            }
                     
            //获取节点的操作列表
            XmlNode actionsNode = node.SelectSingleNode("Actions");
            if (actionsNode != null)
            {
                XmlNodeList xmlActionList = actionsNode.ChildNodes;
                List<ActionEntity> actionList = new List<ActionEntity>();
                foreach (XmlNode element in xmlActionList)
                {
                    actionList.Add(ConvertXmlActionNodeToActionEntity(element));
                }
                entity.ActionList = actionList;
            }

            //获取节点边界列表
            XmlNode boundariesNode = node.SelectSingleNode("Boundaries");
            if (boundariesNode != null)
            {
                XmlNodeList xmlBoundaryList = boundariesNode.ChildNodes;
                List<BoundaryEntity> boundaryList = new List<BoundaryEntity>();
                foreach (XmlNode element in xmlBoundaryList)
                {
                    boundaryList.Add(ConvertXmlBoundaryNodeToBoundaryEntity(element));
                }
                entity.BoundaryList = boundaryList;
            }

            //获取节点自定义章节信息
            XmlNode sectionsNode = node.SelectSingleNode("Sections");
            if (sectionsNode != null)
            {
                XmlNodeList xmlSectionList = sectionsNode.ChildNodes;
                List<SectionEntity> sectionList = new List<SectionEntity>();
                foreach (XmlNode element in xmlSectionList)
                {
                    sectionList.Add(ConvertXmlSectionNodeToSectionEntity(element));
                }
                entity.SectionList = sectionList;
            }


            //节点的Split Join 类型
            string gatewaySplitJoinType = XMLHelper.GetXmlAttribute(typeNode, "gatewaySplitJoinType");
            if (!string.IsNullOrEmpty(gatewaySplitJoinType))
            {
                entity.GatewaySplitJoinType = (GatewaySplitJoinTypeEnum)Enum.Parse(typeof(GatewaySplitJoinTypeEnum), gatewaySplitJoinType);
            }

            string gatewayDirection = XMLHelper.GetXmlAttribute(typeNode, "gatewayDirection");
            //节点的路由信息
            if (!string.IsNullOrEmpty(gatewayDirection))
            {
                entity.GatewayDirectionType = (GatewayDirectionEnum)Enum.Parse(typeof(GatewayDirectionEnum), gatewayDirection);
            }

            string gatewayJoinPass = XMLHelper.GetXmlAttribute(typeNode, "gatewayJoinPass");
            if (!string.IsNullOrEmpty(gatewayJoinPass))
            {
                entity.GatewayJoinPassType = EnumHelper.TryParseEnum<GatewayJoinPassEnum>(gatewayJoinPass);
            }

            return entity;
        }

        /// <summary>
        /// 把Xml节点转换为ActivityTypeDetail 类（用于会签等复杂类型）
        /// </summary>
        /// <param name="typeNode">类型节点</param>
        /// <returns>活动类型详细</returns>
        private ActivityTypeDetail ConvertXmlNodeToActivityTypeDetail(XmlNode typeNode)
        {
            ActivityTypeDetail entity = new ActivityTypeDetail();
            entity.ActivityType = (ActivityTypeEnum)Enum.Parse(typeof(ActivityTypeEnum), XMLHelper.GetXmlAttribute(typeNode, "type"));

            if (!string.IsNullOrEmpty(XMLHelper.GetXmlAttribute(typeNode, "trigger")))
            {
                TriggerTypeEnum triggerType = TriggerTypeEnum.None;
                Enum.TryParse<TriggerTypeEnum>(XMLHelper.GetXmlAttribute(typeNode, "trigger"), out triggerType);
                entity.TriggerType = triggerType;
            }

            if (!string.IsNullOrEmpty(XMLHelper.GetXmlAttribute(typeNode, "complexType")))
            {
                entity.ComplexType = EnumHelper.ParseEnum<ComplexTypeEnum>(XMLHelper.GetXmlAttribute(typeNode, "complexType"));
            }

            if (!string.IsNullOrEmpty(XMLHelper.GetXmlAttribute(typeNode, "mergeType")))
            {
                entity.MergeType = EnumHelper.ParseEnum<MergeTypeEnum>(XMLHelper.GetXmlAttribute(typeNode, "mergeType"));
            }

            if (!string.IsNullOrEmpty(XMLHelper.GetXmlAttribute(typeNode, "compareType")))
            {
                entity.CompareType = EnumHelper.ParseEnum<CompareTypeEnum>(XMLHelper.GetXmlAttribute(typeNode, "compareType"));
            }

            if (!string.IsNullOrEmpty(XMLHelper.GetXmlAttribute(typeNode, "completeOrder")))
            {
                entity.CompleteOrder =  float.Parse(XMLHelper.GetXmlAttribute(typeNode, "completeOrder"));
            }

            if (!string.IsNullOrEmpty(XMLHelper.GetXmlAttribute(typeNode, "skip")))
            {
                var skip = Boolean.Parse(XMLHelper.GetXmlAttribute(typeNode, "skip"));
                var skipto = XMLHelper.GetXmlAttribute(typeNode, "to");

                if (skip)
                {
                    entity.SkipInfo = new SkipInfo { IsSkip = skip, Skipto = skipto };
                }
            }

            return entity;
        }

        /// <summary>
        /// 将Action的XML节点
        /// </summary>
        /// <param name="node">XML节点</param>
        /// <returns>操作实体</returns>
        private ActionEntity ConvertXmlActionNodeToActionEntity(XmlNode node)
        {
            if (node == null) return null;

            ActionEntity action = new ActionEntity();
            var actionType = XMLHelper.GetXmlAttribute(node, "type");
            action.ActionType = EnumHelper.TryParseEnum<ActionTypeEnum>(actionType);

            var actionMethod = XMLHelper.GetXmlAttribute(node, "method");
            action.ActionMethod = EnumHelper.TryParseEnum<ActionMethodEnum>(actionMethod);

            var subMethod = XMLHelper.GetXmlAttribute(node, "subMethod");
            action.SubMethod = EnumHelper.TryParseEnum<SubMethodEnum>(subMethod);

            var fireType = XMLHelper.GetXmlAttribute(node, "fire");
            action.FireType = EnumHelper.TryParseEnum<FireTypeEnum>(fireType);

            if (action.ActionType == ActionTypeEnum.Event)
            {
                if (action.ActionMethod == ActionMethodEnum.Python)
                {
                    throw new WfXpdlException("SQL 接口，暂不支持！");
                }
                action.Expression = XMLHelper.GetXmlAttribute(node, "expression");
                action.Arguments = XMLHelper.GetXmlAttribute(node, "arguments");
            }
            return action;
        }

        /// <summary>
        /// 转换Boundary节点
        /// </summary>
        /// <param name="node">xml节点</param>
        /// <returns>实体对象</returns>
        private BoundaryEntity ConvertXmlBoundaryNodeToBoundaryEntity(XmlNode node)
        {
            BoundaryEntity boundary = new BoundaryEntity();
            var eventType = XMLHelper.GetXmlAttribute(node, "event");
            boundary.EventTriggerType = EnumHelper.TryParseEnum<EventTriggerEnum>(eventType);

            if (boundary.EventTriggerType == EventTriggerEnum.Timer)
            {
                boundary.Expression = XMLHelper.GetXmlAttribute(node, "expression");
            }
            return boundary;
        }

        /// <summary>
        /// 转换Section节点
        /// </summary>
        /// <param name="node">xml节点</param>
        /// <returns>实体对象</returns>
        private SectionEntity ConvertXmlSectionNodeToSectionEntity(XmlNode node)
        {
            SectionEntity section = new SectionEntity();
            section.Name = XMLHelper.GetXmlAttribute(node, "name");
            section.Value = node.InnerText;

            return section;
        }
        #endregion

        #region 转移连线的获取方法
        /// <summary>
        /// 获取转移xml节点
        /// </summary>
        /// <param name="transitionGUID">转移GUID</param>
        /// <returns>XML节点</returns>
        internal XmlNode GetXmlTransitionNode(string transitionGUID)
        {
            XmlNode transitionNode = XMLHelper.GetXmlNodeByXpath(XmlProcessDefinition,
                string.Format("{0}[@id='" + transitionGUID.ToString() + "']", XPDLDefinition.StrXmlTransitionPath));

            return transitionNode;
        }

        /// <summary>
        /// 获取活动转移的To节点信息
        /// </summary>
        /// <param name="fromActivityGUID">起始节点ID</param>
        /// <returns>XML节点</returns>
        internal XmlNode GetForwardXmlTransitionNode(string fromActivityGUID)
        {
            XmlNode transitionNode = XMLHelper.GetXmlNodeByXpath(XmlProcessDefinition,
                string.Format("{0}[@from='" + fromActivityGUID.ToString() + "']", XPDLDefinition.StrXmlTransitionPath));

            return transitionNode;
        }

        /// <summary>
        /// 获取活动转移的To节点列表
        /// </summary>
        /// <param name="fromActivityGUID">起始节点ID</param>
        /// <returns>XML节点列表</returns>
        internal XmlNodeList GetForwardXmlTransitionNodeList(string fromActivityGUID)
        {
            XmlNodeList transitionNodeList = XMLHelper.GetXmlNodeListByXpath(XmlProcessDefinition,
                string.Format("{0}[@from='" + fromActivityGUID.ToString() + "']", XPDLDefinition.StrXmlTransitionPath));

            return transitionNodeList;
        }

        /// <summary>
        /// 根据两个节点，查看是否有连线
        /// </summary>
        /// <param name="fromActivityGUID">起始节点ID</param>
        /// <param name="toActivityGUID">目标节点ID</param>
        /// <returns>转移实体</returns>
        public TransitionEntity GetForwardTransition(string fromActivityGUID, string toActivityGUID)
        {
            XmlNode xmlTransitionNode = GetForwardXmlTransitionNode(fromActivityGUID, toActivityGUID);
            TransitionEntity transition = xmlTransitionNode != null ? 
                ConvertXmlTransitionNodeToTransitionEntity(xmlTransitionNode) : null;

            return transition;
        }

        /// <summary>
        /// 获取当前节点的后续连线的集合
        /// </summary>
        /// <param name="fromActivityGUID">起始节点ID</param>
        /// <returns>转移节点列表</returns>
        public IList<TransitionEntity> GetForwardTransitionList(string fromActivityGUID)
        {
            IList<TransitionEntity> transitionList = new List<TransitionEntity>();
            XmlNodeList transitionNodeList = GetForwardXmlTransitionNodeList(fromActivityGUID);
            foreach (XmlNode transitionNode in transitionNodeList)
            {
                TransitionEntity entity = ConvertXmlTransitionNodeToTransitionEntity(transitionNode);
                transitionList.Add(entity);
            }
            return transitionList;
        }

        /// <summary>
        /// 获取当前节点的后续连线的集合（使用条件过滤）
        /// </summary>
        /// <param name="fromActivityGUID">起始节点ID</param>
        /// <param name="conditionKeyValuePair">条件</param>
        /// <returns>转移实体列表</returns>
        public IList<TransitionEntity> GetForwardTransitionList(string fromActivityGUID,
            IDictionary<string, string> conditionKeyValuePair)
        {
            IList<TransitionEntity> transitionList = new List<TransitionEntity>();
            XmlNodeList transitionNodeList = GetForwardXmlTransitionNodeList(fromActivityGUID);
            foreach (XmlNode transitionNode in transitionNodeList)
            {
                TransitionEntity entity = ConvertXmlTransitionNodeToTransitionEntity(transitionNode);
                bool isValidTranstion = IsValidTransition(entity, conditionKeyValuePair);
                if (isValidTranstion)
                {
                    transitionList.Add(entity);
                }
            }
            return transitionList;
        }

        /// <summary>
        /// XOrSplit类型下的连线列表
        /// </summary>
        /// <param name="fromActivityGUID">起始节点ID</param>
        /// <param name="conditionKeyValuePair">条件</param>
        /// <returns>转移实体列表</returns>
        internal IList<TransitionEntity> GetForwardTransitionListWithConditionXOrSplit(string fromActivityGUID,
            IDictionary<string, string> conditionKeyValuePair)
        {
            IList<TransitionEntity> transitionList = new List<TransitionEntity>();
            XmlNodeList transitionNodeList = GetForwardXmlTransitionNodeList(fromActivityGUID);
            foreach (XmlNode transitionNode in transitionNodeList)
            {
                TransitionEntity entity = ConvertXmlTransitionNodeToTransitionEntity(transitionNode);
                bool isValidTranstion = IsValidTransition(entity, conditionKeyValuePair);
                if (isValidTranstion)
                {
                    transitionList.Add(entity);
                    break;
                }
            }
            return transitionList;
        }


        /// <summary>
        /// 获取活动转移的节点信息
        /// </summary>
        /// <param name="fromActivityGUID">起止节点ID</param>
        /// <param name="toActivityGUID">目标节点ID</param>
        /// <returns>XML节点</returns>
        private XmlNode GetForwardXmlTransitionNode(string fromActivityGUID,
            string toActivityGUID)
        {
            XmlNode transitionNode = XMLHelper.GetXmlNodeByXpath(XmlProcessDefinition,
                string.Format("{0}[@from='" + fromActivityGUID + "' and @to='" + toActivityGUID + "']", XPDLDefinition.StrXmlTransitionPath));
            return transitionNode;
        }

        /// <summary>
        /// 获取前驱节点的列表
        /// </summary>
        /// <param name="toActivityGUID">目标节点ID</param>
        /// <returns>XML节点列表</returns>
        internal XmlNodeList GetXmlBackwardTransitonNodeList(string toActivityGUID)
        {
            XmlNodeList transtionNodeList = XMLHelper.GetXmlNodeListByXpath(XmlProcessDefinition,
                string.Format("{0}[@to='" + toActivityGUID + "']", XPDLDefinition.StrXmlTransitionPath));
            return transtionNodeList;
        }

        /// <summary>
        /// 获取节点的前驱连线
        /// </summary>
        /// <param name="toActivityGUID">目标节点GUID</param>
        /// <returns>转移列表</returns>
        public IList<TransitionEntity> GetBackwardTransitionList(string toActivityGUID)
        {
            XmlNodeList transitionNodeList = GetXmlBackwardTransitonNodeList(toActivityGUID);
            IList<TransitionEntity> transitionList = new List<TransitionEntity>();
            foreach (XmlNode transitionNode in transitionNodeList)
            {
                TransitionEntity transition = ConvertXmlTransitionNodeToTransitionEntity(transitionNode);
                transitionList.Add(transition);
            }
            return transitionList;
        }

		/// <summary>
        /// 获取与合并节点相对应的分支节点
        /// </summary>
        /// <param name="fromActivity">合并节点</param>
        /// <param name="joinCount">经过的合并节点个数</param>
        /// <param name="splitCount">经过的分支节点个数</param>
        /// <returns>对应的分支节点</returns>
        public ActivityEntity GetBackwardGatewayActivity(ActivityEntity fromActivity,
            ref int joinCount,
            ref int splitCount)
        {
            if(fromActivity.ActivityType ==ActivityTypeEnum.GatewayNode 
                && fromActivity.GatewaySplitJoinType == GatewaySplitJoinTypeEnum.Join)
            {
                joinCount++;
                IList<TransitionEntity> backwardTrans = GetBackwardTransitionList(fromActivity.ActivityGUID);
                if(backwardTrans.Count > 0)
                {
                    return GetBackwardGatewayActivity(backwardTrans[0].FromActivity,ref joinCount,ref splitCount);
                }
            }
            else if(fromActivity.ActivityType == ActivityTypeEnum.GatewayNode 
                && fromActivity.GatewaySplitJoinType == GatewaySplitJoinTypeEnum.Split)
            {
                splitCount++;
                if(splitCount == joinCount)
                {
                    return fromActivity;
                }
                else
                {
                    IList<TransitionEntity> backwardTrans = GetBackwardTransitionList(fromActivity.ActivityGUID);
                    if(backwardTrans.Count > 0)
                    {
                        return GetBackwardGatewayActivity(backwardTrans[0].FromActivity, ref joinCount, ref splitCount);
                    }
                }
            }
            else if(fromActivity.ActivityType == ActivityTypeEnum.StartNode 
                || fromActivity.ActivityType== ActivityTypeEnum.EndNode)
            {
                return null;
            }
            else
            {
                IList<TransitionEntity> backwardTrans = GetBackwardTransitionList(fromActivity.ActivityGUID);
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
        internal IList<TransitionEntity> GetBackwardTransitionList(string activityGUID,
            Expression<Func<TransitionEntity, bool>> expression)
        {
            IList<TransitionEntity> transitionList = GetBackwardTransitionList(activityGUID);
            return GetBackwardTransitionList(activityGUID, expression);
        }

        /// <summary>
        /// 获取节点的前驱节点列表(Lambda表达式)
        /// </summary>
        /// <param name="transitionList">转移列表</param>
        /// <param name="expression">表达式</param>
        /// <returns>转移实体列表</returns>
        internal IList<TransitionEntity> GetBackwardTransitionList(IList<TransitionEntity> transitionList,
            Expression<Func<TransitionEntity, bool>> expression)
        {
            IList<TransitionEntity> newTransitionList = new List<TransitionEntity>();
            foreach (TransitionEntity transition in transitionList)
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
        internal IList<TransitionEntity> GetBackworkTransitionListWithCondition(string toActivityGUID)
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
            IList<TransitionEntity> backwardList = GetBackwardTransitionList(toActivityGUID);
            return backwardList.Count;
        }

        #region 解析条件表达式
        /// <summary>
        /// 用LINQ解析条件表达式
        /// </summary>
        /// <param name="transition">转移</param>
        /// <param name="conditionKeyValuePair">条件参数</param>
        /// <returns>解析结果</returns>
        private bool ParseCondition(TransitionEntity transition, IDictionary<string, string> conditionKeyValuePair)
        {
            Boolean result = false;
            try
            {
                string expression = transition.Condition.ConditionText;
                string expressionReplaced = ReplaceParameterToValue(expression, conditionKeyValuePair);

                Expression e = System.Linq.Dynamic.DynamicExpression.Parse(typeof(Boolean), expressionReplaced);
                LambdaExpression LE = Expression.Lambda(e);
                Func<bool> testMe = (Func<bool>)LE.Compile();
                result = testMe();
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
        public bool IsValidTransition(TransitionEntity transition,
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

        /// <summary>
        /// 取代条件表达式中的参数值
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <param name="dictoinary">列表</param>
        /// <returns>表达式</returns>
        internal string ReplaceParameterToValue(string expression, IDictionary<string, string> dictoinary)
        {
            foreach (KeyValuePair<string, string> p in dictoinary)
            {
                if (p.Value == string.Empty /* hacked by shiyonglin 2018-4-24*/ 
                    || !ExpressionParser.IsNumeric(p.Value))
                {
                    //字符串类型的变量处理，加上双引号。
                    string s = "\"" + p.Value.Trim('\"') + "\"";
                    expression = expression.Replace(p.Key, s);
                }
                else
                {
                    expression = expression.Replace(p.Key, p.Value);
                }
            }
            return expression;
        }
        #endregion

        #region Xml节点转换信息
        /// <summary>
        /// 把XML节点转换为ActivityEntity实体对象
        /// </summary>
        /// <param name="node">节点</param>
        /// <returns>转移对象</returns>
        internal TransitionEntity ConvertXmlTransitionNodeToTransitionEntity(XmlNode node)
        {
            //构造转移的基本属性
            TransitionEntity transition = new TransitionEntity();
            transition.TransitionGUID = XMLHelper.GetXmlAttribute(node, "id");
            transition.FromActivityGUID = XMLHelper.GetXmlAttribute(node, "from");
            transition.ToActivityGUID = XMLHelper.GetXmlAttribute(node, "to");
            if (!string.IsNullOrEmpty(XMLHelper.GetXmlAttribute(node, "direction")))
            {
                transition.DirectionType = (TransitionDirectionTypeEnum)Enum.Parse(typeof(TransitionDirectionTypeEnum),
                    XMLHelper.GetXmlAttribute(node, "direction"));
            }

            //转移描述
            XmlNode descriptionNode = node.SelectSingleNode("Description");
            if (descriptionNode != null)
            {
                transition.Description = descriptionNode.InnerText;
            }

            //构造活动节点的实体对象
            transition.FromActivity = GetActivity(transition.FromActivityGUID);
            transition.ToActivity = GetActivity(transition.ToActivityGUID);

            //构造转移的接收者类型
            XmlNode receiverNode = node.SelectSingleNode("Receiver");
            if (receiverNode != null)
            {
                transition.Receiver = new Receiver();
                if (!string.IsNullOrEmpty(XMLHelper.GetXmlAttribute(receiverNode, "type")))
                {
                    transition.Receiver.ReceiverType = (ReceiverTypeEnum)Enum.Parse(typeof(ReceiverTypeEnum),
                        XMLHelper.GetXmlAttribute(receiverNode, "type"));
                    int candidates = 0;
                    if (int.TryParse(XMLHelper.GetXmlAttribute(receiverNode, "candidates"), out candidates) == true)
                    {
                        transition.Receiver.Candidates = candidates;
                    }
                }
            }

            //构造转移的条件节点
            XmlNode conditionNode = node.SelectSingleNode("Condition");
            if (conditionNode != null)
            {
                transition.Condition = new ConditionEntity();
                if (!string.IsNullOrEmpty(XMLHelper.GetXmlAttribute(conditionNode, "type")))
                {
                    ConditionTypeEnum conditionTypeEnum;
                    Enum.TryParse<ConditionTypeEnum>(XMLHelper.GetXmlAttribute(conditionNode, "type"), out conditionTypeEnum);
                    transition.Condition.ConditionType = conditionTypeEnum;
                }

                if ((conditionNode.SelectSingleNode("ConditionText") != null)
                    && !string.IsNullOrEmpty(XMLHelper.GetXmlNodeValue(conditionNode, "ConditionText")))
                {
                    transition.Condition.ConditionText = XMLHelper.GetXmlNodeValue(conditionNode, "ConditionText");
                }
            }

            //构造转移的行为节点
            XmlNode groupBehavioursNode = node.SelectSingleNode("GroupBehaviours");
            if (groupBehavioursNode != null)
            {
                transition.GroupBehaviours = new GroupBehaviourEntity();
                if (!string.IsNullOrEmpty(XMLHelper.GetXmlAttribute(groupBehavioursNode, "priority")))
                {
                    short priority = 0;
                    bool isPriorityParsed = short.TryParse(XMLHelper.GetXmlAttribute(groupBehavioursNode, "priority"), out priority);
                    if (isPriorityParsed) transition.GroupBehaviours.Priority = priority;
                }

                if (!string.IsNullOrEmpty(XMLHelper.GetXmlAttribute(groupBehavioursNode, "forced")))
                {
                    bool isForced = false;
                    bool canBeParsed = Boolean.TryParse(XMLHelper.GetXmlAttribute(groupBehavioursNode, "forced"), out isForced);
                    if (canBeParsed) transition.GroupBehaviours.Forced = isForced;
                }
            }
            return transition;
        }
        #endregion
        #endregion

        #region 节点类型判断
        /// <summary>
        /// 是否并行会签节点
        /// </summary>
        /// <param name="activity">节点</param>
        /// <returns>是否并行</returns>
        public Boolean IsMIParallel(ActivityEntity activity)
        {
            var isParallel = false;
            if (activity.ActivityType == ActivityTypeEnum.MultipleInstanceNode)
            {
                if (activity.ActivityTypeDetail.ComplexType == ComplexTypeEnum.SignTogether
                    || activity.ActivityTypeDetail.ComplexType == ComplexTypeEnum.SignForward)
                {
                    if (activity.ActivityTypeDetail.MergeType == MergeTypeEnum.Parallel)
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
        public Boolean IsMISequence(ActivityEntity activity)
        {
            var isSequence = false;
            if (activity.ActivityType == ActivityTypeEnum.MultipleInstanceNode)
            {
                if (activity.ActivityTypeDetail.ComplexType == ComplexTypeEnum.SignTogether
                    || activity.ActivityTypeDetail.ComplexType == ComplexTypeEnum.SignForward)
                {
                    if (activity.ActivityTypeDetail.MergeType == MergeTypeEnum.Sequence)
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
        public Boolean IsMINode(ActivityEntity activity)
        {
            var isMI = false;
            if (activity.ActivityType == ActivityTypeEnum.MultipleInstanceNode)
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
        public Boolean IsTaskNode(ActivityEntity activity)
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
        public Boolean IsAndSplitMI(ActivityEntity activity)
        {
            var isAndSplitMI = false;
            if (activity.GatewaySplitJoinType == GatewaySplitJoinTypeEnum.Split
                    && activity.GatewayDirectionType == GatewayDirectionEnum.AndSplitMI)
            {
                isAndSplitMI = true;
            }
            return isAndSplitMI;
        }
        #endregion

        #region 资源数据内容操作
        /// <summary>
        /// 获取角色可以编辑的数据项列表
        /// </summary>
        /// <param name="roleCode">角色代码</param>
        /// <returns>列表</returns>
        internal IList<string> GetRoleDataItems(string roleCode)
        {
            XmlNode participantNode = XMLHelper.GetXmlNodeByXpath(this.XmlProcessDefinition,
                string.Format("{0}[@code='" + roleCode + "']", XPDLDefinition.StrXmlSingleParticipantPath));
            string participantGUID = XMLHelper.GetXmlAttribute(participantNode, "id");

            XmlNode participantDataItemsNode = XMLHelper.GetXmlNodeByXpath(XmlProcessDefinition,
                string.Format("{0}[@id='" + participantGUID + "']", XPDLDefinition.StrXmlParticipantDataItemPermissions));

            IList<string> itemList = new List<string>();
            foreach (XmlNode dataItemNode in participantDataItemsNode.ChildNodes)
            {
                string dataItemID = XMLHelper.GetXmlAttribute(dataItemNode, "id");
                XmlNode srcDataItemNode = XMLHelper.GetXmlNodeByXpath(XmlProcessDefinition,
                    string.Format("{0}[@id='" + dataItemID + "']", XPDLDefinition.StrXmlSingleDataItems));

                string dataItemCode = XMLHelper.GetXmlAttribute(srcDataItemNode, "code");
                itemList.Add(dataItemCode);
            }
            return itemList;
        }
        #endregion
    }
}
