using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// participant entity
    /// </summary>
    public class Participant
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string OuterCode { get; set; }
        public string OuterID { get; set; }
        public string OuterType { get; set; }
    }
}
