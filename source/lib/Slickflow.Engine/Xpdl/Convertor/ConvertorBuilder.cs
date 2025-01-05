using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slickflow.Engine.Common;
using Slickflow.Engine.Utility;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Xpdl.Common;
using Slickflow.Engine.Xpdl.Entity;

namespace Slickflow.Engine.Xpdl.Convertor
{
    /// <summary>
    /// Convertor Builder
    /// 转换器构建类
    /// </summary>
    public class ConvertorBuilder
    {
        #region Property and Constructor
        private XmlNode xmlNode;
        public XmlNode XMLNode
        {
            get
            {
                return xmlNode;
            }
        }

        private XmlNamespaceManager xmlNamespaceManager;
        public XmlNamespaceManager XMLNamespaceManager
        {
            get
            {
                return xmlNamespaceManager;
            }
        }

        private Activity mActivityEntity;
        public Activity ActivityEntity
        {
            get
            {
                return mActivityEntity;
            }
        }

        private ConvertorBuilder(XmlNode node, XmlNamespaceManager mgr, Activity entity)
        {
            xmlNode = node;
            xmlNamespaceManager = mgr;
            mActivityEntity = entity;
        }
        #endregion

        #region Get Method
        /// <summary>
        /// Get Forms Node
        /// 获取Forms的XML节点
        /// </summary>
        /// <returns></returns>
        protected XmlNode GetFormsNode()
        {
            var formsNode = XMLNode.SelectSingleNode(XPDLDefinition.Sf_StrXmlPath_Forms, XMLNamespaceManager);
            return formsNode;
        }

        /// <summary>
        /// Get Actions Node
        /// 获取Action的XML节点
        /// </summary>
        /// <returns></returns>
        protected XmlNode GetActionsNode()
        {
            var actionsNode = XMLNode.SelectSingleNode(XPDLDefinition.Sf_StrXmlPath_Actions, XMLNamespaceManager);
            return actionsNode;
        }

        /// <summary>
        /// Get Boundaries Node
        /// 获取Boundary的XML节点
        /// </summary>
        /// <returns></returns>
        protected XmlNode GetBoundariesNode()
        {
            var boundariesNode = XMLNode.SelectSingleNode(XPDLDefinition.Sf_StrXmlPath_Boundaries, XMLNamespaceManager);
            return boundariesNode;
        }

        /// <summary>
        /// Get Sections Node
        /// 获取Section的XML节点
        /// </summary>
        /// <returns></returns>
        protected XmlNode GetSectionsNode()
        {
            var sectionsNode = XMLNode.SelectSingleNode(XPDLDefinition.Sf_StrXmlPath_Sections, XMLNamespaceManager);
            return sectionsNode;
        }

        /// <summary>
        /// Get Performers Node
        /// 获取Performer的XML节点
        /// </summary>
        /// <returns></returns>
        protected XmlNode GetPerformersNode()
        {
            var performersNode = XMLNode.SelectSingleNode(XPDLDefinition.Sf_StrXmlPath_Performers, XMLNamespaceManager);
            return performersNode;
        }

        /// <summary>
        /// Get Services Node
        /// 获取Services的XML节点
        /// </summary>
        /// <returns></returns>
        protected XmlNode GetServicesNode()
        {
            var servicesNode = XMLNode.SelectSingleNode(XPDLDefinition.Sf_StrXmlPath_Services, XMLNamespaceManager);
            return servicesNode;
        }

        /// <summary>
        /// Get Scripts Node
        /// 获取Scripts的XML节点
        /// </summary>
        /// <returns></returns>
        protected XmlNode GetScriptsNode()
        {
            var scriptsNode = XMLNode.SelectSingleNode(XPDLDefinition.Sf_StrXmlPath_Scripts, XMLNamespaceManager);
            return scriptsNode;
        }

        /// <summary>
        /// Get Notifications Node
        /// 获取Notifications的XML节点
        /// </summary>
        /// <returns></returns>
        protected XmlNode GetNotificationsNode()
        {
            var notificationsNode = XMLNode.SelectSingleNode(XPDLDefinition.Sf_StrXmlPath_Notifications, XMLNamespaceManager);
            return notificationsNode;
        }
        #endregion

        #region Convert method
        /// <summary>
        /// Convert general info
        /// 转换通用属性
        /// </summary>
        /// <returns></returns>
        public ConvertorBuilder ConvertGeneral()
        {
            mActivityEntity.ID = XMLHelper.GetXmlAttribute(XMLNode, "id");
            mActivityEntity.ActivityGUID = XMLHelper.GetXmlAttribute(XMLNode, "sf:guid");
            var name = XMLHelper.GetXmlAttribute(XMLNode, "name");
            if (string.IsNullOrEmpty(name))
            {
                mActivityEntity.ActivityName = mActivityEntity.ID;
            }
            else
            {
                mActivityEntity.ActivityName = XMLHelper.GetXmlAttribute(XMLNode, "name");
            }
            mActivityEntity.ActivityCode = XMLHelper.GetXmlAttribute(XMLNode, "sf:code");
            mActivityEntity.ActivityUrl = XMLHelper.GetXmlAttribute(XMLNode, "sf:url");

            //描述信息
            //Description
            XmlNode descNode = XMLNode.SelectSingleNode("documentation");
            if (descNode != null) mActivityEntity.Description = (descNode != null) ? descNode.InnerText : string.Empty;

            return this;
        }

        /// <summary>
        /// Convert Forms
        /// 转换表单信息
        /// </summary>
        /// <returns></returns>
        public ConvertorBuilder ConvertForms()
        {
            return null;
        }

        /// <summary>
        /// Convert Actions
        /// 转换Action节点
        /// </summary>
        /// <returns></returns>
        public ConvertorBuilder ConvertActions()
        {
            var actionsNode = GetActionsNode();
            if (actionsNode != null)
            {
                var xmlActionList = actionsNode.ChildNodes;
                List<Entity.Action> actionList = new List<Entity.Action>();
                foreach (XmlNode element in xmlActionList)
                {
                    actionList.Add(ConvertXmlActionNodeToActionEntity(element));
                }
                mActivityEntity.ActionList = actionList;
            }
            return this;
        }

        /// <summary>
        /// Convert Boundires
        /// 转换Boundaries节点
        /// </summary>
        /// <returns></returns>
        public ConvertorBuilder ConvertBoundires()
        {
            var boundariesNode = GetBoundariesNode();
            if (boundariesNode != null)
            {
                XmlNodeList xmlBoundaryList = boundariesNode.ChildNodes;
                List<Boundary> boundaryList = new List<Boundary>();
                foreach (XmlNode element in xmlBoundaryList)
                {
                    boundaryList.Add(ConvertXmlBoundaryNodeToBoundaryEntity(element));
                }
                mActivityEntity.BoundaryList = boundaryList;
            }
            return this;
        }

        /// <summary>
        /// Convert Performers
        /// 转换Performers节点
        /// </summary>
        /// <returns></returns>
        public ConvertorBuilder ConvertPartakers()
        {
            var performersNode = GetPerformersNode();
            if (performersNode != null)
            {
                XmlNodeList xmlPerformerList = performersNode.ChildNodes;
                List<Partaker>  partakerList = new List<Partaker>();
                foreach (XmlNode element in xmlPerformerList)
                {
                    partakerList.Add(ConvertXmlPerformerNodeToPartakerEntity(element));
                }
                mActivityEntity.PartakerList = partakerList;
            }

            return this;
        }

        /// <summary>
        /// Convert Services
        /// 转换Services节点
        /// </summary>
        /// <returns></returns>
        public ConvertorBuilder ConvertServices()
        {
            var servicesNode = GetServicesNode();
            if (servicesNode != null)
            {
                XmlNodeList xmlServiceList = servicesNode.ChildNodes;
                List<Entity.ServiceDetail> serviceList = new List<Entity.ServiceDetail>();
                foreach (XmlNode element in xmlServiceList)
                {
                    serviceList.Add(ConvertXmlServiceNodeToServiceEntity(element));
                }
                mActivityEntity.ServiceList = serviceList;
            }
            return this;
        }

        /// <summary>
        /// Convert Scripts
        /// 转换Scripts节点
        /// </summary>
        /// <returns></returns>
        public ConvertorBuilder ConvertScripts()
        {
            var scriptsNode = GetScriptsNode();
            if (scriptsNode != null)
            {
                var xmlScriptList = scriptsNode.ChildNodes;
                List<Entity.ScriptDetail> scriptList = new List<Entity.ScriptDetail>();
                foreach (XmlNode element in xmlScriptList)
                {
                    scriptList.Add(ConvertXmlScriptNodeToScriptEntity(element));
                }
                mActivityEntity.ScriptList = scriptList;
            }
            return this;
        }

        /// <summary>
        /// Covnert Sections
        /// 转换Sections节点
        /// </summary>
        /// <returns></returns>
        public ConvertorBuilder ConvertSections()
        {
            var sectionsNode = GetSectionsNode();
            if (sectionsNode != null)
            {
                XmlNodeList xmlSectionList = sectionsNode.ChildNodes;
                List<SectionDetail> sectionList = new List<SectionDetail>();
                foreach (XmlNode element in xmlSectionList)
                {
                    sectionList.Add(ConvertXmlSectionNodeToSectionEntity(element));
                }
                mActivityEntity.SectionList = sectionList;
            }
            return this;
        }

        /// <summary>
        /// Convert Notifications
        /// 转换Notificaitons节点
        /// </summary>
        /// <returns></returns>
        public ConvertorBuilder ConvertNotifications()
        {
            var notificationsNode = GetNotificationsNode();
            if (notificationsNode != null)
            {
                XmlNodeList xmlNotificationList = notificationsNode.ChildNodes;
                List<Partaker> partakerList = new List<Partaker>();
                foreach (XmlNode element in xmlNotificationList)
                {
                    partakerList.Add(ConvertXmlNotificationNodeToPartakerEntity(element));
                }
                mActivityEntity.NotificationList = partakerList;
            }

            return this;
        }

        /// <summary>
        /// Convert Actions
        /// 转换Action实体
        /// </summary>
        /// <param name="node">XML节点</param>
        /// <returns>操作实体</returns>
        private Entity.Action ConvertXmlActionNodeToActionEntity(XmlNode node)
        {
            if (node == null) return null;

            Entity.Action action = new Entity.Action();
            var actionType = XMLHelper.GetXmlAttribute(node, "type");
            action.ActionType = EnumHelper.TryParseEnum<ActionTypeEnum>(actionType);

            var actionMethod = XMLHelper.GetXmlAttribute(node, "method");
            action.ActionMethod = EnumHelper.TryParseEnum<ActionMethodEnum>(actionMethod);

            if (action.ActionMethod != ActionMethodEnum.None)
            {
                var subMethod = XMLHelper.GetXmlAttribute(node, "subMethod");
                action.SubMethod = EnumHelper.TryParseEnum<SubMethodEnum>(subMethod);

                var fireType = XMLHelper.GetXmlAttribute(node, "fire");
                action.FireType = EnumHelper.TryParseEnum<FireTypeEnum>(fireType);

                if (action.ActionType == ActionTypeEnum.Event)
                {
                    action.Arguments = XMLHelper.GetXmlAttribute(node, "arguments");
                    action.Expression = XMLHelper.GetXmlAttribute(node, "expression");

                    if (action.ActionMethod == ActionMethodEnum.CSharpLibrary)
                    {
                        var methodInfoNode = node.SelectSingleNode("MethodInfo");
                        if (methodInfoNode != null)
                        {
                            var methodInfo = action.MethodInfo = new MethodInfo();
                            methodInfo.AssemblyFullName = XMLHelper.GetXmlAttribute(methodInfoNode, "assemblyFullName");
                            methodInfo.TypeFullName = XMLHelper.GetXmlAttribute(methodInfoNode, "typeFullName");
                            //method.ConstructorParameters = XMLHelper.GetXmlAttribute(methodNode, "constructorParameters");
                            methodInfo.MethodName = XMLHelper.GetXmlAttribute(methodInfoNode, "methodName");
                            //method.MethodParameters = XMLHelper.GetXmlAttribute(methodNode, "methodParameters");
                        }
                    }
                    else if (action.ActionMethod == ActionMethodEnum.SQL
                        || action.ActionMethod == ActionMethodEnum.Python)
                    {
                        var codeInfoNode = node.SelectSingleNode("CodeInfo");
                        if (codeInfoNode != null)
                        {
                            var codeInfo = action.CodeInfo = new CodeInfo();
                            codeInfo.CodeText = codeInfoNode.InnerText;
                            codeInfo.CodeText = codeInfo.CodeText.Replace('\t', ' ');
                        }
                    }
                }
            }
            return action;
        }

        /// <summary>
        /// Convert Service
        /// 转换Service实体
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private Entity.ServiceDetail ConvertXmlServiceNodeToServiceEntity(XmlNode node)
        {
            Entity.ServiceDetail service = new Entity.ServiceDetail();

            var serviceMethod = XMLHelper.GetXmlAttribute(node, "method");
            service.Method = EnumHelper.TryParseEnum<ServiceMethodEnum>(serviceMethod);

            var subMethod = XMLHelper.GetXmlAttribute(node, "subMethod");
            service.SubMethod = EnumHelper.TryParseEnum<SubMethodEnum>(subMethod);

            service.Arguments = XMLHelper.GetXmlAttribute(node, "argus");
            service.Expression = XMLHelper.GetXmlAttribute(node, "expression");

            if (service.Method == ServiceMethodEnum.CSharpLibrary)
            {
                var methodInfoNode = node.SelectSingleNode("MethodInfo");
                if (methodInfoNode != null)
                {
                    var methodInfo = service.MethodInfo = new MethodInfo();
                    methodInfo.AssemblyFullName = XMLHelper.GetXmlAttribute(methodInfoNode, "assemblyFullName");
                    methodInfo.TypeFullName = XMLHelper.GetXmlAttribute(methodInfoNode, "typeFullName");
                    //method.ConstructorParameters = XMLHelper.GetXmlAttribute(methodNode, "constructorParameters");
                    methodInfo.MethodName = XMLHelper.GetXmlAttribute(methodInfoNode, "methodName");
                    //method.MethodParameters = XMLHelper.GetXmlAttribute(methodNode, "methodParameters");
                }
            }
            return service;
        }

        /// <summary>
        /// Convert Script Node
        /// 转换脚本任务节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private Entity.ScriptDetail ConvertXmlScriptNodeToScriptEntity(XmlNode node)
        {
            Entity.ScriptDetail script = new Entity.ScriptDetail();

            var scriptMethod = XMLHelper.GetXmlAttribute(node, "method");
            script.Method = EnumHelper.TryParseEnum<ScriptMethodEnum>(scriptMethod);

            script.Arguments = XMLHelper.GetXmlAttribute(node, "argus");
            script.ScriptText = node.InnerText;

            return script;
        }

        /// <summary>
        /// Convert Partaker
        /// 转换Partaker节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private Partaker ConvertXmlPerformerNodeToPartakerEntity(XmlNode node)
        {
            Partaker partaker = new Partaker();
            var type = XMLHelper.GetXmlAttribute(node, "outerType");
            partaker.OuterType = type;
            partaker.OuterID = XMLHelper.GetXmlAttribute(node, "outerId");
            partaker.Name = XMLHelper.GetXmlAttribute(node, "name");
            partaker.OuterCode = XMLHelper.GetXmlAttribute(node, "outerCode");

            return partaker;
        }

        /// <summary>
        /// Convert Boundary
        /// 转换Boundary节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private Boundary ConvertXmlBoundaryNodeToBoundaryEntity(XmlNode node)
        {
            Boundary boundary = new Boundary();
            var eventType = XMLHelper.GetXmlAttribute(node, "event");
            boundary.EventTriggerType = EnumHelper.TryParseEnum<EventTriggerEnum>(eventType);

            if (boundary.EventTriggerType == EventTriggerEnum.Timer)
            {
                boundary.Expression = XMLHelper.GetXmlAttribute(node, "expression");
            }
            return boundary;
        }

        /// <summary>
        /// Convert Section
        /// 转换Section节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private SectionDetail ConvertXmlSectionNodeToSectionEntity(XmlNode node)
        {
            SectionDetail section = new SectionDetail();
            section.Name = XMLHelper.GetXmlAttribute(node, "name");
            section.Value = node.InnerText;

            return section;
        }

        /// <summary>
        /// Convert Partaker
        /// 转换Partaker节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private Partaker ConvertXmlNotificationNodeToPartakerEntity(XmlNode node)
        {
            Partaker partaker = new Partaker();
            var type = XMLHelper.GetXmlAttribute(node, "outerType");
            partaker.OuterType = type;
            partaker.OuterID = XMLHelper.GetXmlAttribute(node, "outerId");
            partaker.Name = XMLHelper.GetXmlAttribute(node, "name");
            partaker.OuterCode = XMLHelper.GetXmlAttribute(node, "outerCode");

            return partaker;
        }
        #endregion

        /// <summary>
        /// Create method
        /// </summary>
        public static ConvertorBuilder Create(XmlNode xmlNode, XmlNamespaceManager xnpmgr, Activity activityEntity)
        {
            if (xmlNode == null 
                || xnpmgr == null
                || activityEntity == null)
            {
                throw new ArgumentNullException("Argument can't be null when converting xmlnode to activity entity");
            }

            var builder = new ConvertorBuilder(xmlNode, xnpmgr, activityEntity);
            return builder;
        }
    }
}
