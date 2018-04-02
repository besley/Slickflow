using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slickflow.ModelDemo.Entity;

namespace Slickflow.ModelDemo.Interface
{
    public interface IAppFlowService
    {
        List<AppFlowEntity> GetPaged(AppFlowQuery query, out int count);
    }
}
