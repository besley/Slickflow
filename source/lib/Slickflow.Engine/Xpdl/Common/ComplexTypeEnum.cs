using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Xpdl.Common
{
    /// <summary>
    /// 节点的其它附属类型
    /// </summary>
    public enum ComplexTypeEnum
    {
        /// <summary>
        /// 多实例-会签节点
        /// </summary>
        SignTogether = 1,

        /// <summary>
        /// 多实例-加签节点
        /// </summary>
        SignForward = 2
    }
}
