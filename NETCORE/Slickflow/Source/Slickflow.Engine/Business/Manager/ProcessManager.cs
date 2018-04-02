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
using System.IO;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Slickflow.Data;
using Slickflow.Engine.Business.Data;
using Slickflow.Engine.Business.Entity;

namespace Slickflow.Engine.Business.Manager
{
    /// <summary>
    /// 流程定义管理类
    /// </summary>
    public class ProcessManager
    {
        #region 获取流程数据
        /// <summary>
        /// 根据流程GUID和版本标识获取流程
        /// </summary>
        /// <param name="processGUID">流程GUID</param>
        /// <param name="version">版本</param>
        /// <returns>流程实体</returns>
        public ProcessEntity GetByVersion(string processGUID, string version)
        {
            ProcessEntity entity = null;
            using (var session = DbFactory.CreateSession())
            {
                var repository = session.GetRepository<ProcessEntity>();
                if (!string.IsNullOrEmpty(version))
                {
                    //sql = @"SELECT 
                    //        * 
                    //    FROM WfProcess 
                    //    WHERE ProcessGUID=@processGUID 
                    //        AND VERSION=@version";
                    entity = repository.Get(e => e.ProcessGUID == processGUID
                        && e.Version == version);
                }
                else
                {
                    //sql = @"SELECT 
                    //        * 
                    //    FROM WfProcess 
                    //    WHERE ProcessGUID=@processGUID 
                    //        AND VERSION=@version";
                    entity = repository.Get(e => e.ProcessGUID == processGUID
                        && e.IsUsing == 1);
                }

                if (entity == null)
                {
                    throw new ApplicationException(string.Format(
                        "数据库没有对应的流程定义记录，ProcessGUID: {0}, Version: {1}", processGUID, version
                    ));
                }
                return entity;
            }           
        }

        /// <summary>
        /// 按照版本获取流程记录
        /// </summary>
        /// <param name="processGUID">流程GUID</param>
        /// <param name="version">版本</param>
        /// <returns>流程实体</returns>
        internal ProcessEntity GetByVersionInternal(string processGUID, string version)
        {
            //string sql = @"SELECT 
            //                * 
            //            FROM WfProcess 
            //            WHERE ProcessGUID=@processGUID 
            //                AND VERSION=@version";
            ProcessEntity entity = null;
            using (var session = DbFactory.CreateSession())
            {
                var list = session.GetRepository<ProcessEntity>().Query(e => e.ProcessGUID == processGUID
                    && e.Version == version).ToList();
                if (list != null)
                {
                    if (list.Count() > 1)
                    {
                        throw new ApplicationException(string.Format(
                            "数据库有多条重复的流程定义记录存在，ProcessGUID: {0}, Version: {1}", processGUID, version
                        ));
                    }
                    else if (list.Count() == 1)
                    {
                        entity = list[0];
                    }
                }
                return entity;
            }
        }

        /// <summary>
        /// 获取所有流程记录
        /// </summary>
        /// <returns>流程列表</returns>
        public List<ProcessEntity> GetAll()
        {
            using (var session = DbFactory.CreateSession())
            {
                var list = session.GetRepository<ProcessEntity>().GetAll().ToList();
                return list;
            }
        }

        /// <summary>
        /// 获取基本属性的流程定义记录列表
        /// </summary>
        /// <returns></returns>
        public List<ProcessEntity> GetListSimple()
        {
            //var sql = @"SELECT 
            //            ID, 
            //            ProcessGUID, 
            //            ProcessName,
            //            Version,
            //            IsUsing,
            //            CreatedDateTime
            //        FROM WfProcess
            //        ORDER BY ID DESC";
            using (var session = DbFactory.CreateSession())
            {
                var list = session.GetRepository<ProcessEntity>().GetDbSet()
                    .Select(p => new ProcessEntity
                    {
                        ID = p.ID,
                        ProcessGUID = p.ProcessGUID,
                        ProcessName = p.ProcessName,
                        Version = p.Version,
                        IsUsing = p.IsUsing,
                        CreatedDateTime = p.CreatedDateTime
                    })
                    .OrderByDescending(p=>p.ID)
                    .ToList();
                return list;
            }
        }
        #endregion

        #region 新增、更新和删除流程数据
        /// <summary>
        /// 新增流程记录
        /// </summary>
        /// <param name="entity"></param>
        public int Insert(ProcessEntity entity)
        {
            using (var session = DbFactory.CreateSession())
            {
                var newID = Insert(entity, session);
                return newID;
            }
        }

        /// <summary>
        /// 新增流程记录
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="session">数据上下文</param>
        public int Insert(ProcessEntity entity, 
            IDbSession session)
        {
            var entityExisted = GetByVersionInternal(entity.ProcessGUID, entity.Version);
            if (entityExisted != null) throw new ApplicationException("相同版本的GUID标识的流程记录已经存在！");

            var newEntity = session.GetRepository<ProcessEntity>().Insert(entity);
            session.SaveChanges();

            return newEntity.ID;
        }

        /// <summary>
        /// 更新流程记录
        /// </summary>
        /// <param name="entity">流程实体</param>
        public void Update(ProcessEntity entity)
        {
            using (var session = DbFactory.CreateSession())
            {
                Update(entity, session);
            }
        }

        /// <summary>
        /// 流程定义记录更新
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="session">数据上下文</param>
        public void Update(ProcessEntity entity, 
            IDbSession session)
        {
            session.GetRepository<ProcessEntity>().Update(entity);
            session.SaveChanges();
        }

        /// <summary>
        /// 更新流程版本IsUsing=0
        /// </summary>
        /// <param name="processGUID">流程GUID</param>
        /// <param name="session">会话</param>
        public void RemoveUsingState(string processGUID, 
            IDbSession session)
        {
            //string strSql = @"UPDATE WfProcess 
            //                  SET IsUsing=0 
            //                  WHERE  ProcessGUID=@processGUID";
            var repository = session.GetRepository<ProcessEntity>();
            var list = repository.Query(e => e.ProcessGUID == processGUID
                && e.IsUsing == 0);
            session.GetRepository<ProcessEntity>().Update(list);
            session.SaveChanges();
        }
        /// <summary>
        /// 删除流程记录
        /// </summary>
        /// <param name="processGUID">流程GUID</param>
        /// <param name="version">版本</param>
        public void Delete(string processGUID, string version)
        {
            //string strSql = @"DELETE FROM WfProcess  
            //                WHERE  ProcessGUID=@processGUID
            //                    AND Version=@version";
            using (var session = DbFactory.CreateSession())
            {
                var repository = session.GetRepository<ProcessEntity>();
                var list = repository.Query(e => e.ProcessGUID == processGUID
                    && e.Version == version);
                repository.Delete(list);
                session.SaveChanges();
            }
        }

		/// <summary>
        /// 删除流程记录
        /// </summary>
        /// <param name="processGUID">流程GUID</param>
        public void Delete(string processGUID)
        {
            //string strSql = @"DELETE FROM WfProcess  
            //                WHERE  ProcessGUID=@processGUID";
            using (var session = DbFactory.CreateSession())
            {
                var repository = session.GetRepository<ProcessEntity>();
                var list = repository.Query(e => e.ProcessGUID == processGUID);
                repository.Delete(list);
                session.SaveChanges();
            }
        }
        #endregion

        #region 流程xml文件操作
        /// <summary>
        /// 流程定义的创建方法
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="extStorage">存储</param>
        /// <returns></returns>
        internal int CreateProcess(ProcessEntity entity)
        {
            using (var session = DbFactory.CreateSession())
            {
                entity.ProcessGUID = Guid.NewGuid().ToString();
                entity.Version = "1";     //default version value;
                entity.IsUsing = 1;
                entity.CreatedDateTime = DateTime.Now;
                entity.XmlFilePath = string.Format("{0}\\{1}", entity.AppType, entity.XmlFileName);
                XmlDocument xmlDoc = GenerateXmlContent(entity);
                entity.XmlContent = xmlDoc.OuterXml;    //流程定义文件

                var newEntity = session.GetRepository<ProcessEntity>().Insert(entity);
                session.SaveChanges();

                return newEntity.ID;       //return the new inseted process id
            }
        }

        /// <summary>
        /// 创建流程新版本
        /// </summary>
        /// <param name="entity">流程实体</param>
        /// <returns>新实例ID</returns>
        internal int CreateProcessVersion(ProcessEntity entity)
        {
            using (var session = DbFactory.CreateSession())
            {
                RemoveUsingState(entity.ProcessGUID, session);
                entity.CreatedDateTime = DateTime.Now;
                entity.IsUsing = 1;
                var newID = Insert(entity);
                return newID;
            }
        }

        /// <summary>
        /// 生成基本XML文档
        /// </summary>
        /// <param name="entity">流程定义实体</param>
        /// <returns>XML文档</returns>
        private XmlDocument GenerateXmlContent(ProcessEntity entity)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<Package/>");
            XmlDeclaration xmldecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);

            //Add the new node to the document.
            XmlElement root = xmlDoc.DocumentElement;
            xmlDoc.InsertBefore(xmldecl, root);

            XmlElement workflowNode = xmlDoc.CreateElement("WorkflowProcesses");
            XmlElement processNode = xmlDoc.CreateElement("Process");
            processNode.SetAttribute("name", entity.ProcessName);
            processNode.SetAttribute("id", entity.ProcessGUID);

            XmlElement descriptionNode = xmlDoc.CreateElement("Description");
            descriptionNode.InnerText = entity.Description;
            processNode.AppendChild(descriptionNode);

            workflowNode.AppendChild(processNode);
            root.AppendChild(workflowNode);

            return xmlDoc;
        }

        /// <summary>
        /// 保存XML文件
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="extStorage">存储</param>
        internal void SaveProcessFile(ProcessFileEntity entity)
        {
            using (var session = DbFactory.CreateSession())
            {
                var processEntity = GetByVersion(entity.ProcessGUID, entity.Version);
                processEntity.StartType = entity.StartType;
                processEntity.StartExpression = entity.StartExpression;
                processEntity.XmlContent = entity.XmlContent;
                processEntity.LastUpdatedDateTime = DateTime.Now;
                session.GetRepository<ProcessEntity>().Update(processEntity);

                session.SaveChanges();
            }
        }

        /// <summary>
        /// 读取流程XML文件内容
        /// </summary>
        /// <param name="processGUID">流程GUID</param>
        /// <param name="version">版本</param>
        /// <returns>流程文件实体</returns>
        internal ProcessFileEntity GetProcessFile(string processGUID, string version)
        {
            var processEntity = GetByVersion(processGUID, version);
            var processFileEntity = FillProcessFileEntity(processEntity);

            return processFileEntity;
        }

        /// <summary>
        /// 根据ID获取流程文件实体
        /// </summary>
        /// <param name="id">流程ID</param>
        /// <param name="extStorage">外部存储</param>
        /// <returns>流程文件实体</returns>
        internal ProcessFileEntity GetProcessFileByID(int id)
        {
            using (var session = DbFactory.CreateSession())
            {
                var processEntity = session.GetRepository<ProcessEntity>().GetByID(id);
                var processFileEntity = FillProcessFileEntity(processEntity);
                return processFileEntity;
            }
        }

        /// <summary>
        /// 转换流程文件实体
        /// </summary>
        /// <param name="processEntity">流程实体</param>
        /// <returns>流程文件实体</returns>
        private ProcessFileEntity FillProcessFileEntity(ProcessEntity processEntity)
        {
            //流程文件实体
            var processFileEntity = new ProcessFileEntity();

            processFileEntity.ProcessGUID = processEntity.ProcessGUID;
            processFileEntity.ProcessName = processEntity.ProcessName;
            processFileEntity.Version = processEntity.Version;
            processFileEntity.StartType = processEntity.StartType;
            processFileEntity.StartExpression = processEntity.StartExpression;
            processFileEntity.Description = processEntity.Description;
            processFileEntity.XmlContent = processEntity.XmlContent;

            return processFileEntity;
        }


        /// <summary>
        /// 读取Xml文档
        /// </summary>
        /// <param name="processGUID">流程GUID</param>
        /// <param name="version">版本</param>
        /// <param name="extStorage">存储</param>
        /// <returns>Xml文档</returns>
        internal XmlDocument GetProcessXmlDocument(string processGUID, string version)
        {
            var processEntity = GetByVersion(processGUID, version);
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(processEntity.XmlContent);

            return xmlDoc;
        }
        #endregion
    }
}
