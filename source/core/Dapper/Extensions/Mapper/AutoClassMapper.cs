using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DapperExtensions.Mapper
{
    /// <summary>
    /// Automatically maps an entity to a table using a combination of reflection and naming conventions for keys.
    /// </summary>
    public class AutoClassMapper<T> : ClassMapper<T> where T : class
    {
        public AutoClassMapper()
        {
            var attr = typeof(T).CustomAttributes.FirstOrDefault(a => a.AttributeType.Name == "Table");
            var tblName = attr?.ConstructorArguments[0].Value?.ToString();

            Table(tblName);
            AutoMap();
        }
    }
}