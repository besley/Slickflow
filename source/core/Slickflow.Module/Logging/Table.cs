using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Module.Logging
{
    /// <summary>
    /// Table Entity to Map Database Object
    /// </summary>
    [AttributeUsageAttribute(AttributeTargets.Class, Inherited = false, AllowMultiple = false), Serializable]
    public class Table : System.Attribute
    {
        public string TableName;
        public Table(string tblName)
        {
            TableName = tblName;
        }
    }

    /// <summary>
    /// Table
    /// 映射数据库表的列对象
    /// Mapping to DataColumn
    /// </summary>
    [AttributeUsageAttribute(AttributeTargets.Property, Inherited = false, AllowMultiple = false), Serializable]
    public class Column : System.Attribute
    {
        public string ColumnName;
        public Column(string colName)
        {
            ColumnName = colName;
        }
    }
}
