using System;
using System.Xml;
using Slickflow.Engine.Utility;
using Slickflow.Engine.Xpdl.Template;

namespace Slickflow.Graph.Model
{
    /// <summary>
    /// Simple BPMN diagram layout with orthogonal (Manhattan) edge routing.
    /// Gateways and events (≤50×50) use top/bottom ports for non-horizontal connections;
    /// tasks use right/left ports and a single mid-X bend.
    /// </summary>
    public sealed class SimpleBpmnDiagramLayout : IBpmnDiagramLayout
    {
        // Nodes whose width OR height fits within this threshold are treated as
        // gateway/event shapes that route via top/bottom ports.
        private const int SmallNodeThreshold = 50;
        // Y-delta below this value is treated as a horizontal connection.
        private const int HorizontalTolerance = 5;

        /// <inheritdoc />
        public void AppendEdgeWaypoints(Flow flow, Edge edge, XmlElement bpmnEdgeElement, XmlDocument xmlDoc)
        {
            var source = edge.Source;
            var target = edge.Target;

            var srcCX = source.Left + source.Width / 2;
            var srcCY = source.Top + source.Height / 2;
            var tgtCX = target.Left + target.Width / 2;
            var tgtCY = target.Top + target.Height / 2;

            int deltaY = tgtCY - srcCY;

            if (Math.Abs(deltaY) <= HorizontalTolerance)
            {
                // Horizontal connection: straight 2-point line.
                AppendWayPoint(bpmnEdgeElement, xmlDoc, source.Left + source.Width, srcCY);
                AppendWayPoint(bpmnEdgeElement, xmlDoc, target.Left, tgtCY);
                return;
            }

            bool goingUp = deltaY < 0;   // target is above source
            bool srcSmall = source.Width <= SmallNodeThreshold && source.Height <= SmallNodeThreshold;
            bool tgtSmall = target.Width <= SmallNodeThreshold && target.Height <= SmallNodeThreshold;

            if (srcSmall)
            {
                // Gateway/event → task or gateway:
                // Exit from source top-center (going up) or bottom-center (going down).
                int exitX = srcCX;
                int exitY = goingUp ? source.Top : source.Top + source.Height;
                int entryX = target.Left;
                int entryY = tgtCY;

                AppendWayPoint(bpmnEdgeElement, xmlDoc, exitX, exitY);
                AppendWayPoint(bpmnEdgeElement, xmlDoc, exitX, entryY);  // vertical to target row
                AppendWayPoint(bpmnEdgeElement, xmlDoc, entryX, entryY); // horizontal to target
            }
            else if (tgtSmall)
            {
                // Task → gateway/event:
                // Exit from source right-center, enter target top (source above target) or bottom (source below target).
                int exitX = source.Left + source.Width;
                int exitY = srcCY;
                int entryX = tgtCX;
                // Source below target (goingUp) → enter gateway from its bottom; else from its top.
                int entryY = goingUp ? target.Top + target.Height : target.Top;

                AppendWayPoint(bpmnEdgeElement, xmlDoc, exitX, exitY);
                AppendWayPoint(bpmnEdgeElement, xmlDoc, entryX, exitY);  // horizontal to target column
                AppendWayPoint(bpmnEdgeElement, xmlDoc, entryX, entryY); // vertical to target port
            }
            else
            {
                // Task → task at different Y: Z-bend at the midpoint X.
                int exitX = source.Left + source.Width;
                int exitY = srcCY;
                int entryX = target.Left;
                int entryY = tgtCY;
                int midX = (exitX + entryX) / 2;

                AppendWayPoint(bpmnEdgeElement, xmlDoc, exitX, exitY);
                AppendWayPoint(bpmnEdgeElement, xmlDoc, midX, exitY);
                AppendWayPoint(bpmnEdgeElement, xmlDoc, midX, entryY);
                AppendWayPoint(bpmnEdgeElement, xmlDoc, entryX, entryY);
            }
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
