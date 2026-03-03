using System;
using System.Linq;
using System.Collections.Generic;
using Slickflow.Engine.Common;
using Slickflow.Engine.Xpdl.Entity;

namespace Slickflow.Graph.Model
{
    /// <summary>
    /// Edge Builder
    /// 连线构造器
    /// </summary>
    public class EdgeBuilder
    {
        #region Property
        private Edge _edge;

        internal Edge Edge
        {
            get { return _edge; }
        }
        #endregion

        #region Add Edge Property
        /// <summary>
        /// Add receiver type
        /// 添加接收者类型
        /// </summary>
        public EdgeBuilder AddReceiver(ReceiverTypeEnum receiverType, int candidates)
        {
            var receiver = new Receiver();
            receiver.ReceiverType = receiverType;
            receiver.Candidates = candidates;
            _edge.Transition.Receiver = receiver;

            return this;
        }

        /// <summary>
        /// Add condition
        /// 添加条件
        /// </summary>
        public EdgeBuilder AddCondition(ConditionTypeEnum conditionType, string conditionText)
        {
            var condition = new ConditionDetail();
            condition.ConditionType = conditionType;
            condition.ConditionText = conditionText;
            _edge.Transition.Condition = condition;

            return this;
        }
        #endregion

        #region Static create and get method
        /// <summary>
        /// Create Edge
        /// </summary>
        public static EdgeBuilder CreateEdge(string description)
        {
            var eb = new EdgeBuilder();
            var edge = new Edge(description);

            eb._edge = edge;

            return eb;
        }

        /// <summary>
        /// Set Transition GUID
        /// </summary>
        internal static void SetTransitionGUID(Edge edge)
        {
            edge.Transition.TransitionId = System.Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Is there a starting node
        /// 是否有起始节点
        /// </summary>
        internal static Boolean HasEdgeFromSource(List<Edge> edges, string sourceActivityCode)
        {
            var isExist = edges.Any<Edge>(t => t.Source.Activity.ActivityCode == sourceActivityCode);
            return isExist;
        }

        /// <summary>
        /// Obtain the connection based on the starting node
        /// 根据起始节点获取连线
        /// </summary>
        internal static Edge GetEdgeFromSource(List<Edge> edges, string sourceActivityCode)
        {
            var edge = edges.Single<Edge>(t => t.Source.Activity.ActivityCode == sourceActivityCode);
            return edge;
        }

        /// <summary>
        /// Obtain the connection based on the starting node
        /// 根据起始节点获取连线
        /// </summary>
        internal static List<Edge> GetEdgesFromSource(List<Edge> edges, string sourceActivityCode)
        {
            var list = edges.Where<Edge>(t => t.Source.Activity.ActivityCode == sourceActivityCode).ToList();
            return list;
        }

        /// <summary>
        /// Obtain connections based on the target node
        /// 根据目标节点获取连线
        /// </summary>
        internal static Edge GetEdgeToTarget(List<Edge> edges, string targetActivityCode)
        {
            var edge = edges.Single<Edge>(t => t.Target.Activity.ActivityCode == targetActivityCode);
            return edge;
        }

        /// <summary>
        /// Obtain connections based on the target node
        /// 根据目标节点获取连线
        /// </summary>
        internal static List<Edge> GetEdgesToTarget(IList<Edge> edges, string targetActivityCode)
        {
            var list = edges.Where<Edge>(t => t.Target.Activity.ActivityCode == targetActivityCode).ToList();
            return list;
        }

        /// <summary>
        /// Obtain connections based on the starting and target nodes
        /// 根据起始和目标节点获取连线
        /// </summary>
        internal static Edge GetEdge(List<Edge> edges, string sourceActivityCode, string targetActivityCode)
        {
            var edge = edges.Single<Edge>(t => t.Source.Activity.ActivityCode == sourceActivityCode
                && t.Target.Activity.ActivityCode == targetActivityCode);
            return edge;
        }

        /// <summary>
        /// Create edge
        /// </summary>
        internal static Edge CreateEdge(List<Edge> edges, Node source, Node target)
        {
            var edge = new Edge();
            edge = CreateEdge(edges, edge, source, target);
            return edge;
        }

        /// <summary>
        /// Create edge
        /// </summary>
        internal static Edge CreateEdge(List<Edge> edges, Edge edge, Node source, Node target)
        {
            if (edge == null) edge = new Edge();

            edge.Source = source;
            edge.Target = target;

            var edgeId = string.Format("Flow_{0}", Utility.GetRandomInt().ToString());
            edge.Id = edgeId;

            edges.Add(edge);

            return edge;
        }

        /// <summary>
        /// Replace Node
        /// 取代节点
        /// </summary>
        internal static void ReplaceSourceTarget(List<Edge> edges, string activityCode, Node newNode)
        {
            ReplaceSource(edges, activityCode, newNode);
            ReplaceTarget(edges, activityCode, newNode);
        }

        /// <summary>
        /// Replace source node
        /// 取代起始节点
        /// </summary>
        internal static void ReplaceSource(List<Edge> edges, string activityCode, Node newNode)
        {
            var listSource = GetEdgesFromSource(edges, activityCode);
            foreach (var edge in listSource)
            {
                edge.Source = newNode;
                EdgeBuilder.SetTransitionGUID(edge);
            }
        }

        /// <summary>
        /// Replace target node
        /// 取代目标节点
        /// </summary>
        internal static void ReplaceTarget(List<Edge> edges, string activityCode, Node newNode)
        {
            var listTarget = EdgeBuilder.GetEdgesToTarget(edges, activityCode);
            foreach (var edge in listTarget)
            {
                edge.Target = newNode;
                EdgeBuilder.SetTransitionGUID(edge);
            }
        }

        /// <summary>
        /// Remove edge
        /// 移除连线
        /// </summary>
        internal static void RemoveEdge(List<Edge> edges, Edge edge)
        {
            edges.Remove(edge);
        }

        /// <summary>
        /// Remove edge
        /// 移除连线
        /// </summary>
        internal static void RemoveEdge(List<Edge> edges, string sourceActivityCode, string targetActivityCode)
        {
            var edge = GetEdge(edges, sourceActivityCode, targetActivityCode);
            edges.Remove(edge);
        }

        /// <summary>
        /// Remove edge
        /// 移除连线
        /// </summary>
        internal static void RemoveEdge(List<Edge> edges, string activityCode)
        {
            int a = edges.RemoveAll(t => (t.Source.Activity.ActivityCode == activityCode));
            int b = edges.RemoveAll(t => (t.Target.Activity.ActivityCode == activityCode));
        }
        #endregion 
    }
}
