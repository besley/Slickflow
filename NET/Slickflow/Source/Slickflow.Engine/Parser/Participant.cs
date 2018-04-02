using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Parser
{
    /// <summary>
    /// 参与者类
    /// </summary>
    public class participant
    {
        public string type { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public int outerId { get; set; }
    }

    
}
