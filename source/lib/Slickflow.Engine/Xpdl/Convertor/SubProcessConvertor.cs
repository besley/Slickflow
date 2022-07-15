using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using Slickflow.Engine.Common;
using Slickflow.Engine.Utility;
using Slickflow.Engine.Xpdl.Common;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Engine.Xpdl.Node;

namespace Slickflow.Engine.Xpdl.Convertor
{
    /// <summary>
    /// 子流程转换器
    /// </summary>
    internal class SubProcessConvertor: ConvertorBase, IConvert
    {
        public SubProcessConvertor(XmlNode node, XmlNamespaceManager xnpmgr) : base(node, xnpmgr)
        {
        }

        public override Activity ConvertElementDetail(Activity entity)
        {
            var xmlSubProcessNode = base.XMLNode;
            //子流程节点
            var subProcessNode = new SubProcessNode(entity);
            subProcessNode.SubProcessGUID = XMLHelper.GetXmlAttribute(xmlSubProcessNode, "sf:guid");
            entity.Node = subProcessNode;

            return entity;
        }
    }
}
