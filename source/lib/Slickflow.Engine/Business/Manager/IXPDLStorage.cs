using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slickflow.Engine.Business.Entity;

namespace Slickflow.Engine.Business.Manager
{
    /// <summary>
    /// 流程定义数据库存储接口
    /// </summary>
    public interface IXPDLStorage
    {
        XmlDocument Read(ProcessEntity entity);
        void Save(string filePath, XmlDocument xmlDoc);
    }
}
