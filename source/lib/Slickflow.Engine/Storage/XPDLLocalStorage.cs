using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using System.Threading;
using Slickflow.Engine.Core;
using Slickflow.Engine.Common;
using Slickflow.Engine.Utility;
using Slickflow.Data;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Core.Result;
using Slickflow.Engine.Core.Event;

namespace Slickflow.Engine.Storage
{
    /// <summary>
    /// 流程定义文件存储
    /// 1） 默认本地文件，可以实现数据库读取，云端存储文件读取等方式
    /// 2） 此继承类是实现数据库存储方式
    /// </summary>
    public class XPDLLocalStorage : IXPDLStorage
    {
        /// <summary>
        /// 本地文件存储读取方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public XmlDocument Read(ProcessEntity entity)
        {
            XmlDocument xmlDoc = null;

            //本地路径存储的文件
            string serverPath = ConfigHelper.GetAppSettingString("WorkflowFileServer");
            string physicalFileName = string.Format("{0}\\{1}", serverPath, entity.XmlFilePath);

            //检查文件是否存在
            if (!File.Exists(physicalFileName))
            {
                throw new ApplicationException(
                    string.Format("Process Xml File Path:{0}", physicalFileName)
                );
            }

            xmlDoc = new XmlDocument();
            xmlDoc.Load(physicalFileName);

            return xmlDoc;
        }

        /// <summary>
        /// XML文件本地存储
        /// </summary>
        /// <param name="filePath">文件存储路径</param>
        /// <param name="xmlDoc">XML文档</param>
        public void Save(string filePath, XmlDocument xmlDoc)
        {
            var serverPath = ConfigHelper.GetAppSettingString("WorkflowFileServer");
            var physicalFileName = string.Format("{0}\\{1}", serverPath, filePath);
            var path = Path.GetDirectoryName(physicalFileName);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            xmlDoc.Save(physicalFileName);
        }
    }
}
