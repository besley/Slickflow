using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Scheduler.Common
{
    /// <summary>
    /// job schedule type
    /// </summary>
    public enum JobScheduleNameEnum
    {
        /// <summary>
        /// trigger timing startup process
        /// </summary>
        TriggerTimingStartupProcess = 1,

        /// <summary>
        /// process instance ovedue treatment
        /// </summary>
        TerminateOverdueProcessInstance = 2,

        /// <summary>
        /// send task email
        /// </summary>
        SendTaskEMail = 3,

        /// <summary>
        /// activity instance overdue treatment
        /// </summary>
        TerminateOverdueActivityInstance = 4
    }
}
