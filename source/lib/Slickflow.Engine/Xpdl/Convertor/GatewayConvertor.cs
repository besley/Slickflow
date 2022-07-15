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
            var gatewatDetailNode = gatewayNode.SelectSingleNode(XPDLDefinition.Sf_StrXmlPath_GatewayDetail, base.XMLNamespaceManager);

            //区分Split和Join的类型
            var incomingCount = gatewayNode.SelectNodes(XPDLDefinition.BPMN2_StrXmlPath_Incoming, base.XMLNamespaceManager).Count;
            var outgoingCount = gatewayNode.SelectNodes(XPDLDefinition.BPMN2_StrXmlPath_Outgoing, base.XMLNamespaceManager).Count;


            if (incomingCount == 1
                && outgoingCount > 1)
            {
                //Split 类型
                gatewayDetail.SplitJoinType = GatewaySplitJoinTypeEnum.Split;
                if (XPDLHelper.IsOrGateway(gatewayNode))
                {
                    var strExtraSplitType = XMLHelper.GetXmlAttribute(gatewatDetailNode, "extraSplitType");
                    if (!string.IsNullOrEmpty(strExtraSplitType))
                    {
                        var extraSplitType = EnumHelper.TryParseEnum<GatewayDirectionEnum>(strExtraSplitType);
                        gatewayDetail.DirectionType = extraSplitType;
                    }
                    else
                    {
                        gatewayDetail.DirectionType = GatewayDirectionEnum.OrSplit;
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
                // Join类型
                gatewayDetail.SplitJoinType = GatewaySplitJoinTypeEnum.Join;
                if (XPDLHelper.IsOrGateway(gatewayNode))
                {
                    var strExtraJoinType = XMLHelper.GetXmlAttribute(gatewatDetailNode, "extraJoinType");
                    if (!string.IsNullOrEmpty(strExtraJoinType))
                    {
                        var extraJoinType = EnumHelper.TryParseEnum<GatewayDirectionEnum>(strExtraJoinType);
                        gatewayDetail.DirectionType = extraJoinType;
                        if (extraJoinType == GatewayDirectionEnum.EOrJoin)
                        {
                            var strJoinPassType = XMLHelper.GetXmlAttribute(gatewatDetailNode, "joinPassType");
                            var joinPassType = EnumHelper.TryParseEnum<GatewayJoinPassEnum>(strJoinPassType);
                            gatewayDetail.JoinPassType = joinPassType;
                        }
                        else
                        {
                            throw new WfXpdlException(string.Format("GatewayConvertor:Unsupport gateway type:{0}", extraJoinType.ToString()));
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
                //AndSplitMI 和 AndJoinMI的情形
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
