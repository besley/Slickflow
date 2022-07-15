using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slickflow.Engine.Common;

namespace Slickflow.Engine.Xpdl.Entity
{
    /// <summary>
    /// 接收者
    /// </summary>
    public class Receiver
    {
        /// <summary>
        /// 接收者类型
        /// </summary>
        public ReceiverTypeEnum ReceiverType
        {
            get;
            set;
        }

        /// <summary>
        /// 数目限制
        /// </summary>
        public int Candidates
        {
            get;
            set;
        }
    }
}
