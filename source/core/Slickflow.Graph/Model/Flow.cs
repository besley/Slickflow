using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Slickflow.Engine.Common;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Engine.Utility;
using System.Diagnostics;
using System.Runtime.Intrinsics.X86;
using System;

namespace Slickflow.Graph.Model
{
    /// <summary>
    /// Process Entity
    /// </summary>
    public class Flow
    {
        #region Properties and Constructor
        public string Name { get; set; }
        public string Code { get; set; }
        public string Version { get; set; }
        public string ProcessId { get; set; }
        public List<Partaker> PartakerList { get; set; }
        public List<Node> Nodes { get; } = new List<Node>();
        public List<Edge> Edges { get; } = new List<Edge>();

        /// <summary>
        /// Flow Constructor
        /// </summary>
        public Flow(string name, string code, string processId, string version)
        {
            Name = name;
            Code = code;
            ProcessId = processId;
            Version = version;
            PartakerList = new List<Partaker>();
        }

        /// <summary>
        /// Load Xml Conent, and Fill into Flow, Vertices and Links
        /// </summary>
        /// <param name="entity">Process Entity</param>
        internal void Load(ProcessEntity entity)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(entity.XmlContent);
            var root = xmlDoc.DocumentElement;

            var xmlNodeProcess = root.SelectSingleNode(XPDLDefinition.BPMN_StrXmlPath_Process,
                XPDLHelper.GetSlickflowXmlNamespaceManager(xmlDoc, true));
            var xmlNodePlane = root.SelectSingleNode(XPDLDefinition.BPMN_StrXmlPath_DiagramPlane,
                XPDLHelper.GetSlickflowXmlNamespaceManager(xmlDoc, true));
            var process = ProcessModelConvertor.ConvertProcessModelFromXML(xmlNodeProcess);

            foreach (var activity in process.ActivityList)
            {
                var node = new Node(activity.ActivityType, activity.ActivityName, activity.ActivityCode);
                node.Activity = activity;
                SetNodePosition(xmlNodePlane, activity, node);
                Nodes.Add(node);
            }

            foreach (var transition in process.TransitionList)
            {
                var edge = new Edge();
                var sourceNode = Nodes.Single(v => v.Activity.ActivityId == transition.FromActivityId);
                edge.Id = transition.TransitionId;
                edge.Source = sourceNode;

                var targetNode = Nodes.Single(v => v.Activity.ActivityId == transition.ToActivityId);
                edge.Target = targetNode;
                //convert xml to transition
                edge.Transition = transition;

                Edges.Add(edge);
            }
        }

        /// <summary>
        /// Set Node Position
        /// </summary>
        /// <param name="xmlNodePlane">plane Node</param>
        /// <param name="activity">Xml Node</param>
        /// <param name="node">Node in Canvas</param>
        private void SetNodePosition(XmlNode xmlNodePlane, Engine.Xpdl.Entity.Activity activity, Node node)
        {
            foreach (XmlNode child in xmlNodePlane.ChildNodes)
            {
                if (child.Name == XPDLDefinition.BPMN_ElementName_Shape)
                {
                    var shapeActivityId = XMLHelper.GetXmlAttribute(child, "bpmnElement");
                    if (shapeActivityId == activity.ActivityId)
                    {
                        var boundNode = child.FirstChild;
                        node.Left = int.Parse(XMLHelper.GetXmlAttribute(boundNode, "x"));
                        node.Top = int.Parse(XMLHelper.GetXmlAttribute(boundNode, "y"));
                        node.Width = int.Parse(XMLHelper.GetXmlAttribute(boundNode, "width"));
                        node.Height = int.Parse(XMLHelper.GetXmlAttribute(boundNode, "height"));
                        break;
                    }
                }
            }
        }
        #endregion
    }
}
