USE [WfDBBpmn2]
GO
/****** Object:  Table [dbo].[WfTransitionInstance]    Script Date: 02/28/2025 17:15:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[WfTransitionInstance](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TransitionID] [varchar](50) NOT NULL,
	[AppName] [nvarchar](50) NOT NULL,
	[AppInstanceID] [varchar](50) NOT NULL,
	[ProcessInstanceID] [int] NOT NULL,
	[ProcessID] [varchar](50) NOT NULL,
	[TransitionType] [tinyint] NOT NULL,
	[FlyingType] [tinyint] NOT NULL,
	[FromActivityInstanceID] [int] NOT NULL,
	[FromActivityID] [varchar](50) NOT NULL,
	[FromActivityType] [smallint] NOT NULL,
	[FromActivityName] [nvarchar](50) NOT NULL,
	[ToActivityInstanceID] [int] NOT NULL,
	[ToActivityID] [varchar](50) NOT NULL,
	[ToActivityType] [smallint] NOT NULL,
	[ToActivityName] [nvarchar](50) NOT NULL,
	[ConditionParseResult] [tinyint] NOT NULL,
	[CreatedByUserID] [varchar](50) NOT NULL,
	[CreatedByUserName] [nvarchar](50) NOT NULL,
	[CreatedDateTime] [datetime] NOT NULL,
	[RecordStatusInvalid] [tinyint] NOT NULL,
	[RowVersionID] [timestamp] NULL,
 CONSTRAINT [PK_WfTransitionInstance] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[WfTransitionInstance] ON
INSERT [dbo].[WfTransitionInstance] ([ID], [TransitionID], [AppName], [AppInstanceID], [ProcessInstanceID], [ProcessID], [TransitionType], [FlyingType], [FromActivityInstanceID], [FromActivityID], [FromActivityType], [FromActivityName], [ToActivityInstanceID], [ToActivityID], [ToActivityType], [ToActivityName], [ConditionParseResult], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [RecordStatusInvalid]) VALUES (1, N'Flow_3000', N'Order-Books', N'123', 1, N'Process_o5uf_2550', 1, 0, 1, N'StartNode_4475', 1, N'Start', 2, N'TaskNode_5417', 4, N'Task-001', 1, N'01', N'Zero', CAST(0x0000B28C00B521C1 AS DateTime), 0)
INSERT [dbo].[WfTransitionInstance] ([ID], [TransitionID], [AppName], [AppInstanceID], [ProcessInstanceID], [ProcessID], [TransitionType], [FlyingType], [FromActivityInstanceID], [FromActivityID], [FromActivityType], [FromActivityName], [ToActivityInstanceID], [ToActivityID], [ToActivityType], [ToActivityName], [ConditionParseResult], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [RecordStatusInvalid]) VALUES (2, N'Flow_4383', N'Order-Books', N'123', 1, N'Process_o5uf_2550', 1, 0, 2, N'TaskNode_5417', 4, N'Task-001', 3, N'TaskNode_5679', 4, N'Task-002', 1, N'01', N'Zero', CAST(0x0000B28C00B52850 AS DateTime), 0)
INSERT [dbo].[WfTransitionInstance] ([ID], [TransitionID], [AppName], [AppInstanceID], [ProcessInstanceID], [ProcessID], [TransitionType], [FlyingType], [FromActivityInstanceID], [FromActivityID], [FromActivityType], [FromActivityName], [ToActivityInstanceID], [ToActivityID], [ToActivityType], [ToActivityName], [ConditionParseResult], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [RecordStatusInvalid]) VALUES (3, N'Flow_5059', N'Order-Books', N'123', 1, N'Process_o5uf_2550', 1, 0, 3, N'TaskNode_5679', 4, N'Task-002', 4, N'TaskNode_6468', 4, N'Task-003', 1, N'101', N'Jenny(模拟)', CAST(0x0000B28C00B52DE0 AS DateTime), 0)
INSERT [dbo].[WfTransitionInstance] ([ID], [TransitionID], [AppName], [AppInstanceID], [ProcessInstanceID], [ProcessID], [TransitionType], [FlyingType], [FromActivityInstanceID], [FromActivityID], [FromActivityType], [FromActivityName], [ToActivityInstanceID], [ToActivityID], [ToActivityType], [ToActivityName], [ConditionParseResult], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [RecordStatusInvalid]) VALUES (4, N'Flow_9721', N'Order-Books', N'123', 1, N'Process_o5uf_2550', 1, 0, 4, N'TaskNode_6468', 4, N'Task-003', 5, N'EndNode_3185', 2, N'End', 1, N'201', N'Terrisa(模拟)', CAST(0x0000B28C00B5332B AS DateTime), 0)
INSERT [dbo].[WfTransitionInstance] ([ID], [TransitionID], [AppName], [AppInstanceID], [ProcessInstanceID], [ProcessID], [TransitionType], [FlyingType], [FromActivityInstanceID], [FromActivityID], [FromActivityType], [FromActivityName], [ToActivityInstanceID], [ToActivityID], [ToActivityType], [ToActivityName], [ConditionParseResult], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [RecordStatusInvalid]) VALUES (5, N'Flow_1308', N'Order-Books', N'123', 2, N'Process_cswy_5517', 1, 0, 6, N'StartNode_6553', 1, N'start', 7, N'TaskNode_6976', 4, N'Task-001', 1, N'01', N'Zero', CAST(0x0000B28D000998A4 AS DateTime), 0)
INSERT [dbo].[WfTransitionInstance] ([ID], [TransitionID], [AppName], [AppInstanceID], [ProcessInstanceID], [ProcessID], [TransitionType], [FlyingType], [FromActivityInstanceID], [FromActivityID], [FromActivityType], [FromActivityName], [ToActivityInstanceID], [ToActivityID], [ToActivityType], [ToActivityName], [ConditionParseResult], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [RecordStatusInvalid]) VALUES (6, N'Flow_7557', N'Order-Books', N'123', 2, N'Process_cswy_5517', 1, 0, 7, N'TaskNode_6976', 4, N'Task-001', 8, N'GatewayNode_1841', 8, N'or-split', 1, N'01', N'Zero', CAST(0x0000B28D0009AD1B AS DateTime), 0)
INSERT [dbo].[WfTransitionInstance] ([ID], [TransitionID], [AppName], [AppInstanceID], [ProcessInstanceID], [ProcessID], [TransitionType], [FlyingType], [FromActivityInstanceID], [FromActivityID], [FromActivityType], [FromActivityName], [ToActivityInstanceID], [ToActivityID], [ToActivityType], [ToActivityName], [ConditionParseResult], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [RecordStatusInvalid]) VALUES (7, N'Flow_3721', N'Order-Books', N'123', 2, N'Process_cswy_5517', 1, 0, 8, N'GatewayNode_1841', 8, N'or-split', 9, N'TaskNode_9595', 4, N'task-020', 1, N'01', N'Zero', CAST(0x0000B28D0009AD1C AS DateTime), 0)
INSERT [dbo].[WfTransitionInstance] ([ID], [TransitionID], [AppName], [AppInstanceID], [ProcessInstanceID], [ProcessID], [TransitionType], [FlyingType], [FromActivityInstanceID], [FromActivityID], [FromActivityType], [FromActivityName], [ToActivityInstanceID], [ToActivityID], [ToActivityType], [ToActivityName], [ConditionParseResult], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [RecordStatusInvalid]) VALUES (8, N'SEND-BACK-UNKNOWN_GUID', N'Order-Books', N'123', 2, N'Process_cswy_5517', 14, 0, 9, N'TaskNode_9595', 4, N'task-020', 10, N'TaskNode_6976', 4, N'Task-001', 1, N'120', N'Lary(模拟)', CAST(0x0000B28D0009BA13 AS DateTime), 0)
INSERT [dbo].[WfTransitionInstance] ([ID], [TransitionID], [AppName], [AppInstanceID], [ProcessInstanceID], [ProcessID], [TransitionType], [FlyingType], [FromActivityInstanceID], [FromActivityID], [FromActivityType], [FromActivityName], [ToActivityInstanceID], [ToActivityID], [ToActivityType], [ToActivityName], [ConditionParseResult], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [RecordStatusInvalid]) VALUES (9, N'Flow_AstartEvent_AsubmitApplication', N'Order-Books', N'123', 3, N'employeeLeaveRequestProcess_hwpb', 1, 0, 11, N'AstartEvent', 1, N'流程开始', 12, N'AsubmitApplication', 4, N'提交请假申请', 1, N'01', N'Zero', CAST(0x0000B28D0009D143 AS DateTime), 0)
INSERT [dbo].[WfTransitionInstance] ([ID], [TransitionID], [AppName], [AppInstanceID], [ProcessInstanceID], [ProcessID], [TransitionType], [FlyingType], [FromActivityInstanceID], [FromActivityID], [FromActivityType], [FromActivityName], [ToActivityInstanceID], [ToActivityID], [ToActivityType], [ToActivityName], [ConditionParseResult], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [RecordStatusInvalid]) VALUES (10, N'JUMP-BYPASS-GUID', N'Order-Books', N'123', 3, N'employeeLeaveRequestProcess_hwpb', 1, 1, 12, N'AsubmitApplication', 4, N'提交请假申请', 13, N'AendEvent', 2, N'流程结束', 1, N'01', N'Zero', CAST(0x0000B28D0009F8FF AS DateTime), 0)
INSERT [dbo].[WfTransitionInstance] ([ID], [TransitionID], [AppName], [AppInstanceID], [ProcessInstanceID], [ProcessID], [TransitionType], [FlyingType], [FromActivityInstanceID], [FromActivityID], [FromActivityType], [FromActivityName], [ToActivityInstanceID], [ToActivityID], [ToActivityType], [ToActivityName], [ConditionParseResult], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [RecordStatusInvalid]) VALUES (11, N'Flow_3000', N'Order-Books', N'123', 4, N'Process_o5uf_2550', 1, 0, 14, N'StartNode_4475', 1, N'Start', 15, N'TaskNode_5417', 4, N'Task-001', 1, N'01', N'Zero', CAST(0x0000B28D00CD8CF9 AS DateTime), 0)
INSERT [dbo].[WfTransitionInstance] ([ID], [TransitionID], [AppName], [AppInstanceID], [ProcessInstanceID], [ProcessID], [TransitionType], [FlyingType], [FromActivityInstanceID], [FromActivityID], [FromActivityType], [FromActivityName], [ToActivityInstanceID], [ToActivityID], [ToActivityType], [ToActivityName], [ConditionParseResult], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [RecordStatusInvalid]) VALUES (12, N'Flow_4383', N'Order-Books', N'123', 4, N'Process_o5uf_2550', 1, 0, 15, N'TaskNode_5417', 4, N'Task-001', 16, N'TaskNode_5679', 4, N'Task-002', 1, N'01', N'Zero', CAST(0x0000B28D00CD9384 AS DateTime), 0)
INSERT [dbo].[WfTransitionInstance] ([ID], [TransitionID], [AppName], [AppInstanceID], [ProcessInstanceID], [ProcessID], [TransitionType], [FlyingType], [FromActivityInstanceID], [FromActivityID], [FromActivityType], [FromActivityName], [ToActivityInstanceID], [ToActivityID], [ToActivityType], [ToActivityName], [ConditionParseResult], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [RecordStatusInvalid]) VALUES (13, N'Flow_5059', N'Order-Books', N'123', 4, N'Process_o5uf_2550', 1, 0, 16, N'TaskNode_5679', 4, N'Task-002', 17, N'TaskNode_6468', 4, N'Task-003', 1, N'130', N'Monica(模拟)', CAST(0x0000B28D00CD99F5 AS DateTime), 0)
INSERT [dbo].[WfTransitionInstance] ([ID], [TransitionID], [AppName], [AppInstanceID], [ProcessInstanceID], [ProcessID], [TransitionType], [FlyingType], [FromActivityInstanceID], [FromActivityID], [FromActivityType], [FromActivityName], [ToActivityInstanceID], [ToActivityID], [ToActivityType], [ToActivityName], [ConditionParseResult], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [RecordStatusInvalid]) VALUES (14, N'Flow_9721', N'Order-Books', N'123', 4, N'Process_o5uf_2550', 1, 0, 17, N'TaskNode_6468', 4, N'Task-003', 18, N'EndNode_3185', 2, N'End', 1, N'31', N'Cindy(模拟)', CAST(0x0000B28D00CD9FD7 AS DateTime), 0)
SET IDENTITY_INSERT [dbo].[WfTransitionInstance] OFF
/****** Object:  Table [dbo].[WfProcessVariable]    Script Date: 02/28/2025 17:15:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[WfProcessVariable](
	[ID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[VariableType] [varchar](50) NOT NULL,
	[AppInstanceID] [varchar](100) NOT NULL,
	[ProcessID] [varchar](50) NOT NULL,
	[ProcessInstanceID] [int] NOT NULL,
	[ActivityID] [varchar](50) NULL,
	[ActivityName] [nvarchar](50) NULL,
	[Name] [varchar](50) NOT NULL,
	[Value] [nvarchar](1024) NOT NULL,
	[LastUpdatedDateTime] [datetime] NOT NULL,
	[RowVersionID] [timestamp] NOT NULL,
 CONSTRAINT [PK_WFPROCESSVARIABLE] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WfProcessVariable', @level2type=N'COLUMN',@level2name=N'VariableType'
GO
/****** Object:  Table [dbo].[WfProcessInstance]    Script Date: 02/28/2025 17:15:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[WfProcessInstance](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ProcessID] [varchar](100) NOT NULL,
	[ProcessName] [nvarchar](50) NOT NULL,
	[Version] [nvarchar](20) NOT NULL,
	[AppInstanceID] [varchar](50) NOT NULL,
	[AppName] [nvarchar](50) NOT NULL,
	[AppInstanceCode] [nvarchar](50) NULL,
	[ProcessState] [smallint] NOT NULL,
	[SubProcessType] [smallint] NULL,
	[SubProcessDefID] [int] NULL,
	[SubProcessID] [varchar](50) NULL,
	[InvokedActivityInstanceID] [int] NULL,
	[InvokedActivityID] [varchar](50) NULL,
	[JobTimerType] [smallint] NULL,
	[JobTimerStatus] [smallint] NULL,
	[TriggerExpression] [nvarchar](200) NULL,
	[OverdueDateTime] [datetime] NULL,
	[JobTimerTreatedDateTime] [datetime] NULL,
	[CreatedDateTime] [datetime] NOT NULL,
	[CreatedByUserID] [varchar](50) NOT NULL,
	[CreatedByUserName] [nvarchar](50) NOT NULL,
	[LastUpdatedDateTime] [datetime] NULL,
	[LastUpdatedByUserID] [varchar](50) NULL,
	[LastUpdatedByUserName] [nvarchar](50) NULL,
	[EndedDateTime] [datetime] NULL,
	[EndedByUserID] [varchar](50) NULL,
	[EndedByUserName] [nvarchar](50) NULL,
	[RecordStatusInvalid] [tinyint] NOT NULL,
	[RowVersionID] [timestamp] NULL,
 CONSTRAINT [PK_WfProcessInstance] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[WfProcessInstance] ON
INSERT [dbo].[WfProcessInstance] ([ID], [ProcessID], [ProcessName], [Version], [AppInstanceID], [AppName], [AppInstanceCode], [ProcessState], [SubProcessType], [SubProcessDefID], [SubProcessID], [InvokedActivityInstanceID], [InvokedActivityID], [JobTimerType], [JobTimerStatus], [TriggerExpression], [OverdueDateTime], [JobTimerTreatedDateTime], [CreatedDateTime], [CreatedByUserID], [CreatedByUserName], [LastUpdatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [EndedDateTime], [EndedByUserID], [EndedByUserName], [RecordStatusInvalid]) VALUES (1, N'Process_o5uf_2550', N'Sequence_2550', N'1', N'123', N'Order-Books', N'123-code', 4, NULL, 0, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, CAST(0x0000B28C00B5219A AS DateTime), N'01', N'Zero', CAST(0x0000B28C00B5219A AS DateTime), N'01', N'Zero', CAST(0x0000B28C00B5332B AS DateTime), N'201', N'Terrisa(模拟)', 0)
INSERT [dbo].[WfProcessInstance] ([ID], [ProcessID], [ProcessName], [Version], [AppInstanceID], [AppName], [AppInstanceCode], [ProcessState], [SubProcessType], [SubProcessDefID], [SubProcessID], [InvokedActivityInstanceID], [InvokedActivityID], [JobTimerType], [JobTimerStatus], [TriggerExpression], [OverdueDateTime], [JobTimerTreatedDateTime], [CreatedDateTime], [CreatedByUserID], [CreatedByUserName], [LastUpdatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [EndedDateTime], [EndedByUserID], [EndedByUserName], [RecordStatusInvalid]) VALUES (2, N'Process_cswy_5517', N'Complex_5517', N'1', N'123', N'Order-Books', N'123-code', 2, NULL, 0, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, CAST(0x0000B28D0009987E AS DateTime), N'01', N'Zero', CAST(0x0000B28D0009987E AS DateTime), N'01', N'Zero', NULL, NULL, NULL, 0)
INSERT [dbo].[WfProcessInstance] ([ID], [ProcessID], [ProcessName], [Version], [AppInstanceID], [AppName], [AppInstanceCode], [ProcessState], [SubProcessType], [SubProcessDefID], [SubProcessID], [InvokedActivityInstanceID], [InvokedActivityID], [JobTimerType], [JobTimerStatus], [TriggerExpression], [OverdueDateTime], [JobTimerTreatedDateTime], [CreatedDateTime], [CreatedByUserID], [CreatedByUserName], [LastUpdatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [EndedDateTime], [EndedByUserID], [EndedByUserName], [RecordStatusInvalid]) VALUES (3, N'employeeLeaveRequestProcess_hwpb', N'员工请假流程', N'1', N'123', N'Order-Books', N'123-code', 4, NULL, 0, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, CAST(0x0000B28D0009D142 AS DateTime), N'01', N'Zero', CAST(0x0000B28D0009D142 AS DateTime), N'01', N'Zero', CAST(0x0000B28D0009F8FF AS DateTime), N'01', N'Zero', 0)
INSERT [dbo].[WfProcessInstance] ([ID], [ProcessID], [ProcessName], [Version], [AppInstanceID], [AppName], [AppInstanceCode], [ProcessState], [SubProcessType], [SubProcessDefID], [SubProcessID], [InvokedActivityInstanceID], [InvokedActivityID], [JobTimerType], [JobTimerStatus], [TriggerExpression], [OverdueDateTime], [JobTimerTreatedDateTime], [CreatedDateTime], [CreatedByUserID], [CreatedByUserName], [LastUpdatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [EndedDateTime], [EndedByUserID], [EndedByUserName], [RecordStatusInvalid]) VALUES (4, N'Process_o5uf_2550', N'Sequence_2550', N'1', N'123', N'Order-Books', N'123-code', 4, NULL, 0, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, CAST(0x0000B28D00CD8CD6 AS DateTime), N'01', N'Zero', CAST(0x0000B28D00CD8CD6 AS DateTime), N'01', N'Zero', CAST(0x0000B28D00CD9FD7 AS DateTime), N'31', N'Cindy(模拟)', 0)
SET IDENTITY_INSERT [dbo].[WfProcessInstance] OFF
/****** Object:  Table [dbo].[WfProcess]    Script Date: 02/28/2025 17:15:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[WfProcess](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ProcessID] [varchar](50) NOT NULL,
	[Version] [nvarchar](20) NOT NULL,
	[ProcessName] [nvarchar](50) NOT NULL,
	[ProcessCode] [varchar](50) NOT NULL,
	[IsUsing] [tinyint] NOT NULL,
	[AppType] [varchar](20) NULL,
	[PackageType] [tinyint] NULL,
	[PackageID] [int] NULL,
	[ParticipantGUID] [varchar](100) NULL,
	[PageUrl] [nvarchar](100) NULL,
	[XmlFileName] [nvarchar](50) NULL,
	[XmlFilePath] [nvarchar](50) NULL,
	[XmlContent] [nvarchar](max) NULL,
	[StartType] [tinyint] NOT NULL,
	[StartExpression] [varchar](100) NULL,
	[Description] [nvarchar](1000) NULL,
	[EndType] [tinyint] NOT NULL,
	[EndExpression] [varchar](100) NULL,
	[CreatedDateTime] [datetime] NOT NULL,
	[LastUpdatedDateTime] [datetime] NULL,
	[RowVersionID] [timestamp] NULL,
 CONSTRAINT [PK_WfProcess] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[WfProcess] ON
INSERT [dbo].[WfProcess] ([ID], [ProcessID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1587, N'Process_o5uf_2550', N'1', N'Sequence_2550', N'Sequence_Code_2550', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL" id="bpmn-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn:process id="Process_o5uf_2550" sf:code="Sequence_Code_2550" name="Sequence_2550" isExecutable="true" sf:version="1">
    <bpmn:startEvent id="StartNode_4475" sf:code="Start" name="Start">
      <bpmn:outgoing>Flow_3000</bpmn:outgoing>
    </bpmn:startEvent>
    <bpmn:task id="TaskNode_5417" sf:code="task001" name="Task-001">
      <bpmn:incoming>Flow_3000</bpmn:incoming>
      <bpmn:outgoing>Flow_4383</bpmn:outgoing>
    </bpmn:task>
    <bpmn:task id="TaskNode_5679" sf:code="task002" name="Task-002">
      <bpmn:incoming>Flow_4383</bpmn:incoming>
      <bpmn:outgoing>Flow_5059</bpmn:outgoing>
    </bpmn:task>
    <bpmn:task id="TaskNode_6468" sf:code="task003" name="Task-003">
      <bpmn:incoming>Flow_5059</bpmn:incoming>
      <bpmn:outgoing>Flow_9721</bpmn:outgoing>
    </bpmn:task>
    <bpmn:endEvent id="EndNode_3185" sf:code="End" name="End">
      <bpmn:incoming>Flow_9721</bpmn:incoming>
    </bpmn:endEvent>
    <bpmn:sequenceFlow id="Flow_3000" name="" sourceRef="StartNode_4475" targetRef="TaskNode_5417" />
    <bpmn:sequenceFlow id="Flow_4383" name="t-001" sourceRef="TaskNode_5417" targetRef="TaskNode_5679" />
    <bpmn:sequenceFlow id="Flow_5059" name="" sourceRef="TaskNode_5679" targetRef="TaskNode_6468" />
    <bpmn:sequenceFlow id="Flow_9721" name="" sourceRef="TaskNode_6468" targetRef="EndNode_3185" />
  </bpmn:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_o5uf_2550">
      <bpmndi:BPMNShape id="BPMNShape_1dtuzzl_di" bpmnElement="StartNode_4475">
        <dc:Bounds x="240" y="180" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="BPMNShape_6uognln_di" bpmnElement="TaskNode_5417">
        <dc:Bounds x="356" y="158" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="BPMNShape_m46q7yf_di" bpmnElement="TaskNode_5679">
        <dc:Bounds x="536" y="158" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="BPMNShape_dbrarcz_di" bpmnElement="TaskNode_6468">
        <dc:Bounds x="716" y="158" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="BPMNShape_39mumkz_di" bpmnElement="EndNode_3185">
        <dc:Bounds x="896" y="180" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNEdge id="Flow_3000_di" bpmnElement="Flow_3000">
        <di:waypoint x="276" y="198" />
        <di:waypoint x="356" y="198" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_4383_di" bpmnElement="Flow_4383">
        <di:waypoint x="456" y="198" />
        <di:waypoint x="536" y="198" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_5059_di" bpmnElement="Flow_5059">
        <di:waypoint x="636" y="198" />
        <di:waypoint x="716" y="198" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_9721_di" bpmnElement="Flow_9721">
        <di:waypoint x="816" y="198" />
        <di:waypoint x="896" y="198" />
      </bpmndi:BPMNEdge>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn:definitions>
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               ', 0, NULL, NULL, 0, NULL, CAST(0x0000B28C00B495EF AS DateTime), CAST(0x0000B28C00B49CF3 AS DateTime))
INSERT [dbo].[WfProcess] ([ID], [ProcessID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1588, N'Process_sobf_7843', N'1', N'EOrder_7843', N'EOrder_Code_7843', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="utf-8"?><bpmn:definitions targetNamespace="http://bpmn.io/schema/bpmn" id="bpmn-diagram" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL"><bpmn:process id="Process_sobf_7843" name="EOrder_7843" isExecutable="true" sf:code="EOrder_Code_7843" sf:version="1"><bpmn:startEvent id="StartNode_9777" name="start" sf:code="Start"><bpmn:outgoing>Flow_5031</bpmn:outgoing></bpmn:startEvent><bpmn:task id="TaskNode_1998" name="Dispatch Order" sf:code="task001"><bpmn:incoming>Flow_5031</bpmn:incoming><bpmn:outgoing>Flow_9870</bpmn:outgoing></bpmn:task><bpmn:inclusiveGateway id="GatewayNode_8109" name="Or-Split" sf:code="orsplit001"><bpmn:incoming>Flow_9870</bpmn:incoming><bpmn:outgoing>Flow_4329</bpmn:outgoing><bpmn:outgoing>Flow_9409</bpmn:outgoing></bpmn:inclusiveGateway><bpmn:task id="TaskNode_6903" name="Print Delivery Note" sf:code="task010"><bpmn:incoming>Flow_4329</bpmn:incoming><bpmn:outgoing>Flow_3770</bpmn:outgoing></bpmn:task><bpmn:task id="TaskNode_5851" name="Sample Making" sf:code="task020"><bpmn:incoming>Flow_9409</bpmn:incoming><bpmn:outgoing>Flow_4783</bpmn:outgoing></bpmn:task><bpmn:task id="TaskNode_5265" name="Produce" sf:code="task030"><bpmn:incoming>Flow_4783</bpmn:incoming><bpmn:outgoing>Flow_4215</bpmn:outgoing></bpmn:task><bpmn:task id="TaskNode_3365" name="QA" sf:code="task040"><bpmn:incoming>Flow_4215</bpmn:incoming><bpmn:outgoing>Flow_6340</bpmn:outgoing></bpmn:task><bpmn:task id="TaskNode_8424" name="Weight" sf:code="task050"><bpmn:incoming>Flow_6340</bpmn:incoming><bpmn:outgoing>Flow_9455</bpmn:outgoing></bpmn:task><bpmn:inclusiveGateway id="GatewayNode_1247" name="Or-Join" sf:code="orjoin001"><bpmn:incoming>Flow_9455</bpmn:incoming><bpmn:incoming>Flow_3770</bpmn:incoming><bpmn:outgoing>Flow_7499</bpmn:outgoing></bpmn:inclusiveGateway><bpmn:endEvent id="EndNode_5026" name="end" sf:code="End"><bpmn:incoming>Flow_7499</bpmn:incoming></bpmn:endEvent><bpmn:sequenceFlow id="Flow_5031" name="" sourceRef="StartNode_9777" targetRef="TaskNode_1998" /><bpmn:sequenceFlow id="Flow_9870" name="" sourceRef="TaskNode_1998" targetRef="GatewayNode_8109" /><bpmn:sequenceFlow id="Flow_4329" name="HasInventory=&quot;Y&quot;" sourceRef="GatewayNode_8109" targetRef="TaskNode_6903"><bpmn:conditionExpression>HasInventory="Y"</bpmn:conditionExpression></bpmn:sequenceFlow><bpmn:sequenceFlow id="Flow_9409" name="HasInventory=&quot;N&quot;" sourceRef="GatewayNode_8109" targetRef="TaskNode_5851"><bpmn:conditionExpression>HasInventory="N"</bpmn:conditionExpression></bpmn:sequenceFlow><bpmn:sequenceFlow id="Flow_4783" name="" sourceRef="TaskNode_5851" targetRef="TaskNode_5265" /><bpmn:sequenceFlow id="Flow_4215" name="" sourceRef="TaskNode_5265" targetRef="TaskNode_3365" /><bpmn:sequenceFlow id="Flow_6340" name="" sourceRef="TaskNode_3365" targetRef="TaskNode_8424" /><bpmn:sequenceFlow id="Flow_9455" name="" sourceRef="TaskNode_8424" targetRef="GatewayNode_1247" /><bpmn:sequenceFlow id="Flow_3770" name="" sourceRef="TaskNode_6903" targetRef="GatewayNode_1247" /><bpmn:sequenceFlow id="Flow_7499" name="" sourceRef="GatewayNode_1247" targetRef="EndNode_5026" /></bpmn:process><bpmndi:BPMNDiagram id="BPMNDiagram_1"><bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_100"><bpmndi:BPMNShape id="BPMNShape_h3r66id_di" bpmnElement="StartNode_9777"><dc:Bounds height="36" width="36" x="240" y="180" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_gjeedyy_di" bpmnElement="TaskNode_1998"><dc:Bounds height="80" width="100" x="356" y="158" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_9ol5b35_di" bpmnElement="GatewayNode_8109"><dc:Bounds height="36" width="36" x="536" y="180" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_pict4c6_di" bpmnElement="TaskNode_6903"><dc:Bounds height="80" width="100" x="652" y="230" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_9ps6t3y_di" bpmnElement="TaskNode_5851"><dc:Bounds height="80" width="100" x="652" y="70" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_t421egp_di" bpmnElement="TaskNode_5265"><dc:Bounds height="80" width="100" x="832" y="70" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_cx7p2qe_di" bpmnElement="TaskNode_3365"><dc:Bounds height="80" width="100" x="1012" y="70" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_t5e8ypu_di" bpmnElement="TaskNode_8424"><dc:Bounds height="80" width="100" x="1192" y="70" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_jkwpxar_di" bpmnElement="GatewayNode_1247"><dc:Bounds height="36" width="36" x="1372" y="180" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_q6gc4s7_di" bpmnElement="EndNode_5026"><dc:Bounds height="36" width="36" x="1488" y="180" /></bpmndi:BPMNShape><bpmndi:BPMNEdge id="Flow_5031_di" bpmnElement="Flow_5031"><di:waypoint x="276" y="198" /><di:waypoint x="356" y="198" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_9870_di" bpmnElement="Flow_9870"><di:waypoint x="456" y="198" /><di:waypoint x="536" y="198" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_4329_di" bpmnElement="Flow_4329"><di:waypoint x="554" y="216" /><di:waypoint x="554" y="270" /><di:waypoint x="652" y="270" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_9409_di" bpmnElement="Flow_9409"><di:waypoint x="554" y="216" /><di:waypoint x="554" y="110" /><di:waypoint x="652" y="110" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_4783_di" bpmnElement="Flow_4783"><di:waypoint x="752" y="110" /><di:waypoint x="832" y="110" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_4215_di" bpmnElement="Flow_4215"><di:waypoint x="932" y="110" /><di:waypoint x="1012" y="110" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_6340_di" bpmnElement="Flow_6340"><di:waypoint x="1112" y="110" /><di:waypoint x="1192" y="110" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_9455_di" bpmnElement="Flow_9455"><di:waypoint x="1292" y="110" /><di:waypoint x="1390" y="110" /><di:waypoint x="1390" y="180" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_3770_di" bpmnElement="Flow_3770"><di:waypoint x="752" y="270" /><di:waypoint x="1390" y="270" /><di:waypoint x="1390" y="216" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_7499_di" bpmnElement="Flow_7499"><di:waypoint x="1408" y="198" /><di:waypoint x="1488" y="198" /></bpmndi:BPMNEdge></bpmndi:BPMNPlane></bpmndi:BPMNDiagram></bpmn:definitions>', 0, NULL, NULL, 0, NULL, CAST(0x0000B28C00B4A696 AS DateTime), NULL)
INSERT [dbo].[WfProcess] ([ID], [ProcessID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1589, N'Process_b0u9_3209', N'1', N'MultipleInstance_3209', N'MultipleInstance_Code_3209', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="utf-8"?><bpmn:definitions targetNamespace="http://bpmn.io/schema/bpmn" id="bpmn-diagram" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL"><bpmn:process id="Process_b0u9_3209" name="MultipleInstance_3209" isExecutable="true" sf:code="MultipleInstance_Code_3209" sf:version="1"><bpmn:startEvent id="StartNode_8531" name="Start" sf:code="Start"><bpmn:outgoing>Flow_2688</bpmn:outgoing></bpmn:startEvent><bpmn:task id="TaskNode_8493" name="Task-001" sf:code="task001"><bpmn:incoming>Flow_2688</bpmn:incoming><bpmn:outgoing>Flow_5490</bpmn:outgoing></bpmn:task><bpmn:task id="MultiSignNode_9440" name="Sign Together" sf:code="MI001"><bpmn:incoming>Flow_5490</bpmn:incoming><bpmn:outgoing>Flow_1443</bpmn:outgoing><bpmn:multiInstanceLoopCharacteristics /></bpmn:task><bpmn:task id="TaskNode_8743" name="Task-003" sf:code="task003"><bpmn:incoming>Flow_1443</bpmn:incoming><bpmn:outgoing>Flow_8383</bpmn:outgoing></bpmn:task><bpmn:endEvent id="EndNode_2837" name="End" sf:code="End"><bpmn:incoming>Flow_8383</bpmn:incoming></bpmn:endEvent><bpmn:sequenceFlow id="Flow_2688" name="" sourceRef="StartNode_8531" targetRef="TaskNode_8493" /><bpmn:sequenceFlow id="Flow_5490" name="" sourceRef="TaskNode_8493" targetRef="MultiSignNode_9440" /><bpmn:sequenceFlow id="Flow_1443" name="" sourceRef="MultiSignNode_9440" targetRef="TaskNode_8743" /><bpmn:sequenceFlow id="Flow_8383" name="" sourceRef="TaskNode_8743" targetRef="EndNode_2837" /></bpmn:process><bpmndi:BPMNDiagram id="BPMNDiagram_1"><bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_100"><bpmndi:BPMNShape id="BPMNShape_d5k1oa7_di" bpmnElement="StartNode_8531"><dc:Bounds height="36" width="36" x="240" y="180" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_xeldlp1_di" bpmnElement="TaskNode_8493"><dc:Bounds height="80" width="100" x="356" y="158" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_147zj6l_di" bpmnElement="MultiSignNode_9440"><dc:Bounds height="80" width="100" x="536" y="158" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_8405co3_di" bpmnElement="TaskNode_8743"><dc:Bounds height="80" width="100" x="716" y="158" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_7cbuja3_di" bpmnElement="EndNode_2837"><dc:Bounds height="36" width="36" x="896" y="180" /></bpmndi:BPMNShape><bpmndi:BPMNEdge id="Flow_2688_di" bpmnElement="Flow_2688"><di:waypoint x="276" y="198" /><di:waypoint x="356" y="198" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_5490_di" bpmnElement="Flow_5490"><di:waypoint x="456" y="198" /><di:waypoint x="536" y="198" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_1443_di" bpmnElement="Flow_1443"><di:waypoint x="636" y="198" /><di:waypoint x="716" y="198" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_8383_di" bpmnElement="Flow_8383"><di:waypoint x="816" y="198" /><di:waypoint x="896" y="198" /></bpmndi:BPMNEdge></bpmndi:BPMNPlane></bpmndi:BPMNDiagram></bpmn:definitions>', 0, NULL, NULL, 0, NULL, CAST(0x0000B28C00B4B12C AS DateTime), NULL)
INSERT [dbo].[WfProcess] ([ID], [ProcessID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1590, N'Process_0rku_7126', N'1', N'AndSplitMI_7126', N'AndSplitMI_Code_7126', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="utf-8"?><bpmn:definitions targetNamespace="http://bpmn.io/schema/bpmn" id="bpmn-diagram" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL"><bpmn:process id="Process_0rku_7126" name="AndSplitMI_7126" isExecutable="true" sf:code="AndSplitMI_Code_7126" sf:version="1"><bpmn:startEvent id="StartNode_3580" name="start" sf:code="Start"><bpmn:outgoing>Flow_4473</bpmn:outgoing></bpmn:startEvent><bpmn:task id="TaskNode_6211" name="Task-001" sf:code="task001"><bpmn:incoming>Flow_4473</bpmn:incoming><bpmn:outgoing>Flow_6384</bpmn:outgoing></bpmn:task><bpmn:parallelGateway id="GatewayNode_1499" name="and-split" sf:code="andsplit001"><bpmn:incoming>Flow_6384</bpmn:incoming><bpmn:outgoing>Flow_7851</bpmn:outgoing><bpmn:extensionElements><sf:gatewayDetail extraSplitType="AndSplitMI" /></bpmn:extensionElements></bpmn:parallelGateway><bpmn:task id="TaskNode_2937" name="task-010" sf:code="task010"><bpmn:incoming>Flow_7851</bpmn:incoming><bpmn:outgoing>Flow_8178</bpmn:outgoing></bpmn:task><bpmn:task id="TaskNode_4574" name="task-011" sf:code="task011"><bpmn:incoming>Flow_8178</bpmn:incoming><bpmn:outgoing>Flow_7414</bpmn:outgoing></bpmn:task><bpmn:parallelGateway id="GatewayNode_3212" name="and-join" sf:code="andjoin001"><bpmn:incoming>Flow_7414</bpmn:incoming><bpmn:outgoing>Flow_9411</bpmn:outgoing><bpmn:extensionElements><sf:gatewayDetail extraJoinType="AndJoinMI" /></bpmn:extensionElements></bpmn:parallelGateway><bpmn:task id="TaskNode_7043" name="task-100" sf:code="task100"><bpmn:incoming>Flow_9411</bpmn:incoming><bpmn:outgoing>Flow_9743</bpmn:outgoing></bpmn:task><bpmn:endEvent id="EndNode_7805" name="end" sf:code="End"><bpmn:incoming>Flow_9743</bpmn:incoming></bpmn:endEvent><bpmn:sequenceFlow id="Flow_4473" name="" sourceRef="StartNode_3580" targetRef="TaskNode_6211" /><bpmn:sequenceFlow id="Flow_6384" name="" sourceRef="TaskNode_6211" targetRef="GatewayNode_1499" /><bpmn:sequenceFlow id="Flow_7851" name="" sourceRef="GatewayNode_1499" targetRef="TaskNode_2937" /><bpmn:sequenceFlow id="Flow_8178" name="" sourceRef="TaskNode_2937" targetRef="TaskNode_4574" /><bpmn:sequenceFlow id="Flow_7414" name="" sourceRef="TaskNode_4574" targetRef="GatewayNode_3212" /><bpmn:sequenceFlow id="Flow_9411" name="" sourceRef="GatewayNode_3212" targetRef="TaskNode_7043" /><bpmn:sequenceFlow id="Flow_9743" name="" sourceRef="TaskNode_7043" targetRef="EndNode_7805" /></bpmn:process><bpmndi:BPMNDiagram id="BPMNDiagram_1"><bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_100"><bpmndi:BPMNShape id="BPMNShape_7yuc9ji_di" bpmnElement="StartNode_3580"><dc:Bounds height="36" width="36" x="240" y="180" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_oey5562_di" bpmnElement="TaskNode_6211"><dc:Bounds height="80" width="100" x="356" y="158" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_urgn28o_di" bpmnElement="GatewayNode_1499"><dc:Bounds height="36" width="36" x="536" y="180" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_tlik70f_di" bpmnElement="TaskNode_2937"><dc:Bounds height="80" width="100" x="652" y="158" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_x9jsya2_di" bpmnElement="TaskNode_4574"><dc:Bounds height="80" width="100" x="832" y="158" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_4ly39w6_di" bpmnElement="GatewayNode_3212"><dc:Bounds height="36" width="36" x="1012" y="180" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_pjf7zl6_di" bpmnElement="TaskNode_7043"><dc:Bounds height="80" width="100" x="1128" y="158" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_8gvk8u6_di" bpmnElement="EndNode_7805"><dc:Bounds height="36" width="36" x="1308" y="180" /></bpmndi:BPMNShape><bpmndi:BPMNEdge id="Flow_4473_di" bpmnElement="Flow_4473"><di:waypoint x="276" y="198" /><di:waypoint x="356" y="198" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_6384_di" bpmnElement="Flow_6384"><di:waypoint x="456" y="198" /><di:waypoint x="536" y="198" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_7851_di" bpmnElement="Flow_7851"><di:waypoint x="572" y="198" /><di:waypoint x="652" y="198" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_8178_di" bpmnElement="Flow_8178"><di:waypoint x="752" y="198" /><di:waypoint x="832" y="198" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_7414_di" bpmnElement="Flow_7414"><di:waypoint x="932" y="198" /><di:waypoint x="1012" y="198" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_9411_di" bpmnElement="Flow_9411"><di:waypoint x="1048" y="198" /><di:waypoint x="1128" y="198" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_9743_di" bpmnElement="Flow_9743"><di:waypoint x="1228" y="198" /><di:waypoint x="1308" y="198" /></bpmndi:BPMNEdge></bpmndi:BPMNPlane></bpmndi:BPMNDiagram></bpmn:definitions>', 0, NULL, NULL, 0, NULL, CAST(0x0000B28C00B4B941 AS DateTime), NULL)
INSERT [dbo].[WfProcess] ([ID], [ProcessID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1591, N'Process_tm2a_6824', N'1', N'AskforLeave_6824', N'AskforLeave_Code_6824', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="utf-8"?><bpmn:definitions targetNamespace="http://bpmn.io/schema/bpmn" id="bpmn-diagram" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL"><bpmn:process id="Process_tm2a_6824" name="AskforLeave_6824" isExecutable="true" sf:code="AskforLeave_Code_6824" sf:version="1"><bpmn:startEvent id="StartNode_3323" name="start" sf:code="Start"><bpmn:outgoing>Flow_8265</bpmn:outgoing></bpmn:startEvent><bpmn:task id="TaskNode_8080" name="Apply Submit" sf:code="task001"><bpmn:incoming>Flow_8265</bpmn:incoming><bpmn:outgoing>Flow_1781</bpmn:outgoing></bpmn:task><bpmn:exclusiveGateway id="GatewayNode_3072" name="XOr-Split" sf:code="xorsplit001"><bpmn:incoming>Flow_1781</bpmn:incoming><bpmn:outgoing>Flow_1498</bpmn:outgoing><bpmn:outgoing>Flow_7644</bpmn:outgoing></bpmn:exclusiveGateway><bpmn:task id="TaskNode_5606" name="Dept Manager Approval" sf:code="task010"><bpmn:incoming>Flow_1498</bpmn:incoming><bpmn:outgoing>Flow_4826</bpmn:outgoing></bpmn:task><bpmn:task id="TaskNode_9917" name="CEO Approval" sf:code="task020"><bpmn:incoming>Flow_7644</bpmn:incoming><bpmn:outgoing>Flow_5512</bpmn:outgoing></bpmn:task><bpmn:exclusiveGateway id="GatewayNode_8716" name="XOr-Join" sf:code="xorjoin001"><bpmn:incoming>Flow_5512</bpmn:incoming><bpmn:incoming>Flow_4826</bpmn:incoming><bpmn:outgoing>Flow_1505</bpmn:outgoing></bpmn:exclusiveGateway><bpmn:task id="TaskNode_6985" name="HR Approval" sf:code="task100"><bpmn:incoming>Flow_1505</bpmn:incoming><bpmn:outgoing>Flow_7641</bpmn:outgoing></bpmn:task><bpmn:endEvent id="EndNode_3614" name="end" sf:code="End"><bpmn:incoming>Flow_7641</bpmn:incoming></bpmn:endEvent><bpmn:sequenceFlow id="Flow_8265" name="" sourceRef="StartNode_3323" targetRef="TaskNode_8080" /><bpmn:sequenceFlow id="Flow_1781" name="" sourceRef="TaskNode_8080" targetRef="GatewayNode_3072" /><bpmn:sequenceFlow id="Flow_1498" name="days&lt;3" sourceRef="GatewayNode_3072" targetRef="TaskNode_5606"><bpmn:conditionExpression>days&lt;3</bpmn:conditionExpression></bpmn:sequenceFlow><bpmn:sequenceFlow id="Flow_7644" name="days&gt;=3" sourceRef="GatewayNode_3072" targetRef="TaskNode_9917"><bpmn:conditionExpression>days&gt;=3</bpmn:conditionExpression></bpmn:sequenceFlow><bpmn:sequenceFlow id="Flow_5512" name="" sourceRef="TaskNode_9917" targetRef="GatewayNode_8716" /><bpmn:sequenceFlow id="Flow_4826" name="" sourceRef="TaskNode_5606" targetRef="GatewayNode_8716" /><bpmn:sequenceFlow id="Flow_1505" name="" sourceRef="GatewayNode_8716" targetRef="TaskNode_6985" /><bpmn:sequenceFlow id="Flow_7641" name="" sourceRef="TaskNode_6985" targetRef="EndNode_3614" /></bpmn:process><bpmndi:BPMNDiagram id="BPMNDiagram_1"><bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_100"><bpmndi:BPMNShape id="BPMNShape_dhkrq8d_di" bpmnElement="StartNode_3323"><dc:Bounds height="36" width="36" x="240" y="180" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_uzho8a9_di" bpmnElement="TaskNode_8080"><dc:Bounds height="80" width="100" x="356" y="158" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_47d7klh_di" bpmnElement="GatewayNode_3072"><dc:Bounds height="36" width="36" x="536" y="180" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_zkgjl97_di" bpmnElement="TaskNode_5606"><dc:Bounds height="80" width="100" x="652" y="230" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_xnxabyf_di" bpmnElement="TaskNode_9917"><dc:Bounds height="80" width="100" x="652" y="70" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_clcfmkw_di" bpmnElement="GatewayNode_8716"><dc:Bounds height="36" width="36" x="832" y="180" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_jkr38n6_di" bpmnElement="TaskNode_6985"><dc:Bounds height="80" width="100" x="948" y="158" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_cf11jmq_di" bpmnElement="EndNode_3614"><dc:Bounds height="36" width="36" x="1128" y="180" /></bpmndi:BPMNShape><bpmndi:BPMNEdge id="Flow_8265_di" bpmnElement="Flow_8265"><di:waypoint x="276" y="198" /><di:waypoint x="356" y="198" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_1781_di" bpmnElement="Flow_1781"><di:waypoint x="456" y="198" /><di:waypoint x="536" y="198" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_1498_di" bpmnElement="Flow_1498"><di:waypoint x="554" y="216" /><di:waypoint x="554" y="270" /><di:waypoint x="652" y="270" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_7644_di" bpmnElement="Flow_7644"><di:waypoint x="554" y="216" /><di:waypoint x="554" y="110" /><di:waypoint x="652" y="110" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_5512_di" bpmnElement="Flow_5512"><di:waypoint x="752" y="110" /><di:waypoint x="850" y="110" /><di:waypoint x="850" y="180" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_4826_di" bpmnElement="Flow_4826"><di:waypoint x="752" y="270" /><di:waypoint x="850" y="270" /><di:waypoint x="850" y="216" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_1505_di" bpmnElement="Flow_1505"><di:waypoint x="868" y="198" /><di:waypoint x="948" y="198" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_7641_di" bpmnElement="Flow_7641"><di:waypoint x="1048" y="198" /><di:waypoint x="1128" y="198" /></bpmndi:BPMNEdge></bpmndi:BPMNPlane></bpmndi:BPMNDiagram></bpmn:definitions>', 0, NULL, NULL, 0, NULL, CAST(0x0000B28C00B4BFD3 AS DateTime), NULL)
INSERT [dbo].[WfProcess] ([ID], [ProcessID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1592, N'Process_cswy_5517', N'1', N'Complex_5517', N'Complex_Code_5517', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="utf-8"?><bpmn:definitions targetNamespace="http://bpmn.io/schema/bpmn" id="bpmn-diagram" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL"><bpmn:process id="Process_cswy_5517" name="Complex_5517" isExecutable="true" sf:code="Complex_Code_5517" sf:version="1"><bpmn:startEvent id="StartNode_6553" name="start" sf:code="Start"><bpmn:outgoing>Flow_1308</bpmn:outgoing></bpmn:startEvent><bpmn:task id="TaskNode_6976" name="Task-001" sf:code="task001"><bpmn:incoming>Flow_1308</bpmn:incoming><bpmn:outgoing>Flow_7557</bpmn:outgoing></bpmn:task><bpmn:inclusiveGateway id="GatewayNode_1841" name="or-split" sf:code="orsplit001"><bpmn:incoming>Flow_7557</bpmn:incoming><bpmn:outgoing>Flow_9736</bpmn:outgoing><bpmn:outgoing>Flow_3721</bpmn:outgoing><bpmn:outgoing>Flow_5294</bpmn:outgoing></bpmn:inclusiveGateway><bpmn:task id="TaskNode_4114" name="task-010" sf:code="task010"><bpmn:incoming>Flow_9736</bpmn:incoming><bpmn:outgoing>Flow_3553</bpmn:outgoing></bpmn:task><bpmn:task id="MultiSignNode_3996" name="MI-011" sf:code="mi011"><bpmn:incoming>Flow_3553</bpmn:incoming><bpmn:outgoing>Flow_5802</bpmn:outgoing><bpmn:multiInstanceLoopCharacteristics /></bpmn:task><bpmn:task id="TaskNode_9595" name="task-020" sf:code="task020"><bpmn:incoming>Flow_3721</bpmn:incoming><bpmn:outgoing>Flow_6054</bpmn:outgoing></bpmn:task><bpmn:subProcess id="SubProcessNode_9554" name="subname021" sf:code="subcode021"><bpmn:incoming>Flow_6054</bpmn:incoming><bpmn:outgoing>Flow_3999</bpmn:outgoing></bpmn:subProcess><bpmn:task id="TaskNode_7635" name="task-030" sf:code="task030"><bpmn:incoming>Flow_5294</bpmn:incoming><bpmn:outgoing>Flow_3695</bpmn:outgoing></bpmn:task><bpmn:task id="TaskNode_8726" name="task-031" sf:code="task031"><bpmn:incoming>Flow_3695</bpmn:incoming><bpmn:outgoing>Flow_8426</bpmn:outgoing></bpmn:task><bpmn:inclusiveGateway id="GatewayNode_5502" name="or-join" sf:code="orjoin001"><bpmn:incoming>Flow_8426</bpmn:incoming><bpmn:incoming>Flow_3999</bpmn:incoming><bpmn:incoming>Flow_5802</bpmn:incoming><bpmn:outgoing>Flow_6240</bpmn:outgoing></bpmn:inclusiveGateway><bpmn:task id="TaskNode_7814" name="task-100" sf:code="task100"><bpmn:incoming>Flow_6240</bpmn:incoming><bpmn:outgoing>Flow_7223</bpmn:outgoing></bpmn:task><bpmn:endEvent id="EndNode_5840" name="end" sf:code="End"><bpmn:incoming>Flow_7223</bpmn:incoming></bpmn:endEvent><bpmn:sequenceFlow id="Flow_1308" name="" sourceRef="StartNode_6553" targetRef="TaskNode_6976" /><bpmn:sequenceFlow id="Flow_7557" name="" sourceRef="TaskNode_6976" targetRef="GatewayNode_1841" /><bpmn:sequenceFlow id="Flow_9736" name="" sourceRef="GatewayNode_1841" targetRef="TaskNode_4114" /><bpmn:sequenceFlow id="Flow_3553" name="" sourceRef="TaskNode_4114" targetRef="MultiSignNode_3996" /><bpmn:sequenceFlow id="Flow_3721" name="" sourceRef="GatewayNode_1841" targetRef="TaskNode_9595" /><bpmn:sequenceFlow id="Flow_6054" name="" sourceRef="TaskNode_9595" targetRef="SubProcessNode_9554" /><bpmn:sequenceFlow id="Flow_5294" name="" sourceRef="GatewayNode_1841" targetRef="TaskNode_7635" /><bpmn:sequenceFlow id="Flow_3695" name="" sourceRef="TaskNode_7635" targetRef="TaskNode_8726" /><bpmn:sequenceFlow id="Flow_8426" name="" sourceRef="TaskNode_8726" targetRef="GatewayNode_5502" /><bpmn:sequenceFlow id="Flow_3999" name="" sourceRef="SubProcessNode_9554" targetRef="GatewayNode_5502" /><bpmn:sequenceFlow id="Flow_5802" name="" sourceRef="MultiSignNode_3996" targetRef="GatewayNode_5502" /><bpmn:sequenceFlow id="Flow_6240" name="" sourceRef="GatewayNode_5502" targetRef="TaskNode_7814" /><bpmn:sequenceFlow id="Flow_7223" name="" sourceRef="TaskNode_7814" targetRef="EndNode_5840" /></bpmn:process><bpmndi:BPMNDiagram id="BPMNDiagram_1"><bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_100"><bpmndi:BPMNShape id="BPMNShape_gckr7sq_di" bpmnElement="StartNode_6553"><dc:Bounds height="36" width="36" x="240" y="180" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_fufzt6k_di" bpmnElement="TaskNode_6976"><dc:Bounds height="80" width="100" x="356" y="158" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_tddjhrw_di" bpmnElement="GatewayNode_1841"><dc:Bounds height="36" width="36" x="536" y="180" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_etakvfs_di" bpmnElement="TaskNode_4114"><dc:Bounds height="80" width="100" x="652" y="390" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_4om8yoo_di" bpmnElement="MultiSignNode_3996"><dc:Bounds height="80" width="100" x="832" y="390" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_8gl0evs_di" bpmnElement="TaskNode_9595"><dc:Bounds height="80" width="100" x="652" y="230" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_99h9wkk_di" bpmnElement="SubProcessNode_9554"><dc:Bounds height="80" width="100" x="832" y="230" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_rzeuwqq_di" bpmnElement="TaskNode_7635"><dc:Bounds height="80" width="100" x="652" y="70" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_1cjwyfq_di" bpmnElement="TaskNode_8726"><dc:Bounds height="80" width="100" x="832" y="70" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_i7agaq7_di" bpmnElement="GatewayNode_5502"><dc:Bounds height="36" width="36" x="1012" y="180" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_hfwz8dt_di" bpmnElement="TaskNode_7814"><dc:Bounds height="80" width="100" x="1128" y="158" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_fv4716y_di" bpmnElement="EndNode_5840"><dc:Bounds height="36" width="36" x="1308" y="180" /></bpmndi:BPMNShape><bpmndi:BPMNEdge id="Flow_1308_di" bpmnElement="Flow_1308"><di:waypoint x="276" y="198" /><di:waypoint x="356" y="198" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_7557_di" bpmnElement="Flow_7557"><di:waypoint x="456" y="198" /><di:waypoint x="536" y="198" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_9736_di" bpmnElement="Flow_9736"><di:waypoint x="554" y="216" /><di:waypoint x="554" y="430" /><di:waypoint x="652" y="430" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_3553_di" bpmnElement="Flow_3553"><di:waypoint x="752" y="430" /><di:waypoint x="832" y="430" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_3721_di" bpmnElement="Flow_3721"><di:waypoint x="554" y="216" /><di:waypoint x="554" y="270" /><di:waypoint x="652" y="270" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_6054_di" bpmnElement="Flow_6054"><di:waypoint x="752" y="270" /><di:waypoint x="832" y="270" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_5294_di" bpmnElement="Flow_5294"><di:waypoint x="554" y="180" /><di:waypoint x="554" y="110" /><di:waypoint x="652" y="110" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_3695_di" bpmnElement="Flow_3695"><di:waypoint x="752" y="110" /><di:waypoint x="832" y="110" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_8426_di" bpmnElement="Flow_8426"><di:waypoint x="932" y="110" /><di:waypoint x="1030" y="110" /><di:waypoint x="1030" y="180" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_3999_di" bpmnElement="Flow_3999"><di:waypoint x="932" y="270" /><di:waypoint x="1030" y="270" /><di:waypoint x="1030" y="216" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_5802_di" bpmnElement="Flow_5802"><di:waypoint x="932" y="430" /><di:waypoint x="1030" y="430" /><di:waypoint x="1030" y="216" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_6240_di" bpmnElement="Flow_6240"><di:waypoint x="1048" y="198" /><di:waypoint x="1128" y="198" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_7223_di" bpmnElement="Flow_7223"><di:waypoint x="1228" y="198" /><di:waypoint x="1308" y="198" /></bpmndi:BPMNEdge></bpmndi:BPMNPlane></bpmndi:BPMNDiagram><bpmndi:BPMNDiagram id="BPMNDiagram_4jgbt1v"><bpmndi:BPMNPlane id="BPMNPlane_zax4gdb" bpmnElement="SubProcessNode_9554" /></bpmndi:BPMNDiagram></bpmn:definitions>', 0, NULL, NULL, 0, NULL, CAST(0x0000B28C00B4D22E AS DateTime), NULL)
INSERT [dbo].[WfProcess] ([ID], [ProcessID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1593, N'Process_93v0_7704', N'1', N'Warehousing_7704', N'Warehousing_Code_7704', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="utf-8"?><bpmn:definitions targetNamespace="http://bpmn.io/schema/bpmn" id="bpmn-diagram" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL"><bpmn:process id="Process_93v0_7704" name="Warehousing_7704" isExecutable="true" sf:code="Warehousing_Code_7704" sf:version="1"><bpmn:startEvent id="StartNode_6375" name="start" sf:code="Start"><bpmn:outgoing>Flow_3365</bpmn:outgoing></bpmn:startEvent><bpmn:task id="TaskNode_2544" name="Warehouse Signature" sf:code="task001"><bpmn:incoming>Flow_3365</bpmn:incoming><bpmn:outgoing>Flow_5067</bpmn:outgoing></bpmn:task><bpmn:exclusiveGateway id="GatewayNode_6532" name="And-Split" sf:code="andsplit001"><bpmn:incoming>Flow_5067</bpmn:incoming><bpmn:outgoing>Flow_2058</bpmn:outgoing><bpmn:outgoing>Flow_3938</bpmn:outgoing></bpmn:exclusiveGateway><bpmn:task id="TaskNode_4706" name="Signature of the Comprehensive Department" sf:code="task010"><bpmn:incoming>Flow_2058</bpmn:incoming><bpmn:outgoing>Flow_5153</bpmn:outgoing></bpmn:task><bpmn:task id="TaskNode_7908" name="Financial Signature" sf:code="task020"><bpmn:incoming>Flow_3938</bpmn:incoming><bpmn:outgoing>Flow_3234</bpmn:outgoing></bpmn:task><bpmn:parallelGateway id="GatewayNode_3589" name="And-Join" sf:code="andjoin001"><bpmn:incoming>Flow_3234</bpmn:incoming><bpmn:incoming>Flow_5153</bpmn:incoming><bpmn:outgoing>Flow_6052</bpmn:outgoing></bpmn:parallelGateway><bpmn:task id="TaskNode_8774" name="CEO Signature" sf:code="task007"><bpmn:incoming>Flow_6052</bpmn:incoming><bpmn:outgoing>Flow_9098</bpmn:outgoing></bpmn:task><bpmn:endEvent id="EndNode_2138" name="end" sf:code="End"><bpmn:incoming>Flow_9098</bpmn:incoming></bpmn:endEvent><bpmn:sequenceFlow id="Flow_3365" name="" sourceRef="StartNode_6375" targetRef="TaskNode_2544" /><bpmn:sequenceFlow id="Flow_5067" name="" sourceRef="TaskNode_2544" targetRef="GatewayNode_6532" /><bpmn:sequenceFlow id="Flow_2058" name="" sourceRef="GatewayNode_6532" targetRef="TaskNode_4706" /><bpmn:sequenceFlow id="Flow_3938" name="" sourceRef="GatewayNode_6532" targetRef="TaskNode_7908" /><bpmn:sequenceFlow id="Flow_3234" name="" sourceRef="TaskNode_7908" targetRef="GatewayNode_3589" /><bpmn:sequenceFlow id="Flow_5153" name="" sourceRef="TaskNode_4706" targetRef="GatewayNode_3589" /><bpmn:sequenceFlow id="Flow_6052" name="" sourceRef="GatewayNode_3589" targetRef="TaskNode_8774" /><bpmn:sequenceFlow id="Flow_9098" name="" sourceRef="TaskNode_8774" targetRef="EndNode_2138" /></bpmn:process><bpmndi:BPMNDiagram id="BPMNDiagram_1"><bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_100"><bpmndi:BPMNShape id="BPMNShape_no9uwha_di" bpmnElement="StartNode_6375"><dc:Bounds height="36" width="36" x="240" y="180" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_hf6j4dj_di" bpmnElement="TaskNode_2544"><dc:Bounds height="80" width="100" x="356" y="158" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_yuufbxp_di" bpmnElement="GatewayNode_6532"><dc:Bounds height="36" width="36" x="536" y="180" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_ospzefl_di" bpmnElement="TaskNode_4706"><dc:Bounds height="80" width="100" x="652" y="230" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_z211irs_di" bpmnElement="TaskNode_7908"><dc:Bounds height="80" width="100" x="652" y="70" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_1h4ytnc_di" bpmnElement="GatewayNode_3589"><dc:Bounds height="36" width="36" x="832" y="180" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_62s88z5_di" bpmnElement="TaskNode_8774"><dc:Bounds height="80" width="100" x="948" y="158" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_gk61iqf_di" bpmnElement="EndNode_2138"><dc:Bounds height="36" width="36" x="1128" y="180" /></bpmndi:BPMNShape><bpmndi:BPMNEdge id="Flow_3365_di" bpmnElement="Flow_3365"><di:waypoint x="276" y="198" /><di:waypoint x="356" y="198" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_5067_di" bpmnElement="Flow_5067"><di:waypoint x="456" y="198" /><di:waypoint x="536" y="198" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_2058_di" bpmnElement="Flow_2058"><di:waypoint x="554" y="216" /><di:waypoint x="554" y="270" /><di:waypoint x="652" y="270" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_3938_di" bpmnElement="Flow_3938"><di:waypoint x="554" y="216" /><di:waypoint x="554" y="110" /><di:waypoint x="652" y="110" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_3234_di" bpmnElement="Flow_3234"><di:waypoint x="752" y="110" /><di:waypoint x="850" y="110" /><di:waypoint x="850" y="180" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_5153_di" bpmnElement="Flow_5153"><di:waypoint x="752" y="270" /><di:waypoint x="850" y="270" /><di:waypoint x="850" y="216" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_6052_di" bpmnElement="Flow_6052"><di:waypoint x="868" y="198" /><di:waypoint x="948" y="198" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_9098_di" bpmnElement="Flow_9098"><di:waypoint x="1048" y="198" /><di:waypoint x="1128" y="198" /></bpmndi:BPMNEdge></bpmndi:BPMNPlane></bpmndi:BPMNDiagram></bpmn:definitions>', 0, NULL, NULL, 0, NULL, CAST(0x0000B28C00B4DBA4 AS DateTime), NULL)
INSERT [dbo].[WfProcess] ([ID], [ProcessID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1594, N'Process_g1uv_7492', N'1', N'OfficeIn_7492', N'OfficeIn_Code_7492', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="utf-8"?><bpmn:definitions targetNamespace="http://bpmn.io/schema/bpmn" id="bpmn-diagram" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL"><bpmn:process id="Process_g1uv_7492" name="OfficeIn_7492" isExecutable="true" sf:code="OfficeIn_Code_7492" sf:version="1"><bpmn:startEvent id="StartNode_8245" name="start" sf:code="Start"><bpmn:outgoing>Flow_4856</bpmn:outgoing></bpmn:startEvent><bpmn:task id="TaskNode_2261" name="Warehouse Signature" sf:code="task001"><bpmn:incoming>Flow_4856</bpmn:incoming><bpmn:outgoing>Flow_5250</bpmn:outgoing></bpmn:task><bpmn:inclusiveGateway id="GatewayNode_9177" name="Or-Split" sf:code="orsplit001"><bpmn:incoming>Flow_5250</bpmn:incoming><bpmn:outgoing>Flow_3812</bpmn:outgoing><bpmn:outgoing>Flow_8970</bpmn:outgoing><bpmn:outgoing>Flow_6852</bpmn:outgoing></bpmn:inclusiveGateway><bpmn:task id="TaskNode_1268" name="Signature of Administrative Department" sf:code="task010"><bpmn:incoming>Flow_3812</bpmn:incoming><bpmn:outgoing>Flow_2948</bpmn:outgoing></bpmn:task><bpmn:task id="TaskNode_8098" name="Financial Signature" sf:code="task020"><bpmn:incoming>Flow_8970</bpmn:incoming><bpmn:outgoing>Flow_7753</bpmn:outgoing></bpmn:task><bpmn:task id="TaskNode_3015" name="CEO Signature" sf:code="task030"><bpmn:incoming>Flow_6852</bpmn:incoming><bpmn:outgoing>Flow_8806</bpmn:outgoing></bpmn:task><bpmn:inclusiveGateway id="GatewayNode_9899" name="Or-Join" sf:code="orjoin001"><bpmn:incoming>Flow_8806</bpmn:incoming><bpmn:incoming>Flow_7753</bpmn:incoming><bpmn:incoming>Flow_2948</bpmn:incoming><bpmn:outgoing>Flow_6987</bpmn:outgoing></bpmn:inclusiveGateway><bpmn:task id="TaskNode_7076" name="Finance Signature" sf:code="task007"><bpmn:incoming>Flow_6987</bpmn:incoming><bpmn:outgoing>Flow_1854</bpmn:outgoing></bpmn:task><bpmn:endEvent id="EndNode_2736" name="end" sf:code="End"><bpmn:incoming>Flow_1854</bpmn:incoming></bpmn:endEvent><bpmn:sequenceFlow id="Flow_4856" name="" sourceRef="StartNode_8245" targetRef="TaskNode_2261" /><bpmn:sequenceFlow id="Flow_5250" name="" sourceRef="TaskNode_2261" targetRef="GatewayNode_9177" /><bpmn:sequenceFlow id="Flow_3812" name="surplus = &quot;normal&quot;" sourceRef="GatewayNode_9177" targetRef="TaskNode_1268"><bpmn:conditionExpression>surplus = "normal"</bpmn:conditionExpression></bpmn:sequenceFlow><bpmn:sequenceFlow id="Flow_8970" name="surplus = &quot;normal&quot;" sourceRef="GatewayNode_9177" targetRef="TaskNode_8098"><bpmn:conditionExpression>surplus = "normal"</bpmn:conditionExpression></bpmn:sequenceFlow><bpmn:sequenceFlow id="Flow_6852" name="surplus = &quot;overamount&quot;" sourceRef="GatewayNode_9177" targetRef="TaskNode_3015"><bpmn:conditionExpression>surplus = "overamount"</bpmn:conditionExpression></bpmn:sequenceFlow><bpmn:sequenceFlow id="Flow_8806" name="" sourceRef="TaskNode_3015" targetRef="GatewayNode_9899" /><bpmn:sequenceFlow id="Flow_7753" name="" sourceRef="TaskNode_8098" targetRef="GatewayNode_9899" /><bpmn:sequenceFlow id="Flow_2948" name="" sourceRef="TaskNode_1268" targetRef="GatewayNode_9899" /><bpmn:sequenceFlow id="Flow_6987" name="" sourceRef="GatewayNode_9899" targetRef="TaskNode_7076" /><bpmn:sequenceFlow id="Flow_1854" name="" sourceRef="TaskNode_7076" targetRef="EndNode_2736" /></bpmn:process><bpmndi:BPMNDiagram id="BPMNDiagram_1"><bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_100"><bpmndi:BPMNShape id="BPMNShape_glgd19p_di" bpmnElement="StartNode_8245"><dc:Bounds height="36" width="36" x="240" y="180" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_kkjl3oz_di" bpmnElement="TaskNode_2261"><dc:Bounds height="80" width="100" x="356" y="158" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_0qd7v0r_di" bpmnElement="GatewayNode_9177"><dc:Bounds height="36" width="36" x="536" y="180" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_zkdaoxu_di" bpmnElement="TaskNode_1268"><dc:Bounds height="80" width="100" x="652" y="390" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_j7chlpc_di" bpmnElement="TaskNode_8098"><dc:Bounds height="80" width="100" x="652" y="230" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_atq1msy_di" bpmnElement="TaskNode_3015"><dc:Bounds height="80" width="100" x="652" y="70" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_hdri6ey_di" bpmnElement="GatewayNode_9899"><dc:Bounds height="36" width="36" x="832" y="180" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_kwviwhb_di" bpmnElement="TaskNode_7076"><dc:Bounds height="80" width="100" x="948" y="158" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_x69zdun_di" bpmnElement="EndNode_2736"><dc:Bounds height="36" width="36" x="1128" y="180" /></bpmndi:BPMNShape><bpmndi:BPMNEdge id="Flow_4856_di" bpmnElement="Flow_4856"><di:waypoint x="276" y="198" /><di:waypoint x="356" y="198" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_5250_di" bpmnElement="Flow_5250"><di:waypoint x="456" y="198" /><di:waypoint x="536" y="198" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_3812_di" bpmnElement="Flow_3812"><di:waypoint x="554" y="216" /><di:waypoint x="554" y="430" /><di:waypoint x="652" y="430" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_8970_di" bpmnElement="Flow_8970"><di:waypoint x="554" y="216" /><di:waypoint x="554" y="270" /><di:waypoint x="652" y="270" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_6852_di" bpmnElement="Flow_6852"><di:waypoint x="554" y="180" /><di:waypoint x="554" y="110" /><di:waypoint x="652" y="110" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_8806_di" bpmnElement="Flow_8806"><di:waypoint x="752" y="110" /><di:waypoint x="850" y="110" /><di:waypoint x="850" y="180" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_7753_di" bpmnElement="Flow_7753"><di:waypoint x="752" y="270" /><di:waypoint x="850" y="270" /><di:waypoint x="850" y="216" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_2948_di" bpmnElement="Flow_2948"><di:waypoint x="752" y="430" /><di:waypoint x="850" y="430" /><di:waypoint x="850" y="216" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_6987_di" bpmnElement="Flow_6987"><di:waypoint x="868" y="198" /><di:waypoint x="948" y="198" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_1854_di" bpmnElement="Flow_1854"><di:waypoint x="1048" y="198" /><di:waypoint x="1128" y="198" /></bpmndi:BPMNEdge></bpmndi:BPMNPlane></bpmndi:BPMNDiagram></bpmn:definitions>', 0, NULL, NULL, 0, NULL, CAST(0x0000B28C00B4E1F8 AS DateTime), NULL)
INSERT [dbo].[WfProcess] ([ID], [ProcessID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1595, N'Process_b58k_7184', N'1', N'Contract_7184', N'Contract_Code_7184', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="utf-8"?><bpmn:definitions targetNamespace="http://bpmn.io/schema/bpmn" id="bpmn-diagram" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL"><bpmn:process id="Process_b58k_7184" name="Contract_7184" isExecutable="true" sf:code="Contract_Code_7184" sf:version="1"><bpmn:startEvent id="StartNode_5058" name="start" sf:code="Start"><bpmn:outgoing>Flow_8916</bpmn:outgoing></bpmn:startEvent><bpmn:task id="TaskNode_1298" name="Contract Draft" sf:code="task001"><bpmn:incoming>Flow_8916</bpmn:incoming><bpmn:outgoing>Flow_5803</bpmn:outgoing></bpmn:task><bpmn:task id="TaskNode_2891" name="Approved by BA Manager" sf:code="task002"><bpmn:incoming>Flow_5803</bpmn:incoming><bpmn:outgoing>Flow_4693</bpmn:outgoing></bpmn:task><bpmn:parallelGateway id="GatewayNode_5026" name="And-Split" sf:code="andsplit001"><bpmn:incoming>Flow_4693</bpmn:incoming><bpmn:outgoing>Flow_4859</bpmn:outgoing><bpmn:outgoing>Flow_3546</bpmn:outgoing><bpmn:outgoing>Flow_5282</bpmn:outgoing></bpmn:parallelGateway><bpmn:task id="TaskNode_3042" name="Contract Department Review" sf:code="task010"><bpmn:incoming>Flow_4859</bpmn:incoming><bpmn:outgoing>Flow_4575</bpmn:outgoing></bpmn:task><bpmn:task id="TaskNode_9813" name="Financial Department Review" sf:code="task020"><bpmn:incoming>Flow_3546</bpmn:incoming><bpmn:outgoing>Flow_8128</bpmn:outgoing></bpmn:task><bpmn:task id="TaskNode_6006" name="Group Headquarters Review" sf:code="task030"><bpmn:incoming>Flow_5282</bpmn:incoming><bpmn:outgoing>Flow_3759</bpmn:outgoing></bpmn:task><bpmn:parallelGateway id="GatewayNode_7284" name="And-Join" sf:code="andjoin001"><bpmn:incoming>Flow_3759</bpmn:incoming><bpmn:incoming>Flow_8128</bpmn:incoming><bpmn:incoming>Flow_4575</bpmn:incoming><bpmn:outgoing>Flow_2750</bpmn:outgoing></bpmn:parallelGateway><bpmn:task id="TaskNode_2503" name="Contract Archived" sf:code="task007"><bpmn:incoming>Flow_2750</bpmn:incoming><bpmn:outgoing>Flow_5578</bpmn:outgoing></bpmn:task><bpmn:endEvent id="EndNode_7655" name="end" sf:code="End"><bpmn:incoming>Flow_5578</bpmn:incoming></bpmn:endEvent><bpmn:sequenceFlow id="Flow_8916" name="" sourceRef="StartNode_5058" targetRef="TaskNode_1298" /><bpmn:sequenceFlow id="Flow_5803" name="" sourceRef="TaskNode_1298" targetRef="TaskNode_2891" /><bpmn:sequenceFlow id="Flow_4693" name="" sourceRef="TaskNode_2891" targetRef="GatewayNode_5026" /><bpmn:sequenceFlow id="Flow_4859" name="" sourceRef="GatewayNode_5026" targetRef="TaskNode_3042" /><bpmn:sequenceFlow id="Flow_3546" name="" sourceRef="GatewayNode_5026" targetRef="TaskNode_9813" /><bpmn:sequenceFlow id="Flow_5282" name="" sourceRef="GatewayNode_5026" targetRef="TaskNode_6006" /><bpmn:sequenceFlow id="Flow_3759" name="" sourceRef="TaskNode_6006" targetRef="GatewayNode_7284" /><bpmn:sequenceFlow id="Flow_8128" name="" sourceRef="TaskNode_9813" targetRef="GatewayNode_7284" /><bpmn:sequenceFlow id="Flow_4575" name="" sourceRef="TaskNode_3042" targetRef="GatewayNode_7284" /><bpmn:sequenceFlow id="Flow_2750" name="" sourceRef="GatewayNode_7284" targetRef="TaskNode_2503" /><bpmn:sequenceFlow id="Flow_5578" name="" sourceRef="TaskNode_2503" targetRef="EndNode_7655" /></bpmn:process><bpmndi:BPMNDiagram id="BPMNDiagram_1"><bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_100"><bpmndi:BPMNShape id="BPMNShape_8pdmj33_di" bpmnElement="StartNode_5058"><dc:Bounds height="36" width="36" x="240" y="180" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_o5z4i8q_di" bpmnElement="TaskNode_1298"><dc:Bounds height="80" width="100" x="356" y="158" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_4l0743w_di" bpmnElement="TaskNode_2891"><dc:Bounds height="80" width="100" x="536" y="158" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_q32xg9g_di" bpmnElement="GatewayNode_5026"><dc:Bounds height="36" width="36" x="716" y="180" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_f2x8q3l_di" bpmnElement="TaskNode_3042"><dc:Bounds height="80" width="100" x="832" y="390" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_rthxjav_di" bpmnElement="TaskNode_9813"><dc:Bounds height="80" width="100" x="832" y="230" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_fgo7fp4_di" bpmnElement="TaskNode_6006"><dc:Bounds height="80" width="100" x="832" y="70" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_7dttfi7_di" bpmnElement="GatewayNode_7284"><dc:Bounds height="36" width="36" x="1012" y="180" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_3gycbit_di" bpmnElement="TaskNode_2503"><dc:Bounds height="80" width="100" x="1128" y="158" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_bbmg7av_di" bpmnElement="EndNode_7655"><dc:Bounds height="36" width="36" x="1308" y="180" /></bpmndi:BPMNShape><bpmndi:BPMNEdge id="Flow_8916_di" bpmnElement="Flow_8916"><di:waypoint x="276" y="198" /><di:waypoint x="356" y="198" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_5803_di" bpmnElement="Flow_5803"><di:waypoint x="456" y="198" /><di:waypoint x="536" y="198" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_4693_di" bpmnElement="Flow_4693"><di:waypoint x="636" y="198" /><di:waypoint x="716" y="198" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_4859_di" bpmnElement="Flow_4859"><di:waypoint x="734" y="216" /><di:waypoint x="734" y="430" /><di:waypoint x="832" y="430" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_3546_di" bpmnElement="Flow_3546"><di:waypoint x="734" y="216" /><di:waypoint x="734" y="270" /><di:waypoint x="832" y="270" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_5282_di" bpmnElement="Flow_5282"><di:waypoint x="734" y="180" /><di:waypoint x="734" y="110" /><di:waypoint x="832" y="110" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_3759_di" bpmnElement="Flow_3759"><di:waypoint x="932" y="110" /><di:waypoint x="1030" y="110" /><di:waypoint x="1030" y="180" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_8128_di" bpmnElement="Flow_8128"><di:waypoint x="932" y="270" /><di:waypoint x="1030" y="270" /><di:waypoint x="1030" y="216" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_4575_di" bpmnElement="Flow_4575"><di:waypoint x="932" y="430" /><di:waypoint x="1030" y="430" /><di:waypoint x="1030" y="216" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_2750_di" bpmnElement="Flow_2750"><di:waypoint x="1048" y="198" /><di:waypoint x="1128" y="198" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_5578_di" bpmnElement="Flow_5578"><di:waypoint x="1228" y="198" /><di:waypoint x="1308" y="198" /></bpmndi:BPMNEdge></bpmndi:BPMNPlane></bpmndi:BPMNDiagram></bpmn:definitions>', 0, NULL, NULL, 0, NULL, CAST(0x0000B28C00B4E810 AS DateTime), NULL)
INSERT [dbo].[WfProcess] ([ID], [ProcessID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1596, N'Process_y24l_2034', N'1', N'Reimbursement_2034', N'Reimbursement_Code_2034', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="utf-8"?><bpmn:definitions targetNamespace="http://bpmn.io/schema/bpmn" id="bpmn-diagram" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL"><bpmn:process id="Process_y24l_2034" name="Reimbursement_2034" isExecutable="true" sf:code="Reimbursement_Code_2034" sf:version="1"><bpmn:startEvent id="StartNode_4801" name="start" sf:code="Start"><bpmn:outgoing>Flow_5632</bpmn:outgoing></bpmn:startEvent><bpmn:task id="TaskNode_3686" name="Reimbursement Submit" sf:code="task001"><bpmn:incoming>Flow_5632</bpmn:incoming><bpmn:outgoing>Flow_2965</bpmn:outgoing></bpmn:task><bpmn:task id="TaskNode_4136" name="Finalcial Approval" sf:code="task002"><bpmn:incoming>Flow_2965</bpmn:incoming><bpmn:outgoing>Flow_7260</bpmn:outgoing></bpmn:task><bpmn:exclusiveGateway id="GatewayNode_8699" name="XOr-Split" sf:code="orsplit001"><bpmn:incoming>Flow_7260</bpmn:incoming><bpmn:outgoing>Flow_6225</bpmn:outgoing><bpmn:outgoing>Flow_9826</bpmn:outgoing></bpmn:exclusiveGateway><bpmn:task id="TaskNode_8945" name="Approved by the Director in Charge" sf:code="task010"><bpmn:incoming>Flow_6225</bpmn:incoming><bpmn:outgoing>Flow_9841</bpmn:outgoing></bpmn:task><bpmn:task id="TaskNode_4135" name="CEO Approval" sf:code="task020"><bpmn:incoming>Flow_9826</bpmn:incoming><bpmn:outgoing>Flow_7081</bpmn:outgoing></bpmn:task><bpmn:exclusiveGateway id="GatewayNode_5360" name="XOr-Join" sf:code="orjoin001"><bpmn:incoming>Flow_7081</bpmn:incoming><bpmn:incoming>Flow_9841</bpmn:incoming><bpmn:outgoing>Flow_3179</bpmn:outgoing></bpmn:exclusiveGateway><bpmn:endEvent id="EndNode_4889" name="end" sf:code="End"><bpmn:incoming>Flow_3179</bpmn:incoming></bpmn:endEvent><bpmn:sequenceFlow id="Flow_5632" name="" sourceRef="StartNode_4801" targetRef="TaskNode_3686" /><bpmn:sequenceFlow id="Flow_2965" name="" sourceRef="TaskNode_3686" targetRef="TaskNode_4136" /><bpmn:sequenceFlow id="Flow_7260" name="" sourceRef="TaskNode_4136" targetRef="GatewayNode_8699" /><bpmn:sequenceFlow id="Flow_6225" name="money&lt;10000" sourceRef="GatewayNode_8699" targetRef="TaskNode_8945"><bpmn:conditionExpression>money&lt;10000</bpmn:conditionExpression></bpmn:sequenceFlow><bpmn:sequenceFlow id="Flow_9826" name="money&gt;=10000" sourceRef="GatewayNode_8699" targetRef="TaskNode_4135"><bpmn:conditionExpression>money&gt;=10000</bpmn:conditionExpression></bpmn:sequenceFlow><bpmn:sequenceFlow id="Flow_7081" name="" sourceRef="TaskNode_4135" targetRef="GatewayNode_5360" /><bpmn:sequenceFlow id="Flow_9841" name="" sourceRef="TaskNode_8945" targetRef="GatewayNode_5360" /><bpmn:sequenceFlow id="Flow_3179" name="" sourceRef="GatewayNode_5360" targetRef="EndNode_4889" /></bpmn:process><bpmndi:BPMNDiagram id="BPMNDiagram_1"><bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_100"><bpmndi:BPMNShape id="BPMNShape_y8mjzmw_di" bpmnElement="StartNode_4801"><dc:Bounds height="36" width="36" x="240" y="180" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_ocjah5t_di" bpmnElement="TaskNode_3686"><dc:Bounds height="80" width="100" x="356" y="158" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_b4a4eiv_di" bpmnElement="TaskNode_4136"><dc:Bounds height="80" width="100" x="536" y="158" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_65ejbsh_di" bpmnElement="GatewayNode_8699"><dc:Bounds height="36" width="36" x="716" y="180" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_hzos6e4_di" bpmnElement="TaskNode_8945"><dc:Bounds height="80" width="100" x="832" y="230" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_6lu1omw_di" bpmnElement="TaskNode_4135"><dc:Bounds height="80" width="100" x="832" y="70" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_b9su5d9_di" bpmnElement="GatewayNode_5360"><dc:Bounds height="36" width="36" x="1012" y="180" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_k8p4zt6_di" bpmnElement="EndNode_4889"><dc:Bounds height="36" width="36" x="1128" y="180" /></bpmndi:BPMNShape><bpmndi:BPMNEdge id="Flow_5632_di" bpmnElement="Flow_5632"><di:waypoint x="276" y="198" /><di:waypoint x="356" y="198" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_2965_di" bpmnElement="Flow_2965"><di:waypoint x="456" y="198" /><di:waypoint x="536" y="198" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_7260_di" bpmnElement="Flow_7260"><di:waypoint x="636" y="198" /><di:waypoint x="716" y="198" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_6225_di" bpmnElement="Flow_6225"><di:waypoint x="734" y="216" /><di:waypoint x="734" y="270" /><di:waypoint x="832" y="270" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_9826_di" bpmnElement="Flow_9826"><di:waypoint x="734" y="216" /><di:waypoint x="734" y="110" /><di:waypoint x="832" y="110" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_7081_di" bpmnElement="Flow_7081"><di:waypoint x="932" y="110" /><di:waypoint x="1030" y="110" /><di:waypoint x="1030" y="180" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_9841_di" bpmnElement="Flow_9841"><di:waypoint x="932" y="270" /><di:waypoint x="1030" y="270" /><di:waypoint x="1030" y="216" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_3179_di" bpmnElement="Flow_3179"><di:waypoint x="1048" y="198" /><di:waypoint x="1128" y="198" /></bpmndi:BPMNEdge></bpmndi:BPMNPlane></bpmndi:BPMNDiagram></bpmn:definitions>', 0, NULL, NULL, 0, NULL, CAST(0x0000B28C00B4EDC8 AS DateTime), NULL)
INSERT [dbo].[WfProcess] ([ID], [ProcessID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1602, N'employeeLeaveRequestProcess_hwpb', N'1', N'员工请假流程', N'employeeLeaveRequestProcess_hwpb', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn:definitions xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL">
  <bpmn:process id="employeeLeaveRequestProcess_hwpb" name="员工请假流程" isExecutable="false">
    <bpmn:startEvent id="AstartEvent" name="流程开始" />
    <bpmn:userTask id="AsubmitApplication" name="提交请假申请" assignee="" />
    <bpmn:userTask id="AdepartmentManagerApproval" name="部门经理审批" assignee="" />
    <bpmn:serviceTask id="AHRConfirmation" name="HR确认" implementation="" />
    <bpmn:userTask id="AresultNotification" name="通知结果" assignee="" />
    <bpmn:userTask id="AapprovalRejected" name="审批驳回" assignee="" />
    <bpmn:endEvent id="AendEvent" name="流程结束" />
    <bpmn:sequenceFlow id="Flow_AstartEvent_AsubmitApplication" sourceRef="AstartEvent" targetRef="AsubmitApplication" />
    <bpmn:sequenceFlow id="Flow_AsubmitApplication_AdepartmentManagerApproval" sourceRef="AsubmitApplication" targetRef="AdepartmentManagerApproval" />
    <bpmn:sequenceFlow id="Flow_AdepartmentManagerApproval_AHRConfirmation" sourceRef="AdepartmentManagerApproval" targetRef="AHRConfirmation" />
    <bpmn:sequenceFlow id="Flow_AHRConfirmation_AresultNotification" sourceRef="AHRConfirmation" targetRef="AresultNotification" />
    <bpmn:sequenceFlow id="Flow_AresultNotification_AendEvent" sourceRef="AresultNotification" targetRef="AendEvent" />
    <bpmn:sequenceFlow id="Flow_AdepartmentManagerApproval_AapprovalRejected" sourceRef="AdepartmentManagerApproval" targetRef="AapprovalRejected">
      <bpmn:conditionExpression>approval != ''通过''</bpmn:conditionExpression>
    </bpmn:sequenceFlow>
    <bpmn:sequenceFlow id="Flow_AapprovalRejected_AendEvent" sourceRef="AapprovalRejected" targetRef="AendEvent" />
  </bpmn:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="employeeLeaveRequestProcess_hwpb">
      <bpmndi:BPMNShape id="Shape_AstartEvent" bpmnElement="AstartEvent">
        <dc:Bounds x="100" y="100" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Shape_AsubmitApplication" bpmnElement="AsubmitApplication">
        <dc:Bounds x="300" y="100" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Shape_AdepartmentManagerApproval" bpmnElement="AdepartmentManagerApproval">
        <dc:Bounds x="500" y="100" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Shape_AresultNotification" bpmnElement="AresultNotification">
        <dc:Bounds x="1000" y="50" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Shape_AendEvent" bpmnElement="AendEvent">
        <dc:Bounds x="1032" y="172" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="1027" y="218" width="45" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Shape_AapprovalRejected" bpmnElement="AapprovalRejected">
        <dc:Bounds x="790" y="150" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Shape_AHRConfirmation" bpmnElement="AHRConfirmation">
        <dc:Bounds x="790" y="50" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNEdge id="Edge_AstartEvent_AsubmitApplication" bpmnElement="Flow_AstartEvent_AsubmitApplication">
        <di:waypoint x="136" y="118" />
        <di:waypoint x="218" y="118" />
        <di:waypoint x="300" y="140" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Edge_AsubmitApplication_AdepartmentManagerApproval" bpmnElement="Flow_AsubmitApplication_AdepartmentManagerApproval">
        <di:waypoint x="400" y="140" />
        <di:waypoint x="450" y="140" />
        <di:waypoint x="500" y="140" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Edge_AdepartmentManagerApproval_AHRConfirmation" bpmnElement="Flow_AdepartmentManagerApproval_AHRConfirmation">
        <di:waypoint x="600" y="140" />
        <di:waypoint x="650" y="140" />
        <di:waypoint x="650" y="90" />
        <di:waypoint x="790" y="90" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Edge_AHRConfirmation_AresultNotification" bpmnElement="Flow_AHRConfirmation_AresultNotification">
        <di:waypoint x="890" y="90" />
        <di:waypoint x="1000" y="90" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Edge_AresultNotification_AendEvent" bpmnElement="Flow_AresultNotification_AendEvent">
        <di:waypoint x="1050" y="130" />
        <di:waypoint x="1050" y="172" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Edge_AdepartmentManagerApproval_AapprovalRejected" bpmnElement="Flow_AdepartmentManagerApproval_AapprovalRejected">
        <di:waypoint x="600" y="140" />
        <di:waypoint x="650" y="140" />
        <di:waypoint x="650" y="190" />
        <di:waypoint x="790" y="190" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Edge_AapprovalRejected_AendEvent" bpmnElement="Flow_AapprovalRejected_AendEvent">
        <di:waypoint x="890" y="190" />
        <di:waypoint x="1032" y="190" />
      </bpmndi:BPMNEdge>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn:definitions>
', 0, NULL, NULL, 0, NULL, CAST(0x0000B28C00C0143D AS DateTime), CAST(0x0000B28C00C035AB AS DateTime))
INSERT [dbo].[WfProcess] ([ID], [ProcessID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1603, N'Submit_Request_Process_o870', N'1', N'Submit Requests Form Process', N'Submit_Request_Process_o870', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="utf-8"?><bpmn:definitions xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL"><bpmn:process id="Submit_Request_Process_o870" name="Submit Requests Form Process" isExecutable="false"><bpmn:startEvent id="AStartEvent_1" name="Start" /><bpmn:userTask id="AUserTask_1" name="Fill out Submit Requests Form" assignee="" /><bpmn:serviceTask id="AServiceTask_1" name="Submit Request" implementation="" /><bpmn:exclusiveGateway id="AGateway_1" name="Approval Condition" /><bpmn:userTask id="AUserTask_2" name="Approve Request" assignee="" /><bpmn:endEvent id="AEndEvent_1" name="Request Approved" /><bpmn:userTask id="AUserTask_3" name="Reject Request" assignee="" /><bpmn:endEvent id="AEndEvent_2" name="Request Rejected" /><bpmn:sequenceFlow id="Flow_AStartEvent_1_AUserTask_1" sourceRef="AStartEvent_1" targetRef="AUserTask_1" /><bpmn:sequenceFlow id="Flow_AUserTask_1_AServiceTask_1" sourceRef="AUserTask_1" targetRef="AServiceTask_1" /><bpmn:sequenceFlow id="Flow_AServiceTask_1_AGateway_1" sourceRef="AServiceTask_1" targetRef="AGateway_1" /><bpmn:sequenceFlow id="Flow_AGateway_1_AUserTask_2" sourceRef="AGateway_1" targetRef="AUserTask_2" /><bpmn:sequenceFlow id="Flow_AUserTask_2_AEndEvent_1" sourceRef="AUserTask_2" targetRef="AEndEvent_1" /><bpmn:sequenceFlow id="Flow_AGateway_1_AUserTask_3" sourceRef="AGateway_1" targetRef="AUserTask_3" /><bpmn:sequenceFlow id="Flow_AUserTask_3_AEndEvent_2" sourceRef="AUserTask_3" targetRef="AEndEvent_2" /></bpmn:process><bpmndi:BPMNDiagram id="BPMNDiagram_1"><bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Submit_Request_Process_o870"><bpmndi:BPMNShape id="Shape_AStartEvent_1" bpmnElement="AStartEvent_1"><dc:Bounds x="100" y="100" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AUserTask_1" bpmnElement="AUserTask_1"><dc:Bounds x="300" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AServiceTask_1" bpmnElement="AServiceTask_1"><dc:Bounds x="500" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AGateway_1" bpmnElement="AGateway_1"><dc:Bounds x="700" y="100" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AUserTask_2" bpmnElement="AUserTask_2"><dc:Bounds x="900" y="50" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AEndEvent_1" bpmnElement="AEndEvent_1"><dc:Bounds x="1100" y="50" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AUserTask_3" bpmnElement="AUserTask_3"><dc:Bounds x="900" y="150" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AEndEvent_2" bpmnElement="AEndEvent_2"><dc:Bounds x="1100" y="150" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNEdge id="Edge_AStartEvent_1_AUserTask_1" bpmnElement="Flow_AStartEvent_1_AUserTask_1"><di:waypoint x="136" y="118" /><di:waypoint x="218" y="118" /><di:waypoint x="300" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AUserTask_1_AServiceTask_1" bpmnElement="Flow_AUserTask_1_AServiceTask_1"><di:waypoint x="400" y="140" /><di:waypoint x="450" y="140" /><di:waypoint x="500" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AServiceTask_1_AGateway_1" bpmnElement="Flow_AServiceTask_1_AGateway_1"><di:waypoint x="600" y="140" /><di:waypoint x="650" y="140" /><di:waypoint x="700" y="118" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AGateway_1_AUserTask_2" bpmnElement="Flow_AGateway_1_AUserTask_2"><di:waypoint x="736" y="118" /><di:waypoint x="786" y="118" /><di:waypoint x="786" y="90" /><di:waypoint x="900" y="90" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AUserTask_2_AEndEvent_1" bpmnElement="Flow_AUserTask_2_AEndEvent_1"><di:waypoint x="1000" y="90" /><di:waypoint x="1050" y="90" /><di:waypoint x="1100" y="68" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AGateway_1_AUserTask_3" bpmnElement="Flow_AGateway_1_AUserTask_3"><di:waypoint x="736" y="118" /><di:waypoint x="786" y="118" /><di:waypoint x="786" y="190" /><di:waypoint x="900" y="190" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AUserTask_3_AEndEvent_2" bpmnElement="Flow_AUserTask_3_AEndEvent_2"><di:waypoint x="1000" y="190" /><di:waypoint x="1050" y="190" /><di:waypoint x="1100" y="168" /></bpmndi:BPMNEdge></bpmndi:BPMNPlane></bpmndi:BPMNDiagram></bpmn:definitions>', 0, NULL, NULL, 0, NULL, CAST(0x0000B28D000BCA4D AS DateTime), NULL)
INSERT [dbo].[WfProcess] ([ID], [ProcessID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1604, N'Process_to9r_6959', N'1', N'Gateway_6959', N'Gateway_Code_6959', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL" id="bpmn-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn:process id="Process_to9r_6959" sf:code="Gateway_Code_6959" name="Gateway_6959" isExecutable="true" sf:version="1">
    <bpmn:startEvent id="StartNode_4301" sf:code="Start" name="start">
      <bpmn:outgoing>Flow_3791</bpmn:outgoing>
    </bpmn:startEvent>
    <bpmn:task id="TaskNode_2923" sf:code="task001" name="Task-001">
      <bpmn:incoming>Flow_3791</bpmn:incoming>
      <bpmn:outgoing>Flow_8038</bpmn:outgoing>
    </bpmn:task>
    <bpmn:parallelGateway id="GatewayNode_5531" sf:code="andsplit001" name="and-split">
      <bpmn:incoming>Flow_8038</bpmn:incoming>
      <bpmn:outgoing>Flow_6173</bpmn:outgoing>
      <bpmn:outgoing>Flow_6045</bpmn:outgoing>
    </bpmn:parallelGateway>
    <bpmn:task id="TaskNode_1376" sf:code="task010" name="task-010">
      <bpmn:incoming>Flow_6173</bpmn:incoming>
      <bpmn:outgoing>Flow_7180</bpmn:outgoing>
    </bpmn:task>
    <bpmn:task id="TaskNode_7986" sf:code="task020" name="task-020">
      <bpmn:incoming>Flow_6045</bpmn:incoming>
      <bpmn:outgoing>Flow_7936</bpmn:outgoing>
    </bpmn:task>
    <bpmn:parallelGateway id="GatewayNode_2816" sf:code="andjoin001" name="and-join">
      <bpmn:incoming>Flow_7936</bpmn:incoming>
      <bpmn:incoming>Flow_7180</bpmn:incoming>
      <bpmn:outgoing>Flow_4715</bpmn:outgoing>
    </bpmn:parallelGateway>
    <bpmn:task id="TaskNode_2479" sf:code="task100" name="task-100">
      <bpmn:incoming>Flow_4715</bpmn:incoming>
      <bpmn:outgoing>Flow_9952</bpmn:outgoing>
    </bpmn:task>
    <bpmn:endEvent id="EndNode_5055" sf:code="End" name="end">
      <bpmn:incoming>Flow_9952</bpmn:incoming>
    </bpmn:endEvent>
    <bpmn:sequenceFlow id="Flow_3791" name="" sourceRef="StartNode_4301" targetRef="TaskNode_2923" />
    <bpmn:sequenceFlow id="Flow_8038" name="" sourceRef="TaskNode_2923" targetRef="GatewayNode_5531" />
    <bpmn:sequenceFlow id="Flow_6173" name="" sourceRef="GatewayNode_5531" targetRef="TaskNode_1376" />
    <bpmn:sequenceFlow id="Flow_6045" name="" sourceRef="GatewayNode_5531" targetRef="TaskNode_7986" />
    <bpmn:sequenceFlow id="Flow_7936" name="" sourceRef="TaskNode_7986" targetRef="GatewayNode_2816" />
    <bpmn:sequenceFlow id="Flow_7180" name="" sourceRef="TaskNode_1376" targetRef="GatewayNode_2816" />
    <bpmn:sequenceFlow id="Flow_4715" name="" sourceRef="GatewayNode_2816" targetRef="TaskNode_2479" />
    <bpmn:sequenceFlow id="Flow_9952" name="" sourceRef="TaskNode_2479" targetRef="EndNode_5055" />
  </bpmn:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_to9r_6959">
      <bpmndi:BPMNShape id="BPMNShape_9na1drv_di" bpmnElement="StartNode_4301">
        <dc:Bounds x="240" y="180" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="BPMNShape_l3y8bx4_di" bpmnElement="TaskNode_2923">
        <dc:Bounds x="356" y="158" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="BPMNShape_itlv3na_di" bpmnElement="GatewayNode_5531">
        <dc:Bounds x="536" y="180" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="BPMNShape_g6lz88n_di" bpmnElement="TaskNode_1376">
        <dc:Bounds x="652" y="230" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="BPMNShape_mlp4wmo_di" bpmnElement="TaskNode_7986">
        <dc:Bounds x="652" y="70" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="BPMNShape_dnca32n_di" bpmnElement="GatewayNode_2816">
        <dc:Bounds x="832" y="180" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="BPMNShape_040561s_di" bpmnElement="TaskNode_2479">
        <dc:Bounds x="948" y="158" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="BPMNShape_y4rfkex_di" bpmnElement="EndNode_5055">
        <dc:Bounds x="1128" y="180" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNEdge id="Flow_3791_di" bpmnElement="Flow_3791">
        <di:waypoint x="276" y="198" />
        <di:waypoint x="356" y="198" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_8038_di" bpmnElement="Flow_8038">
        <di:waypoint x="456" y="198" />
        <di:waypoint x="536" y="198" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_6173_di" bpmnElement="Flow_6173">
        <di:waypoint x="554" y="216" />
        <di:waypoint x="554" y="270" />
        <di:waypoint x="652" y="270" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_6045_di" bpmnElement="Flow_6045">
        <di:waypoint x="554" y="216" />
        <di:waypoint x="554" y="110" />
        <di:waypoint x="652" y="110" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_7936_di" bpmnElement="Flow_7936">
        <di:waypoint x="752" y="110" />
        <di:waypoint x="850" y="110" />
        <di:waypoint x="850" y="180" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_7180_di" bpmnElement="Flow_7180">
        <di:waypoint x="752" y="270" />
        <di:waypoint x="850" y="270" />
        <di:waypoint x="850" y="216" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_4715_di" bpmnElement="Flow_4715">
        <di:waypoint x="868" y="198" />
        <di:waypoint x="948" y="198" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_9952_di" bpmnElement="Flow_9952">
        <di:waypoint x="1048" y="198" />
        <di:waypoint x="1128" y="198" />
      </bpmndi:BPMNEdge>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn:definitions>
', 0, NULL, NULL, 0, NULL, CAST(0x0000B28E00B6DC89 AS DateTime), CAST(0x0000B28E00B6E305 AS DateTime))
INSERT [dbo].[WfProcess] ([ID], [ProcessID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1605, N'Process_i2rt_9195', N'1', N'Warehousing_9195', N'Warehousing_Code_9195', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL" id="bpmn-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn:process id="Process_i2rt_9195" sf:code="Warehousing_Code_9195" name="Warehousing_9195" isExecutable="true" sf:version="1">
    <bpmn:startEvent id="StartNode_1906" sf:code="Start" name="start">
      <bpmn:outgoing>Flow_8858</bpmn:outgoing>
    </bpmn:startEvent>
    <bpmn:task id="TaskNode_7488" sf:code="task001" name="Warehouse Signature">
      <bpmn:incoming>Flow_8858</bpmn:incoming>
      <bpmn:outgoing>Flow_3316</bpmn:outgoing>
    </bpmn:task>
    <bpmn:exclusiveGateway id="GatewayNode_7228" sf:code="andsplit001" name="And-Split">
      <bpmn:incoming>Flow_3316</bpmn:incoming>
      <bpmn:outgoing>Flow_3186</bpmn:outgoing>
      <bpmn:outgoing>Flow_8272</bpmn:outgoing>
    </bpmn:exclusiveGateway>
    <bpmn:task id="TaskNode_1520" sf:code="task010" name="Signature of the Comprehensive Department">
      <bpmn:incoming>Flow_3186</bpmn:incoming>
      <bpmn:outgoing>Flow_9015</bpmn:outgoing>
    </bpmn:task>
    <bpmn:task id="TaskNode_8715" sf:code="task020" name="Financial Signature">
      <bpmn:incoming>Flow_8272</bpmn:incoming>
      <bpmn:outgoing>Flow_6032</bpmn:outgoing>
    </bpmn:task>
    <bpmn:parallelGateway id="GatewayNode_6440" sf:code="andjoin001" name="And-Join">
      <bpmn:incoming>Flow_6032</bpmn:incoming>
      <bpmn:incoming>Flow_9015</bpmn:incoming>
      <bpmn:outgoing>Flow_4052</bpmn:outgoing>
    </bpmn:parallelGateway>
    <bpmn:task id="TaskNode_5124" sf:code="task007" name="CEO Signature">
      <bpmn:incoming>Flow_4052</bpmn:incoming>
      <bpmn:outgoing>Flow_9011</bpmn:outgoing>
    </bpmn:task>
    <bpmn:endEvent id="EndNode_7765" sf:code="End" name="end">
      <bpmn:incoming>Flow_9011</bpmn:incoming>
    </bpmn:endEvent>
    <bpmn:sequenceFlow id="Flow_8858" name="" sourceRef="StartNode_1906" targetRef="TaskNode_7488" />
    <bpmn:sequenceFlow id="Flow_3316" name="" sourceRef="TaskNode_7488" targetRef="GatewayNode_7228" />
    <bpmn:sequenceFlow id="Flow_3186" name="" sourceRef="GatewayNode_7228" targetRef="TaskNode_1520" />
    <bpmn:sequenceFlow id="Flow_8272" name="" sourceRef="GatewayNode_7228" targetRef="TaskNode_8715" />
    <bpmn:sequenceFlow id="Flow_6032" name="" sourceRef="TaskNode_8715" targetRef="GatewayNode_6440" />
    <bpmn:sequenceFlow id="Flow_9015" name="" sourceRef="TaskNode_1520" targetRef="GatewayNode_6440" />
    <bpmn:sequenceFlow id="Flow_4052" name="" sourceRef="GatewayNode_6440" targetRef="TaskNode_5124" />
    <bpmn:sequenceFlow id="Flow_9011" name="" sourceRef="TaskNode_5124" targetRef="EndNode_7765" />
  </bpmn:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_i2rt_9195">
      <bpmndi:BPMNShape id="BPMNShape_b55jylo_di" bpmnElement="StartNode_1906">
        <dc:Bounds x="240" y="180" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="BPMNShape_7qmd9oj_di" bpmnElement="TaskNode_7488">
        <dc:Bounds x="356" y="158" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="BPMNShape_nspsn1w_di" bpmnElement="GatewayNode_7228" isMarkerVisible="true">
        <dc:Bounds x="536" y="180" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="BPMNShape_r4oeg4x_di" bpmnElement="TaskNode_1520">
        <dc:Bounds x="652" y="230" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="BPMNShape_7svmtub_di" bpmnElement="TaskNode_8715">
        <dc:Bounds x="652" y="70" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="BPMNShape_66f3l1g_di" bpmnElement="GatewayNode_6440">
        <dc:Bounds x="832" y="180" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="BPMNShape_epgfi2b_di" bpmnElement="TaskNode_5124">
        <dc:Bounds x="948" y="158" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="BPMNShape_38ihkb5_di" bpmnElement="EndNode_7765">
        <dc:Bounds x="1128" y="180" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNEdge id="Flow_8858_di" bpmnElement="Flow_8858">
        <di:waypoint x="276" y="198" />
        <di:waypoint x="356" y="198" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_3316_di" bpmnElement="Flow_3316">
        <di:waypoint x="456" y="198" />
        <di:waypoint x="536" y="198" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_3186_di" bpmnElement="Flow_3186">
        <di:waypoint x="554" y="216" />
        <di:waypoint x="554" y="270" />
        <di:waypoint x="652" y="270" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_8272_di" bpmnElement="Flow_8272">
        <di:waypoint x="554" y="216" />
        <di:waypoint x="554" y="110" />
        <di:waypoint x="652" y="110" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_6032_di" bpmnElement="Flow_6032">
        <di:waypoint x="752" y="110" />
        <di:waypoint x="850" y="110" />
        <di:waypoint x="850" y="180" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_9015_di" bpmnElement="Flow_9015">
        <di:waypoint x="752" y="270" />
        <di:waypoint x="850" y="270" />
        <di:waypoint x="850" y="216" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_4052_di" bpmnElement="Flow_4052">
        <di:waypoint x="868" y="198" />
        <di:waypoint x="948" y="198" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_9011_di" bpmnElement="Flow_9011">
        <di:waypoint x="1048" y="198" />
        <di:waypoint x="1128" y="198" />
      </bpmndi:BPMNEdge>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn:definitions>
', 0, NULL, NULL, 0, NULL, CAST(0x0000B28E00B6F79C AS DateTime), CAST(0x0000B28E00B6FA3A AS DateTime))
INSERT [dbo].[WfProcess] ([ID], [ProcessID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1606, N'2def5a36-8ecd-4c33-8591-4c0f698517d4', N'1', N'Process_Name_3687', N'Process_Code_3687', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL" id="bpmn-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn:process id="Process_3687" sf:code="Process_Code_3687" name="Process_Name_3687" isExecutable="true" sf:version="1">
    <bpmn:startEvent id="StartEvent_1" name="Start" />
  </bpmn:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_3687">
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="412" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="418" y="276" width="25" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn:definitions>
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     ', 0, NULL, NULL, 0, NULL, CAST(0x0000B28E00D8B0AE AS DateTime), NULL)
INSERT [dbo].[WfProcess] ([ID], [ProcessID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1607, N'ab154402-04ef-46e3-a0bd-e0a3404ba5bf', N'1', N'Process_Name_3687', N'Process_Code_3687', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL" id="bpmn-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn:process id="Process_3687" sf:code="Process_Code_3687" name="Process_Name_3687" isExecutable="true" sf:version="1">
    <bpmn:startEvent id="StartEvent_1" name="Start" />
  </bpmn:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_3687">
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="412" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="418" y="276" width="25" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn:definitions>
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     ', 0, NULL, NULL, 0, NULL, CAST(0x0000B28E00D8B0AE AS DateTime), NULL)
INSERT [dbo].[WfProcess] ([ID], [ProcessID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1608, N'Process_6387', N'1', N'Process_Name_6387', N'Process_Code_6387', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL" id="bpmn-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn:process id="Process_6387" sf:code="Process_Code_6387" name="Process_Name_6387" isExecutable="true" sf:version="1">
    <bpmn:startEvent id="StartEvent_1" name="Start" />
  </bpmn:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_6387">
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="412" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn:definitions>
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           ', 0, NULL, NULL, 0, NULL, CAST(0x0000B28E00D91B64 AS DateTime), NULL)
INSERT [dbo].[WfProcess] ([ID], [ProcessID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1609, N'Process_e9b8_8807', N'1', N'Complex_8807', N'Complex_Code_8807', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="utf-8"?><bpmn:definitions targetNamespace="http://bpmn.io/schema/bpmn" id="bpmn-diagram" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL"><bpmn:process id="Process_e9b8_8807" name="Complex_8807" isExecutable="true" sf:code="Complex_Code_8807" sf:version="1"><bpmn:startEvent id="StartNode_4366" name="start" sf:code="Start"><bpmn:outgoing>Flow_7842</bpmn:outgoing></bpmn:startEvent><bpmn:task id="TaskNode_3874" name="Task-001" sf:code="task001"><bpmn:incoming>Flow_7842</bpmn:incoming><bpmn:outgoing>Flow_6731</bpmn:outgoing></bpmn:task><bpmn:inclusiveGateway id="GatewayNode_3988" name="or-split" sf:code="orsplit001"><bpmn:incoming>Flow_6731</bpmn:incoming><bpmn:outgoing>Flow_3659</bpmn:outgoing><bpmn:outgoing>Flow_5511</bpmn:outgoing><bpmn:outgoing>Flow_6254</bpmn:outgoing></bpmn:inclusiveGateway><bpmn:task id="TaskNode_5743" name="task-010" sf:code="task010"><bpmn:incoming>Flow_3659</bpmn:incoming><bpmn:outgoing>Flow_7138</bpmn:outgoing></bpmn:task><bpmn:task id="MultiSignNode_4598" name="MI-011" sf:code="mi011"><bpmn:incoming>Flow_7138</bpmn:incoming><bpmn:outgoing>Flow_9124</bpmn:outgoing><bpmn:multiInstanceLoopCharacteristics /></bpmn:task><bpmn:task id="TaskNode_5973" name="task-020" sf:code="task020"><bpmn:incoming>Flow_5511</bpmn:incoming><bpmn:outgoing>Flow_4069</bpmn:outgoing></bpmn:task><bpmn:subProcess id="SubProcessNode_8646" name="subname021" sf:code="subcode021"><bpmn:incoming>Flow_4069</bpmn:incoming><bpmn:outgoing>Flow_2150</bpmn:outgoing></bpmn:subProcess><bpmn:task id="TaskNode_9679" name="task-030" sf:code="task030"><bpmn:incoming>Flow_6254</bpmn:incoming><bpmn:outgoing>Flow_1632</bpmn:outgoing></bpmn:task><bpmn:task id="TaskNode_1660" name="task-031" sf:code="task031"><bpmn:incoming>Flow_1632</bpmn:incoming><bpmn:outgoing>Flow_9562</bpmn:outgoing></bpmn:task><bpmn:inclusiveGateway id="GatewayNode_4225" name="or-join" sf:code="orjoin001"><bpmn:incoming>Flow_9562</bpmn:incoming><bpmn:incoming>Flow_2150</bpmn:incoming><bpmn:incoming>Flow_9124</bpmn:incoming><bpmn:outgoing>Flow_7253</bpmn:outgoing></bpmn:inclusiveGateway><bpmn:task id="TaskNode_8423" name="task-100" sf:code="task100"><bpmn:incoming>Flow_7253</bpmn:incoming><bpmn:outgoing>Flow_9959</bpmn:outgoing></bpmn:task><bpmn:endEvent id="EndNode_3453" name="end" sf:code="End"><bpmn:incoming>Flow_9959</bpmn:incoming></bpmn:endEvent><bpmn:sequenceFlow id="Flow_7842" name="" sourceRef="StartNode_4366" targetRef="TaskNode_3874" /><bpmn:sequenceFlow id="Flow_6731" name="" sourceRef="TaskNode_3874" targetRef="GatewayNode_3988" /><bpmn:sequenceFlow id="Flow_3659" name="" sourceRef="GatewayNode_3988" targetRef="TaskNode_5743" /><bpmn:sequenceFlow id="Flow_7138" name="" sourceRef="TaskNode_5743" targetRef="MultiSignNode_4598" /><bpmn:sequenceFlow id="Flow_5511" name="" sourceRef="GatewayNode_3988" targetRef="TaskNode_5973" /><bpmn:sequenceFlow id="Flow_4069" name="" sourceRef="TaskNode_5973" targetRef="SubProcessNode_8646" /><bpmn:sequenceFlow id="Flow_6254" name="" sourceRef="GatewayNode_3988" targetRef="TaskNode_9679" /><bpmn:sequenceFlow id="Flow_1632" name="" sourceRef="TaskNode_9679" targetRef="TaskNode_1660" /><bpmn:sequenceFlow id="Flow_9562" name="" sourceRef="TaskNode_1660" targetRef="GatewayNode_4225" /><bpmn:sequenceFlow id="Flow_2150" name="" sourceRef="SubProcessNode_8646" targetRef="GatewayNode_4225" /><bpmn:sequenceFlow id="Flow_9124" name="" sourceRef="MultiSignNode_4598" targetRef="GatewayNode_4225" /><bpmn:sequenceFlow id="Flow_7253" name="" sourceRef="GatewayNode_4225" targetRef="TaskNode_8423" /><bpmn:sequenceFlow id="Flow_9959" name="" sourceRef="TaskNode_8423" targetRef="EndNode_3453" /></bpmn:process><bpmndi:BPMNDiagram id="BPMNDiagram_1"><bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_100"><bpmndi:BPMNShape id="BPMNShape_0sbrs54_di" bpmnElement="StartNode_4366"><dc:Bounds height="36" width="36" x="240" y="180" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_pghtuxr_di" bpmnElement="TaskNode_3874"><dc:Bounds height="80" width="100" x="356" y="158" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_rxxg6ad_di" bpmnElement="GatewayNode_3988"><dc:Bounds height="36" width="36" x="536" y="180" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_89f2kwa_di" bpmnElement="TaskNode_5743"><dc:Bounds height="80" width="100" x="652" y="390" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_akqio2v_di" bpmnElement="MultiSignNode_4598"><dc:Bounds height="80" width="100" x="832" y="390" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_0vvgsky_di" bpmnElement="TaskNode_5973"><dc:Bounds height="80" width="100" x="652" y="230" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_qb41qbz_di" bpmnElement="SubProcessNode_8646"><dc:Bounds height="80" width="100" x="832" y="230" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_9grrg5g_di" bpmnElement="TaskNode_9679"><dc:Bounds height="80" width="100" x="652" y="70" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_8u067xz_di" bpmnElement="TaskNode_1660"><dc:Bounds height="80" width="100" x="832" y="70" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_ktdvuag_di" bpmnElement="GatewayNode_4225"><dc:Bounds height="36" width="36" x="1012" y="180" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_5gszp47_di" bpmnElement="TaskNode_8423"><dc:Bounds height="80" width="100" x="1128" y="158" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_d2n5xd7_di" bpmnElement="EndNode_3453"><dc:Bounds height="36" width="36" x="1308" y="180" /></bpmndi:BPMNShape><bpmndi:BPMNEdge id="Flow_7842_di" bpmnElement="Flow_7842"><di:waypoint x="276" y="198" /><di:waypoint x="356" y="198" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_6731_di" bpmnElement="Flow_6731"><di:waypoint x="456" y="198" /><di:waypoint x="536" y="198" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_3659_di" bpmnElement="Flow_3659"><di:waypoint x="554" y="216" /><di:waypoint x="554" y="430" /><di:waypoint x="652" y="430" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_7138_di" bpmnElement="Flow_7138"><di:waypoint x="752" y="430" /><di:waypoint x="832" y="430" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_5511_di" bpmnElement="Flow_5511"><di:waypoint x="554" y="216" /><di:waypoint x="554" y="270" /><di:waypoint x="652" y="270" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_4069_di" bpmnElement="Flow_4069"><di:waypoint x="752" y="270" /><di:waypoint x="832" y="270" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_6254_di" bpmnElement="Flow_6254"><di:waypoint x="554" y="180" /><di:waypoint x="554" y="110" /><di:waypoint x="652" y="110" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_1632_di" bpmnElement="Flow_1632"><di:waypoint x="752" y="110" /><di:waypoint x="832" y="110" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_9562_di" bpmnElement="Flow_9562"><di:waypoint x="932" y="110" /><di:waypoint x="1030" y="110" /><di:waypoint x="1030" y="180" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_2150_di" bpmnElement="Flow_2150"><di:waypoint x="932" y="270" /><di:waypoint x="1030" y="270" /><di:waypoint x="1030" y="216" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_9124_di" bpmnElement="Flow_9124"><di:waypoint x="932" y="430" /><di:waypoint x="1030" y="430" /><di:waypoint x="1030" y="216" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_7253_di" bpmnElement="Flow_7253"><di:waypoint x="1048" y="198" /><di:waypoint x="1128" y="198" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_9959_di" bpmnElement="Flow_9959"><di:waypoint x="1228" y="198" /><di:waypoint x="1308" y="198" /></bpmndi:BPMNEdge></bpmndi:BPMNPlane></bpmndi:BPMNDiagram><bpmndi:BPMNDiagram id="BPMNDiagram_939wsxn"><bpmndi:BPMNPlane id="BPMNPlane_4dz4zik" bpmnElement="SubProcessNode_8646" /></bpmndi:BPMNDiagram></bpmn:definitions>', 0, NULL, NULL, 0, NULL, CAST(0x0000B28E00DBAD48 AS DateTime), NULL)
INSERT [dbo].[WfProcess] ([ID], [ProcessID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1610, N'Process_jqz2_7916', N'1', N'AskforLeave_7916', N'AskforLeave_Code_7916', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="utf-8"?><bpmn:definitions targetNamespace="http://bpmn.io/schema/bpmn" id="bpmn-diagram" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL"><bpmn:process id="Process_jqz2_7916" name="AskforLeave_7916" isExecutable="true" sf:code="AskforLeave_Code_7916" sf:version="1"><bpmn:startEvent id="StartNode_9317" name="start" sf:code="Start"><bpmn:outgoing>Flow_3366</bpmn:outgoing></bpmn:startEvent><bpmn:task id="TaskNode_3142" name="Apply Submit" sf:code="task001"><bpmn:incoming>Flow_3366</bpmn:incoming><bpmn:outgoing>Flow_9542</bpmn:outgoing></bpmn:task><bpmn:exclusiveGateway id="GatewayNode_7145" name="XOr-Split" sf:code="xorsplit001"><bpmn:incoming>Flow_9542</bpmn:incoming><bpmn:outgoing>Flow_2841</bpmn:outgoing><bpmn:outgoing>Flow_7279</bpmn:outgoing></bpmn:exclusiveGateway><bpmn:task id="TaskNode_8096" name="Dept Manager Approval" sf:code="task010"><bpmn:incoming>Flow_2841</bpmn:incoming><bpmn:outgoing>Flow_6580</bpmn:outgoing></bpmn:task><bpmn:task id="TaskNode_6209" name="CEO Approval" sf:code="task020"><bpmn:incoming>Flow_7279</bpmn:incoming><bpmn:outgoing>Flow_2147</bpmn:outgoing></bpmn:task><bpmn:exclusiveGateway id="GatewayNode_1775" name="XOr-Join" sf:code="xorjoin001"><bpmn:incoming>Flow_2147</bpmn:incoming><bpmn:incoming>Flow_6580</bpmn:incoming><bpmn:outgoing>Flow_3240</bpmn:outgoing></bpmn:exclusiveGateway><bpmn:task id="TaskNode_7953" name="HR Approval" sf:code="task100"><bpmn:incoming>Flow_3240</bpmn:incoming><bpmn:outgoing>Flow_5700</bpmn:outgoing></bpmn:task><bpmn:endEvent id="EndNode_5557" name="end" sf:code="End"><bpmn:incoming>Flow_5700</bpmn:incoming></bpmn:endEvent><bpmn:sequenceFlow id="Flow_3366" name="" sourceRef="StartNode_9317" targetRef="TaskNode_3142" /><bpmn:sequenceFlow id="Flow_9542" name="" sourceRef="TaskNode_3142" targetRef="GatewayNode_7145" /><bpmn:sequenceFlow id="Flow_2841" name="days&lt;3" sourceRef="GatewayNode_7145" targetRef="TaskNode_8096"><bpmn:conditionExpression>days&lt;3</bpmn:conditionExpression></bpmn:sequenceFlow><bpmn:sequenceFlow id="Flow_7279" name="days&gt;=3" sourceRef="GatewayNode_7145" targetRef="TaskNode_6209"><bpmn:conditionExpression>days&gt;=3</bpmn:conditionExpression></bpmn:sequenceFlow><bpmn:sequenceFlow id="Flow_2147" name="" sourceRef="TaskNode_6209" targetRef="GatewayNode_1775" /><bpmn:sequenceFlow id="Flow_6580" name="" sourceRef="TaskNode_8096" targetRef="GatewayNode_1775" /><bpmn:sequenceFlow id="Flow_3240" name="" sourceRef="GatewayNode_1775" targetRef="TaskNode_7953" /><bpmn:sequenceFlow id="Flow_5700" name="" sourceRef="TaskNode_7953" targetRef="EndNode_5557" /></bpmn:process><bpmndi:BPMNDiagram id="BPMNDiagram_1"><bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_100"><bpmndi:BPMNShape id="BPMNShape_19qtjht_di" bpmnElement="StartNode_9317"><dc:Bounds height="36" width="36" x="240" y="180" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_2ntr84j_di" bpmnElement="TaskNode_3142"><dc:Bounds height="80" width="100" x="356" y="158" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_hvuk38g_di" bpmnElement="GatewayNode_7145"><dc:Bounds height="36" width="36" x="536" y="180" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_hkq3kt8_di" bpmnElement="TaskNode_8096"><dc:Bounds height="80" width="100" x="652" y="230" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_xs5q78y_di" bpmnElement="TaskNode_6209"><dc:Bounds height="80" width="100" x="652" y="70" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_h757x7y_di" bpmnElement="GatewayNode_1775"><dc:Bounds height="36" width="36" x="832" y="180" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_5wkemce_di" bpmnElement="TaskNode_7953"><dc:Bounds height="80" width="100" x="948" y="158" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_vsrgzeq_di" bpmnElement="EndNode_5557"><dc:Bounds height="36" width="36" x="1128" y="180" /></bpmndi:BPMNShape><bpmndi:BPMNEdge id="Flow_3366_di" bpmnElement="Flow_3366"><di:waypoint x="276" y="198" /><di:waypoint x="356" y="198" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_9542_di" bpmnElement="Flow_9542"><di:waypoint x="456" y="198" /><di:waypoint x="536" y="198" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_2841_di" bpmnElement="Flow_2841"><di:waypoint x="554" y="216" /><di:waypoint x="554" y="270" /><di:waypoint x="652" y="270" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_7279_di" bpmnElement="Flow_7279"><di:waypoint x="554" y="216" /><di:waypoint x="554" y="110" /><di:waypoint x="652" y="110" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_2147_di" bpmnElement="Flow_2147"><di:waypoint x="752" y="110" /><di:waypoint x="850" y="110" /><di:waypoint x="850" y="180" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_6580_di" bpmnElement="Flow_6580"><di:waypoint x="752" y="270" /><di:waypoint x="850" y="270" /><di:waypoint x="850" y="216" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_3240_di" bpmnElement="Flow_3240"><di:waypoint x="868" y="198" /><di:waypoint x="948" y="198" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_5700_di" bpmnElement="Flow_5700"><di:waypoint x="1048" y="198" /><di:waypoint x="1128" y="198" /></bpmndi:BPMNEdge></bpmndi:BPMNPlane></bpmndi:BPMNDiagram></bpmn:definitions>', 0, NULL, NULL, 0, NULL, CAST(0x0000B28E0104B62A AS DateTime), NULL)
INSERT [dbo].[WfProcess] ([ID], [ProcessID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1611, N'reimbursementProcess_i447', N'1', N'报销流程', N'reimbursementProcess_i447', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="utf-8"?><bpmn:definitions xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL"><bpmn:process id="reimbursementProcess_i447" name="报销流程" isExecutable="false"><bpmn:startEvent id="AstartEvent" name="开始" /><bpmn:userTask id="AdepartmentReview" name="部门初审" assignee="departmentHead" /><bpmn:inclusiveGateway id="AamountDecision" name="金额判断" /><bpmn:userTask id="AfinanceReview" name="财务复核" assignee="financeManager" /><bpmn:serviceTask id="AsyncERP" name="同步ERP" implementation="java:com.erp.SyncReimbursement" /><bpmn:endEvent id="AendEvent" name="结束" /><bpmn:sequenceFlow id="Flow_AstartEvent_AdepartmentReview" sourceRef="AstartEvent" targetRef="AdepartmentReview" /><bpmn:sequenceFlow id="Flow_AdepartmentReview_AamountDecision" sourceRef="AdepartmentReview" targetRef="AamountDecision" /><bpmn:sequenceFlow id="Flow_AamountDecision_AfinanceReview" sourceRef="AamountDecision" targetRef="AfinanceReview"><bpmn:conditionExpression>${amount &gt; 5000}</bpmn:conditionExpression></bpmn:sequenceFlow><bpmn:sequenceFlow id="Flow_AamountDecision_AsyncERP" sourceRef="AamountDecision" targetRef="AsyncERP"><bpmn:conditionExpression>${amount &lt;= 5000}</bpmn:conditionExpression></bpmn:sequenceFlow><bpmn:sequenceFlow id="Flow_AfinanceReview_AsyncERP" sourceRef="AfinanceReview" targetRef="AsyncERP" /><bpmn:sequenceFlow id="Flow_AsyncERP_AendEvent" sourceRef="AsyncERP" targetRef="AendEvent" /></bpmn:process><bpmndi:BPMNDiagram id="BPMNDiagram_1"><bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="reimbursementProcess_i447"><bpmndi:BPMNShape id="Shape_AstartEvent" bpmnElement="AstartEvent"><dc:Bounds x="100" y="100" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AdepartmentReview" bpmnElement="AdepartmentReview"><dc:Bounds x="300" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AamountDecision" bpmnElement="AamountDecision"><dc:Bounds x="500" y="100" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AfinanceReview" bpmnElement="AfinanceReview"><dc:Bounds x="700" y="50" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AsyncERP" bpmnElement="AsyncERP"><dc:Bounds x="700" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AendEvent" bpmnElement="AendEvent"><dc:Bounds x="900" y="100" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNEdge id="Edge_AstartEvent_AdepartmentReview" bpmnElement="Flow_AstartEvent_AdepartmentReview"><di:waypoint x="136" y="118" /><di:waypoint x="218" y="118" /><di:waypoint x="300" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AdepartmentReview_AamountDecision" bpmnElement="Flow_AdepartmentReview_AamountDecision"><di:waypoint x="400" y="140" /><di:waypoint x="450" y="140" /><di:waypoint x="500" y="118" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AamountDecision_AfinanceReview" bpmnElement="Flow_AamountDecision_AfinanceReview"><di:waypoint x="536" y="118" /><di:waypoint x="586" y="118" /><di:waypoint x="586" y="90" /><di:waypoint x="700" y="90" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AamountDecision_AsyncERP" bpmnElement="Flow_AamountDecision_AsyncERP"><di:waypoint x="536" y="118" /><di:waypoint x="618" y="118" /><di:waypoint x="700" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AfinanceReview_AsyncERP" bpmnElement="Flow_AfinanceReview_AsyncERP"><di:waypoint x="800" y="90" /><di:waypoint x="850" y="90" /><di:waypoint x="850" y="140" /><di:waypoint x="700" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AsyncERP_AendEvent" bpmnElement="Flow_AsyncERP_AendEvent"><di:waypoint x="800" y="140" /><di:waypoint x="850" y="140" /><di:waypoint x="900" y="118" /></bpmndi:BPMNEdge></bpmndi:BPMNPlane></bpmndi:BPMNDiagram></bpmn:definitions>', 0, NULL, NULL, 0, NULL, CAST(0x0000B28E010906FF AS DateTime), NULL)
INSERT [dbo].[WfProcess] ([ID], [ProcessID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1612, N'Process_koxx_1942', N'1', N'AskforLeave_1942', N'AskforLeave_Code_1942', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL" id="bpmn-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn:process id="Process_koxx_1942" sf:code="AskforLeave_Code_1942" name="AskforLeave_1942" isExecutable="true" sf:version="1">
    <bpmn:extensionElements>
      <sf:forms>
        <sf:form name="Form_0quwv7l" outerId="64" outerCode="BO94VE" />
        <sf:form name="Form_1u453x3" outerId="65" outerCode="WNMNE4" />
        <sf:form name="Form_Test_Sample" outerId="63" outerCode="QMIS2H" />
      </sf:forms>
    </bpmn:extensionElements>
    <bpmn:startEvent id="StartNode_6174" sf:code="Start" name="start">
      <bpmn:outgoing>Flow_3720</bpmn:outgoing>
    </bpmn:startEvent>
    <bpmn:task id="TaskNode_3574" sf:code="task001" name="Apply Submit">
      <bpmn:extensionElements>
        <sf:actions>
          <sf:action fireType="before" methodType="WebApi" subMethodType="HttpGet" />
        </sf:actions>
        <sf:notifications>
          <sf:notification name="Bill" outerId="8" outerCode="" outerType="User" />
        </sf:notifications>
        <sf:performers>
          <sf:performer name="testrole" outerId="21" outerCode="testrole" outerType="Role" />
        </sf:performers>
      </bpmn:extensionElements>
      <bpmn:incoming>Flow_3720</bpmn:incoming>
      <bpmn:outgoing>Flow_1562</bpmn:outgoing>
    </bpmn:task>
    <bpmn:exclusiveGateway id="GatewayNode_2837" sf:code="xorsplit001" name="XOr-Split">
      <bpmn:incoming>Flow_1562</bpmn:incoming>
      <bpmn:outgoing>Flow_9167</bpmn:outgoing>
      <bpmn:outgoing>Flow_9742</bpmn:outgoing>
    </bpmn:exclusiveGateway>
    <bpmn:task id="TaskNode_1488" sf:code="task010" name="Dept Manager Approval">
      <bpmn:incoming>Flow_9167</bpmn:incoming>
      <bpmn:outgoing>Flow_8802</bpmn:outgoing>
    </bpmn:task>
    <bpmn:task id="TaskNode_3135" sf:code="task020" name="CEO Approval">
      <bpmn:incoming>Flow_9742</bpmn:incoming>
      <bpmn:outgoing>Flow_5731</bpmn:outgoing>
    </bpmn:task>
    <bpmn:exclusiveGateway id="GatewayNode_3469" sf:code="xorjoin001" name="XOr-Join">
      <bpmn:incoming>Flow_5731</bpmn:incoming>
      <bpmn:incoming>Flow_8802</bpmn:incoming>
      <bpmn:outgoing>Flow_9599</bpmn:outgoing>
    </bpmn:exclusiveGateway>
    <bpmn:task id="TaskNode_5646" sf:code="task100" name="HR Approval">
      <bpmn:incoming>Flow_9599</bpmn:incoming>
      <bpmn:outgoing>Flow_3189</bpmn:outgoing>
    </bpmn:task>
    <bpmn:endEvent id="EndNode_7404" sf:code="End" name="end">
      <bpmn:incoming>Flow_3189</bpmn:incoming>
    </bpmn:endEvent>
    <bpmn:sequenceFlow id="Flow_3720" name="" sourceRef="StartNode_6174" targetRef="TaskNode_3574" />
    <bpmn:sequenceFlow id="Flow_1562" name="" sourceRef="TaskNode_3574" targetRef="GatewayNode_2837" />
    <bpmn:sequenceFlow id="Flow_9167" name="days&#60;3" sourceRef="GatewayNode_2837" targetRef="TaskNode_1488">
      <bpmn:conditionExpression>days&lt;3</bpmn:conditionExpression>
    </bpmn:sequenceFlow>
    <bpmn:sequenceFlow id="Flow_9742" name="days&#62;=3" sourceRef="GatewayNode_2837" targetRef="TaskNode_3135">
      <bpmn:conditionExpression>days&gt;=3</bpmn:conditionExpression>
    </bpmn:sequenceFlow>
    <bpmn:sequenceFlow id="Flow_5731" name="" sourceRef="TaskNode_3135" targetRef="GatewayNode_3469" />
    <bpmn:sequenceFlow id="Flow_8802" name="" sourceRef="TaskNode_1488" targetRef="GatewayNode_3469" />
    <bpmn:sequenceFlow id="Flow_9599" name="" sourceRef="GatewayNode_3469" targetRef="TaskNode_5646" />
    <bpmn:sequenceFlow id="Flow_3189" name="" sourceRef="TaskNode_5646" targetRef="EndNode_7404" />
  </bpmn:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_koxx_1942">
      <bpmndi:BPMNShape id="BPMNShape_yp7ubt8_di" bpmnElement="StartNode_6174">
        <dc:Bounds x="240" y="180" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="BPMNShape_jdaby0d_di" bpmnElement="TaskNode_3574">
        <dc:Bounds x="356" y="158" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="BPMNShape_at83gxq_di" bpmnElement="GatewayNode_2837" isMarkerVisible="true">
        <dc:Bounds x="536" y="180" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="BPMNShape_fo4ix5u_di" bpmnElement="TaskNode_1488">
        <dc:Bounds x="652" y="230" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="BPMNShape_zo1pjcd_di" bpmnElement="TaskNode_3135">
        <dc:Bounds x="652" y="70" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="BPMNShape_nwmoy8g_di" bpmnElement="GatewayNode_3469" isMarkerVisible="true">
        <dc:Bounds x="832" y="180" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="BPMNShape_dvetvf8_di" bpmnElement="TaskNode_5646">
        <dc:Bounds x="948" y="158" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="BPMNShape_pjj639t_di" bpmnElement="EndNode_7404">
        <dc:Bounds x="1128" y="180" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNEdge id="Flow_3720_di" bpmnElement="Flow_3720">
        <di:waypoint x="276" y="198" />
        <di:waypoint x="356" y="198" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1562_di" bpmnElement="Flow_1562">
        <di:waypoint x="456" y="198" />
        <di:waypoint x="536" y="198" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_9167_di" bpmnElement="Flow_9167">
        <di:waypoint x="554" y="216" />
        <di:waypoint x="554" y="270" />
        <di:waypoint x="652" y="270" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_9742_di" bpmnElement="Flow_9742">
        <di:waypoint x="554" y="216" />
        <di:waypoint x="554" y="110" />
        <di:waypoint x="652" y="110" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_5731_di" bpmnElement="Flow_5731">
        <di:waypoint x="752" y="110" />
        <di:waypoint x="850" y="110" />
        <di:waypoint x="850" y="180" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_8802_di" bpmnElement="Flow_8802">
        <di:waypoint x="752" y="270" />
        <di:waypoint x="850" y="270" />
        <di:waypoint x="850" y="216" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_9599_di" bpmnElement="Flow_9599">
        <di:waypoint x="868" y="198" />
        <di:waypoint x="948" y="198" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_3189_di" bpmnElement="Flow_3189">
        <di:waypoint x="1048" y="198" />
        <di:waypoint x="1128" y="198" />
      </bpmndi:BPMNEdge>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn:definitions>
', 0, NULL, NULL, 0, NULL, CAST(0x0000B28E01096AE9 AS DateTime), CAST(0x0000B29100F89E11 AS DateTime))
INSERT [dbo].[WfProcess] ([ID], [ProcessID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1613, N'WashEyeEquipmentCheckProcess_8f0x', N'1', N'洗眼器检查流程', N'WashEyeEquipmentCheckProcess_8f0x', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="utf-8"?><bpmn:definitions xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL"><bpmn:process id="WashEyeEquipmentCheckProcess_8f0x" name="洗眼器检查流程" isExecutable="false"><bpmn:startEvent id="AStartEvent_1" name="开始" /><bpmn:task id="ATask_1" name="启动检查" /><bpmn:task id="ATask_2" name="外观检查" /><bpmn:task id="ATask_3" name="功能测试" /><bpmn:exclusiveGateway id="AGateway_1" name="检查结果" /><bpmn:task id="ATask_4" name="记录检查结果" /><bpmn:task id="ATask_5" name="安排维护" /><bpmn:endEvent id="AEndEvent_1" name="结束" /><bpmn:sequenceFlow id="Flow_AStartEvent_1_ATask_1" sourceRef="AStartEvent_1" targetRef="ATask_1" /><bpmn:sequenceFlow id="Flow_ATask_1_ATask_2" sourceRef="ATask_1" targetRef="ATask_2" /><bpmn:sequenceFlow id="Flow_ATask_2_ATask_3" sourceRef="ATask_2" targetRef="ATask_3" /><bpmn:sequenceFlow id="Flow_ATask_3_AGateway_1" sourceRef="ATask_3" targetRef="AGateway_1" /><bpmn:sequenceFlow id="Flow_AGateway_1_ATask_4" sourceRef="AGateway_1" targetRef="ATask_4" /><bpmn:sequenceFlow id="Flow_AGateway_1_ATask_5" sourceRef="AGateway_1" targetRef="ATask_5" /><bpmn:sequenceFlow id="Flow_ATask_4_AEndEvent_1" sourceRef="ATask_4" targetRef="AEndEvent_1" /><bpmn:sequenceFlow id="Flow_ATask_5_AEndEvent_1" sourceRef="ATask_5" targetRef="AEndEvent_1" /></bpmn:process><bpmndi:BPMNDiagram id="BPMNDiagram_1"><bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="WashEyeEquipmentCheckProcess_8f0x"><bpmndi:BPMNShape id="Shape_AStartEvent_1" bpmnElement="AStartEvent_1"><dc:Bounds x="100" y="100" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_ATask_1" bpmnElement="ATask_1"><dc:Bounds x="300" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_ATask_2" bpmnElement="ATask_2"><dc:Bounds x="500" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_ATask_3" bpmnElement="ATask_3"><dc:Bounds x="700" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AGateway_1" bpmnElement="AGateway_1"><dc:Bounds x="900" y="100" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_ATask_4" bpmnElement="ATask_4"><dc:Bounds x="1100" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_ATask_5" bpmnElement="ATask_5"><dc:Bounds x="1100" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AEndEvent_1" bpmnElement="AEndEvent_1"><dc:Bounds x="1300" y="100" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNEdge id="Edge_AStartEvent_1_ATask_1" bpmnElement="Flow_AStartEvent_1_ATask_1"><di:waypoint x="136" y="118" /><di:waypoint x="218" y="118" /><di:waypoint x="300" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_ATask_1_ATask_2" bpmnElement="Flow_ATask_1_ATask_2"><di:waypoint x="400" y="140" /><di:waypoint x="450" y="140" /><di:waypoint x="500" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_ATask_2_ATask_3" bpmnElement="Flow_ATask_2_ATask_3"><di:waypoint x="600" y="140" /><di:waypoint x="650" y="140" /><di:waypoint x="700" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_ATask_3_AGateway_1" bpmnElement="Flow_ATask_3_AGateway_1"><di:waypoint x="800" y="140" /><di:waypoint x="850" y="140" /><di:waypoint x="900" y="118" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AGateway_1_ATask_4" bpmnElement="Flow_AGateway_1_ATask_4"><di:waypoint x="936" y="118" /><di:waypoint x="1018" y="118" /><di:waypoint x="1100" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AGateway_1_ATask_5" bpmnElement="Flow_AGateway_1_ATask_5"><di:waypoint x="936" y="118" /><di:waypoint x="1018" y="118" /><di:waypoint x="1100" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_ATask_4_AEndEvent_1" bpmnElement="Flow_ATask_4_AEndEvent_1"><di:waypoint x="1200" y="140" /><di:waypoint x="1250" y="140" /><di:waypoint x="1300" y="118" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_ATask_5_AEndEvent_1" bpmnElement="Flow_ATask_5_AEndEvent_1"><di:waypoint x="1200" y="140" /><di:waypoint x="1250" y="140" /><di:waypoint x="1300" y="118" /></bpmndi:BPMNEdge></bpmndi:BPMNPlane></bpmndi:BPMNDiagram></bpmn:definitions>', 0, NULL, NULL, 0, NULL, CAST(0x0000B28F00B58EC8 AS DateTime), NULL)
INSERT [dbo].[WfProcess] ([ID], [ProcessID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1614, N'employee_leave_process_jebj', N'1', N'员工请假流程', N'employee_leave_process_jebj', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="utf-8"?><bpmn:definitions xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL"><bpmn:process id="employee_leave_process_jebj" name="员工请假流程" isExecutable="false"><bpmn:startEvent id="AstartEvent" name="开始" /><bpmn:userTask id="AsubmitApplication" name="提交请假申请" assignee="" /><bpmn:userTask id="AdepartmentApproval" name="部门负责人审批" assignee="" /><bpmn:userTask id="AhrApproval" name="人力资源部审批" assignee="" /><bpmn:endEvent id="Aapprove" name="批准" /><bpmn:endEvent id="Areject" name="拒绝" /><bpmn:endEvent id="AendEvent" name="结束" /><bpmn:sequenceFlow id="Flow_AstartEvent_AsubmitApplication" sourceRef="AstartEvent" targetRef="AsubmitApplication" /><bpmn:sequenceFlow id="Flow_AsubmitApplication_AdepartmentApproval" sourceRef="AsubmitApplication" targetRef="AdepartmentApproval" /><bpmn:sequenceFlow id="Flow_AdepartmentApproval_AhrApproval" sourceRef="AdepartmentApproval" targetRef="AhrApproval"><bpmn:conditionExpression>${approverDecision == ''approve''}</bpmn:conditionExpression></bpmn:sequenceFlow><bpmn:sequenceFlow id="Flow_AdepartmentApproval_Areject" sourceRef="AdepartmentApproval" targetRef="Areject"><bpmn:conditionExpression>${approverDecision == ''reject''}</bpmn:conditionExpression></bpmn:sequenceFlow><bpmn:sequenceFlow id="Flow_AhrApproval_Aapprove" sourceRef="AhrApproval" targetRef="Aapprove"><bpmn:conditionExpression>${hrDecision == ''approve''}</bpmn:conditionExpression></bpmn:sequenceFlow><bpmn:sequenceFlow id="Flow_AhrApproval_Areject" sourceRef="AhrApproval" targetRef="Areject"><bpmn:conditionExpression>${hrDecision == ''reject''}</bpmn:conditionExpression></bpmn:sequenceFlow><bpmn:sequenceFlow id="Flow_Aapprove_AendEvent" sourceRef="Aapprove" targetRef="AendEvent" /><bpmn:sequenceFlow id="Flow_Areject_AendEvent" sourceRef="Areject" targetRef="AendEvent" /></bpmn:process><bpmndi:BPMNDiagram id="BPMNDiagram_1"><bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="employee_leave_process_jebj"><bpmndi:BPMNShape id="Shape_AstartEvent" bpmnElement="AstartEvent"><dc:Bounds x="100" y="100" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AsubmitApplication" bpmnElement="AsubmitApplication"><dc:Bounds x="300" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AdepartmentApproval" bpmnElement="AdepartmentApproval"><dc:Bounds x="500" y="150" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AhrApproval" bpmnElement="AhrApproval"><dc:Bounds x="700" y="50" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_Aapprove" bpmnElement="Aapprove"><dc:Bounds x="900" y="50" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_Areject" bpmnElement="Areject"><dc:Bounds x="700" y="50" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AendEvent" bpmnElement="AendEvent"><dc:Bounds x="900" y="50" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNEdge id="Edge_AstartEvent_AsubmitApplication" bpmnElement="Flow_AstartEvent_AsubmitApplication"><di:waypoint x="136" y="118" /><di:waypoint x="218" y="118" /><di:waypoint x="300" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AsubmitApplication_AdepartmentApproval" bpmnElement="Flow_AsubmitApplication_AdepartmentApproval"><di:waypoint x="400" y="140" /><di:waypoint x="450" y="140" /><di:waypoint x="450" y="190" /><di:waypoint x="500" y="190" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AdepartmentApproval_AhrApproval" bpmnElement="Flow_AdepartmentApproval_AhrApproval"><di:waypoint x="600" y="190" /><di:waypoint x="650" y="190" /><di:waypoint x="650" y="90" /><di:waypoint x="700" y="90" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AdepartmentApproval_Areject" bpmnElement="Flow_AdepartmentApproval_Areject"><di:waypoint x="600" y="190" /><di:waypoint x="650" y="190" /><di:waypoint x="650" y="68" /><di:waypoint x="700" y="68" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AhrApproval_Aapprove" bpmnElement="Flow_AhrApproval_Aapprove"><di:waypoint x="800" y="90" /><di:waypoint x="850" y="90" /><di:waypoint x="900" y="68" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AhrApproval_Areject" bpmnElement="Flow_AhrApproval_Areject"><di:waypoint x="800" y="90" /><di:waypoint x="750" y="90" /><di:waypoint x="700" y="68" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_Aapprove_AendEvent" bpmnElement="Flow_Aapprove_AendEvent"><di:waypoint x="936" y="68" /><di:waypoint x="918" y="68" /><di:waypoint x="900" y="68" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_Areject_AendEvent" bpmnElement="Flow_Areject_AendEvent"><di:waypoint x="736" y="68" /><di:waypoint x="818" y="68" /><di:waypoint x="900" y="68" /></bpmndi:BPMNEdge></bpmndi:BPMNPlane></bpmndi:BPMNDiagram></bpmn:definitions>', 0, NULL, NULL, 0, NULL, CAST(0x0000B291010050C2 AS DateTime), NULL)
INSERT [dbo].[WfProcess] ([ID], [ProcessID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1615, N'employee_leave_process_y3g6', N'1', N'员工请假流程', N'employee_leave_process_y3g6', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="utf-8"?><bpmn:definitions xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL"><bpmn:process id="employee_leave_process_y3g6" name="员工请假流程" isExecutable="false"><bpmn:startEvent id="Astart_event" name="开始" /><bpmn:userTask id="Asubmit_request" name="提交请假申请" assignee="" /><bpmn:inclusiveGateway id="Asupervisor_approval" name="直接上级审批" /><bpmn:inclusiveGateway id="Adepartment_manager_approval" name="部门主管审批" /><bpmn:userTask id="Ahr_notification" name="HR处理并通知结果" assignee="" /><bpmn:endEvent id="Aend_event_approved" name="审批通过" /><bpmn:endEvent id="Aend_event_rejected" name="审批未通过" /><bpmn:userTask id="Afeedback_comment" name="反馈意见" assignee="" /><bpmn:sequenceFlow id="Flow_Astart_event_Asubmit_request" sourceRef="Astart_event" targetRef="Asubmit_request" /><bpmn:sequenceFlow id="Flow_Asubmit_request_Asupervisor_approval" sourceRef="Asubmit_request" targetRef="Asupervisor_approval" /><bpmn:sequenceFlow id="Flow_Asupervisor_approval_Adepartment_manager_approval" sourceRef="Asupervisor_approval" targetRef="Adepartment_manager_approval" /><bpmn:sequenceFlow id="Flow_Asupervisor_approval_Aend_event_rejected" sourceRef="Asupervisor_approval" targetRef="Aend_event_rejected" /><bpmn:sequenceFlow id="Flow_Adepartment_manager_approval_Ahr_notification" sourceRef="Adepartment_manager_approval" targetRef="Ahr_notification" /><bpmn:sequenceFlow id="Flow_Adepartment_manager_approval_Aend_event_rejected" sourceRef="Adepartment_manager_approval" targetRef="Aend_event_rejected" /><bpmn:sequenceFlow id="Flow_Ahr_notification_Aend_event_approved" sourceRef="Ahr_notification" targetRef="Aend_event_approved" /><bpmn:sequenceFlow id="Flow_Afeedback_comment_Aend_event_rejected" sourceRef="Afeedback_comment" targetRef="Aend_event_rejected" /></bpmn:process><bpmndi:BPMNDiagram id="BPMNDiagram_1"><bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="employee_leave_process_y3g6"><bpmndi:BPMNShape id="Shape_Astart_event" bpmnElement="Astart_event"><dc:Bounds x="100" y="100" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_Asubmit_request" bpmnElement="Asubmit_request"><dc:Bounds x="300" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_Asupervisor_approval" bpmnElement="Asupervisor_approval"><dc:Bounds x="500" y="150" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_Adepartment_manager_approval" bpmnElement="Adepartment_manager_approval"><dc:Bounds x="700" y="150" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_Ahr_notification" bpmnElement="Ahr_notification"><dc:Bounds x="900" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_Aend_event_approved" bpmnElement="Aend_event_approved"><dc:Bounds x="1100" y="100" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_Aend_event_rejected" bpmnElement="Aend_event_rejected"><dc:Bounds x="700" y="150" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_Afeedback_comment" bpmnElement="Afeedback_comment"><dc:Bounds x="0" y="150" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNEdge id="Edge_Astart_event_Asubmit_request" bpmnElement="Flow_Astart_event_Asubmit_request"><di:waypoint x="136" y="118" /><di:waypoint x="218" y="118" /><di:waypoint x="300" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_Asubmit_request_Asupervisor_approval" bpmnElement="Flow_Asubmit_request_Asupervisor_approval"><di:waypoint x="400" y="140" /><di:waypoint x="450" y="140" /><di:waypoint x="450" y="168" /><di:waypoint x="500" y="168" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_Asupervisor_approval_Adepartment_manager_approval" bpmnElement="Flow_Asupervisor_approval_Adepartment_manager_approval"><di:waypoint x="536" y="168" /><di:waypoint x="618" y="168" /><di:waypoint x="700" y="168" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_Asupervisor_approval_Aend_event_rejected" bpmnElement="Flow_Asupervisor_approval_Aend_event_rejected"><di:waypoint x="536" y="168" /><di:waypoint x="618" y="168" /><di:waypoint x="700" y="168" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_Adepartment_manager_approval_Ahr_notification" bpmnElement="Flow_Adepartment_manager_approval_Ahr_notification"><di:waypoint x="736" y="168" /><di:waypoint x="786" y="168" /><di:waypoint x="786" y="140" /><di:waypoint x="900" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_Adepartment_manager_approval_Aend_event_rejected" bpmnElement="Flow_Adepartment_manager_approval_Aend_event_rejected"><di:waypoint x="736" y="168" /><di:waypoint x="718" y="168" /><di:waypoint x="700" y="168" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_Ahr_notification_Aend_event_approved" bpmnElement="Flow_Ahr_notification_Aend_event_approved"><di:waypoint x="1000" y="140" /><di:waypoint x="1050" y="140" /><di:waypoint x="1100" y="118" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_Afeedback_comment_Aend_event_rejected" bpmnElement="Flow_Afeedback_comment_Aend_event_rejected"><di:waypoint x="100" y="190" /><di:waypoint x="400" y="190" /><di:waypoint x="700" y="168" /></bpmndi:BPMNEdge></bpmndi:BPMNPlane></bpmndi:BPMNDiagram></bpmn:definitions>', 0, NULL, NULL, 0, NULL, CAST(0x0000B291013121F4 AS DateTime), NULL)
INSERT [dbo].[WfProcess] ([ID], [ProcessID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1616, N'employee_leave_process_qq5s', N'1', N'员工请假流程', N'employee_leave_process_qq5s', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="utf-8"?><bpmn:definitions xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL"><bpmn:process id="employee_leave_process_qq5s" name="员工请假流程" isExecutable="false"><bpmn:startEvent id="Astart_event" name="开始" /><bpmn:userTask id="Asubmit_request" name="提交请假申请" assignee="" /><bpmn:inclusiveGateway id="Asupervisor_approval" name="直接上级审批" /><bpmn:inclusiveGateway id="Adepartment_manager_approval" name="部门主管审批" /><bpmn:userTask id="Ahr_notification" name="HR处理并通知结果" assignee="" /><bpmn:endEvent id="Aend_event_approved" name="审批通过" /><bpmn:endEvent id="Aend_event_rejected" name="审批未通过" /><bpmn:userTask id="Afeedback_comment" name="反馈意见" assignee="" /><bpmn:sequenceFlow id="Flow_Astart_event_Asubmit_request" sourceRef="Astart_event" targetRef="Asubmit_request" /><bpmn:sequenceFlow id="Flow_Asubmit_request_Asupervisor_approval" sourceRef="Asubmit_request" targetRef="Asupervisor_approval" /><bpmn:sequenceFlow id="Flow_Asupervisor_approval_Adepartment_manager_approval" sourceRef="Asupervisor_approval" targetRef="Adepartment_manager_approval" /><bpmn:sequenceFlow id="Flow_Asupervisor_approval_Aend_event_rejected" sourceRef="Asupervisor_approval" targetRef="Aend_event_rejected" /><bpmn:sequenceFlow id="Flow_Adepartment_manager_approval_Ahr_notification" sourceRef="Adepartment_manager_approval" targetRef="Ahr_notification" /><bpmn:sequenceFlow id="Flow_Adepartment_manager_approval_Aend_event_rejected" sourceRef="Adepartment_manager_approval" targetRef="Aend_event_rejected" /><bpmn:sequenceFlow id="Flow_Ahr_notification_Aend_event_approved" sourceRef="Ahr_notification" targetRef="Aend_event_approved" /><bpmn:sequenceFlow id="Flow_Afeedback_comment_Aend_event_rejected" sourceRef="Afeedback_comment" targetRef="Aend_event_rejected" /></bpmn:process><bpmndi:BPMNDiagram id="BPMNDiagram_1"><bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="employee_leave_process_qq5s"><bpmndi:BPMNShape id="Shape_Astart_event" bpmnElement="Astart_event"><dc:Bounds x="100" y="100" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_Asubmit_request" bpmnElement="Asubmit_request"><dc:Bounds x="300" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_Asupervisor_approval" bpmnElement="Asupervisor_approval"><dc:Bounds x="500" y="150" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_Adepartment_manager_approval" bpmnElement="Adepartment_manager_approval"><dc:Bounds x="700" y="150" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_Ahr_notification" bpmnElement="Ahr_notification"><dc:Bounds x="900" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_Aend_event_approved" bpmnElement="Aend_event_approved"><dc:Bounds x="1100" y="100" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_Aend_event_rejected" bpmnElement="Aend_event_rejected"><dc:Bounds x="700" y="150" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_Afeedback_comment" bpmnElement="Afeedback_comment"><dc:Bounds x="500" y="150" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNEdge id="Edge_Astart_event_Asubmit_request" bpmnElement="Flow_Astart_event_Asubmit_request"><di:waypoint x="136" y="118" /><di:waypoint x="218" y="118" /><di:waypoint x="300" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_Asubmit_request_Asupervisor_approval" bpmnElement="Flow_Asubmit_request_Asupervisor_approval"><di:waypoint x="400" y="140" /><di:waypoint x="450" y="140" /><di:waypoint x="450" y="168" /><di:waypoint x="500" y="168" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_Asupervisor_approval_Adepartment_manager_approval" bpmnElement="Flow_Asupervisor_approval_Adepartment_manager_approval"><di:waypoint x="536" y="168" /><di:waypoint x="618" y="168" /><di:waypoint x="700" y="168" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_Asupervisor_approval_Aend_event_rejected" bpmnElement="Flow_Asupervisor_approval_Aend_event_rejected"><di:waypoint x="536" y="168" /><di:waypoint x="618" y="168" /><di:waypoint x="700" y="168" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_Adepartment_manager_approval_Ahr_notification" bpmnElement="Flow_Adepartment_manager_approval_Ahr_notification"><di:waypoint x="736" y="168" /><di:waypoint x="786" y="168" /><di:waypoint x="786" y="140" /><di:waypoint x="900" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_Adepartment_manager_approval_Aend_event_rejected" bpmnElement="Flow_Adepartment_manager_approval_Aend_event_rejected"><di:waypoint x="736" y="168" /><di:waypoint x="718" y="168" /><di:waypoint x="700" y="168" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_Ahr_notification_Aend_event_approved" bpmnElement="Flow_Ahr_notification_Aend_event_approved"><di:waypoint x="1000" y="140" /><di:waypoint x="1050" y="140" /><di:waypoint x="1100" y="118" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_Afeedback_comment_Aend_event_rejected" bpmnElement="Flow_Afeedback_comment_Aend_event_rejected"><di:waypoint x="600" y="190" /><di:waypoint x="650" y="190" /><di:waypoint x="700" y="168" /></bpmndi:BPMNEdge></bpmndi:BPMNPlane></bpmndi:BPMNDiagram></bpmn:definitions>', 0, NULL, NULL, 0, NULL, CAST(0x0000B291013606F0 AS DateTime), NULL)
INSERT [dbo].[WfProcess] ([ID], [ProcessID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1617, N'employee_leave_process_vvde', N'1', N'员工请假流程', N'employee_leave_process_vvde', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="utf-8"?><bpmn:definitions xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL"><bpmn:process id="employee_leave_process_vvde" name="员工请假流程" isExecutable="false"><bpmn:startEvent id="Astart_event" name="开始" /><bpmn:userTask id="Asubmit_request" name="提交请假申请" assignee="" /><bpmn:inclusiveGateway id="Asupervisor_approval" name="直接上级审批" /><bpmn:inclusiveGateway id="Adepartment_manager_approval" name="部门主管审批" /><bpmn:userTask id="Ahr_notification" name="HR处理并通知结果" assignee="" /><bpmn:endEvent id="Aend_event_approved" name="审批通过" /><bpmn:endEvent id="Aend_event_rejected" name="审批未通过" /><bpmn:userTask id="Afeedback_comment" name="反馈意见" assignee="" /><bpmn:sequenceFlow id="Flow_Astart_event_Asubmit_request" sourceRef="Astart_event" targetRef="Asubmit_request" /><bpmn:sequenceFlow id="Flow_Asubmit_request_Asupervisor_approval" sourceRef="Asubmit_request" targetRef="Asupervisor_approval" /><bpmn:sequenceFlow id="Flow_Asupervisor_approval_Adepartment_manager_approval" sourceRef="Asupervisor_approval" targetRef="Adepartment_manager_approval" /><bpmn:sequenceFlow id="Flow_Asupervisor_approval_Aend_event_rejected" sourceRef="Asupervisor_approval" targetRef="Aend_event_rejected" /><bpmn:sequenceFlow id="Flow_Adepartment_manager_approval_Ahr_notification" sourceRef="Adepartment_manager_approval" targetRef="Ahr_notification" /><bpmn:sequenceFlow id="Flow_Adepartment_manager_approval_Aend_event_rejected" sourceRef="Adepartment_manager_approval" targetRef="Aend_event_rejected" /><bpmn:sequenceFlow id="Flow_Ahr_notification_Aend_event_approved" sourceRef="Ahr_notification" targetRef="Aend_event_approved" /><bpmn:sequenceFlow id="Flow_Afeedback_comment_Aend_event_rejected" sourceRef="Afeedback_comment" targetRef="Aend_event_rejected" /></bpmn:process><bpmndi:BPMNDiagram id="BPMNDiagram_1"><bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="employee_leave_process_vvde"><bpmndi:BPMNShape id="Shape_Astart_event" bpmnElement="Astart_event"><dc:Bounds x="100" y="100" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_Asubmit_request" bpmnElement="Asubmit_request"><dc:Bounds x="300" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_Asupervisor_approval" bpmnElement="Asupervisor_approval"><dc:Bounds x="500" y="250" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_Adepartment_manager_approval" bpmnElement="Adepartment_manager_approval"><dc:Bounds x="700" y="250" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_Ahr_notification" bpmnElement="Ahr_notification"><dc:Bounds x="900" y="200" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_Aend_event_approved" bpmnElement="Aend_event_approved"><dc:Bounds x="1100" y="100" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_Aend_event_rejected" bpmnElement="Aend_event_rejected"><dc:Bounds x="700" y="150" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_Afeedback_comment" bpmnElement="Afeedback_comment"><dc:Bounds x="500" y="250" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNEdge id="Edge_Astart_event_Asubmit_request" bpmnElement="Flow_Astart_event_Asubmit_request"><di:waypoint x="136" y="118" /><di:waypoint x="218" y="118" /><di:waypoint x="300" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_Asubmit_request_Asupervisor_approval" bpmnElement="Flow_Asubmit_request_Asupervisor_approval"><di:waypoint x="400" y="140" /><di:waypoint x="450" y="140" /><di:waypoint x="450" y="268" /><di:waypoint x="500" y="268" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_Asupervisor_approval_Adepartment_manager_approval" bpmnElement="Flow_Asupervisor_approval_Adepartment_manager_approval"><di:waypoint x="536" y="268" /><di:waypoint x="618" y="268" /><di:waypoint x="700" y="268" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_Asupervisor_approval_Aend_event_rejected" bpmnElement="Flow_Asupervisor_approval_Aend_event_rejected"><di:waypoint x="536" y="268" /><di:waypoint x="586" y="268" /><di:waypoint x="586" y="168" /><di:waypoint x="700" y="168" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_Adepartment_manager_approval_Ahr_notification" bpmnElement="Flow_Adepartment_manager_approval_Ahr_notification"><di:waypoint x="736" y="268" /><di:waypoint x="786" y="268" /><di:waypoint x="786" y="240" /><di:waypoint x="900" y="240" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_Adepartment_manager_approval_Aend_event_rejected" bpmnElement="Flow_Adepartment_manager_approval_Aend_event_rejected"><di:waypoint x="736" y="268" /><di:waypoint x="786" y="268" /><di:waypoint x="786" y="168" /><di:waypoint x="700" y="168" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_Ahr_notification_Aend_event_approved" bpmnElement="Flow_Ahr_notification_Aend_event_approved"><di:waypoint x="1000" y="240" /><di:waypoint x="1050" y="240" /><di:waypoint x="1050" y="118" /><di:waypoint x="1100" y="118" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_Afeedback_comment_Aend_event_rejected" bpmnElement="Flow_Afeedback_comment_Aend_event_rejected"><di:waypoint x="600" y="290" /><di:waypoint x="650" y="290" /><di:waypoint x="650" y="168" /><di:waypoint x="700" y="168" /></bpmndi:BPMNEdge></bpmndi:BPMNPlane></bpmndi:BPMNDiagram></bpmn:definitions>', 0, NULL, NULL, 0, NULL, CAST(0x0000B291013640F0 AS DateTime), NULL)
INSERT [dbo].[WfProcess] ([ID], [ProcessID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1618, N'employeeLeaveRequestProcess_aob9', N'1', N'员工请假流程', N'employeeLeaveRequestProcess_aob9', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn:definitions xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL">
  <bpmn:process id="employeeLeaveRequestProcess_aob9" name="员工请假流程" isExecutable="false">
    <bpmn:startEvent id="AstartEvent" name="流程开始" />
    <bpmn:userTask id="AsubmitApplication" name="提交请假申请" assignee="" />
    <bpmn:userTask id="AdepartmentManagerApproval" name="部门经理审批" assignee="" />
    <bpmn:serviceTask id="AHRConfirmation" name="HR确认" implementation="" />
    <bpmn:userTask id="AresultNotification" name="通知结果" assignee="" />
    <bpmn:userTask id="AapprovalRejected" name="审批驳回" assignee="" />
    <bpmn:endEvent id="AendEvent" name="流程结束" />
    <bpmn:sequenceFlow id="Flow_AstartEvent_AsubmitApplication" sourceRef="AstartEvent" targetRef="AsubmitApplication" />
    <bpmn:sequenceFlow id="Flow_AsubmitApplication_AdepartmentManagerApproval" sourceRef="AsubmitApplication" targetRef="AdepartmentManagerApproval" />
    <bpmn:sequenceFlow id="Flow_AdepartmentManagerApproval_AHRConfirmation" sourceRef="AdepartmentManagerApproval" targetRef="AHRConfirmation" />
    <bpmn:sequenceFlow id="Flow_AHRConfirmation_AresultNotification" sourceRef="AHRConfirmation" targetRef="AresultNotification" />
    <bpmn:sequenceFlow id="Flow_AresultNotification_AendEvent" sourceRef="AresultNotification" targetRef="AendEvent" />
    <bpmn:sequenceFlow id="Flow_AdepartmentManagerApproval_AapprovalRejected" sourceRef="AdepartmentManagerApproval" targetRef="AapprovalRejected">
      <bpmn:conditionExpression>approval != ''通过''</bpmn:conditionExpression>
    </bpmn:sequenceFlow>
    <bpmn:sequenceFlow id="Flow_AapprovalRejected_AendEvent" sourceRef="AapprovalRejected" targetRef="AendEvent" />
  </bpmn:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="employeeLeaveRequestProcess_aob9">
      <bpmndi:BPMNShape id="Shape_AstartEvent" bpmnElement="AstartEvent">
        <dc:Bounds x="100" y="100" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Shape_AsubmitApplication" bpmnElement="AsubmitApplication">
        <dc:Bounds x="300" y="100" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Shape_AdepartmentManagerApproval" bpmnElement="AdepartmentManagerApproval">
        <dc:Bounds x="500" y="100" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Shape_AHRConfirmation" bpmnElement="AHRConfirmation">
        <dc:Bounds x="700" y="50" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Shape_AapprovalRejected" bpmnElement="AapprovalRejected">
        <dc:Bounds x="700" y="250" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Shape_AendEvent" bpmnElement="AendEvent">
        <dc:Bounds x="1132" y="272" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="1128" y="308" width="44" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Shape_AresultNotification" bpmnElement="AresultNotification">
        <dc:Bounds x="950" y="130" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNEdge id="Edge_AstartEvent_AsubmitApplication" bpmnElement="Flow_AstartEvent_AsubmitApplication">
        <di:waypoint x="136" y="118" />
        <di:waypoint x="218" y="118" />
        <di:waypoint x="300" y="140" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Edge_AsubmitApplication_AdepartmentManagerApproval" bpmnElement="Flow_AsubmitApplication_AdepartmentManagerApproval">
        <di:waypoint x="400" y="140" />
        <di:waypoint x="450" y="140" />
        <di:waypoint x="500" y="140" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Edge_AdepartmentManagerApproval_AHRConfirmation" bpmnElement="Flow_AdepartmentManagerApproval_AHRConfirmation">
        <di:waypoint x="600" y="140" />
        <di:waypoint x="650" y="140" />
        <di:waypoint x="650" y="90" />
        <di:waypoint x="700" y="90" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Edge_AHRConfirmation_AresultNotification" bpmnElement="Flow_AHRConfirmation_AresultNotification">
        <di:waypoint x="800" y="90" />
        <di:waypoint x="850" y="90" />
        <di:waypoint x="850" y="170" />
        <di:waypoint x="950" y="170" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Edge_AresultNotification_AendEvent" bpmnElement="Flow_AresultNotification_AendEvent">
        <di:waypoint x="1050" y="170" />
        <di:waypoint x="1091" y="170" />
        <di:waypoint x="1091" y="290" />
        <di:waypoint x="1132" y="290" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Edge_AdepartmentManagerApproval_AapprovalRejected" bpmnElement="Flow_AdepartmentManagerApproval_AapprovalRejected">
        <di:waypoint x="600" y="140" />
        <di:waypoint x="650" y="140" />
        <di:waypoint x="650" y="290" />
        <di:waypoint x="700" y="290" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Edge_AapprovalRejected_AendEvent" bpmnElement="Flow_AapprovalRejected_AendEvent">
        <di:waypoint x="800" y="290" />
        <di:waypoint x="1132" y="290" />
      </bpmndi:BPMNEdge>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn:definitions>
', 0, NULL, NULL, 0, NULL, CAST(0x0000B2910144C34D AS DateTime), CAST(0x0000B2910144E1B7 AS DateTime))
INSERT [dbo].[WfProcess] ([ID], [ProcessID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1619, N'employeeLeaveRequestProcess_hgt7', N'1', N'员工请假流程', N'employeeLeaveRequestProcess_hgt7', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="utf-8"?><bpmn:definitions xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL"><bpmn:process id="employeeLeaveRequestProcess_hgt7" name="员工请假流程" isExecutable="false"><bpmn:startEvent id="AstartEvent" name="流程开始" /><bpmn:userTask id="AsubmitApplication" name="提交请假申请" assignee="" /><bpmn:userTask id="AdepartmentManagerApproval" name="部门经理审批" assignee="" /><bpmn:serviceTask id="AHRConfirmation" name="HR确认" implementation="" /><bpmn:userTask id="AresultNotification" name="通知结果" assignee="" /><bpmn:userTask id="AapprovalRejected" name="审批驳回" assignee="" /><bpmn:endEvent id="AendEvent" name="流程结束" /><bpmn:sequenceFlow id="Flow_AstartEvent_AsubmitApplication" sourceRef="AstartEvent" targetRef="AsubmitApplication" /><bpmn:sequenceFlow id="Flow_AsubmitApplication_AdepartmentManagerApproval" sourceRef="AsubmitApplication" targetRef="AdepartmentManagerApproval" /><bpmn:sequenceFlow id="Flow_AdepartmentManagerApproval_AHRConfirmation" sourceRef="AdepartmentManagerApproval" targetRef="AHRConfirmation" /><bpmn:sequenceFlow id="Flow_AHRConfirmation_AresultNotification" sourceRef="AHRConfirmation" targetRef="AresultNotification" /><bpmn:sequenceFlow id="Flow_AresultNotification_AendEvent" sourceRef="AresultNotification" targetRef="AendEvent" /><bpmn:sequenceFlow id="Flow_AdepartmentManagerApproval_AapprovalRejected" sourceRef="AdepartmentManagerApproval" targetRef="AapprovalRejected"><bpmn:conditionExpression>approval != ''通过''</bpmn:conditionExpression></bpmn:sequenceFlow><bpmn:sequenceFlow id="Flow_AapprovalRejected_AendEvent" sourceRef="AapprovalRejected" targetRef="AendEvent" /></bpmn:process><bpmndi:BPMNDiagram id="BPMNDiagram_1"><bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="employeeLeaveRequestProcess_hgt7"><bpmndi:BPMNShape id="Shape_AstartEvent" bpmnElement="AstartEvent"><dc:Bounds x="100" y="100" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AsubmitApplication" bpmnElement="AsubmitApplication"><dc:Bounds x="300" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AdepartmentManagerApproval" bpmnElement="AdepartmentManagerApproval"><dc:Bounds x="500" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AHRConfirmation" bpmnElement="AHRConfirmation"><dc:Bounds x="700" y="50" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AresultNotification" bpmnElement="AresultNotification"><dc:Bounds x="900" y="250" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AapprovalRejected" bpmnElement="AapprovalRejected"><dc:Bounds x="700" y="250" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AendEvent" bpmnElement="AendEvent"><dc:Bounds x="900" y="150" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNEdge id="Edge_AstartEvent_AsubmitApplication" bpmnElement="Flow_AstartEvent_AsubmitApplication"><di:waypoint x="136" y="118" /><di:waypoint x="218" y="118" /><di:waypoint x="300" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AsubmitApplication_AdepartmentManagerApproval" bpmnElement="Flow_AsubmitApplication_AdepartmentManagerApproval"><di:waypoint x="400" y="140" /><di:waypoint x="450" y="140" /><di:waypoint x="500" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AdepartmentManagerApproval_AHRConfirmation" bpmnElement="Flow_AdepartmentManagerApproval_AHRConfirmation"><di:waypoint x="600" y="140" /><di:waypoint x="650" y="140" /><di:waypoint x="650" y="90" /><di:waypoint x="700" y="90" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AHRConfirmation_AresultNotification" bpmnElement="Flow_AHRConfirmation_AresultNotification"><di:waypoint x="800" y="90" /><di:waypoint x="850" y="90" /><di:waypoint x="850" y="290" /><di:waypoint x="900" y="290" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AresultNotification_AendEvent" bpmnElement="Flow_AresultNotification_AendEvent"><di:waypoint x="1000" y="290" /><di:waypoint x="1050" y="290" /><di:waypoint x="1050" y="168" /><di:waypoint x="900" y="168" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AdepartmentManagerApproval_AapprovalRejected" bpmnElement="Flow_AdepartmentManagerApproval_AapprovalRejected"><di:waypoint x="600" y="140" /><di:waypoint x="650" y="140" /><di:waypoint x="650" y="290" /><di:waypoint x="700" y="290" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AapprovalRejected_AendEvent" bpmnElement="Flow_AapprovalRejected_AendEvent"><di:waypoint x="800" y="290" /><di:waypoint x="850" y="290" /><di:waypoint x="850" y="168" /><di:waypoint x="900" y="168" /></bpmndi:BPMNEdge></bpmndi:BPMNPlane></bpmndi:BPMNDiagram></bpmn:definitions>', 0, NULL, NULL, 0, NULL, CAST(0x0000B291014657E3 AS DateTime), NULL)
INSERT [dbo].[WfProcess] ([ID], [ProcessID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1620, N'employeeLeaveRequestProcess_7pib', N'1', N'员工请假流程', N'employeeLeaveRequestProcess_7pib', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="utf-8"?><bpmn:definitions xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL"><bpmn:process id="employeeLeaveRequestProcess_7pib" name="员工请假流程" isExecutable="false"><bpmn:startEvent id="AstartEvent" name="流程开始" /><bpmn:userTask id="AsubmitApplication" name="提交请假申请" assignee="" /><bpmn:userTask id="AdepartmentManagerApproval" name="部门经理审批" assignee="" /><bpmn:serviceTask id="AHRConfirmation" name="HR确认" implementation="" /><bpmn:userTask id="AresultNotification" name="通知结果" assignee="" /><bpmn:userTask id="AapprovalRejected" name="审批驳回" assignee="" /><bpmn:endEvent id="AendEvent" name="流程结束" /><bpmn:sequenceFlow id="Flow_AstartEvent_AsubmitApplication" sourceRef="AstartEvent" targetRef="AsubmitApplication" /><bpmn:sequenceFlow id="Flow_AsubmitApplication_AdepartmentManagerApproval" sourceRef="AsubmitApplication" targetRef="AdepartmentManagerApproval" /><bpmn:sequenceFlow id="Flow_AdepartmentManagerApproval_AHRConfirmation" sourceRef="AdepartmentManagerApproval" targetRef="AHRConfirmation" /><bpmn:sequenceFlow id="Flow_AHRConfirmation_AresultNotification" sourceRef="AHRConfirmation" targetRef="AresultNotification" /><bpmn:sequenceFlow id="Flow_AresultNotification_AendEvent" sourceRef="AresultNotification" targetRef="AendEvent" /><bpmn:sequenceFlow id="Flow_AdepartmentManagerApproval_AapprovalRejected" sourceRef="AdepartmentManagerApproval" targetRef="AapprovalRejected"><bpmn:conditionExpression>approval != ''通过''</bpmn:conditionExpression></bpmn:sequenceFlow><bpmn:sequenceFlow id="Flow_AapprovalRejected_AendEvent" sourceRef="AapprovalRejected" targetRef="AendEvent" /></bpmn:process><bpmndi:BPMNDiagram id="BPMNDiagram_1"><bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="employeeLeaveRequestProcess_7pib"><bpmndi:BPMNShape id="Shape_AstartEvent" bpmnElement="AstartEvent"><dc:Bounds x="100" y="100" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AsubmitApplication" bpmnElement="AsubmitApplication"><dc:Bounds x="300" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AdepartmentManagerApproval" bpmnElement="AdepartmentManagerApproval"><dc:Bounds x="500" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AHRConfirmation" bpmnElement="AHRConfirmation"><dc:Bounds x="700" y="50" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AresultNotification" bpmnElement="AresultNotification"><dc:Bounds x="900" y="250" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AapprovalRejected" bpmnElement="AapprovalRejected"><dc:Bounds x="700" y="250" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AendEvent" bpmnElement="AendEvent"><dc:Bounds x="900" y="150" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNEdge id="Edge_AstartEvent_AsubmitApplication" bpmnElement="Flow_AstartEvent_AsubmitApplication"><di:waypoint x="136" y="118" /><di:waypoint x="218" y="118" /><di:waypoint x="300" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AsubmitApplication_AdepartmentManagerApproval" bpmnElement="Flow_AsubmitApplication_AdepartmentManagerApproval"><di:waypoint x="400" y="140" /><di:waypoint x="450" y="140" /><di:waypoint x="500" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AdepartmentManagerApproval_AHRConfirmation" bpmnElement="Flow_AdepartmentManagerApproval_AHRConfirmation"><di:waypoint x="600" y="140" /><di:waypoint x="650" y="140" /><di:waypoint x="650" y="90" /><di:waypoint x="700" y="90" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AHRConfirmation_AresultNotification" bpmnElement="Flow_AHRConfirmation_AresultNotification"><di:waypoint x="800" y="90" /><di:waypoint x="850" y="90" /><di:waypoint x="850" y="290" /><di:waypoint x="900" y="290" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AresultNotification_AendEvent" bpmnElement="Flow_AresultNotification_AendEvent"><di:waypoint x="1000" y="290" /><di:waypoint x="1050" y="290" /><di:waypoint x="1050" y="168" /><di:waypoint x="900" y="168" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AdepartmentManagerApproval_AapprovalRejected" bpmnElement="Flow_AdepartmentManagerApproval_AapprovalRejected"><di:waypoint x="600" y="140" /><di:waypoint x="650" y="140" /><di:waypoint x="650" y="290" /><di:waypoint x="700" y="290" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AapprovalRejected_AendEvent" bpmnElement="Flow_AapprovalRejected_AendEvent"><di:waypoint x="800" y="290" /><di:waypoint x="850" y="290" /><di:waypoint x="850" y="168" /><di:waypoint x="900" y="168" /></bpmndi:BPMNEdge></bpmndi:BPMNPlane></bpmndi:BPMNDiagram></bpmn:definitions>', 0, NULL, NULL, 0, NULL, CAST(0x0000B2910150764E AS DateTime), NULL)
INSERT [dbo].[WfProcess] ([ID], [ProcessID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1621, N'BoilerMaintenanceProcess_m4vc', N'1', N'锅炉维修流程', N'BoilerMaintenanceProcess_m4vc', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="utf-8"?><bpmn:definitions xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL"><bpmn:process id="BoilerMaintenanceProcess_m4vc" name="锅炉维修流程" isExecutable="false"><bpmn:startEvent id="AStartEvent" name="开始" /><bpmn:task id="AReceiveMaintenanceRequest" name="接收维修请求" /><bpmn:task id="AAssessBoilerCondition" name="评估锅炉状况" /><bpmn:task id="ADetermineRepairNeeds" name="确定维修需求" /><bpmn:task id="AScheduleRepair" name="安排维修" /><bpmn:task id="APerformRepair" name="执行维修" /><bpmn:task id="ATestBoilerOperation" name="测试锅炉运行" /><bpmn:task id="ACompleteMaintenance" name="完成维修" /><bpmn:endEvent id="AEndEvent" name="结束" /><bpmn:sequenceFlow id="Flow_AStartEvent_AReceiveMaintenanceRequest" sourceRef="AStartEvent" targetRef="AReceiveMaintenanceRequest" /><bpmn:sequenceFlow id="Flow_AReceiveMaintenanceRequest_AAssessBoilerCondition" sourceRef="AReceiveMaintenanceRequest" targetRef="AAssessBoilerCondition" /><bpmn:sequenceFlow id="Flow_AAssessBoilerCondition_ADetermineRepairNeeds" sourceRef="AAssessBoilerCondition" targetRef="ADetermineRepairNeeds" /><bpmn:sequenceFlow id="Flow_ADetermineRepairNeeds_AScheduleRepair" sourceRef="ADetermineRepairNeeds" targetRef="AScheduleRepair" /><bpmn:sequenceFlow id="Flow_AScheduleRepair_APerformRepair" sourceRef="AScheduleRepair" targetRef="APerformRepair" /><bpmn:sequenceFlow id="Flow_APerformRepair_ATestBoilerOperation" sourceRef="APerformRepair" targetRef="ATestBoilerOperation" /><bpmn:sequenceFlow id="Flow_ATestBoilerOperation_ACompleteMaintenance" sourceRef="ATestBoilerOperation" targetRef="ACompleteMaintenance" /><bpmn:sequenceFlow id="Flow_ACompleteMaintenance_AEndEvent" sourceRef="ACompleteMaintenance" targetRef="AEndEvent" /></bpmn:process><bpmndi:BPMNDiagram id="BPMNDiagram_1"><bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="BoilerMaintenanceProcess_m4vc"><bpmndi:BPMNShape id="Shape_AStartEvent" bpmnElement="AStartEvent"><dc:Bounds x="100" y="100" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AReceiveMaintenanceRequest" bpmnElement="AReceiveMaintenanceRequest"><dc:Bounds x="300" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AAssessBoilerCondition" bpmnElement="AAssessBoilerCondition"><dc:Bounds x="500" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_ADetermineRepairNeeds" bpmnElement="ADetermineRepairNeeds"><dc:Bounds x="700" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AScheduleRepair" bpmnElement="AScheduleRepair"><dc:Bounds x="900" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_APerformRepair" bpmnElement="APerformRepair"><dc:Bounds x="1100" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_ATestBoilerOperation" bpmnElement="ATestBoilerOperation"><dc:Bounds x="1300" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_ACompleteMaintenance" bpmnElement="ACompleteMaintenance"><dc:Bounds x="1500" y="200" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AEndEvent" bpmnElement="AEndEvent"><dc:Bounds x="1700" y="100" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNEdge id="Edge_AStartEvent_AReceiveMaintenanceRequest" bpmnElement="Flow_AStartEvent_AReceiveMaintenanceRequest"><di:waypoint x="136" y="118" /><di:waypoint x="218" y="118" /><di:waypoint x="300" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AReceiveMaintenanceRequest_AAssessBoilerCondition" bpmnElement="Flow_AReceiveMaintenanceRequest_AAssessBoilerCondition"><di:waypoint x="400" y="140" /><di:waypoint x="450" y="140" /><di:waypoint x="500" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AAssessBoilerCondition_ADetermineRepairNeeds" bpmnElement="Flow_AAssessBoilerCondition_ADetermineRepairNeeds"><di:waypoint x="600" y="140" /><di:waypoint x="650" y="140" /><di:waypoint x="700" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_ADetermineRepairNeeds_AScheduleRepair" bpmnElement="Flow_ADetermineRepairNeeds_AScheduleRepair"><di:waypoint x="800" y="140" /><di:waypoint x="850" y="140" /><di:waypoint x="900" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AScheduleRepair_APerformRepair" bpmnElement="Flow_AScheduleRepair_APerformRepair"><di:waypoint x="1000" y="140" /><di:waypoint x="1050" y="140" /><di:waypoint x="1100" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_APerformRepair_ATestBoilerOperation" bpmnElement="Flow_APerformRepair_ATestBoilerOperation"><di:waypoint x="1200" y="140" /><di:waypoint x="1250" y="140" /><di:waypoint x="1300" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_ATestBoilerOperation_ACompleteMaintenance" bpmnElement="Flow_ATestBoilerOperation_ACompleteMaintenance"><di:waypoint x="1400" y="140" /><di:waypoint x="1450" y="140" /><di:waypoint x="1450" y="240" /><di:waypoint x="1500" y="240" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_ACompleteMaintenance_AEndEvent" bpmnElement="Flow_ACompleteMaintenance_AEndEvent"><di:waypoint x="1600" y="240" /><di:waypoint x="1650" y="240" /><di:waypoint x="1650" y="118" /><di:waypoint x="1700" y="118" /></bpmndi:BPMNEdge></bpmndi:BPMNPlane></bpmndi:BPMNDiagram></bpmn:definitions>', 0, NULL, NULL, 0, NULL, CAST(0x0000B291015A7682 AS DateTime), NULL)
INSERT [dbo].[WfProcess] ([ID], [ProcessID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1622, N'RobotEmotionDetectionProcess_3oz3', N'1', N'机器人情感检测流程', N'RobotEmotionDetectionProcess_3oz3', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="utf-8"?><bpmn:definitions xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL"><bpmn:process id="RobotEmotionDetectionProcess_3oz3" name="机器人情感检测流程" isExecutable="false"><bpmn:startEvent id="AStartEvent" name="开始" /><bpmn:task id="ACaptureFacialExpression" name="捕捉面部表情" /><bpmn:task id="AAnalyzeVoiceTone" name="分析语音语调" /><bpmn:task id="ADetermineEmotion" name="确定情感状态" /><bpmn:endEvent id="AEndEvent" name="结束" /><bpmn:sequenceFlow id="Flow_AStartEvent_ACaptureFacialExpression" sourceRef="AStartEvent" targetRef="ACaptureFacialExpression" /><bpmn:sequenceFlow id="Flow_ACaptureFacialExpression_AAnalyzeVoiceTone" sourceRef="ACaptureFacialExpression" targetRef="AAnalyzeVoiceTone" /><bpmn:sequenceFlow id="Flow_AAnalyzeVoiceTone_ADetermineEmotion" sourceRef="AAnalyzeVoiceTone" targetRef="ADetermineEmotion" /><bpmn:sequenceFlow id="Flow_ADetermineEmotion_AEndEvent" sourceRef="ADetermineEmotion" targetRef="AEndEvent" /></bpmn:process><bpmndi:BPMNDiagram id="BPMNDiagram_1"><bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="RobotEmotionDetectionProcess_3oz3"><bpmndi:BPMNShape id="Shape_AStartEvent" bpmnElement="AStartEvent"><dc:Bounds x="100" y="100" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_ACaptureFacialExpression" bpmnElement="ACaptureFacialExpression"><dc:Bounds x="300" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AAnalyzeVoiceTone" bpmnElement="AAnalyzeVoiceTone"><dc:Bounds x="500" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_ADetermineEmotion" bpmnElement="ADetermineEmotion"><dc:Bounds x="700" y="200" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AEndEvent" bpmnElement="AEndEvent"><dc:Bounds x="900" y="100" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNEdge id="Edge_AStartEvent_ACaptureFacialExpression" bpmnElement="Flow_AStartEvent_ACaptureFacialExpression"><di:waypoint x="136" y="118" /><di:waypoint x="218" y="118" /><di:waypoint x="300" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_ACaptureFacialExpression_AAnalyzeVoiceTone" bpmnElement="Flow_ACaptureFacialExpression_AAnalyzeVoiceTone"><di:waypoint x="400" y="140" /><di:waypoint x="450" y="140" /><di:waypoint x="500" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AAnalyzeVoiceTone_ADetermineEmotion" bpmnElement="Flow_AAnalyzeVoiceTone_ADetermineEmotion"><di:waypoint x="600" y="140" /><di:waypoint x="650" y="140" /><di:waypoint x="650" y="240" /><di:waypoint x="700" y="240" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_ADetermineEmotion_AEndEvent" bpmnElement="Flow_ADetermineEmotion_AEndEvent"><di:waypoint x="800" y="240" /><di:waypoint x="850" y="240" /><di:waypoint x="850" y="118" /><di:waypoint x="900" y="118" /></bpmndi:BPMNEdge></bpmndi:BPMNPlane></bpmndi:BPMNDiagram></bpmn:definitions>', 0, NULL, NULL, 0, NULL, CAST(0x0000B291015AE10F AS DateTime), NULL)
INSERT [dbo].[WfProcess] ([ID], [ProcessID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1623, N'MineClearanceProcess_ksoh', N'1', N'地雷排查流程', N'MineClearanceProcess_ksoh', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="utf-8"?><bpmn:definitions xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL"><bpmn:process id="MineClearanceProcess_ksoh" name="地雷排查流程" isExecutable="false"><bpmn:startEvent id="AStartEvent" name="开始排查" /><bpmn:task id="ATask1" name="区域划分" /><bpmn:task id="ATask2" name="设备检查" /><bpmn:task id="ATask3" name="地雷探测" /><bpmn:task id="ATask4" name="标记可疑区域" /><bpmn:task id="ATask5" name="人工排查" /><bpmn:task id="ATask6" name="地雷处理" /><bpmn:task id="ATask7" name="区域清理" /><bpmn:endEvent id="AEndEvent" name="排查完成" /><bpmn:sequenceFlow id="Flow_AStartEvent_ATask1" sourceRef="AStartEvent" targetRef="ATask1" /><bpmn:sequenceFlow id="Flow_ATask1_ATask2" sourceRef="ATask1" targetRef="ATask2" /><bpmn:sequenceFlow id="Flow_ATask2_ATask3" sourceRef="ATask2" targetRef="ATask3" /><bpmn:sequenceFlow id="Flow_ATask3_ATask4" sourceRef="ATask3" targetRef="ATask4" /><bpmn:sequenceFlow id="Flow_ATask4_ATask5" sourceRef="ATask4" targetRef="ATask5" /><bpmn:sequenceFlow id="Flow_ATask5_ATask6" sourceRef="ATask5" targetRef="ATask6" /><bpmn:sequenceFlow id="Flow_ATask6_ATask7" sourceRef="ATask6" targetRef="ATask7" /><bpmn:sequenceFlow id="Flow_ATask7_AEndEvent" sourceRef="ATask7" targetRef="AEndEvent" /></bpmn:process><bpmndi:BPMNDiagram id="BPMNDiagram_1"><bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="MineClearanceProcess_ksoh"><bpmndi:BPMNShape id="Shape_AStartEvent" bpmnElement="AStartEvent"><dc:Bounds x="100" y="100" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_ATask1" bpmnElement="ATask1"><dc:Bounds x="300" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_ATask2" bpmnElement="ATask2"><dc:Bounds x="500" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_ATask3" bpmnElement="ATask3"><dc:Bounds x="700" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_ATask4" bpmnElement="ATask4"><dc:Bounds x="900" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_ATask5" bpmnElement="ATask5"><dc:Bounds x="1100" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_ATask6" bpmnElement="ATask6"><dc:Bounds x="1300" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_ATask7" bpmnElement="ATask7"><dc:Bounds x="1500" y="200" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AEndEvent" bpmnElement="AEndEvent"><dc:Bounds x="1700" y="100" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNEdge id="Edge_AStartEvent_ATask1" bpmnElement="Flow_AStartEvent_ATask1"><di:waypoint x="136" y="118" /><di:waypoint x="218" y="118" /><di:waypoint x="300" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_ATask1_ATask2" bpmnElement="Flow_ATask1_ATask2"><di:waypoint x="400" y="140" /><di:waypoint x="450" y="140" /><di:waypoint x="500" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_ATask2_ATask3" bpmnElement="Flow_ATask2_ATask3"><di:waypoint x="600" y="140" /><di:waypoint x="650" y="140" /><di:waypoint x="700" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_ATask3_ATask4" bpmnElement="Flow_ATask3_ATask4"><di:waypoint x="800" y="140" /><di:waypoint x="850" y="140" /><di:waypoint x="900" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_ATask4_ATask5" bpmnElement="Flow_ATask4_ATask5"><di:waypoint x="1000" y="140" /><di:waypoint x="1050" y="140" /><di:waypoint x="1100" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_ATask5_ATask6" bpmnElement="Flow_ATask5_ATask6"><di:waypoint x="1200" y="140" /><di:waypoint x="1250" y="140" /><di:waypoint x="1300" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_ATask6_ATask7" bpmnElement="Flow_ATask6_ATask7"><di:waypoint x="1400" y="140" /><di:waypoint x="1450" y="140" /><di:waypoint x="1450" y="240" /><di:waypoint x="1500" y="240" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_ATask7_AEndEvent" bpmnElement="Flow_ATask7_AEndEvent"><di:waypoint x="1600" y="240" /><di:waypoint x="1650" y="240" /><di:waypoint x="1650" y="118" /><di:waypoint x="1700" y="118" /></bpmndi:BPMNEdge></bpmndi:BPMNPlane></bpmndi:BPMNDiagram></bpmn:definitions>', 0, NULL, NULL, 0, NULL, CAST(0x0000B291015BC1AA AS DateTime), NULL)
INSERT [dbo].[WfProcess] ([ID], [ProcessID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1624, N'ShipInspectionProcess_x5gx', N'1', N'船舶验收流程', N'ShipInspectionProcess_x5gx', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="utf-8"?><bpmn:definitions xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL"><bpmn:process id="ShipInspectionProcess_x5gx" name="船舶验收流程" isExecutable="false"><bpmn:startEvent id="AStartEvent" name="开始" /><bpmn:exclusiveGateway id="ACheckTonnage" name="检查吨位" /><bpmn:serviceTask id="ADomesticInspection" name="国内核检" implementation="" /><bpmn:serviceTask id="AInternationalInspection" name="国外核检" implementation="" /><bpmn:endEvent id="AEndEvent" name="结束" /><bpmn:sequenceFlow id="Flow_AStartEvent_ACheckTonnage" sourceRef="AStartEvent" targetRef="ACheckTonnage" /><bpmn:sequenceFlow id="Flow_ACheckTonnage_ADomesticInspection" sourceRef="ACheckTonnage" targetRef="ADomesticInspection"><bpmn:conditionExpression>吨位 &lt;= 100</bpmn:conditionExpression></bpmn:sequenceFlow><bpmn:sequenceFlow id="Flow_ACheckTonnage_AInternationalInspection" sourceRef="ACheckTonnage" targetRef="AInternationalInspection"><bpmn:conditionExpression>吨位 &gt; 100</bpmn:conditionExpression></bpmn:sequenceFlow><bpmn:sequenceFlow id="Flow_ADomesticInspection_AEndEvent" sourceRef="ADomesticInspection" targetRef="AEndEvent" /><bpmn:sequenceFlow id="Flow_AInternationalInspection_AEndEvent" sourceRef="AInternationalInspection" targetRef="AEndEvent" /></bpmn:process><bpmndi:BPMNDiagram id="BPMNDiagram_1"><bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="ShipInspectionProcess_x5gx"><bpmndi:BPMNShape id="Shape_AStartEvent" bpmnElement="AStartEvent"><dc:Bounds x="100" y="100" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_ACheckTonnage" bpmnElement="ACheckTonnage"><dc:Bounds x="300" y="100" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_ADomesticInspection" bpmnElement="ADomesticInspection"><dc:Bounds x="500" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AInternationalInspection" bpmnElement="AInternationalInspection"><dc:Bounds x="500" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AEndEvent" bpmnElement="AEndEvent"><dc:Bounds x="700" y="100" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNEdge id="Edge_AStartEvent_ACheckTonnage" bpmnElement="Flow_AStartEvent_ACheckTonnage"><di:waypoint x="136" y="118" /><di:waypoint x="218" y="118" /><di:waypoint x="300" y="118" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_ACheckTonnage_ADomesticInspection" bpmnElement="Flow_ACheckTonnage_ADomesticInspection"><di:waypoint x="336" y="118" /><di:waypoint x="418" y="118" /><di:waypoint x="500" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_ACheckTonnage_AInternationalInspection" bpmnElement="Flow_ACheckTonnage_AInternationalInspection"><di:waypoint x="336" y="118" /><di:waypoint x="418" y="118" /><di:waypoint x="500" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_ADomesticInspection_AEndEvent" bpmnElement="Flow_ADomesticInspection_AEndEvent"><di:waypoint x="600" y="140" /><di:waypoint x="650" y="140" /><di:waypoint x="700" y="118" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AInternationalInspection_AEndEvent" bpmnElement="Flow_AInternationalInspection_AEndEvent"><di:waypoint x="600" y="140" /><di:waypoint x="650" y="140" /><di:waypoint x="700" y="118" /></bpmndi:BPMNEdge></bpmndi:BPMNPlane></bpmndi:BPMNDiagram></bpmn:definitions>', 0, NULL, NULL, 0, NULL, CAST(0x0000B291015CF032 AS DateTime), NULL)
INSERT [dbo].[WfProcess] ([ID], [ProcessID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1625, N'FireworkDetectionProcess_uhlv', N'1', N'烟花检测流程', N'FireworkDetectionProcess_uhlv', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="utf-8"?><bpmn:definitions xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL"><bpmn:process id="FireworkDetectionProcess_uhlv" name="烟花检测流程" isExecutable="false"><bpmn:startEvent id="AStartEvent" name="开始检测" /><bpmn:serviceTask id="ACaptureImage" name="捕捉图像" implementation="" /><bpmn:serviceTask id="AAnalyzeImage" name="分析图像" implementation="" /><bpmn:serviceTask id="ADetectFirework" name="检测烟花" implementation="" /><bpmn:serviceTask id="ALogResult" name="记录结果" implementation="" /><bpmn:endEvent id="AEndEvent" name="结束检测" /><bpmn:sequenceFlow id="Flow_AStartEvent_ACaptureImage" sourceRef="AStartEvent" targetRef="ACaptureImage" /><bpmn:sequenceFlow id="Flow_ACaptureImage_AAnalyzeImage" sourceRef="ACaptureImage" targetRef="AAnalyzeImage" /><bpmn:sequenceFlow id="Flow_AAnalyzeImage_ADetectFirework" sourceRef="AAnalyzeImage" targetRef="ADetectFirework" /><bpmn:sequenceFlow id="Flow_ADetectFirework_ALogResult" sourceRef="ADetectFirework" targetRef="ALogResult" /><bpmn:sequenceFlow id="Flow_ALogResult_AEndEvent" sourceRef="ALogResult" targetRef="AEndEvent" /></bpmn:process><bpmndi:BPMNDiagram id="BPMNDiagram_1"><bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="FireworkDetectionProcess_uhlv"><bpmndi:BPMNShape id="Shape_AStartEvent" bpmnElement="AStartEvent"><dc:Bounds x="100" y="100" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_ACaptureImage" bpmnElement="ACaptureImage"><dc:Bounds x="300" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AAnalyzeImage" bpmnElement="AAnalyzeImage"><dc:Bounds x="500" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_ADetectFirework" bpmnElement="ADetectFirework"><dc:Bounds x="700" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_ALogResult" bpmnElement="ALogResult"><dc:Bounds x="900" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AEndEvent" bpmnElement="AEndEvent"><dc:Bounds x="1100" y="100" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNEdge id="Edge_AStartEvent_ACaptureImage" bpmnElement="Flow_AStartEvent_ACaptureImage"><di:waypoint x="136" y="118" /><di:waypoint x="218" y="118" /><di:waypoint x="300" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_ACaptureImage_AAnalyzeImage" bpmnElement="Flow_ACaptureImage_AAnalyzeImage"><di:waypoint x="400" y="140" /><di:waypoint x="450" y="140" /><di:waypoint x="500" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AAnalyzeImage_ADetectFirework" bpmnElement="Flow_AAnalyzeImage_ADetectFirework"><di:waypoint x="600" y="140" /><di:waypoint x="650" y="140" /><di:waypoint x="700" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_ADetectFirework_ALogResult" bpmnElement="Flow_ADetectFirework_ALogResult"><di:waypoint x="800" y="140" /><di:waypoint x="850" y="140" /><di:waypoint x="900" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_ALogResult_AEndEvent" bpmnElement="Flow_ALogResult_AEndEvent"><di:waypoint x="1000" y="140" /><di:waypoint x="1050" y="140" /><di:waypoint x="1100" y="118" /></bpmndi:BPMNEdge></bpmndi:BPMNPlane></bpmndi:BPMNDiagram></bpmn:definitions>', 0, NULL, NULL, 0, NULL, CAST(0x0000B2910161384B AS DateTime), NULL)
INSERT [dbo].[WfProcess] ([ID], [ProcessID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1626, N'ArtificialRainfallProcess_39p2', N'1', N'人工降雨流程', N'ArtificialRainfallProcess_39p2', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="utf-8"?><bpmn:definitions xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL"><bpmn:process id="ArtificialRainfallProcess_39p2" name="人工降雨流程" isExecutable="false"><bpmn:startEvent id="AStartEvent" name="开始" /><bpmn:task id="AWeatherAnalysis" name="天气分析" /><bpmn:exclusiveGateway id="ADecision" name="是否适合人工降雨" /><bpmn:task id="APrepareEquipment" name="准备设备" /><bpmn:task id="ADeployEquipment" name="部署设备" /><bpmn:task id="AInitiateRainfall" name="启动人工降雨" /><bpmn:task id="AMonitorRainfall" name="监测降雨效果" /><bpmn:endEvent id="AEndEvent" name="结束" /><bpmn:sequenceFlow id="Flow_AStartEvent_AWeatherAnalysis" sourceRef="AStartEvent" targetRef="AWeatherAnalysis" /><bpmn:sequenceFlow id="Flow_AWeatherAnalysis_ADecision" sourceRef="AWeatherAnalysis" targetRef="ADecision" /><bpmn:sequenceFlow id="Flow_ADecision_APrepareEquipment" sourceRef="ADecision" targetRef="APrepareEquipment"><bpmn:conditionExpression>适合人工降雨</bpmn:conditionExpression></bpmn:sequenceFlow><bpmn:sequenceFlow id="Flow_ADecision_AEndEvent" sourceRef="ADecision" targetRef="AEndEvent"><bpmn:conditionExpression>不适合人工降雨</bpmn:conditionExpression></bpmn:sequenceFlow><bpmn:sequenceFlow id="Flow_APrepareEquipment_ADeployEquipment" sourceRef="APrepareEquipment" targetRef="ADeployEquipment" /><bpmn:sequenceFlow id="Flow_ADeployEquipment_AInitiateRainfall" sourceRef="ADeployEquipment" targetRef="AInitiateRainfall" /><bpmn:sequenceFlow id="Flow_AInitiateRainfall_AMonitorRainfall" sourceRef="AInitiateRainfall" targetRef="AMonitorRainfall" /><bpmn:sequenceFlow id="Flow_AMonitorRainfall_AEndEvent" sourceRef="AMonitorRainfall" targetRef="AEndEvent" /></bpmn:process><bpmndi:BPMNDiagram id="BPMNDiagram_1"><bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="ArtificialRainfallProcess_39p2"><bpmndi:BPMNShape id="Shape_AStartEvent" bpmnElement="AStartEvent"><dc:Bounds x="100" y="100" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AWeatherAnalysis" bpmnElement="AWeatherAnalysis"><dc:Bounds x="300" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_ADecision" bpmnElement="ADecision"><dc:Bounds x="500" y="150" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_APrepareEquipment" bpmnElement="APrepareEquipment"><dc:Bounds x="700" y="50" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_ADeployEquipment" bpmnElement="ADeployEquipment"><dc:Bounds x="900" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AInitiateRainfall" bpmnElement="AInitiateRainfall"><dc:Bounds x="1100" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AMonitorRainfall" bpmnElement="AMonitorRainfall"><dc:Bounds x="1300" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AEndEvent" bpmnElement="AEndEvent"><dc:Bounds x="700" y="150" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNEdge id="Edge_AStartEvent_AWeatherAnalysis" bpmnElement="Flow_AStartEvent_AWeatherAnalysis"><di:waypoint x="136" y="118" /><di:waypoint x="218" y="118" /><di:waypoint x="300" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AWeatherAnalysis_ADecision" bpmnElement="Flow_AWeatherAnalysis_ADecision"><di:waypoint x="400" y="140" /><di:waypoint x="450" y="140" /><di:waypoint x="450" y="168" /><di:waypoint x="500" y="168" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_ADecision_APrepareEquipment" bpmnElement="Flow_ADecision_APrepareEquipment"><di:waypoint x="536" y="168" /><di:waypoint x="586" y="168" /><di:waypoint x="586" y="90" /><di:waypoint x="700" y="90" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_ADecision_AEndEvent" bpmnElement="Flow_ADecision_AEndEvent"><di:waypoint x="536" y="168" /><di:waypoint x="618" y="168" /><di:waypoint x="700" y="168" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_APrepareEquipment_ADeployEquipment" bpmnElement="Flow_APrepareEquipment_ADeployEquipment"><di:waypoint x="800" y="90" /><di:waypoint x="850" y="90" /><di:waypoint x="850" y="140" /><di:waypoint x="900" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_ADeployEquipment_AInitiateRainfall" bpmnElement="Flow_ADeployEquipment_AInitiateRainfall"><di:waypoint x="1000" y="140" /><di:waypoint x="1050" y="140" /><di:waypoint x="1100" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AInitiateRainfall_AMonitorRainfall" bpmnElement="Flow_AInitiateRainfall_AMonitorRainfall"><di:waypoint x="1200" y="140" /><di:waypoint x="1250" y="140" /><di:waypoint x="1300" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AMonitorRainfall_AEndEvent" bpmnElement="Flow_AMonitorRainfall_AEndEvent"><di:waypoint x="1400" y="140" /><di:waypoint x="1450" y="140" /><di:waypoint x="1450" y="168" /><di:waypoint x="700" y="168" /></bpmndi:BPMNEdge></bpmndi:BPMNPlane></bpmndi:BPMNDiagram></bpmn:definitions>', 0, NULL, NULL, 0, NULL, CAST(0x0000B2910161893B AS DateTime), NULL)
INSERT [dbo].[WfProcess] ([ID], [ProcessID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1627, N'RocketLaunchProcess_smva', N'1', N'火箭发射流程', N'RocketLaunchProcess_smva', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="utf-8"?><bpmn:definitions xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL"><bpmn:process id="RocketLaunchProcess_smva" name="火箭发射流程" isExecutable="false"><bpmn:startEvent id="AStartEvent_1" name="发射指令接收" /><bpmn:userTask id="ATask_1" name="发射准备阶段" assignee="" /><bpmn:serviceTask id="ATask_2" name="燃料加注系统" implementation="" /><bpmn:manualTask id="ATask_3" name="最终安全检查" /><bpmn:businessRuleTask id="ATask_4" name="倒计时启动" /><bpmn:sendTask id="ATask_5" name="点火发射" /><bpmn:receiveTask id="ATask_6" name="助推器分离" /><bpmn:endEvent id="AEndEvent_1" name="进入预定轨道" /><bpmn:sequenceFlow id="Flow_AStartEvent_1_ATask_1" sourceRef="AStartEvent_1" targetRef="ATask_1" /><bpmn:sequenceFlow id="Flow_ATask_1_ATask_2" sourceRef="ATask_1" targetRef="ATask_2" /><bpmn:sequenceFlow id="Flow_ATask_2_ATask_3" sourceRef="ATask_2" targetRef="ATask_3" /><bpmn:sequenceFlow id="Flow_ATask_3_ATask_4" sourceRef="ATask_3" targetRef="ATask_4" /><bpmn:sequenceFlow id="Flow_ATask_4_ATask_5" sourceRef="ATask_4" targetRef="ATask_5" /><bpmn:sequenceFlow id="Flow_ATask_5_ATask_6" sourceRef="ATask_5" targetRef="ATask_6" /><bpmn:sequenceFlow id="Flow_ATask_6_AEndEvent_1" sourceRef="ATask_6" targetRef="AEndEvent_1" /></bpmn:process><bpmndi:BPMNDiagram id="BPMNDiagram_1"><bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="RocketLaunchProcess_smva"><bpmndi:BPMNShape id="Shape_AStartEvent_1" bpmnElement="AStartEvent_1"><dc:Bounds x="100" y="100" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_ATask_1" bpmnElement="ATask_1"><dc:Bounds x="300" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_ATask_2" bpmnElement="ATask_2"><dc:Bounds x="500" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_ATask_3" bpmnElement="ATask_3"><dc:Bounds x="700" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_ATask_4" bpmnElement="ATask_4"><dc:Bounds x="900" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_ATask_5" bpmnElement="ATask_5"><dc:Bounds x="1100" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_ATask_6" bpmnElement="ATask_6"><dc:Bounds x="1300" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AEndEvent_1" bpmnElement="AEndEvent_1"><dc:Bounds x="1500" y="100" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNEdge id="Edge_AStartEvent_1_ATask_1" bpmnElement="Flow_AStartEvent_1_ATask_1"><di:waypoint x="136" y="118" /><di:waypoint x="218" y="118" /><di:waypoint x="300" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_ATask_1_ATask_2" bpmnElement="Flow_ATask_1_ATask_2"><di:waypoint x="400" y="140" /><di:waypoint x="450" y="140" /><di:waypoint x="500" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_ATask_2_ATask_3" bpmnElement="Flow_ATask_2_ATask_3"><di:waypoint x="600" y="140" /><di:waypoint x="650" y="140" /><di:waypoint x="700" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_ATask_3_ATask_4" bpmnElement="Flow_ATask_3_ATask_4"><di:waypoint x="800" y="140" /><di:waypoint x="850" y="140" /><di:waypoint x="900" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_ATask_4_ATask_5" bpmnElement="Flow_ATask_4_ATask_5"><di:waypoint x="1000" y="140" /><di:waypoint x="1050" y="140" /><di:waypoint x="1100" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_ATask_5_ATask_6" bpmnElement="Flow_ATask_5_ATask_6"><di:waypoint x="1200" y="140" /><di:waypoint x="1250" y="140" /><di:waypoint x="1300" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_ATask_6_AEndEvent_1" bpmnElement="Flow_ATask_6_AEndEvent_1"><di:waypoint x="1400" y="140" /><di:waypoint x="1450" y="140" /><di:waypoint x="1500" y="118" /></bpmndi:BPMNEdge></bpmndi:BPMNPlane></bpmndi:BPMNDiagram></bpmn:definitions>', 0, NULL, NULL, 0, NULL, CAST(0x0000B29200979CA4 AS DateTime), NULL)
INSERT [dbo].[WfProcess] ([ID], [ProcessID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1628, N'Process_SewageTreatment_avat', N'1', N'污水处理主流程', N'Process_SewageTreatment_avat', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="utf-8"?><bpmn:definitions xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL"><bpmn:process id="Process_SewageTreatment_avat" name="污水处理主流程" isExecutable="false"><bpmn:startEvent id="AStartEvent_1" name="进水口接收污水" /><bpmn:task id="ATask_1" name="格栅过滤（粗筛）" /><bpmn:task id="ATask_2" name="沉砂池处理" /><bpmn:task id="ATask_3" name="初级沉淀池" /><bpmn:exclusiveGateway id="AGateway_1" name="污泥分流判断" /><bpmn:task id="ATask_4" name="活性污泥法处理" /><bpmn:task id="ATask_5" name="二级沉淀池" /><bpmn:task id="ATask_6" name="加氯消毒" /><bpmn:endEvent id="AEndEvent_1" name="达标排放" /><bpmn:task id="ATask_7" name="污泥浓缩" /><bpmn:task id="ATask_8" name="污泥脱水" /><bpmn:endEvent id="AEndEvent_2" name="污泥最终处置" /><bpmn:sequenceFlow id="Flow_AStartEvent_1_ATask_1" sourceRef="AStartEvent_1" targetRef="ATask_1" /><bpmn:sequenceFlow id="Flow_ATask_1_ATask_2" sourceRef="ATask_1" targetRef="ATask_2" /><bpmn:sequenceFlow id="Flow_ATask_2_ATask_3" sourceRef="ATask_2" targetRef="ATask_3" /><bpmn:sequenceFlow id="Flow_ATask_3_AGateway_1" sourceRef="ATask_3" targetRef="AGateway_1" /><bpmn:sequenceFlow id="Flow_AGateway_1_ATask_4" sourceRef="AGateway_1" targetRef="ATask_4" /><bpmn:sequenceFlow id="Flow_ATask_4_ATask_5" sourceRef="ATask_4" targetRef="ATask_5" /><bpmn:sequenceFlow id="Flow_ATask_5_ATask_6" sourceRef="ATask_5" targetRef="ATask_6" /><bpmn:sequenceFlow id="Flow_ATask_6_AEndEvent_1" sourceRef="ATask_6" targetRef="AEndEvent_1" /><bpmn:sequenceFlow id="Flow_AGateway_1_ATask_7" sourceRef="AGateway_1" targetRef="ATask_7" /><bpmn:sequenceFlow id="Flow_ATask_7_ATask_8" sourceRef="ATask_7" targetRef="ATask_8" /><bpmn:sequenceFlow id="Flow_ATask_8_AEndEvent_2" sourceRef="ATask_8" targetRef="AEndEvent_2" /></bpmn:process><bpmndi:BPMNDiagram id="BPMNDiagram_1"><bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_SewageTreatment_avat"><bpmndi:BPMNShape id="Shape_AStartEvent_1" bpmnElement="AStartEvent_1"><dc:Bounds x="100" y="100" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_ATask_1" bpmnElement="ATask_1"><dc:Bounds x="300" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_ATask_2" bpmnElement="ATask_2"><dc:Bounds x="500" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_ATask_3" bpmnElement="ATask_3"><dc:Bounds x="700" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AGateway_1" bpmnElement="AGateway_1"><dc:Bounds x="900" y="100" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_ATask_4" bpmnElement="ATask_4"><dc:Bounds x="1100" y="50" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_ATask_5" bpmnElement="ATask_5"><dc:Bounds x="1300" y="50" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_ATask_6" bpmnElement="ATask_6"><dc:Bounds x="1500" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AEndEvent_1" bpmnElement="AEndEvent_1"><dc:Bounds x="1700" y="100" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_ATask_7" bpmnElement="ATask_7"><dc:Bounds x="1100" y="150" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_ATask_8" bpmnElement="ATask_8"><dc:Bounds x="1300" y="150" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AEndEvent_2" bpmnElement="AEndEvent_2"><dc:Bounds x="1500" y="150" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNEdge id="Edge_AStartEvent_1_ATask_1" bpmnElement="Flow_AStartEvent_1_ATask_1"><di:waypoint x="136" y="118" /><di:waypoint x="218" y="118" /><di:waypoint x="300" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_ATask_1_ATask_2" bpmnElement="Flow_ATask_1_ATask_2"><di:waypoint x="400" y="140" /><di:waypoint x="450" y="140" /><di:waypoint x="500" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_ATask_2_ATask_3" bpmnElement="Flow_ATask_2_ATask_3"><di:waypoint x="600" y="140" /><di:waypoint x="650" y="140" /><di:waypoint x="700" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_ATask_3_AGateway_1" bpmnElement="Flow_ATask_3_AGateway_1"><di:waypoint x="800" y="140" /><di:waypoint x="850" y="140" /><di:waypoint x="900" y="118" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AGateway_1_ATask_4" bpmnElement="Flow_AGateway_1_ATask_4"><di:waypoint x="936" y="118" /><di:waypoint x="986" y="118" /><di:waypoint x="986" y="90" /><di:waypoint x="1100" y="90" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_ATask_4_ATask_5" bpmnElement="Flow_ATask_4_ATask_5"><di:waypoint x="1200" y="90" /><di:waypoint x="1250" y="90" /><di:waypoint x="1300" y="90" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_ATask_5_ATask_6" bpmnElement="Flow_ATask_5_ATask_6"><di:waypoint x="1400" y="90" /><di:waypoint x="1450" y="90" /><di:waypoint x="1450" y="140" /><di:waypoint x="1500" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_ATask_6_AEndEvent_1" bpmnElement="Flow_ATask_6_AEndEvent_1"><di:waypoint x="1600" y="140" /><di:waypoint x="1650" y="140" /><di:waypoint x="1700" y="118" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AGateway_1_ATask_7" bpmnElement="Flow_AGateway_1_ATask_7"><di:waypoint x="936" y="118" /><di:waypoint x="986" y="118" /><di:waypoint x="986" y="190" /><di:waypoint x="1100" y="190" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_ATask_7_ATask_8" bpmnElement="Flow_ATask_7_ATask_8"><di:waypoint x="1200" y="190" /><di:waypoint x="1250" y="190" /><di:waypoint x="1300" y="190" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_ATask_8_AEndEvent_2" bpmnElement="Flow_ATask_8_AEndEvent_2"><di:waypoint x="1400" y="190" /><di:waypoint x="1450" y="190" /><di:waypoint x="1500" y="168" /></bpmndi:BPMNEdge></bpmndi:BPMNPlane></bpmndi:BPMNDiagram></bpmn:definitions>', 0, NULL, NULL, 0, NULL, CAST(0x0000B292009E4F76 AS DateTime), NULL)
INSERT [dbo].[WfProcess] ([ID], [ProcessID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1629, N'Process_VisionTest_vj8u', N'1', N'视力检测流程', N'Process_VisionTest_vj8u', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="utf-8"?><bpmn:definitions xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL"><bpmn:process id="Process_VisionTest_vj8u" name="视力检测流程" isExecutable="false"><bpmn:startEvent id="AStartEvent_1" name="开始检测" /><bpmn:userTask id="AUserTask_Register" name="登记基本信息" assignee="" /><bpmn:userTask id="AUserTask_PreliminaryCheck" name="初步问诊" assignee="" /><bpmn:serviceTask id="AServiceTask_LeftEyeTest" name="左眼视力测试" implementation="" /><bpmn:serviceTask id="AServiceTask_RightEyeTest" name="右眼视力测试" implementation="" /><bpmn:serviceTask id="AServiceTask_DataAnalysis" name="生成检测报告" implementation="" /><bpmn:exclusiveGateway id="AExclusiveGateway_1" name="判断是否需要配镜" /><bpmn:userTask id="AUserTask_AdviceNormal" name="日常护眼建议" assignee="" /><bpmn:userTask id="AUserTask_AdviceGlasses" name="配镜方案建议" assignee="" /><bpmn:endEvent id="AEndEvent_1" name="流程结束" /><bpmn:sequenceFlow id="Flow_AStartEvent_1_AUserTask_Register" sourceRef="AStartEvent_1" targetRef="AUserTask_Register" /><bpmn:sequenceFlow id="Flow_AUserTask_Register_AUserTask_PreliminaryCheck" sourceRef="AUserTask_Register" targetRef="AUserTask_PreliminaryCheck" /><bpmn:sequenceFlow id="Flow_AUserTask_PreliminaryCheck_AServiceTask_LeftEyeTest" sourceRef="AUserTask_PreliminaryCheck" targetRef="AServiceTask_LeftEyeTest" /><bpmn:sequenceFlow id="Flow_AServiceTask_LeftEyeTest_AServiceTask_RightEyeTest" sourceRef="AServiceTask_LeftEyeTest" targetRef="AServiceTask_RightEyeTest" /><bpmn:sequenceFlow id="Flow_AServiceTask_RightEyeTest_AServiceTask_DataAnalysis" sourceRef="AServiceTask_RightEyeTest" targetRef="AServiceTask_DataAnalysis" /><bpmn:sequenceFlow id="Flow_AServiceTask_DataAnalysis_AExclusiveGateway_1" sourceRef="AServiceTask_DataAnalysis" targetRef="AExclusiveGateway_1" /><bpmn:sequenceFlow id="Flow_AExclusiveGateway_1_AUserTask_AdviceNormal" sourceRef="AExclusiveGateway_1" targetRef="AUserTask_AdviceNormal" /><bpmn:sequenceFlow id="Flow_AExclusiveGateway_1_AUserTask_AdviceGlasses" sourceRef="AExclusiveGateway_1" targetRef="AUserTask_AdviceGlasses" /><bpmn:sequenceFlow id="Flow_AUserTask_AdviceNormal_AEndEvent_1" sourceRef="AUserTask_AdviceNormal" targetRef="AEndEvent_1" /><bpmn:sequenceFlow id="Flow_AUserTask_AdviceGlasses_AEndEvent_1" sourceRef="AUserTask_AdviceGlasses" targetRef="AEndEvent_1" /></bpmn:process><bpmndi:BPMNDiagram id="BPMNDiagram_1"><bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_VisionTest_vj8u"><bpmndi:BPMNShape id="Shape_AStartEvent_1" bpmnElement="AStartEvent_1"><dc:Bounds x="100" y="100" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AUserTask_Register" bpmnElement="AUserTask_Register"><dc:Bounds x="300" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AUserTask_PreliminaryCheck" bpmnElement="AUserTask_PreliminaryCheck"><dc:Bounds x="500" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AServiceTask_LeftEyeTest" bpmnElement="AServiceTask_LeftEyeTest"><dc:Bounds x="700" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AServiceTask_RightEyeTest" bpmnElement="AServiceTask_RightEyeTest"><dc:Bounds x="900" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AServiceTask_DataAnalysis" bpmnElement="AServiceTask_DataAnalysis"><dc:Bounds x="1100" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AExclusiveGateway_1" bpmnElement="AExclusiveGateway_1"><dc:Bounds x="1300" y="100" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AUserTask_AdviceNormal" bpmnElement="AUserTask_AdviceNormal"><dc:Bounds x="1500" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AUserTask_AdviceGlasses" bpmnElement="AUserTask_AdviceGlasses"><dc:Bounds x="1500" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AEndEvent_1" bpmnElement="AEndEvent_1"><dc:Bounds x="1700" y="100" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNEdge id="Edge_AStartEvent_1_AUserTask_Register" bpmnElement="Flow_AStartEvent_1_AUserTask_Register"><di:waypoint x="136" y="118" /><di:waypoint x="218" y="118" /><di:waypoint x="300" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AUserTask_Register_AUserTask_PreliminaryCheck" bpmnElement="Flow_AUserTask_Register_AUserTask_PreliminaryCheck"><di:waypoint x="400" y="140" /><di:waypoint x="450" y="140" /><di:waypoint x="500" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AUserTask_PreliminaryCheck_AServiceTask_LeftEyeTest" bpmnElement="Flow_AUserTask_PreliminaryCheck_AServiceTask_LeftEyeTest"><di:waypoint x="600" y="140" /><di:waypoint x="650" y="140" /><di:waypoint x="700" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AServiceTask_LeftEyeTest_AServiceTask_RightEyeTest" bpmnElement="Flow_AServiceTask_LeftEyeTest_AServiceTask_RightEyeTest"><di:waypoint x="800" y="140" /><di:waypoint x="850" y="140" /><di:waypoint x="900" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AServiceTask_RightEyeTest_AServiceTask_DataAnalysis" bpmnElement="Flow_AServiceTask_RightEyeTest_AServiceTask_DataAnalysis"><di:waypoint x="1000" y="140" /><di:waypoint x="1050" y="140" /><di:waypoint x="1100" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AServiceTask_DataAnalysis_AExclusiveGateway_1" bpmnElement="Flow_AServiceTask_DataAnalysis_AExclusiveGateway_1"><di:waypoint x="1200" y="140" /><di:waypoint x="1250" y="140" /><di:waypoint x="1300" y="118" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AExclusiveGateway_1_AUserTask_AdviceNormal" bpmnElement="Flow_AExclusiveGateway_1_AUserTask_AdviceNormal"><di:waypoint x="1336" y="118" /><di:waypoint x="1418" y="118" /><di:waypoint x="1500" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AExclusiveGateway_1_AUserTask_AdviceGlasses" bpmnElement="Flow_AExclusiveGateway_1_AUserTask_AdviceGlasses"><di:waypoint x="1336" y="118" /><di:waypoint x="1418" y="118" /><di:waypoint x="1500" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AUserTask_AdviceNormal_AEndEvent_1" bpmnElement="Flow_AUserTask_AdviceNormal_AEndEvent_1"><di:waypoint x="1600" y="140" /><di:waypoint x="1650" y="140" /><di:waypoint x="1700" y="118" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AUserTask_AdviceGlasses_AEndEvent_1" bpmnElement="Flow_AUserTask_AdviceGlasses_AEndEvent_1"><di:waypoint x="1600" y="140" /><di:waypoint x="1650" y="140" /><di:waypoint x="1700" y="118" /></bpmndi:BPMNEdge></bpmndi:BPMNPlane></bpmndi:BPMNDiagram></bpmn:definitions>', 0, NULL, NULL, 0, NULL, CAST(0x0000B29200A59A4B AS DateTime), NULL)
INSERT [dbo].[WfProcess] ([ID], [ProcessID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1630, N'ICU_Diagnosis_Process_fdhu', N'1', N'ICU诊断与治疗流程', N'ICU_Diagnosis_Process_fdhu', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="utf-8"?><bpmn:definitions xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL"><bpmn:process id="ICU_Diagnosis_Process_fdhu" name="ICU诊断与治疗流程" isExecutable="false"><bpmn:startEvent id="AStartEvent_1" name="患者接收" /><bpmn:task id="ATask_1" name="初步生命体征评估" /><bpmn:task id="ATask_2" name="执行紧急检查（血气分析/影像学）" /><bpmn:exclusiveGateway id="AExclusiveGateway_1" name="是否需要插管？" /><bpmn:task id="ATask_3" name="气管插管操作" /><bpmn:task id="ATask_4" name="多学科会诊" /><bpmn:task id="ATask_5" name="实施目标治疗（如CRRT/ECMO）" /><bpmn:intermediateCatchEvent id="AIntermediateEvent_1" name="每小时再评估" /><bpmn:endEvent id="AEndEvent_1" name="转出ICU/出院" /><bpmn:sequenceFlow id="Flow_AStartEvent_1_ATask_1" sourceRef="AStartEvent_1" targetRef="ATask_1" /><bpmn:sequenceFlow id="Flow_ATask_1_ATask_2" sourceRef="ATask_1" targetRef="ATask_2" /><bpmn:sequenceFlow id="Flow_ATask_2_AExclusiveGateway_1" sourceRef="ATask_2" targetRef="AExclusiveGateway_1" /><bpmn:sequenceFlow id="Flow_AExclusiveGateway_1_ATask_3" sourceRef="AExclusiveGateway_1" targetRef="ATask_3" /><bpmn:sequenceFlow id="Flow_AExclusiveGateway_1_ATask_4" sourceRef="AExclusiveGateway_1" targetRef="ATask_4" /><bpmn:sequenceFlow id="Flow_ATask_3_ATask_4" sourceRef="ATask_3" targetRef="ATask_4" /><bpmn:sequenceFlow id="Flow_ATask_4_ATask_5" sourceRef="ATask_4" targetRef="ATask_5" /><bpmn:sequenceFlow id="Flow_ATask_5_AIntermediateEvent_1" sourceRef="ATask_5" targetRef="AIntermediateEvent_1" /><bpmn:sequenceFlow id="Flow_AIntermediateEvent_1_AEndEvent_1" sourceRef="AIntermediateEvent_1" targetRef="AEndEvent_1" /><bpmn:sequenceFlow id="Flow_AIntermediateEvent_1_ATask_2" sourceRef="AIntermediateEvent_1" targetRef="ATask_2" /></bpmn:process><bpmndi:BPMNDiagram id="BPMNDiagram_1"><bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="ICU_Diagnosis_Process_fdhu"><bpmndi:BPMNShape id="Shape_AStartEvent_1" bpmnElement="AStartEvent_1"><dc:Bounds x="100" y="100" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_ATask_1" bpmnElement="ATask_1"><dc:Bounds x="300" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_ATask_2" bpmnElement="ATask_2"><dc:Bounds x="500" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AExclusiveGateway_1" bpmnElement="AExclusiveGateway_1"><dc:Bounds x="700" y="100" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_ATask_3" bpmnElement="ATask_3"><dc:Bounds x="900" y="50" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_ATask_4" bpmnElement="ATask_4"><dc:Bounds x="900" y="150" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_ATask_5" bpmnElement="ATask_5"><dc:Bounds x="1100" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AIntermediateEvent_1" bpmnElement="AIntermediateEvent_1"><dc:Bounds x="1300" y="100" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AEndEvent_1" bpmnElement="AEndEvent_1"><dc:Bounds x="1500" y="100" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNEdge id="Edge_AStartEvent_1_ATask_1" bpmnElement="Flow_AStartEvent_1_ATask_1"><di:waypoint x="136" y="118" /><di:waypoint x="218" y="118" /><di:waypoint x="300" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_ATask_1_ATask_2" bpmnElement="Flow_ATask_1_ATask_2"><di:waypoint x="400" y="140" /><di:waypoint x="450" y="140" /><di:waypoint x="500" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_ATask_2_AExclusiveGateway_1" bpmnElement="Flow_ATask_2_AExclusiveGateway_1"><di:waypoint x="600" y="140" /><di:waypoint x="650" y="140" /><di:waypoint x="700" y="118" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AExclusiveGateway_1_ATask_3" bpmnElement="Flow_AExclusiveGateway_1_ATask_3"><di:waypoint x="736" y="118" /><di:waypoint x="786" y="118" /><di:waypoint x="786" y="90" /><di:waypoint x="900" y="90" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AExclusiveGateway_1_ATask_4" bpmnElement="Flow_AExclusiveGateway_1_ATask_4"><di:waypoint x="736" y="118" /><di:waypoint x="786" y="118" /><di:waypoint x="786" y="190" /><di:waypoint x="900" y="190" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_ATask_3_ATask_4" bpmnElement="Flow_ATask_3_ATask_4"><di:waypoint x="1000" y="90" /><di:waypoint x="1050" y="90" /><di:waypoint x="1050" y="190" /><di:waypoint x="900" y="190" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_ATask_4_ATask_5" bpmnElement="Flow_ATask_4_ATask_5"><di:waypoint x="1000" y="190" /><di:waypoint x="1050" y="190" /><di:waypoint x="1050" y="140" /><di:waypoint x="1100" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_ATask_5_AIntermediateEvent_1" bpmnElement="Flow_ATask_5_AIntermediateEvent_1"><di:waypoint x="1200" y="140" /><di:waypoint x="1250" y="140" /><di:waypoint x="1300" y="118" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AIntermediateEvent_1_AEndEvent_1" bpmnElement="Flow_AIntermediateEvent_1_AEndEvent_1"><di:waypoint x="1336" y="118" /><di:waypoint x="1418" y="118" /><di:waypoint x="1500" y="118" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AIntermediateEvent_1_ATask_2" bpmnElement="Flow_AIntermediateEvent_1_ATask_2"><di:waypoint x="1336" y="118" /><di:waypoint x="918" y="118" /><di:waypoint x="500" y="140" /></bpmndi:BPMNEdge></bpmndi:BPMNPlane></bpmndi:BPMNDiagram></bpmn:definitions>', 0, NULL, NULL, 0, NULL, CAST(0x0000B29200A82427 AS DateTime), NULL)
INSERT [dbo].[WfProcess] ([ID], [ProcessID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1631, N'InterviewProcess_zxof', N'1', N'Employee Interview Process', N'InterviewProcess_zxof', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="utf-8"?><bpmn:definitions xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL"><bpmn:process id="InterviewProcess_zxof" name="Employee Interview Process" isExecutable="false"><bpmn:startEvent id="AStartEvent" name="Start" /><bpmn:userTask id="AResumeScreening" name="Resume Screening" assignee="HR" /><bpmn:exclusiveGateway id="AGateway_Screening" name="Qualified?" /><bpmn:userTask id="AInitialInterview" name="Initial Interview" assignee="HiringManager" /><bpmn:serviceTask id="AWrittenTest" name="Written Test" implementation="Send test via email" /><bpmn:userTask id="AFinalInterview" name="Final Interview" assignee="DepartmentHead" /><bpmn:exclusiveGateway id="AGateway_Interview" name="Need Extra Interview?" /><bpmn:userTask id="ADecisionMeeting" name="Hiring Decision Meeting" assignee="" /><bpmn:serviceTask id="ASendOffer" name="Send Offer Letter" implementation="Generate offer letter" /><bpmn:userTask id="AOnboardingPrep" name="Onboarding Preparation" assignee="HR" /><bpmn:endEvent id="AEndEvent" name="End" /><bpmn:serviceTask id="ARejectEnd" name="Rejection Notice" implementation="Send rejection email" /><bpmn:sequenceFlow id="Flow_AStartEvent_AResumeScreening" sourceRef="AStartEvent" targetRef="AResumeScreening" /><bpmn:sequenceFlow id="Flow_AResumeScreening_AGateway_Screening" sourceRef="AResumeScreening" targetRef="AGateway_Screening" /><bpmn:sequenceFlow id="Flow_AGateway_Screening_AInitialInterview" sourceRef="AGateway_Screening" targetRef="AInitialInterview" /><bpmn:sequenceFlow id="Flow_AGateway_Screening_ARejectEnd" sourceRef="AGateway_Screening" targetRef="ARejectEnd" /><bpmn:sequenceFlow id="Flow_AInitialInterview_AWrittenTest" sourceRef="AInitialInterview" targetRef="AWrittenTest" /><bpmn:sequenceFlow id="Flow_AWrittenTest_AFinalInterview" sourceRef="AWrittenTest" targetRef="AFinalInterview" /><bpmn:sequenceFlow id="Flow_AFinalInterview_AGateway_Interview" sourceRef="AFinalInterview" targetRef="AGateway_Interview" /><bpmn:sequenceFlow id="Flow_AGateway_Interview_ADecisionMeeting" sourceRef="AGateway_Interview" targetRef="ADecisionMeeting" /><bpmn:sequenceFlow id="Flow_AGateway_Interview_AInitialInterview" sourceRef="AGateway_Interview" targetRef="AInitialInterview" /><bpmn:sequenceFlow id="Flow_ADecisionMeeting_ASendOffer" sourceRef="ADecisionMeeting" targetRef="ASendOffer" /><bpmn:sequenceFlow id="Flow_ASendOffer_AOnboardingPrep" sourceRef="ASendOffer" targetRef="AOnboardingPrep" /><bpmn:sequenceFlow id="Flow_AOnboardingPrep_AEndEvent" sourceRef="AOnboardingPrep" targetRef="AEndEvent" /><bpmn:sequenceFlow id="Flow_ARejectEnd_AEndEvent" sourceRef="ARejectEnd" targetRef="AEndEvent" /></bpmn:process><bpmndi:BPMNDiagram id="BPMNDiagram_1"><bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="InterviewProcess_zxof"><bpmndi:BPMNShape id="Shape_AStartEvent" bpmnElement="AStartEvent"><dc:Bounds x="100" y="100" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AResumeScreening" bpmnElement="AResumeScreening"><dc:Bounds x="300" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AGateway_Screening" bpmnElement="AGateway_Screening"><dc:Bounds x="500" y="100" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AInitialInterview" bpmnElement="AInitialInterview"><dc:Bounds x="1500" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AWrittenTest" bpmnElement="AWrittenTest"><dc:Bounds x="900" y="50" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AFinalInterview" bpmnElement="AFinalInterview"><dc:Bounds x="1100" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AGateway_Interview" bpmnElement="AGateway_Interview"><dc:Bounds x="1300" y="100" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_ADecisionMeeting" bpmnElement="ADecisionMeeting"><dc:Bounds x="1500" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_ASendOffer" bpmnElement="ASendOffer"><dc:Bounds x="1700" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AOnboardingPrep" bpmnElement="AOnboardingPrep"><dc:Bounds x="1900" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AEndEvent" bpmnElement="AEndEvent"><dc:Bounds x="900" y="150" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_ARejectEnd" bpmnElement="ARejectEnd"><dc:Bounds x="700" y="150" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNEdge id="Edge_AStartEvent_AResumeScreening" bpmnElement="Flow_AStartEvent_AResumeScreening"><di:waypoint x="136" y="118" /><di:waypoint x="218" y="118" /><di:waypoint x="300" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AResumeScreening_AGateway_Screening" bpmnElement="Flow_AResumeScreening_AGateway_Screening"><di:waypoint x="400" y="140" /><di:waypoint x="450" y="140" /><di:waypoint x="500" y="118" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AGateway_Screening_AInitialInterview" bpmnElement="Flow_AGateway_Screening_AInitialInterview"><di:waypoint x="536" y="118" /><di:waypoint x="1018" y="118" /><di:waypoint x="1500" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AGateway_Screening_ARejectEnd" bpmnElement="Flow_AGateway_Screening_ARejectEnd"><di:waypoint x="536" y="118" /><di:waypoint x="586" y="118" /><di:waypoint x="586" y="190" /><di:waypoint x="700" y="190" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AInitialInterview_AWrittenTest" bpmnElement="Flow_AInitialInterview_AWrittenTest"><di:waypoint x="1600" y="140" /><di:waypoint x="1650" y="140" /><di:waypoint x="1650" y="90" /><di:waypoint x="900" y="90" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AWrittenTest_AFinalInterview" bpmnElement="Flow_AWrittenTest_AFinalInterview"><di:waypoint x="1000" y="90" /><di:waypoint x="1050" y="90" /><di:waypoint x="1050" y="140" /><di:waypoint x="1100" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AFinalInterview_AGateway_Interview" bpmnElement="Flow_AFinalInterview_AGateway_Interview"><di:waypoint x="1200" y="140" /><di:waypoint x="1250" y="140" /><di:waypoint x="1300" y="118" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AGateway_Interview_ADecisionMeeting" bpmnElement="Flow_AGateway_Interview_ADecisionMeeting"><di:waypoint x="1336" y="118" /><di:waypoint x="1418" y="118" /><di:waypoint x="1500" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AGateway_Interview_AInitialInterview" bpmnElement="Flow_AGateway_Interview_AInitialInterview"><di:waypoint x="1336" y="118" /><di:waypoint x="1418" y="118" /><di:waypoint x="1500" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_ADecisionMeeting_ASendOffer" bpmnElement="Flow_ADecisionMeeting_ASendOffer"><di:waypoint x="1600" y="140" /><di:waypoint x="1650" y="140" /><di:waypoint x="1700" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_ASendOffer_AOnboardingPrep" bpmnElement="Flow_ASendOffer_AOnboardingPrep"><di:waypoint x="1800" y="140" /><di:waypoint x="1850" y="140" /><di:waypoint x="1900" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AOnboardingPrep_AEndEvent" bpmnElement="Flow_AOnboardingPrep_AEndEvent"><di:waypoint x="2000" y="140" /><di:waypoint x="2050" y="140" /><di:waypoint x="2050" y="168" /><di:waypoint x="900" y="168" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_ARejectEnd_AEndEvent" bpmnElement="Flow_ARejectEnd_AEndEvent"><di:waypoint x="800" y="190" /><di:waypoint x="850" y="190" /><di:waypoint x="900" y="168" /></bpmndi:BPMNEdge></bpmndi:BPMNPlane></bpmndi:BPMNDiagram></bpmn:definitions>', 0, NULL, NULL, 0, NULL, CAST(0x0000B29200C959C6 AS DateTime), NULL)
SET IDENTITY_INSERT [dbo].[WfProcess] OFF
/****** Object:  Table [dbo].[WfLog]    Script Date: 02/28/2025 17:15:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WfLog](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[EventTypeID] [int] NOT NULL,
	[Priority] [int] NOT NULL,
	[Severity] [nvarchar](50) NOT NULL,
	[Title] [nvarchar](256) NOT NULL,
	[Message] [nvarchar](500) NULL,
	[StackTrace] [nvarchar](4000) NULL,
	[InnerStackTrace] [nvarchar](4000) NULL,
	[RequestData] [nvarchar](2000) NULL,
	[Timestamp] [datetime] NOT NULL,
 CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[WfLog] ON
INSERT [dbo].[WfLog] ([ID], [EventTypeID], [Priority], [Severity], [Title], [Message], [StackTrace], [InnerStackTrace], [RequestData], [Timestamp]) VALUES (1, 2, 1, N'HIGH', N'PROCESS REVISE ERROR', N'Can''t revise the current process, there isn''t any sendback operation information!', N'   at Slickflow.Engine.Core.Pattern.NodeMediatorRevise.ExecuteWorkItem() in D:\Cloud365\GitHomeCore-20250120\SfBpmn2\source\Lib\Slickflow.Engine\Core\Pattern\NodeMediatorRevise.cs:line 110
   at Slickflow.Engine.Core.Runtime.WfRuntimeManagerRevise.ExecuteInstanceImp(IDbSession session) in D:\Cloud365\GitHomeCore-20250120\SfBpmn2\source\Lib\Slickflow.Engine\Core\Runtime\WfRuntimeManagerRevise.cs:line 29
   at Slickflow.Engine.Core.Runtime.WfRuntimeManager.Execute(IDbSession session) in D:\Cloud365\GitHomeCore-20250120\SfBpmn2\source\Lib\Slickflow.Engine\Core\Runtime\WfRuntimeManager.cs:line 58
   at Slickflow.Engine.Service.WorkflowService.Revise(IDbConnection conn, IDbTransaction trans) in D:\Cloud365\GitHomeCore-20250120\SfBpmn2\source\Lib\Slickflow.Engine\Service\WorkflowService.cs:line 1711', NULL, N'{"AppName":"Order-Books","AppInstanceID":"123","AppInstanceCode":"123-code","ProcessID":"employeeLeaveRequestProcess_hwpb","ProcessCode":null,"Version":"1","UserID":"01","UserName":"Zero","CompanyID":null,"TaskID":11,"ActivityInstanceID":null,"ApprovalStatus":0,"Conditions":{},"DynamicVariables":null,"ControlParameterSheet":null,"NextActivityPerformers":{"AdepartmentManagerApproval":[{"UserID":"201","UserName":"Terrisa(模拟)"}]},"NextPerformerType":0,"MessageTopic":null}', CAST(0x0000B28D0009EDE9 AS DateTime))
INSERT [dbo].[WfLog] ([ID], [EventTypeID], [Priority], [Severity], [Title], [Message], [StackTrace], [InnerStackTrace], [RequestData], [Timestamp]) VALUES (2, 2, 1, N'HIGH', N'PROCESS RUN ERROR', N'Can''t revise the current process, there isn''t any sendback operation information!', N'   at Slickflow.Engine.Core.Pattern.NodeMediatorRevise.ExecuteWorkItem() in D:\Cloud365\GitHomeCore-20250120\SfBpmn2\source\Lib\Slickflow.Engine\Core\Pattern\NodeMediatorRevise.cs:line 110
   at Slickflow.Engine.Core.Runtime.WfRuntimeManagerRevise.ExecuteInstanceImp(IDbSession session) in D:\Cloud365\GitHomeCore-20250120\SfBpmn2\source\Lib\Slickflow.Engine\Core\Runtime\WfRuntimeManagerRevise.cs:line 29
   at Slickflow.Engine.Core.Runtime.WfRuntimeManager.Execute(IDbSession session) in D:\Cloud365\GitHomeCore-20250120\SfBpmn2\source\Lib\Slickflow.Engine\Core\Runtime\WfRuntimeManager.cs:line 58', NULL, N'{"AppName":"Order-Books","AppInstanceID":"123","AppInstanceCode":null,"ProcessID":null,"ProcessCode":null,"Version":null,"UserID":"01","UserName":"Zero","CompanyID":null,"TaskID":null,"ActivityInstanceID":null,"ApprovalStatus":0,"Conditions":{},"DynamicVariables":null,"ControlParameterSheet":null,"NextActivityPerformers":{"AdepartmentManagerApproval":[{"UserID":"201","UserName":"Terrisa(模拟)"}]},"NextPerformerType":0,"MessageTopic":null}', CAST(0x0000B28D0009EDC5 AS DateTime))
INSERT [dbo].[WfLog] ([ID], [EventTypeID], [Priority], [Severity], [Title], [Message], [StackTrace], [InnerStackTrace], [RequestData], [Timestamp]) VALUES (3, 1, 2, N'NORMAL', N'An error occurred when generating process by AI', N'this is a testing...', N'   at Slickflow.AI.Model.BpmnAIService.GenerateProcessByAI(AIRequestEntity request) in D:\Cloud365\GitHomeCore\SfBpmn2\source\lib\Slickflow.AI\Model\BpmnAIService.cs:line 273', NULL, NULL, CAST(0x0000B291015FF0A6 AS DateTime))
INSERT [dbo].[WfLog] ([ID], [EventTypeID], [Priority], [Severity], [Title], [Message], [StackTrace], [InnerStackTrace], [RequestData], [Timestamp]) VALUES (4, 1, 2, N'NORMAL', N'An error occurred when generating process by AI', N'this is a testing...', N'   at Slickflow.AI.Model.BpmnAIService.GenerateProcessByAI(AIRequestEntity request) in D:\Cloud365\GitHomeCore\SfBpmn2\source\lib\Slickflow.AI\Model\BpmnAIService.cs:line 277', NULL, N'"{\n  \"Process\": {\n    \"id\": \"FireworkDetectionProcess\",\n    \"name\": \"烟花检测流程\"\n  },\n  \"ProcessNodes\": [\n    {\n      \"id\": \"StartEvent\",\n      \"name\": \"开始检测\",\n      \"type\": \"StartEvent\"\n    },\n    {\n      \"id\": \"CaptureImage\",\n      \"name\": \"捕捉图像\",\n      \"type\": \"ServiceTask\"\n    },\n    {\n      \"id\": \"AnalyzeImage\",\n      \"name\": \"分析图像\",\n      \"type\": \"ServiceTask\"\n    },\n    {\n      \"id\": \"DetectFirework\",\n      \"name\": \"检测烟花\",\n      \"type\": \"ServiceTask\"\n    },\n    {\n      \"id\": \"LogResult\",\n      \"name\": \"记录结果\",\n      \"type\": \"ServiceTask\"\n    },\n    {\n      \"id\": \"EndEvent\",\n      \"name\": \"结束检测\",\n      \"type\": \"EndEvent\"\n    }\n  ],\n  \"SequenceFlows\": [\n    {\n      \"id\": \"Flow1\",\n      \"sourceRef\": \"StartEvent\",\n      \"targetRef\": \"CaptureImage\"\n    },\n    {\n      \"id\": \"Flow2\",\n      \"sourceRef\": \"CaptureImage\",\n      \"targetRef\": \"AnalyzeImage\"\n    },\n    {\n      \"id\": \"Flow3\",\n      \"sourceRef\": \"AnalyzeImage\",\n      \"targetRef\": \"DetectFirework\"\n    },\n    {\n      \"id\": \"Flow4\",\n      \"sourceRef\": \"DetectFirework\",\n      \"targetRef\": \"LogResult\"\n    },\n    {\n      \"id\": \"Flow5\",\n      \"sourceRef\": \"LogResult\",\n      \"targetRef\": \"EndEvent\"\n    }\n  ]\n}"', CAST(0x0000B2910160CC1C AS DateTime))
INSERT [dbo].[WfLog] ([ID], [EventTypeID], [Priority], [Severity], [Title], [Message], [StackTrace], [InnerStackTrace], [RequestData], [Timestamp]) VALUES (5, 1, 2, N'NORMAL', N'An error occurred when generating process by AI', N'One or more errors occurred. (One or more errors occurred. (API调用失败: Response status code does not indicate success: 400 (Bad Request).))', N'   at System.Threading.Tasks.Task.ThrowIfExceptional(Boolean includeTaskCanceledExceptions)
   at System.Threading.Tasks.Task`1.GetResultCore(Boolean waitCompletionNotification)
   at Slickflow.AI.Model.BpmnAIService.GenerateProcessByAI(AIRequestEntity request) in D:\Cloud365\GitHomeCore\SfBpmn2\source\lib\Slickflow.AI\Model\BpmnAIService.cs:line 275', N'   at System.Threading.Tasks.Task.ThrowIfExceptional(Boolean includeTaskCanceledExceptions)
   at System.Threading.Tasks.Task`1.GetResultCore(Boolean waitCompletionNotification)
   at Slickflow.AI.Utility.TextSimilarity.GetAnswer(String question) in D:\Cloud365\GitHomeCore\SfBpmn2\source\lib\Slickflow.AI\Utility\EnhancedDeepSeekService.cs:line 174
   at Slickflow.AI.Utility.LocalCacheService.GetAnswer(String question) in D:\Cloud365\GitHomeCore\SfBpmn2\source\lib\Slickflow.AI\Utility\EnhancedDeepSeekService.cs:line 64
   at Slickflow.AI.Utility.EnhancedDeepSeekService.GetAnswerAsync(String question) in D:\Cloud365\GitHomeCore\SfBpmn2\source\lib\Slickflow.AI\Utility\EnhancedDeepSeekService.cs:line 23', N'""', CAST(0x0000B2920090E436 AS DateTime))
INSERT [dbo].[WfLog] ([ID], [EventTypeID], [Priority], [Severity], [Title], [Message], [StackTrace], [InnerStackTrace], [RequestData], [Timestamp]) VALUES (6, 1, 2, N'NORMAL', N'An error occurred when generating process by AI', N'One or more errors occurred. (One or more errors occurred. (API调用失败: Response status code does not indicate success: 400 (Bad Request).))', N'   at System.Threading.Tasks.Task.ThrowIfExceptional(Boolean includeTaskCanceledExceptions)
   at System.Threading.Tasks.Task`1.GetResultCore(Boolean waitCompletionNotification)
   at Slickflow.AI.Model.BpmnAIService.GenerateProcessByAI(AIRequestEntity request) in D:\Cloud365\GitHomeCore\SfBpmn2\source\lib\Slickflow.AI\Model\BpmnAIService.cs:line 275', N'   at System.Threading.Tasks.Task.ThrowIfExceptional(Boolean includeTaskCanceledExceptions)
   at System.Threading.Tasks.Task`1.GetResultCore(Boolean waitCompletionNotification)
   at Slickflow.AI.Utility.TextSimilarity.GetAnswer(String question) in D:\Cloud365\GitHomeCore\SfBpmn2\source\lib\Slickflow.AI\Utility\EnhancedDeepSeekService.cs:line 174
   at Slickflow.AI.Utility.LocalCacheService.GetAnswer(String question) in D:\Cloud365\GitHomeCore\SfBpmn2\source\lib\Slickflow.AI\Utility\EnhancedDeepSeekService.cs:line 64
   at Slickflow.AI.Utility.EnhancedDeepSeekService.GetAnswerAsync(String question) in D:\Cloud365\GitHomeCore\SfBpmn2\source\lib\Slickflow.AI\Utility\EnhancedDeepSeekService.cs:line 23', N'""', CAST(0x0000B292009224EE AS DateTime))
INSERT [dbo].[WfLog] ([ID], [EventTypeID], [Priority], [Severity], [Title], [Message], [StackTrace], [InnerStackTrace], [RequestData], [Timestamp]) VALUES (7, 1, 2, N'NORMAL', N'An error occurred when generating process by AI', N'One or more errors occurred. (One or more errors occurred. (API调用失败: Response status code does not indicate success: 400 (Bad Request).))', N'   at System.Threading.Tasks.Task.ThrowIfExceptional(Boolean includeTaskCanceledExceptions)
   at System.Threading.Tasks.Task`1.GetResultCore(Boolean waitCompletionNotification)
   at Slickflow.AI.Model.BpmnAIService.GenerateProcessByAI(AIRequestEntity request) in D:\Cloud365\GitHomeCore\SfBpmn2\source\lib\Slickflow.AI\Model\BpmnAIService.cs:line 275', N'   at System.Threading.Tasks.Task.ThrowIfExceptional(Boolean includeTaskCanceledExceptions)
   at System.Threading.Tasks.Task`1.GetResultCore(Boolean waitCompletionNotification)
   at Slickflow.AI.Utility.TextSimilarity.GetAnswer(String question) in D:\Cloud365\GitHomeCore\SfBpmn2\source\lib\Slickflow.AI\Utility\EnhancedDeepSeekService.cs:line 174
   at Slickflow.AI.Utility.LocalCacheService.GetAnswer(String question) in D:\Cloud365\GitHomeCore\SfBpmn2\source\lib\Slickflow.AI\Utility\EnhancedDeepSeekService.cs:line 64
   at Slickflow.AI.Utility.EnhancedDeepSeekService.GetAnswerAsync(String question) in D:\Cloud365\GitHomeCore\SfBpmn2\source\lib\Slickflow.AI\Utility\EnhancedDeepSeekService.cs:line 23', N'""', CAST(0x0000B29200962541 AS DateTime))
INSERT [dbo].[WfLog] ([ID], [EventTypeID], [Priority], [Severity], [Title], [Message], [StackTrace], [InnerStackTrace], [RequestData], [Timestamp]) VALUES (8, 1, 2, N'NORMAL', N'An error occurred when generating process by AI', N'One or more errors occurred. (One or more errors occurred. (API调用失败: Response status code does not indicate success: 400 (Bad Request).))', N'   at System.Threading.Tasks.Task.ThrowIfExceptional(Boolean includeTaskCanceledExceptions)
   at System.Threading.Tasks.Task`1.GetResultCore(Boolean waitCompletionNotification)
   at Slickflow.AI.Model.BpmnAIService.GenerateProcessByAI(AIRequestEntity request) in D:\Cloud365\GitHomeCore\SfBpmn2\source\lib\Slickflow.AI\Model\BpmnAIService.cs:line 275', N'   at System.Threading.Tasks.Task.ThrowIfExceptional(Boolean includeTaskCanceledExceptions)
   at System.Threading.Tasks.Task`1.GetResultCore(Boolean waitCompletionNotification)
   at Slickflow.AI.Utility.TextSimilarity.GetAnswer(String question) in D:\Cloud365\GitHomeCore\SfBpmn2\source\lib\Slickflow.AI\Utility\EnhancedDeepSeekService.cs:line 174
   at Slickflow.AI.Utility.LocalCacheService.GetAnswer(String question) in D:\Cloud365\GitHomeCore\SfBpmn2\source\lib\Slickflow.AI\Utility\EnhancedDeepSeekService.cs:line 64
   at Slickflow.AI.Utility.EnhancedDeepSeekService.GetAnswerAsync(String question) in D:\Cloud365\GitHomeCore\SfBpmn2\source\lib\Slickflow.AI\Utility\EnhancedDeepSeekService.cs:line 23', N'""', CAST(0x0000B29200968F17 AS DateTime))
INSERT [dbo].[WfLog] ([ID], [EventTypeID], [Priority], [Severity], [Title], [Message], [StackTrace], [InnerStackTrace], [RequestData], [Timestamp]) VALUES (9, 1, 2, N'NORMAL', N'An error occurred when generating process by AI', N'Invalid node type: startEvent', N'   at Slickflow.Graph.Convertor.JsonBpmnConvertor.ValidateNodeTypes(ProcessData processData) in D:\Cloud365\GitHomeCore\SfBpmn2\source\lib\Slickflow.Graph\Convertor\JsonBpmnConvertor.cs:line 51
   at Slickflow.Graph.Convertor.JsonBpmnConvertor.ConvertToBpmn(String jsonInput) in D:\Cloud365\GitHomeCore\SfBpmn2\source\lib\Slickflow.Graph\Convertor\JsonBpmnConvertor.cs:line 93
   at Slickflow.AI.Model.BpmnAIService.GenerateProcessByAI(AIRequestEntity request) in D:\Cloud365\GitHomeCore\SfBpmn2\source\lib\Slickflow.AI\Model\BpmnAIService.cs:line 279', NULL, N'"{\n  \"Process\": {\n    \"id\": \"Process_SewageTest\",\n    \"name\": \"污水检测流程\"\n  },\n  \"ProcessNodes\": [\n    {\n      \"id\": \"StartEvent_1\",\n      \"name\": \"开始检测\",\n      \"type\": \"startEvent\"\n    },\n    {\n      \"id\": \"Task_1\",\n      \"name\": \"接收污水样品\",\n      \"type\": \"task\",\n      \"participant\": \"接收部门\"\n    },\n    {\n      \"id\": \"Task_2\",\n      \"name\": \"样品预处理\",\n      \"type\": \"task\",\n      \"participant\": \"实验室技术员\"\n    },\n    {\n      \"id\": \"Task_3\",\n      \"name\": \"检测分析（COD/氨氮等）\",\n      \"type\": \"task\",\n      \"participant\": \"检测设备\"\n    },\n    {\n      \"id\": \"Task_4\",\n      \"name\": \"数据记录与处理\",\n      \"type\": \"task\",\n      \"participant\": \"数据分析师\"\n    },\n    {\n      \"id\": \"Task_5\",\n      \"name\": \"生成检测报告\",\n      \"type\": \"task\",\n      \"participant\": \"报告专员\"\n    },\n    {\n      \"id\": \"ExclusiveGateway_1\",\n      \"name\": \"审核是否通过？\",\n      \"type\": \"exclusiveGateway\"\n    },\n    {\n      \"id\": \"Task_6\",\n      \"name\": \"报告重新检测\",\n      \"type\": \"task\",\n      \"participant\": \"质量控制部\"\n    },\n    {\n      \"id\": \"EndEvent_1\",\n      \"name\": \"报告存档发布\",\n      \"type\": \"endEvent\"\n    }\n  ],\n  \"SequenceFlows\": [\n    {\n      \"id\": \"Flow_1\",\n      \"sourceRef\": \"StartEvent_1\",\n      \"targetRef\": \"Task_1\"\n    },\n    {\n      \"id\": \"Flow_2\",\n      \"sourceRef\": \"Task_1\",\n      \"targetRef\": \"Task_2\"\n    },\n    {\n      \"id\": \"Flow_3\",\n      \"sourceRef\": \"Task_2\",\n      \"targetRef\": \"Task_3\"\n    },\n    {\n      \"id\": \"Flow_4\",\n      \"sourceRef\": \"Task_3\",\n      \"targetRef\": \"Task_4\"\n    },\n    {\n      \"id\": \"Flow_5\",\n      \"sourceRef\": \"Task_4\",\n      \"targetRef\": \"Task_5\"\n    },\n    {\n      \"id\": \"Flow_6\",\n      \"sourceRef\": \"Task_5\",\n      \"targetRef\": \"ExclusiveGateway_1\"\n    },\n    {\n      \"id\": \"Flow_7\",\n      \"sourceRef\":', CAST(0x0000B29200998EBC AS DateTime))
INSERT [dbo].[WfLog] ([ID], [EventTypeID], [Priority], [Severity], [Title], [Message], [StackTrace], [InnerStackTrace], [RequestData], [Timestamp]) VALUES (10, 1, 2, N'NORMAL', N'An error occurred when generating process by AI', N'Unsupported BPMN element type: startEvent', N'   at Slickflow.Graph.Convertor.JsonBpmnConvertor.GetElementType(String type) in D:\Cloud365\GitHomeCore\SfBpmn2\source\lib\Slickflow.Graph\Convertor\JsonBpmnConvertor.cs:line 368
   at Slickflow.Graph.Convertor.JsonBpmnConvertor.CreateNodeElement(XmlDocument xmlDoc, ProcessNode node) in D:\Cloud365\GitHomeCore\SfBpmn2\source\lib\Slickflow.Graph\Convertor\JsonBpmnConvertor.cs:line 170
   at Slickflow.Graph.Convertor.JsonBpmnConvertor.CreateProcessElement(XmlDocument xmlDoc, ProcessData processData) in D:\Cloud365\GitHomeCore\SfBpmn2\source\lib\Slickflow.Graph\Convertor\JsonBpmnConvertor.cs:line 157
   at Slickflow.Graph.Convertor.JsonBpmnConvertor.ConvertToBpmn(String jsonInput) in D:\Cloud365\GitHomeCore\SfBpmn2\source\lib\Slickflow.Graph\Convertor\JsonBpmnConvertor.cs:line 111
   at Slickflow.AI.Model.BpmnAIService.GenerateProcessByAI(AIRequestEntity request) in D:\Cloud365\GitHomeCore\SfBpmn2\source\lib\Slickflow.AI\Model\BpmnAIService.cs:line 279', NULL, N'"{\n  \"Process\": {\n    \"id\": \"Process_SewageTest\",\n    \"name\": \"污水检测流程\"\n  },\n  \"ProcessNodes\": [\n    {\n      \"id\": \"StartEvent_1\",\n      \"name\": \"开始检测\",\n      \"type\": \"startEvent\"\n    },\n    {\n      \"id\": \"Task_1\",\n      \"name\": \"接收污水样品\",\n      \"type\": \"task\",\n      \"participant\": \"接收部门\"\n    },\n    {\n      \"id\": \"Task_2\",\n      \"name\": \"样品预处理\",\n      \"type\": \"task\",\n      \"participant\": \"实验室技术员\"\n    },\n    {\n      \"id\": \"Task_3\",\n      \"name\": \"检测分析（COD/氨氮等）\",\n      \"type\": \"task\",\n      \"participant\": \"检测设备\"\n    },\n    {\n      \"id\": \"Task_4\",\n      \"name\": \"数据记录与处理\",\n      \"type\": \"task\",\n      \"participant\": \"数据分析师\"\n    },\n    {\n      \"id\": \"Task_5\",\n      \"name\": \"生成检测报告\",\n      \"type\": \"task\",\n      \"participant\": \"报告专员\"\n    },\n    {\n      \"id\": \"ExclusiveGateway_1\",\n      \"name\": \"审核是否通过？\",\n      \"type\": \"exclusiveGateway\"\n    },\n    {\n      \"id\": \"Task_6\",\n      \"name\": \"报告重新检测\",\n      \"type\": \"task\",\n      \"participant\": \"质量控制部\"\n    },\n    {\n      \"id\": \"EndEvent_1\",\n      \"name\": \"报告存档发布\",\n      \"type\": \"endEvent\"\n    }\n  ],\n  \"SequenceFlows\": [\n    {\n      \"id\": \"Flow_1\",\n      \"sourceRef\": \"StartEvent_1\",\n      \"targetRef\": \"Task_1\"\n    },\n    {\n      \"id\": \"Flow_2\",\n      \"sourceRef\": \"Task_1\",\n      \"targetRef\": \"Task_2\"\n    },\n    {\n      \"id\": \"Flow_3\",\n      \"sourceRef\": \"Task_2\",\n      \"targetRef\": \"Task_3\"\n    },\n    {\n      \"id\": \"Flow_4\",\n      \"sourceRef\": \"Task_3\",\n      \"targetRef\": \"Task_4\"\n    },\n    {\n      \"id\": \"Flow_5\",\n      \"sourceRef\": \"Task_4\",\n      \"targetRef\": \"Task_5\"\n    },\n    {\n      \"id\": \"Flow_6\",\n      \"sourceRef\": \"Task_5\",\n      \"targetRef\": \"ExclusiveGateway_1\"\n    },\n    {\n      \"id\": \"Flow_7\",\n      \"sourceRef\":', CAST(0x0000B292009D2748 AS DateTime))
INSERT [dbo].[WfLog] ([ID], [EventTypeID], [Priority], [Severity], [Title], [Message], [StackTrace], [InnerStackTrace], [RequestData], [Timestamp]) VALUES (11, 1, 2, N'NORMAL', N'An error occurred when generating process by AI', N'One or more errors occurred. (One or more errors occurred. (API调用失败: Response status code does not indicate success: 400 (Bad Request).))', N'   at System.Threading.Tasks.Task.ThrowIfExceptional(Boolean includeTaskCanceledExceptions)
   at System.Threading.Tasks.Task`1.GetResultCore(Boolean waitCompletionNotification)
   at Slickflow.AI.Model.BpmnAIService.GenerateProcessByAI(AIRequestEntity request) in D:\Cloud365\GitHomeCore\SfBpmn2\source\lib\Slickflow.AI\Model\BpmnAIService.cs:line 275', N'   at System.Threading.Tasks.Task.ThrowIfExceptional(Boolean includeTaskCanceledExceptions)
   at System.Threading.Tasks.Task`1.GetResultCore(Boolean waitCompletionNotification)
   at Slickflow.AI.Utility.TextSimilarity.GetAnswer(String question) in D:\Cloud365\GitHomeCore\SfBpmn2\source\lib\Slickflow.AI\Utility\EnhancedDeepSeekService.cs:line 174
   at Slickflow.AI.Utility.LocalCacheService.GetAnswer(String question) in D:\Cloud365\GitHomeCore\SfBpmn2\source\lib\Slickflow.AI\Utility\EnhancedDeepSeekService.cs:line 64
   at Slickflow.AI.Utility.EnhancedDeepSeekService.GetAnswerAsync(String question) in D:\Cloud365\GitHomeCore\SfBpmn2\source\lib\Slickflow.AI\Utility\EnhancedDeepSeekService.cs:line 23', N'""', CAST(0x0000B292009EB374 AS DateTime))
INSERT [dbo].[WfLog] ([ID], [EventTypeID], [Priority], [Severity], [Title], [Message], [StackTrace], [InnerStackTrace], [RequestData], [Timestamp]) VALUES (12, 1, 2, N'NORMAL', N'An error occurred when generating process by AI', N'One or more errors occurred. (One or more errors occurred. (A task was canceled.))', N'   at System.Threading.Tasks.Task.ThrowIfExceptional(Boolean includeTaskCanceledExceptions)
   at System.Threading.Tasks.Task`1.GetResultCore(Boolean waitCompletionNotification)
   at Slickflow.AI.Model.BpmnAIService.GenerateProcessByAI(AIRequestEntity request) in D:\Cloud365\GitHomeCore\SfBpmn2\source\lib\Slickflow.AI\Model\BpmnAIService.cs:line 275', N'   at System.Threading.Tasks.Task.ThrowIfExceptional(Boolean includeTaskCanceledExceptions)
   at System.Threading.Tasks.Task`1.GetResultCore(Boolean waitCompletionNotification)
   at Slickflow.AI.Utility.TextSimilarity.GetAnswer(String question) in D:\Cloud365\GitHomeCore\SfBpmn2\source\lib\Slickflow.AI\Utility\EnhancedDeepSeekService.cs:line 174
   at Slickflow.AI.Utility.LocalCacheService.GetAnswer(String question) in D:\Cloud365\GitHomeCore\SfBpmn2\source\lib\Slickflow.AI\Utility\EnhancedDeepSeekService.cs:line 64
   at Slickflow.AI.Utility.EnhancedDeepSeekService.GetAnswerAsync(String question) in D:\Cloud365\GitHomeCore\SfBpmn2\source\lib\Slickflow.AI\Utility\EnhancedDeepSeekService.cs:line 23', N'""', CAST(0x0000B29200A2FBCC AS DateTime))
INSERT [dbo].[WfLog] ([ID], [EventTypeID], [Priority], [Severity], [Title], [Message], [StackTrace], [InnerStackTrace], [RequestData], [Timestamp]) VALUES (13, 1, 2, N'NORMAL', N'An error occurred when generating process by AI', N'Invalid node type: IntermediateCatchEvent', N'   at Slickflow.Graph.Convertor.JsonBpmnConvertor.ValidateNodeTypes(ProcessData processData) in D:\Cloud365\GitHomeCore\SfBpmn2\source\lib\Slickflow.Graph\Convertor\JsonBpmnConvertor.cs:line 54
   at Slickflow.Graph.Convertor.JsonBpmnConvertor.ConvertToBpmn(String jsonInput) in D:\Cloud365\GitHomeCore\SfBpmn2\source\lib\Slickflow.Graph\Convertor\JsonBpmnConvertor.cs:line 96
   at Slickflow.AI.Model.BpmnAIService.GenerateProcessByAI(AIRequestEntity request) in D:\Cloud365\GitHomeCore\SfBpmn2\source\lib\Slickflow.AI\Model\BpmnAIService.cs:line 279', NULL, N'"{\n  \"Process\": {\n    \"id\": \"ICU_Diagnosis_Process\",\n    \"name\": \"ICU诊断与治疗流程\"\n  },\n  \"ProcessNodes\": [\n    {\n      \"id\": \"StartEvent_1\",\n      \"name\": \"患者接收\",\n      \"type\": \"StartEvent\"\n    },\n    {\n      \"id\": \"Task_1\",\n      \"name\": \"初步生命体征评估\",\n      \"type\": \"Task\"\n    },\n    {\n      \"id\": \"Task_2\",\n      \"name\": \"执行紧急检查（血气分析/影像学）\",\n      \"type\": \"Task\"\n    },\n    {\n      \"id\": \"ExclusiveGateway_1\",\n      \"name\": \"是否需要插管？\",\n      \"type\": \"ExclusiveGateway\"\n    },\n    {\n      \"id\": \"Task_3\",\n      \"name\": \"气管插管操作\",\n      \"type\": \"Task\"\n    },\n    {\n      \"id\": \"Task_4\",\n      \"name\": \"多学科会诊\",\n      \"type\": \"Task\"\n    },\n    {\n      \"id\": \"Task_5\",\n      \"name\": \"实施目标治疗（如CRRT/ECMO）\",\n      \"type\": \"Task\"\n    },\n    {\n      \"id\": \"IntermediateEvent_1\",\n      \"name\": \"每小时再评估\",\n      \"type\": \"IntermediateCatchEvent\"\n    },\n    {\n      \"id\": \"EndEvent_1\",\n      \"name\": \"转出ICU/出院\",\n      \"type\": \"EndEvent\"\n    }\n  ],\n  \"SequenceFlows\": [\n    {\"id\": \"Flow_1\", \"sourceRef\": \"StartEvent_1\", \"targetRef\": \"Task_1\"},\n    {\"id\": \"Flow_2\", \"sourceRef\": \"Task_1\", \"targetRef\": \"Task_2\"},\n    {\"id\": \"Flow_3\", \"sourceRef\": \"Task_2\", \"targetRef\": \"ExclusiveGateway_1\"},\n    {\"id\": \"Flow_4\", \"sourceRef\": \"ExclusiveGateway_1\", \"targetRef\": \"Task_3\", \"condition\": \"是\"},\n    {\"id\": \"Flow_5\", \"sourceRef\": \"ExclusiveGateway_1\", \"targetRef\": \"Task_4\", \"condition\": \"否\"},\n    {\"id\": \"Flow_6\", \"sourceRef\": \"Task_3\", \"targetRef\": \"Task_4\"},\n    {\"id\": \"Flow_7\", \"sourceRef\": \"Task_4\", \"targetRef\": \"Task_5\"},\n    {\"id\": \"Flow_8\", \"sourceRef\": \"Task_5\", \"targetRef\": \"IntermediateEvent_1\"},\n    {\"id\": \"Flow_9\", \"sourceRef\": \"IntermediateEvent_1\", \"targetRef\": \"EndEvent_1\", \"condition\": \"稳定\"},\n    {\"id\', CAST(0x0000B29200A3F988 AS DateTime))
INSERT [dbo].[WfLog] ([ID], [EventTypeID], [Priority], [Severity], [Title], [Message], [StackTrace], [InnerStackTrace], [RequestData], [Timestamp]) VALUES (14, 1, 2, N'NORMAL', N'An error occurred when generating process by AI', N'Unsupported BPMN element type: startEvent', N'   at Slickflow.Graph.Convertor.JsonBpmnConvertor.GetElementType(String type) in D:\Cloud365\GitHomeCore\SfBpmn2\source\lib\Slickflow.Graph\Convertor\JsonBpmnConvertor.cs:line 372
   at Slickflow.Graph.Convertor.JsonBpmnConvertor.CreateNodeElement(XmlDocument xmlDoc, ProcessNode node) in D:\Cloud365\GitHomeCore\SfBpmn2\source\lib\Slickflow.Graph\Convertor\JsonBpmnConvertor.cs:line 172
   at Slickflow.Graph.Convertor.JsonBpmnConvertor.CreateProcessElement(XmlDocument xmlDoc, ProcessData processData) in D:\Cloud365\GitHomeCore\SfBpmn2\source\lib\Slickflow.Graph\Convertor\JsonBpmnConvertor.cs:line 159
   at Slickflow.Graph.Convertor.JsonBpmnConvertor.ConvertToBpmn(String jsonInput) in D:\Cloud365\GitHomeCore\SfBpmn2\source\lib\Slickflow.Graph\Convertor\JsonBpmnConvertor.cs:line 113
   at Slickflow.AI.Model.BpmnAIService.GenerateProcessByAI(AIRequestEntity request) in D:\Cloud365\GitHomeCore\SfBpmn2\source\lib\Slickflow.AI\Model\BpmnAIService.cs:line 279', NULL, N'"{\n  \"Process\": {\n    \"id\": \"Process_SewageTest\",\n    \"name\": \"污水检测流程\"\n  },\n  \"ProcessNodes\": [\n    {\n      \"id\": \"StartEvent_1\",\n      \"name\": \"开始检测\",\n      \"type\": \"startEvent\"\n    },\n    {\n      \"id\": \"Task_1\",\n      \"name\": \"接收污水样品\",\n      \"type\": \"task\",\n      \"participant\": \"接收部门\"\n    },\n    {\n      \"id\": \"Task_2\",\n      \"name\": \"样品预处理\",\n      \"type\": \"task\",\n      \"participant\": \"实验室技术员\"\n    },\n    {\n      \"id\": \"Task_3\",\n      \"name\": \"检测分析（COD/氨氮等）\",\n      \"type\": \"task\",\n      \"participant\": \"检测设备\"\n    },\n    {\n      \"id\": \"Task_4\",\n      \"name\": \"数据记录与处理\",\n      \"type\": \"task\",\n      \"participant\": \"数据分析师\"\n    },\n    {\n      \"id\": \"Task_5\",\n      \"name\": \"生成检测报告\",\n      \"type\": \"task\",\n      \"participant\": \"报告专员\"\n    },\n    {\n      \"id\": \"ExclusiveGateway_1\",\n      \"name\": \"审核是否通过？\",\n      \"type\": \"exclusiveGateway\"\n    },\n    {\n      \"id\": \"Task_6\",\n      \"name\": \"报告重新检测\",\n      \"type\": \"task\",\n      \"participant\": \"质量控制部\"\n    },\n    {\n      \"id\": \"EndEvent_1\",\n      \"name\": \"报告存档发布\",\n      \"type\": \"endEvent\"\n    }\n  ],\n  \"SequenceFlows\": [\n    {\n      \"id\": \"Flow_1\",\n      \"sourceRef\": \"StartEvent_1\",\n      \"targetRef\": \"Task_1\"\n    },\n    {\n      \"id\": \"Flow_2\",\n      \"sourceRef\": \"Task_1\",\n      \"targetRef\": \"Task_2\"\n    },\n    {\n      \"id\": \"Flow_3\",\n      \"sourceRef\": \"Task_2\",\n      \"targetRef\": \"Task_3\"\n    },\n    {\n      \"id\": \"Flow_4\",\n      \"sourceRef\": \"Task_3\",\n      \"targetRef\": \"Task_4\"\n    },\n    {\n      \"id\": \"Flow_5\",\n      \"sourceRef\": \"Task_4\",\n      \"targetRef\": \"Task_5\"\n    },\n    {\n      \"id\": \"Flow_6\",\n      \"sourceRef\": \"Task_5\",\n      \"targetRef\": \"ExclusiveGateway_1\"\n    },\n    {\n      \"id\": \"Flow_7\",\n      \"sourceRef\":', CAST(0x0000B29200A5B6D3 AS DateTime))
INSERT [dbo].[WfLog] ([ID], [EventTypeID], [Priority], [Severity], [Title], [Message], [StackTrace], [InnerStackTrace], [RequestData], [Timestamp]) VALUES (15, 1, 2, N'NORMAL', N'An error occurred when generating process by AI', N'Unknown node type:IntermediateCatchEvent', N'   at Slickflow.Graph.Convertor.BpmnDefine.GetShapeSize(String activityType) in D:\Cloud365\GitHomeCore\SfBpmn2\source\lib\Slickflow.Graph\Convertor\ProcessData.cs:line 203
   at Slickflow.Graph.Convertor.JsonBpmnConvertor.CreateShapeElement(XmlDocument xmlDoc, ProcessNode node) in D:\Cloud365\GitHomeCore\SfBpmn2\source\lib\Slickflow.Graph\Convertor\JsonBpmnConvertor.cs:line 266
   at Slickflow.Graph.Convertor.JsonBpmnConvertor.CreateDiagramElement(XmlDocument xmlDoc, ProcessData processData) in D:\Cloud365\GitHomeCore\SfBpmn2\source\lib\Slickflow.Graph\Convertor\JsonBpmnConvertor.cs:line 241
   at Slickflow.Graph.Convertor.JsonBpmnConvertor.ConvertToBpmn(String jsonInput) in D:\Cloud365\GitHomeCore\SfBpmn2\source\lib\Slickflow.Graph\Convertor\JsonBpmnConvertor.cs:line 115
   at Slickflow.AI.Model.BpmnAIService.GenerateProcessByAI(AIRequestEntity request) in D:\Cloud365\GitHomeCore\SfBpmn2\source\lib\Slickflow.AI\Model\BpmnAIService.cs:line 279', NULL, N'"{\n  \"Process\": {\n    \"id\": \"ICU_Diagnosis_Process\",\n    \"name\": \"ICU诊断与治疗流程\"\n  },\n  \"ProcessNodes\": [\n    {\n      \"id\": \"StartEvent_1\",\n      \"name\": \"患者接收\",\n      \"type\": \"StartEvent\"\n    },\n    {\n      \"id\": \"Task_1\",\n      \"name\": \"初步生命体征评估\",\n      \"type\": \"Task\"\n    },\n    {\n      \"id\": \"Task_2\",\n      \"name\": \"执行紧急检查（血气分析/影像学）\",\n      \"type\": \"Task\"\n    },\n    {\n      \"id\": \"ExclusiveGateway_1\",\n      \"name\": \"是否需要插管？\",\n      \"type\": \"ExclusiveGateway\"\n    },\n    {\n      \"id\": \"Task_3\",\n      \"name\": \"气管插管操作\",\n      \"type\": \"Task\"\n    },\n    {\n      \"id\": \"Task_4\",\n      \"name\": \"多学科会诊\",\n      \"type\": \"Task\"\n    },\n    {\n      \"id\": \"Task_5\",\n      \"name\": \"实施目标治疗（如CRRT/ECMO）\",\n      \"type\": \"Task\"\n    },\n    {\n      \"id\": \"IntermediateEvent_1\",\n      \"name\": \"每小时再评估\",\n      \"type\": \"IntermediateCatchEvent\"\n    },\n    {\n      \"id\": \"EndEvent_1\",\n      \"name\": \"转出ICU/出院\",\n      \"type\": \"EndEvent\"\n    }\n  ],\n  \"SequenceFlows\": [\n    {\"id\": \"Flow_1\", \"sourceRef\": \"StartEvent_1\", \"targetRef\": \"Task_1\"},\n    {\"id\": \"Flow_2\", \"sourceRef\": \"Task_1\", \"targetRef\": \"Task_2\"},\n    {\"id\": \"Flow_3\", \"sourceRef\": \"Task_2\", \"targetRef\": \"ExclusiveGateway_1\"},\n    {\"id\": \"Flow_4\", \"sourceRef\": \"ExclusiveGateway_1\", \"targetRef\": \"Task_3\", \"condition\": \"是\"},\n    {\"id\": \"Flow_5\", \"sourceRef\": \"ExclusiveGateway_1\", \"targetRef\": \"Task_4\", \"condition\": \"否\"},\n    {\"id\": \"Flow_6\", \"sourceRef\": \"Task_3\", \"targetRef\": \"Task_4\"},\n    {\"id\": \"Flow_7\", \"sourceRef\": \"Task_4\", \"targetRef\": \"Task_5\"},\n    {\"id\": \"Flow_8\", \"sourceRef\": \"Task_5\", \"targetRef\": \"IntermediateEvent_1\"},\n    {\"id\": \"Flow_9\", \"sourceRef\": \"IntermediateEvent_1\", \"targetRef\": \"EndEvent_1\", \"condition\": \"稳定\"},\n    {\"id\', CAST(0x0000B29200A76ADD AS DateTime))
INSERT [dbo].[WfLog] ([ID], [EventTypeID], [Priority], [Severity], [Title], [Message], [StackTrace], [InnerStackTrace], [RequestData], [Timestamp]) VALUES (16, 1, 2, N'NORMAL', N'An error occurred when generating process by AI', N'One or more errors occurred. (One or more errors occurred. (A task was canceled.))', N'   at System.Threading.Tasks.Task.ThrowIfExceptional(Boolean includeTaskCanceledExceptions)
   at System.Threading.Tasks.Task`1.GetResultCore(Boolean waitCompletionNotification)
   at Slickflow.AI.Model.BpmnAIService.GenerateProcessByAI(AIRequestEntity request) in D:\Cloud365\GitHomeCore\SfBpmn2\source\lib\Slickflow.AI\Model\BpmnAIService.cs:line 275', N'   at System.Threading.Tasks.Task.ThrowIfExceptional(Boolean includeTaskCanceledExceptions)
   at System.Threading.Tasks.Task`1.GetResultCore(Boolean waitCompletionNotification)
   at Slickflow.AI.Utility.TextSimilarity.GetAnswer(String question) in D:\Cloud365\GitHomeCore\SfBpmn2\source\lib\Slickflow.AI\Utility\EnhancedDeepSeekService.cs:line 174
   at Slickflow.AI.Utility.LocalCacheService.GetAnswer(String question) in D:\Cloud365\GitHomeCore\SfBpmn2\source\lib\Slickflow.AI\Utility\EnhancedDeepSeekService.cs:line 64
   at Slickflow.AI.Utility.EnhancedDeepSeekService.GetAnswerAsync(String question) in D:\Cloud365\GitHomeCore\SfBpmn2\source\lib\Slickflow.AI\Utility\EnhancedDeepSeekService.cs:line 23', N'""', CAST(0x0000B29200B7B3AE AS DateTime))
SET IDENTITY_INSERT [dbo].[WfLog] OFF
/****** Object:  Table [dbo].[WfJobSchedule]    Script Date: 02/28/2025 17:15:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[WfJobSchedule](
	[ID] [int] NOT NULL,
	[ScheduleGUID] [varchar](100) NULL,
	[ScheduleName] [varchar](100) NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
	[ScheduleType] [tinyint] NOT NULL,
	[Status] [smallint] NOT NULL,
	[CronExpression] [varchar](100) NULL,
	[LastUpdatedDateTime] [datetime] NULL,
	[LastUpdatedByUserID] [varchar](50) NULL,
	[LastUpdatedByUserName] [nvarchar](50) NULL,
 CONSTRAINT [PK_WFJOBSCHEDULE] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WfJobSchedule', @level2type=N'COLUMN',@level2name=N'Status'
GO
/****** Object:  Table [dbo].[WfJobLog]    Script Date: 02/28/2025 17:15:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[WfJobLog](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[JobType] [varchar](50) NOT NULL,
	[JobName] [varchar](200) NOT NULL,
	[JobKey] [varchar](50) NULL,
	[RefClass] [varchar](50) NOT NULL,
	[RefIDs] [varchar](4000) NOT NULL,
	[Status] [smallint] NOT NULL,
	[Message] [nvarchar](4000) NULL,
	[StackTrace] [nvarchar](max) NULL,
	[InnerStackTrace] [nvarchar](max) NULL,
	[RequestData] [nvarchar](2000) NULL,
	[CreatedDateTime] [datetime] NOT NULL,
	[CreatedByUserID] [varchar](50) NOT NULL,
	[CreatedByUserName] [varchar](50) NOT NULL,
 CONSTRAINT [PK_WFJOBS] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PROCESS-INSTANCE
   ACTIVITY-INSTANCE' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WfJobLog', @level2type=N'COLUMN',@level2name=N'RefClass'
GO
/****** Object:  Table [dbo].[WfJobInfo]    Script Date: 02/28/2025 17:15:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[WfJobInfo](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ProcessID] [varchar](50) NOT NULL,
	[ProcessName] [nvarchar](50) NOT NULL,
	[Version] [nvarchar](20) NOT NULL,
	[ActivityID] [varchar](50) NOT NULL,
	[ActivityName] [nvarchar](50) NOT NULL,
	[ActivityType] [varchar](20) NOT NULL,
	[TriggerType] [varchar](20) NOT NULL,
	[MessageDirection] [varchar](20) NOT NULL,
	[JobName] [nvarchar](100) NOT NULL,
	[Topic] [nvarchar](100) NOT NULL,
	[JobStatus] [varchar](20) NOT NULL,
	[CreatedDateTime] [datetime] NOT NULL,
	[CreatedUserID] [varchar](50) NOT NULL,
	[CreatedUserName] [nvarchar](50) NOT NULL,
	[LastUpdatedDateTime] [datetime] NULL,
	[LastUpdatedUserID] [varchar](50) NULL,
	[LastUpdatedUserName] [nvarchar](50) NULL,
 CONSTRAINT [PK_WFJOBINFO] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[WfJobInfo] ON
INSERT [dbo].[WfJobInfo] ([ID], [ProcessID], [ProcessName], [Version], [ActivityID], [ActivityName], [ActivityType], [TriggerType], [MessageDirection], [JobName], [Topic], [JobStatus], [CreatedDateTime], [CreatedUserID], [CreatedUserName], [LastUpdatedDateTime], [LastUpdatedUserID], [LastUpdatedUserName]) VALUES (3, N'214ad24b-9097-41de-8e74-a5913c518429', N'SignalInterProcess', N'1', N'c53f07ba-e65c-465b-d81f-ea992249712c', N'Event_0lt2165', N'IntermediateNode', N'Signal', N'Catch', N'IntermediateNode.Signal.Catch.OrderDistributed', N'OrderDistributed', N'Subscribed', CAST(0x0000B2010127D2B0 AS DateTime), N'ADMIN_1001', N'ADMINISTRATOR_JOB', CAST(0x0000B20201564FCA AS DateTime), N'ADMIN_1001', N'ADMINISTRATOR_JOB')
INSERT [dbo].[WfJobInfo] ([ID], [ProcessID], [ProcessName], [Version], [ActivityID], [ActivityName], [ActivityType], [TriggerType], [MessageDirection], [JobName], [Topic], [JobStatus], [CreatedDateTime], [CreatedUserID], [CreatedUserName], [LastUpdatedDateTime], [LastUpdatedUserID], [LastUpdatedUserName]) VALUES (4, N'2a7a60a3-f270-429f-98b6-e3c30c60b6e5', N'SignalStartProcess', N'1', N'9cbf4daf-0c9b-42d2-9773-bbeb1b45227b', N'Start', N'StartNode', N'Signal', N'Catch', N'StartNode.Signal.Catch.OrderDistributed', N'OrderDistributed', N'Subscribed', CAST(0x0000B20201556F66 AS DateTime), N'ADMIN_1001', N'ADMINISTRATOR_JOB', CAST(0x0000B20300CF421D AS DateTime), N'ADMIN_1001', N'ADMINISTRATOR_JOB')
SET IDENTITY_INSERT [dbo].[WfJobInfo] OFF
/****** Object:  StoredProcedure [dbo].[pr_com_QuerySQLPaged]    Script Date: 02/28/2025 17:15:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Stored Procedure

create PROCEDURE  [dbo].[pr_com_QuerySQLPaged]      
     @Query nvarchar(MAX), --SQL语句    
     @PageSize int, --每页大小   
     @CurrentPage  int ,  --当前页码   
     @Field nvarchar(40)='', --排序字段   
     @Order nvarchar(10) = 'asc ' --排序顺序   
AS    
    declare @PageCount int,
	        @TempSize int,    
			@TempNum int,  
			@strSQL varchar(max),
			@strField varchar(40),   
			@strFielddesc varchar(40),
			@Tempindex int 

    --0,1都做第一页处理
	if (@currentPage = 0)
		set @currentPage = 1

    set @TempNum = @CurrentPage * @PageSize    
	set @strField = ''
	set @strFielddesc = ''

	--计算总页数
	declare @strCountSQL nvarchar(MAX)
	set @strCountSQL = 'SELECT @total=COUNT(1) FROM (' + @Query + ')T'

	--总记录数
	DECLARE @rowsCount int
	DECLARE @params nvarchar(500)
	SET @params = '@total int OUTPUT'
	EXEC sp_executesql @strCountSQL, @params, @total=@rowsCount OUTPUT

	--根据总记录数，计算页数
	SET @PageCount = ceiling(convert(float, @rowsCount) / convert(float, @PageSize))

	--超过最后一页，显示尾页
    if(@CurrentPage>=@PageCount)    
        set @TempSize=@rowsCount-(@PageCount-1)*@PageSize    
    else  
        set @TempSize=@PageSize  

	SET @Tempindex=Charindex('projcode',@Query,0)
    if( @Tempindex>0 and @Tempindex<Charindex('from',@Query,0))
	begin
		if(@Field<>'' and @Field<>'projcode')
		begin
			set @strField = ',projcode ';
			set	@strFielddesc =',projcode desc ';
		end 
	end 

	--分页SQL
    if(@Order='desc')    
    begin    
      set @strSQL = '
            select *   
            from (   
                    select top '+CONVERT(varchar(10),@TempSize)+' *   
                    from (  
                            select top '+CONVERT(varchar(10),@TempNum)+' *   
                            from ('+@Query+') as t0   
                            order by '+@Field+' desc '+@strField+'  
                    ) as t1   
                    order by '+@Field+@strFielddesc+' 
            ) as t2   
            order by '+@Field+' desc' +@strField   
    end    
    else    
    begin    
      set @strSQL = '
            select *   
            from (  
                    select top '+CONVERT(varchar(10),@TempSize)+' *   
                    from (  
                            select top '+ CONVERT(varchar(10), @TempNum ) + ' *   
                            from ('+@Query+') as t0  
                            order by '+@Field+' asc '+@strField +'
                    ) as t1   
                    order by '+@Field+' desc  '+@strFielddesc+' 
            ) as t2   
            order by '+@Field +@strField  
    end  
    exec(@strSQL)
GO
/****** Object:  Table [dbo].[ManProductOrder]    Script Date: 02/28/2025 17:15:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ManProductOrder](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[OrderCode] [varchar](30) NULL,
	[Status] [smallint] NULL,
	[ProductName] [nvarchar](100) NULL,
	[Quantity] [int] NULL,
	[UnitPrice] [decimal](18, 2) NULL,
	[TotalPrice] [decimal](18, 2) NULL,
	[CreatedTime] [datetime] NULL,
	[CustomerName] [nvarchar](50) NULL,
	[Address] [nvarchar](100) NULL,
	[Mobile] [varchar](30) NULL,
	[Remark] [nvarchar](1000) NULL,
	[LastUpdatedTime] [datetime] NULL,
 CONSTRAINT [PK_MADPRODUCTORDER] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[ManProductOrder] ON
INSERT [dbo].[ManProductOrder] ([ID], [OrderCode], [Status], [ProductName], [Quantity], [UnitPrice], [TotalPrice], [CreatedTime], [CustomerName], [Address], [Mobile], [Remark], [LastUpdatedTime]) VALUES (675, N'TB324384', 8, N'遥控灯D型', 5, CAST(1000.00 AS Decimal(18, 2)), CAST(5000.00 AS Decimal(18, 2)), CAST(0x0000A72900F8491F AS DateTime), N'BBC', N'英国伦敦', N'739538', N'C店', CAST(0x0000A72901008DCD AS DateTime))
INSERT [dbo].[ManProductOrder] ([ID], [OrderCode], [Status], [ProductName], [Quantity], [UnitPrice], [TotalPrice], [CreatedTime], [CustomerName], [Address], [Mobile], [Remark], [LastUpdatedTime]) VALUES (676, N'TB377329', 3, N'遥控灯D型', 7, CAST(1000.00 AS Decimal(18, 2)), CAST(7000.00 AS Decimal(18, 2)), CAST(0x0000A79000C4C367 AS DateTime), N'阿里巴巴', N'杭州西湖区', N'802382', N'B店', CAST(0x0000A79000CD1AA9 AS DateTime))
INSERT [dbo].[ManProductOrder] ([ID], [OrderCode], [Status], [ProductName], [Quantity], [UnitPrice], [TotalPrice], [CreatedTime], [CustomerName], [Address], [Mobile], [Remark], [LastUpdatedTime]) VALUES (677, N'TB730548', 3, N'智能玩具C型', 6, CAST(1000.00 AS Decimal(18, 2)), CAST(6000.00 AS Decimal(18, 2)), CAST(0x0000A79100A22D8A AS DateTime), N'汇丰银行', N'上海人民广场', N'338600', N'F店', CAST(0x0000A90201173470 AS DateTime))
INSERT [dbo].[ManProductOrder] ([ID], [OrderCode], [Status], [ProductName], [Quantity], [UnitPrice], [TotalPrice], [CreatedTime], [CustomerName], [Address], [Mobile], [Remark], [LastUpdatedTime]) VALUES (678, N'TB574787', 3, N'智能玩具C型', 7, CAST(1000.00 AS Decimal(18, 2)), CAST(7000.00 AS Decimal(18, 2)), CAST(0x0000A7B8009E3C10 AS DateTime), N'汇丰银行', N'上海人民广场', N'553578', N'C店', CAST(0x0000A7B8009E525E AS DateTime))
INSERT [dbo].[ManProductOrder] ([ID], [OrderCode], [Status], [ProductName], [Quantity], [UnitPrice], [TotalPrice], [CreatedTime], [CustomerName], [Address], [Mobile], [Remark], [LastUpdatedTime]) VALUES (679, N'TB100834', 4, N'童话玩具A型', 6, CAST(1000.00 AS Decimal(18, 2)), CAST(6000.00 AS Decimal(18, 2)), CAST(0x0000A7D8013AFD08 AS DateTime), N'HACK 新闻', N'美国纽约', N'974724', N'A店', CAST(0x0000A7D8013B21C8 AS DateTime))
INSERT [dbo].[ManProductOrder] ([ID], [OrderCode], [Status], [ProductName], [Quantity], [UnitPrice], [TotalPrice], [CreatedTime], [CustomerName], [Address], [Mobile], [Remark], [LastUpdatedTime]) VALUES (680, N'TB752624', 8, N'海盗船F型', 4, CAST(1000.00 AS Decimal(18, 2)), CAST(4000.00 AS Decimal(18, 2)), CAST(0x0000A83F00B6AFBD AS DateTime), N'花旗银行', N'上海浦东新区', N'100628', N'F店', CAST(0x0000A83F00B7513E AS DateTime))
INSERT [dbo].[ManProductOrder] ([ID], [OrderCode], [Status], [ProductName], [Quantity], [UnitPrice], [TotalPrice], [CreatedTime], [CustomerName], [Address], [Mobile], [Remark], [LastUpdatedTime]) VALUES (681, N'TB517477', 3, N'童话玩具A型', 4, CAST(1000.00 AS Decimal(18, 2)), CAST(4000.00 AS Decimal(18, 2)), CAST(0x0000A83F00E5C20C AS DateTime), N'中石油', N'北京燕山', N'120409', N'C店', CAST(0x0000A842010B62E7 AS DateTime))
INSERT [dbo].[ManProductOrder] ([ID], [OrderCode], [Status], [ProductName], [Quantity], [UnitPrice], [TotalPrice], [CreatedTime], [CustomerName], [Address], [Mobile], [Remark], [LastUpdatedTime]) VALUES (682, N'TB601588', 4, N'遥控灯D型', 4, CAST(1000.00 AS Decimal(18, 2)), CAST(4000.00 AS Decimal(18, 2)), CAST(0x0000A842010B8971 AS DateTime), N'花旗银行', N'上海浦东新区', N'428885', N'A店', CAST(0x0000A842010BA376 AS DateTime))
INSERT [dbo].[ManProductOrder] ([ID], [OrderCode], [Status], [ProductName], [Quantity], [UnitPrice], [TotalPrice], [CreatedTime], [CustomerName], [Address], [Mobile], [Remark], [LastUpdatedTime]) VALUES (683, N'TB393078', 3, N'LED节能灯E型', 1, CAST(1000.00 AS Decimal(18, 2)), CAST(1000.00 AS Decimal(18, 2)), CAST(0x0000A97E00B17993 AS DateTime), N'阿里巴巴', N'杭州西湖区', N'500282', N'B店', CAST(0x0000AE6000AC5ED1 AS DateTime))
INSERT [dbo].[ManProductOrder] ([ID], [OrderCode], [Status], [ProductName], [Quantity], [UnitPrice], [TotalPrice], [CreatedTime], [CustomerName], [Address], [Mobile], [Remark], [LastUpdatedTime]) VALUES (684, N'TB937073', 3, N'智能玩具C型', 1, CAST(1000.00 AS Decimal(18, 2)), CAST(1000.00 AS Decimal(18, 2)), CAST(0x0000AA0801600730 AS DateTime), N'中石油', N'北京燕山', N'376673', N'F店', CAST(0x0000AA0801604495 AS DateTime))
INSERT [dbo].[ManProductOrder] ([ID], [OrderCode], [Status], [ProductName], [Quantity], [UnitPrice], [TotalPrice], [CreatedTime], [CustomerName], [Address], [Mobile], [Remark], [LastUpdatedTime]) VALUES (685, N'TB359987', 3, N'海盗船F型', 9, CAST(1000.00 AS Decimal(18, 2)), CAST(9000.00 AS Decimal(18, 2)), CAST(0x0000AAF600EF411F AS DateTime), N'中国邮政', N'北京复兴门', N'568964', N'F店', CAST(0x0000AAF600EF4E8F AS DateTime))
INSERT [dbo].[ManProductOrder] ([ID], [OrderCode], [Status], [ProductName], [Quantity], [UnitPrice], [TotalPrice], [CreatedTime], [CustomerName], [Address], [Mobile], [Remark], [LastUpdatedTime]) VALUES (686, N'TB588656', 8, N'智能玩具C型', 3, CAST(1000.00 AS Decimal(18, 2)), CAST(3000.00 AS Decimal(18, 2)), CAST(0x0000ABBC00ACBC53 AS DateTime), N'花旗银行', N'上海浦东新区', N'666540', N'B店', CAST(0x0000ABBC00B3A68E AS DateTime))
INSERT [dbo].[ManProductOrder] ([ID], [OrderCode], [Status], [ProductName], [Quantity], [UnitPrice], [TotalPrice], [CreatedTime], [CustomerName], [Address], [Mobile], [Remark], [LastUpdatedTime]) VALUES (687, N'TB720748', 8, N'遥控飞机B型', 4, CAST(1000.00 AS Decimal(18, 2)), CAST(4000.00 AS Decimal(18, 2)), CAST(0x0000AC2300F206A3 AS DateTime), N'花旗银行', N'上海浦东新区', N'140223', N'C店', CAST(0x0000AC2300FC0757 AS DateTime))
INSERT [dbo].[ManProductOrder] ([ID], [OrderCode], [Status], [ProductName], [Quantity], [UnitPrice], [TotalPrice], [CreatedTime], [CustomerName], [Address], [Mobile], [Remark], [LastUpdatedTime]) VALUES (688, N'TB332639', 3, N'童话玩具A型', 1, CAST(1000.00 AS Decimal(18, 2)), CAST(1000.00 AS Decimal(18, 2)), CAST(0x0000AC7E00CF9C4A AS DateTime), N'阿里巴巴', N'杭州西湖区', N'175105', N'B店', CAST(0x0000ACB000A03E28 AS DateTime))
INSERT [dbo].[ManProductOrder] ([ID], [OrderCode], [Status], [ProductName], [Quantity], [UnitPrice], [TotalPrice], [CreatedTime], [CustomerName], [Address], [Mobile], [Remark], [LastUpdatedTime]) VALUES (689, N'TB741954', 8, N'童话玩具A型', 4, CAST(1000.00 AS Decimal(18, 2)), CAST(4000.00 AS Decimal(18, 2)), CAST(0x0000ACB000A1DABF AS DateTime), N'中石油', N'北京燕山', N'164151', N'B店', CAST(0x0000ACB000A24580 AS DateTime))
INSERT [dbo].[ManProductOrder] ([ID], [OrderCode], [Status], [ProductName], [Quantity], [UnitPrice], [TotalPrice], [CreatedTime], [CustomerName], [Address], [Mobile], [Remark], [LastUpdatedTime]) VALUES (690, N'TB332806', 3, N'童话玩具A型', 2, CAST(1000.00 AS Decimal(18, 2)), CAST(2000.00 AS Decimal(18, 2)), CAST(0x0000AD8F01004388 AS DateTime), N'青田麦家', N'福建岭南', N'909976', N'C店', CAST(0x0000AD8F0100596E AS DateTime))
INSERT [dbo].[ManProductOrder] ([ID], [OrderCode], [Status], [ProductName], [Quantity], [UnitPrice], [TotalPrice], [CreatedTime], [CustomerName], [Address], [Mobile], [Remark], [LastUpdatedTime]) VALUES (691, N'TB452818', 3, N'遥控灯D型', 2, CAST(1000.00 AS Decimal(18, 2)), CAST(2000.00 AS Decimal(18, 2)), CAST(0x0000AE6000AC6F84 AS DateTime), N'中石油', N'北京燕山', N'534659', N'F店', CAST(0x0000AE6000AC8A4F AS DateTime))
INSERT [dbo].[ManProductOrder] ([ID], [OrderCode], [Status], [ProductName], [Quantity], [UnitPrice], [TotalPrice], [CreatedTime], [CustomerName], [Address], [Mobile], [Remark], [LastUpdatedTime]) VALUES (692, N'TB263682', 8, N'海盗船F型', 7, CAST(1000.00 AS Decimal(18, 2)), CAST(7000.00 AS Decimal(18, 2)), CAST(0x0000AE60012DC62E AS DateTime), N'汇丰银行', N'上海人民广场', N'636622', N'A店', CAST(0x0000AECD008F64EB AS DateTime))
INSERT [dbo].[ManProductOrder] ([ID], [OrderCode], [Status], [ProductName], [Quantity], [UnitPrice], [TotalPrice], [CreatedTime], [CustomerName], [Address], [Mobile], [Remark], [LastUpdatedTime]) VALUES (693, N'TB307954', 8, N'童话玩具A型', 1, CAST(1000.00 AS Decimal(18, 2)), CAST(1000.00 AS Decimal(18, 2)), CAST(0x0000AE8200F0C6D4 AS DateTime), N'阿里巴巴', N'杭州西湖区', N'248689', N'B店', CAST(0x0000AE8200F14F39 AS DateTime))
INSERT [dbo].[ManProductOrder] ([ID], [OrderCode], [Status], [ProductName], [Quantity], [UnitPrice], [TotalPrice], [CreatedTime], [CustomerName], [Address], [Mobile], [Remark], [LastUpdatedTime]) VALUES (694, N'TB293064', 3, N'智能玩具C型', 6, CAST(1000.00 AS Decimal(18, 2)), CAST(6000.00 AS Decimal(18, 2)), CAST(0x0000AECE0149D716 AS DateTime), N'阿里巴巴', N'杭州西湖区', N'960191', N'J店', CAST(0x0000AECE014A0F89 AS DateTime))
INSERT [dbo].[ManProductOrder] ([ID], [OrderCode], [Status], [ProductName], [Quantity], [UnitPrice], [TotalPrice], [CreatedTime], [CustomerName], [Address], [Mobile], [Remark], [LastUpdatedTime]) VALUES (695, N'TB226532', 3, N'遥控飞机B型', 6, CAST(1000.00 AS Decimal(18, 2)), CAST(6000.00 AS Decimal(18, 2)), CAST(0x0000B10C01086012 AS DateTime), N'青田麦家', N'福建岭南', N'675404', N'A店', CAST(0x0000B10C01087690 AS DateTime))
INSERT [dbo].[ManProductOrder] ([ID], [OrderCode], [Status], [ProductName], [Quantity], [UnitPrice], [TotalPrice], [CreatedTime], [CustomerName], [Address], [Mobile], [Remark], [LastUpdatedTime]) VALUES (696, N'TB207310', 8, N'智能玩具C型', 1, CAST(1000.00 AS Decimal(18, 2)), CAST(1000.00 AS Decimal(18, 2)), CAST(0x0000B1960098B665 AS DateTime), N'中石油', N'北京燕山', N'558283', N'F店', CAST(0x0000B19600AB9549 AS DateTime))
INSERT [dbo].[ManProductOrder] ([ID], [OrderCode], [Status], [ProductName], [Quantity], [UnitPrice], [TotalPrice], [CreatedTime], [CustomerName], [Address], [Mobile], [Remark], [LastUpdatedTime]) VALUES (697, N'TB257249', 4, N'童话玩具A型', 1, CAST(1000.00 AS Decimal(18, 2)), CAST(1000.00 AS Decimal(18, 2)), CAST(0x0000B19600B55EDB AS DateTime), N'汇丰银行', N'上海人民广场', N'678931', N'A店', CAST(0x0000B19600B597A1 AS DateTime))
INSERT [dbo].[ManProductOrder] ([ID], [OrderCode], [Status], [ProductName], [Quantity], [UnitPrice], [TotalPrice], [CreatedTime], [CustomerName], [Address], [Mobile], [Remark], [LastUpdatedTime]) VALUES (698, N'TB409086', 4, N'遥控飞机B型', 3, CAST(1000.00 AS Decimal(18, 2)), CAST(3000.00 AS Decimal(18, 2)), CAST(0x0000B19600E2DD28 AS DateTime), N'BBC', N'英国伦敦', N'653688', N'J店', CAST(0x0000B19600E31CD0 AS DateTime))
SET IDENTITY_INSERT [dbo].[ManProductOrder] OFF
/****** Object:  Table [dbo].[HrsLeaveOpinion]    Script Date: 02/28/2025 17:15:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[HrsLeaveOpinion](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[AppInstanceID] [varchar](50) NOT NULL,
	[ActivityID] [varchar](50) NULL,
	[ActivityName] [nvarchar](50) NOT NULL,
	[Remark] [nvarchar](1000) NULL,
	[ChangedTime] [datetime] NOT NULL,
	[ChangedUserID] [int] NOT NULL,
	[ChangedUserName] [nvarchar](50) NULL,
 CONSTRAINT [PK_HRSLEAVEOPINION] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[HrsLeaveOpinion] ON
INSERT [dbo].[HrsLeaveOpinion] ([ID], [AppInstanceID], [ActivityID], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (1, N'34', N'00000000-0000-0000-0000-000000000000', N'流程发起', N'申请人:6-路天明', CAST(0x0000A7BC013216A4 AS DateTime), 6, N'路天明')
INSERT [dbo].[HrsLeaveOpinion] ([ID], [AppInstanceID], [ActivityID], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (2, N'34', N'c437c27a-8351-4805-fd4f-4e270084320a', N'部门经理审批', N'张恒丰(ID:5) agree', CAST(0x0000A7BC01326448 AS DateTime), 5, N'张恒丰')
INSERT [dbo].[HrsLeaveOpinion] ([ID], [AppInstanceID], [ActivityID], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (3, N'35', N'00000000-0000-0000-0000-000000000000', N'流程发起', N'申请人:6-路天明', CAST(0x0000A7D8013B4E1C AS DateTime), 6, N'路天明')
INSERT [dbo].[HrsLeaveOpinion] ([ID], [AppInstanceID], [ActivityID], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (4, N'35', N'c437c27a-8351-4805-fd4f-4e270084320a', N'部门经理审批', N'张恒丰(ID:5) tongyi', CAST(0x0000A7D8013B7631 AS DateTime), 5, N'张恒丰')
INSERT [dbo].[HrsLeaveOpinion] ([ID], [AppInstanceID], [ActivityID], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (5, N'36', N'00000000-0000-0000-0000-000000000000', N'流程发起', N'申请人:6-路天明', CAST(0x0000A7EE00B0927D AS DateTime), 6, N'路天明')
INSERT [dbo].[HrsLeaveOpinion] ([ID], [AppInstanceID], [ActivityID], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (6, N'37', N'00000000-0000-0000-0000-000000000000', N'流程发起', N'申请人:6-路天明', CAST(0x0000A83F00E74309 AS DateTime), 6, N'路天明')
INSERT [dbo].[HrsLeaveOpinion] ([ID], [AppInstanceID], [ActivityID], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (7, N'37', N'c437c27a-8351-4805-fd4f-4e270084320a', N'部门经理审批', N'张恒丰(ID:5) 同意', CAST(0x0000A83F00E772A8 AS DateTime), 5, N'张恒丰')
INSERT [dbo].[HrsLeaveOpinion] ([ID], [AppInstanceID], [ActivityID], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (8, N'37', N'da9f744b-3f97-40c9-c4f8-67d5a60a2485', N'人事经理审批', N'李颖(ID:4) ', CAST(0x0000A83F00E7C07C AS DateTime), 4, N'李颖')
INSERT [dbo].[HrsLeaveOpinion] ([ID], [AppInstanceID], [ActivityID], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (9, N'38', N'00000000-0000-0000-0000-000000000000', N'流程发起', N'申请人:6-路天明', CAST(0x0000A842010CEE96 AS DateTime), 6, N'路天明')
INSERT [dbo].[HrsLeaveOpinion] ([ID], [AppInstanceID], [ActivityID], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (10, N'39', N'00000000-0000-0000-0000-000000000000', N'流程发起', N'申请人:6-LuTianMing', CAST(0x0000AC2300FEBD4A AS DateTime), 6, N'LuTianMing')
INSERT [dbo].[HrsLeaveOpinion] ([ID], [AppInstanceID], [ActivityID], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (11, N'39', N'c437c27a-8351-4805-fd4f-4e270084320a', N'部门经理审批', N'ZhangFeng(ID:5) 同意', CAST(0x0000AC2300FF6C20 AS DateTime), 5, N'ZhangFeng')
INSERT [dbo].[HrsLeaveOpinion] ([ID], [AppInstanceID], [ActivityID], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (12, N'39', N'da9f744b-3f97-40c9-c4f8-67d5a60a2485', N'人事经理审批', N'LiYin(ID:4) ', CAST(0x0000AC2300FF8A21 AS DateTime), 4, N'LiYin')
SET IDENTITY_INSERT [dbo].[HrsLeaveOpinion] OFF
/****** Object:  Table [dbo].[HrsLeave]    Script Date: 02/28/2025 17:15:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HrsLeave](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[LeaveType] [nvarchar](50) NOT NULL,
	[Days] [decimal](18, 1) NOT NULL,
	[FromDate] [date] NOT NULL,
	[ToDate] [date] NOT NULL,
	[CurrentActivityText] [nvarchar](50) NULL,
	[Status] [int] NULL,
	[CreatedUserID] [nvarchar](50) NOT NULL,
	[CreatedUserName] [nvarchar](50) NOT NULL,
	[CreatedDateTime] [date] NOT NULL,
	[Remark] [nvarchar](1000) NULL,
	[Opinions] [nvarchar](2000) NULL,
 CONSTRAINT [PK_HRLEAVE] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'HrsLeave', @level2type=N'COLUMN',@level2name=N'Days'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'HrsLeave', @level2type=N'COLUMN',@level2name=N'FromDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'HrsLeave', @level2type=N'COLUMN',@level2name=N'ToDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'HrsLeave', @level2type=N'COLUMN',@level2name=N'CurrentActivityText'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'HrsLeave', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'HrsLeave', @level2type=N'COLUMN',@level2name=N'CreatedUserID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'HrsLeave', @level2type=N'COLUMN',@level2name=N'CreatedUserName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'HrsLeave', @level2type=N'COLUMN',@level2name=N'CreatedDateTime'
GO
SET IDENTITY_INSERT [dbo].[HrsLeave] ON
INSERT [dbo].[HrsLeave] ([ID], [LeaveType], [Days], [FromDate], [ToDate], [CurrentActivityText], [Status], [CreatedUserID], [CreatedUserName], [CreatedDateTime], [Remark], [Opinions]) VALUES (80, N'事假', CAST(4.0 AS Decimal(18, 1)), CAST(0x5B420B00 AS Date), CAST(0x5F420B00 AS Date), NULL, 0, N'6', N'Lucy', CAST(0x64420B00 AS Date), N'dsf', N'safewfewasf')
INSERT [dbo].[HrsLeave] ([ID], [LeaveType], [Days], [FromDate], [ToDate], [CurrentActivityText], [Status], [CreatedUserID], [CreatedUserName], [CreatedDateTime], [Remark], [Opinions]) VALUES (81, N'事假', CAST(4.0 AS Decimal(18, 1)), CAST(0x5A420B00 AS Date), CAST(0x5E420B00 AS Date), NULL, 0, N'6', N'Lucy', CAST(0x64420B00 AS Date), N'wfe', N'eqwrewqrfsdaegfeasfasfds')
INSERT [dbo].[HrsLeave] ([ID], [LeaveType], [Days], [FromDate], [ToDate], [CurrentActivityText], [Status], [CreatedUserID], [CreatedUserName], [CreatedDateTime], [Remark], [Opinions]) VALUES (82, N'事假', CAST(6.0 AS Decimal(18, 1)), CAST(0x5A420B00 AS Date), CAST(0x60420B00 AS Date), NULL, 0, N'6', N'Lucy', CAST(0x64420B00 AS Date), N'asdf', N'wrdsadfsftrhgfr')
INSERT [dbo].[HrsLeave] ([ID], [LeaveType], [Days], [FromDate], [ToDate], [CurrentActivityText], [Status], [CreatedUserID], [CreatedUserName], [CreatedDateTime], [Remark], [Opinions]) VALUES (83, N'事假', CAST(3.0 AS Decimal(18, 1)), CAST(0x5B420B00 AS Date), CAST(0x5E420B00 AS Date), NULL, 0, N'6', N'Lucy', CAST(0x74420B00 AS Date), N'u', N'ydfyi')
INSERT [dbo].[HrsLeave] ([ID], [LeaveType], [Days], [FromDate], [ToDate], [CurrentActivityText], [Status], [CreatedUserID], [CreatedUserName], [CreatedDateTime], [Remark], [Opinions]) VALUES (84, N'Personal', CAST(2.0 AS Decimal(18, 1)), CAST(0x26440B00 AS Date), CAST(0x28440B00 AS Date), NULL, 0, N'6', N'Lucy', CAST(0x27440B00 AS Date), N'test', N'')
SET IDENTITY_INSERT [dbo].[HrsLeave] OFF
/****** Object:  UserDefinedFunction [dbo].[fn_com_SplitString]    Script Date: 02/28/2025 17:15:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create FUNCTION [dbo].[fn_com_SplitString] ( @stringToSplit nvarchar(4000) )
RETURNS
 @returnList TABLE ([ID] int)
AS
BEGIN

 DECLARE @name NVARCHAR(255)
 DECLARE @pos INT

 WHILE CHARINDEX(',', @stringToSplit) > 0
 BEGIN
  SELECT @pos  = CHARINDEX(',', @stringToSplit)  
  SELECT @name = SUBSTRING(@stringToSplit, 1, @pos-1)
  

  INSERT INTO @returnList 
  SELECT CONVERT(INT,  @name)

  SELECT @stringToSplit = SUBSTRING(@stringToSplit, @pos+1, LEN(@stringToSplit)-@pos)
 END

 INSERT INTO @returnList
 SELECT @stringToSplit

 RETURN
END
GO
/****** Object:  Table [dbo].[FbFormProcess]    Script Date: 02/28/2025 17:15:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FbFormProcess](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FormID] [int] NOT NULL,
	[Version] [varchar](20) NOT NULL,
	[ProcessID] [int] NOT NULL,
	[ProcessGUID] [varchar](100) NOT NULL,
	[ProcessVersion] [varchar](20) NOT NULL,
	[ProcessName] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_FbFormProcess] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[FbFormProcess] ON
INSERT [dbo].[FbFormProcess] ([ID], [FormID], [Version], [ProcessID], [ProcessGUID], [ProcessVersion], [ProcessName]) VALUES (14, 24, N'1', 3, N'072af8c3-482a-4b1c-890b-685ce2fcc75d', N'1', N'PriceProcess(SequenceTest)')
INSERT [dbo].[FbFormProcess] ([ID], [FormID], [Version], [ProcessID], [ProcessGUID], [ProcessVersion], [ProcessName]) VALUES (15, 32, N'1', 3, N'072af8c3-482a-4b1c-890b-685ce2fcc75d', N'1', N'PriceProcess(SequenceTest)')
INSERT [dbo].[FbFormProcess] ([ID], [FormID], [Version], [ProcessID], [ProcessGUID], [ProcessVersion], [ProcessName]) VALUES (16, 33, N'1', 3, N'072af8c3-482a-4b1c-890b-685ce2fcc75d', N'1', N'PriceProcess(SequenceTest)')
INSERT [dbo].[FbFormProcess] ([ID], [FormID], [Version], [ProcessID], [ProcessGUID], [ProcessVersion], [ProcessName]) VALUES (17, 34, N'1', 3, N'072af8c3-482a-4b1c-890b-685ce2fcc75d', N'1', N'PriceProcess(SequenceTest)')
INSERT [dbo].[FbFormProcess] ([ID], [FormID], [Version], [ProcessID], [ProcessGUID], [ProcessVersion], [ProcessName]) VALUES (18, 35, N'1', 3, N'072af8c3-482a-4b1c-890b-685ce2fcc75d', N'1', N'PriceProcess(SequenceTest)')
INSERT [dbo].[FbFormProcess] ([ID], [FormID], [Version], [ProcessID], [ProcessGUID], [ProcessVersion], [ProcessName]) VALUES (19, 36, N'2', 3, N'072af8c3-482a-4b1c-890b-685ce2fcc75d', N'1', N'PriceProcess(SequenceTest)')
INSERT [dbo].[FbFormProcess] ([ID], [FormID], [Version], [ProcessID], [ProcessGUID], [ProcessVersion], [ProcessName]) VALUES (20, 39, N'2', 3, N'072af8c3-482a-4b1c-890b-685ce2fcc75d', N'1', N'PriceProcess(SequenceTest)')
INSERT [dbo].[FbFormProcess] ([ID], [FormID], [Version], [ProcessID], [ProcessGUID], [ProcessVersion], [ProcessName]) VALUES (21, 52, N'1', 3, N'072af8c3-482a-4b1c-890b-685ce2fcc75d', N'1', N'PriceProcess(SequenceTest)')
INSERT [dbo].[FbFormProcess] ([ID], [FormID], [Version], [ProcessID], [ProcessGUID], [ProcessVersion], [ProcessName]) VALUES (23, 56, N'1', 24, N'2acffb20-6bd1-4891-98c9-c76d022d1445', N'1', N'请假流程(WebDemo)')
INSERT [dbo].[FbFormProcess] ([ID], [FormID], [Version], [ProcessID], [ProcessGUID], [ProcessVersion], [ProcessName]) VALUES (24, 57, N'1', 3, N'072af8c3-482a-4b1c-890b-685ce2fcc75d', N'1', N'PriceProcess(SequenceTest)')
INSERT [dbo].[FbFormProcess] ([ID], [FormID], [Version], [ProcessID], [ProcessGUID], [ProcessVersion], [ProcessName]) VALUES (25, 60, N'1', 24, N'2acffb20-6bd1-4891-98c9-c76d022d1445', N'1', N'请假流程(WebDemo)')
INSERT [dbo].[FbFormProcess] ([ID], [FormID], [Version], [ProcessID], [ProcessGUID], [ProcessVersion], [ProcessName]) VALUES (26, 61, N'1', 24, N'2acffb20-6bd1-4891-98c9-c76d022d1445', N'1', N'请假流程(WebDemo)')
INSERT [dbo].[FbFormProcess] ([ID], [FormID], [Version], [ProcessID], [ProcessGUID], [ProcessVersion], [ProcessName]) VALUES (27, 62, N'1', 104, N'b2a18777-43f1-4d4d-b9d5-f92aa655a93f', N'1', N'Ask for leave')
INSERT [dbo].[FbFormProcess] ([ID], [FormID], [Version], [ProcessID], [ProcessGUID], [ProcessVersion], [ProcessName]) VALUES (32, 63, N'1', 857, N'75BF39C8-5F4B-441F-9A08-39E0B46A9903', N'1', N'AskForLeave(WebDemo)')
INSERT [dbo].[FbFormProcess] ([ID], [FormID], [Version], [ProcessID], [ProcessGUID], [ProcessVersion], [ProcessName]) VALUES (35, 65, N'1', 1483, N'fdad2829-7449-4840-b6f7-1104b19972d5', N'1', N'Process_Name_3023')
SET IDENTITY_INSERT [dbo].[FbFormProcess] OFF
/****** Object:  Table [dbo].[FbFormFieldEvent]    Script Date: 02/28/2025 17:15:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FbFormFieldEvent](
	[ID] [int] NOT NULL,
	[FormID] [int] NOT NULL,
	[FieldID] [int] NOT NULL,
	[EventName] [nvarchar](100) NOT NULL,
	[EventArguments] [nvarchar](100) NULL,
	[IsDisabled] [bit] NOT NULL,
	[CommandText] [nvarchar](4000) NULL
) ON [PRIMARY]
GO
INSERT [dbo].[FbFormFieldEvent] ([ID], [FormID], [FieldID], [EventName], [EventArguments], [IsDisabled], [CommandText]) VALUES (12, 0, 0, N'onchange', NULL, 0, N'alert(''beijing'')')
/****** Object:  Table [dbo].[FbFormFieldActivityEdit]    Script Date: 02/28/2025 17:15:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FbFormFieldActivityEdit](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ProcessDefID] [int] NOT NULL,
	[ProcessID] [varchar](50) NOT NULL,
	[ProcessName] [nvarchar](50) NULL,
	[ProcessVersion] [nvarchar](20) NOT NULL,
	[ActivityGUID] [varchar](100) NOT NULL,
	[ActivityName] [varchar](100) NOT NULL,
	[FormID] [int] NOT NULL,
	[FormName] [nvarchar](50) NOT NULL,
	[FormVersion] [nvarchar](20) NOT NULL,
	[FieldsPermission] [nvarchar](max) NULL,
 CONSTRAINT [PK_FbFormFieldActivityEdit] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[FbFormFieldActivityEdit] ON
INSERT [dbo].[FbFormFieldActivityEdit] ([ID], [ProcessDefID], [ProcessID], [ProcessName], [ProcessVersion], [ActivityGUID], [ActivityName], [FormID], [FormName], [FormVersion], [FieldsPermission]) VALUES (6, 1493, N'b39c385c-15b1-4972-9b17-2fa9148e5fa9', N'Process_Name_9505', N'1', N'6f7fac31-4c23-429a-f4e3-65bf45ae086d', N'Task2', 65, N'
            Form_1u453x3Form_Test_Sample', N'1', N'[{"FieldName":"leavetype","IsNotVisible":false,"IsReadOnly":true},{"FieldName":"days","IsNotVisible":true,"IsReadOnly":false},{"FieldName":"remark","IsNotVisible":false,"IsReadOnly":false}]')
SET IDENTITY_INSERT [dbo].[FbFormFieldActivityEdit] OFF
/****** Object:  Table [dbo].[FbFormField]    Script Date: 02/28/2025 17:15:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FbFormField](
	[ID] [int] NOT NULL,
	[FormID] [int] NOT NULL,
	[Version] [varchar](20) NOT NULL,
	[FieldType] [varchar](50) NOT NULL,
	[FieldName] [nvarchar](100) NOT NULL,
	[FieldCode] [varchar](50) NOT NULL,
	[FieldGUID] [varchar](100) NOT NULL,
	[Description] [nvarchar](1000) NULL,
	[ControlStyle] [nvarchar](2000) NULL,
	[IsMandatory] [bit] NULL,
	[FieldDataType] [smallint] NOT NULL,
	[ConditionKey] [varchar](50) NULL,
	[VariableName] [varchar](50) NULL,
	[RefFormID] [int] NULL,
	[DataSourceType] [varchar](50) NULL,
	[DataEntityOptions] [nvarchar](2000) NULL,
	[DataEntityName] [nvarchar](200) NULL,
	[DataValueField] [nvarchar](100) NULL,
	[DataTextField] [nvarchar](100) NULL,
	[CascadeControlCode] [nvarchar](50) NULL,
	[CascadeFieldName] [nvarchar](50) NULL,
	[Format] [nvarchar](100) NULL,
	[Url] [nvarchar](100) NULL,
	[OrderID] [int] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[FbFormField] ([ID], [FormID], [Version], [FieldType], [FieldName], [FieldCode], [FieldGUID], [Description], [ControlStyle], [IsMandatory], [FieldDataType], [ConditionKey], [VariableName], [RefFormID], [DataSourceType], [DataEntityOptions], [DataEntityName], [DataValueField], [DataTextField], [CascadeControlCode], [CascadeFieldName], [Format], [Url], [OrderID]) VALUES (4, 0, N'1', N'', N'dsafdsaf', N'dsafdsafA22', N'6f6b2350-a25e-11eb-ad9c-81dfb5004d62', NULL, NULL, NULL, 0, N'', N'', 0, N'0', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0)
/****** Object:  Table [dbo].[FbFormData]    Script Date: 02/28/2025 17:15:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FbFormData](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FormID] [int] NOT NULL,
	[FormDataContent] [nvarchar](max) NOT NULL,
	[CreatedUserID] [varchar](100) NULL,
	[CreatedUserName] [varchar](100) NULL,
	[CreatedDate] [datetime] NULL,
	[LastUpdatedUserID] [varchar](100) NULL,
	[LastUpdatedUserName] [varchar](100) NULL,
	[LastUpdatedDate] [datetime] NULL,
	[RowVersionID] [timestamp] NOT NULL,
 CONSTRAINT [PK_FbFormData] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[FbFormData] ON
INSERT [dbo].[FbFormData] ([ID], [FormID], [FormDataContent], [CreatedUserID], [CreatedUserName], [CreatedDate], [LastUpdatedUserID], [LastUpdatedUserName], [LastUpdatedDate]) VALUES (31, 63, N'{"Name":"jack","textfield_4r7o8k":"","textarea_ap2ur8":"","days":67,"checkbox_3ja15k":false,"select_910eas":null}', NULL, NULL, CAST(0x0000B1E700A5FCF2 AS DateTime), NULL, NULL, CAST(0x0000B1EA01005DE0 AS DateTime))
INSERT [dbo].[FbFormData] ([ID], [FormID], [FormDataContent], [CreatedUserID], [CreatedUserName], [CreatedDate], [LastUpdatedUserID], [LastUpdatedUserName], [LastUpdatedDate]) VALUES (32, 63, N'{"Name":"dfa","textfield_4r7o8k":"","textarea_ap2ur8":"","number_p7lawp":999,"checkbox_3ja15k":false}', NULL, NULL, CAST(0x0000B1E701135C49 AS DateTime), NULL, NULL, CAST(0x0000B1E701135C4A AS DateTime))
INSERT [dbo].[FbFormData] ([ID], [FormID], [FormDataContent], [CreatedUserID], [CreatedUserName], [CreatedDate], [LastUpdatedUserID], [LastUpdatedUserName], [LastUpdatedDate]) VALUES (35, 63, N'{"Name":"","textfield_4r7o8k":"","textarea_ap2ur8":"","days":null,"checkbox_3ja15k":false,"select_910eas":null}', NULL, NULL, CAST(0x0000B1EA00A89E6C AS DateTime), NULL, NULL, CAST(0x0000B1EA00A89E6C AS DateTime))
INSERT [dbo].[FbFormData] ([ID], [FormID], [FormDataContent], [CreatedUserID], [CreatedUserName], [CreatedDate], [LastUpdatedUserID], [LastUpdatedUserName], [LastUpdatedDate]) VALUES (36, 63, N'{"Name":"","textfield_4r7o8k":"","textarea_ap2ur8":"","days":null,"checkbox_3ja15k":false,"select_910eas":null}', NULL, NULL, CAST(0x0000B1EA00A9E7BB AS DateTime), NULL, NULL, CAST(0x0000B1EA00A9E7BB AS DateTime))
INSERT [dbo].[FbFormData] ([ID], [FormID], [FormDataContent], [CreatedUserID], [CreatedUserName], [CreatedDate], [LastUpdatedUserID], [LastUpdatedUserName], [LastUpdatedDate]) VALUES (37, 63, N'{"Name":"","textfield_4r7o8k":"","textarea_ap2ur8":"","days":null,"checkbox_3ja15k":false,"select_910eas":null}', NULL, NULL, CAST(0x0000B1EA00FC261C AS DateTime), NULL, NULL, CAST(0x0000B1EA00FC261C AS DateTime))
INSERT [dbo].[FbFormData] ([ID], [FormID], [FormDataContent], [CreatedUserID], [CreatedUserName], [CreatedDate], [LastUpdatedUserID], [LastUpdatedUserName], [LastUpdatedDate]) VALUES (38, 63, N'{"Name":"","textfield_4r7o8k":"","textarea_ap2ur8":"","days":null,"checkbox_3ja15k":false,"select_910eas":null}', NULL, NULL, CAST(0x0000B1EA00FC4D51 AS DateTime), NULL, NULL, CAST(0x0000B1EA00FC4D51 AS DateTime))
INSERT [dbo].[FbFormData] ([ID], [FormID], [FormDataContent], [CreatedUserID], [CreatedUserName], [CreatedDate], [LastUpdatedUserID], [LastUpdatedUserName], [LastUpdatedDate]) VALUES (39, 63, N'{"Name":"jack","textfield_4r7o8k":"","textarea_ap2ur8":"","days":898,"checkbox_3ja15k":false,"select_910eas":null}', NULL, NULL, CAST(0x0000B1EA00FD7009 AS DateTime), NULL, NULL, CAST(0x0000B1EA01092480 AS DateTime))
INSERT [dbo].[FbFormData] ([ID], [FormID], [FormDataContent], [CreatedUserID], [CreatedUserName], [CreatedDate], [LastUpdatedUserID], [LastUpdatedUserName], [LastUpdatedDate]) VALUES (40, 63, N'{"Name":"jack","textfield_4r7o8k":"","textarea_ap2ur8":"","days":898,"checkbox_3ja15k":false,"select_910eas":null}', NULL, NULL, CAST(0x0000B1EA00FDBDEA AS DateTime), NULL, NULL, CAST(0x0000B1EA01095221 AS DateTime))
INSERT [dbo].[FbFormData] ([ID], [FormID], [FormDataContent], [CreatedUserID], [CreatedUserName], [CreatedDate], [LastUpdatedUserID], [LastUpdatedUserName], [LastUpdatedDate]) VALUES (41, 63, N'{"Name":"jack","textfield_4r7o8k":"","textarea_ap2ur8":"","days":2,"checkbox_3ja15k":false,"select_910eas":null}', NULL, NULL, CAST(0x0000B1EA00FEF80A AS DateTime), NULL, NULL, CAST(0x0000B1F1013034B1 AS DateTime))
INSERT [dbo].[FbFormData] ([ID], [FormID], [FormDataContent], [CreatedUserID], [CreatedUserName], [CreatedDate], [LastUpdatedUserID], [LastUpdatedUserName], [LastUpdatedDate]) VALUES (42, 65, N'{"leavetype":"personal","days":6,"remark":""}', NULL, NULL, CAST(0x0000B1F10152CC4E AS DateTime), NULL, NULL, CAST(0x0000B1F10152DF50 AS DateTime))
INSERT [dbo].[FbFormData] ([ID], [FormID], [FormDataContent], [CreatedUserID], [CreatedUserName], [CreatedDate], [LastUpdatedUserID], [LastUpdatedUserName], [LastUpdatedDate]) VALUES (43, 65, N'{"leavetype":"hospital","days":2,"remark":""}', NULL, NULL, CAST(0x0000B1F1015C060B AS DateTime), NULL, NULL, CAST(0x0000B1F200901F6E AS DateTime))
INSERT [dbo].[FbFormData] ([ID], [FormID], [FormDataContent], [CreatedUserID], [CreatedUserName], [CreatedDate], [LastUpdatedUserID], [LastUpdatedUserName], [LastUpdatedDate]) VALUES (44, 65, N'{"leavetype":null,"days":null,"remark":""}', NULL, NULL, CAST(0x0000B1F1015DE09E AS DateTime), NULL, NULL, CAST(0x0000B1F101652121 AS DateTime))
INSERT [dbo].[FbFormData] ([ID], [FormID], [FormDataContent], [CreatedUserID], [CreatedUserName], [CreatedDate], [LastUpdatedUserID], [LastUpdatedUserName], [LastUpdatedDate]) VALUES (45, 65, N'{"leavetype":null,"days":null,"remark":""}', NULL, NULL, CAST(0x0000B1F10165766D AS DateTime), NULL, NULL, CAST(0x0000B1F2008FF7B8 AS DateTime))
INSERT [dbo].[FbFormData] ([ID], [FormID], [FormDataContent], [CreatedUserID], [CreatedUserName], [CreatedDate], [LastUpdatedUserID], [LastUpdatedUserName], [LastUpdatedDate]) VALUES (46, 65, N'{"leavetype":"personal","days":4,"remark":""}', NULL, NULL, CAST(0x0000B1F200938365 AS DateTime), NULL, NULL, CAST(0x0000B1F201375B35 AS DateTime))
SET IDENTITY_INSERT [dbo].[FbFormData] OFF
/****** Object:  Table [dbo].[FbForm]    Script Date: 02/28/2025 17:15:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FbForm](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FormName] [nvarchar](50) NOT NULL,
	[FormCode] [varchar](50) NOT NULL,
	[Version] [nvarchar](20) NOT NULL,
	[FieldSummary] [nvarchar](max) NULL,
	[TemplateContent] [nvarchar](max) NULL,
	[HTMLContent] [nvarchar](max) NULL,
	[Description] [nvarchar](1000) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastUpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_FbForm] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[FbForm] ON
INSERT [dbo].[FbForm] ([ID], [FormName], [FormCode], [Version], [FieldSummary], [TemplateContent], [HTMLContent], [Description], [CreatedDate], [LastUpdatedDate]) VALUES (63, N'Form_Test_Sample', N'QMIS2H', N'1', NULL, N'{"components":[{"key":"Name","type":"textfield","label":"Name","id":"Field_04cms3p","layout":{"row":"Row_12s9h36"}},{"label":"Text field","type":"textfield","layout":{"row":"Row_0sbwk3f","columns":null},"id":"Field_1ij6kae","key":"textfield_4r7o8k"},{"label":"Text area","type":"textarea","layout":{"row":"Row_0snzrhg","columns":null},"id":"Field_0ibib5u","key":"textarea_ap2ur8"},{"label":"Days","type":"number","layout":{"row":"Row_0tolbj2","columns":null},"id":"Field_1iiuhbh","key":"days","properties":{"IsCondition":"days"}},{"label":"Checkbox","type":"checkbox","layout":{"row":"Row_1lygnxi","columns":null},"id":"Field_0mcnzle","key":"checkbox_3ja15k"},{"label":"Select","type":"select","layout":{"row":"Row_16oze0r","columns":null},"id":"Field_0uc7r2i","key":"select_910eas","valuesKey":"Name"}],"schemaVersion":16,"exporter":{"name":"form-js","version":"0.1.0"},"type":"default","id":"Form_Test_Sample"}', NULL, NULL, CAST(0x0000B1E601015C75 AS DateTime), CAST(0x0000B1F100EC91E6 AS DateTime))
INSERT [dbo].[FbForm] ([ID], [FormName], [FormCode], [Version], [FieldSummary], [TemplateContent], [HTMLContent], [Description], [CreatedDate], [LastUpdatedDate]) VALUES (64, N'Form_0quwv7l', N'BO94VE', N'1', N'["Name","textfield_jwz4gt","textarea_fyb86a","number_wewpz8"]', N'{"components":[{"key":"Name","type":"textfield","label":"Name","id":"Field_1b686e0","layout":{"row":"Row_1hj7dcz"}},{"label":"Text field","type":"textfield","layout":{"row":"Row_1i6ymju","columns":null},"id":"Field_0jswdmy","key":"textfield_jwz4gt"},{"label":"Text area","type":"textarea","layout":{"row":"Row_0hjeuyx","columns":null},"id":"Field_1hxvsed","key":"textarea_fyb86a"},{"label":"Number","type":"number","layout":{"row":"Row_0rdn6s2","columns":null},"id":"Field_1ifnq2z","key":"number_wewpz8"},{"action":"submit","label":"Button","type":"button","layout":{"row":"Row_0c57o1n","columns":null},"id":"Field_1fdbwc0"}],"schemaVersion":16,"exporter":{"name":"form-js","version":"0.1.0"},"type":"default","id":"Form_0quwv7l"}', NULL, NULL, CAST(0x0000B1F1014ED95E AS DateTime), CAST(0x0000B28F00EA27A3 AS DateTime))
INSERT [dbo].[FbForm] ([ID], [FormName], [FormCode], [Version], [FieldSummary], [TemplateContent], [HTMLContent], [Description], [CreatedDate], [LastUpdatedDate]) VALUES (65, N'Form_1u453x3', N'WNMNE4', N'1', N'["leavetype","days","remark"]', N'{"components":[{"values":[{"label":"Personal","value":"personal"},{"label":"Hospital","value":"hospital"},{"label":"Vacation","value":"vacation"}],"label":"LeaveType","type":"select","layout":{"row":"Row_07w2yaf","columns":null},"id":"Field_0n0s4m0","key":"leavetype","validate":{"required":true}},{"label":"Days","type":"number","layout":{"row":"Row_00mjdyd","columns":null},"id":"Field_1fngofn","key":"days","properties":{"IsCondition":"days"},"validate":{"required":true}},{"key":"remark","type":"textfield","label":"Remark","id":"Field_0ebnmue","layout":{"row":"Row_17zudw4"}}],"schemaVersion":16,"exporter":{"name":"form-js","version":"0.1.0"},"type":"default","id":"Form_1u453x3"}', NULL, NULL, CAST(0x0000B1F10151B3FA AS DateTime), CAST(0x0000B24401285EE8 AS DateTime))
SET IDENTITY_INSERT [dbo].[FbForm] OFF
/****** Object:  Table [dbo].[BizAppFlow]    Script Date: 02/28/2025 17:15:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BizAppFlow](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[AppName] [nvarchar](50) NOT NULL,
	[AppInstanceID] [varchar](50) NOT NULL,
	[AppInstanceCode] [varchar](50) NULL,
	[Status] [varchar](10) NULL,
	[ActivityName] [nvarchar](50) NOT NULL,
	[Remark] [nvarchar](1000) NULL,
	[ChangedTime] [datetime] NOT NULL,
	[ChangedUserID] [varchar](50) NOT NULL,
	[ChangedUserName] [nvarchar](50) NULL,
 CONSTRAINT [PK_SALWALLWAORDERFLOW] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[BizAppFlow] ON
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (113, N'流程发起', N'3', NULL, NULL, N'流程发起', N'mssqlserver申请人:6-普通员工-小明', CAST(0x0000A4F500DC22C7 AS DateTime), N'6', N'普通员工-小明')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (114, N'生产订单', N'624', N'TB300427', NULL, N'派单', N'完成派单', CAST(0x0000A4F5010C6DBA AS DateTime), N'7', N' 业务员-小陈')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (115, N'生产订单', N'625', N'TB906432', NULL, N'派单', N'完成派单', CAST(0x0000A4F5010C92A0 AS DateTime), N'7', N' 业务员-小陈')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (116, N'生产订单', N'626', N'TB338322', NULL, N'派单', N'完成派单', CAST(0x0000A4F5010CA251 AS DateTime), N'7', N' 业务员-小陈')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (117, N'生产订单', N'627', N'TB612344', NULL, N'派单', N'完成派单', CAST(0x0000A4F5014DA236 AS DateTime), N'7', N' 业务员-小陈')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (118, N'生产订单', N'628', N'TB683061', NULL, N'派单', N'完成派单', CAST(0x0000A4F5014DAB96 AS DateTime), N'7', N' 业务员-小陈')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (119, N'生产订单', N'628', N'TB683061', NULL, N'打样', N'完成打样', CAST(0x0000A4F5014DC627 AS DateTime), N'11', N'打样员-飞雨')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (120, N'生产订单', N'627', N'TB612344', NULL, N'打样', N'完成打样', CAST(0x0000A4F5014DCFC6 AS DateTime), N'11', N'打样员-飞雨')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (121, N'生产订单', N'627', N'TB612344', NULL, N'生产', N'完成生产', CAST(0x0000A4F700D56961 AS DateTime), N'9', N'跟单员-张明')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (122, N'生产订单', N'631', N'TB490683', NULL, N'派单', N'完成派单', CAST(0x0000A4F900FBE434 AS DateTime), N'7', N' 业务员-小陈')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (123, N'生产订单', N'630', N'TB351094', NULL, N'派单', N'完成派单', CAST(0x0000A4FC016B0F5F AS DateTime), N'7', N' 业务员-小陈')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (124, N'生产订单', N'632', N'TB366615', NULL, N'派单', N'完成派单', CAST(0x0000A4FF00F6BDB6 AS DateTime), N'8', N'业务员-小宋')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (125, N'生产订单', N'634', N'TB969829', NULL, N'派单', N'完成派单', CAST(0x0000A4FF00F6C6CD AS DateTime), N'8', N'业务员-小宋')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (126, N'生产订单', N'633', N'TB751853', NULL, N'派单', N'完成派单', CAST(0x0000A4FF0181C823 AS DateTime), N'7', N' 业务员-小陈')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (127, N'生产订单', N'639', N'TB792242', NULL, N'派单', N'完成派单', CAST(0x0000A5000117A5C8 AS DateTime), N'7', N' 业务员-小陈')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (128, N'生产订单', N'639', N'TB792242', NULL, N'打样', N'完成打样', CAST(0x0000A501008BED22 AS DateTime), N'11', N'打样员-飞雨')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (129, N'生产订单', N'640', N'TB429545', NULL, N'派单', N'完成派单', CAST(0x0000A50A010D8B79 AS DateTime), N'7', N' 业务员-小陈')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (130, N'生产订单', N'641', N'TB817384', NULL, N'派单', N'完成派单', CAST(0x0000A50B00B381FA AS DateTime), N'7', N' 业务员-小陈')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (131, N'生产订单', N'644', N'TB348804', NULL, N'派单', N'完成派单', CAST(0x0000A50B00DCCBEB AS DateTime), N'7', N' 业务员-小陈')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (132, N'生产订单', N'643', N'TB351670', NULL, N'派单', N'完成派单', CAST(0x0000A50B00DCD1CD AS DateTime), N'7', N' 业务员-小陈')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (133, N'生产订单', N'646', N'TB992099', NULL, N'派单', N'完成派单', CAST(0x0000A50B00E44F16 AS DateTime), N'7', N' 业务员-小陈')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (134, N'生产订单', N'648', N'TB588606', NULL, N'派单', N'完成派单', CAST(0x0000A50B00EAF847 AS DateTime), N'7', N' 业务员-小陈')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (135, N'生产订单', N'642', N'TB434232', NULL, N'派单', N'完成派单', CAST(0x0000A50C0120B5EA AS DateTime), N'7', N' 业务员-小陈')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (136, N'生产订单', N'647', N'TB285386', NULL, N'派单', N'完成派单', CAST(0x0000A50F00A2DEAE AS DateTime), N'7', N' 业务员-小陈')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (137, N'生产订单', N'652', N'TB991726', NULL, N'派单', N'完成派单', CAST(0x0000A51001628464 AS DateTime), N'7', N' 业务员-小陈')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (138, N'生产订单', N'652', N'TB991726', NULL, N'打样', N'完成打样', CAST(0x0000A5100162D19D AS DateTime), N'11', N'打样员-飞雨')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (139, N'生产订单', N'652', N'TB991726', NULL, N'生产', N'完成生产', CAST(0x0000A510016319E3 AS DateTime), N'10', N'跟单员-李杰')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (140, N'生产订单', N'651', N'TB728743', NULL, N'派单', N'完成派单', CAST(0x0000A513010AF607 AS DateTime), N'7', N' 业务员-小陈')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (141, N'生产订单', N'650', N'TB328175', NULL, N'派单', N'完成派单', CAST(0x0000A513010AFA75 AS DateTime), N'7', N' 业务员-小陈')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (142, N'流程发起', N'4', NULL, NULL, N'流程发起', N'申请人:6-普通员工-小明', CAST(0x0000A52B012C1E90 AS DateTime), N'6', N'普通员工-小明')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (143, N'流程发起', N'5', NULL, NULL, N'流程发起', N'申请人:6-普通员工-小明', CAST(0x0000A52C0091FF62 AS DateTime), N'6', N'普通员工-小明')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (144, N'流程发起', N'6', NULL, NULL, N'流程发起', N'申请人:6-普通员工-小明', CAST(0x0000A52C010A2086 AS DateTime), N'6', N'普通员工-小明')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (145, N'请假流程', N'6', NULL, NULL, N'部门经理审批', N'部门经理-张(ID:5) 同意', CAST(0x0000A52C01153273 AS DateTime), N'5', N'部门经理-张')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (146, N'生产订单', N'659', N'TB710707', NULL, N'派单', N'完成派单', CAST(0x0000A578013DAC71 AS DateTime), N'7', N' 业务员-小陈')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (147, N'生产订单', N'658', N'TB575859', NULL, N'派单', N'完成派单', CAST(0x0000A57801501892 AS DateTime), N'7', N' 业务员-小陈')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (148, N'生产订单', N'659', N'TB710707', NULL, N'打样', N'完成打样', CAST(0x0000A57801503093 AS DateTime), N'11', N'打样员-飞雨')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (149, N'生产订单', N'657', N'TB358232', NULL, N'派单', N'完成派单', CAST(0x0000A5780167A1AD AS DateTime), N'7', N' 业务员-小陈')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (150, N'生产订单', N'656', N'TB779780', NULL, N'派单', N'完成派单', CAST(0x0000A57A01211907 AS DateTime), N'7', N' 业务员-小陈')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (151, N'生产订单', N'655', N'TB322602', NULL, N'派单', N'完成派单', CAST(0x0000A57C014BF2A2 AS DateTime), N'7', N' 业务员-小陈')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (152, N'生产订单', N'654', N'TB271916', NULL, N'派单', N'完成派单', CAST(0x0000A57C014D273A AS DateTime), N'7', N' 业务员-小陈')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (153, N'生产订单', N'654', N'TB271916', NULL, N'打样', N'完成打样', CAST(0x0000A57C014D8A62 AS DateTime), N'11', N'打样员-飞雨')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (154, N'生产订单', N'653', N'TB559248', NULL, N'派单', N'完成派单', CAST(0x0000A57D012BCA76 AS DateTime), N'7', N' 业务员-小陈')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (155, N'生产订单', N'649', N'TB771229', NULL, N'派单', N'完成派单', CAST(0x0000A57D014D0D3C AS DateTime), N'7', N' 业务员-小陈')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (158, N'生产订单', N'645', N'TB642095', NULL, N'派单', N'完成派单', CAST(0x0000A57D016233C7 AS DateTime), N'7', N' 业务员-小陈')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (159, N'生产订单', N'660', N'TB967961', NULL, N'派单', N'完成派单', CAST(0x0000A57D0162ECB4 AS DateTime), N'7', N' 业务员-小陈')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (160, N'生产订单', N'661', N'TB751700', NULL, N'派单', N'完成派单', CAST(0x0000A57D01648298 AS DateTime), N'7', N' 业务员-小陈')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (161, N'生产订单', N'661', N'TB751700', NULL, N'打样', N'完成打样', CAST(0x0000A57D01649AEE AS DateTime), N'11', N'打样员-飞雨')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (162, N'生产订单', N'661', N'TB751700', NULL, N'生产', N'完成生产', CAST(0x0000A57D0164B2E1 AS DateTime), N'9', N'跟单员-张明')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (163, N'生产订单', N'661', N'TB751700', NULL, N'质检', N'完成质检', CAST(0x0000A57D0164C7F0 AS DateTime), N'13', N'质检员-杰米')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (164, N'生产订单', N'661', N'TB751700', NULL, N'称重', N'完成称重', CAST(0x0000A57D01657E79 AS DateTime), N'15', N'包装员-大汉')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (165, N'生产订单', N'661', N'TB751700', NULL, N'发货', N'完成发货', CAST(0x0000A57D016593FC AS DateTime), N'15', N'包装员-大汉')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (166, N'生产订单', N'652', N'TB991726', NULL, N'派单', N'完成派单', CAST(0x0000A57E014A4DF8 AS DateTime), N'8', N'业务员-小宋')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (167, N'生产订单', N'662', N'TB647767', NULL, N'派单', N'完成派单', CAST(0x0000A57E0169A99B AS DateTime), N'7', N' 业务员-小陈')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (168, N'生产订单', N'638', N'TB561443', NULL, N'派单', N'完成派单', CAST(0x0000A57F013BE354 AS DateTime), N'8', N'业务员-小宋')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (169, N'生产订单', N'663', N'TB809544', NULL, N'派单', N'完成派单', CAST(0x0000A57F013C7377 AS DateTime), N'7', N' 业务员-小陈')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (170, N'生产订单', N'664', N'TB914891', NULL, N'派单', N'完成派单', CAST(0x0000A57F013CE48D AS DateTime), N'7', N' 业务员-小陈')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (171, N'生产订单', N'665', N'TB929075', NULL, N'派单', N'完成派单', CAST(0x0000A57F014515AA AS DateTime), N'7', N' 业务员-小陈')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (172, N'生产订单', N'666', N'TB225725', NULL, N'派单', N'完成派单', CAST(0x0000A57F0146F53B AS DateTime), N'7', N' 业务员-小陈')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (173, N'生产订单', N'667', N'TB164370', NULL, N'派单', N'完成派单', CAST(0x0000A57F014779F2 AS DateTime), N'7', N' 业务员-小陈')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (174, N'生产订单', N'667', N'TB164370', NULL, N'打样', N'完成打样', CAST(0x0000A57F0147D7EC AS DateTime), N'11', N'打样员-飞雨')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (175, N'生产订单', N'667', N'TB164370', NULL, N'生产', N'完成生产', CAST(0x0000A57F0147EF54 AS DateTime), N'9', N'跟单员-张明')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (176, N'生产订单', N'667', N'TB164370', NULL, N'质检', N'完成质检', CAST(0x0000A57F0148008F AS DateTime), N'13', N'质检员-杰米')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (177, N'生产订单', N'667', N'TB164370', NULL, N'称重', N'完成称重', CAST(0x0000A57F01481487 AS DateTime), N'15', N'包装员-大汉')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (178, N'生产订单', N'667', N'TB164370', NULL, N'发货', N'完成发货', CAST(0x0000A57F01483D30 AS DateTime), N'16', N'包装员-小威')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (179, N'流程发起', N'7', NULL, NULL, N'流程发起', N'申请人:6-普通员工-小明', CAST(0x0000A5B700B21B49 AS DateTime), N'6', N'普通员工-小明')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (180, N'请假流程', N'7', NULL, NULL, N'部门经理审批', N'部门经理-张(ID:5) 同意', CAST(0x0000A5B700B252AE AS DateTime), N'5', N'部门经理-张')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (181, N'请假流程', N'7', NULL, NULL, N'总经理审批', N'总经理-陈(ID:1) 同意', CAST(0x0000A5B700B27226 AS DateTime), N'1', N'总经理-陈')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (182, N'请假流程', N'7', NULL, NULL, N'人事经理审批', N'人事经理-李小姐(ID:4) ', CAST(0x0000A5B700B28A14 AS DateTime), N'4', N'人事经理-李小姐')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (183, N'流程发起', N'8', NULL, NULL, N'流程发起', N'申请人:6-普通员工-小明', CAST(0x0000A5B700B38A15 AS DateTime), N'6', N'普通员工-小明')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (184, N'请假流程', N'8', NULL, NULL, N'部门经理审批', N'部门经理-张(ID:5) 同意', CAST(0x0000A5B700B3AAF1 AS DateTime), N'5', N'部门经理-张')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (185, N'生产订单', N'669', N'TB747473', NULL, N'派单', N'完成派单', CAST(0x0000A5B700B3E831 AS DateTime), N'7', N' 业务员-小陈')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (186, N'生产订单', N'669', N'TB747473', NULL, N'打样', N'完成打样', CAST(0x0000A5B700B3FCE9 AS DateTime), N'11', N'打样员-飞雨')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (187, N'生产订单', N'670', N'TB630627', NULL, N'派单', N'完成派单', CAST(0x0000A5B700B44E62 AS DateTime), N'7', N' 业务员-小陈')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (188, N'生产订单', N'670', N'TB630627', NULL, N'打样', N'完成打样', CAST(0x0000A5B700B46695 AS DateTime), N'11', N'打样员-飞雨')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (189, N'生产订单', N'670', N'TB630627', NULL, N'生产', N'完成生产', CAST(0x0000A5B700B47ECE AS DateTime), N'9', N'跟单员-张明')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (190, N'生产订单', N'670', N'TB630627', NULL, N'质检', N'完成质检', CAST(0x0000A5B700B493A5 AS DateTime), N'13', N'质检员-杰米')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (191, N'生产订单', N'670', N'TB630627', NULL, N'称重', N'完成称重', CAST(0x0000A5B700B4A808 AS DateTime), N'15', N'包装员-大汉')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (192, N'生产订单', N'670', N'TB630627', NULL, N'发货', N'完成发货', CAST(0x0000A5B700B4C4D8 AS DateTime), N'15', N'包装员-大汉')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (193, N'生产订单', N'671', N'TB165916', NULL, N'派单', N'完成派单', CAST(0x0000A5C5009C0E1E AS DateTime), N'7', N' 业务员-小陈')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (194, N'流程发起', N'9', NULL, NULL, N'流程发起', N'申请人:6-普通员工-小明', CAST(0x0000A5C500A0D72F AS DateTime), N'6', N'普通员工-小明')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (195, N'流程发起', N'10', NULL, NULL, N'流程发起', N'申请人:6-普通员工-小明', CAST(0x0000A5C500B43CBB AS DateTime), N'6', N'普通员工-小明')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (196, N'流程发起', N'11', NULL, NULL, N'流程发起', N'申请人:6-普通员工-小明', CAST(0x0000A5C500FE9389 AS DateTime), N'6', N'普通员工-小明')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (197, N'生产订单', N'673', N'TB508950', NULL, N'派单', N'完成派单', CAST(0x0000A61300EE9CA7 AS DateTime), N'7', N' 业务员-小陈')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (198, N'生产订单', N'673', N'TB508950', NULL, N'打样', N'完成打样', CAST(0x0000A61300EEB976 AS DateTime), N'11', N'打样员-飞雨')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (199, N'生产订单', N'673', N'TB508950', NULL, N'生产', N'完成生产', CAST(0x0000A61300EED70C AS DateTime), N'9', N'跟单员-张明')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (200, N'生产订单', N'674', N'TB760538', NULL, N'派单', N'完成派单', CAST(0x0000A6320100EBD7 AS DateTime), N'7', N'陈盖茨')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (201, N'生产订单', N'674', N'TB760538', NULL, N'生产', N'完成生产', CAST(0x0000A6320112805C AS DateTime), N'11', N'飞羽')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (202, N'生产订单', N'672', N'TB247595', NULL, N'派单', N'完成派单', CAST(0x0000A67D015B8A25 AS DateTime), N'7', N'陈盖茨')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (203, N'生产订单', N'668', N'TB885696', NULL, N'派单', N'完成派单', CAST(0x0000A72900F7E12C AS DateTime), N'7', N'陈盖茨')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (204, N'生产订单', N'675', N'TB324384', NULL, N'派单', N'完成派单', CAST(0x0000A72900F8541C AS DateTime), N'7', N'陈盖茨')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (205, N'生产订单', N'675', N'TB324384', NULL, N'打样', N'完成打样', CAST(0x0000A72900FEA7FD AS DateTime), N'11', N'飞羽')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (206, N'生产订单', N'675', N'TB324384', NULL, N'生产', N'完成生产', CAST(0x0000A729010052AD AS DateTime), N'9', N'张明')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (207, N'生产订单', N'675', N'TB324384', NULL, N'质检', N'完成质检', CAST(0x0000A72901006C05 AS DateTime), N'13', N'杰米')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (208, N'生产订单', N'675', N'TB324384', NULL, N'称重', N'完成称重', CAST(0x0000A72901007EE5 AS DateTime), N'15', N'大汉')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (209, N'生产订单', N'675', N'TB324384', NULL, N'发货', N'完成发货', CAST(0x0000A72901008DCD AS DateTime), N'15', N'大汉')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (210, N'流程发起', N'12', NULL, NULL, N'流程发起', N'申请人:6-路天明', CAST(0x0000A7290103EC77 AS DateTime), N'6', N'路天明')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (211, N'请假流程', N'12', NULL, NULL, N'部门经理审批', N'张恒丰(ID:5) 同意', CAST(0x0000A72901040C66 AS DateTime), N'5', N'张恒丰')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (212, N'请假流程', N'12', NULL, NULL, N'人事经理审批', N'李颖(ID:4) ', CAST(0x0000A72901043923 AS DateTime), N'4', N'李颖')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (213, N'流程发起', N'13', NULL, NULL, N'流程发起', N'申请人:6-路天明', CAST(0x0000A73600E34BD1 AS DateTime), N'6', N'路天明')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (214, N'请假流程', N'13', NULL, NULL, N'部门经理审批', N'张恒丰(ID:5) AGREE', CAST(0x0000A73600E3664D AS DateTime), N'5', N'张恒丰')
GO
print 'Processed 100 total records'
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (215, N'请假流程', N'13', NULL, NULL, N'人事经理审批', N'李颖(ID:4) ', CAST(0x0000A73600E378AA AS DateTime), N'4', N'李颖')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (216, N'生产订单', N'676', N'TB377329', NULL, N'派单', N'完成派单', CAST(0x0000A79000CD1AA5 AS DateTime), N'7', N'陈盖茨')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (217, N'流程发起', N'32', NULL, NULL, N'流程发起', N'申请人:6-路天明', CAST(0x0000A7B8009703E0 AS DateTime), N'6', N'路天明')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (218, N'请假流程', N'32', NULL, NULL, N'部门经理审批', N'张恒丰(ID:5) 同意', CAST(0x0000A7B80097B401 AS DateTime), N'5', N'张恒丰')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (219, N'流程发起', N'33', NULL, NULL, N'流程发起', N'申请人:6-路天明', CAST(0x0000A7B8009BF515 AS DateTime), N'6', N'路天明')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (220, N'生产订单', N'678', N'TB574787', NULL, N'派单', N'完成派单', CAST(0x0000A7B8009E525B AS DateTime), N'7', N'陈盖茨')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (221, N'生产订单', N'679', N'TB100834', NULL, N'派单', N'完成派单', CAST(0x0000A7D8013B0D59 AS DateTime), N'7', N'陈盖茨')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (222, N'生产订单', N'679', N'TB100834', NULL, N'打样', N'完成打样', CAST(0x0000A7D8013B21C8 AS DateTime), N'11', N'飞羽')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (223, N'生产订单', N'680', N'TB752624', NULL, N'派单', N'完成派单', CAST(0x0000A83F00B6F0E8 AS DateTime), N'7', N'陈盖茨')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (224, N'生产订单', N'680', N'TB752624', NULL, N'打样', N'完成打样', CAST(0x0000A83F00B706F3 AS DateTime), N'11', N'飞羽')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (225, N'生产订单', N'680', N'TB752624', NULL, N'生产', N'完成生产', CAST(0x0000A83F00B715C3 AS DateTime), N'9', N'张明')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (226, N'生产订单', N'680', N'TB752624', NULL, N'质检', N'完成质检', CAST(0x0000A83F00B72520 AS DateTime), N'13', N'杰米')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (227, N'生产订单', N'680', N'TB752624', NULL, N'发货', N'完成发货', CAST(0x0000A83F00B73839 AS DateTime), N'15', N'大汉')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (228, N'生产订单', N'680', N'TB752624', NULL, N'发货', N'完成发货', CAST(0x0000A83F00B7513D AS DateTime), N'16', N'小威')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (229, N'生产订单', N'681', N'TB517477', NULL, N'派单', N'完成派单', CAST(0x0000A83F00E5D4E7 AS DateTime), N'7', N'陈盖茨')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (230, N'生产订单', N'681', N'TB265497', NULL, N'派单', N'完成派单', CAST(0x0000A842010B62E3 AS DateTime), N'7', N'陈盖茨')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (231, N'生产订单', N'682', N'TB601588', NULL, N'派单', N'完成派单', CAST(0x0000A842010B92E7 AS DateTime), N'7', N'陈盖茨')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (232, N'生产订单', N'682', N'TB601588', NULL, N'打样', N'完成打样', CAST(0x0000A842010BA375 AS DateTime), N'11', N'飞羽')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (233, N'生产订单', N'677', N'TB730548', NULL, N'派单', N'完成派单', CAST(0x0000A9020117346D AS DateTime), N'7', N'陈盖茨')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (234, N'生产订单', N'684', N'TB937073', NULL, N'派单', N'完成派单', CAST(0x0000AA0801604495 AS DateTime), N'7', N'陈盖茨')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (235, N'生产订单', N'685', N'TB359987', NULL, N'派单', N'完成派单', CAST(0x0000AAF600EF4E8C AS DateTime), N'11', N'飞羽')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (236, N'生产订单', N'686', N'TB588656', NULL, N'派单', N'完成派单', CAST(0x0000ABBC00B3407D AS DateTime), N'7', N'Gates')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (237, N'生产订单', N'686', N'TB588656', NULL, N'打样', N'完成打样', CAST(0x0000ABBC00B35BCA AS DateTime), N'11', N'FeiYu')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (238, N'生产订单', N'686', N'TB588656', NULL, N'生产', N'完成生产', CAST(0x0000ABBC00B373CA AS DateTime), N'9', N'ZhangMing')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (239, N'生产订单', N'686', N'TB588656', NULL, N'质检', N'完成质检', CAST(0x0000ABBC00B38910 AS DateTime), N'13', N'Jimi')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (240, N'生产订单', N'686', N'TB588656', NULL, N'称重', N'完成称重', CAST(0x0000ABBC00B39DDC AS DateTime), N'15', N'DaHan')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (241, N'生产订单', N'686', N'TB588656', NULL, N'发货', N'完成发货', CAST(0x0000ABBC00B3A68D AS DateTime), N'15', N'DaHan')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (242, N'生产订单', N'687', N'TB720748', NULL, N'派单', N'完成派单', CAST(0x0000AC2300FA38B0 AS DateTime), N'7', N'Gates')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (243, N'生产订单', N'687', N'TB720748', NULL, N'打样', N'完成打样', CAST(0x0000AC2300FBAD6B AS DateTime), N'11', N'FeiYu')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (244, N'生产订单', N'687', N'TB720748', NULL, N'生产', N'完成生产', CAST(0x0000AC2300FBC556 AS DateTime), N'9', N'ZhangMing')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (245, N'生产订单', N'687', N'TB720748', NULL, N'质检', N'完成质检', CAST(0x0000AC2300FBE00F AS DateTime), N'13', N'Jimi')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (246, N'生产订单', N'687', N'TB720748', NULL, N'发货', N'完成发货', CAST(0x0000AC2300FBF5BF AS DateTime), N'15', N'DaHan')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (247, N'生产订单', N'687', N'TB720748', NULL, N'发货', N'完成发货', CAST(0x0000AC2300FC0757 AS DateTime), N'15', N'DaHan')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (248, N'生产订单', N'688', N'TB332639', NULL, N'派单', N'完成派单', CAST(0x0000ACB000A03E27 AS DateTime), N'7', N'Gates')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (249, N'生产订单', N'689', N'TB741954', NULL, N'派单', N'完成派单', CAST(0x0000ACB000A1EC13 AS DateTime), N'7', N'Gates')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (250, N'生产订单', N'689', N'TB741954', NULL, N'打样', N'完成打样', CAST(0x0000ACB000A2023D AS DateTime), N'11', N'FeiYu')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (251, N'生产订单', N'689', N'TB741954', NULL, N'生产', N'完成生产', CAST(0x0000ACB000A21455 AS DateTime), N'9', N'ZhangMing')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (252, N'生产订单', N'689', N'TB741954', NULL, N'质检', N'完成质检', CAST(0x0000ACB000A224F6 AS DateTime), N'13', N'Jimi')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (253, N'生产订单', N'689', N'TB741954', NULL, N'称重', N'完成称重', CAST(0x0000ACB000A236B8 AS DateTime), N'15', N'DaHan')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (254, N'生产订单', N'689', N'TB741954', NULL, N'发货', N'完成发货', CAST(0x0000ACB000A24580 AS DateTime), N'15', N'DaHan')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (255, N'生产订单', N'690', N'TB332806', NULL, N'派单', N'完成派单', CAST(0x0000AD8F01005966 AS DateTime), N'7', N'Peter')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (256, N'生产订单', N'683', N'TB393078', NULL, N'派单', N'完成派单', CAST(0x0000AE6000AC5ED0 AS DateTime), N'7', N'Peter')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (257, N'生产订单', N'691', N'TB452818', NULL, N'派单', N'完成派单', CAST(0x0000AE6000AC8A4F AS DateTime), N'8', N'Bill')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (258, N'生产订单', N'693', N'TB307954', NULL, N'派单', N'完成派单', CAST(0x0000AE8200F0D3BC AS DateTime), N'7', N'Peter')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (259, N'生产订单', N'693', N'TB307954', NULL, N'打样', N'完成打样', CAST(0x0000AE8200F0F10C AS DateTime), N'11', N'Fisher')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (260, N'生产订单', N'693', N'TB307954', NULL, N'生产', N'完成生产', CAST(0x0000AE8200F103B1 AS DateTime), N'9', N'Tuda')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (261, N'生产订单', N'693', N'TB307954', NULL, N'质检', N'完成质检', CAST(0x0000AE8200F12A12 AS DateTime), N'13', N'Jimi')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (262, N'生产订单', N'693', N'TB307954', NULL, N'称重', N'完成称重', CAST(0x0000AE8200F13BEE AS DateTime), N'16', N'Smith')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (263, N'生产订单', N'693', N'TB307954', NULL, N'发货', N'完成发货', CAST(0x0000AE8200F14F39 AS DateTime), N'16', N'Smith')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (268, N'生产订单', N'692', N'TB263682', NULL, N'派单', N'完成派单', CAST(0x0000AECD008DB4CA AS DateTime), N'7', N'Peter')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (269, N'生产订单', N'692', N'TB263682', NULL, N'打样', N'完成打样', CAST(0x0000AECD008DCEBD AS DateTime), N'11', N'Fisher')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (270, N'生产订单', N'692', N'TB263682', NULL, N'生产', N'完成生产', CAST(0x0000AECD008DE1E6 AS DateTime), N'9', N'Tuda')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (271, N'生产订单', N'692', N'TB263682', NULL, N'质检', N'完成质检', CAST(0x0000AECD008DF757 AS DateTime), N'13', N'Jimi')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (272, N'生产订单', N'692', N'TB263682', NULL, N'称重', N'完成称重', CAST(0x0000AECD008F4CFB AS DateTime), N'15', N'Damark')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (273, N'生产订单', N'692', N'TB263682', NULL, N'发货', N'完成发货', CAST(0x0000AECD008F64EB AS DateTime), N'15', N'Damark')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (274, N'生产订单', N'694', N'TB293064', NULL, N'派单', N'完成派单', CAST(0x0000AECE014A0BAC AS DateTime), N'7', N'Peter')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (275, N'生产订单', N'695', N'TB226532', NULL, N'派单', N'完成派单', CAST(0x0000B10C0108768F AS DateTime), N'7', N'Peter')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (276, N'生产订单', N'696', N'TB207310', NULL, N'派单', N'完成派单', CAST(0x0000B1960098D0CF AS DateTime), N'7', N'Peter')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (277, N'生产订单', N'696', N'TB207310', NULL, N'打样', N'完成打样', CAST(0x0000B19600AB1337 AS DateTime), N'11', N'Fisher')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (278, N'生产订单', N'696', N'TB207310', NULL, N'生产', N'完成生产', CAST(0x0000B19600AB4D0D AS DateTime), N'9', N'Tuda')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (279, N'生产订单', N'696', N'TB207310', NULL, N'质检', N'完成质检', CAST(0x0000B19600AB71E1 AS DateTime), N'13', N'Jimi')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (280, N'生产订单', N'696', N'TB207310', NULL, N'发货', N'完成发货', CAST(0x0000B19600AB8621 AS DateTime), N'15', N'Damark')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (281, N'生产订单', N'696', N'TB207310', NULL, N'发货', N'完成发货', CAST(0x0000B19600AB9549 AS DateTime), N'15', N'Damark')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (282, N'生产订单', N'697', N'TB257249', NULL, N'派单', N'完成派单', CAST(0x0000B19600B573DE AS DateTime), N'7', N'Peter')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (283, N'生产订单', N'697', N'TB257249', NULL, N'打样', N'完成打样', CAST(0x0000B19600B597A1 AS DateTime), N'11', N'Fisher')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (284, N'生产订单', N'698', N'TB409086', NULL, N'派单', N'完成派单', CAST(0x0000B19600E2F00E AS DateTime), N'7', N'Peter')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (285, N'生产订单', N'698', N'TB409086', NULL, N'打样', N'完成打样', CAST(0x0000B19600E31CD0 AS DateTime), N'11', N'Fisher')
SET IDENTITY_INSERT [dbo].[BizAppFlow] OFF
/****** Object:  Table [dbo].[tmpTest]    Script Date: 02/28/2025 17:15:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tmpTest](
	[ID] [int] NOT NULL,
	[Title] [varchar](50) NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[tmpTest] ([ID], [Title]) VALUES (1, N'a999')
INSERT [dbo].[tmpTest] ([ID], [Title]) VALUES (2, N'B')
INSERT [dbo].[tmpTest] ([ID], [Title]) VALUES (3, N'C')
INSERT [dbo].[tmpTest] ([ID], [Title]) VALUES (1, N'a999')
INSERT [dbo].[tmpTest] ([ID], [Title]) VALUES (2, N'B')
INSERT [dbo].[tmpTest] ([ID], [Title]) VALUES (3, N'C')
/****** Object:  Table [dbo].[SysUserResource]    Script Date: 02/28/2025 17:15:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SysUserResource](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[ResourceID] [int] NOT NULL,
 CONSTRAINT [PK_SysUserResource] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[SysUserResource] ON
INSERT [dbo].[SysUserResource] ([ID], [UserID], [ResourceID]) VALUES (1, 7, 1)
INSERT [dbo].[SysUserResource] ([ID], [UserID], [ResourceID]) VALUES (2, 7, 2)
INSERT [dbo].[SysUserResource] ([ID], [UserID], [ResourceID]) VALUES (3, 7, 4)
INSERT [dbo].[SysUserResource] ([ID], [UserID], [ResourceID]) VALUES (4, 7, 5)
INSERT [dbo].[SysUserResource] ([ID], [UserID], [ResourceID]) VALUES (5, 8, 1)
INSERT [dbo].[SysUserResource] ([ID], [UserID], [ResourceID]) VALUES (6, 8, 2)
INSERT [dbo].[SysUserResource] ([ID], [UserID], [ResourceID]) VALUES (7, 8, 4)
INSERT [dbo].[SysUserResource] ([ID], [UserID], [ResourceID]) VALUES (8, 8, 5)
INSERT [dbo].[SysUserResource] ([ID], [UserID], [ResourceID]) VALUES (9, 11, 1)
INSERT [dbo].[SysUserResource] ([ID], [UserID], [ResourceID]) VALUES (10, 11, 2)
INSERT [dbo].[SysUserResource] ([ID], [UserID], [ResourceID]) VALUES (11, 11, 6)
INSERT [dbo].[SysUserResource] ([ID], [UserID], [ResourceID]) VALUES (12, 12, 1)
INSERT [dbo].[SysUserResource] ([ID], [UserID], [ResourceID]) VALUES (13, 12, 2)
INSERT [dbo].[SysUserResource] ([ID], [UserID], [ResourceID]) VALUES (14, 12, 6)
INSERT [dbo].[SysUserResource] ([ID], [UserID], [ResourceID]) VALUES (15, 9, 1)
INSERT [dbo].[SysUserResource] ([ID], [UserID], [ResourceID]) VALUES (16, 9, 2)
INSERT [dbo].[SysUserResource] ([ID], [UserID], [ResourceID]) VALUES (17, 9, 7)
INSERT [dbo].[SysUserResource] ([ID], [UserID], [ResourceID]) VALUES (18, 10, 1)
INSERT [dbo].[SysUserResource] ([ID], [UserID], [ResourceID]) VALUES (19, 10, 2)
INSERT [dbo].[SysUserResource] ([ID], [UserID], [ResourceID]) VALUES (20, 10, 7)
INSERT [dbo].[SysUserResource] ([ID], [UserID], [ResourceID]) VALUES (21, 13, 1)
INSERT [dbo].[SysUserResource] ([ID], [UserID], [ResourceID]) VALUES (22, 13, 2)
INSERT [dbo].[SysUserResource] ([ID], [UserID], [ResourceID]) VALUES (23, 13, 8)
INSERT [dbo].[SysUserResource] ([ID], [UserID], [ResourceID]) VALUES (24, 14, 1)
INSERT [dbo].[SysUserResource] ([ID], [UserID], [ResourceID]) VALUES (25, 14, 2)
INSERT [dbo].[SysUserResource] ([ID], [UserID], [ResourceID]) VALUES (26, 14, 8)
INSERT [dbo].[SysUserResource] ([ID], [UserID], [ResourceID]) VALUES (27, 15, 1)
INSERT [dbo].[SysUserResource] ([ID], [UserID], [ResourceID]) VALUES (28, 15, 2)
INSERT [dbo].[SysUserResource] ([ID], [UserID], [ResourceID]) VALUES (29, 15, 9)
INSERT [dbo].[SysUserResource] ([ID], [UserID], [ResourceID]) VALUES (30, 15, 10)
INSERT [dbo].[SysUserResource] ([ID], [UserID], [ResourceID]) VALUES (31, 16, 1)
INSERT [dbo].[SysUserResource] ([ID], [UserID], [ResourceID]) VALUES (32, 16, 2)
INSERT [dbo].[SysUserResource] ([ID], [UserID], [ResourceID]) VALUES (33, 16, 9)
INSERT [dbo].[SysUserResource] ([ID], [UserID], [ResourceID]) VALUES (34, 16, 10)
SET IDENTITY_INSERT [dbo].[SysUserResource] OFF
/****** Object:  Table [dbo].[SysUser]    Script Date: 02/28/2025 17:15:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SysUser](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[EMail] [varchar](100) NULL,
 CONSTRAINT [PK_SysUser] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[SysUser] ON
INSERT [dbo].[SysUser] ([ID], [UserName], [EMail]) VALUES (1, N'Cindy', NULL)
INSERT [dbo].[SysUser] ([ID], [UserName], [EMail]) VALUES (2, N'Henry', NULL)
INSERT [dbo].[SysUser] ([ID], [UserName], [EMail]) VALUES (3, N'Test', N'jack@163.com')
INSERT [dbo].[SysUser] ([ID], [UserName], [EMail]) VALUES (4, N'LeeO', NULL)
INSERT [dbo].[SysUser] ([ID], [UserName], [EMail]) VALUES (5, N'Ada', NULL)
INSERT [dbo].[SysUser] ([ID], [UserName], [EMail]) VALUES (6, N'Lucy', NULL)
INSERT [dbo].[SysUser] ([ID], [UserName], [EMail]) VALUES (7, N'Peter', NULL)
INSERT [dbo].[SysUser] ([ID], [UserName], [EMail]) VALUES (8, N'Bill', NULL)
INSERT [dbo].[SysUser] ([ID], [UserName], [EMail]) VALUES (9, N'Tuda', NULL)
INSERT [dbo].[SysUser] ([ID], [UserName], [EMail]) VALUES (10, N'Jack', N'hr@ruochisoft.com')
INSERT [dbo].[SysUser] ([ID], [UserName], [EMail]) VALUES (11, N'Fisher', N'hr@ruochisoft.com')
INSERT [dbo].[SysUser] ([ID], [UserName], [EMail]) VALUES (12, N'Sherley', N'hr@ruochisoft.com')
INSERT [dbo].[SysUser] ([ID], [UserName], [EMail]) VALUES (13, N'Jimi', NULL)
INSERT [dbo].[SysUser] ([ID], [UserName], [EMail]) VALUES (14, N'William', NULL)
INSERT [dbo].[SysUser] ([ID], [UserName], [EMail]) VALUES (15, N'Damark', NULL)
INSERT [dbo].[SysUser] ([ID], [UserName], [EMail]) VALUES (16, N'Smith', NULL)
INSERT [dbo].[SysUser] ([ID], [UserName], [EMail]) VALUES (17, N'Yolanda', NULL)
INSERT [dbo].[SysUser] ([ID], [UserName], [EMail]) VALUES (18, N'Jinny', NULL)
INSERT [dbo].[SysUser] ([ID], [UserName], [EMail]) VALUES (19, N'Susan', N'hr@ruochisoft.com')
SET IDENTITY_INSERT [dbo].[SysUser] OFF
/****** Object:  Table [dbo].[SysRoleUser]    Script Date: 02/28/2025 17:15:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SysRoleUser](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RoleID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
 CONSTRAINT [PK_SysRoleUser] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[SysRoleUser] ON
INSERT [dbo].[SysRoleUser] ([ID], [RoleID], [UserID]) VALUES (1, 8, 1)
INSERT [dbo].[SysRoleUser] ([ID], [RoleID], [UserID]) VALUES (2, 7, 2)
INSERT [dbo].[SysRoleUser] ([ID], [RoleID], [UserID]) VALUES (3, 4, 3)
INSERT [dbo].[SysRoleUser] ([ID], [RoleID], [UserID]) VALUES (4, 3, 4)
INSERT [dbo].[SysRoleUser] ([ID], [RoleID], [UserID]) VALUES (5, 2, 5)
INSERT [dbo].[SysRoleUser] ([ID], [RoleID], [UserID]) VALUES (6, 1, 6)
INSERT [dbo].[SysRoleUser] ([ID], [RoleID], [UserID]) VALUES (7, 9, 7)
INSERT [dbo].[SysRoleUser] ([ID], [RoleID], [UserID]) VALUES (8, 9, 8)
INSERT [dbo].[SysRoleUser] ([ID], [RoleID], [UserID]) VALUES (9, 10, 11)
INSERT [dbo].[SysRoleUser] ([ID], [RoleID], [UserID]) VALUES (10, 10, 12)
INSERT [dbo].[SysRoleUser] ([ID], [RoleID], [UserID]) VALUES (11, 11, 9)
INSERT [dbo].[SysRoleUser] ([ID], [RoleID], [UserID]) VALUES (12, 11, 10)
INSERT [dbo].[SysRoleUser] ([ID], [RoleID], [UserID]) VALUES (13, 12, 13)
INSERT [dbo].[SysRoleUser] ([ID], [RoleID], [UserID]) VALUES (14, 12, 14)
INSERT [dbo].[SysRoleUser] ([ID], [RoleID], [UserID]) VALUES (15, 13, 15)
INSERT [dbo].[SysRoleUser] ([ID], [RoleID], [UserID]) VALUES (16, 13, 16)
INSERT [dbo].[SysRoleUser] ([ID], [RoleID], [UserID]) VALUES (17, 14, 17)
INSERT [dbo].[SysRoleUser] ([ID], [RoleID], [UserID]) VALUES (19, 2, 17)
SET IDENTITY_INSERT [dbo].[SysRoleUser] OFF
/****** Object:  Table [dbo].[SysRoleGroupResource]    Script Date: 02/28/2025 17:15:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SysRoleGroupResource](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RgType] [smallint] NOT NULL,
	[RgID] [int] NOT NULL,
	[ResourceID] [int] NOT NULL,
	[PermissionType] [smallint] NOT NULL,
 CONSTRAINT [PK_SysRoleGroupResource] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[SysRoleGroupResource] ON
INSERT [dbo].[SysRoleGroupResource] ([ID], [RgType], [RgID], [ResourceID], [PermissionType]) VALUES (1, 1, 9, 1, 1)
INSERT [dbo].[SysRoleGroupResource] ([ID], [RgType], [RgID], [ResourceID], [PermissionType]) VALUES (2, 1, 9, 2, 1)
INSERT [dbo].[SysRoleGroupResource] ([ID], [RgType], [RgID], [ResourceID], [PermissionType]) VALUES (3, 1, 9, 4, 1)
INSERT [dbo].[SysRoleGroupResource] ([ID], [RgType], [RgID], [ResourceID], [PermissionType]) VALUES (4, 1, 9, 5, 1)
INSERT [dbo].[SysRoleGroupResource] ([ID], [RgType], [RgID], [ResourceID], [PermissionType]) VALUES (5, 1, 10, 1, 1)
INSERT [dbo].[SysRoleGroupResource] ([ID], [RgType], [RgID], [ResourceID], [PermissionType]) VALUES (6, 1, 10, 2, 1)
INSERT [dbo].[SysRoleGroupResource] ([ID], [RgType], [RgID], [ResourceID], [PermissionType]) VALUES (7, 1, 10, 6, 1)
INSERT [dbo].[SysRoleGroupResource] ([ID], [RgType], [RgID], [ResourceID], [PermissionType]) VALUES (8, 1, 11, 7, 1)
INSERT [dbo].[SysRoleGroupResource] ([ID], [RgType], [RgID], [ResourceID], [PermissionType]) VALUES (9, 1, 12, 8, 1)
INSERT [dbo].[SysRoleGroupResource] ([ID], [RgType], [RgID], [ResourceID], [PermissionType]) VALUES (10, 1, 13, 9, 1)
INSERT [dbo].[SysRoleGroupResource] ([ID], [RgType], [RgID], [ResourceID], [PermissionType]) VALUES (11, 1, 13, 10, 1)
SET IDENTITY_INSERT [dbo].[SysRoleGroupResource] OFF
/****** Object:  Table [dbo].[SysRole]    Script Date: 02/28/2025 17:15:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SysRole](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RoleCode] [varchar](50) NOT NULL,
	[RoleName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_SysRole] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[SysRole] ON
INSERT [dbo].[SysRole] ([ID], [RoleCode], [RoleName]) VALUES (1, N'employees', N'普通员工')
INSERT [dbo].[SysRole] ([ID], [RoleCode], [RoleName]) VALUES (2, N'depmanager', N'部门经理')
INSERT [dbo].[SysRole] ([ID], [RoleCode], [RoleName]) VALUES (3, N'hrmanager', N'人事经理')
INSERT [dbo].[SysRole] ([ID], [RoleCode], [RoleName]) VALUES (4, N'director', N'主管总监')
INSERT [dbo].[SysRole] ([ID], [RoleCode], [RoleName]) VALUES (7, N'deputygeneralmanager', N'副总经理')
INSERT [dbo].[SysRole] ([ID], [RoleCode], [RoleName]) VALUES (8, N'generalmanager', N'总经理')
INSERT [dbo].[SysRole] ([ID], [RoleCode], [RoleName]) VALUES (9, N'salesmate', N'业务员')
INSERT [dbo].[SysRole] ([ID], [RoleCode], [RoleName]) VALUES (10, N'techmate', N'打样员')
INSERT [dbo].[SysRole] ([ID], [RoleCode], [RoleName]) VALUES (11, N'merchandiser', N'跟单员')
INSERT [dbo].[SysRole] ([ID], [RoleCode], [RoleName]) VALUES (12, N'qcmate', N'质检员')
INSERT [dbo].[SysRole] ([ID], [RoleCode], [RoleName]) VALUES (13, N'expressmate', N'包装员')
INSERT [dbo].[SysRole] ([ID], [RoleCode], [RoleName]) VALUES (14, N'finacemanager', N'财务经理')
INSERT [dbo].[SysRole] ([ID], [RoleCode], [RoleName]) VALUES (21, N'testrole', N'testrole')
SET IDENTITY_INSERT [dbo].[SysRole] OFF
/****** Object:  Table [dbo].[SysResource]    Script Date: 02/28/2025 17:15:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SysResource](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ResourceType] [smallint] NOT NULL,
	[ParentResourceID] [int] NOT NULL,
	[ResourceName] [nvarchar](50) NOT NULL,
	[ResourceCode] [varchar](100) NOT NULL,
	[OrderNo] [smallint] NULL,
 CONSTRAINT [PK_SysResource] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[SysResource] ON
INSERT [dbo].[SysResource] ([ID], [ResourceType], [ParentResourceID], [ResourceName], [ResourceCode], [OrderNo]) VALUES (1, 1, 0, N'生产订单系统', N'SfDemo.Made', 1)
INSERT [dbo].[SysResource] ([ID], [ResourceType], [ParentResourceID], [ResourceName], [ResourceCode], [OrderNo]) VALUES (2, 2, 1, N'生产订单流程', N'SfDemo.Made.POrder', 1)
INSERT [dbo].[SysResource] ([ID], [ResourceType], [ParentResourceID], [ResourceName], [ResourceCode], [OrderNo]) VALUES (4, 5, 2, N'同步订单', N'SfDemo.Made.POrder.SyncOrder', 1)
INSERT [dbo].[SysResource] ([ID], [ResourceType], [ParentResourceID], [ResourceName], [ResourceCode], [OrderNo]) VALUES (5, 5, 2, N'分派订单', N'SfDemo.Made.POrder.Dispatch', 2)
INSERT [dbo].[SysResource] ([ID], [ResourceType], [ParentResourceID], [ResourceName], [ResourceCode], [OrderNo]) VALUES (6, 5, 2, N'打样', N'SfDemo.Made.POrder.Sample', 3)
INSERT [dbo].[SysResource] ([ID], [ResourceType], [ParentResourceID], [ResourceName], [ResourceCode], [OrderNo]) VALUES (7, 5, 2, N'生产', N'SfDemo.Made.POrder.Manufacture', 4)
INSERT [dbo].[SysResource] ([ID], [ResourceType], [ParentResourceID], [ResourceName], [ResourceCode], [OrderNo]) VALUES (8, 5, 2, N'质检', N'SfDemo.Made.POrder.QCCheck', 5)
INSERT [dbo].[SysResource] ([ID], [ResourceType], [ParentResourceID], [ResourceName], [ResourceCode], [OrderNo]) VALUES (9, 5, 2, N'称重', N'SfDemo.Made.POrder.Weight', 6)
INSERT [dbo].[SysResource] ([ID], [ResourceType], [ParentResourceID], [ResourceName], [ResourceCode], [OrderNo]) VALUES (10, 5, 2, N'发货', N'SfDemo.Made.POrder.Delivery', 7)
SET IDENTITY_INSERT [dbo].[SysResource] OFF
/****** Object:  Table [dbo].[SysEmployeeManager]    Script Date: 02/28/2025 17:15:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SysEmployeeManager](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeID] [int] NOT NULL,
	[EmpUserID] [int] NOT NULL,
	[ManagerID] [int] NOT NULL,
	[MgrUserID] [int] NOT NULL,
 CONSTRAINT [PK_SysEmployeeManager] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[SysEmployeeManager] ON
INSERT [dbo].[SysEmployeeManager] ([ID], [EmployeeID], [EmpUserID], [ManagerID], [MgrUserID]) VALUES (1, 1, 6, 2, 5)
INSERT [dbo].[SysEmployeeManager] ([ID], [EmployeeID], [EmpUserID], [ManagerID], [MgrUserID]) VALUES (2, 4, 10, 5, 17)
INSERT [dbo].[SysEmployeeManager] ([ID], [EmployeeID], [EmpUserID], [ManagerID], [MgrUserID]) VALUES (4, 6, 9, 3, 5)
INSERT [dbo].[SysEmployeeManager] ([ID], [EmployeeID], [EmpUserID], [ManagerID], [MgrUserID]) VALUES (5, 4, 10, 7, 18)
SET IDENTITY_INSERT [dbo].[SysEmployeeManager] OFF
/****** Object:  Table [dbo].[SysEmployee]    Script Date: 02/28/2025 17:15:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SysEmployee](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DeptID] [int] NOT NULL,
	[EmpCode] [varchar](50) NOT NULL,
	[EmpName] [nvarchar](50) NOT NULL,
	[UserID] [int] NULL,
	[Mobile] [varchar](20) NULL,
	[EMail] [varchar](100) NULL,
	[Remark] [nvarchar](500) NULL,
 CONSTRAINT [PK_SYSEMPLOYEE] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[SysEmployee] ON
INSERT [dbo].[SysEmployee] ([ID], [DeptID], [EmpCode], [EmpName], [UserID], [Mobile], [EMail], [Remark]) VALUES (1, 2, N'0001', N'路天明', 6, NULL, NULL, NULL)
INSERT [dbo].[SysEmployee] ([ID], [DeptID], [EmpCode], [EmpName], [UserID], [Mobile], [EMail], [Remark]) VALUES (2, 2, N'0002', N'张经理', 5, NULL, NULL, NULL)
INSERT [dbo].[SysEmployee] ([ID], [DeptID], [EmpCode], [EmpName], [UserID], [Mobile], [EMail], [Remark]) VALUES (3, 3, N'0003', N'金经理', 18, NULL, NULL, NULL)
INSERT [dbo].[SysEmployee] ([ID], [DeptID], [EmpCode], [EmpName], [UserID], [Mobile], [EMail], [Remark]) VALUES (4, 4, N'0004', N'阿杰', 10, NULL, NULL, NULL)
INSERT [dbo].[SysEmployee] ([ID], [DeptID], [EmpCode], [EmpName], [UserID], [Mobile], [EMail], [Remark]) VALUES (5, 4, N'0005', N'崔经理', 17, NULL, NULL, NULL)
INSERT [dbo].[SysEmployee] ([ID], [DeptID], [EmpCode], [EmpName], [UserID], [Mobile], [EMail], [Remark]) VALUES (6, 2, N'0010', N'张明', 9, NULL, NULL, NULL)
INSERT [dbo].[SysEmployee] ([ID], [DeptID], [EmpCode], [EmpName], [UserID], [Mobile], [EMail], [Remark]) VALUES (7, 4, N'0030', N'金兰', 18, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[SysEmployee] OFF
/****** Object:  Table [dbo].[SysDepartment]    Script Date: 02/28/2025 17:15:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SysDepartment](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DeptCode] [varchar](50) NOT NULL,
	[DeptName] [nvarchar](100) NOT NULL,
	[ParentID] [int] NOT NULL,
	[Description] [nvarchar](500) NULL,
 CONSTRAINT [PK_SYSDEPARTMENT] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[SysDepartment] ON
INSERT [dbo].[SysDepartment] ([ID], [DeptCode], [DeptName], [ParentID], [Description]) VALUES (1, N'CP', N'SlickOne科技', 0, NULL)
INSERT [dbo].[SysDepartment] ([ID], [DeptCode], [DeptName], [ParentID], [Description]) VALUES (2, N'TH', N'技术部', 1, NULL)
INSERT [dbo].[SysDepartment] ([ID], [DeptCode], [DeptName], [ParentID], [Description]) VALUES (3, N'HR', N'人事部', 1, NULL)
INSERT [dbo].[SysDepartment] ([ID], [DeptCode], [DeptName], [ParentID], [Description]) VALUES (4, N'FN', N'财务部', 1, NULL)
SET IDENTITY_INSERT [dbo].[SysDepartment] OFF
/****** Object:  StoredProcedure [dbo].[pr_sys_UserSave]    Script Date: 02/28/2025 17:15:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[pr_sys_UserSave]
   @userID			int,
   @userName		varchar(100)

AS

BEGIN

	SET NOCOUNT ON
	-- 检查条件
	IF EXISTS(SELECT 1 
			  FROM SysUser 
			  WHERE ID<>@userID 
				AND (UserName=@userName)
			  )
		RAISERROR ('插入或编辑用户数据失败: 有重复的名称已经存在!', 16, 1)

    --插入或者编辑				
	BEGIN TRY
		IF (@userID>0)
			UPDATE SysUser
			SET UserName=@userName
			WHERE ID=@userID
		ELSE
		    INSERT INTO SysUser(UserName)
		    VALUES(@userName)
	END TRY
	BEGIN CATCH
			DECLARE @error int, @message varchar(4000)
			SELECT @error = ERROR_NUMBER()
				, @message = ERROR_MESSAGE();
			RAISERROR ('插入或编辑用户数据失败: %d: %s', 16, 1, @error, @message)
	END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[pr_sys_UserDelete]    Script Date: 02/28/2025 17:15:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[pr_sys_UserDelete]
   @userID			int

AS

BEGIN

	SET NOCOUNT ON
    --删除操作				
	BEGIN TRY
		DELETE FROM SysRoleUser WHERE UserID=@userID
		DELETE FROM SysUser WHERE ID=@userID
	END TRY
	BEGIN CATCH
			DECLARE @error int, @message varchar(4000)
			SELECT @error = ERROR_NUMBER()
				, @message = ERROR_MESSAGE();
			RAISERROR ('删除用户数据失败: %d: %s', 16, 1, @error, @message)
	END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[pr_sys_RoleUserDelete]    Script Date: 02/28/2025 17:15:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[pr_sys_RoleUserDelete]
   @userID			int,
   @roleID			int

AS

BEGIN

	SET NOCOUNT ON
    --删除操作				
	BEGIN TRY
		IF (@userID = -1)
			DELETE FROM SysRoleUser WHERE RoleID=@roleID
		ELSE
			DELETE FROM SysRoleUser WHERE UserID=@userID AND RoleID=@roleID
	END TRY
	BEGIN CATCH
			DECLARE @error int, @message varchar(4000)
			SELECT @error = ERROR_NUMBER()
				, @message = ERROR_MESSAGE();
			RAISERROR ('删除角色下的用户数据失败: %d: %s', 16, 1, @error, @message)
	END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[pr_sys_RoleSave]    Script Date: 02/28/2025 17:15:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[pr_sys_RoleSave]
   @roleID			int,
   @roleCode		varchar(50),
   @roleName		nvarchar(100)

AS

BEGIN

	SET NOCOUNT ON
	-- 检查条件
	IF EXISTS(SELECT 1 
			  FROM SysRole 
			  WHERE ID<>@roleID 
				AND (RoleCode=@roleCode OR RoleName=@roleName)
			  )
		RAISERROR ('插入或编辑角色数据失败: 有重复的名称或者编码已经存在!', 16, 1)

    --插入或者编辑				
	BEGIN TRY
		IF (@roleID>0)
			UPDATE SysRole
			SET RoleCode=@roleCode, RoleName=@roleName
			WHERE ID=@roleID
		ELSE
		    INSERT INTO SysRole(RoleCode, RoleName)
		    VALUES(@roleCode, @roleName)
	END TRY
	BEGIN CATCH
			DECLARE @error int, @message varchar(4000)
			SELECT @error = ERROR_NUMBER()
				, @message = ERROR_MESSAGE();
			RAISERROR ('插入或编辑角色数据失败: %d: %s', 16, 1, @error, @message)
	END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[pr_sys_RoleDelete]    Script Date: 02/28/2025 17:15:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[pr_sys_RoleDelete]
   @roleID			int

AS

BEGIN

	SET NOCOUNT ON
    --删除操作				
	BEGIN TRY
		DELETE FROM SysRoleUser WHERE RoleID=@roleID
		DELETE FROM SysRole WHERE ID=@roleID
	END TRY
	BEGIN CATCH
			DECLARE @error int, @message varchar(4000)
			SELECT @error = ERROR_NUMBER()
				, @message = ERROR_MESSAGE();
			RAISERROR ('删除角色数据失败: %d: %s', 16, 1, @error, @message)
	END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[pr_sys_DeptUserListRankQuery]    Script Date: 02/28/2025 17:15:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[pr_sys_DeptUserListRankQuery]
   @roleIDs				varchar(8000),
   @curUserID			int,
   @receiverType			int

AS

BEGIN
    --ReceiverType= 1 上司
    --ReceiverType= 2 同级
    --ReceiverType= 3 下属
	SET NOCOUNT ON
	
    DECLARE @error int, @message varchar(4000)
    
    --Activity节点需要定义接收者类型，前提也需要定义角色信息
	IF (@receiverType = 0)
		BEGIN
			SELECT @error = ERROR_NUMBER()
				, @message = ERROR_MESSAGE();
			RAISERROR ('无效的接收者类型@receiverType！查询失败: %d: %s', 16, 1, @error, @message)
		END
	ELSE IF (@roleIDs = '')
		BEGIN
			SELECT @error = ERROR_NUMBER()
				, @message = ERROR_MESSAGE();
			RAISERROR ('无效的角色定义@@roleIDs！查询失败: %d: %s', 16, 1, @error, @message)
		END
		
	--ReceiverType=0, throw an error
	DECLARE @tblRoleIDS as TABLE(ID int)
	
	INSERT INTO @tblRoleIDS(ID)
	SELECT ID 
	FROM dbo.fn_com_SplitString(@roleIDs)
	
	BEGIN TRY
		IF (@receiverType = 1)	--上司
			BEGIN
				SELECT 
					U.ID AS UserID,
					U.UserName
				FROM SysUser U
				INNER JOIN SysEmployeeManager EM
					ON U.ID = EM.MgrUserID
				INNER JOIN SysRoleUser RU
				    ON U.ID = RU.UserID
				INNER JOIN @tblRoleIDS R
				    ON R.ID = RU.RoleID
				WHERE EM.EmpUserID = @curUserID
			END
		ELSE IF (@receiverType = 2) --同级
			BEGIN
				SELECT 
					U.ID AS UserID,
					U.UserName
				FROM SysUser U
				INNER JOIN SysEmployeeManager EM
					ON U.ID = EM.EmpUserID
				INNER JOIN SysRoleUser RU
				    ON U.ID = RU.UserID
				INNER JOIN @tblRoleIDS R
				    ON R.ID = RU.RoleID
				WHERE EM.MgrUserID IN
					(
						SELECT 
							MgrUserID
						FROM SysEmployeeManager
						WHERE EmpUserID = @curUserID
					)
			END
		ELSE IF (@receiverType = 3) --下属
			BEGIN
				SELECT 
					U.ID AS UserID,
					U.UserName
				FROM SysUser U
				INNER JOIN SysEmployeeManager EM
					ON U.ID = EM.EmpUserID
				INNER JOIN SysRoleUser RU
				    ON U.ID = RU.UserID
				INNER JOIN @tblRoleIDS R
				    ON R.ID = RU.RoleID
				WHERE EM.MgrUserID = @curUserID
			END
		
	END TRY
	BEGIN CATCH
		SELECT @error = ERROR_NUMBER()
			, @message = ERROR_MESSAGE();
		RAISERROR ('查询员工上司下属关系数据失败: %d: %s', 16, 1, @error, @message)
	END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[pr_fb_FormDelete]    Script Date: 02/28/2025 17:15:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[pr_fb_FormDelete]
   @formID			int

AS

BEGIN

	SET NOCOUNT ON

	BEGIN TRANSACTION
	BEGIN TRY
			
		--1. 删除实体信息主表数据
		DELETE 
		FROM FbFormData
		WHERE FormID = @formID
		
		--3. 删除属性事件数据
		DELETE 
		FROM FbFormFieldEvent
		WHERE FormID = @formID
		
		--4. 删除实体属性表数据
		DELETE
		FROM FbFormField
		WHERE FormID = @formID
		
		--5. 删除定义主表数据
		DELETE
		FROM FbForm
		WHERE ID = @formID
		
		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
			ROLLBACK TRANSACTION

			DECLARE @error int, @message varchar(4000)
			SELECT @error = ERROR_NUMBER()
				, @message = ERROR_MESSAGE();
			RAISERROR ('删除实体及其扩展属性失败: %d: %s', 16, 1, @error, @message)
	END CATCH
END
GO
/****** Object:  Table [dbo].[WfActivityInstance]    Script Date: 02/28/2025 17:15:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[WfActivityInstance](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ProcessInstanceID] [int] NOT NULL,
	[AppName] [nvarchar](50) NOT NULL,
	[AppInstanceID] [varchar](50) NOT NULL,
	[AppInstanceCode] [varchar](50) NULL,
	[ProcessID] [varchar](50) NOT NULL,
	[ActivityID] [varchar](50) NOT NULL,
	[ActivityName] [nvarchar](50) NOT NULL,
	[ActivityCode] [varchar](50) NULL,
	[ActivityType] [smallint] NOT NULL,
	[ActivityState] [smallint] NOT NULL,
	[WorkItemType] [smallint] NOT NULL,
	[AssignedToUserIDs] [nvarchar](1000) NULL,
	[AssignedToUserNames] [nvarchar](2000) NULL,
	[BackwardType] [smallint] NULL,
	[BackSrcActivityInstanceID] [int] NULL,
	[BackOrgActivityInstanceID] [int] NULL,
	[GatewayDirectionTypeID] [smallint] NULL,
	[CanNotRenewInstance] [tinyint] NOT NULL,
	[ApprovalStatus] [smallint] NULL,
	[TokensRequired] [int] NOT NULL,
	[TokensHad] [int] NOT NULL,
	[JobTimerType] [smallint] NULL,
	[JobTimerStatus] [smallint] NULL,
	[TriggerExpression] [nvarchar](200) NULL,
	[OverdueDateTime] [datetime] NULL,
	[JobTimerTreatedDateTime] [datetime] NULL,
	[ComplexType] [smallint] NULL,
	[MergeType] [smallint] NULL,
	[MIHostActivityInstanceID] [int] NULL,
	[CompareType] [smallint] NULL,
	[CompleteOrder] [float] NULL,
	[SignForwardType] [smallint] NULL,
	[NextStepPerformers] [nvarchar](4000) NULL,
	[CreatedByUserID] [varchar](50) NOT NULL,
	[CreatedByUserName] [nvarchar](50) NOT NULL,
	[CreatedDateTime] [datetime] NOT NULL,
	[LastUpdatedByUserID] [varchar](50) NULL,
	[LastUpdatedByUserName] [nvarchar](50) NULL,
	[LastUpdatedDateTime] [datetime] NULL,
	[EndedDateTime] [datetime] NULL,
	[EndedByUserID] [varchar](50) NULL,
	[EndedByUserName] [nvarchar](50) NULL,
	[RecordStatusInvalid] [tinyint] NOT NULL,
	[RowVersionID] [timestamp] NULL,
 CONSTRAINT [PK_WfActivityInstance] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[WfActivityInstance] ON
INSERT [dbo].[WfActivityInstance] ([ID], [ProcessInstanceID], [AppName], [AppInstanceID], [AppInstanceCode], [ProcessID], [ActivityID], [ActivityName], [ActivityCode], [ActivityType], [ActivityState], [WorkItemType], [AssignedToUserIDs], [AssignedToUserNames], [BackwardType], [BackSrcActivityInstanceID], [BackOrgActivityInstanceID], [GatewayDirectionTypeID], [CanNotRenewInstance], [ApprovalStatus], [TokensRequired], [TokensHad], [JobTimerType], [JobTimerStatus], [TriggerExpression], [OverdueDateTime], [JobTimerTreatedDateTime], [ComplexType], [MergeType], [MIHostActivityInstanceID], [CompareType], [CompleteOrder], [SignForwardType], [NextStepPerformers], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [LastUpdatedDateTime], [EndedDateTime], [EndedByUserID], [EndedByUserName], [RecordStatusInvalid]) VALUES (1, 1, N'Order-Books', N'123', N'123-code', N'Process_o5uf_2550', N'StartNode_4475', N'Start', N'Start', 1, 4, 0, NULL, NULL, 0, NULL, NULL, NULL, 0, 0, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'01', N'Zero', CAST(0x0000B28C00B521A9 AS DateTime), NULL, NULL, NULL, CAST(0x0000B28C00B521B6 AS DateTime), N'01', N'Zero', 0)
INSERT [dbo].[WfActivityInstance] ([ID], [ProcessInstanceID], [AppName], [AppInstanceID], [AppInstanceCode], [ProcessID], [ActivityID], [ActivityName], [ActivityCode], [ActivityType], [ActivityState], [WorkItemType], [AssignedToUserIDs], [AssignedToUserNames], [BackwardType], [BackSrcActivityInstanceID], [BackOrgActivityInstanceID], [GatewayDirectionTypeID], [CanNotRenewInstance], [ApprovalStatus], [TokensRequired], [TokensHad], [JobTimerType], [JobTimerStatus], [TriggerExpression], [OverdueDateTime], [JobTimerTreatedDateTime], [ComplexType], [MergeType], [MIHostActivityInstanceID], [CompareType], [CompleteOrder], [SignForwardType], [NextStepPerformers], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [LastUpdatedDateTime], [EndedDateTime], [EndedByUserID], [EndedByUserName], [RecordStatusInvalid]) VALUES (2, 1, N'Order-Books', N'123', N'123-code', N'Process_o5uf_2550', N'TaskNode_5417', N'Task-001', N'task001', 4, 4, 1, N'01', N'Zero', 0, NULL, NULL, NULL, 0, 0, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'01', N'Zero', CAST(0x0000B28C00B521BD AS DateTime), NULL, NULL, NULL, CAST(0x0000B28C00B5284D AS DateTime), N'01', N'Zero', 0)
INSERT [dbo].[WfActivityInstance] ([ID], [ProcessInstanceID], [AppName], [AppInstanceID], [AppInstanceCode], [ProcessID], [ActivityID], [ActivityName], [ActivityCode], [ActivityType], [ActivityState], [WorkItemType], [AssignedToUserIDs], [AssignedToUserNames], [BackwardType], [BackSrcActivityInstanceID], [BackOrgActivityInstanceID], [GatewayDirectionTypeID], [CanNotRenewInstance], [ApprovalStatus], [TokensRequired], [TokensHad], [JobTimerType], [JobTimerStatus], [TriggerExpression], [OverdueDateTime], [JobTimerTreatedDateTime], [ComplexType], [MergeType], [MIHostActivityInstanceID], [CompareType], [CompleteOrder], [SignForwardType], [NextStepPerformers], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [LastUpdatedDateTime], [EndedDateTime], [EndedByUserID], [EndedByUserName], [RecordStatusInvalid]) VALUES (3, 1, N'Order-Books', N'123', N'123-code', N'Process_o5uf_2550', N'TaskNode_5679', N'Task-002', N'task002', 4, 4, 1, N'171,101,221', N'Queen(模拟),Jenny(模拟),Vectoria(模拟)', 0, NULL, NULL, NULL, 0, 0, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'01', N'Zero', CAST(0x0000B28C00B5284F AS DateTime), NULL, NULL, NULL, CAST(0x0000B28C00B52DDF AS DateTime), N'101', N'Jenny(模拟)', 0)
INSERT [dbo].[WfActivityInstance] ([ID], [ProcessInstanceID], [AppName], [AppInstanceID], [AppInstanceCode], [ProcessID], [ActivityID], [ActivityName], [ActivityCode], [ActivityType], [ActivityState], [WorkItemType], [AssignedToUserIDs], [AssignedToUserNames], [BackwardType], [BackSrcActivityInstanceID], [BackOrgActivityInstanceID], [GatewayDirectionTypeID], [CanNotRenewInstance], [ApprovalStatus], [TokensRequired], [TokensHad], [JobTimerType], [JobTimerStatus], [TriggerExpression], [OverdueDateTime], [JobTimerTreatedDateTime], [ComplexType], [MergeType], [MIHostActivityInstanceID], [CompareType], [CompleteOrder], [SignForwardType], [NextStepPerformers], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [LastUpdatedDateTime], [EndedDateTime], [EndedByUserID], [EndedByUserName], [RecordStatusInvalid]) VALUES (4, 1, N'Order-Books', N'123', N'123-code', N'Process_o5uf_2550', N'TaskNode_6468', N'Task-003', N'task003', 4, 4, 1, N'220,201,100', N'Venica(模拟),Terrisa(模拟),Jack(模拟)', 0, NULL, NULL, NULL, 0, 0, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'101', N'Jenny(模拟)', CAST(0x0000B28C00B52DDF AS DateTime), NULL, NULL, NULL, CAST(0x0000B28C00B53329 AS DateTime), N'201', N'Terrisa(模拟)', 0)
INSERT [dbo].[WfActivityInstance] ([ID], [ProcessInstanceID], [AppName], [AppInstanceID], [AppInstanceCode], [ProcessID], [ActivityID], [ActivityName], [ActivityCode], [ActivityType], [ActivityState], [WorkItemType], [AssignedToUserIDs], [AssignedToUserNames], [BackwardType], [BackSrcActivityInstanceID], [BackOrgActivityInstanceID], [GatewayDirectionTypeID], [CanNotRenewInstance], [ApprovalStatus], [TokensRequired], [TokensHad], [JobTimerType], [JobTimerStatus], [TriggerExpression], [OverdueDateTime], [JobTimerTreatedDateTime], [ComplexType], [MergeType], [MIHostActivityInstanceID], [CompareType], [CompleteOrder], [SignForwardType], [NextStepPerformers], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [LastUpdatedDateTime], [EndedDateTime], [EndedByUserID], [EndedByUserName], [RecordStatusInvalid]) VALUES (5, 1, N'Order-Books', N'123', N'123-code', N'Process_o5uf_2550', N'EndNode_3185', N'End', N'End', 2, 4, 0, NULL, NULL, 0, NULL, NULL, NULL, 0, 0, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'201', N'Terrisa(模拟)', CAST(0x0000B28C00B5332A AS DateTime), NULL, NULL, NULL, CAST(0x0000B28C00B5332B AS DateTime), N'201', N'Terrisa(模拟)', 0)
INSERT [dbo].[WfActivityInstance] ([ID], [ProcessInstanceID], [AppName], [AppInstanceID], [AppInstanceCode], [ProcessID], [ActivityID], [ActivityName], [ActivityCode], [ActivityType], [ActivityState], [WorkItemType], [AssignedToUserIDs], [AssignedToUserNames], [BackwardType], [BackSrcActivityInstanceID], [BackOrgActivityInstanceID], [GatewayDirectionTypeID], [CanNotRenewInstance], [ApprovalStatus], [TokensRequired], [TokensHad], [JobTimerType], [JobTimerStatus], [TriggerExpression], [OverdueDateTime], [JobTimerTreatedDateTime], [ComplexType], [MergeType], [MIHostActivityInstanceID], [CompareType], [CompleteOrder], [SignForwardType], [NextStepPerformers], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [LastUpdatedDateTime], [EndedDateTime], [EndedByUserID], [EndedByUserName], [RecordStatusInvalid]) VALUES (6, 2, N'Order-Books', N'123', N'123-code', N'Process_cswy_5517', N'StartNode_6553', N'start', N'Start', 1, 4, 0, NULL, NULL, 0, NULL, NULL, NULL, 0, 0, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'01', N'Zero', CAST(0x0000B28D00099891 AS DateTime), NULL, NULL, NULL, CAST(0x0000B28D0009989C AS DateTime), N'01', N'Zero', 0)
INSERT [dbo].[WfActivityInstance] ([ID], [ProcessInstanceID], [AppName], [AppInstanceID], [AppInstanceCode], [ProcessID], [ActivityID], [ActivityName], [ActivityCode], [ActivityType], [ActivityState], [WorkItemType], [AssignedToUserIDs], [AssignedToUserNames], [BackwardType], [BackSrcActivityInstanceID], [BackOrgActivityInstanceID], [GatewayDirectionTypeID], [CanNotRenewInstance], [ApprovalStatus], [TokensRequired], [TokensHad], [JobTimerType], [JobTimerStatus], [TriggerExpression], [OverdueDateTime], [JobTimerTreatedDateTime], [ComplexType], [MergeType], [MIHostActivityInstanceID], [CompareType], [CompleteOrder], [SignForwardType], [NextStepPerformers], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [LastUpdatedDateTime], [EndedDateTime], [EndedByUserID], [EndedByUserName], [RecordStatusInvalid]) VALUES (7, 2, N'Order-Books', N'123', N'123-code', N'Process_cswy_5517', N'TaskNode_6976', N'Task-001', N'task001', 4, 4, 1, N'01', N'Zero', 0, NULL, NULL, NULL, 0, 0, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'01', N'Zero', CAST(0x0000B28D000998A0 AS DateTime), NULL, NULL, NULL, CAST(0x0000B28D0009AD13 AS DateTime), N'01', N'Zero', 0)
INSERT [dbo].[WfActivityInstance] ([ID], [ProcessInstanceID], [AppName], [AppInstanceID], [AppInstanceCode], [ProcessID], [ActivityID], [ActivityName], [ActivityCode], [ActivityType], [ActivityState], [WorkItemType], [AssignedToUserIDs], [AssignedToUserNames], [BackwardType], [BackSrcActivityInstanceID], [BackOrgActivityInstanceID], [GatewayDirectionTypeID], [CanNotRenewInstance], [ApprovalStatus], [TokensRequired], [TokensHad], [JobTimerType], [JobTimerStatus], [TriggerExpression], [OverdueDateTime], [JobTimerTreatedDateTime], [ComplexType], [MergeType], [MIHostActivityInstanceID], [CompareType], [CompleteOrder], [SignForwardType], [NextStepPerformers], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [LastUpdatedDateTime], [EndedDateTime], [EndedByUserID], [EndedByUserName], [RecordStatusInvalid]) VALUES (8, 2, N'Order-Books', N'123', N'123-code', N'Process_cswy_5517', N'GatewayNode_1841', N'or-split', N'orsplit001', 8, 4, 0, NULL, NULL, 0, NULL, NULL, 1, 0, 0, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'01', N'Zero', CAST(0x0000B28D0009AD16 AS DateTime), NULL, NULL, NULL, CAST(0x0000B28D0009AD17 AS DateTime), N'01', N'Zero', 0)
INSERT [dbo].[WfActivityInstance] ([ID], [ProcessInstanceID], [AppName], [AppInstanceID], [AppInstanceCode], [ProcessID], [ActivityID], [ActivityName], [ActivityCode], [ActivityType], [ActivityState], [WorkItemType], [AssignedToUserIDs], [AssignedToUserNames], [BackwardType], [BackSrcActivityInstanceID], [BackOrgActivityInstanceID], [GatewayDirectionTypeID], [CanNotRenewInstance], [ApprovalStatus], [TokensRequired], [TokensHad], [JobTimerType], [JobTimerStatus], [TriggerExpression], [OverdueDateTime], [JobTimerTreatedDateTime], [ComplexType], [MergeType], [MIHostActivityInstanceID], [CompareType], [CompleteOrder], [SignForwardType], [NextStepPerformers], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [LastUpdatedDateTime], [EndedDateTime], [EndedByUserID], [EndedByUserName], [RecordStatusInvalid]) VALUES (9, 2, N'Order-Books', N'123', N'123-code', N'Process_cswy_5517', N'TaskNode_9595', N'task-020', N'task020', 4, 7, 1, N'120', N'Lary(模拟)', 0, NULL, NULL, NULL, 0, 0, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'01', N'Zero', CAST(0x0000B28D0009AD1B AS DateTime), NULL, NULL, NULL, CAST(0x0000B28D0009BA14 AS DateTime), N'120', N'Lary(模拟)', 0)
INSERT [dbo].[WfActivityInstance] ([ID], [ProcessInstanceID], [AppName], [AppInstanceID], [AppInstanceCode], [ProcessID], [ActivityID], [ActivityName], [ActivityCode], [ActivityType], [ActivityState], [WorkItemType], [AssignedToUserIDs], [AssignedToUserNames], [BackwardType], [BackSrcActivityInstanceID], [BackOrgActivityInstanceID], [GatewayDirectionTypeID], [CanNotRenewInstance], [ApprovalStatus], [TokensRequired], [TokensHad], [JobTimerType], [JobTimerStatus], [TriggerExpression], [OverdueDateTime], [JobTimerTreatedDateTime], [ComplexType], [MergeType], [MIHostActivityInstanceID], [CompareType], [CompleteOrder], [SignForwardType], [NextStepPerformers], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [LastUpdatedDateTime], [EndedDateTime], [EndedByUserID], [EndedByUserName], [RecordStatusInvalid]) VALUES (10, 2, N'Order-Books', N'123', N'123-code', N'Process_cswy_5517', N'TaskNode_6976', N'Task-001', N'task001', 4, 1, 1, N'01', N'Zero', 1, 9, 7, NULL, 0, 0, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'120', N'Lary(模拟)', CAST(0x0000B28D0009BA12 AS DateTime), NULL, NULL, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[WfActivityInstance] ([ID], [ProcessInstanceID], [AppName], [AppInstanceID], [AppInstanceCode], [ProcessID], [ActivityID], [ActivityName], [ActivityCode], [ActivityType], [ActivityState], [WorkItemType], [AssignedToUserIDs], [AssignedToUserNames], [BackwardType], [BackSrcActivityInstanceID], [BackOrgActivityInstanceID], [GatewayDirectionTypeID], [CanNotRenewInstance], [ApprovalStatus], [TokensRequired], [TokensHad], [JobTimerType], [JobTimerStatus], [TriggerExpression], [OverdueDateTime], [JobTimerTreatedDateTime], [ComplexType], [MergeType], [MIHostActivityInstanceID], [CompareType], [CompleteOrder], [SignForwardType], [NextStepPerformers], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [LastUpdatedDateTime], [EndedDateTime], [EndedByUserID], [EndedByUserName], [RecordStatusInvalid]) VALUES (11, 3, N'Order-Books', N'123', N'123-code', N'employeeLeaveRequestProcess_hwpb', N'AstartEvent', N'流程开始', NULL, 1, 4, 0, NULL, NULL, 0, NULL, NULL, NULL, 0, 0, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'01', N'Zero', CAST(0x0000B28D0009D142 AS DateTime), NULL, NULL, NULL, CAST(0x0000B28D0009D143 AS DateTime), N'01', N'Zero', 0)
INSERT [dbo].[WfActivityInstance] ([ID], [ProcessInstanceID], [AppName], [AppInstanceID], [AppInstanceCode], [ProcessID], [ActivityID], [ActivityName], [ActivityCode], [ActivityType], [ActivityState], [WorkItemType], [AssignedToUserIDs], [AssignedToUserNames], [BackwardType], [BackSrcActivityInstanceID], [BackOrgActivityInstanceID], [GatewayDirectionTypeID], [CanNotRenewInstance], [ApprovalStatus], [TokensRequired], [TokensHad], [JobTimerType], [JobTimerStatus], [TriggerExpression], [OverdueDateTime], [JobTimerTreatedDateTime], [ComplexType], [MergeType], [MIHostActivityInstanceID], [CompareType], [CompleteOrder], [SignForwardType], [NextStepPerformers], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [LastUpdatedDateTime], [EndedDateTime], [EndedByUserID], [EndedByUserName], [RecordStatusInvalid]) VALUES (12, 3, N'Order-Books', N'123', N'123-code', N'employeeLeaveRequestProcess_hwpb', N'AsubmitApplication', N'提交请假申请', NULL, 4, 4, 1, N'01', N'Zero', 0, NULL, NULL, NULL, 0, 1, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'01', N'Zero', CAST(0x0000B28D0009D143 AS DateTime), NULL, NULL, NULL, CAST(0x0000B28D0009F8FE AS DateTime), N'01', N'Zero', 0)
INSERT [dbo].[WfActivityInstance] ([ID], [ProcessInstanceID], [AppName], [AppInstanceID], [AppInstanceCode], [ProcessID], [ActivityID], [ActivityName], [ActivityCode], [ActivityType], [ActivityState], [WorkItemType], [AssignedToUserIDs], [AssignedToUserNames], [BackwardType], [BackSrcActivityInstanceID], [BackOrgActivityInstanceID], [GatewayDirectionTypeID], [CanNotRenewInstance], [ApprovalStatus], [TokensRequired], [TokensHad], [JobTimerType], [JobTimerStatus], [TriggerExpression], [OverdueDateTime], [JobTimerTreatedDateTime], [ComplexType], [MergeType], [MIHostActivityInstanceID], [CompareType], [CompleteOrder], [SignForwardType], [NextStepPerformers], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [LastUpdatedDateTime], [EndedDateTime], [EndedByUserID], [EndedByUserName], [RecordStatusInvalid]) VALUES (13, 3, N'Order-Books', N'123', N'123-code', N'employeeLeaveRequestProcess_hwpb', N'AendEvent', N'流程结束', NULL, 2, 4, 0, NULL, NULL, 0, NULL, NULL, NULL, 0, 0, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'01', N'Zero', CAST(0x0000B28D0009F8FE AS DateTime), NULL, NULL, NULL, CAST(0x0000B28D0009F8FF AS DateTime), N'01', N'Zero', 0)
INSERT [dbo].[WfActivityInstance] ([ID], [ProcessInstanceID], [AppName], [AppInstanceID], [AppInstanceCode], [ProcessID], [ActivityID], [ActivityName], [ActivityCode], [ActivityType], [ActivityState], [WorkItemType], [AssignedToUserIDs], [AssignedToUserNames], [BackwardType], [BackSrcActivityInstanceID], [BackOrgActivityInstanceID], [GatewayDirectionTypeID], [CanNotRenewInstance], [ApprovalStatus], [TokensRequired], [TokensHad], [JobTimerType], [JobTimerStatus], [TriggerExpression], [OverdueDateTime], [JobTimerTreatedDateTime], [ComplexType], [MergeType], [MIHostActivityInstanceID], [CompareType], [CompleteOrder], [SignForwardType], [NextStepPerformers], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [LastUpdatedDateTime], [EndedDateTime], [EndedByUserID], [EndedByUserName], [RecordStatusInvalid]) VALUES (14, 4, N'Order-Books', N'123', N'123-code', N'Process_o5uf_2550', N'StartNode_4475', N'Start', N'Start', 1, 4, 0, NULL, NULL, 0, NULL, NULL, NULL, 0, 0, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'01', N'Zero', CAST(0x0000B28D00CD8CE3 AS DateTime), NULL, NULL, NULL, CAST(0x0000B28D00CD8CEE AS DateTime), N'01', N'Zero', 0)
INSERT [dbo].[WfActivityInstance] ([ID], [ProcessInstanceID], [AppName], [AppInstanceID], [AppInstanceCode], [ProcessID], [ActivityID], [ActivityName], [ActivityCode], [ActivityType], [ActivityState], [WorkItemType], [AssignedToUserIDs], [AssignedToUserNames], [BackwardType], [BackSrcActivityInstanceID], [BackOrgActivityInstanceID], [GatewayDirectionTypeID], [CanNotRenewInstance], [ApprovalStatus], [TokensRequired], [TokensHad], [JobTimerType], [JobTimerStatus], [TriggerExpression], [OverdueDateTime], [JobTimerTreatedDateTime], [ComplexType], [MergeType], [MIHostActivityInstanceID], [CompareType], [CompleteOrder], [SignForwardType], [NextStepPerformers], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [LastUpdatedDateTime], [EndedDateTime], [EndedByUserID], [EndedByUserName], [RecordStatusInvalid]) VALUES (15, 4, N'Order-Books', N'123', N'123-code', N'Process_o5uf_2550', N'TaskNode_5417', N'Task-001', N'task001', 4, 4, 1, N'01', N'Zero', 0, NULL, NULL, NULL, 0, 0, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'01', N'Zero', CAST(0x0000B28D00CD8CF5 AS DateTime), NULL, NULL, NULL, CAST(0x0000B28D00CD9382 AS DateTime), N'01', N'Zero', 0)
INSERT [dbo].[WfActivityInstance] ([ID], [ProcessInstanceID], [AppName], [AppInstanceID], [AppInstanceCode], [ProcessID], [ActivityID], [ActivityName], [ActivityCode], [ActivityType], [ActivityState], [WorkItemType], [AssignedToUserIDs], [AssignedToUserNames], [BackwardType], [BackSrcActivityInstanceID], [BackOrgActivityInstanceID], [GatewayDirectionTypeID], [CanNotRenewInstance], [ApprovalStatus], [TokensRequired], [TokensHad], [JobTimerType], [JobTimerStatus], [TriggerExpression], [OverdueDateTime], [JobTimerTreatedDateTime], [ComplexType], [MergeType], [MIHostActivityInstanceID], [CompareType], [CompleteOrder], [SignForwardType], [NextStepPerformers], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [LastUpdatedDateTime], [EndedDateTime], [EndedByUserID], [EndedByUserName], [RecordStatusInvalid]) VALUES (16, 4, N'Order-Books', N'123', N'123-code', N'Process_o5uf_2550', N'TaskNode_5679', N'Task-002', N'task002', 4, 4, 1, N'241,130,40', N'Xiusey(模拟),Monica(模拟),Daney(模拟)', 0, NULL, NULL, NULL, 0, 0, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'01', N'Zero', CAST(0x0000B28D00CD9383 AS DateTime), NULL, NULL, NULL, CAST(0x0000B28D00CD9998 AS DateTime), N'130', N'Monica(模拟)', 0)
INSERT [dbo].[WfActivityInstance] ([ID], [ProcessInstanceID], [AppName], [AppInstanceID], [AppInstanceCode], [ProcessID], [ActivityID], [ActivityName], [ActivityCode], [ActivityType], [ActivityState], [WorkItemType], [AssignedToUserIDs], [AssignedToUserNames], [BackwardType], [BackSrcActivityInstanceID], [BackOrgActivityInstanceID], [GatewayDirectionTypeID], [CanNotRenewInstance], [ApprovalStatus], [TokensRequired], [TokensHad], [JobTimerType], [JobTimerStatus], [TriggerExpression], [OverdueDateTime], [JobTimerTreatedDateTime], [ComplexType], [MergeType], [MIHostActivityInstanceID], [CompareType], [CompleteOrder], [SignForwardType], [NextStepPerformers], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [LastUpdatedDateTime], [EndedDateTime], [EndedByUserID], [EndedByUserName], [RecordStatusInvalid]) VALUES (17, 4, N'Order-Books', N'123', N'123-code', N'Process_o5uf_2550', N'TaskNode_6468', N'Task-003', N'task003', 4, 4, 1, N'31,11,250', N'Cindy(模拟),Andrew(模拟),Yonga(模拟)', 0, NULL, NULL, NULL, 0, 0, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'130', N'Monica(模拟)', CAST(0x0000B28D00CD99EC AS DateTime), NULL, NULL, NULL, CAST(0x0000B28D00CD9FD6 AS DateTime), N'31', N'Cindy(模拟)', 0)
INSERT [dbo].[WfActivityInstance] ([ID], [ProcessInstanceID], [AppName], [AppInstanceID], [AppInstanceCode], [ProcessID], [ActivityID], [ActivityName], [ActivityCode], [ActivityType], [ActivityState], [WorkItemType], [AssignedToUserIDs], [AssignedToUserNames], [BackwardType], [BackSrcActivityInstanceID], [BackOrgActivityInstanceID], [GatewayDirectionTypeID], [CanNotRenewInstance], [ApprovalStatus], [TokensRequired], [TokensHad], [JobTimerType], [JobTimerStatus], [TriggerExpression], [OverdueDateTime], [JobTimerTreatedDateTime], [ComplexType], [MergeType], [MIHostActivityInstanceID], [CompareType], [CompleteOrder], [SignForwardType], [NextStepPerformers], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [LastUpdatedDateTime], [EndedDateTime], [EndedByUserID], [EndedByUserName], [RecordStatusInvalid]) VALUES (18, 4, N'Order-Books', N'123', N'123-code', N'Process_o5uf_2550', N'EndNode_3185', N'End', N'End', 2, 4, 0, NULL, NULL, 0, NULL, NULL, NULL, 0, 0, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'31', N'Cindy(模拟)', CAST(0x0000B28D00CD9FD6 AS DateTime), NULL, NULL, NULL, CAST(0x0000B28D00CD9FD7 AS DateTime), N'31', N'Cindy(模拟)', 0)
SET IDENTITY_INSERT [dbo].[WfActivityInstance] OFF
/****** Object:  View [dbo].[vw_SysRoleUserView]    Script Date: 02/28/2025 17:15:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_SysRoleUserView]
AS
SELECT  dbo.SysRoleUser.ID,
    dbo.SysRole.ID as RoleID, 
	dbo.SysRole.RoleName, 
	dbo.SysRole.RoleCode, 
	dbo.SysUser.ID as UserID,
	dbo.SysUser.UserName
FROM         dbo.SysRole LEFT JOIN
             dbo.SysRoleUser ON dbo.SysRole.ID = dbo.SysRoleUser.RoleID LEFT JOIN
             dbo.SysUser ON dbo.SysRoleUser.UserID = dbo.SysUser.ID
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[41] 4[24] 2[17] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "SysRole"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 110
               Right = 180
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "SysRoleUser"
            Begin Extent = 
               Top = 4
               Left = 313
               Bottom = 108
               Right = 455
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "SysUser"
            Begin Extent = 
               Top = 165
               Left = 175
               Bottom = 254
               Right = 317
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_SysRoleUserView'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_SysRoleUserView'
GO
/****** Object:  View [dbo].[vw_FbFormDataView]    Script Date: 02/28/2025 17:15:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_FbFormDataView]
AS
SELECT     dbo.FbFormData.ID, dbo.FbFormData.FormID, dbo.FbForm.FormName, dbo.FbForm.FormCode, dbo.FbForm.Version, dbo.FbFormData.CreatedUserID, dbo.FbFormData.CreatedUserName, 
                      dbo.FbFormData.CreatedDate, dbo.FbFormData.LastUpdatedUserID, dbo.FbFormData.LastUpdatedUserName, dbo.FbFormData.LastUpdatedDate
FROM         dbo.FbForm INNER JOIN
                      dbo.FbFormData ON dbo.FbForm.ID = dbo.FbFormData.FormID
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "FbForm"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 126
               Right = 211
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "FbFormData"
            Begin Extent = 
               Top = 6
               Left = 249
               Bottom = 126
               Right = 448
            End
            DisplayFlags = 280
            TopColumn = 6
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_FbFormDataView'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_FbFormDataView'
GO
/****** Object:  Table [dbo].[WfTasks]    Script Date: 02/28/2025 17:15:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[WfTasks](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ActivityInstanceID] [int] NOT NULL,
	[ProcessInstanceID] [int] NOT NULL,
	[AppName] [nvarchar](50) NOT NULL,
	[AppInstanceID] [varchar](50) NOT NULL,
	[ProcessID] [varchar](50) NOT NULL,
	[ActivityID] [varchar](50) NOT NULL,
	[ActivityName] [nvarchar](50) NOT NULL,
	[TaskType] [smallint] NOT NULL,
	[TaskState] [smallint] NOT NULL,
	[EntrustedTaskID] [int] NULL,
	[AssignedToUserID] [varchar](50) NOT NULL,
	[AssignedToUserName] [nvarchar](50) NOT NULL,
	[IsEMailSent] [tinyint] NOT NULL,
	[CreatedByUserID] [varchar](50) NOT NULL,
	[CreatedByUserName] [nvarchar](50) NOT NULL,
	[CreatedDateTime] [datetime] NOT NULL,
	[LastUpdatedDateTime] [datetime] NULL,
	[LastUpdatedByUserID] [varchar](50) NULL,
	[LastUpdatedByUserName] [nvarchar](50) NULL,
	[EndedByUserID] [varchar](50) NULL,
	[EndedByUserName] [nvarchar](50) NULL,
	[EndedDateTime] [datetime] NULL,
	[RecordStatusInvalid] [tinyint] NOT NULL,
	[RowVersionID] [timestamp] NULL,
 CONSTRAINT [PK_SSIP_WfTasks] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[WfTasks] ON
INSERT [dbo].[WfTasks] ([ID], [ActivityInstanceID], [ProcessInstanceID], [AppName], [AppInstanceID], [ProcessID], [ActivityID], [ActivityName], [TaskType], [TaskState], [EntrustedTaskID], [AssignedToUserID], [AssignedToUserName], [IsEMailSent], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [EndedByUserID], [EndedByUserName], [EndedDateTime], [RecordStatusInvalid]) VALUES (1, 2, 1, N'Order-Books', N'123', N'Process_o5uf_2550', N'TaskNode_5417', N'Task-001', 1, 4, NULL, N'01', N'Zero', 0, N'01', N'Zero', CAST(0x0000B28C00B521BE AS DateTime), NULL, NULL, NULL, N'01', N'Zero', CAST(0x0000B28C00B5284A AS DateTime), 0)
INSERT [dbo].[WfTasks] ([ID], [ActivityInstanceID], [ProcessInstanceID], [AppName], [AppInstanceID], [ProcessID], [ActivityID], [ActivityName], [TaskType], [TaskState], [EntrustedTaskID], [AssignedToUserID], [AssignedToUserName], [IsEMailSent], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [EndedByUserID], [EndedByUserName], [EndedDateTime], [RecordStatusInvalid]) VALUES (2, 3, 1, N'Order-Books', N'123', N'Process_o5uf_2550', N'TaskNode_5679', N'Task-002', 1, 1, NULL, N'171', N'Queen(模拟)', 0, N'01', N'Zero', CAST(0x0000B28C00B5284F AS DateTime), NULL, NULL, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[WfTasks] ([ID], [ActivityInstanceID], [ProcessInstanceID], [AppName], [AppInstanceID], [ProcessID], [ActivityID], [ActivityName], [TaskType], [TaskState], [EntrustedTaskID], [AssignedToUserID], [AssignedToUserName], [IsEMailSent], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [EndedByUserID], [EndedByUserName], [EndedDateTime], [RecordStatusInvalid]) VALUES (3, 3, 1, N'Order-Books', N'123', N'Process_o5uf_2550', N'TaskNode_5679', N'Task-002', 1, 4, NULL, N'101', N'Jenny(模拟)', 0, N'01', N'Zero', CAST(0x0000B28C00B52850 AS DateTime), NULL, NULL, NULL, N'101', N'Jenny(模拟)', CAST(0x0000B28C00B52DDE AS DateTime), 0)
INSERT [dbo].[WfTasks] ([ID], [ActivityInstanceID], [ProcessInstanceID], [AppName], [AppInstanceID], [ProcessID], [ActivityID], [ActivityName], [TaskType], [TaskState], [EntrustedTaskID], [AssignedToUserID], [AssignedToUserName], [IsEMailSent], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [EndedByUserID], [EndedByUserName], [EndedDateTime], [RecordStatusInvalid]) VALUES (4, 3, 1, N'Order-Books', N'123', N'Process_o5uf_2550', N'TaskNode_5679', N'Task-002', 1, 1, NULL, N'221', N'Vectoria(模拟)', 0, N'01', N'Zero', CAST(0x0000B28C00B52850 AS DateTime), NULL, NULL, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[WfTasks] ([ID], [ActivityInstanceID], [ProcessInstanceID], [AppName], [AppInstanceID], [ProcessID], [ActivityID], [ActivityName], [TaskType], [TaskState], [EntrustedTaskID], [AssignedToUserID], [AssignedToUserName], [IsEMailSent], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [EndedByUserID], [EndedByUserName], [EndedDateTime], [RecordStatusInvalid]) VALUES (5, 4, 1, N'Order-Books', N'123', N'Process_o5uf_2550', N'TaskNode_6468', N'Task-003', 1, 1, NULL, N'220', N'Venica(模拟)', 0, N'101', N'Jenny(模拟)', CAST(0x0000B28C00B52DE0 AS DateTime), NULL, NULL, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[WfTasks] ([ID], [ActivityInstanceID], [ProcessInstanceID], [AppName], [AppInstanceID], [ProcessID], [ActivityID], [ActivityName], [TaskType], [TaskState], [EntrustedTaskID], [AssignedToUserID], [AssignedToUserName], [IsEMailSent], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [EndedByUserID], [EndedByUserName], [EndedDateTime], [RecordStatusInvalid]) VALUES (6, 4, 1, N'Order-Books', N'123', N'Process_o5uf_2550', N'TaskNode_6468', N'Task-003', 1, 4, NULL, N'201', N'Terrisa(模拟)', 0, N'101', N'Jenny(模拟)', CAST(0x0000B28C00B52DE0 AS DateTime), NULL, NULL, NULL, N'201', N'Terrisa(模拟)', CAST(0x0000B28C00B53329 AS DateTime), 0)
INSERT [dbo].[WfTasks] ([ID], [ActivityInstanceID], [ProcessInstanceID], [AppName], [AppInstanceID], [ProcessID], [ActivityID], [ActivityName], [TaskType], [TaskState], [EntrustedTaskID], [AssignedToUserID], [AssignedToUserName], [IsEMailSent], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [EndedByUserID], [EndedByUserName], [EndedDateTime], [RecordStatusInvalid]) VALUES (7, 4, 1, N'Order-Books', N'123', N'Process_o5uf_2550', N'TaskNode_6468', N'Task-003', 1, 1, NULL, N'100', N'Jack(模拟)', 0, N'101', N'Jenny(模拟)', CAST(0x0000B28C00B52DE0 AS DateTime), NULL, NULL, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[WfTasks] ([ID], [ActivityInstanceID], [ProcessInstanceID], [AppName], [AppInstanceID], [ProcessID], [ActivityID], [ActivityName], [TaskType], [TaskState], [EntrustedTaskID], [AssignedToUserID], [AssignedToUserName], [IsEMailSent], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [EndedByUserID], [EndedByUserName], [EndedDateTime], [RecordStatusInvalid]) VALUES (8, 7, 2, N'Order-Books', N'123', N'Process_cswy_5517', N'TaskNode_6976', N'Task-001', 1, 4, NULL, N'01', N'Zero', 0, N'01', N'Zero', CAST(0x0000B28D000998A1 AS DateTime), NULL, NULL, NULL, N'01', N'Zero', CAST(0x0000B28D0009AD12 AS DateTime), 0)
INSERT [dbo].[WfTasks] ([ID], [ActivityInstanceID], [ProcessInstanceID], [AppName], [AppInstanceID], [ProcessID], [ActivityID], [ActivityName], [TaskType], [TaskState], [EntrustedTaskID], [AssignedToUserID], [AssignedToUserName], [IsEMailSent], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [EndedByUserID], [EndedByUserName], [EndedDateTime], [RecordStatusInvalid]) VALUES (9, 9, 2, N'Order-Books', N'123', N'Process_cswy_5517', N'TaskNode_9595', N'task-020', 1, 9, NULL, N'120', N'Lary(模拟)', 0, N'01', N'Zero', CAST(0x0000B28D0009AD1C AS DateTime), NULL, NULL, NULL, N'120', N'Lary(模拟)', CAST(0x0000B28D0009BA13 AS DateTime), 0)
INSERT [dbo].[WfTasks] ([ID], [ActivityInstanceID], [ProcessInstanceID], [AppName], [AppInstanceID], [ProcessID], [ActivityID], [ActivityName], [TaskType], [TaskState], [EntrustedTaskID], [AssignedToUserID], [AssignedToUserName], [IsEMailSent], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [EndedByUserID], [EndedByUserName], [EndedDateTime], [RecordStatusInvalid]) VALUES (10, 10, 2, N'Order-Books', N'123', N'Process_cswy_5517', N'TaskNode_6976', N'Task-001', 1, 1, NULL, N'01', N'Zero', 0, N'120', N'Lary(模拟)', CAST(0x0000B28D0009BA12 AS DateTime), NULL, NULL, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[WfTasks] ([ID], [ActivityInstanceID], [ProcessInstanceID], [AppName], [AppInstanceID], [ProcessID], [ActivityID], [ActivityName], [TaskType], [TaskState], [EntrustedTaskID], [AssignedToUserID], [AssignedToUserName], [IsEMailSent], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [EndedByUserID], [EndedByUserName], [EndedDateTime], [RecordStatusInvalid]) VALUES (11, 12, 3, N'Order-Books', N'123', N'employeeLeaveRequestProcess_hwpb', N'AsubmitApplication', N'提交请假申请', 1, 1, NULL, N'01', N'Zero', 0, N'01', N'Zero', CAST(0x0000B28D0009D143 AS DateTime), NULL, NULL, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[WfTasks] ([ID], [ActivityInstanceID], [ProcessInstanceID], [AppName], [AppInstanceID], [ProcessID], [ActivityID], [ActivityName], [TaskType], [TaskState], [EntrustedTaskID], [AssignedToUserID], [AssignedToUserName], [IsEMailSent], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [EndedByUserID], [EndedByUserName], [EndedDateTime], [RecordStatusInvalid]) VALUES (12, 15, 4, N'Order-Books', N'123', N'Process_o5uf_2550', N'TaskNode_5417', N'Task-001', 1, 4, NULL, N'01', N'Zero', 0, N'01', N'Zero', CAST(0x0000B28D00CD8CF6 AS DateTime), NULL, NULL, NULL, N'01', N'Zero', CAST(0x0000B28D00CD9380 AS DateTime), 0)
INSERT [dbo].[WfTasks] ([ID], [ActivityInstanceID], [ProcessInstanceID], [AppName], [AppInstanceID], [ProcessID], [ActivityID], [ActivityName], [TaskType], [TaskState], [EntrustedTaskID], [AssignedToUserID], [AssignedToUserName], [IsEMailSent], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [EndedByUserID], [EndedByUserName], [EndedDateTime], [RecordStatusInvalid]) VALUES (13, 16, 4, N'Order-Books', N'123', N'Process_o5uf_2550', N'TaskNode_5679', N'Task-002', 1, 1, NULL, N'241', N'Xiusey(模拟)', 0, N'01', N'Zero', CAST(0x0000B28D00CD9384 AS DateTime), NULL, NULL, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[WfTasks] ([ID], [ActivityInstanceID], [ProcessInstanceID], [AppName], [AppInstanceID], [ProcessID], [ActivityID], [ActivityName], [TaskType], [TaskState], [EntrustedTaskID], [AssignedToUserID], [AssignedToUserName], [IsEMailSent], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [EndedByUserID], [EndedByUserName], [EndedDateTime], [RecordStatusInvalid]) VALUES (14, 16, 4, N'Order-Books', N'123', N'Process_o5uf_2550', N'TaskNode_5679', N'Task-002', 1, 4, NULL, N'130', N'Monica(模拟)', 0, N'01', N'Zero', CAST(0x0000B28D00CD9384 AS DateTime), NULL, NULL, NULL, N'130', N'Monica(模拟)', CAST(0x0000B28D00CD9992 AS DateTime), 0)
INSERT [dbo].[WfTasks] ([ID], [ActivityInstanceID], [ProcessInstanceID], [AppName], [AppInstanceID], [ProcessID], [ActivityID], [ActivityName], [TaskType], [TaskState], [EntrustedTaskID], [AssignedToUserID], [AssignedToUserName], [IsEMailSent], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [EndedByUserID], [EndedByUserName], [EndedDateTime], [RecordStatusInvalid]) VALUES (15, 16, 4, N'Order-Books', N'123', N'Process_o5uf_2550', N'TaskNode_5679', N'Task-002', 1, 1, NULL, N'40', N'Daney(模拟)', 0, N'01', N'Zero', CAST(0x0000B28D00CD9384 AS DateTime), NULL, NULL, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[WfTasks] ([ID], [ActivityInstanceID], [ProcessInstanceID], [AppName], [AppInstanceID], [ProcessID], [ActivityID], [ActivityName], [TaskType], [TaskState], [EntrustedTaskID], [AssignedToUserID], [AssignedToUserName], [IsEMailSent], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [EndedByUserID], [EndedByUserName], [EndedDateTime], [RecordStatusInvalid]) VALUES (16, 17, 4, N'Order-Books', N'123', N'Process_o5uf_2550', N'TaskNode_6468', N'Task-003', 1, 4, NULL, N'31', N'Cindy(模拟)', 0, N'130', N'Monica(模拟)', CAST(0x0000B28D00CD99F3 AS DateTime), NULL, NULL, NULL, N'31', N'Cindy(模拟)', CAST(0x0000B28D00CD9FD5 AS DateTime), 0)
INSERT [dbo].[WfTasks] ([ID], [ActivityInstanceID], [ProcessInstanceID], [AppName], [AppInstanceID], [ProcessID], [ActivityID], [ActivityName], [TaskType], [TaskState], [EntrustedTaskID], [AssignedToUserID], [AssignedToUserName], [IsEMailSent], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [EndedByUserID], [EndedByUserName], [EndedDateTime], [RecordStatusInvalid]) VALUES (17, 17, 4, N'Order-Books', N'123', N'Process_o5uf_2550', N'TaskNode_6468', N'Task-003', 1, 1, NULL, N'11', N'Andrew(模拟)', 0, N'130', N'Monica(模拟)', CAST(0x0000B28D00CD99F4 AS DateTime), NULL, NULL, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[WfTasks] ([ID], [ActivityInstanceID], [ProcessInstanceID], [AppName], [AppInstanceID], [ProcessID], [ActivityID], [ActivityName], [TaskType], [TaskState], [EntrustedTaskID], [AssignedToUserID], [AssignedToUserName], [IsEMailSent], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [EndedByUserID], [EndedByUserName], [EndedDateTime], [RecordStatusInvalid]) VALUES (18, 17, 4, N'Order-Books', N'123', N'Process_o5uf_2550', N'TaskNode_6468', N'Task-003', 1, 1, NULL, N'250', N'Yonga(模拟)', 0, N'130', N'Monica(模拟)', CAST(0x0000B28D00CD99F4 AS DateTime), NULL, NULL, NULL, NULL, NULL, NULL, 0)
SET IDENTITY_INSERT [dbo].[WfTasks] OFF
/****** Object:  View [dbo].[vwWfActivityInstanceTasks]    Script Date: 02/28/2025 17:15:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vwWfActivityInstanceTasks]
AS
SELECT     dbo.WfTasks.ID AS TaskID, dbo.WfActivityInstance.AppName, dbo.WfActivityInstance.AppInstanceID, dbo.WfActivityInstance.ProcessID, dbo.WfProcessInstance.Version, 
                      dbo.WfTasks.ProcessInstanceID, dbo.WfActivityInstance.ActivityID, dbo.WfTasks.ActivityInstanceID, dbo.WfActivityInstance.ActivityName, dbo.WfActivityInstance.ActivityCode, 
                      dbo.WfActivityInstance.ActivityType, dbo.WfActivityInstance.WorkItemType, dbo.WfActivityInstance.BackSrcActivityInstanceID, dbo.WfActivityInstance.CreatedByUserID AS PreviousUserID, 
                      dbo.WfActivityInstance.CreatedByUserName AS PreviousUserName, dbo.WfActivityInstance.CreatedDateTime AS PreviousDateTime, dbo.WfTasks.TaskType, dbo.WfTasks.EntrustedTaskID, 
                      dbo.WfTasks.AssignedToUserID, dbo.WfTasks.AssignedToUserName, dbo.WfTasks.IsEMailSent, dbo.WfTasks.CreatedDateTime, dbo.WfTasks.LastUpdatedDateTime, dbo.WfTasks.EndedDateTime,
                       dbo.WfTasks.EndedByUserID, dbo.WfTasks.EndedByUserName, dbo.WfTasks.TaskState, dbo.WfActivityInstance.ActivityState, dbo.WfTasks.RecordStatusInvalid, 
                      dbo.WfProcessInstance.ProcessState, dbo.WfActivityInstance.ComplexType, dbo.WfActivityInstance.MIHostActivityInstanceID, dbo.WfActivityInstance.ApprovalStatus, 
                      dbo.WfActivityInstance.CompleteOrder, dbo.WfProcessInstance.AppInstanceCode, dbo.WfProcessInstance.ProcessName, dbo.WfProcessInstance.CreatedByUserName, 
                      dbo.WfProcessInstance.CreatedDateTime AS PCreatedDateTime, CASE WHEN MIHostActivityInstanceID IS NULL THEN ActivityState ELSE
                          (SELECT     ActivityState
                            FROM          dbo.WfActivityInstance a WITH (NOLOCK)
                            WHERE      a.ID = dbo.WfActivityInstance.MIHostActivityInstanceID) END AS MiHostState, dbo.WfProcessInstance.SubProcessType, dbo.WfProcessInstance.SubProcessDefID, 
                      dbo.WfProcessInstance.SubProcessID
FROM         dbo.WfActivityInstance WITH (NOLOCK) INNER JOIN
                      dbo.WfTasks WITH (NOLOCK) ON dbo.WfActivityInstance.ID = dbo.WfTasks.ActivityInstanceID INNER JOIN
                      dbo.WfProcessInstance WITH (NOLOCK) ON dbo.WfActivityInstance.ProcessInstanceID = dbo.WfProcessInstance.ID
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[31] 4[51] 2[14] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = -192
         Left = 0
      End
      Begin Tables = 
         Begin Table = "WfActivityInstance"
            Begin Extent = 
               Top = 23
               Left = 415
               Bottom = 142
               Right = 630
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "WfTasks"
            Begin Extent = 
               Top = 126
               Left = 38
               Bottom = 245
               Right = 249
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "WfProcessInstance"
            Begin Extent = 
               Top = 259
               Left = 258
               Bottom = 378
               Right = 475
            End
            DisplayFlags = 280
            TopColumn = 7
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 3930
         Alias = 2145
         Table = 2595
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vwWfActivityInstanceTasks'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vwWfActivityInstanceTasks'
GO
/****** Object:  Default [DF__HrsLeave__LeaveT__5165187F]    Script Date: 02/28/2025 17:15:49 ******/
ALTER TABLE [dbo].[HrsLeave] ADD  CONSTRAINT [DF__HrsLeave__LeaveT__5165187F]  DEFAULT ((0)) FOR [LeaveType]
GO
/****** Object:  Default [DF_SSIP_WfActivityInstance_State]    Script Date: 02/28/2025 17:15:49 ******/
ALTER TABLE [dbo].[WfActivityInstance] ADD  CONSTRAINT [DF_SSIP_WfActivityInstance_State]  DEFAULT ((0)) FOR [ActivityState]
GO
/****** Object:  Default [DF_WfActivityInstance_WorkItemType]    Script Date: 02/28/2025 17:15:49 ******/
ALTER TABLE [dbo].[WfActivityInstance] ADD  CONSTRAINT [DF_WfActivityInstance_WorkItemType]  DEFAULT ((0)) FOR [WorkItemType]
GO
/****** Object:  Default [DF_SSIP_WfActivityInstance_CanInvokeNextActivity]    Script Date: 02/28/2025 17:15:49 ******/
ALTER TABLE [dbo].[WfActivityInstance] ADD  CONSTRAINT [DF_SSIP_WfActivityInstance_CanInvokeNextActivity]  DEFAULT ((0)) FOR [CanNotRenewInstance]
GO
/****** Object:  Default [DF_SSIP_WfActivityInstance_TokensRequired]    Script Date: 02/28/2025 17:15:49 ******/
ALTER TABLE [dbo].[WfActivityInstance] ADD  CONSTRAINT [DF_SSIP_WfActivityInstance_TokensRequired]  DEFAULT ((1)) FOR [TokensRequired]
GO
/****** Object:  Default [DF_SSIP_WfActivityInstance_CreatedDateTime]    Script Date: 02/28/2025 17:15:49 ******/
ALTER TABLE [dbo].[WfActivityInstance] ADD  CONSTRAINT [DF_SSIP_WfActivityInstance_CreatedDateTime]  DEFAULT (getdate()) FOR [CreatedDateTime]
GO
/****** Object:  Default [DF_SSIP_WfActivityInstance_RecordStatusInvalid]    Script Date: 02/28/2025 17:15:49 ******/
ALTER TABLE [dbo].[WfActivityInstance] ADD  CONSTRAINT [DF_SSIP_WfActivityInstance_RecordStatusInvalid]  DEFAULT ((0)) FOR [RecordStatusInvalid]
GO
/****** Object:  Default [DF__WfJobSche__Statu__73BA3083]    Script Date: 02/28/2025 17:15:49 ******/
ALTER TABLE [dbo].[WfJobSchedule] ADD  CONSTRAINT [DF__WfJobSche__Statu__73BA3083]  DEFAULT ((0)) FOR [Status]
GO
/****** Object:  Default [DF_WfProcess_Version]    Script Date: 02/28/2025 17:15:49 ******/
ALTER TABLE [dbo].[WfProcess] ADD  CONSTRAINT [DF_WfProcess_Version]  DEFAULT ((1)) FOR [Version]
GO
/****** Object:  Default [DF_WfProcess_IsUsing]    Script Date: 02/28/2025 17:15:49 ******/
ALTER TABLE [dbo].[WfProcess] ADD  CONSTRAINT [DF_WfProcess_IsUsing]  DEFAULT ((0)) FOR [IsUsing]
GO
/****** Object:  Default [DF_WfProcess_IsTimingStartup]    Script Date: 02/28/2025 17:15:49 ******/
ALTER TABLE [dbo].[WfProcess] ADD  CONSTRAINT [DF_WfProcess_IsTimingStartup]  DEFAULT ((0)) FOR [StartType]
GO
/****** Object:  Default [DF_WfProcess_EndType]    Script Date: 02/28/2025 17:15:49 ******/
ALTER TABLE [dbo].[WfProcess] ADD  CONSTRAINT [DF_WfProcess_EndType]  DEFAULT ((0)) FOR [EndType]
GO
/****** Object:  Default [DF_SSIP-WfPROCESS_CreatedDateTime]    Script Date: 02/28/2025 17:15:49 ******/
ALTER TABLE [dbo].[WfProcess] ADD  CONSTRAINT [DF_SSIP-WfPROCESS_CreatedDateTime]  DEFAULT (getdate()) FOR [CreatedDateTime]
GO
/****** Object:  Default [DF_WfProcessInstance_Version]    Script Date: 02/28/2025 17:15:49 ******/
ALTER TABLE [dbo].[WfProcessInstance] ADD  CONSTRAINT [DF_WfProcessInstance_Version]  DEFAULT ((1)) FOR [Version]
GO
/****** Object:  Default [DF_SSIP_WfProcessInstance_State]    Script Date: 02/28/2025 17:15:49 ******/
ALTER TABLE [dbo].[WfProcessInstance] ADD  CONSTRAINT [DF_SSIP_WfProcessInstance_State]  DEFAULT ((0)) FOR [ProcessState]
GO
/****** Object:  Default [DF_WfProcessInstance_InvokedActivityInstanceID]    Script Date: 02/28/2025 17:15:49 ******/
ALTER TABLE [dbo].[WfProcessInstance] ADD  CONSTRAINT [DF_WfProcessInstance_InvokedActivityInstanceID]  DEFAULT ((0)) FOR [InvokedActivityInstanceID]
GO
/****** Object:  Default [DF_SSIP_WfProcessInstance_CreatedDateTime]    Script Date: 02/28/2025 17:15:49 ******/
ALTER TABLE [dbo].[WfProcessInstance] ADD  CONSTRAINT [DF_SSIP_WfProcessInstance_CreatedDateTime]  DEFAULT (getdate()) FOR [CreatedDateTime]
GO
/****** Object:  Default [DF_SSIP_WfProcessInstance_RecordStatus]    Script Date: 02/28/2025 17:15:49 ******/
ALTER TABLE [dbo].[WfProcessInstance] ADD  CONSTRAINT [DF_SSIP_WfProcessInstance_RecordStatus]  DEFAULT ((0)) FOR [RecordStatusInvalid]
GO
/****** Object:  Default [DF_SSIP_WfTasks_IsCompleted]    Script Date: 02/28/2025 17:15:49 ******/
ALTER TABLE [dbo].[WfTasks] ADD  CONSTRAINT [DF_SSIP_WfTasks_IsCompleted]  DEFAULT ((0)) FOR [TaskState]
GO
/****** Object:  Default [DF_WfTasks_IsEMailSent]    Script Date: 02/28/2025 17:15:49 ******/
ALTER TABLE [dbo].[WfTasks] ADD  CONSTRAINT [DF_WfTasks_IsEMailSent]  DEFAULT ((0)) FOR [IsEMailSent]
GO
/****** Object:  Default [DF_SSIP_WfTasks_CreatedDateTime]    Script Date: 02/28/2025 17:15:49 ******/
ALTER TABLE [dbo].[WfTasks] ADD  CONSTRAINT [DF_SSIP_WfTasks_CreatedDateTime]  DEFAULT (getdate()) FOR [CreatedDateTime]
GO
/****** Object:  Default [DF_SSIP_WfTasks_RecordStatusInvalid]    Script Date: 02/28/2025 17:15:49 ******/
ALTER TABLE [dbo].[WfTasks] ADD  CONSTRAINT [DF_SSIP_WfTasks_RecordStatusInvalid]  DEFAULT ((0)) FOR [RecordStatusInvalid]
GO
/****** Object:  Default [DF_WfTransitionInstance_IsBackwardFlying]    Script Date: 02/28/2025 17:15:49 ******/
ALTER TABLE [dbo].[WfTransitionInstance] ADD  CONSTRAINT [DF_WfTransitionInstance_IsBackwardFlying]  DEFAULT ((0)) FOR [FlyingType]
GO
/****** Object:  Default [DF_SSIP_WfTransitionInstance_ConditionParseResult]    Script Date: 02/28/2025 17:15:49 ******/
ALTER TABLE [dbo].[WfTransitionInstance] ADD  CONSTRAINT [DF_SSIP_WfTransitionInstance_ConditionParseResult]  DEFAULT ((0)) FOR [ConditionParseResult]
GO
/****** Object:  Default [DF_SSIP_WfTransitionInstance_CreatedDateTime]    Script Date: 02/28/2025 17:15:49 ******/
ALTER TABLE [dbo].[WfTransitionInstance] ADD  CONSTRAINT [DF_SSIP_WfTransitionInstance_CreatedDateTime]  DEFAULT (getdate()) FOR [CreatedDateTime]
GO
/****** Object:  Default [DF_SSIP_WfTransitionInstance_RecordStatusInvalid]    Script Date: 02/28/2025 17:15:49 ******/
ALTER TABLE [dbo].[WfTransitionInstance] ADD  CONSTRAINT [DF_SSIP_WfTransitionInstance_RecordStatusInvalid]  DEFAULT ((0)) FOR [RecordStatusInvalid]
GO
/****** Object:  ForeignKey [FK_WfActivityInstance_ProcessInstanceID]    Script Date: 02/28/2025 17:15:49 ******/
ALTER TABLE [dbo].[WfActivityInstance]  WITH NOCHECK ADD  CONSTRAINT [FK_WfActivityInstance_ProcessInstanceID] FOREIGN KEY([ProcessInstanceID])
REFERENCES [dbo].[WfProcessInstance] ([ID])
GO
ALTER TABLE [dbo].[WfActivityInstance] CHECK CONSTRAINT [FK_WfActivityInstance_ProcessInstanceID]
GO
/****** Object:  ForeignKey [FK_WfTasks_ActivityInstanceID]    Script Date: 02/28/2025 17:15:49 ******/
ALTER TABLE [dbo].[WfTasks]  WITH NOCHECK ADD  CONSTRAINT [FK_WfTasks_ActivityInstanceID] FOREIGN KEY([ActivityInstanceID])
REFERENCES [dbo].[WfActivityInstance] ([ID])
GO
ALTER TABLE [dbo].[WfTasks] CHECK CONSTRAINT [FK_WfTasks_ActivityInstanceID]
GO
