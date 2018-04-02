using System.Text;
using System.Linq;
using System.Collections.Generic;
using System;

namespace DapperExtensions.Mapper
{
    /// <summary>
    /// Automatically maps an entity to a table using a combination of reflection and naming conventions for keys.
    /// </summary>
    public class AutoClassMapper<T> : ClassMapper<T> where T : class
    {
        public AutoClassMapper()
        {
            System.Attribute attr = System.Attribute.GetCustomAttributes(typeof(T))[0];
            Table((attr as dynamic).TableName);
            AutoMap();
        }
    }
}