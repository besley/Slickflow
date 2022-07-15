using System;
using System.Xml;
using System.Xml.Schema;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Engine.Business.Entity;

namespace Slickflow.Graph.BPMN
{
    public class ProcessXmlBuilder
    {
        /// <summary>
        /// 初始化流程实体
        /// </summary>
        /// <returns></returns>
        public static ProcessFileEntity InitNewBPMNFile()
        {
            return GenerateXmlContent();
        }

        /// <summary>
        /// 生成基本XML文档
        /// </summary>
        /// <returns>XML文档</returns>
        private static ProcessFileEntity GenerateXmlContent()
        {
            var fileEntity = new ProcessFileEntity();
            fileEntity.ProcessGUID = Guid.NewGuid().ToString();

            var randomNumber = GetRandomInt();
            fileEntity.ProcessName = String.Format("Process_Name_{0}", randomNumber.ToString());
            fileEntity.ProcessCode = String.Format("Process_Code_{0}", randomNumber.ToString());
            fileEntity.Version = "1";
            fileEntity.IsUsing = 1;

            var xmlDoc = new XmlDocument();
            string namespaceUri = "http://www.omg.org/spec/BPMN/20100524/MODEL";
            XmlElement root = xmlDoc.CreateElement("bpmn2", "definitions", namespaceUri);
            xmlDoc.AppendChild(root);

            root.SetAttribute("targetNamespace", "http://bpmn.io/schema/bpmn");
            root.SetAttribute("id", "bpmn2-diagram");

            //xml namespace
            root.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
            root.SetAttribute("xmlns:bpmndi", "http://www.omg.org/spec/BPMN/20100524/DI");
            root.SetAttribute("xmlns:dc", "http://www.omg.org/spec/DD/20100524/DC");
            root.SetAttribute("xmlns:di", "http://www.omg.org/spec/DD/20100524/DI");
            root.SetAttribute("xsi:schemaLocation", "http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd");

            //add xml declartion
            XmlDeclaration xmldecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDoc.InsertBefore(xmldecl, root);

            var processNode = xmlDoc.CreateElement("bpmn2", "process", namespaceUri);
            root.AppendChild(processNode);

            processNode.SetAttribute("isExecutable", "false");
            processNode.SetAttribute("id", fileEntity.ProcessCode);
            processNode.SetAttribute("guid", fileEntity.ProcessGUID);
            processNode.SetAttribute("name", fileEntity.ProcessName);
            processNode.SetAttribute("code", fileEntity.ProcessCode);
            processNode.SetAttribute("version", fileEntity.Version);

            var startNode = xmlDoc.CreateElement("bpmn2", "startEvent", namespaceUri);
            processNode.AppendChild(startNode);
            startNode.SetAttribute("id", "StartEvent_1");
            startNode.SetAttribute("guid", Guid.NewGuid().ToString());

            //bpmn diagram
            var bpmndiNameSpaceUri = "http://www.omg.org/spec/BPMN/20100524/DI";
            var diagramNode = xmlDoc.CreateElement("bpmndi", "BPMNDiagram", bpmndiNameSpaceUri);
            root.AppendChild(diagramNode);
            diagramNode.SetAttribute("id", "BPMNDiagram_1");

            var planeNode = xmlDoc.CreateElement("bpmndi", "BPMNPlane", bpmndiNameSpaceUri);
            diagramNode.AppendChild(planeNode);
            planeNode.SetAttribute("id", "BPMNPlane_1");
            planeNode.SetAttribute("bpmnElement", "Process_100");

            var shapeNode = xmlDoc.CreateElement("bpmndi", "BPMNShape", bpmndiNameSpaceUri);
            planeNode.AppendChild(shapeNode);
            shapeNode.SetAttribute("id", "_BPMNShape_StartEvent_2");
            shapeNode.SetAttribute("bpmnElement", "StartEvent_1");

            var dcNameSpaceUri = "http://www.omg.org/spec/DD/20100524/DC";
            var boundsNode = xmlDoc.CreateElement("dc", "Bounds", dcNameSpaceUri);
            shapeNode.AppendChild(boundsNode);

            boundsNode.SetAttribute("height", "36.0");
            boundsNode.SetAttribute("width", "36.0");
            boundsNode.SetAttribute("x", "412.0");
            boundsNode.SetAttribute("y", "240.0");

            fileEntity.XmlContent = xmlDoc.InnerXml;

            return fileEntity;
        }

        private static int GetRandomInt()
        {
            Random r = new Random();
            int value = r.Next(1000, 9999);
            return value;
        }
    }
}