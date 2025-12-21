using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Common
{
     /// <summary>
     /// 子流程对象
     /// Sub Process Info
     /// </summary>
    public class SubInfo
    {
        /// <summary>
        /// Sub Process Id
        /// </summary>
        public string SubProcessDefId
        {
            get;
            set;
        }

        /// <summary>
        /// Sub Process GUID
        /// </summary>
        public string SubProcessId
        {
            get;
            set;
        }

        /// <summary>
        /// Sub Process Name
        /// </summary>
        public string SubProcessName
        {
            get;
            set;
        }

        /// <summary>
        /// Sub Process Type
        /// </summary>
        public string SubType
        {
            get;
            set;
        }

        /// <summary>
        /// Sub Process Variable
        /// </summary>
        public string SubVar
        {
            get;
            set;
        }
    }
}
