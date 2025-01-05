using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Slickflow.Engine.Common;
using Slickflow.Engine.Utility;
using Slickflow.Engine.Xpdl.Entity;

namespace Slickflow.Engine.Xpdl
{
    /// <summary>
    /// Dynamic Runable
    /// </summary>
    internal interface IDynamicRunable
    {
        /// <summary>
        /// Invoke method
        /// </summary>
        object InvokeMethod(TaskImplementDetail implementation, object[] userParameters);
    }
}
