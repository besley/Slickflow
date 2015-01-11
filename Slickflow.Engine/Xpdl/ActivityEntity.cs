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
using Slickflow.Engine.Common;
using Slickflow.Engine.Xpdl.Node;

namespace Slickflow.Engine.Xpdl
{
    /// <summary>
    /// 活动节点属性定义
    /// </summary>
    public class ActivityEntity
    {
        public string ActivityGUID { get; set; }
        public string ProcessGUID { get; set; }
        public ActivityTypeEnum ActivityType{ get; set; }
        public ActivityTypeDetail ActivityTypeDetail { get; set; }
        public NodeBase Node { get; set; }

        internal bool IsAutomanticWorkItem
        {
            get
            {
                if ((TaskImplementDetail != null)
                    && ((TaskImplementDetail.ImplementationType | ImplementationTypeEnum.Automantic)
                    == ImplementationTypeEnum.Automantic))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        internal bool IsSimpleWorkItem
        {
            get
            {
                if ((ActivityType | ActivityTypeEnum.SimpleWorkItem) == ActivityTypeEnum.SimpleWorkItem)
                    return true;
                else
                    return false;
            }
        }

        public GatewaySplitJoinTypeEnum GatewaySplitJoinType { get; set; }
        public GatewayDirectionEnum GatewayDirectionType { get; set; }
        public string ActivityName { get; set; }
        public string ActivityCode { get; set; }
        public string Description { get; set; }
        public TaskImplementDetail TaskImplementDetail { get; set; }

        public IList<Role> _roles;
        public IList<Role> Roles
        {
            get
            {
                if (_roles == null)
                {
                    var processModel = new ProcessModel(ProcessGUID);
                    _roles = processModel.GetActivityRoles(ActivityGUID);
                }
                return _roles;
            }
        }

        public IList<Participant> _participants;
        public IList<Participant> Participants
        {
            get
            {
                if (_participants == null)
                {
                    var processModel = new ProcessModel(ProcessGUID);
                    _participants = processModel.GetActivityParticipants(ActivityGUID);
                }
                return _participants;
            }
        }
    }
}
