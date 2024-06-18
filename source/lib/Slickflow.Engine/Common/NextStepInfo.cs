using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// 下一步的步骤信息
    /// </summary>
    public class NextStepInfo
    {
        /// <summary>
        /// 步骤消息描述
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 下一步活动角色用户树
        /// </summary>
        public IList<NodeView> NextActivityRoleUserTree { get; set; }

        /// <summary>
        /// 下一步活动预选执行用户
        /// </summary>
        public IDictionary<string, PerformerList> NextActivityPerformers { get; set; }
    }
}
