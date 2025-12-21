using System;
using System.Collections.Generic;
using System.Linq;

namespace Slickflow.Data
{
    /// <summary>
    /// Pager
    /// 分页对象
    /// </summary>
    public class Pager
    {
        /// <summary>
        /// Page Index
        /// 页索引
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// Page Size
        /// 页大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Table Name
        /// 表名称
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// Key Field Name
        /// 主键字段
        /// </summary>
        public string KeyFieldName { get; set; }

        /// <summary>
        /// Order Type
        /// 是否降序
        /// </summary>
        public bool IsDesc { get; set; }

        /// <summary>
        /// Where Syntax
        /// 查询条件
        /// </summary>
        public string StrWhere { get; set; }

        /// <summary>
        /// Field by Order
        /// 排序字段
        /// </summary>
        public string FieldOrder { get; set; }

        /// <summary>
        /// Total Rows Count
        /// 总记录行数
        /// </summary>
        public int TotalRowsCount { get; set; }

        /// <summary>
        /// Total Pages Count
        /// 总页数
        /// </summary>
        public int TotalPagesCount { get; set; }
    }
}
