using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.BizAppService.Entity
{
    /// <summary>
    /// Product Order Entity
    /// 生产订单实体
    /// </summary>
    [Table("man_product_order")]
    public class ProductOrderEntity
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("order_code")]
        public string OrderCode { get; set; }
        [Column("status")]
        public int Status { get; set; }
        [Column("product_name")]
        public string ProductName { get; set; }
        [Column("quantity")]
        public int Quantity { get; set; }
        [Column("unit_price")]
        public decimal UnitPrice { get; set; }
        [Column("total_price")]
        public decimal TotalPrice { get; set; }
        [Column("created_time")]
        public DateTime CreatedTime { get; set; }
        [Column("customer_name")]
        public string CustomerName { get; set; }
        [Column("address")]
        public string Address { get; set; }
        [Column("mobile")]
        public string Mobile { get; set; }
        [Column("remark")]
        public string Remark { get; set; }
        [Column("updated_time")]
        public Nullable<DateTime> UpdatedTime { get; set; }
    }
}
