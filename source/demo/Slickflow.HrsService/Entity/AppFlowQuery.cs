using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.HrsService.Entity
{
    /// <summary>
    /// Application flow query
    /// </summary>
    public class AppFlowQuery : QueryBase
    {
        public string AppInstanceID { get; set; }
    }
}
