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
    /// 序列化帮助类
    /// </summary>
    public static class SerializationHelper
    {
        /// <summary>
        /// DataContractSerializer序列化一个对象
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="obj">实例对象</param>
        /// <returns>序列化后的文本</returns>
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
        /// 反序列化一个对象
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="strObject">实例对象</param>
        /// <returns>序列化后的对象</returns>
        public static object Deserialize(Type type, string strObject)
        {
            using (XmlReader reader = XmlReader.Create(new StringReader(strObject)))
            {
                XmlSerializer serializer = new XmlSerializer(type);

                return serializer.Deserialize(reader);
            }
        }

        /// <summary>
        /// 添加分隔符
        /// </summary>
        /// <param name="list">列表</param>
        /// <param name="seperator">分隔符</param>
        /// <returns>字符串</returns>
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
