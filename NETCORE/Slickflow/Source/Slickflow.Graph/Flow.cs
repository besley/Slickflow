using System.Collections.Generic;
using Slickflow.Engine.Common;

namespace Slickflow.Graph
{
    /// <summary>
    /// 流程实体
    /// </summary>
    public class Flow
    {
        #region 属性及构造方法
        public string Name { get; set; }
        public string Code { get; set; }
        public IList<Participant> ParticipantList { get; set; }
        private IList<Vertex> _vertices = new List<Vertex>();
        public IList<Vertex> Vertices {
            get
            {
                return _vertices;
            }
        }

        private IList<Link> _links = new List<Link>();
        public IList<Link> Links {
            get
            {
                return _links;
            }
        }

        public Flow(string name, string code)
        {
            Name = name;
            Code = code;
            ParticipantList = new List<Participant>();
        }
        #endregion
    }
}
