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
