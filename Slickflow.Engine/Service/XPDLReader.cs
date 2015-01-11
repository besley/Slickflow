using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slickflow.Engine.Business.Entity;

namespace Slickflow.Engine.Service
{
    /// <summary>
    /// 流程XML定义文件的Rader对象
    /// 扩展读取流程定义文件的方法，比如可以是数据库存储或远程文件方式等
    /// ProcessModel默认为本地路径方式，此处如果需要，扩展Read()方法
    /// </summary>
    public class XPDLReader : IXPDLReader
    {
        /// <summary>
        /// 获取是否读取的标志位
        /// 如需按其它方式读取，设置该方法返回 true，并实现Read()方法
        /// </summary>
        /// <returns></returns>
        public bool IsReadable()
        {
            //如需按其它方式读取，设置该方法返回 true，并实现Read()方法
            return false;
        }

        /// <summary>
        /// 读取方法，比如数据库或远程方式等
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public XmlDocument Read(ProcessEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
