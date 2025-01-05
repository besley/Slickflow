using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Slickflow.Engine.Common;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Xpdl.Common;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Engine.Xpdl.Node;
using Slickflow.Engine.Utility;
using System.Xml.Linq;

namespace Slickflow.Engine.Xpdl
{
    /// <summary>
    /// Convert Helper
    /// </summary>
    public class ConvertHelper
    {
        #region Activity Convert
        /// <summary>
        /// Convert XmlNode to Activity Entity
        /// </summary>
        /// <param name="xmlNode">Xml Node</param>
        /// <param name="xnpmgr">XML namespace manager</param>
        /// <param name="processGUID">ProcessGUID</param>
        /// <returns>Activity Entity</returns>
        public static Activity ConvertXmlActivityNodeToActivityEntity(XmlNode xmlNode, 
            XmlNamespaceManager xnpmgr, 
            string processGUID)
        {
            if (xmlNode == null)
            {
                throw new WfXpdlException("The xml node can't be null, please check the xml file");
            }

            ActivityTypeEnum activityType = ActivityTypeEnum.Unknown;
            var convert = ConvertorFactory.CreateConvertor(xmlNode, xnpmgr, out activityType);
            Activity entity = convert.Convert();
            entity.ProcessGUID = processGUID;
            entity.ActivityType = activityType;

            return entity;
        }
        #endregion

        #region Form Node Convert
        /// <summary>
        /// Convert nodt to Form
        /// </summary>
        /// <param name="xmlNode">Xml Node</param>
        /// <param name="xnpmgr">XML namespace manager</param>
        /// <param name="processGUID">ProcessGUID</param>
        /// <returns>Form</returns>
        /// <exception cref="WfXpdlException"></exception>
        public static FormOuter ComnvertXmlFormNodeToFormEntity(XmlNode xmlNode,
            XmlNamespaceManager xnpmgr,
            string processGUID)
        {
            if (xmlNode == null)
            {
                throw new WfXpdlException("The xml node can't be null, please check the xml file");
            }

            var form = new FormOuter();
            form.OuterID = XMLHelper.GetXmlAttribute(xmlNode, "outerId");
            form.OuterCode = XMLHelper.GetXmlAttribute(xmlNode, "outerCode");
            form.Name = XMLHelper.GetXmlAttribute(xmlNode, "name");
            return form;

        }
        #endregion
    }
}
