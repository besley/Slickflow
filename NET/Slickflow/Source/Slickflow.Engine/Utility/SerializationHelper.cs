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
    public class SerializationHelper
    {
        /// <summary>
        /// DataContractSerializer序列化一个对象
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="obj">实例对象</param>
        /// <returns>序列化后的文本</returns>
        public static string Serialize(Type type, object obj, string rootName, string nameSpace)
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
        /// <returns>序列化后的对象</returns>
        public static object Deserialize(Type type, string strObject, string rootName, string nameSpace)
        {
            using (XmlReader reader = XmlReader.Create(new StringReader(strObject)))
            {
                XmlSerializer serializer = new XmlSerializer(type);

                return serializer.Deserialize(reader);
            }
        }
    }
}
