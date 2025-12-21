using Slickflow.Engine.Core.Pattern.Event.Signal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Xpdl
{
    /// <summary>
    /// Constant definitions used in the XML file of process definition
    /// 流程定义的XML文件中，用到的常量定义
    /// </summary>
    public class XPDLDefinition
    {
        public static readonly string BPMN_NameSpacePrefix = "bpmn";
        public static readonly string BPMN_NameSpacePrefix_Value = "http://www.omg.org/spec/BPMN/20100524/MODEL";

        public static readonly string Sf_NameSpacePrefix = "sf";
        public static readonly string Sf_NameSpacePrefix_Value = "http://www.slickflow.com/schema/sf";

        public static readonly string BPMNDI_NameSpacePrefix = "bpmndi";
        public static readonly string BPMNDI_NameSpacePrefix_Value = "http://www.omg.org/spec/BPMN/20100524/DI";

        public static readonly string Sf_StrXmlPath_ExtensionElements = "bpmn:extensionElements";
        public static readonly string Sf_StrXmlPath_Forms = "bpmn:extensionElements/sf:forms";
        public static readonly string Sf_StrXmlPath_Actions = "bpmn:extensionElements/sf:actions";
        public static readonly string Sf_StrXmlPath_Variables = "bpmn:extensionElements/sf:variables";
        public static readonly string Sf_StrXmlPath_Boundaries = "bpmn:extensionElements/sf:boundaries";
        public static readonly string Sf_StrXmlPath_GroupBehaviours = "bpmn:extensionElements/sf:groupBehaviours";
        public static readonly string Sf_StrXmlPath_GatewayDetail = "bpmn:extensionElements/sf:gatewayDetail";
        public static readonly string Sf_StrXmlPath_MultiSignDetail = "bpmn:extensionElements/sf:multiSignDetail";
        public static readonly string Sf_StrXmlPath_Performers = "bpmn:extensionElements/sf:performers";
        public static readonly string Sf_StrXmlPath_SubInfoes = "bpmn:extensionElements/sf:subInfoes";
        public static readonly string Sf_StrXmlPath_Sections = "bpmn:extensionElements/sf:sections";
        public static readonly string Sf_StrXmlPath_Services = "bpmn:extensionElements/sf:services";
        public static readonly string Sf_StrXmlPath_AIServices = "bpmn:extensionElements/sf:aIServices";
        public static readonly string Sf_StrXmlPath_Scripts = "bpmn:extensionElements/sf:scripts";
        public static readonly string Sf_StrXmlPath_Notifications = "bpmn:extensionElements/sf:notifications";
        public static readonly string Sf_StrXmlPath_SequenceFlow = "bpmn:sequenceFlow";
        public static readonly string Sf_ElementName_Forms= "sf:forms";
        public static readonly string Sf_ElementName_Services = "sf:services";
        public static readonly string Sf_ElementName_AIServices = "sf:aIServices";

        public static readonly string BPMN_ElementName_Definitions = "bpmn:definitions";
        public static readonly string BPMN_ElementName_Collaboration = "bpmn:collaboration";
        public static readonly string BPMN_ElementName_Participant = "bpmn:participant";
        public static readonly string BPMN_ElementName_Process = "bpmn:process";
        public static readonly string BPMN_ElementName_ExtensionElements = "bpmn:extensionElements";
        public static readonly string BPMN_ElementName_SubProcess = "bpmn:subProcess";
        public static readonly string BPMN_ElementName_SequenceFlow = "bpmn:sequenceFlow";
        public static readonly string BPMN_ElementName_StartEvent = "bpmn:startEvent";
        public static readonly string BPMN_ElementName_EndEvent = "bpmn:endEvent";
        public static readonly string BPMN_ElementName_IntermediateEvent_Catch = "bpmn:intermediateCatchEvent";
        public static readonly string BPMN_ElementName_IntermediateEvent_Throw = "bpmn:intermediateThrowEvent";
        public static readonly string BPMN_ElementName_EventDefinition_Timer = "bpmn:timerEventDefinition";
        public static readonly string BPMN_ElementName_EventDefinition_Conditon = "bpmn:conditionalEventDefinition";
        public static readonly string BPMN_ElementName_EventDefinition_Message = "bpmn:messageEventDefinition";
        public static readonly string BPMN_ElementName_EventDefinition_Signal = "bpmn:signalEventDefinition";
        public static readonly string BPMN_ElementName_MessageFlow = "bpmn:messageFlow";
        public static readonly string BPMN_ElementName_Message = "bpmn:message";

        public static readonly string BPMN_ElementName_Task = "bpmn:task";
        public static readonly string BPMNElementNameUserTask = "bpmn:userTask";
        public static readonly string BPMN_ElementName_ManualTask = "bpmn:manualTask";
        public static readonly string BPMN_ElementName_ServiceTask = "bpmn:serviceTask";
        public static readonly string BPMN_ElementName_ScriptTask = "bpmn:scriptTask";
        public static readonly string BPMN_ElementName_ExclusiveGateway = "bpmn:exclusiveGateway";
        public static readonly string BPMN_ElementName_InclusiveGateway = "bpmn:inclusiveGateway";
        public static readonly string BPMN_ElementName_ParallelGateway = "bpmn:parallelGateway";
        public static readonly string BPMN_ElementName_ConditionExpression = "bpmn:conditionExpression";
        public static readonly string BPMN_ElementName_GroupBehaviours = "sf:groupBehaviours";

        public static readonly string BPMN_StrXmlPath_Collaboration = "bpmn:collaboration";
        public static readonly string BPMN_StrXmlPath_Process = "bpmn:process";
        public static readonly string BPMN_StrXmlPath_Process_Sub = "bpmn:process/bpmn:subProcess";
        public static readonly string BPMN_StrXmlPath_SequenceFlow = "bpmn:process/bpmn:sequenceFlow";
        public static readonly string BPMN_StrXmlPath_StartEvent = "bpmn:process/bpmn:startEvent";
        public static readonly string BPMN_StrXmlPath_EndEvent = "bpmn:process/bpmn:endEvent";
        public static readonly string BPMN_StrXmlPath_Incoming = "bpmn:incoming";
        public static readonly string BPMN_StrXmlPath_Outgoing = "bpmn:outgoing";
        public static readonly string BPMN_StrXmlPath_LaneSet = "bpmn:laneSet";
        public static readonly string BPMN_StrXmlPath_Message = "bpmn:message";
        public static readonly string BPMN_StrXmlPath_MessageFlow = "bpmn:messageFlow";
        public static readonly string BPMN_StrXmlPath_Signal = "bpmn:signal";
        public static readonly string BPMN_StrXmlPath_SequenceFlow_Condition = "bpmn:sequenceFlow/bpmn:conditionExpression";
        public static readonly string BPMN_StrXmlPath_SequenceFlow_GroupBehaviours = "bpmn:extensionElements/sf:groupBehaviours";

        public static readonly string BPMN_ElementName_Diagram = "bpmndi:BPMNDiagram";
        public static readonly string BPMN_ElementName_Plane = "bpmndi:BPMNPlane";
        public static readonly string BPMN_ElementName_Shape = "bpmndi:BPMNShape";
        public static readonly string BPMN_StrXmlPath_DiagramPlane = "bpmndi:BPMNDiagram/bpmndi:BPMNPlane";
    }
}
