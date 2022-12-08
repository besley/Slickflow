using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.BizAppService.Entity
{
    /// <summary>
    /// 生产订单实体
    /// </summary>
    [Table("ManProductOrder")]
    public class ProductOrderEntity
    {
        public int ID { get; set; }
        public string OrderCode { get; set; }
        public int Status { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedTime { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public string Remark { get; set; }
        public Nullable<DateTime> LastUpdatedTime { get; set; }
    }
}
