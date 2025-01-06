using System;
using System.Collections.Generic;
using System.Text;
using Slickflow.HrsService.Entity;
using Slickflow.HrsService.Common;

namespace Slickflow.HrsService.Interface
{
    /// <summary>
    /// Leave Service Interface
    /// </summary>
    public interface IHrsLeaveService
    {
        List<HrsLeaveEntity> GetPaged(HrsLeaveQuery query, out int count);
        List<HrsLeaveEntity> QueryLeave(HrsLeaveQuery query);
        HrsLeaveEntity GetByID(int id);
        void Submit(HrsLeaveEntity entity);
        void Update(HrsLeaveEntity entity);
    }
}
