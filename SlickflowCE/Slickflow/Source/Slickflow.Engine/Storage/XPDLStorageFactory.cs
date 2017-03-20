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
using Slickflow.Engine.Parser;

namespace Slickflow.Engine.Storage
{
    /// <summary>
    /// 外部存储的工厂方法
    /// </summary>
    internal class XPDLStorageFactory
    {
        /// <summary>
        /// 创建外部存储方法实例
        /// </summary>
        /// <returns></returns>
        internal static IXPDLStorage CreateXPDLStorage()
        {
            IXPDLStorage localStorage = null;
            //读取本地文件XML存储方式
            var storageType = ConfigHelper.GetAppSettingString("WorkflowFileStorageType");
            if (!string.IsNullOrEmpty(storageType) && storageType == "localfile")
            {
                localStorage = new XPDLLocalStorage();
            }
            return localStorage;
        }
    }
}
