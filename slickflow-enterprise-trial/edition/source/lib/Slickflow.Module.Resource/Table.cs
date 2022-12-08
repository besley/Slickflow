using System;

namespace Slickflow.Module.Resource
{
    /// <summary>
    /// 映射数据库表对象
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
}
