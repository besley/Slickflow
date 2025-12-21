using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Engine.Common;
using Slickflow.Engine.Xpdl.Node;

namespace Slickflow.Engine.Xpdl.Entity
{
    /// <summary>
    /// Activity
    /// </summary>
    public class Activity
    {
        /// <summary>
        /// ActivityId
        /// </summary>
        public string ActivityId { get; set; }
        /// <summary>
        /// Activity Name
        /// </summary>
        public string ActivityName { get; set; }

        /// <summary>
        /// Activity Code
        /// </summary>
        public string ActivityCode { get; set; }

        /// <summary>
        /// URL of the activity associated page
        /// 活动关联页面的URL
        /// </summary>
        public string ActivityUrl { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// ProcessId
        /// </summary>
        public string ProcessId { get; set; }

        /// <summary>
        /// Activity Type
        /// </summary>
        public ActivityTypeEnum ActivityType{ get; set; }

        /// <summary>
        /// WorkItem Type
        /// </summary>
        public WorkItemTypeEnum WorkItemType { get; set; }

        /// <summary>
        /// Trigger Detail
        /// </summary>
        public TriggerDetail TriggerDetail { get; set; }

        /// <summary>
        /// Gateway Detail
        /// </summary>
        public GatewayDetail GatewayDetail { get; set; }

        /// <summary>
        /// Mutiple Instance Detail
        /// </summary>
        public MultiSignDetail MultiSignDetail { get; set; }

        /// <summary>
        /// Skip Detail
        /// </summary>
        public SkipDetail SkipDetail { get; set; }

        /// <summary>
        /// Node
        /// </summary>
        public NodeBase Node { get; set; }

        /// <summary>
        /// Section List
        /// </summary>
        public List<SectionDetail> SectionList { get; set; }

        /// <summary>
        /// Custom attributes associated with activities
        /// JSON data format
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
        /// Action List
        /// </summary>
        public List<Action> ActionList { get; set; }

        /// <summary>
        /// Variable List
        /// </summary>
        public List<VariableDetail> VariableList { get; set; }

        /// <summary>
        /// Service List
        /// </summary>
        public List<ServiceDetail> ServiceList { get; set; }

        /// <summary>
        /// AI Service List
        /// </summary>
        public List<AIServiceDetail> AIServiceList { get; set; }

        /// <summary>
        /// Script List
        /// </summary>
        public List<ScriptDetail> ScriptList { get; set; }

        /// <summary>
        /// Boundary List
        /// </summary>
        public List<Boundary> BoundaryList { get; set; }

        /// <summary>
        /// Partaker List
        /// </summary>
        public List<Partaker> PartakerList { get; set; }

        /// <summary>
        /// Notification List
        /// </summary>
        public List<Partaker> NotificationList { get; set; }
    }
}
