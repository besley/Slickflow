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
using Slickflow.Engine.Common;
using Slickflow.Engine.Utility;
using Slickflow.Data;
using Slickflow.Engine.Business.Entity;

namespace Slickflow.Engine.Business.Manager
{
    /// <summary>
    /// 流程定义管理类
    /// </summary>
    public class ProcessManager : ManagerBase
    {
        #region 获取流程数据
        /// <summary>
        /// 根据流程GUID和版本标识获取流程
        /// </summary>
        /// <param name="processGUID"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public ProcessEntity GetByVersion(string processGUID, string version)
        {
            String sql = string.Empty;
            ProcessEntity entity = null;

            if (!string.IsNullOrEmpty(version))
            {
                sql = @"SELECT 
                            * 
                        FROM WfProcess 
                        WHERE ProcessGUID=@processGUID 
                            AND VERSION=@version";
            }
            else
            {
                sql = @"SELECT 
                            * 
                        FROM WfProcess 
                        WHERE ProcessGUID=@processGUID 
                            AND IsUsing=1";             //当前使用的版本
            }

            var list = Repository.Query<ProcessEntity>(sql, new { processGUID=processGUID, version=version})
                            .ToList<ProcessEntity>();

            if (list != null && list.Count() == 1)
            {
                entity = list[0];
            }
            else
            {
                throw new ApplicationException(string.Format(
                    "数据库没有对应的流程定义记录，ProcessGUID: {0}, Version: {1}", processGUID, version
                ));
            }
            return entity;
        }

        /// <summary>
        /// 获取所有流程记录
        /// </summary>
        /// <returns></returns>
        public List<ProcessEntity> GetAll()
        {
            var list = Repository.GetAll<ProcessEntity>().ToList<ProcessEntity>();
            return list;
        }

        /// <summary>
        /// 获取基本属性的流程定义记录列表
        /// </summary>
        /// <returns></returns>
        public List<ProcessEntity> GetListSimple()
        {
            var sql = @"SELECT 
                        ID, 
                        ProcessGUID, 
                        ProcessName,
                        Version,
                        IsUsing,
                        CreatedDateTime
                    FROM WfProcess
                    ORDER BY ID DESC";
            var list = Repository.Query<ProcessEntity>(sql).ToList();
            return list;
        }
        #endregion

        #region 新增、更新和删除流程数据
        /// <summary>
        /// 新增流程记录
        /// </summary>
        /// <param name="entity"></param>
        public void Insert(ProcessEntity entity)
        {
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                Repository.Insert<ProcessEntity>(session.Connection, entity, session.Transaction);
                session.Commit();
            }
            catch (System.Exception)
            {
                session.Rollback();
                throw;
            }
            finally
            {
                session.Dispose();
            }
        }

        /// <summary>
        /// 新增流程记录
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="entity"></param>
        /// <param name="trans"></param>
        public int Insert(IDbConnection conn, ProcessEntity entity, IDbTransaction trans)
        {
            return Repository.Insert<ProcessEntity>(conn, entity, trans);
        }

        /// <summary>
        /// 更新流程记录
        /// </summary>
        /// <param name="entity"></param>
        public void Update(ProcessEntity entity)
        {
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();
                Repository.Update<ProcessEntity>(session.Connection, entity, session.Transaction);
                session.Commit();
            }
            catch (System.Exception)
            {
                session.Rollback();
                throw;
            }
            finally
            {
                session.Dispose();
            }
        }

        /// <summary>
        /// 流程定义记录更新
        /// </summary>
        /// <param name="conn">链接</param>
        /// <param name="entity">实体</param>
        /// <param name="trans">事务</param>
        public void Update(IDbConnection conn, ProcessEntity entity, IDbTransaction trans)
        {
            Repository.Update<ProcessEntity>(conn, entity, trans);
        }

        /// <summary>
        /// 更新流程版本IsUsing=0
        /// </summary>
        /// <param name="conn">链接</param>
        /// <param name="processGUID">流程GUID</param>
        /// <param name="trans">事务</param>
        public void UpdateUsingState(IDbConnection conn, string processGUID, IDbTransaction trans)
        {
            string strSql = @"UPDATE WfProcess 
                              SET IsUsing=0 
                              WHERE  ProcessGUID=@processGUID";

            Repository.Execute(conn, strSql, new { processGUID = processGUID }, trans);
        }
        /// <summary>
        /// 删除流程记录
        /// </summary>
        /// <param name="processGUID">流程GUID</param>
        /// <param name="version">版本</param>
        public void Delete(string processGUID, string version)
        {
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                var entity = GetByVersion(processGUID, version);

                session.BeginTrans();
                Repository.Delete<ProcessEntity>(session.Connection, entity, session.Transaction);
                session.Commit();
            }
            catch (System.Exception)
            {
                session.Rollback();
                throw;
            }
            finally
            {
                session.Dispose();
            }
        }

		/// <summary>
        /// 删除流程记录
        /// </summary>
        /// <param name="conn">链接</param>
        /// <param name="processGUID">流程GUID</param>
        /// <param name="trans">事务</param>
        public void Delete(IDbConnection conn, string processGUID, IDbTransaction trans)
        {
            string strSql = "DELETE FROM WfProcess  WHERE  ProcessGUID=@processGUID";
            Repository.Execute(conn, strSql, new { processGUID = processGUID }, trans);
        }

        /// <summary>
        /// 删除流程记录
        /// </summary>
        /// <param name="processGUID">流程定义GUID</param>
        /// <param name="version">版本</param>
        /// <param name="localStorage">本地存储</param>
        public void DeleteProcess(string processGUID, string version, IXPDLStorage localStorage)
        {
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();
                var entity = GetByVersion(processGUID, version);

                string strSql = @"DELETE FROM WfProcess  
                                WHERE  ProcessGUID=@processGUID
                                    AND Version=@version";

                Repository.Execute(session.Connection, strSql, 
                    new { processGUID = processGUID, version = version },  
                    session.Transaction);

                if (localStorage != null)
                {
                    //delete the xml file
                    var serverPath = ConfigHelper.GetAppSettingString("WorkflowFileServer");
                    var physicalFileName = string.Format("{0}\\{1}", serverPath, entity.XmlFilePath);
                    File.Delete(physicalFileName);
                }

                session.Commit();
            }
            catch (System.Exception ex)
            {
                session.Rollback();
                throw;
            }
            finally
            {
                session.Dispose();
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
        internal int CreateProcess(ProcessEntity entity, IXPDLStorage extStorage = null)
        {
            int processID = 0;
            var session = SessionFactory.CreateSession();

            try
            {
                session.BeginTrans();

                entity.ProcessGUID = Guid.NewGuid().ToString();
                entity.Version = "1";     //default version value;
                entity.IsUsing = 1;
                entity.CreatedDateTime = DateTime.Now;
                entity.XmlFilePath = string.Format("{0}\\{1}", entity.AppType, entity.XmlFileName);
                XmlDocument xmlDoc = GenerateXmlContent(entity);
                entity.XmlContent = xmlDoc.OuterXml;    //流程定义文件

                processID = Insert(session.Connection, entity, session.Transaction);

                if (extStorage != null)
                {
                    extStorage.Save(entity.XmlFilePath, xmlDoc);
                }
                session.Commit();

                return processID;
            }
            catch (System.Exception ex)
            {
                session.Rollback();
                throw;
            }
            finally
            {
                session.Dispose();
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
        internal void SaveProcessFile(ProcessFileEntity entity, IXPDLStorage extStorage = null)
        {
            //默认数据库存储
            var session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();

                var processEntity = GetByVersion(entity.ProcessGUID, entity.Version);
                processEntity.XmlContent = entity.XmlContent;
                processEntity.LastUpdatedDateTime = DateTime.Now;

                //本地存储
                if (extStorage != null)
                {
                    var xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(entity.XmlContent);
                    
                    extStorage.Save(processEntity.XmlFilePath, xmlDoc);
                }

                //数据库存储
                Repository.Update<ProcessEntity>(session.Connection, processEntity, session.Transaction);

                session.Commit();
            }
            catch
            {
                session.Rollback();
                throw;
            }
            finally
            {
                session.Dispose();
            }
        }

        /// <summary>
        /// 读取流程XML文件内容
        /// </summary>
        /// <param name="processGUID">流程GUID</param>
        /// <param name="version">版本</param>
        /// <param name="extStorage">存储</param>
        /// <returns>流程文件实体</returns>
        internal ProcessFileEntity GetProcessFile(string processGUID, string version, IXPDLStorage extStorage = null)
        {
            var processEntity = GetByVersion(processGUID, version);

            //流程文件实体
            var processFileEntity = new ProcessFileEntity();
            processFileEntity.ProcessGUID = processEntity.ProcessGUID;
            processFileEntity.ProcessName = processEntity.ProcessName;
            processFileEntity.Version = processEntity.Version;
            processFileEntity.Description = processEntity.Description;

            if (extStorage != null)
            {
                //扩展方式读取xml文件内容
                var xmlDoc = extStorage.Read(processEntity);
                processFileEntity.XmlContent = xmlDoc.OuterXml;
            }
            else
            {
                processFileEntity.XmlContent = processEntity.XmlContent;
            }
            return processFileEntity;
        }

        /// <summary>
        /// 读取Xml文档
        /// </summary>
        /// <param name="processGUID">流程GUID</param>
        /// <param name="version">版本</param>
        /// <param name="extStorage">存储</param>
        /// <returns>Xml文档</returns>
        internal XmlDocument GetProcessXmlDocument(string processGUID, string version, IXPDLStorage extStorage = null)
        {
            var processEntity = GetByVersion(processGUID, version);

            var xmlDoc = new XmlDocument();
            if (extStorage != null)
            {
                xmlDoc = extStorage.Read(processEntity);
            }
            else
            {
                xmlDoc.LoadXml(processEntity.XmlContent);
            }

            return xmlDoc;
        }
        #endregion
    }
}
