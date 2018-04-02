using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Parser
{
    /// <summary>
    /// 连接线类
    /// </summary>
    public class sline
    {
        public string id { get; set; }
        public string name { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public string Description { get; set; }
        public source source { get; set; }
        public dest dest { get; set; }
    }
}
