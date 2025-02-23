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
    /// Gateway Convertor
    /// </summary>
    internal class GatewayConvertor : ConvertorBase, IConvert
    {
        public GatewayConvertor(XmlNode node, XmlNamespaceManager xnpmgr) : base(node, xnpmgr)
        {
        }

        public override Activity ConvertElementDetail(Activity entity)
        {
            var gatewayDetail = new GatewayDetail();
            var gatewayNode = base.XMLNode;

            //节点详细类型设置
            //Node detailed type setting
            var gatewatDetailNode = gatewayNode.SelectSingleNode(XPDLDefinition.Sf_StrXmlPath_GatewayDetail, base.XMLNamespaceManager);

            //区分Split和Join的类型
            //Distinguish between Split and Join types
            var incomingCount = gatewayNode.SelectNodes(XPDLDefinition.BPMN_StrXmlPath_Incoming, base.XMLNamespaceManager).Count;
            var outgoingCount = gatewayNode.SelectNodes(XPDLDefinition.BPMN_StrXmlPath_Outgoing, base.XMLNamespaceManager).Count;

            if (incomingCount == 1
                && outgoingCount > 1)
            {
                //Split 
                gatewayDetail.SplitJoinType = GatewaySplitJoinTypeEnum.Split;
                if (XPDLHelper.IsOrGateway(gatewayNode))
                {
                    gatewayDetail.DirectionType = GatewayDirectionEnum.OrSplit;
                    var strExtraSplitType = XMLHelper.GetXmlAttribute(gatewatDetailNode, "extraSplitType");
                    if (!string.IsNullOrEmpty(strExtraSplitType))
                    {
                        var extraSplitType = EnumHelper.TryParseEnum<GatewayDirectionEnum>(strExtraSplitType);
                        if (extraSplitType != GatewayDirectionEnum.None)
                        {
                            gatewayDetail.DirectionType = extraSplitType;
                        }
                    }
                }
                else if (XPDLHelper.IsXOrGateway(gatewayNode))
                {
                    gatewayDetail.DirectionType = GatewayDirectionEnum.XOrSplit;
                }
                else if (XPDLHelper.IsAndGateway(gatewayNode))
                {
                    gatewayDetail.DirectionType = GatewayDirectionEnum.AndSplit;
                }
                else
                {
                    throw new WfXpdlException(string.Format("GatewayConvertor:Unsupport gateway name:{0}", gatewayNode.Name));
                }
            }
            else if (outgoingCount == 1
                && incomingCount > 1)
            {
                // Join
                gatewayDetail.SplitJoinType = GatewaySplitJoinTypeEnum.Join;
                if (XPDLHelper.IsOrGateway(gatewayNode))
                {
                    gatewayDetail.DirectionType = GatewayDirectionEnum.OrJoin;
                    var strExtraJoinType = XMLHelper.GetXmlAttribute(gatewatDetailNode, "extraJoinType");
                    if (!string.IsNullOrEmpty(strExtraJoinType))
                    {
                        var extraJoinType = EnumHelper.TryParseEnum<GatewayDirectionEnum>(strExtraJoinType);
                        if (extraJoinType != GatewayDirectionEnum.None) 
                        {
                            gatewayDetail.DirectionType = extraJoinType;
                            if (extraJoinType == GatewayDirectionEnum.EOrJoin)
                            {
                                var strJoinPassType = XMLHelper.GetXmlAttribute(gatewatDetailNode, "joinPassType");
                                var joinPassType = EnumHelper.TryParseEnum<GatewayJoinPassEnum>(strJoinPassType);
                                gatewayDetail.JoinPassType = joinPassType;
                            }
                        }
                    }
                    else
                    {
                        gatewayDetail.DirectionType = GatewayDirectionEnum.OrJoin;
                    }
                }
                else if (XPDLHelper.IsXOrGateway(gatewayNode))
                {
                    gatewayDetail.DirectionType = GatewayDirectionEnum.XOrJoin;
                }
                else if (XPDLHelper.IsAndGateway(gatewayNode))
                {
                    gatewayDetail.DirectionType = GatewayDirectionEnum.AndJoin;
                }
                else
                {
                    throw new WfXpdlException(string.Format("GatewayConvertor:Unsupport gateway name:{0}", gatewayNode.Name));
                }
            }
            else if (outgoingCount == 1
                && incomingCount == 1)
            {
                //AndSplitMI and AndJoinMI
                var strExtraSplitType = XMLHelper.GetXmlAttribute(gatewatDetailNode, "extraSplitType");
                if (!string.IsNullOrEmpty(strExtraSplitType))
                {
                    var extraSplitType = EnumHelper.TryParseEnum<GatewayDirectionEnum>(strExtraSplitType);
                    gatewayDetail.DirectionType = extraSplitType;
                    gatewayDetail.SplitJoinType = GatewaySplitJoinTypeEnum.Split;
                }

                var strExtraJoinType = XMLHelper.GetXmlAttribute(gatewatDetailNode, "extraJoinType");
                if (!string.IsNullOrEmpty(strExtraJoinType))
                {
                    var extraJoinType = EnumHelper.TryParseEnum<GatewayDirectionEnum>(strExtraJoinType);
                    gatewayDetail.DirectionType = extraJoinType;
                    gatewayDetail.SplitJoinType = GatewaySplitJoinTypeEnum.Join;
                }
            }
            else
            {
                throw new WfXpdlException("GatewayCovnert:Unsupport incoming and outgoing count case");
            }
            entity.GatewayDetail = gatewayDetail;

            return entity;
        }
    }
}
