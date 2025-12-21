using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slickflow.Engine.Xpdl.Common;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Engine.Utility;

namespace Slickflow.Engine.Xpdl
{
    /// <summary>
    /// Convert collaborative models into entities
    /// 协作模型转换成实体
    /// </summary>
    public static class CollaborationModelHelper
    {
        /// <summary>
        /// Convert to Collaboration from xml
        /// </summary>
        public static Collaboration ConvertCollaborationFromXml(XmlDocument xmlDoc, 
            XmlNode xmlNodeCollaboration)
        {
            var collaboration = ConvertCollaboration(xmlNodeCollaboration);
            var participantList = new List<Participant>();

            foreach (XmlNode child in xmlNodeCollaboration.ChildNodes)
            {
                if (child.Name == XPDLDefinition.BPMN_ElementName_Participant)
                {
                    var participant = ConvertParticipant(child);
                    participantList.Add(participant);
                }
            }
            collaboration.ParticipantList = participantList;
            return collaboration;
        }

        /// <summary>
        /// Convert to Collaboration
        /// </summary>
        private static Collaboration ConvertCollaboration(XmlNode xmlNodeCollaboration)
        {
            var collaboration = new Collaboration();
            collaboration.Id = XMLHelper.GetXmlAttribute(xmlNodeCollaboration, "id");
            collaboration.Name = XMLHelper.GetXmlAttribute(xmlNodeCollaboration, "name");
            collaboration.Code = XMLHelper.GetXmlAttribute(xmlNodeCollaboration, "code");
            collaboration.CollaborationId = XMLHelper.GetXmlAttribute(xmlNodeCollaboration, "id");

            return collaboration;
        }

        /// <summary>
        /// Convert Participant
        /// </summary>
        private static Participant ConvertParticipant(XmlNode xmlNodeParticipant)
        {
            var participant = new Participant();    
            participant.Id = XMLHelper.GetXmlAttribute(xmlNodeParticipant, "id");
            participant.Name = XMLHelper.GetXmlAttribute(xmlNodeParticipant, "name");
            participant.Code = XMLHelper.GetXmlAttribute(xmlNodeParticipant, "code");
            participant.ParticipantId = XMLHelper.GetXmlAttribute(xmlNodeParticipant, "id");
            participant.ProcessRef = XMLHelper.GetXmlAttribute(xmlNodeParticipant, "processRef");
            var xmlProcess = XMLHelper.GetXmlNodeByXpath(xmlNodeParticipant.OwnerDocument,
                string.Format("{0}[@id='" + participant.ProcessRef + "']", XPDLDefinition.BPMN_StrXmlPath_Process),
                XPDLHelper.GetSlickflowXmlNamespaceManager(xmlNodeParticipant.OwnerDocument));
            participant.Process = ConvertProcess(xmlProcess);

            return participant;
        }

        /// <summary>
        /// Convert Process
        /// </summary>
        private static Process ConvertProcess(XmlNode xmlProcess)
        {
            Process process = new Process();
            process.ProcessId = XMLHelper.GetXmlAttribute(xmlProcess, "id");

            return process;
        }     
    }
}
