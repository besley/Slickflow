using System;
using System.Collections.Generic;
using System.Text;

namespace Slickflow.Engine.Xpdl.Entity
{
    /// <summary>
    /// Process Validate Entity
    /// </summary>
    public class ProcessValidate
    {
        public IList<Activity> ActivityList { get; set; }
        public IList<Transition> TransitionList { get; set; }
        public string StartActivityGUID { get; set; }
    }
}
