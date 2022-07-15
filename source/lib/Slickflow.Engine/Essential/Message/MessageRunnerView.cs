using System;
using System.Collections.Generic;
using System.Text;
using Slickflow.Engine.Common;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Xpdl.Entity;

namespace Slickflow.Engine.Essential.Message
{
    /// <summary>
    /// 消息运行者对象视图
    /// </summary>
    public class MessageRunnerView
    {
        /// <summary>
        /// 目标流程实体
        /// </summary>
        public ProcessEntity ProcessEntity { get; set; }
        /// <summary>
        /// 目标节点实体
        /// </summary>
        public Activity ActivityEntity { get; set; }
        /// <summary>
        /// 当前运行者身份
        /// </summary>
        public WfAppRunner WfAppRunner { get; set; }
    }
}
