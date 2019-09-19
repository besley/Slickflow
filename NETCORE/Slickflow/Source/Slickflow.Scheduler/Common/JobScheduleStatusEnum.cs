using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Scheduler.Common
{
    /// <summary>
    /// job schedule status
    /// </summary>
    public enum JobScheduleStatusEnum
    {
        /// <summary>
        /// uninitialized value
        /// </summary>
        Uninitialized = -1,

        /// <summary>
        /// waitting for running
        /// </summary>
        Ready = 0,

        /// <summary>
        /// running
        /// </summary>
        Running = 1
    }
}
