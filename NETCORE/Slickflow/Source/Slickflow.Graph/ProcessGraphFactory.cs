/*
* Slickflow 工作流引擎遵循LGPL协议，也可联系作者商业授权并获取技术支持；
* 除此之外的使用则视为不正当使用，请您务必避免由此带来的商业版权纠纷。
*  
The Slickflow project.
Copyright (C) 2014  .NET Workflow Engine Library

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, you can access the official
web page about lgpl: https://www.gnu.org/licenses/lgpl.html
*/

using System;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Common;
using Slickflow.Engine.Xpdl;

namespace Slickflow.Graph
{
    /// <summary>
    /// 流程图形代码创建工厂类
    /// </summary>
    public class ProcessGraphFactory
    {
        /// <summary>
        /// 创建流程
        /// </summary>
        /// <param name="fileEntity">流程文件实体对象</param>
        /// <returns>流程实体</returns>
        public static ProcessEntity CreateProcess(ProcessFileEntity fileEntity)
        {
            ProcessEntity entity = null;
            var templateType = fileEntity.TemplateType;
            if (templateType == ProcessTemplateType.Blank)
            {
                entity = CreateFlowBlank(fileEntity);
            }
            else if (templateType == ProcessTemplateType.Simple)
            {
                entity = CreateFlowSimple(fileEntity);
            }
            else if(templateType == ProcessTemplateType.Sequence)
            {
                entity = CreateFlowSequence(fileEntity);
            } 
            else if(templateType == ProcessTemplateType.Gateway)
            {
                entity = CreateFlowGateway(fileEntity);
            }
            else if(templateType == ProcessTemplateType.SubProcess)
            {
                entity = CreateFlowSubProcess(fileEntity);
            }
            else if(templateType == ProcessTemplateType.MultipleInstance)
            {
                entity = CreateFlowMI(fileEntity);
            }
            else if(templateType == ProcessTemplateType.AndSplitMI)
            {
                entity = CreateFlowAndSplitMI(fileEntity);
            }
            else if(templateType == ProcessTemplateType.Complex)
            {
                entity = CreateFlowComplex(fileEntity);
            }
            else
            {
                entity = CreateFlowBlank(fileEntity);
            }

            return entity;
        }

        /// <summary>
        /// 创建空白流程
        /// </summary>
        /// <param name="fileEntity">文件实体</param>
        /// <returns>流程实体</returns>
        private static ProcessEntity CreateFlowBlank(ProcessFileEntity fileEntity)
        {
            var entity = InsertProcessEntity(fileEntity);
            return entity;
        }

        /// <summary>
        /// 创建简单流程
        /// </summary>
        /// <param name="fileEntity">文件实体</param>
        /// <returns>流程实体</returns>
        private static ProcessEntity CreateFlowSimple(ProcessFileEntity fileEntity)
        {
            var processName = !string.IsNullOrEmpty(fileEntity.ProcessName) 
                ? fileEntity.ProcessName : "test-process-simple";
            var pmb = ProcessModelBuilder.CreateProcess(processName, fileEntity.ProcessCode);
            var xmlContent = pmb.Start()
               .Task()
               .End()
               .Serialize();

            var entity = InsertProcessEntity(fileEntity, xmlContent);
            return entity;
        }

        /// <summary>
        /// 创建活动和边界的流程
        /// </summary>
        /// <param name="fileEntity">文件实体</param>
        /// <returns>流程实体</returns>
        private static ProcessEntity CreateFlowSequence(ProcessFileEntity fileEntity)
        {
            var processName = !string.IsNullOrEmpty(fileEntity.ProcessName) 
                ? fileEntity.ProcessName : "test-process-sequence";
            var pmb = ProcessModelBuilder.CreateProcess(processName, fileEntity.ProcessCode);
            var xmlContent =
                pmb.Start()
                   .Task(
                        VertexBuilder.CreateTask("Task-001")
                                   .SetUrl("http://www.slickflow.com")
                                   .AddRole("TestRole")
                                   .AddAction(
                                        VertexBuilder.CreateAction(ActionTypeEnum.Event,
                                            FireTypeEnum.Before,
                                            "Slickflow.Module.External.OrderSubmitService"
                                        ))
                    )
                   .Task(
                        VertexBuilder.CreateTask("Task-002"),
                        LinkBuilder.CreateTransition("t-001")
                                   .AddCondition(ConditionTypeEnum.Expression, "a>2")
                    )
                  .Task(
                        VertexBuilder.CreateTask("Task-003")
                    )
                   .End()
                   .Serialize();

            var entity = InsertProcessEntity(fileEntity, xmlContent);
            return entity;
        }

        /// <summary>
        /// 创建子流程
        /// </summary>
        /// <param name="fileEntity">文件实体</param>
        /// <returns>流程实体</returns>
        private static ProcessEntity CreateFlowSubProcess(ProcessFileEntity fileEntity)
        {
            var processName = !string.IsNullOrEmpty(fileEntity.ProcessName) 
                ? fileEntity.ProcessName : "test-process-subprocess";
            var pmb = ProcessModelBuilder.CreateProcess(processName, fileEntity.ProcessCode);
            var xmlContent =
                pmb.Start()
                   .Task("Task-001")
                   .Task(
                        VertexBuilder.CreateSubProcess("InterSubProcess")
                    )
                    .Task("Task-003")
                   .End()
                   .Serialize();

            var entity = InsertProcessEntity(fileEntity, xmlContent);
            return entity;
        }

        /// <summary>
        /// 创建分支流程
        /// </summary>
        /// <param name="fileEntity">文件实体</param>
        /// <returns>流程实体</returns>
        private static ProcessEntity CreateFlowGateway(ProcessFileEntity fileEntity)
        {
            var processName = !string.IsNullOrEmpty(fileEntity.ProcessName)
                ? fileEntity.ProcessName : "test-process-gateway";
            var pmb = ProcessModelBuilder.CreateProcess(processName, fileEntity.ProcessCode);
            var xmlContent =
                pmb.Start("start")
                  .Task(
                        VertexBuilder.CreateTask("Task-001")
                  )
                  .Split("split")
                  .Parallels(
                        () => pmb.Branch(
                            () => pmb.Task("task-010"),
                            () => pmb.Task("task-011")
                        )
                        , () => pmb.Branch(
                             () => pmb.Task("task-020"),
                             () => pmb.Task("task-021")
                         )
                  )
                  .Join("join")
                  .Task("task-100")
                  .End("end")
                  .Serialize();

            var entity = InsertProcessEntity(fileEntity, xmlContent);
            return entity;
        }

        /// <summary>
        /// 创建并行分支容器流程
        /// </summary>
        /// <param name="fileEntity">文件实体</param>
        /// <returns>流程实体</returns>
        private static ProcessEntity CreateFlowMI(ProcessFileEntity fileEntity)
        {
            var processName = !string.IsNullOrEmpty(fileEntity.ProcessName)
                ? fileEntity.ProcessName : "test-process-sequence";
            var pmb = ProcessModelBuilder.CreateProcess(processName, fileEntity.ProcessCode);
            var xmlContent =
                pmb.Start()
                   .Task(
                        VertexBuilder.CreateTask("Task-001")
                                   .SetUrl("http://www.slickflow.com")
                                   .AddRole("TestRole")
                    )
                   .Task(
                        VertexBuilder.CreateMultipleInstance("Sign Together")
                    )
                  .Task(
                        VertexBuilder.CreateTask("Task-003")
                    )
                   .End()
                   .Serialize();

            var entity = InsertProcessEntity(fileEntity, xmlContent);
            return entity;
        }

        /// <summary>
        /// 创建并行分支容器流程
        /// </summary>
        /// <param name="fileEntity">文件实体</param>
        /// <returns>流程实体</returns>
        private static ProcessEntity CreateFlowSequenceAdvanced(ProcessFileEntity fileEntity)
        {
            var processName = !string.IsNullOrEmpty(fileEntity.ProcessName)
                 ? fileEntity.ProcessName : "test-process-sequence";
            var pmb = ProcessModelBuilder.CreateProcess(processName, fileEntity.ProcessCode);
            var xmlContent =
                pmb.Start()
                   .Task(
                        VertexBuilder.CreateTask("Task-001")
                                   .SetUrl("http://www.slickflow.com")
                                   .AddRole("TestRole")
                                   .AddAction(
                                        VertexBuilder.CreateAction(ActionTypeEnum.Event,
                                            FireTypeEnum.Before,
                                            "Slickflow.Module.External.OrderSubmitService"
                                        ))
                    )
                   .Task(
                        VertexBuilder.CreateTask("Task-002"),
                        LinkBuilder.CreateTransition("t-001")
                                   .AddCondition(ConditionTypeEnum.Expression, "a>2")
                    )
                  .Task(
                        VertexBuilder.CreateSubProcess("Task-003")
                                     .AddBoundary(
                                        VertexBuilder.CreateBoundary(EventTriggerEnum.Timer,
                                            "P2M10D"
                                    ))
                    )
                   .End()
                   .Serialize();

            var entity = InsertProcessEntity(fileEntity, xmlContent);
            return entity;
        }

        /// <summary>
        /// 创建分支流程
        /// </summary>
        /// <param name="fileEntity">文件实体</param>
        /// <returns>流程实体</returns>
        private static ProcessEntity CreateFlowGatewayAdvanced(ProcessFileEntity fileEntity)
        {
            var processName = !string.IsNullOrEmpty(fileEntity.ProcessName)
                ? fileEntity.ProcessName : "test-process-gateway";
            var pmb = ProcessModelBuilder.CreateProcess(processName, fileEntity.ProcessCode);
            var xmlContent =
                pmb.Start("start")
                  .Task(
                        VertexBuilder.CreateTask("Task-001")
                                   .AddAction(
                                        VertexBuilder.CreateAction(ActionTypeEnum.Event,
                                            FireTypeEnum.Before,
                                            " Slickflow.Module.External.OrderSubmitService"
                                    ))
                  )
                  .AndSplit("and-split")
                  .Parallels(
                        () => pmb.Branch(
                            () => pmb.Task("task-010"),
                            () => pmb.Task("task-011")
                        )
                        , () => pmb.Branch(
                             () => pmb.Task("task-020"),
                             () => pmb.Task("task-021")
                         )
                        , () => pmb.Branch(
                             () => pmb.Task("task-030"),
                             () => pmb.Task("task-031")
                         )
                  )
                  .AndJoin("and-join")
                  .Task("task-100")
                  .End("end")
                  .Serialize();

            var entity = InsertProcessEntity(fileEntity, xmlContent);
            return entity;
        }

        /// <summary>
        /// 创建并行分支容器流程
        /// </summary>
        /// <param name="fileEntity">文件实体</param>
        /// <returns>流程实体</returns>
        private static ProcessEntity CreateFlowAndSplitMI(ProcessFileEntity fileEntity)
        {
            var processName = !string.IsNullOrEmpty(fileEntity.ProcessName)
                ? fileEntity.ProcessName : "test-process-gateway";
            var pmb = ProcessModelBuilder.CreateProcess(processName, fileEntity.ProcessCode);
            var xmlContent =
                pmb.Start("start")
                  .Task(
                        VertexBuilder.CreateTask("Task-001")
                  )
                  .AndSplitMI("and-split")
                  .Parallels(
                        () => pmb.Branch(
                            () => pmb.Task("task-010"),
                            () => pmb.Task("task-011")
                        )
                  )
                  .AndJoinMI("and-join")
                  .Task("task-100")
                  .End("end")
                  .Serialize();

            var entity = InsertProcessEntity(fileEntity, xmlContent);
            return entity;
        }

        /// <summary>
        /// 创建并行分支容器流程
        /// </summary>
        /// <param name="fileEntity">文件实体</param>
        /// <returns>流程实体</returns>
        private static ProcessEntity CreateFlowComplex(ProcessFileEntity fileEntity)
        {
            var processName = !string.IsNullOrEmpty(fileEntity.ProcessName)
                ? fileEntity.ProcessName : "test-process-gateway";
            var pmb = ProcessModelBuilder.CreateProcess(processName, fileEntity.ProcessCode);
            var xmlContent =
                pmb.Start("start")
                  .Task(
                        VertexBuilder.CreateTask("Task-001")
                  )
                  .OrSplit("or-split")
                  .Parallels(
                        () => pmb.Branch(
                            () => pmb.Task("task-010"),
                            () => pmb.Task(
                                    VertexBuilder.CreateMultipleInstance("MI-011")
                                )
                        )
                        ,() => pmb.Branch(
                             () => pmb.Task("task-020"),
                             () => pmb.Task(
                                    VertexBuilder.CreateSubProcess("Sub-021")
                                )
                         )
                        ,() => pmb.Branch(
                             () => pmb.Task("task-030"),
                             () => pmb.Task("task-031")
                         )
                  )
                  .OrJoin("or-join")
                  .Task("task-100")
                  .End("end")
                  .Serialize();

            var entity = InsertProcessEntity(fileEntity, xmlContent);
            return entity;
        }

        /// <summary>
        /// 插入流程实体
        /// </summary>
        /// <param name="fileEntity">流程文件实体</param>
        /// <param name="xmlContent">XML文件内容</param>
        /// <returns>流程实体</returns>
        private static ProcessEntity InsertProcessEntity(ProcessFileEntity fileEntity,
            string xmlContent = null)
        {
            var entity = new ProcessEntity
            {
                ProcessGUID = string.IsNullOrEmpty(fileEntity.ProcessGUID) ? Guid.NewGuid().ToString() : fileEntity.ProcessGUID,
                ProcessName = fileEntity.ProcessName,
                ProcessCode = fileEntity.ProcessCode,
                Version = string.IsNullOrEmpty(fileEntity.Version) ? "1" : fileEntity.Version,
                IsUsing = fileEntity.IsUsing,
                XmlContent = xmlContent,
                Description = fileEntity.Description,
                CreatedDateTime = System.DateTime.Now
            };

            var pm = new ProcessManager();
            var processID = pm.Insert(entity);
            entity.ID = processID;

            return entity;
        }
    }
}
