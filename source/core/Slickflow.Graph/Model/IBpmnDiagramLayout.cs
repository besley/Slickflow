using System.Xml;

namespace Slickflow.Graph.Model
{
    /// <summary>
    /// BPMN diagram layout: computes edge waypoints (and optionally shape bounds) for serialization.
    /// Community edition uses a simple implementation; Enterprise edition can provide advanced routing.
    /// </summary>
    public interface IBpmnDiagramLayout
    {
        /// <summary>
        /// Appends BPMNEdge waypoint elements (di:waypoint) to the given bpmnEdgeElement for the given edge.
        /// May update edge.BranchOrder when computing split/join branch order.
        /// </summary>
        /// <param name="flow">Flow containing nodes and edges</param>
        /// <param name="edge">Sequence flow to layout</param>
        /// <param name="bpmnEdgeElement">BPMNEdge XML element to append waypoints to</param>
        /// <param name="xmlDoc">Document used to create waypoint elements</param>
        void AppendEdgeWaypoints(Flow flow, Edge edge, XmlElement bpmnEdgeElement, XmlDocument xmlDoc);
    }
}
