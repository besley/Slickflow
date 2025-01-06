using System;
using System.Collections.Generic;
using System.Text;

namespace Slickflow.HrsService.Entity
{
    /// <summary>
    /// Leave Query
    /// </summary>
    public class HrsLeaveQuery : QueryBase
    {
        public string CreatedUserID { get; set; }
    }
}
