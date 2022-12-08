using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slickflow.Engine.Common;

namespace Slickflow.Engine.Xpdl.Entity
{
    /// <summary>
    /// 边界实体对象
    /// </summary>
    public class Boundary
    {
        public EventTriggerEnum EventTriggerType { get; set; }
        public string Expression { get; set; }
    }
}
