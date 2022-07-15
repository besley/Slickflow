using Slickflow.Engine.Xpdl.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Slickflow.Engine.Common;
using Slickflow.Engine.Utility;
using Slickflow.Engine.Xpdl.Common;

namespace Slickflow.Engine.Xpdl.Convertor
{
    /// <summary>
    /// 网关转换器
    /// </summary>
    internal class DefaultlConvertor : ConvertorBase, IConvert
    {
        public DefaultlConvertor(XmlNode node, XmlNamespaceManager xnpmgr) : base(node, xnpmgr)
        {
        }

        public override Activity ConvertElementDetail(Activity entity)
        {
            return entity;
        }
    }
}
