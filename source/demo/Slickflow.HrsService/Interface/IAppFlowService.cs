using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slickflow.HrsService.Entity;

namespace Slickflow.HrsService.Interface
{
    /// <summary>
    /// Application flow service
    /// </summary>
    public interface IAppFlowService
    {
        List<AppFlowEntity> GetPaged(AppFlowQuery query, out int count);
    }
}
