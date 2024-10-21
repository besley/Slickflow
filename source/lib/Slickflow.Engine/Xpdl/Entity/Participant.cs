using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Xpdl.Entity
{
    /// <summary>
    /// 参与者流程
    /// </summary>
    public class Participant
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string ParticipantGUID { get; set; }
        public string ProcessRef { get; set; }
        public Process Process { get; set; }
    }
}
