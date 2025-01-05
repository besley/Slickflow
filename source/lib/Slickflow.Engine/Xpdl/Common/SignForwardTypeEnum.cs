using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Xpdl.Common
{
    /// <summary>
    /// Sign forward Type
    /// </summary>
    public enum SignForwardTypeEnum
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,

        /// <summary>
        /// Before
        /// </summary>
        SignForwardBefore = 1,

        /// <summary>
        /// Behind
        /// </summary>
        SignForwardBehind = 2,

        /// <summary>
        /// Parallel
        /// </summary>
        SignForwardParallel = 3
    }
}
