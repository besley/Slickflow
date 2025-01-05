using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// Partaker entity
    /// 参与者实体
    /// </summary>
    public class Partaker
    {
        /// <summary>
        /// ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Partaker name
        /// 参与者名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Partaker code
        /// 参与者代码
        /// </summary>
        public string OuterCode { get; set; }

        /// <summary>
        /// Partaker id
        /// 参与者ID
        /// </summary>
        public string OuterID { get; set; }

        /// <summary>
        /// Partaker type
        /// 参与者类型
        /// </summary>
        public string OuterType { get; set; }
    }
}
