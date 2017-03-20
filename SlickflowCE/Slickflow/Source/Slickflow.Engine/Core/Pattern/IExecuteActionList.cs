using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Common;

namespace Slickflow.Engine.Core.Pattern
{
    /// <summary>
    /// 调用外部程序或服务的接口
    /// </summary>
    internal interface IExecuteActionList
    {
        void ExecteActionList(IList<ActionEntity> actionList, IDictionary<string, ActionParameterInternal> actionMethodParameters);
    }
}
