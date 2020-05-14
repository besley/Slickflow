using System;
using System.Collections.Generic;
using System.Text;

namespace Slickflow.Engine.Xpdl.Entity
{
    /// <summary>
    /// Process Validate Entity
    /// </summary>
    public class ProcessValidateEntity
    {
        public IList<ActivityEntity> ActivityList { get; set; }
        public IList<TransitionEntity> TransitionList { get; set; }
        public string StartActivityGUID { get; set; }
    }
}
