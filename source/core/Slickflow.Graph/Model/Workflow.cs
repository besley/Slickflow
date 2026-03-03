using System;
using System.Linq;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Common;
using Slickflow.Engine.Xpdl.Common;
using Slickflow.Engine.External;
using Slickflow.Engine.Service;
using Slickflow.Graph.Common;
using Slickflow.Data;

namespace Slickflow.Graph.Model
{
    /// <summary>
    /// Workflow class - process model builder for creating workflow process graphs.
    /// </summary>
    public class Workflow
    {
        #region Constants
        /// <summary>
        /// Vertical center ratio for node positioning.
        /// </summary>
        private const double VERTICAL_CENTER_RATIO = 0.5;

        /// <summary>
        /// Default horizontal shift distance when moving nodes along the canvas.
        /// </summary>
        private const int DEFAULT_SHIFT_DISTANCE = 210;

        /// <summary>
        /// Level delta used by uncover operations when adjusting node levels.
        /// </summary>
        private const int LEVEL_DELTA_FOR_UNCOVER = 2;
        #endregion

        #region Property and Constructor
        private Flow _flow = null;
        private Stack<Node> _sequenceStack = new Stack<Node>();
        private Stack<Node> _splitStack = new Stack<Node>();
        private Stack<Node> _joinStack = new Stack<Node>();
        private Stack<int> _splitTopPositionStack = new Stack<int>();
        private Stack<int> _joinTopPositionStack = new Stack<int>();

        private List<Node> Nodes => _flow?.Nodes;

        private List<Edge> Edges => _flow?.Edges;

        /// <summary>
        /// Initializes a new instance of the Workflow class.
        /// </summary>
        /// <param name="name">Process name</param>
        /// <param name="code">Process code</param>
        public Workflow(string name, string code)
        {
            var processId = $"process_{Utility.GetRandomInt().ToString()}";
            _flow = new Flow(name, code, processId, "1");
        }

        /// <summary>
        /// Private constructor for internal use (e.g., LoadProcess).
        /// </summary>
        private Workflow()
        {
        }
        #endregion

        #region Process metadata
        /// <summary>
        /// Gets the process id of the current workflow definition.
        /// </summary>
        public string ProcessId => _flow?.ProcessId;

        /// <summary>
        /// Gets the process code of the current workflow definition.
        /// </summary>
        public string ProcessCode => _flow?.Code;

        /// <summary>
        /// Gets the version of the current workflow definition.
        /// </summary>
        public string Version => _flow?.Version;
        #endregion

        #region Stack and List operation
        /// <summary>
        /// Get the internal stack by stack type (sequence, split, join).
        /// </summary>
        private Stack<Node> GetStack(StackTypeEnum stackType)
        {
            return stackType switch
            {
                StackTypeEnum.Sequence => _sequenceStack,
                StackTypeEnum.Split => _splitStack,
                StackTypeEnum.Join => _joinStack,
                _ => throw new ArgumentException($"Unknown stack type: {stackType}", nameof(stackType))
            };
        }

        /// <summary>
        /// Push a node into the specified internal stack.
        /// </summary>
        private void AppendNodeIntoStack(StackTypeEnum stackType, Node node)
        {
            var stack = GetStack(stackType);
            stack.Push(node);
        }

        /// <summary>
        /// Get the last node in the flow's node list.
        /// </summary>
        private Node GetLastNode()
        {
            if (_flow?.Nodes == null || !_flow.Nodes.Any())
            {
                throw new InvalidOperationException("No vertices available in the flow.");
            }
            return _flow.Nodes.Last();
        }

        /// <summary>
        /// Pop the previous node from the specified stack.
        /// </summary>
        private Node GetPreviousNodeFromStack(StackTypeEnum stackType)
        {
            var stack = GetStack(stackType);
            try
            {
                return stack.Pop();
            }
            catch (InvalidOperationException ex)
            {
                throw new ApplicationException(
                    $"An error occurred when reading node stack info. Stack type: {stackType}, Error: {ex.Message}",
                    ex);
            }
        }

        /// <summary>
        /// Get the element count of the specified stack.
        /// </summary>
        private int GetStackCount(StackTypeEnum stackType)
        {
            var stack = GetStack(stackType);
            return stack.Count;
        }
        #endregion

        #region Parallel and join
        /// <summary>
        /// Parallels - simplified version for single-task branches (recommended).
        /// </summary>
        /// <param name="tasks">Task tuples (name, code)</param>
        /// <returns>Workflow instance</returns>
        public Workflow Parallels(params (string name, string code)[] tasks)
        {
            return Parallels(true, tasks);
        }

        /// <summary>
        /// Parallels - simplified version for single-task branches (recommended).
        /// </summary>
        /// <param name="needJoin">Whether to join branches</param>
        /// <param name="tasks">Task tuples (name, code)</param>
        /// <returns>Workflow instance</returns>
        public Workflow Parallels(Boolean needJoin, params (string name, string code)[] tasks)
        {
            AppendSplitStack(needJoin, tasks.Length);

            foreach (var (name, code) in tasks)
            {
                // Get gateway node from split stack
                var gatewayNode = GetPreviousNodeFromStack(StackTypeEnum.Split);
                AppendNodeIntoStack(StackTypeEnum.Sequence, gatewayNode);

                // Create Task directly, no need for Branch wrapper
                Task(name, code);

                // Position adjustment logic (reuse Branch's position adjustment logic)
                var originalTop = _splitTopPositionStack.Pop();
                var targetNode = GetLastNode();
                
                // Handle nested branch positions
                if (gatewayNode.Activity.GatewayDetail != null
                    && gatewayNode.Activity.GatewayDetail.DirectionType == GatewayDirectionEnum.AndSplitMI)
                {
                    // AndSplitMI type nodes don't need to adjust Top position
                }
                else
                {
                    targetNode.Top = originalTop + CanvasPositionDefine.BRANCH_POSITION_HEIGHT;
                }

                // Connect to Join node if needed
                if (needJoin == true)
                {
                    var branchNode = GetPreviousNodeFromStack(StackTypeEnum.Sequence);
                    AppendNodeIntoStack(StackTypeEnum.Join, branchNode);
                }
            }

            return this;
        }

        /// <summary>
        /// Parallels - using Action delegates for clearer semantics (flexible for complex branches).
        /// </summary>
        /// <param name="branches">Branch actions</param>
        /// <returns>Workflow instance</returns>
        public Workflow Parallels(params Action[] branches)
        {
            return Parallels(true, branches);
        }

        /// <summary>
        /// Parallels - using Action delegates for clearer semantics (flexible for complex branches).
        /// </summary>
        /// <param name="needJoin">Whether to join branches</param>
        /// <param name="branches">Branch actions</param>
        /// <returns>Workflow instance</returns>
        public Workflow Parallels(Boolean needJoin, params Action[] branches)
        {
            AppendSplitStack(needJoin, branches.Length);

            foreach (var action in branches)
            {
                action();  // Clearer semantics: execute action
            }
            return this;
        }

        ///// <summary>
        ///// Parallels - legacy version using Func (kept for backward compatibility).
        ///// </summary>
        ///// <param name="branches">Branch functions</param>
        ///// <returns>ProcessModelBuilder instance</returns>
        //[Obsolete("Use Parallels(params Action[] branches) or Parallels(params (string, string)[] tasks) instead. This method will be removed in a future version.")]
        //public ProcessModelBuilder Parallels(params Func<ProcessModelBuilder>[] branches)
        //{
        //    // Convert Func<ProcessModelBuilder>[] to Action[] and call Action version
        //    return Parallels(branches.Select(b => (Action)(() => b())).ToArray());
        //}

        /////// <summary>
        /////// Parallels (legacy signature).
        /////// </summary>
        ////public ProcessModelBuilder Parallels(params Func<Boolean, ProcessModelBuilder>[] branches)
        ////{
        ////    var pmb = Parallels(true, branches);
        ////    return pmb;
        ////}

        ///// <summary>
        ///// Parallels - legacy version using Func with needJoin parameter.
        ///// </summary>
        ///// <param name="needJoin">Whether to join branches</param>
        ///// <param name="branches">Branch functions</param>
        ///// <returns>ProcessModelBuilder instance</returns>
        //[Obsolete("Use Parallels(Boolean needJoin, params Action[] branches) or Parallels(Boolean needJoin, params (string, string)[] tasks) instead. This method will be removed in a future version.")]
        //public ProcessModelBuilder Parallels(Boolean needJoin,
        //    params Func<ProcessModelBuilder>[] branches)
        //{
        //    // Convert Func<ProcessModelBuilder>[] to Action[] and call Action version
        //    return Parallels(needJoin, branches.Select(b => (Action)(() => b())).ToArray());
        //}

        ///// <summary>
        ///// Parallels - legacy version using Func with Boolean parameter.
        ///// </summary>
        ///// <param name="needJoin">Whether to join branches</param>
        ///// <param name="branches">Branch functions with Boolean parameter</param>
        ///// <returns>ProcessModelBuilder instance</returns>
        //[Obsolete("Use Parallels(Boolean needJoin, params Action[] branches) or Parallels(Boolean needJoin, params (string, string)[] tasks) instead. This method will be removed in a future version.")]
        //public ProcessModelBuilder Parallels(Boolean needJoin,
        //    params Func<Boolean, ProcessModelBuilder>[] branches)
        //{
        //    AppendSplitStack(needJoin, branches.Length);

        //    foreach (var func in branches)
        //    {
        //        func(needJoin);             //branch function
        //    }
        //    return this;
        //}

        /// <summary>
        /// Append split stack: push the current gateway node into split/join-related stacks.
        /// </summary>
        private void AppendSplitStack(Boolean needJoin,
            int branchCount)
        {
            var gatewayNode = GetLastNode();
            if (needJoin == true) _joinTopPositionStack.Push(gatewayNode.Top);   //for join node position

            for (var i = 0; i < branchCount; i++)
            {
                // Record the branch top position so that Branch() can position nodes correctly.
                // For a parallel-branch container with a single branch, the Top is aligned with the gateway node.
                var top = branchCount == 1 ? gatewayNode.Top : gatewayNode.Top + (i - 1) * CanvasPositionDefine.LINK_POSITION_WIDTH;
                _splitTopPositionStack.Push(top);   //for branch node position

                AppendNodeIntoStack(StackTypeEnum.Split, gatewayNode);
            }
        }

        /// <summary>
        /// Create branch using workflow builder functions.
        /// </summary>
        public Workflow Branch(params Func<Workflow>[] nodes)
        {
            Branch(true, nodes);
            return this;
        }

        /// <summary>
        /// Create branch using workflow builder functions with optional join.
        /// </summary>
        public Workflow Branch(Boolean needJoin,
            params Func<Workflow>[] nodeFuncs)
        {
            var first = nodeFuncs.First();
            var last = nodeFuncs.Last();

            foreach (var func in nodeFuncs)
            {
                if (func.Equals(first))
                {
                    var gatewayNode = GetPreviousNodeFromStack(StackTypeEnum.Split);
                    AppendNodeIntoStack(StackTypeEnum.Sequence, gatewayNode);

                    // Execute node creation method
                    func();

                    var originalTop = _splitTopPositionStack.Pop();
                    // Handle nested branch positions
                    var targetNode = GetLastNode();
                    if (gatewayNode.Activity.GatewayDetail != null
                        && gatewayNode.Activity.GatewayDetail.DirectionType == GatewayDirectionEnum.AndSplitMI)
                    {
                        // AndSplitMI type nodes don't need to adjust Top position
                    }
                    else
                    {
                        targetNode.Top = originalTop + CanvasPositionDefine.BRANCH_POSITION_HEIGHT;
                    }

                    // Special case: branch contains only a single node.
                    //When there is only one node in a branch
                    if (func.Equals(last))
                    {
                        if (needJoin == true)
                        {
                            // Branch nodes that need to be merged.
                            //Branch nodes that need to be merged
                            var firstLastBranchNode = GetPreviousNodeFromStack(StackTypeEnum.Sequence);
                            AppendNodeIntoStack(StackTypeEnum.Join, firstLastBranchNode);
                        }
                    }
                }
                else if (func.Equals(last))
                {
                    func();

                    if (needJoin == true)
                    {
                        // Branch nodes that need to be merged.
                        //Branch nodes that need to be merged
                        var lastBranchNode = GetPreviousNodeFromStack(StackTypeEnum.Sequence);
                        AppendNodeIntoStack(StackTypeEnum.Join, lastBranchNode);
                    }
                }
                else
                {
                    func();
                }
            }
            return this;
        }
        #endregion

        #region Create Node
        /// <summary>
        /// Create start node.
        /// </summary>
        public Workflow Start(string activityName = null,
            string activityCode = null,
            TriggerTypeEnum triggerType = TriggerTypeEnum.None)
        {
            if (string.IsNullOrEmpty(activityName)) activityName = "Start";
            if (string.IsNullOrEmpty(activityCode)) activityCode = $"Activity_Start_{Utility.GetRandomString(4)}";

            Start(NodeBuilder.CreateStart(activityName, activityCode, triggerType));
            return this;
        }

        /// <summary>
        /// Create start node.
        /// </summary>
        public Workflow Start(NodeBuilder vb)
        {
            var node = vb.Node;
            NodeBuilder.AppendNode(this.Nodes, node);

            AppendNodeIntoStack(StackTypeEnum.Sequence, node);

            return this;
        }

        /// <summary>
        /// Create intermediate node.
        /// </summary>
        public Workflow Intermediate(string activityName = null,
            string activityCode = null,
            TriggerTypeEnum triggerType = TriggerTypeEnum.None,
            EdgeBuilder lb = null)
        {
            if (string.IsNullOrEmpty(activityCode)) activityCode = "Intermediate";

            Intermediate(NodeBuilder.CreateIntermediate(activityName, activityCode, triggerType), lb);
            return this;
        }

        /// <summary>
        /// Create intermediate node.
        /// </summary>
        public Workflow Intermediate(NodeBuilder vb, EdgeBuilder lb)
        {
            var node = vb.Node;
            NodeBuilder.AppendNode(this.Nodes, node);

            JoinEdge(node, lb);

            AppendNodeIntoStack(StackTypeEnum.Sequence, node);

            return this;
        }

        /// <summary>
        /// Create end node.
        /// </summary>
        public Workflow End(string activityName = null,
            string activityCode = null,
            EdgeBuilder lb = null)
        {
            if (string.IsNullOrEmpty(activityName)) activityName = "End";
            if (string.IsNullOrEmpty(activityCode)) activityCode = $"Activity_End_{Utility.GetRandomString(4)}";

            End(NodeBuilder.CreateEnd(activityName, activityCode), lb);
            return this;
        }

        /// <summary>
        /// Create end node.
        /// </summary>
        public Workflow End(NodeBuilder nb, EdgeBuilder lb = null)
        {
            var node = nb.Node;
            NodeBuilder.AppendNode(this.Nodes, node);

            var previousNode = GetPreviousNodeFromStack(StackTypeEnum.Sequence);
            Edge edge = null;
            if (lb != null)
            {
                edge = EdgeBuilder.CreateEdge(this.Edges, lb.Edge, previousNode, node);
            }
            else
            {
                edge = EdgeBuilder.CreateEdge(this.Edges, previousNode, node);
            }

            // Set the target node level
            SetTargetNodeLevel(edge.Target, edge.Source.Level);

            AdjustTargetNodePosition(edge.Source, edge.Target);

            return this;
        }

        /// <summary>
        /// Create task node.
        /// </summary>
        public Workflow Task(string activityName,
            string activityCode = null,
            EdgeBuilder lb = null,
            Nullable<Boolean> isJoin = null)
        {
            Task(
                NodeBuilder.CreateTask(activityName, activityCode),
                lb,
                isJoin
            );

            return this;
        }

        /// <summary>
        /// Create task node.
        /// </summary>
        public Workflow Task(NodeBuilder vb,
            EdgeBuilder lb = null,
            Nullable<Boolean> isJoin = null)
        {
            var node = vb.Node;
            NodeBuilder.AppendNode(this.Nodes, node);

            if (isJoin != null && isJoin.Value == true)
            {
                JoinBranches(node);       //node is a join node, to merge branches 
            }
            else
            {
                JoinEdge(node, lb);       //node is a simple node, only make a edge between two nodes
            }
            AppendNodeIntoStack(StackTypeEnum.Sequence, node);

            return this;
        }

        /// <summary>
        /// Create service task node bound to a local method by registry key (LocalMethod, BPMN2 ##DelegateExpression semantic).
        /// Register the delegate before Run: ServiceTaskDelegateRegistry.Global.Register(delegateKey, delegate).
        /// </summary>
        public Workflow ServiceTask(string activityName, string activityCode, string delegateKey,
            EdgeBuilder lb = null, Nullable<Boolean> isJoin = null)
        {
            var vb = NodeBuilder.CreateServiceTask(activityName, activityCode, delegateKey);
            var node = vb.Node;
            NodeBuilder.AppendNode(this.Nodes, node);
            if (isJoin != null && isJoin.Value == true)
                JoinBranches(node);
            else
                JoinEdge(node, lb);
            AppendNodeIntoStack(StackTypeEnum.Sequence, node);
            return this;
        }

        /// <summary>
        /// Create service task node and bind a local plugin service by generic type.
        /// </summary>
        public Workflow ServiceTask<TService>(string activityName,
            string activityCode = null,
            EdgeBuilder lb = null,
            Nullable<Boolean> isJoin = null)
            where TService : ExternalServiceBase, IExternalService
        {
            ServiceTask<TService>(
                NodeBuilder.CreateServiceTask<TService>(activityName, activityCode),
                lb,
                isJoin
            );

            return this;
        }

        /// <summary>
        /// Create service task node and bind a local plugin service by generic type.
        /// </summary>
        public Workflow ServiceTask<TService>(NodeBuilder vb,
            EdgeBuilder lb = null,
            Nullable<Boolean> isJoin = null)
            where TService : ExternalServiceBase, IExternalService
        {
            var node = vb.Node;
            NodeBuilder.AppendNode(this.Nodes, node);

            if (isJoin != null && isJoin.Value == true)
            {
                JoinBranches(node);       // node is a join node, to merge branches
            }
            else
            {
                JoinEdge(node, lb);       // node is a simple node, only make an edge between two nodes
            }
            AppendNodeIntoStack(StackTypeEnum.Sequence, node);

            return this;
        }

        /// <summary>
        /// Create agent (AI task) node.
        /// </summary>
        public Workflow Agent(string activityName,
            string activityCode = null,
            EdgeBuilder lb = null,
            Nullable<Boolean> isJoin = null)
        {
            Agent(
                NodeBuilder.CreateTask(activityName, activityCode),
                lb,
                isJoin
            );

            return this;
        }

        /// <summary>
        /// Create RAG AI service task node.
        /// </summary>
        public Workflow RagService(string activityName,
            string activityCode = null,
            EdgeBuilder lb = null,
            Nullable<Boolean> isJoin = null)
        {
            RagService(
                NodeBuilder.CreateRagService(activityName, activityCode),
                lb,
                isJoin
            );

            return this;
        }

        /// <summary>
        /// Create RAG AI service task node.
        /// </summary>
        public Workflow RagService(NodeBuilder vb,
            EdgeBuilder lb = null,
            Nullable<Boolean> isJoin = null)
        {
            var node = vb.Node;
            NodeBuilder.AppendNode(this.Nodes, node);

            if (isJoin != null && isJoin.Value == true)
            {
                JoinBranches(node);       // node is a join node, to merge branches
            }
            else
            {
                JoinEdge(node, lb);       // node is a simple node, only make an edge between two nodes
            }
            AppendNodeIntoStack(StackTypeEnum.Sequence, node);

            return this;
        }

        /// <summary>
        /// Create LLM AI service task node.
        /// </summary>
        public Workflow LlmService(string activityName,
            string activityCode = null,
            EdgeBuilder lb = null,
            Nullable<Boolean> isJoin = null)
        {
            LlmService(
                NodeBuilder.CreateLlmService(activityName, activityCode),
                lb,
                isJoin
            );

            return this;
        }

        /// <summary>
        /// Create LLM AI service task node.
        /// </summary>
        public Workflow LlmService(NodeBuilder vb,
            EdgeBuilder lb = null,
            Nullable<Boolean> isJoin = null)
        {
            var node = vb.Node;
            NodeBuilder.AppendNode(this.Nodes, node);

            if (isJoin != null && isJoin.Value == true)
            {
                JoinBranches(node);       // node is a join node, to merge branches
            }
            else
            {
                JoinEdge(node, lb);       // node is a simple node, only make an edge between two nodes
            }
            AppendNodeIntoStack(StackTypeEnum.Sequence, node);

            return this;
        }

        /// <summary>
        /// Create agent (AI task) node.
        /// </summary>
        public Workflow Agent(NodeBuilder vb,
            EdgeBuilder lb = null,
            Nullable<Boolean> isJoin = null)
        {
            var node = vb.Node;
            NodeBuilder.AppendNode(this.Nodes, node);

            if (isJoin != null && isJoin.Value == true)
            {
                JoinBranches(node);       //node is a join node, to merge branches 
            }
            else
            {
                JoinEdge(node, lb);       //node is a simple node, only make a edge between two nodes
            }
            AppendNodeIntoStack(StackTypeEnum.Sequence, node);

            return this;
        }

        /// <summary>
        /// Direct connection without node (no-op placeholder).
        /// </summary>
        public Workflow Dumb()
        {
            return this;
        }

        /// <summary>
        /// Join edge between previous sequence node and current node.
        /// </summary>
        private void JoinEdge(Node node, EdgeBuilder lb)
        {
            Edge edge = null;
            var previousNode = GetPreviousNodeFromStack(StackTypeEnum.Sequence);
            if (lb != null)
            {
                edge = EdgeBuilder.CreateEdge(this.Edges, lb.Edge, previousNode, node);
            }
            else
            {
                edge = EdgeBuilder.CreateEdge(this.Edges, previousNode, node);
            }

            // Set the target node level
            SetTargetNodeLevel(edge.Target, edge.Source.Level);

            // Adjust the position of the target node
            AdjustTargetNodePosition(edge.Source, edge.Target);
        }

        /// <summary>
        /// Create split node.
        /// </summary>
        public Workflow Split(string activityName = null,
            string activityCode = null,
            EdgeBuilder lb = null)
        {
            Split(NodeBuilder.CreateSplit(null, activityName, activityCode), lb);
            return this;
        }

        /// <summary>
        /// Create split node.
        /// </summary>
        public Workflow Split(NodeBuilder vb,
            EdgeBuilder lb = null)
        {
            var node = vb.Node;
            NodeBuilder.AppendNode(this.Nodes, node);

            Edge edge = null;
            var previousNode = GetPreviousNodeFromStack(StackTypeEnum.Sequence);
            if (lb != null)
            {
                edge = EdgeBuilder.CreateEdge(this.Edges, lb.Edge, previousNode, node);
            }
            else
            {
                edge = EdgeBuilder.CreateEdge(this.Edges, previousNode, node);
            }

            // Set the target node level
            SetTargetNodeLevel(edge.Target, edge.Source.Level);

            AdjustTargetNodePosition(edge.Source, edge.Target);

            return this;
        }

        /// <summary>
        /// Create AND-split gateway node.
        /// </summary>
        public Workflow AndSplit(string activityName = null,
            string activityCode = null,
            EdgeBuilder lb = null)
        {
            Split(NodeBuilder.CreateSplit(GatewayDirectionEnum.AndSplit, activityName, activityCode), lb);
            return this;
        }

        /// <summary>
        /// Create OR-split gateway node.
        /// </summary>
        public Workflow OrSplit(string activityName = null,
            string activityCode = null,
            EdgeBuilder lb = null)
        {
            Split(NodeBuilder.CreateSplit(GatewayDirectionEnum.OrSplit, activityName, activityCode), lb);
            return this;
        }

        /// <summary>
        /// Create XOR-split gateway node.
        /// </summary>
        public Workflow XOrSplit(string activityName = null,
            string activityCode = null,
            EdgeBuilder lb = null)
        {
            Split(NodeBuilder.CreateSplit(GatewayDirectionEnum.XOrSplit, activityName, activityCode), lb);
            return this;
        }

        /// <summary>
        /// Create AND-split multi-instance container gateway node.
        /// </summary>
        public Workflow AndSplitMI(string activityName = null,
            string activityCode = null,
            EdgeBuilder lb = null)
        {
            Split(NodeBuilder.CreateSplit(GatewayDirectionEnum.AndSplitMI, activityName, activityCode), lb);
            return this;
        }

        /// <summary>
        /// Create join node.
        /// </summary>
        public Workflow Join(string activityName = null,
            string activityCode = null)
        {
            Join(NodeBuilder.CreateJoin(null, activityName, activityCode));
            return this;
        }

        /// <summary>
        /// Create join node.
        /// </summary>
        public Workflow Join(NodeBuilder vb)
        {
            var node = vb.Node;
            NodeBuilder.AppendNode(this.Nodes, node);

            //连接各分支
            //Connect various branches
            JoinBranches(node);

            AppendNodeIntoStack(StackTypeEnum.Sequence, node);

            return this;
        }

        /// <summary>
        /// Join branch connections.
        /// </summary>
        private void JoinBranches(Node node)
        {
            var branchCount = GetStackCount(StackTypeEnum.Join);
            for (var i = 0; i < branchCount; i++)
            {
                var previousNode = GetPreviousNodeFromStack(StackTypeEnum.Join);
                var edge = EdgeBuilder.CreateEdge(this.Edges, null, previousNode, node);

                // Set the target node level
                SetTargetNodeLevel(node, previousNode.Level);

                AdjustJoinNodePosition(edge, branchCount);
            }
        }

        /// <summary>
        /// Set the target node level.
        /// </summary>
        private void SetTargetNodeLevel(Node targetNode, int sourceLevel)
        {
            var count = this.Edges.Count(t => t.Target.Activity.ActivityCode == targetNode.Activity.ActivityCode);
            if (count == 0 || count == 1)
            {
                targetNode.Level = sourceLevel + 1;
            }
            else // count > 1
            {
                if (targetNode.Level > sourceLevel + 1)
                {
                    // Take the higher level to make canvas rearrangement easier.
                    targetNode.Level = sourceLevel + 1;
                }
                else
                {
                    ;// throw new InvalidOperationException($"Invalid Operation ..., target count:{count}, source level:{sourceLevel}, target elvel:{targetNode.Level}, please contact support team.");
                }
            }
        }

        /// <summary>
        /// Create AND-join node.
        /// </summary>
        public Workflow AndJoin(string activityName = null,
            string activityCode = null)
        {
            Join(NodeBuilder.CreateJoin(GatewayDirectionEnum.AndJoin, activityName, activityCode));
            return this;
        }


        /// <summary>
        /// Create OR-join node.
        /// </summary>
        public Workflow OrJoin(string activityName = null,
            string activityCode = null)
        {
            Join(NodeBuilder.CreateJoin(GatewayDirectionEnum.OrJoin, activityName, activityCode));
            return this;
        }

        /// <summary>
        /// Create XOR-join node.
        /// </summary>
        public Workflow XOrJoin(string activityName = null,
            string activityCode = null)
        {
            Join(NodeBuilder.CreateJoin(GatewayDirectionEnum.XOrJoin, activityName, activityCode));
            return this;
        }


        /// <summary>
        /// Create AND-join multi-instance container node.
        /// </summary>
        public Workflow AndJoinMI(string activityName = null,
            string activityCode = null)
        {
            Join(NodeBuilder.CreateJoin(GatewayDirectionEnum.AndJoinMI, activityName, activityCode));
            return this;
        }


        /// <summary>
        /// Create enhanced OR-join node.
        /// </summary>
        public Workflow EOrJoin(string activityName = null,
            string activityCode = null)
        {
            Join(NodeBuilder.CreateJoin(GatewayDirectionEnum.EOrJoin, activityName, activityCode));
            return this;
        }

        /// <summary>
        /// Set the position attributes of the merge (join) node.
        /// </summary>
        private void AdjustJoinNodePosition(Edge edge,
            int branchCount)
        {
            var source = edge.Source;
            var target = edge.Target;

            // When connecting, determine the positions of the two nodes based on the node type.
            var newLeft = source.Left + source.Width + CanvasPositionDefine.SHIFT_POSITION_WIDTH;
            if (newLeft > target.Left || target.Left == 0) target.Left = newLeft;

            // Compute node position based on whether nodes are being merged.
            //Calculate node positions based on whether to merge or not
            if (branchCount > 0 && target.Top == 0)
            {
                var splitTop = _joinTopPositionStack.Pop();
                target.Top = splitTop;
            }            
        }

        /// <summary>
        /// Adjust the position of the target node.
        /// </summary>
        private void AdjustTargetNodePosition(Node source, Node target)
        {
            target.Left = source.Left + source.Width + CanvasPositionDefine.SHIFT_POSITION_WIDTH;
            target.Top = source.Top + Convert.ToInt32(VERTICAL_CENTER_RATIO * source.Height) 
                         - Convert.ToInt32(VERTICAL_CENTER_RATIO * target.Height);
        }
        #endregion

        #region NodeBuilder Create
        /// <summary>
        /// Create NodeBuilder
        /// </summary>
        public NodeBuilder GetBuilder(string activityCode)
        {
            if (string.IsNullOrEmpty(activityCode))
            {
                throw new ArgumentException("Activity code cannot be null or empty.", nameof(activityCode));
            }

            var node = NodeBuilder.GetNode(this.Nodes, activityCode);
            if (node == null)
            {
                throw new ApplicationException($"The activity doesn't exist! Activity code: {activityCode}");
            }
            var vb = new NodeBuilder();
            vb.SetNode(node);

            return vb;
        }
        #endregion

        #region Process xml
        /// <summary>
        /// XML Serialize
        /// </summary>
        public string Serialize()
        {
            var pxb = new ProcessXmlBuilder(_flow);
            string xmlContent = pxb.Serialize();

            return xmlContent;
        }

        /// <summary>
        /// Generate process graphic XML and store it to the database.
        /// </summary>
        public ProcessEntity Build()
        {
            var pxb = new ProcessXmlBuilder(_flow);
            string xmlContent = pxb.Serialize();
            var entity = new ProcessEntity
            {
                ProcessId = _flow.ProcessId,
                ProcessName = _flow.Name,
                ProcessCode = _flow.Code,
                Version = _flow.Version,
                Status = 1,
                XmlContent = xmlContent,
                CreatedDateTime = System.DateTime.UtcNow,
                UpdatedDateTime = System.DateTime.UtcNow
            };

            var pm = new ProcessManager();
            var processId = pm.Insert(entity);
            entity.Id = processId;

            return entity;
        }

        /// <summary>
        /// Generate process graphic XML and build an in-memory ProcessEntity without saving to the database.
        /// </summary>
        public ProcessEntity BuildInMemory()
        {
            var pxb = new ProcessXmlBuilder(_flow);
            string xmlContent = pxb.Serialize();
            var entity = new ProcessEntity
            {
                ProcessId = _flow.ProcessId,
                ProcessName = _flow.Name,
                ProcessCode = _flow.Code,
                Version = _flow.Version,
                Status = 1,
                XmlContent = xmlContent,
                CreatedDateTime = System.DateTime.UtcNow,
                UpdatedDateTime = System.DateTime.UtcNow
            };

            return entity;
        }

        /// <summary>
        /// Store process graphic XML to database (obsolete, use Build()).
        /// </summary>
        [Obsolete("Replaced by Build() method")]
        public ProcessEntity Store()
        {
            return Build();
        }

        /// <summary>\
        /// Process entity update method.
        /// </summary>
        public ProcessEntity Update()
        {
            var pxb = new ProcessXmlBuilder(_flow);
            string xmlContent = pxb.Serialize();

            var pm = new ProcessManager();
            var entity = pm.GetByName(_flow.Name, _flow.Version);
            entity.XmlContent = xmlContent;
            entity.UpdatedDateTime = System.DateTime.UtcNow;
            pm.Update(entity);

            return entity;
        }

        /// <summary>
        /// Print node and connection information.
        /// </summary>
        public void Print()
        {
            foreach (var node in _flow.Nodes)
            {
                Console.WriteLine($"Node: {node.Name} (Code: {node.Activity.ActivityCode})");
            }

            foreach (var edge in _flow.Edges)
            {
                Console.WriteLine($"Edge: {edge.Source.Name} -> {edge.Target.Name}");
            }
        }
        #endregion

        #region Node add edit remove
        /// <summary>
        /// Append a new node after the current node.
        /// </summary>
        public Workflow Add(string currentActivityCode, ActivityTypeEnum addActivityType, 
            string addActivityName, string addActivityCode)
        {
            return Add(currentActivityCode, addActivityType, NodeBuilder.CreateNode(addActivityType, 
                addActivityName, addActivityCode));
        }

        /// <summary>
        /// Add node.
        /// </summary>
        public Workflow Add(string currentActivityCode, ActivityTypeEnum addActivityType, 
            NodeBuilder vb)
        {
            var currentNode = NodeBuilder.GetNode(this.Nodes, currentActivityCode);
            var existEdge = EdgeBuilder.GetEdgeFromSource(this.Edges, currentActivityCode);
            if (existEdge != null)
            {
                var targetNode = existEdge.Target;
                Insert(currentActivityCode, targetNode.Activity.ActivityCode, addActivityType, vb);
            }
            else
            {
                var addNode = vb.Node;
                addNode.Activity.ActivityType = addActivityType;

                NodeBuilder.SetPosition(addNode, currentNode.Left + currentNode.Width + CanvasPositionDefine.SHIFT_POSITION_WIDTH, currentNode.Top);
                NodeBuilder.AppendNode(this.Nodes, addNode);

                //create edge from insert node to current node
                Connect(currentNode, addNode);
            }
            return this;
        }

        /// <summary>
        /// Add task node.
        /// </summary>
        public Workflow AddTask(string currentActivityCode, string addActivityName, string addActivityCode)
        {
            return AddTask(currentActivityCode, NodeBuilder.CreateTask(addActivityName, addActivityCode));
        }

        /// <summary>
        /// Add task node.
        /// </summary>
        public Workflow AddTask(string currentActivityCode, NodeBuilder vb)
        {
            return Add(currentActivityCode, ActivityTypeEnum.TaskNode, vb);
        }

        /// <summary>
        /// Insert node.
        /// </summary>
        public Workflow Insert(string currentActivityCode, ActivityTypeEnum insertActivityType,
            string insertActivityName, string insertActivityCode)
        {
            return Insert(currentActivityCode, insertActivityType, NodeBuilder.CreateNode(insertActivityType,
                insertActivityName, insertActivityCode));
        }

        /// <summary>
        /// Insert node.
        /// </summary>
        public Workflow Insert(string currentActivityCode, ActivityTypeEnum insertActivityType,
            NodeBuilder vb)
        {
            //insert the node into list
            var currentNode = NodeBuilder.GetNode(this.Nodes, currentActivityCode);
            var insertNode = vb.Node;
            NodeBuilder.SetPosition(insertNode, currentNode.Left, currentNode.Top);
            NodeBuilder.AppendNode(this.Nodes, insertNode);

            //change edges target to the new insert node, replace the current node
            var edges = EdgeBuilder.GetEdgesToTarget(_flow.Edges, currentActivityCode);
            foreach (var edge in edges)
            {
                edge.Target = insertNode;
                EdgeBuilder.SetTransitionGUID(edge);
            }

            //create edge from insert node to current node
            Connect(insertNode, currentNode);
            
            //move backend vertices 
            MoveAfterwardsNodesPosition(currentNode, currentNode.Left, DEFAULT_SHIFT_DISTANCE);

            return this;
        }

        /// <summary>
        /// Insert task node.
        /// </summary>
        public Workflow InsertTask(string currentActivityCode, string insertActivityName, string insertActivityCode)
        {
            return InsertTask(currentActivityCode, NodeBuilder.CreateTask(insertActivityName, insertActivityCode));
        }

        /// <summary>
        /// Insert task node.
        /// </summary>
        public Workflow InsertTask(string currentActivityCode, NodeBuilder vb)
        {
            return Insert(currentActivityCode, ActivityTypeEnum.TaskNode, vb);
        }

        /// <summary>
        /// Insert node.
        /// </summary>
        public Workflow Insert(string frontActivityCode, string behindActivityCode, ActivityTypeEnum insertActivityType,
            string insertActivityName, string insertActivityCode)
        {
            return Insert(frontActivityCode, behindActivityCode, insertActivityType, 
                NodeBuilder.CreateNode(insertActivityType, insertActivityName, insertActivityCode)
                );
        }

        /// <summary>
        /// Insert node.
        /// </summary>
        public Workflow Insert(string frontActivityCode, string behindActivityCode, 
            ActivityTypeEnum insertActivityType, NodeBuilder vb)
        {
            var frontNode = NodeBuilder.GetNode(this.Nodes, frontActivityCode);
            var behindNode = NodeBuilder.GetNode(this.Nodes, behindActivityCode);

            //remove edge from first node to second node
            EdgeBuilder.RemoveEdge(this.Edges, frontActivityCode, behindActivityCode);

            var insertNode = vb.Node;
            insertNode.Activity.ActivityType = insertActivityType;

            //set position
            insertNode.Left = frontNode.Left + frontNode.Width + CanvasPositionDefine.SHIFT_POSITION_WIDTH;
            insertNode.Top = frontNode.Top;

            NodeBuilder.AppendNode(this.Nodes, insertNode);

            //edge between first node and target node
            Connect(frontNode, insertNode);

            //edge between target node and second node
            Connect(insertNode, behindNode);

            //adjust second node position, and afterwards vertices postion too
            MoveAfterwardsNodesPosition(behindNode, insertNode.Left, insertNode.Width + CanvasPositionDefine.SHIFT_POSITION_WIDTH);
            
            return this;
        }

        /// <summary>
        /// Insert task node
        /// 插入节点
        /// </summary>
        public Workflow InsertTask(string frontActivityCode, string behindActivityCode, string insertActivityName, string insertActivityCode)
        {
            return InsertTask(frontActivityCode, behindActivityCode, NodeBuilder.CreateTask(insertActivityName, insertActivityCode));
        }

        /// <summary>
        /// Insert node
        /// 插入节点
        /// </summary>
        public Workflow InsertTask(string frontActivityCode, string behindActivityCode, NodeBuilder vb)
        {
            return Insert(frontActivityCode, behindActivityCode, ActivityTypeEnum.TaskNode, vb);
        }

        /// <summary>
        /// Move a set of nodes forward (to the right) starting from current node.
        /// </summary>
        private void MoveAfterwardsNodesPosition(Node currentNode, int originalLeft, int distance)
        {
            if (originalLeft + distance > 0) currentNode.Left = originalLeft + distance;
            var edges = _flow.Edges;
            var list = edges.Where(t => t.Source == currentNode).ToList();
            foreach (var edge in list)
            {
                var target = edge.Target;
                if (target.Left > originalLeft)  target.Left = target.Left + distance;
                MoveAfterwardsNodesPositionInternal(target, originalLeft, distance);
            }
        }

        /// <summary>
        /// Move a set of nodes forward (to the right) recursively.
        /// </summary>
        private void MoveAfterwardsNodesPositionInternal(Node source, int originalLeft, int distance)
        {
            var edges = _flow.Edges;
            var list = edges.Where<Edge>(t => t.Source == source).ToList();
            foreach (var edge in list)
            {
                var target = edge.Target;
                if (target.Left > originalLeft) target.Left = target.Left + distance;
                MoveAfterwardsNodesPositionInternal(target, originalLeft, distance);
            }
        }

        /// <summary>
        /// Cover block operation (block insert between two activities).
        /// </summary>
        public Workflow Cover(string sourceActivityCode, string targetActivityCode,
            NodeBuilder splitNodeBuilder,  NodeBuilder joinNodeBuilder,
            NodeBuilder firstBranchNodeBuilder, NodeBuilder secondBranchNodeBuilder)
        {
            //add split node
            var splitNode = splitNodeBuilder.Node;
            Insert(sourceActivityCode, targetActivityCode, splitNode.Activity.ActivityType, splitNodeBuilder);

            //insert join node between split and target node
            var joinNode = joinNodeBuilder.Node;
            Insert(splitNode.Activity.ActivityCode, targetActivityCode, joinNode.Activity.ActivityType, joinNodeBuilder);

            //remove edge between split and join
            EdgeBuilder.RemoveEdge(this.Edges, splitNode.Activity.ActivityCode, joinNode.Activity.ActivityCode);

            //first branch
            var firstBranchNode = firstBranchNodeBuilder.Node;
            Fork(splitNode.Activity.ActivityCode, joinNode.Activity.ActivityCode, firstBranchNode.Activity.ActivityType, firstBranchNodeBuilder);

            //connect first branch and join node
            Connect(firstBranchNode.Activity.ActivityCode, joinNode.Activity.ActivityCode);

            //second branch
            var secondBranchNode = secondBranchNodeBuilder.Node;
            Fork(splitNode.Activity.ActivityCode, joinNode.Activity.ActivityCode, secondBranchNode.Activity.ActivityType, secondBranchNodeBuilder);

            //connect second branch and join node
            Connect(secondBranchNode.Activity.ActivityCode, joinNode.Activity.ActivityCode);

            return this;
        }

        /// <summary>
        /// Uncover block operation (block remove between two activities).
        /// </summary>
        public Workflow Uncover(string fromActivityCode, string toActivityCode)
        {
            var list = new List<string>();
            var edges = EdgeBuilder.GetEdgesFromSource(this.Edges, fromActivityCode);
            foreach (var edge in edges)
            {
                var targetActivityCode = edge.Target.Activity.ActivityCode;
                if (targetActivityCode != toActivityCode)
                {
                    if (!list.Contains(targetActivityCode)) list.Add(targetActivityCode);
                    GetChildNodeListFromSource(list, edge.Target.Activity.ActivityCode, toActivityCode);
                }
            }

            //remove all node from list and with edges
            foreach (var activityCode in list)
            {
                NodeBuilder.RemoveNodeWithEdges(this.Nodes, this.Edges, activityCode);
            }

            //move toActivity forward with delta calculation
            var fromNode = NodeBuilder.GetNode(this.Nodes, fromActivityCode);
            var toNode = NodeBuilder.GetNode(this.Nodes, toActivityCode);
            var deltaLevel = Math.Abs(Math.Abs(toNode.Level - fromNode.Level) - LEVEL_DELTA_FOR_UNCOVER);
            MoveAfterwardsNodesPosition(toNode, 
                toNode.Left, 
                deltaLevel * (0 - toNode.Width - CanvasPositionDefine.SHIFT_POSITION_WIDTH));

            return this;
        }

        /// <summary>
        /// Query the node set based on the starting node.
        /// </summary>
        private void GetChildNodeListFromSource(List<string> list, string sourceActivityCode, string finalActivityCode)
        {
            var edges = EdgeBuilder.GetEdgesFromSource(this.Edges, sourceActivityCode);
            foreach (var edge in edges)
            {
                var targetActivityCode = edge.Target.Activity.ActivityCode;
                if (targetActivityCode != finalActivityCode)
                {
                    if (!list.Contains(targetActivityCode)) list.Add(targetActivityCode);
                    GetChildNodeListFromSource(list, edge.Target.Activity.ActivityCode, finalActivityCode);
                }
            }
        }

        /// <summary>
        /// Connect source and target node.
        /// </summary>
        public Workflow Connect(string sourceActivityCode, string targetActivityCode)
        {
            var sourceNode = NodeBuilder.GetNode(this.Nodes, sourceActivityCode);
            var targetNode = NodeBuilder.GetNode(this.Nodes, targetActivityCode);
            Connect(sourceNode, targetNode);

            return this;
        }

        /// <summary>
        /// Connect source and target node
        /// 连接两个节点
        /// </summary>
        private Workflow Connect(Node sourceNode, Node targetNode)
        {
            var edge = EdgeBuilder.CreateEdge(this.Edges, sourceNode, targetNode);

            // Set the target node level
            SetTargetNodeLevel(edge.Target, edge.Source.Level);

            return this;
        }

        /// <summary>
        /// Disconnect between source and target.
        /// </summary>
        public Workflow Disconnect(string sourceActivityCode, string targetActivityCode)
        {
            EdgeBuilder.RemoveEdge(this.Edges, sourceActivityCode, targetActivityCode);
            return this;
        }

        /// <summary>
        /// Customize node via builder callback.
        /// </summary>
        public Workflow Set(string activityCode,  Func<string, NodeBuilder> vb)
        {
            vb(activityCode);
            return this;
        }

        /// <summary>
        /// Create fork.
        /// </summary>
        public Workflow Fork(string currentActivityCode, ActivityTypeEnum forkActivityType, string forkActivityName, string forkActivityCode)
        {
            return Fork(currentActivityCode, forkActivityType, NodeBuilder.CreateNode(forkActivityType, forkActivityName, forkActivityCode));
        }

        /// <summary>
        /// Create fork.
        /// </summary>
        public Workflow Fork(string currentActivityCode, ActivityTypeEnum forkActivityType, NodeBuilder vb)
        {
            var isExist = EdgeBuilder.HasEdgeFromSource(this.Edges, currentActivityCode);
            if (isExist == false)
            {
                Add(currentActivityCode, forkActivityType, vb);
            }
            else
            {
                var forkNode = vb.Node;
                var currentNode = NodeBuilder.GetNode(this.Nodes, currentActivityCode);
                //create edge between currentNode and forkNode
                Connect(currentNode, forkNode);

                NodeBuilder.SetPosition(forkNode, 
                    currentNode.Left + CanvasPositionDefine.SHIFT_POSITION_WIDTH, 
                    currentNode.Top + currentNode.Height + CanvasPositionDefine.BRANCH_POSITION_HEIGHT);
                NodeBuilder.AppendNode(this.Nodes, forkNode);
            }
            return this;
        }

        /// <summary>
        /// Create fork.
        /// </summary>
        public Workflow Fork(string firstActivityCode, string secondActivityCode, ActivityTypeEnum forkActivityType,
            string forkActivityName, string forkActivityCode)
        {
            return Fork(firstActivityCode, secondActivityCode, forkActivityType, NodeBuilder.CreateNode(forkActivityType, forkActivityName, forkActivityCode));
        }

        /// <summary>
        /// Create fork.
        /// </summary>
        public Workflow Fork(string firstActivityCode, string secondActivityCode, ActivityTypeEnum forkActivityType,
            NodeBuilder vb)
        {
            var firstNode = NodeBuilder.GetNode(this.Nodes, firstActivityCode);
            var secondNode = NodeBuilder.GetNode(this.Nodes, secondActivityCode);
            var forkNode = vb.Node;
            if (firstNode.Activity.ActivityType != ActivityTypeEnum.GatewayNode)
            {
                NodeBuilder.SetPosition(forkNode, 
                    firstNode.Left + CanvasPositionDefine.SHIFT_POSITION_WIDTH, 
                    firstNode.Top + CanvasPositionDefine.BRANCH_POSITION_HEIGHT);    //the sequence pattern
            }
            else
            {
                var top = GetTopIncludeGatewayNodes(firstActivityCode);
                NodeBuilder.SetPosition(forkNode, 
                    firstNode.Left + CanvasPositionDefine.SHIFT_POSITION_WIDTH, 
                    top + CanvasPositionDefine.BRANCH_POSITION_HEIGHT);
            }
            
            NodeBuilder.AppendNode(this.Nodes, forkNode);

            Connect(firstNode, forkNode);
            Connect(forkNode, secondNode);

            return this;
        }

        /// <summary>
        /// Get top position including gateway nodes.
        /// </summary>
        private int GetTopIncludeGatewayNodes(string splitNodeCode)
        {
            int top = 0;
            var edges = EdgeBuilder.GetEdgesFromSource(this.Edges, splitNodeCode);
            foreach (var edge in edges)
            {
                if (edge.Target.Top > top) top = edge.Target.Top;
            }
            return top;
        }

        /// <summary>
        /// Replace node
        /// 取代节点
        /// </summary>
        public Workflow Replace(string currentActivityCode, ActivityTypeEnum replacedByActivityType, 
            string replacedByActivityName, string replacedByActivityCode)
        {
            return Replace(currentActivityCode, NodeBuilder.CreateNode(replacedByActivityType, replacedByActivityName, replacedByActivityCode));
        }

        /// <summary>
        /// Replace node
        /// 取代节点
        /// </summary>
        public Workflow Replace(string currentActivityCode, NodeBuilder vb)
        {
            var currentNode = NodeBuilder.GetNode(this.Nodes, currentActivityCode);
            var replacedByNode = vb.GetNode();

            //replace soure property in the edges
            //replace target property in the edges
            EdgeBuilder.ReplaceSourceTarget(this.Edges, currentActivityCode, replacedByNode);

            //adjust new node position
            NodeBuilder.SetPosition(replacedByNode, currentNode.Left, currentNode.Top);
            
            //remove original node from the list
            NodeBuilder.RemoveNode(this.Nodes, currentNode);

            //add the new node into the list
            NodeBuilder.AppendNode(this.Nodes, replacedByNode);

            return this;
        }
            
        /// <summary>
        /// Exchange node
        /// Table:
        ///         source      target
        ///         1           x          source=2
        ///         2           x          source=1
        ///         x           1          target=2
        ///         x           2          target=1
        ///         1           2          source=2;target=1
        ///         2           1          source=1;target=2
        /// </summary>
        /// <param name="firstActivityCode"></param>
        /// <param name="secondActivityCode"></param>
        /// <returns></returns>
        public Workflow Exchange(string firstActivityCode, string secondActivityCode)
        {
            var firstNode = NodeBuilder.GetNode(this.Nodes, firstActivityCode);
            var secondNode = NodeBuilder.GetNode(this.Nodes, secondActivityCode);

            var list = _flow.Edges.Where(t => t.Source.Activity.ActivityCode == firstActivityCode
                || t.Target.Activity.ActivityCode == firstActivityCode
                || t.Source.Activity.ActivityCode == secondActivityCode
                || t.Target.Activity.ActivityCode == secondActivityCode).ToList();
            foreach (var edge in list)
            {
                if (edge.Source.Activity.ActivityCode == firstActivityCode)
                {
                    edge.Source = secondNode;
                    if (edge.Target.Activity.ActivityCode == secondActivityCode)
                    {
                        edge.Target = firstNode;
                    }
                }
                else if(edge.Source.Activity.ActivityCode == secondActivityCode)
                {
                    edge.Source = firstNode;
                    if (edge.Target.Activity.ActivityCode == firstActivityCode)
                    {
                        edge.Target = secondNode;
                    }
                }
                else
                {
                    if (edge.Target.Activity.ActivityCode == firstActivityCode)
                    {
                        edge.Target = secondNode;
                    }
                    else
                    {
                        edge.Target = firstNode;
                    }
                }
                EdgeBuilder.SetTransitionGUID(edge);
            }

            //adjust two node position
            int firstLeft = firstNode.Left, firstTop = firstNode.Top, secondLeft = secondNode.Left, secondTop = secondNode.Top;
            NodeBuilder.SetPosition(firstNode, secondLeft, secondTop);
            NodeBuilder.SetPosition(secondNode, firstLeft, firstTop);

            return this;
        }

        /// <summary>
        /// Remove node
        /// </summary>
        public Workflow Remove(string activityCode, Boolean isCaughtUp = false)
        {
            if (isCaughtUp == true)
            {
                var list = _flow.Edges.Where(t => t.Source.Activity.ActivityCode == activityCode).ToList();
                if (list.Count == 1)
                {
                    //the follow up node as new target node to replace the deleting node
                    var followNode = list[0].Target;
                    var listA = _flow.Edges.Where(t => t.Target.Activity.ActivityCode == activityCode).ToList();
                    foreach (var edge in listA)
                    {
                        edge.Target = followNode;
                        EdgeBuilder.SetTransitionGUID(edge);
                    }
                    //the afterward node can be caught up after removed
                    MoveAfterwardsNodesPosition(followNode, followNode.Left, 0-followNode.Width-CanvasPositionDefine.SHIFT_POSITION_WIDTH);
                }
            }

            //remove edges related with the removed node
            EdgeBuilder.RemoveEdge(_flow.Edges, activityCode);

            NodeBuilder.RemoveNode(_flow.Nodes, activityCode);
            
            return this;
        }
        #endregion

        #region Static create process
        /// <summary>
        /// Create process (obsolete - use constructor instead)
        /// </summary>
        public static Workflow CreateProcess(string name, string code)
        {
            return new Workflow(name, code);
        }

        /// <summary>
        /// Create process with specific processId and version
        /// 使用指定的 processId 和 version 创建流程
        /// </summary>
        public static Workflow Create(string name, string code, string processId, string version)
        {
            var wf = new Workflow();
            wf._flow = new Flow(name, code, processId, version);
            return wf;
        }

        /// <summary>
        /// Load process
        /// 获取流程
        /// </summary>
        public static Workflow LoadProcess(string code, string version = null)
        {
            var wf = new Workflow();
            if (string.IsNullOrEmpty(version)) version = "1";
            var wfService = new WorkflowService();
            var entity = wfService.GetProcessByCode(code, version);

            if (entity != null)
            {
                wf._flow = new Flow(entity.ProcessName, entity.ProcessCode, entity.ProcessId, entity.Version);
                wf._flow.Load(entity);
            }
            else
            {
                throw new ApplicationException("There isn't a process with this process code and version");
            }
            return wf;
        }
        #endregion
    }
}
