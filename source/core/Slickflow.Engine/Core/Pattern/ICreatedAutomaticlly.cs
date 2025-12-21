using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slickflow.Data;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Common;
using Slickflow.Engine.Xpdl.Entity;

namespace Slickflow.Engine.Core.Pattern
{
    internal interface ICreatedAutomaticlly
    {
        ActivityInstanceEntity CreatedAutomaticlly(Activity toActivity, 
            ProcessInstanceEntity processInstance, 
            WfAppRunner runner,
            IDbSession session);
    }
}
