/*
* Slickflow 工作流引擎遵循LGPL协议，也可联系作者商业授权并获取技术支持；
* 除此之外的使用则视为不正当使用，请您务必避免由此带来的商业版权纠纷。
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

namespace Slickflow.Engine.Xpdl.Entity
{
    /// <summary>
    /// 活动节点属性定义
    /// </summary>
    public class Activity
    {
        /// <summary>
        /// 标识ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 活动GUID
        /// </summary>
        public string ActivityGUID { get; set; }
        /// <summary>
        /// 活动名称
        /// </summary>
        public string ActivityName { get; set; }

        /// <summary>
        /// 活动代码
        /// </summary>
        public string ActivityCode { get; set; }

        /// <summary>
        /// 活动关联页面的URL
        /// </summary>
        public string ActivityUrl { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 流程GUID
        /// </summary>
        public string ProcessGUID { get; set; }

        /// <summary>
        /// 活动类型
        /// </summary>
        public ActivityTypeEnum ActivityType{ get; set; }

        /// <summary>
        /// 子流程节点
        /// </summary>
        public Process SubProcess { get; set; }

        /// <summary>
        /// 工作项类型
        /// </summary>
        public WorkItemTypeEnum WorkItemType { get; set; }

        /// <summary>
        /// 活动类型Detail
        /// </summary>
        public ActivityTypeDetail ActivityTypeDetail { get; set; }

        /// <summary>
        /// 触发Detail
        /// </summary>
        public TriggerDetail TriggerDetail { get; set; }

        /// <summary>
        /// 网关类型Detail
        /// </summary>
        public GatewayDetail GatewayDetail { get; set; }

        /// <summary>
        /// 会签类型Detail
        /// </summary>
        public MultiSignDetail MultiSignDetail { get; set; }

        /// <summary>
        /// 子流程Detail
        /// </summary>
        public SubProcessDetail SubProcessDetail { get; set; }

        /// <summary>
        /// 跳转信息
        /// </summary>
        public SkipDetail SkipDetail { get; set; }

        /// <summary>
        /// 节点
        /// </summary>
        public NodeBase Node { get; set; }

        /// <summary>
        /// 自定义章节
        /// </summary>
        public List<SectionDetail> SectionList { get; set; }

        /// <summary>
        /// 活动关联的自定义属性
        /// JSON数据格式
        /// </summary>
        public string MyProperties
        {
            get
            {
                var myProperties = string.Empty;
                if (SectionList != null && SectionList.Count() > 0)
                {
                    var section = SectionList.First(s => s.Name == "myProperties");
                    myProperties = section.Value;
                }
                return myProperties;
            }
        }

        /// <summary>
        /// 操作列表
        /// </summary>
        public List<Action> ActionList { get; set; }

        /// <summary>
        /// 服务列表
        /// </summary>
        public List<ServiceDetail> ServiceList { get; set; }

        /// <summary>
        /// 脚本列表
        /// </summary>
        public List<ScriptDetail> ScriptList { get; set; }

        /// <summary>
        /// 边界列表
        /// </summary>
        public List<Boundary> BoundaryList { get; set; }

        /// <summary>
        /// 参与者列表
        /// </summary>
        public List<Participant> ParticipantList { get; set; }
    }
}
