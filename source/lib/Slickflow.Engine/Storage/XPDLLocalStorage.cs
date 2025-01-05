using System;
using System.IO;
using System.Xml;
using Slickflow.Engine.Utility;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;

namespace Slickflow.Engine.Storage
{
    /// <summary>
    /// Process definition file storage
    /// 1) Default local files, which can be read from databases, cloud storage files, and other methods
    /// 2) This inheritance class implements database storage methods
    /// 流程定义文件存储
    /// 1） 默认本地文件，可以实现数据库读取，云端存储文件读取等方式
    /// 2） 此继承类是实现数据库存储方式
    /// </summary>
    public class XPDLLocalStorage : IXPDLStorage
    {
        /// <summary>
        /// Read local file
        /// 本地文件存储读取方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public XmlDocument Read(ProcessEntity entity)
        {
            XmlDocument xmlDoc = null;

            //本地路径存储的文件
            //Files stored in local path
            string serverPath = ConfigHelper.GetAppSettingString("WorkflowFileServer");
            string physicalFileName = string.Format("{0}\\{1}", serverPath, entity.XmlFilePath);

            //检查文件是否存在
            //Check if the file exists
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
        /// Save to local file
        /// XML文件本地存储
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="xmlDoc"></param>
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
