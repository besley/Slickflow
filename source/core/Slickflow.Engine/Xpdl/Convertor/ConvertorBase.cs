using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slickflow.Engine.Common;
using Slickflow.Engine.Utility;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Xpdl.Common;
using Slickflow.Engine.Xpdl.Entity;

namespace Slickflow.Engine.Xpdl.Convertor
{
    /// <summary>
    /// Convert Base
    /// 转换器基类
    /// </summary>
    public abstract class ConvertorBase : IConvert
    {
        #region Property and Constructor
        abstract public Activity ConvertElementDetail(Activity entity);
        private XmlNode xmlNode;
        public XmlNode XMLNode
        {
            get
            {
                return xmlNode;
            }
        }

        private XmlNamespaceManager xmlNamespaceManager;
        public XmlNamespaceManager XMLNamespaceManager
        {
            get
            {
                return xmlNamespaceManager;
            }
        }

        public ConvertorBase(XmlNode node, XmlNamespaceManager xnmgr)
        {
            xmlNode = node;
            xmlNamespaceManager = xnmgr;
        }
        #endregion

        /// <summary>
        /// Convert
        /// </summary>
        /// <returns></returns>
        public Activity Convert()
        {
            var activity = new Activity();
            var convertorBuilder = ConvertorBuilderFactory.Create(XMLNode, XMLNamespaceManager, activity);
            convertorBuilder.ConvertGeneral()
                .ConvertPartakers()
                .ConvertActions()
                .ConvertSections()
                .ConvertServices()
                .ConvertAIServices()
                .ConvertBoundires()
                .ConvertVariables()
                .ConvertNotifications();

            ConvertElementDetail(activity);
            return activity;
        }
    }
}
