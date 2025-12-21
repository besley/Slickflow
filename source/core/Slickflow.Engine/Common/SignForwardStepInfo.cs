using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Common
{
   /// <summary>
   /// Sign Forward Step Info
   /// 加签活动信息
   /// </summary>
    public class SignForwardStepInfo
    {
        /// <summary>
        /// Sign Forward Role User Tree
        /// 加签活动角色用户树
        /// </summary>
        public IList<NodeView> SignForwardRoleUserTree { get; set; }
    }
}
