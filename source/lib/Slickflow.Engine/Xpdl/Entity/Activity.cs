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
        /// 工作项类型
        /// </summary>
        public WorkItemTypeEnum WorkItemType { get; set; }

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
        public List<Partaker> PartakerList { get; set; }

        /// <summary>
        /// 通知用户列表
        /// </summary>
        public List<Partaker> NotificationList { get; set; }
    }
}
