using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slickflow.BizAppService.Entity;

namespace Slickflow.BizAppService.Interface
{
    public interface IAppFlowService
    {
        List<AppFlowEntity> GetPaged(AppFlowQuery query, out int count);
    }
}
