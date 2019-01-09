/*
* Slickflow 开源项目遵循LGPL协议，也可联系作者获取企业版商业授权和技术支持；
* 除此之外的使用则视为不正当使用，请您务必避免由此带来的一切商业版权纠纷和损失。
* 
The Slickflow project.
Copyright (C) 2014  .NET Workflow Engine Library

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, you can access the official
web page about lgpl: https://www.gnu.org/licenses/lgpl.html
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
