using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Engine.Common;

namespace Slickflow.Engine.Xpdl.Entity
{
    /// <summary>
    /// Task Implement Detail
    /// </summary>
    public class TaskImplementDetail
    {
        /// <summary>
        /// Task implementation type definition
        /// 任务实现类型定义
        /// </summary>
        public ImplementationTypeEnum ImplementationType
        {
            get;
            set;
        }

        public string Assembly
        {
            get;
            set;
        }

        public string Interface
        {
            get;
            set;
        }

        public string Method
        {
            get;
            set;
        }

        public string Content
        {
            get;
            set;
        }
    }
}
