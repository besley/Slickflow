using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Common;
using Slickflow.Engine.Utility;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Xpdl.Common;
using Slickflow.Engine.Xpdl.Template;
using System.Xml;

namespace Slickflow.Graph.Model
{
    /// <summary>
    /// Process XML builder: builds BPMN process and diagram XML from an in-memory Flow model.
    /// </summary>
    public class ProcessXmlBuilder
    {
        #region Property and Constructor
        private Flow _flow = null;
        private ProcessEntity _proecessEntity = null;
        private XmlDocument _processXmlDoc = new XmlDocument();
        private readonly IBpmnDiagramLayout _layout;

        internal ProcessXmlBuilder(Flow flow)
            : this(flow, DiagramLayoutProvider.Current)
        { }

        internal ProcessXmlBuilder(Flow flow, IBpmnDiagramLayout layout)
        {
            _flow = flow;
            _layout = layout ?? DiagramLayoutProvider.Current;
        }
        #endregion

        /// <summary>
        /// Serialize the in-memory flow into BPMN XML string.
        /// </summary>
        internal string Serialize()
        {
            if (_flow == null || string.IsNullOrEmpty(_flow.ProcessId))
                throw new ApplicationException("The process diagram can't be null,please check its content firstly!");

            //create process body xml
            CreateProcessXml(_flow.ProcessId, _flow.Name, _flow.Code, _flow.Version);

            //create activity xml
            foreach (var node in _flow.Nodes)
            {
                CreateActivity(node);
            }

            //create transition xml
            foreach (var edge in _flow.Edges)
            {
                CreateTransition(edge);

                CreateEdge(edge);
            }

            // Use custom serialization to ensure correct attribute order
            var xmlContent = _processXmlDoc.OuterXml;
            _proecessEntity.XmlContent = xmlContent;

            return xmlContent;
        }

        /// <summary>
        /// Create process XML document structure and initialize ProcessEntity metadata.
        /// </summary>
        private ProcessXmlBuilder CreateProcessXml(string processId, string name, string code, string version)
        {
            if (string.IsNullOrEmpty(version)) version = "1";

            _proecessEntity = new ProcessEntity();
            _proecessEntity.ProcessId = processId;
            _proecessEntity.ProcessName = name;
            _proecessEntity.ProcessCode = code;
            _proecessEntity.Version = version;
            _proecessEntity.Status = 1;

            var xmlDoc = CreateDocumentRoot();
            var root = xmlDoc.DocumentElement;

            var processNode = xmlDoc.CreateElement(ProcessNamespaceDefine.m_namespace_prefix_bpmn, "process", ProcessNamespaceDefine.m_namespace_uri_bpmn);
            root.AppendChild(processNode);
            processNode.SetAttribute("id", processId);
            processNode.SetAttribute("name", name);
            processNode.SetAttribute("isExecutable", "true");
            processNode.SetAttribute("code", ProcessNamespaceDefine.m_namespace_uri_sf, code);
            processNode.SetAttribute("version", ProcessNamespaceDefine.m_namespace_uri_sf, version);

            //bpmn diagram
            var diagramNode = xmlDoc.CreateElement("bpmndi", "BPMNDiagram", ProcessNamespaceDefine.m_namespace_uri_bpmndi);
            root.AppendChild(diagramNode);
            diagramNode.SetAttribute("id", "BPMNDiagram_1");

            var planeNode = xmlDoc.CreateElement("bpmndi", "BPMNPlane", ProcessNamespaceDefine.m_namespace_uri_bpmndi);
            diagramNode.AppendChild(planeNode);
            planeNode.SetAttribute("id", "BPMNPlane_1");
            planeNode.SetAttribute("bpmnElement", processId);

            _processXmlDoc = xmlDoc;
            _proecessEntity.XmlContent = xmlDoc.OuterXml;
            return this;
        }

        /// <summary>
        /// Get XML namespace manager for Slickflow BPMN document.
        /// </summary>
        private XmlNamespaceManager GetSlickflowGraphXmlNamespaceManager(XmlDocument document)
        {
            var nsmgr = new XmlNamespaceManager(document.NameTable);
            nsmgr.AddNamespace(ProcessNamespaceDefine.m_namespace_prefix_bpmn, ProcessNamespaceDefine.m_namespace_uri_bpmn);
            nsmgr.AddNamespace(ProcessNamespaceDefine.m_namespace_prefix_bpmndi, ProcessNamespaceDefine.m_namespace_uri_bpmndi);
            nsmgr.AddNamespace(ProcessNamespaceDefine.m_namespace_prefix_di, ProcessNamespaceDefine.m_namespace_uri_di);
            nsmgr.AddNamespace(ProcessNamespaceDefine.m_namespace_prefix_dc, ProcessNamespaceDefine.m_namespace_uri_dc);
            nsmgr.AddNamespace(ProcessNamespaceDefine.m_namespace_prefix_sf, ProcessNamespaceDefine.m_namespace_uri_sf);

            return nsmgr;
        }

        /// <summary>
        /// Initialize a new BPMN file entity with default process id/name/code.
        /// </summary>
        public static ProcessFileEntity InitNewBPMNFile()
        {
            var fileEntity = new ProcessFileEntity();
            var randomNumber = Utility.GetRandomInt();
            fileEntity.ProcessId = String.Format("Process_{0}", randomNumber.ToString());
            fileEntity.ProcessName = String.Format("Process_Name_{0}", randomNumber.ToString());
            fileEntity.ProcessCode = String.Format("Process_Code_{0}", randomNumber.ToString());
            fileEntity.Version = "1";
            fileEntity.Status = 1;
            fileEntity.XmlContent = GenerateXmlContentWithStartNode(fileEntity.ProcessId, 
                fileEntity.ProcessName, 
                fileEntity.ProcessCode, 
                fileEntity.Version);

            return fileEntity;
        }

        /// <summary>
        /// Generate XML content containing only a start node for a new process.
        /// </summary>
        public static string GenerateXmlContentWithStartNode(string processId, string processName, string processCode, string version)
        {
            var xmlDoc = CreateDocumentRoot();
            var root = xmlDoc.DocumentElement;

            var processNode = xmlDoc.CreateElement(ProcessNamespaceDefine.m_namespace_prefix_bpmn, "process", ProcessNamespaceDefine.m_namespace_uri_bpmn);
            root.AppendChild(processNode);
            processNode.SetAttribute("id", processId);
            processNode.SetAttribute("name", processName);
            processNode.SetAttribute("isExecutable", "true");
            processNode.SetAttribute("code", ProcessNamespaceDefine.m_namespace_uri_sf, processCode);
            processNode.SetAttribute("version", ProcessNamespaceDefine.m_namespace_uri_sf, version);

            //start node
            var startNode = xmlDoc.CreateElement(ProcessNamespaceDefine.m_namespace_prefix_bpmn, "startEvent", ProcessNamespaceDefine.m_namespace_uri_bpmn);
            processNode.AppendChild(startNode);
            startNode.SetAttribute("id", "StartEvent_1");
            startNode.SetAttribute("name", "Start");

            //bpmn diagram
            var diagramNode = xmlDoc.CreateElement("bpmndi", "BPMNDiagram", ProcessNamespaceDefine.m_namespace_uri_bpmndi);
            root.AppendChild(diagramNode);
            diagramNode.SetAttribute("id", "BPMNDiagram_1");

            var planeNode = xmlDoc.CreateElement("bpmndi", "BPMNPlane", ProcessNamespaceDefine.m_namespace_uri_bpmndi);
            diagramNode.AppendChild(planeNode);
            planeNode.SetAttribute("id", "BPMNPlane_1");
            planeNode.SetAttribute("bpmnElement", processId);

            var shapeNode = xmlDoc.CreateElement("bpmndi", "BPMNShape", ProcessNamespaceDefine.m_namespace_uri_bpmndi);
            planeNode.AppendChild(shapeNode);
            shapeNode.SetAttribute("id", "_BPMNShape_StartEvent_2");
            shapeNode.SetAttribute("bpmnElement", "StartEvent_1");

            var boundsNode = xmlDoc.CreateElement("dc", "Bounds", ProcessNamespaceDefine.m_namespace_uri_dc);
            shapeNode.AppendChild(boundsNode);

            boundsNode.SetAttribute("height", "36.0");
            boundsNode.SetAttribute("width", "36.0");
            boundsNode.SetAttribute("x", "412.0");
            boundsNode.SetAttribute("y", "240.0");

            var xmlContent = xmlDoc.OuterXml;

            return xmlContent;
        }

        /// <summary>
        /// Generate a blank BPMN XML document (no activities, only definitions and diagram root).
        /// </summary>
        private static XmlDocument GenerateXmlContentBlank(string processId)
        {
            var xmlDoc = CreateDocumentRoot();
            var root = xmlDoc.DocumentElement;

            var processNode = xmlDoc.CreateElement(ProcessNamespaceDefine.m_namespace_prefix_bpmn, "process", ProcessNamespaceDefine.m_namespace_uri_bpmn);
            root.AppendChild(processNode);

            //bpmn diagram
            var diagramNode = xmlDoc.CreateElement("bpmndi", "BPMNDiagram", ProcessNamespaceDefine.m_namespace_uri_bpmndi);
            root.AppendChild(diagramNode);
            diagramNode.SetAttribute("id", "BPMNDiagram_1");

            var planeNode = xmlDoc.CreateElement("bpmndi", "BPMNPlane", ProcessNamespaceDefine.m_namespace_uri_bpmndi);
            diagramNode.AppendChild(planeNode);
            planeNode.SetAttribute("id", "BPMNPlane_1");
            planeNode.SetAttribute("bpmnElement", processId);

            return xmlDoc;
        }
        
        /// <summary>
        /// Create BPMN definitions root element with namespaces and schema declarations.
        /// </summary>
        private static XmlDocument CreateDocumentRoot()
        {
            var xmlDoc = new XmlDocument();
            
            //add xml declaration first
            XmlDeclaration xmldecl = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
            xmlDoc.AppendChild(xmldecl);

            //create root element with bpmn namespace
            XmlElement root = xmlDoc.CreateElement(ProcessNamespaceDefine.m_namespace_prefix_bpmn, "definitions", ProcessNamespaceDefine.m_namespace_uri_bpmn);
            xmlDoc.AppendChild(root);

            //xml namespace declarations - must be in correct order
            //All xmlns declarations must come first, before other attributes
            root.SetAttribute("xmlns:xsi", ProcessNamespaceDefine.m_namespace_uri_xsi);
            root.SetAttribute("xmlns:bpmndi", ProcessNamespaceDefine.m_namespace_uri_bpmndi);
            root.SetAttribute("xmlns:dc", ProcessNamespaceDefine.m_namespace_uri_dc);
            root.SetAttribute("xmlns:di", ProcessNamespaceDefine.m_namespace_uri_di);
            root.SetAttribute("xmlns:sf", ProcessNamespaceDefine.m_namespace_uri_sf);
            root.SetAttribute("xmlns:bpmn", ProcessNamespaceDefine.m_namespace_uri_bpmn);
            
            //Other attributes after xmlns declarations
            root.SetAttribute("id", "bpmn-diagram");
            root.SetAttribute("targetNamespace", ProcessNamespaceDefine.m_namespace_uri_target);
            root.SetAttribute("xsi:schemaLocation", "http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd");

            return xmlDoc;
        }

        /// <summary>
        /// Create BPMN activity and its diagram shape for the given node.
        /// </summary>
        private ProcessXmlBuilder CreateActivity(Node node)
        {
            // Construct process information nodes
            CreateActivityXmlNode(_processXmlDoc, node);

            // Construct graphical (diagram) information nodes
            CreateShapeNode(_processXmlDoc, node);

            return this;
        }

        /// <summary>
        /// Determine whether the node is an event-type node.
        /// </summary>
        private Boolean IsEventNode(Node v)
        {
            if (v.NodeType == ActivityTypeEnum.StartNode
                || v.NodeType == ActivityTypeEnum.IntermediateNode
                || v.NodeType == ActivityTypeEnum.EndNode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Determine whether the node is a gateway-type node.
        /// </summary>
        private Boolean IsGatewayNode(Node v)
        {
            if (v.NodeType == ActivityTypeEnum.GatewayNode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Get activity (node) type.
        /// </summary>
        private ActivityTypeEnum GetActivityType(Node v)
        {
            return v.NodeType;
        }

        /// <summary>
        /// Create BPMN activity XML node.
        /// </summary>
        private void CreateActivityXmlNode(XmlDocument xmlDoc,
            Node node)
        {
            XmlElement activityNode = xmlDoc.CreateElement(ProcessNamespaceDefine.m_namespace_prefix_bpmn, 
                    GetActivityPrefixName(node), 
                    XPDLDefinition.BPMN_NameSpacePrefix_Value);
            activityNode.SetAttribute("id", node.Id);
            activityNode.SetAttribute("name", node.Name);
            activityNode.SetAttribute("code", ProcessNamespaceDefine.m_namespace_uri_sf, node.Code);

            // extensionElements must be first child for ServiceTask so ConvertorFactory can distinguish ServiceNode vs AIServiceNode
            // Schema aligned with sfd frontend: sf:variables before sf:services for ServiceNode; sf:aiServices first for AIServiceNode
            if (node.NodeType == ActivityTypeEnum.ServiceNode && node.Activity.ServiceList != null && node.Activity.ServiceList.Count > 0)
            {
                XmlElement extensionNode = xmlDoc.CreateElement(ProcessNamespaceDefine.m_namespace_prefix_bpmn,
                    "extensionElements", XPDLDefinition.BPMN_NameSpacePrefix_Value);
                XmlElement variablesNode = xmlDoc.CreateElement("sf", "variables", XPDLDefinition.Sf_NameSpacePrefix_Value);
                extensionNode.AppendChild(variablesNode);
                XmlElement servicesNode = xmlDoc.CreateElement("sf", "services", XPDLDefinition.Sf_NameSpacePrefix_Value);
                extensionNode.AppendChild(servicesNode);
                foreach (var svc in node.Activity.ServiceList)
                {
                    XmlElement serviceNode = xmlDoc.CreateElement("sf", "service", XPDLDefinition.Sf_NameSpacePrefix_Value);
                    serviceNode.SetAttribute("methodType", svc.Method.ToString());
                    if (svc.SubMethod != SubMethodEnum.None)
                        serviceNode.SetAttribute("subMethod", svc.SubMethod.ToString());
                    if (!string.IsNullOrEmpty(svc.Arguments))
                        serviceNode.SetAttribute("argus", svc.Arguments);
                    if (!string.IsNullOrEmpty(svc.Expression))
                        serviceNode.SetAttribute("expression", svc.Expression);
                    servicesNode.AppendChild(serviceNode);
                }
                activityNode.AppendChild(extensionNode);
            }
            else if (node.NodeType == ActivityTypeEnum.AIServiceNode && node.Activity.AIServiceList != null && node.Activity.AIServiceList.Count > 0)
            {
                XmlElement extensionNode = xmlDoc.CreateElement(ProcessNamespaceDefine.m_namespace_prefix_bpmn,
                    "extensionElements", XPDLDefinition.BPMN_NameSpacePrefix_Value);
                XmlElement aiServicesNode = xmlDoc.CreateElement("sf", "aiServices", XPDLDefinition.Sf_NameSpacePrefix_Value);
                extensionNode.AppendChild(aiServicesNode);
                foreach (var aiSvc in node.Activity.AIServiceList)
                {
                    XmlElement aiServiceNode = xmlDoc.CreateElement("sf", "aiService", XPDLDefinition.Sf_NameSpacePrefix_Value);
                    aiServiceNode.SetAttribute("type", aiSvc.AIServiceType.ToString());
                    if (!string.IsNullOrEmpty(aiSvc.ConfigUUID))
                        aiServiceNode.SetAttribute("configUUID", aiSvc.ConfigUUID);
                    aiServicesNode.AppendChild(aiServiceNode);
                }
                XmlElement variablesNode = xmlDoc.CreateElement("sf", "variables", XPDLDefinition.Sf_NameSpacePrefix_Value);
                extensionNode.AppendChild(variablesNode);
                activityNode.AppendChild(extensionNode);
            }

            //incoming 
            var lstIncoming = GetNodeIncoming(node);
            foreach (var incoming in lstIncoming)
            {
                XmlElement incomingNode = xmlDoc.CreateElement(ProcessNamespaceDefine.m_namespace_prefix_bpmn,
                    "incoming", XPDLDefinition.BPMN_NameSpacePrefix_Value);
                incomingNode.InnerText = incoming;
                activityNode.AppendChild(incomingNode);
            }

            //outgoing
            var lstOutgoing = GetNodeOutgoing(node);
            foreach (var outgoing in lstOutgoing)
            {
                XmlElement outgoingNode = xmlDoc.CreateElement(ProcessNamespaceDefine.m_namespace_prefix_bpmn,
                    "outgoing", XPDLDefinition.BPMN_NameSpacePrefix_Value);
                outgoingNode.InnerText = outgoing;
                activityNode.AppendChild(outgoingNode);
            }

            if (node.NodeType == ActivityTypeEnum.MultiSignNode)
            {
                XmlElement multiNode = xmlDoc.CreateElement(ProcessNamespaceDefine.m_namespace_prefix_bpmn,
                    "multiInstanceLoopCharacteristics", XPDLDefinition.BPMN_NameSpacePrefix_Value);
                activityNode.AppendChild(multiNode);
            }
            else if (node.NodeType == ActivityTypeEnum.GatewayNode)
            {
                if (node.Activity.GatewayDetail.DirectionType == GatewayDirectionEnum.AndSplitMI)
                {
                    XmlElement extensionNode = xmlDoc.CreateElement(ProcessNamespaceDefine.m_namespace_prefix_bpmn,
                        "extensionElements", XPDLDefinition.BPMN_NameSpacePrefix_Value);
                    activityNode.AppendChild(extensionNode);

                    XmlElement gatewayDetailNode = xmlDoc.CreateElement("sf",
                        "gatewayDetail", XPDLDefinition.Sf_NameSpacePrefix_Value);
                    gatewayDetailNode.SetAttribute("extraSplitType", "AndSplitMI");
                    extensionNode.AppendChild(gatewayDetailNode);
                }
                else if(node.Activity.GatewayDetail.DirectionType == GatewayDirectionEnum.AndJoinMI)
                {
                    XmlElement extensionNode = xmlDoc.CreateElement(ProcessNamespaceDefine.m_namespace_prefix_bpmn,
                        "extensionElements", XPDLDefinition.BPMN_NameSpacePrefix_Value);
                    activityNode.AppendChild(extensionNode);

                    XmlElement gatewayDetailNode = xmlDoc.CreateElement("sf",
                        "gatewayDetail", XPDLDefinition.Sf_NameSpacePrefix_Value);
                    gatewayDetailNode.SetAttribute("extraJoinType", "AndJoinMI");
                    extensionNode.AppendChild(gatewayDetailNode);
                }
            }
            var root = xmlDoc.DocumentElement;
            var processNode = root.SelectSingleNode(XPDLDefinition.BPMN_StrXmlPath_Process,
                XPDLHelper.GetSlickflowXmlNamespaceManager(xmlDoc));
            processNode.AppendChild(activityNode);
        }

        /// <summary>
        /// Get activity prefix name (BPMN element name) for a node.
        /// </summary>
        private string GetActivityPrefixName(Node node)
        {
            var name = string.Empty;
            var activityType = node.NodeType;
            if (activityType == ActivityTypeEnum.StartNode)
            {
                name = "startEvent";
            }
            else if (activityType == ActivityTypeEnum.EndNode)
            {
                name = "endEvent";
            } 
            else if (activityType == ActivityTypeEnum.IntermediateNode)
            {
                name = "intermediateCatchEvent";
            }
            else if (activityType == ActivityTypeEnum.TaskNode)
            {
                name = "task";
            }
            else if (activityType == ActivityTypeEnum.GatewayNode)
            {
                if (node.Activity.GatewayDetail.DirectionType == GatewayDirectionEnum.AndSplit
                    || node.Activity.GatewayDetail.DirectionType == GatewayDirectionEnum.AndJoin
                    || node.Activity.GatewayDetail.DirectionType == GatewayDirectionEnum.AndSplitMI
                    || node.Activity.GatewayDetail.DirectionType == GatewayDirectionEnum.AndJoinMI)
                {
                    name = "parallelGateway";
                } 
                else if (node.Activity.GatewayDetail.DirectionType == GatewayDirectionEnum.OrSplit
                    || node.Activity.GatewayDetail.DirectionType == GatewayDirectionEnum.OrJoin)
                {
                    name = "inclusiveGateway";
                }
                else if (node.Activity.GatewayDetail.DirectionType == GatewayDirectionEnum.XOrSplit
                    || node.Activity.GatewayDetail.DirectionType == GatewayDirectionEnum.XOrJoin)
                {
                    name = "exclusiveGateway";
                }
                else
                {
                    throw new ApplicationException(String.Format("NOT supported gateway type:{0}", node.Activity.GatewayDetail.DirectionType.ToString()));
                }
            }
            else if (activityType == ActivityTypeEnum.MultiSignNode)
            {
                name = "task";
            }
            else if (activityType == ActivityTypeEnum.SubProcessNode)
            {
                name = "subProcess";
            }
            else if (activityType == ActivityTypeEnum.ServiceNode || activityType == ActivityTypeEnum.AIServiceNode)
            {
                name = "serviceTask";
            }
            else
            {
                throw new ApplicationException(String.Format("NOT supported activity type:{0}", node.NodeType.ToString()));
            }
            return name;
        }

        /// <summary>
        /// Get vertex incoming sequence-flow ids.
        /// </summary>
        private List<string> GetNodeIncoming(Node node)
        {
            var lstIncoming = new List<string>();
            var edges = _flow.Edges;
            foreach (var edge in edges)
            {
                if (edge.Target.Id == node.Id)
                {
                    lstIncoming.Add(edge.Id);
                }
            }
            return lstIncoming;
        }

        /// <summary>
        /// Get vertex outgoing sequence-flow ids.
        /// </summary>
        private List<string> GetNodeOutgoing(Node node)
        {
            var lstOutgoing = new List<string>();
            var edges = _flow.Edges;
            foreach (var edge in edges)
            {
                if (edge.Source.Id == node.Id)
                {
                   lstOutgoing.Add(edge.Id);
                }
            }
            return lstOutgoing;
        }

        /// <summary>
        /// Create BPMNShape diagram node for an activity.
        /// </summary>
        private void CreateShapeNode(XmlDocument xmlDoc,
            Node node)
        {
            XmlElement shapeNode = xmlDoc.CreateElement("bpmndi", "BPMNShape", ProcessNamespaceDefine.m_namespace_uri_bpmndi);
            shapeNode.SetAttribute("id", string.Format("{0}_di", node.Id));
            shapeNode.SetAttribute("bpmnElement", node.Id);

            var boundsNode = xmlDoc.CreateElement("dc", "Bounds", ProcessNamespaceDefine.m_namespace_uri_dc);
            shapeNode.AppendChild(boundsNode);

            boundsNode.SetAttribute("x", node.Left.ToString());
            boundsNode.SetAttribute("y", node.Top.ToString());
            boundsNode.SetAttribute("width", node.Width.ToString());
            boundsNode.SetAttribute("height", node.Height.ToString());

            var labelNode = xmlDoc.CreateElement("bpmndi", "BPMNLabel", ProcessNamespaceDefine.m_namespace_uri_bpmndi);
            shapeNode.AppendChild(labelNode);

            var root = xmlDoc.DocumentElement;
            var diagramNode = root.SelectSingleNode(ProcessNamespaceDefine.m_bpmndi_node_diagram,
                GetSlickflowGraphXmlNamespaceManager(xmlDoc));
            var planeNode = diagramNode.SelectSingleNode(ProcessNamespaceDefine.m_bpmndi_node_plane,
                GetSlickflowGraphXmlNamespaceManager(xmlDoc));
            planeNode.AppendChild(shapeNode);

            if (node.NodeType == ActivityTypeEnum.SubProcessNode)
            {
                //subprocess diagram
                var subDiagramNode = xmlDoc.CreateElement("bpmndi", "BPMNDiagram", ProcessNamespaceDefine.m_namespace_uri_bpmndi);
                root.AppendChild(subDiagramNode);
                subDiagramNode.SetAttribute("id", string.Format("{0}_{1}", "BPMNDiagram", Utility.GetRandomString(7)));

                var subPlaneNode = xmlDoc.CreateElement("bpmndi", "BPMNPlane", ProcessNamespaceDefine.m_namespace_uri_bpmndi);
                subDiagramNode.AppendChild(subPlaneNode);
                subPlaneNode.SetAttribute("id", string.Format("{0}_{1}", "BPMNPlane", Utility.GetRandomString(7)));
                subPlaneNode.SetAttribute("bpmnElement", node.Id);
            }
        }

        /// <summary>
        /// Create BPMN sequenceFlow (transition) node.
        /// </summary>
        private void CreateTransition(Edge edge)
        {
            var sequenceFlowNode = _processXmlDoc.CreateElement(ProcessNamespaceDefine.m_namespace_prefix_bpmn, "sequenceFlow", ProcessNamespaceDefine.m_namespace_uri_bpmn);

            XMLHelper.SetXmlAttribute(sequenceFlowNode, "id", edge.Id);
            XMLHelper.SetXmlAttribute(sequenceFlowNode, "name", edge.Transition.Description);

            sequenceFlowNode.SetAttribute("sourceRef", edge.Source.Id);
            sequenceFlowNode.SetAttribute("targetRef", edge.Target.Id);

            // Create condition node when edge has Condition definition
            if (edge.Transition.Condition != null)
            {
                // Create ConditionExpression node
                var bpmnConditionNode = _processXmlDoc.CreateElement(ProcessNamespaceDefine.m_namespace_prefix_bpmn, "conditionExpression", XPDLDefinition.BPMN_NameSpacePrefix_Value);
                sequenceFlowNode.AppendChild(bpmnConditionNode);

                if (!string.IsNullOrEmpty(edge.Transition.Condition.ConditionText))
                {
                    bpmnConditionNode.InnerText = edge.Transition.Condition.ConditionText;
                }
            }

            var root = _processXmlDoc.DocumentElement;
            var processNode = root.SelectSingleNode(ProcessNamespaceDefine.m_bpmn_node_process,
                GetSlickflowGraphXmlNamespaceManager(_processXmlDoc));
            processNode.AppendChild(sequenceFlowNode);

        }

        /// <summary>
        /// Create BPMNEdge diagram element for a sequenceFlow.
        /// Waypoints are computed by the configured diagram layout (Community: simple two-point; Enterprise: full routing).
        /// </summary>
        private void CreateEdge(Edge edge)
        {
            var edgeNode = _processXmlDoc.CreateElement("bpmndi", "BPMNEdge", ProcessNamespaceDefine.m_namespace_uri_bpmndi);
            XMLHelper.SetXmlAttribute(edgeNode, "id", String.Format("{0}_di", edge.Id));
            XMLHelper.SetXmlAttribute(edgeNode, "bpmnElement", edge.Id);

            _layout.AppendEdgeWaypoints(_flow, edge, edgeNode, _processXmlDoc);

            var root = _processXmlDoc.DocumentElement;
            var diagramNode = root.SelectSingleNode(ProcessNamespaceDefine.m_bpmndi_node_diagram,
                GetSlickflowGraphXmlNamespaceManager(_processXmlDoc));
            var planeNode = diagramNode.SelectSingleNode(ProcessNamespaceDefine.m_bpmndi_node_plane,
                GetSlickflowGraphXmlNamespaceManager(_processXmlDoc));
            planeNode.AppendChild(edgeNode);
        }
    }
}