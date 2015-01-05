using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        /// 活动节点编码
        /// </summary>
        public String ActivityCode { get; set; }
        public IList<Role> Roles { get; set; }
        public IList<Participant> Participants { get; set; }
        public Boolean IsSkipTo { get; set; }
    }
}
