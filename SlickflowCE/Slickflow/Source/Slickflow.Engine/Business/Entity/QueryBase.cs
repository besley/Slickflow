using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Business.Entity
{
    /// <summary>
    /// 查询基类
    /// </summary>
    public abstract class QueryBase
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalRowsCount { get; set; }
    }

    /// <summary>
    /// 查询泛型类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Query<T> : QueryBase
        where T:class
    {
        public T Entity { get; set; }
    }
}
