using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Xpdl.Entity
{
    /// <summary>
    /// 流程对象
    /// </summary>
    public class Process
    {
        public string ID { get; set; }
        public string ProcessGUID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        private IList<Activity> activityList = new List<Activity>();
        public IList<Activity> ActivityList { 
            get
            {
                return activityList;
            }
        }
        private IList<Transition> transitionList = new List<Transition>();
        public IList<Transition> TransitionList { 
            get 
            {
                return transitionList;
            } 
        }
    }
}
