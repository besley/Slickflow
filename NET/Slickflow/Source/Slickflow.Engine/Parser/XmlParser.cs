using System;
using System.IO;
using System.Xml;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Diagnostics;
using Slickflow.Engine.Utility;
using Slickflow.Engine.Common;
using Slickflow.Engine.Service;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Xpdl;

namespace Slickflow.Engine.Parser
{
    #region 暂时没用到的代码，后期使用
    ///// <summary>
    ///// Parser 文件夹下文件说明：
    ///// 服务器端XML处理方法，依赖设计器的实现方式。
    ///// CS/winform 等可以采用以下代码
    ///// BS/Web XML文件处理依赖客户端Javascript等实现
    ///// </summary>
    //public class XmlParser
    //{
    //    #region 流程XML维护对应的方法
    //    private readonly string XML_NODE_PANTICIPANTS = "Participants";
    //    private readonly string XML_NODE_WORKFLOWPROCESS = "WorkflowProcess";

    //    ///// <summary>
    //    ///// 流程定义Package 对象读取
    //    ///// </summary>
    //    ///// <param name="processGUID"></param>
    //    ///// <returns></returns>
    //    //public ProcessPackageEntity LoadProcess(string processGUID)
    //    //{
    //    //    var processModel = new ProcessModel(processGUID);
    //    //    var xmlDoc = processModel.GetProcessXmlDocument();
    //    //    var entity = Deserialize(processGUID, xmlDoc);

    //    //    return entity;
    //    //}

    //    ///// <summary>
    //    ///// 反序列化方法
    //    ///// </summary>
    //    ///// <param name="xmlDoc"></param>
    //    ///// <returns></returns>
    //    //private ProcessPackageEntity Deserialize(string processGUID, XmlDocument xmlDoc)
    //    //{
    //    //    //封装Package对象
    //    //    var package = new package();
    //    //    var root = xmlDoc.DocumentElement;

    //    //    foreach (XmlNode node in root.ChildNodes)
    //    //    {
    //    //        if (node.Name == XML_NODE_PANTICIPANTS)
    //    //        {
    //    //            package.participants = DeserializeParticipant(node);
    //    //        }
    //    //        else if (node.Name == XML_NODE_WORKFLOWPROCESS)
    //    //        {
    //    //            var workflowProcessNode = node.FirstChild;
    //    //            var processNode = workflowProcessNode.FirstChild;

    //    //            package.process = DeserializeProcess(processNode);
    //    //        }
    //    //    }

    //    //    //封装实体对象
    //    //    var entity = new ProcessPackageEntity();
    //    //    entity.ProcessGUID = processGUID;
    //    //    entity.package = package;

    //    //    return entity;
    //    //}

    //    /// <summary>
    //    /// 反序列化Participant对象
    //    /// </summary>
    //    /// <param name="node"></param>
    //    /// <returns></returns>
    //    private List<participant> DeserializeParticipant(XmlNode node)
    //    {
    //        List<participant> list = new List<participant>();
    //        foreach (XmlNode child in node.ChildNodes)
    //        {
    //            var type = child.Attributes["type"].ToString();
    //            var id = child.Attributes["id"].ToString();
    //            var name = child.Attributes["name"].ToString();
    //            var code = child.Attributes["code"].ToString();
    //            var outerIdString = child.Attributes["outerId"].ToString();
    //            int outerId = 0;
    //            int.TryParse(outerIdString, out outerId);

    //            list.Add(new participant
    //            {
    //                type = type,
    //                id = id,
    //                name = name,
    //                code = code,
    //                outerId = outerId
    //            });
    //        }

    //        return list;
    //    }

    //    /// <summary>
    //    /// 保存xml文件
    //    /// </summary>
    //    /// <param name="package"></param>
    //    public void SaveProcessFile(package package, int version)
    //    {
    //        var xmlDoc = Serialize(package);

    //        var processGUID = package.process.id;
    //        var pm = new ProcessManager();
    //        var entity = pm.GetByGUID(processGUID, version);

    //        if (entity != null)
    //        {
    //            var serverPath = ConfigHelper.GetAppSettingString("WorkflowFileServer");
    //            var physicalFileName = string.Format("{0}\\{1}", serverPath, entity.XmlFilePath);

    //            //判断目录是否存在，否则创建
    //            var pathName = Path.GetDirectoryName(physicalFileName);
    //            if (!Directory.Exists(pathName))
    //                Directory.CreateDirectory(pathName);

    //            xmlDoc.Save(physicalFileName);
    //        }
    //        else
    //        {
    //            throw new ApplicationException(string.Format("无法获取流程定义记录！ ProcessGUID: {0}", processGUID));
    //        }
    //    }

    //    /// <summary>
    //    /// 实体序列化为XML对象
    //    /// </summary>
    //    /// <param name="package"></param>
    //    /// <returns></returns>
    //    private XmlDocument Serialize(package package)
    //    {
    //        var xmlDoc = new XmlDocument();
    //        var xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
    //        var root = xmlDoc.CreateElement("Package");

    //        xmlDoc.InsertBefore(xmlDeclaration, xmlDoc.DocumentElement);
    //        xmlDoc.AppendChild(root);

    //        var participantsNode = xmlDoc.CreateElement("Participants");
    //        root.AppendChild(participantsNode);
            
    //        var workflowProcessNode = xmlDoc.CreateElement("WorkflowProcess");
    //        root.AppendChild(workflowProcessNode);

    //        var processNode = xmlDoc.CreateElement("Process");
    //        workflowProcessNode.AppendChild(processNode);

    //        //added participants list
    //        package.participants.ForEach((a) =>
    //        {
    //            var pNode = xmlDoc.CreateElement("Participant");
    //            pNode.SetAttribute("type", a.type);
    //            pNode.SetAttribute("id", a.id);
    //            pNode.SetAttribute("name", a.name);
    //            pNode.SetAttribute("code", a.code);
    //            pNode.SetAttribute("outerId", a.outerId.ToString());

    //            participantsNode.AppendChild(pNode);
    //        });

    //        //added process node
    //        var process = package.process;
    //        processNode.SetAttribute("id", process.id);
    //        processNode.SetAttribute("name", process.name);

    //        //added snodes
    //        var activitiesNode = xmlDoc.CreateElement("Activities");
    //        processNode.AppendChild(activitiesNode);

    //        process.snodes.ForEach((a) =>
    //        {
    //            var aNode = xmlDoc.CreateElement("Activity");
    //            aNode.SetAttribute("name", a.name);
    //            aNode.SetAttribute("id", a.id);
    //            if (!string.IsNullOrEmpty(a.code))
    //                aNode.SetAttribute("code", a.code);

    //            var activityTypeNode = xmlDoc.CreateElement("ActivityType");
    //            activityTypeNode.SetAttribute("type", a.type);
    //            aNode.AppendChild(activityTypeNode);

    //            //added performers node
    //            var performersNode = xmlDoc.CreateElement("Performers");
    //            a.performers.ForEach((p) =>
    //            {
    //                var pNode = xmlDoc.CreateElement("Performer");
    //                pNode.SetAttribute("id", p.id);

    //                performersNode.AppendChild(pNode);
    //            });
    //            aNode.AppendChild(performersNode);

    //            //added geography node
    //            var geographyNode = xmlDoc.CreateElement("Geography");
    //            aNode.AppendChild(geographyNode);

    //            //added widget node
    //            var widgetNode = xmlDoc.CreateElement("Widget");
    //            widgetNode.SetAttribute("nodeId", a.nodeId.ToString());
    //            widgetNode.SetAttribute("left", a.x.ToString());
    //            widgetNode.SetAttribute("top", a.y.ToString());
    //            widgetNode.SetAttribute("width", a.width.ToString());
    //            widgetNode.SetAttribute("height", a.height.ToString());

    //            //added connectors node
    //            var connectorsNode = xmlDoc.CreateElement("Connectors");
    //            a.inputConnectors.ForEach((c1) =>
    //            {
    //                var cNode = xmlDoc.CreateElement("Connector");
    //                cNode.SetAttribute("type", c1.type);
    //                cNode.SetAttribute("index", c1.index.ToString());
    //                cNode.SetAttribute("name", c1.name);

    //                connectorsNode.AppendChild(cNode);
    //            });

    //            a.outputConnectors.ForEach((c2) =>
    //            {
    //                var cNode = xmlDoc.CreateElement("Connector");
    //                cNode.SetAttribute("type", c2.type);
    //                cNode.SetAttribute("index", c2.index.ToString());
    //                cNode.SetAttribute("name", c2.name);

    //                connectorsNode.AppendChild(cNode);
    //            });
    //            widgetNode.AppendChild(connectorsNode);
    //            geographyNode.AppendChild(widgetNode);

    //            activitiesNode.AppendChild(aNode);                
    //        });

    //        //added slines
    //        var transitionsNode = xmlDoc.CreateElement("Transitions");
    //        processNode.AppendChild(transitionsNode);

    //        process.slines.ForEach((t) =>
    //        {
    //            var tNode = xmlDoc.CreateElement("Transition");
    //            tNode.SetAttribute("name", t.name);
    //            tNode.SetAttribute("id", t.id);
    //            tNode.SetAttribute("from", t.from);
    //            tNode.SetAttribute("to", t.to);

    //            var dNode = xmlDoc.CreateElement("Description");
    //            tNode.AppendChild(dNode);

    //            var dtext = xmlDoc.CreateTextNode(t.Description);
    //            tNode.AppendChild(dtext);

    //            var geographyNode = xmlDoc.CreateElement("Geography");
    //            tNode.AppendChild(geographyNode);

    //            var lineNode = xmlDoc.CreateElement("Line");
    //            geographyNode.AppendChild(lineNode);

    //            lineNode.SetAttribute("fromNode", t.source.nodeId.ToString());
    //            lineNode.SetAttribute("fromConnector", t.source.connectorIndex.ToString());
    //            lineNode.SetAttribute("toNode", t.dest.nodeId.ToString());
    //            lineNode.SetAttribute("toConnector", t.dest.connectorIndex.ToString());

    //            transitionsNode.AppendChild(tNode);
    //        });

    //        return xmlDoc;
    //    }
    //    #endregion
    //}

    #endregion
}