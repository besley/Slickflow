using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Business.Entity
{
    /// <summary>
    /// 转移查询实体
    /// </summary>
    public class TransitionInstanceQuery
    {
        public string AppInstanceID { get; set; }
        public string ProcessGUID { get; set; }
    }
}
