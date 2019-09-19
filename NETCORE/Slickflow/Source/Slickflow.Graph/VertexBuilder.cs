using System;
using System.Collections.Generic;
using Slickflow.Engine.Common;
using Slickflow.Engine.Xpdl;
using Slickflow.Module.Resource;

namespace Slickflow.Graph
{
    /// <summary>
    /// 节点构造器
    /// </summary>
    public class VertexBuilder
    {
        #region 基本属性
        internal Vertex _vertex = null;
        #endregion

        #region 添加节点附属配置
        /// <summary>
        /// 添加页面地址
        /// </summary>
        /// <param name="url">Url</param>
        /// <returns>节点构造器</returns>
        public VertexBuilder SetUrl(string url)
        {
            _vertex.Activity.ActivityUrl = url;

            return this;
        }

        /// <summary>
        /// 添加执行角色
        /// </summary>
        /// <param name="roleCode">执行角色</param>
        /// <returns>节点构造器</returns>
        public VertexBuilder AddRole(string roleCode)
        {
            if (_vertex.RoleList == null) _vertex.RoleList = new List<Role>();

            var rs = ResourceServiceFactory.Create();
            var role = rs.GetRoleByCode(roleCode);
            AppendRoleIntoRoleList(_vertex.RoleList, role);
            
            return this;
        }

        /// <summary>
        /// 添加角色到角色列表
        /// </summary>
        /// <param name="roleList">角色列表</param>
        /// <param name="role">角色</param>
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
        /// 添加边界属性
        /// </summary>
        /// <param name="boundary">边界</param>
        /// <returns>节点构造器</returns>
        public VertexBuilder AddBoundary(BoundaryEntity boundary)
        {
            if (_vertex.Activity.BoundaryList == null) _vertex.Activity.BoundaryList = new List<BoundaryEntity>();
            _vertex.Activity.BoundaryList.Add(boundary);

            return this;
        }

        /// <summary>
        /// 添加操作类
        /// </summary>
        /// <param name="action">操作</param>
        /// <returns>节点构造器</returns>
        public VertexBuilder AddAction(ActionEntity action)
        {
            if (_vertex.Activity.ActionList == null) _vertex.Activity.ActionList = new List<ActionEntity>();
            _vertex.Activity.ActionList.Add(action);

            return this;
        }

        /// <summary>
        /// 设置会签节点属性
        /// </summary>
        /// <param name="complexType">会签类型</param>
        /// <param name="mergeType">合并类型</param>
        /// <param name="compareType">通过类型</param>
        /// <param name="completeOrder">通过率</param>
        /// <returns>节点构造器</returns>
        public VertexBuilder SetMultipleInstance(ComplexTypeEnum complexType, 
            MergeTypeEnum mergeType,
            CompareTypeEnum compareType,
            float completeOrder)
        {
            _vertex.Activity.ActivityTypeDetail.ComplexType = complexType;
            _vertex.Activity.ActivityTypeDetail.MergeType = mergeType;
            _vertex.Activity.ActivityTypeDetail.CompareType = compareType;
            _vertex.Activity.ActivityTypeDetail.CompleteOrder = completeOrder;

            return this;
        }

        public VertexBuilder SetSubProcess(string processName, string version = null)
        {
            //_vertex.Activity.sub
            return this;
        }
        #endregion

        #region 静态创建方法
        /// <summary>
        /// 创建开始节点
        /// </summary>
        /// <param name="name">节点名称</param>
        /// <param name="code">节点代码</param>
        /// <returns>节点构造器</returns>
        public static VertexBuilder CreateStart(string name = null, string code = null)
        {
            if (string.IsNullOrEmpty(name)) name = "Start";
            var vb = CreateVertex(ActivityTypeEnum.StartNode, name, code);

            return vb;
        }

        /// <summary>
        /// 创建结束节点
        /// </summary>
        /// <param name="name">节点名称</param>
        /// <param name="code">节点代码</param>
        /// <returns>节点构造器</returns>
        public static VertexBuilder CreateEnd(string name = null, string code = null)
        {
            if (string.IsNullOrEmpty(name)) name = "End";
            var vb = CreateVertex(ActivityTypeEnum.EndNode, name, code);

            return vb;
        }

        /// <summary>
        /// 创建任务节点
        /// </summary>
        /// <param name="name">节点名称</param>
        /// <param name="code">节点代码</param>
        /// <returns>节点构造器</returns>
        public static VertexBuilder CreateTask(string name = null, string code = null)
        {
            if (string.IsNullOrEmpty(name)) name = "Task";
            var vb = CreateVertex(ActivityTypeEnum.TaskNode, name, code);

            return vb;
        }

        /// <summary>
        /// 创建分支节点
        /// </summary>
        /// <param name="direction">分支类型</param>
        /// <param name="name">节点名称</param>
        /// <param name="code">节点代码</param>
        /// <returns>节点构造器</returns>
        public static VertexBuilder CreateSplit(Nullable<GatewayDirectionEnum> direction,
            string name = null, 
            string code = null)
        {
            if (string.IsNullOrEmpty(name)) name = "Split";
            var vb = CreateVertex(ActivityTypeEnum.GatewayNode, name, code);
            vb._vertex.Activity.GatewaySplitJoinType = GatewaySplitJoinTypeEnum.Split;
            if (direction != null) vb._vertex.Activity.GatewayDirectionType = direction.Value;

            return vb;
        }

        /// <summary>
        /// 创建合并节点
        /// </summary>
        /// <param name="direction">合并类型</param>
        /// <param name="name">节点名称</param>
        /// <param name="code">节点代码</param>
        /// <returns>节点构造器</returns>
        public static VertexBuilder CreateJoin(Nullable<GatewayDirectionEnum> direction,
            string name = null, 
            string code = null)
        {
            if (string.IsNullOrEmpty(name)) name = "Join";
            var vb = CreateVertex(ActivityTypeEnum.GatewayNode, name, code);
            vb._vertex.Activity.GatewaySplitJoinType = GatewaySplitJoinTypeEnum.Join;
            if (direction != null) vb._vertex.Activity.GatewayDirectionType = direction.Value;

            return vb;
        }

        /// <summary>
        /// 创建多实例节点
        /// </summary>
        /// <param name="name">节点名称</param>
        /// <param name="code">节点代码</param>
        /// <returns>节点构造器</returns>
        public static VertexBuilder CreateMultipleInstance(string name = null, string code = null)
        {
            var vb = CreateVertex(ActivityTypeEnum.MultipleInstanceNode, name, code);
            return vb;
        }

        /// <summary>
        /// 创建子流程节点
        /// </summary>
        /// <param name="subProcessCode">子流程代码</param>
        /// <param name="name">节点名称</param>
        /// <param name="code">节点代码</param>
        /// <returns>节点构造器</returns>
        public static VertexBuilder CreateSubProcess(string subProcessCode, string name = null, string code = null)
        {
            if (string.IsNullOrEmpty(name)) name = "subprocess";
            if (string.IsNullOrEmpty(code)) code = "subprocess";

            var vb = CreateVertex(ActivityTypeEnum.SubProcessNode, name, code);
            return vb;
        }

        /// <summary>
        /// 创建节点
        /// </summary>
        /// <param name="ativityType">节点类型</param>
        /// <param name="name">节点名称</param>
        /// <param name="code">节点代码</param>
        /// <returns>节点构造器</returns>
        private static VertexBuilder CreateVertex(ActivityTypeEnum ativityType, string name, string code = null)
        {
            var nb = new VertexBuilder();
            var vertex = new Vertex(ativityType, name, code);
            vertex.Activity.ActivityGUID = Guid.NewGuid().ToString();

            if (ativityType == ActivityTypeEnum.StartNode)
            {
                //开始节点实在整个流程图的起始位置
                vertex.Left = 50;
                vertex.Top = 160;
                vertex.Level = 1;
            }
            nb._vertex = vertex;

            return nb;
        }
        #endregion

        #region 创建附属配置
        /// <summary>
        /// 创建操作
        /// </summary>
        /// <param name="actionType">操作类型</param>
        /// <param name="fireType">触发位置类型</param>
        /// <param name="expression">表达式</param>
        /// <returns>操作实体</returns>
        public static ActionEntity CreateAction(ActionTypeEnum actionType,
            FireTypeEnum fireType,
            string expression)
        {
            var action = new ActionEntity();
            action.ActionType = actionType;
            action.FireType = fireType;
            action.Expression = expression;

            return action;
        }

        //public static Performer CreatePerformer(string name, string code, string outerId)
        //{
        //    var participant
        //}

        /// <summary>
        /// 创建边界实体
        /// </summary>
        /// <param name="trigger">触发类型</param>
        /// <param name="expression">表达式</param>
        /// <returns>边界实体</returns>
        public static BoundaryEntity CreateBoundary(EventTriggerEnum trigger, string expression)
        {
            var boundary = new BoundaryEntity();
            boundary.EventTriggerType = trigger;
            boundary.Expression = expression;

            return boundary;
        }
        #endregion
    }
}
