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
    /// Sub Process Convertor
    /// </summary>
    internal class SubProcessConvertor: ConvertorBase, IConvert
    {
        public SubProcessConvertor(XmlNode node, XmlNamespaceManager xnpmgr) : base(node, xnpmgr)
        {
        }

        public override Activity ConvertElementDetail(Activity entity)
        {
            var subInfoList = ConvertSubInfoes();
            if (subInfoList.Count > 0)
            {
                //外部子流程节点属性信息
                //External subprocess node attribute information
                var subProcessNode = new SubProcessNode(entity);
                var subInfoNode = subInfoList[0];
                subProcessNode.SubProcessDefId = int.Parse(subInfoNode.SubProcessDefId);
                subProcessNode.SubProcessId = subInfoNode.SubProcessId;
                entity.Node = subProcessNode;
            }
            else
            {
                //内部子流程包括子节点信息
                //Internal sub processes include sub node information
                var subProcess = ProcessModelConvertor.ConvertSubProcess(this.XMLNode);
                subProcess.XmlContent = this.XMLNode.OuterXml;
                if (subProcess != null)
                {
                    var subProcessNode = new SubProcessNode(entity);
                    subProcessNode.SubProcessId = subProcess.ProcessId;
                    subProcessNode.SubProcessNested = subProcess;
                    
                    entity.Node = subProcessNode;
                }
                else
                {
                    throw new WfXpdlException("SubProcessCovnert:none sub process informtion");
                }
            }
            return entity;
        }

        /// <summary>
        /// Convert sub process object list
        /// 转换子流程对象列表
        /// </summary>
        /// <returns></returns>
        public List<SubInfo> ConvertSubInfoes()
        {
            List<SubInfo> subInfoList = new List<SubInfo>();
            var subInfoesNode = GetSubInfoesNode();
            if (subInfoesNode != null)
            {
                //指定外部子流程id信息
                //Specify external subprocess Id information
                XmlNodeList xmlSubInfoList = subInfoesNode.ChildNodes;
                foreach (XmlNode element in xmlSubInfoList)
                {
                    subInfoList.Add(ConvertXmlSubInfoNodeToSubInfoEntity(element));
                }
            }
            return subInfoList;
        }

        /// <summary>
        /// Retrieve the XML node of SubInfo
        /// 获取SubInfo的XML节点
        /// </summary>
        /// <returns></returns>
        protected XmlNode GetSubInfoesNode()
        {
            var subInfoesNode = XMLNode.SelectSingleNode(XPDLDefinition.Sf_StrXmlPath_SubInfoes, XMLNamespaceManager);
            return subInfoesNode;
        }

        /// <summary>
        /// Convert SubInfo node
        /// 转换SubInfo节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private SubInfo ConvertXmlSubInfoNodeToSubInfoEntity(XmlNode node)
        {
            SubInfo subInfo = new SubInfo();
            subInfo.SubProcessDefId = XMLHelper.GetXmlAttribute(node, "subId");
            subInfo.SubProcessId = XMLHelper.GetXmlAttribute(node, "subProcessId");
            subInfo.SubProcessName = XMLHelper.GetXmlAttribute(node, "subProcessName");
            subInfo.SubType = XMLHelper.GetXmlAttribute(node, "subType");
            subInfo.SubVar = XMLHelper.GetXmlAttribute(node, "subVar");

            return subInfo;
        }
    }
}
