using System;
using System.Xml;
using Slickflow.Engine.Utility;
using Slickflow.Engine.Business.Entity;

namespace Slickflow.Engine.Xpdl.Template
{
    public class ProcessXmlInitializer
    {
        /// <summary>
        /// Initialize bpmn file
        /// 初始化流程实体
        /// </summary>
        public static ProcessFileEntity InitNewBPMNFile()
        {
            var fileEntity = new ProcessFileEntity();
            var randomNumber = RandomSequenceGenerator.GetRandomInt4();
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
        /// Intialize bpmn file blank
        /// 初始化流程实体
        /// </summary>
        public static ProcessFileEntity InitNewBPMNFileBlank(ProcessFileEntity entity)
        {
            entity.XmlContent = GenerateXmlContentWithStartNode(entity.ProcessId, entity.ProcessName, entity.ProcessCode, entity.Version);
            return entity;
        }

        /// <summary>
        /// Generate xml content with start node
        /// 生成基本XML文档
        /// </summary>
        private static string GenerateXmlContentWithStartNode(string processId, string processName, string processCode, string version)
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
        /// Create document root element
        /// 创建根流程文档
        /// </summary>
        private static XmlDocument CreateDocumentRoot()
        {
            var xmlDoc = new XmlDocument();
            XmlElement root = xmlDoc.CreateElement(ProcessNamespaceDefine.m_namespace_prefix_bpmn, "definitions", ProcessNamespaceDefine.m_namespace_uri_bpmn);
            xmlDoc.AppendChild(root);

            root.SetAttribute("targetNamespace", ProcessNamespaceDefine.m_namespace_uri_target);
            root.SetAttribute("id", "bpmn-diagram");

            //xml namespace
            root.SetAttribute("xmlns:xsi", ProcessNamespaceDefine.m_namespace_uri_xsi);
            root.SetAttribute("xmlns:bpmndi", ProcessNamespaceDefine.m_namespace_uri_bpmndi);
            root.SetAttribute("xmlns:dc", ProcessNamespaceDefine.m_namespace_uri_dc);
            root.SetAttribute("xmlns:di", ProcessNamespaceDefine.m_namespace_uri_di);
            root.SetAttribute("xmlns:sf", ProcessNamespaceDefine.m_namespace_uri_sf);
            root.SetAttribute("xsi:schemaLocation", "http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd");

            //add xml declartion
            XmlDeclaration xmldecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDoc.InsertBefore(xmldecl, root);

            return xmlDoc;
        }
    }
}
