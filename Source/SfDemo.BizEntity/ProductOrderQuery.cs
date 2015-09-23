using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SfDemo.BizEntity
{
    public class ProductOrderQuery : QueryBase
    {
        public string OrderCode { get; set; }
        public string ProductName { get; set; }
    }
}
