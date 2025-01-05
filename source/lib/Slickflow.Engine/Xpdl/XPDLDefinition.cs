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
        public static readonly string BPMN2_NameSpacePrefix = "bpmn2";
        public static readonly string BPMN2_NameSpacePrefix_Value = "http://www.omg.org/spec/BPMN/20100524/MODEL";

        public static readonly string Sf_NameSpacePrefix = "sf";
        public static readonly string Sf_NameSpacePrefix_Value = "http://www.slickflow.com/schema/sf";

        public static readonly string BPMNDI_NameSpacePrefix = "bpmndi";
        public static readonly string BPMNDI_NameSpacePrefix_Value = "http://www.omg.org/spec/BPMN/20100524/DI";

        public static readonly string Sf_StrXmlPath_ExtensionElements = "bpmn2:extensionElements";
        public static readonly string Sf_StrXmlPath_Forms = "bpmn2:extensionElements/sf:forms";
        public static readonly string Sf_StrXmlPath_Actions = "bpmn2:extensionElements/sf:actions";
        public static readonly string Sf_StrXmlPath_Boundaries = "bpmn2:extensionElements/sf:boundaries";
        public static readonly string Sf_StrXmlPath_GroupBehaviours = "bpmn2:extensionElements/sf:groupBehaviours";
        public static readonly string Sf_StrXmlPath_GatewayDetail = "bpmn2:extensionElements/sf:gatewayDetail";
        public static readonly string Sf_StrXmlPath_MultiSignDetail = "bpmn2:extensionElements/sf:multiSignDetail";
        public static readonly string Sf_StrXmlPath_Performers = "bpmn2:extensionElements/sf:performers";
        public static readonly string Sf_StrXmlPath_SubInfoes = "bpmn2:extensionElements/sf:subInfoes";
        public static readonly string Sf_StrXmlPath_Sections = "bpmn2:extensionElements/sf:sections";
        public static readonly string Sf_StrXmlPath_Services = "bpmn2:extensionElements/sf:services";
        public static readonly string Sf_StrXmlPath_Scripts = "bpmn2:extensionElements/sf:scripts";
        public static readonly string Sf_StrXmlPath_Notifications = "bpmn2:extensionElements/sf:notifications";
        public static readonly string Sf_StrXmlPath_SequenceFlow = "bpmn2:sequenceFlow";
        public static readonly string Sf_ElementName_Forms= "sf:forms";

        public static readonly string BPMN2_ElementName_Definitions = "bpmn2:definitions";
        public static readonly string BPMN2_ElementName_Collaboration = "bpmn2:collaboration";
        public static readonly string BPMN2_ElementName_Participant = "bpmn2:participant";
        public static readonly string BPMN2_ElementName_Process = "bpmn2:process";
        public static readonly string BPMN2_ElementName_ExtensionElements = "bpmn2:extensionElements";
        public static readonly string BPMN2_ElementName_SubProcess = "bpmn2:subProcess";
        public static readonly string BPMN2_ElementName_SequenceFlow = "bpmn2:sequenceFlow";
        public static readonly string BPMN2_ElementName_StartEvent = "bpmn2:startEvent";
        public static readonly string BPMN2_ElementName_EndEvent = "bpmn2:endEvent";
        public static readonly string BPMN2_ElementName_IntermediateEvent_Catch = "bpmn2:intermediateCatchEvent";
        public static readonly string BPMN2_ElementName_IntermediateEvent_Throw = "bpmn2:intermediateThrowEvent";
        public static readonly string BPMN2_ElementName_EventDefinition_Timer = "bpmn2:timerEventDefinition";
        public static readonly string BPMN2_ElementName_EventDefinition_Conditon = "bpmn2:conditionalEventDefinition";
        public static readonly string BPMN2_ElementName_EventDefinition_Message = "bpmn2:messageEventDefinition";
        public static readonly string BPMN2_ElementName_EventDefinition_Signal = "bpmn2:signalEventDefinition";
        public static readonly string BPMN2_ElementName_MessageFlow = "bpmn2:messageFlow";
        public static readonly string BPMN2_ElementName_Message = "bpmn2:message";

        public static readonly string BPMN2_ElementName_Task = "bpmn2:task";
        public static readonly string BPMN2_ElementName_UserTask = "bpmn2:userTask";
        public static readonly string BPMN2_ElementName_ManualTask = "bpmn2:manualTask";
        public static readonly string BPMN2_ElementName_ServiceTask = "bpmn2:serviceTask";
        public static readonly string BPMN2_ElementName_ScriptTask = "bpmn2:scriptTask";
        public static readonly string BPMN2_ElementName_ExclusiveGateway = "bpmn2:exclusiveGateway";
        public static readonly string BPMN2_ElementName_InclusiveGateway = "bpmn2:inclusiveGateway";
        public static readonly string BPMN2_ElementName_ParallelGateway = "bpmn2:parallelGateway";
        public static readonly string BPMN2_ElementName_ConditionExpression = "bpmn2:conditionExpression";
        public static readonly string BPMN2_ElementName_GroupBehaviours = "sf:groupBehaviours";

        public static readonly string BPMN2_StrXmlPath_Collaboration = "bpmn2:collaboration";
        public static readonly string BPMN2_StrXmlPath_Process = "bpmn2:process";
        public static readonly string BPMN2_StrXmlPath_Process_Sub = "bpmn2:process/bpmn2:subProcess";
        public static readonly string BPMN2_StrXmlPath_SequenceFlow = "bpmn2:process/bpmn2:sequenceFlow";
        public static readonly string BPMN2_StrXmlPath_StartEvent = "bpmn2:process/bpmn2:startEvent";
        public static readonly string BPMN2_StrXmlPath_EndEvent = "bpmn2:process/bpmn2:endEvent";
        public static readonly string BPMN2_StrXmlPath_Incoming = "bpmn2:incoming";
        public static readonly string BPMN2_StrXmlPath_Outgoing = "bpmn2:outgoing";
        public static readonly string BPMN2_StrXmlPath_LaneSet = "bpmn2:laneSet";
        public static readonly string BPMN2_StrXmlPath_Message = "bpmn2:message";
        public static readonly string BPMN2_StrXmlPath_MessageFlow = "bpmn2:messageFlow";
        public static readonly string BPMN2_StrXmlPath_Signal = "bpmn2:signal";
        public static readonly string BPMN2_StrXmlPath_SequenceFlow_Condition = "bpmn2:sequenceFlow/bpmn2:conditionExpression";
        public static readonly string BPMN2_StrXmlPath_SequenceFlow_GroupBehaviours = "bpmn2:extensionElements/sf:groupBehaviours";

        public static readonly string BPMN2_ElementName_Diagram = "bpmndi:BPMNDiagram";
        public static readonly string BPMN2_ElementName_Plane = "bpmndi:BPMNPlane";
        public static readonly string BPMN2_ElementName_Shape = "bpmndi:BPMNShape";
        public static readonly string BPMN2_StrXmlPath_DiagramPlane = "bpmndi:BPMNDiagram/bpmndi:BPMNPlane";
    }
}
