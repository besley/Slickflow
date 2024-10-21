using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Common
{
   /// <summary>
   /// 上一步活动信息
   /// </summary>
    public class SignForwardStepInfo
    {
        /// <summary>
        /// 下一步活动角色用户树
        /// </summary>
        public IList<NodeView> SignForwardRoleUserTree { get; set; }
    }
}
