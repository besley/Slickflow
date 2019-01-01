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
                if (CachedHelper.GetXpdlCache(ProcessEntity.ProcessGUID, ProcessEntity.Version) == null)
                {
                    var pm = new ProcessManager();
                    var xmlDoc = pm.GetProcessXmlDocument(ProcessEntity.ProcessGUID, ProcessEntity.Version, 
                        XPDLStorageFactory.CreateXPDLStorage());    //xml文件读取方式，数据库或外部文件

                    CachedHelper.SetXpdlCache(ProcessEntity.ProcessGUID, ProcessEntity.Version, xmlDoc);
                }
                return CachedHelper.GetXpdlCache(ProcessEntity.ProcessGUID, ProcessEntity.Version);
            }
        }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="processGUID"></param>
        /// <param name="version"></param>
        public ProcessModel(string processGUID, string version)
        {
            ProcessEntity = (new ProcessManager()).GetByVersion(processGUID, version);
        }
        #endregion

        #region 流程定义数据获取
        /// <summary>
        /// 获取角色列表
        /// </summary> 
        /// <returns></returns>
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
        /// <returns></returns>
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
        /// <returns></returns>
        public ActivityEntity GetEndActivity()
        {
            string nodeType = "EndNode";
            XmlNode endTypeNode = GetXmlActivityTypeNodeFromXmlFile(nodeType);
            return ConvertXmlActivityNodeToActivityEntity(endTypeNode.ParentNode);
        }

        /// <summary>
        /// 获取任务类型的节点
        /// </summary>
        /// <returns></returns>
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
        /// 获取任务类型的节点(包含会签节点和子流程节点)
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        private  IList<ActivityEntity> GetAllTaskActivityListByType()
        {
            List<ActivityEntity> activityList = new List<ActivityEntity>();
            var nodeTypes = new string[] { "TaskNode", "MultipleInstanceNode", "SubProcessNode" };
            foreach (var item in nodeTypes)
            {
                XmlNodeList nodeList = GetXmlActivityListByTypeFromXmlFile(item);

                ActivityEntity entity = null;

                foreach (XmlNode node in nodeList)
                {
                    entity = ConvertXmlActivityNodeToActivityEntity(node.ParentNode);
                    activityList.Add(entity);
                } 
            }

            return activityList;
        }

        /// <summary>
        /// 获取任务类型的节点(包含会签节点和子流程节点)，按照Transition顺序组成序列
        /// </summary>
        /// <returns></returns>
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
        /// <returns></returns>
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
                    activityList.Add(toActivity);
                }
                //递归遍历转移数据
                TranverseTransitionList(activityList, toActivity.ActivityGUID);
            }

            return activityList;
        }

        /// <summary>
        /// 获取流程的第一个可办理节点
        /// </summary>
        /// <returns></returns>
        public ActivityEntity GetFirstActivity()
        {
            ActivityEntity startActivity = GetStartActivity();
            ActivityEntity firstActivity = GetNextActivity(startActivity.ActivityGUID);
            return firstActivity;
        }

        /// <summary>
        /// 获取当前节点的下一个节点信息
        /// </summary>
        /// <param name="activityGUID"></param>
        /// <returns></returns>
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
        /// <param name="root"></param>
        /// <param name="treeNodeList"></param>
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
        /// <param name="currentActivityGUID"></param>
        /// <param name="conditionKeyValuePair"></param>
        /// <returns></returns>
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
        /// <param name="currentActivityGUID"></param>
        /// <param name="conditionKeyValuePair"></param>
        /// <param name="activityResource"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
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
                if (gatewayNode.Activity.GatewaySplitJoinType == GatewaySplitJoinTypeEnum.Split
                    && gatewayNode.Activity.GatewayDirectionType == GatewayDirectionEnum.AndSplitMI)
                {
                    //与分支(多实例)--不需要判断条件，直接返回下一步列表
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
                        if (activityResource.NextActivityPerformers != null)
                        {
                            if (expression.Compile().Invoke(activityResource, c.Activity))
                            {
                                newRoot.Add(c);
                            }
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
        /// <param name="root"></param>
        /// <param name="activityResource"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        private NextActivityComponent GetNextActivityListByExpressionRecurisivly(NextActivityComponent root,
           ActivityResource activityResource,
           Expression<Func<ActivityResource, ActivityEntity, bool>> expression)
        {
            NextActivityComponent r1 = null;
            foreach (NextActivityComponent c in root)
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
                        r1 = AddChildToNewGatewayComponent(r1, root, c);
                    }
                }
            }
            return r1;
        }

        /// <summary>
        /// 添加子节点到网关节点
        /// </summary>
        /// <param name="newRoot">新的根节点</param>
        /// <param name="root">根节点</param>
        /// <param name="child">子节点</param>
        /// <returns>下一步活动节点</returns>
        private NextActivityComponent AddChildToNewGatewayComponent(NextActivityComponent newRoot,
            NextActivityComponent root,
            NextActivityComponent child)
        {
            if ((newRoot == null) && (child != null))
                newRoot = NextActivityComponentFactory.CreateNextActivityComponent(root);

            if ((newRoot != null) && (child != null))
                newRoot.Add(child);
            return newRoot;
        }
        #endregion

        #region 流程流转解析，处理流程上一步退回步骤列表

        /// <summary>
        /// 获取上一步
        /// </summary>
        /// <param name="currentActivityGUID"></param>
        /// <returns></returns>
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
        /// <param name="toActivityGUID">当前运行节点GUID</param>
        /// <param name="hasGatewayPassed">是否经过网关</param>
        /// <returns>前置节点列表</returns>
        public IList<ActivityEntity> GetPreviousActivityList(string toActivityGUID, out bool hasGatewayPassed)
        {
            var hasGatewayPassedInternal = false;
            IList<ActivityEntity> previousActivityList = new List<ActivityEntity>();
            GetPreviousActivityList(toActivityGUID, previousActivityList, ref hasGatewayPassedInternal);
            hasGatewayPassed = hasGatewayPassedInternal;

            return previousActivityList;
        }

        /// <summary>
        /// 获取前置节点列表（不包含多实例节点内部的判断，因为没有相应的Transition记录）
        /// </summary>
        /// <param name="currentActivityGUID">当前运行节点GUID</param>
        /// <param name="previousActivityList">前置节点列表</param>
        /// <param name="hasGatewayPassed">是否经过网关</param>
        private void GetPreviousActivityList(string currentActivityGUID, IList<ActivityEntity> previousActivityList, ref bool hasGatewayPassed)
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
        #endregion

        #endregion

 
        #region Xml活动节点读取操作
        /// <summary>
        /// 获取XML的节点信息
        /// </summary>
        /// <param name="activityGUID"></param>
        /// <returns></returns>
        private XmlNode GetXmlActivityNodeFromXmlFile(string activityGUID)
        {
            XmlNode xmlNode = XMLHelper.GetXmlNodeByXpath(XmlProcessDefinition,
                    string.Format("{0}[@id='" + activityGUID + "']", XPDLDefinition.StrXmlActivityPath));
            return xmlNode;
        }

        /// <summary>
        /// 获取活动节点的类型信息
        /// </summary>
        /// <param name="nodeType"></param>
        /// <returns></returns>
        private XmlNode GetXmlActivityTypeNodeFromXmlFile(string nodeType)
        {
            XmlNode typeNode = XMLHelper.GetXmlNodeByXpath(XmlProcessDefinition,
                string.Format("{0}/ActivityType[@type='" + nodeType + "']", XPDLDefinition.StrXmlActivityPath));
            return typeNode;
        }

        /// <summary>
        /// 获取特定类型的活动节点
        /// </summary>
        /// <param name="nodeType"></param>
        /// <returns></returns>
        private XmlNodeList GetXmlActivityListByTypeFromXmlFile(string nodeType)
        {
            XmlNodeList nodeList = XMLHelper.GetXmlNodeListByXpath(XmlProcessDefinition,
                string.Format("{0}/ActivityType[@type='" + nodeType + "']", XPDLDefinition.StrXmlActivityPath));
            return nodeList;
        }

        /// <summary>
        /// 获取参与者信息
        /// </summary>
        /// <param name="participantGUID"></param>
        /// <returns></returns>
        private XmlNode GetXmlParticipantNodeFromXmlFile(string participantGUID)
        {
            XmlNode participantNode = XMLHelper.GetXmlNodeByXpath(XmlProcessDefinition,
                string.Format("{0}[@id='" + participantGUID + "']", XPDLDefinition.StrXmlSingleParticipantPath));
            return participantNode;
        }

        /// <summary>
        /// 获取当前节点信息
        /// </summary>
        /// <returns></returns>
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
        /// <returns></returns>
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
        /// <param name="performerGUID"></param>
        /// <returns></returns>
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
        /// <param name="performerGUID"></param>
        /// <returns></returns>
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
        /// <param name="activityGUID"></param>
        /// <returns></returns>
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
        /// <param name="activityGUID"></param>
        /// <returns></returns>
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
        /// <param name="node"></param>
        /// <returns></returns>
        internal ActivityEntity ConvertXmlActivityNodeToActivityEntity(XmlNode node)
        {
            ActivityEntity entity = new ActivityEntity();
            entity.ActivityName = XMLHelper.GetXmlAttribute(node, "name");
            entity.ActivityCode = XMLHelper.GetXmlAttribute(node, "code");
            entity.ActivityGUID = XMLHelper.GetXmlAttribute(node, "id");
            entity.ProcessGUID = ProcessEntity.ProcessGUID;

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

            return entity;
        }

        /// <summary>
        /// 把Xml节点转换为ActivityTypeDetail 类（用于会签等复杂类型）
        /// </summary>
        /// <param name="typeNode"></param>
        /// <returns></returns>
        private ActivityTypeDetail ConvertXmlNodeToActivityTypeDetail(XmlNode typeNode)
        {
            ActivityTypeDetail entity = new ActivityTypeDetail();
            entity.ActivityType = (ActivityTypeEnum)Enum.Parse(typeof(ActivityTypeEnum), XMLHelper.GetXmlAttribute(typeNode, "type"));

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
        /// <param name="node"></param>
        /// <returns></returns>
        private ActionEntity ConvertXmlActionNodeToActionEntity(XmlNode node)
        {
            if (node == null) return null;

            ActionEntity action = new ActionEntity();
            var actionType = XMLHelper.GetXmlAttribute(node, "type");
            try
            {
                action.ActionType = EnumHelper.ParseEnum<ActionTypeEnum>(actionType);
            }
            catch
            {
                action.ActionType = ActionTypeEnum.None;
            }

            return action;
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
        internal IList<TransitionEntity> GetForwardTransitionList(string fromActivityGUID,
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
                string.Format("{0}[@from='" + fromActivityGUID + "' and to='" + toActivityGUID + "']", XPDLDefinition.StrXmlTransitionPath));
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
        internal IList<TransitionEntity> GetBackwardTransitionList(string toActivityGUID)
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
        public ActivityEntity GetBackwardGatewayActivity(ActivityEntity fromActivity,ref int joinCount,ref int splitCount)
        {
            if(fromActivity.ActivityType ==ActivityTypeEnum.GatewayNode && fromActivity.GatewaySplitJoinType == GatewaySplitJoinTypeEnum.Join){
                joinCount++;
                IList<TransitionEntity> backwardTrans = GetBackwardTransitionList(fromActivity.ActivityGUID);
                if(backwardTrans.Count>0){
                    return GetBackwardGatewayActivity(backwardTrans[0].FromActivity,ref joinCount,ref splitCount);
                }
            }else if(fromActivity.ActivityType == ActivityTypeEnum.GatewayNode && fromActivity.GatewaySplitJoinType == GatewaySplitJoinTypeEnum.Split){
                splitCount++;
                if(splitCount==joinCount){
                    return fromActivity;
                }else{
                    IList<TransitionEntity> backwardTrans = GetBackwardTransitionList(fromActivity.ActivityGUID);
                    if(backwardTrans.Count>0){
                        return GetBackwardGatewayActivity(backwardTrans[0].FromActivity,ref joinCount,ref splitCount);
                    }
                }
            }else if(fromActivity.ActivityType == ActivityTypeEnum.StartNode || fromActivity.ActivityType== ActivityTypeEnum.EndNode)
                return null;
            else{
                IList<TransitionEntity> backwardTrans = GetBackwardTransitionList(fromActivity.ActivityGUID);
                    if(backwardTrans.Count>0){
                        return GetBackwardGatewayActivity(backwardTrans[0].FromActivity,ref joinCount,ref splitCount);
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
        /// <param name="toActivityGUID"></param>
        /// <returns></returns>
        internal IList<TransitionEntity> GetBackworkTransitionListWithCondition(string toActivityGUID)
        {
            return GetBackwardTransitionList(toActivityGUID,
                (t => t.Condition != null && !string.IsNullOrEmpty(t.Condition.ConditionText)));
        }

        /// <summary>
        /// 获取并行连线的，类型为必需类型
        /// </summary>
        /// <param name="transitionList"></param>
        /// <returns></returns>
        //internal IList<TransitionEntity> GetBackwardTransitionListNecessary(IList<TransitionEntity> transitionList)
        //{
        //    return GetBackwardTransitionList(transitionList,
        //        (t => (t.GroupBehaviour.ParallelOption != null) && (t.GroupBehaviour.ParallelOption == ParallelOptionEnum.Necessary)));
        //}

        /// <summary>
        /// 获取节点前驱连线上必须的Token数目
        /// </summary>
        /// <param name="toActivityGUID"></param>
        /// <returns></returns>
        //internal int GetBackwardTransitionListNecessaryCount(string toActivityGUID)
        //{
        //    IList<TransitionEntity> backwardList = GetBackwardTransitionList(toActivityGUID);
        //    IList<TransitionEntity> necBackwardList = GetBackwardTransitionListNecessary(backwardList);
        //    return necBackwardList.Count;
        //}

        /// <summary>
        /// 获取节点前驱连线的数目
        /// </summary>
        /// <param name="toActivityGUID"></param>
        /// <returns></returns>
        public int GetBackwardTransitionListCount(string toActivityGUID)
        {
            IList<TransitionEntity> backwardList = GetBackwardTransitionList(toActivityGUID);
            return backwardList.Count;
        }

        #region 解析条件表达式
        /// <summary>
        /// 用LINQ解析条件表达式
        /// </summary>
        /// <param name="transition"></param>
        /// <param name="conditionKeyValuePair"></param>
        /// <returns></returns>
        private bool ParseCondition(TransitionEntity transition, IDictionary<string, string> conditionKeyValuePair)
        {
            string expression = transition.Condition.ConditionText;
            string expressionReplaced = ReplaceParameterToValue(expression, conditionKeyValuePair);

            Expression e = System.Linq.Dynamic.DynamicExpression.Parse(typeof(Boolean), expressionReplaced);
            LambdaExpression LE = Expression.Lambda(e);
            Func<bool> testMe = (Func<bool>)LE.Compile();
            bool result = testMe();

            return result;
        }

        /// <summary>
        /// 是否是满足条件的Transition，如果条件为空，默认是有效的。
        /// </summary>
        /// <param name="transition"></param>
        /// <param name="conditionKeyValuePair"></param>
        /// <returns></returns>
        public bool IsValidTransition(TransitionEntity transition,
           IDictionary<string, string> conditionKeyValuePair)
        {
            bool isValid = false;

            if (transition.Condition != null && !string.IsNullOrEmpty(transition.Condition.ConditionText))
            {
                if (conditionKeyValuePair != null)
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
        /// 判断整个连线集合，是否满足条件
        /// </summary>
        /// <param name="transitionList"></param>
        /// <param name="conditionKeyValuePair"></param>
        /// <returns></returns>
        public bool CheckAndSplitOccurrenceCondition(IList<TransitionEntity> transitionList,
            IDictionary<string, string> conditionKeyValuePair)
        {
            bool isValidAll = true;
            foreach (TransitionEntity transition in transitionList)
            {
                //只有是必需验证的条件，采取检查
                if (transition.GroupBehaviour !=null && transition.GroupBehaviour.ParallelOption == ParallelOptionEnum.Necessary)
                {
                    bool isVailid = IsValidTransition(transition, conditionKeyValuePair);
                    if (!isVailid)
                    {
                        isValidAll = false;
                        break;
                    }
                }
            }
            return isValidAll;
        }

        /// <summary>
        /// 取代条件表达式中的参数值
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="dictoinary"></param>
        /// <returns></returns>
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
        /// <param name="node"></param>
        /// <returns></returns>
        internal TransitionEntity ConvertXmlTransitionNodeToTransitionEntity(XmlNode node)
        {
            //构造转移的基本属性
            TransitionEntity entity = new TransitionEntity();
            entity.TransitionGUID = XMLHelper.GetXmlAttribute(node, "id");
            entity.FromActivityGUID = XMLHelper.GetXmlAttribute(node, "from");
            entity.ToActivityGUID = XMLHelper.GetXmlAttribute(node, "to");
            if (!string.IsNullOrEmpty(XMLHelper.GetXmlAttribute(node, "direction")))
            {
                entity.DirectionType = (TransitionDirectionTypeEnum)Enum.Parse(typeof(TransitionDirectionTypeEnum),
                    XMLHelper.GetXmlAttribute(node, "direction"));
            }

            //构造活动节点的实体对象
            entity.FromActivity = GetActivity(entity.FromActivityGUID);
            entity.ToActivity = GetActivity(entity.ToActivityGUID);

            //构造转移的接收者类型
            XmlNode receiverNode = node.SelectSingleNode("Receiver");
            if (receiverNode != null)
            {
                entity.Receiver = new Receiver();
                if (!string.IsNullOrEmpty(XMLHelper.GetXmlAttribute(receiverNode, "type")))
                {
                    entity.Receiver.ReceiverType = (ReceiverTypeEnum)Enum.Parse(typeof(ReceiverTypeEnum),
                        XMLHelper.GetXmlAttribute(receiverNode, "type"));
                    int candidates = 0;
                    if (int.TryParse(XMLHelper.GetXmlAttribute(receiverNode, "candidates"), out candidates) == true)
                    {
                        entity.Receiver.Candidates = candidates;
                    }
                }
            }

            //构造转移的条件节点
            XmlNode conditionNode = node.SelectSingleNode("Condition");
            if (conditionNode != null)
            {
                entity.Condition = new ConditionEntity();
                if (!string.IsNullOrEmpty(XMLHelper.GetXmlAttribute(conditionNode, "type")))
                {
                    ConditionTypeEnum conditionTypeEnum;
                    Enum.TryParse<ConditionTypeEnum>(XMLHelper.GetXmlAttribute(conditionNode, "type"), out conditionTypeEnum);
                    entity.Condition.ConditionType = conditionTypeEnum;
                }

                if ((conditionNode.SelectSingleNode("ConditionText") != null)
                    && !string.IsNullOrEmpty(XMLHelper.GetXmlNodeValue(conditionNode, "ConditionText")))
                {
                    entity.Condition.ConditionText = XMLHelper.GetXmlNodeValue(conditionNode, "ConditionText");
                }
            }

            //构造转移的行为节点
            XmlNode groupBehaviourNode = node.SelectSingleNode("GroupBehaviour");
            if (groupBehaviourNode != null)
            {
                entity.GroupBehaviour = new GroupBehaviourEntity();
                if (!string.IsNullOrEmpty(XMLHelper.GetXmlAttribute(groupBehaviourNode, "priority")))
                {
                    entity.GroupBehaviour.Priority = short.Parse(XMLHelper.GetXmlAttribute(groupBehaviourNode, "priority"));
                }

                if (!string.IsNullOrEmpty(XMLHelper.GetXmlAttribute(groupBehaviourNode, "parallelOption")))
                {
                    entity.GroupBehaviour.ParallelOption = (ParallelOptionEnum)Enum.Parse(typeof(ParallelOptionEnum),
                        XMLHelper.GetXmlAttribute(groupBehaviourNode, "parallelOption"));
                }
            }
            return entity;
        }
        #endregion
        #endregion

        #region 资源数据内容操作
        /// <summary>
        /// 获取角色可以编辑的数据项列表
        /// </summary>
        /// <param name="roleCode"></param>
        /// <returns></returns>
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
