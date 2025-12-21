using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// Next Step Info
    /// 下一步的步骤信息
    /// </summary>
    public class NextStepInfo
    {
        /// <summary>
        /// Step info description
        /// 步骤消息描述
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Next Activity Role User Tree
        /// 下一步活动角色用户树
        /// </summary>
        public IList<NodeView> NextActivityRoleUserTree { get; set; }

        /// <summary>
        /// Next activity pre selection of executing performers
        /// 下一步活动预选执行用户
        /// </summary>
        public IDictionary<string, PerformerList> NextActivityPerformers { get; set; }
    }
}
