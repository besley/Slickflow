using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Parser
{
    /// <summary>
    /// 节点类
    /// </summary>
    public class snode
    {
        public string name { get; set; }
        public string id { get; set; }
        public string Description { get; set; }
        public string type { get; set; }
        public string code { get; set; }
        public List<performer> performers { get; set; }
        public List<connector> inputConnectors { get; set; }
        public List<connector> outputConnectors { get; set; }
        public int nodeId { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int height { get; set; }
        public int width { get; set; }
    }
}
