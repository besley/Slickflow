using Slickflow.Engine.Common;
using Slickflow.Engine.External;
using Slickflow.Engine.Xpdl.Common;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Module.Resource;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Slickflow.Graph.Model
{
    /// <summary>
    /// Node builder: fluent helpers to construct workflow nodes and attach roles, actions, services, scripts and layout info.
    /// </summary>
    public class NodeBuilder
    {
        #region Core node property
        private Node _node = null;

        internal Node Node
        {
            get
            {
                return _node;
            }
        }

        internal Node GetNode()
        {
            return _node;
        }

        public void SetNode(Node node)
        {
            _node = node;
        }
        #endregion

        #region Attach node metadata
        /// <summary>
        /// Set activity display name.
        /// </summary>
        /// <param name="name">Activity name</param>
        /// <returns>NodeBuilder</returns>
        public NodeBuilder SetName(string name)
        {
            _node.Activity.ActivityName = name;
            return this;
        }

        /// <summary>
        /// Set activity code.
        /// </summary>
        /// <param name="code">Activity code</param>
        /// <returns>NodeBuilder</returns>
        public NodeBuilder SetCode(string code)
        {
            _node.Activity.ActivityCode = code;
            return this;
        }
        /// <summary>
        /// Set activity page url.
        /// </summary>
        /// <param name="url">Page url</param>
        /// <returns>NodeBuilder</returns>
        public NodeBuilder SetUrl(string url)
        {
            _node.Activity.ActivityUrl = url;

            return this;
        }

        /// <summary>
        /// Add an executor role for this activity by role code.
        /// </summary>
        /// <param name="roleCode">Role code</param>
        /// <returns>NodeBuilder</returns>
        public NodeBuilder AddRole(string roleCode)
        {
            if (_node.RoleList == null) _node.RoleList = new List<Role>();

            var rs = ResourceServiceFactory.Create();
            var role = rs.GetRoleByCode(roleCode);
            AppendRoleIntoRoleList(_node.RoleList, role);

            return this;
        }

        /// <summary>
        /// Append role instance into role list if not already exists.
        /// </summary>
        /// <param name="roleList">Role list</param>
        /// <param name="role">Role entity</param>
        private void AppendRoleIntoRoleList(IList<Role> roleList, Role role)
        {
            var isExist = false;
            foreach (var r in roleList)
            {
                if (r.RoleName == role.RoleName && r.RoleCode == role.RoleCode)
                {
                    isExist = true;
                    break;
                }
            }
            if (isExist == false) roleList.Add(role);
        }


        /// <summary>
        /// Add boundary configuration to current activity.
        /// </summary>
        /// <param name="boundary">Boundary definition</param>
        /// <returns>NodeBuilder</returns>
        public NodeBuilder AddBoundary(Boundary boundary)
        {
            if (_node.Activity.BoundaryList == null) _node.Activity.BoundaryList = new List<Boundary>();
            _node.Activity.BoundaryList.Add(boundary);

            return this;
        }

        /// <summary>
        /// Add action definition to current activity.
        /// </summary>
        /// <param name="action">Action</param>
        /// <returns>NodeBuilder</returns>
        public NodeBuilder AddAction(Engine.Xpdl.Entity.Action action)
        {
            if (_node.Activity.ActionList == null) _node.Activity.ActionList = new List<Engine.Xpdl.Entity.Action>();
            _node.Activity.ActionList.Add(action);

            return this;
        }

        /// <summary>
        /// Add service definition to current activity.
        /// </summary>
        /// <param name="service">Service detail</param>
        /// <returns>NodeBuilder</returns>
        public NodeBuilder AddService(ServiceDetail service)
        {
            if (_node.Activity.ServiceList == null) _node.Activity.ServiceList = new List<ServiceDetail>();
            _node.Activity.ServiceList.Add(service);

            return this;
        }

        /// <summary>
        /// Add script definition to current activity.
        /// </summary>
        /// <param name="script">Script detail</param>
        /// <returns>NodeBuilder</returns>
        public NodeBuilder AddScript(ScriptDetail script)
        {
            if (_node.Activity.ScriptList == null) _node.Activity.ScriptList = new List<ScriptDetail>();
            _node.Activity.ScriptList.Add(script);

            return this;
        }

        /// <summary>
        /// Configure multi-instance (countersign) properties for the activity.
        /// </summary>
        /// <param name="complexType">Multi-instance type</param>
        /// <param name="mergeType">Merge strategy</param>
        /// <param name="compareType">Completion evaluation type</param>
        /// <param name="completeOrder">Completion threshold (percentage or ratio)</param>
        /// <returns>NodeBuilder</returns>
        public NodeBuilder SetMultipleInstance(ComplexTypeEnum complexType,
            MergeTypeEnum mergeType,
            CompareTypeEnum compareType,
            float completeOrder)
        {
            _node.Activity.MultiSignDetail.ComplexType = complexType;
            _node.Activity.MultiSignDetail.MergeType = mergeType;
            _node.Activity.MultiSignDetail.CompareType = compareType;
            _node.Activity.MultiSignDetail.CompleteOrder = completeOrder;

            return this;
        }

        /// <summary>
        /// Configure sub-process binding (reserved for future use).
        /// </summary>
        /// <param name="processName">Sub-process name</param>
        /// <param name="version">Sub-process version</param>
        /// <returns>NodeBuilder</returns>
        public NodeBuilder SetSubProcess(string processName, string version = null)
        {
            //_node.Activity.sub
            return this;
        }
        #endregion

        #region Static factory helpers
        /// <summary>
        /// Create a start event node.
        /// </summary>
        /// <param name="activityName">Activity name</param>
        /// <param name="activityCode">Activity code</param>
        /// <param name="triggerType">Trigger type</param>
        /// <returns>NodeBuilder</returns>
        public static NodeBuilder CreateStart(string activityName = null, string activityCode = null, TriggerTypeEnum triggerType = TriggerTypeEnum.None)
        {
            if (string.IsNullOrEmpty(activityName)) activityName = "Start";
            var nb = CreateNode(ActivityTypeEnum.StartNode, activityName, activityCode, triggerType);

            return nb;
        }

        /// <summary>
        /// Create an intermediate event node.
        /// </summary>
        /// <param name="activityName">Activity name</param>
        /// <param name="activityCode">Activity code</param>
        /// <param name="triggerType">Trigger type</param>
        /// <returns>NodeBuilder</returns>
        public static NodeBuilder CreateIntermediate(string activityName = null, string activityCode = null, TriggerTypeEnum triggerType = TriggerTypeEnum.None)
        {
            if (string.IsNullOrEmpty(activityName)) activityName = "Intermediate";
            var nb = CreateNode(ActivityTypeEnum.IntermediateNode, activityName, activityCode, triggerType);

            return nb;
        }

        /// <summary>
        /// Create an end event node.
        /// </summary>
        /// <param name="activityName">Activity name</param>
        /// <param name="activityCode">Activity code</param>
        /// <param name="triggerType">Trigger type</param>
        /// <returns>NodeBuilder</returns>
        public static NodeBuilder CreateEnd(string activityName = null, string activityCode = null, TriggerTypeEnum triggerType = TriggerTypeEnum.None)
        {
            if (string.IsNullOrEmpty(activityName)) activityName = "End";
            var nb = CreateNode(ActivityTypeEnum.EndNode, activityName, activityCode, triggerType);

            return nb;
        }

        /// <summary>
        /// Create a task node.
        /// </summary>
        /// <param name="activityName">Activity name</param>
        /// <param name="activityCode">Activity code</param>
        /// <returns>NodeBuilder</returns>
        public static NodeBuilder CreateTask(string activityName, string activityCode = null)
        {
            if (string.IsNullOrEmpty(activityName))
            {
                throw new ApplicationException("Activity name and code cannot be empty!");
            }

            if (string.IsNullOrEmpty(activityCode))
            {
                activityCode = $"Activity_{Utility.GetRandomString(6)}" ;
            }
            var nb = CreateNode(ActivityTypeEnum.TaskNode, activityName, activityCode);

            return nb;
        }

        /// <summary>
        /// Create a service task node and bind a local plugin service by generic type.
        /// </summary>
        /// <typeparam name="TService">Concrete service type that inherits ExternalServiceBase and implements IExternalService</typeparam>
        /// <param name="activityName">Activity name</param>
        /// <param name="activityCode">Activity code (auto-generated when null or empty)</param>
        /// <returns>NodeBuilder</returns>
        public static NodeBuilder CreateServiceTask<TService>(string activityName, string activityCode = null)
            where TService : ExternalServiceBase, IExternalService
        {
            if (string.IsNullOrEmpty(activityName))
            {
                throw new ApplicationException("Activity name cannot be empty!");
            }

            if (string.IsNullOrEmpty(activityCode))
            {
                activityCode = $"Activity_{Utility.GetRandomString(6)}";
            }

            // 1) Create a ServiceTask node
            var nb = CreateNode(ActivityTypeEnum.ServiceNode, activityName, activityCode);

            // 2) Bind ServiceDetail, expression uses the concrete implementation FullName
            var serviceDetail = new ServiceDetail
            {
                Method = ServiceMethodEnum.LocalService,
                Expression = typeof(TService).FullName
            };
            nb.AddService(serviceDetail);

            return nb;
        }

        /// <summary>
        /// Create a service task node bound to a local method by registry key (LocalMethod, BPMN2 ##DelegateExpression semantic).
        /// Register the delegate with ServiceTaskDelegateRegistry.Global.Register(key, delegate) before Run.
        /// </summary>
        public static NodeBuilder CreateServiceTask(string activityName, string activityCode, string delegateKey)
        {
            if (string.IsNullOrEmpty(activityName))
                throw new ApplicationException("Activity name cannot be empty!");
            if (string.IsNullOrEmpty(delegateKey))
                throw new ArgumentException("Delegate key cannot be empty.", nameof(delegateKey));
            if (string.IsNullOrEmpty(activityCode))
                activityCode = $"Activity_{Utility.GetRandomString(6)}";

            var nb = CreateNode(ActivityTypeEnum.ServiceNode, activityName, activityCode);
            var serviceDetail = new ServiceDetail
            {
                Method = ServiceMethodEnum.LocalMethod,
                Expression = delegateKey
            };
            nb.AddService(serviceDetail);
            return nb;
        }

        /// <summary>
        /// Create an AI service node (generic AI service task, no type specified).
        /// </summary>
        /// <param name="activityName">Activity name</param>
        /// <param name="activityCode">Activity code</param>
        /// <returns>NodeBuilder</returns>
        public static NodeBuilder CreateAgent(string activityName, string activityCode = null)
        {
            if (string.IsNullOrEmpty(activityName))
            {
                throw new ApplicationException("Activity name and code cannot be empty!");
            }

            if (string.IsNullOrEmpty(activityCode))
            {
                activityCode = Utility.GetRandomString(6);
            }
            var nb = CreateNode(ActivityTypeEnum.AIServiceNode, activityName, activityCode);

            return nb;
        }

        /// <summary>
        /// Create a RAG AI service task node.
        /// </summary>
        public static NodeBuilder CreateRagService(string activityName, string activityCode = null)
        {
            if (string.IsNullOrEmpty(activityName))
            {
                throw new ApplicationException("Activity name and code cannot be empty!");
            }

            if (string.IsNullOrEmpty(activityCode))
            {
                activityCode = Utility.GetRandomString(6);
            }

            var nb = CreateNode(ActivityTypeEnum.AIServiceNode, activityName, activityCode);
            if (nb._node.Activity.AIServiceList == null)
                nb._node.Activity.AIServiceList = new List<AiServiceDetail>();

            nb._node.Activity.AIServiceList.Add(new AiServiceDetail
            {
                AIServiceType = AiServiceTypeEnum.RAG
            });

            return nb;
        }

        /// <summary>
        /// Create an LLM AI service task node.
        /// </summary>
        public static NodeBuilder CreateLlmService(string activityName, string activityCode = null)
        {
            if (string.IsNullOrEmpty(activityName))
            {
                throw new ApplicationException("Activity name and code cannot be empty!");
            }

            if (string.IsNullOrEmpty(activityCode))
            {
                activityCode = Utility.GetRandomString(6);
            }

            var nb = CreateNode(ActivityTypeEnum.AIServiceNode, activityName, activityCode);
            if (nb._node.Activity.AIServiceList == null)
                nb._node.Activity.AIServiceList = new List<AiServiceDetail>();

            nb._node.Activity.AIServiceList.Add(new AiServiceDetail
            {
                AIServiceType = AiServiceTypeEnum.LLM
            });

            return nb;
        }

        /// <summary>
        /// Create a split gateway node.
        /// </summary>
        /// <param name="direction">Gateway direction type</param>
        /// <param name="activityName">Activity name</param>
        /// <param name="activityCode">Activity code</param>
        /// <returns>NodeBuilder</returns>
        public static NodeBuilder CreateSplit(Nullable<GatewayDirectionEnum> direction,
            string activityName = null,
            string activityCode = null)
        {
            if (string.IsNullOrEmpty(activityName)) activityName = "Split";
            var nb = CreateNode(ActivityTypeEnum.GatewayNode, activityName, activityCode);

            if (nb._node.Activity.GatewayDetail == null) nb._node.Activity.GatewayDetail = new GatewayDetail();

            nb._node.Activity.GatewayDetail.SplitJoinType = GatewaySplitJoinTypeEnum.Split;
            if (direction != null) nb._node.Activity.GatewayDetail.DirectionType = direction.Value;

            return nb;
        }

        /// <summary>
        /// Create a join gateway node.
        /// </summary>
        /// <param name="direction">Gateway direction type</param>
        /// <param name="activityName">Activity name</param>
        /// <param name="activityCode">Activity code</param>
        /// <returns>NodeBuilder</returns>
        public static NodeBuilder CreateJoin(Nullable<GatewayDirectionEnum> direction,
            string activityName = null,
            string activityCode = null)
        {
            if (string.IsNullOrEmpty(activityName)) activityName = "Join";
            var nb = CreateNode(ActivityTypeEnum.GatewayNode, activityName, activityCode);

            if (nb._node.Activity.GatewayDetail == null) nb._node.Activity.GatewayDetail = new GatewayDetail();

            nb._node.Activity.GatewayDetail.SplitJoinType = GatewaySplitJoinTypeEnum.Join;
            if (direction != null) nb._node.Activity.GatewayDetail.DirectionType = direction.Value;

            return nb;
        }

        /// <summary>
        /// Create a multi-instance (countersign) task node.
        /// </summary>
        /// <param name="activityName">Activity name</param>
        /// <param name="activityCode">Activity code</param>
        /// <returns>NodeBuilder</returns>
        public static NodeBuilder CreateMultipleInstance(string activityName = null, string activityCode = null)
        {
            var nb = CreateNode(ActivityTypeEnum.MultiSignNode, activityName, activityCode);
            return nb;
        }

        /// <summary>
        /// Create a sub-process node.
        /// </summary>
        /// <param name="subProcessCode">Sub-process code</param>
        /// <param name="activityName">Activity name</param>
        /// <param name="activityCode">Activity code</param>
        /// <returns>NodeBuilder</returns>
        public static NodeBuilder CreateSubProcess(string subProcessCode, string activityName = null, string activityCode = null)
        {
            if (string.IsNullOrEmpty(activityName)) activityName = "subprocess";
            if (string.IsNullOrEmpty(activityCode)) activityCode = "subprocess";

            var nb = CreateNode(ActivityTypeEnum.SubProcessNode, activityName, activityCode);
            return nb;
        }

        /// <summary>
        /// Internal helper to create a node with default size and initial canvas position.
        /// </summary>
        /// <param name="activityType">Activity type</param>
        /// <param name="activityName">Activity name</param>
        /// <param name="activityCode">Activity code</param>
        /// <param name="triggerType">Trigger type</param>
        /// <returns>NodeBuilder</returns>
        internal static NodeBuilder CreateNode(ActivityTypeEnum activityType, string activityName,
            string activityCode = null, 
            TriggerTypeEnum triggerType = TriggerTypeEnum.None)
        {
            var nb = new NodeBuilder();
            var node = new Node(activityType, activityName, activityCode, triggerType);

            if (activityType == ActivityTypeEnum.StartNode)
            {
                // Place start node at the origin of the canvas
                node.Left = CanvasPositionDefine.CANVAS_POSITION_LEFT;
                node.Top = CanvasPositionDefine.CANVAS_POSITION_TOP;
                node.Level = 1;
            }

            // Set default node width and height based on activity type
            SetNodeShape(activityType, node);

            nb._node = node;

            return nb;
        }

        /// <summary>
        /// Set node visual shape based on activity type.
        /// </summary>
        /// <param name="activityType">Activity type</param>
        /// <param name="node">Node</param>
        private static void SetNodeShape(ActivityTypeEnum activityType, Node node)
        {
            if (activityType == ActivityTypeEnum.StartNode
                || activityType == ActivityTypeEnum.IntermediateNode
                || activityType == ActivityTypeEnum.EndNode)
            {
                node.Width = CanvasPositionDefine.EVENT_POSITION_WIDTH;
                node.Height = CanvasPositionDefine.EVENT_POSITION_HEIGHT;
            }
            else if (activityType == ActivityTypeEnum.GatewayNode)
            {
                node.Width = CanvasPositionDefine.GATEWAY_POSITION_WIDTH;
                node.Height = CanvasPositionDefine.GATEWAY_POSITION_HEIGHT;
            }
            else
            {
                node.Width = CanvasPositionDefine.TASK_POSITION_WIDTH;
                node.Height = CanvasPositionDefine.TASK_POSITION_HEIGHT;
            }
        }
        #endregion

        #region Static list helpers
        /// <summary>
        /// Get node by activity code.
        /// </summary>
        /// <param name="nodes">Node list</param>
        /// <param name="activityCode">Activity code</param>
        /// <returns>Node</returns>
        internal static Node GetNode(IList<Node> nodes, string activityCode)
        {
            var node = nodes.Single<Node>(v => v.Activity.ActivityCode == activityCode);
            return node;
        }

        /// <summary>
        /// Get node by activity id.
        /// </summary>
        internal static Node GetNodeById(IList<Node> nodes, string activityId)
        {
            var node = nodes.Single<Node>(v => v.Activity.ActivityId == activityId);
            return node;
        }

        /// <summary>
        /// Get node by activity name.
        /// </summary>
        internal static Node GetNodeByName(IList<Node> nodes, string activityName)
        {
            var node = nodes.Single<Node>(v => v.Activity.ActivityName == activityName);
            return node;
        }

        /// <summary>
        /// Append node when activity code does not exist in list; throws if duplicated.
        /// </summary>
        internal static void AppendNode(IList<Node> nodes, Node node)
        {
            var isExist = nodes.Any<Node>(v => v.Activity.ActivityCode == node.Activity.ActivityCode);
            if (isExist == false)
                nodes.Add(node);
            else
                throw new ApplicationException(string.Format("Node Code{0} already exist, please assign a new node code!", node.Activity.ActivityCode));
        }

        /// <summary>
        /// Remove node from list.
        /// </summary>
        internal static void RemoveNode(IList<Node> nodes, Node node)
        {
            nodes.Remove(node);
        }

        internal static void RemoveNode(IList<Node> nodes, string activityCode)
        {
            var node = GetNode(nodes, activityCode);
            nodes.Remove(node);
        }

        /// <summary>
        /// Remove node and its incoming/outgoing edges.
        /// </summary>
        /// <param name="nodes">Node collection</param>
        /// <param name="edges">Edge collection</param>
        /// <param name="activityCode">Activity code</param>
        internal static void RemoveNodeWithEdges(IList<Node> nodes, IList<Edge> edges, string activityCode)
        {
            var node = GetNode(nodes, activityCode);
            if (node != null)
            {
                var listA = edges.Where(a => a.Source == node).ToList();
                RemoveEdges(edges, listA);

                var listB = edges.Where(a => a.Target == node).ToList();
                RemoveEdges(edges, listB);
            }
            nodes.Remove(node);
        }

        /// <summary>
        /// Remove edges from collection.
        /// </summary>
        /// <param name="edges">Edge collection</param>
        /// <param name="removedEdges">Edges to remove</param>
        private static void RemoveEdges(IList<Edge> edges, IEnumerable<Edge> removedEdges            )
        {
            foreach (var t in removedEdges)
            {
                edges.Remove(t);
            }
        }
        #endregion

        #region Factory helpers for attached configuration
        /// <summary>
        /// Create an Action definition.
        /// </summary>
        /// <param name="actionType">Action type</param>
        /// <param name="fireType">Fire position type</param>
        /// <param name="arguments">Argument list</param>
        /// <param name="expression">Expression</param>
        /// <param name="codeText">Inline code text</param>
        /// <returns>Action entity</returns>
        private static Engine.Xpdl.Entity.Action CreateAction(ActionTypeEnum actionType,
            ActionMethodEnum methodType,
            FireTypeEnum fireType,
            string arguments,
            string expression,
            string codeText = null)
        {
            var action = new Engine.Xpdl.Entity.Action();
            action.ActionType = actionType;
            action.FireType = fireType;
            action.ActionMethod = methodType;
            action.Arguments = arguments;
            action.Expression = expression;

            return action;
        }

        /// <summary>
        /// Create a local-service Action that invokes a C# service by expression.
        /// </summary>
        /// <param name="fireType">Fire position</param>
        /// <param name="arguments">Arguments</param>
        /// <param name="expression">Expression</param>
        /// <returns>Action entity</returns>
        public static Engine.Xpdl.Entity.Action CreateActionLocalService(FireTypeEnum fireType,
            string arguments,
            string expression)
        {
            return CreateAction(ActionTypeEnum.Event, ActionMethodEnum.LocalService, fireType, arguments, expression);
        }

        /// <summary>
        /// Create an Action that executes inline C# code.
        /// </summary>
        /// <param name="fireType">Fire position</param>
        /// <param name="arguments">Argument list</param>
        /// <param name="codeText">Code expression</param>
        /// <returns>Action entity</returns>
        public static Engine.Xpdl.Entity.Action CreateActionCSharpCode(FireTypeEnum fireType,
            string arguments,
            string codeText)
        {
            return CreateAction(ActionTypeEnum.Event, ActionMethodEnum.CSharpLibrary, fireType, arguments, null, codeText);
        }

        /// <summary>
        /// Create a WebApi Action definition.
        /// </summary>
        /// <param name="fireType">Fire position</param>
        /// <param name="subMethod">WebApi HTTP method</param>
        /// <param name="expression">Endpoint expression</param>
        /// <returns>Action entity</returns>
        public static Engine.Xpdl.Entity.Action CreateActionWebApi(FireTypeEnum fireType,
            SubMethodEnum subMethod,
            string expression)
        {
            var action = new Engine.Xpdl.Entity.Action();
            action.ActionType = ActionTypeEnum.Event;
            action.ActionMethod = ActionMethodEnum.WebApi;
            action.SubMethod = subMethod;
            action.FireType = fireType;
            action.Expression = expression;

            return action;
        }

        /// <summary>
        /// Create an Action that executes a SQL script.
        /// </summary>
        /// <param name="fireType">Fire position</param>
        /// <param name="arguments">Argument list</param>
        /// <param name="script">SQL script or code</param>
        /// <returns>Action entity</returns>
        public static Engine.Xpdl.Entity.Action CreateActionSQLScript(FireTypeEnum fireType,
            string arguments,
            string script)
        {
            return CreateAction(ActionTypeEnum.Event, ActionMethodEnum.SQL, fireType, arguments, null, script);
        }

        /// <summary>
        /// Create an Action that executes a stored procedure.
        /// </summary>
        /// <param name="fireType">Fire position</param>
        /// <param name="arguments">Argument list</param>
        /// <param name="expression">Procedure name or code</param>
        /// <returns>Action entity</returns>
        public static Engine.Xpdl.Entity.Action CreateActionStoreProcedure(FireTypeEnum fireType,
            string arguments,
            string expression)
        {
            return CreateAction(ActionTypeEnum.Event, ActionMethodEnum.StoreProcedure, fireType, arguments, expression);
        }

        /// <summary>
        /// Create an Action that executes a Python script.
        /// </summary>
        /// <param name="fireType">Fire position</param>
        /// <param name="arguments">Argument list</param>
        /// <param name="script">Script or code</param>
        /// <returns>Action entity</returns>
        public static Engine.Xpdl.Entity.Action CreateActionPythonScript(FireTypeEnum fireType,
            string arguments,
            string script)
        {
            return CreateAction(ActionTypeEnum.Event, ActionMethodEnum.Python, fireType, arguments, null, script);
        }

        /// <summary>
        /// Create a boundary definition for an activity.
        /// </summary>
        /// <param name="trigger">Event trigger type</param>
        /// <param name="expression">Trigger expression</param>
        /// <returns>Boundary entity</returns>
        public static Boundary CreateBoundary(EventTriggerEnum trigger, string expression)
        {
            var boundary = new Boundary();
            boundary.EventTriggerType = trigger;
            boundary.Expression = expression;

            return boundary;
        }

        /// <summary>
        /// Set node canvas position.
        /// </summary>
        /// <param name="node">Node</param>
        /// <param name="left">Left position</param>
        /// <param name="top">Top position</param>
        public static void SetPosition(Node node, int left, int top)
        {
            node.Left = left;
            node.Top = top;
        }
        #endregion
    }
}
