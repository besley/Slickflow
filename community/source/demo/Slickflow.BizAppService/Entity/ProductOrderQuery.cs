using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.BizAppService.Entity
{
    public class ProductOrderQuery : QueryBase
    {
        public string OrderCode { get; set; }
        public string ProductName { get; set; }
        public int Status { get; set; }
    }
}
