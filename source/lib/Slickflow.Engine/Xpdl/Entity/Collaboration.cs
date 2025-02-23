using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Xpdl.Entity
{
    /// <summary>
    /// Collaboration
    /// </summary>
    public class Collaboration
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string CollaborationID { get; set; }
        public List<Participant> ParticipantList { get; set; }
    }
}
