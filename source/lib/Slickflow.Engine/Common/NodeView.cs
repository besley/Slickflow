
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Module.Resource;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// View objects of workflow flow nodes
    /// 工作流流转节点的视图对象
    /// </summary>
    public class NodeView
    {
        /// <summary>
        /// ActivityGUID
        /// 活动节点GUID
        /// </summary>
        public String ActivityGUID { get; set; }

        /// <summary>
        /// Activity Name
        /// 活动节点名称
        /// </summary>
        public String ActivityName { get; set; }

        /// <summary>
        /// Activity Type
        /// 活动类型
        /// </summary>
        public ActivityTypeEnum ActivityType { get; set; }

        /// <summary>
        /// Activity Code
        /// 活动节点编码
        /// </summary>
        public String ActivityCode { get; set; }

        /// <summary>
        /// Activity Url
        /// 页面地址
        /// </summary>
        public String ActivityUrl { get; set; }

        /// <summary>
        /// Custom attributes associated with activities
        /// JSON data format
        /// 活动关联的自定义属性
        /// JSON数据格式
        /// </summary>
        public string MyProperties { get; set; }

        /// <summary>
        /// Role List
        /// 角色列表
        /// </summary>
        public IList<Role> Roles { get; set; }

        /// <summary>
        /// User List
        /// 用户列表
        /// </summary>
        public IList<User> Users { get; set; }

        /// <summary>
        /// Partaker List
        /// 参与者列表
        /// </summary>
        public IList<Partaker> Partakers { get; set; }

        /// <summary>
        /// Wether to skip to
        /// 是否跳过
        /// </summary>
        public Boolean IsSkipTo { get; set; }

        /// <summary>
        /// Receiver Type
        /// 接收者类型
        /// </summary>
        public ReceiverTypeEnum ReceiverType { get; set; }
    }
}
