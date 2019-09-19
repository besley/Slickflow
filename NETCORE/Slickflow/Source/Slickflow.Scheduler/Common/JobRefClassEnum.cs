using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Scheduler.Common
{
    /// <summary>
    /// Job Reference Class
    /// </summary>
    public enum JobRefClassEnum
    {
        /// <summary>
        /// WfProcess
        /// </summary>
        Process = 1,

        /// <summary>
        /// WfProcessInstance
        /// </summary>
        ProcessInstance = 2,

        /// <summary>
        /// Activity
        /// </summary>
        Activity = 3,

        /// <summary>
        /// WfActivityInsance
        /// </summary>
        ActivityInstance = 4,

        /// <summary>
        /// WfTasks
        /// </summary>
        Task = 5
    }
}
