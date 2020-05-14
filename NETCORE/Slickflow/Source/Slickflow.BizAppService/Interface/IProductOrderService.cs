using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Slickflow.BizAppService.Entity;

namespace Slickflow.BizAppService.Interface
{
    /// <summary>
    /// 生产订单服务接口
    /// </summary>
    public partial interface IProductOrderService
    {
        List<ProductOrderEntity> GetPaged(ProductOrderQuery query, out int count);
        ProductOrderEntity SyncOrder(IDbConnection conn, IDbTransaction trans);
        WfAppResult Dispatch(ProductOrderEntity entity);
        WfAppResult Sample(ProductOrderEntity entity);
        WfAppResult Manufacture(ProductOrderEntity entity);
        WfAppResult QCCheck(ProductOrderEntity entity);
        WfAppResult Weight(ProductOrderEntity entity);
        WfAppResult Delivery(ProductOrderEntity entity);
    }
}
