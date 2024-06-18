using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slickflow.Engine.Common;

namespace Slickflow.HrsService.Entity
{
    public class HrsLeaveRunner
    {
        public HrsLeaveEntity Entity { get; set; }
        public WfAppRunner Runner { get; set; }
    }
}
