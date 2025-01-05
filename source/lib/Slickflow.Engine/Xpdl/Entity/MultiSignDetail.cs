using Slickflow.Engine.Xpdl.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Xpdl.Entity
{
    /// <summary>
    /// Multiple Instance Sign detail
    /// 会签节点详细类型
    /// </summary>
    public class MultiSignDetail
    {
        /// <summary>
        /// Type of countersignature and additional signature
        /// 会签加签类型
        /// </summary>
        public ComplexTypeEnum ComplexType { get; set; }

        /// <summary>
        /// Serial parallel type (countersignature)
        /// 串行并行类型(会签加签)
        /// </summary>
        public MergeTypeEnum MergeType { get; set; }

        /// <summary>
        /// Type of pass rate setting for co signing and additional signing nodes
        /// 会签加签节点的通过率设置类型
        /// </summary>
        public CompareTypeEnum CompareType { get; set; }

        /// <summary>
        /// The pass rate of signing the master node record
        /// 会签主节点记录的通过率
        /// </summary>
        public float? CompleteOrder { get; set; }

        /// <summary>
        /// Sign Forward Type
        /// before/behind/parallel
        /// 加签类型
        /// </summary>
        public SignForwardTypeEnum SignForwardType { get; set; }
    }
}
