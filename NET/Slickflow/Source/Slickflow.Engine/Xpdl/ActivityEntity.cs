/*
* Slickflow 软件遵循自有项目开源协议，也可联系作者获取企业版商业授权和技术支持；
* 除此之外的使用则视为不正当使用，请您务必避免由此带来的一切商业版权纠纷和损失。
* 
The Slickflow Open License (SfPL 1.0)
Copyright (C) 2014  .NET Workflow Engine Library

1. Slickflow software must be legally used, and should not be used in violation of law, 
   morality and other acts that endanger social interests;
2. Non-transferable, non-transferable and indivisible authorization of this software;
3. The source code can be modified to apply Slickflow components in their own projects 
   or products, but Slickflow source code can not be separately encapsulated for sale or 
   distributed to third-party users;
4. The intellectual property rights of Slickflow software shall be protected by law, and
   no documents such as technical data shall be made public or sold.
5. The enterprise, ultimate and universe version can be provided with commercial license, 
   technical support and upgrade service.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Engine.Common;
using Slickflow.Engine.Xpdl.Node;

namespace Slickflow.Engine.Xpdl
{
    /// <summary>
    /// 活动节点属性定义
    /// </summary>
    public class ActivityEntity
    {
        /// <summary>
        /// 活动GUID
        /// </summary>
        public string ActivityGUID { get; set; }

        /// <summary>
        /// 流程GUID
        /// </summary>
        public string ProcessGUID { get; set; }

        /// <summary>
        /// 活动类型
        /// </summary>
        public ActivityTypeEnum ActivityType{ get; set; }

        /// <summary>
        /// 工作项类型
        /// </summary>
        public WorkItemTypeEnum WorkItemType { get; set; }

        /// <summary>
        /// 活动类型Detail
        /// </summary>
        public ActivityTypeDetail ActivityTypeDetail { get; set; }

        /// <summary>
        /// 节点
        /// </summary>
        public NodeBase Node { get; set; }

        /// <summary>
        /// 网关分支合并类型
        /// </summary>
        public GatewaySplitJoinTypeEnum GatewaySplitJoinType { get; set; }

        /// <summary>
        /// 网关方向类型
        /// </summary>
        public GatewayDirectionEnum GatewayDirectionType { get; set; }

        /// <summary>
        /// 活动名称
        /// </summary>
        public string ActivityName { get; set; }

        /// <summary>
        /// 活动代码
        /// </summary>
        public string ActivityCode { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 操作列表
        /// </summary>
        public List<ActionEntity> ActionList { get; set; }
    }
}
