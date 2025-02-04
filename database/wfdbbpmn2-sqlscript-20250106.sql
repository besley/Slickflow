USE [WfDBBpmn2]
GO
/****** Object:  Table [dbo].[WfTransitionInstance]    Script Date: 01/06/2025 13:31:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[WfTransitionInstance](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TransitionGUID] [varchar](100) NOT NULL,
	[AppName] [nvarchar](50) NOT NULL,
	[AppInstanceID] [varchar](50) NOT NULL,
	[ProcessInstanceID] [int] NOT NULL,
	[ProcessGUID] [varchar](100) NOT NULL,
	[TransitionType] [tinyint] NOT NULL,
	[FlyingType] [tinyint] NOT NULL,
	[FromActivityInstanceID] [int] NOT NULL,
	[FromActivityGUID] [varchar](100) NOT NULL,
	[FromActivityType] [smallint] NOT NULL,
	[FromActivityName] [nvarchar](50) NOT NULL,
	[ToActivityInstanceID] [int] NOT NULL,
	[ToActivityGUID] [varchar](100) NOT NULL,
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
INSERT [dbo].[WfTransitionInstance] ([ID], [TransitionGUID], [AppName], [AppInstanceID], [ProcessInstanceID], [ProcessGUID], [TransitionType], [FlyingType], [FromActivityInstanceID], [FromActivityGUID], [FromActivityType], [FromActivityName], [ToActivityInstanceID], [ToActivityGUID], [ToActivityType], [ToActivityName], [ConditionParseResult], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [RecordStatusInvalid]) VALUES (1316, N'033e5de4-b568-437b-a457-8dbff5c12f2a', N'SamplePrice', N'100', 485, N'8186fe2b-f3d7-4955-b915-8ed821941eb9', 1, 0, 1968, N'a31a9bc4-f1f9-45d6-82aa-1b3c049ce6a5', 1, N'Start', 1969, N'28d7c3ae-bb9a-4177-8459-8b3f062c621c', 4, N'Task-001', 1, N'10', N'Long', CAST(0x0000B256010AD67C AS DateTime), 0)
INSERT [dbo].[WfTransitionInstance] ([ID], [TransitionGUID], [AppName], [AppInstanceID], [ProcessInstanceID], [ProcessGUID], [TransitionType], [FlyingType], [FromActivityInstanceID], [FromActivityGUID], [FromActivityType], [FromActivityName], [ToActivityInstanceID], [ToActivityGUID], [ToActivityType], [ToActivityName], [ConditionParseResult], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [RecordStatusInvalid]) VALUES (1317, N'd9d38bd2-1b85-49cd-b3ce-c884429054f1', N'ProductOrder', N'699', 486, N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', 1, 0, 1970, N'0253ff58-47f1-4203-9986-ef4d3e49199d', 1, N'Start', 1971, N'62b71f52-2c6d-4476-d3c8-bd5f9ac1b94f', 4, N'派单', 1, N'7', N'Peter', CAST(0x0000B25D00B19D46 AS DateTime), 0)
INSERT [dbo].[WfTransitionInstance] ([ID], [TransitionGUID], [AppName], [AppInstanceID], [ProcessInstanceID], [ProcessGUID], [TransitionType], [FlyingType], [FromActivityInstanceID], [FromActivityGUID], [FromActivityType], [FromActivityName], [ToActivityInstanceID], [ToActivityGUID], [ToActivityType], [ToActivityName], [ConditionParseResult], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [RecordStatusInvalid]) VALUES (1318, N'6997a8c8-9df8-49b2-b7ca-05bd973946f2', N'ProductOrder', N'699', 486, N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', 1, 0, 1971, N'62b71f52-2c6d-4476-d3c8-bd5f9ac1b94f', 4, N'派单', 1972, N'1bc8a032-ab92-4f6e-9bf1-28e87e5a6ece', 8, N'Gateway_1qnv0ou', 1, N'7', N'Peter', CAST(0x0000B25D00B19D61 AS DateTime), 0)
INSERT [dbo].[WfTransitionInstance] ([ID], [TransitionGUID], [AppName], [AppInstanceID], [ProcessInstanceID], [ProcessGUID], [TransitionType], [FlyingType], [FromActivityInstanceID], [FromActivityGUID], [FromActivityType], [FromActivityName], [ToActivityInstanceID], [ToActivityGUID], [ToActivityType], [ToActivityName], [ConditionParseResult], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [RecordStatusInvalid]) VALUES (1319, N'f6feb6be-aa40-45bc-b69b-069b9ca49227', N'ProductOrder', N'699', 486, N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', 1, 0, 1972, N'1bc8a032-ab92-4f6e-9bf1-28e87e5a6ece', 8, N'Gateway_1qnv0ou', 1973, N'126ed9cc-b661-4e77-ae95-b5001ad9ce9c', 4, N'打样', 1, N'7', N'Peter', CAST(0x0000B25D00B19D62 AS DateTime), 0)
INSERT [dbo].[WfTransitionInstance] ([ID], [TransitionGUID], [AppName], [AppInstanceID], [ProcessInstanceID], [ProcessGUID], [TransitionType], [FlyingType], [FromActivityInstanceID], [FromActivityGUID], [FromActivityType], [FromActivityName], [ToActivityInstanceID], [ToActivityGUID], [ToActivityType], [ToActivityName], [ConditionParseResult], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [RecordStatusInvalid]) VALUES (1320, N'59262a37-51df-4fd0-cd54-36797fddfa65', N'ProductOrder', N'699', 486, N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', 1, 0, 1973, N'126ed9cc-b661-4e77-ae95-b5001ad9ce9c', 4, N'打样', 1974, N'5b6ba25a-8dd4-40c4-d27e-f834f0be0168', 4, N'生产', 1, N'11', N'Fisher', CAST(0x0000B25D00B1B98A AS DateTime), 0)
INSERT [dbo].[WfTransitionInstance] ([ID], [TransitionGUID], [AppName], [AppInstanceID], [ProcessInstanceID], [ProcessGUID], [TransitionType], [FlyingType], [FromActivityInstanceID], [FromActivityGUID], [FromActivityType], [FromActivityName], [ToActivityInstanceID], [ToActivityGUID], [ToActivityType], [ToActivityName], [ConditionParseResult], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [RecordStatusInvalid]) VALUES (1321, N'd9d38bd2-1b85-49cd-b3ce-c884429054f1', N'ProductOrder', N'700', 487, N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', 1, 0, 1975, N'0253ff58-47f1-4203-9986-ef4d3e49199d', 1, N'Start', 1976, N'62b71f52-2c6d-4476-d3c8-bd5f9ac1b94f', 4, N'派单', 1, N'7', N'Peter', CAST(0x0000B25D00B247AD AS DateTime), 0)
INSERT [dbo].[WfTransitionInstance] ([ID], [TransitionGUID], [AppName], [AppInstanceID], [ProcessInstanceID], [ProcessGUID], [TransitionType], [FlyingType], [FromActivityInstanceID], [FromActivityGUID], [FromActivityType], [FromActivityName], [ToActivityInstanceID], [ToActivityGUID], [ToActivityType], [ToActivityName], [ConditionParseResult], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [RecordStatusInvalid]) VALUES (1322, N'6997a8c8-9df8-49b2-b7ca-05bd973946f2', N'ProductOrder', N'700', 487, N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', 1, 0, 1976, N'62b71f52-2c6d-4476-d3c8-bd5f9ac1b94f', 4, N'派单', 1977, N'1bc8a032-ab92-4f6e-9bf1-28e87e5a6ece', 8, N'Gateway_1qnv0ou', 1, N'7', N'Peter', CAST(0x0000B25D00B247B0 AS DateTime), 0)
INSERT [dbo].[WfTransitionInstance] ([ID], [TransitionGUID], [AppName], [AppInstanceID], [ProcessInstanceID], [ProcessGUID], [TransitionType], [FlyingType], [FromActivityInstanceID], [FromActivityGUID], [FromActivityType], [FromActivityName], [ToActivityInstanceID], [ToActivityGUID], [ToActivityType], [ToActivityName], [ConditionParseResult], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [RecordStatusInvalid]) VALUES (1323, N'f6feb6be-aa40-45bc-b69b-069b9ca49227', N'ProductOrder', N'700', 487, N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', 1, 0, 1977, N'1bc8a032-ab92-4f6e-9bf1-28e87e5a6ece', 8, N'Gateway_1qnv0ou', 1978, N'126ed9cc-b661-4e77-ae95-b5001ad9ce9c', 4, N'打样', 1, N'7', N'Peter', CAST(0x0000B25D00B247B1 AS DateTime), 0)
INSERT [dbo].[WfTransitionInstance] ([ID], [TransitionGUID], [AppName], [AppInstanceID], [ProcessInstanceID], [ProcessGUID], [TransitionType], [FlyingType], [FromActivityInstanceID], [FromActivityGUID], [FromActivityType], [FromActivityName], [ToActivityInstanceID], [ToActivityGUID], [ToActivityType], [ToActivityName], [ConditionParseResult], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [RecordStatusInvalid]) VALUES (1324, N'59262a37-51df-4fd0-cd54-36797fddfa65', N'ProductOrder', N'700', 487, N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', 1, 0, 1978, N'126ed9cc-b661-4e77-ae95-b5001ad9ce9c', 4, N'打样', 1979, N'5b6ba25a-8dd4-40c4-d27e-f834f0be0168', 4, N'生产', 1, N'11', N'Fisher', CAST(0x0000B25D00B25ABB AS DateTime), 0)
INSERT [dbo].[WfTransitionInstance] ([ID], [TransitionGUID], [AppName], [AppInstanceID], [ProcessInstanceID], [ProcessGUID], [TransitionType], [FlyingType], [FromActivityInstanceID], [FromActivityGUID], [FromActivityType], [FromActivityName], [ToActivityInstanceID], [ToActivityGUID], [ToActivityType], [ToActivityName], [ConditionParseResult], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [RecordStatusInvalid]) VALUES (1325, N'd9d38bd2-1b85-49cd-b3ce-c884429054f1', N'ProductOrder', N'701', 488, N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', 1, 0, 1980, N'0253ff58-47f1-4203-9986-ef4d3e49199d', 1, N'Start', 1981, N'62b71f52-2c6d-4476-d3c8-bd5f9ac1b94f', 4, N'Dispatch(派单)', 1, N'7', N'Peter', CAST(0x0000B25D00B43CB9 AS DateTime), 0)
INSERT [dbo].[WfTransitionInstance] ([ID], [TransitionGUID], [AppName], [AppInstanceID], [ProcessInstanceID], [ProcessGUID], [TransitionType], [FlyingType], [FromActivityInstanceID], [FromActivityGUID], [FromActivityType], [FromActivityName], [ToActivityInstanceID], [ToActivityGUID], [ToActivityType], [ToActivityName], [ConditionParseResult], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [RecordStatusInvalid]) VALUES (1326, N'6997a8c8-9df8-49b2-b7ca-05bd973946f2', N'ProductOrder', N'701', 488, N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', 1, 0, 1981, N'62b71f52-2c6d-4476-d3c8-bd5f9ac1b94f', 4, N'Dispatch(派单)', 1982, N'1bc8a032-ab92-4f6e-9bf1-28e87e5a6ece', 8, N'Gateway_1qnv0ou', 1, N'7', N'Peter', CAST(0x0000B25D00B43CCC AS DateTime), 0)
INSERT [dbo].[WfTransitionInstance] ([ID], [TransitionGUID], [AppName], [AppInstanceID], [ProcessInstanceID], [ProcessGUID], [TransitionType], [FlyingType], [FromActivityInstanceID], [FromActivityGUID], [FromActivityType], [FromActivityName], [ToActivityInstanceID], [ToActivityGUID], [ToActivityType], [ToActivityName], [ConditionParseResult], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [RecordStatusInvalid]) VALUES (1327, N'f6feb6be-aa40-45bc-b69b-069b9ca49227', N'ProductOrder', N'701', 488, N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', 1, 0, 1982, N'1bc8a032-ab92-4f6e-9bf1-28e87e5a6ece', 8, N'Gateway_1qnv0ou', 1983, N'126ed9cc-b661-4e77-ae95-b5001ad9ce9c', 4, N'Sample(打样)', 1, N'7', N'Peter', CAST(0x0000B25D00B43CCD AS DateTime), 0)
INSERT [dbo].[WfTransitionInstance] ([ID], [TransitionGUID], [AppName], [AppInstanceID], [ProcessInstanceID], [ProcessGUID], [TransitionType], [FlyingType], [FromActivityInstanceID], [FromActivityGUID], [FromActivityType], [FromActivityName], [ToActivityInstanceID], [ToActivityGUID], [ToActivityType], [ToActivityName], [ConditionParseResult], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [RecordStatusInvalid]) VALUES (1328, N'59262a37-51df-4fd0-cd54-36797fddfa65', N'ProductOrder', N'701', 488, N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', 1, 0, 1983, N'126ed9cc-b661-4e77-ae95-b5001ad9ce9c', 4, N'Sample(打样)', 1984, N'5b6ba25a-8dd4-40c4-d27e-f834f0be0168', 4, N'Manufacture(生产)', 1, N'11', N'Fisher', CAST(0x0000B25D00B45323 AS DateTime), 0)
INSERT [dbo].[WfTransitionInstance] ([ID], [TransitionGUID], [AppName], [AppInstanceID], [ProcessInstanceID], [ProcessGUID], [TransitionType], [FlyingType], [FromActivityInstanceID], [FromActivityGUID], [FromActivityType], [FromActivityName], [ToActivityInstanceID], [ToActivityGUID], [ToActivityType], [ToActivityName], [ConditionParseResult], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [RecordStatusInvalid]) VALUES (1329, N'b9c7e52f-0e0c-4085-cba0-9f5f0759af41', N'ProductOrder', N'701', 488, N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', 1, 0, 1984, N'5b6ba25a-8dd4-40c4-d27e-f834f0be0168', 4, N'Manufacture(生产)', 1985, N'78a69a65-d406-4056-9dc2-b25751fc6263', 4, N'QCCheck(质检)', 1, N'9', N'Tuda', CAST(0x0000B25D00B47180 AS DateTime), 0)
INSERT [dbo].[WfTransitionInstance] ([ID], [TransitionGUID], [AppName], [AppInstanceID], [ProcessInstanceID], [ProcessGUID], [TransitionType], [FlyingType], [FromActivityInstanceID], [FromActivityGUID], [FromActivityType], [FromActivityName], [ToActivityInstanceID], [ToActivityGUID], [ToActivityType], [ToActivityName], [ConditionParseResult], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [RecordStatusInvalid]) VALUES (1330, N'4b54a9e5-34b2-490f-b57e-548f7f7b0e14', N'ProductOrder', N'701', 488, N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', 1, 0, 1985, N'78a69a65-d406-4056-9dc2-b25751fc6263', 4, N'QCCheck(质检)', 1986, N'1c1bceae-7bce-4394-cc0b-6bbf1a87bf2d', 4, N'Weight(称重)', 1, N'13', N'Jimi', CAST(0x0000B25D00B492C1 AS DateTime), 0)
INSERT [dbo].[WfTransitionInstance] ([ID], [TransitionGUID], [AppName], [AppInstanceID], [ProcessInstanceID], [ProcessGUID], [TransitionType], [FlyingType], [FromActivityInstanceID], [FromActivityGUID], [FromActivityType], [FromActivityName], [ToActivityInstanceID], [ToActivityGUID], [ToActivityType], [ToActivityName], [ConditionParseResult], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [RecordStatusInvalid]) VALUES (1331, N'89efad47-ba53-48d8-9856-0581d6915187', N'ProductOrder', N'701', 488, N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', 1, 0, 1986, N'1c1bceae-7bce-4394-cc0b-6bbf1a87bf2d', 4, N'Weight(称重)', 1987, N'b112b4d0-1cc1-4667-bdda-73c85e16266e', 4, N'Print(打印发货单)', 1, N'15', N'Damark', CAST(0x0000B25D00B4A742 AS DateTime), 0)
INSERT [dbo].[WfTransitionInstance] ([ID], [TransitionGUID], [AppName], [AppInstanceID], [ProcessInstanceID], [ProcessGUID], [TransitionType], [FlyingType], [FromActivityInstanceID], [FromActivityGUID], [FromActivityType], [FromActivityName], [ToActivityInstanceID], [ToActivityGUID], [ToActivityType], [ToActivityName], [ConditionParseResult], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [RecordStatusInvalid]) VALUES (1332, N'd4d614a4-792f-4932-8366-8781be420caa', N'ProductOrder', N'701', 488, N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', 1, 0, 1987, N'b112b4d0-1cc1-4667-bdda-73c85e16266e', 4, N'Print(打印发货单)', 1988, N'652441d3-2e61-4df0-a50f-499c253e6c19', 2, N'End', 1, N'15', N'Damark', CAST(0x0000B25D00B4B0BA AS DateTime), 0)
INSERT [dbo].[WfTransitionInstance] ([ID], [TransitionGUID], [AppName], [AppInstanceID], [ProcessInstanceID], [ProcessGUID], [TransitionType], [FlyingType], [FromActivityInstanceID], [FromActivityGUID], [FromActivityType], [FromActivityName], [ToActivityInstanceID], [ToActivityGUID], [ToActivityType], [ToActivityName], [ConditionParseResult], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [RecordStatusInvalid]) VALUES (1333, N'b9c7e52f-0e0c-4085-cba0-9f5f0759af41', N'ProductOrder', N'700', 487, N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', 1, 0, 1979, N'5b6ba25a-8dd4-40c4-d27e-f834f0be0168', 4, N'生产', 1989, N'78a69a65-d406-4056-9dc2-b25751fc6263', 4, N'QCCheck(质检)', 1, N'9', N'Tuda', CAST(0x0000B25D00B534DF AS DateTime), 0)
SET IDENTITY_INSERT [dbo].[WfTransitionInstance] OFF
/****** Object:  Table [dbo].[WfProcessVariable]    Script Date: 01/06/2025 13:31:59 ******/
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
	[ProcessGUID] [varchar](100) NOT NULL,
	[ProcessInstanceID] [int] NOT NULL,
	[ActivityGUID] [varchar](100) NULL,
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
/****** Object:  Table [dbo].[WfProcessInstance]    Script Date: 01/06/2025 13:31:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[WfProcessInstance](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ProcessGUID] [varchar](100) NOT NULL,
	[ProcessName] [nvarchar](50) NOT NULL,
	[Version] [nvarchar](20) NOT NULL,
	[AppInstanceID] [varchar](50) NOT NULL,
	[AppName] [nvarchar](50) NOT NULL,
	[AppInstanceCode] [nvarchar](50) NULL,
	[ProcessState] [smallint] NOT NULL,
	[SubProcessType] [smallint] NULL,
	[SubProcessID] [int] NULL,
	[SubProcessGUID] [varchar](100) NULL,
	[InvokedActivityInstanceID] [int] NULL,
	[InvokedActivityGUID] [varchar](100) NULL,
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
INSERT [dbo].[WfProcessInstance] ([ID], [ProcessGUID], [ProcessName], [Version], [AppInstanceID], [AppName], [AppInstanceCode], [ProcessState], [SubProcessType], [SubProcessID], [SubProcessGUID], [InvokedActivityInstanceID], [InvokedActivityGUID], [JobTimerType], [JobTimerStatus], [TriggerExpression], [OverdueDateTime], [JobTimerTreatedDateTime], [CreatedDateTime], [CreatedByUserID], [CreatedByUserName], [LastUpdatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [EndedDateTime], [EndedByUserID], [EndedByUserName], [RecordStatusInvalid]) VALUES (485, N'8186fe2b-f3d7-4955-b915-8ed821941eb9', N'Sequence_4517', N'1', N'100', N'SamplePrice', NULL, 2, NULL, 0, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, CAST(0x0000B256010AD653 AS DateTime), N'10', N'Long', CAST(0x0000B256010AD654 AS DateTime), N'10', N'Long', NULL, NULL, NULL, 0)
INSERT [dbo].[WfProcessInstance] ([ID], [ProcessGUID], [ProcessName], [Version], [AppInstanceID], [AppName], [AppInstanceCode], [ProcessState], [SubProcessType], [SubProcessID], [SubProcessGUID], [InvokedActivityInstanceID], [InvokedActivityGUID], [JobTimerType], [JobTimerStatus], [TriggerExpression], [OverdueDateTime], [JobTimerTreatedDateTime], [CreatedDateTime], [CreatedByUserID], [CreatedByUserName], [LastUpdatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [EndedDateTime], [EndedByUserID], [EndedByUserName], [RecordStatusInvalid]) VALUES (486, N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', N'OrderProcess_Name_7829', N'1', N'699', N'ProductOrder', NULL, 2, NULL, 0, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, CAST(0x0000B25D00B19D2F AS DateTime), N'7', N'Peter', CAST(0x0000B25D00B19D2F AS DateTime), N'7', N'Peter', NULL, NULL, NULL, 0)
INSERT [dbo].[WfProcessInstance] ([ID], [ProcessGUID], [ProcessName], [Version], [AppInstanceID], [AppName], [AppInstanceCode], [ProcessState], [SubProcessType], [SubProcessID], [SubProcessGUID], [InvokedActivityInstanceID], [InvokedActivityGUID], [JobTimerType], [JobTimerStatus], [TriggerExpression], [OverdueDateTime], [JobTimerTreatedDateTime], [CreatedDateTime], [CreatedByUserID], [CreatedByUserName], [LastUpdatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [EndedDateTime], [EndedByUserID], [EndedByUserName], [RecordStatusInvalid]) VALUES (487, N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', N'OrderProcess_Name_7829', N'1', N'700', N'ProductOrder', NULL, 2, NULL, 0, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, CAST(0x0000B25D00B247AC AS DateTime), N'7', N'Peter', CAST(0x0000B25D00B247AC AS DateTime), N'7', N'Peter', NULL, NULL, NULL, 0)
INSERT [dbo].[WfProcessInstance] ([ID], [ProcessGUID], [ProcessName], [Version], [AppInstanceID], [AppName], [AppInstanceCode], [ProcessState], [SubProcessType], [SubProcessID], [SubProcessGUID], [InvokedActivityInstanceID], [InvokedActivityGUID], [JobTimerType], [JobTimerStatus], [TriggerExpression], [OverdueDateTime], [JobTimerTreatedDateTime], [CreatedDateTime], [CreatedByUserID], [CreatedByUserName], [LastUpdatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [EndedDateTime], [EndedByUserID], [EndedByUserName], [RecordStatusInvalid]) VALUES (488, N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', N'OrderProcess_Name_7829', N'1', N'701', N'ProductOrder', NULL, 4, NULL, 0, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, CAST(0x0000B25D00B43CA8 AS DateTime), N'7', N'Peter', CAST(0x0000B25D00B43CA8 AS DateTime), N'7', N'Peter', CAST(0x0000B25D00B4B0BA AS DateTime), N'15', N'Damark', 0)
SET IDENTITY_INSERT [dbo].[WfProcessInstance] OFF
/****** Object:  Table [dbo].[WfProcess]    Script Date: 01/06/2025 13:31:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[WfProcess](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ProcessGUID] [varchar](100) NOT NULL,
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
INSERT [dbo].[WfProcess] ([ID], [ProcessGUID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (939, N'c31c84ba-ba8b-490a-9955-64c257b96b86', N'1', N'Sequence_Process_Name_2280', N'Process_Code_2280', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn2:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL" xmlns:sf="http://www.slickflow.com/schema/sf" id="bpmn2-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn2:process id="Process_100" sf:guid="c31c84ba-ba8b-490a-9955-64c257b96b86" sf:code="Process_Code_2280" name="Process_Name_2280" isExecutable="false" version="1">
    <bpmn2:startEvent id="StartEvent_1" sf:guid="dedcb54f-0c29-4798-89b0-16f79fa0c9c3">
      <bpmn2:outgoing>Flow_11trhl8</bpmn2:outgoing>
    </bpmn2:startEvent>
    <bpmn2:task id="Activity_0i9msjg" sf:guid="3b5fb01d-3c1e-419f-c533-fb0a5bc36a14" name="submit">
      <bpmn2:incoming>Flow_11trhl8</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1l1tmra</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_11trhl8" sf:guid="029f40f5-7ee5-4b95-c331-a93bd0a7d1dc" sourceRef="StartEvent_1" targetRef="Activity_0i9msjg" sf:from="dedcb54f-0c29-4798-89b0-16f79fa0c9c3" sf:to="3b5fb01d-3c1e-419f-c533-fb0a5bc36a14" />
    <bpmn2:task id="Activity_0pstie0" sf:guid="48a9ab8c-d81f-4844-9cc6-22a1416fb550" name="approval">
      <bpmn2:incoming>Flow_1l1tmra</bpmn2:incoming>
      <bpmn2:outgoing>Flow_14dmuy6</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_1l1tmra" sf:guid="00317cf9-0983-4288-e460-dabcfbf497f3" sourceRef="Activity_0i9msjg" targetRef="Activity_0pstie0" sf:from="3b5fb01d-3c1e-419f-c533-fb0a5bc36a14" sf:to="48a9ab8c-d81f-4844-9cc6-22a1416fb550" />
    <bpmn2:endEvent id="Event_00evm63" sf:guid="78c67d89-2efb-4ef5-dc2d-3bffe69cb5a9">
      <bpmn2:incoming>Flow_14dmuy6</bpmn2:incoming>
    </bpmn2:endEvent>
    <bpmn2:sequenceFlow id="Flow_14dmuy6" sf:guid="4b3c315a-88cc-46e3-f9cc-5d0b44d57aac" sourceRef="Activity_0pstie0" targetRef="Event_00evm63" sf:from="48a9ab8c-d81f-4844-9cc6-22a1416fb550" sf:to="78c67d89-2efb-4ef5-dc2d-3bffe69cb5a9" />
  </bpmn2:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_100">
      <bpmndi:BPMNEdge id="Flow_11trhl8_di" bpmnElement="Flow_11trhl8">
        <di:waypoint x="448" y="258" />
        <di:waypoint x="500" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1l1tmra_di" bpmnElement="Flow_1l1tmra">
        <di:waypoint x="600" y="258" />
        <di:waypoint x="660" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_14dmuy6_di" bpmnElement="Flow_14dmuy6">
        <di:waypoint x="760" y="258" />
        <di:waypoint x="822" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="412" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0i9msjg_di" bpmnElement="Activity_0i9msjg">
        <dc:Bounds x="500" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0pstie0_di" bpmnElement="Activity_0pstie0">
        <dc:Bounds x="660" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_00evm63_di" bpmnElement="Event_00evm63">
        <dc:Bounds x="822" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn2:definitions>
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            ', 0, NULL, N'', 0, NULL, CAST(0x0000AEB100C283A0 AS DateTime), NULL)
INSERT [dbo].[WfProcess] ([ID], [ProcessGUID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (940, N'2658f680-d87b-4abe-ab29-13a6cbb79a51', N'1', N'XOR_Process_Name_7740', N'Process_Code_7740', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn2:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:sfs="http://www.slickflow.com/schema/bpmn/sfs" xmlns:sfgb="http://www.slickflow.com/schema/sfgb" id="bpmn2-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn2:process id="Process_100" sf:guid="2658f680-d87b-4abe-ab29-13a6cbb79a51" sf:code="Process_Code_7740" name="Process_Name_7740" isExecutable="false" version="1">
    <bpmn2:startEvent id="StartEvent_1" sf:guid="e4c9e2e2-060b-4a2b-ab4c-d4f7b08fa4cb">
      <bpmn2:outgoing>Flow_1az9vyq</bpmn2:outgoing>
    </bpmn2:startEvent>
    <bpmn2:task id="Activity_0vh3e0b" sf:guid="76006c43-4591-4c6f-bc59-8b8464158dd8" name="submit">
      <bpmn2:extensionElements>
        <sfs:sections>
          <sfs:section name="myProperties">{</sfs:section>
        </sfs:sections>
      </bpmn2:extensionElements>
      <bpmn2:incoming>Flow_1az9vyq</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0qbrx62</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_1az9vyq" sf:guid="3b2e8f81-2cd9-42d0-e6a0-af5f4e8aa010" sourceRef="StartEvent_1" targetRef="Activity_0vh3e0b" sf:from="e4c9e2e2-060b-4a2b-ab4c-d4f7b08fa4cb" sf:to="76006c43-4591-4c6f-bc59-8b8464158dd8" />
    <bpmn2:sequenceFlow id="Flow_0qbrx62" sf:guid="345726a3-2a88-492c-f87e-b2a7fab77eb9" sourceRef="Activity_0vh3e0b" targetRef="Gateway_1iahdo9" sf:from="76006c43-4591-4c6f-bc59-8b8464158dd8" sf:to="568e5678-b32c-4dde-828b-ae4b11a1de47" />
    <bpmn2:task id="Activity_0ugcz3l" sf:guid="11c0fa35-f824-4968-d969-7516fcf4dbf4" name="dept approval">
      <bpmn2:incoming>Flow_0r9c45z</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1w4xcgr</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_0r9c45z" sf:guid="85f7f49e-bf2d-4299-d094-dc7c45f6f0b4" name="days&#62;=1" sourceRef="Gateway_1iahdo9" targetRef="Activity_0ugcz3l" sf:from="568e5678-b32c-4dde-828b-ae4b11a1de47" sf:to="11c0fa35-f824-4968-d969-7516fcf4dbf4">
      <bpmn2:extensionElements>
        <sfgb:groupBehaviours priority="1" />
      </bpmn2:extensionElements>
      <bpmn2:conditionExpression>days&gt;=1</bpmn2:conditionExpression>
    </bpmn2:sequenceFlow>
    <bpmn2:task id="Activity_1o2nyb1" sf:guid="b129c8b6-418b-4bc7-8aaf-ab6667f14eac" name="CEO approval">
      <bpmn2:incoming>Flow_196cbl1</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1szizo4</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_196cbl1" sf:guid="1171244e-0b56-4bfb-e9be-272868b20f8b" name="days&#62;=10" sourceRef="Gateway_1iahdo9" targetRef="Activity_1o2nyb1" sf:from="568e5678-b32c-4dde-828b-ae4b11a1de47" sf:to="b129c8b6-418b-4bc7-8aaf-ab6667f14eac">
      <bpmn2:extensionElements>
        <sfgb:groupBehaviours priority="-1" />
      </bpmn2:extensionElements>
      <bpmn2:conditionExpression>days&gt;=10</bpmn2:conditionExpression>
    </bpmn2:sequenceFlow>
    <bpmn2:exclusiveGateway id="Gateway_0qwni8v" sf:guid="3735e056-3b4e-49d2-d16c-3d3474026931">
      <bpmn2:incoming>Flow_1w4xcgr</bpmn2:incoming>
      <bpmn2:incoming>Flow_1szizo4</bpmn2:incoming>
      <bpmn2:incoming>Flow_1wk3qc9</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0yr2z25</bpmn2:outgoing>
    </bpmn2:exclusiveGateway>
    <bpmn2:sequenceFlow id="Flow_1w4xcgr" sf:guid="006f8e32-5c88-4d6a-b483-480a03a24fed" sourceRef="Activity_0ugcz3l" targetRef="Gateway_0qwni8v" sf:from="11c0fa35-f824-4968-d969-7516fcf4dbf4" sf:to="3735e056-3b4e-49d2-d16c-3d3474026931" />
    <bpmn2:sequenceFlow id="Flow_1szizo4" sf:guid="873cc1d8-1ca4-4a34-b25a-2d9a159a0df7" sourceRef="Activity_1o2nyb1" targetRef="Gateway_0qwni8v" sf:from="b129c8b6-418b-4bc7-8aaf-ab6667f14eac" sf:to="3735e056-3b4e-49d2-d16c-3d3474026931" />
    <bpmn2:task id="Activity_12r2pfa" sf:guid="bc6bdf41-62cd-4388-a317-1c800e79c13a" name="HR approval">
      <bpmn2:incoming>Flow_0yr2z25</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1tlvwu1</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_0yr2z25" sf:guid="8869159f-f844-4d1c-89d1-ea409c57e217" sourceRef="Gateway_0qwni8v" targetRef="Activity_12r2pfa" sf:from="3735e056-3b4e-49d2-d16c-3d3474026931" sf:to="bc6bdf41-62cd-4388-a317-1c800e79c13a" />
    <bpmn2:endEvent id="Event_1tg7zck" sf:guid="e75efa41-6f74-43de-e872-042b8a7dc0c1">
      <bpmn2:incoming>Flow_1tlvwu1</bpmn2:incoming>
    </bpmn2:endEvent>
    <bpmn2:sequenceFlow id="Flow_1tlvwu1" sf:guid="6a4eba80-396a-4b37-c1e7-425161d94394" sourceRef="Activity_12r2pfa" targetRef="Event_1tg7zck" sf:from="bc6bdf41-62cd-4388-a317-1c800e79c13a" sf:to="e75efa41-6f74-43de-e872-042b8a7dc0c1" />
    <bpmn2:exclusiveGateway id="Gateway_1iahdo9" sf:guid="568e5678-b32c-4dde-828b-ae4b11a1de47">
      <bpmn2:incoming>Flow_0qbrx62</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0r9c45z</bpmn2:outgoing>
      <bpmn2:outgoing>Flow_196cbl1</bpmn2:outgoing>
      <bpmn2:outgoing>Flow_0fs1biu</bpmn2:outgoing>
    </bpmn2:exclusiveGateway>
    <bpmn2:task id="Activity_09k9fbd" sf:guid="809a8f14-0417-4e3b-db9b-1d9f783aad2a" name="CTO approval">
      <bpmn2:incoming>Flow_0fs1biu</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1wk3qc9</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_0fs1biu" sf:guid="5488ff4d-3753-478d-d5dd-8cd252d2f3c3" name="days&#62;=3" sourceRef="Gateway_1iahdo9" targetRef="Activity_09k9fbd" sf:from="568e5678-b32c-4dde-828b-ae4b11a1de47" sf:to="809a8f14-0417-4e3b-db9b-1d9f783aad2a">
      <bpmn2:extensionElements>
        <sfgb:groupBehaviours priority="0" />
      </bpmn2:extensionElements>
      <bpmn2:conditionExpression>days&gt;=3</bpmn2:conditionExpression>
    </bpmn2:sequenceFlow>
    <bpmn2:sequenceFlow id="Flow_1wk3qc9" sf:guid="9e198c94-4fd8-4b3f-e657-82dc0b78ecfe" sourceRef="Activity_09k9fbd" targetRef="Gateway_0qwni8v" sf:from="809a8f14-0417-4e3b-db9b-1d9f783aad2a" sf:to="3735e056-3b4e-49d2-d16c-3d3474026931" />
  </bpmn2:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_100">
      <bpmndi:BPMNEdge id="Flow_1wk3qc9_di" bpmnElement="Flow_1wk3qc9">
        <di:waypoint x="850" y="258" />
        <di:waypoint x="915" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0fs1biu_di" bpmnElement="Flow_0fs1biu">
        <di:waypoint x="705" y="258" />
        <di:waypoint x="750" y="258" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="706" y="240" width="43" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1tlvwu1_di" bpmnElement="Flow_1tlvwu1">
        <di:waypoint x="1130" y="258" />
        <di:waypoint x="1202" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0yr2z25_di" bpmnElement="Flow_0yr2z25">
        <di:waypoint x="965" y="258" />
        <di:waypoint x="1030" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1szizo4_di" bpmnElement="Flow_1szizo4">
        <di:waypoint x="860" y="370" />
        <di:waypoint x="940" y="370" />
        <di:waypoint x="940" y="283" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1w4xcgr_di" bpmnElement="Flow_1w4xcgr">
        <di:waypoint x="850" y="140" />
        <di:waypoint x="940" y="140" />
        <di:waypoint x="940" y="233" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_196cbl1_di" bpmnElement="Flow_196cbl1">
        <di:waypoint x="680" y="283" />
        <di:waypoint x="680" y="370" />
        <di:waypoint x="760" y="370" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="671" y="324" width="49" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0r9c45z_di" bpmnElement="Flow_0r9c45z">
        <di:waypoint x="680" y="233" />
        <di:waypoint x="680" y="140" />
        <di:waypoint x="750" y="140" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="674" y="184" width="43" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0qbrx62_di" bpmnElement="Flow_0qbrx62">
        <di:waypoint x="600" y="258" />
        <di:waypoint x="655" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1az9vyq_di" bpmnElement="Flow_1az9vyq">
        <di:waypoint x="448" y="258" />
        <di:waypoint x="500" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="412" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0vh3e0b_di" bpmnElement="Activity_0vh3e0b">
        <dc:Bounds x="500" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0ugcz3l_di" bpmnElement="Activity_0ugcz3l">
        <dc:Bounds x="750" y="100" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1o2nyb1_di" bpmnElement="Activity_1o2nyb1">
        <dc:Bounds x="760" y="330" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Gateway_0qwni8v_di" bpmnElement="Gateway_0qwni8v" isMarkerVisible="true">
        <dc:Bounds x="915" y="233" width="50" height="50" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_12r2pfa_di" bpmnElement="Activity_12r2pfa">
        <dc:Bounds x="1030" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_1tg7zck_di" bpmnElement="Event_1tg7zck">
        <dc:Bounds x="1202" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Gateway_0xy5l0c_di" bpmnElement="Gateway_1iahdo9" isMarkerVisible="true">
        <dc:Bounds x="655" y="233" width="50" height="50" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_09k9fbd_di" bpmnElement="Activity_09k9fbd">
        <dc:Bounds x="750" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn2:definitions>
', 0, NULL, N'', 0, NULL, CAST(0x0000AEB100C3396D AS DateTime), CAST(0x0000AEB500DB99EA AS DateTime))
INSERT [dbo].[WfProcess] ([ID], [ProcessGUID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (941, N'a1b35862-4a5e-45e6-ba2e-8c2ed326c102', N'1', N'Parallel_Process_Name_9482', N'Process_Code_9482', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn2:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL" xmlns:sf="http://www.slickflow.com/schema/sf" id="bpmn2-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn2:process id="Process_100" sf:guid="a1b35862-4a5e-45e6-ba2e-8c2ed326c102" sf:code="Process_Code_9482" name="Process_Name_9482" isExecutable="false" version="1">
    <bpmn2:startEvent id="StartEvent_1" sf:guid="fb23dee4-cc03-408d-86bb-c1fa93e92044">
      <bpmn2:outgoing>Flow_1gv76yx</bpmn2:outgoing>
    </bpmn2:startEvent>
    <bpmn2:task id="Activity_1ppfeyr" sf:guid="e3b212d4-0a0d-413e-d355-e4427acf7cd7" name="submit order">
      <bpmn2:incoming>Flow_1gv76yx</bpmn2:incoming>
      <bpmn2:outgoing>Flow_18z4mru</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_1gv76yx" sf:guid="a05a2c0c-030a-4a8b-dd4b-38511dd7400b" sourceRef="StartEvent_1" targetRef="Activity_1ppfeyr" sf:from="fb23dee4-cc03-408d-86bb-c1fa93e92044" sf:to="e3b212d4-0a0d-413e-d355-e4427acf7cd7" />
    <bpmn2:sequenceFlow id="Flow_18z4mru" sf:guid="1a679d7c-99fc-443c-aaaf-ce0190a59aaa" sourceRef="Activity_1ppfeyr" targetRef="Gateway_0wg821j" sf:from="e3b212d4-0a0d-413e-d355-e4427acf7cd7" sf:to="18bb81aa-1530-488a-b565-da7894654bbd" />
    <bpmn2:task id="Activity_0moxgs4" sf:guid="99924678-c13e-4579-952f-3a704817d3d2" name="hotel">
      <bpmn2:incoming>Flow_1pqagwm</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0ztdy09</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_1pqagwm" sf:guid="30f5d44c-885e-4117-960f-5b822fffb8ba" sourceRef="Gateway_0wg821j" targetRef="Activity_0moxgs4" sf:from="18bb81aa-1530-488a-b565-da7894654bbd" sf:to="99924678-c13e-4579-952f-3a704817d3d2" />
    <bpmn2:task id="Activity_07bnjkj" sf:guid="10f2b828-00c5-400a-d81f-051df3e1b731" name="air plane">
      <bpmn2:incoming>Flow_1v4t1nd</bpmn2:incoming>
      <bpmn2:outgoing>Flow_09p5vcz</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_1v4t1nd" sf:guid="40a4b1ce-7e48-407e-abef-c1a333b4d80a" sourceRef="Gateway_0wg821j" targetRef="Activity_07bnjkj" sf:from="18bb81aa-1530-488a-b565-da7894654bbd" sf:to="10f2b828-00c5-400a-d81f-051df3e1b731" />
    <bpmn2:sequenceFlow id="Flow_0ztdy09" sf:guid="cf542298-abbf-4da0-f436-243f50375e4b" sourceRef="Activity_0moxgs4" targetRef="Gateway_13msrpm" sf:from="99924678-c13e-4579-952f-3a704817d3d2" sf:to="982529db-4ea9-4c1a-862c-ee55ea489c77" />
    <bpmn2:sequenceFlow id="Flow_09p5vcz" sf:guid="da15b554-fe02-46f8-dcbd-450c98ad1828" sourceRef="Activity_07bnjkj" targetRef="Gateway_13msrpm" sf:from="10f2b828-00c5-400a-d81f-051df3e1b731" sf:to="982529db-4ea9-4c1a-862c-ee55ea489c77" />
    <bpmn2:task id="Activity_0al1ht8" sf:guid="231fb3e7-3929-4290-8af1-92cef6e1aaff" name="letsgo">
      <bpmn2:incoming>Flow_0dip1go</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1k1gidj</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_0dip1go" sf:guid="4483bbe4-9bdb-448a-d63b-ba711599fdad" sourceRef="Gateway_13msrpm" targetRef="Activity_0al1ht8" sf:from="982529db-4ea9-4c1a-862c-ee55ea489c77" sf:to="231fb3e7-3929-4290-8af1-92cef6e1aaff" />
    <bpmn2:endEvent id="Event_065w21k" sf:guid="e21c8c08-79e1-42ce-ac27-9a7626732391">
      <bpmn2:incoming>Flow_1k1gidj</bpmn2:incoming>
    </bpmn2:endEvent>
    <bpmn2:sequenceFlow id="Flow_1k1gidj" sf:guid="86c4873a-84ad-4455-c2c0-39a2efbe2c24" sourceRef="Activity_0al1ht8" targetRef="Event_065w21k" sf:from="231fb3e7-3929-4290-8af1-92cef6e1aaff" sf:to="e21c8c08-79e1-42ce-ac27-9a7626732391" />
    <bpmn2:parallelGateway id="Gateway_0wg821j" sf:guid="18bb81aa-1530-488a-b565-da7894654bbd">
      <bpmn2:incoming>Flow_18z4mru</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1pqagwm</bpmn2:outgoing>
      <bpmn2:outgoing>Flow_1v4t1nd</bpmn2:outgoing>
    </bpmn2:parallelGateway>
    <bpmn2:parallelGateway id="Gateway_13msrpm" sf:guid="982529db-4ea9-4c1a-862c-ee55ea489c77">
      <bpmn2:incoming>Flow_0ztdy09</bpmn2:incoming>
      <bpmn2:incoming>Flow_09p5vcz</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0dip1go</bpmn2:outgoing>
    </bpmn2:parallelGateway>
  </bpmn2:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_100">
      <bpmndi:BPMNEdge id="Flow_1k1gidj_di" bpmnElement="Flow_1k1gidj">
        <di:waypoint x="1190" y="258" />
        <di:waypoint x="1292" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0dip1go_di" bpmnElement="Flow_0dip1go">
        <di:waypoint x="995" y="258" />
        <di:waypoint x="1090" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_09p5vcz_di" bpmnElement="Flow_09p5vcz">
        <di:waypoint x="860" y="370" />
        <di:waypoint x="970" y="370" />
        <di:waypoint x="970" y="283" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0ztdy09_di" bpmnElement="Flow_0ztdy09">
        <di:waypoint x="850" y="140" />
        <di:waypoint x="970" y="140" />
        <di:waypoint x="970" y="233" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1v4t1nd_di" bpmnElement="Flow_1v4t1nd">
        <di:waypoint x="680" y="283" />
        <di:waypoint x="680" y="370" />
        <di:waypoint x="760" y="370" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1pqagwm_di" bpmnElement="Flow_1pqagwm">
        <di:waypoint x="680" y="233" />
        <di:waypoint x="680" y="140" />
        <di:waypoint x="750" y="140" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_18z4mru_di" bpmnElement="Flow_18z4mru">
        <di:waypoint x="600" y="258" />
        <di:waypoint x="655" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1gv76yx_di" bpmnElement="Flow_1gv76yx">
        <di:waypoint x="408" y="258" />
        <di:waypoint x="500" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="372" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1ppfeyr_di" bpmnElement="Activity_1ppfeyr">
        <dc:Bounds x="500" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0moxgs4_di" bpmnElement="Activity_0moxgs4">
        <dc:Bounds x="750" y="100" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_07bnjkj_di" bpmnElement="Activity_07bnjkj">
        <dc:Bounds x="760" y="330" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0al1ht8_di" bpmnElement="Activity_0al1ht8">
        <dc:Bounds x="1090" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_065w21k_di" bpmnElement="Event_065w21k">
        <dc:Bounds x="1292" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Gateway_191i8xf_di" bpmnElement="Gateway_0wg821j">
        <dc:Bounds x="655" y="233" width="50" height="50" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Gateway_04o9i0w_di" bpmnElement="Gateway_13msrpm">
        <dc:Bounds x="945" y="233" width="50" height="50" />
      </bpmndi:BPMNShape>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn2:definitions>
', 0, NULL, N'', 0, NULL, CAST(0x0000AEB1011C6DD2 AS DateTime), CAST(0x0000AEB1011FF2F8 AS DateTime))
INSERT [dbo].[WfProcess] ([ID], [ProcessGUID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (942, N'bd527e3e-b4e4-49be-a781-b4fff400d64d', N'1', N'Or_Process_Name_3300', N'Process_Code_3300', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn2:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL" xmlns:sf="http://www.slickflow.com/schema/sf" id="bpmn2-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn2:process id="Process_100" sf:guid="bd527e3e-b4e4-49be-a781-b4fff400d64d" sf:code="Process_Code_3300" name="Process_Name_3300" isExecutable="false" version="1">
    <bpmn2:startEvent id="StartEvent_1" sf:guid="35661a9a-d358-4d3d-bcb6-591db72ba009">
      <bpmn2:outgoing>Flow_0ffysfc</bpmn2:outgoing>
    </bpmn2:startEvent>
    <bpmn2:task id="Activity_1j10j8l" sf:guid="ad95fd2a-f562-40f6-cca8-f70816bf2e4c" name="submit">
      <bpmn2:incoming>Flow_0ffysfc</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0q4icsx</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_0ffysfc" sf:guid="441546cf-1ab0-4323-cc86-101608d0b316" sourceRef="StartEvent_1" targetRef="Activity_1j10j8l" sf:from="35661a9a-d358-4d3d-bcb6-591db72ba009" sf:to="ad95fd2a-f562-40f6-cca8-f70816bf2e4c" />
    <bpmn2:sequenceFlow id="Flow_0q4icsx" sf:guid="b2716346-0481-4330-f461-02899b8da632" sourceRef="Activity_1j10j8l" targetRef="Gateway_1ge8uxv" sf:from="ad95fd2a-f562-40f6-cca8-f70816bf2e4c" sf:to="a29e6f33-ef95-42e0-dd90-6a86ea7218b4" />
    <bpmn2:task id="Activity_1fzfeau" sf:guid="c63c4a39-2346-4b6d-9525-18042805e71a" name="A">
      <bpmn2:incoming>Flow_1sxwlf1</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1ovm48c</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_1sxwlf1" sf:guid="fe0d86d6-da67-4be6-dd40-5332d0c9ca53" name="money &#62; 2000" sourceRef="Gateway_1ge8uxv" targetRef="Activity_1fzfeau" sf:from="a29e6f33-ef95-42e0-dd90-6a86ea7218b4" sf:to="c63c4a39-2346-4b6d-9525-18042805e71a">
      <bpmn2:conditionExpression>money&gt;2000</bpmn2:conditionExpression>
    </bpmn2:sequenceFlow>
    <bpmn2:task id="Activity_1052ejc" sf:guid="1b9feaa5-0603-41e2-9686-dd68c0c4fd76" name="C">
      <bpmn2:incoming>Flow_1hzqdc6</bpmn2:incoming>
      <bpmn2:outgoing>Flow_08modhp</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_1hzqdc6" sf:guid="f2f2e0f1-01d0-4b24-d7cf-e32d97cda182" name="city=&#34;beijing&#34;" sourceRef="Gateway_1ge8uxv" targetRef="Activity_1052ejc" sf:from="a29e6f33-ef95-42e0-dd90-6a86ea7218b4" sf:to="1b9feaa5-0603-41e2-9686-dd68c0c4fd76">
      <bpmn2:conditionExpression>city="beijing"</bpmn2:conditionExpression>
    </bpmn2:sequenceFlow>
    <bpmn2:task id="Activity_1vhfzhy" sf:guid="b4b34884-db35-4097-8713-f88ae3a2fc7e" name="B">
      <bpmn2:incoming>Flow_0yylaa4</bpmn2:incoming>
      <bpmn2:outgoing>Flow_05qkhmq</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_0yylaa4" sf:guid="38b4eba7-d4f7-4b4a-e9bb-ff718d183758" name="age&#60;28" sourceRef="Gateway_1ge8uxv" targetRef="Activity_1vhfzhy" sf:from="a29e6f33-ef95-42e0-dd90-6a86ea7218b4" sf:to="b4b34884-db35-4097-8713-f88ae3a2fc7e">
      <bpmn2:conditionExpression>age &gt; 28</bpmn2:conditionExpression>
    </bpmn2:sequenceFlow>
    <bpmn2:sequenceFlow id="Flow_1ovm48c" sf:guid="abd535d4-64f4-44b9-8491-bb52618d92ca" sourceRef="Activity_1fzfeau" targetRef="Gateway_10slfim" sf:from="c63c4a39-2346-4b6d-9525-18042805e71a" sf:to="1b4ff14e-c9e3-4d4f-db65-9a768d9949c7" />
    <bpmn2:sequenceFlow id="Flow_05qkhmq" sf:guid="fe22ef40-5c9e-45a7-fec3-725f36016b9b" sourceRef="Activity_1vhfzhy" targetRef="Gateway_10slfim" sf:from="b4b34884-db35-4097-8713-f88ae3a2fc7e" sf:to="1b4ff14e-c9e3-4d4f-db65-9a768d9949c7" />
    <bpmn2:sequenceFlow id="Flow_08modhp" sf:guid="4d8e88a3-aead-4c41-94c9-4188ccd917ee" sourceRef="Activity_1052ejc" targetRef="Gateway_10slfim" sf:from="1b9feaa5-0603-41e2-9686-dd68c0c4fd76" sf:to="1b4ff14e-c9e3-4d4f-db65-9a768d9949c7" />
    <bpmn2:task id="Activity_0rfssjl" sf:guid="407f74a3-178f-4ed6-ca52-fa7be7f35632" name="agree">
      <bpmn2:incoming>Flow_0exzivr</bpmn2:incoming>
      <bpmn2:outgoing>Flow_051nz4w</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_0exzivr" sf:guid="cf8fe877-11e2-4273-9335-4f01965468de" sourceRef="Gateway_10slfim" targetRef="Activity_0rfssjl" sf:from="1b4ff14e-c9e3-4d4f-db65-9a768d9949c7" sf:to="407f74a3-178f-4ed6-ca52-fa7be7f35632" />
    <bpmn2:endEvent id="Event_1s3x2jj" sf:guid="e7aae68d-8f7b-4aad-f215-d22558f47127">
      <bpmn2:incoming>Flow_051nz4w</bpmn2:incoming>
    </bpmn2:endEvent>
    <bpmn2:sequenceFlow id="Flow_051nz4w" sf:guid="2c0c0037-8333-4c3e-d06d-0d70c2c6ae1c" sourceRef="Activity_0rfssjl" targetRef="Event_1s3x2jj" sf:from="407f74a3-178f-4ed6-ca52-fa7be7f35632" sf:to="e7aae68d-8f7b-4aad-f215-d22558f47127" />
    <bpmn2:inclusiveGateway id="Gateway_1ge8uxv" sf:guid="a29e6f33-ef95-42e0-dd90-6a86ea7218b4">
      <bpmn2:incoming>Flow_0q4icsx</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1sxwlf1</bpmn2:outgoing>
      <bpmn2:outgoing>Flow_1hzqdc6</bpmn2:outgoing>
      <bpmn2:outgoing>Flow_0yylaa4</bpmn2:outgoing>
    </bpmn2:inclusiveGateway>
    <bpmn2:inclusiveGateway id="Gateway_10slfim" sf:guid="1b4ff14e-c9e3-4d4f-db65-9a768d9949c7">
      <bpmn2:incoming>Flow_1ovm48c</bpmn2:incoming>
      <bpmn2:incoming>Flow_05qkhmq</bpmn2:incoming>
      <bpmn2:incoming>Flow_08modhp</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0exzivr</bpmn2:outgoing>
    </bpmn2:inclusiveGateway>
  </bpmn2:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_100">
      <bpmndi:BPMNEdge id="Flow_0ffysfc_di" bpmnElement="Flow_0ffysfc">
        <di:waypoint x="408" y="258" />
        <di:waypoint x="500" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0q4icsx_di" bpmnElement="Flow_0q4icsx">
        <di:waypoint x="600" y="258" />
        <di:waypoint x="655" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1sxwlf1_di" bpmnElement="Flow_1sxwlf1">
        <di:waypoint x="680" y="233" />
        <di:waypoint x="680" y="80" />
        <di:waypoint x="760" y="80" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="661" y="154" width="69" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1hzqdc6_di" bpmnElement="Flow_1hzqdc6">
        <di:waypoint x="680" y="283" />
        <di:waypoint x="680" y="420" />
        <di:waypoint x="760" y="420" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="664" y="349" width="62" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0yylaa4_di" bpmnElement="Flow_0yylaa4">
        <di:waypoint x="705" y="258" />
        <di:waypoint x="760" y="258" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="714" y="240" width="37" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1ovm48c_di" bpmnElement="Flow_1ovm48c">
        <di:waypoint x="860" y="80" />
        <di:waypoint x="960" y="80" />
        <di:waypoint x="960" y="233" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_05qkhmq_di" bpmnElement="Flow_05qkhmq">
        <di:waypoint x="860" y="258" />
        <di:waypoint x="935" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_08modhp_di" bpmnElement="Flow_08modhp">
        <di:waypoint x="860" y="420" />
        <di:waypoint x="960" y="420" />
        <di:waypoint x="960" y="283" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0exzivr_di" bpmnElement="Flow_0exzivr">
        <di:waypoint x="985" y="258" />
        <di:waypoint x="1070" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_051nz4w_di" bpmnElement="Flow_051nz4w">
        <di:waypoint x="1170" y="258" />
        <di:waypoint x="1262" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNShape id="Activity_1j10j8l_di" bpmnElement="Activity_1j10j8l">
        <dc:Bounds x="500" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1052ejc_di" bpmnElement="Activity_1052ejc">
        <dc:Bounds x="760" y="380" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1vhfzhy_di" bpmnElement="Activity_1vhfzhy">
        <dc:Bounds x="760" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0rfssjl_di" bpmnElement="Activity_0rfssjl">
        <dc:Bounds x="1070" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_1s3x2jj_di" bpmnElement="Event_1s3x2jj">
        <dc:Bounds x="1262" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="372" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1fzfeau_di" bpmnElement="Activity_1fzfeau">
        <dc:Bounds x="760" y="40" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Gateway_1amo7lr_di" bpmnElement="Gateway_1ge8uxv">
        <dc:Bounds x="655" y="233" width="50" height="50" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Gateway_135tjjr_di" bpmnElement="Gateway_10slfim">
        <dc:Bounds x="935" y="233" width="50" height="50" />
      </bpmndi:BPMNShape>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn2:definitions>
', 0, NULL, N'', 0, NULL, CAST(0x0000AEB1012050BF AS DateTime), CAST(0x0000AEB10120F52B AS DateTime))
INSERT [dbo].[WfProcess] ([ID], [ProcessGUID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (943, N'4d658eee-e32f-439e-8b71-4fb5a0882c8e', N'1', N'Sub_Process_Name_2740', N'Process_Code_2740', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn2:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL" xmlns:sf="http://www.slickflow.com/schema/sf" id="bpmn2-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn2:process id="Process_100" sf:guid="4d658eee-e32f-439e-8b71-4fb5a0882c8e" sf:code="Process_Code_2740" name="Process_Name_2740" isExecutable="false" version="1">
    <bpmn2:startEvent id="StartEvent_1" sf:guid="5adccfc4-b5d6-4b43-a43a-63908c65cd49">
      <bpmn2:outgoing>Flow_10ciiqz</bpmn2:outgoing>
    </bpmn2:startEvent>
    <bpmn2:task id="Activity_17s8v3d" sf:guid="9a60060c-8325-453e-8c0f-049c093c4a97" name="main submit">
      <bpmn2:incoming>Flow_10ciiqz</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1hzp2c5</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_10ciiqz" sf:guid="95ba568a-dd9e-4d33-ecd6-c64a934faadd" sourceRef="StartEvent_1" targetRef="Activity_17s8v3d" sf:from="5adccfc4-b5d6-4b43-a43a-63908c65cd49" sf:to="9a60060c-8325-453e-8c0f-049c093c4a97" />
    <bpmn2:sequenceFlow id="Flow_1hzp2c5" sf:guid="88b2458b-b0dd-401c-a746-19f3887d8369" sourceRef="Activity_17s8v3d" targetRef="Activity_0vg5f3g" sf:from="9a60060c-8325-453e-8c0f-049c093c4a97" sf:to="dec65b31-31c8-416d-c986-d193aa2d1000" />
    <bpmn2:subProcess id="Activity_0vg5f3g" sf:guid="dec65b31-31c8-416d-c986-d193aa2d1000" name="order check">
      <bpmn2:incoming>Flow_1hzp2c5</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1u4a6n7</bpmn2:outgoing>
      <bpmn2:startEvent id="Event_1lwijr3" sf:guid="7d617290-e3aa-40ba-8cab-94b3ccdc9ffb">
        <bpmn2:outgoing>Flow_0hxdo4y</bpmn2:outgoing>
      </bpmn2:startEvent>
      <bpmn2:task id="Activity_0it4pk4" sf:guid="f88c87b5-34c5-4723-8080-3329f91da48d" name="sub order">
        <bpmn2:incoming>Flow_0hxdo4y</bpmn2:incoming>
        <bpmn2:outgoing>Flow_1xw67qt</bpmn2:outgoing>
      </bpmn2:task>
      <bpmn2:sequenceFlow id="Flow_0hxdo4y" sf:guid="ff48105e-9fc0-4bde-ba11-3e970d459b3b" sourceRef="Event_1lwijr3" targetRef="Activity_0it4pk4" sf:from="7d617290-e3aa-40ba-8cab-94b3ccdc9ffb" sf:to="f88c87b5-34c5-4723-8080-3329f91da48d" />
      <bpmn2:task id="Activity_0jkc2m1" sf:guid="317d702c-109d-4a17-e47f-897c04986de0" name="sub payment">
        <bpmn2:incoming>Flow_1xw67qt</bpmn2:incoming>
        <bpmn2:outgoing>Flow_1dgmmfv</bpmn2:outgoing>
      </bpmn2:task>
      <bpmn2:sequenceFlow id="Flow_1xw67qt" sf:guid="45088e54-594a-493d-c900-6147d4202c89" sourceRef="Activity_0it4pk4" targetRef="Activity_0jkc2m1" sf:from="f88c87b5-34c5-4723-8080-3329f91da48d" sf:to="317d702c-109d-4a17-e47f-897c04986de0" />
      <bpmn2:endEvent id="Event_049i66g" sf:guid="fc6bb23e-2701-4c0c-b3df-76fbca620996">
        <bpmn2:incoming>Flow_1dgmmfv</bpmn2:incoming>
      </bpmn2:endEvent>
      <bpmn2:sequenceFlow id="Flow_1dgmmfv" sf:guid="6688152c-40b2-45a1-e095-c3cc1428ed0d" sourceRef="Activity_0jkc2m1" targetRef="Event_049i66g" sf:from="317d702c-109d-4a17-e47f-897c04986de0" sf:to="fc6bb23e-2701-4c0c-b3df-76fbca620996" />
    </bpmn2:subProcess>
    <bpmn2:task id="Activity_1xe2kd5" sf:guid="f8c1d57a-504a-447d-9d9c-bcd38b74cdf5" name="dept approval">
      <bpmn2:incoming>Flow_1u4a6n7</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0925eqi</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_1u4a6n7" sf:guid="8bf276f5-6bab-4164-971d-d1d7ed061c3f" sourceRef="Activity_0vg5f3g" targetRef="Activity_1xe2kd5" sf:from="dec65b31-31c8-416d-c986-d193aa2d1000" sf:to="f8c1d57a-504a-447d-9d9c-bcd38b74cdf5" />
    <bpmn2:endEvent id="Event_1vt1rub" sf:guid="88a1c907-d35d-45d1-c9e1-c647aab1a2c0">
      <bpmn2:incoming>Flow_0925eqi</bpmn2:incoming>
    </bpmn2:endEvent>
    <bpmn2:sequenceFlow id="Flow_0925eqi" sf:guid="275c9c15-a317-4dae-800c-78ea0ae0fe20" sourceRef="Activity_1xe2kd5" targetRef="Event_1vt1rub" sf:from="f8c1d57a-504a-447d-9d9c-bcd38b74cdf5" sf:to="88a1c907-d35d-45d1-c9e1-c647aab1a2c0" />
  </bpmn2:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_100">
      <bpmndi:BPMNEdge id="Flow_10ciiqz_di" bpmnElement="Flow_10ciiqz">
        <di:waypoint x="448" y="258" />
        <di:waypoint x="500" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1hzp2c5_di" bpmnElement="Flow_1hzp2c5">
        <di:waypoint x="600" y="258" />
        <di:waypoint x="660" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1u4a6n7_di" bpmnElement="Flow_1u4a6n7">
        <di:waypoint x="760" y="258" />
        <di:waypoint x="820" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0925eqi_di" bpmnElement="Flow_0925eqi">
        <di:waypoint x="920" y="258" />
        <di:waypoint x="982" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="412" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_17s8v3d_di" bpmnElement="Activity_17s8v3d">
        <dc:Bounds x="500" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1xe2kd5_di" bpmnElement="Activity_1xe2kd5">
        <dc:Bounds x="820" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_1vt1rub_di" bpmnElement="Event_1vt1rub">
        <dc:Bounds x="982" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1bq1rvp_di" bpmnElement="Activity_0vg5f3g">
        <dc:Bounds x="660" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
  <bpmndi:BPMNDiagram id="BPMNDiagram_0cg3ki3">
    <bpmndi:BPMNPlane id="BPMNPlane_1bgtnj8" bpmnElement="Activity_0vg5f3g">
      <bpmndi:BPMNEdge id="Flow_0hxdo4y_di" bpmnElement="Flow_0hxdo4y">
        <di:waypoint x="418" y="200" />
        <di:waypoint x="470" y="200" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1xw67qt_di" bpmnElement="Flow_1xw67qt">
        <di:waypoint x="570" y="200" />
        <di:waypoint x="630" y="200" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1dgmmfv_di" bpmnElement="Flow_1dgmmfv">
        <di:waypoint x="730" y="200" />
        <di:waypoint x="792" y="200" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNShape id="Event_1lwijr3_di" bpmnElement="Event_1lwijr3">
        <dc:Bounds x="382" y="182" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0it4pk4_di" bpmnElement="Activity_0it4pk4">
        <dc:Bounds x="470" y="160" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0jkc2m1_di" bpmnElement="Activity_0jkc2m1">
        <dc:Bounds x="630" y="160" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_049i66g_di" bpmnElement="Event_049i66g">
        <dc:Bounds x="792" y="182" width="36" height="36" />
      </bpmndi:BPMNShape>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn2:definitions>
', 0, NULL, N'', 0, NULL, CAST(0x0000AEB10121C066 AS DateTime), NULL)
INSERT [dbo].[WfProcess] ([ID], [ProcessGUID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (946, N'0787ce58-d741-4cbe-91ee-d76cc3376231', N'1', N'XOR_Process_Name_8458', N'XOR_Process_Code_8458', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn2:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:sfgb="http://www.slickflow.com/schema/sfgb" id="bpmn2-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn2:process id="Process_100" sf:guid="0787ce58-d741-4cbe-91ee-d76cc3376231" sf:code="Process_Code_8458" name="Process_Name_8458" isExecutable="false" version="1">
    <bpmn2:startEvent id="StartEvent_1" sf:guid="68bd3f97-1799-41b2-8e79-5a4ee19eec51">
      <bpmn2:outgoing>Flow_1wz3fqr</bpmn2:outgoing>
    </bpmn2:startEvent>
    <bpmn2:task id="Activity_1b4w4h2" sf:guid="aaa3345f-41c9-4b22-d708-f541d2cc71c3" name="submit">
      <bpmn2:incoming>Flow_1wz3fqr</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1gu42ip</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_1wz3fqr" sf:guid="6d56b204-3e35-49e4-92f9-15ce0848d5cc" sourceRef="StartEvent_1" targetRef="Activity_1b4w4h2" sf:from="68bd3f97-1799-41b2-8e79-5a4ee19eec51" sf:to="aaa3345f-41c9-4b22-d708-f541d2cc71c3" />
    <bpmn2:exclusiveGateway id="Gateway_0d2nw9i" sf:guid="2e9272e7-18c7-45ee-ea49-460a451ff8d1">
      <bpmn2:incoming>Flow_1gu42ip</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1m2wcn0</bpmn2:outgoing>
      <bpmn2:outgoing>Flow_0ni1ffz</bpmn2:outgoing>
      <bpmn2:outgoing>Flow_10zxzgi</bpmn2:outgoing>
    </bpmn2:exclusiveGateway>
    <bpmn2:sequenceFlow id="Flow_1gu42ip" sf:guid="ca954c7d-2444-4abb-8354-2b79ad791b27" sourceRef="Activity_1b4w4h2" targetRef="Gateway_0d2nw9i" sf:from="aaa3345f-41c9-4b22-d708-f541d2cc71c3" sf:to="2e9272e7-18c7-45ee-ea49-460a451ff8d1" />
    <bpmn2:task id="Activity_119qrx2" sf:guid="263f0967-2e74-42a4-fc37-64783f9edd51" name="CTO approval">
      <bpmn2:incoming>Flow_1m2wcn0</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1jt004b</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_1m2wcn0" sf:guid="07a0efea-9326-45ff-9783-774c29857216" name="days&#62;=3" sourceRef="Gateway_0d2nw9i" targetRef="Activity_119qrx2" sf:from="2e9272e7-18c7-45ee-ea49-460a451ff8d1" sf:to="263f0967-2e74-42a4-fc37-64783f9edd51">
      <bpmn2:extensionElements>
        <sfgb:groupBehaviours priority="0" />
      </bpmn2:extensionElements>
      <bpmn2:conditionExpression>days&gt;=3</bpmn2:conditionExpression>
    </bpmn2:sequenceFlow>
    <bpmn2:task id="Activity_0ysbo5a" sf:guid="f4392dba-4fde-4b38-af60-795e4654d832" name="CEO approval">
      <bpmn2:incoming>Flow_0ni1ffz</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1dl1rdb</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_0ni1ffz" sf:guid="29a9060f-3401-4835-aeae-746d2e06aebf" name="days&#62;=10" sourceRef="Gateway_0d2nw9i" targetRef="Activity_0ysbo5a" sf:from="2e9272e7-18c7-45ee-ea49-460a451ff8d1" sf:to="f4392dba-4fde-4b38-af60-795e4654d832">
      <bpmn2:extensionElements>
        <sfgb:groupBehaviours priority="-1" />
      </bpmn2:extensionElements>
      <bpmn2:conditionExpression>days&gt;=10</bpmn2:conditionExpression>
    </bpmn2:sequenceFlow>
    <bpmn2:task id="Activity_0i8b28s" sf:guid="cde9b392-338f-4b13-8480-844634f830a8" name="dept approval">
      <bpmn2:incoming>Flow_10zxzgi</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1066uin</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_10zxzgi" sf:guid="b82ad77c-3cc1-4148-aa57-b6e67d589b56" name="days&#62;=1" sourceRef="Gateway_0d2nw9i" targetRef="Activity_0i8b28s" sf:from="2e9272e7-18c7-45ee-ea49-460a451ff8d1" sf:to="cde9b392-338f-4b13-8480-844634f830a8">
      <bpmn2:extensionElements>
        <sfgb:groupBehaviours priority="1" />
      </bpmn2:extensionElements>
      <bpmn2:conditionExpression>days&gt;=1</bpmn2:conditionExpression>
    </bpmn2:sequenceFlow>
    <bpmn2:exclusiveGateway id="Gateway_1u2xcv8" sf:guid="3bec1338-13a8-4045-a2e7-1271639f8777">
      <bpmn2:incoming>Flow_1jt004b</bpmn2:incoming>
      <bpmn2:incoming>Flow_1066uin</bpmn2:incoming>
      <bpmn2:incoming>Flow_1dl1rdb</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1uldew1</bpmn2:outgoing>
    </bpmn2:exclusiveGateway>
    <bpmn2:sequenceFlow id="Flow_1jt004b" sf:guid="8b9f780b-e240-4e77-c96a-04c4a6308a4b" sourceRef="Activity_119qrx2" targetRef="Gateway_1u2xcv8" sf:from="263f0967-2e74-42a4-fc37-64783f9edd51" sf:to="3bec1338-13a8-4045-a2e7-1271639f8777" />
    <bpmn2:sequenceFlow id="Flow_1066uin" sf:guid="c8f61196-117d-49d8-ca5f-34641e6c7076" sourceRef="Activity_0i8b28s" targetRef="Gateway_1u2xcv8" sf:from="cde9b392-338f-4b13-8480-844634f830a8" sf:to="3bec1338-13a8-4045-a2e7-1271639f8777" />
    <bpmn2:sequenceFlow id="Flow_1dl1rdb" sf:guid="88ecb6ed-bc6b-41b7-acea-42b026aac21e" sourceRef="Activity_0ysbo5a" targetRef="Gateway_1u2xcv8" sf:from="f4392dba-4fde-4b38-af60-795e4654d832" sf:to="3bec1338-13a8-4045-a2e7-1271639f8777" />
    <bpmn2:task id="Activity_18su0gu" sf:guid="f617b619-8be7-4a97-dc37-588b7ab8f013" name="HR approval">
      <bpmn2:incoming>Flow_1uldew1</bpmn2:incoming>
      <bpmn2:outgoing>Flow_13vm6x1</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_1uldew1" sf:guid="a1bb4258-3bc1-4a95-e679-456e192eeb69" sourceRef="Gateway_1u2xcv8" targetRef="Activity_18su0gu" sf:from="3bec1338-13a8-4045-a2e7-1271639f8777" sf:to="f617b619-8be7-4a97-dc37-588b7ab8f013" />
    <bpmn2:endEvent id="Event_1ek6qub" sf:guid="62d1b82b-004e-41af-af04-8bd280dcf0fc">
      <bpmn2:incoming>Flow_13vm6x1</bpmn2:incoming>
    </bpmn2:endEvent>
    <bpmn2:sequenceFlow id="Flow_13vm6x1" sf:guid="56d35d65-f74d-4a6a-cb13-7bbbb91df09b" sourceRef="Activity_18su0gu" targetRef="Event_1ek6qub" sf:from="f617b619-8be7-4a97-dc37-588b7ab8f013" sf:to="62d1b82b-004e-41af-af04-8bd280dcf0fc" />
  </bpmn2:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_100">
      <bpmndi:BPMNEdge id="Flow_13vm6x1_di" bpmnElement="Flow_13vm6x1">
        <di:waypoint x="1120" y="258" />
        <di:waypoint x="1182" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1uldew1_di" bpmnElement="Flow_1uldew1">
        <di:waypoint x="965" y="258" />
        <di:waypoint x="1020" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1dl1rdb_di" bpmnElement="Flow_1dl1rdb">
        <di:waypoint x="860" y="410" />
        <di:waypoint x="940" y="410" />
        <di:waypoint x="940" y="283" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1066uin_di" bpmnElement="Flow_1066uin">
        <di:waypoint x="860" y="110" />
        <di:waypoint x="940" y="110" />
        <di:waypoint x="940" y="233" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1jt004b_di" bpmnElement="Flow_1jt004b">
        <di:waypoint x="860" y="258" />
        <di:waypoint x="915" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_10zxzgi_di" bpmnElement="Flow_10zxzgi">
        <di:waypoint x="680" y="233" />
        <di:waypoint x="680" y="110" />
        <di:waypoint x="760" y="110" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="674" y="169" width="43" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0ni1ffz_di" bpmnElement="Flow_0ni1ffz">
        <di:waypoint x="680" y="283" />
        <di:waypoint x="680" y="410" />
        <di:waypoint x="760" y="410" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="671" y="346" width="49" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1m2wcn0_di" bpmnElement="Flow_1m2wcn0">
        <di:waypoint x="705" y="258" />
        <di:waypoint x="760" y="258" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="711" y="240" width="43" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1gu42ip_di" bpmnElement="Flow_1gu42ip">
        <di:waypoint x="600" y="258" />
        <di:waypoint x="655" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1wz3fqr_di" bpmnElement="Flow_1wz3fqr">
        <di:waypoint x="448" y="258" />
        <di:waypoint x="500" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="412" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1b4w4h2_di" bpmnElement="Activity_1b4w4h2">
        <dc:Bounds x="500" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Gateway_0d2nw9i_di" bpmnElement="Gateway_0d2nw9i" isMarkerVisible="true">
        <dc:Bounds x="655" y="233" width="50" height="50" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_119qrx2_di" bpmnElement="Activity_119qrx2">
        <dc:Bounds x="760" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0i8b28s_di" bpmnElement="Activity_0i8b28s">
        <dc:Bounds x="760" y="70" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Gateway_1u2xcv8_di" bpmnElement="Gateway_1u2xcv8" isMarkerVisible="true">
        <dc:Bounds x="915" y="233" width="50" height="50" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_18su0gu_di" bpmnElement="Activity_18su0gu">
        <dc:Bounds x="1020" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_1ek6qub_di" bpmnElement="Event_1ek6qub">
        <dc:Bounds x="1182" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0ysbo5a_di" bpmnElement="Activity_0ysbo5a">
        <dc:Bounds x="760" y="370" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn2:definitions>
', 0, NULL, N'', 0, NULL, CAST(0x0000AEB500E0A76E AS DateTime), CAST(0x0000AEB500E6AE32 AS DateTime))
INSERT [dbo].[WfProcess] ([ID], [ProcessGUID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (947, N'46e5a383-af1d-4ab3-947e-a7a1ad35bb8e', N'1', N'Process_Name_5606', N'Process_Code_5606', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn2:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL" xmlns:sf="http://www.slickflow.com/schema/sf" id="bpmn2-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn2:process id="Process_100" sf:guid="46e5a383-af1d-4ab3-947e-a7a1ad35bb8e" sf:code="Process_Code_5606" name="Process_Name_5606" isExecutable="false" version="1">
    <bpmn2:startEvent id="StartEvent_1" sf:guid="ed34b7b4-0867-472d-bdd8-acefb78e12b5">
      <bpmn2:outgoing>Flow_09zzqck</bpmn2:outgoing>
    </bpmn2:startEvent>
    <bpmn2:task id="Activity_0yjv0g4" sf:guid="08f1ff80-7383-4824-e826-09d891f3f31f">
      <bpmn2:incoming>Flow_09zzqck</bpmn2:incoming>
      <bpmn2:outgoing>Flow_18uc6he</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_09zzqck" sf:guid="d5d41cf0-b1da-435a-ab5e-1db9c19e9ce6" sourceRef="StartEvent_1" targetRef="Activity_0yjv0g4" sf:from="ed34b7b4-0867-472d-bdd8-acefb78e12b5" sf:to="08f1ff80-7383-4824-e826-09d891f3f31f" />
    <bpmn2:sequenceFlow id="Flow_18uc6he" sf:guid="06e0fcc2-75ce-47f1-cc8f-446c66413570" sourceRef="Activity_0yjv0g4" targetRef="Gateway_1wmcd38" sf:from="08f1ff80-7383-4824-e826-09d891f3f31f" sf:to="8fcb037c-6f20-4feb-8add-9365b71d00de" />
    <bpmn2:inclusiveGateway id="Gateway_1wmcd38" sf:guid="8fcb037c-6f20-4feb-8add-9365b71d00de">
      <bpmn2:incoming>Flow_18uc6he</bpmn2:incoming>
    </bpmn2:inclusiveGateway>
  </bpmn2:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_100">
      <bpmndi:BPMNEdge id="Flow_09zzqck_di" bpmnElement="Flow_09zzqck">
        <di:waypoint x="448" y="258" />
        <di:waypoint x="500" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_18uc6he_di" bpmnElement="Flow_18uc6he">
        <di:waypoint x="600" y="258" />
        <di:waypoint x="655" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="412" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0yjv0g4_di" bpmnElement="Activity_0yjv0g4">
        <dc:Bounds x="500" y="218" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Gateway_0j0hzwc_di" bpmnElement="Gateway_1wmcd38">
        <dc:Bounds x="655" y="233" width="50" height="50" />
      </bpmndi:BPMNShape>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn2:definitions>
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        ', 0, NULL, NULL, 0, NULL, CAST(0x0000AEB501328FC0 AS DateTime), NULL)
INSERT [dbo].[WfProcess] ([ID], [ProcessGUID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (948, N'7d84eb68-9bfe-416a-9958-c6391ab5e649', N'1', N'EOrJoin Process', N'Process_Code_1597', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn2:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:sfat="http://www.slickflow.com/schema/sfat" xmlns:sfgb="http://www.slickflow.com/schema/sfgb" id="bpmn2-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn2:process id="Process_100" sf:guid="7d84eb68-9bfe-416a-9958-c6391ab5e649" sf:code="Process_Code_1597" name="EOrJoin Process" isExecutable="false" version="1">
    <bpmn2:startEvent id="StartEvent_1" sf:guid="a19131fc-011b-41fc-a18a-f973a9bd1625">
      <bpmn2:outgoing>Flow_1df7e0h</bpmn2:outgoing>
    </bpmn2:startEvent>
    <bpmn2:task id="Activity_069rl4f" sf:guid="dba6a232-9b2a-465a-bc39-20c720f5fcab" name="submit">
      <bpmn2:incoming>Flow_1df7e0h</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0luzaha</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_1df7e0h" sf:guid="3133f12d-cbf8-4d9e-d1da-8abc9da086f6" sourceRef="StartEvent_1" targetRef="Activity_069rl4f" sf:from="a19131fc-011b-41fc-a18a-f973a9bd1625" sf:to="dba6a232-9b2a-465a-bc39-20c720f5fcab" />
    <bpmn2:sequenceFlow id="Flow_0luzaha" sf:guid="8540df18-3aa7-4fe5-afba-635eec48ad1a" sourceRef="Activity_069rl4f" targetRef="Gateway_1oq8ofu" sf:from="dba6a232-9b2a-465a-bc39-20c720f5fcab" sf:to="dadfbf3c-ead7-4c08-9535-6f6c65390632" />
    <bpmn2:task id="Activity_16xfym4" sf:guid="59522b86-31ef-4cf3-8857-7741536d27d0" name="finace apply">
      <bpmn2:incoming>Flow_0higuqp</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1xv3l3t</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_0higuqp" sf:guid="c72fda58-bf0c-4f3d-8f45-ffc3dcaab5d1" name="money&#62;=100" sourceRef="Gateway_1oq8ofu" targetRef="Activity_16xfym4" sf:from="dadfbf3c-ead7-4c08-9535-6f6c65390632" sf:to="59522b86-31ef-4cf3-8857-7741536d27d0">
      <bpmn2:conditionExpression>money&gt;=1000</bpmn2:conditionExpression>
    </bpmn2:sequenceFlow>
    <bpmn2:task id="Activity_19kj0h0" sf:guid="8cb5aff1-89b3-4952-b6d6-c3a8a7fd1f57" name="human apply">
      <bpmn2:incoming>Flow_11rl5oq</bpmn2:incoming>
      <bpmn2:outgoing>Flow_13px652</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_11rl5oq" sf:guid="a45f9ac2-4c8a-4608-bbc2-a72e48bdceb8" name="person&#62;=5" sourceRef="Gateway_1oq8ofu" targetRef="Activity_19kj0h0" sf:from="dadfbf3c-ead7-4c08-9535-6f6c65390632" sf:to="8cb5aff1-89b3-4952-b6d6-c3a8a7fd1f57">
      <bpmn2:conditionExpression>person&gt;=5</bpmn2:conditionExpression>
    </bpmn2:sequenceFlow>
    <bpmn2:task id="Activity_1tin5zd" sf:guid="c94508ad-ca20-47bf-fd86-5150de617e8f" name="factory apply">
      <bpmn2:incoming>Flow_1pprdki</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0q3ia22</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_1pprdki" sf:guid="894c8b08-7f85-4d11-90fb-eeef25c5f888" name="days&#62;=3" sourceRef="Gateway_1oq8ofu" targetRef="Activity_1tin5zd" sf:from="dadfbf3c-ead7-4c08-9535-6f6c65390632" sf:to="c94508ad-ca20-47bf-fd86-5150de617e8f">
      <bpmn2:conditionExpression>days&gt;=3</bpmn2:conditionExpression>
    </bpmn2:sequenceFlow>
    <bpmn2:sequenceFlow id="Flow_0q3ia22" sf:guid="8ccd1157-1cab-440d-87f6-e99f2f0fd427" sourceRef="Activity_1tin5zd" targetRef="Gateway_1yurpx8" sf:from="c94508ad-ca20-47bf-fd86-5150de617e8f" sf:to="84a732cb-617f-4d10-8be4-39073b37d5f2" />
    <bpmn2:sequenceFlow id="Flow_1xv3l3t" sf:guid="cbe65f38-915d-4046-b959-ff3b4809351d" sourceRef="Activity_16xfym4" targetRef="Gateway_1yurpx8" sf:from="59522b86-31ef-4cf3-8857-7741536d27d0" sf:to="84a732cb-617f-4d10-8be4-39073b37d5f2">
      <bpmn2:extensionElements>
        <sfgb:groupBehaviours forcedMerge="true" />
      </bpmn2:extensionElements>
    </bpmn2:sequenceFlow>
    <bpmn2:sequenceFlow id="Flow_13px652" sf:guid="ae532e68-b34a-4007-cb28-91fb7226fad5" sourceRef="Activity_19kj0h0" targetRef="Gateway_1yurpx8" sf:from="8cb5aff1-89b3-4952-b6d6-c3a8a7fd1f57" sf:to="84a732cb-617f-4d10-8be4-39073b37d5f2">
      <bpmn2:extensionElements>
        <sf:groupBehaviours forcedMerge="true" />
      </bpmn2:extensionElements>
    </bpmn2:sequenceFlow>
    <bpmn2:task id="Activity_0pvxvon" sf:guid="520ed583-315c-40f0-c099-0fa5e58ebb71" name="boss approval">
      <bpmn2:incoming>Flow_195gcmh</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1yjau8f</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_195gcmh" sf:guid="7d9bd9c3-437c-401f-a87e-5e1eb416b470" sourceRef="Gateway_1yurpx8" targetRef="Activity_0pvxvon" sf:from="84a732cb-617f-4d10-8be4-39073b37d5f2" sf:to="520ed583-315c-40f0-c099-0fa5e58ebb71" />
    <bpmn2:endEvent id="Event_1olbpop" sf:guid="0c43558a-e821-4bf5-c23e-215a3c95350c">
      <bpmn2:incoming>Flow_1yjau8f</bpmn2:incoming>
    </bpmn2:endEvent>
    <bpmn2:sequenceFlow id="Flow_1yjau8f" sf:guid="98a42cf3-42d1-4bb4-c69a-9bc1f99e58d6" sourceRef="Activity_0pvxvon" targetRef="Event_1olbpop" sf:from="520ed583-315c-40f0-c099-0fa5e58ebb71" sf:to="0c43558a-e821-4bf5-c23e-215a3c95350c" />
    <bpmn2:inclusiveGateway id="Gateway_1oq8ofu" sf:guid="dadfbf3c-ead7-4c08-9535-6f6c65390632">
      <bpmn2:incoming>Flow_0luzaha</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0higuqp</bpmn2:outgoing>
      <bpmn2:outgoing>Flow_11rl5oq</bpmn2:outgoing>
      <bpmn2:outgoing>Flow_1pprdki</bpmn2:outgoing>
    </bpmn2:inclusiveGateway>
    <bpmn2:inclusiveGateway id="Gateway_1yurpx8" sf:guid="84a732cb-617f-4d10-8be4-39073b37d5f2">
      <bpmn2:extensionElements>
        <sfat:activityTypeDetail extraJoinType="EOrJoin" joinPassType="Forced" />
        <sf:gatewayDetail extraJoinType="EOrJoin" joinPassType="Count" />
      </bpmn2:extensionElements>
      <bpmn2:incoming>Flow_0q3ia22</bpmn2:incoming>
      <bpmn2:incoming>Flow_1xv3l3t</bpmn2:incoming>
      <bpmn2:incoming>Flow_13px652</bpmn2:incoming>
      <bpmn2:outgoing>Flow_195gcmh</bpmn2:outgoing>
    </bpmn2:inclusiveGateway>
  </bpmn2:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_100">
      <bpmndi:BPMNEdge id="Flow_1yjau8f_di" bpmnElement="Flow_1yjau8f">
        <di:waypoint x="1140" y="258" />
        <di:waypoint x="1212" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_195gcmh_di" bpmnElement="Flow_195gcmh">
        <di:waypoint x="985" y="258" />
        <di:waypoint x="1040" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_13px652_di" bpmnElement="Flow_13px652">
        <di:waypoint x="860" y="370" />
        <di:waypoint x="960" y="370" />
        <di:waypoint x="960" y="283" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1xv3l3t_di" bpmnElement="Flow_1xv3l3t">
        <di:waypoint x="860" y="258" />
        <di:waypoint x="935" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0q3ia22_di" bpmnElement="Flow_0q3ia22">
        <di:waypoint x="860" y="130" />
        <di:waypoint x="960" y="130" />
        <di:waypoint x="960" y="233" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1pprdki_di" bpmnElement="Flow_1pprdki">
        <di:waypoint x="680" y="233" />
        <di:waypoint x="680" y="130" />
        <di:waypoint x="760" y="130" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="674" y="179" width="43" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_11rl5oq_di" bpmnElement="Flow_11rl5oq">
        <di:waypoint x="680" y="283" />
        <di:waypoint x="680" y="370" />
        <di:waypoint x="760" y="370" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="669" y="324" width="53" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0higuqp_di" bpmnElement="Flow_0higuqp">
        <di:waypoint x="705" y="258" />
        <di:waypoint x="760" y="258" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="701" y="240" width="63" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0luzaha_di" bpmnElement="Flow_0luzaha">
        <di:waypoint x="600" y="258" />
        <di:waypoint x="655" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1df7e0h_di" bpmnElement="Flow_1df7e0h">
        <di:waypoint x="448" y="258" />
        <di:waypoint x="500" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="412" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_069rl4f_di" bpmnElement="Activity_069rl4f">
        <dc:Bounds x="500" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_16xfym4_di" bpmnElement="Activity_16xfym4">
        <dc:Bounds x="760" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_19kj0h0_di" bpmnElement="Activity_19kj0h0">
        <dc:Bounds x="760" y="330" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1tin5zd_di" bpmnElement="Activity_1tin5zd">
        <dc:Bounds x="760" y="90" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0pvxvon_di" bpmnElement="Activity_0pvxvon">
        <dc:Bounds x="1040" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_1olbpop_di" bpmnElement="Event_1olbpop">
        <dc:Bounds x="1212" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Gateway_0fft2mz_di" bpmnElement="Gateway_1oq8ofu">
        <dc:Bounds x="655" y="233" width="50" height="50" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Gateway_1htj0r4_di" bpmnElement="Gateway_1yurpx8">
        <dc:Bounds x="935" y="233" width="50" height="50" />
      </bpmndi:BPMNShape>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn2:definitions>
', 0, NULL, N'', 0, NULL, CAST(0x0000AEB600B61484 AS DateTime), CAST(0x0000B1DE009FE489 AS DateTime))
INSERT [dbo].[WfProcess] ([ID], [ProcessGUID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (949, N'6460bdee-7b9d-48b6-abc5-59c39b6148da', N'1', N'ApprovalOrSplitProcess', N'Process_Code_2434', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn2:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:sfat="http://www.slickflow.com/schema/sfat" xmlns:sfgb="http://www.slickflow.com/schema/sfgb" id="bpmn2-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn2:process id="Process_100" sf:guid="6460bdee-7b9d-48b6-abc5-59c39b6148da" sf:code="Process_Code_2434" name="ApprovalOrSplitProcess" isExecutable="false" version="1">
    <bpmn2:startEvent id="StartEvent_1" sf:guid="ae62ea52-c544-4dbb-a034-ae2dbc2b9fcf">
      <bpmn2:outgoing>Flow_00a9vxq</bpmn2:outgoing>
    </bpmn2:startEvent>
    <bpmn2:task id="Activity_1uvql90" sf:guid="ec55e7c5-a916-497e-fefd-e4b47ca4718a" name="submit">
      <bpmn2:incoming>Flow_00a9vxq</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1brb3w4</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_00a9vxq" sf:guid="89e9b5a5-045b-4a72-fa46-dd0a571724fd" sourceRef="StartEvent_1" targetRef="Activity_1uvql90" sf:from="ae62ea52-c544-4dbb-a034-ae2dbc2b9fcf" sf:to="ec55e7c5-a916-497e-fefd-e4b47ca4718a" />
    <bpmn2:sequenceFlow id="Flow_1brb3w4" sf:guid="79c307da-9e55-4e1d-d681-aaf7e65be2ee" sourceRef="Activity_1uvql90" targetRef="Gateway_04eny66" sf:from="ec55e7c5-a916-497e-fefd-e4b47ca4718a" sf:to="349feaf0-05c1-4787-c45e-2fdce2661464" />
    <bpmn2:task id="Activity_04bbyou" sf:guid="5d790853-80bd-444a-e612-d5bed7ab3008" name="have a vacaton">
      <bpmn2:incoming>Flow_0dt8qdk</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1h76oqx</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_0dt8qdk" sf:guid="f82f2828-6a68-4d9f-aed1-ed460ca2dbda" name="agree" sourceRef="Gateway_04eny66" targetRef="Activity_04bbyou" sf:from="349feaf0-05c1-4787-c45e-2fdce2661464" sf:to="5d790853-80bd-444a-e612-d5bed7ab3008">
      <bpmn2:extensionElements>
        <sfgb:groupBehaviours approval="Agreed" />
        <sf:groupBehaviours approval="Agreed" />
      </bpmn2:extensionElements>
    </bpmn2:sequenceFlow>
    <bpmn2:task id="Activity_1p9v78c" sf:guid="38d7dbd8-ca62-4b5b-8bb9-2f91ae9b304a" name="go to work">
      <bpmn2:incoming>Flow_025e520</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1k5uo4x</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_025e520" sf:guid="7442e24d-6f1e-41d2-9c23-ecd5dc6ef66d" name="refuse" sourceRef="Gateway_04eny66" targetRef="Activity_1p9v78c" sf:from="349feaf0-05c1-4787-c45e-2fdce2661464" sf:to="38d7dbd8-ca62-4b5b-8bb9-2f91ae9b304a">
      <bpmn2:extensionElements>
        <sfgb:groupBehaviours approval="Refused" />
        <sf:groupBehaviours approval="Refused" />
      </bpmn2:extensionElements>
    </bpmn2:sequenceFlow>
    <bpmn2:sequenceFlow id="Flow_1h76oqx" sf:guid="9b4cc031-1c61-4938-affa-04c2b3a0117d" sourceRef="Activity_04bbyou" targetRef="Gateway_0n58ftc" sf:from="5d790853-80bd-444a-e612-d5bed7ab3008" sf:to="7a254388-e639-41e4-ea16-ac19c02ff29c" />
    <bpmn2:sequenceFlow id="Flow_1k5uo4x" sf:guid="e86fc3dc-8dd7-4603-93a0-5881041e14e8" sourceRef="Activity_1p9v78c" targetRef="Gateway_0n58ftc" sf:from="38d7dbd8-ca62-4b5b-8bb9-2f91ae9b304a" sf:to="7a254388-e639-41e4-ea16-ac19c02ff29c" />
    <bpmn2:task id="Activity_02n4ivs" sf:guid="d0ce1511-2e7e-42e0-b0b8-57cbf8bb5ca7" name="work off">
      <bpmn2:incoming>Flow_0m3toxk</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0xd1fb6</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_0m3toxk" sf:guid="a511aade-55ee-4d58-a524-6098b2cd1f93" sourceRef="Gateway_0n58ftc" targetRef="Activity_02n4ivs" sf:from="7a254388-e639-41e4-ea16-ac19c02ff29c" sf:to="d0ce1511-2e7e-42e0-b0b8-57cbf8bb5ca7" />
    <bpmn2:endEvent id="Event_0d3xy1a" sf:guid="3d0c0d6d-9268-4efa-b3f7-2aa379a985bd">
      <bpmn2:incoming>Flow_0xd1fb6</bpmn2:incoming>
    </bpmn2:endEvent>
    <bpmn2:sequenceFlow id="Flow_0xd1fb6" sf:guid="8816393f-1a6c-4ea3-9b28-0a132068c442" sourceRef="Activity_02n4ivs" targetRef="Event_0d3xy1a" sf:from="d0ce1511-2e7e-42e0-b0b8-57cbf8bb5ca7" sf:to="3d0c0d6d-9268-4efa-b3f7-2aa379a985bd" />
    <bpmn2:inclusiveGateway id="Gateway_04eny66" sf:guid="349feaf0-05c1-4787-c45e-2fdce2661464">
      <bpmn2:extensionElements>
        <sfat:activityTypeDetail extraSplitType="ApprovalOrSplit" />
        <sf:gatewayDetail extraSplitType="ApprovalOrSplit" />
      </bpmn2:extensionElements>
      <bpmn2:incoming>Flow_1brb3w4</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0dt8qdk</bpmn2:outgoing>
      <bpmn2:outgoing>Flow_025e520</bpmn2:outgoing>
    </bpmn2:inclusiveGateway>
    <bpmn2:inclusiveGateway id="Gateway_0n58ftc" sf:guid="7a254388-e639-41e4-ea16-ac19c02ff29c">
      <bpmn2:incoming>Flow_1h76oqx</bpmn2:incoming>
      <bpmn2:incoming>Flow_1k5uo4x</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0m3toxk</bpmn2:outgoing>
    </bpmn2:inclusiveGateway>
  </bpmn2:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_100">
      <bpmndi:BPMNEdge id="Flow_0xd1fb6_di" bpmnElement="Flow_0xd1fb6">
        <di:waypoint x="1150" y="258" />
        <di:waypoint x="1232" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0m3toxk_di" bpmnElement="Flow_0m3toxk">
        <di:waypoint x="975" y="258" />
        <di:waypoint x="1050" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1k5uo4x_di" bpmnElement="Flow_1k5uo4x">
        <di:waypoint x="860" y="370" />
        <di:waypoint x="950" y="370" />
        <di:waypoint x="950" y="283" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1h76oqx_di" bpmnElement="Flow_1h76oqx">
        <di:waypoint x="850" y="150" />
        <di:waypoint x="950" y="150" />
        <di:waypoint x="950" y="233" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_025e520_di" bpmnElement="Flow_025e520">
        <di:waypoint x="680" y="283" />
        <di:waypoint x="680" y="370" />
        <di:waypoint x="760" y="370" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="679" y="324" width="33" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0dt8qdk_di" bpmnElement="Flow_0dt8qdk">
        <di:waypoint x="680" y="233" />
        <di:waypoint x="680" y="150" />
        <di:waypoint x="750" y="150" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="681" y="189" width="29" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1brb3w4_di" bpmnElement="Flow_1brb3w4">
        <di:waypoint x="600" y="258" />
        <di:waypoint x="655" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_00a9vxq_di" bpmnElement="Flow_00a9vxq">
        <di:waypoint x="448" y="258" />
        <di:waypoint x="500" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="412" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1uvql90_di" bpmnElement="Activity_1uvql90">
        <dc:Bounds x="500" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_04bbyou_di" bpmnElement="Activity_04bbyou">
        <dc:Bounds x="750" y="110" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1p9v78c_di" bpmnElement="Activity_1p9v78c">
        <dc:Bounds x="760" y="330" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_02n4ivs_di" bpmnElement="Activity_02n4ivs">
        <dc:Bounds x="1050" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_0d3xy1a_di" bpmnElement="Event_0d3xy1a">
        <dc:Bounds x="1232" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Gateway_0x7yvja_di" bpmnElement="Gateway_04eny66">
        <dc:Bounds x="655" y="233" width="50" height="50" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Gateway_1xqs9lx_di" bpmnElement="Gateway_0n58ftc">
        <dc:Bounds x="925" y="233" width="50" height="50" />
      </bpmndi:BPMNShape>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn2:definitions>
', 0, NULL, N'', 0, NULL, CAST(0x0000AEB700C85742 AS DateTime), CAST(0x0000B1830141398D AS DateTime))
INSERT [dbo].[WfProcess] ([ID], [ProcessGUID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (950, N'b58ab3a5-ed13-49b6-8fc8-8ec0778d150f', N'1', N'Process_Name_8924', N'Process_Code_8924', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn2:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL" xmlns:sf="http://www.slickflow.com/schema/sf" id="bpmn2-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn2:process id="Process_100" sf:guid="b58ab3a5-ed13-49b6-8fc8-8ec0778d150f" sf:code="Process_Code_8924" name="Process_Name_8924" isExecutable="false" version="1">
    <bpmn2:startEvent id="StartEvent_1" sf:guid="52eb91a0-30c4-4a4b-8c3e-bb3fe66183d5">
      <bpmn2:outgoing>Flow_0rhkw9o</bpmn2:outgoing>
    </bpmn2:startEvent>
    <bpmn2:task id="Activity_0vtfvbb" sf:guid="0b27ff81-409e-4198-8151-3dfe4d74ed62" name="sbumit">
      <bpmn2:incoming>Flow_0rhkw9o</bpmn2:incoming>
      <bpmn2:outgoing>Flow_02m0gtj</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_0rhkw9o" sf:guid="4a2413ee-73c8-43df-cf4b-0a88c7404883" sourceRef="StartEvent_1" targetRef="Activity_0vtfvbb" sf:from="52eb91a0-30c4-4a4b-8c3e-bb3fe66183d5" sf:to="0b27ff81-409e-4198-8151-3dfe4d74ed62" />
    <bpmn2:sequenceFlow id="Flow_02m0gtj" sf:guid="6868dd46-2716-4e98-90d5-f826f3b04974" sourceRef="Activity_0vtfvbb" targetRef="Gateway_1c8qn8y" sf:from="0b27ff81-409e-4198-8151-3dfe4d74ed62" sf:to="8765c0be-ad47-4942-f28c-4a6052aebeea" />
    <bpmn2:task id="Activity_0jg4ydn" sf:guid="8ee9d662-ec26-433d-8a24-62ded13fcb13" name="dept destribute">
      <bpmn2:incoming>Flow_0px4xn5</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0abh1ex</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_0px4xn5" sf:guid="5fd2d016-30d3-4d44-828e-1ed202797055" sourceRef="Gateway_1c8qn8y" targetRef="Activity_0jg4ydn" sf:from="8765c0be-ad47-4942-f28c-4a6052aebeea" sf:to="8ee9d662-ec26-433d-8a24-62ded13fcb13" />
    <bpmn2:parallelGateway id="Gateway_1c8qn8y" sf:guid="8765c0be-ad47-4942-f28c-4a6052aebeea">
      <bpmn2:incoming>Flow_02m0gtj</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0px4xn5</bpmn2:outgoing>
    </bpmn2:parallelGateway>
    <bpmn2:sequenceFlow id="Flow_0abh1ex" sf:guid="d88fac0f-45ce-425c-e136-fe7fadeeee22" sourceRef="Activity_0jg4ydn" targetRef="Activity_1b1pbcm" sf:from="8ee9d662-ec26-433d-8a24-62ded13fcb13" sf:to="4d705363-c2ed-432b-cd06-07144e39a018" />
    <bpmn2:parallelGateway id="Gateway_0k1i3qp" sf:guid="4d705363-c2ed-432b-cd06-07144e39a018">
      <bpmn2:incoming>Flow_0j4mzft</bpmn2:incoming>
      <bpmn2:outgoing>Flow_13dizyz</bpmn2:outgoing>
    </bpmn2:parallelGateway>
    <bpmn2:task id="Activity_045902a" sf:guid="e68e9367-0071-457e-da4f-4abfea6db5cc" name="finale approval">
      <bpmn2:incoming>Flow_13dizyz</bpmn2:incoming>
      <bpmn2:outgoing>Flow_13dltga</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_13dizyz" sf:guid="f866585b-fb6b-42c1-a17f-b315cee77d4e" sourceRef="Gateway_0k1i3qp" targetRef="Activity_045902a" sf:from="4d705363-c2ed-432b-cd06-07144e39a018" sf:to="e68e9367-0071-457e-da4f-4abfea6db5cc" />
    <bpmn2:endEvent id="Event_0jdmmz3" sf:guid="4362b8c0-306f-47c9-bb67-69d4da493b09">
      <bpmn2:incoming>Flow_13dltga</bpmn2:incoming>
    </bpmn2:endEvent>
    <bpmn2:sequenceFlow id="Flow_13dltga" sf:guid="8950aae0-19f5-4a14-bbec-a499e19c55c6" sourceRef="Activity_045902a" targetRef="Event_0jdmmz3" sf:from="e68e9367-0071-457e-da4f-4abfea6db5cc" sf:to="4362b8c0-306f-47c9-bb67-69d4da493b09" />
    <bpmn2:task id="Activity_1b1pbcm" sf:guid="9571cc0d-2555-4a59-a632-869f3dd53646" name="dept manager approval">
      <bpmn2:incoming>Flow_0abh1ex</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0j4mzft</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_0j4mzft" sf:guid="f9e93896-a90e-433d-ee50-f4e49fc94567" sourceRef="Activity_1b1pbcm" targetRef="Gateway_0k1i3qp" sf:from="9571cc0d-2555-4a59-a632-869f3dd53646" sf:to="4d705363-c2ed-432b-cd06-07144e39a018" />
  </bpmn2:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_100">
      <bpmndi:BPMNEdge id="Flow_0rhkw9o_di" bpmnElement="Flow_0rhkw9o">
        <di:waypoint x="448" y="258" />
        <di:waypoint x="500" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_02m0gtj_di" bpmnElement="Flow_02m0gtj">
        <di:waypoint x="600" y="258" />
        <di:waypoint x="655" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0px4xn5_di" bpmnElement="Flow_0px4xn5">
        <di:waypoint x="705" y="258" />
        <di:waypoint x="760" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0abh1ex_di" bpmnElement="Flow_0abh1ex">
        <di:waypoint x="860" y="258" />
        <di:waypoint x="920" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_13dizyz_di" bpmnElement="Flow_13dizyz">
        <di:waypoint x="1125" y="250" />
        <di:waypoint x="1170" y="250" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_13dltga_di" bpmnElement="Flow_13dltga">
        <di:waypoint x="1270" y="250" />
        <di:waypoint x="1352" y="250" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0j4mzft_di" bpmnElement="Flow_0j4mzft">
        <di:waypoint x="1020" y="250" />
        <di:waypoint x="1075" y="250" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="412" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0vtfvbb_di" bpmnElement="Activity_0vtfvbb">
        <dc:Bounds x="500" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Gateway_1rr8ngv_di" bpmnElement="Gateway_1c8qn8y">
        <dc:Bounds x="655" y="233" width="50" height="50" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0jg4ydn_di" bpmnElement="Activity_0jg4ydn">
        <dc:Bounds x="760" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_045902a_di" bpmnElement="Activity_045902a">
        <dc:Bounds x="1170" y="210" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Gateway_1iu40lv_di" bpmnElement="Gateway_0k1i3qp">
        <dc:Bounds x="1075" y="225" width="50" height="50" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_0jdmmz3_di" bpmnElement="Event_0jdmmz3">
        <dc:Bounds x="1352" y="232" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1b1pbcm_di" bpmnElement="Activity_1b1pbcm">
        <dc:Bounds x="920" y="210" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn2:definitions>
', 0, NULL, NULL, 0, NULL, CAST(0x0000AEB800726F84 AS DateTime), NULL)
INSERT [dbo].[WfProcess] ([ID], [ProcessGUID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (951, N'2ab4c826-89ed-47a3-aeee-7a800ac20161', N'1', N'Process_Name_1517', N'Process_Code_1517', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn2:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL" xmlns:sf="http://www.slickflow.com/schema/sf" id="bpmn2-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn2:process id="Process_100" sf:guid="2ab4c826-89ed-47a3-aeee-7a800ac20161" sf:code="Process_Code_1517" name="Process_Name_1517" isExecutable="false" version="1">
    <bpmn2:startEvent id="StartEvent_1" sf:guid="e5524817-e066-4c9f-ad1a-c2296c4320c6">
      <bpmn2:outgoing>Flow_1kq4nc9</bpmn2:outgoing>
    </bpmn2:startEvent>
    <bpmn2:sequenceFlow id="Flow_1kq4nc9" sf:guid="203ab340-d49d-4a81-e697-35244bf934b4" sourceRef="StartEvent_1" targetRef="Activity_1rkbxqu" sf:from="e5524817-e066-4c9f-ad1a-c2296c4320c6" sf:to="7517a8ed-aa51-4cd7-953a-c107e8ecdfdd" />
    <bpmn2:task id="Activity_0ancku1" sf:guid="d496d664-23f7-4b03-86db-2701b4cdbbeb">
      <bpmn2:incoming>Flow_0ic5p5p</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0g8akt1</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_0ic5p5p" sf:guid="e69916eb-547e-4999-f469-d8934a7869bf" sourceRef="Gateway_13rkzri" targetRef="Activity_0ancku1" sf:from="7517a8ed-aa51-4cd7-953a-c107e8ecdfdd" sf:to="d496d664-23f7-4b03-86db-2701b4cdbbeb">
      <bpmn2:extensionElements>
        <sf:groupBehaviours approval="Refused" />
      </bpmn2:extensionElements>
    </bpmn2:sequenceFlow>
    <bpmn2:parallelGateway id="Gateway_13rkzri" sf:guid="7517a8ed-aa51-4cd7-953a-c107e8ecdfdd">
      <bpmn2:extensionElements>
        <sf:activityTypeDetail extraSplitType="AndSplitMI" />
      </bpmn2:extensionElements>
      <bpmn2:incoming>Flow_0y9foss</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0ic5p5p</bpmn2:outgoing>
    </bpmn2:parallelGateway>
    <bpmn2:task id="Activity_1rkbxqu" sf:guid="4e73a807-56b5-44ec-adad-d02659aea440">
      <bpmn2:incoming>Flow_1kq4nc9</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0y9foss</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_0y9foss" sf:guid="817694f7-69c8-4163-e2f7-3a2bb92fd313" sourceRef="Activity_1rkbxqu" targetRef="Gateway_13rkzri" sf:from="4e73a807-56b5-44ec-adad-d02659aea440" sf:to="7517a8ed-aa51-4cd7-953a-c107e8ecdfdd" />
    <bpmn2:task id="Activity_0vd7vus" sf:guid="9c7d139f-d2c0-43ca-961f-263a2d4c56c3">
      <bpmn2:incoming>Flow_0g8akt1</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1572emp</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_0g8akt1" sf:guid="fb4be272-0fff-4b12-e765-db577cd73d33" sourceRef="Activity_0ancku1" targetRef="Activity_0vd7vus" sf:from="d496d664-23f7-4b03-86db-2701b4cdbbeb" sf:to="9c7d139f-d2c0-43ca-961f-263a2d4c56c3" />
    <bpmn2:exclusiveGateway id="Gateway_0qgnetj" sf:guid="15da3008-cd3b-47a9-ab54-3e479b0eb0ba">
      <bpmn2:incoming>Flow_1572emp</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0ju2t2u</bpmn2:outgoing>
    </bpmn2:exclusiveGateway>
    <bpmn2:sequenceFlow id="Flow_1572emp" sf:guid="aacd2d43-f165-40b5-88ae-7a2cbd832b16" sourceRef="Activity_0vd7vus" targetRef="Gateway_0qgnetj" sf:from="9c7d139f-d2c0-43ca-961f-263a2d4c56c3" sf:to="15da3008-cd3b-47a9-ab54-3e479b0eb0ba" />
    <bpmn2:task id="Activity_0mm5wch" sf:guid="326c5302-ae2f-4118-b5e0-9e8235a619d4">
      <bpmn2:incoming>Flow_0ju2t2u</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1hhr1bp</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_0ju2t2u" sf:guid="a5206623-16eb-4cd0-8f7d-c39e83bcc6e3" sourceRef="Gateway_0qgnetj" targetRef="Activity_0mm5wch" sf:from="15da3008-cd3b-47a9-ab54-3e479b0eb0ba" sf:to="326c5302-ae2f-4118-b5e0-9e8235a619d4" />
    <bpmn2:endEvent id="Event_0ggoowa" sf:guid="6942367a-597e-424a-8fdb-d0462cf484e8">
      <bpmn2:incoming>Flow_1hhr1bp</bpmn2:incoming>
    </bpmn2:endEvent>
    <bpmn2:sequenceFlow id="Flow_1hhr1bp" sf:guid="2db0f232-f5a1-4ffc-8be3-048a0c44d811" sourceRef="Activity_0mm5wch" targetRef="Event_0ggoowa" sf:from="326c5302-ae2f-4118-b5e0-9e8235a619d4" sf:to="6942367a-597e-424a-8fdb-d0462cf484e8" />
  </bpmn2:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_100">
      <bpmndi:BPMNEdge id="Flow_1kq4nc9_di" bpmnElement="Flow_1kq4nc9">
        <di:waypoint x="508" y="250" />
        <di:waypoint x="590" y="250" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0ic5p5p_di" bpmnElement="Flow_0ic5p5p">
        <di:waypoint x="825" y="250" />
        <di:waypoint x="920" y="250" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0y9foss_di" bpmnElement="Flow_0y9foss">
        <di:waypoint x="690" y="250" />
        <di:waypoint x="775" y="250" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0g8akt1_di" bpmnElement="Flow_0g8akt1">
        <di:waypoint x="1020" y="250" />
        <di:waypoint x="1120" y="250" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1572emp_di" bpmnElement="Flow_1572emp">
        <di:waypoint x="1220" y="250" />
        <di:waypoint x="1295" y="250" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0ju2t2u_di" bpmnElement="Flow_0ju2t2u">
        <di:waypoint x="1345" y="250" />
        <di:waypoint x="1410" y="250" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1hhr1bp_di" bpmnElement="Flow_1hhr1bp">
        <di:waypoint x="1510" y="250" />
        <di:waypoint x="1592" y="250" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNShape id="Gateway_1s7o6rm_di" bpmnElement="Gateway_13rkzri">
        <dc:Bounds x="775" y="225" width="50" height="50" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0ancku1_di" bpmnElement="Activity_0ancku1">
        <dc:Bounds x="920" y="210" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1rkbxqu_di" bpmnElement="Activity_1rkbxqu">
        <dc:Bounds x="590" y="210" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0vd7vus_di" bpmnElement="Activity_0vd7vus">
        <dc:Bounds x="1120" y="210" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="472" y="232" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Gateway_0qgnetj_di" bpmnElement="Gateway_0qgnetj" isMarkerVisible="true">
        <dc:Bounds x="1295" y="225" width="50" height="50" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0mm5wch_di" bpmnElement="Activity_0mm5wch">
        <dc:Bounds x="1410" y="210" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_0ggoowa_di" bpmnElement="Event_0ggoowa">
        <dc:Bounds x="1592" y="232" width="36" height="36" />
      </bpmndi:BPMNShape>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn2:definitions>
', 0, NULL, NULL, 0, NULL, CAST(0x0000AEB800E44ECC AS DateTime), NULL)
INSERT [dbo].[WfProcess] ([ID], [ProcessGUID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (952, N'ea34fa73-8183-42ae-a951-c58a765b168c', N'1', N'Process_Name_3720', N'Process_Code_3720', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn2:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL" xmlns:sf="http://www.slickflow.com/schema/sf" id="bpmn2-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn2:process id="Process_100" sf:guid="ea34fa73-8183-42ae-a951-c58a765b168c" sf:code="Process_Code_3720" name="Process_Name_3720" isExecutable="false" version="1">
    <bpmn2:startEvent id="StartEvent_1" sf:guid="15299555-926e-4706-ae3c-350a0d94ec6b">
      <bpmn2:outgoing>Flow_1frqxta</bpmn2:outgoing>
    </bpmn2:startEvent>
    <bpmn2:task id="Activity_1uud5kw" sf:guid="a3eb36cd-638d-440f-ca57-9bcf24e6ea8e">
      <bpmn2:incoming>Flow_1frqxta</bpmn2:incoming>
      <bpmn2:outgoing>Flow_19gbif3</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_1frqxta" sf:guid="8e48fbb1-707c-47b5-9644-3ceac516c922" sourceRef="StartEvent_1" targetRef="Activity_1uud5kw" sf:from="15299555-926e-4706-ae3c-350a0d94ec6b" sf:to="a3eb36cd-638d-440f-ca57-9bcf24e6ea8e" />
    <bpmn2:exclusiveGateway id="Gateway_02fkexx" sf:guid="2a7900bb-45da-4e2b-d737-689c6f9f7104">
      <bpmn2:incoming>Flow_19gbif3</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0u8o17i</bpmn2:outgoing>
    </bpmn2:exclusiveGateway>
    <bpmn2:sequenceFlow id="Flow_19gbif3" sf:guid="1fdb9cae-7fd8-4281-beb3-1755061a3cb6" sourceRef="Activity_1uud5kw" targetRef="Gateway_02fkexx" sf:from="a3eb36cd-638d-440f-ca57-9bcf24e6ea8e" sf:to="2a7900bb-45da-4e2b-d737-689c6f9f7104" />
    <bpmn2:task id="Activity_085fbyt" sf:guid="db704bf5-6729-46f3-b85b-821d2f0664db">
      <bpmn2:incoming>Flow_0u8o17i</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1ndzzix</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_0u8o17i" sf:guid="02759d62-3c44-4395-ec20-864dca89c25e" sourceRef="Gateway_02fkexx" targetRef="Activity_085fbyt" sf:from="2a7900bb-45da-4e2b-d737-689c6f9f7104" sf:to="db704bf5-6729-46f3-b85b-821d2f0664db" />
    <bpmn2:exclusiveGateway id="Gateway_0geu58z" sf:guid="4a4e28b9-f4ec-4832-c360-b3d8c6899fed">
      <bpmn2:incoming>Flow_1ndzzix</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1c7x7kk</bpmn2:outgoing>
    </bpmn2:exclusiveGateway>
    <bpmn2:sequenceFlow id="Flow_1ndzzix" sf:guid="d7c3dcc2-6f4e-4476-b487-e66a23c6fedb" sourceRef="Activity_085fbyt" targetRef="Gateway_0geu58z" sf:from="db704bf5-6729-46f3-b85b-821d2f0664db" sf:to="4a4e28b9-f4ec-4832-c360-b3d8c6899fed" />
    <bpmn2:task id="Activity_0y45uae" sf:guid="80215ed8-13af-413e-f44b-fa8b74755c50">
      <bpmn2:incoming>Flow_1c7x7kk</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1302ha7</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_1c7x7kk" sf:guid="066543ff-f6db-4a79-8f82-d0f503008d29" sourceRef="Gateway_0geu58z" targetRef="Activity_0y45uae" sf:from="4a4e28b9-f4ec-4832-c360-b3d8c6899fed" sf:to="80215ed8-13af-413e-f44b-fa8b74755c50" />
    <bpmn2:endEvent id="Event_0mo18bv" sf:guid="698a45d1-3284-4c58-9032-fad4f36096a7">
      <bpmn2:incoming>Flow_1302ha7</bpmn2:incoming>
    </bpmn2:endEvent>
    <bpmn2:sequenceFlow id="Flow_1302ha7" sf:guid="c9f976f1-f34f-4ad0-e71f-53421199f6ec" sourceRef="Activity_0y45uae" targetRef="Event_0mo18bv" sf:from="80215ed8-13af-413e-f44b-fa8b74755c50" sf:to="698a45d1-3284-4c58-9032-fad4f36096a7" />
  </bpmn2:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_100">
      <bpmndi:BPMNEdge id="Flow_1frqxta_di" bpmnElement="Flow_1frqxta">
        <di:waypoint x="448" y="258" />
        <di:waypoint x="500" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_19gbif3_di" bpmnElement="Flow_19gbif3">
        <di:waypoint x="600" y="258" />
        <di:waypoint x="655" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0u8o17i_di" bpmnElement="Flow_0u8o17i">
        <di:waypoint x="705" y="258" />
        <di:waypoint x="760" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1ndzzix_di" bpmnElement="Flow_1ndzzix">
        <di:waypoint x="860" y="258" />
        <di:waypoint x="915" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1c7x7kk_di" bpmnElement="Flow_1c7x7kk">
        <di:waypoint x="965" y="258" />
        <di:waypoint x="1020" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1302ha7_di" bpmnElement="Flow_1302ha7">
        <di:waypoint x="1120" y="258" />
        <di:waypoint x="1182" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="412" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1uud5kw_di" bpmnElement="Activity_1uud5kw">
        <dc:Bounds x="500" y="218" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Gateway_02fkexx_di" bpmnElement="Gateway_02fkexx" isMarkerVisible="true">
        <dc:Bounds x="655" y="233" width="50" height="50" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_085fbyt_di" bpmnElement="Activity_085fbyt">
        <dc:Bounds x="760" y="218" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Gateway_0geu58z_di" bpmnElement="Gateway_0geu58z" isMarkerVisible="true">
        <dc:Bounds x="915" y="233" width="50" height="50" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0y45uae_di" bpmnElement="Activity_0y45uae">
        <dc:Bounds x="1020" y="218" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_0mo18bv_di" bpmnElement="Event_0mo18bv">
        <dc:Bounds x="1182" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn2:definitions>
', 0, NULL, NULL, 0, NULL, CAST(0x0000AEB800E47C84 AS DateTime), NULL)
INSERT [dbo].[WfProcess] ([ID], [ProcessGUID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (953, N'82dd8fd0-dc61-49a5-8db5-9431a8b7da3d', N'1', N'AndSplitMI-Demo', N'Process_Code_6113', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn2:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL" xmlns:sf="http://www.slickflow.com/schema/sf" id="bpmn2-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn2:process id="Process_100" sf:guid="82dd8fd0-dc61-49a5-8db5-9431a8b7da3d" sf:code="Process_Code_6113" name="AndSplitMI-Demo" isExecutable="false" version="1">
    <bpmn2:startEvent id="StartEvent_1" sf:guid="4cf0a1bb-3128-43f6-a44a-3bf32cdee96c">
      <bpmn2:outgoing>Flow_0di99uc</bpmn2:outgoing>
    </bpmn2:startEvent>
    <bpmn2:task id="Activity_1w6o2a6" sf:guid="2e94d266-73ca-4650-deda-bf56889cda88" name="submit">
      <bpmn2:incoming>Flow_0di99uc</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0v6sapo</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_0di99uc" sf:guid="e28ed8ce-8236-4309-94ca-4d5c95baf102" sourceRef="StartEvent_1" targetRef="Activity_1w6o2a6" sf:from="4cf0a1bb-3128-43f6-a44a-3bf32cdee96c" sf:to="2e94d266-73ca-4650-deda-bf56889cda88" />
    <bpmn2:sequenceFlow id="Flow_0v6sapo" sf:guid="f2818832-e784-4c90-e5a6-e7465cc8754a" sourceRef="Activity_1w6o2a6" targetRef="Gateway_1ndyfad" sf:from="2e94d266-73ca-4650-deda-bf56889cda88" sf:to="b3d0a7fe-6cb0-4f94-e11c-3c116fb94c76" />
    <bpmn2:task id="Activity_0iclvnb" sf:guid="040d0a5f-9f3b-463d-a883-1a977b7ebe0f" name="dept distribute">
      <bpmn2:incoming>Flow_126ixp0</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0m958a9</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_126ixp0" sf:guid="5234c58d-2aab-4120-9047-37838e610a08" sourceRef="Gateway_1ndyfad" targetRef="Activity_0iclvnb" sf:from="b3d0a7fe-6cb0-4f94-e11c-3c116fb94c76" sf:to="040d0a5f-9f3b-463d-a883-1a977b7ebe0f" />
    <bpmn2:sequenceFlow id="Flow_0m958a9" sf:guid="159b867d-e6de-4a1f-c349-7f388011033a" sourceRef="Activity_0iclvnb" targetRef="Gateway_1jsms4p" sf:from="040d0a5f-9f3b-463d-a883-1a977b7ebe0f" sf:to="c21bb5b3-cd0a-41cc-c105-2db0269be984" />
    <bpmn2:task id="Activity_1jn5ktv" sf:guid="f751f2a7-c2e0-4dbf-95be-e558b5f8a002" name="final approval">
      <bpmn2:incoming>Flow_06a1c6p</bpmn2:incoming>
      <bpmn2:outgoing>Flow_07rpzgi</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_06a1c6p" sf:guid="ae612280-dfa2-462a-9642-2e9b89076903" sourceRef="Gateway_1jsms4p" targetRef="Activity_1jn5ktv" sf:from="c21bb5b3-cd0a-41cc-c105-2db0269be984" sf:to="f751f2a7-c2e0-4dbf-95be-e558b5f8a002" />
    <bpmn2:endEvent id="Event_1n63kut" sf:guid="a36dfcbc-aadb-4660-9efb-14977e2352e7">
      <bpmn2:incoming>Flow_07rpzgi</bpmn2:incoming>
    </bpmn2:endEvent>
    <bpmn2:sequenceFlow id="Flow_07rpzgi" sf:guid="54672e28-fda7-47ea-96d7-be675c2fa912" sourceRef="Activity_1jn5ktv" targetRef="Event_1n63kut" sf:from="f751f2a7-c2e0-4dbf-95be-e558b5f8a002" sf:to="a36dfcbc-aadb-4660-9efb-14977e2352e7" />
    <bpmn2:parallelGateway id="Gateway_1ndyfad" sf:guid="b3d0a7fe-6cb0-4f94-e11c-3c116fb94c76">
      <bpmn2:extensionElements>
        <sf:gatewayDetail extraSplitType="AndSplitMI" />
      </bpmn2:extensionElements>
      <bpmn2:incoming>Flow_0v6sapo</bpmn2:incoming>
      <bpmn2:outgoing>Flow_126ixp0</bpmn2:outgoing>
    </bpmn2:parallelGateway>
    <bpmn2:parallelGateway id="Gateway_1jsms4p" sf:guid="c21bb5b3-cd0a-41cc-c105-2db0269be984">
      <bpmn2:extensionElements>
        <sf:gatewayDetail extraJoinType="AndJoinMI" />
      </bpmn2:extensionElements>
      <bpmn2:incoming>Flow_0m958a9</bpmn2:incoming>
      <bpmn2:outgoing>Flow_06a1c6p</bpmn2:outgoing>
    </bpmn2:parallelGateway>
  </bpmn2:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_100">
      <bpmndi:BPMNEdge id="Flow_07rpzgi_di" bpmnElement="Flow_07rpzgi">
        <di:waypoint x="1120" y="258" />
        <di:waypoint x="1182" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_06a1c6p_di" bpmnElement="Flow_06a1c6p">
        <di:waypoint x="965" y="258" />
        <di:waypoint x="1020" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0m958a9_di" bpmnElement="Flow_0m958a9">
        <di:waypoint x="860" y="258" />
        <di:waypoint x="915" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_126ixp0_di" bpmnElement="Flow_126ixp0">
        <di:waypoint x="705" y="258" />
        <di:waypoint x="760" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0v6sapo_di" bpmnElement="Flow_0v6sapo">
        <di:waypoint x="600" y="258" />
        <di:waypoint x="655" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0di99uc_di" bpmnElement="Flow_0di99uc">
        <di:waypoint x="448" y="258" />
        <di:waypoint x="500" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="412" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1w6o2a6_di" bpmnElement="Activity_1w6o2a6">
        <dc:Bounds x="500" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0iclvnb_di" bpmnElement="Activity_0iclvnb">
        <dc:Bounds x="760" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1jn5ktv_di" bpmnElement="Activity_1jn5ktv">
        <dc:Bounds x="1020" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_1n63kut_di" bpmnElement="Event_1n63kut">
        <dc:Bounds x="1182" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Gateway_1bj410k_di" bpmnElement="Gateway_1ndyfad">
        <dc:Bounds x="655" y="233" width="50" height="50" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Gateway_09jomcv_di" bpmnElement="Gateway_1jsms4p">
        <dc:Bounds x="915" y="233" width="50" height="50" />
      </bpmndi:BPMNShape>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn2:definitions>
', 0, NULL, N'', 0, NULL, CAST(0x0000AEB800E54D55 AS DateTime), CAST(0x0000AEFB014A4D94 AS DateTime))
INSERT [dbo].[WfProcess] ([ID], [ProcessGUID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (954, N'54713763-c37c-41a0-8c35-87993c534125', N'1', N'ServiceTask_Process_Name_8849', N'ServiceTask_Process_Code_8849', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn2:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL" xmlns:sf="http://www.slickflow.com/schema/sf" id="bpmn2-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn2:process id="Process_100" sf:guid="54713763-c37c-41a0-8c35-87993c534125" sf:code="Process_Code_8849" name="Process_Name_8849" isExecutable="false" version="1">
    <bpmn2:startEvent id="StartEvent_1" sf:guid="e5c1c478-6208-40b5-a270-29a92049386c">
      <bpmn2:outgoing>Flow_07kcr5h</bpmn2:outgoing>
    </bpmn2:startEvent>
    <bpmn2:task id="Activity_1etjxtv" sf:guid="22af27ee-31df-483d-c742-3524700ec8f9" name="submit">
      <bpmn2:incoming>Flow_07kcr5h</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1m520tv</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_07kcr5h" sf:guid="39dab616-b364-417f-f53d-a56a09918be0" sourceRef="StartEvent_1" targetRef="Activity_1etjxtv" sf:from="e5c1c478-6208-40b5-a270-29a92049386c" sf:to="22af27ee-31df-483d-c742-3524700ec8f9" />
    <bpmn2:sequenceFlow id="Flow_1m520tv" sf:guid="7903cc44-858b-40b7-b689-04324e88d856" sourceRef="Activity_1etjxtv" targetRef="Activity_1f2v6e6" sf:from="22af27ee-31df-483d-c742-3524700ec8f9" sf:to="11827231-932c-4745-fd48-b8177095c733" />
    <bpmn2:endEvent id="Event_1tjghcp" sf:guid="c6ef15aa-7f7f-4424-c3c1-4757f8b21968">
      <bpmn2:incoming>Flow_0d8co6h</bpmn2:incoming>
    </bpmn2:endEvent>
    <bpmn2:sequenceFlow id="Flow_0d8co6h" sf:guid="2fa2d59a-0abc-477c-c602-bcc68f32b551" sourceRef="Activity_1f2v6e6" targetRef="Event_1tjghcp" sf:from="11827231-932c-4745-fd48-b8177095c733" sf:to="c6ef15aa-7f7f-4424-c3c1-4757f8b21968" />
    <bpmn2:serviceTask id="Activity_1f2v6e6" sf:guid="11827231-932c-4745-fd48-b8177095c733" name="service">
      <bpmn2:extensionElements>
        <sf:services>
          <sf:service methodType="WebApi" subMethodType="HttpGet" expression="http://localhost/sfapi2/api/wfbasic/hello" />
        </sf:services>
      </bpmn2:extensionElements>
      <bpmn2:incoming>Flow_1m520tv</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0d8co6h</bpmn2:outgoing>
    </bpmn2:serviceTask>
  </bpmn2:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_100">
      <bpmndi:BPMNEdge id="Flow_0d8co6h_di" bpmnElement="Flow_0d8co6h">
        <di:waypoint x="760" y="258" />
        <di:waypoint x="822" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1m520tv_di" bpmnElement="Flow_1m520tv">
        <di:waypoint x="600" y="258" />
        <di:waypoint x="660" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_07kcr5h_di" bpmnElement="Flow_07kcr5h">
        <di:waypoint x="448" y="258" />
        <di:waypoint x="500" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="412" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1etjxtv_di" bpmnElement="Activity_1etjxtv">
        <dc:Bounds x="500" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_1tjghcp_di" bpmnElement="Event_1tjghcp">
        <dc:Bounds x="822" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0orf2s0_di" bpmnElement="Activity_1f2v6e6">
        <dc:Bounds x="660" y="218" width="100" height="80" />
      </bpmndi:BPMNShape>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn2:definitions>
                                                                                                                                                                                                                                                                  ', 0, NULL, N'', 0, NULL, CAST(0x0000AEB9009E6F05 AS DateTime), CAST(0x0000AEB9015A2D07 AS DateTime))
INSERT [dbo].[WfProcess] ([ID], [ProcessGUID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (955, N'f28ed6f9-7a60-4822-aece-e843fef4dede', N'1', N'ScriptTask_Process_Name_9317', N'ScriptTask_Process_Code_9317', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn2:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL" xmlns:sf="http://www.slickflow.com/schema/sf" id="bpmn2-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn2:process id="Process_100" sf:guid="f28ed6f9-7a60-4822-aece-e843fef4dede" sf:code="Process_Code_9317" name="Process_Name_9317" isExecutable="false" version="1">
    <bpmn2:startEvent id="StartEvent_1" sf:guid="3dfe73a7-44a0-48ba-96ce-00d298b3873e">
      <bpmn2:outgoing>Flow_0fk95yp</bpmn2:outgoing>
    </bpmn2:startEvent>
    <bpmn2:task id="Activity_14mtosc" sf:guid="f22ee381-1869-4905-e6a3-984d32ab2b4a" name="submit">
      <bpmn2:incoming>Flow_0fk95yp</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1ttonil</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_0fk95yp" sf:guid="2a7a6a59-7f7e-4e6b-ea5b-6f6c3d123b6c" sourceRef="StartEvent_1" targetRef="Activity_14mtosc" sf:from="3dfe73a7-44a0-48ba-96ce-00d298b3873e" sf:to="f22ee381-1869-4905-e6a3-984d32ab2b4a" />
    <bpmn2:endEvent id="Event_194geh7" sf:guid="8c5fe1df-fd00-4b99-e4a2-bde38bddb14f">
      <bpmn2:incoming>Flow_0dx4le1</bpmn2:incoming>
    </bpmn2:endEvent>
    <bpmn2:sequenceFlow id="Flow_1ttonil" sf:guid="c74d6be9-b30a-4c2b-dd5c-b2f657d82f5a" sourceRef="Activity_14mtosc" targetRef="Activity_0zlr0as" sf:from="f22ee381-1869-4905-e6a3-984d32ab2b4a" sf:to="fd99a7a9-2f8c-4b5b-8373-42ee42cd2902" />
    <bpmn2:scriptTask id="Activity_0zlr0as" sf:guid="fd99a7a9-2f8c-4b5b-8373-42ee42cd2902" name="script">
      <bpmn2:extensionElements>
        <sf:scripts>
          <sf:script scriptType="SQL">select * from wflog;</sf:script>
        </sf:scripts>
      </bpmn2:extensionElements>
      <bpmn2:incoming>Flow_1ttonil</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0dx4le1</bpmn2:outgoing>
    </bpmn2:scriptTask>
    <bpmn2:sequenceFlow id="Flow_0dx4le1" sf:guid="65923c88-e9de-44c2-bd49-6a64e5259b27" sourceRef="Activity_0zlr0as" targetRef="Event_194geh7" sf:from="fd99a7a9-2f8c-4b5b-8373-42ee42cd2902" sf:to="8c5fe1df-fd00-4b99-e4a2-bde38bddb14f" />
  </bpmn2:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_100">
      <bpmndi:BPMNEdge id="Flow_0dx4le1_di" bpmnElement="Flow_0dx4le1">
        <di:waypoint x="810" y="258" />
        <di:waypoint x="912" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1ttonil_di" bpmnElement="Flow_1ttonil">
        <di:waypoint x="630" y="258" />
        <di:waypoint x="710" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0fk95yp_di" bpmnElement="Flow_0fk95yp">
        <di:waypoint x="448" y="258" />
        <di:waypoint x="530" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="412" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_14mtosc_di" bpmnElement="Activity_14mtosc">
        <dc:Bounds x="530" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_194geh7_di" bpmnElement="Event_194geh7">
        <dc:Bounds x="912" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0i6xj8u_di" bpmnElement="Activity_0zlr0as">
        <dc:Bounds x="710" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn2:definitions>
                                                                                                                                                                                                                                                                                               ', 0, NULL, N'', 0, NULL, CAST(0x0000AEB901666015 AS DateTime), CAST(0x0000AEBA00759BB4 AS DateTime))
INSERT [dbo].[WfProcess] ([ID], [ProcessGUID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (956, N'c0874da2-7f6f-4159-98be-e51cd8091993', N'1', N'Sequence_Process_Name_9450', N'Sequence_Process_Code_9450', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn2:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL" xmlns:sf="http://www.slickflow.com/schema/sf" id="bpmn2-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn2:process id="Process_100" sf:guid="c0874da2-7f6f-4159-98be-e51cd8091993" sf:code="Sequence_Process_Code_9450" name="Sequence_Process_Name_9450" isExecutable="false" version="1">
    <bpmn2:startEvent id="StartEvent_1" sf:guid="a92268dc-b7e7-43ab-a85e-a31772b79628">
      <bpmn2:outgoing>Flow_1k2kjdm</bpmn2:outgoing>
    </bpmn2:startEvent>
    <bpmn2:task id="Activity_0vwbrxl" sf:guid="e455e638-5e17-4623-e608-6404db7e80b8" name="submit">
      <bpmn2:incoming>Flow_1k2kjdm</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1y9y72b</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_1k2kjdm" sf:guid="bd9885dc-ca0c-47e6-a121-b34741ea0841" sourceRef="StartEvent_1" targetRef="Activity_0vwbrxl" sf:from="a92268dc-b7e7-43ab-a85e-a31772b79628" sf:to="e455e638-5e17-4623-e608-6404db7e80b8" />
    <bpmn2:task id="Activity_1gc2sq3" sf:guid="7acaacc8-8689-4330-9f99-5f9bcdd7d729" name="dept approval">
      <bpmn2:incoming>Flow_1y9y72b</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1oesje1</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_1y9y72b" sf:guid="e7902c43-a9ef-4ac8-a84c-14bdf8e437ff" sourceRef="Activity_0vwbrxl" targetRef="Activity_1gc2sq3" sf:from="e455e638-5e17-4623-e608-6404db7e80b8" sf:to="7acaacc8-8689-4330-9f99-5f9bcdd7d729" />
    <bpmn2:task id="Activity_17734ew" sf:guid="84b44602-c11e-4aa0-86e4-e33ae0153189" name="hr approval">
      <bpmn2:incoming>Flow_1oesje1</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1jr0j1q</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_1oesje1" sf:guid="436381a3-a9c2-4281-b896-afe9375aa7c8" sourceRef="Activity_1gc2sq3" targetRef="Activity_17734ew" sf:from="7acaacc8-8689-4330-9f99-5f9bcdd7d729" sf:to="84b44602-c11e-4aa0-86e4-e33ae0153189" />
    <bpmn2:endEvent id="Event_07sz6r5" sf:guid="dc14808f-af5b-457e-ca6a-3f55d2d5b990">
      <bpmn2:incoming>Flow_1jr0j1q</bpmn2:incoming>
    </bpmn2:endEvent>
    <bpmn2:sequenceFlow id="Flow_1jr0j1q" sf:guid="60061e2c-aeb6-4f96-f8dd-c2260e011c1e" sourceRef="Activity_17734ew" targetRef="Event_07sz6r5" sf:from="84b44602-c11e-4aa0-86e4-e33ae0153189" sf:to="dc14808f-af5b-457e-ca6a-3f55d2d5b990" />
  </bpmn2:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_100">
      <bpmndi:BPMNEdge id="Flow_1jr0j1q_di" bpmnElement="Flow_1jr0j1q">
        <di:waypoint x="920" y="258" />
        <di:waypoint x="982" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1oesje1_di" bpmnElement="Flow_1oesje1">
        <di:waypoint x="760" y="258" />
        <di:waypoint x="820" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1y9y72b_di" bpmnElement="Flow_1y9y72b">
        <di:waypoint x="600" y="258" />
        <di:waypoint x="660" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1k2kjdm_di" bpmnElement="Flow_1k2kjdm">
        <di:waypoint x="448" y="258" />
        <di:waypoint x="500" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="412" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0vwbrxl_di" bpmnElement="Activity_0vwbrxl">
        <dc:Bounds x="500" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1gc2sq3_di" bpmnElement="Activity_1gc2sq3">
        <dc:Bounds x="660" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_17734ew_di" bpmnElement="Activity_17734ew">
        <dc:Bounds x="820" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_07sz6r5_di" bpmnElement="Event_07sz6r5">
        <dc:Bounds x="982" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn2:definitions>
', 0, NULL, N'', 0, NULL, CAST(0x0000AEBA00920580 AS DateTime), CAST(0x0000AEBC010CD106 AS DateTime))
INSERT [dbo].[WfProcess] ([ID], [ProcessGUID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (957, N'595cc76a-349a-4b0a-9154-ce17e3057924', N'1', N'Multiple_Process_Name_8706', N'Multiple_Process_Code_8706', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn2:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL" xmlns:sf="http://www.slickflow.com/schema/sf" id="bpmn2-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn2:process id="Process_100" sf:guid="595cc76a-349a-4b0a-9154-ce17e3057924" sf:code="Process_Code_8706" name="Process_Name_8706" isExecutable="false" version="1">
    <bpmn2:startEvent id="StartEvent_1" sf:guid="88b632f1-2698-45f1-b18a-ebb23cb87323">
      <bpmn2:outgoing>Flow_0vo9nrj</bpmn2:outgoing>
    </bpmn2:startEvent>
    <bpmn2:task id="Activity_0q2zurd" sf:guid="ea7b5fab-7e06-4ca9-ec34-22f0126a25ea" name="submit">
      <bpmn2:incoming>Flow_0vo9nrj</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1txn5ur</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_0vo9nrj" sf:guid="817bdf68-cc03-4c7a-e579-32ae016c76ae" sourceRef="StartEvent_1" targetRef="Activity_0q2zurd" sf:from="88b632f1-2698-45f1-b18a-ebb23cb87323" sf:to="ea7b5fab-7e06-4ca9-ec34-22f0126a25ea" />
    <bpmn2:sequenceFlow id="Flow_1txn5ur" sf:guid="4a46d141-2e41-4f88-f11f-a62f7954caec" sourceRef="Activity_0q2zurd" targetRef="Activity_002a2kp" sf:from="ea7b5fab-7e06-4ca9-ec34-22f0126a25ea" sf:to="0862cd35-b547-42de-df13-debd36b385f0" />
    <bpmn2:task id="Activity_0fpu030" sf:guid="6920fdd9-ce90-4676-c526-0a6be7fab13f" name="final approval">
      <bpmn2:incoming>Flow_1hgdwu3</bpmn2:incoming>
      <bpmn2:outgoing>Flow_167r2ze</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_1hgdwu3" sf:guid="d07d925b-b1ff-4220-d9f2-17aa1f75af0a" sourceRef="Activity_002a2kp" targetRef="Activity_0fpu030" sf:from="0862cd35-b547-42de-df13-debd36b385f0" sf:to="6920fdd9-ce90-4676-c526-0a6be7fab13f" />
    <bpmn2:endEvent id="Event_1eiokr5" sf:guid="9805427e-b3ae-4db1-f726-c6af43038a00">
      <bpmn2:incoming>Flow_167r2ze</bpmn2:incoming>
    </bpmn2:endEvent>
    <bpmn2:sequenceFlow id="Flow_167r2ze" sf:guid="6049f97a-6d93-44b7-b476-55008f658e88" sourceRef="Activity_0fpu030" targetRef="Event_1eiokr5" sf:from="6920fdd9-ce90-4676-c526-0a6be7fab13f" sf:to="9805427e-b3ae-4db1-f726-c6af43038a00" />
    <bpmn2:userTask id="Activity_002a2kp" sf:guid="0862cd35-b547-42de-df13-debd36b385f0" name="sign together">
      <bpmn2:extensionElements>
        <sf:multiSignDetail complexType="SignTogether" mergeType="Sequence" compareType="Count" completeOrder="2" />
      </bpmn2:extensionElements>
      <bpmn2:incoming>Flow_1txn5ur</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1hgdwu3</bpmn2:outgoing>
    </bpmn2:userTask>
  </bpmn2:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_100">
      <bpmndi:BPMNEdge id="Flow_167r2ze_di" bpmnElement="Flow_167r2ze">
        <di:waypoint x="920" y="258" />
        <di:waypoint x="982" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1hgdwu3_di" bpmnElement="Flow_1hgdwu3">
        <di:waypoint x="760" y="258" />
        <di:waypoint x="820" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1txn5ur_di" bpmnElement="Flow_1txn5ur">
        <di:waypoint x="600" y="258" />
        <di:waypoint x="660" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0vo9nrj_di" bpmnElement="Flow_0vo9nrj">
        <di:waypoint x="448" y="258" />
        <di:waypoint x="500" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="412" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0q2zurd_di" bpmnElement="Activity_0q2zurd">
        <dc:Bounds x="500" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0fpu030_di" bpmnElement="Activity_0fpu030">
        <dc:Bounds x="820" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_1eiokr5_di" bpmnElement="Event_1eiokr5">
        <dc:Bounds x="982" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0qmlq2r_di" bpmnElement="Activity_002a2kp">
        <dc:Bounds x="660" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn2:definitions>
', 0, NULL, N'', 0, NULL, CAST(0x0000AEBA013B45D3 AS DateTime), CAST(0x0000AEBB00A1F27A AS DateTime))
INSERT [dbo].[WfProcess] ([ID], [ProcessGUID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (958, N'8be2e5d2-e1c9-4eaa-9444-20008a2cdc61', N'1', N'Conditional_Process_Name_9115', N'Conditional_Process_Code_9115', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn2:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL" xmlns:sf="http://www.slickflow.com/schema/sf" id="bpmn2-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn2:process id="Process_100" sf:guid="8be2e5d2-e1c9-4eaa-9444-20008a2cdc61" sf:code="Sequence_Process_Code_9115" name="Sequence_Process_Name_9115" isExecutable="false" version="1">
    <bpmn2:task id="Activity_0hfs0tq" sf:guid="d3e3fa00-f16e-4e8e-fc64-59d4d05825e6" name="submit">
      <bpmn2:incoming>Flow_1hetrom</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1ui3mz5</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_1hetrom" sf:guid="bbdb10eb-4677-4880-ccac-046316461cff" sourceRef="StartEvent_1" targetRef="Activity_0hfs0tq" sf:from="d723a427-2ff4-4ff0-929b-be569935ec46" sf:to="d3e3fa00-f16e-4e8e-fc64-59d4d05825e6" />
    <bpmn2:sequenceFlow id="Flow_1ui3mz5" sf:guid="023302f3-362f-4310-addc-7197e3aff616" sourceRef="Activity_0hfs0tq" targetRef="Event_1com9as" sf:from="d3e3fa00-f16e-4e8e-fc64-59d4d05825e6" sf:to="060168fc-14b9-47ca-c187-6d05239ac082" />
    <bpmn2:task id="Activity_0wm1wid" sf:guid="bba86d0b-e6cc-47d7-c0cc-a85f05725a57" name="hr approval">
      <bpmn2:incoming>Flow_0z4t3bc</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0kr4zrp</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_0z4t3bc" sf:guid="8d723d57-83c0-4f5f-f3a3-26b9cc15c74f" sourceRef="Event_1com9as" targetRef="Activity_0wm1wid" sf:from="060168fc-14b9-47ca-c187-6d05239ac082" sf:to="bba86d0b-e6cc-47d7-c0cc-a85f05725a57">
      <bpmn2:conditionExpression>ugcyucu</bpmn2:conditionExpression>
    </bpmn2:sequenceFlow>
    <bpmn2:endEvent id="Event_01q1f7z" sf:guid="c2f22667-ff69-48ff-e845-933506413837">
      <bpmn2:incoming>Flow_165q2cw</bpmn2:incoming>
    </bpmn2:endEvent>
    <bpmn2:sequenceFlow id="Flow_0kr4zrp" sf:guid="d8fb0a8e-3b0c-452b-eeb0-19cb4e14c2db" sourceRef="Activity_0wm1wid" targetRef="Event_12q5dqr" sf:from="bba86d0b-e6cc-47d7-c0cc-a85f05725a57" sf:to="c2f22667-ff69-48ff-e845-933506413837" />
    <bpmn2:intermediateCatchEvent id="Event_1com9as" sf:guid="060168fc-14b9-47ca-c187-6d05239ac082" element="[object Object]">
      <bpmn2:incoming>Flow_1ui3mz5</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0z4t3bc</bpmn2:outgoing>
      <bpmn2:conditionalEventDefinition id="ConditionalEventDefinition_1y79d9l">
        <bpmn2:condition xsi:type="bpmn2:tFormalExpression" />
      </bpmn2:conditionalEventDefinition>
    </bpmn2:intermediateCatchEvent>
    <bpmn2:sequenceFlow id="Flow_04laypl" sf:guid="69317a84-29c5-4cd0-cae2-23f8fa3da6c2" sourceRef="Event_12q5dqr" targetRef="Activity_0z8sh6w" sf:from="5f95a26b-1be6-4e00-a95c-11c9164706dd" sf:to="c2f22667-ff69-48ff-e845-933506413837" />
    <bpmn2:task id="Activity_0z8sh6w" sf:guid="c61e477e-2a5f-4e61-f47e-56e5c0167e40" name="hava a vacation">
      <bpmn2:incoming>Flow_04laypl</bpmn2:incoming>
      <bpmn2:outgoing>Flow_165q2cw</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_165q2cw" sf:guid="98354c1c-c63c-4d49-b7b7-618b25922aba" sourceRef="Activity_0z8sh6w" targetRef="Event_01q1f7z" sf:from="c61e477e-2a5f-4e61-f47e-56e5c0167e40" sf:to="c2f22667-ff69-48ff-e845-933506413837" />
    <bpmn2:intermediateCatchEvent id="Event_12q5dqr" sf:guid="5f95a26b-1be6-4e00-a95c-11c9164706dd">
      <bpmn2:incoming>Flow_0kr4zrp</bpmn2:incoming>
      <bpmn2:outgoing>Flow_04laypl</bpmn2:outgoing>
      <bpmn2:messageEventDefinition id="MessageEventDefinition_08ya0yf" messageRef="Message_3pkctfr" />
    </bpmn2:intermediateCatchEvent>
    <bpmn2:startEvent id="StartEvent_1" sf:guid="d723a427-2ff4-4ff0-929b-be569935ec46">
      <bpmn2:outgoing>Flow_1hetrom</bpmn2:outgoing>
      <bpmn2:timerEventDefinition id="TimerEventDefinition_0wen5m2">
        <bpmn2:timeDate xsi:type="bpmn2:tFormalExpression">2022-08-09</bpmn2:timeDate>
      </bpmn2:timerEventDefinition>
    </bpmn2:startEvent>
  </bpmn2:process>
  <bpmn2:message id="Message_3pkctfr" name="Message_3pkctfr" />
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_100">
      <bpmndi:BPMNEdge id="Flow_165q2cw_di" bpmnElement="Flow_165q2cw">
        <di:waypoint x="1100" y="258" />
        <di:waypoint x="1212" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_04laypl_di" bpmnElement="Flow_04laypl">
        <di:waypoint x="948" y="258" />
        <di:waypoint x="1000" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0kr4zrp_di" bpmnElement="Flow_0kr4zrp">
        <di:waypoint x="840" y="258" />
        <di:waypoint x="912" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0z4t3bc_di" bpmnElement="Flow_0z4t3bc">
        <di:waypoint x="688" y="258" />
        <di:waypoint x="740" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1ui3mz5_di" bpmnElement="Flow_1ui3mz5">
        <di:waypoint x="600" y="258" />
        <di:waypoint x="652" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1hetrom_di" bpmnElement="Flow_1hetrom">
        <di:waypoint x="398" y="258" />
        <di:waypoint x="500" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNShape id="Activity_0hfs0tq_di" bpmnElement="Activity_0hfs0tq">
        <dc:Bounds x="500" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0wm1wid_di" bpmnElement="Activity_0wm1wid">
        <dc:Bounds x="740" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_01q1f7z_di" bpmnElement="Event_01q1f7z">
        <dc:Bounds x="1212" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_06haejl_di" bpmnElement="Event_1com9as">
        <dc:Bounds x="652" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0z8sh6w_di" bpmnElement="Activity_0z8sh6w">
        <dc:Bounds x="1000" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_1u57ejy_di" bpmnElement="Event_12q5dqr">
        <dc:Bounds x="912" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_0vn3k5e_di" bpmnElement="StartEvent_1">
        <dc:Bounds x="362" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn2:definitions>
', 0, NULL, N'', 0, NULL, CAST(0x0000AEBB00D658A9 AS DateTime), CAST(0x0000AEBC010BD7BE AS DateTime))
INSERT [dbo].[WfProcess] ([ID], [ProcessGUID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (959, N'58b3d12b-010f-4da4-8f92-103935266840', N'1', N'Process_Name_9349', N'Process_Code_9349', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn2:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL" xmlns:sf="http://www.slickflow.com/schema/sf" id="bpmn2-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn2:process id="Process_100" sf:guid="58b3d12b-010f-4da4-8f92-103935266840" sf:code="Process_Code_9349" name="Process_Name_9349" isExecutable="false" version="1">
    <bpmn2:startEvent id="StartEvent_1" sf:guid="1bca8d2c-1593-4764-8efb-202712c6409a">
      <bpmn2:outgoing>Flow_0nfre8y</bpmn2:outgoing>
    </bpmn2:startEvent>
    <bpmn2:task id="Activity_1jxill8" sf:guid="ceae8d4e-4523-4d15-ce9f-9f135ae459ab">
      <bpmn2:incoming>Flow_0nfre8y</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0ti9vab</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_0nfre8y" sf:guid="04d14d94-a344-4112-c22e-e84739d9986f" sourceRef="StartEvent_1" targetRef="Activity_1jxill8" sf:from="1bca8d2c-1593-4764-8efb-202712c6409a" sf:to="ceae8d4e-4523-4d15-ce9f-9f135ae459ab" />
    <bpmn2:endEvent id="Event_1g9yfmb" sf:guid="18b3fde1-d88a-4be5-81ec-2dcc4d202a3b" name="End">
      <bpmn2:incoming>Flow_0ti9vab</bpmn2:incoming>
    </bpmn2:endEvent>
    <bpmn2:sequenceFlow id="Flow_0ti9vab" sf:guid="28cd9127-e241-4b22-9219-94a832f31ce9" sourceRef="Activity_1jxill8" targetRef="Event_1g9yfmb" sf:from="ceae8d4e-4523-4d15-ce9f-9f135ae459ab" sf:to="18b3fde1-d88a-4be5-81ec-2dcc4d202a3b" />
  </bpmn2:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_100">
      <bpmndi:BPMNEdge id="Flow_0nfre8y_di" bpmnElement="Flow_0nfre8y">
        <di:waypoint x="448" y="258" />
        <di:waypoint x="500" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0ti9vab_di" bpmnElement="Flow_0ti9vab">
        <di:waypoint x="600" y="258" />
        <di:waypoint x="652" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="412" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1jxill8_di" bpmnElement="Activity_1jxill8">
        <dc:Bounds x="500" y="218" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_1g9yfmb_di" bpmnElement="Event_1g9yfmb">
        <dc:Bounds x="652" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="661" y="283" width="19" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn2:definitions>
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               ', 0, NULL, NULL, 0, NULL, CAST(0x0000AEBC00E95BC4 AS DateTime), CAST(0x0000AEBC00E9905C AS DateTime))
INSERT [dbo].[WfProcess] ([ID], [ProcessGUID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (960, N'dc482c4c-02a8-468e-b570-34121b660f73', N'1', N'Process_Name_8141', N'Process_Code_8141', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn2:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL" xmlns:sf="http://www.slickflow.com/schema/sf" id="bpmn2-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn2:process id="Process_100" sf:guid="dc482c4c-02a8-468e-b570-34121b660f73" sf:code="Process_Code_8141" sf:version="1" sf:url="qqqqqqqqqqqq" name="Process_Name_8141" isExecutable="true">
    <bpmn2:documentation>yffffffffyu8</bpmn2:documentation>
    <bpmn2:startEvent id="StartEvent_1" sf:guid="44339305-6611-4415-a34f-878417a21c79" />
  </bpmn2:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_100">
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="412" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn2:definitions>
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   ', 0, NULL, NULL, 0, NULL, CAST(0x0000AEBC013638C7 AS DateTime), NULL)
INSERT [dbo].[WfProcess] ([ID], [ProcessGUID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (961, N'1f60fe27-cb9f-44da-ab6c-696f381a2d08', N'1', N'Process_Name_9911', N'Process_Code_9911', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn2:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL" xmlns:sf="http://www.slickflow.com/schema/sf" id="bpmn2-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn2:process id="Process_Code_9911" sf:guid="1f60fe27-cb9f-44da-ab6c-696f381a2d08" sf:code="Process_Code_9911" sf:version="1" name="Process_Name_9911" isExecutable="false">
    <bpmn2:startEvent id="StartEvent_1" sf:guid="e5e716fb-ca95-4933-9c4f-9fab49e448e2" name="Start" />
  </bpmn2:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_Code_9911">
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="412" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="418" y="283" width="24" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn2:definitions>
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     ', 0, NULL, NULL, 0, NULL, CAST(0x0000AEBD00A2B435 AS DateTime), NULL)
INSERT [dbo].[WfProcess] ([ID], [ProcessGUID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (962, N'0fa008f2-4a1d-4b9e-9b25-08514a509d72', N'1', N'Process_Name_5490', N'Process_Code_5490', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn2:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL" xmlns:sf="http://www.slickflow.com/schema/sf" id="bpmn2-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn2:process id="Process_Code_5490" sf:guid="0fa008f2-4a1d-4b9e-9b25-08514a509d72" sf:code="Process_Code_5490" name="Process_Name_5490" isExecutable="false" version="1">
    <bpmn2:startEvent id="StartEvent_1" sf:guid="dda3869b-8a48-48ac-98cf-082bac9f49e2" name="Start">
      <bpmn2:outgoing>Flow_1g0zup4</bpmn2:outgoing>
    </bpmn2:startEvent>
    <bpmn2:task id="Activity_1l9tdyk" sf:guid="5a0deb2b-21ab-416b-c000-972e32e30d84">
      <bpmn2:incoming>Flow_1g0zup4</bpmn2:incoming>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_1g0zup4" sf:guid="4c0f4f1e-b053-41e8-996a-d36a82a1c116" sourceRef="StartEvent_1" targetRef="Activity_1l9tdyk" sf:from="dda3869b-8a48-48ac-98cf-082bac9f49e2" sf:to="5a0deb2b-21ab-416b-c000-972e32e30d84" />
  </bpmn2:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_Code_5490">
      <bpmndi:BPMNEdge id="Flow_1g0zup4_di" bpmnElement="Flow_1g0zup4">
        <di:waypoint x="448" y="258" />
        <di:waypoint x="500" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="412" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="418" y="283" width="24" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1l9tdyk_di" bpmnElement="Activity_1l9tdyk">
        <dc:Bounds x="500" y="218" width="100" height="80" />
      </bpmndi:BPMNShape>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn2:definitions>
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        ', 0, NULL, NULL, 0, NULL, CAST(0x0000AEBD00E00A54 AS DateTime), NULL)
INSERT [dbo].[WfProcess] ([ID], [ProcessGUID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (963, N'd72dba55-52cc-4214-a3f4-b204da6497ed', N'1', N'Process_Name_3463', N'Process_Code_3463', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn2:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL" xmlns:sf="http://www.slickflow.com/schema/sf" id="bpmn2-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn2:process id="Process_Code_3463" sf:guid="d72dba55-52cc-4214-a3f4-b204da6497ed" sf:code="Process_Code_3463" name="Simple134" isExecutable="false" version="1">
    <bpmn2:startEvent id="StartEvent_1" sf:guid="d9295b72-c5a3-49c8-8a72-93b6e88d3dbf" name="Start">
      <bpmn2:outgoing>Flow_0ie5u6m</bpmn2:outgoing>
    </bpmn2:startEvent>
    <bpmn2:task id="Activity_117zlqx" sf:guid="47cc70ee-8b5e-41c3-8488-93004698537e" name="submit">
      <bpmn2:incoming>Flow_0ie5u6m</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1oxes6b</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_0ie5u6m" sf:guid="b6eecfa1-c0d0-49dc-aa5c-5849f00370c8" sourceRef="StartEvent_1" targetRef="Activity_117zlqx" sf:from="d9295b72-c5a3-49c8-8a72-93b6e88d3dbf" sf:to="47cc70ee-8b5e-41c3-8488-93004698537e" />
    <bpmn2:endEvent id="Event_0e64ko5" sf:guid="e8f9178f-86c4-46f3-f67b-3f12c03a64d1" name="End">
      <bpmn2:incoming>Flow_1oxes6b</bpmn2:incoming>
    </bpmn2:endEvent>
    <bpmn2:sequenceFlow id="Flow_1oxes6b" sf:guid="e95ab78b-eac6-46a8-cb34-6e6cd135cbd6" sourceRef="Activity_117zlqx" targetRef="Event_0e64ko5" sf:from="47cc70ee-8b5e-41c3-8488-93004698537e" sf:to="e8f9178f-86c4-46f3-f67b-3f12c03a64d1" />
  </bpmn2:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_Code_3463">
      <bpmndi:BPMNEdge id="Flow_1oxes6b_di" bpmnElement="Flow_1oxes6b">
        <di:waypoint x="600" y="258" />
        <di:waypoint x="652" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0ie5u6m_di" bpmnElement="Flow_0ie5u6m">
        <di:waypoint x="448" y="258" />
        <di:waypoint x="500" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="412" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="418" y="283" width="24" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_117zlqx_di" bpmnElement="Activity_117zlqx">
        <dc:Bounds x="500" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_0e64ko5_di" bpmnElement="Event_0e64ko5">
        <dc:Bounds x="652" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="661" y="283" width="19" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn2:definitions>
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             ', 0, NULL, NULL, 0, NULL, CAST(0x0000AEBD00EFCEAC AS DateTime), CAST(0x0000AEBD00FD2955 AS DateTime))
INSERT [dbo].[WfProcess] ([ID], [ProcessGUID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (964, N'd72dba55-52cc-4214-a3f4-b204da6497ed', N'2', N'Simple134lhjolh', N'Process_Code_3463', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn2:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL" xmlns:sf="http://www.slickflow.com/schema/sf" id="bpmn2-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn2:process id="Process_Code_3463" sf:guid="d72dba55-52cc-4214-a3f4-b204da6497ed" sf:code="Process_Code_3463" name="Simple134lhjolh" isExecutable="false" version="1">
    <bpmn2:startEvent id="StartEvent_1" sf:guid="d9295b72-c5a3-49c8-8a72-93b6e88d3dbf" name="Start">
      <bpmn2:outgoing>Flow_0ie5u6m</bpmn2:outgoing>
    </bpmn2:startEvent>
    <bpmn2:task id="Activity_117zlqx" sf:guid="47cc70ee-8b5e-41c3-8488-93004698537e" name="submit">
      <bpmn2:incoming>Flow_0ie5u6m</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1oxes6b</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_0ie5u6m" sf:guid="b6eecfa1-c0d0-49dc-aa5c-5849f00370c8" sourceRef="StartEvent_1" targetRef="Activity_117zlqx" sf:from="d9295b72-c5a3-49c8-8a72-93b6e88d3dbf" sf:to="47cc70ee-8b5e-41c3-8488-93004698537e" />
    <bpmn2:endEvent id="Event_0e64ko5" sf:guid="e8f9178f-86c4-46f3-f67b-3f12c03a64d1" name="End">
      <bpmn2:incoming>Flow_1oxes6b</bpmn2:incoming>
    </bpmn2:endEvent>
    <bpmn2:sequenceFlow id="Flow_1oxes6b" sf:guid="e95ab78b-eac6-46a8-cb34-6e6cd135cbd6" sourceRef="Activity_117zlqx" targetRef="Event_0e64ko5" sf:from="47cc70ee-8b5e-41c3-8488-93004698537e" sf:to="e8f9178f-86c4-46f3-f67b-3f12c03a64d1" />
  </bpmn2:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_Code_3463">
      <bpmndi:BPMNEdge id="Flow_1oxes6b_di" bpmnElement="Flow_1oxes6b">
        <di:waypoint x="600" y="258" />
        <di:waypoint x="652" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0ie5u6m_di" bpmnElement="Flow_0ie5u6m">
        <di:waypoint x="448" y="258" />
        <di:waypoint x="500" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="412" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="418" y="283" width="24" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_117zlqx_di" bpmnElement="Activity_117zlqx">
        <dc:Bounds x="500" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_0e64ko5_di" bpmnElement="Event_0e64ko5">
        <dc:Bounds x="652" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="661" y="283" width="19" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn2:definitions>
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       ', 0, NULL, NULL, 0, NULL, CAST(0x0000AEBD00FDCE91 AS DateTime), CAST(0x0000AEBD011B66AA AS DateTime))
INSERT [dbo].[WfProcess] ([ID], [ProcessGUID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (965, N'2d988eef-c857-453d-9918-ab8d50273302', N'1', N'InterCondition_Process_Name_7339', N'Process_Code_7339', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn2:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL" xmlns:sf="http://www.slickflow.com/schema/sf" id="bpmn2-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn2:process id="Process_Code_7339" sf:guid="2d988eef-c857-453d-9918-ab8d50273302" sf:code="Process_Code_7339" name="InterCondition_Process_Name_7339" isExecutable="false" version="1">
    <bpmn2:startEvent id="StartEvent_1" sf:guid="1f2f5dc9-e30b-4146-a1a2-fe8f72621740" name="Start">
      <bpmn2:outgoing>Flow_05wy8jw</bpmn2:outgoing>
    </bpmn2:startEvent>
    <bpmn2:task id="Activity_0zin9te" sf:guid="544ea721-06ae-4a3f-a690-4de95d8635c2" name="submit">
      <bpmn2:incoming>Flow_05wy8jw</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1v273ds</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_05wy8jw" sf:guid="34d47cb3-fa45-4edd-a1c5-88d92feae64c" sourceRef="StartEvent_1" targetRef="Activity_0zin9te" sf:from="1f2f5dc9-e30b-4146-a1a2-fe8f72621740" sf:to="544ea721-06ae-4a3f-a690-4de95d8635c2" />
    <bpmn2:sequenceFlow id="Flow_1v273ds" sf:guid="01743e32-8ead-4d5a-8026-778126901e4b" sourceRef="Activity_0zin9te" targetRef="Event_0n423z8" sf:from="544ea721-06ae-4a3f-a690-4de95d8635c2" sf:to="071dbb1f-5fad-4f79-8a24-3be4cf8a5cbf" />
    <bpmn2:intermediateCatchEvent id="Event_0n423z8" sf:guid="071dbb1f-5fad-4f79-8a24-3be4cf8a5cbf">
      <bpmn2:incoming>Flow_1v273ds</bpmn2:incoming>
      <bpmn2:outgoing>Flow_006t723</bpmn2:outgoing>
      <bpmn2:conditionalEventDefinition id="ConditionalEventDefinition_1jdalzg">
        <bpmn2:condition xsi:type="bpmn2:tFormalExpression">@day1-@day2&gt;=3</bpmn2:condition>
      </bpmn2:conditionalEventDefinition>
    </bpmn2:intermediateCatchEvent>
    <bpmn2:task id="Activity_0suu3ud" sf:guid="793c0285-0c7f-41e1-923a-ff75d2a9ee58" name="final approval">
      <bpmn2:incoming>Flow_006t723</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1a9useh</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_006t723" sf:guid="f41b546d-76b0-45e0-902c-d1e3b691d56f" sourceRef="Event_0n423z8" targetRef="Activity_0suu3ud" sf:from="071dbb1f-5fad-4f79-8a24-3be4cf8a5cbf" sf:to="793c0285-0c7f-41e1-923a-ff75d2a9ee58" />
    <bpmn2:endEvent id="Event_0w36r7z" sf:guid="f60c5810-c7a0-4ee9-c43d-eec71d14065e">
      <bpmn2:incoming>Flow_1a9useh</bpmn2:incoming>
    </bpmn2:endEvent>
    <bpmn2:sequenceFlow id="Flow_1a9useh" sf:guid="72cba9f3-95f1-4d44-87ee-6b25066f556b" sourceRef="Activity_0suu3ud" targetRef="Event_0w36r7z" sf:from="793c0285-0c7f-41e1-923a-ff75d2a9ee58" sf:to="f60c5810-c7a0-4ee9-c43d-eec71d14065e" />
  </bpmn2:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_Code_7339">
      <bpmndi:BPMNEdge id="Flow_05wy8jw_di" bpmnElement="Flow_05wy8jw">
        <di:waypoint x="448" y="258" />
        <di:waypoint x="500" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1v273ds_di" bpmnElement="Flow_1v273ds">
        <di:waypoint x="600" y="258" />
        <di:waypoint x="652" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_006t723_di" bpmnElement="Flow_006t723">
        <di:waypoint x="688" y="258" />
        <di:waypoint x="740" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1a9useh_di" bpmnElement="Flow_1a9useh">
        <di:waypoint x="840" y="258" />
        <di:waypoint x="892" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="412" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="418" y="283" width="24" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0zin9te_di" bpmnElement="Activity_0zin9te">
        <dc:Bounds x="500" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_1nchxrk_di" bpmnElement="Event_0n423z8">
        <dc:Bounds x="652" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0suu3ud_di" bpmnElement="Activity_0suu3ud">
        <dc:Bounds x="740" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_0w36r7z_di" bpmnElement="Event_0w36r7z">
        <dc:Bounds x="892" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn2:definitions>
', 0, NULL, NULL, 0, NULL, CAST(0x0000AEBF00F30B4F AS DateTime), CAST(0x0000AEBF00F4F723 AS DateTime))
INSERT [dbo].[WfProcess] ([ID], [ProcessGUID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (966, N'52023958-996d-48ac-b9a7-ba51e48d4821', N'1', N'ConditionalStart_Process_Name_1475', N'Process_Code_1475', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn2:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL" xmlns:sf="http://www.slickflow.com/schema/sf" id="bpmn2-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn2:process id="Process_Code_1475" sf:guid="52023958-996d-48ac-b9a7-ba51e48d4821" sf:code="Process_Code_1475" name="ConditionalStart_Process_Name_1475" isExecutable="false" version="1">
    <bpmn2:startEvent id="StartEvent_1" sf:guid="3dec72c9-09eb-482c-8565-58bd2be28eba" name="Start">
      <bpmn2:outgoing>Flow_0qo8ll2</bpmn2:outgoing>
      <bpmn2:conditionalEventDefinition id="ConditionalEventDefinition_0bp6d4g">
        <bpmn2:condition xsi:type="bpmn2:tFormalExpression">days&gt;2</bpmn2:condition>
      </bpmn2:conditionalEventDefinition>
    </bpmn2:startEvent>
    <bpmn2:task id="Activity_1ku2t6g" sf:guid="44e50c37-c992-46cd-a315-fb0b39360256" name="submit">
      <bpmn2:incoming>Flow_0qo8ll2</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1i9z5mq</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_0qo8ll2" sf:guid="411561b7-8e89-4b6a-c87b-98d3672bc285" sourceRef="StartEvent_1" targetRef="Activity_1ku2t6g" sf:from="3dec72c9-09eb-482c-8565-58bd2be28eba" sf:to="44e50c37-c992-46cd-a315-fb0b39360256" />
    <bpmn2:task id="Activity_0zkrl8d" sf:guid="ce3e52d1-167d-48ce-aaf9-2589f3a970ab" name="dept approval">
      <bpmn2:incoming>Flow_1i9z5mq</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0bfvptm</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_1i9z5mq" sf:guid="a46a6cf5-51a5-4a86-e3c6-9d11f4cac1af" sourceRef="Activity_1ku2t6g" targetRef="Activity_0zkrl8d" sf:from="44e50c37-c992-46cd-a315-fb0b39360256" sf:to="ce3e52d1-167d-48ce-aaf9-2589f3a970ab" />
    <bpmn2:endEvent id="Event_04c49pk" sf:guid="ed92e0c7-7637-4a38-e189-e7a38d2dac97">
      <bpmn2:incoming>Flow_0bfvptm</bpmn2:incoming>
    </bpmn2:endEvent>
    <bpmn2:sequenceFlow id="Flow_0bfvptm" sf:guid="1d5af6ef-6864-4059-a4a0-77187dd6bc51" sourceRef="Activity_0zkrl8d" targetRef="Event_04c49pk" sf:from="ce3e52d1-167d-48ce-aaf9-2589f3a970ab" sf:to="ed92e0c7-7637-4a38-e189-e7a38d2dac97" />
  </bpmn2:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_Code_1475">
      <bpmndi:BPMNEdge id="Flow_0qo8ll2_di" bpmnElement="Flow_0qo8ll2">
        <di:waypoint x="448" y="258" />
        <di:waypoint x="500" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1i9z5mq_di" bpmnElement="Flow_1i9z5mq">
        <di:waypoint x="600" y="258" />
        <di:waypoint x="660" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0bfvptm_di" bpmnElement="Flow_0bfvptm">
        <di:waypoint x="760" y="258" />
        <di:waypoint x="822" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNShape id="Event_134b8tf_di" bpmnElement="StartEvent_1">
        <dc:Bounds x="412" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="418" y="283" width="24" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1ku2t6g_di" bpmnElement="Activity_1ku2t6g">
        <dc:Bounds x="500" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0zkrl8d_di" bpmnElement="Activity_0zkrl8d">
        <dc:Bounds x="660" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_04c49pk_di" bpmnElement="Event_04c49pk">
        <dc:Bounds x="822" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn2:definitions>
                                                                                                           ', 0, NULL, NULL, 0, NULL, CAST(0x0000AEBF01271B7D AS DateTime), CAST(0x0000AEBF01272D61 AS DateTime))
INSERT [dbo].[WfProcess] ([ID], [ProcessGUID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (967, N'21868559-34bc-442c-9ecd-e56dbeff5c11', N'1', N'TimerStart_Process_Name_9932', N'Process_Code_9932', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn2:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL" xmlns:sf="http://www.slickflow.com/schema/sf" id="bpmn2-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn2:process id="Process_Code_9932" sf:guid="21868559-34bc-442c-9ecd-e56dbeff5c11" sf:code="Process_Code_9932" name="TimerStart_Process_Name_9932" isExecutable="false" version="1">
    <bpmn2:startEvent id="StartEvent_1" sf:guid="44a809df-ceea-4bc1-bb30-93a25078237b" name="Start">
      <bpmn2:outgoing>Flow_1qlpdhh</bpmn2:outgoing>
      <bpmn2:timerEventDefinition id="TimerEventDefinition_1qbfb4n">
        <bpmn2:timeDuration xsi:type="bpmn2:tFormalExpression">* 2 * * *</bpmn2:timeDuration>
      </bpmn2:timerEventDefinition>
    </bpmn2:startEvent>
    <bpmn2:task id="Activity_19ll2wk" sf:guid="aef8c09c-e2c2-4f8e-85c5-406cff05f65c" name="submit">
      <bpmn2:incoming>Flow_1qlpdhh</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0ap7160</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_1qlpdhh" sf:guid="56250368-74fc-465a-a4af-a87abde8fbad" sourceRef="StartEvent_1" targetRef="Activity_19ll2wk" sf:from="44a809df-ceea-4bc1-bb30-93a25078237b" sf:to="aef8c09c-e2c2-4f8e-85c5-406cff05f65c" />
    <bpmn2:task id="Activity_1v7unjc" sf:guid="10903780-3215-4f74-e2af-d0d26760feae" name="dept approval">
      <bpmn2:incoming>Flow_0ap7160</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1obk7r6</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_0ap7160" sf:guid="c123b66b-a98a-43f2-bbcb-6f4d8aa87dc2" sourceRef="Activity_19ll2wk" targetRef="Activity_1v7unjc" sf:from="aef8c09c-e2c2-4f8e-85c5-406cff05f65c" sf:to="10903780-3215-4f74-e2af-d0d26760feae" />
    <bpmn2:endEvent id="Event_0qa746r" sf:guid="95257425-6c64-49c3-b524-9f8b3367dd04">
      <bpmn2:incoming>Flow_1obk7r6</bpmn2:incoming>
    </bpmn2:endEvent>
    <bpmn2:sequenceFlow id="Flow_1obk7r6" sf:guid="82b553ad-c2cc-42ed-f3ca-a80cc7e6c2dc" sourceRef="Activity_1v7unjc" targetRef="Event_0qa746r" sf:from="10903780-3215-4f74-e2af-d0d26760feae" sf:to="95257425-6c64-49c3-b524-9f8b3367dd04" />
  </bpmn2:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_Code_9932">
      <bpmndi:BPMNEdge id="Flow_1obk7r6_di" bpmnElement="Flow_1obk7r6">
        <di:waypoint x="760" y="258" />
        <di:waypoint x="822" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0ap7160_di" bpmnElement="Flow_0ap7160">
        <di:waypoint x="600" y="258" />
        <di:waypoint x="660" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1qlpdhh_di" bpmnElement="Flow_1qlpdhh">
        <di:waypoint x="448" y="258" />
        <di:waypoint x="500" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNShape id="Event_00xidiy_di" bpmnElement="StartEvent_1">
        <dc:Bounds x="412" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="418" y="283" width="24" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_19ll2wk_di" bpmnElement="Activity_19ll2wk">
        <dc:Bounds x="500" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1v7unjc_di" bpmnElement="Activity_1v7unjc">
        <dc:Bounds x="660" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_0qa746r_di" bpmnElement="Event_0qa746r">
        <dc:Bounds x="822" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn2:definitions>
                                                                                                                             ', 1, N'* 2 * * *', NULL, 0, NULL, CAST(0x0000AEBF012F4192 AS DateTime), CAST(0x0000AEBF015F77E0 AS DateTime))
INSERT [dbo].[WfProcess] ([ID], [ProcessGUID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (968, N'e4ae3139-48b3-4a1d-8f93-ab5821366a02', N'1', N'RecieverType_Process_Name_9566', N'Process_Code_9566', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn2:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL" xmlns:sf="http://www.slickflow.com/schema/sf" id="bpmn2-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn2:process id="Process_Code_9566" sf:guid="e4ae3139-48b3-4a1d-8f93-ab5821366a02" sf:code="Process_Code_9566" name="RecieverType_Process_Name_9566" isExecutable="false" version="1">
    <bpmn2:startEvent id="StartEvent_1" sf:guid="2050d4e3-e959-4585-8c67-cc43f3165da5" name="Start">
      <bpmn2:outgoing>Flow_0tzv6u0</bpmn2:outgoing>
    </bpmn2:startEvent>
    <bpmn2:task id="Activity_0gmfd6n" sf:guid="d6a93303-92f3-4337-b65a-2d191926cbaa" name="submit">
      <bpmn2:incoming>Flow_0tzv6u0</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1rarmd8</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_0tzv6u0" sf:guid="ebd66fc9-65a0-4a81-a84c-d80b24930d52" sourceRef="StartEvent_1" targetRef="Activity_0gmfd6n" sf:from="2050d4e3-e959-4585-8c67-cc43f3165da5" sf:to="d6a93303-92f3-4337-b65a-2d191926cbaa" />
    <bpmn2:task id="Activity_0b9qg7f" sf:guid="f007a55b-2343-4211-802f-1629fee3a161" name="dept manager approval">
      <bpmn2:incoming>Flow_1rarmd8</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1l9cyco</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_1rarmd8" sf:guid="427256f6-76de-4fea-e526-a3c19803aad9" sourceRef="Activity_0gmfd6n" targetRef="Activity_0b9qg7f" sf:from="d6a93303-92f3-4337-b65a-2d191926cbaa" sf:to="f007a55b-2343-4211-802f-1629fee3a161">
      <bpmn2:extensionElements>
        <sf:groupBehaviours recieverType="Superior" />
      </bpmn2:extensionElements>
    </bpmn2:sequenceFlow>
    <bpmn2:endEvent id="Event_1e4y459" sf:guid="05a5379c-6beb-45c7-dc34-ab044b1f8e70" name="End">
      <bpmn2:incoming>Flow_1l9cyco</bpmn2:incoming>
    </bpmn2:endEvent>
    <bpmn2:sequenceFlow id="Flow_1l9cyco" sf:guid="9dba0846-e937-411e-d7da-4b9597670786" sourceRef="Activity_0b9qg7f" targetRef="Event_1e4y459" sf:from="f007a55b-2343-4211-802f-1629fee3a161" sf:to="05a5379c-6beb-45c7-dc34-ab044b1f8e70" />
  </bpmn2:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_Code_9566">
      <bpmndi:BPMNEdge id="Flow_1l9cyco_di" bpmnElement="Flow_1l9cyco">
        <di:waypoint x="760" y="258" />
        <di:waypoint x="822" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1rarmd8_di" bpmnElement="Flow_1rarmd8">
        <di:waypoint x="600" y="258" />
        <di:waypoint x="660" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0tzv6u0_di" bpmnElement="Flow_0tzv6u0">
        <di:waypoint x="448" y="258" />
        <di:waypoint x="500" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="412" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="418" y="283" width="24" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0gmfd6n_di" bpmnElement="Activity_0gmfd6n">
        <dc:Bounds x="500" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0b9qg7f_di" bpmnElement="Activity_0b9qg7f">
        <dc:Bounds x="660" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_1e4y459_di" bpmnElement="Event_1e4y459">
        <dc:Bounds x="822" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="831" y="283" width="19" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn2:definitions>
                                  ', 0, NULL, NULL, 0, NULL, CAST(0x0000AEC0007E2774 AS DateTime), CAST(0x0000AEC000807677 AS DateTime))
INSERT [dbo].[WfProcess] ([ID], [ProcessGUID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (969, N'2a3c73f5-081e-426f-9178-93cdc67286c4', N'1', N'AskForLeaving_Processs', N'Process_Code_3434', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn2:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL" xmlns:sf="http://www.slickflow.com/schema/sf" id="bpmn2-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn2:process id="Process_Code_3434" sf:guid="2a3c73f5-081e-426f-9178-93cdc67286c4" sf:code="Process_Code_3434" name="AskForLeaving_Processs" isExecutable="false" version="1">
    <bpmn2:startEvent id="StartEvent_1" sf:guid="b9cbc1fb-c0af-4b76-9024-45aff26448e4" name="Start">
      <bpmn2:outgoing>Flow_01nh6a5</bpmn2:outgoing>
    </bpmn2:startEvent>
    <bpmn2:task id="Activity_0455xjg" sf:guid="c03b3b51-604b-48db-c5e1-4215ad01a22e" name="submit">
      <bpmn2:extensionElements>
        <sf:performers>
          <sf:performer name="普通员工" outerId="1" outerCode="employees" outerType="Role" />
        </sf:performers>
      </bpmn2:extensionElements>
      <bpmn2:incoming>Flow_01nh6a5</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1hnajae</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_01nh6a5" sf:guid="76f7bbe5-0d68-43ec-efd3-bf91932d5b75" sourceRef="StartEvent_1" targetRef="Activity_0455xjg" sf:from="b9cbc1fb-c0af-4b76-9024-45aff26448e4" sf:to="c03b3b51-604b-48db-c5e1-4215ad01a22e" />
    <bpmn2:exclusiveGateway id="Gateway_17fv9ol" sf:guid="5b203f12-4992-4e7a-c91f-12cc7fc6eb05">
      <bpmn2:incoming>Flow_1hnajae</bpmn2:incoming>
      <bpmn2:outgoing>Flow_149y4hy</bpmn2:outgoing>
      <bpmn2:outgoing>Flow_0hc28lh</bpmn2:outgoing>
    </bpmn2:exclusiveGateway>
    <bpmn2:sequenceFlow id="Flow_1hnajae" sf:guid="1534e0a5-e14a-4a1a-f587-29fd67ca9c83" sourceRef="Activity_0455xjg" targetRef="Gateway_17fv9ol" sf:from="c03b3b51-604b-48db-c5e1-4215ad01a22e" sf:to="5b203f12-4992-4e7a-c91f-12cc7fc6eb05" />
    <bpmn2:task id="Activity_1kcsr5d" sf:guid="cf13dd79-df24-4503-92fe-88556238bf7e" name="Dept Manager Approval">
      <bpmn2:extensionElements>
        <sf:performers>
          <sf:performer name="部门经理" outerId="2" outerCode="depmanager" outerType="Role" />
        </sf:performers>
      </bpmn2:extensionElements>
      <bpmn2:incoming>Flow_149y4hy</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0oks1po</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_149y4hy" sf:guid="ed25ab84-a608-4472-dbde-6a9ee1acbe1b" name="days&#60;3" sourceRef="Gateway_17fv9ol" targetRef="Activity_1kcsr5d" sf:from="5b203f12-4992-4e7a-c91f-12cc7fc6eb05" sf:to="cf13dd79-df24-4503-92fe-88556238bf7e">
      <bpmn2:extensionElements>
        <sf:groupBehaviours recieverType="Superior" />
      </bpmn2:extensionElements>
      <bpmn2:conditionExpression>days&lt;3</bpmn2:conditionExpression>
    </bpmn2:sequenceFlow>
    <bpmn2:task id="Activity_1h8of8g" sf:guid="b56b6594-1a11-49ed-d450-058216bef608" name="CEO Approval">
      <bpmn2:extensionElements>
        <sf:performers>
          <sf:performer name="总经理" outerId="8" outerCode="generalmanager" outerType="Role" />
        </sf:performers>
      </bpmn2:extensionElements>
      <bpmn2:incoming>Flow_0hc28lh</bpmn2:incoming>
      <bpmn2:outgoing>Flow_186px29</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_0hc28lh" sf:guid="3ce248d0-46d5-4062-b403-3292866f0189" name="days&#62;=3" sourceRef="Gateway_17fv9ol" targetRef="Activity_1h8of8g" sf:from="5b203f12-4992-4e7a-c91f-12cc7fc6eb05" sf:to="b56b6594-1a11-49ed-d450-058216bef608">
      <bpmn2:conditionExpression>days&gt;=3</bpmn2:conditionExpression>
    </bpmn2:sequenceFlow>
    <bpmn2:exclusiveGateway id="Gateway_0xf5tpz" sf:guid="4a68990e-b637-4449-d1e1-48cafc2e2cf8">
      <bpmn2:incoming>Flow_0oks1po</bpmn2:incoming>
      <bpmn2:incoming>Flow_186px29</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1i8lxty</bpmn2:outgoing>
    </bpmn2:exclusiveGateway>
    <bpmn2:sequenceFlow id="Flow_0oks1po" sf:guid="4d47fe5f-66c2-4cc2-9076-0e1dd4b2d92a" sourceRef="Activity_1kcsr5d" targetRef="Gateway_0xf5tpz" sf:from="cf13dd79-df24-4503-92fe-88556238bf7e" sf:to="4a68990e-b637-4449-d1e1-48cafc2e2cf8" />
    <bpmn2:sequenceFlow id="Flow_186px29" sf:guid="cab5ae0e-cc32-4d41-8336-c4830b456584" sourceRef="Activity_1h8of8g" targetRef="Gateway_0xf5tpz" sf:from="b56b6594-1a11-49ed-d450-058216bef608" sf:to="4a68990e-b637-4449-d1e1-48cafc2e2cf8" />
    <bpmn2:task id="Activity_0yl4zlo" sf:guid="8f3f3707-275f-4798-db33-2c0754321246" name="HR Approval">
      <bpmn2:extensionElements>
        <sf:performers>
          <sf:performer name="人事经理" outerId="3" outerCode="hrmanager" outerType="Role" />
        </sf:performers>
      </bpmn2:extensionElements>
      <bpmn2:incoming>Flow_1i8lxty</bpmn2:incoming>
      <bpmn2:outgoing>Flow_12rafcm</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_1i8lxty" sf:guid="32feeba9-834f-430a-cbbd-2649e08576d2" sourceRef="Gateway_0xf5tpz" targetRef="Activity_0yl4zlo" sf:from="4a68990e-b637-4449-d1e1-48cafc2e2cf8" sf:to="8f3f3707-275f-4798-db33-2c0754321246" />
    <bpmn2:endEvent id="Event_0xplf2z" sf:guid="4944c340-ce6d-4b28-e19d-16841e76f5d3" name="End">
      <bpmn2:incoming>Flow_12rafcm</bpmn2:incoming>
    </bpmn2:endEvent>
    <bpmn2:sequenceFlow id="Flow_12rafcm" sf:guid="556f89c8-1765-4056-fd12-9e5ff257311e" sourceRef="Activity_0yl4zlo" targetRef="Event_0xplf2z" sf:from="8f3f3707-275f-4798-db33-2c0754321246" sf:to="4944c340-ce6d-4b28-e19d-16841e76f5d3" />
  </bpmn2:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_Code_3434">
      <bpmndi:BPMNEdge id="Flow_12rafcm_di" bpmnElement="Flow_12rafcm">
        <di:waypoint x="1140" y="258" />
        <di:waypoint x="1222" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1i8lxty_di" bpmnElement="Flow_1i8lxty">
        <di:waypoint x="965" y="258" />
        <di:waypoint x="1040" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_186px29_di" bpmnElement="Flow_186px29">
        <di:waypoint x="860" y="370" />
        <di:waypoint x="940" y="370" />
        <di:waypoint x="940" y="283" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0oks1po_di" bpmnElement="Flow_0oks1po">
        <di:waypoint x="860" y="150" />
        <di:waypoint x="940" y="150" />
        <di:waypoint x="940" y="233" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0hc28lh_di" bpmnElement="Flow_0hc28lh">
        <di:waypoint x="680" y="283" />
        <di:waypoint x="680" y="370" />
        <di:waypoint x="760" y="370" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="674" y="324" width="43" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_149y4hy_di" bpmnElement="Flow_149y4hy">
        <di:waypoint x="680" y="233" />
        <di:waypoint x="680" y="150" />
        <di:waypoint x="760" y="150" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="677" y="189" width="38" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1hnajae_di" bpmnElement="Flow_1hnajae">
        <di:waypoint x="600" y="258" />
        <di:waypoint x="655" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_01nh6a5_di" bpmnElement="Flow_01nh6a5">
        <di:waypoint x="448" y="258" />
        <di:waypoint x="500" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="412" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="418" y="283" width="24" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0455xjg_di" bpmnElement="Activity_0455xjg">
        <dc:Bounds x="500" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Gateway_17fv9ol_di" bpmnElement="Gateway_17fv9ol" isMarkerVisible="true">
        <dc:Bounds x="655" y="233" width="50" height="50" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1h8of8g_di" bpmnElement="Activity_1h8of8g">
        <dc:Bounds x="760" y="330" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Gateway_0xf5tpz_di" bpmnElement="Gateway_0xf5tpz" isMarkerVisible="true">
        <dc:Bounds x="915" y="233" width="50" height="50" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0yl4zlo_di" bpmnElement="Activity_0yl4zlo">
        <dc:Bounds x="1040" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_0xplf2z_di" bpmnElement="Event_0xplf2z">
        <dc:Bounds x="1222" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="1230" y="283" width="20" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1kcsr5d_di" bpmnElement="Activity_1kcsr5d">
        <dc:Bounds x="760" y="110" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn2:definitions>
', 0, NULL, NULL, 0, NULL, CAST(0x0000AECC00A92635 AS DateTime), CAST(0x0000B1F2014BE972 AS DateTime))
INSERT [dbo].[WfProcess] ([ID], [ProcessGUID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (970, N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', N'1', N'OrderProcess_Name_7829', N'Process_Code_7829', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn2:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL" xmlns:sf="http://www.slickflow.com/schema/sf" id="bpmn2-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn2:process id="Process_Code_7829" sf:guid="6a51f9c1-3c81-46e8-bd34-084bd7b940ad" sf:code="Process_Code_7829" name="OrderProcess_Name_7829" isExecutable="false" version="1">
    <bpmn2:startEvent id="StartEvent_1" sf:guid="0253ff58-47f1-4203-9986-ef4d3e49199d" name="Start">
      <bpmn2:outgoing>Flow_0pw6g7s</bpmn2:outgoing>
    </bpmn2:startEvent>
    <bpmn2:task id="Activity_19izw9e" sf:guid="62b71f52-2c6d-4476-d3c8-bd5f9ac1b94f" sf:code="Dispatching" name="Dispatch(派单)">
      <bpmn2:extensionElements>
        <sf:performers>
          <sf:performer name="业务员" outerId="9" outerCode="salesmate" outerType="Role" />
        </sf:performers>
      </bpmn2:extensionElements>
      <bpmn2:incoming>Flow_0pw6g7s</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0r3t1aq</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_0pw6g7s" sf:guid="d9d38bd2-1b85-49cd-b3ce-c884429054f1" sourceRef="StartEvent_1" targetRef="Activity_19izw9e" sf:from="0253ff58-47f1-4203-9986-ef4d3e49199d" sf:to="62b71f52-2c6d-4476-d3c8-bd5f9ac1b94f" />
    <bpmn2:exclusiveGateway id="Gateway_1qnv0ou" sf:guid="1bc8a032-ab92-4f6e-9bf1-28e87e5a6ece">
      <bpmn2:incoming>Flow_0r3t1aq</bpmn2:incoming>
      <bpmn2:outgoing>Flow_037zl1c</bpmn2:outgoing>
      <bpmn2:outgoing>Flow_0h19ak5</bpmn2:outgoing>
      <bpmn2:outgoing>Flow_008vkxd</bpmn2:outgoing>
    </bpmn2:exclusiveGateway>
    <bpmn2:sequenceFlow id="Flow_0r3t1aq" sf:guid="6997a8c8-9df8-49b2-b7ca-05bd973946f2" sourceRef="Activity_19izw9e" targetRef="Gateway_1qnv0ou" sf:from="62b71f52-2c6d-4476-d3c8-bd5f9ac1b94f" sf:to="1bc8a032-ab92-4f6e-9bf1-28e87e5a6ece" />
    <bpmn2:task id="Activity_0g010k5" sf:guid="b112b4d0-1cc1-4667-bdda-73c85e16266e" sf:code="Delivering" name="Print(打印发货单)">
      <bpmn2:extensionElements>
        <sf:performers>
          <sf:performer name="包装员" outerId="13" outerCode="expressmate" outerType="Role" />
        </sf:performers>
      </bpmn2:extensionElements>
      <bpmn2:incoming>Flow_037zl1c</bpmn2:incoming>
      <bpmn2:incoming>Flow_1awf0u3</bpmn2:incoming>
      <bpmn2:outgoing>Flow_123danr</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_037zl1c" sf:guid="fdd241b5-2e4a-45f5-8888-5614745e60ce" sourceRef="Gateway_1qnv0ou" targetRef="Activity_0g010k5" sf:from="1bc8a032-ab92-4f6e-9bf1-28e87e5a6ece" sf:to="b112b4d0-1cc1-4667-bdda-73c85e16266e">
      <bpmn2:conditionExpression>CanUseStock == "true" &amp;&amp; IsHavingWeight == "true"</bpmn2:conditionExpression>
    </bpmn2:sequenceFlow>
    <bpmn2:task id="Activity_1ony46i" sf:guid="1c1bceae-7bce-4394-cc0b-6bbf1a87bf2d" sf:code="Weighting" name="Weight(称重)">
      <bpmn2:extensionElements>
        <sf:performers>
          <sf:performer name="包装员" outerId="13" outerCode="expressmate" outerType="Role" />
        </sf:performers>
      </bpmn2:extensionElements>
      <bpmn2:incoming>Flow_0h19ak5</bpmn2:incoming>
      <bpmn2:incoming>Flow_1mqxtxv</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1awf0u3</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_0h19ak5" sf:guid="fcdb6b60-d3b6-4d04-981d-396f483218ac" sourceRef="Gateway_1qnv0ou" targetRef="Activity_1ony46i" sf:from="1bc8a032-ab92-4f6e-9bf1-28e87e5a6ece" sf:to="1c1bceae-7bce-4394-cc0b-6bbf1a87bf2d">
      <bpmn2:conditionExpression>CanUseStock == "true" &amp;&amp; IsHavingWeight == "false"</bpmn2:conditionExpression>
    </bpmn2:sequenceFlow>
    <bpmn2:task id="Activity_0h602el" sf:guid="126ed9cc-b661-4e77-ae95-b5001ad9ce9c" sf:code="Sampling" name="Sample(打样)">
      <bpmn2:extensionElements>
        <sf:performers>
          <sf:performer name="打样员" outerId="10" outerCode="techmate" outerType="Role" />
        </sf:performers>
      </bpmn2:extensionElements>
      <bpmn2:incoming>Flow_008vkxd</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1ww0wbt</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_008vkxd" sf:guid="f6feb6be-aa40-45bc-b69b-069b9ca49227" sourceRef="Gateway_1qnv0ou" targetRef="Activity_0h602el" sf:from="1bc8a032-ab92-4f6e-9bf1-28e87e5a6ece" sf:to="126ed9cc-b661-4e77-ae95-b5001ad9ce9c">
      <bpmn2:conditionExpression>CanUseStock == "false" &amp;&amp; IsHavingWeight == "false"</bpmn2:conditionExpression>
    </bpmn2:sequenceFlow>
    <bpmn2:task id="Activity_1lh8exc" sf:guid="5b6ba25a-8dd4-40c4-d27e-f834f0be0168" sf:code="Manufacturing" name="Manufacture(生产)">
      <bpmn2:extensionElements>
        <sf:performers>
          <sf:performer name="包装员" outerId="13" outerCode="expressmate" outerType="Role" />
          <sf:performer name="跟单员" outerId="11" outerCode="merchandiser" outerType="Role" />
        </sf:performers>
      </bpmn2:extensionElements>
      <bpmn2:incoming>Flow_1ww0wbt</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0eyyhe6</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_1ww0wbt" sf:guid="59262a37-51df-4fd0-cd54-36797fddfa65" sourceRef="Activity_0h602el" targetRef="Activity_1lh8exc" sf:from="126ed9cc-b661-4e77-ae95-b5001ad9ce9c" sf:to="5b6ba25a-8dd4-40c4-d27e-f834f0be0168" />
    <bpmn2:task id="Activity_1057yne" sf:guid="78a69a65-d406-4056-9dc2-b25751fc6263" sf:code="QCChecking" name="QCCheck(质检)">
      <bpmn2:extensionElements>
        <sf:performers>
          <sf:performer name="质检员" outerId="12" outerCode="qcmate" outerType="Role" />
        </sf:performers>
      </bpmn2:extensionElements>
      <bpmn2:incoming>Flow_0eyyhe6</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1mqxtxv</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_0eyyhe6" sf:guid="b9c7e52f-0e0c-4085-cba0-9f5f0759af41" sourceRef="Activity_1lh8exc" targetRef="Activity_1057yne" sf:from="5b6ba25a-8dd4-40c4-d27e-f834f0be0168" sf:to="78a69a65-d406-4056-9dc2-b25751fc6263" />
    <bpmn2:endEvent id="Event_1ba6vfy" sf:guid="652441d3-2e61-4df0-a50f-499c253e6c19" name="End">
      <bpmn2:incoming>Flow_123danr</bpmn2:incoming>
    </bpmn2:endEvent>
    <bpmn2:sequenceFlow id="Flow_123danr" sf:guid="d4d614a4-792f-4932-8366-8781be420caa" sourceRef="Activity_0g010k5" targetRef="Event_1ba6vfy" sf:from="b112b4d0-1cc1-4667-bdda-73c85e16266e" sf:to="652441d3-2e61-4df0-a50f-499c253e6c19" />
    <bpmn2:sequenceFlow id="Flow_1mqxtxv" sf:guid="4b54a9e5-34b2-490f-b57e-548f7f7b0e14" sourceRef="Activity_1057yne" targetRef="Activity_1ony46i" sf:from="78a69a65-d406-4056-9dc2-b25751fc6263" sf:to="1c1bceae-7bce-4394-cc0b-6bbf1a87bf2d" />
    <bpmn2:sequenceFlow id="Flow_1awf0u3" sf:guid="89efad47-ba53-48d8-9856-0581d6915187" sourceRef="Activity_1ony46i" targetRef="Activity_0g010k5" sf:from="1c1bceae-7bce-4394-cc0b-6bbf1a87bf2d" sf:to="b112b4d0-1cc1-4667-bdda-73c85e16266e" />
  </bpmn2:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_Code_7829">
      <bpmndi:BPMNEdge id="Flow_1awf0u3_di" bpmnElement="Flow_1awf0u3">
        <di:waypoint x="1200" y="218" />
        <di:waypoint x="1200" y="140" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1mqxtxv_di" bpmnElement="Flow_1mqxtxv">
        <di:waypoint x="1200" y="380" />
        <di:waypoint x="1200" y="298" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_123danr_di" bpmnElement="Flow_123danr">
        <di:waypoint x="1250" y="100" />
        <di:waypoint x="1442" y="100" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0eyyhe6_di" bpmnElement="Flow_0eyyhe6">
        <di:waypoint x="1050" y="420" />
        <di:waypoint x="1150" y="420" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1ww0wbt_di" bpmnElement="Flow_1ww0wbt">
        <di:waypoint x="860" y="420" />
        <di:waypoint x="950" y="420" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_008vkxd_di" bpmnElement="Flow_008vkxd">
        <di:waypoint x="680" y="283" />
        <di:waypoint x="680" y="420" />
        <di:waypoint x="760" y="420" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0h19ak5_di" bpmnElement="Flow_0h19ak5">
        <di:waypoint x="705" y="258" />
        <di:waypoint x="1150" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_037zl1c_di" bpmnElement="Flow_037zl1c">
        <di:waypoint x="680" y="233" />
        <di:waypoint x="680" y="100" />
        <di:waypoint x="1150" y="100" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0r3t1aq_di" bpmnElement="Flow_0r3t1aq">
        <di:waypoint x="600" y="258" />
        <di:waypoint x="655" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0pw6g7s_di" bpmnElement="Flow_0pw6g7s">
        <di:waypoint x="448" y="258" />
        <di:waypoint x="500" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="412" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="418" y="283" width="24" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_19izw9e_di" bpmnElement="Activity_19izw9e">
        <dc:Bounds x="500" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Gateway_1qnv0ou_di" bpmnElement="Gateway_1qnv0ou" isMarkerVisible="true">
        <dc:Bounds x="655" y="233" width="50" height="50" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0h602el_di" bpmnElement="Activity_0h602el">
        <dc:Bounds x="760" y="380" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1057yne_di" bpmnElement="Activity_1057yne">
        <dc:Bounds x="1150" y="380" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_1ba6vfy_di" bpmnElement="Event_1ba6vfy">
        <dc:Bounds x="1442" y="82" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="1451" y="125" width="20" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1ony46i_di" bpmnElement="Activity_1ony46i">
        <dc:Bounds x="1150" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0g010k5_di" bpmnElement="Activity_0g010k5">
        <dc:Bounds x="1150" y="60" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1lh8exc_di" bpmnElement="Activity_1lh8exc">
        <dc:Bounds x="950" y="380" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn2:definitions>
', 0, NULL, NULL, 0, NULL, CAST(0x0000AECD0073AE5A AS DateTime), CAST(0x0000B25D00B3F8DE AS DateTime))
INSERT [dbo].[WfProcess] ([ID], [ProcessGUID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1077, N'7a6492ce-aae1-4662-93d3-e238b6f86880', N'1', N'Process_Name_595N29', N'Process_Code_595N29', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn2:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL" id="bpmn2-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn2:process id="Process_Code_1458" sf:guid="1cf95ead-218f-44f3-869f-65be4f8ff7ef" sf:code="Process_Code_595N29" name="Process_Name_595N29" isExecutable="false" sf:name="Process_Name_1458" sf:version="1">
    <bpmn2:startEvent id="StartEvent_1" sf:guid="db1050ce-281c-4350-bee2-560f310322a3" name="Start">
      <bpmn2:outgoing>Flow_0tsxeas</bpmn2:outgoing>
    </bpmn2:startEvent>
    <bpmn2:task id="Activity_0goc5c7" sf:guid="36875aa5-d450-40f0-a2ea-dda2f866d9ec">
      <bpmn2:incoming>Flow_0tsxeas</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0re6tyd</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_0tsxeas" sf:guid="083f49f0-1595-4648-fd2f-b557c51f45a5" sourceRef="StartEvent_1" targetRef="Activity_0goc5c7" sf:from="db1050ce-281c-4350-bee2-560f310322a3" sf:to="36875aa5-d450-40f0-a2ea-dda2f866d9ec" />
    <bpmn2:task id="Activity_10i0gj4" sf:guid="69de4f01-95d3-491f-d56a-f6692d119dcb">
      <bpmn2:incoming>Flow_0re6tyd</bpmn2:incoming>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_0re6tyd" sf:guid="b50029c8-26e0-4aa4-f0f9-766bc9258017" sourceRef="Activity_0goc5c7" targetRef="Activity_10i0gj4" sf:from="36875aa5-d450-40f0-a2ea-dda2f866d9ec" sf:to="69de4f01-95d3-491f-d56a-f6692d119dcb" />
  </bpmn2:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_Code_1458">
      <bpmndi:BPMNEdge id="Flow_0tsxeas_di" bpmnElement="Flow_0tsxeas">
        <di:waypoint x="448" y="258" />
        <di:waypoint x="500" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0re6tyd_di" bpmnElement="Flow_0re6tyd">
        <di:waypoint x="600" y="258" />
        <di:waypoint x="660" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="412" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="418" y="283" width="24" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0goc5c7_di" bpmnElement="Activity_0goc5c7">
        <dc:Bounds x="500" y="218" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_10i0gj4_di" bpmnElement="Activity_10i0gj4">
        <dc:Bounds x="660" y="218" width="100" height="80" />
      </bpmndi:BPMNShape>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn2:definitions>
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         ', 0, NULL, NULL, 0, NULL, CAST(0x0000AEF60104FB76 AS DateTime), NULL)
INSERT [dbo].[WfProcess] ([ID], [ProcessGUID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1090, N'bd46e812-b278-4d51-acf6-b250e50d27c3', N'1', N'Process_Name_R8IUXK', N'Process_Code_R8IUXK', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn2:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL" id="bpmn2-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn2:process id="Process_Code_9016" sf:guid="84ccb3c2-8530-4c7b-c54c-6dd55d8724e0" sf:code="Process_Code_R8IUXK" name="Process_Name_R8IUXK" isExecutable="false" sf:name="Process_Name_9016" sf:version="1">
    <bpmn2:startEvent id="StartEvent_1" sf:guid="7737cbf7-dae9-4f7c-918c-b25c05da48dc" name="Start">
      <bpmn2:outgoing>Flow_1myntgu</bpmn2:outgoing>
    </bpmn2:startEvent>
    <bpmn2:task id="Activity_16rhsby" sf:guid="47c82c4e-57cd-4144-dbd6-9c917220b3d8">
      <bpmn2:incoming>Flow_1myntgu</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1k89md3</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_1myntgu" sf:guid="a2efdcca-0749-434d-f753-bec2d6c90d69" sourceRef="StartEvent_1" targetRef="Activity_16rhsby" sf:from="7737cbf7-dae9-4f7c-918c-b25c05da48dc" sf:to="47c82c4e-57cd-4144-dbd6-9c917220b3d8" />
    <bpmn2:exclusiveGateway id="Gateway_1pt72k5" sf:guid="27d10ad5-364d-4c35-be68-eeb21c353106">
      <bpmn2:incoming>Flow_1k89md3</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0b8pksh</bpmn2:outgoing>
      <bpmn2:outgoing>Flow_1m9rk4g</bpmn2:outgoing>
    </bpmn2:exclusiveGateway>
    <bpmn2:sequenceFlow id="Flow_1k89md3" sf:guid="e50356bf-a499-46f1-a923-0ba02bae1f46" sourceRef="Activity_16rhsby" targetRef="Gateway_1pt72k5" sf:from="47c82c4e-57cd-4144-dbd6-9c917220b3d8" sf:to="27d10ad5-364d-4c35-be68-eeb21c353106" />
    <bpmn2:task id="Activity_0mpp6ux" sf:guid="e4a88c95-f609-4206-f412-fedb55d76599">
      <bpmn2:incoming>Flow_0b8pksh</bpmn2:incoming>
      <bpmn2:outgoing>Flow_08jqxcg</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_0b8pksh" sf:guid="ca755a7e-0434-43b1-98b3-410af1684540" sourceRef="Gateway_1pt72k5" targetRef="Activity_0mpp6ux" sf:from="27d10ad5-364d-4c35-be68-eeb21c353106" sf:to="e4a88c95-f609-4206-f412-fedb55d76599" />
    <bpmn2:task id="Activity_1n49mng" sf:guid="e26021cb-bdb3-4e5d-931c-bf3025a9c5d0">
      <bpmn2:incoming>Flow_1m9rk4g</bpmn2:incoming>
      <bpmn2:outgoing>Flow_125shxq</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_1m9rk4g" sf:guid="f23eb9a5-f9f3-4bf4-9c12-9591fad0f5b4" sourceRef="Gateway_1pt72k5" targetRef="Activity_1n49mng" sf:from="27d10ad5-364d-4c35-be68-eeb21c353106" sf:to="e26021cb-bdb3-4e5d-931c-bf3025a9c5d0" />
    <bpmn2:exclusiveGateway id="Gateway_0jnk4gu" sf:guid="0d571779-b9df-4fa7-f80f-3473752430b9">
      <bpmn2:incoming>Flow_08jqxcg</bpmn2:incoming>
      <bpmn2:incoming>Flow_125shxq</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0ec6i1l</bpmn2:outgoing>
    </bpmn2:exclusiveGateway>
    <bpmn2:sequenceFlow id="Flow_08jqxcg" sf:guid="49321fae-0be8-4b95-9ed0-fe101d57f926" sourceRef="Activity_0mpp6ux" targetRef="Gateway_0jnk4gu" sf:from="e4a88c95-f609-4206-f412-fedb55d76599" sf:to="0d571779-b9df-4fa7-f80f-3473752430b9" />
    <bpmn2:sequenceFlow id="Flow_125shxq" sf:guid="d0586108-d14e-44a5-e172-166ed49e091a" sourceRef="Activity_1n49mng" targetRef="Gateway_0jnk4gu" sf:from="e26021cb-bdb3-4e5d-931c-bf3025a9c5d0" sf:to="0d571779-b9df-4fa7-f80f-3473752430b9" />
    <bpmn2:task id="Activity_1al7ae9" sf:guid="545e2586-4785-4cff-d7a1-2c56652b8a09">
      <bpmn2:incoming>Flow_0ec6i1l</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1rdxlm5</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_0ec6i1l" sf:guid="a75941bf-7d5a-4293-9259-6ac0cacf8859" sourceRef="Gateway_0jnk4gu" targetRef="Activity_1al7ae9" sf:from="0d571779-b9df-4fa7-f80f-3473752430b9" sf:to="545e2586-4785-4cff-d7a1-2c56652b8a09" />
    <bpmn2:endEvent id="Event_049idc2" sf:guid="99c8399e-2c6b-4a08-a03a-2f56efd9a94d">
      <bpmn2:incoming>Flow_1rdxlm5</bpmn2:incoming>
    </bpmn2:endEvent>
    <bpmn2:sequenceFlow id="Flow_1rdxlm5" sf:guid="b590d7bc-f770-4997-ac0f-74677c15852a" sourceRef="Activity_1al7ae9" targetRef="Event_049idc2" sf:from="545e2586-4785-4cff-d7a1-2c56652b8a09" sf:to="99c8399e-2c6b-4a08-a03a-2f56efd9a94d" />
  </bpmn2:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_Code_9016">
      <bpmndi:BPMNEdge id="Flow_1myntgu_di" bpmnElement="Flow_1myntgu">
        <di:waypoint x="448" y="258" />
        <di:waypoint x="500" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1k89md3_di" bpmnElement="Flow_1k89md3">
        <di:waypoint x="600" y="258" />
        <di:waypoint x="655" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0b8pksh_di" bpmnElement="Flow_0b8pksh">
        <di:waypoint x="680" y="233" />
        <di:waypoint x="680" y="160" />
        <di:waypoint x="780" y="160" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1m9rk4g_di" bpmnElement="Flow_1m9rk4g">
        <di:waypoint x="680" y="283" />
        <di:waypoint x="680" y="370" />
        <di:waypoint x="780" y="370" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_08jqxcg_di" bpmnElement="Flow_08jqxcg">
        <di:waypoint x="880" y="160" />
        <di:waypoint x="980" y="160" />
        <di:waypoint x="980" y="233" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_125shxq_di" bpmnElement="Flow_125shxq">
        <di:waypoint x="880" y="370" />
        <di:waypoint x="980" y="370" />
        <di:waypoint x="980" y="283" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0ec6i1l_di" bpmnElement="Flow_0ec6i1l">
        <di:waypoint x="1005" y="258" />
        <di:waypoint x="1060" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1rdxlm5_di" bpmnElement="Flow_1rdxlm5">
        <di:waypoint x="1160" y="258" />
        <di:waypoint x="1222" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="412" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="418" y="283" width="24" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_16rhsby_di" bpmnElement="Activity_16rhsby">
        <dc:Bounds x="500" y="218" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Gateway_1pt72k5_di" bpmnElement="Gateway_1pt72k5" isMarkerVisible="true">
        <dc:Bounds x="655" y="233" width="50" height="50" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Gateway_0jnk4gu_di" bpmnElement="Gateway_0jnk4gu" isMarkerVisible="true">
        <dc:Bounds x="955" y="233" width="50" height="50" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0mpp6ux_di" bpmnElement="Activity_0mpp6ux">
        <dc:Bounds x="780" y="120" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1n49mng_di" bpmnElement="Activity_1n49mng">
        <dc:Bounds x="780" y="330" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1al7ae9_di" bpmnElement="Activity_1al7ae9">
        <dc:Bounds x="1060" y="218" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_049idc2_di" bpmnElement="Event_049idc2">
        <dc:Bounds x="1222" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn2:definitions>
', 0, NULL, NULL, 0, NULL, CAST(0x0000AEF60166FD4F AS DateTime), NULL)
INSERT [dbo].[WfProcess] ([ID], [ProcessGUID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1163, N'd57e924c-955c-4589-8ca9-6a1d7171c035', N'1', N'SignForwardTest', N'Process_Code_2654', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn2:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL" id="bpmn2-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn2:process id="Process_Code_2654" sf:guid="d57e924c-955c-4589-8ca9-6a1d7171c035" sf:code="Process_Code_2654" name="SignForwardTest" isExecutable="true" sf:version="1">
    <bpmn2:startEvent id="StartEvent_1" sf:guid="c3871892-5781-42d4-9dc4-4e1eed6ae3f5" name="Start">
      <bpmn2:outgoing>Flow_0mq2yjg</bpmn2:outgoing>
    </bpmn2:startEvent>
    <bpmn2:task id="Activity_1xzc586" sf:guid="4990e9b1-48c4-4822-be00-8befdeceadac" name="submit">
      <bpmn2:incoming>Flow_0mq2yjg</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0uc9vo0</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_0mq2yjg" sf:guid="870b24df-96c5-4125-ca28-8b6ddb8a63c6" sourceRef="StartEvent_1" targetRef="Activity_1xzc586" sf:from="c3871892-5781-42d4-9dc4-4e1eed6ae3f5" sf:to="4990e9b1-48c4-4822-be00-8befdeceadac" />
    <bpmn2:task id="Activity_1oovugb" sf:guid="b0c1e301-af74-4009-9600-8cc3117fc32a" name="signforward">
      <bpmn2:extensionElements>
        <sf:multiSignDetail complexType="SignForward" mergeType="Sequence" compareType="Count" completeOrder="2" />
      </bpmn2:extensionElements>
      <bpmn2:incoming>Flow_0uc9vo0</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1najfxn</bpmn2:outgoing>
      <bpmn2:multiInstanceLoopCharacteristics isSequential="true" />
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_0uc9vo0" sf:guid="e6e235d3-3a97-4bb7-88a3-08989f8ba265" sourceRef="Activity_1xzc586" targetRef="Activity_1oovugb" sf:from="4990e9b1-48c4-4822-be00-8befdeceadac" sf:to="b0c1e301-af74-4009-9600-8cc3117fc32a" />
    <bpmn2:task id="Activity_0hdojzo" sf:guid="f750da40-f2f9-485c-ef1f-6c80c68d90ca" name="final approval">
      <bpmn2:incoming>Flow_1najfxn</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0xqcw42</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_1najfxn" sf:guid="a2537997-2e1f-4cab-b9c4-7a8a74ca6eb5" sourceRef="Activity_1oovugb" targetRef="Activity_0hdojzo" sf:from="b0c1e301-af74-4009-9600-8cc3117fc32a" sf:to="f750da40-f2f9-485c-ef1f-6c80c68d90ca" />
    <bpmn2:endEvent id="Event_1g75k7k" sf:guid="40ecc295-b3d4-4886-e0f7-6e1d0290d6dd">
      <bpmn2:incoming>Flow_0xqcw42</bpmn2:incoming>
    </bpmn2:endEvent>
    <bpmn2:sequenceFlow id="Flow_0xqcw42" sf:guid="bdbe5279-fc56-476b-9f3c-679e8fd35aa6" sourceRef="Activity_0hdojzo" targetRef="Event_1g75k7k" sf:from="f750da40-f2f9-485c-ef1f-6c80c68d90ca" sf:to="40ecc295-b3d4-4886-e0f7-6e1d0290d6dd" />
  </bpmn2:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_Code_2654">
      <bpmndi:BPMNEdge id="Flow_0xqcw42_di" bpmnElement="Flow_0xqcw42">
        <di:waypoint x="920" y="258" />
        <di:waypoint x="982" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1najfxn_di" bpmnElement="Flow_1najfxn">
        <di:waypoint x="760" y="258" />
        <di:waypoint x="820" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0uc9vo0_di" bpmnElement="Flow_0uc9vo0">
        <di:waypoint x="600" y="258" />
        <di:waypoint x="660" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0mq2yjg_di" bpmnElement="Flow_0mq2yjg">
        <di:waypoint x="448" y="258" />
        <di:waypoint x="500" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="412" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="418" y="283" width="24" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1xzc586_di" bpmnElement="Activity_1xzc586">
        <dc:Bounds x="500" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1oovugb_di" bpmnElement="Activity_1oovugb">
        <dc:Bounds x="660" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0hdojzo_di" bpmnElement="Activity_0hdojzo">
        <dc:Bounds x="820" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_1g75k7k_di" bpmnElement="Event_1g75k7k">
        <dc:Bounds x="982" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn2:definitions>
', 0, NULL, NULL, 0, NULL, CAST(0x0000AEFF0097EAB8 AS DateTime), CAST(0x0000AEFF0098E30D AS DateTime))
INSERT [dbo].[WfProcess] ([ID], [ProcessGUID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1164, N'63c701ae-7dcf-462d-b6ce-1b1bedebd557', N'1', N'SequenceTest', N'Process_Code_6971', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn2:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL" id="bpmn2-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn2:process id="Process_Code_6971" sf:guid="63c701ae-7dcf-462d-b6ce-1b1bedebd557" sf:code="Process_Code_6971" name="SequenceTest" isExecutable="true" sf:version="1">
    <bpmn2:startEvent id="StartEvent_1" sf:guid="c48abb60-cf86-46e7-8eab-24c4d7a9470c" name="Start">
      <bpmn2:outgoing>Flow_0sddhvu</bpmn2:outgoing>
    </bpmn2:startEvent>
    <bpmn2:task id="Activity_0j5x1i9" sf:guid="ba54c500-5e0d-41a0-8156-67d90108990e" name="task01">
      <bpmn2:incoming>Flow_0sddhvu</bpmn2:incoming>
      <bpmn2:outgoing>Flow_07acz5o</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_0sddhvu" sf:guid="42f08d0b-2e7f-44c4-e462-66a7e6112146" sourceRef="StartEvent_1" targetRef="Activity_0j5x1i9" sf:from="c48abb60-cf86-46e7-8eab-24c4d7a9470c" sf:to="ba54c500-5e0d-41a0-8156-67d90108990e" />
    <bpmn2:task id="Activity_19mudll" sf:guid="0670094d-5d9d-4c45-ac70-606686dce2a2" name="task02">
      <bpmn2:incoming>Flow_07acz5o</bpmn2:incoming>
      <bpmn2:outgoing>Flow_19bunit</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_07acz5o" sf:guid="022bac9d-e75c-42d5-8e21-00c7372166c4" sourceRef="Activity_0j5x1i9" targetRef="Activity_19mudll" sf:from="ba54c500-5e0d-41a0-8156-67d90108990e" sf:to="0670094d-5d9d-4c45-ac70-606686dce2a2" />
    <bpmn2:task id="Activity_0y8vb8v" sf:guid="de491780-9c0a-4890-d457-2feaa0568460" name="task03">
      <bpmn2:incoming>Flow_19bunit</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0lnxy5b</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_19bunit" sf:guid="013cd3af-3b8c-49ca-b695-82db1414a3ea" sourceRef="Activity_19mudll" targetRef="Activity_0y8vb8v" sf:from="0670094d-5d9d-4c45-ac70-606686dce2a2" sf:to="de491780-9c0a-4890-d457-2feaa0568460" />
    <bpmn2:endEvent id="Event_1qmcqq9" sf:guid="aacda913-0766-4384-e6ee-80b961621b2d">
      <bpmn2:incoming>Flow_0lnxy5b</bpmn2:incoming>
    </bpmn2:endEvent>
    <bpmn2:sequenceFlow id="Flow_0lnxy5b" sf:guid="735dbe00-357a-40a0-81ad-106aac9d9b8d" sourceRef="Activity_0y8vb8v" targetRef="Event_1qmcqq9" sf:from="de491780-9c0a-4890-d457-2feaa0568460" sf:to="aacda913-0766-4384-e6ee-80b961621b2d" />
  </bpmn2:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_Code_6971">
      <bpmndi:BPMNEdge id="Flow_19bunit_di" bpmnElement="Flow_19bunit">
        <di:waypoint x="760" y="258" />
        <di:waypoint x="820" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_07acz5o_di" bpmnElement="Flow_07acz5o">
        <di:waypoint x="600" y="258" />
        <di:waypoint x="660" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0sddhvu_di" bpmnElement="Flow_0sddhvu">
        <di:waypoint x="448" y="258" />
        <di:waypoint x="500" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0lnxy5b_di" bpmnElement="Flow_0lnxy5b">
        <di:waypoint x="920" y="258" />
        <di:waypoint x="982" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="412" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="418" y="283" width="24" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0j5x1i9_di" bpmnElement="Activity_0j5x1i9">
        <dc:Bounds x="500" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_19mudll_di" bpmnElement="Activity_19mudll">
        <dc:Bounds x="660" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0y8vb8v_di" bpmnElement="Activity_0y8vb8v">
        <dc:Bounds x="820" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_1qmcqq9_di" bpmnElement="Event_1qmcqq9">
        <dc:Bounds x="982" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn2:definitions>
', 0, NULL, NULL, 0, NULL, CAST(0x0000AF01006EC568 AS DateTime), CAST(0x0000B0B401012A29 AS DateTime))
INSERT [dbo].[WfProcess] ([ID], [ProcessGUID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1243, N'd328d673-6de8-48cf-9ebb-67f1dcc08457', N'1', N'调休申请', N'askforleave_txsq', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn2:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL" id="bpmn2-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn2:process id="pProcess_Code_3815" sf:guid="d328d673-6de8-48cf-9ebb-67f1dcc08457" sf:code="askforleave_txsq" name="调休申请" isExecutable="true" sf:version="1">
    <bpmn2:startEvent id="StartEvent_1" sf:guid="9f83ba5e-53a3-4409-9611-42252cece8af">
      <bpmn2:outgoing>Flow_0krf2of</bpmn2:outgoing>
    </bpmn2:startEvent>
    <bpmn2:task id="Activity_1cfxx7x" sf:guid="284ab014-4453-4530-e378-d10c80e0a3bb" name="申请人">
      <bpmn2:incoming>Flow_0krf2of</bpmn2:incoming>
      <bpmn2:outgoing>Flow_09d6o5f</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_0krf2of" sf:guid="4dcf61d2-a7e1-46a4-81da-129624acecfa" sourceRef="StartEvent_1" targetRef="Activity_1cfxx7x" sf:from="9f83ba5e-53a3-4409-9611-42252cece8af" sf:to="284ab014-4453-4530-e378-d10c80e0a3bb" />
    <bpmn2:task id="Activity_1dz05vz" sf:guid="d03540be-5327-4c39-8303-0887caa51c6e" name="组长">
      <bpmn2:extensionElements>
        <sf:performers>
          <sf:performer name="组长" outerId="27" outerType="Role" />
        </sf:performers>
      </bpmn2:extensionElements>
      <bpmn2:incoming>Flow_0nb0wtp</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1279a93</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:task id="Activity_0lfco9x" sf:guid="0395c603-1749-49f0-c049-ba92add9cf3d" name="人事">
      <bpmn2:extensionElements>
        <sf:performers>
          <sf:performer name="考勤审批员" outerId="55" outerCode="kqspy" outerType="Role" />
        </sf:performers>
      </bpmn2:extensionElements>
      <bpmn2:incoming>Flow_0a20fu5</bpmn2:incoming>
      <bpmn2:incoming>Flow_0awy9ae</bpmn2:incoming>
      <bpmn2:outgoing>Flow_011souw</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:task id="Activity_0n8ezkz" sf:guid="d5cb869f-6530-4300-9a27-8a1fd37ea919" name="办公室负责人">
      <bpmn2:extensionElements>
        <sf:performers>
          <sf:performer name="办公室负责人" outerId="15" outerType="Role" />
        </sf:performers>
      </bpmn2:extensionElements>
      <bpmn2:incoming>Flow_011souw</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0fdfse7</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_011souw" sf:guid="be770be1-3535-45b3-98e8-2294179e303f" sourceRef="Activity_0lfco9x" targetRef="Activity_0n8ezkz" sf:from="0395c603-1749-49f0-c049-ba92add9cf3d" sf:to="d5cb869f-6530-4300-9a27-8a1fd37ea919" />
    <bpmn2:task id="Activity_1x22mw0" sf:guid="6b4ad689-cdef-42ab-f079-d25c8896fd47" name="人事">
      <bpmn2:extensionElements>
        <sf:performers>
          <sf:performer name="考勤审批员" outerId="55" outerCode="kqspy" outerType="Role" />
        </sf:performers>
      </bpmn2:extensionElements>
      <bpmn2:incoming>Flow_0fdfse7</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1cr17c4</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_0fdfse7" sf:guid="fc62105a-5cc9-486a-ff31-2c34d445e09a" sourceRef="Activity_0n8ezkz" targetRef="Activity_1x22mw0" sf:from="d5cb869f-6530-4300-9a27-8a1fd37ea919" sf:to="6b4ad689-cdef-42ab-f079-d25c8896fd47" />
    <bpmn2:endEvent id="Event_0xyj93z" sf:guid="c5338701-0bea-416f-9e7f-34a472aba032">
      <bpmn2:incoming>Flow_1cr17c4</bpmn2:incoming>
    </bpmn2:endEvent>
    <bpmn2:sequenceFlow id="Flow_1cr17c4" sf:guid="3fa4ecaa-7bfe-47c2-b541-443191ab6cf5" sourceRef="Activity_1x22mw0" targetRef="Event_0xyj93z" sf:from="6b4ad689-cdef-42ab-f079-d25c8896fd47" sf:to="c5338701-0bea-416f-9e7f-34a472aba032" />
    <bpmn2:task id="Activity_1m71pwp" sf:guid="97dceb24-bf5e-4665-9a07-12a219517ab5" name="部门负责人">
      <bpmn2:extensionElements>
        <sf:performers>
          <sf:performer name="部门负责人" outerId="17" outerType="Role" />
        </sf:performers>
      </bpmn2:extensionElements>
      <bpmn2:incoming>Flow_1279a93</bpmn2:incoming>
      <bpmn2:incoming>Flow_1ndu1t0</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0a20fu5</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_1279a93" sf:guid="60597f18-2d7e-4703-93ae-5b955b68f7bf" sourceRef="Activity_1dz05vz" targetRef="Activity_1m71pwp" sf:from="d03540be-5327-4c39-8303-0887caa51c6e" sf:to="97dceb24-bf5e-4665-9a07-12a219517ab5">
      <bpmn2:extensionElements>
        <sf:groupBehaviours recieverType="Superior" />
      </bpmn2:extensionElements>
    </bpmn2:sequenceFlow>
    <bpmn2:sequenceFlow id="Flow_09d6o5f" sf:guid="16b88900-d8ee-4f0d-a946-f33dadc7d64d" sourceRef="Activity_1cfxx7x" targetRef="Gateway_01ta8ky" sf:from="284ab014-4453-4530-e378-d10c80e0a3bb" sf:to="8f0b8f54-1664-40f3-a0e2-b8191ec58041" />
    <bpmn2:sequenceFlow id="Flow_0nb0wtp" sf:guid="1cf549d4-f6fc-4498-af30-c215cea4cd7c" sourceRef="Gateway_01ta8ky" targetRef="Activity_1dz05vz" sf:from="8f0b8f54-1664-40f3-a0e2-b8191ec58041" sf:to="b86ae64d-e120-41f4-b44f-5e68fe09a109">
      <bpmn2:extensionElements>
        <sf:groupBehaviours priority="2" recieverType="Superior" />
      </bpmn2:extensionElements>
      <bpmn2:conditionExpression>groupleader&gt;0</bpmn2:conditionExpression>
    </bpmn2:sequenceFlow>
    <bpmn2:sequenceFlow id="Flow_1ndu1t0" sf:guid="24c99cda-6492-4c79-b7fa-b98a06c15e6f" sourceRef="Gateway_01ta8ky" targetRef="Activity_1m71pwp" sf:from="8f0b8f54-1664-40f3-a0e2-b8191ec58041" sf:to="97dceb24-bf5e-4665-9a07-12a219517ab5">
      <bpmn2:extensionElements>
        <sf:groupBehaviours priority="1" recieverType="Superior" />
      </bpmn2:extensionElements>
      <bpmn2:conditionExpression>groupleader=0</bpmn2:conditionExpression>
    </bpmn2:sequenceFlow>
    <bpmn2:sequenceFlow id="Flow_0a20fu5" sf:guid="fdac1557-4098-44cb-d3af-d1e25daff454" sourceRef="Activity_1m71pwp" targetRef="Activity_0lfco9x" sf:from="97dceb24-bf5e-4665-9a07-12a219517ab5" sf:to="8bf8e461-82ee-4608-cda7-541cfc5aa6f7" />
    <bpmn2:sequenceFlow id="Flow_0awy9ae" sf:guid="37597ef2-be03-4250-ddca-5c750d9e0962" sourceRef="Gateway_01ta8ky" targetRef="Activity_0lfco9x" sf:from="8f0b8f54-1664-40f3-a0e2-b8191ec58041" sf:to="0395c603-1749-49f0-c049-ba92add9cf3d">
      <bpmn2:extensionElements>
        <sf:groupBehaviours priority="0" />
      </bpmn2:extensionElements>
      <bpmn2:conditionExpression>departmenthead&gt;0</bpmn2:conditionExpression>
    </bpmn2:sequenceFlow>
    <bpmn2:exclusiveGateway id="Gateway_01ta8ky" sf:guid="8f0b8f54-1664-40f3-a0e2-b8191ec58041">
      <bpmn2:incoming>Flow_09d6o5f</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0nb0wtp</bpmn2:outgoing>
      <bpmn2:outgoing>Flow_1ndu1t0</bpmn2:outgoing>
      <bpmn2:outgoing>Flow_0awy9ae</bpmn2:outgoing>
    </bpmn2:exclusiveGateway>
  </bpmn2:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="pProcess_Code_3815">
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="102" y="332" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1cfxx7x_di" bpmnElement="Activity_1cfxx7x">
        <dc:Bounds x="200" y="310" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1dz05vz_di" bpmnElement="Activity_1dz05vz">
        <dc:Bounds x="470" y="310" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0lfco9x_di" bpmnElement="Activity_0lfco9x">
        <dc:Bounds x="910" y="310" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0n8ezkz_di" bpmnElement="Activity_0n8ezkz">
        <dc:Bounds x="1060" y="310" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1x22mw0_di" bpmnElement="Activity_1x22mw0">
        <dc:Bounds x="1220" y="310" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_0xyj93z_di" bpmnElement="Event_0xyj93z">
        <dc:Bounds x="1382" y="332" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1m71pwp_di" bpmnElement="Activity_1m71pwp">
        <dc:Bounds x="640" y="310" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Gateway_1p9w9c0_di" bpmnElement="Gateway_01ta8ky" isMarkerVisible="true">
        <dc:Bounds x="325" y="325" width="50" height="50" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNEdge id="Flow_0krf2of_di" bpmnElement="Flow_0krf2of">
        <di:waypoint x="138" y="350" />
        <di:waypoint x="200" y="350" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_011souw_di" bpmnElement="Flow_011souw">
        <di:waypoint x="1010" y="350" />
        <di:waypoint x="1060" y="350" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0fdfse7_di" bpmnElement="Flow_0fdfse7">
        <di:waypoint x="1160" y="350" />
        <di:waypoint x="1220" y="350" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1cr17c4_di" bpmnElement="Flow_1cr17c4">
        <di:waypoint x="1320" y="350" />
        <di:waypoint x="1382" y="350" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1279a93_di" bpmnElement="Flow_1279a93">
        <di:waypoint x="570" y="350" />
        <di:waypoint x="640" y="350" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_09d6o5f_di" bpmnElement="Flow_09d6o5f">
        <di:waypoint x="300" y="350" />
        <di:waypoint x="325" y="350" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0nb0wtp_di" bpmnElement="Flow_0nb0wtp">
        <di:waypoint x="375" y="350" />
        <di:waypoint x="470" y="350" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1ndu1t0_di" bpmnElement="Flow_1ndu1t0">
        <di:waypoint x="350" y="375" />
        <di:waypoint x="350" y="420" />
        <di:waypoint x="690" y="420" />
        <di:waypoint x="690" y="390" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="504" y="422" width="32" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0a20fu5_di" bpmnElement="Flow_0a20fu5">
        <di:waypoint x="740" y="350" />
        <di:waypoint x="910" y="350" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0awy9ae_di" bpmnElement="Flow_0awy9ae">
        <di:waypoint x="350" y="325" />
        <di:waypoint x="350" y="270" />
        <di:waypoint x="960" y="270" />
        <di:waypoint x="960" y="310" />
      </bpmndi:BPMNEdge>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn2:definitions>
', 0, NULL, NULL, 0, NULL, CAST(0x0000B0810100B64D AS DateTime), NULL)
INSERT [dbo].[WfProcess] ([ID], [ProcessGUID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1245, N'cfd46f46-14a5-437f-811e-b63b20fd30c5', N'1', N'调休申请', N'askforleave_txsq', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn2:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL" id="bpmn2-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn2:process id="pProcess_Code_4524" sf:guid="cfd46f46-14a5-437f-811e-b63b20fd30c5" sf:code="askforleave_txsq" name="调休申请" isExecutable="true" sf:version="1">
    <bpmn2:startEvent id="StartEvent_1" sf:guid="ac626163-d938-4ac8-aa23-2742c2147ffd" name="Start">
      <bpmn2:outgoing>Flow_1lta11r</bpmn2:outgoing>
    </bpmn2:startEvent>
    <bpmn2:task id="Activity_1kl58aq" sf:guid="c3010294-0aef-4105-fa14-dc27f54218b1" name="申请人">
      <bpmn2:incoming>Flow_1lta11r</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0c8kax8</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_1lta11r" sourceRef="StartEvent_1" targetRef="Activity_1kl58aq" />
    <bpmn2:exclusiveGateway id="Gateway_1ypa96a" sf:guid="c1d721e7-0d42-433e-8b8f-49f40efb7913">
      <bpmn2:incoming>Flow_0c8kax8</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0g208dt</bpmn2:outgoing>
      <bpmn2:outgoing>Flow_0h0qkyn</bpmn2:outgoing>
      <bpmn2:outgoing>Flow_1gnhu4k</bpmn2:outgoing>
    </bpmn2:exclusiveGateway>
    <bpmn2:sequenceFlow id="Flow_0c8kax8" sourceRef="Activity_1kl58aq" targetRef="Gateway_1ypa96a" />
    <bpmn2:task id="Activity_11qd62g" sf:guid="26a0525e-4920-4090-8ce3-a20a7fe264d7" name="组长/副职">
      <bpmn2:extensionElements>
        <sf:performers>
          <sf:performer name="组长" outerId="27" outerCode="ZuZhang" outerType="Role" />
        </sf:performers>
      </bpmn2:extensionElements>
      <bpmn2:incoming>Flow_0g208dt</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1fz9loo</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_0g208dt" sourceRef="Gateway_1ypa96a" targetRef="Activity_11qd62g">
      <bpmn2:extensionElements>
        <sf:groupBehaviours priority="2" recieverType="Superior" />
      </bpmn2:extensionElements>
      <bpmn2:conditionExpression>groupleader&gt;0</bpmn2:conditionExpression>
    </bpmn2:sequenceFlow>
    <bpmn2:task id="Activity_0tnhpo3" sf:guid="914309c7-1455-492b-8f6e-860a8c9fbf03" name="部门负责人">
      <bpmn2:extensionElements>
        <sf:performers>
          <sf:performer name="部门负责人" outerId="17" outerCode="BuMenFuZeRen" outerType="Role" />
        </sf:performers>
      </bpmn2:extensionElements>
      <bpmn2:incoming>Flow_1fz9loo</bpmn2:incoming>
      <bpmn2:incoming>Flow_1gnhu4k</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1x850o9</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_1fz9loo" sourceRef="Activity_11qd62g" targetRef="Activity_0tnhpo3">
      <bpmn2:extensionElements>
        <sf:groupBehaviours recieverType="Superior" />
      </bpmn2:extensionElements>
    </bpmn2:sequenceFlow>
    <bpmn2:task id="Activity_0hwr5l3" sf:guid="edb55048-3c26-4b5c-9125-5f3898f72471" name="人事">
      <bpmn2:extensionElements>
        <sf:performers>
          <sf:performer name="考勤审批员" outerId="55" outerCode="KaoQinShenPiYuan" outerType="Role" />
        </sf:performers>
      </bpmn2:extensionElements>
      <bpmn2:incoming>Flow_1x850o9</bpmn2:incoming>
      <bpmn2:incoming>Flow_0h0qkyn</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0mntf4j</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_1x850o9" sourceRef="Activity_0tnhpo3" targetRef="Activity_0hwr5l3" />
    <bpmn2:task id="Activity_09kupv4" sf:guid="292f357f-3f98-47c1-960c-2f0a247e0e33" name="办公室负责人">
      <bpmn2:extensionElements>
        <sf:performers>
          <sf:performer name="办公室负责人" outerId="15" outerCode="BanGongShiFuZeRen" outerType="Role" />
        </sf:performers>
      </bpmn2:extensionElements>
      <bpmn2:incoming>Flow_0mntf4j</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0g2b6wc</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_0mntf4j" sourceRef="Activity_0hwr5l3" targetRef="Activity_09kupv4" />
    <bpmn2:task id="Activity_0dwo0xn" sf:guid="b7393338-20ff-49aa-8e9d-eebd9f23f902" name="人事归档">
      <bpmn2:extensionElements>
        <sf:performers>
          <sf:performer name="考勤审批员" outerId="55" outerCode="KaoQinShenPiYuan" outerType="Role" />
        </sf:performers>
      </bpmn2:extensionElements>
      <bpmn2:incoming>Flow_0g2b6wc</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1nkgnl4</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_0g2b6wc" sourceRef="Activity_09kupv4" targetRef="Activity_0dwo0xn" />
    <bpmn2:endEvent id="Event_1wl1wsj" sf:guid="811710cb-c35b-45dd-d007-18ad0b1c0211">
      <bpmn2:incoming>Flow_1nkgnl4</bpmn2:incoming>
    </bpmn2:endEvent>
    <bpmn2:sequenceFlow id="Flow_1nkgnl4" sourceRef="Activity_0dwo0xn" targetRef="Event_1wl1wsj" />
    <bpmn2:sequenceFlow id="Flow_0h0qkyn" sourceRef="Gateway_1ypa96a" targetRef="Activity_0hwr5l3">
      <bpmn2:extensionElements>
        <sf:groupBehaviours priority="0" />
      </bpmn2:extensionElements>
      <bpmn2:conditionExpression>departmenthead&gt;0</bpmn2:conditionExpression>
    </bpmn2:sequenceFlow>
    <bpmn2:sequenceFlow id="Flow_1gnhu4k" sourceRef="Gateway_1ypa96a" targetRef="Activity_0tnhpo3">
      <bpmn2:extensionElements>
        <sf:groupBehaviours priority="1" recieverType="Superior" />
      </bpmn2:extensionElements>
      <bpmn2:conditionExpression>groupleader=0</bpmn2:conditionExpression>
    </bpmn2:sequenceFlow>
  </bpmn2:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="pProcess_Code_4524">
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="132" y="362" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="138" y="405" width="25" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1kl58aq_di" bpmnElement="Activity_1kl58aq">
        <dc:Bounds x="220" y="340" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Gateway_1ypa96a_di" bpmnElement="Gateway_1ypa96a" isMarkerVisible="true">
        <dc:Bounds x="375" y="355" width="50" height="50" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_11qd62g_di" bpmnElement="Activity_11qd62g">
        <dc:Bounds x="480" y="340" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0tnhpo3_di" bpmnElement="Activity_0tnhpo3">
        <dc:Bounds x="640" y="340" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0hwr5l3_di" bpmnElement="Activity_0hwr5l3">
        <dc:Bounds x="800" y="340" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_09kupv4_di" bpmnElement="Activity_09kupv4">
        <dc:Bounds x="960" y="340" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0dwo0xn_di" bpmnElement="Activity_0dwo0xn">
        <dc:Bounds x="1120" y="340" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_1wl1wsj_di" bpmnElement="Event_1wl1wsj">
        <dc:Bounds x="1282" y="362" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNEdge id="Flow_1lta11r_di" bpmnElement="Flow_1lta11r">
        <di:waypoint x="168" y="380" />
        <di:waypoint x="220" y="380" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0c8kax8_di" bpmnElement="Flow_0c8kax8">
        <di:waypoint x="320" y="380" />
        <di:waypoint x="375" y="380" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0g208dt_di" bpmnElement="Flow_0g208dt">
        <di:waypoint x="425" y="380" />
        <di:waypoint x="480" y="380" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1fz9loo_di" bpmnElement="Flow_1fz9loo">
        <di:waypoint x="580" y="380" />
        <di:waypoint x="640" y="380" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1x850o9_di" bpmnElement="Flow_1x850o9">
        <di:waypoint x="740" y="380" />
        <di:waypoint x="800" y="380" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0mntf4j_di" bpmnElement="Flow_0mntf4j">
        <di:waypoint x="900" y="380" />
        <di:waypoint x="960" y="380" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0g2b6wc_di" bpmnElement="Flow_0g2b6wc">
        <di:waypoint x="1060" y="380" />
        <di:waypoint x="1120" y="380" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1nkgnl4_di" bpmnElement="Flow_1nkgnl4">
        <di:waypoint x="1220" y="380" />
        <di:waypoint x="1282" y="380" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0h0qkyn_di" bpmnElement="Flow_0h0qkyn">
        <di:waypoint x="400" y="355" />
        <di:waypoint x="400" y="320" />
        <di:waypoint x="850" y="320" />
        <di:waypoint x="850" y="340" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1gnhu4k_di" bpmnElement="Flow_1gnhu4k">
        <di:waypoint x="400" y="405" />
        <di:waypoint x="400" y="450" />
        <di:waypoint x="690" y="450" />
        <di:waypoint x="690" y="420" />
      </bpmndi:BPMNEdge>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn2:definitions>
', 0, NULL, NULL, 0, NULL, CAST(0x0000B08200C26869 AS DateTime), NULL)
INSERT [dbo].[WfProcess] ([ID], [ProcessGUID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1317, N'Change-0d34749c-0934-46b4-a890-ecc7fabd0b31', N'1', N'流程_变更出库', N'Process_CK_Change', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn2:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL" id="bpmn2-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn2:process id="pProcess_Code_7437" sf:guid="Change-0d34749c-0934-46b4-a890-ecc7fabd0b31" sf:code="Process_CK_Change" name="流程_变更出库" isExecutable="true" sf:version="1">
    <bpmn2:startEvent id="StartEvent_1" sf:guid="dcb9c7ae-da56-4c8d-8882-5ceb72eaa5cf" name="Start">
      <bpmn2:outgoing>Flow_1qisib8</bpmn2:outgoing>
    </bpmn2:startEvent>
    <bpmn2:task id="Activity_1o7zovc" sf:guid="64852abd-a2cc-432e-f39c-93bc5ea04785" name="提交申请">
      <bpmn2:incoming>Flow_1qisib8</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0al21wp</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_1qisib8" sf:guid="33a256d9-67eb-4577-f177-dc2ae331a77c" sourceRef="StartEvent_1" targetRef="Activity_1o7zovc" sf:from="dcb9c7ae-da56-4c8d-8882-5ceb72eaa5cf" sf:to="64852abd-a2cc-432e-f39c-93bc5ea04785" />
    <bpmn2:task id="Activity_0k3tiyg" sf:guid="1aed11ef-a005-4db6-bdb2-4927a98c4682" name="项目经理审批">
      <bpmn2:extensionElements>
        <sf:performers>
          <sf:performer name="项目经理" outerId="3" outerCode="3" outerType="Role" />
        </sf:performers>
      </bpmn2:extensionElements>
      <bpmn2:incoming>Flow_0al21wp</bpmn2:incoming>
      <bpmn2:outgoing>Flow_06z9936</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_0al21wp" sf:guid="d687b533-be76-4d5d-88d9-86703c4289ec" sourceRef="Activity_1o7zovc" targetRef="Activity_0k3tiyg" sf:from="64852abd-a2cc-432e-f39c-93bc5ea04785" sf:to="1aed11ef-a005-4db6-bdb2-4927a98c4682" />
    <bpmn2:task id="Activity_0fa04py" sf:guid="4aed0055-2403-4be0-c74a-596501892c26" name="CCB审批">
      <bpmn2:extensionElements>
        <sf:performers>
          <sf:performer name="CCB" outerId="1" outerCode="1" outerType="Role" />
        </sf:performers>
      </bpmn2:extensionElements>
      <bpmn2:incoming>Flow_06z9936</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1xdai8b</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_06z9936" sf:guid="32613d68-1e97-4bcd-f2e3-b4a0c8434e20" sourceRef="Activity_0k3tiyg" targetRef="Activity_0fa04py" sf:from="1aed11ef-a005-4db6-bdb2-4927a98c4682" sf:to="4aed0055-2403-4be0-c74a-596501892c26" />
    <bpmn2:task id="Activity_12o8616" sf:guid="74ee8c9c-e4af-4c40-db70-65391f3a6552" name="配置项管理员出库">
      <bpmn2:extensionElements>
        <sf:performers>
          <sf:performer name="配置管理员" outerId="4" outerCode="4" outerType="Role" />
        </sf:performers>
      </bpmn2:extensionElements>
      <bpmn2:incoming>Flow_1xdai8b</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0x5txlc</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_1xdai8b" sf:guid="160ddbcb-facf-4fc9-f1ce-f38bf097b496" sourceRef="Activity_0fa04py" targetRef="Activity_12o8616" sf:from="4aed0055-2403-4be0-c74a-596501892c26" sf:to="74ee8c9c-e4af-4c40-db70-65391f3a6552" />
    <bpmn2:endEvent id="Event_05gkrkl" sf:guid="7d81824c-94e1-485e-93f7-ded41a8a725a" name="End">
      <bpmn2:incoming>Flow_0x5txlc</bpmn2:incoming>
    </bpmn2:endEvent>
    <bpmn2:sequenceFlow id="Flow_0x5txlc" sf:guid="3e6b4b74-8dfe-4833-bb87-248e6448b793" sourceRef="Activity_12o8616" targetRef="Event_05gkrkl" sf:from="74ee8c9c-e4af-4c40-db70-65391f3a6552" sf:to="7d81824c-94e1-485e-93f7-ded41a8a725a" />
  </bpmn2:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="pProcess_Code_7437">
      <bpmndi:BPMNEdge id="Flow_1qisib8_di" bpmnElement="Flow_1qisib8">
        <di:waypoint x="448" y="258" />
        <di:waypoint x="500" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0al21wp_di" bpmnElement="Flow_0al21wp">
        <di:waypoint x="600" y="258" />
        <di:waypoint x="660" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_06z9936_di" bpmnElement="Flow_06z9936">
        <di:waypoint x="760" y="258" />
        <di:waypoint x="820" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1xdai8b_di" bpmnElement="Flow_1xdai8b">
        <di:waypoint x="920" y="258" />
        <di:waypoint x="980" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0x5txlc_di" bpmnElement="Flow_0x5txlc">
        <di:waypoint x="1080" y="258" />
        <di:waypoint x="1142" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="412" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="418" y="283" width="25" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1o7zovc_di" bpmnElement="Activity_1o7zovc">
        <dc:Bounds x="500" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0k3tiyg_di" bpmnElement="Activity_0k3tiyg">
        <dc:Bounds x="660" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0fa04py_di" bpmnElement="Activity_0fa04py">
        <dc:Bounds x="820" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_12o8616_di" bpmnElement="Activity_12o8616">
        <dc:Bounds x="980" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_05gkrkl_di" bpmnElement="Event_05gkrkl">
        <dc:Bounds x="1142" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="1150" y="283" width="20" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn2:definitions>
', 0, NULL, NULL, 0, NULL, CAST(0x0000B0B40101B0BC AS DateTime), NULL)
INSERT [dbo].[WfProcess] ([ID], [ProcessGUID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1362, N'8c368e73-e5bd-47e4-9891-4e276b03c0ad', N'1', N'Process_Name_2170', N'Process_Code_2170', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn2:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL" id="bpmn2-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn2:process id="pProcess_Code_2170" sf:guid="8c368e73-e5bd-47e4-9891-4e276b03c0ad" sf:code="Process_Code_2170" name="Process_Name_2170" isExecutable="true" sf:version="1">
    <bpmn2:startEvent id="StartEvent_1" sf:guid="9eb713b0-36db-4e70-bb1a-db3aeaf8e474" name="Start">
      <bpmn2:outgoing>Flow_0n2ze3a</bpmn2:outgoing>
      <bpmn2:conditionalEventDefinition id="ConditionalEventDefinition_1ts7p9t">
        <bpmn2:condition xsi:type="bpmn2:tFormalExpression">a&gt;5</bpmn2:condition>
      </bpmn2:conditionalEventDefinition>
    </bpmn2:startEvent>
    <bpmn2:task id="Activity_04g3fr5" sf:guid="6f12b165-bc97-4b37-c86e-b47da1a4c163" name="task1">
      <bpmn2:incoming>Flow_0n2ze3a</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0vnhowp</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_0n2ze3a" sf:guid="89c5fb48-78a7-40bc-9f67-bb3b10a0ee46" sourceRef="StartEvent_1" targetRef="Activity_04g3fr5" sf:from="9eb713b0-36db-4e70-bb1a-db3aeaf8e474" sf:to="6f12b165-bc97-4b37-c86e-b47da1a4c163">
      <bpmn2:conditionExpression />
    </bpmn2:sequenceFlow>
    <bpmn2:task id="Activity_03ki979" sf:guid="815c9753-c81d-448e-8fb4-e4e1828cf9f5" name="task2">
      <bpmn2:incoming>Flow_0vnhowp</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0y291y5</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_0vnhowp" sf:guid="7f7cfc4b-c8c3-4d43-e0d7-1d48fca5b786" sourceRef="Activity_04g3fr5" targetRef="Activity_03ki979" sf:from="6f12b165-bc97-4b37-c86e-b47da1a4c163" sf:to="815c9753-c81d-448e-8fb4-e4e1828cf9f5" />
    <bpmn2:endEvent id="Event_0zc14a5" sf:guid="0e30f260-c810-4a38-8332-4d8e3bc493e6" name="End">
      <bpmn2:incoming>Flow_0y291y5</bpmn2:incoming>
    </bpmn2:endEvent>
    <bpmn2:sequenceFlow id="Flow_0y291y5" sf:guid="e8f2d11e-dcaf-4ae7-9f67-99435bcc0109" sourceRef="Activity_03ki979" targetRef="Event_0zc14a5" sf:from="815c9753-c81d-448e-8fb4-e4e1828cf9f5" sf:to="0e30f260-c810-4a38-8332-4d8e3bc493e6" />
  </bpmn2:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="pProcess_Code_2170">
      <bpmndi:BPMNEdge id="Flow_0y291y5_di" bpmnElement="Flow_0y291y5">
        <di:waypoint x="760" y="258" />
        <di:waypoint x="822" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0vnhowp_di" bpmnElement="Flow_0vnhowp">
        <di:waypoint x="600" y="258" />
        <di:waypoint x="660" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0n2ze3a_di" bpmnElement="Flow_0n2ze3a">
        <di:waypoint x="448" y="258" />
        <di:waypoint x="500" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNShape id="Event_152tec8_di" bpmnElement="StartEvent_1">
        <dc:Bounds x="412" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="418" y="283" width="24" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_04g3fr5_di" bpmnElement="Activity_04g3fr5">
        <dc:Bounds x="500" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_03ki979_di" bpmnElement="Activity_03ki979">
        <dc:Bounds x="660" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_0zc14a5_di" bpmnElement="Event_0zc14a5">
        <dc:Bounds x="822" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="831" y="283" width="19" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn2:definitions>
', 0, NULL, NULL, 0, NULL, CAST(0x0000B0B500A131AF AS DateTime), CAST(0x0000B0BC010D8437 AS DateTime))
INSERT [dbo].[WfProcess] ([ID], [ProcessGUID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1363, N'e70d94f1-97fa-4dce-b23d-1fafc5cf12de', N'1', N'Process_Name_1931', N'Process_Code_1931', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn2:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL" id="bpmn2-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn2:process id="pProcess_Code_1931" sf:guid="e70d94f1-97fa-4dce-b23d-1fafc5cf12de" sf:code="Process_Code_1931" name="Process_Name_1931" isExecutable="true" sf:version="1">
    <bpmn2:startEvent id="StartEvent_1" sf:guid="a04bc77b-bc05-4a7e-89ec-4de5278f0cfe" name="Start">
      <bpmn2:outgoing>Flow_02iqr3u</bpmn2:outgoing>
    </bpmn2:startEvent>
    <bpmn2:task id="Activity_041a7uk" sf:guid="a10aa2eb-01fb-40c3-b797-90d24d9b7471">
      <bpmn2:incoming>Flow_02iqr3u</bpmn2:incoming>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_02iqr3u" sf:guid="2a978769-8424-485a-e69e-3b677ee536f2" sourceRef="StartEvent_1" targetRef="Activity_041a7uk" sf:from="a04bc77b-bc05-4a7e-89ec-4de5278f0cfe" sf:to="a10aa2eb-01fb-40c3-b797-90d24d9b7471" />
    <bpmn2:endEvent id="Event_0humg7q" sf:guid="7681a861-623a-4673-b20c-e21b0d119323" />
  </bpmn2:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="pProcess_Code_1931">
      <bpmndi:BPMNEdge id="Flow_02iqr3u_di" bpmnElement="Flow_02iqr3u">
        <di:waypoint x="448" y="258" />
        <di:waypoint x="590" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="412" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="418" y="283" width="24" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_041a7uk_di" bpmnElement="Activity_041a7uk">
        <dc:Bounds x="590" y="218" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_0humg7q_di" bpmnElement="Event_0humg7q">
        <dc:Bounds x="1122" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn2:definitions>
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        ', 0, NULL, NULL, 0, NULL, CAST(0x0000B0BC014D9A7A AS DateTime), CAST(0x0000B0C600D43FA3 AS DateTime))
INSERT [dbo].[WfProcess] ([ID], [ProcessGUID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1364, N'0e30ef92-64bc-47b8-959b-c3e37de221e9', N'1', N'会签合并测试', N'Process_Code_2522', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn2:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL" id="bpmn2-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn2:process id="pProcess_Code_2522" sf:guid="0e30ef92-64bc-47b8-959b-c3e37de221e9" sf:code="Process_Code_2522" name="会签合并测试" isExecutable="true" sf:version="1">
    <bpmn2:startEvent id="StartEvent_1" sf:guid="2205a31a-a850-410c-94b6-775bd1d6b278" name="Start">
      <bpmn2:outgoing>Flow_0lfs4bv</bpmn2:outgoing>
    </bpmn2:startEvent>
    <bpmn2:task id="Activity_1vn8pdc" sf:guid="ca5249c0-a8fc-4299-8165-359e5fb92f9b" name="Task-01">
      <bpmn2:incoming>Flow_0lfs4bv</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0rxpy9b</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_0lfs4bv" sf:guid="c0cc9ca2-a516-4bf0-ac1f-5c7f2e85c4c0" sourceRef="StartEvent_1" targetRef="Activity_1vn8pdc" sf:from="2205a31a-a850-410c-94b6-775bd1d6b278" sf:to="ca5249c0-a8fc-4299-8165-359e5fb92f9b" />
    <bpmn2:task id="Activity_0hss79q" sf:guid="aaf0ffd7-3b4f-431d-ce52-411bdbe48890" name="Task-02">
      <bpmn2:extensionElements>
        <sf:multiSignDetail complexType="SignTogether" mergeType="Parallel" compareType="Count" completeOrder="2" />
      </bpmn2:extensionElements>
      <bpmn2:incoming>Flow_0rxpy9b</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1lwxdno</bpmn2:outgoing>
      <bpmn2:multiInstanceLoopCharacteristics isSequential="true" />
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_0rxpy9b" sf:guid="5be11d2b-1932-41f4-d151-ba406e44e323" sourceRef="Activity_1vn8pdc" targetRef="Activity_0hss79q" sf:from="ca5249c0-a8fc-4299-8165-359e5fb92f9b" sf:to="aaf0ffd7-3b4f-431d-ce52-411bdbe48890" />
    <bpmn2:sequenceFlow id="Flow_1lwxdno" sf:guid="29df7eaf-891c-4b54-9716-9af342839cb9" sourceRef="Activity_0hss79q" targetRef="Gateway_0mv3z8q" sf:from="aaf0ffd7-3b4f-431d-ce52-411bdbe48890" sf:to="728f6ed2-95ed-425d-c521-c9c8a84a4f7d" />
    <bpmn2:endEvent id="Event_1j1js28" sf:guid="4fc6c7b2-807c-4819-c825-40224aa69afa">
      <bpmn2:incoming>Flow_0y1dz4t</bpmn2:incoming>
      <bpmn2:incoming>Flow_11gfchj</bpmn2:incoming>
    </bpmn2:endEvent>
    <bpmn2:sequenceFlow id="Flow_0y1dz4t" sf:guid="e3f95bba-9e59-4ba0-c937-853a6be52d9c" sourceRef="Gateway_1k49b42" targetRef="Event_1j1js28" sf:from="36266a43-0fca-461b-e2eb-0c80c731b1b7" sf:to="4fc6c7b2-807c-4819-c825-40224aa69afa" />
    <bpmn2:task id="Activity_056w0go" sf:guid="86a9080e-2eba-4683-a6f1-5d84b30388ee" name="Task-03">
      <bpmn2:incoming>Flow_0frpq2j</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1l5jqd4</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_0frpq2j" sf:guid="3c475b1c-15be-4c61-af9b-6e5bb8390e32" sourceRef="Gateway_0mv3z8q" targetRef="Activity_056w0go" sf:from="728f6ed2-95ed-425d-c521-c9c8a84a4f7d" sf:to="86a9080e-2eba-4683-a6f1-5d84b30388ee" />
    <bpmn2:task id="Activity_0zy9snq" sf:guid="1a5a0a58-99a2-4c91-95df-f9aa2e8e81c1" name="Task-04">
      <bpmn2:incoming>Flow_1ka6fx7</bpmn2:incoming>
      <bpmn2:outgoing>Flow_010jvgy</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_1ka6fx7" sf:guid="2f030fb9-51bd-4353-c0e2-9014de93f130" sourceRef="Gateway_0mv3z8q" targetRef="Activity_0zy9snq" sf:from="728f6ed2-95ed-425d-c521-c9c8a84a4f7d" sf:to="1a5a0a58-99a2-4c91-95df-f9aa2e8e81c1" />
    <bpmn2:sequenceFlow id="Flow_1l5jqd4" sf:guid="2c56a8e3-0f13-40dd-f55e-470ac5741c84" sourceRef="Activity_056w0go" targetRef="Gateway_1k49b42" sf:from="86a9080e-2eba-4683-a6f1-5d84b30388ee" sf:to="36266a43-0fca-461b-e2eb-0c80c731b1b7" />
    <bpmn2:sequenceFlow id="Flow_010jvgy" sf:guid="9ec58ffa-994e-4f03-9d62-a3f30d0f72b7" sourceRef="Activity_0zy9snq" targetRef="Gateway_1k49b42" sf:from="1a5a0a58-99a2-4c91-95df-f9aa2e8e81c1" sf:to="36266a43-0fca-461b-e2eb-0c80c731b1b7" />
    <bpmn2:parallelGateway id="Gateway_0mv3z8q" sf:guid="728f6ed2-95ed-425d-c521-c9c8a84a4f7d">
      <bpmn2:incoming>Flow_1lwxdno</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0frpq2j</bpmn2:outgoing>
      <bpmn2:outgoing>Flow_1ka6fx7</bpmn2:outgoing>
      <bpmn2:outgoing>Flow_11gfchj</bpmn2:outgoing>
    </bpmn2:parallelGateway>
    <bpmn2:parallelGateway id="Gateway_1k49b42" sf:guid="36266a43-0fca-461b-e2eb-0c80c731b1b7">
      <bpmn2:incoming>Flow_1l5jqd4</bpmn2:incoming>
      <bpmn2:incoming>Flow_010jvgy</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0y1dz4t</bpmn2:outgoing>
    </bpmn2:parallelGateway>
    <bpmn2:sequenceFlow id="Flow_11gfchj" sf:guid="ff888ad5-d385-4540-f7f4-a5daf5d7715d" sourceRef="Gateway_0mv3z8q" targetRef="Event_1j1js28" sf:from="728f6ed2-95ed-425d-c521-c9c8a84a4f7d" sf:to="4fc6c7b2-807c-4819-c825-40224aa69afa" />
  </bpmn2:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="pProcess_Code_2522">
      <bpmndi:BPMNEdge id="Flow_010jvgy_di" bpmnElement="Flow_010jvgy">
        <di:waypoint x="1040" y="370" />
        <di:waypoint x="1130" y="370" />
        <di:waypoint x="1130" y="283" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1l5jqd4_di" bpmnElement="Flow_1l5jqd4">
        <di:waypoint x="1040" y="170" />
        <di:waypoint x="1130" y="170" />
        <di:waypoint x="1130" y="233" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1ka6fx7_di" bpmnElement="Flow_1ka6fx7">
        <di:waypoint x="850" y="283" />
        <di:waypoint x="850" y="370" />
        <di:waypoint x="940" y="370" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0frpq2j_di" bpmnElement="Flow_0frpq2j">
        <di:waypoint x="850" y="233" />
        <di:waypoint x="850" y="170" />
        <di:waypoint x="940" y="170" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0y1dz4t_di" bpmnElement="Flow_0y1dz4t">
        <di:waypoint x="1155" y="258" />
        <di:waypoint x="1242" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1lwxdno_di" bpmnElement="Flow_1lwxdno">
        <di:waypoint x="760" y="258" />
        <di:waypoint x="825" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0rxpy9b_di" bpmnElement="Flow_0rxpy9b">
        <di:waypoint x="600" y="258" />
        <di:waypoint x="660" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0lfs4bv_di" bpmnElement="Flow_0lfs4bv">
        <di:waypoint x="448" y="258" />
        <di:waypoint x="500" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_11gfchj_di" bpmnElement="Flow_11gfchj">
        <di:waypoint x="850" y="283" />
        <di:waypoint x="850" y="520" />
        <di:waypoint x="1260" y="520" />
        <di:waypoint x="1260" y="276" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="412" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="418" y="283" width="25" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1vn8pdc_di" bpmnElement="Activity_1vn8pdc">
        <dc:Bounds x="500" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0hss79q_di" bpmnElement="Activity_0hss79q">
        <dc:Bounds x="660" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_1j1js28_di" bpmnElement="Event_1j1js28">
        <dc:Bounds x="1242" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_056w0go_di" bpmnElement="Activity_056w0go">
        <dc:Bounds x="940" y="130" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0zy9snq_di" bpmnElement="Activity_0zy9snq">
        <dc:Bounds x="940" y="330" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Gateway_0zh6kl5_di" bpmnElement="Gateway_0mv3z8q">
        <dc:Bounds x="825" y="233" width="50" height="50" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Gateway_12pdwyc_di" bpmnElement="Gateway_1k49b42">
        <dc:Bounds x="1105" y="233" width="50" height="50" />
      </bpmndi:BPMNShape>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn2:definitions>
', 0, NULL, NULL, 0, NULL, CAST(0x0000B0D3010310AA AS DateTime), CAST(0x0000B0D301146D57 AS DateTime))
INSERT [dbo].[WfProcess] ([ID], [ProcessGUID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1365, N'0e30ef92-64bc-47b8-959b-c3e37de221e9', N'2', N'会签合并测试', N'Process_Code_2522', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn2:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL" id="bpmn2-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn2:process id="pProcess_Code_2522" sf:guid="0e30ef92-64bc-47b8-959b-c3e37de221e9" sf:code="Process_Code_2522" name="会签合并测试" isExecutable="true" sf:version="1">
    <bpmn2:startEvent id="StartEvent_1" sf:guid="2205a31a-a850-410c-94b6-775bd1d6b278" name="Start">
      <bpmn2:outgoing>Flow_0lfs4bv</bpmn2:outgoing>
    </bpmn2:startEvent>
    <bpmn2:task id="Activity_1vn8pdc" sf:guid="ca5249c0-a8fc-4299-8165-359e5fb92f9b" name="Task-01">
      <bpmn2:incoming>Flow_0lfs4bv</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0rxpy9b</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_0lfs4bv" sf:guid="c0cc9ca2-a516-4bf0-ac1f-5c7f2e85c4c0" sourceRef="StartEvent_1" targetRef="Activity_1vn8pdc" sf:from="2205a31a-a850-410c-94b6-775bd1d6b278" sf:to="ca5249c0-a8fc-4299-8165-359e5fb92f9b" />
    <bpmn2:task id="Activity_0hss79q" sf:guid="aaf0ffd7-3b4f-431d-ce52-411bdbe48890" name="Task-02">
      <bpmn2:extensionElements>
        <sf:multiSignDetail complexType="SignTogether" mergeType="Parallel" compareType="Count" completeOrder="2" />
      </bpmn2:extensionElements>
      <bpmn2:incoming>Flow_0rxpy9b</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1lwxdno</bpmn2:outgoing>
      <bpmn2:multiInstanceLoopCharacteristics isSequential="true" />
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_0rxpy9b" sf:guid="5be11d2b-1932-41f4-d151-ba406e44e323" sourceRef="Activity_1vn8pdc" targetRef="Activity_0hss79q" sf:from="ca5249c0-a8fc-4299-8165-359e5fb92f9b" sf:to="aaf0ffd7-3b4f-431d-ce52-411bdbe48890" />
    <bpmn2:sequenceFlow id="Flow_1lwxdno" sf:guid="29df7eaf-891c-4b54-9716-9af342839cb9" sourceRef="Activity_0hss79q" targetRef="Gateway_0mv3z8q" sf:from="aaf0ffd7-3b4f-431d-ce52-411bdbe48890" sf:to="728f6ed2-95ed-425d-c521-c9c8a84a4f7d" />
    <bpmn2:endEvent id="Event_1j1js28" sf:guid="4fc6c7b2-807c-4819-c825-40224aa69afa">
      <bpmn2:incoming>Flow_0y1dz4t</bpmn2:incoming>
    </bpmn2:endEvent>
    <bpmn2:sequenceFlow id="Flow_0y1dz4t" sf:guid="e3f95bba-9e59-4ba0-c937-853a6be52d9c" sourceRef="Gateway_1k49b42" targetRef="Event_1j1js28" sf:from="36266a43-0fca-461b-e2eb-0c80c731b1b7" sf:to="4fc6c7b2-807c-4819-c825-40224aa69afa" />
    <bpmn2:task id="Activity_056w0go" sf:guid="86a9080e-2eba-4683-a6f1-5d84b30388ee" name="Task-03">
      <bpmn2:extensionElements>
        <sf:multiSignDetail complexType="SignTogether" mergeType="Sequence" compareType="Count" completeOrder="2" />
      </bpmn2:extensionElements>
      <bpmn2:incoming>Flow_0frpq2j</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1l5jqd4</bpmn2:outgoing>
      <bpmn2:multiInstanceLoopCharacteristics isSequential="true" />
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_0frpq2j" sf:guid="3c475b1c-15be-4c61-af9b-6e5bb8390e32" sourceRef="Gateway_0mv3z8q" targetRef="Activity_056w0go" sf:from="728f6ed2-95ed-425d-c521-c9c8a84a4f7d" sf:to="86a9080e-2eba-4683-a6f1-5d84b30388ee" />
    <bpmn2:task id="Activity_0zy9snq" sf:guid="1a5a0a58-99a2-4c91-95df-f9aa2e8e81c1" name="Task-04">
      <bpmn2:extensionElements>
        <sf:multiSignDetail complexType="SignTogether" mergeType="Parallel" compareType="Count" completeOrder="2" />
      </bpmn2:extensionElements>
      <bpmn2:incoming>Flow_1ka6fx7</bpmn2:incoming>
      <bpmn2:outgoing>Flow_010jvgy</bpmn2:outgoing>
      <bpmn2:multiInstanceLoopCharacteristics />
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_1ka6fx7" sf:guid="2f030fb9-51bd-4353-c0e2-9014de93f130" sourceRef="Gateway_0mv3z8q" targetRef="Activity_0zy9snq" sf:from="728f6ed2-95ed-425d-c521-c9c8a84a4f7d" sf:to="1a5a0a58-99a2-4c91-95df-f9aa2e8e81c1" />
    <bpmn2:sequenceFlow id="Flow_1l5jqd4" sf:guid="2c56a8e3-0f13-40dd-f55e-470ac5741c84" sourceRef="Activity_056w0go" targetRef="Gateway_1k49b42" sf:from="86a9080e-2eba-4683-a6f1-5d84b30388ee" sf:to="36266a43-0fca-461b-e2eb-0c80c731b1b7" />
    <bpmn2:sequenceFlow id="Flow_010jvgy" sf:guid="9ec58ffa-994e-4f03-9d62-a3f30d0f72b7" sourceRef="Activity_0zy9snq" targetRef="Gateway_1k49b42" sf:from="1a5a0a58-99a2-4c91-95df-f9aa2e8e81c1" sf:to="36266a43-0fca-461b-e2eb-0c80c731b1b7" />
    <bpmn2:parallelGateway id="Gateway_0mv3z8q" sf:guid="728f6ed2-95ed-425d-c521-c9c8a84a4f7d">
      <bpmn2:incoming>Flow_1lwxdno</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0frpq2j</bpmn2:outgoing>
      <bpmn2:outgoing>Flow_1ka6fx7</bpmn2:outgoing>
    </bpmn2:parallelGateway>
    <bpmn2:parallelGateway id="Gateway_1k49b42" sf:guid="36266a43-0fca-461b-e2eb-0c80c731b1b7">
      <bpmn2:incoming>Flow_1l5jqd4</bpmn2:incoming>
      <bpmn2:incoming>Flow_010jvgy</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0y1dz4t</bpmn2:outgoing>
    </bpmn2:parallelGateway>
  </bpmn2:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="pProcess_Code_2522">
      <bpmndi:BPMNEdge id="Flow_010jvgy_di" bpmnElement="Flow_010jvgy">
        <di:waypoint x="1040" y="370" />
        <di:waypoint x="1130" y="370" />
        <di:waypoint x="1130" y="283" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1l5jqd4_di" bpmnElement="Flow_1l5jqd4">
        <di:waypoint x="1040" y="170" />
        <di:waypoint x="1130" y="170" />
        <di:waypoint x="1130" y="233" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1ka6fx7_di" bpmnElement="Flow_1ka6fx7">
        <di:waypoint x="850" y="283" />
        <di:waypoint x="850" y="370" />
        <di:waypoint x="940" y="370" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0frpq2j_di" bpmnElement="Flow_0frpq2j">
        <di:waypoint x="850" y="233" />
        <di:waypoint x="850" y="170" />
        <di:waypoint x="940" y="170" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0y1dz4t_di" bpmnElement="Flow_0y1dz4t">
        <di:waypoint x="1155" y="258" />
        <di:waypoint x="1242" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1lwxdno_di" bpmnElement="Flow_1lwxdno">
        <di:waypoint x="760" y="258" />
        <di:waypoint x="825" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0rxpy9b_di" bpmnElement="Flow_0rxpy9b">
        <di:waypoint x="600" y="258" />
        <di:waypoint x="660" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0lfs4bv_di" bpmnElement="Flow_0lfs4bv">
        <di:waypoint x="448" y="258" />
        <di:waypoint x="500" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="412" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="418" y="283" width="25" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1vn8pdc_di" bpmnElement="Activity_1vn8pdc">
        <dc:Bounds x="500" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0hss79q_di" bpmnElement="Activity_0hss79q">
        <dc:Bounds x="660" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_1j1js28_di" bpmnElement="Event_1j1js28">
        <dc:Bounds x="1242" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_056w0go_di" bpmnElement="Activity_056w0go">
        <dc:Bounds x="940" y="130" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0zy9snq_di" bpmnElement="Activity_0zy9snq">
        <dc:Bounds x="940" y="330" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Gateway_0zh6kl5_di" bpmnElement="Gateway_0mv3z8q">
        <dc:Bounds x="825" y="233" width="50" height="50" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Gateway_12pdwyc_di" bpmnElement="Gateway_1k49b42">
        <dc:Bounds x="1105" y="233" width="50" height="50" />
      </bpmndi:BPMNShape>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn2:definitions>
', 0, NULL, NULL, 0, NULL, CAST(0x0000B0D301112F28 AS DateTime), CAST(0x0000B0D301116DDB AS DateTime))
INSERT [dbo].[WfProcess] ([ID], [ProcessGUID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1366, N'cbee9ff6-922b-4776-adcf-5f1ad775388e', N'1', N'Process_Name_6738', N'Process_Code_6738', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn2:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL" id="bpmn2-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn2:process id="pProcess_Code_6738" sf:guid="cbee9ff6-922b-4776-adcf-5f1ad775388e" sf:code="Process_Code_6738" name="Process_Name_6738" isExecutable="true" sf:version="1">
    <bpmn2:startEvent id="StartEvent_1" sf:guid="58a3545d-2697-446e-9b47-983a92848687" name="Start">
      <bpmn2:outgoing>Flow_0asgj92</bpmn2:outgoing>
    </bpmn2:startEvent>
    <bpmn2:task id="Activity_0u59x9g" sf:guid="2f3d3f21-6e4d-4164-9581-2bf42f33b555" name="Task-01">
      <bpmn2:incoming>Flow_0asgj92</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0ugbr6r</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_0asgj92" sf:guid="3e61dc29-517f-4371-f5a7-3cbf01768ead" sourceRef="StartEvent_1" targetRef="Activity_0u59x9g" sf:from="58a3545d-2697-446e-9b47-983a92848687" sf:to="2f3d3f21-6e4d-4164-9581-2bf42f33b555" />
    <bpmn2:sequenceFlow id="Flow_0ugbr6r" sf:guid="5f3dad93-cd94-437f-9181-a6ee480a3887" sourceRef="Activity_0u59x9g" targetRef="Gateway_1818ck7" sf:from="2f3d3f21-6e4d-4164-9581-2bf42f33b555" sf:to="5e20aff5-5549-4e18-8b3e-7ce4a7abc3c8" />
    <bpmn2:task id="Activity_1in8dem" sf:guid="9ebbdfe3-48a7-4643-89e9-7fa88eef0cdd" name="Task-02">
      <bpmn2:incoming>Flow_1s9g0vx</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0fbbwaj</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:endEvent id="Event_1nct2qo" sf:guid="4247b6c7-aef6-4312-bda4-2dc3ff33df90" name="End">
      <bpmn2:incoming>Flow_1424ixm</bpmn2:incoming>
      <bpmn2:incoming>Flow_0fbbwaj</bpmn2:incoming>
    </bpmn2:endEvent>
    <bpmn2:inclusiveGateway id="Gateway_1818ck7" sf:guid="5e20aff5-5549-4e18-8b3e-7ce4a7abc3c8">
      <bpmn2:incoming>Flow_0ugbr6r</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1s9g0vx</bpmn2:outgoing>
      <bpmn2:outgoing>Flow_1424ixm</bpmn2:outgoing>
    </bpmn2:inclusiveGateway>
    <bpmn2:sequenceFlow id="Flow_1s9g0vx" sf:guid="00ebbd50-1f36-4c78-af95-e526bd9ac74c" sourceRef="Gateway_1818ck7" targetRef="Activity_1in8dem" sf:from="5e20aff5-5549-4e18-8b3e-7ce4a7abc3c8" sf:to="9ebbdfe3-48a7-4643-89e9-7fa88eef0cdd" />
    <bpmn2:sequenceFlow id="Flow_1424ixm" sf:guid="33a5d037-c3be-4166-9630-5b8e59ea8911" sourceRef="Gateway_1818ck7" targetRef="Event_1nct2qo" sf:from="5e20aff5-5549-4e18-8b3e-7ce4a7abc3c8" sf:to="4247b6c7-aef6-4312-bda4-2dc3ff33df90" />
    <bpmn2:sequenceFlow id="Flow_0fbbwaj" sf:guid="9d8d95ad-268d-4abf-82c1-bb757354ba19" sourceRef="Activity_1in8dem" targetRef="Event_1nct2qo" sf:from="9ebbdfe3-48a7-4643-89e9-7fa88eef0cdd" sf:to="4247b6c7-aef6-4312-bda4-2dc3ff33df90" />
  </bpmn2:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="pProcess_Code_6738">
      <bpmndi:BPMNEdge id="Flow_1424ixm_di" bpmnElement="Flow_1424ixm">
        <di:waypoint x="680" y="283" />
        <di:waypoint x="680" y="370" />
        <di:waypoint x="762" y="370" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1s9g0vx_di" bpmnElement="Flow_1s9g0vx">
        <di:waypoint x="680" y="233" />
        <di:waypoint x="680" y="170" />
        <di:waypoint x="750" y="170" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0ugbr6r_di" bpmnElement="Flow_0ugbr6r">
        <di:waypoint x="600" y="258" />
        <di:waypoint x="655" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0asgj92_di" bpmnElement="Flow_0asgj92">
        <di:waypoint x="448" y="258" />
        <di:waypoint x="500" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0fbbwaj_di" bpmnElement="Flow_0fbbwaj">
        <di:waypoint x="800" y="210" />
        <di:waypoint x="800" y="281" />
        <di:waypoint x="780" y="281" />
        <di:waypoint x="780" y="352" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="412" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="418" y="283" width="24" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0u59x9g_di" bpmnElement="Activity_0u59x9g">
        <dc:Bounds x="500" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1in8dem_di" bpmnElement="Activity_1in8dem">
        <dc:Bounds x="750" y="130" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_1nct2qo_di" bpmnElement="Event_1nct2qo">
        <dc:Bounds x="762" y="352" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="771" y="395" width="19" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Gateway_0itsu83_di" bpmnElement="Gateway_1818ck7">
        <dc:Bounds x="655" y="233" width="50" height="50" />
      </bpmndi:BPMNShape>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn2:definitions>
', 0, NULL, NULL, 0, NULL, CAST(0x0000B0D3015E5F96 AS DateTime), CAST(0x0000B0D5013CFB66 AS DateTime))
INSERT [dbo].[WfProcess] ([ID], [ProcessGUID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1367, N'cbee9ff6-922b-4776-adcf-5f1ad775388e', N'2', N'Process_Name_6738', N'Process_Code_6738', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn2:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL" id="bpmn2-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn2:process id="pProcess_Code_6738" sf:guid="cbee9ff6-922b-4776-adcf-5f1ad775388e" sf:code="Process_Code_6738" name="Process_Name_6738" isExecutable="true" sf:version="1">
    <bpmn2:startEvent id="StartEvent_1" sf:guid="58a3545d-2697-446e-9b47-983a92848687" name="Start">
      <bpmn2:outgoing>Flow_0asgj92</bpmn2:outgoing>
    </bpmn2:startEvent>
    <bpmn2:task id="Activity_0u59x9g" sf:guid="2f3d3f21-6e4d-4164-9581-2bf42f33b555" name="Task-01">
      <bpmn2:incoming>Flow_0asgj92</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0ugbr6r</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_0asgj92" sf:guid="3e61dc29-517f-4371-f5a7-3cbf01768ead" sourceRef="StartEvent_1" targetRef="Activity_0u59x9g" sf:from="58a3545d-2697-446e-9b47-983a92848687" sf:to="2f3d3f21-6e4d-4164-9581-2bf42f33b555" />
    <bpmn2:sequenceFlow id="Flow_0ugbr6r" sf:guid="5f3dad93-cd94-437f-9181-a6ee480a3887" sourceRef="Activity_0u59x9g" targetRef="Gateway_1818ck7" sf:from="2f3d3f21-6e4d-4164-9581-2bf42f33b555" sf:to="5e20aff5-5549-4e18-8b3e-7ce4a7abc3c8" />
    <bpmn2:task id="Activity_1in8dem" sf:guid="9ebbdfe3-48a7-4643-89e9-7fa88eef0cdd" name="Task-02">
      <bpmn2:incoming>Flow_1s9g0vx</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1emzlvz</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:endEvent id="Event_1nct2qo" sf:guid="4247b6c7-aef6-4312-bda4-2dc3ff33df90" name="End">
      <bpmn2:incoming>Flow_0dq3rbk</bpmn2:incoming>
    </bpmn2:endEvent>
    <bpmn2:inclusiveGateway id="Gateway_1818ck7" sf:guid="5e20aff5-5549-4e18-8b3e-7ce4a7abc3c8">
      <bpmn2:incoming>Flow_0ugbr6r</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1s9g0vx</bpmn2:outgoing>
      <bpmn2:outgoing>Flow_1t296dr</bpmn2:outgoing>
    </bpmn2:inclusiveGateway>
    <bpmn2:sequenceFlow id="Flow_1s9g0vx" sf:guid="00ebbd50-1f36-4c78-af95-e526bd9ac74c" sourceRef="Gateway_1818ck7" targetRef="Activity_1in8dem" sf:from="5e20aff5-5549-4e18-8b3e-7ce4a7abc3c8" sf:to="9ebbdfe3-48a7-4643-89e9-7fa88eef0cdd" />
    <bpmn2:task id="Activity_0y9vvrm" sf:guid="a043c2de-be51-459d-d3c3-b511380cee0c" name="Task-03">
      <bpmn2:incoming>Flow_1t296dr</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1hii2hh</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_1t296dr" sf:guid="aa5c60ad-e241-4955-c75b-baa2a418b055" sourceRef="Gateway_1818ck7" targetRef="Activity_0y9vvrm" sf:from="5e20aff5-5549-4e18-8b3e-7ce4a7abc3c8" sf:to="a043c2de-be51-459d-d3c3-b511380cee0c" />
    <bpmn2:sequenceFlow id="Flow_1emzlvz" sf:guid="f3e6ae20-c2d0-4e7d-edc9-62ddd0189661" sourceRef="Activity_1in8dem" targetRef="Gateway_0om4a3m" sf:from="9ebbdfe3-48a7-4643-89e9-7fa88eef0cdd" sf:to="fa4c7eff-2dd8-4215-de78-672b0117dc96" />
    <bpmn2:inclusiveGateway id="Gateway_0om4a3m" sf:guid="fa4c7eff-2dd8-4215-de78-672b0117dc96">
      <bpmn2:incoming>Flow_1emzlvz</bpmn2:incoming>
      <bpmn2:incoming>Flow_1hii2hh</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0dq3rbk</bpmn2:outgoing>
    </bpmn2:inclusiveGateway>
    <bpmn2:sequenceFlow id="Flow_1hii2hh" sf:guid="fbf984d3-90c8-460a-96eb-6520cd40845f" sourceRef="Activity_0y9vvrm" targetRef="Gateway_0om4a3m" sf:from="a043c2de-be51-459d-d3c3-b511380cee0c" sf:to="fa4c7eff-2dd8-4215-de78-672b0117dc96" />
    <bpmn2:sequenceFlow id="Flow_0dq3rbk" sf:guid="4e630274-5e8f-481d-c2c6-e2fbf862cab0" sourceRef="Gateway_0om4a3m" targetRef="Event_1nct2qo" sf:from="fa4c7eff-2dd8-4215-de78-672b0117dc96" sf:to="4247b6c7-aef6-4312-bda4-2dc3ff33df90" />
  </bpmn2:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="pProcess_Code_6738">
      <bpmndi:BPMNEdge id="Flow_1s9g0vx_di" bpmnElement="Flow_1s9g0vx">
        <di:waypoint x="680" y="233" />
        <di:waypoint x="680" y="170" />
        <di:waypoint x="750" y="170" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0ugbr6r_di" bpmnElement="Flow_0ugbr6r">
        <di:waypoint x="600" y="258" />
        <di:waypoint x="655" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0asgj92_di" bpmnElement="Flow_0asgj92">
        <di:waypoint x="448" y="258" />
        <di:waypoint x="500" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1t296dr_di" bpmnElement="Flow_1t296dr">
        <di:waypoint x="680" y="283" />
        <di:waypoint x="680" y="370" />
        <di:waypoint x="750" y="370" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1emzlvz_di" bpmnElement="Flow_1emzlvz">
        <di:waypoint x="850" y="170" />
        <di:waypoint x="930" y="170" />
        <di:waypoint x="930" y="233" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1hii2hh_di" bpmnElement="Flow_1hii2hh">
        <di:waypoint x="850" y="370" />
        <di:waypoint x="930" y="370" />
        <di:waypoint x="930" y="283" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0dq3rbk_di" bpmnElement="Flow_0dq3rbk">
        <di:waypoint x="955" y="258" />
        <di:waypoint x="1052" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="412" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="418" y="283" width="24" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0u59x9g_di" bpmnElement="Activity_0u59x9g">
        <dc:Bounds x="500" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1in8dem_di" bpmnElement="Activity_1in8dem">
        <dc:Bounds x="750" y="130" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Gateway_0itsu83_di" bpmnElement="Gateway_1818ck7">
        <dc:Bounds x="655" y="233" width="50" height="50" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0y9vvrm_di" bpmnElement="Activity_0y9vvrm">
        <dc:Bounds x="750" y="330" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Gateway_1yu237c_di" bpmnElement="Gateway_0om4a3m">
        <dc:Bounds x="905" y="233" width="50" height="50" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_1nct2qo_di" bpmnElement="Event_1nct2qo">
        <dc:Bounds x="1052" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="1061" y="283" width="19" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn2:definitions>
', 0, NULL, NULL, 0, NULL, CAST(0x0000B0D500D6D57B AS DateTime), CAST(0x0000B0D500D7D030 AS DateTime))
INSERT [dbo].[WfProcess] ([ID], [ProcessGUID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1368, N'02636d59-1b25-478c-9382-7076b8a590ab', N'1', N'Process_Name_3656', N'Process_Code_3656', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn2:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL" id="bpmn2-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn2:process id="pProcess_Code_3656" sf:guid="02636d59-1b25-478c-9382-7076b8a590ab" sf:code="Process_Code_3656" name="Process_Name_3656" isExecutable="true" sf:version="1">
    <bpmn2:startEvent id="StartEvent_1" sf:guid="f9379974-9628-4d1f-a3a7-096a2094fe2d">
      <bpmn2:outgoing>Flow_0ym4lu9</bpmn2:outgoing>
    </bpmn2:startEvent>
    <bpmn2:task id="Activity_1fvy2uu" sf:guid="b0ad4817-040b-467a-f3df-cbb58f98991a">
      <bpmn2:incoming>Flow_0ym4lu9</bpmn2:incoming>
      <bpmn2:outgoing>Flow_089yz2b</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_0ym4lu9" sf:guid="a4e67543-19b2-40b2-da1b-751c3597bc7a" sourceRef="StartEvent_1" targetRef="Activity_1fvy2uu" sf:from="f9379974-9628-4d1f-a3a7-096a2094fe2d" sf:to="b0ad4817-040b-467a-f3df-cbb58f98991a" />
    <bpmn2:sequenceFlow id="Flow_089yz2b" sf:guid="eefa276c-5a05-4cf1-fdb0-e7a5df6971bf" sourceRef="Activity_1fvy2uu" targetRef="Gateway_17guic4" sf:from="b0ad4817-040b-467a-f3df-cbb58f98991a" sf:to="e58bebe8-4a72-4374-b204-f63550a009fb" />
    <bpmn2:inclusiveGateway id="Gateway_17guic4" sf:guid="e58bebe8-4a72-4374-b204-f63550a009fb">
      <bpmn2:incoming>Flow_089yz2b</bpmn2:incoming>
      <bpmn2:outgoing>Flow_00qn242</bpmn2:outgoing>
      <bpmn2:outgoing>Flow_04ix3d7</bpmn2:outgoing>
      <bpmn2:outgoing>Flow_11tc2p1</bpmn2:outgoing>
      <bpmn2:outgoing>Flow_0d7z6oi</bpmn2:outgoing>
      <bpmn2:outgoing>Flow_1l4tsyw</bpmn2:outgoing>
      <bpmn2:outgoing>Flow_18dyoyr</bpmn2:outgoing>
      <bpmn2:outgoing>Flow_03obcxf</bpmn2:outgoing>
    </bpmn2:inclusiveGateway>
    <bpmn2:task id="Activity_0n02fh6" sf:guid="4754864c-f01d-46fb-d2fb-053acfbe0ebd">
      <bpmn2:incoming>Flow_00qn242</bpmn2:incoming>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_00qn242" sf:guid="f78a65b7-07a3-4fd5-e048-6a7c8014925a" sourceRef="Gateway_17guic4" targetRef="Activity_0n02fh6" sf:from="e58bebe8-4a72-4374-b204-f63550a009fb" sf:to="4754864c-f01d-46fb-d2fb-053acfbe0ebd" />
    <bpmn2:task id="Activity_0c8t3uy" sf:guid="1d4eadef-00e4-4313-fc7e-549040b331e4">
      <bpmn2:incoming>Flow_04ix3d7</bpmn2:incoming>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_04ix3d7" sf:guid="88635e0e-51df-458b-f752-90c5d9b86719" sourceRef="Gateway_17guic4" targetRef="Activity_0c8t3uy" sf:from="e58bebe8-4a72-4374-b204-f63550a009fb" sf:to="1d4eadef-00e4-4313-fc7e-549040b331e4" />
    <bpmn2:endEvent id="Event_16nn1kl" sf:guid="e4d36b7a-acdc-49c4-84dd-b551dc8143c0">
      <bpmn2:incoming>Flow_1biuolr</bpmn2:incoming>
      <bpmn2:incoming>Flow_18dyoyr</bpmn2:incoming>
      <bpmn2:incoming>Flow_03obcxf</bpmn2:incoming>
    </bpmn2:endEvent>
    <bpmn2:task id="Activity_081oq9f" sf:guid="23d2580d-4d30-4a73-fbcd-d751e78387a3">
      <bpmn2:incoming>Flow_11tc2p1</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0vh06l7</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_11tc2p1" sf:guid="8debc3b6-2e9d-4a78-a41e-422055d45f69" sourceRef="Gateway_17guic4" targetRef="Activity_081oq9f" sf:from="e58bebe8-4a72-4374-b204-f63550a009fb" sf:to="23d2580d-4d30-4a73-fbcd-d751e78387a3" />
    <bpmn2:task id="Activity_1yutj8m" sf:guid="5f8b01e9-98fb-4759-890a-f2461c1fefd9">
      <bpmn2:incoming>Flow_0d7z6oi</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0q0nq3n</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_0d7z6oi" sf:guid="3d8ee17d-c4ec-4896-fbc3-ef735c63da8d" sourceRef="Gateway_17guic4" targetRef="Activity_1yutj8m" sf:from="e58bebe8-4a72-4374-b204-f63550a009fb" sf:to="5f8b01e9-98fb-4759-890a-f2461c1fefd9" />
    <bpmn2:task id="Activity_1s4h1g1" sf:guid="db7b63c3-fabc-4cab-edf7-26bbbb0fe912">
      <bpmn2:incoming>Flow_1l4tsyw</bpmn2:incoming>
      <bpmn2:outgoing>Flow_09m1gug</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_1l4tsyw" sf:guid="01b1fc05-273a-4e7a-dc0e-f06c79eb8262" sourceRef="Gateway_17guic4" targetRef="Activity_1s4h1g1" sf:from="e58bebe8-4a72-4374-b204-f63550a009fb" sf:to="db7b63c3-fabc-4cab-edf7-26bbbb0fe912" />
    <bpmn2:sequenceFlow id="Flow_0q0nq3n" sf:guid="42e9bf1b-283f-4e21-8baa-4c994f16cc2b" sourceRef="Activity_1yutj8m" targetRef="Gateway_13ysa2f" sf:from="5f8b01e9-98fb-4759-890a-f2461c1fefd9" sf:to="b8fefc1d-2ae1-4a19-9365-aa532b02eb24" />
    <bpmn2:inclusiveGateway id="Gateway_13ysa2f" sf:guid="b8fefc1d-2ae1-4a19-9365-aa532b02eb24">
      <bpmn2:incoming>Flow_0q0nq3n</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0p82aro</bpmn2:outgoing>
      <bpmn2:outgoing>Flow_18l8tfa</bpmn2:outgoing>
    </bpmn2:inclusiveGateway>
    <bpmn2:task id="Activity_1gg78jj" sf:guid="d20baa37-159d-421d-f178-1675d9c0e38b">
      <bpmn2:incoming>Flow_0p82aro</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1vxc23n</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_0p82aro" sf:guid="046071dc-d151-49c6-9f17-bf03857a0a68" sourceRef="Gateway_13ysa2f" targetRef="Activity_1gg78jj" sf:from="b8fefc1d-2ae1-4a19-9365-aa532b02eb24" sf:to="d20baa37-159d-421d-f178-1675d9c0e38b" />
    <bpmn2:task id="Activity_0rxjyvh" sf:guid="1ad25c59-03a4-400e-924a-e66fb028d768">
      <bpmn2:incoming>Flow_0vh06l7</bpmn2:incoming>
      <bpmn2:incoming>Flow_18l8tfa</bpmn2:incoming>
      <bpmn2:incoming>Flow_1vxc23n</bpmn2:incoming>
      <bpmn2:incoming>Flow_09m1gug</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1biuolr</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_0vh06l7" sf:guid="6c32b5e4-43e4-4027-bdd7-cf0f20826407" sourceRef="Activity_081oq9f" targetRef="Activity_0rxjyvh" sf:from="23d2580d-4d30-4a73-fbcd-d751e78387a3" sf:to="1ad25c59-03a4-400e-924a-e66fb028d768" />
    <bpmn2:sequenceFlow id="Flow_18l8tfa" sf:guid="e3e8bf4b-8b57-432c-c18f-af18b9d68bd7" sourceRef="Gateway_13ysa2f" targetRef="Activity_0rxjyvh" sf:from="b8fefc1d-2ae1-4a19-9365-aa532b02eb24" sf:to="1ad25c59-03a4-400e-924a-e66fb028d768" />
    <bpmn2:sequenceFlow id="Flow_1vxc23n" sf:guid="2132dc3e-537b-46f6-f97a-7c4adebd93e8" sourceRef="Activity_1gg78jj" targetRef="Activity_0rxjyvh" sf:from="d20baa37-159d-421d-f178-1675d9c0e38b" sf:to="1ad25c59-03a4-400e-924a-e66fb028d768" />
    <bpmn2:sequenceFlow id="Flow_09m1gug" sf:guid="78e3aa6f-2f8f-4f39-fd2b-9a8fa5ff657a" sourceRef="Activity_1s4h1g1" targetRef="Activity_0rxjyvh" sf:from="db7b63c3-fabc-4cab-edf7-26bbbb0fe912" sf:to="1ad25c59-03a4-400e-924a-e66fb028d768" />
    <bpmn2:sequenceFlow id="Flow_1biuolr" sf:guid="0037abd7-97b2-43d7-c2ff-42dd5a9e7655" sourceRef="Activity_0rxjyvh" targetRef="Event_16nn1kl" sf:from="1ad25c59-03a4-400e-924a-e66fb028d768" sf:to="e4d36b7a-acdc-49c4-84dd-b551dc8143c0" />
    <bpmn2:sequenceFlow id="Flow_18dyoyr" sf:guid="4236c83e-bc7c-4ba7-963d-0551336b5604" sourceRef="Gateway_17guic4" targetRef="Event_16nn1kl" sf:from="e58bebe8-4a72-4374-b204-f63550a009fb" sf:to="e4d36b7a-acdc-49c4-84dd-b551dc8143c0" />
    <bpmn2:sequenceFlow id="Flow_03obcxf" sf:guid="9d87659a-b206-4d27-b815-1e3cbc64663a" sourceRef="Gateway_17guic4" targetRef="Event_16nn1kl" sf:from="e58bebe8-4a72-4374-b204-f63550a009fb" sf:to="e4d36b7a-acdc-49c4-84dd-b551dc8143c0" />
  </bpmn2:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="pProcess_Code_3656">
      <bpmndi:BPMNEdge id="Flow_03obcxf_di" bpmnElement="Flow_03obcxf">
        <di:waypoint x="470" y="365" />
        <di:waypoint x="470" y="490" />
        <di:waypoint x="358" y="490" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_18dyoyr_di" bpmnElement="Flow_18dyoyr">
        <di:waypoint x="480" y="375" />
        <di:waypoint x="480" y="490" />
        <di:waypoint x="358" y="490" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1biuolr_di" bpmnElement="Flow_1biuolr">
        <di:waypoint x="1070" y="460" />
        <di:waypoint x="714" y="460" />
        <di:waypoint x="714" y="490" />
        <di:waypoint x="358" y="490" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_09m1gug_di" bpmnElement="Flow_09m1gug">
        <di:waypoint x="660" y="570" />
        <di:waypoint x="1100" y="570" />
        <di:waypoint x="1100" y="500" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1vxc23n_di" bpmnElement="Flow_1vxc23n">
        <di:waypoint x="930" y="460" />
        <di:waypoint x="1070" y="460" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_18l8tfa_di" bpmnElement="Flow_18l8tfa">
        <di:waypoint x="740" y="435" />
        <di:waypoint x="740" y="400" />
        <di:waypoint x="1120" y="400" />
        <di:waypoint x="1120" y="420" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0vh06l7_di" bpmnElement="Flow_0vh06l7">
        <di:waypoint x="660" y="350" />
        <di:waypoint x="1120" y="350" />
        <di:waypoint x="1120" y="420" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0p82aro_di" bpmnElement="Flow_0p82aro">
        <di:waypoint x="765" y="460" />
        <di:waypoint x="830" y="460" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0q0nq3n_di" bpmnElement="Flow_0q0nq3n">
        <di:waypoint x="660" y="460" />
        <di:waypoint x="715" y="460" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1l4tsyw_di" bpmnElement="Flow_1l4tsyw">
        <di:waypoint x="480" y="375" />
        <di:waypoint x="480" y="570" />
        <di:waypoint x="560" y="570" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0d7z6oi_di" bpmnElement="Flow_0d7z6oi">
        <di:waypoint x="480" y="375" />
        <di:waypoint x="480" y="460" />
        <di:waypoint x="560" y="460" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_11tc2p1_di" bpmnElement="Flow_11tc2p1">
        <di:waypoint x="505" y="350" />
        <di:waypoint x="560" y="350" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_04ix3d7_di" bpmnElement="Flow_04ix3d7">
        <di:waypoint x="480" y="325" />
        <di:waypoint x="480" y="150" />
        <di:waypoint x="420" y="150" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_00qn242_di" bpmnElement="Flow_00qn242">
        <di:waypoint x="480" y="325" />
        <di:waypoint x="480" y="180" />
        <di:waypoint x="560" y="180" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_089yz2b_di" bpmnElement="Flow_089yz2b">
        <di:waypoint x="400" y="350" />
        <di:waypoint x="455" y="350" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0ym4lu9_di" bpmnElement="Flow_0ym4lu9">
        <di:waypoint x="248" y="350" />
        <di:waypoint x="300" y="350" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="212" y="332" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1fvy2uu_di" bpmnElement="Activity_1fvy2uu">
        <dc:Bounds x="300" y="310" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Gateway_00shhid_di" bpmnElement="Gateway_17guic4">
        <dc:Bounds x="455" y="325" width="50" height="50" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0n02fh6_di" bpmnElement="Activity_0n02fh6">
        <dc:Bounds x="560" y="140" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0c8t3uy_di" bpmnElement="Activity_0c8t3uy">
        <dc:Bounds x="320" y="110" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_16nn1kl_di" bpmnElement="Event_16nn1kl">
        <dc:Bounds x="322" y="472" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_081oq9f_di" bpmnElement="Activity_081oq9f">
        <dc:Bounds x="560" y="310" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1yutj8m_di" bpmnElement="Activity_1yutj8m">
        <dc:Bounds x="560" y="420" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1s4h1g1_di" bpmnElement="Activity_1s4h1g1">
        <dc:Bounds x="560" y="530" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Gateway_0knjyal_di" bpmnElement="Gateway_13ysa2f">
        <dc:Bounds x="715" y="435" width="50" height="50" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1gg78jj_di" bpmnElement="Activity_1gg78jj">
        <dc:Bounds x="830" y="420" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0rxjyvh_di" bpmnElement="Activity_0rxjyvh">
        <dc:Bounds x="1070" y="420" width="100" height="80" />
      </bpmndi:BPMNShape>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn2:definitions>
', 0, NULL, NULL, 0, NULL, CAST(0x0000B0E200A55DC0 AS DateTime), CAST(0x0000B10C0100CB34 AS DateTime))
INSERT [dbo].[WfProcess] ([ID], [ProcessGUID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1370, N'637cb2d1-c060-4f07-bc7b-d632dadada30', N'1', N'MessageCatch', N'daohfoah', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn2:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL" id="bpmn2-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn2:process id="daohfoah" sf:guid="637cb2d1-c060-4f07-bc7b-d632dadada30" sf:code="daohfoah" name="MessageCatch" isExecutable="true" sf:version="1">
    <bpmn2:startEvent id="StartNode_8343" sf:guid="1b278041-6a2c-41e5-bbac-e42c3f013535" sf:code="Start" name="Start">
      <bpmn2:outgoing>Flow_3133</bpmn2:outgoing>
    </bpmn2:startEvent>
    <bpmn2:task id="TaskNode_5385" sf:guid="fa27afb8-952a-45ff-99e0-460058c021b7" sf:code="task001" name="Task-001">
      <bpmn2:incoming>Flow_3133</bpmn2:incoming>
      <bpmn2:outgoing>Flow_9959</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:task id="TaskNode_5405" sf:guid="3ed5fb13-2046-49a1-ad2f-6b485a4cab7b" sf:code="task002" name="Task-002">
      <bpmn2:incoming>Flow_9959</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1b4i3uf</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:task id="TaskNode_4757" sf:guid="eaf9ef23-e48e-46a5-94aa-ece1e69cd28d" sf:code="task003" name="Task-003">
      <bpmn2:incoming>Flow_1ptj72v</bpmn2:incoming>
      <bpmn2:outgoing>Flow_4080</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:endEvent id="EndNode_2679" sf:guid="8c918263-4bfd-4c3e-9ed1-4f462f93ffc6" sf:code="End" name="End">
      <bpmn2:incoming>Flow_4080</bpmn2:incoming>
    </bpmn2:endEvent>
    <bpmn2:sequenceFlow id="Flow_3133" sf:guid="dba78cc6-a08c-4de9-95ea-dedf04cf8c1d" sourceRef="StartNode_8343" targetRef="TaskNode_5385" sf:from="1b278041-6a2c-41e5-bbac-e42c3f013535" sf:to="fa27afb8-952a-45ff-99e0-460058c021b7" />
    <bpmn2:sequenceFlow id="Flow_9959" sf:guid="22ec0cc3-505c-43d4-9386-548caf968b4a" sourceRef="TaskNode_5385" targetRef="TaskNode_5405" sf:from="fa27afb8-952a-45ff-99e0-460058c021b7" sf:to="3ed5fb13-2046-49a1-ad2f-6b485a4cab7b" />
    <bpmn2:sequenceFlow id="Flow_4080" sf:guid="c2ceac13-bca3-41b2-9f49-bf7b7f4855ea" sourceRef="TaskNode_4757" targetRef="EndNode_2679" sf:from="eaf9ef23-e48e-46a5-94aa-ece1e69cd28d" sf:to="8c918263-4bfd-4c3e-9ed1-4f462f93ffc6" />
    <bpmn2:sequenceFlow id="Flow_1b4i3uf" sf:guid="fac19bb5-5339-40a7-8fb1-31d4c9bfbb46" sourceRef="TaskNode_5405" targetRef="Event_13v3rtd" sf:from="3ed5fb13-2046-49a1-ad2f-6b485a4cab7b" sf:to="3ff1e222-df10-4d6e-dc5f-fa87f9f19571" />
    <bpmn2:sequenceFlow id="Flow_1ptj72v" sf:guid="abf052f4-67e3-4a7b-e19b-9bd1755129ff" sourceRef="Event_13v3rtd" targetRef="TaskNode_4757" sf:from="3ff1e222-df10-4d6e-dc5f-fa87f9f19571" sf:to="eaf9ef23-e48e-46a5-94aa-ece1e69cd28d" />
    <bpmn2:intermediateThrowEvent id="Event_13v3rtd" sf:guid="3ff1e222-df10-4d6e-dc5f-fa87f9f19571">
      <bpmn2:incoming>Flow_1b4i3uf</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1ptj72v</bpmn2:outgoing>
      <bpmn2:messageEventDefinition id="MessageEventDefinition_1cyc77k" messageRef="Message_GEE4JX" />
    </bpmn2:intermediateThrowEvent>
  </bpmn2:process>
  <bpmn2:message id="Message_GEE4JX" name="udtt" />
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="daohfoah">
      <bpmndi:BPMNEdge id="Flow_4080_di" bpmnElement="Flow_4080">
        <di:waypoint x="971" y="200" />
        <di:waypoint x="1027" y="200" />
        <di:waypoint x="1027" y="202" />
        <di:waypoint x="1082" y="202" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_9959_di" bpmnElement="Flow_9959">
        <di:waypoint x="340" y="200" />
        <di:waypoint x="500" y="200" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_3133_di" bpmnElement="Flow_3133">
        <di:waypoint x="240" y="200" />
        <di:waypoint x="340" y="200" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1b4i3uf_di" bpmnElement="Flow_1b4i3uf">
        <di:waypoint x="600" y="200" />
        <di:waypoint x="692" y="200" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1ptj72v_di" bpmnElement="Flow_1ptj72v">
        <di:waypoint x="728" y="200" />
        <di:waypoint x="871" y="200" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNShape id="BPMNShape_lwus86b_di" bpmnElement="StartNode_8343">
        <dc:Bounds x="240" y="180" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="BPMNShape_6zyt241_di" bpmnElement="TaskNode_5385">
        <dc:Bounds x="340" y="160" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="BPMNShape_75oy6nn_di" bpmnElement="TaskNode_5405">
        <dc:Bounds x="500" y="160" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="BPMNShape_vdh8s28_di" bpmnElement="TaskNode_4757">
        <dc:Bounds x="871" y="160" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="BPMNShape_70za8dj_di" bpmnElement="EndNode_2679">
        <dc:Bounds x="1082" y="182" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="1090" y="218" width="20" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_1j73lef_di" bpmnElement="Event_13v3rtd">
        <dc:Bounds x="692" y="182" width="36" height="36" />
      </bpmndi:BPMNShape>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn2:definitions>
', 0, NULL, N'', 0, NULL, CAST(0x0000B10C01064B1D AS DateTime), CAST(0x0000B1A7012DA06D AS DateTime))
INSERT [dbo].[WfProcess] ([ID], [ProcessGUID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1371, N'3a151512-e4dd-4124-dcea-6015a7b03889', N'1', N'oanfinfwhi', N'lnangoiahoi', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="utf-8"?><bpmn2:definitions targetNamespace="http://bpmn.io/schema/bpmn" id="bpmn2-diagram" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd" xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL"><bpmn2:process id="lnangoiahoi" name="oanfinfwhi" isExecutable="true" sf:guid="3a151512-e4dd-4124-dcea-6015a7b03889" sf:code="lnangoiahoi" sf:version="1"><bpmn2:startEvent id="StartNode_8674" name="start" sf:guid="c152db42-b1a1-4048-8db8-99f09c816dde" sf:code="Start"><bpmn2:outgoing>Flow_8933</bpmn2:outgoing></bpmn2:startEvent><bpmn2:task id="TaskNode_2651" name="Task-001" sf:guid="f49f79a5-4609-4ce7-9022-5903e7503eed" sf:code="task001"><bpmn2:incoming>Flow_8933</bpmn2:incoming><bpmn2:outgoing>Flow_2445</bpmn2:outgoing></bpmn2:task><bpmn2:parallelGateway id="GatewayNode_3582" name="and-split" sf:guid="d240d286-0124-440a-b067-25b7c67d2749" sf:code="andsplit001"><bpmn2:incoming>Flow_2445</bpmn2:incoming><bpmn2:outgoing>Flow_8221</bpmn2:outgoing><bpmn2:outgoing>Flow_5101</bpmn2:outgoing></bpmn2:parallelGateway><bpmn2:task id="TaskNode_6881" name="task-010" sf:guid="a6fb062c-2676-413d-a7ad-8ff2e82c602f" sf:code="task010"><bpmn2:incoming>Flow_8221</bpmn2:incoming><bpmn2:outgoing>Flow_6040</bpmn2:outgoing></bpmn2:task><bpmn2:task id="TaskNode_2885" name="task-020" sf:guid="76f1dce3-4564-4971-aeb9-35ad86c18801" sf:code="task020"><bpmn2:incoming>Flow_5101</bpmn2:incoming><bpmn2:outgoing>Flow_2602</bpmn2:outgoing></bpmn2:task><bpmn2:parallelGateway id="GatewayNode_9498" name="and-join" sf:guid="d79b224d-3f50-4f37-977d-3caf5b4d734a" sf:code="andjoin001"><bpmn2:incoming>Flow_2602</bpmn2:incoming><bpmn2:incoming>Flow_6040</bpmn2:incoming><bpmn2:outgoing>Flow_1765</bpmn2:outgoing></bpmn2:parallelGateway><bpmn2:task id="TaskNode_7216" name="task-100" sf:guid="70b31f6d-4557-4025-953f-60f9f4c22164" sf:code="task100"><bpmn2:incoming>Flow_1765</bpmn2:incoming><bpmn2:outgoing>Flow_5959</bpmn2:outgoing></bpmn2:task><bpmn2:endEvent id="EndNode_4790" name="end" sf:guid="de5e568a-0cbb-4c3b-b45d-a06a4f006439" sf:code="End"><bpmn2:incoming>Flow_5959</bpmn2:incoming></bpmn2:endEvent><bpmn2:sequenceFlow id="Flow_8933" sf:guid="26487d80-c327-476d-9ecd-d7a9ab7ec8ed" sf:from="c152db42-b1a1-4048-8db8-99f09c816dde" sf:to="f49f79a5-4609-4ce7-9022-5903e7503eed" sourceRef="StartNode_8674" targetRef="TaskNode_2651" /><bpmn2:sequenceFlow id="Flow_2445" sf:guid="b8a9146a-99f9-4024-9ca8-5293f821d216" sf:from="f49f79a5-4609-4ce7-9022-5903e7503eed" sf:to="d240d286-0124-440a-b067-25b7c67d2749" sourceRef="TaskNode_2651" targetRef="GatewayNode_3582" /><bpmn2:sequenceFlow id="Flow_8221" sf:guid="55d67cf5-8600-4df2-8ee6-050f3b22fc79" sf:from="d240d286-0124-440a-b067-25b7c67d2749" sf:to="a6fb062c-2676-413d-a7ad-8ff2e82c602f" sourceRef="GatewayNode_3582" targetRef="TaskNode_6881" /><bpmn2:sequenceFlow id="Flow_5101" sf:guid="8e74d5d3-ad32-408f-b707-7785bdfb9ea5" sf:from="d240d286-0124-440a-b067-25b7c67d2749" sf:to="76f1dce3-4564-4971-aeb9-35ad86c18801" sourceRef="GatewayNode_3582" targetRef="TaskNode_2885" /><bpmn2:sequenceFlow id="Flow_2602" sf:guid="b5a681ad-92ac-4937-80b0-e9b0fe12eaf3" sf:from="76f1dce3-4564-4971-aeb9-35ad86c18801" sf:to="d79b224d-3f50-4f37-977d-3caf5b4d734a" sourceRef="TaskNode_2885" targetRef="GatewayNode_9498" /><bpmn2:sequenceFlow id="Flow_6040" sf:guid="8adf5211-8789-42c2-b9ad-810f11eb52e9" sf:from="a6fb062c-2676-413d-a7ad-8ff2e82c602f" sf:to="d79b224d-3f50-4f37-977d-3caf5b4d734a" sourceRef="TaskNode_6881" targetRef="GatewayNode_9498" /><bpmn2:sequenceFlow id="Flow_1765" sf:guid="fef2c3b5-c000-493c-8a2b-789a2b441c4c" sf:from="d79b224d-3f50-4f37-977d-3caf5b4d734a" sf:to="70b31f6d-4557-4025-953f-60f9f4c22164" sourceRef="GatewayNode_9498" targetRef="TaskNode_7216" /><bpmn2:sequenceFlow id="Flow_5959" sf:guid="355d42e0-8d5b-4c7a-8ffd-b0ec172ccf77" sf:from="70b31f6d-4557-4025-953f-60f9f4c22164" sf:to="de5e568a-0cbb-4c3b-b45d-a06a4f006439" sourceRef="TaskNode_7216" targetRef="EndNode_4790" /></bpmn2:process><bpmndi:BPMNDiagram id="BPMNDiagram_1"><bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_100"><bpmndi:BPMNShape id="BPMNShape_a6a1uno_di" bpmnElement="StartNode_8674"><dc:Bounds height="36" width="36" x="240" y="180" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_gix0hns_di" bpmnElement="TaskNode_2651"><dc:Bounds height="80" width="100" x="340" y="160" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_yuhxnoj_di" bpmnElement="GatewayNode_3582"><dc:Bounds height="36" width="36" x="500" y="180" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_lgknntg_di" bpmnElement="TaskNode_6881"><dc:Bounds height="80" width="100" x="660" y="230" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_eqllcnj_di" bpmnElement="TaskNode_2885"><dc:Bounds height="80" width="100" x="660" y="70" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_vvfe0y2_di" bpmnElement="GatewayNode_9498"><dc:Bounds height="36" width="36" x="880" y="180" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_4m9135e_di" bpmnElement="TaskNode_7216"><dc:Bounds height="80" width="100" x="980" y="160" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_xjdbv6u_di" bpmnElement="EndNode_4790"><dc:Bounds height="36" width="36" x="1160" y="180" /></bpmndi:BPMNShape><bpmndi:BPMNEdge id="Flow_8933_di" bpmnElement="Flow_8933"><di:waypoint x="240" y="200" /><di:waypoint x="340" y="200" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_2445_di" bpmnElement="Flow_2445"><di:waypoint x="340" y="200" /><di:waypoint x="500" y="200" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_8221_di" bpmnElement="Flow_8221"><di:waypoint x="518" y="180" /><di:waypoint x="518" y="270" /><di:waypoint x="660" y="270" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_5101_di" bpmnElement="Flow_5101"><di:waypoint x="518" y="216" /><di:waypoint x="518" y="110" /><di:waypoint x="660" y="110" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_2602_di" bpmnElement="Flow_2602"><di:waypoint x="760" y="110" /><di:waypoint x="898" y="110" /><di:waypoint x="898" y="180" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_6040_di" bpmnElement="Flow_6040"><di:waypoint x="760" y="270" /><di:waypoint x="898" y="270" /><di:waypoint x="898" y="216" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_1765_di" bpmnElement="Flow_1765"><di:waypoint x="880" y="200" /><di:waypoint x="980" y="200" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_5959_di" bpmnElement="Flow_5959"><di:waypoint x="980" y="200" /><di:waypoint x="1160" y="200" /></bpmndi:BPMNEdge></bpmndi:BPMNPlane></bpmndi:BPMNDiagram></bpmn2:definitions>', 0, NULL, N'', 0, NULL, CAST(0x0000B10C01066156 AS DateTime), NULL)
INSERT [dbo].[WfProcess] ([ID], [ProcessGUID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1372, N'482daae3-3a2c-4de3-a71e-dfecc860b2e1', N'1', N'ActionsExampleProcess', N'Process_Code_3582', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn2:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL" id="bpmn2-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn2:process id="pProcess_Code_3582" sf:guid="482daae3-3a2c-4de3-a71e-dfecc860b2e1" sf:code="Process_Code_3582" name="ActionsExampleProcess" isExecutable="true" sf:version="1">
    <bpmn2:task id="Activity_0r7iyhi" sf:guid="2c738e1e-47bc-4fbb-cb98-af9c7448193d" name="task-01">
      <bpmn2:incoming>Flow_17nr29g</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0cjduvi</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_17nr29g" sf:guid="01dc903a-8df1-41d5-f9bf-7cc184cf90b2" sourceRef="StartEvent_1" targetRef="Activity_0r7iyhi" sf:from="25df5323-9801-463f-be8f-4cceff4d1bdc" sf:to="2c738e1e-47bc-4fbb-cb98-af9c7448193d" />
    <bpmn2:task id="Activity_08txs9e" sf:guid="628c41c0-e8e3-448a-b0c1-aa062db03c26" name="task-02">
      <bpmn2:extensionElements>
        <sf:actions>
          <sf:action fireType="before" methodType="WebApi" subMethodType="HttpPost" argus="adasjf" expression="http://localhost/sfdapi/api/post" />
        </sf:actions>
      </bpmn2:extensionElements>
      <bpmn2:incoming>Flow_0cjduvi</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1p6n0vh</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_0cjduvi" sf:guid="23c9ab92-da14-4e8b-ee7f-83bf4b4b0743" sourceRef="Activity_0r7iyhi" targetRef="Activity_08txs9e" sf:from="2c738e1e-47bc-4fbb-cb98-af9c7448193d" sf:to="628c41c0-e8e3-448a-b0c1-aa062db03c26" />
    <bpmn2:endEvent id="Event_0hofh1b" sf:guid="c7bb35ab-b66f-4559-f378-80f65461b8c6" name="End">
      <bpmn2:incoming>Flow_1p6n0vh</bpmn2:incoming>
    </bpmn2:endEvent>
    <bpmn2:sequenceFlow id="Flow_1p6n0vh" sf:guid="8036606c-5423-467f-b418-7ad41c78f3cf" sourceRef="Activity_08txs9e" targetRef="Event_0hofh1b" sf:from="628c41c0-e8e3-448a-b0c1-aa062db03c26" sf:to="c7bb35ab-b66f-4559-f378-80f65461b8c6" />
    <bpmn2:startEvent id="StartEvent_1" sf:guid="25df5323-9801-463f-be8f-4cceff4d1bdc" name="Start">
      <bpmn2:outgoing>Flow_17nr29g</bpmn2:outgoing>
      <bpmn2:conditionalEventDefinition id="ConditionalEventDefinition_13dcb7s">
        <bpmn2:condition xsi:type="bpmn2:tFormalExpression">fdshsdh</bpmn2:condition>
      </bpmn2:conditionalEventDefinition>
    </bpmn2:startEvent>
  </bpmn2:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="pProcess_Code_3582">
      <bpmndi:BPMNEdge id="Flow_1p6n0vh_di" bpmnElement="Flow_1p6n0vh">
        <di:waypoint x="760" y="258" />
        <di:waypoint x="822" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0cjduvi_di" bpmnElement="Flow_0cjduvi">
        <di:waypoint x="600" y="258" />
        <di:waypoint x="660" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_17nr29g_di" bpmnElement="Flow_17nr29g">
        <di:waypoint x="448" y="258" />
        <di:waypoint x="500" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNShape id="Activity_0r7iyhi_di" bpmnElement="Activity_0r7iyhi">
        <dc:Bounds x="500" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_08txs9e_di" bpmnElement="Activity_08txs9e">
        <dc:Bounds x="660" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_0hofh1b_di" bpmnElement="Event_0hofh1b">
        <dc:Bounds x="822" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="831" y="283" width="19" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_0a5susb_di" bpmnElement="StartEvent_1">
        <dc:Bounds x="412" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="418" y="283" width="24" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn2:definitions>
', 0, NULL, NULL, 0, NULL, CAST(0x0000B11200E22D63 AS DateTime), CAST(0x0000B113010605AF AS DateTime))
INSERT [dbo].[WfProcess] ([ID], [ProcessGUID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1373, N'84e645bd-c23a-48bf-9c61-5c8d94dc135f', N'1', N'Process_Name_8772', N'Process_Code_8772', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn2:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL" id="bpmn2-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn2:process id="pProcess_Code_8772" sf:guid="84e645bd-c23a-48bf-9c61-5c8d94dc135f" sf:code="Process_Code_8772" name="Process_Name_8772" isExecutable="true" sf:version="1">
    <bpmn2:startEvent id="StartEvent_1" sf:guid="60c94466-5680-49e6-8aab-f3ebbe4c4ebc" name="Start">
      <bpmn2:outgoing>Flow_1hgz4hx</bpmn2:outgoing>
    </bpmn2:startEvent>
    <bpmn2:task id="Activity_0nzekc0" sf:guid="438f85ba-e380-42c3-f0aa-225ca32eb845" name="task-01">
      <bpmn2:incoming>Flow_1hgz4hx</bpmn2:incoming>
      <bpmn2:outgoing>Flow_19gcly3</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_1hgz4hx" sf:guid="dabc5eb5-bfba-4c23-b0e9-3b17e30e1e29" sourceRef="StartEvent_1" targetRef="Activity_0nzekc0" sf:from="60c94466-5680-49e6-8aab-f3ebbe4c4ebc" sf:to="438f85ba-e380-42c3-f0aa-225ca32eb845" />
    <bpmn2:task id="Activity_1jrw61n" sf:guid="18c878fb-b001-4967-896a-9def1edecb77" name="multi-02">
      <bpmn2:extensionElements>
        <sf:multiSignDetail complexType="SignTogether" mergeType="Sequence" compareType="Count" completeOrder="5" />
      </bpmn2:extensionElements>
      <bpmn2:incoming>Flow_19gcly3</bpmn2:incoming>
      <bpmn2:outgoing>Flow_09rc929</bpmn2:outgoing>
      <bpmn2:multiInstanceLoopCharacteristics />
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_19gcly3" sf:guid="6a8f94e9-15bf-401f-fc81-2252fcac5635" name="a &#60;10" sourceRef="Activity_0nzekc0" targetRef="Activity_1jrw61n" sf:from="438f85ba-e380-42c3-f0aa-225ca32eb845" sf:to="18c878fb-b001-4967-896a-9def1edecb77">
      <bpmn2:extensionElements>
        <sf:sections>
          <sf:section name="myProperties">transiton custom propery</sf:section>
        </sf:sections>
      </bpmn2:extensionElements>
    </bpmn2:sequenceFlow>
    <bpmn2:task id="Activity_19naj9l" sf:guid="882ff76c-b543-4ea2-bc0c-a9cab63a2c48" name="task-03">
      <bpmn2:incoming>Flow_09rc929</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1t1jyen</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_09rc929" sf:guid="31087a92-8c65-4537-f844-8a0aaec1356e" sourceRef="Activity_1jrw61n" targetRef="Activity_19naj9l" sf:from="18c878fb-b001-4967-896a-9def1edecb77" sf:to="882ff76c-b543-4ea2-bc0c-a9cab63a2c48" />
    <bpmn2:endEvent id="Event_0d0kc27" sf:guid="4baeb8dc-ed7a-404c-c5b7-a1fc7f6fe77a">
      <bpmn2:incoming>Flow_1t1jyen</bpmn2:incoming>
    </bpmn2:endEvent>
    <bpmn2:sequenceFlow id="Flow_1t1jyen" sf:guid="2722dd69-51d6-4c9f-e9b8-af2eaac8065d" sourceRef="Activity_19naj9l" targetRef="Event_0d0kc27" sf:from="882ff76c-b543-4ea2-bc0c-a9cab63a2c48" sf:to="4baeb8dc-ed7a-404c-c5b7-a1fc7f6fe77a" />
  </bpmn2:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="pProcess_Code_8772">
      <bpmndi:BPMNEdge id="Flow_1t1jyen_di" bpmnElement="Flow_1t1jyen">
        <di:waypoint x="920" y="258" />
        <di:waypoint x="982" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_09rc929_di" bpmnElement="Flow_09rc929">
        <di:waypoint x="760" y="258" />
        <di:waypoint x="820" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_19gcly3_di" bpmnElement="Flow_19gcly3">
        <di:waypoint x="600" y="258" />
        <di:waypoint x="660" y="258" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="616" y="240" width="28" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1hgz4hx_di" bpmnElement="Flow_1hgz4hx">
        <di:waypoint x="448" y="258" />
        <di:waypoint x="500" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="412" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="418" y="283" width="24" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0nzekc0_di" bpmnElement="Activity_0nzekc0">
        <dc:Bounds x="500" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1jrw61n_di" bpmnElement="Activity_1jrw61n">
        <dc:Bounds x="660" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_19naj9l_di" bpmnElement="Activity_19naj9l">
        <dc:Bounds x="820" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_0d0kc27_di" bpmnElement="Event_0d0kc27">
        <dc:Bounds x="982" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn2:definitions>
', 0, NULL, NULL, 0, NULL, CAST(0x0000B11301076B30 AS DateTime), CAST(0x0000B1130141AD77 AS DateTime))
INSERT [dbo].[WfProcess] ([ID], [ProcessGUID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1399, N'072af8c3-482a-4b1c-890b-685ce2fcc75d', N'1', N'PriceProcess(SequenceTest)', N'PriceProcessCode', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="utf-8"?><bpmn2:definitions targetNamespace="http://bpmn.io/schema/bpmn" id="bpmn2-diagram" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd" xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL"><bpmn2:process id="PriceProcessCode" name="PriceProcess(SequenceTest)" isExecutable="true" sf:guid="072af8c3-482a-4b1c-890b-685ce2fcc75d" sf:code="PriceProcessCode" sf:version="1"><bpmn2:startEvent id="startEvent_AJBNOX" name="Start" sf:guid="9b78486d-5b8f-4be4-948e-522356e84e79" sf:code="startEvent_AJBNOX" sf:url="null"><bpmn2:outgoing>Flow_1177</bpmn2:outgoing></bpmn2:startEvent><bpmn2:endEvent id="endEvent_9IQ4FV" name="End" sf:guid="b53eb9ab-3af6-41ad-d722-bed946d19792" sf:code="endEvent_9IQ4FV" sf:url="null"><bpmn2:incoming>Flow_9263</bpmn2:incoming></bpmn2:endEvent><bpmn2:task id="task_5Q1Q82" name="Sales Submit" sf:guid="3c438212-4863-4ff8-efc9-a9096c4a8230" sf:code="task_5Q1Q82" sf:url="null"><bpmn2:extensionElements><sf:boundaries><sf:boundary event="Timer" expression="PT5M" /></sf:boundaries></bpmn2:extensionElements><bpmn2:extensionElements><sf:performers><sf:performer name="业务员(Sales)" outerId="9" outerCode="salesmate" outerType="Role" /></sf:performers></bpmn2:extensionElements><bpmn2:incoming>Flow_1177</bpmn2:incoming><bpmn2:outgoing>Flow_5825</bpmn2:outgoing></bpmn2:task><bpmn2:task id="task_HNGPSC" name="Manager Signature" sf:guid="eb833577-abb5-4239-875a-5f2e2fcb6d57" sf:code="task_HNGPSC" sf:url="null"><bpmn2:extensionElements><sf:boundaries><sf:boundary event="Timer" expression="" /></sf:boundaries></bpmn2:extensionElements><bpmn2:extensionElements><sf:performers><sf:performer name="打样员(Tech)" outerId="10" outerCode="techmate" outerType="Role" /></sf:performers></bpmn2:extensionElements><bpmn2:incoming>Flow_5825</bpmn2:incoming><bpmn2:outgoing>Flow_5356</bpmn2:outgoing></bpmn2:task><bpmn2:task id="task_9S66UP" name="Sales Confirm" sf:guid="cab57060-f433-422a-a66f-4a5ecfafd54e" sf:code="task_9S66UP" sf:url="null"><bpmn2:extensionElements><sf:performers><sf:performer name="业务员(Sales)" outerId="9" outerCode="salesmate" outerType="Role" /></sf:performers></bpmn2:extensionElements><bpmn2:incoming>Flow_5356</bpmn2:incoming><bpmn2:outgoing>Flow_9263</bpmn2:outgoing></bpmn2:task><bpmn2:sequenceFlow id="Flow_5825" sf:guid="5432de95-cbcd-4349-9cf0-7e67904c52aa" sf:from="3c438212-4863-4ff8-efc9-a9096c4a8230" sf:to="eb833577-abb5-4239-875a-5f2e2fcb6d57" sourceRef="task_5Q1Q82" targetRef="task_HNGPSC" /><bpmn2:sequenceFlow id="Flow_5356" sf:guid="ac609b39-b6eb-4506-c36f-670c5ed53f5c" sf:from="eb833577-abb5-4239-875a-5f2e2fcb6d57" sf:to="cab57060-f433-422a-a66f-4a5ecfafd54e" sourceRef="task_HNGPSC" targetRef="task_9S66UP" /><bpmn2:sequenceFlow id="Flow_9263" sf:guid="2d5c0e7b-1303-48cb-c22b-3cd2b45701e3" sf:from="cab57060-f433-422a-a66f-4a5ecfafd54e" sf:to="b53eb9ab-3af6-41ad-d722-bed946d19792" sourceRef="task_9S66UP" targetRef="endEvent_9IQ4FV" /><bpmn2:sequenceFlow id="Flow_1177" sf:guid="9cf01621-2dd5-474a-8889-cdbe53a0b72e" sf:from="9b78486d-5b8f-4be4-948e-522356e84e79" sf:to="3c438212-4863-4ff8-efc9-a9096c4a8230" sourceRef="startEvent_AJBNOX" targetRef="task_5Q1Q82" /></bpmn2:process><bpmndi:BPMNDiagram id="BPMNDiagram_1"><bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_100"><bpmndi:BPMNShape id="BPMNShape_JQJYFTS_di" bpmnElement="startEvent_AJBNOX"><dc:Bounds height="36" width="36" x="140" y="117" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_QZOBGUN_di" bpmnElement="endEvent_9IQ4FV"><dc:Bounds height="36" width="36" x="820" y="117" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_OASXNTS_di" bpmnElement="task_5Q1Q82"><dc:Bounds height="80" width="100" x="280" y="123" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_QNQGAXQ_di" bpmnElement="task_HNGPSC"><dc:Bounds height="80" width="100" x="450" y="120" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_NUTTRRU_di" bpmnElement="task_9S66UP"><dc:Bounds height="80" width="100" x="640" y="123" /></bpmndi:BPMNShape><bpmndi:BPMNEdge id="Flow_5825_di" bpmnElement="Flow_5825"><di:waypoint x="380" y="163" /><di:waypoint x="450" y="160" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_5356_di" bpmnElement="Flow_5356"><di:waypoint x="550" y="160" /><di:waypoint x="640" y="163" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_9263_di" bpmnElement="Flow_9263"><di:waypoint x="740" y="163" /><di:waypoint x="820" y="135" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_1177_di" bpmnElement="Flow_1177"><di:waypoint x="176" y="135" /><di:waypoint x="280" y="163" /></bpmndi:BPMNEdge></bpmndi:BPMNPlane></bpmndi:BPMNDiagram></bpmn2:definitions>', 0, NULL, NULL, 0, NULL, CAST(0x0000B115015E6D8F AS DateTime), NULL)
INSERT [dbo].[WfProcess] ([ID], [ProcessGUID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1474, N'5cc99496-28d8-4f6f-a214-c9d7ba9a4fcc', N'1', N'PoolProcess_Name_1GP86R', N'PoolProcess_Name_1GP86R_001', 0, NULL, 2, 1475, N'20a59388-34c2-47a9-e79b-edc1b17ac284', NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn2:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL" id="bpmn2-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn2:collaboration id="Collaboration_1vavxl7" sf:guid="2905e10d-0f80-46de-ed8a-c2a4aa51e0db" sf:code="Collaboration_Code_FGH4VA" name="Collaboration_Name_FGH4VA">
    <bpmn2:participant id="Participant_0fmh0ih" sf:guid="20a59388-34c2-47a9-e79b-edc1b17ac284" sf:code="PoolProcess_Code_1GP86R" name="PoolProcess_Name_1GP86R" processRef="Sequence_Code_3993" />
  </bpmn2:collaboration>
  <bpmn2:process id="Sequence_Code_3993" sf:guid="5cc99496-28d8-4f6f-a214-c9d7ba9a4fcc" sf:code="Sequence_Code_3993" name="pool_tesxt_001" isExecutable="true" sf:version="1">
    <bpmn2:startEvent id="StartNode_8532" sf:guid="b03151d8-9629-4dae-a77f-37f9c104b8a7" sf:code="Start" name="Start">
      <bpmn2:outgoing>Flow_3075</bpmn2:outgoing>
    </bpmn2:startEvent>
    <bpmn2:task id="TaskNode_7005" sf:guid="c1d0f4ca-f702-4721-93fa-4c81c65921da" sf:code="task001" name="Task-001">
      <bpmn2:incoming>Flow_3075</bpmn2:incoming>
      <bpmn2:outgoing>Flow_5275</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:task id="TaskNode_8081" sf:guid="b8527083-e1df-4abc-b292-908ba110f03e" sf:code="task002" name="Task-002">
      <bpmn2:incoming>Flow_5275</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1616</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:task id="TaskNode_5151" sf:guid="762745ff-9a06-4b2c-a19a-7cfcc6b0b306" sf:code="task003" name="Task-003">
      <bpmn2:incoming>Flow_1616</bpmn2:incoming>
      <bpmn2:outgoing>Flow_7010</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:endEvent id="EndNode_9009" sf:guid="e1159628-93cc-43d9-83aa-76e9e063398c" sf:code="End" name="End">
      <bpmn2:incoming>Flow_7010</bpmn2:incoming>
    </bpmn2:endEvent>
    <bpmn2:sequenceFlow id="Flow_7010" sf:guid="a8a8d19a-e667-4da3-8907-247e44e5a815" name="" sourceRef="TaskNode_5151" targetRef="EndNode_9009" sf:from="762745ff-9a06-4b2c-a19a-7cfcc6b0b306" sf:to="e1159628-93cc-43d9-83aa-76e9e063398c" />
    <bpmn2:sequenceFlow id="Flow_1616" sf:guid="f36ef4ef-9d50-4834-b20d-f3b617240e0c" name="" sourceRef="TaskNode_8081" targetRef="TaskNode_5151" sf:from="b8527083-e1df-4abc-b292-908ba110f03e" sf:to="762745ff-9a06-4b2c-a19a-7cfcc6b0b306" />
    <bpmn2:sequenceFlow id="Flow_5275" sf:guid="0416b815-3fa7-4cc1-82c6-115b520f707f" name="t-001" sourceRef="TaskNode_7005" targetRef="TaskNode_8081" sf:from="c1d0f4ca-f702-4721-93fa-4c81c65921da" sf:to="b8527083-e1df-4abc-b292-908ba110f03e" />
    <bpmn2:sequenceFlow id="Flow_3075" sf:guid="36ca4e35-4689-48a8-b23e-dab02bbdc39c" name="" sourceRef="StartNode_8532" targetRef="TaskNode_7005" sf:from="b03151d8-9629-4dae-a77f-37f9c104b8a7" sf:to="c1d0f4ca-f702-4721-93fa-4c81c65921da" />
  </bpmn2:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Collaboration_1vavxl7">
      <bpmndi:BPMNShape id="Participant_0fmh0ih_di" bpmnElement="Participant_0fmh0ih" isHorizontal="true">
        <dc:Bounds x="186" y="73" width="770" height="250" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNEdge id="Flow_7010_di" bpmnElement="Flow_7010">
        <di:waypoint x="816" y="198" />
        <di:waypoint x="896" y="198" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1616_di" bpmnElement="Flow_1616">
        <di:waypoint x="636" y="198" />
        <di:waypoint x="716" y="198" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_5275_di" bpmnElement="Flow_5275">
        <di:waypoint x="456" y="198" />
        <di:waypoint x="536" y="198" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="483" y="173" width="27" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_3075_di" bpmnElement="Flow_3075">
        <di:waypoint x="276" y="198" />
        <di:waypoint x="356" y="198" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNShape id="BPMNShape_y8elhfu_di" bpmnElement="StartNode_8532">
        <dc:Bounds x="240" y="180" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="246" y="216" width="25" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="BPMNShape_8yj8of6_di" bpmnElement="TaskNode_7005">
        <dc:Bounds x="356" y="158" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="BPMNShape_uj5lg7o_di" bpmnElement="TaskNode_8081">
        <dc:Bounds x="536" y="158" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="BPMNShape_hphi9uq_di" bpmnElement="TaskNode_5151">
        <dc:Bounds x="716" y="158" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="BPMNShape_lgeckr9_di" bpmnElement="EndNode_9009">
        <dc:Bounds x="896" y="180" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="904" y="216" width="20" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn2:definitions>
', 0, NULL, NULL, 0, NULL, CAST(0x0000B19B00EA3711 AS DateTime), CAST(0x0000B19B00EA532C AS DateTime))
INSERT [dbo].[WfProcess] ([ID], [ProcessGUID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1475, N'2905e10d-0f80-46de-ed8a-c2a4aa51e0db', N'1', N'Collaboration_Name_FGH4VA', N'Collaboration_Name_FGH4VA_001', 1, NULL, 1, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn2:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL" id="bpmn2-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn2:collaboration id="Collaboration_1vavxl7" sf:guid="2905e10d-0f80-46de-ed8a-c2a4aa51e0db" sf:code="Collaboration_Code_FGH4VA" name="Collaboration_Name_FGH4VA">
    <bpmn2:participant id="Participant_0fmh0ih" sf:guid="20a59388-34c2-47a9-e79b-edc1b17ac284" sf:code="PoolProcess_Code_1GP86R" name="PoolProcess_Name_1GP86R" processRef="Sequence_Code_3993" />
  </bpmn2:collaboration>
  <bpmn2:process id="Sequence_Code_3993" sf:guid="5cc99496-28d8-4f6f-a214-c9d7ba9a4fcc" sf:code="Sequence_Code_3993" name="pool_tesxt_001" isExecutable="true" sf:version="1">
    <bpmn2:startEvent id="StartNode_8532" sf:guid="b03151d8-9629-4dae-a77f-37f9c104b8a7" sf:code="Start" name="Start">
      <bpmn2:outgoing>Flow_3075</bpmn2:outgoing>
    </bpmn2:startEvent>
    <bpmn2:task id="TaskNode_7005" sf:guid="c1d0f4ca-f702-4721-93fa-4c81c65921da" sf:code="task001" name="Task-001">
      <bpmn2:incoming>Flow_3075</bpmn2:incoming>
      <bpmn2:outgoing>Flow_5275</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:task id="TaskNode_8081" sf:guid="b8527083-e1df-4abc-b292-908ba110f03e" sf:code="task002" name="Task-002">
      <bpmn2:incoming>Flow_5275</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1616</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:task id="TaskNode_5151" sf:guid="762745ff-9a06-4b2c-a19a-7cfcc6b0b306" sf:code="task003" name="Task-003">
      <bpmn2:incoming>Flow_1616</bpmn2:incoming>
      <bpmn2:outgoing>Flow_7010</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:endEvent id="EndNode_9009" sf:guid="e1159628-93cc-43d9-83aa-76e9e063398c" sf:code="End" name="End">
      <bpmn2:incoming>Flow_7010</bpmn2:incoming>
    </bpmn2:endEvent>
    <bpmn2:sequenceFlow id="Flow_7010" sf:guid="a8a8d19a-e667-4da3-8907-247e44e5a815" name="" sourceRef="TaskNode_5151" targetRef="EndNode_9009" sf:from="762745ff-9a06-4b2c-a19a-7cfcc6b0b306" sf:to="e1159628-93cc-43d9-83aa-76e9e063398c" />
    <bpmn2:sequenceFlow id="Flow_1616" sf:guid="f36ef4ef-9d50-4834-b20d-f3b617240e0c" name="" sourceRef="TaskNode_8081" targetRef="TaskNode_5151" sf:from="b8527083-e1df-4abc-b292-908ba110f03e" sf:to="762745ff-9a06-4b2c-a19a-7cfcc6b0b306" />
    <bpmn2:sequenceFlow id="Flow_5275" sf:guid="0416b815-3fa7-4cc1-82c6-115b520f707f" name="t-001" sourceRef="TaskNode_7005" targetRef="TaskNode_8081" sf:from="c1d0f4ca-f702-4721-93fa-4c81c65921da" sf:to="b8527083-e1df-4abc-b292-908ba110f03e" />
    <bpmn2:sequenceFlow id="Flow_3075" sf:guid="36ca4e35-4689-48a8-b23e-dab02bbdc39c" name="" sourceRef="StartNode_8532" targetRef="TaskNode_7005" sf:from="b03151d8-9629-4dae-a77f-37f9c104b8a7" sf:to="c1d0f4ca-f702-4721-93fa-4c81c65921da" />
  </bpmn2:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Collaboration_1vavxl7">
      <bpmndi:BPMNShape id="Participant_0fmh0ih_di" bpmnElement="Participant_0fmh0ih" isHorizontal="true">
        <dc:Bounds x="186" y="73" width="770" height="250" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNEdge id="Flow_7010_di" bpmnElement="Flow_7010">
        <di:waypoint x="816" y="198" />
        <di:waypoint x="896" y="198" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1616_di" bpmnElement="Flow_1616">
        <di:waypoint x="636" y="198" />
        <di:waypoint x="716" y="198" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_5275_di" bpmnElement="Flow_5275">
        <di:waypoint x="456" y="198" />
        <di:waypoint x="536" y="198" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="483" y="173" width="27" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_3075_di" bpmnElement="Flow_3075">
        <di:waypoint x="276" y="198" />
        <di:waypoint x="356" y="198" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNShape id="BPMNShape_y8elhfu_di" bpmnElement="StartNode_8532">
        <dc:Bounds x="240" y="180" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="246" y="216" width="25" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="BPMNShape_8yj8of6_di" bpmnElement="TaskNode_7005">
        <dc:Bounds x="356" y="158" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="BPMNShape_uj5lg7o_di" bpmnElement="TaskNode_8081">
        <dc:Bounds x="536" y="158" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="BPMNShape_hphi9uq_di" bpmnElement="TaskNode_5151">
        <dc:Bounds x="716" y="158" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="BPMNShape_lgeckr9_di" bpmnElement="EndNode_9009">
        <dc:Bounds x="896" y="180" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="904" y="216" width="20" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn2:definitions>
', 0, NULL, NULL, 0, NULL, CAST(0x0000B19B00EA532C AS DateTime), NULL)
INSERT [dbo].[WfProcess] ([ID], [ProcessGUID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1476, N'36681b77-710e-4816-adb4-bf9f9990309e', N'1', N'PoolProcess_Name_RAAENQ', N'PoolProcess_Name_RAAENQ_001', 0, NULL, 2, 1480, N'63104654-2484-4265-de97-a6f7c64e04e9', NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn2:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL" id="bpmn2-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn2:collaboration id="Collaboration_05s33px" sf:guid="86f4606c-971e-4552-a4e1-ab288a1507ab" sf:code="Collaboration_Code_R41AFI" name="MainProcess_Name_R41AFI">
    <bpmn2:participant id="Participant_1j3cwd9" sf:guid="63104654-2484-4265-de97-a6f7c64e04e9" sf:code="PoolProcess_Code_RAAENQ" name="PoolProcess_Name_RAAENQ" processRef="Sequence_Code_7275" />
  </bpmn2:collaboration>
  <bpmn2:process id="Sequence_Code_7275" sf:guid="36681b77-710e-4816-adb4-bf9f9990309e" sf:code="Sequence_Code_7275" name="Sequence_7275" isExecutable="true" sf:version="1">
    <bpmn2:startEvent id="StartNode_3091" sf:guid="c6897ca2-a7d8-4bc9-a944-5934cb13efcd" sf:code="Start" name="Start">
      <bpmn2:outgoing>Flow_9846</bpmn2:outgoing>
    </bpmn2:startEvent>
    <bpmn2:task id="TaskNode_5567" sf:guid="780c0d64-0178-401f-9c7b-3f0e2869ef14" sf:code="task001" name="Task-001">
      <bpmn2:incoming>Flow_9846</bpmn2:incoming>
      <bpmn2:outgoing>Flow_4301</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:task id="TaskNode_9923" sf:guid="1a2c0993-469d-4769-8208-1e6d7e5de7ae" sf:code="task002" name="Task-002">
      <bpmn2:incoming>Flow_4301</bpmn2:incoming>
      <bpmn2:outgoing>Flow_3281</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:task id="TaskNode_8839" sf:guid="35ee5b9b-1630-4307-bd18-197a7b98acd9" sf:code="task003" name="Task-003">
      <bpmn2:incoming>Flow_3281</bpmn2:incoming>
      <bpmn2:outgoing>Flow_9785</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:endEvent id="EndNode_8248" sf:guid="3fbd59e2-05f7-42ef-bc6b-abfaccf62926" sf:code="End" name="End">
      <bpmn2:incoming>Flow_9785</bpmn2:incoming>
    </bpmn2:endEvent>
    <bpmn2:sequenceFlow id="Flow_9785" sf:guid="3b010ad8-21f1-41ad-8a9e-08c2e80b53c3" name="" sourceRef="TaskNode_8839" targetRef="EndNode_8248" sf:from="35ee5b9b-1630-4307-bd18-197a7b98acd9" sf:to="3fbd59e2-05f7-42ef-bc6b-abfaccf62926" />
    <bpmn2:sequenceFlow id="Flow_3281" sf:guid="f310a6e8-c3d2-4a62-a85a-9aba6a9da0d7" name="" sourceRef="TaskNode_9923" targetRef="TaskNode_8839" sf:from="1a2c0993-469d-4769-8208-1e6d7e5de7ae" sf:to="35ee5b9b-1630-4307-bd18-197a7b98acd9" />
    <bpmn2:sequenceFlow id="Flow_4301" sf:guid="e5d3d2b5-5fb6-41a0-b501-c0f97b3bb801" name="t-001" sourceRef="TaskNode_5567" targetRef="TaskNode_9923" sf:from="780c0d64-0178-401f-9c7b-3f0e2869ef14" sf:to="1a2c0993-469d-4769-8208-1e6d7e5de7ae" />
    <bpmn2:sequenceFlow id="Flow_9846" sf:guid="b2d0895e-5f7f-4759-b1db-7797d15ce2ad" name="" sourceRef="StartNode_3091" targetRef="TaskNode_5567" sf:from="c6897ca2-a7d8-4bc9-a944-5934cb13efcd" sf:to="780c0d64-0178-401f-9c7b-3f0e2869ef14" />
  </bpmn2:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Collaboration_05s33px">
      <bpmndi:BPMNShape id="Participant_1j3cwd9_di" bpmnElement="Participant_1j3cwd9" isHorizontal="true">
        <dc:Bounds x="186" y="73" width="770" height="250" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNEdge id="Flow_9846_di" bpmnElement="Flow_9846">
        <di:waypoint x="276" y="198" />
        <di:waypoint x="356" y="198" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_4301_di" bpmnElement="Flow_4301">
        <di:waypoint x="456" y="198" />
        <di:waypoint x="536" y="198" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="483" y="173" width="27" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_3281_di" bpmnElement="Flow_3281">
        <di:waypoint x="636" y="198" />
        <di:waypoint x="716" y="198" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_9785_di" bpmnElement="Flow_9785">
        <di:waypoint x="816" y="198" />
        <di:waypoint x="896" y="198" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNShape id="BPMNShape_r76j3zh_di" bpmnElement="StartNode_3091">
        <dc:Bounds x="240" y="180" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="246" y="216" width="25" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="BPMNShape_t5yslhk_di" bpmnElement="TaskNode_5567">
        <dc:Bounds x="356" y="158" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="BPMNShape_k7afm68_di" bpmnElement="TaskNode_9923">
        <dc:Bounds x="536" y="158" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="BPMNShape_z7z2n5b_di" bpmnElement="TaskNode_8839">
        <dc:Bounds x="716" y="158" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="BPMNShape_l686zch_di" bpmnElement="EndNode_8248">
        <dc:Bounds x="896" y="180" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="904" y="216" width="20" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn2:definitions>
', 0, NULL, NULL, 0, NULL, CAST(0x0000B19B00EBA37E AS DateTime), CAST(0x0000B19B00F6653C AS DateTime))
INSERT [dbo].[WfProcess] ([ID], [ProcessGUID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1480, N'86f4606c-971e-4552-a4e1-ab288a1507ab', N'1', N'MainProcess_Name_R41AFI', N'Collaboration_Name_R41AFI_001', 1, NULL, 1, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn2:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL" id="bpmn2-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn2:collaboration id="Collaboration_05s33px" sf:guid="86f4606c-971e-4552-a4e1-ab288a1507ab" sf:code="Collaboration_Code_R41AFI" name="MainProcess_Name_R41AFI">
    <bpmn2:participant id="Participant_1j3cwd9" sf:guid="63104654-2484-4265-de97-a6f7c64e04e9" sf:code="PoolProcess_Code_RAAENQ" name="PoolProcess_Name_RAAENQ" processRef="Sequence_Code_7275" />
  </bpmn2:collaboration>
  <bpmn2:process id="Sequence_Code_7275" sf:guid="36681b77-710e-4816-adb4-bf9f9990309e" sf:code="Sequence_Code_7275" name="Sequence_7275" isExecutable="true" sf:version="1">
    <bpmn2:startEvent id="StartNode_3091" sf:guid="c6897ca2-a7d8-4bc9-a944-5934cb13efcd" sf:code="Start" name="Start">
      <bpmn2:outgoing>Flow_9846</bpmn2:outgoing>
    </bpmn2:startEvent>
    <bpmn2:task id="TaskNode_5567" sf:guid="780c0d64-0178-401f-9c7b-3f0e2869ef14" sf:code="task001" name="Task-001">
      <bpmn2:incoming>Flow_9846</bpmn2:incoming>
      <bpmn2:outgoing>Flow_4301</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:task id="TaskNode_9923" sf:guid="1a2c0993-469d-4769-8208-1e6d7e5de7ae" sf:code="task002" name="Task-002">
      <bpmn2:incoming>Flow_4301</bpmn2:incoming>
      <bpmn2:outgoing>Flow_3281</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:task id="TaskNode_8839" sf:guid="35ee5b9b-1630-4307-bd18-197a7b98acd9" sf:code="task003" name="Task-003">
      <bpmn2:incoming>Flow_3281</bpmn2:incoming>
      <bpmn2:outgoing>Flow_9785</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:endEvent id="EndNode_8248" sf:guid="3fbd59e2-05f7-42ef-bc6b-abfaccf62926" sf:code="End" name="End">
      <bpmn2:incoming>Flow_9785</bpmn2:incoming>
    </bpmn2:endEvent>
    <bpmn2:sequenceFlow id="Flow_9785" sf:guid="3b010ad8-21f1-41ad-8a9e-08c2e80b53c3" name="" sourceRef="TaskNode_8839" targetRef="EndNode_8248" sf:from="35ee5b9b-1630-4307-bd18-197a7b98acd9" sf:to="3fbd59e2-05f7-42ef-bc6b-abfaccf62926" />
    <bpmn2:sequenceFlow id="Flow_3281" sf:guid="f310a6e8-c3d2-4a62-a85a-9aba6a9da0d7" name="" sourceRef="TaskNode_9923" targetRef="TaskNode_8839" sf:from="1a2c0993-469d-4769-8208-1e6d7e5de7ae" sf:to="35ee5b9b-1630-4307-bd18-197a7b98acd9" />
    <bpmn2:sequenceFlow id="Flow_4301" sf:guid="e5d3d2b5-5fb6-41a0-b501-c0f97b3bb801" name="t-001" sourceRef="TaskNode_5567" targetRef="TaskNode_9923" sf:from="780c0d64-0178-401f-9c7b-3f0e2869ef14" sf:to="1a2c0993-469d-4769-8208-1e6d7e5de7ae" />
    <bpmn2:sequenceFlow id="Flow_9846" sf:guid="b2d0895e-5f7f-4759-b1db-7797d15ce2ad" name="" sourceRef="StartNode_3091" targetRef="TaskNode_5567" sf:from="c6897ca2-a7d8-4bc9-a944-5934cb13efcd" sf:to="780c0d64-0178-401f-9c7b-3f0e2869ef14" />
  </bpmn2:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Collaboration_05s33px">
      <bpmndi:BPMNShape id="Participant_1j3cwd9_di" bpmnElement="Participant_1j3cwd9" isHorizontal="true">
        <dc:Bounds x="186" y="73" width="770" height="250" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNEdge id="Flow_9846_di" bpmnElement="Flow_9846">
        <di:waypoint x="276" y="198" />
        <di:waypoint x="356" y="198" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_4301_di" bpmnElement="Flow_4301">
        <di:waypoint x="456" y="198" />
        <di:waypoint x="536" y="198" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="483" y="173" width="27" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_3281_di" bpmnElement="Flow_3281">
        <di:waypoint x="636" y="198" />
        <di:waypoint x="716" y="198" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_9785_di" bpmnElement="Flow_9785">
        <di:waypoint x="816" y="198" />
        <di:waypoint x="896" y="198" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNShape id="BPMNShape_r76j3zh_di" bpmnElement="StartNode_3091">
        <dc:Bounds x="240" y="180" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="246" y="216" width="25" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="BPMNShape_t5yslhk_di" bpmnElement="TaskNode_5567">
        <dc:Bounds x="356" y="158" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="BPMNShape_k7afm68_di" bpmnElement="TaskNode_9923">
        <dc:Bounds x="536" y="158" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="BPMNShape_z7z2n5b_di" bpmnElement="TaskNode_8839">
        <dc:Bounds x="716" y="158" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="BPMNShape_l686zch_di" bpmnElement="EndNode_8248">
        <dc:Bounds x="896" y="180" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="904" y="216" width="20" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn2:definitions>
', 0, NULL, NULL, 0, NULL, CAST(0x0000B19B00F527D8 AS DateTime), CAST(0x0000B19B00F6652D AS DateTime))
INSERT [dbo].[WfProcess] ([ID], [ProcessGUID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1481, N'8ab71b4a-5011-485d-d685-1836f8ced3b6', N'1', N'LaneSetTestProcess_L0EWGT', N'Collaboration_Name_L0EWGT_001', 1, NULL, 1, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn2:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL" id="bpmn2-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn2:collaboration id="Collaboration_1nlgink" sf:guid="8ab71b4a-5011-485d-d685-1836f8ced3b6" sf:code="Collaboration_Code_L0EWGT" name="LaneSetTestProcess_L0EWGT">
    <bpmn2:participant id="Participant_1n4i5ug" sf:guid="2777febd-acd1-4952-954a-74c1ce856888" sf:code="PoolProcess_Code_IDWHIK" name="PoolProcess_Name_IDWHIK" processRef="pProcess_Code_9606" />
  </bpmn2:collaboration>
  <bpmn2:process id="pProcess_Code_9606" sf:guid="f95f06ce-8eae-499c-9c9a-74167f15e622" sf:code="Process_Code_9606" name="Process_Name_9606" isExecutable="true" sf:version="1">
    <bpmn2:laneSet id="LaneSet_1yl2m57">
      <bpmn2:lane id="Lane_1gzyoib" sf:guid="8e7493a0-b2ee-4916-8089-71dfe02629fa">
        <bpmn2:flowNodeRef>StartEvent_1</bpmn2:flowNodeRef>
        <bpmn2:flowNodeRef>Activity_0uiyesc</bpmn2:flowNodeRef>
      </bpmn2:lane>
      <bpmn2:lane id="Lane_08jywig" sf:guid="dd79b108-055c-4185-f23e-e0d85b16028e">
        <bpmn2:flowNodeRef>Activity_0plhd7i</bpmn2:flowNodeRef>
        <bpmn2:flowNodeRef>Event_1y6lqar</bpmn2:flowNodeRef>
      </bpmn2:lane>
    </bpmn2:laneSet>
    <bpmn2:startEvent id="StartEvent_1" sf:guid="383bce20-24c1-4917-8f35-16cd2c74553b" name="Start">
      <bpmn2:outgoing>Flow_15llq5b</bpmn2:outgoing>
    </bpmn2:startEvent>
    <bpmn2:task id="Activity_0uiyesc" sf:guid="8b0f1486-2f9e-45e0-9450-fa4c3a058a15" name="a">
      <bpmn2:incoming>Flow_15llq5b</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1ivi1oy</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_15llq5b" sf:guid="6bd94cea-4017-4f74-f1e8-210c4e381c49" sourceRef="StartEvent_1" targetRef="Activity_0uiyesc" sf:from="383bce20-24c1-4917-8f35-16cd2c74553b" sf:to="8b0f1486-2f9e-45e0-9450-fa4c3a058a15" />
    <bpmn2:task id="Activity_0plhd7i" sf:guid="90fa1499-b962-4024-9974-b7398cc5bbe4" name="b">
      <bpmn2:incoming>Flow_1ivi1oy</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0ozpu9f</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:endEvent id="Event_1y6lqar" sf:guid="a2214847-7d0f-4efe-c2eb-faf508314c50">
      <bpmn2:incoming>Flow_0ozpu9f</bpmn2:incoming>
    </bpmn2:endEvent>
    <bpmn2:sequenceFlow id="Flow_0ozpu9f" sf:guid="691accdf-f367-4615-aca9-fe89367fb24b" sourceRef="Activity_0plhd7i" targetRef="Event_1y6lqar" sf:from="90fa1499-b962-4024-9974-b7398cc5bbe4" sf:to="a2214847-7d0f-4efe-c2eb-faf508314c50" />
    <bpmn2:sequenceFlow id="Flow_1ivi1oy" sf:guid="b7a0b482-6b78-444e-d942-bd038556c1c0" sourceRef="Activity_0uiyesc" targetRef="Activity_0plhd7i" sf:from="8b0f1486-2f9e-45e0-9450-fa4c3a058a15" sf:to="90fa1499-b962-4024-9974-b7398cc5bbe4" />
  </bpmn2:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Collaboration_1nlgink">
      <bpmndi:BPMNShape id="Participant_1n4i5ug_di" bpmnElement="Participant_1n4i5ug" isHorizontal="true">
        <dc:Bounds x="330" y="140" width="600" height="370" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Lane_1gzyoib_di" bpmnElement="Lane_1gzyoib" isHorizontal="true">
        <dc:Bounds x="360" y="140" width="570" height="250" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Lane_08jywig_di" bpmnElement="Lane_08jywig" isHorizontal="true">
        <dc:Bounds x="360" y="390" width="570" height="120" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNEdge id="Flow_15llq5b_di" bpmnElement="Flow_15llq5b">
        <di:waypoint x="448" y="258" />
        <di:waypoint x="500" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0ozpu9f_di" bpmnElement="Flow_0ozpu9f">
        <di:waypoint x="670" y="450" />
        <di:waypoint x="722" y="450" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1ivi1oy_di" bpmnElement="Flow_1ivi1oy">
        <di:waypoint x="550" y="298" />
        <di:waypoint x="550" y="354" />
        <di:waypoint x="620" y="354" />
        <di:waypoint x="620" y="410" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="412" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="418" y="283" width="25" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0uiyesc_di" bpmnElement="Activity_0uiyesc">
        <dc:Bounds x="500" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0plhd7i_di" bpmnElement="Activity_0plhd7i">
        <dc:Bounds x="570" y="410" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_1y6lqar_di" bpmnElement="Event_1y6lqar">
        <dc:Bounds x="722" y="432" width="36" height="36" />
      </bpmndi:BPMNShape>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn2:definitions>
', 0, NULL, NULL, 0, NULL, CAST(0x0000B1A7010B5428 AS DateTime), CAST(0x0000B1A7010B764F AS DateTime))
INSERT [dbo].[WfProcess] ([ID], [ProcessGUID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1482, N'f95f06ce-8eae-499c-9c9a-74167f15e622', N'1', N'PoolProcess_Name_IDWHIK', N'PoolProcess_Name_IDWHIK_001', 1, NULL, 2, 1481, N'2777febd-acd1-4952-954a-74c1ce856888', NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn2:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL" id="bpmn2-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn2:collaboration id="Collaboration_1nlgink" sf:guid="8ab71b4a-5011-485d-d685-1836f8ced3b6" sf:code="Collaboration_Code_L0EWGT" name="LaneSetTestProcess_L0EWGT">
    <bpmn2:participant id="Participant_1n4i5ug" sf:guid="2777febd-acd1-4952-954a-74c1ce856888" sf:code="PoolProcess_Code_IDWHIK" name="PoolProcess_Name_IDWHIK" processRef="pProcess_Code_9606" />
  </bpmn2:collaboration>
  <bpmn2:process id="pProcess_Code_9606" sf:guid="f95f06ce-8eae-499c-9c9a-74167f15e622" sf:code="Process_Code_9606" name="Process_Name_9606" isExecutable="true" sf:version="1">
    <bpmn2:laneSet id="LaneSet_1yl2m57">
      <bpmn2:lane id="Lane_1gzyoib" sf:guid="8e7493a0-b2ee-4916-8089-71dfe02629fa">
        <bpmn2:flowNodeRef>StartEvent_1</bpmn2:flowNodeRef>
        <bpmn2:flowNodeRef>Activity_0uiyesc</bpmn2:flowNodeRef>
      </bpmn2:lane>
      <bpmn2:lane id="Lane_08jywig" sf:guid="dd79b108-055c-4185-f23e-e0d85b16028e">
        <bpmn2:flowNodeRef>Activity_0plhd7i</bpmn2:flowNodeRef>
        <bpmn2:flowNodeRef>Event_1y6lqar</bpmn2:flowNodeRef>
      </bpmn2:lane>
    </bpmn2:laneSet>
    <bpmn2:startEvent id="StartEvent_1" sf:guid="383bce20-24c1-4917-8f35-16cd2c74553b" name="Start">
      <bpmn2:outgoing>Flow_15llq5b</bpmn2:outgoing>
    </bpmn2:startEvent>
    <bpmn2:task id="Activity_0uiyesc" sf:guid="8b0f1486-2f9e-45e0-9450-fa4c3a058a15" name="a">
      <bpmn2:incoming>Flow_15llq5b</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1ivi1oy</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_15llq5b" sf:guid="6bd94cea-4017-4f74-f1e8-210c4e381c49" sourceRef="StartEvent_1" targetRef="Activity_0uiyesc" sf:from="383bce20-24c1-4917-8f35-16cd2c74553b" sf:to="8b0f1486-2f9e-45e0-9450-fa4c3a058a15" />
    <bpmn2:task id="Activity_0plhd7i" sf:guid="90fa1499-b962-4024-9974-b7398cc5bbe4" name="b">
      <bpmn2:incoming>Flow_1ivi1oy</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0ozpu9f</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:endEvent id="Event_1y6lqar" sf:guid="a2214847-7d0f-4efe-c2eb-faf508314c50">
      <bpmn2:incoming>Flow_0ozpu9f</bpmn2:incoming>
    </bpmn2:endEvent>
    <bpmn2:sequenceFlow id="Flow_0ozpu9f" sf:guid="691accdf-f367-4615-aca9-fe89367fb24b" sourceRef="Activity_0plhd7i" targetRef="Event_1y6lqar" sf:from="90fa1499-b962-4024-9974-b7398cc5bbe4" sf:to="a2214847-7d0f-4efe-c2eb-faf508314c50" />
    <bpmn2:sequenceFlow id="Flow_1ivi1oy" sf:guid="b7a0b482-6b78-444e-d942-bd038556c1c0" sourceRef="Activity_0uiyesc" targetRef="Activity_0plhd7i" sf:from="8b0f1486-2f9e-45e0-9450-fa4c3a058a15" sf:to="90fa1499-b962-4024-9974-b7398cc5bbe4" />
  </bpmn2:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Collaboration_1nlgink">
      <bpmndi:BPMNShape id="Participant_1n4i5ug_di" bpmnElement="Participant_1n4i5ug" isHorizontal="true">
        <dc:Bounds x="330" y="140" width="600" height="370" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Lane_1gzyoib_di" bpmnElement="Lane_1gzyoib" isHorizontal="true">
        <dc:Bounds x="360" y="140" width="570" height="250" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Lane_08jywig_di" bpmnElement="Lane_08jywig" isHorizontal="true">
        <dc:Bounds x="360" y="390" width="570" height="120" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNEdge id="Flow_15llq5b_di" bpmnElement="Flow_15llq5b">
        <di:waypoint x="448" y="258" />
        <di:waypoint x="500" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0ozpu9f_di" bpmnElement="Flow_0ozpu9f">
        <di:waypoint x="670" y="450" />
        <di:waypoint x="722" y="450" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1ivi1oy_di" bpmnElement="Flow_1ivi1oy">
        <di:waypoint x="550" y="298" />
        <di:waypoint x="550" y="354" />
        <di:waypoint x="620" y="354" />
        <di:waypoint x="620" y="410" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="412" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="418" y="283" width="25" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0uiyesc_di" bpmnElement="Activity_0uiyesc">
        <dc:Bounds x="500" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0plhd7i_di" bpmnElement="Activity_0plhd7i">
        <dc:Bounds x="570" y="410" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_1y6lqar_di" bpmnElement="Event_1y6lqar">
        <dc:Bounds x="722" y="432" width="36" height="36" />
      </bpmndi:BPMNShape>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn2:definitions>
', 0, NULL, NULL, 0, NULL, CAST(0x0000B1A7010B5454 AS DateTime), CAST(0x0000B1A7010B7657 AS DateTime))
INSERT [dbo].[WfProcess] ([ID], [ProcessGUID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1483, N'fdad2829-7449-4840-b6f7-1104b19972d5', N'1', N'Process_Name_3023', N'Process_Code_3023', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn2:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL" id="bpmn2-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn2:process id="pProcess_Code_3023" sf:guid="fdad2829-7449-4840-b6f7-1104b19972d5" sf:code="Process_Code_3023" name="Process_Name_3023" isExecutable="true" sf:version="1">
    <bpmn2:startEvent id="StartEvent_1" sf:guid="bbcb1b2d-13d2-4d36-9332-ab2789eb4431" name="Start">
      <bpmn2:outgoing>Flow_0zn9cpu</bpmn2:outgoing>
      <bpmn2:signalEventDefinition id="SignalEventDefinition_08vuqb4" />
    </bpmn2:startEvent>
    <bpmn2:task id="Activity_1tqa3bb" sf:guid="129b1747-dc42-4909-af59-34c118491504" name="task-01">
      <bpmn2:incoming>Flow_0zn9cpu</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0up2935</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_0zn9cpu" sf:guid="df6f5541-87db-44fd-8979-bf593c09ece7" sourceRef="StartEvent_1" targetRef="Activity_1tqa3bb" sf:from="bbcb1b2d-13d2-4d36-9332-ab2789eb4431" sf:to="129b1747-dc42-4909-af59-34c118491504" />
    <bpmn2:task id="Activity_138bcms" sf:guid="805e9d77-bb4e-483e-8268-10e592bf9339" name="task-02">
      <bpmn2:incoming>Flow_0up2935</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0zl8tz2</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_0up2935" sf:guid="b3ca5f07-2535-44ab-b31d-71c590ad8ca2" sourceRef="Activity_1tqa3bb" targetRef="Activity_138bcms" sf:from="129b1747-dc42-4909-af59-34c118491504" sf:to="805e9d77-bb4e-483e-8268-10e592bf9339" />
    <bpmn2:endEvent id="Event_0rkxz33" sf:guid="fadcf617-6c39-483a-8b2a-5d5e7eb677a3">
      <bpmn2:incoming>Flow_0zl8tz2</bpmn2:incoming>
    </bpmn2:endEvent>
    <bpmn2:sequenceFlow id="Flow_0zl8tz2" sf:guid="4ca9684d-09b5-4f4e-9c0e-9e9c9103cdac" sourceRef="Activity_138bcms" targetRef="Event_0rkxz33" sf:from="805e9d77-bb4e-483e-8268-10e592bf9339" sf:to="fadcf617-6c39-483a-8b2a-5d5e7eb677a3" />
  </bpmn2:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="pProcess_Code_3023">
      <bpmndi:BPMNEdge id="Flow_0zn9cpu_di" bpmnElement="Flow_0zn9cpu">
        <di:waypoint x="448" y="258" />
        <di:waypoint x="500" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0up2935_di" bpmnElement="Flow_0up2935">
        <di:waypoint x="600" y="258" />
        <di:waypoint x="660" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0zl8tz2_di" bpmnElement="Flow_0zl8tz2">
        <di:waypoint x="760" y="258" />
        <di:waypoint x="822" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNShape id="Event_0a4zzgr_di" bpmnElement="StartEvent_1">
        <dc:Bounds x="412" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="418" y="283" width="25" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1tqa3bb_di" bpmnElement="Activity_1tqa3bb">
        <dc:Bounds x="500" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_138bcms_di" bpmnElement="Activity_138bcms">
        <dc:Bounds x="660" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_0rkxz33_di" bpmnElement="Event_0rkxz33">
        <dc:Bounds x="822" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn2:definitions>
                                                                                                                                                                                                                                                                       ', 0, NULL, NULL, 0, NULL, CAST(0x0000B1F000B1F113 AS DateTime), NULL)
INSERT [dbo].[WfProcess] ([ID], [ProcessGUID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1484, N'2a7a60a3-f270-429f-98b6-e3c30c60b6e5', N'1', N'SignalStartProcess', N'Process_Code_4293', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn2:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL" id="bpmn2-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn2:process id="pProcess_Code_4293" sf:guid="2a7a60a3-f270-429f-98b6-e3c30c60b6e5" sf:code="Process_Code_4293" name="SignalStartProcess" isExecutable="true" sf:version="1">
    <bpmn2:task id="Activity_0bpvc55" sf:guid="65df7259-0964-4a32-fc38-baf6c8efaf21" name="Making Sample">
      <bpmn2:incoming>Flow_1fg1gtd</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1x6oxax</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_1fg1gtd" sf:guid="4898b693-8228-4e4a-925b-5f9c7f3fe496" sourceRef="StartEvent_1" targetRef="Activity_0bpvc55" sf:from="9cbf4daf-0c9b-42d2-9773-bbeb1b45227b" sf:to="65df7259-0964-4a32-fc38-baf6c8efaf21" />
    <bpmn2:task id="Activity_1vgjjii" sf:guid="b5552174-b958-4cc3-903d-07fca3163737" name="Planning">
      <bpmn2:incoming>Flow_1x6oxax</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1waakbp</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_1x6oxax" sf:guid="aa735425-b180-46a9-9b91-d069542639ee" sourceRef="Activity_0bpvc55" targetRef="Activity_1vgjjii" sf:from="65df7259-0964-4a32-fc38-baf6c8efaf21" sf:to="b5552174-b958-4cc3-903d-07fca3163737" />
    <bpmn2:endEvent id="Event_0cgdsot" sf:guid="7c450a7f-ea0a-439c-f3d1-1f1d09320730" name="End">
      <bpmn2:incoming>Flow_1kk81eg</bpmn2:incoming>
    </bpmn2:endEvent>
    <bpmn2:startEvent id="StartEvent_1" sf:guid="9cbf4daf-0c9b-42d2-9773-bbeb1b45227b" name="Start">
      <bpmn2:outgoing>Flow_1fg1gtd</bpmn2:outgoing>
      <bpmn2:signalEventDefinition id="SignalEventDefinition_1ik52yf" signalRef="Siganl_7H351S" />
    </bpmn2:startEvent>
    <bpmn2:task id="Activity_1c1rdcy" sf:guid="8f0bdb86-eca9-4acf-8daf-744bb53d7dae" name="Delivery">
      <bpmn2:incoming>Flow_1waakbp</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1kk81eg</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_1waakbp" sf:guid="6a381bd0-bc4d-44b6-98bf-ebc531b51378" sourceRef="Activity_1vgjjii" targetRef="Activity_1c1rdcy" sf:from="b5552174-b958-4cc3-903d-07fca3163737" sf:to="8f0bdb86-eca9-4acf-8daf-744bb53d7dae" />
    <bpmn2:sequenceFlow id="Flow_1kk81eg" sf:guid="bf2135bd-9ca8-4f58-8d7f-4f0a607c4c64" sourceRef="Activity_1c1rdcy" targetRef="Event_0cgdsot" sf:from="8f0bdb86-eca9-4acf-8daf-744bb53d7dae" sf:to="7c450a7f-ea0a-439c-f3d1-1f1d09320730" />
  </bpmn2:process>
  <bpmn2:signal id="Siganl_7H351S" name="OrderDistributed" />
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="pProcess_Code_4293">
      <bpmndi:BPMNEdge id="Flow_1kk81eg_di" bpmnElement="Flow_1kk81eg">
        <di:waypoint x="950" y="258" />
        <di:waypoint x="1042" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1waakbp_di" bpmnElement="Flow_1waakbp">
        <di:waypoint x="780" y="258" />
        <di:waypoint x="850" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1x6oxax_di" bpmnElement="Flow_1x6oxax">
        <di:waypoint x="600" y="258" />
        <di:waypoint x="680" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1fg1gtd_di" bpmnElement="Flow_1fg1gtd">
        <di:waypoint x="398" y="258" />
        <di:waypoint x="500" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNShape id="Activity_0bpvc55_di" bpmnElement="Activity_0bpvc55">
        <dc:Bounds x="500" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1vgjjii_di" bpmnElement="Activity_1vgjjii">
        <dc:Bounds x="680" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_0cgdsot_di" bpmnElement="Event_0cgdsot">
        <dc:Bounds x="1042" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="1050" y="283" width="20" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_1m3pqcf_di" bpmnElement="StartEvent_1">
        <dc:Bounds x="362" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="368" y="283" width="25" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1c1rdcy_di" bpmnElement="Activity_1c1rdcy">
        <dc:Bounds x="850" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn2:definitions>
', 0, NULL, NULL, 0, NULL, CAST(0x0000B1F4009B6B43 AS DateTime), CAST(0x0000B20000B521EA AS DateTime))
INSERT [dbo].[WfProcess] ([ID], [ProcessGUID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1487, N'4f65f09b-fa66-4e23-823d-5e39d9f04a20', N'1', N'MessageQueueProcess_Main', N'MessageQueueProcess_Main_001', 1, NULL, 1, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn2:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL" id="bpmn2-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn2:collaboration id="Collaboration_02n62l8" sf:guid="4f65f09b-fa66-4e23-823d-5e39d9f04a20" sf:code="Collaboration_Code_ZQLFR8" name="MessageQueueProcess_Main">
    <bpmn2:participant id="Participant_03l8iuq" sf:guid="408f1f2f-5a21-4244-eebb-8907b09744a0" sf:code="PoolProcess_Code_KUA264" name="Order" processRef="pProcess_Code_5833" />
    <bpmn2:participant id="Participant_0vv09vs" sf:guid="09b8f03d-533a-403d-d1a2-31128280925c" sf:code="PoolProcess_Code_WFLLWN" name="Manufatrue" processRef="Process_0xl1dow" />
    <bpmn2:messageFlow id="Flow_1wqa7t9" sf:guid="2cbb6f18-d3a0-469f-ef83-32c5008dbdea" name="order created" sourceRef="Event_09yiudg" targetRef="Event_16iokyd" sf:from="d74b4c2c-b48f-476b-f7a1-bb62b7e8a3d9" sf:to="dfe156f9-3b71-410f-f2b4-2e241d40c1b7" />
    <bpmn2:messageFlow id="Flow_1kklsu8" sf:guid="196aa691-c786-4584-f922-f9bef3a9e7f4" name="schedule approved" sourceRef="Event_0i5rm9k" targetRef="Event_0uovloq" sf:from="835449ed-66e7-45c3-caac-185a4ef7eb03" sf:to="56209dc7-d27b-4da3-b9af-9e6584ee8fc1" />
  </bpmn2:collaboration>
  <bpmn2:process id="pProcess_Code_5833" sf:guid="d90df9fa-a5f6-4b05-ae27-333d3a712d4f" sf:code="Process_Code_5833" name="MessageQueueProcess" isExecutable="true" sf:version="1">
    <bpmn2:startEvent id="StartEvent_1" sf:guid="09c1f1ed-fa90-4960-a41c-3e0878619468" name="Start">
      <bpmn2:outgoing>Flow_1cm2kyu</bpmn2:outgoing>
    </bpmn2:startEvent>
    <bpmn2:task id="Activity_15c26cz" sf:guid="c779d466-45d6-432a-bd41-0efb3cb6151d" name="Sync Order">
      <bpmn2:incoming>Flow_1cm2kyu</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1i9pglk</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:task id="Activity_1543w6q" sf:guid="154d933b-8c40-47eb-c353-34081c499639" name="Order Distribute">
      <bpmn2:incoming>Flow_1hytkae</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0jco7lx</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:task id="Activity_0mdf4yf" sf:guid="6fa7c51e-1fb7-4005-e4ac-b775fec77534" name="Customer Feedback">
      <bpmn2:incoming>Flow_1m58tra</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1538t0q</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:endEvent id="Event_0xbh6pq" sf:guid="bbac1ee2-bdfd-4a42-cc45-fe79f135d5b7" name="End">
      <bpmn2:incoming>Flow_1538t0q</bpmn2:incoming>
    </bpmn2:endEvent>
    <bpmn2:sequenceFlow id="Flow_1cm2kyu" sf:guid="3a8e284f-beee-4bd9-f2ce-6d5b67c785c3" sourceRef="StartEvent_1" targetRef="Activity_15c26cz" sf:from="09c1f1ed-fa90-4960-a41c-3e0878619468" sf:to="c779d466-45d6-432a-bd41-0efb3cb6151d" />
    <bpmn2:sequenceFlow id="Flow_1i9pglk" sf:guid="d6e15f82-0b29-4bf6-c724-8e8207408d70" sourceRef="Activity_15c26cz" targetRef="Event_09yiudg" sf:from="c779d466-45d6-432a-bd41-0efb3cb6151d" sf:to="d74b4c2c-b48f-476b-f7a1-bb62b7e8a3d9" />
    <bpmn2:sequenceFlow id="Flow_1hytkae" sf:guid="3da0c7a4-cf1d-425c-c4d1-f0dc6aa6a58b" sourceRef="Event_09yiudg" targetRef="Activity_1543w6q" sf:from="d74b4c2c-b48f-476b-f7a1-bb62b7e8a3d9" sf:to="154d933b-8c40-47eb-c353-34081c499639" />
    <bpmn2:sequenceFlow id="Flow_0jco7lx" sf:guid="1eca1e89-11ad-4a27-9a53-8993d13c1ffa" sourceRef="Activity_1543w6q" targetRef="Event_0uovloq" sf:from="154d933b-8c40-47eb-c353-34081c499639" sf:to="56209dc7-d27b-4da3-b9af-9e6584ee8fc1" />
    <bpmn2:sequenceFlow id="Flow_1m58tra" sf:guid="74f5c980-1b2c-4630-9210-52d3452d53da" sourceRef="Event_0uovloq" targetRef="Activity_0mdf4yf" sf:from="56209dc7-d27b-4da3-b9af-9e6584ee8fc1" sf:to="6fa7c51e-1fb7-4005-e4ac-b775fec77534" />
    <bpmn2:sequenceFlow id="Flow_1538t0q" sf:guid="bfd95714-b227-437b-db09-2fdf1bf783ab" sourceRef="Activity_0mdf4yf" targetRef="Event_0xbh6pq" sf:from="6fa7c51e-1fb7-4005-e4ac-b775fec77534" sf:to="bbac1ee2-bdfd-4a42-cc45-fe79f135d5b7" />
    <bpmn2:intermediateThrowEvent id="Event_09yiudg" sf:guid="d74b4c2c-b48f-476b-f7a1-bb62b7e8a3d9">
      <bpmn2:incoming>Flow_1i9pglk</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1hytkae</bpmn2:outgoing>
      <bpmn2:messageEventDefinition id="MessageEventDefinition_0fa58jl" messageRef="Message_JT0H0A" />
    </bpmn2:intermediateThrowEvent>
    <bpmn2:intermediateCatchEvent id="Event_0uovloq" sf:guid="56209dc7-d27b-4da3-b9af-9e6584ee8fc1">
      <bpmn2:incoming>Flow_0jco7lx</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1m58tra</bpmn2:outgoing>
      <bpmn2:messageEventDefinition id="MessageEventDefinition_05rmg4g" messageRef="Message_6XFDUJ" />
    </bpmn2:intermediateCatchEvent>
  </bpmn2:process>
  <bpmn2:process id="Process_0xl1dow" sf:guid="f2f698de-9204-4331-bd41-d6dae15c06a3" sf:code="Process_Code_ChildUUNPYX" name="Process_Name_Child_UUNPYX">
    <bpmn2:sequenceFlow id="Flow_0gopvfk" sf:guid="ad2e6cec-17c6-4736-b4c4-0ec0831640e5" sourceRef="Event_16iokyd" targetRef="Activity_0zvpopy" sf:from="dfe156f9-3b71-410f-f2b4-2e241d40c1b7" sf:to="f482fc1e-a0b0-4b76-a64c-a847ac50b1c2" />
    <bpmn2:sequenceFlow id="Flow_0fxvwoz" sf:guid="5f2643bb-fc6e-4aa3-a0ad-d6c94b7cf0e8" sourceRef="Activity_0zvpopy" targetRef="Activity_05rwkyq" sf:from="f482fc1e-a0b0-4b76-a64c-a847ac50b1c2" sf:to="77c087df-60cf-4ac4-b1d5-51dda2541762" />
    <bpmn2:sequenceFlow id="Flow_1yp7fxb" sf:guid="8ca7b7d7-655e-463d-a13d-aae57ecd4b47" sourceRef="Activity_05rwkyq" targetRef="Event_0i5rm9k" sf:from="77c087df-60cf-4ac4-b1d5-51dda2541762" sf:to="835449ed-66e7-45c3-caac-185a4ef7eb03" />
    <bpmn2:startEvent id="Event_16iokyd" sf:guid="dfe156f9-3b71-410f-f2b4-2e241d40c1b7" name="Start">
      <bpmn2:outgoing>Flow_0gopvfk</bpmn2:outgoing>
      <bpmn2:messageEventDefinition id="MessageEventDefinition_1w7wwl6" messageRef="Message_BJCV29" />
    </bpmn2:startEvent>
    <bpmn2:task id="Activity_0zvpopy" sf:guid="f482fc1e-a0b0-4b76-a64c-a847ac50b1c2" name="Make Schedule">
      <bpmn2:incoming>Flow_0gopvfk</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0fxvwoz</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:task id="Activity_05rwkyq" sf:guid="77c087df-60cf-4ac4-b1d5-51dda2541762" name="Approval">
      <bpmn2:incoming>Flow_0fxvwoz</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1yp7fxb</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:endEvent id="Event_0i5rm9k" sf:guid="835449ed-66e7-45c3-caac-185a4ef7eb03" name="End">
      <bpmn2:incoming>Flow_1yp7fxb</bpmn2:incoming>
      <bpmn2:messageEventDefinition id="MessageEventDefinition_1nlh84h" messageRef="Message_O17G67" />
    </bpmn2:endEvent>
  </bpmn2:process>
  <bpmn2:message id="Message_JT0H0A" name="OrderCreatedNow" />
  <bpmn2:message id="Message_BJCV29" name="OrderCreatedNow" />
  <bpmn2:message id="Message_O17G67" name="ScheduleApprovedNow" />
  <bpmn2:message id="Message_6XFDUJ" name="ScheduleApprovedNow" />
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Collaboration_02n62l8">
      <bpmndi:BPMNShape id="Participant_03l8iuq_di" bpmnElement="Participant_03l8iuq" isHorizontal="true">
        <dc:Bounds x="360" y="133" width="830" height="250" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNEdge id="Flow_1538t0q_di" bpmnElement="Flow_1538t0q">
        <di:waypoint x="1080" y="258" />
        <di:waypoint x="1132" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1m58tra_di" bpmnElement="Flow_1m58tra">
        <di:waypoint x="928" y="258" />
        <di:waypoint x="980" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0jco7lx_di" bpmnElement="Flow_0jco7lx">
        <di:waypoint x="840" y="258" />
        <di:waypoint x="892" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1hytkae_di" bpmnElement="Flow_1hytkae">
        <di:waypoint x="688" y="258" />
        <di:waypoint x="740" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1i9pglk_di" bpmnElement="Flow_1i9pglk">
        <di:waypoint x="600" y="258" />
        <di:waypoint x="652" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1cm2kyu_di" bpmnElement="Flow_1cm2kyu">
        <di:waypoint x="448" y="258" />
        <di:waypoint x="500" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="412" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="418" y="283" width="25" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_15c26cz_di" bpmnElement="Activity_15c26cz">
        <dc:Bounds x="500" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1543w6q_di" bpmnElement="Activity_1543w6q">
        <dc:Bounds x="740" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0mdf4yf_di" bpmnElement="Activity_0mdf4yf">
        <dc:Bounds x="980" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_0xbh6pq_di" bpmnElement="Event_0xbh6pq">
        <dc:Bounds x="1132" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="1140" y="283" width="20" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_039e5vc_di" bpmnElement="Event_09yiudg">
        <dc:Bounds x="652" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_0yrgn7f_di" bpmnElement="Event_0uovloq">
        <dc:Bounds x="892" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Participant_0vv09vs_di" bpmnElement="Participant_0vv09vs" isHorizontal="true">
        <dc:Bounds x="360" y="480" width="830" height="250" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNEdge id="Flow_1yp7fxb_di" bpmnElement="Flow_1yp7fxb">
        <di:waypoint x="960" y="610" />
        <di:waypoint x="1102" y="610" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0fxvwoz_di" bpmnElement="Flow_0fxvwoz">
        <di:waypoint x="720" y="610" />
        <di:waypoint x="860" y="610" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0gopvfk_di" bpmnElement="Flow_0gopvfk">
        <di:waypoint x="488" y="610" />
        <di:waypoint x="620" y="610" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNShape id="Event_1hsr997_di" bpmnElement="Event_16iokyd">
        <dc:Bounds x="452" y="592" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="458" y="635" width="25" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0zvpopy_di" bpmnElement="Activity_0zvpopy">
        <dc:Bounds x="620" y="570" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_05rwkyq_di" bpmnElement="Activity_05rwkyq">
        <dc:Bounds x="860" y="570" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_0fwgvj6_di" bpmnElement="Event_0i5rm9k">
        <dc:Bounds x="1102" y="592" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="1110" y="635" width="20" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNEdge id="Flow_1wqa7t9_di" bpmnElement="Flow_1wqa7t9">
        <di:waypoint x="670" y="276" />
        <di:waypoint x="670" y="434" />
        <di:waypoint x="470" y="434" />
        <di:waypoint x="470" y="592" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="536" y="416" width="68" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1kklsu8_di" bpmnElement="Flow_1kklsu8">
        <di:waypoint x="1120" y="592" />
        <di:waypoint x="1120" y="434" />
        <di:waypoint x="910" y="434" />
        <di:waypoint x="910" y="276" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="991" y="416" width="49" height="27" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn2:definitions>
', 0, NULL, NULL, 0, NULL, CAST(0x0000B1F500DED053 AS DateTime), CAST(0x0000B1F60097E351 AS DateTime))
INSERT [dbo].[WfProcess] ([ID], [ProcessGUID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1488, N'd90df9fa-a5f6-4b05-ae27-333d3a712d4f', N'1', N'Order', N'Order_001', 1, NULL, 2, 1487, N'408f1f2f-5a21-4244-eebb-8907b09744a0', NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn2:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL" id="bpmn2-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn2:collaboration id="Collaboration_02n62l8" sf:guid="4f65f09b-fa66-4e23-823d-5e39d9f04a20" sf:code="Collaboration_Code_ZQLFR8" name="MessageQueueProcess_Main">
    <bpmn2:participant id="Participant_03l8iuq" sf:guid="408f1f2f-5a21-4244-eebb-8907b09744a0" sf:code="PoolProcess_Code_KUA264" name="Order" processRef="pProcess_Code_5833" />
    <bpmn2:participant id="Participant_0vv09vs" sf:guid="09b8f03d-533a-403d-d1a2-31128280925c" sf:code="PoolProcess_Code_WFLLWN" name="Manufatrue" processRef="Process_0xl1dow" />
    <bpmn2:messageFlow id="Flow_1wqa7t9" sf:guid="2cbb6f18-d3a0-469f-ef83-32c5008dbdea" name="order created" sourceRef="Event_09yiudg" targetRef="Event_16iokyd" sf:from="d74b4c2c-b48f-476b-f7a1-bb62b7e8a3d9" sf:to="dfe156f9-3b71-410f-f2b4-2e241d40c1b7" />
    <bpmn2:messageFlow id="Flow_1kklsu8" sf:guid="196aa691-c786-4584-f922-f9bef3a9e7f4" name="schedule approved" sourceRef="Event_0i5rm9k" targetRef="Event_0uovloq" sf:from="835449ed-66e7-45c3-caac-185a4ef7eb03" sf:to="56209dc7-d27b-4da3-b9af-9e6584ee8fc1" />
  </bpmn2:collaboration>
  <bpmn2:process id="pProcess_Code_5833" sf:guid="d90df9fa-a5f6-4b05-ae27-333d3a712d4f" sf:code="Process_Code_5833" name="MessageQueueProcess" isExecutable="true" sf:version="1">
    <bpmn2:startEvent id="StartEvent_1" sf:guid="09c1f1ed-fa90-4960-a41c-3e0878619468" name="Start">
      <bpmn2:outgoing>Flow_1cm2kyu</bpmn2:outgoing>
    </bpmn2:startEvent>
    <bpmn2:task id="Activity_15c26cz" sf:guid="c779d466-45d6-432a-bd41-0efb3cb6151d" name="Sync Order">
      <bpmn2:incoming>Flow_1cm2kyu</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1i9pglk</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:task id="Activity_1543w6q" sf:guid="154d933b-8c40-47eb-c353-34081c499639" name="Order Distribute">
      <bpmn2:incoming>Flow_1hytkae</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0jco7lx</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:task id="Activity_0mdf4yf" sf:guid="6fa7c51e-1fb7-4005-e4ac-b775fec77534" name="Customer Feedback">
      <bpmn2:incoming>Flow_1m58tra</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1538t0q</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:endEvent id="Event_0xbh6pq" sf:guid="bbac1ee2-bdfd-4a42-cc45-fe79f135d5b7" name="End">
      <bpmn2:incoming>Flow_1538t0q</bpmn2:incoming>
    </bpmn2:endEvent>
    <bpmn2:sequenceFlow id="Flow_1cm2kyu" sf:guid="3a8e284f-beee-4bd9-f2ce-6d5b67c785c3" sourceRef="StartEvent_1" targetRef="Activity_15c26cz" sf:from="09c1f1ed-fa90-4960-a41c-3e0878619468" sf:to="c779d466-45d6-432a-bd41-0efb3cb6151d" />
    <bpmn2:sequenceFlow id="Flow_1i9pglk" sf:guid="d6e15f82-0b29-4bf6-c724-8e8207408d70" sourceRef="Activity_15c26cz" targetRef="Event_09yiudg" sf:from="c779d466-45d6-432a-bd41-0efb3cb6151d" sf:to="d74b4c2c-b48f-476b-f7a1-bb62b7e8a3d9" />
    <bpmn2:sequenceFlow id="Flow_1hytkae" sf:guid="3da0c7a4-cf1d-425c-c4d1-f0dc6aa6a58b" sourceRef="Event_09yiudg" targetRef="Activity_1543w6q" sf:from="d74b4c2c-b48f-476b-f7a1-bb62b7e8a3d9" sf:to="154d933b-8c40-47eb-c353-34081c499639" />
    <bpmn2:sequenceFlow id="Flow_0jco7lx" sf:guid="1eca1e89-11ad-4a27-9a53-8993d13c1ffa" sourceRef="Activity_1543w6q" targetRef="Event_0uovloq" sf:from="154d933b-8c40-47eb-c353-34081c499639" sf:to="56209dc7-d27b-4da3-b9af-9e6584ee8fc1" />
    <bpmn2:sequenceFlow id="Flow_1m58tra" sf:guid="74f5c980-1b2c-4630-9210-52d3452d53da" sourceRef="Event_0uovloq" targetRef="Activity_0mdf4yf" sf:from="56209dc7-d27b-4da3-b9af-9e6584ee8fc1" sf:to="6fa7c51e-1fb7-4005-e4ac-b775fec77534" />
    <bpmn2:sequenceFlow id="Flow_1538t0q" sf:guid="bfd95714-b227-437b-db09-2fdf1bf783ab" sourceRef="Activity_0mdf4yf" targetRef="Event_0xbh6pq" sf:from="6fa7c51e-1fb7-4005-e4ac-b775fec77534" sf:to="bbac1ee2-bdfd-4a42-cc45-fe79f135d5b7" />
    <bpmn2:intermediateThrowEvent id="Event_09yiudg" sf:guid="d74b4c2c-b48f-476b-f7a1-bb62b7e8a3d9">
      <bpmn2:incoming>Flow_1i9pglk</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1hytkae</bpmn2:outgoing>
      <bpmn2:messageEventDefinition id="MessageEventDefinition_0fa58jl" messageRef="Message_JT0H0A" />
    </bpmn2:intermediateThrowEvent>
    <bpmn2:intermediateCatchEvent id="Event_0uovloq" sf:guid="56209dc7-d27b-4da3-b9af-9e6584ee8fc1">
      <bpmn2:incoming>Flow_0jco7lx</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1m58tra</bpmn2:outgoing>
      <bpmn2:messageEventDefinition id="MessageEventDefinition_05rmg4g" messageRef="Message_6XFDUJ" />
    </bpmn2:intermediateCatchEvent>
  </bpmn2:process>
  <bpmn2:process id="Process_0xl1dow" sf:guid="f2f698de-9204-4331-bd41-d6dae15c06a3" sf:code="Process_Code_ChildUUNPYX" name="Process_Name_Child_UUNPYX">
    <bpmn2:sequenceFlow id="Flow_0gopvfk" sf:guid="ad2e6cec-17c6-4736-b4c4-0ec0831640e5" sourceRef="Event_16iokyd" targetRef="Activity_0zvpopy" sf:from="dfe156f9-3b71-410f-f2b4-2e241d40c1b7" sf:to="f482fc1e-a0b0-4b76-a64c-a847ac50b1c2" />
    <bpmn2:sequenceFlow id="Flow_0fxvwoz" sf:guid="5f2643bb-fc6e-4aa3-a0ad-d6c94b7cf0e8" sourceRef="Activity_0zvpopy" targetRef="Activity_05rwkyq" sf:from="f482fc1e-a0b0-4b76-a64c-a847ac50b1c2" sf:to="77c087df-60cf-4ac4-b1d5-51dda2541762" />
    <bpmn2:sequenceFlow id="Flow_1yp7fxb" sf:guid="8ca7b7d7-655e-463d-a13d-aae57ecd4b47" sourceRef="Activity_05rwkyq" targetRef="Event_0i5rm9k" sf:from="77c087df-60cf-4ac4-b1d5-51dda2541762" sf:to="835449ed-66e7-45c3-caac-185a4ef7eb03" />
    <bpmn2:startEvent id="Event_16iokyd" sf:guid="dfe156f9-3b71-410f-f2b4-2e241d40c1b7" name="Start">
      <bpmn2:outgoing>Flow_0gopvfk</bpmn2:outgoing>
      <bpmn2:messageEventDefinition id="MessageEventDefinition_1w7wwl6" messageRef="Message_BJCV29" />
    </bpmn2:startEvent>
    <bpmn2:task id="Activity_0zvpopy" sf:guid="f482fc1e-a0b0-4b76-a64c-a847ac50b1c2" name="Make Schedule">
      <bpmn2:incoming>Flow_0gopvfk</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0fxvwoz</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:task id="Activity_05rwkyq" sf:guid="77c087df-60cf-4ac4-b1d5-51dda2541762" name="Approval">
      <bpmn2:incoming>Flow_0fxvwoz</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1yp7fxb</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:endEvent id="Event_0i5rm9k" sf:guid="835449ed-66e7-45c3-caac-185a4ef7eb03" name="End">
      <bpmn2:incoming>Flow_1yp7fxb</bpmn2:incoming>
      <bpmn2:messageEventDefinition id="MessageEventDefinition_1nlh84h" messageRef="Message_O17G67" />
    </bpmn2:endEvent>
  </bpmn2:process>
  <bpmn2:message id="Message_JT0H0A" name="OrderCreatedNow" />
  <bpmn2:message id="Message_BJCV29" name="OrderCreatedNow" />
  <bpmn2:message id="Message_O17G67" name="ScheduleApprovedNow" />
  <bpmn2:message id="Message_6XFDUJ" name="ScheduleApprovedNow" />
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Collaboration_02n62l8">
      <bpmndi:BPMNShape id="Participant_03l8iuq_di" bpmnElement="Participant_03l8iuq" isHorizontal="true">
        <dc:Bounds x="360" y="133" width="830" height="250" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNEdge id="Flow_1538t0q_di" bpmnElement="Flow_1538t0q">
        <di:waypoint x="1080" y="258" />
        <di:waypoint x="1132" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1m58tra_di" bpmnElement="Flow_1m58tra">
        <di:waypoint x="928" y="258" />
        <di:waypoint x="980" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0jco7lx_di" bpmnElement="Flow_0jco7lx">
        <di:waypoint x="840" y="258" />
        <di:waypoint x="892" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1hytkae_di" bpmnElement="Flow_1hytkae">
        <di:waypoint x="688" y="258" />
        <di:waypoint x="740" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1i9pglk_di" bpmnElement="Flow_1i9pglk">
        <di:waypoint x="600" y="258" />
        <di:waypoint x="652" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1cm2kyu_di" bpmnElement="Flow_1cm2kyu">
        <di:waypoint x="448" y="258" />
        <di:waypoint x="500" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="412" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="418" y="283" width="25" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_15c26cz_di" bpmnElement="Activity_15c26cz">
        <dc:Bounds x="500" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1543w6q_di" bpmnElement="Activity_1543w6q">
        <dc:Bounds x="740" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0mdf4yf_di" bpmnElement="Activity_0mdf4yf">
        <dc:Bounds x="980" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_0xbh6pq_di" bpmnElement="Event_0xbh6pq">
        <dc:Bounds x="1132" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="1140" y="283" width="20" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_039e5vc_di" bpmnElement="Event_09yiudg">
        <dc:Bounds x="652" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_0yrgn7f_di" bpmnElement="Event_0uovloq">
        <dc:Bounds x="892" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Participant_0vv09vs_di" bpmnElement="Participant_0vv09vs" isHorizontal="true">
        <dc:Bounds x="360" y="480" width="830" height="250" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNEdge id="Flow_1yp7fxb_di" bpmnElement="Flow_1yp7fxb">
        <di:waypoint x="960" y="610" />
        <di:waypoint x="1102" y="610" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0fxvwoz_di" bpmnElement="Flow_0fxvwoz">
        <di:waypoint x="720" y="610" />
        <di:waypoint x="860" y="610" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0gopvfk_di" bpmnElement="Flow_0gopvfk">
        <di:waypoint x="488" y="610" />
        <di:waypoint x="620" y="610" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNShape id="Event_1hsr997_di" bpmnElement="Event_16iokyd">
        <dc:Bounds x="452" y="592" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="458" y="635" width="25" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0zvpopy_di" bpmnElement="Activity_0zvpopy">
        <dc:Bounds x="620" y="570" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_05rwkyq_di" bpmnElement="Activity_05rwkyq">
        <dc:Bounds x="860" y="570" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_0fwgvj6_di" bpmnElement="Event_0i5rm9k">
        <dc:Bounds x="1102" y="592" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="1110" y="635" width="20" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNEdge id="Flow_1wqa7t9_di" bpmnElement="Flow_1wqa7t9">
        <di:waypoint x="670" y="276" />
        <di:waypoint x="670" y="434" />
        <di:waypoint x="470" y="434" />
        <di:waypoint x="470" y="592" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="536" y="416" width="68" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1kklsu8_di" bpmnElement="Flow_1kklsu8">
        <di:waypoint x="1120" y="592" />
        <di:waypoint x="1120" y="434" />
        <di:waypoint x="910" y="434" />
        <di:waypoint x="910" y="276" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="991" y="416" width="49" height="27" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn2:definitions>
', 0, NULL, NULL, 0, NULL, CAST(0x0000B1F500DED054 AS DateTime), CAST(0x0000B1F60097E35F AS DateTime))
INSERT [dbo].[WfProcess] ([ID], [ProcessGUID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1489, N'f2f698de-9204-4331-bd41-d6dae15c06a3', N'1', N'Manufatrue', N'Manufatrue_001', 0, NULL, 2, 1487, N'09b8f03d-533a-403d-d1a2-31128280925c', NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn2:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL" id="bpmn2-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn2:collaboration id="Collaboration_02n62l8" sf:guid="4f65f09b-fa66-4e23-823d-5e39d9f04a20" sf:code="Collaboration_Code_ZQLFR8" name="MessageQueueProcess_Main">
    <bpmn2:participant id="Participant_03l8iuq" sf:guid="408f1f2f-5a21-4244-eebb-8907b09744a0" sf:code="PoolProcess_Code_KUA264" name="Order" processRef="pProcess_Code_5833" />
    <bpmn2:participant id="Participant_0vv09vs" sf:guid="09b8f03d-533a-403d-d1a2-31128280925c" sf:code="PoolProcess_Code_WFLLWN" name="Manufatrue" processRef="Process_0xl1dow" />
    <bpmn2:messageFlow id="Flow_1wqa7t9" sf:guid="2cbb6f18-d3a0-469f-ef83-32c5008dbdea" name="order created" sourceRef="Event_09yiudg" targetRef="Event_16iokyd" sf:from="d74b4c2c-b48f-476b-f7a1-bb62b7e8a3d9" sf:to="dfe156f9-3b71-410f-f2b4-2e241d40c1b7" />
    <bpmn2:messageFlow id="Flow_1kklsu8" sf:guid="196aa691-c786-4584-f922-f9bef3a9e7f4" name="schedule approved" sourceRef="Event_0i5rm9k" targetRef="Event_0uovloq" sf:from="835449ed-66e7-45c3-caac-185a4ef7eb03" sf:to="56209dc7-d27b-4da3-b9af-9e6584ee8fc1" />
  </bpmn2:collaboration>
  <bpmn2:process id="pProcess_Code_5833" sf:guid="d90df9fa-a5f6-4b05-ae27-333d3a712d4f" sf:code="Process_Code_5833" name="MessageQueueProcess" isExecutable="true" sf:version="1">
    <bpmn2:startEvent id="StartEvent_1" sf:guid="09c1f1ed-fa90-4960-a41c-3e0878619468" name="Start">
      <bpmn2:outgoing>Flow_1cm2kyu</bpmn2:outgoing>
    </bpmn2:startEvent>
    <bpmn2:task id="Activity_15c26cz" sf:guid="c779d466-45d6-432a-bd41-0efb3cb6151d" name="Sync Order">
      <bpmn2:incoming>Flow_1cm2kyu</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1i9pglk</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:task id="Activity_1543w6q" sf:guid="154d933b-8c40-47eb-c353-34081c499639" name="Order Distribute">
      <bpmn2:incoming>Flow_1hytkae</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0jco7lx</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:task id="Activity_0mdf4yf" sf:guid="6fa7c51e-1fb7-4005-e4ac-b775fec77534" name="Customer Feedback">
      <bpmn2:incoming>Flow_1m58tra</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1538t0q</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:endEvent id="Event_0xbh6pq" sf:guid="bbac1ee2-bdfd-4a42-cc45-fe79f135d5b7" name="End">
      <bpmn2:incoming>Flow_1538t0q</bpmn2:incoming>
    </bpmn2:endEvent>
    <bpmn2:sequenceFlow id="Flow_1cm2kyu" sf:guid="3a8e284f-beee-4bd9-f2ce-6d5b67c785c3" sourceRef="StartEvent_1" targetRef="Activity_15c26cz" sf:from="09c1f1ed-fa90-4960-a41c-3e0878619468" sf:to="c779d466-45d6-432a-bd41-0efb3cb6151d" />
    <bpmn2:sequenceFlow id="Flow_1i9pglk" sf:guid="d6e15f82-0b29-4bf6-c724-8e8207408d70" sourceRef="Activity_15c26cz" targetRef="Event_09yiudg" sf:from="c779d466-45d6-432a-bd41-0efb3cb6151d" sf:to="d74b4c2c-b48f-476b-f7a1-bb62b7e8a3d9" />
    <bpmn2:sequenceFlow id="Flow_1hytkae" sf:guid="3da0c7a4-cf1d-425c-c4d1-f0dc6aa6a58b" sourceRef="Event_09yiudg" targetRef="Activity_1543w6q" sf:from="d74b4c2c-b48f-476b-f7a1-bb62b7e8a3d9" sf:to="154d933b-8c40-47eb-c353-34081c499639" />
    <bpmn2:sequenceFlow id="Flow_0jco7lx" sf:guid="1eca1e89-11ad-4a27-9a53-8993d13c1ffa" sourceRef="Activity_1543w6q" targetRef="Event_0uovloq" sf:from="154d933b-8c40-47eb-c353-34081c499639" sf:to="56209dc7-d27b-4da3-b9af-9e6584ee8fc1" />
    <bpmn2:sequenceFlow id="Flow_1m58tra" sf:guid="74f5c980-1b2c-4630-9210-52d3452d53da" sourceRef="Event_0uovloq" targetRef="Activity_0mdf4yf" sf:from="56209dc7-d27b-4da3-b9af-9e6584ee8fc1" sf:to="6fa7c51e-1fb7-4005-e4ac-b775fec77534" />
    <bpmn2:sequenceFlow id="Flow_1538t0q" sf:guid="bfd95714-b227-437b-db09-2fdf1bf783ab" sourceRef="Activity_0mdf4yf" targetRef="Event_0xbh6pq" sf:from="6fa7c51e-1fb7-4005-e4ac-b775fec77534" sf:to="bbac1ee2-bdfd-4a42-cc45-fe79f135d5b7" />
    <bpmn2:intermediateThrowEvent id="Event_09yiudg" sf:guid="d74b4c2c-b48f-476b-f7a1-bb62b7e8a3d9">
      <bpmn2:incoming>Flow_1i9pglk</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1hytkae</bpmn2:outgoing>
      <bpmn2:messageEventDefinition id="MessageEventDefinition_0fa58jl" messageRef="Message_JT0H0A" />
    </bpmn2:intermediateThrowEvent>
    <bpmn2:intermediateCatchEvent id="Event_0uovloq" sf:guid="56209dc7-d27b-4da3-b9af-9e6584ee8fc1">
      <bpmn2:incoming>Flow_0jco7lx</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1m58tra</bpmn2:outgoing>
      <bpmn2:messageEventDefinition id="MessageEventDefinition_05rmg4g" messageRef="Message_6XFDUJ" />
    </bpmn2:intermediateCatchEvent>
  </bpmn2:process>
  <bpmn2:process id="Process_0xl1dow" sf:guid="f2f698de-9204-4331-bd41-d6dae15c06a3" sf:code="Process_Code_ChildUUNPYX" name="Process_Name_Child_UUNPYX">
    <bpmn2:sequenceFlow id="Flow_0gopvfk" sf:guid="ad2e6cec-17c6-4736-b4c4-0ec0831640e5" sourceRef="Event_16iokyd" targetRef="Activity_0zvpopy" sf:from="dfe156f9-3b71-410f-f2b4-2e241d40c1b7" sf:to="f482fc1e-a0b0-4b76-a64c-a847ac50b1c2" />
    <bpmn2:sequenceFlow id="Flow_0fxvwoz" sf:guid="5f2643bb-fc6e-4aa3-a0ad-d6c94b7cf0e8" sourceRef="Activity_0zvpopy" targetRef="Activity_05rwkyq" sf:from="f482fc1e-a0b0-4b76-a64c-a847ac50b1c2" sf:to="77c087df-60cf-4ac4-b1d5-51dda2541762" />
    <bpmn2:sequenceFlow id="Flow_1yp7fxb" sf:guid="8ca7b7d7-655e-463d-a13d-aae57ecd4b47" sourceRef="Activity_05rwkyq" targetRef="Event_0i5rm9k" sf:from="77c087df-60cf-4ac4-b1d5-51dda2541762" sf:to="835449ed-66e7-45c3-caac-185a4ef7eb03" />
    <bpmn2:startEvent id="Event_16iokyd" sf:guid="dfe156f9-3b71-410f-f2b4-2e241d40c1b7" name="Start">
      <bpmn2:outgoing>Flow_0gopvfk</bpmn2:outgoing>
      <bpmn2:messageEventDefinition id="MessageEventDefinition_1w7wwl6" messageRef="Message_BJCV29" />
    </bpmn2:startEvent>
    <bpmn2:task id="Activity_0zvpopy" sf:guid="f482fc1e-a0b0-4b76-a64c-a847ac50b1c2" name="Make Schedule">
      <bpmn2:incoming>Flow_0gopvfk</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0fxvwoz</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:task id="Activity_05rwkyq" sf:guid="77c087df-60cf-4ac4-b1d5-51dda2541762" name="Approval">
      <bpmn2:incoming>Flow_0fxvwoz</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1yp7fxb</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:endEvent id="Event_0i5rm9k" sf:guid="835449ed-66e7-45c3-caac-185a4ef7eb03" name="End">
      <bpmn2:incoming>Flow_1yp7fxb</bpmn2:incoming>
      <bpmn2:messageEventDefinition id="MessageEventDefinition_1nlh84h" messageRef="Message_O17G67" />
    </bpmn2:endEvent>
  </bpmn2:process>
  <bpmn2:message id="Message_JT0H0A" name="OrderCreatedNow" />
  <bpmn2:message id="Message_BJCV29" name="OrderCreatedNow" />
  <bpmn2:message id="Message_O17G67" name="ScheduleApprovedNow" />
  <bpmn2:message id="Message_6XFDUJ" name="ScheduleApprovedNow" />
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Collaboration_02n62l8">
      <bpmndi:BPMNShape id="Participant_03l8iuq_di" bpmnElement="Participant_03l8iuq" isHorizontal="true">
        <dc:Bounds x="360" y="133" width="830" height="250" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNEdge id="Flow_1538t0q_di" bpmnElement="Flow_1538t0q">
        <di:waypoint x="1080" y="258" />
        <di:waypoint x="1132" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1m58tra_di" bpmnElement="Flow_1m58tra">
        <di:waypoint x="928" y="258" />
        <di:waypoint x="980" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0jco7lx_di" bpmnElement="Flow_0jco7lx">
        <di:waypoint x="840" y="258" />
        <di:waypoint x="892" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1hytkae_di" bpmnElement="Flow_1hytkae">
        <di:waypoint x="688" y="258" />
        <di:waypoint x="740" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1i9pglk_di" bpmnElement="Flow_1i9pglk">
        <di:waypoint x="600" y="258" />
        <di:waypoint x="652" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1cm2kyu_di" bpmnElement="Flow_1cm2kyu">
        <di:waypoint x="448" y="258" />
        <di:waypoint x="500" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="412" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="418" y="283" width="25" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_15c26cz_di" bpmnElement="Activity_15c26cz">
        <dc:Bounds x="500" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1543w6q_di" bpmnElement="Activity_1543w6q">
        <dc:Bounds x="740" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0mdf4yf_di" bpmnElement="Activity_0mdf4yf">
        <dc:Bounds x="980" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_0xbh6pq_di" bpmnElement="Event_0xbh6pq">
        <dc:Bounds x="1132" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="1140" y="283" width="20" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_039e5vc_di" bpmnElement="Event_09yiudg">
        <dc:Bounds x="652" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_0yrgn7f_di" bpmnElement="Event_0uovloq">
        <dc:Bounds x="892" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Participant_0vv09vs_di" bpmnElement="Participant_0vv09vs" isHorizontal="true">
        <dc:Bounds x="360" y="480" width="830" height="250" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNEdge id="Flow_1yp7fxb_di" bpmnElement="Flow_1yp7fxb">
        <di:waypoint x="960" y="610" />
        <di:waypoint x="1102" y="610" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0fxvwoz_di" bpmnElement="Flow_0fxvwoz">
        <di:waypoint x="720" y="610" />
        <di:waypoint x="860" y="610" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0gopvfk_di" bpmnElement="Flow_0gopvfk">
        <di:waypoint x="488" y="610" />
        <di:waypoint x="620" y="610" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNShape id="Event_1hsr997_di" bpmnElement="Event_16iokyd">
        <dc:Bounds x="452" y="592" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="458" y="635" width="25" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0zvpopy_di" bpmnElement="Activity_0zvpopy">
        <dc:Bounds x="620" y="570" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_05rwkyq_di" bpmnElement="Activity_05rwkyq">
        <dc:Bounds x="860" y="570" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_0fwgvj6_di" bpmnElement="Event_0i5rm9k">
        <dc:Bounds x="1102" y="592" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="1110" y="635" width="20" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNEdge id="Flow_1wqa7t9_di" bpmnElement="Flow_1wqa7t9">
        <di:waypoint x="670" y="276" />
        <di:waypoint x="670" y="434" />
        <di:waypoint x="470" y="434" />
        <di:waypoint x="470" y="592" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="536" y="416" width="68" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1kklsu8_di" bpmnElement="Flow_1kklsu8">
        <di:waypoint x="1120" y="592" />
        <di:waypoint x="1120" y="434" />
        <di:waypoint x="910" y="434" />
        <di:waypoint x="910" y="276" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="991" y="416" width="49" height="27" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn2:definitions>
', 0, NULL, NULL, 0, NULL, CAST(0x0000B1F500DF5F57 AS DateTime), CAST(0x0000B1F60097E361 AS DateTime))
INSERT [dbo].[WfProcess] ([ID], [ProcessGUID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1490, N'0a3fcd51-f1a5-456c-ad15-989416189be7', N'1', N'SignalProcess-Throw', N'Process_Code_7444', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn2:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL" id="bpmn2-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn2:process id="pProcess_Code_7444" sf:guid="0a3fcd51-f1a5-456c-ad15-989416189be7" sf:code="Process_Code_7444" name="SignalProcess-Throw" isExecutable="true" sf:version="1">
    <bpmn2:startEvent id="StartEvent_1" sf:guid="10da6449-4759-4e5a-b2f5-e7083dc2ecae" name="Start">
      <bpmn2:outgoing>Flow_149gt04</bpmn2:outgoing>
    </bpmn2:startEvent>
    <bpmn2:task id="Activity_1xmwkyy" sf:guid="18031f4f-c600-4b7d-f231-e20434aa69c0" name="Order Sync">
      <bpmn2:incoming>Flow_149gt04</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0wxn2ou</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_149gt04" sf:guid="07e066af-b2f7-480b-9e03-60b3a7bfe9c9" sourceRef="StartEvent_1" targetRef="Activity_1xmwkyy" sf:from="10da6449-4759-4e5a-b2f5-e7083dc2ecae" sf:to="18031f4f-c600-4b7d-f231-e20434aa69c0" />
    <bpmn2:task id="Activity_0zr59mw" sf:guid="5c76ed14-3431-4031-f74b-7741dd5aec42" name="Customer Feedback">
      <bpmn2:incoming>Flow_0ojbdf4</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1f5z470</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_0ojbdf4" sf:guid="15d3e35b-8dbc-4462-8eeb-cc9e4ef90425" sourceRef="Event_0y96chd" targetRef="Activity_0zr59mw" sf:from="41c0e7a9-459e-475f-bd01-3164e902782b" sf:to="5c76ed14-3431-4031-f74b-7741dd5aec42" />
    <bpmn2:endEvent id="Event_0zzo2nm" sf:guid="a792181e-e6c1-43a8-cd0b-42dc4904e88e">
      <bpmn2:incoming>Flow_1f5z470</bpmn2:incoming>
    </bpmn2:endEvent>
    <bpmn2:sequenceFlow id="Flow_1f5z470" sf:guid="2491d14a-9591-4b6c-e1e3-145f84b51a90" sourceRef="Activity_0zr59mw" targetRef="Event_0zzo2nm" sf:from="5c76ed14-3431-4031-f74b-7741dd5aec42" sf:to="a792181e-e6c1-43a8-cd0b-42dc4904e88e" />
    <bpmn2:intermediateThrowEvent id="Event_0y96chd" sf:guid="41c0e7a9-459e-475f-bd01-3164e902782b">
      <bpmn2:incoming>Flow_0or7rdv</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0ojbdf4</bpmn2:outgoing>
      <bpmn2:signalEventDefinition id="SignalEventDefinition_1ifbi8k" signalRef="Siganl_QJ3T6O" />
    </bpmn2:intermediateThrowEvent>
    <bpmn2:task id="Activity_04r3esv" sf:guid="a8369c91-92eb-4dbf-849a-a00619115865" name="Order Distribute">
      <bpmn2:incoming>Flow_0wxn2ou</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0or7rdv</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_0wxn2ou" sf:guid="dfc70b5b-2d73-44d1-dcce-ca8de7195ed4" sourceRef="Activity_1xmwkyy" targetRef="Activity_04r3esv" sf:from="18031f4f-c600-4b7d-f231-e20434aa69c0" sf:to="a8369c91-92eb-4dbf-849a-a00619115865" />
    <bpmn2:sequenceFlow id="Flow_0or7rdv" sf:guid="3febddc6-5ec8-4a2c-a94e-f5eac7592e6e" sourceRef="Activity_04r3esv" targetRef="Event_0y96chd" sf:from="a8369c91-92eb-4dbf-849a-a00619115865" sf:to="41c0e7a9-459e-475f-bd01-3164e902782b" />
  </bpmn2:process>
  <bpmn2:signal id="Siganl_QJ3T6O" name="OrderDistributed" />
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="pProcess_Code_7444">
      <bpmndi:BPMNEdge id="Flow_0or7rdv_di" bpmnElement="Flow_0or7rdv">
        <di:waypoint x="610" y="258" />
        <di:waypoint x="652" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0wxn2ou_di" bpmnElement="Flow_0wxn2ou">
        <di:waypoint x="420" y="258" />
        <di:waypoint x="510" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1f5z470_di" bpmnElement="Flow_1f5z470">
        <di:waypoint x="840" y="258" />
        <di:waypoint x="892" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0ojbdf4_di" bpmnElement="Flow_0ojbdf4">
        <di:waypoint x="688" y="258" />
        <di:waypoint x="740" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_149gt04_di" bpmnElement="Flow_149gt04">
        <di:waypoint x="238" y="258" />
        <di:waypoint x="320" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="202" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="208" y="283" width="25" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1xmwkyy_di" bpmnElement="Activity_1xmwkyy">
        <dc:Bounds x="320" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0zr59mw_di" bpmnElement="Activity_0zr59mw">
        <dc:Bounds x="740" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_0zzo2nm_di" bpmnElement="Event_0zzo2nm">
        <dc:Bounds x="892" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_0iyxl0r_di" bpmnElement="Event_0y96chd">
        <dc:Bounds x="652" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_04r3esv_di" bpmnElement="Activity_04r3esv">
        <dc:Bounds x="510" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn2:definitions>
', 0, NULL, NULL, 0, NULL, CAST(0x0000B1FD01073767 AS DateTime), CAST(0x0000B1FD01076520 AS DateTime))
INSERT [dbo].[WfProcess] ([ID], [ProcessGUID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1491, N'214ad24b-9097-41de-8e74-a5913c518429', N'1', N'SignalInterProcess', N'Process_Code_2942', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn2:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL" id="bpmn2-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn2:process id="pProcess_Code_2942" sf:guid="214ad24b-9097-41de-8e74-a5913c518429" sf:code="Process_Code_2942" name="SignalInterProcess" isExecutable="true" sf:version="1">
    <bpmn2:startEvent id="StartEvent_1" sf:guid="933c6f89-bee8-4a6f-90a6-776d04110d7e" name="Start">
      <bpmn2:outgoing>Flow_05uls8h</bpmn2:outgoing>
    </bpmn2:startEvent>
    <bpmn2:task id="Activity_14gjf8t" sf:guid="2ada939a-a6cc-4fd4-8ac4-e2c0bd65e85e" name="Rent Factory">
      <bpmn2:incoming>Flow_05uls8h</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0o8cc24</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_05uls8h" sf:guid="51555654-6dfa-47be-be34-b208c4b5f866" sourceRef="StartEvent_1" targetRef="Activity_14gjf8t" sf:from="933c6f89-bee8-4a6f-90a6-776d04110d7e" sf:to="2ada939a-a6cc-4fd4-8ac4-e2c0bd65e85e" />
    <bpmn2:sequenceFlow id="Flow_0o8cc24" sf:guid="b528481e-e043-4c23-cad8-9d3142913b31" sourceRef="Activity_14gjf8t" targetRef="Event_0lt2165" sf:from="2ada939a-a6cc-4fd4-8ac4-e2c0bd65e85e" sf:to="c53f07ba-e65c-465b-d81f-ea992249712c" />
    <bpmn2:task id="Activity_0pntgy0" sf:guid="1e68a897-755e-4d94-db9f-09aa6d21ff04" name="Making Plan">
      <bpmn2:incoming>Flow_0g319gg</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1190lyi</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_0g319gg" sf:guid="04828f97-3160-4646-b600-bba5e25e2b79" sourceRef="Event_0lt2165" targetRef="Activity_0pntgy0" sf:from="c53f07ba-e65c-465b-d81f-ea992249712c" sf:to="1e68a897-755e-4d94-db9f-09aa6d21ff04" />
    <bpmn2:endEvent id="Event_1encihk" sf:guid="a3d9618f-64d7-4a25-b4d2-3cb341b16380">
      <bpmn2:incoming>Flow_1190lyi</bpmn2:incoming>
    </bpmn2:endEvent>
    <bpmn2:sequenceFlow id="Flow_1190lyi" sf:guid="acd2f8bd-b69e-419f-f706-f21fea831036" sourceRef="Activity_0pntgy0" targetRef="Event_1encihk" sf:from="1e68a897-755e-4d94-db9f-09aa6d21ff04" sf:to="a3d9618f-64d7-4a25-b4d2-3cb341b16380" />
    <bpmn2:intermediateCatchEvent id="Event_0lt2165" sf:guid="c53f07ba-e65c-465b-d81f-ea992249712c">
      <bpmn2:incoming>Flow_0o8cc24</bpmn2:incoming>
      <bpmn2:outgoing>Flow_0g319gg</bpmn2:outgoing>
      <bpmn2:signalEventDefinition id="SignalEventDefinition_0ijialr" signalRef="Siganl_CLJDLS" />
    </bpmn2:intermediateCatchEvent>
  </bpmn2:process>
  <bpmn2:signal id="Siganl_CLJDLS" name="OrderDistributed" />
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="pProcess_Code_2942">
      <bpmndi:BPMNEdge id="Flow_05uls8h_di" bpmnElement="Flow_05uls8h">
        <di:waypoint x="448" y="258" />
        <di:waypoint x="500" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0o8cc24_di" bpmnElement="Flow_0o8cc24">
        <di:waypoint x="600" y="258" />
        <di:waypoint x="652" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0g319gg_di" bpmnElement="Flow_0g319gg">
        <di:waypoint x="688" y="258" />
        <di:waypoint x="740" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1190lyi_di" bpmnElement="Flow_1190lyi">
        <di:waypoint x="840" y="258" />
        <di:waypoint x="892" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="412" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="418" y="283" width="25" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_14gjf8t_di" bpmnElement="Activity_14gjf8t">
        <dc:Bounds x="500" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0pntgy0_di" bpmnElement="Activity_0pntgy0">
        <dc:Bounds x="740" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_1encihk_di" bpmnElement="Event_1encihk">
        <dc:Bounds x="892" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_0m8hxf0_di" bpmnElement="Event_0lt2165">
        <dc:Bounds x="652" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn2:definitions>
', 0, NULL, NULL, 0, NULL, CAST(0x0000B20000B57823 AS DateTime), CAST(0x0000B200010A829B AS DateTime))
INSERT [dbo].[WfProcess] ([ID], [ProcessGUID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1492, N'6178ea18-31b2-44a5-976b-9f9857311be6', N'1', N'TimerCronExpression', N'Process_Code_7296', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn2:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL" id="bpmn2-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn2:process id="pProcess_Code_7296" sf:guid="6178ea18-31b2-44a5-976b-9f9857311be6" sf:code="Process_Code_7296" name="TimerCronExpression" isExecutable="true" sf:version="1">
    <bpmn2:task id="Activity_0tvn8u2" sf:guid="1c8f6324-1db1-42d3-89e4-7413d0998bd4">
      <bpmn2:incoming>Flow_02kc0xr</bpmn2:incoming>
      <bpmn2:outgoing>Flow_04ns8hq</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_02kc0xr" sf:guid="7e4511d3-fdd1-40d3-a48d-f66e1be7fbc5" sourceRef="StartEvent_1" targetRef="Activity_0tvn8u2" sf:from="9b259799-b3a0-45b7-b2ed-a1c37a500f53" sf:to="1c8f6324-1db1-42d3-89e4-7413d0998bd4" />
    <bpmn2:task id="Activity_0gcaks8" sf:guid="982af86b-827f-427f-b6da-68fe91ba11f2">
      <bpmn2:incoming>Flow_04ns8hq</bpmn2:incoming>
      <bpmn2:outgoing>Flow_10ygvts</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_04ns8hq" sf:guid="d3bd5f6e-71c1-404b-a934-035fd65027f2" sourceRef="Activity_0tvn8u2" targetRef="Activity_0gcaks8" sf:from="1c8f6324-1db1-42d3-89e4-7413d0998bd4" sf:to="982af86b-827f-427f-b6da-68fe91ba11f2" />
    <bpmn2:endEvent id="Event_1k2bat1" sf:guid="caa2be84-8b4a-43cd-8e63-ede4a4878ef0">
      <bpmn2:incoming>Flow_10ygvts</bpmn2:incoming>
    </bpmn2:endEvent>
    <bpmn2:sequenceFlow id="Flow_10ygvts" sf:guid="09afd62f-3958-47f5-d8dd-97945e848206" sourceRef="Activity_0gcaks8" targetRef="Event_1k2bat1" sf:from="982af86b-827f-427f-b6da-68fe91ba11f2" sf:to="caa2be84-8b4a-43cd-8e63-ede4a4878ef0" />
    <bpmn2:startEvent id="StartEvent_1" sf:guid="9b259799-b3a0-45b7-b2ed-a1c37a500f53" name="Start">
      <bpmn2:outgoing>Flow_02kc0xr</bpmn2:outgoing>
      <bpmn2:timerEventDefinition id="TimerEventDefinition_1k0sj2j" />
    </bpmn2:startEvent>
  </bpmn2:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="pProcess_Code_7296">
      <bpmndi:BPMNEdge id="Flow_02kc0xr_di" bpmnElement="Flow_02kc0xr">
        <di:waypoint x="448" y="258" />
        <di:waypoint x="500" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_04ns8hq_di" bpmnElement="Flow_04ns8hq">
        <di:waypoint x="600" y="258" />
        <di:waypoint x="660" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_10ygvts_di" bpmnElement="Flow_10ygvts">
        <di:waypoint x="760" y="258" />
        <di:waypoint x="822" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNShape id="Activity_0tvn8u2_di" bpmnElement="Activity_0tvn8u2">
        <dc:Bounds x="500" y="218" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0gcaks8_di" bpmnElement="Activity_0gcaks8">
        <dc:Bounds x="660" y="218" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_1k2bat1_di" bpmnElement="Event_1k2bat1">
        <dc:Bounds x="822" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_1jpnzst_di" bpmnElement="StartEvent_1">
        <dc:Bounds x="412" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="418" y="283" width="25" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn2:definitions>
                                                                                                                                                                                                                                                                                                                                                               ', 0, NULL, NULL, 0, NULL, CAST(0x0000B20600E4450B AS DateTime), CAST(0x0000B20600E45576 AS DateTime))
INSERT [dbo].[WfProcess] ([ID], [ProcessGUID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1493, N'b39c385c-15b1-4972-9b17-2fa9148e5fa9', N'1', N'Process_Name_9505', N'Process_Code_9505', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn2:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL" id="bpmn2-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn2:process id="pProcess_Code_9505" sf:guid="b39c385c-15b1-4972-9b17-2fa9148e5fa9" sf:code="Process_Code_9505" name="Process_Name_9505" isExecutable="true" sf:version="1">
    <bpmn2:extensionElements>
      <sf:forms>
        <sf:form name="Form_1u453x3" outerId="65" outerCode="WNMNE4" />
        <sf:form name="Form_Test_Sample" outerId="63" outerCode="QMIS2H" />
      </sf:forms>
    </bpmn2:extensionElements>
    <bpmn2:startEvent id="StartEvent_1" sf:guid="25b93434-80bd-412a-a847-2590a16351e6" name="Start">
      <bpmn2:outgoing>Flow_0yozj8d</bpmn2:outgoing>
    </bpmn2:startEvent>
    <bpmn2:task id="Activity_0w04ty5" sf:guid="cfacd581-abfc-40ee-8a93-cae20aab4102" name="Task1">
      <bpmn2:incoming>Flow_0yozj8d</bpmn2:incoming>
      <bpmn2:outgoing>Flow_185i1jc</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_0yozj8d" sf:guid="1d822e9f-4dfc-4836-953e-de7f9ce62204" sourceRef="StartEvent_1" targetRef="Activity_0w04ty5" sf:from="25b93434-80bd-412a-a847-2590a16351e6" sf:to="cfacd581-abfc-40ee-8a93-cae20aab4102" />
    <bpmn2:task id="Activity_0k588my" sf:guid="6f7fac31-4c23-429a-f4e3-65bf45ae086d" name="Task2">
      <bpmn2:extensionElements>
        <sf:performers>
          <sf:performer name="财务经理" outerId="14" outerCode="finacemanager" outerType="Role" />
        </sf:performers>
      </bpmn2:extensionElements>
      <bpmn2:incoming>Flow_185i1jc</bpmn2:incoming>
      <bpmn2:outgoing>Flow_09n04jj</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_185i1jc" sf:guid="0729434d-916b-4880-c4de-d6d42bcc9a13" sourceRef="Activity_0w04ty5" targetRef="Activity_0k588my" sf:from="cfacd581-abfc-40ee-8a93-cae20aab4102" sf:to="6f7fac31-4c23-429a-f4e3-65bf45ae086d" />
    <bpmn2:endEvent id="Event_0dr43ag" sf:guid="60602162-b3ca-41f7-fe85-3b2b393bae21" name="End">
      <bpmn2:incoming>Flow_1nke00d</bpmn2:incoming>
    </bpmn2:endEvent>
    <bpmn2:task id="Activity_1587h96" sf:guid="fa14ab01-a0f7-4e8d-ee54-6d9d50cc5858" name="Task3">
      <bpmn2:extensionElements>
        <sf:notifications>
          <sf:notification name="Bill" outerId="8" outerCode="" outerType="User" />
        </sf:notifications>
        <sf:forms />
      </bpmn2:extensionElements>
      <bpmn2:incoming>Flow_09n04jj</bpmn2:incoming>
      <bpmn2:outgoing>Flow_1nke00d</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_09n04jj" sf:guid="541271b0-d9f9-4384-8a83-580baba02afe" sourceRef="Activity_0k588my" targetRef="Activity_1587h96" sf:from="6f7fac31-4c23-429a-f4e3-65bf45ae086d" sf:to="fa14ab01-a0f7-4e8d-ee54-6d9d50cc5858" />
    <bpmn2:sequenceFlow id="Flow_1nke00d" sf:guid="68953a1e-2215-46dc-b68a-09f0102307ca" sourceRef="Activity_1587h96" targetRef="Event_0dr43ag" sf:from="fa14ab01-a0f7-4e8d-ee54-6d9d50cc5858" sf:to="60602162-b3ca-41f7-fe85-3b2b393bae21" />
  </bpmn2:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="pProcess_Code_9505">
      <bpmndi:BPMNEdge id="Flow_1nke00d_di" bpmnElement="Flow_1nke00d">
        <di:waypoint x="920" y="258" />
        <di:waypoint x="1022" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_09n04jj_di" bpmnElement="Flow_09n04jj">
        <di:waypoint x="760" y="258" />
        <di:waypoint x="820" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_185i1jc_di" bpmnElement="Flow_185i1jc">
        <di:waypoint x="600" y="258" />
        <di:waypoint x="660" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0yozj8d_di" bpmnElement="Flow_0yozj8d">
        <di:waypoint x="448" y="258" />
        <di:waypoint x="500" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="412" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="418" y="283" width="25" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0w04ty5_di" bpmnElement="Activity_0w04ty5">
        <dc:Bounds x="500" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0k588my_di" bpmnElement="Activity_0k588my">
        <dc:Bounds x="660" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_0dr43ag_di" bpmnElement="Event_0dr43ag">
        <dc:Bounds x="1022" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="1030" y="283" width="20" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1587h96_di" bpmnElement="Activity_1587h96">
        <dc:Bounds x="820" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn2:definitions>
', 0, NULL, NULL, 0, NULL, CAST(0x0000B2100137695D AS DateTime), CAST(0x0000B2490089FAF9 AS DateTime))
INSERT [dbo].[WfProcess] ([ID], [ProcessGUID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1494, N'8186fe2b-f3d7-4955-b915-8ed821941eb9', N'1', N'Sequence_4517', N'Sequence_Code_4517', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn2:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL" id="bpmn2-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn2:process id="Sequence_Code_4517" sf:guid="8186fe2b-f3d7-4955-b915-8ed821941eb9" sf:code="Sequence_Code_4517" name="Sequence_4517" isExecutable="true" sf:version="1">
    <bpmn2:startEvent id="StartNode_6564" sf:guid="a31a9bc4-f1f9-45d6-82aa-1b3c049ce6a5" sf:code="Start" name="Start">
      <bpmn2:outgoing>Flow_1983</bpmn2:outgoing>
    </bpmn2:startEvent>
    <bpmn2:task id="TaskNode_3273" sf:guid="28d7c3ae-bb9a-4177-8459-8b3f062c621c" sf:code="task001" name="Task-001">
      <bpmn2:incoming>Flow_1983</bpmn2:incoming>
      <bpmn2:outgoing>Flow_5817</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:task id="TaskNode_6944" sf:guid="7cd06127-c2d4-4a4b-aa27-6dd185c05934" sf:code="task002" name="Task-002">
      <bpmn2:incoming>Flow_5817</bpmn2:incoming>
      <bpmn2:outgoing>Flow_7285</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:task id="TaskNode_7643" sf:guid="bf0fbc56-3892-43e5-a90c-55b623604827" sf:code="task003" name="Task-003">
      <bpmn2:incoming>Flow_7285</bpmn2:incoming>
      <bpmn2:outgoing>Flow_8996</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:endEvent id="EndNode_7358" sf:guid="f97f5686-10e5-476b-8e1d-0ea3028d056a" sf:code="End" name="End">
      <bpmn2:incoming>Flow_8996</bpmn2:incoming>
    </bpmn2:endEvent>
    <bpmn2:sequenceFlow id="Flow_1983" sf:guid="033e5de4-b568-437b-a457-8dbff5c12f2a" name="" sourceRef="StartNode_6564" targetRef="TaskNode_3273" sf:from="a31a9bc4-f1f9-45d6-82aa-1b3c049ce6a5" sf:to="28d7c3ae-bb9a-4177-8459-8b3f062c621c" />
    <bpmn2:sequenceFlow id="Flow_5817" sf:guid="e140c874-e78a-4bf7-9369-86bb1b0fafce" name="t-001" sourceRef="TaskNode_3273" targetRef="TaskNode_6944" sf:from="28d7c3ae-bb9a-4177-8459-8b3f062c621c" sf:to="7cd06127-c2d4-4a4b-aa27-6dd185c05934" />
    <bpmn2:sequenceFlow id="Flow_7285" sf:guid="8bbaaabf-fcc8-4e4b-8286-f0f63e28da43" name="" sourceRef="TaskNode_6944" targetRef="TaskNode_7643" sf:from="7cd06127-c2d4-4a4b-aa27-6dd185c05934" sf:to="bf0fbc56-3892-43e5-a90c-55b623604827" />
    <bpmn2:sequenceFlow id="Flow_8996" sf:guid="f5c2ff0a-fe4a-449a-86a7-3ff232d21288" name="" sourceRef="TaskNode_7643" targetRef="EndNode_7358" sf:from="bf0fbc56-3892-43e5-a90c-55b623604827" sf:to="f97f5686-10e5-476b-8e1d-0ea3028d056a" />
  </bpmn2:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Sequence_Code_4517">
      <bpmndi:BPMNEdge id="Flow_8996_di" bpmnElement="Flow_8996">
        <di:waypoint x="816" y="198" />
        <di:waypoint x="896" y="198" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_7285_di" bpmnElement="Flow_7285">
        <di:waypoint x="636" y="198" />
        <di:waypoint x="716" y="198" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_5817_di" bpmnElement="Flow_5817">
        <di:waypoint x="456" y="198" />
        <di:waypoint x="536" y="198" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1983_di" bpmnElement="Flow_1983">
        <di:waypoint x="276" y="198" />
        <di:waypoint x="356" y="198" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNShape id="BPMNShape_h556hv5_di" bpmnElement="StartNode_6564">
        <dc:Bounds x="240" y="180" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="BPMNShape_g6vsjdx_di" bpmnElement="TaskNode_3273">
        <dc:Bounds x="356" y="158" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="BPMNShape_gy5eu2j_di" bpmnElement="TaskNode_6944">
        <dc:Bounds x="536" y="158" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="BPMNShape_l7xs466_di" bpmnElement="TaskNode_7643">
        <dc:Bounds x="716" y="158" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="BPMNShape_lldj0kv_di" bpmnElement="EndNode_7358">
        <dc:Bounds x="896" y="180" width="36" height="36" />
      </bpmndi:BPMNShape>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn2:definitions>
', 0, NULL, NULL, 0, NULL, CAST(0x0000B25600F97C61 AS DateTime), CAST(0x0000B25C01610613 AS DateTime))
INSERT [dbo].[WfProcess] ([ID], [ProcessGUID], [Version], [ProcessName], [ProcessCode], [IsUsing], [AppType], [PackageType], [PackageID], [ParticipantGUID], [PageUrl], [XmlFileName], [XmlFilePath], [XmlContent], [StartType], [StartExpression], [Description], [EndType], [EndExpression], [CreatedDateTime], [LastUpdatedDateTime]) VALUES (1495, N'bca4fd4c-1b31-4c96-9da2-1e086238dd31', N'1', N'Process_Name_4602', N'Process_Code_4602', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<bpmn2:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL" id="bpmn2-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn2:process id="pProcess_Code_4602" sf:guid="bca4fd4c-1b31-4c96-9da2-1e086238dd31" sf:code="Process_Code_4602" name="Process_Name_4602" isExecutable="true" sf:version="1">
    <bpmn2:startEvent id="StartEvent_1" sf:guid="f9a9cba7-03cc-4295-9552-5a32dce3a229" name="Start">
      <bpmn2:outgoing>Flow_08lyh0j</bpmn2:outgoing>
    </bpmn2:startEvent>
    <bpmn2:task id="Activity_06zabg7" sf:guid="0fa65889-2436-41bd-ac52-4fcaf807c1b2">
      <bpmn2:incoming>Flow_08lyh0j</bpmn2:incoming>
      <bpmn2:outgoing>Flow_186tcip</bpmn2:outgoing>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_08lyh0j" sf:guid="00665233-1de0-47c3-9eeb-18a70a87baf5" sourceRef="StartEvent_1" targetRef="Activity_06zabg7" sf:from="f9a9cba7-03cc-4295-9552-5a32dce3a229" sf:to="0fa65889-2436-41bd-ac52-4fcaf807c1b2" />
    <bpmn2:task id="Activity_01da7ha" sf:guid="fe1f2512-2188-4770-b74a-1a71be793ba0">
      <bpmn2:incoming>Flow_186tcip</bpmn2:incoming>
    </bpmn2:task>
    <bpmn2:sequenceFlow id="Flow_186tcip" sf:guid="d107fd31-a866-4e8e-c2a0-1b3fe3ad13fe" sourceRef="Activity_06zabg7" targetRef="Activity_01da7ha" sf:from="0fa65889-2436-41bd-ac52-4fcaf807c1b2" sf:to="fe1f2512-2188-4770-b74a-1a71be793ba0" />
  </bpmn2:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="pProcess_Code_4602">
      <bpmndi:BPMNEdge id="Flow_08lyh0j_di" bpmnElement="Flow_08lyh0j">
        <di:waypoint x="448" y="258" />
        <di:waypoint x="500" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_186tcip_di" bpmnElement="Flow_186tcip">
        <di:waypoint x="600" y="258" />
        <di:waypoint x="660" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="412" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="418" y="283" width="25" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_06zabg7_di" bpmnElement="Activity_06zabg7">
        <dc:Bounds x="500" y="218" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_01da7ha_di" bpmnElement="Activity_01da7ha">
        <dc:Bounds x="660" y="218" width="100" height="80" />
      </bpmndi:BPMNShape>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn2:definitions>
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        ', 0, NULL, NULL, 0, NULL, CAST(0x0000B25C016110FE AS DateTime), NULL)
SET IDENTITY_INSERT [dbo].[WfProcess] OFF
/****** Object:  Table [dbo].[WfLog]    Script Date: 01/06/2025 13:31:59 ******/
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
INSERT [dbo].[WfLog] ([ID], [EventTypeID], [Priority], [Severity], [Title], [Message], [StackTrace], [InnerStackTrace], [RequestData], [Timestamp]) VALUES (958, 2, 2, N'NORMAL', N'PROCESS TASK EMAL SEND ERROR', N'Mailbox name not allowed. The server response was: authentication is required', N'   at System.Net.Mail.MailCommand.CheckResponse(SmtpStatusCode statusCode, String response)
   at System.Net.Mail.MailCommand.EndSend(IAsyncResult result)
   at System.Net.Mail.SendMailAsyncResult.SendMailFromCompleted(IAsyncResult result)
--- End of stack trace from previous location ---
   at System.Net.Mail.SendMailAsyncResult.End(IAsyncResult result)
   at System.Net.Mail.SmtpClient.SendMailCallback(IAsyncResult result)', NULL, NULL, CAST(0x0000B256010AD757 AS DateTime))
SET IDENTITY_INSERT [dbo].[WfLog] OFF
/****** Object:  Table [dbo].[WfJobSchedule]    Script Date: 01/06/2025 13:31:59 ******/
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
/****** Object:  Table [dbo].[WfJobLog]    Script Date: 01/06/2025 13:31:59 ******/
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
/****** Object:  Table [dbo].[WfJobInfo]    Script Date: 01/06/2025 13:31:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[WfJobInfo](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ProcessGUID] [varchar](100) NOT NULL,
	[ProcessName] [nvarchar](50) NOT NULL,
	[Version] [nvarchar](20) NOT NULL,
	[ActivityGUID] [varchar](100) NOT NULL,
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
INSERT [dbo].[WfJobInfo] ([ID], [ProcessGUID], [ProcessName], [Version], [ActivityGUID], [ActivityName], [ActivityType], [TriggerType], [MessageDirection], [JobName], [Topic], [JobStatus], [CreatedDateTime], [CreatedUserID], [CreatedUserName], [LastUpdatedDateTime], [LastUpdatedUserID], [LastUpdatedUserName]) VALUES (3, N'214ad24b-9097-41de-8e74-a5913c518429', N'SignalInterProcess', N'1', N'c53f07ba-e65c-465b-d81f-ea992249712c', N'Event_0lt2165', N'IntermediateNode', N'Signal', N'Catch', N'IntermediateNode.Signal.Catch.OrderDistributed', N'OrderDistributed', N'Subscribed', CAST(0x0000B2010127D2B0 AS DateTime), N'ADMIN_1001', N'ADMINISTRATOR_JOB', CAST(0x0000B20201564FCA AS DateTime), N'ADMIN_1001', N'ADMINISTRATOR_JOB')
INSERT [dbo].[WfJobInfo] ([ID], [ProcessGUID], [ProcessName], [Version], [ActivityGUID], [ActivityName], [ActivityType], [TriggerType], [MessageDirection], [JobName], [Topic], [JobStatus], [CreatedDateTime], [CreatedUserID], [CreatedUserName], [LastUpdatedDateTime], [LastUpdatedUserID], [LastUpdatedUserName]) VALUES (4, N'2a7a60a3-f270-429f-98b6-e3c30c60b6e5', N'SignalStartProcess', N'1', N'9cbf4daf-0c9b-42d2-9773-bbeb1b45227b', N'Start', N'StartNode', N'Signal', N'Catch', N'StartNode.Signal.Catch.OrderDistributed', N'OrderDistributed', N'Subscribed', CAST(0x0000B20201556F66 AS DateTime), N'ADMIN_1001', N'ADMINISTRATOR_JOB', CAST(0x0000B20300CF421D AS DateTime), N'ADMIN_1001', N'ADMINISTRATOR_JOB')
SET IDENTITY_INSERT [dbo].[WfJobInfo] OFF
/****** Object:  StoredProcedure [dbo].[pr_com_QuerySQLPaged]    Script Date: 01/06/2025 13:32:00 ******/
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
/****** Object:  Table [dbo].[ManProductOrder]    Script Date: 01/06/2025 13:31:59 ******/
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
INSERT [dbo].[ManProductOrder] ([ID], [OrderCode], [Status], [ProductName], [Quantity], [UnitPrice], [TotalPrice], [CreatedTime], [CustomerName], [Address], [Mobile], [Remark], [LastUpdatedTime]) VALUES (699, N'TB738319', 4, N'AIVoice-E', 5, CAST(1000.00 AS Decimal(18, 2)), CAST(5000.00 AS Decimal(18, 2)), CAST(0x0000B25D00ABB0EF AS DateTime), N'PostUK', N'FuxingGate', N'931378', N'Store-A', CAST(0x0000B25D00B1B98B AS DateTime))
INSERT [dbo].[ManProductOrder] ([ID], [OrderCode], [Status], [ProductName], [Quantity], [UnitPrice], [TotalPrice], [CreatedTime], [CustomerName], [Address], [Mobile], [Remark], [LastUpdatedTime]) VALUES (700, N'TB895251', 5, N'LED-D', 6, CAST(1000.00 AS Decimal(18, 2)), CAST(6000.00 AS Decimal(18, 2)), CAST(0x0000B25D00B2313D AS DateTime), N'CitiBank', N'PuDongNewArea', N'188831', N'Store-C', CAST(0x0000B25D00B534E2 AS DateTime))
INSERT [dbo].[ManProductOrder] ([ID], [OrderCode], [Status], [ProductName], [Quantity], [UnitPrice], [TotalPrice], [CreatedTime], [CustomerName], [Address], [Mobile], [Remark], [LastUpdatedTime]) VALUES (701, N'TB986130', 8, N'LED-D', 8, CAST(1000.00 AS Decimal(18, 2)), CAST(8000.00 AS Decimal(18, 2)), CAST(0x0000B25D00B42737 AS DateTime), N'UBS', N'Wangfujing', N'427628', N'Store-B', CAST(0x0000B25D00B4B0BC AS DateTime))
INSERT [dbo].[ManProductOrder] ([ID], [OrderCode], [Status], [ProductName], [Quantity], [UnitPrice], [TotalPrice], [CreatedTime], [CustomerName], [Address], [Mobile], [Remark], [LastUpdatedTime]) VALUES (702, N'TB840430', 1, N'Aircraft-B', 4, CAST(1000.00 AS Decimal(18, 2)), CAST(4000.00 AS Decimal(18, 2)), CAST(0x0000B25D00C0FBBC AS DateTime), N'HACK-News', N'NewYork', N'268583', N'Store-F', NULL)
SET IDENTITY_INSERT [dbo].[ManProductOrder] OFF
/****** Object:  Table [dbo].[HrsLeaveOpinion]    Script Date: 01/06/2025 13:31:59 ******/
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
/****** Object:  Table [dbo].[HrsLeave]    Script Date: 01/06/2025 13:31:59 ******/
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
/****** Object:  UserDefinedFunction [dbo].[fn_com_SplitString]    Script Date: 01/06/2025 13:32:00 ******/
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
/****** Object:  Table [dbo].[FbFormProcess]    Script Date: 01/06/2025 13:31:59 ******/
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
/****** Object:  Table [dbo].[FbFormFieldEvent]    Script Date: 01/06/2025 13:31:59 ******/
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
/****** Object:  Table [dbo].[FbFormFieldActivityEdit]    Script Date: 01/06/2025 13:31:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FbFormFieldActivityEdit](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ProcessID] [int] NOT NULL,
	[ProcessGUID] [varchar](100) NOT NULL,
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
INSERT [dbo].[FbFormFieldActivityEdit] ([ID], [ProcessID], [ProcessGUID], [ProcessName], [ProcessVersion], [ActivityGUID], [ActivityName], [FormID], [FormName], [FormVersion], [FieldsPermission]) VALUES (6, 1493, N'b39c385c-15b1-4972-9b17-2fa9148e5fa9', N'Process_Name_9505', N'1', N'6f7fac31-4c23-429a-f4e3-65bf45ae086d', N'Task2', 65, N'
            Form_1u453x3Form_Test_Sample', N'1', N'[{"FieldName":"leavetype","IsNotVisible":false,"IsReadOnly":true},{"FieldName":"days","IsNotVisible":true,"IsReadOnly":false},{"FieldName":"remark","IsNotVisible":false,"IsReadOnly":false}]')
SET IDENTITY_INSERT [dbo].[FbFormFieldActivityEdit] OFF
/****** Object:  Table [dbo].[FbFormField]    Script Date: 01/06/2025 13:31:59 ******/
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
/****** Object:  Table [dbo].[FbFormData]    Script Date: 01/06/2025 13:31:59 ******/
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
/****** Object:  Table [dbo].[FbForm]    Script Date: 01/06/2025 13:31:59 ******/
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
INSERT [dbo].[FbForm] ([ID], [FormName], [FormCode], [Version], [FieldSummary], [TemplateContent], [HTMLContent], [Description], [CreatedDate], [LastUpdatedDate]) VALUES (64, N'Form_0quwv7l', N'BO94VE', N'1', NULL, N'{"components":[{"key":"Name","type":"textfield","label":"Name","id":"Field_1b686e0","layout":{"row":"Row_1hj7dcz"}},{"label":"Text field","type":"textfield","layout":{"row":"Row_1i6ymju","columns":null},"id":"Field_0jswdmy","key":"textfield_jwz4gt"},{"label":"Text area","type":"textarea","layout":{"row":"Row_0hjeuyx","columns":null},"id":"Field_1hxvsed","key":"textarea_fyb86a"},{"label":"Number","type":"number","layout":{"row":"Row_0rdn6s2","columns":null},"id":"Field_1ifnq2z","key":"number_wewpz8"}],"schemaVersion":16,"exporter":{"name":"form-js","version":"0.1.0"},"type":"default","id":"Form_0quwv7l"}', NULL, NULL, CAST(0x0000B1F1014ED95E AS DateTime), CAST(0x0000B1F1014EE476 AS DateTime))
INSERT [dbo].[FbForm] ([ID], [FormName], [FormCode], [Version], [FieldSummary], [TemplateContent], [HTMLContent], [Description], [CreatedDate], [LastUpdatedDate]) VALUES (65, N'Form_1u453x3', N'WNMNE4', N'1', N'["leavetype","days","remark"]', N'{"components":[{"values":[{"label":"Personal","value":"personal"},{"label":"Hospital","value":"hospital"},{"label":"Vacation","value":"vacation"}],"label":"LeaveType","type":"select","layout":{"row":"Row_07w2yaf","columns":null},"id":"Field_0n0s4m0","key":"leavetype","validate":{"required":true}},{"label":"Days","type":"number","layout":{"row":"Row_00mjdyd","columns":null},"id":"Field_1fngofn","key":"days","properties":{"IsCondition":"days"},"validate":{"required":true}},{"key":"remark","type":"textfield","label":"Remark","id":"Field_0ebnmue","layout":{"row":"Row_17zudw4"}}],"schemaVersion":16,"exporter":{"name":"form-js","version":"0.1.0"},"type":"default","id":"Form_1u453x3"}', NULL, NULL, CAST(0x0000B1F10151B3FA AS DateTime), CAST(0x0000B24401285EE8 AS DateTime))
SET IDENTITY_INSERT [dbo].[FbForm] OFF
/****** Object:  Table [dbo].[BizAppFlow]    Script Date: 01/06/2025 13:31:59 ******/
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
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (286, N'ProductOrder', N'699', N'TB738319', NULL, N'Disptach', N'Dispatch Completed', CAST(0x0000B25D00B19D63 AS DateTime), N'7', N'Peter')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (287, N'ProductOrder', N'699', N'TB738319', NULL, N'Sample', N'Sample Completed', CAST(0x0000B25D00B1B98A AS DateTime), N'11', N'Fisher')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (288, N'ProductOrder', N'700', N'TB895251', NULL, N'Disptach', N'Dispatch Completed', CAST(0x0000B25D00B247B1 AS DateTime), N'7', N'Peter')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (289, N'ProductOrder', N'700', N'TB895251', NULL, N'Sample', N'Sample Completed', CAST(0x0000B25D00B25ABB AS DateTime), N'11', N'Fisher')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (290, N'ProductOrder', N'701', N'TB986130', NULL, N'Disptach', N'Dispatch Completed', CAST(0x0000B25D00B43CCE AS DateTime), N'7', N'Peter')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (291, N'ProductOrder', N'701', N'TB986130', NULL, N'Sample', N'Sample Completed', CAST(0x0000B25D00B45323 AS DateTime), N'11', N'Fisher')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (292, N'ProductOrder', N'701', N'TB986130', NULL, N'Manufacture', N'Manufacture Completed', CAST(0x0000B25D00B47181 AS DateTime), N'9', N'Tuda')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (293, N'ProductOrder', N'701', N'TB986130', NULL, N'QC check', N'QC check complted', CAST(0x0000B25D00B492C1 AS DateTime), N'13', N'Jimi')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (294, N'ProductOrder', N'701', N'TB986130', NULL, N'Weight', N'Weight Completed', CAST(0x0000B25D00B4A742 AS DateTime), N'15', N'Damark')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (295, N'ProductOrder', N'701', N'TB986130', NULL, N'Delivery', N'Delivery Completed', CAST(0x0000B25D00B4B0BC AS DateTime), N'15', N'Damark')
INSERT [dbo].[BizAppFlow] ([ID], [AppName], [AppInstanceID], [AppInstanceCode], [Status], [ActivityName], [Remark], [ChangedTime], [ChangedUserID], [ChangedUserName]) VALUES (296, N'ProductOrder', N'700', N'TB895251', NULL, N'Sample', N'Sample Completed', CAST(0x0000B25D00B534E2 AS DateTime), N'9', N'Tuda')
SET IDENTITY_INSERT [dbo].[BizAppFlow] OFF
/****** Object:  Table [dbo].[tmpTest]    Script Date: 01/06/2025 13:31:59 ******/
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
/****** Object:  Table [dbo].[SysUserResource]    Script Date: 01/06/2025 13:31:59 ******/
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
/****** Object:  Table [dbo].[SysUser]    Script Date: 01/06/2025 13:31:59 ******/
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
INSERT [dbo].[SysUser] ([ID], [UserName], [EMail]) VALUES (11, N'Fisher', N'support@ruochisoft.com')
INSERT [dbo].[SysUser] ([ID], [UserName], [EMail]) VALUES (12, N'Sherley', N'hr@ruochisoft.com')
INSERT [dbo].[SysUser] ([ID], [UserName], [EMail]) VALUES (13, N'Jimi', NULL)
INSERT [dbo].[SysUser] ([ID], [UserName], [EMail]) VALUES (14, N'William', NULL)
INSERT [dbo].[SysUser] ([ID], [UserName], [EMail]) VALUES (15, N'Damark', NULL)
INSERT [dbo].[SysUser] ([ID], [UserName], [EMail]) VALUES (16, N'Smith', NULL)
INSERT [dbo].[SysUser] ([ID], [UserName], [EMail]) VALUES (17, N'Yolanda', NULL)
INSERT [dbo].[SysUser] ([ID], [UserName], [EMail]) VALUES (18, N'Jinny', NULL)
INSERT [dbo].[SysUser] ([ID], [UserName], [EMail]) VALUES (19, N'Susan', N'hr@ruochisoft.com')
SET IDENTITY_INSERT [dbo].[SysUser] OFF
/****** Object:  Table [dbo].[SysRoleUser]    Script Date: 01/06/2025 13:31:59 ******/
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
/****** Object:  Table [dbo].[SysRoleGroupResource]    Script Date: 01/06/2025 13:31:59 ******/
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
/****** Object:  Table [dbo].[SysRole]    Script Date: 01/06/2025 13:31:59 ******/
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
INSERT [dbo].[SysRole] ([ID], [RoleCode], [RoleName]) VALUES (1, N'employees', N'Employee(普通员工)')
INSERT [dbo].[SysRole] ([ID], [RoleCode], [RoleName]) VALUES (2, N'depmanager', N'Manager(部门经理)')
INSERT [dbo].[SysRole] ([ID], [RoleCode], [RoleName]) VALUES (3, N'hrmanager', N'HR(人事经理)')
INSERT [dbo].[SysRole] ([ID], [RoleCode], [RoleName]) VALUES (4, N'director', N'Director(主管总监)')
INSERT [dbo].[SysRole] ([ID], [RoleCode], [RoleName]) VALUES (7, N'deputygeneralmanager', N'DuputManager(副总经理)')
INSERT [dbo].[SysRole] ([ID], [RoleCode], [RoleName]) VALUES (8, N'generalmanager', N'(CEO)总经理')
INSERT [dbo].[SysRole] ([ID], [RoleCode], [RoleName]) VALUES (9, N'salesmate', N'Salesman(业务员)')
INSERT [dbo].[SysRole] ([ID], [RoleCode], [RoleName]) VALUES (10, N'techmate', N'SampleMate(打样员)')
INSERT [dbo].[SysRole] ([ID], [RoleCode], [RoleName]) VALUES (11, N'merchandiser', N'Merchaandiser(跟单员)')
INSERT [dbo].[SysRole] ([ID], [RoleCode], [RoleName]) VALUES (12, N'qcmate', N'QCMate(质检员)')
INSERT [dbo].[SysRole] ([ID], [RoleCode], [RoleName]) VALUES (13, N'expressmate', N'ExpressMate(包装员)')
INSERT [dbo].[SysRole] ([ID], [RoleCode], [RoleName]) VALUES (14, N'finacemanager', N'FinaceManager(财务经理)')
INSERT [dbo].[SysRole] ([ID], [RoleCode], [RoleName]) VALUES (21, N'testrole', N'testrole')
SET IDENTITY_INSERT [dbo].[SysRole] OFF
/****** Object:  Table [dbo].[SysResource]    Script Date: 01/06/2025 13:31:59 ******/
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
/****** Object:  Table [dbo].[SysEmployeeManager]    Script Date: 01/06/2025 13:31:59 ******/
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
/****** Object:  Table [dbo].[SysEmployee]    Script Date: 01/06/2025 13:31:59 ******/
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
/****** Object:  Table [dbo].[SysDepartment]    Script Date: 01/06/2025 13:31:59 ******/
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
/****** Object:  StoredProcedure [dbo].[pr_sys_UserSave]    Script Date: 01/06/2025 13:32:00 ******/
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
/****** Object:  StoredProcedure [dbo].[pr_sys_UserDelete]    Script Date: 01/06/2025 13:32:00 ******/
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
/****** Object:  StoredProcedure [dbo].[pr_sys_RoleUserDelete]    Script Date: 01/06/2025 13:32:00 ******/
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
/****** Object:  StoredProcedure [dbo].[pr_sys_RoleSave]    Script Date: 01/06/2025 13:32:00 ******/
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
/****** Object:  StoredProcedure [dbo].[pr_sys_RoleDelete]    Script Date: 01/06/2025 13:32:00 ******/
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
/****** Object:  StoredProcedure [dbo].[pr_sys_DeptUserListRankQuery]    Script Date: 01/06/2025 13:32:00 ******/
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
/****** Object:  StoredProcedure [dbo].[pr_fb_FormDelete]    Script Date: 01/06/2025 13:32:00 ******/
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
/****** Object:  Table [dbo].[WfActivityInstance]    Script Date: 01/06/2025 13:31:59 ******/
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
	[ProcessGUID] [varchar](100) NOT NULL,
	[ActivityGUID] [varchar](100) NOT NULL,
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
INSERT [dbo].[WfActivityInstance] ([ID], [ProcessInstanceID], [AppName], [AppInstanceID], [AppInstanceCode], [ProcessGUID], [ActivityGUID], [ActivityName], [ActivityCode], [ActivityType], [ActivityState], [WorkItemType], [AssignedToUserIDs], [AssignedToUserNames], [BackwardType], [BackSrcActivityInstanceID], [BackOrgActivityInstanceID], [GatewayDirectionTypeID], [CanNotRenewInstance], [ApprovalStatus], [TokensRequired], [TokensHad], [JobTimerType], [JobTimerStatus], [TriggerExpression], [OverdueDateTime], [JobTimerTreatedDateTime], [ComplexType], [MergeType], [MIHostActivityInstanceID], [CompareType], [CompleteOrder], [SignForwardType], [NextStepPerformers], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [LastUpdatedDateTime], [EndedDateTime], [EndedByUserID], [EndedByUserName], [RecordStatusInvalid]) VALUES (1968, 485, N'SamplePrice', N'100', NULL, N'8186fe2b-f3d7-4955-b915-8ed821941eb9', N'a31a9bc4-f1f9-45d6-82aa-1b3c049ce6a5', N'Start', N'Start', 1, 4, 0, NULL, NULL, 0, NULL, NULL, NULL, 0, 0, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'10', N'Long', CAST(0x0000B256010AD664 AS DateTime), NULL, NULL, NULL, CAST(0x0000B256010AD66D AS DateTime), N'10', N'Long', 0)
INSERT [dbo].[WfActivityInstance] ([ID], [ProcessInstanceID], [AppName], [AppInstanceID], [AppInstanceCode], [ProcessGUID], [ActivityGUID], [ActivityName], [ActivityCode], [ActivityType], [ActivityState], [WorkItemType], [AssignedToUserIDs], [AssignedToUserNames], [BackwardType], [BackSrcActivityInstanceID], [BackOrgActivityInstanceID], [GatewayDirectionTypeID], [CanNotRenewInstance], [ApprovalStatus], [TokensRequired], [TokensHad], [JobTimerType], [JobTimerStatus], [TriggerExpression], [OverdueDateTime], [JobTimerTreatedDateTime], [ComplexType], [MergeType], [MIHostActivityInstanceID], [CompareType], [CompleteOrder], [SignForwardType], [NextStepPerformers], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [LastUpdatedDateTime], [EndedDateTime], [EndedByUserID], [EndedByUserName], [RecordStatusInvalid]) VALUES (1969, 485, N'SamplePrice', N'100', NULL, N'8186fe2b-f3d7-4955-b915-8ed821941eb9', N'28d7c3ae-bb9a-4177-8459-8b3f062c621c', N'Task-001', N'task001', 4, 1, 1, N'10', N'Long', 0, NULL, NULL, NULL, 0, 0, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'10', N'Long', CAST(0x0000B256010AD671 AS DateTime), NULL, NULL, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[WfActivityInstance] ([ID], [ProcessInstanceID], [AppName], [AppInstanceID], [AppInstanceCode], [ProcessGUID], [ActivityGUID], [ActivityName], [ActivityCode], [ActivityType], [ActivityState], [WorkItemType], [AssignedToUserIDs], [AssignedToUserNames], [BackwardType], [BackSrcActivityInstanceID], [BackOrgActivityInstanceID], [GatewayDirectionTypeID], [CanNotRenewInstance], [ApprovalStatus], [TokensRequired], [TokensHad], [JobTimerType], [JobTimerStatus], [TriggerExpression], [OverdueDateTime], [JobTimerTreatedDateTime], [ComplexType], [MergeType], [MIHostActivityInstanceID], [CompareType], [CompleteOrder], [SignForwardType], [NextStepPerformers], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [LastUpdatedDateTime], [EndedDateTime], [EndedByUserID], [EndedByUserName], [RecordStatusInvalid]) VALUES (1970, 486, N'ProductOrder', N'699', NULL, N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', N'0253ff58-47f1-4203-9986-ef4d3e49199d', N'Start', NULL, 1, 4, 0, NULL, NULL, 0, NULL, NULL, NULL, 0, 0, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'7', N'Peter', CAST(0x0000B25D00B19D37 AS DateTime), NULL, NULL, NULL, CAST(0x0000B25D00B19D3D AS DateTime), N'7', N'Peter', 0)
INSERT [dbo].[WfActivityInstance] ([ID], [ProcessInstanceID], [AppName], [AppInstanceID], [AppInstanceCode], [ProcessGUID], [ActivityGUID], [ActivityName], [ActivityCode], [ActivityType], [ActivityState], [WorkItemType], [AssignedToUserIDs], [AssignedToUserNames], [BackwardType], [BackSrcActivityInstanceID], [BackOrgActivityInstanceID], [GatewayDirectionTypeID], [CanNotRenewInstance], [ApprovalStatus], [TokensRequired], [TokensHad], [JobTimerType], [JobTimerStatus], [TriggerExpression], [OverdueDateTime], [JobTimerTreatedDateTime], [ComplexType], [MergeType], [MIHostActivityInstanceID], [CompareType], [CompleteOrder], [SignForwardType], [NextStepPerformers], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [LastUpdatedDateTime], [EndedDateTime], [EndedByUserID], [EndedByUserName], [RecordStatusInvalid]) VALUES (1971, 486, N'ProductOrder', N'699', NULL, N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', N'62b71f52-2c6d-4476-d3c8-bd5f9ac1b94f', N'派单', N'Dispatching', 4, 4, 1, N'7', N'Peter', 0, NULL, NULL, NULL, 0, 0, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'7', N'Peter', CAST(0x0000B25D00B19D42 AS DateTime), NULL, NULL, NULL, CAST(0x0000B25D00B19D5D AS DateTime), N'7', N'Peter', 0)
INSERT [dbo].[WfActivityInstance] ([ID], [ProcessInstanceID], [AppName], [AppInstanceID], [AppInstanceCode], [ProcessGUID], [ActivityGUID], [ActivityName], [ActivityCode], [ActivityType], [ActivityState], [WorkItemType], [AssignedToUserIDs], [AssignedToUserNames], [BackwardType], [BackSrcActivityInstanceID], [BackOrgActivityInstanceID], [GatewayDirectionTypeID], [CanNotRenewInstance], [ApprovalStatus], [TokensRequired], [TokensHad], [JobTimerType], [JobTimerStatus], [TriggerExpression], [OverdueDateTime], [JobTimerTreatedDateTime], [ComplexType], [MergeType], [MIHostActivityInstanceID], [CompareType], [CompleteOrder], [SignForwardType], [NextStepPerformers], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [LastUpdatedDateTime], [EndedDateTime], [EndedByUserID], [EndedByUserName], [RecordStatusInvalid]) VALUES (1972, 486, N'ProductOrder', N'699', NULL, N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', N'1bc8a032-ab92-4f6e-9bf1-28e87e5a6ece', N'Gateway_1qnv0ou', NULL, 8, 4, 0, NULL, NULL, 0, NULL, NULL, 2, 0, 0, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'7', N'Peter', CAST(0x0000B25D00B19D5F AS DateTime), NULL, NULL, NULL, CAST(0x0000B25D00B19D5F AS DateTime), N'7', N'Peter', 0)
INSERT [dbo].[WfActivityInstance] ([ID], [ProcessInstanceID], [AppName], [AppInstanceID], [AppInstanceCode], [ProcessGUID], [ActivityGUID], [ActivityName], [ActivityCode], [ActivityType], [ActivityState], [WorkItemType], [AssignedToUserIDs], [AssignedToUserNames], [BackwardType], [BackSrcActivityInstanceID], [BackOrgActivityInstanceID], [GatewayDirectionTypeID], [CanNotRenewInstance], [ApprovalStatus], [TokensRequired], [TokensHad], [JobTimerType], [JobTimerStatus], [TriggerExpression], [OverdueDateTime], [JobTimerTreatedDateTime], [ComplexType], [MergeType], [MIHostActivityInstanceID], [CompareType], [CompleteOrder], [SignForwardType], [NextStepPerformers], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [LastUpdatedDateTime], [EndedDateTime], [EndedByUserID], [EndedByUserName], [RecordStatusInvalid]) VALUES (1973, 486, N'ProductOrder', N'699', NULL, N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', N'126ed9cc-b661-4e77-ae95-b5001ad9ce9c', N'打样', N'Sampling', 4, 4, 1, N'11,12', N'Fisher,Sherley', 0, NULL, NULL, NULL, 0, 0, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'7', N'Peter', CAST(0x0000B25D00B19D62 AS DateTime), NULL, NULL, NULL, CAST(0x0000B25D00B1B989 AS DateTime), N'11', N'Fisher', 0)
INSERT [dbo].[WfActivityInstance] ([ID], [ProcessInstanceID], [AppName], [AppInstanceID], [AppInstanceCode], [ProcessGUID], [ActivityGUID], [ActivityName], [ActivityCode], [ActivityType], [ActivityState], [WorkItemType], [AssignedToUserIDs], [AssignedToUserNames], [BackwardType], [BackSrcActivityInstanceID], [BackOrgActivityInstanceID], [GatewayDirectionTypeID], [CanNotRenewInstance], [ApprovalStatus], [TokensRequired], [TokensHad], [JobTimerType], [JobTimerStatus], [TriggerExpression], [OverdueDateTime], [JobTimerTreatedDateTime], [ComplexType], [MergeType], [MIHostActivityInstanceID], [CompareType], [CompleteOrder], [SignForwardType], [NextStepPerformers], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [LastUpdatedDateTime], [EndedDateTime], [EndedByUserID], [EndedByUserName], [RecordStatusInvalid]) VALUES (1974, 486, N'ProductOrder', N'699', NULL, N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', N'5b6ba25a-8dd4-40c4-d27e-f834f0be0168', N'生产', N'Manufacturing', 4, 1, 1, N'9,10,15,16', N'Tuda,Jack,Damark,Smith', 0, NULL, NULL, NULL, 0, 0, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'11', N'Fisher', CAST(0x0000B25D00B1B98A AS DateTime), NULL, NULL, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[WfActivityInstance] ([ID], [ProcessInstanceID], [AppName], [AppInstanceID], [AppInstanceCode], [ProcessGUID], [ActivityGUID], [ActivityName], [ActivityCode], [ActivityType], [ActivityState], [WorkItemType], [AssignedToUserIDs], [AssignedToUserNames], [BackwardType], [BackSrcActivityInstanceID], [BackOrgActivityInstanceID], [GatewayDirectionTypeID], [CanNotRenewInstance], [ApprovalStatus], [TokensRequired], [TokensHad], [JobTimerType], [JobTimerStatus], [TriggerExpression], [OverdueDateTime], [JobTimerTreatedDateTime], [ComplexType], [MergeType], [MIHostActivityInstanceID], [CompareType], [CompleteOrder], [SignForwardType], [NextStepPerformers], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [LastUpdatedDateTime], [EndedDateTime], [EndedByUserID], [EndedByUserName], [RecordStatusInvalid]) VALUES (1975, 487, N'ProductOrder', N'700', NULL, N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', N'0253ff58-47f1-4203-9986-ef4d3e49199d', N'Start', NULL, 1, 4, 0, NULL, NULL, 0, NULL, NULL, NULL, 0, 0, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'7', N'Peter', CAST(0x0000B25D00B247AC AS DateTime), NULL, NULL, NULL, CAST(0x0000B25D00B247AD AS DateTime), N'7', N'Peter', 0)
INSERT [dbo].[WfActivityInstance] ([ID], [ProcessInstanceID], [AppName], [AppInstanceID], [AppInstanceCode], [ProcessGUID], [ActivityGUID], [ActivityName], [ActivityCode], [ActivityType], [ActivityState], [WorkItemType], [AssignedToUserIDs], [AssignedToUserNames], [BackwardType], [BackSrcActivityInstanceID], [BackOrgActivityInstanceID], [GatewayDirectionTypeID], [CanNotRenewInstance], [ApprovalStatus], [TokensRequired], [TokensHad], [JobTimerType], [JobTimerStatus], [TriggerExpression], [OverdueDateTime], [JobTimerTreatedDateTime], [ComplexType], [MergeType], [MIHostActivityInstanceID], [CompareType], [CompleteOrder], [SignForwardType], [NextStepPerformers], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [LastUpdatedDateTime], [EndedDateTime], [EndedByUserID], [EndedByUserName], [RecordStatusInvalid]) VALUES (1976, 487, N'ProductOrder', N'700', NULL, N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', N'62b71f52-2c6d-4476-d3c8-bd5f9ac1b94f', N'派单', N'Dispatching', 4, 4, 1, N'7', N'Peter', 0, NULL, NULL, NULL, 0, 0, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'7', N'Peter', CAST(0x0000B25D00B247AD AS DateTime), NULL, NULL, NULL, CAST(0x0000B25D00B247AF AS DateTime), N'7', N'Peter', 0)
INSERT [dbo].[WfActivityInstance] ([ID], [ProcessInstanceID], [AppName], [AppInstanceID], [AppInstanceCode], [ProcessGUID], [ActivityGUID], [ActivityName], [ActivityCode], [ActivityType], [ActivityState], [WorkItemType], [AssignedToUserIDs], [AssignedToUserNames], [BackwardType], [BackSrcActivityInstanceID], [BackOrgActivityInstanceID], [GatewayDirectionTypeID], [CanNotRenewInstance], [ApprovalStatus], [TokensRequired], [TokensHad], [JobTimerType], [JobTimerStatus], [TriggerExpression], [OverdueDateTime], [JobTimerTreatedDateTime], [ComplexType], [MergeType], [MIHostActivityInstanceID], [CompareType], [CompleteOrder], [SignForwardType], [NextStepPerformers], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [LastUpdatedDateTime], [EndedDateTime], [EndedByUserID], [EndedByUserName], [RecordStatusInvalid]) VALUES (1977, 487, N'ProductOrder', N'700', NULL, N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', N'1bc8a032-ab92-4f6e-9bf1-28e87e5a6ece', N'Gateway_1qnv0ou', NULL, 8, 4, 0, NULL, NULL, 0, NULL, NULL, 2, 0, 0, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'7', N'Peter', CAST(0x0000B25D00B247B0 AS DateTime), NULL, NULL, NULL, CAST(0x0000B25D00B247B0 AS DateTime), N'7', N'Peter', 0)
INSERT [dbo].[WfActivityInstance] ([ID], [ProcessInstanceID], [AppName], [AppInstanceID], [AppInstanceCode], [ProcessGUID], [ActivityGUID], [ActivityName], [ActivityCode], [ActivityType], [ActivityState], [WorkItemType], [AssignedToUserIDs], [AssignedToUserNames], [BackwardType], [BackSrcActivityInstanceID], [BackOrgActivityInstanceID], [GatewayDirectionTypeID], [CanNotRenewInstance], [ApprovalStatus], [TokensRequired], [TokensHad], [JobTimerType], [JobTimerStatus], [TriggerExpression], [OverdueDateTime], [JobTimerTreatedDateTime], [ComplexType], [MergeType], [MIHostActivityInstanceID], [CompareType], [CompleteOrder], [SignForwardType], [NextStepPerformers], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [LastUpdatedDateTime], [EndedDateTime], [EndedByUserID], [EndedByUserName], [RecordStatusInvalid]) VALUES (1978, 487, N'ProductOrder', N'700', NULL, N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', N'126ed9cc-b661-4e77-ae95-b5001ad9ce9c', N'打样', N'Sampling', 4, 4, 1, N'11,12', N'Fisher,Sherley', 0, NULL, NULL, NULL, 0, 0, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'7', N'Peter', CAST(0x0000B25D00B247B1 AS DateTime), NULL, NULL, NULL, CAST(0x0000B25D00B25ABA AS DateTime), N'11', N'Fisher', 0)
INSERT [dbo].[WfActivityInstance] ([ID], [ProcessInstanceID], [AppName], [AppInstanceID], [AppInstanceCode], [ProcessGUID], [ActivityGUID], [ActivityName], [ActivityCode], [ActivityType], [ActivityState], [WorkItemType], [AssignedToUserIDs], [AssignedToUserNames], [BackwardType], [BackSrcActivityInstanceID], [BackOrgActivityInstanceID], [GatewayDirectionTypeID], [CanNotRenewInstance], [ApprovalStatus], [TokensRequired], [TokensHad], [JobTimerType], [JobTimerStatus], [TriggerExpression], [OverdueDateTime], [JobTimerTreatedDateTime], [ComplexType], [MergeType], [MIHostActivityInstanceID], [CompareType], [CompleteOrder], [SignForwardType], [NextStepPerformers], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [LastUpdatedDateTime], [EndedDateTime], [EndedByUserID], [EndedByUserName], [RecordStatusInvalid]) VALUES (1979, 487, N'ProductOrder', N'700', NULL, N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', N'5b6ba25a-8dd4-40c4-d27e-f834f0be0168', N'生产', N'Manufacturing', 4, 4, 1, N'9,10,15,16', N'Tuda,Jack,Damark,Smith', 0, NULL, NULL, NULL, 0, 0, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'11', N'Fisher', CAST(0x0000B25D00B25ABA AS DateTime), NULL, NULL, NULL, CAST(0x0000B25D00B534D8 AS DateTime), N'9', N'Tuda', 0)
INSERT [dbo].[WfActivityInstance] ([ID], [ProcessInstanceID], [AppName], [AppInstanceID], [AppInstanceCode], [ProcessGUID], [ActivityGUID], [ActivityName], [ActivityCode], [ActivityType], [ActivityState], [WorkItemType], [AssignedToUserIDs], [AssignedToUserNames], [BackwardType], [BackSrcActivityInstanceID], [BackOrgActivityInstanceID], [GatewayDirectionTypeID], [CanNotRenewInstance], [ApprovalStatus], [TokensRequired], [TokensHad], [JobTimerType], [JobTimerStatus], [TriggerExpression], [OverdueDateTime], [JobTimerTreatedDateTime], [ComplexType], [MergeType], [MIHostActivityInstanceID], [CompareType], [CompleteOrder], [SignForwardType], [NextStepPerformers], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [LastUpdatedDateTime], [EndedDateTime], [EndedByUserID], [EndedByUserName], [RecordStatusInvalid]) VALUES (1980, 488, N'ProductOrder', N'701', NULL, N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', N'0253ff58-47f1-4203-9986-ef4d3e49199d', N'Start', NULL, 1, 4, 0, NULL, NULL, 0, NULL, NULL, NULL, 0, 0, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'7', N'Peter', CAST(0x0000B25D00B43CAE AS DateTime), NULL, NULL, NULL, CAST(0x0000B25D00B43CB2 AS DateTime), N'7', N'Peter', 0)
INSERT [dbo].[WfActivityInstance] ([ID], [ProcessInstanceID], [AppName], [AppInstanceID], [AppInstanceCode], [ProcessGUID], [ActivityGUID], [ActivityName], [ActivityCode], [ActivityType], [ActivityState], [WorkItemType], [AssignedToUserIDs], [AssignedToUserNames], [BackwardType], [BackSrcActivityInstanceID], [BackOrgActivityInstanceID], [GatewayDirectionTypeID], [CanNotRenewInstance], [ApprovalStatus], [TokensRequired], [TokensHad], [JobTimerType], [JobTimerStatus], [TriggerExpression], [OverdueDateTime], [JobTimerTreatedDateTime], [ComplexType], [MergeType], [MIHostActivityInstanceID], [CompareType], [CompleteOrder], [SignForwardType], [NextStepPerformers], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [LastUpdatedDateTime], [EndedDateTime], [EndedByUserID], [EndedByUserName], [RecordStatusInvalid]) VALUES (1981, 488, N'ProductOrder', N'701', NULL, N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', N'62b71f52-2c6d-4476-d3c8-bd5f9ac1b94f', N'Dispatch(派单)', N'Dispatching', 4, 4, 1, N'7', N'Peter', 0, NULL, NULL, NULL, 0, 0, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'7', N'Peter', CAST(0x0000B25D00B43CB6 AS DateTime), NULL, NULL, NULL, CAST(0x0000B25D00B43CC9 AS DateTime), N'7', N'Peter', 0)
INSERT [dbo].[WfActivityInstance] ([ID], [ProcessInstanceID], [AppName], [AppInstanceID], [AppInstanceCode], [ProcessGUID], [ActivityGUID], [ActivityName], [ActivityCode], [ActivityType], [ActivityState], [WorkItemType], [AssignedToUserIDs], [AssignedToUserNames], [BackwardType], [BackSrcActivityInstanceID], [BackOrgActivityInstanceID], [GatewayDirectionTypeID], [CanNotRenewInstance], [ApprovalStatus], [TokensRequired], [TokensHad], [JobTimerType], [JobTimerStatus], [TriggerExpression], [OverdueDateTime], [JobTimerTreatedDateTime], [ComplexType], [MergeType], [MIHostActivityInstanceID], [CompareType], [CompleteOrder], [SignForwardType], [NextStepPerformers], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [LastUpdatedDateTime], [EndedDateTime], [EndedByUserID], [EndedByUserName], [RecordStatusInvalid]) VALUES (1982, 488, N'ProductOrder', N'701', NULL, N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', N'1bc8a032-ab92-4f6e-9bf1-28e87e5a6ece', N'Gateway_1qnv0ou', NULL, 8, 4, 0, NULL, NULL, 0, NULL, NULL, 2, 0, 0, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'7', N'Peter', CAST(0x0000B25D00B43CCB AS DateTime), NULL, NULL, NULL, CAST(0x0000B25D00B43CCB AS DateTime), N'7', N'Peter', 0)
INSERT [dbo].[WfActivityInstance] ([ID], [ProcessInstanceID], [AppName], [AppInstanceID], [AppInstanceCode], [ProcessGUID], [ActivityGUID], [ActivityName], [ActivityCode], [ActivityType], [ActivityState], [WorkItemType], [AssignedToUserIDs], [AssignedToUserNames], [BackwardType], [BackSrcActivityInstanceID], [BackOrgActivityInstanceID], [GatewayDirectionTypeID], [CanNotRenewInstance], [ApprovalStatus], [TokensRequired], [TokensHad], [JobTimerType], [JobTimerStatus], [TriggerExpression], [OverdueDateTime], [JobTimerTreatedDateTime], [ComplexType], [MergeType], [MIHostActivityInstanceID], [CompareType], [CompleteOrder], [SignForwardType], [NextStepPerformers], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [LastUpdatedDateTime], [EndedDateTime], [EndedByUserID], [EndedByUserName], [RecordStatusInvalid]) VALUES (1983, 488, N'ProductOrder', N'701', NULL, N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', N'126ed9cc-b661-4e77-ae95-b5001ad9ce9c', N'Sample(打样)', N'Sampling', 4, 4, 1, N'11,12', N'Fisher,Sherley', 0, NULL, NULL, NULL, 0, 0, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'7', N'Peter', CAST(0x0000B25D00B43CCD AS DateTime), NULL, NULL, NULL, CAST(0x0000B25D00B45322 AS DateTime), N'11', N'Fisher', 0)
INSERT [dbo].[WfActivityInstance] ([ID], [ProcessInstanceID], [AppName], [AppInstanceID], [AppInstanceCode], [ProcessGUID], [ActivityGUID], [ActivityName], [ActivityCode], [ActivityType], [ActivityState], [WorkItemType], [AssignedToUserIDs], [AssignedToUserNames], [BackwardType], [BackSrcActivityInstanceID], [BackOrgActivityInstanceID], [GatewayDirectionTypeID], [CanNotRenewInstance], [ApprovalStatus], [TokensRequired], [TokensHad], [JobTimerType], [JobTimerStatus], [TriggerExpression], [OverdueDateTime], [JobTimerTreatedDateTime], [ComplexType], [MergeType], [MIHostActivityInstanceID], [CompareType], [CompleteOrder], [SignForwardType], [NextStepPerformers], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [LastUpdatedDateTime], [EndedDateTime], [EndedByUserID], [EndedByUserName], [RecordStatusInvalid]) VALUES (1984, 488, N'ProductOrder', N'701', NULL, N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', N'5b6ba25a-8dd4-40c4-d27e-f834f0be0168', N'Manufacture(生产)', N'Manufacturing', 4, 4, 1, N'9,10,15,16', N'Tuda,Jack,Damark,Smith', 0, NULL, NULL, NULL, 0, 0, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'11', N'Fisher', CAST(0x0000B25D00B45323 AS DateTime), NULL, NULL, NULL, CAST(0x0000B25D00B47180 AS DateTime), N'9', N'Tuda', 0)
INSERT [dbo].[WfActivityInstance] ([ID], [ProcessInstanceID], [AppName], [AppInstanceID], [AppInstanceCode], [ProcessGUID], [ActivityGUID], [ActivityName], [ActivityCode], [ActivityType], [ActivityState], [WorkItemType], [AssignedToUserIDs], [AssignedToUserNames], [BackwardType], [BackSrcActivityInstanceID], [BackOrgActivityInstanceID], [GatewayDirectionTypeID], [CanNotRenewInstance], [ApprovalStatus], [TokensRequired], [TokensHad], [JobTimerType], [JobTimerStatus], [TriggerExpression], [OverdueDateTime], [JobTimerTreatedDateTime], [ComplexType], [MergeType], [MIHostActivityInstanceID], [CompareType], [CompleteOrder], [SignForwardType], [NextStepPerformers], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [LastUpdatedDateTime], [EndedDateTime], [EndedByUserID], [EndedByUserName], [RecordStatusInvalid]) VALUES (1985, 488, N'ProductOrder', N'701', NULL, N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', N'78a69a65-d406-4056-9dc2-b25751fc6263', N'QCCheck(质检)', N'QCChecking', 4, 4, 1, N'13,14', N'Jimi,William', 0, NULL, NULL, NULL, 0, 0, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'9', N'Tuda', CAST(0x0000B25D00B47180 AS DateTime), NULL, NULL, NULL, CAST(0x0000B25D00B492C0 AS DateTime), N'13', N'Jimi', 0)
INSERT [dbo].[WfActivityInstance] ([ID], [ProcessInstanceID], [AppName], [AppInstanceID], [AppInstanceCode], [ProcessGUID], [ActivityGUID], [ActivityName], [ActivityCode], [ActivityType], [ActivityState], [WorkItemType], [AssignedToUserIDs], [AssignedToUserNames], [BackwardType], [BackSrcActivityInstanceID], [BackOrgActivityInstanceID], [GatewayDirectionTypeID], [CanNotRenewInstance], [ApprovalStatus], [TokensRequired], [TokensHad], [JobTimerType], [JobTimerStatus], [TriggerExpression], [OverdueDateTime], [JobTimerTreatedDateTime], [ComplexType], [MergeType], [MIHostActivityInstanceID], [CompareType], [CompleteOrder], [SignForwardType], [NextStepPerformers], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [LastUpdatedDateTime], [EndedDateTime], [EndedByUserID], [EndedByUserName], [RecordStatusInvalid]) VALUES (1986, 488, N'ProductOrder', N'701', NULL, N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', N'1c1bceae-7bce-4394-cc0b-6bbf1a87bf2d', N'Weight(称重)', N'Weighting', 4, 4, 1, N'15,16', N'Damark,Smith', 0, NULL, NULL, NULL, 0, 0, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'13', N'Jimi', CAST(0x0000B25D00B492C1 AS DateTime), NULL, NULL, NULL, CAST(0x0000B25D00B4A741 AS DateTime), N'15', N'Damark', 0)
INSERT [dbo].[WfActivityInstance] ([ID], [ProcessInstanceID], [AppName], [AppInstanceID], [AppInstanceCode], [ProcessGUID], [ActivityGUID], [ActivityName], [ActivityCode], [ActivityType], [ActivityState], [WorkItemType], [AssignedToUserIDs], [AssignedToUserNames], [BackwardType], [BackSrcActivityInstanceID], [BackOrgActivityInstanceID], [GatewayDirectionTypeID], [CanNotRenewInstance], [ApprovalStatus], [TokensRequired], [TokensHad], [JobTimerType], [JobTimerStatus], [TriggerExpression], [OverdueDateTime], [JobTimerTreatedDateTime], [ComplexType], [MergeType], [MIHostActivityInstanceID], [CompareType], [CompleteOrder], [SignForwardType], [NextStepPerformers], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [LastUpdatedDateTime], [EndedDateTime], [EndedByUserID], [EndedByUserName], [RecordStatusInvalid]) VALUES (1987, 488, N'ProductOrder', N'701', NULL, N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', N'b112b4d0-1cc1-4667-bdda-73c85e16266e', N'Print(打印发货单)', N'Delivering', 4, 4, 1, N'15,16', N'Damark,Smith', 0, NULL, NULL, NULL, 0, 0, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'15', N'Damark', CAST(0x0000B25D00B4A742 AS DateTime), NULL, NULL, NULL, CAST(0x0000B25D00B4B0B9 AS DateTime), N'15', N'Damark', 0)
INSERT [dbo].[WfActivityInstance] ([ID], [ProcessInstanceID], [AppName], [AppInstanceID], [AppInstanceCode], [ProcessGUID], [ActivityGUID], [ActivityName], [ActivityCode], [ActivityType], [ActivityState], [WorkItemType], [AssignedToUserIDs], [AssignedToUserNames], [BackwardType], [BackSrcActivityInstanceID], [BackOrgActivityInstanceID], [GatewayDirectionTypeID], [CanNotRenewInstance], [ApprovalStatus], [TokensRequired], [TokensHad], [JobTimerType], [JobTimerStatus], [TriggerExpression], [OverdueDateTime], [JobTimerTreatedDateTime], [ComplexType], [MergeType], [MIHostActivityInstanceID], [CompareType], [CompleteOrder], [SignForwardType], [NextStepPerformers], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [LastUpdatedDateTime], [EndedDateTime], [EndedByUserID], [EndedByUserName], [RecordStatusInvalid]) VALUES (1988, 488, N'ProductOrder', N'701', NULL, N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', N'652441d3-2e61-4df0-a50f-499c253e6c19', N'End', NULL, 2, 4, 0, NULL, NULL, 0, NULL, NULL, NULL, 0, 0, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'15', N'Damark', CAST(0x0000B25D00B4B0BA AS DateTime), NULL, NULL, NULL, CAST(0x0000B25D00B4B0BA AS DateTime), N'15', N'Damark', 0)
INSERT [dbo].[WfActivityInstance] ([ID], [ProcessInstanceID], [AppName], [AppInstanceID], [AppInstanceCode], [ProcessGUID], [ActivityGUID], [ActivityName], [ActivityCode], [ActivityType], [ActivityState], [WorkItemType], [AssignedToUserIDs], [AssignedToUserNames], [BackwardType], [BackSrcActivityInstanceID], [BackOrgActivityInstanceID], [GatewayDirectionTypeID], [CanNotRenewInstance], [ApprovalStatus], [TokensRequired], [TokensHad], [JobTimerType], [JobTimerStatus], [TriggerExpression], [OverdueDateTime], [JobTimerTreatedDateTime], [ComplexType], [MergeType], [MIHostActivityInstanceID], [CompareType], [CompleteOrder], [SignForwardType], [NextStepPerformers], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [LastUpdatedDateTime], [EndedDateTime], [EndedByUserID], [EndedByUserName], [RecordStatusInvalid]) VALUES (1989, 487, N'ProductOrder', N'700', NULL, N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', N'78a69a65-d406-4056-9dc2-b25751fc6263', N'QCCheck(质检)', N'QCChecking', 4, 1, 1, N'13,14', N'Jimi,William', 0, NULL, NULL, NULL, 0, 0, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'9', N'Tuda', CAST(0x0000B25D00B534DA AS DateTime), NULL, NULL, NULL, NULL, NULL, NULL, 0)
SET IDENTITY_INSERT [dbo].[WfActivityInstance] OFF
/****** Object:  View [dbo].[vw_SysRoleUserView]    Script Date: 01/06/2025 13:31:59 ******/
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
/****** Object:  View [dbo].[vw_FbFormDataView]    Script Date: 01/06/2025 13:31:59 ******/
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
/****** Object:  Table [dbo].[WfTasks]    Script Date: 01/06/2025 13:31:59 ******/
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
	[ProcessGUID] [varchar](100) NOT NULL,
	[ActivityGUID] [varchar](100) NOT NULL,
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
INSERT [dbo].[WfTasks] ([ID], [ActivityInstanceID], [ProcessInstanceID], [AppName], [AppInstanceID], [ProcessGUID], [ActivityGUID], [ActivityName], [TaskType], [TaskState], [EntrustedTaskID], [AssignedToUserID], [AssignedToUserName], [IsEMailSent], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [EndedByUserID], [EndedByUserName], [EndedDateTime], [RecordStatusInvalid]) VALUES (2319, 1969, 485, N'SamplePrice', N'100', N'8186fe2b-f3d7-4955-b915-8ed821941eb9', N'28d7c3ae-bb9a-4177-8459-8b3f062c621c', N'Task-001', 1, 1, NULL, N'10', N'Long', 0, N'10', N'Long', CAST(0x0000B256010AD672 AS DateTime), NULL, NULL, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[WfTasks] ([ID], [ActivityInstanceID], [ProcessInstanceID], [AppName], [AppInstanceID], [ProcessGUID], [ActivityGUID], [ActivityName], [TaskType], [TaskState], [EntrustedTaskID], [AssignedToUserID], [AssignedToUserName], [IsEMailSent], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [EndedByUserID], [EndedByUserName], [EndedDateTime], [RecordStatusInvalid]) VALUES (2320, 1971, 486, N'ProductOrder', N'699', N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', N'62b71f52-2c6d-4476-d3c8-bd5f9ac1b94f', N'派单', 1, 4, NULL, N'7', N'Peter', 0, N'7', N'Peter', CAST(0x0000B25D00B19D43 AS DateTime), NULL, NULL, NULL, N'7', N'Peter', CAST(0x0000B25D00B19D5B AS DateTime), 0)
INSERT [dbo].[WfTasks] ([ID], [ActivityInstanceID], [ProcessInstanceID], [AppName], [AppInstanceID], [ProcessGUID], [ActivityGUID], [ActivityName], [TaskType], [TaskState], [EntrustedTaskID], [AssignedToUserID], [AssignedToUserName], [IsEMailSent], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [EndedByUserID], [EndedByUserName], [EndedDateTime], [RecordStatusInvalid]) VALUES (2321, 1973, 486, N'ProductOrder', N'699', N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', N'126ed9cc-b661-4e77-ae95-b5001ad9ce9c', N'打样', 1, 4, NULL, N'11', N'Fisher', 0, N'7', N'Peter', CAST(0x0000B25D00B19D62 AS DateTime), NULL, NULL, NULL, N'11', N'Fisher', CAST(0x0000B25D00B1B989 AS DateTime), 0)
INSERT [dbo].[WfTasks] ([ID], [ActivityInstanceID], [ProcessInstanceID], [AppName], [AppInstanceID], [ProcessGUID], [ActivityGUID], [ActivityName], [TaskType], [TaskState], [EntrustedTaskID], [AssignedToUserID], [AssignedToUserName], [IsEMailSent], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [EndedByUserID], [EndedByUserName], [EndedDateTime], [RecordStatusInvalid]) VALUES (2322, 1973, 486, N'ProductOrder', N'699', N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', N'126ed9cc-b661-4e77-ae95-b5001ad9ce9c', N'打样', 1, 1, NULL, N'12', N'Sherley', 0, N'7', N'Peter', CAST(0x0000B25D00B19D62 AS DateTime), NULL, NULL, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[WfTasks] ([ID], [ActivityInstanceID], [ProcessInstanceID], [AppName], [AppInstanceID], [ProcessGUID], [ActivityGUID], [ActivityName], [TaskType], [TaskState], [EntrustedTaskID], [AssignedToUserID], [AssignedToUserName], [IsEMailSent], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [EndedByUserID], [EndedByUserName], [EndedDateTime], [RecordStatusInvalid]) VALUES (2323, 1974, 486, N'ProductOrder', N'699', N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', N'5b6ba25a-8dd4-40c4-d27e-f834f0be0168', N'生产', 1, 1, NULL, N'9', N'Tuda', 0, N'11', N'Fisher', CAST(0x0000B25D00B1B98A AS DateTime), NULL, NULL, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[WfTasks] ([ID], [ActivityInstanceID], [ProcessInstanceID], [AppName], [AppInstanceID], [ProcessGUID], [ActivityGUID], [ActivityName], [TaskType], [TaskState], [EntrustedTaskID], [AssignedToUserID], [AssignedToUserName], [IsEMailSent], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [EndedByUserID], [EndedByUserName], [EndedDateTime], [RecordStatusInvalid]) VALUES (2324, 1974, 486, N'ProductOrder', N'699', N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', N'5b6ba25a-8dd4-40c4-d27e-f834f0be0168', N'生产', 1, 1, NULL, N'10', N'Jack', 0, N'11', N'Fisher', CAST(0x0000B25D00B1B98A AS DateTime), NULL, NULL, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[WfTasks] ([ID], [ActivityInstanceID], [ProcessInstanceID], [AppName], [AppInstanceID], [ProcessGUID], [ActivityGUID], [ActivityName], [TaskType], [TaskState], [EntrustedTaskID], [AssignedToUserID], [AssignedToUserName], [IsEMailSent], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [EndedByUserID], [EndedByUserName], [EndedDateTime], [RecordStatusInvalid]) VALUES (2325, 1974, 486, N'ProductOrder', N'699', N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', N'5b6ba25a-8dd4-40c4-d27e-f834f0be0168', N'生产', 1, 1, NULL, N'15', N'Damark', 0, N'11', N'Fisher', CAST(0x0000B25D00B1B98A AS DateTime), NULL, NULL, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[WfTasks] ([ID], [ActivityInstanceID], [ProcessInstanceID], [AppName], [AppInstanceID], [ProcessGUID], [ActivityGUID], [ActivityName], [TaskType], [TaskState], [EntrustedTaskID], [AssignedToUserID], [AssignedToUserName], [IsEMailSent], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [EndedByUserID], [EndedByUserName], [EndedDateTime], [RecordStatusInvalid]) VALUES (2326, 1974, 486, N'ProductOrder', N'699', N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', N'5b6ba25a-8dd4-40c4-d27e-f834f0be0168', N'生产', 1, 1, NULL, N'16', N'Smith', 0, N'11', N'Fisher', CAST(0x0000B25D00B1B98A AS DateTime), NULL, NULL, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[WfTasks] ([ID], [ActivityInstanceID], [ProcessInstanceID], [AppName], [AppInstanceID], [ProcessGUID], [ActivityGUID], [ActivityName], [TaskType], [TaskState], [EntrustedTaskID], [AssignedToUserID], [AssignedToUserName], [IsEMailSent], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [EndedByUserID], [EndedByUserName], [EndedDateTime], [RecordStatusInvalid]) VALUES (2327, 1976, 487, N'ProductOrder', N'700', N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', N'62b71f52-2c6d-4476-d3c8-bd5f9ac1b94f', N'派单', 1, 4, NULL, N'7', N'Peter', 0, N'7', N'Peter', CAST(0x0000B25D00B247AD AS DateTime), NULL, NULL, NULL, N'7', N'Peter', CAST(0x0000B25D00B247AF AS DateTime), 0)
INSERT [dbo].[WfTasks] ([ID], [ActivityInstanceID], [ProcessInstanceID], [AppName], [AppInstanceID], [ProcessGUID], [ActivityGUID], [ActivityName], [TaskType], [TaskState], [EntrustedTaskID], [AssignedToUserID], [AssignedToUserName], [IsEMailSent], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [EndedByUserID], [EndedByUserName], [EndedDateTime], [RecordStatusInvalid]) VALUES (2328, 1978, 487, N'ProductOrder', N'700', N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', N'126ed9cc-b661-4e77-ae95-b5001ad9ce9c', N'打样', 1, 4, NULL, N'11', N'Fisher', 0, N'7', N'Peter', CAST(0x0000B25D00B247B1 AS DateTime), NULL, NULL, NULL, N'11', N'Fisher', CAST(0x0000B25D00B25ABA AS DateTime), 0)
INSERT [dbo].[WfTasks] ([ID], [ActivityInstanceID], [ProcessInstanceID], [AppName], [AppInstanceID], [ProcessGUID], [ActivityGUID], [ActivityName], [TaskType], [TaskState], [EntrustedTaskID], [AssignedToUserID], [AssignedToUserName], [IsEMailSent], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [EndedByUserID], [EndedByUserName], [EndedDateTime], [RecordStatusInvalid]) VALUES (2329, 1978, 487, N'ProductOrder', N'700', N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', N'126ed9cc-b661-4e77-ae95-b5001ad9ce9c', N'打样', 1, 1, NULL, N'12', N'Sherley', 0, N'7', N'Peter', CAST(0x0000B25D00B247B1 AS DateTime), NULL, NULL, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[WfTasks] ([ID], [ActivityInstanceID], [ProcessInstanceID], [AppName], [AppInstanceID], [ProcessGUID], [ActivityGUID], [ActivityName], [TaskType], [TaskState], [EntrustedTaskID], [AssignedToUserID], [AssignedToUserName], [IsEMailSent], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [EndedByUserID], [EndedByUserName], [EndedDateTime], [RecordStatusInvalid]) VALUES (2330, 1979, 487, N'ProductOrder', N'700', N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', N'5b6ba25a-8dd4-40c4-d27e-f834f0be0168', N'生产', 1, 4, NULL, N'9', N'Tuda', 0, N'11', N'Fisher', CAST(0x0000B25D00B25ABB AS DateTime), NULL, NULL, NULL, N'9', N'Tuda', CAST(0x0000B25D00B534D5 AS DateTime), 0)
INSERT [dbo].[WfTasks] ([ID], [ActivityInstanceID], [ProcessInstanceID], [AppName], [AppInstanceID], [ProcessGUID], [ActivityGUID], [ActivityName], [TaskType], [TaskState], [EntrustedTaskID], [AssignedToUserID], [AssignedToUserName], [IsEMailSent], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [EndedByUserID], [EndedByUserName], [EndedDateTime], [RecordStatusInvalid]) VALUES (2331, 1979, 487, N'ProductOrder', N'700', N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', N'5b6ba25a-8dd4-40c4-d27e-f834f0be0168', N'生产', 1, 1, NULL, N'10', N'Jack', 0, N'11', N'Fisher', CAST(0x0000B25D00B25ABB AS DateTime), NULL, NULL, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[WfTasks] ([ID], [ActivityInstanceID], [ProcessInstanceID], [AppName], [AppInstanceID], [ProcessGUID], [ActivityGUID], [ActivityName], [TaskType], [TaskState], [EntrustedTaskID], [AssignedToUserID], [AssignedToUserName], [IsEMailSent], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [EndedByUserID], [EndedByUserName], [EndedDateTime], [RecordStatusInvalid]) VALUES (2332, 1979, 487, N'ProductOrder', N'700', N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', N'5b6ba25a-8dd4-40c4-d27e-f834f0be0168', N'生产', 1, 1, NULL, N'15', N'Damark', 0, N'11', N'Fisher', CAST(0x0000B25D00B25ABB AS DateTime), NULL, NULL, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[WfTasks] ([ID], [ActivityInstanceID], [ProcessInstanceID], [AppName], [AppInstanceID], [ProcessGUID], [ActivityGUID], [ActivityName], [TaskType], [TaskState], [EntrustedTaskID], [AssignedToUserID], [AssignedToUserName], [IsEMailSent], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [EndedByUserID], [EndedByUserName], [EndedDateTime], [RecordStatusInvalid]) VALUES (2333, 1979, 487, N'ProductOrder', N'700', N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', N'5b6ba25a-8dd4-40c4-d27e-f834f0be0168', N'生产', 1, 1, NULL, N'16', N'Smith', 0, N'11', N'Fisher', CAST(0x0000B25D00B25ABB AS DateTime), NULL, NULL, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[WfTasks] ([ID], [ActivityInstanceID], [ProcessInstanceID], [AppName], [AppInstanceID], [ProcessGUID], [ActivityGUID], [ActivityName], [TaskType], [TaskState], [EntrustedTaskID], [AssignedToUserID], [AssignedToUserName], [IsEMailSent], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [EndedByUserID], [EndedByUserName], [EndedDateTime], [RecordStatusInvalid]) VALUES (2334, 1981, 488, N'ProductOrder', N'701', N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', N'62b71f52-2c6d-4476-d3c8-bd5f9ac1b94f', N'Dispatch(派单)', 1, 4, NULL, N'7', N'Peter', 0, N'7', N'Peter', CAST(0x0000B25D00B43CB6 AS DateTime), NULL, NULL, NULL, N'7', N'Peter', CAST(0x0000B25D00B43CC9 AS DateTime), 0)
INSERT [dbo].[WfTasks] ([ID], [ActivityInstanceID], [ProcessInstanceID], [AppName], [AppInstanceID], [ProcessGUID], [ActivityGUID], [ActivityName], [TaskType], [TaskState], [EntrustedTaskID], [AssignedToUserID], [AssignedToUserName], [IsEMailSent], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [EndedByUserID], [EndedByUserName], [EndedDateTime], [RecordStatusInvalid]) VALUES (2335, 1983, 488, N'ProductOrder', N'701', N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', N'126ed9cc-b661-4e77-ae95-b5001ad9ce9c', N'Sample(打样)', 1, 4, NULL, N'11', N'Fisher', 0, N'7', N'Peter', CAST(0x0000B25D00B43CCD AS DateTime), NULL, NULL, NULL, N'11', N'Fisher', CAST(0x0000B25D00B45322 AS DateTime), 0)
INSERT [dbo].[WfTasks] ([ID], [ActivityInstanceID], [ProcessInstanceID], [AppName], [AppInstanceID], [ProcessGUID], [ActivityGUID], [ActivityName], [TaskType], [TaskState], [EntrustedTaskID], [AssignedToUserID], [AssignedToUserName], [IsEMailSent], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [EndedByUserID], [EndedByUserName], [EndedDateTime], [RecordStatusInvalid]) VALUES (2336, 1983, 488, N'ProductOrder', N'701', N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', N'126ed9cc-b661-4e77-ae95-b5001ad9ce9c', N'Sample(打样)', 1, 1, NULL, N'12', N'Sherley', 0, N'7', N'Peter', CAST(0x0000B25D00B43CCD AS DateTime), NULL, NULL, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[WfTasks] ([ID], [ActivityInstanceID], [ProcessInstanceID], [AppName], [AppInstanceID], [ProcessGUID], [ActivityGUID], [ActivityName], [TaskType], [TaskState], [EntrustedTaskID], [AssignedToUserID], [AssignedToUserName], [IsEMailSent], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [EndedByUserID], [EndedByUserName], [EndedDateTime], [RecordStatusInvalid]) VALUES (2337, 1984, 488, N'ProductOrder', N'701', N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', N'5b6ba25a-8dd4-40c4-d27e-f834f0be0168', N'Manufacture(生产)', 1, 4, NULL, N'9', N'Tuda', 0, N'11', N'Fisher', CAST(0x0000B25D00B45323 AS DateTime), NULL, NULL, NULL, N'9', N'Tuda', CAST(0x0000B25D00B4717F AS DateTime), 0)
INSERT [dbo].[WfTasks] ([ID], [ActivityInstanceID], [ProcessInstanceID], [AppName], [AppInstanceID], [ProcessGUID], [ActivityGUID], [ActivityName], [TaskType], [TaskState], [EntrustedTaskID], [AssignedToUserID], [AssignedToUserName], [IsEMailSent], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [EndedByUserID], [EndedByUserName], [EndedDateTime], [RecordStatusInvalid]) VALUES (2338, 1984, 488, N'ProductOrder', N'701', N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', N'5b6ba25a-8dd4-40c4-d27e-f834f0be0168', N'Manufacture(生产)', 1, 1, NULL, N'10', N'Jack', 0, N'11', N'Fisher', CAST(0x0000B25D00B45323 AS DateTime), NULL, NULL, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[WfTasks] ([ID], [ActivityInstanceID], [ProcessInstanceID], [AppName], [AppInstanceID], [ProcessGUID], [ActivityGUID], [ActivityName], [TaskType], [TaskState], [EntrustedTaskID], [AssignedToUserID], [AssignedToUserName], [IsEMailSent], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [EndedByUserID], [EndedByUserName], [EndedDateTime], [RecordStatusInvalid]) VALUES (2339, 1984, 488, N'ProductOrder', N'701', N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', N'5b6ba25a-8dd4-40c4-d27e-f834f0be0168', N'Manufacture(生产)', 1, 1, NULL, N'15', N'Damark', 0, N'11', N'Fisher', CAST(0x0000B25D00B45323 AS DateTime), NULL, NULL, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[WfTasks] ([ID], [ActivityInstanceID], [ProcessInstanceID], [AppName], [AppInstanceID], [ProcessGUID], [ActivityGUID], [ActivityName], [TaskType], [TaskState], [EntrustedTaskID], [AssignedToUserID], [AssignedToUserName], [IsEMailSent], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [EndedByUserID], [EndedByUserName], [EndedDateTime], [RecordStatusInvalid]) VALUES (2340, 1984, 488, N'ProductOrder', N'701', N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', N'5b6ba25a-8dd4-40c4-d27e-f834f0be0168', N'Manufacture(生产)', 1, 1, NULL, N'16', N'Smith', 0, N'11', N'Fisher', CAST(0x0000B25D00B45323 AS DateTime), NULL, NULL, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[WfTasks] ([ID], [ActivityInstanceID], [ProcessInstanceID], [AppName], [AppInstanceID], [ProcessGUID], [ActivityGUID], [ActivityName], [TaskType], [TaskState], [EntrustedTaskID], [AssignedToUserID], [AssignedToUserName], [IsEMailSent], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [EndedByUserID], [EndedByUserName], [EndedDateTime], [RecordStatusInvalid]) VALUES (2341, 1985, 488, N'ProductOrder', N'701', N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', N'78a69a65-d406-4056-9dc2-b25751fc6263', N'QCCheck(质检)', 1, 4, NULL, N'13', N'Jimi', 0, N'9', N'Tuda', CAST(0x0000B25D00B47180 AS DateTime), NULL, NULL, NULL, N'13', N'Jimi', CAST(0x0000B25D00B492C0 AS DateTime), 0)
INSERT [dbo].[WfTasks] ([ID], [ActivityInstanceID], [ProcessInstanceID], [AppName], [AppInstanceID], [ProcessGUID], [ActivityGUID], [ActivityName], [TaskType], [TaskState], [EntrustedTaskID], [AssignedToUserID], [AssignedToUserName], [IsEMailSent], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [EndedByUserID], [EndedByUserName], [EndedDateTime], [RecordStatusInvalid]) VALUES (2342, 1985, 488, N'ProductOrder', N'701', N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', N'78a69a65-d406-4056-9dc2-b25751fc6263', N'QCCheck(质检)', 1, 1, NULL, N'14', N'William', 0, N'9', N'Tuda', CAST(0x0000B25D00B47180 AS DateTime), NULL, NULL, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[WfTasks] ([ID], [ActivityInstanceID], [ProcessInstanceID], [AppName], [AppInstanceID], [ProcessGUID], [ActivityGUID], [ActivityName], [TaskType], [TaskState], [EntrustedTaskID], [AssignedToUserID], [AssignedToUserName], [IsEMailSent], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [EndedByUserID], [EndedByUserName], [EndedDateTime], [RecordStatusInvalid]) VALUES (2343, 1986, 488, N'ProductOrder', N'701', N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', N'1c1bceae-7bce-4394-cc0b-6bbf1a87bf2d', N'Weight(称重)', 1, 4, NULL, N'15', N'Damark', 0, N'13', N'Jimi', CAST(0x0000B25D00B492C1 AS DateTime), NULL, NULL, NULL, N'15', N'Damark', CAST(0x0000B25D00B4A741 AS DateTime), 0)
INSERT [dbo].[WfTasks] ([ID], [ActivityInstanceID], [ProcessInstanceID], [AppName], [AppInstanceID], [ProcessGUID], [ActivityGUID], [ActivityName], [TaskType], [TaskState], [EntrustedTaskID], [AssignedToUserID], [AssignedToUserName], [IsEMailSent], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [EndedByUserID], [EndedByUserName], [EndedDateTime], [RecordStatusInvalid]) VALUES (2344, 1986, 488, N'ProductOrder', N'701', N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', N'1c1bceae-7bce-4394-cc0b-6bbf1a87bf2d', N'Weight(称重)', 1, 1, NULL, N'16', N'Smith', 0, N'13', N'Jimi', CAST(0x0000B25D00B492C1 AS DateTime), NULL, NULL, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[WfTasks] ([ID], [ActivityInstanceID], [ProcessInstanceID], [AppName], [AppInstanceID], [ProcessGUID], [ActivityGUID], [ActivityName], [TaskType], [TaskState], [EntrustedTaskID], [AssignedToUserID], [AssignedToUserName], [IsEMailSent], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [EndedByUserID], [EndedByUserName], [EndedDateTime], [RecordStatusInvalid]) VALUES (2345, 1987, 488, N'ProductOrder', N'701', N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', N'b112b4d0-1cc1-4667-bdda-73c85e16266e', N'Print(打印发货单)', 1, 4, NULL, N'15', N'Damark', 0, N'15', N'Damark', CAST(0x0000B25D00B4A742 AS DateTime), NULL, NULL, NULL, N'15', N'Damark', CAST(0x0000B25D00B4B0B9 AS DateTime), 0)
INSERT [dbo].[WfTasks] ([ID], [ActivityInstanceID], [ProcessInstanceID], [AppName], [AppInstanceID], [ProcessGUID], [ActivityGUID], [ActivityName], [TaskType], [TaskState], [EntrustedTaskID], [AssignedToUserID], [AssignedToUserName], [IsEMailSent], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [EndedByUserID], [EndedByUserName], [EndedDateTime], [RecordStatusInvalid]) VALUES (2346, 1987, 488, N'ProductOrder', N'701', N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', N'b112b4d0-1cc1-4667-bdda-73c85e16266e', N'Print(打印发货单)', 1, 1, NULL, N'16', N'Smith', 0, N'15', N'Damark', CAST(0x0000B25D00B4A742 AS DateTime), NULL, NULL, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[WfTasks] ([ID], [ActivityInstanceID], [ProcessInstanceID], [AppName], [AppInstanceID], [ProcessGUID], [ActivityGUID], [ActivityName], [TaskType], [TaskState], [EntrustedTaskID], [AssignedToUserID], [AssignedToUserName], [IsEMailSent], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [EndedByUserID], [EndedByUserName], [EndedDateTime], [RecordStatusInvalid]) VALUES (2347, 1989, 487, N'ProductOrder', N'700', N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', N'78a69a65-d406-4056-9dc2-b25751fc6263', N'QCCheck(质检)', 1, 1, NULL, N'13', N'Jimi', 0, N'9', N'Tuda', CAST(0x0000B25D00B534DE AS DateTime), NULL, NULL, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[WfTasks] ([ID], [ActivityInstanceID], [ProcessInstanceID], [AppName], [AppInstanceID], [ProcessGUID], [ActivityGUID], [ActivityName], [TaskType], [TaskState], [EntrustedTaskID], [AssignedToUserID], [AssignedToUserName], [IsEMailSent], [CreatedByUserID], [CreatedByUserName], [CreatedDateTime], [LastUpdatedDateTime], [LastUpdatedByUserID], [LastUpdatedByUserName], [EndedByUserID], [EndedByUserName], [EndedDateTime], [RecordStatusInvalid]) VALUES (2348, 1989, 487, N'ProductOrder', N'700', N'6a51f9c1-3c81-46e8-bd34-084bd7b940ad', N'78a69a65-d406-4056-9dc2-b25751fc6263', N'QCCheck(质检)', 1, 1, NULL, N'14', N'William', 0, N'9', N'Tuda', CAST(0x0000B25D00B534DF AS DateTime), NULL, NULL, NULL, NULL, NULL, NULL, 0)
SET IDENTITY_INSERT [dbo].[WfTasks] OFF
/****** Object:  View [dbo].[vwWfActivityInstanceTasks]    Script Date: 01/06/2025 13:31:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vwWfActivityInstanceTasks]
AS
SELECT     dbo.WfTasks.ID AS TaskID, dbo.WfActivityInstance.AppName, dbo.WfActivityInstance.AppInstanceID, dbo.WfActivityInstance.ProcessGUID, dbo.WfProcessInstance.Version, 
                      dbo.WfTasks.ProcessInstanceID, dbo.WfActivityInstance.ActivityGUID, dbo.WfTasks.ActivityInstanceID, dbo.WfActivityInstance.ActivityName, dbo.WfActivityInstance.ActivityCode, 
                      dbo.WfActivityInstance.ActivityType, dbo.WfActivityInstance.WorkItemType, dbo.WfActivityInstance.BackSrcActivityInstanceID, dbo.WfActivityInstance.CreatedByUserID AS PreviousUserID, 
                      dbo.WfActivityInstance.CreatedByUserName AS PreviousUserName, dbo.WfActivityInstance.CreatedDateTime AS PreviousDateTime, dbo.WfTasks.TaskType, dbo.WfTasks.EntrustedTaskID, 
                      dbo.WfTasks.AssignedToUserID, dbo.WfTasks.AssignedToUserName, dbo.WfTasks.IsEMailSent, dbo.WfTasks.CreatedDateTime, dbo.WfTasks.LastUpdatedDateTime, dbo.WfTasks.EndedDateTime,
                       dbo.WfTasks.EndedByUserID, dbo.WfTasks.EndedByUserName, dbo.WfTasks.TaskState, dbo.WfActivityInstance.ActivityState, dbo.WfTasks.RecordStatusInvalid, 
                      dbo.WfProcessInstance.ProcessState, dbo.WfActivityInstance.ComplexType, dbo.WfActivityInstance.MIHostActivityInstanceID, dbo.WfActivityInstance.ApprovalStatus, 
                      dbo.WfActivityInstance.CompleteOrder, dbo.WfProcessInstance.AppInstanceCode, dbo.WfProcessInstance.ProcessName, dbo.WfProcessInstance.CreatedByUserName, 
                      dbo.WfProcessInstance.CreatedDateTime AS PCreatedDateTime, CASE WHEN MIHostActivityInstanceID IS NULL THEN ActivityState ELSE
                          (SELECT     ActivityState
                            FROM          dbo.WfActivityInstance a WITH (NOLOCK)
                            WHERE      a.ID = dbo.WfActivityInstance.MIHostActivityInstanceID) END AS MiHostState, dbo.WfProcessInstance.SubProcessType, dbo.WfProcessInstance.SubProcessID, 
                      dbo.WfProcessInstance.SubProcessGUID
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
/****** Object:  Default [DF__HrsLeave__LeaveT__5165187F]    Script Date: 01/06/2025 13:31:59 ******/
ALTER TABLE [dbo].[HrsLeave] ADD  CONSTRAINT [DF__HrsLeave__LeaveT__5165187F]  DEFAULT ((0)) FOR [LeaveType]
GO
/****** Object:  Default [DF_SSIP_WfActivityInstance_State]    Script Date: 01/06/2025 13:31:59 ******/
ALTER TABLE [dbo].[WfActivityInstance] ADD  CONSTRAINT [DF_SSIP_WfActivityInstance_State]  DEFAULT ((0)) FOR [ActivityState]
GO
/****** Object:  Default [DF_WfActivityInstance_WorkItemType]    Script Date: 01/06/2025 13:31:59 ******/
ALTER TABLE [dbo].[WfActivityInstance] ADD  CONSTRAINT [DF_WfActivityInstance_WorkItemType]  DEFAULT ((0)) FOR [WorkItemType]
GO
/****** Object:  Default [DF_SSIP_WfActivityInstance_CanInvokeNextActivity]    Script Date: 01/06/2025 13:31:59 ******/
ALTER TABLE [dbo].[WfActivityInstance] ADD  CONSTRAINT [DF_SSIP_WfActivityInstance_CanInvokeNextActivity]  DEFAULT ((0)) FOR [CanNotRenewInstance]
GO
/****** Object:  Default [DF_SSIP_WfActivityInstance_TokensRequired]    Script Date: 01/06/2025 13:31:59 ******/
ALTER TABLE [dbo].[WfActivityInstance] ADD  CONSTRAINT [DF_SSIP_WfActivityInstance_TokensRequired]  DEFAULT ((1)) FOR [TokensRequired]
GO
/****** Object:  Default [DF_SSIP_WfActivityInstance_CreatedDateTime]    Script Date: 01/06/2025 13:31:59 ******/
ALTER TABLE [dbo].[WfActivityInstance] ADD  CONSTRAINT [DF_SSIP_WfActivityInstance_CreatedDateTime]  DEFAULT (getdate()) FOR [CreatedDateTime]
GO
/****** Object:  Default [DF_SSIP_WfActivityInstance_RecordStatusInvalid]    Script Date: 01/06/2025 13:31:59 ******/
ALTER TABLE [dbo].[WfActivityInstance] ADD  CONSTRAINT [DF_SSIP_WfActivityInstance_RecordStatusInvalid]  DEFAULT ((0)) FOR [RecordStatusInvalid]
GO
/****** Object:  Default [DF__WfJobSche__Statu__73BA3083]    Script Date: 01/06/2025 13:31:59 ******/
ALTER TABLE [dbo].[WfJobSchedule] ADD  CONSTRAINT [DF__WfJobSche__Statu__73BA3083]  DEFAULT ((0)) FOR [Status]
GO
/****** Object:  Default [DF_WfProcess_Version]    Script Date: 01/06/2025 13:31:59 ******/
ALTER TABLE [dbo].[WfProcess] ADD  CONSTRAINT [DF_WfProcess_Version]  DEFAULT ((1)) FOR [Version]
GO
/****** Object:  Default [DF_WfProcess_IsUsing]    Script Date: 01/06/2025 13:31:59 ******/
ALTER TABLE [dbo].[WfProcess] ADD  CONSTRAINT [DF_WfProcess_IsUsing]  DEFAULT ((0)) FOR [IsUsing]
GO
/****** Object:  Default [DF_WfProcess_IsTimingStartup]    Script Date: 01/06/2025 13:31:59 ******/
ALTER TABLE [dbo].[WfProcess] ADD  CONSTRAINT [DF_WfProcess_IsTimingStartup]  DEFAULT ((0)) FOR [StartType]
GO
/****** Object:  Default [DF_WfProcess_EndType]    Script Date: 01/06/2025 13:31:59 ******/
ALTER TABLE [dbo].[WfProcess] ADD  CONSTRAINT [DF_WfProcess_EndType]  DEFAULT ((0)) FOR [EndType]
GO
/****** Object:  Default [DF_SSIP-WfPROCESS_CreatedDateTime]    Script Date: 01/06/2025 13:31:59 ******/
ALTER TABLE [dbo].[WfProcess] ADD  CONSTRAINT [DF_SSIP-WfPROCESS_CreatedDateTime]  DEFAULT (getdate()) FOR [CreatedDateTime]
GO
/****** Object:  Default [DF_WfProcessInstance_Version]    Script Date: 01/06/2025 13:31:59 ******/
ALTER TABLE [dbo].[WfProcessInstance] ADD  CONSTRAINT [DF_WfProcessInstance_Version]  DEFAULT ((1)) FOR [Version]
GO
/****** Object:  Default [DF_SSIP_WfProcessInstance_State]    Script Date: 01/06/2025 13:31:59 ******/
ALTER TABLE [dbo].[WfProcessInstance] ADD  CONSTRAINT [DF_SSIP_WfProcessInstance_State]  DEFAULT ((0)) FOR [ProcessState]
GO
/****** Object:  Default [DF_WfProcessInstance_InvokedActivityInstanceID]    Script Date: 01/06/2025 13:31:59 ******/
ALTER TABLE [dbo].[WfProcessInstance] ADD  CONSTRAINT [DF_WfProcessInstance_InvokedActivityInstanceID]  DEFAULT ((0)) FOR [InvokedActivityInstanceID]
GO
/****** Object:  Default [DF_SSIP_WfProcessInstance_CreatedDateTime]    Script Date: 01/06/2025 13:31:59 ******/
ALTER TABLE [dbo].[WfProcessInstance] ADD  CONSTRAINT [DF_SSIP_WfProcessInstance_CreatedDateTime]  DEFAULT (getdate()) FOR [CreatedDateTime]
GO
/****** Object:  Default [DF_SSIP_WfProcessInstance_RecordStatus]    Script Date: 01/06/2025 13:31:59 ******/
ALTER TABLE [dbo].[WfProcessInstance] ADD  CONSTRAINT [DF_SSIP_WfProcessInstance_RecordStatus]  DEFAULT ((0)) FOR [RecordStatusInvalid]
GO
/****** Object:  Default [DF_SSIP_WfTasks_IsCompleted]    Script Date: 01/06/2025 13:31:59 ******/
ALTER TABLE [dbo].[WfTasks] ADD  CONSTRAINT [DF_SSIP_WfTasks_IsCompleted]  DEFAULT ((0)) FOR [TaskState]
GO
/****** Object:  Default [DF_WfTasks_IsEMailSent]    Script Date: 01/06/2025 13:31:59 ******/
ALTER TABLE [dbo].[WfTasks] ADD  CONSTRAINT [DF_WfTasks_IsEMailSent]  DEFAULT ((0)) FOR [IsEMailSent]
GO
/****** Object:  Default [DF_SSIP_WfTasks_CreatedDateTime]    Script Date: 01/06/2025 13:31:59 ******/
ALTER TABLE [dbo].[WfTasks] ADD  CONSTRAINT [DF_SSIP_WfTasks_CreatedDateTime]  DEFAULT (getdate()) FOR [CreatedDateTime]
GO
/****** Object:  Default [DF_SSIP_WfTasks_RecordStatusInvalid]    Script Date: 01/06/2025 13:31:59 ******/
ALTER TABLE [dbo].[WfTasks] ADD  CONSTRAINT [DF_SSIP_WfTasks_RecordStatusInvalid]  DEFAULT ((0)) FOR [RecordStatusInvalid]
GO
/****** Object:  Default [DF_WfTransitionInstance_IsBackwardFlying]    Script Date: 01/06/2025 13:31:59 ******/
ALTER TABLE [dbo].[WfTransitionInstance] ADD  CONSTRAINT [DF_WfTransitionInstance_IsBackwardFlying]  DEFAULT ((0)) FOR [FlyingType]
GO
/****** Object:  Default [DF_SSIP_WfTransitionInstance_ConditionParseResult]    Script Date: 01/06/2025 13:31:59 ******/
ALTER TABLE [dbo].[WfTransitionInstance] ADD  CONSTRAINT [DF_SSIP_WfTransitionInstance_ConditionParseResult]  DEFAULT ((0)) FOR [ConditionParseResult]
GO
/****** Object:  Default [DF_SSIP_WfTransitionInstance_CreatedDateTime]    Script Date: 01/06/2025 13:31:59 ******/
ALTER TABLE [dbo].[WfTransitionInstance] ADD  CONSTRAINT [DF_SSIP_WfTransitionInstance_CreatedDateTime]  DEFAULT (getdate()) FOR [CreatedDateTime]
GO
/****** Object:  Default [DF_SSIP_WfTransitionInstance_RecordStatusInvalid]    Script Date: 01/06/2025 13:31:59 ******/
ALTER TABLE [dbo].[WfTransitionInstance] ADD  CONSTRAINT [DF_SSIP_WfTransitionInstance_RecordStatusInvalid]  DEFAULT ((0)) FOR [RecordStatusInvalid]
GO
/****** Object:  ForeignKey [FK_WfActivityInstance_ProcessInstanceID]    Script Date: 01/06/2025 13:31:59 ******/
ALTER TABLE [dbo].[WfActivityInstance]  WITH NOCHECK ADD  CONSTRAINT [FK_WfActivityInstance_ProcessInstanceID] FOREIGN KEY([ProcessInstanceID])
REFERENCES [dbo].[WfProcessInstance] ([ID])
GO
ALTER TABLE [dbo].[WfActivityInstance] CHECK CONSTRAINT [FK_WfActivityInstance_ProcessInstanceID]
GO
/****** Object:  ForeignKey [FK_WfTasks_ActivityInstanceID]    Script Date: 01/06/2025 13:31:59 ******/
ALTER TABLE [dbo].[WfTasks]  WITH NOCHECK ADD  CONSTRAINT [FK_WfTasks_ActivityInstanceID] FOREIGN KEY([ActivityInstanceID])
REFERENCES [dbo].[WfActivityInstance] ([ID])
GO
ALTER TABLE [dbo].[WfTasks] CHECK CONSTRAINT [FK_WfTasks_ActivityInstanceID]
GO
