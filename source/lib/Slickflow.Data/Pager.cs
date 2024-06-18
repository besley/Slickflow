using System;
using System.Collections.Generic;
using System.Linq;

namespace Slickflow.Data
{
    /// <summary>
    /// 分页对象
    /// </summary>
    public class Pager
    {
        /// <summary>
        /// 页索引
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 页大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 表名称
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 主键字段
        /// </summary>
        public string KeyFieldName { get; set; }

        /// <summary>
        /// 是否降序
        /// </summary>
        public bool IsDesc { get; set; }

        /// <summary>
        /// 查询条件
        /// </summary>
        public string StrWhere { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        public string FieldOrder { get; set; }

        /// <summary>
        /// 总记录行数
        /// </summary>
        public int TotalRowsCount { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPagesCount { get; set; }
    }
}
