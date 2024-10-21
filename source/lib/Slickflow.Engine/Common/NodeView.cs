
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
        public IList<Partaker> Partakers { get; set; }
        public Boolean IsSkipTo { get; set; }
        public ReceiverTypeEnum ReceiverType { get; set; }
    }
}
