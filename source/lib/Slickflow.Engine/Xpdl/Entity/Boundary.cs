using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slickflow.Engine.Common;

namespace Slickflow.Engine.Xpdl.Entity
{
    /// <summary>
    /// Boundary
    /// </summary>
    public class Boundary
    {
        public EventTriggerEnum EventTriggerType { get; set; }
        public string Expression { get; set; }
    }
}
