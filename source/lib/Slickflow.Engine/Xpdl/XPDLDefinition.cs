using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Xpdl
{
    /// <summary>
    /// 流程定义的XML文件中，用到的常量定义
    /// </summary>
    internal class XPDLDefinition
    {
        internal static readonly string StrXmlProcessPath = "WorkflowProcesses/Process";
        internal static readonly string StrXmlTransitionPath = "WorkflowProcesses/Process/Transitions/Transition";
        internal static readonly string StrXmlActivityPath = "WorkflowProcesses/Process/Activities/Activity";
        internal static readonly string StrXmlParticipantsPath = "Participants";
        internal static readonly string StrXmlSingleParticipantPath = "Participants/Participant";
        internal static readonly string StrXmlDataItemPermissions = "DataCollection/DataItemPermissions";
        internal static readonly string StrXmlParticipantDataItemPermissions = "DataCollection/DataItemPermissions/Participant";
        internal static readonly string StrXmlDataItems = "DataCollection/DataItems";
        internal static readonly string StrXmlSingleDataItems = "DataCollection/DataItems/DataItem";

        internal static readonly string BPMN2_NameSpacePrefix = "bpmn2";
        internal static readonly string BPMN2_NameSpacePrefix_Value = "http://www.omg.org/spec/BPMN/20100524/MODEL";

        internal static readonly string Sf_NameSpacePrefix = "sf";
        internal static readonly string Sf_NameSpacePrefix_Value = "http://www.slickflow.com/schema/sf";
        internal static readonly string Sf_StrXmlPath_Actions = "bpmn2:extensionElements/sf:actions";
        internal static readonly string Sf_StrXmlPath_Boundaries = "bpmn2:extensionElements/sf:boundaries";
        internal static readonly string Sf_StrXmlPath_GroupBehaviours = "bpmn2:extensionElements/sf:groupBehaviours";
        internal static readonly string Sf_StrXmlPath_GatewayDetail = "bpmn2:extensionElements/sf:gatewayDetail";
        internal static readonly string Sf_StrXmlPath_MultiSignDetail = "bpmn2:extensionElements/sf:multiSignDetail";
        internal static readonly string Sf_StrXmlPath_Performers = "bpmn2:extensionElements/sf:performers";
        internal static readonly string Sf_StrXmlPath_Sections = "bpmn2:extensionElements/sf:sections";
        internal static readonly string Sf_StrXmlPath_Services = "bpmn2:extensionElements/sf:services";
        internal static readonly string Sf_StrXmlPath_Scripts = "bpmn2:extensionElements/sf:scripts";

        internal static readonly string BPMN2_ElementName_Process = "bpmn2:process";
        internal static readonly string BPMN2_ElementName_SubProcess = "bpmn2:subProcess";
        internal static readonly string BPMN2_ElementName_SequenceFlow = "bpmn2:sequenceFlow";
        internal static readonly string BPMN2_ElementName_StartEvent = "bpmn2:startEvent";
        internal static readonly string BPMN2_ElementName_EndEvent = "bpmn2:endEvent";
        internal static readonly string BPMN2_ElementName_IntermediateEvent_Catch = "bpmn2:intermediateCatchEvent";
        internal static readonly string BPMN2_ElementName_IntermediateEvent_Throw = "bpmn2:intermediateThrowEvent";
        internal static readonly string BPMN2_ElementName_EventDefinition_Timer = "bpmn2:timerEventDefinition";
        internal static readonly string BPMN2_ElementName_EventDefinition_Conditon = "bpmn2:conditionalEventDefinition";
        internal static readonly string BPMN2_ElementName_EventDefinition_Message = "bpmn2:messageEventDefinition";
        
        internal static readonly string BPMN2_ElementName_Task = "bpmn2:task";
        internal static readonly string BPMN2_ElementName_UserTask = "bpmn2:userTask";
        internal static readonly string BPMN2_ElementName_ManualTask = "bpmn2:manualTask";
        internal static readonly string BPMN2_ElementName_ServiceTask = "bpmn2:serviceTask";
        internal static readonly string BPMN2_ElementName_ScriptTask = "bpmn2:scriptTask";
        internal static readonly string BPMN2_ElementName_ExclusiveGateway = "bpmn2:exclusiveGateway";
        internal static readonly string BPMN2_ElementName_InclusiveGateway = "bpmn2:inclusiveGateway";
        internal static readonly string BPMN2_ElementName_ParallelGateway = "bpmn2:parallelGateway";
        internal static readonly string BPMN2_ElementName_ConditionExpression = "bpmn2:conditionExpression";
        internal static readonly string BPMN2_ElementName_GroupBehaviours = "sf:groupBehaviours";

        internal static readonly string BPMN2_StrXmlPath_Process = "bpmn2:process";
        internal static readonly string BPMN2_StrXmlPath_Process_Sub = "bpmn2:process/bpmn2:subProcess";
        internal static readonly string BPMN2_StrXmlPath_TaskNode = "bpmn2:process/bpmn2:task";
        internal static readonly string BPMN2_StrXmlPath_StartNode = "bpmn2:process/bpmn2:startEvent";
        internal static readonly string BPMN2_StrXmlPath_EndNode = "bpmn2:process/bpmn2:endEvent";

        internal static readonly string BPMN2_StrXmlPath_SequenceFlow = "bpmn2:process/bpmn2:sequenceFlow";
        internal static readonly string BPMN2_StrXmlPath_Incoming = "bpmn2:incoming";
        internal static readonly string BPMN2_StrXmlPath_Outgoing = "bpmn2:outgoing";
        internal static readonly string BPMN2_StrXmlPath_SequenceFlow_Condition = "bpmn2:sequenceFlow/bpmn2:conditionExpression";
        internal static readonly string BPMN2_StrXmlPath_SequenceFlow_GroupBehaviours = "bpmn2:extensionElements/sf:groupBehaviours";    
    }
}
