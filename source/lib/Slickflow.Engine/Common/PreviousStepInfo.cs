using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Common
{
   /// <summary>
   /// Previous Step Info
   /// 上一步活动信息
   /// </summary>
    public class PreviousStepInfo
    {
        /// <summary>
        /// Previous Activity Role User Tree
        /// 上一步活动角色用户树
        /// </summary>
        public IList<NodeView> PreviousActivityRoleUserTree { get; set; }

        /// <summary>
        /// Has ever passed a gateway node
        /// 是否经历过网关节点
        /// </summary>
        public Boolean HasGatewayPassed { get; set; }
    }
}
