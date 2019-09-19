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
using System.Collections.Generic;
using System.Xml;
using Slickflow.Engine.Common;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Utility;
using Slickflow.Module.Resource;

namespace Slickflow.Graph
{
    /// <summary>
    /// 流程XML 生成器
    /// </summary>
    public class ProcessXmlBuilder
    {
        #region 属性及构造方法
        private Flow _flow = null;
        private ProcessEntity _proecessEntity = null;
        private XmlDocument _processXmlDoc = new XmlDocument();
        private XmlNode _activitiesXmlNode = null;
        private XmlNode _transitionsXmlNode = null;
        private IList<string> _nodeGUIDList = new List<string>();
        private string _strXmlParticipantsPath = "Participants";
        private string _strXmlSingleParticipantPath = "Participants/Participant";

        public ProcessXmlBuilder(Flow flow)
        {
            _flow = flow;
        }
        #endregion

        /// <summary>
        /// 序列化方法
        /// </summary>
        public string Serialize()
        {
            if (_flow == null) throw new ApplicationException("流程图形对象不能为空，请确认是否有初始化!");

            //create process body xml
            CreateProcessXml(_flow.Name, _flow.Code);

            //geography parent guid
            var parentID = Guid.NewGuid().ToString();

            //create activity xml
            foreach (var vertex in _flow.Vertices)
            {
                CreateActivity(vertex, parentID);
            }

            //create transition xml
            foreach (var link in _flow.Links)
            {
                CreateTransition(link, parentID);
            }

            //create participants xml
            CreatePaticipantsXml(_flow, _processXmlDoc);

            var xmlContent = _processXmlDoc.OuterXml;
            _proecessEntity.XmlContent = xmlContent;

            return xmlContent;
        }

        /// <summary>
        /// 创建序列化流程
        /// </summary>
        /// <param name="name">流程名称</param>
        /// <param name="code">流程编码</param>
        /// <returns>XML生成器</returns>
        private ProcessXmlBuilder CreateProcessXml(string name, string code)
        {
            _processXmlDoc = GenerateXmlContent(name, code);
            var workflowNode = _processXmlDoc.DocumentElement.SelectSingleNode("WorkflowProcesses");
            var processNode = workflowNode.SelectSingleNode("Process");
            _activitiesXmlNode = _processXmlDoc.CreateElement("Activities");
            processNode.AppendChild(_activitiesXmlNode);

            _transitionsXmlNode = _processXmlDoc.CreateElement("Transitions");
            processNode.AppendChild(_transitionsXmlNode);

            return this;
        }

        /// <summary>
        /// 生成基本XML文档
        /// </summary>
        /// <returns>XML文档</returns>
        private XmlDocument GenerateXmlContent(string name, string code)
        {
            _proecessEntity = new ProcessEntity
            {
                ProcessGUID = Guid.NewGuid().ToString(),
                Version = "1",
                ProcessName = name,
                ProcessCode = code,
            };

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<Package/>");
            XmlDeclaration xmldecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);

            //Add the new node to the document.
            XmlElement root = xmlDoc.DocumentElement;
            xmlDoc.InsertBefore(xmldecl, root);

            XmlElement participantsNode = xmlDoc.CreateElement("Participants");
            root.AppendChild(participantsNode);

            XmlElement workflowNode = xmlDoc.CreateElement("WorkflowProcesses");
            root.AppendChild(workflowNode);

            XmlElement processNode = xmlDoc.CreateElement("Process");
            workflowNode.AppendChild(processNode);

            processNode.SetAttribute("id", _proecessEntity.ProcessGUID);
            processNode.SetAttribute("name", _proecessEntity.ProcessName);
            processNode.SetAttribute("code", _proecessEntity.ProcessCode);
            processNode.SetAttribute("version", _proecessEntity.Version);

            XmlElement descriptionNode = xmlDoc.CreateElement("Description");
            descriptionNode.InnerText = _proecessEntity.Description;
            processNode.AppendChild(descriptionNode);

            return xmlDoc;
        }

        /// <summary>
        /// 创建活动节点
        /// </summary>
        /// <param name="node">节点</param>
        /// <returns>XML生成器</returns>
        private ProcessXmlBuilder CreateActivity(Vertex node, string parentID)
        {
            var activityGUID = string.Empty;
            var activityNode = CreateActivityXmlNode(_processXmlDoc,
                parentID,
                node);
            _activitiesXmlNode.AppendChild(activityNode);

            _nodeGUIDList.Add(node.ActivityGUID);
            return this;
        }

        /// <summary>
        /// 创建活动XML节点
        /// </summary>
        /// <param name="xmlDoc">XML文档</param>
        /// <param name="vertex">节点</param>
        /// <returns>XML节点</returns>
        private XmlNode CreateActivityXmlNode(XmlDocument xmlDoc,
            string parentID,
            Vertex vertex)
        {
            int width = 0;
            int height = 0;
            if (vertex.VertexType == ActivityTypeEnum.StartNode
                || vertex.VertexType == ActivityTypeEnum.EndNode)
            {
                width = 32;
                height = 32;
            }
            else
            {
                width = 72;
                height = 32;
            }

            var activityNode = xmlDoc.CreateElement("Activity");
            XMLHelper.SetXmlAttribute(activityNode, "id", vertex.ActivityGUID);
            XMLHelper.SetXmlAttribute(activityNode, "name", vertex.Name);
            XMLHelper.SetXmlAttribute(activityNode, "code", vertex.Code);
            XMLHelper.SetXmlAttribute(activityNode, "url", vertex.Activity.ActivityUrl);

            var descriptionNode = xmlDoc.CreateElement("Description");
            activityNode.AppendChild(descriptionNode);

            var activityTypeNode = xmlDoc.CreateElement("ActivityType");
            XMLHelper.SetXmlAttribute(activityTypeNode, "type", vertex.VertexType.ToString());
            activityNode.AppendChild(activityTypeNode);

            if (vertex.VertexType == ActivityTypeEnum.GatewayNode)
            {
                XMLHelper.SetXmlAttribute(activityTypeNode, "gatewaySplitJoinType", 
                    vertex.Activity.GatewaySplitJoinType.ToString());
                XMLHelper.SetXmlAttribute(activityTypeNode, "gatewayDirection",
                    vertex.Activity.GatewayDirectionType.ToString());
            }
            else if (vertex.VertexType == ActivityTypeEnum.MultipleInstanceNode)
            {
                if (vertex.Activity.ActivityTypeDetail != null)
                {
                    XMLHelper.SetXmlAttribute(activityTypeNode, "complexType", vertex.Activity.ActivityTypeDetail.CompareType.ToString());
                    XMLHelper.SetXmlAttribute(activityTypeNode, "mergeType", vertex.Activity.ActivityTypeDetail.MergeType.ToString());
                    XMLHelper.SetXmlAttribute(activityTypeNode, "compareType", vertex.Activity.ActivityTypeDetail.CompareType.ToString());
                    XMLHelper.SetXmlAttribute(activityTypeNode, "completeOrder", vertex.Activity.ActivityTypeDetail.CompleteOrder.ToString());
                }
            }
            else if (vertex.VertexType == ActivityTypeEnum.SubProcessNode)
            {
                if (vertex.Activity.ActivityTypeDetail != null)
                {
                    XMLHelper.SetXmlAttribute(activityTypeNode, "subId", vertex.Activity.ActivityTypeDetail.SubProcessGUID);
                }
            }
            
            //roles
            if (vertex.RoleList != null && vertex.RoleList.Count > 0)
            {
                var performersNode = xmlDoc.CreateElement("Performers");
                activityNode.AppendChild(performersNode);

                XmlNode performerNode = null;
                Participant newParticipant = null;
                foreach (var role in vertex.RoleList)
                {
                    performerNode = xmlDoc.CreateElement("Performer");
                    performersNode.AppendChild(performerNode);

                    newParticipant = CheckRoleInParticipantListOfFlow(_flow.ParticipantList, role);
                    if (newParticipant != null)
                    {
                        XMLHelper.SetXmlAttribute(performerNode, "id", newParticipant.ID);
                    }
                }
            }

            //actions
            if (vertex.Activity.ActionList != null && vertex.Activity.ActionList.Count > 0)
            {
                var actionsNode = xmlDoc.CreateElement("Actions");
                activityNode.AppendChild(actionsNode);

                foreach (var action in vertex.Activity.ActionList)
                {
                    var actionNode = xmlDoc.CreateElement("Action");
                    actionsNode.AppendChild(actionNode);

                    XMLHelper.SetXmlAttribute(actionNode, "type", action.ActionType.ToString());
                    XMLHelper.SetXmlAttribute(actionNode, "fire", action.FireType.ToString());
                    XMLHelper.SetXmlAttribute(actionNode, "method", action.ActionMethod.ToString());
                    XMLHelper.SetXmlAttribute(actionNode, "subMethod", action.SubMethod.ToString());
                    XMLHelper.SetXmlAttribute(actionNode, "arguments", action.Arguments);
                    XMLHelper.SetXmlAttribute(actionNode, "expression", action.Expression);
                }
            }

            //boundary
            if (vertex.Activity.BoundaryList != null && vertex.Activity.BoundaryList.Count > 0)
            {
                var boudariesNode = xmlDoc.CreateElement("Boundaries");
                activityNode.AppendChild(boudariesNode);

                foreach (var boundary in vertex.Activity.BoundaryList)
                {
                    var boundaryNode = xmlDoc.CreateElement("Boundary");
                    boudariesNode.AppendChild(boundaryNode);

                    XMLHelper.SetXmlAttribute(boundaryNode, "event", boundary.EventTriggerType.ToString());
                    XMLHelper.SetXmlAttribute(boundaryNode, "expression", boundary.Expression);
                }
            }
            
            //geography
            var geographyNode = xmlDoc.CreateElement("Geography");
            activityNode.AppendChild(geographyNode);
            XMLHelper.SetXmlAttribute(geographyNode, "parent", parentID);
            XMLHelper.SetXmlAttribute(geographyNode, "style", GraphStyle.SetNodeStyle(vertex.Activity));

            var widgetNode = xmlDoc.CreateElement("Widget");
            XMLHelper.SetXmlAttribute(widgetNode, "left", vertex.Left.ToString());
            XMLHelper.SetXmlAttribute(widgetNode, "top", vertex.Top.ToString());
            XMLHelper.SetXmlAttribute(widgetNode, "width", width.ToString());
            XMLHelper.SetXmlAttribute(widgetNode, "height", height.ToString());

            geographyNode.AppendChild(widgetNode);

            return activityNode;
        }

        /// <summary>
        /// 添加节点上的角色
        /// </summary>
        /// <param name="participantList">参与者列表</param>
        /// <param name="role">角色</param>
        /// <returns>参与者实体</returns>
        private Participant CheckRoleInParticipantListOfFlow(IList<Participant> participantList, Role role)
        {
            Participant participant = null;

            var isExist = false;
            foreach (var p in participantList)
            {
                if (p.Code == role.RoleCode)
                {
                    isExist = true;
                    break;
                }
            }

            if (isExist == true)
            {
                participant = new Participant
                {
                    ID = Guid.NewGuid().ToString(),
                    Name = role.RoleName,
                    Code = role.RoleCode,
                    OuterID = role.ID
                };
                participantList.Add(participant);
            }
            return participant;
        }

        /// <summary>
        /// 创建转移
        /// </summary>
        /// <param name="link">链接对象</param>
        /// <param name="parentID">图形容器对象</param>
        /// <returns>XML生成器</returns>
        private ProcessXmlBuilder CreateTransition(Link link, 
            string parentID)
        {
            var transitionNode = CreateTransitionXmlNode(_processXmlDoc, parentID, link);
            _transitionsXmlNode.AppendChild(transitionNode);

            return this;
        }

        /// <summary>
        /// 创建转移XML节点
        /// </summary>
        /// <param name="xmlDoc">XML文档</param>
        /// <param name="link">链接对象</param>
        /// <param name="parentID">图形容器对象</param>
        /// <returns>XML节点</returns>
        private XmlNode CreateTransitionXmlNode(XmlDocument xmlDoc,
            string parentID,
            Link link)
        {
            var transitionNode = xmlDoc.CreateElement("Transition");
            XMLHelper.SetXmlAttribute(transitionNode, "id", Guid.NewGuid().ToString());
            XMLHelper.SetXmlAttribute(transitionNode, "from", link.Source.ActivityGUID);
            XMLHelper.SetXmlAttribute(transitionNode, "to", link.Target.ActivityGUID);

            var descriptionNode = xmlDoc.CreateElement("Description");
            descriptionNode.InnerText = link.Description;
            transitionNode.AppendChild(descriptionNode);

            if (link.Transition.Receiver != null)
            {
                var receiverNode = xmlDoc.CreateElement("Receiver");
                XMLHelper.SetXmlAttribute(receiverNode, "type", link.Transition.Receiver.ReceiverType.ToString());
                transitionNode.AppendChild(receiverNode);
            }
            
            if (link.Transition.Condition != null
                && !string.IsNullOrEmpty(link.Transition.Condition.ConditionText))
            {
                var conditionNode = xmlDoc.CreateElement("Condition");
                XMLHelper.SetXmlAttribute(conditionNode, "type", link.Transition.Condition.ConditionType.ToString());
                transitionNode.AppendChild(conditionNode);

                var conditionTextNode = xmlDoc.CreateElement("ConditionText");
                conditionTextNode.InnerText = link.Transition.Condition.ConditionText;
                conditionNode.AppendChild(conditionTextNode);

            }

            var geographyNode = xmlDoc.CreateElement("Geography");
            transitionNode.AppendChild(geographyNode);
            XMLHelper.SetXmlAttribute(geographyNode, "parent", parentID);

            return transitionNode;
        }

        /// <summary>
        /// 创建参与者列表
        /// </summary>
        /// <param name="flow">流程图形实体</param>
        /// <param name="xmlDoc">流程XML文档</param>
        private void CreatePaticipantsXml(Flow flow, XmlDocument xmlDoc)
        {
            var participantsNode = XMLHelper.GetXmlNodeByXpath(xmlDoc, _strXmlParticipantsPath);
            var participantsList = flow.ParticipantList;

            XmlNode participantNode = null;
            foreach (var p in participantsList)
            {
                participantNode = xmlDoc.CreateElement("Participant");
                participantsNode.AppendChild(participantNode);

                XMLHelper.SetXmlAttribute(participantNode, "id", p.ID);
                XMLHelper.SetXmlAttribute(participantNode, "name", p.Name);
                XMLHelper.SetXmlAttribute(participantNode, "code", p.Code);
                XMLHelper.SetXmlAttribute(participantNode, "outerId", p.OuterID);
            }
        }
    }
}
