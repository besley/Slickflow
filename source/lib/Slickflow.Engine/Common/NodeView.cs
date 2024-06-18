/*
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
using Slickflow.Module.Resource;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// 工作流流转节点的视图对象
    /// </summary>
    public class NodeView
    {
        /// <summary>
        /// 活动节点GUID
        /// </summary>
        public String ActivityGUID { get; set; }

        /// <summary>
        /// 活动节点名称
        /// </summary>
        public String ActivityName { get; set; }

        /// <summary>
        /// 活动类型
        /// </summary>
        public ActivityTypeEnum ActivityType { get; set; }

        /// <summary>
        /// 活动节点编码
        /// </summary>
        public String ActivityCode { get; set; }

        /// <summary>
        /// 页面地址
        /// </summary>
        public String ActivityUrl { get; set; }

        /// <summary>
        /// 活动关联的自定义属性
        /// JSON数据格式
        /// </summary>
        public string MyProperties { get; set; }

        public IList<Role> Roles { get; set; }
        public IList<User> Users { get; set; }
        public IList<Participant> Participants { get; set; }
        public Boolean IsSkipTo { get; set; }
        public ReceiverTypeEnum ReceiverType { get; set; }
    }
}
