using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slickflow.Engine.Common;
using Slickflow.Engine.Xpdl.Common;


namespace Slickflow.Engine.Xpdl.Entity
{
    /// <summary>
    /// 服务清单
    /// </summary>
    public class ScriptDetail
    {
        public ScriptMethodEnum Method { get; set; }
        public string Arguments { get; set; }
        public string ScriptText { get; set; }
    }
}
