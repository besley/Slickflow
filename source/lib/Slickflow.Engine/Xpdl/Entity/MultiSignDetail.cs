using Slickflow.Engine.Xpdl.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Xpdl.Entity
{
    /// <summary>
    /// 会签节点详细类型
    /// </summary>
    public class MultiSignDetail
    {
        /// <summary>
        /// 会签加签类型
        /// </summary>
        public ComplexTypeEnum ComplexType { get; set; }

        /// <summary>
        /// 串行并行类型(会签加签)
        /// </summary>
        public MergeTypeEnum MergeType { get; set; }

        /// <summary>
        /// 会签加签节点的通过率设置类型
        /// </summary>
        public CompareTypeEnum CompareType { get; set; }

        /// <summary>
        /// 会签主节点记录的通过率
        /// </summary>
        public float? CompleteOrder { get; set; }

        /// <summary>
        /// 加签类型
        /// </summary>
        public SignForwardTypeEnum SignForwardType { get; set; }
    }
}
