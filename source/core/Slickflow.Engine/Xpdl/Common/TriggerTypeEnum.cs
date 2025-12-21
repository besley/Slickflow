using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Xpdl.Common
{
    /// <summary>
    /// Trigger Type
    /// </summary>
    public enum TriggerTypeEnum
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,

        /// <summary>
        /// EMail
        /// </summary>
        EMail = 1,

        /// <summary>
        /// Timer
        /// </summary>
        Timer = 2,

        /// <summary>
        /// Message
        /// </summary>
        Message = 3,

        /// <summary>
        /// Singal
        /// </summary>
        Signal = 4,

        /// <summary>
        /// Conditional
        /// </summary>
        Conditional = 5
    }
}
