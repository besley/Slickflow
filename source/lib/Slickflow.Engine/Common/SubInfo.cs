using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Common
{
     /// <summary>
     /// 子流程对象
     /// </summary>
    public class SubInfo
    {
        public string SubID
        {
            get;
            set;
        }

        public string SubProcessGUID
        {
            get;
            set;
        }

        public string SubProcessName
        {
            get;
            set;
        }

        public string SubType
        {
            get;
            set;
        }

        public string SubVar
        {
            get;
            set;
        }
    }
}
