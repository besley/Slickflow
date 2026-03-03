using System.Xml;
using Slickflow.Engine.Utility;
using Slickflow.Engine.Xpdl.Template;

namespace Slickflow.Graph.Model
{
    /// <summary>
    /// Simple BPMN diagram layout: two-point edge waypoints (source right-center to target left-center).
    /// Used when Enterprise diagram layout is not registered.
    /// </summary>
    public sealed class SimpleBpmnDiagramLayout : IBpmnDiagramLayout
    {
        /// <inheritdoc />
        public void AppendEdgeWaypoints(Flow flow, Edge edge, XmlElement bpmnEdgeElement, XmlDocument xmlDoc)
        {
            var source = edge.Source;
            var target = edge.Target;
            AppendWayPoint(bpmnEdgeElement, xmlDoc, source.Left + source.Width, source.Top + (source.Height / 2));
            AppendWayPoint(bpmnEdgeElement, xmlDoc, target.Left, target.Top + (target.Height / 2));
        }

        private static void AppendWayPoint(XmlElement edgeNode, XmlDocument xmlDoc, int x, int y)
        {
            var wayPointNode = xmlDoc.CreateElement("di", "waypoint", ProcessNamespaceDefine.m_namespace_uri_di);
            XMLHelper.SetXmlAttribute(wayPointNode, "x", x.ToString());
            XMLHelper.SetXmlAttribute(wayPointNode, "y", y.ToString());
            edgeNode.AppendChild(wayPointNode);
        }
    }
}
