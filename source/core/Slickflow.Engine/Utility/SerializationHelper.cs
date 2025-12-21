using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Utility
{
    /// <summary>
    /// Serialization Helper
    /// </summary>
    public static class SerializationHelper
    {
        /// <summary>
        /// Serialize method
        /// </summary>
        public static string Serialize(Type type, object obj)
        {
            using (MemoryStream mStream = new MemoryStream())
            {
                XmlSerializer serializer = new XmlSerializer(type);
                serializer.Serialize(mStream, obj);

                //Convert.ToBase64String(byte[]);
                return System.Text.Encoding.UTF8.GetString(mStream.ToArray());
            }
        }


        /// <summary>
        /// Deserialize
        /// </summary>
        public static object Deserialize(Type type, string strObject)
        {
            using (XmlReader reader = XmlReader.Create(new StringReader(strObject)))
            {
                XmlSerializer serializer = new XmlSerializer(type);

                return serializer.Deserialize(reader);
            }
        }

        /// <summary>
        /// Split Strings
        /// </summary>
        public static string SplitStrings(this IEnumerable<string> list, string seperator = ", ")
        {
            var result = list.Aggregate(
                new StringBuilder(),
                (sb, s) => (sb.Length == 0 ? sb : sb.Append(seperator)).Append(s),
                sb => sb.ToString());

            return result;
        }
    }
}
