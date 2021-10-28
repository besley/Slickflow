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
using System.Xml;
using Slickflow.Data;
using Slickflow.Module.Localize;
using Slickflow.Engine.Common;
using Slickflow.Engine.Utility;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Xpdl.Entity;

namespace Slickflow.Engine.Business.Manager
{
    /// <summary>
    /// 流程定义管理类
    /// </summary>
    public class ProcessManager : ManagerBase
    {
        #region 获取流程数据
        /// <summary>
        /// 根据ID获取流程实体
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns>流程实体</returns>
        public ProcessEntity GetByID(int id)
        {
            var entity = Repository.GetById<ProcessEntity>(id);
            return entity;
        }

        /// <summary>
        /// 根据ID获取流程实体
        /// </summary>
        /// <param name="conn">链接</param>
        /// <param name="id">主键ID</param>
        /// <param name="trans">交易</param>
        /// <returns>流程实体</returns>
        public ProcessEntity GetByID(IDbConnection conn, int id, IDbTransaction trans)
        {
            var entity = Repository.GetById<ProcessEntity>(conn, id, trans);
            return entity;
        }

        /// <summary>
        /// 根据流程GUID和版本标识获取流程
        /// </summary>
        /// <param name="processGUID">流程GUID</param>
        /// <param name="version">流程版本</param>
        /// <param name="throwException">是否抛出异常</param>
        /// <returns>流程实体</returns>
        public ProcessEntity GetByVersion(string processGUID, string version, bool throwException = true)
        {
            using (var session = SessionFactory.CreateSession())
            {
                return GetByVersion(session.Connection, processGUID, version, throwException, session.Transaction);
            }
        }

        /// <summary>
        /// 根据版本选择流程
        /// </summary>
        /// <param name="conn">数据库链接</param>
        /// <param name="processGUID">流程GUID</param>
        /// <param name="version">版本</param>
        /// <param name="throwException">抛出异常</param>
        /// <param name="trans">交易</param>
        /// <returns>流程实体</returns>
        internal ProcessEntity GetByVersion(IDbConnection conn, 
            string processGUID, 
            string version, 
            bool throwException = true,
            IDbTransaction trans = null)
        {
            ProcessEntity entity = null;
            var sql = @"SELECT 
                            * 
                        FROM WfProcess 
                        WHERE ProcessGUID=@processGUID 
                            AND VERSION=@version";
            var list = Repository.Query<ProcessEntity>(conn, 
                sql,
                new
                {
                    processGUID = processGUID,
                    version = version
                },
                trans).ToList<ProcessEntity>();

            if (list.Count() == 1)
            {
                entity = list[0];
            }
            else
            {
                if (throwException == true)
                {
                    throw new ApplicationException(LocalizeHelper.GetEngineMessage("processmanager.getbyversion.error",
                        string.Format("ProcessGUID: {0}, Version: {1}", processGUID, version)
                    ));
                }
            }
            return entity;
        }

        /// <summary>
        /// 获取当前使用的流程版本
        /// </summary>
        /// <param name="processGUID">流程GUID</param>
        /// <returns>流程实体</returns>
        public ProcessEntity GetVersionUsing(string processGUID)
        {
            using (var session = SessionFactory.CreateSession())
            {
                return GetVersionUsing(session.Connection, processGUID, session.Transaction);
            }
        }

        /// <summary>
        /// 获取当前使用的流程版本
        /// </summary>
        /// <param name="conn">数据库连接</param>
        /// <param name="processGUID">流程GUID</param>
        /// <param name="trans">事务</param>
        /// <returns>流程实体</returns>
        public ProcessEntity GetVersionUsing(IDbConnection conn, string processGUID, IDbTransaction trans)
        {
            ProcessEntity entity = null;
            var sql = @"SELECT 
                            * 
                        FROM WfProcess 
                        WHERE ProcessGUID=@processGUID 
                            AND IsUsing=1 
                        ORDER BY ID DESC";             //current using process definition record
            var list = Repository.Query<ProcessEntity>(conn,
                sql,
                new
                {
                    processGUID = processGUID
                },
                trans
                ).ToList<ProcessEntity>();

            if (list.Count() == 1)
            {
                entity = list[0];
            }
            else
            {
                throw new ApplicationException(LocalizeHelper.GetEngineMessage("processmanager.getbyversion.error",
                    string.Format("ProcessGUID: {0}", processGUID)
                ));
            }
            return entity;
        }

        /// <summary>
        /// 根据流程名称和版本标识获取流程
        /// </summary>
        /// <param name="processName">流程名称</param>
        /// <param name="version">流程版本</param>
        /// <returns>流程实体</returns>
        public ProcessEntity GetByName(string processName, string version = null)
        {
            ProcessEntity entity = null;
            if (string.IsNullOrEmpty(version)) version = "1";
            var sql = @"SELECT 
                            * 
                        FROM WfProcess 
                        WHERE ProcessName=@processName 
                            AND VERSION=@version";

            var list = Repository.Query<ProcessEntity>(sql, 
                new { 
                    processName = processName, 
                    version = version 
                })
                .ToList<ProcessEntity>();


            if (list.Count() == 1)
            {
                entity = list[0];
            }
            else if (list.Count() == 0)
            {
                ;
            }
            else
            {
                throw new ApplicationException(LocalizeHelper.GetEngineMessage("processmanager.getbyname.error"));
            }
            return entity;
        }

        /// <summary>
        /// 根据流程名称和版本标识获取流程
        /// </summary>
        /// <param name="processCode">流程代码</param>
        /// <param name="version">流程版本</param>
        /// <returns>流程实体</returns>
        public ProcessEntity GetByCode(string processCode, string version = null)
        {
            ProcessEntity entity = null;
            if (string.IsNullOrEmpty(version)) version = "1";
            var sql = @"SELECT 
                            * 
                        FROM WfProcess 
                        WHERE ProcessCode=@processCode
                            AND VERSION=@version";

            var list = Repository.Query<ProcessEntity>(sql, 
                new { 
                    processCode = processCode, 
                    version = version 
                })
                .ToList<ProcessEntity>();

            if (list.Count() == 1)
            {
                entity = list[0];
            }
            else if (list.Count() == 0)
            {
                ;
            }
            else
            {
                throw new ApplicationException(LocalizeHelper.GetEngineMessage("processmanager.getbycode.error"));
            }
            return entity;
        }

        /// <summary>
        /// 按照版本获取流程记录
        /// </summary>
        /// <param name="processGUID">流程GUID</param>
        /// <param name="version">版本</param>
        /// <returns>流程实体</returns>
        internal ProcessEntity GetByVersionInternal(string processGUID, string version)
        {
            ProcessEntity entity = null;
            string sql = @"SELECT 
                            * 
                        FROM WfProcess 
                        WHERE ProcessGUID=@processGUID 
                            AND VERSION=@version";
            var list = Repository.Query<ProcessEntity>(sql, 
                new { 
                    processGUID = processGUID, 
                    version = version 
                })
                .ToList<ProcessEntity>();

            if (list.Count() > 1)
            {
                throw new ApplicationException(LocalizeHelper.GetEngineMessage("processmanager.getbyversioninternal.error"));
            }
            else if (list.Count() == 1)
            {
                entity = list[0];
            }
            return entity;
        }

        /// <summary>
        /// 根据消息主题获取流程
        /// </summary>
        /// <param name="topic">消息表达式</param>
        /// <returns>流程实体</returns>
        public ProcessEntity GetByMessage(string topic)
        {
            ProcessEntity entity = null;
            //StartType:2  --- message
            var sql = @"SELECT 
                            * 
                        FROM WfProcess 
                        WHERE StartType=2           
                            AND StartExpression=@startExpression";
            var list = Repository.Query<ProcessEntity>(
                sql, 
                new { 
                    startExpression = topic 
                })
                .ToList<ProcessEntity>();

            if (list.Count() > 1)
            {
                throw new ApplicationException(LocalizeHelper.GetEngineMessage("processmanager.getbymessage.error"));
            }
            else if (list.Count() == 1)
            {
                entity = list[0];
            }
            return entity;
        }

        /// <summary>
        /// 根据AppType获取流程记录
        /// </summary>
        /// <param name="appType"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public ProcessEntity GetSingleByAppType(string appType, 
            IDbSession session)
        {
            ProcessEntity entity = null;
            var sql = @"SELECT 
                            * 
                        FROM WfProcess 
                        WHERE AppType=@appType";
            var list = Repository.Query<ProcessEntity>(session.Connection, 
                sql,
                new { 
                    appType = appType 
                }, session.Transaction)
                .ToList<ProcessEntity>();

            if (list.Count() == 1)
            {
                entity = list[0];
            }
            else
            {
                throw new ApplicationException(LocalizeHelper.GetEngineMessage("processmanager.getsinglebyapptype.error"));
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
                        ProcessCode,
                        Version,
                        IsUsing,
                        AppType,
                        PackageType,
                        PackageProcessID,
                        PageUrl,
                        StartType,
                        StartExpression,
                        EndType,
                        EndExpression,
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
        public int Insert(ProcessEntity entity)
        {
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();
                int processID = Insert(session.Connection, entity, session.Transaction);
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
        /// 新增流程记录
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="entity"></param>
        /// <param name="trans"></param>
        public int Insert(IDbConnection conn, ProcessEntity entity, IDbTransaction trans)
        {
            var entityExisted = GetByVersionInternal(entity.ProcessGUID, entity.Version);
            if (entityExisted != null)
            {
                throw new ApplicationException(LocalizeHelper.GetEngineMessage("processmanager.insert.error"));
            }

            return Repository.Insert<ProcessEntity>(conn, entity, trans);
        }

        /// <summary>
        /// 更新流程记录
        /// </summary>
        /// <param name="entity">实体</param>
        public void Update(ProcessEntity entity)
        {
            var entityDB = GetByVersion(entity.ProcessGUID, entity.Version);

            IDbSession session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();
                if (entityDB.PackageType == (short)PackageTypeEnum.MainProcess)
                {
                    //更新主流程泳道流程的使用状态信息
                    var sql = @"UPDATE WfProcess
                                SET IsUsing=@isUsing
                                WHERE PackageProcessID=@packageProcessID";
                    Repository.Execute(session.Connection,
                        sql, 
                        new { isUsing = entity.IsUsing, packageProcessID = entity.ID },
                        session.Transaction);

                }
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
        /// <param name="version">版本</param>
        /// <param name="trans">事务</param>
        public void UpdateUsingState(IDbConnection conn, 
            string processGUID, 
            string version,
            IDbTransaction trans)
        {
            //string strSql = @"UPDATE WfProcess 
            //                  SET IsUsing=0 
            //                  WHERE  ProcessGUID=@processGUID";
            //Repository.Execute(conn, strSql, new { processGUID = processGUID }, trans);
            var entity = GetByVersion(conn, processGUID, version, true, trans);
            entity.IsUsing = 0;
            Repository.Update<ProcessEntity>(conn, entity, trans);
        }

        /// <summary>
        /// 升级流程记录
        /// </summary>
        /// <param name="processGUID">流程GUID</param>
        /// <param name="version">流程版本</param>
        /// <param name="newVersion">新版本编号</param>
        /// <returns>新流程ID</returns>
        public int Upgrade(string processGUID, string version, string newVersion)
        {
            int newProcessID = 0;
            if (string.IsNullOrEmpty(newVersion))
            {
                throw new WorkflowException(LocalizeHelper.GetEngineMessage("processmanager.upgrade.error"));
            }

            IDbSession session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();
                var entity = GetByVersion(session.Connection, processGUID, version, true, session.Transaction);
                var originProcessID = entity.ID;

                entity.Version = newVersion;
                entity.CreatedDateTime = System.DateTime.Now;
                //升级主流程版本
                newProcessID = Repository.Insert<ProcessEntity>(session.Connection, entity, session.Transaction);

                if (entity.PackageType == (short)PackageTypeEnum.MainProcess)
                {
                    //升级泳道流程版本
                    UpgradePoolProcess(session.Connection, originProcessID, newProcessID, newVersion, session.Transaction);
                }
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
            return newProcessID;
        }

        /// <summary>
        /// 升级泳道流程记录
        /// </summary>
        /// <param name="conn">数据库链接</param>
        /// <param name="originProcessID">原流程ID</param>
        /// <param name="newProcessID">新流程ID</param>
        /// <param name="newVersion">新版本</param>
        /// <param name="trans">事务</param>
        public void UpgradePoolProcess(IDbConnection conn,
            int originProcessID,
            int newProcessID,
            string newVersion,
            IDbTransaction trans)
        {
            //升级泳道流程的使用状态信息
            var selSql = @"SELECT * FROM WfProcess
                        WHERE PackageType = 2
                            AND PackageProcessID=@packageProcessID";
            var list = Repository.Query<ProcessEntity>(conn,
                selSql,
                new { packageProcessID = originProcessID },
                trans);
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
                if (entity.PackageType == (short)PackageTypeEnum.MainProcess)
                {
                    //删除泳道流程
                    string strPoolSql = @"DELETE FROM WfProcess  
                                WHERE  PackageProcessID=@packageProcessID";

                    Repository.Execute(session.Connection, strPoolSql,
                        new { packageProcessID = entity.ID },
                        session.Transaction);

                }

                //删除主流程
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
                if (string.IsNullOrEmpty(entity.ProcessGUID))
                {
                    entity.ProcessGUID = Guid.NewGuid().ToString();
                }
                
                if (String.IsNullOrEmpty(entity.Version))
                {
                    entity.Version = "1";     //default version value;
                }
                entity.IsUsing = 1;
                entity.CreatedDateTime = DateTime.Now;
                entity.XmlFilePath = string.Format("{0}\\{1}", entity.AppType, entity.XmlFileName);
                XmlDocument xmlDoc = GenerateXmlContent(entity);
                entity.XmlContent = xmlDoc.OuterXml;    //process xml file

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
            processNode.SetAttribute("id", entity.ProcessGUID);
            processNode.SetAttribute("name", entity.ProcessName);
            processNode.SetAttribute("code", entity.ProcessCode);
            processNode.SetAttribute("version", entity.Version);
            
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

                var processEntity = GetByVersion(session.Connection, entity.ProcessGUID, entity.Version, false, session.Transaction);
                var processModel = ProcessModelFactory.Create(session.Connection, entity.ProcessGUID, entity.Version, session.Transaction);
                if (processEntity != null)
                {
                    processEntity.XmlContent = entity.XmlContent;
                    SetProcessStartEndType(processModel, processEntity, entity.XmlContent);

                    processEntity.LastUpdatedDateTime = DateTime.Now;

                    //数据库存储
                    Repository.Update<ProcessEntity>(session.Connection, processEntity, session.Transaction);
                }
                else
                {
                    processEntity = new ProcessEntity();
                    processEntity.ProcessGUID = entity.ProcessGUID;
                    processEntity.ProcessName = entity.ProcessName;
                    processEntity.ProcessCode = entity.ProcessCode;
                    processEntity.Version = entity.Version;
                    processEntity.IsUsing = entity.IsUsing;
                    processEntity.XmlContent = entity.XmlContent;
                    processEntity.CreatedDateTime = DateTime.Now;
                    SetProcessStartEndType(processModel, processEntity, entity.XmlContent);

                    //数据库存储
                    Repository.Insert<ProcessEntity>(session.Connection, processEntity, session.Transaction);
                }

                //本地存储
                if (extStorage != null)
                {
                    var xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(entity.XmlContent);

                    extStorage.Save(processEntity.XmlFilePath, xmlDoc);
                }
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
        /// 设置流程的启动类型
        /// </summary>
        /// <param name="processModel">流程模型</param>
        /// <param name="entity">流程实体</param>
        /// <param name="xmlConent">XML内容</param>
        private void SetProcessStartEndType(IProcessModel processModel, ProcessEntity entity, string xmlConent)
        {
            //StartNode
            var startNode = processModel.GetActivityByType(xmlConent, entity.ProcessGUID, ActivityTypeEnum.StartNode);
            if (startNode != null)
            {
                if (startNode.ActivityTypeDetail.TriggerType == TriggerTypeEnum.Timer)
                {
                    entity.StartType = (byte)ProcessStartTypeEnum.Timer;
                    entity.StartExpression = startNode.ActivityTypeDetail.Expression;
                }
                else if (startNode.ActivityTypeDetail.TriggerType == TriggerTypeEnum.Message)
                {
                    entity.StartType = (byte)ProcessStartTypeEnum.Message;
                    entity.StartExpression = startNode.ActivityTypeDetail.Expression;
                }
            }

            //EndNode
            var endNode = processModel.GetActivityByType(xmlConent, entity.ProcessGUID, ActivityTypeEnum.EndNode);
            if (endNode != null)
            {
                if (endNode.ActivityTypeDetail.TriggerType == TriggerTypeEnum.Timer)
                {
                    entity.EndType = (byte)ProcessEndTypeEnum.Timer;
                    entity.EndExpression = endNode.ActivityTypeDetail.Expression;
                }
                else if(endNode.ActivityTypeDetail.TriggerType == TriggerTypeEnum.Message)
                {
                    entity.EndType = (byte)ProcessEndTypeEnum.Message;
                    entity.EndExpression = endNode.ActivityTypeDetail.Expression;
                }
            }
        }

        /// <summary>
        /// 或者主流程下的泳道流程列表
        /// </summary>
        /// <param name="conn">链接</param>
        /// <param name="packageProcessID">主图形GUID</param>
        /// <param name="trans">事务</param>
        /// <returns>流程实体列表</returns>
        private List<ProcessEntity> GetPoolProcessList(IDbConnection conn, 
            int packageProcessID, 
            IDbTransaction trans)
        {
            string sql = @"SELECT 
                            ID, ProcessGUID, Version, ProcessName, ProcessCode
                        FROM WfProcess
                        WHERE PackageProcessID=@packageProcessID";
            var list = Repository.Query<ProcessEntity>(conn,
                sql,
                new
                {
                    packageProcessID = packageProcessID
                },
                trans).ToList();
            return list;
        }

        /// <summary>
        /// 删除泳道流程记录
        /// </summary>
        /// <param name="conn">数据库链接</param>
        /// <param name="packageProcessID">主流程ID</param>
        /// <param name="poolProcessIDs">泳道流程ID列表</param>
        /// <param name="trans">交易</param>
        private void DeletePoolProcess(IDbConnection conn, 
            int packageProcessID, 
            List<int> poolProcessIDs, 
            IDbTransaction trans)
        {
            string sql = @"DELETE FROM WfProcess
                            WHERE PackageProcessID=@packageProcessID 
                                AND ID in @ids";
            Repository.Execute(conn, sql,
                new
                {
                    packageProcessID = packageProcessID,
                    ids = poolProcessIDs
                }, trans);
        }

        /// <summary>
        /// 保存泳道流程
        /// </summary>
        /// <param name="file">泳道流程实体</param>
        internal void SaveProcessFilePool(ProcessFilePool file)
        {
            var packageProcessClient = file.ProcessEntityList.Single(a=>a.PackageType.Value == (short)PackageTypeEnum.MainProcess);
            var poolProcessListClient = file.ProcessEntityList.Where(a => a.PackageType.Value == (short)PackageTypeEnum.PoolProcess).ToList();
            
            var session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();
                var packageProcessDB = GetByVersion(session.Connection, packageProcessClient.ProcessGUID, packageProcessClient.Version, 
                    false, session.Transaction);
                if (packageProcessDB == null)
                {
                    //插入主流程，然后插入泳道流
                    var packageGUID = Guid.NewGuid().ToString();
                    packageProcessClient.XmlContent = file.XmlContent;
                    packageProcessClient.CreatedDateTime = System.DateTime.Now;
                    var processModelClient = ProcessModelFactory.Create(session.Connection, packageProcessClient.ProcessGUID, packageProcessClient.Version, session.Transaction);
                    SetProcessStartEndType(processModelClient, packageProcessClient, file.XmlContent);

                    var newPackageProcessID = Insert(session.Connection, packageProcessClient, session.Transaction);
                    foreach (var pool in poolProcessListClient)
                    {
                        pool.PackageProcessID = newPackageProcessID;
                        pool.XmlContent = file.XmlContent;
                        var processModelPool = ProcessModelFactory.Create(session.Connection, pool.ProcessGUID, pool.Version, session.Transaction);
                        SetProcessStartEndType(processModelPool, pool, file.XmlContent);
                        Insert(session.Connection, pool, session.Transaction);
                    }
                }
                else
                {
                    var poolProcessListDataBase = GetPoolProcessList(session.Connection, packageProcessDB.ID, session.Transaction);
                    List<int> poolProcessIDs = new List<int>();
                    foreach (var pool in poolProcessListDataBase)
                    {
                        //多泳道流程变为单一流程
                        if (poolProcessListClient.Count() == 0)
                        {
                            //删除数据库中所有泳道流程
                            poolProcessIDs.Add(pool.ID);
                        }
                        else
                        {
                            var item = poolProcessListClient.Find(a => a.ProcessGUID == pool.ProcessGUID && a.Version == pool.Version);
                            if (item == null)
                            {
                                //删除ID列表
                                poolProcessIDs.Add(pool.ID);
                            }
                        }
                    }

                    //删除图形中不含有的泳道流程
                    if (poolProcessIDs.Count() > 0)
                    {
                        DeletePoolProcess(session.Connection, packageProcessDB.ID, poolProcessIDs, session.Transaction);
                    }

                    //如果是泳道流程变为单一流程，更新PackageType和PackageProcessID
                    if (poolProcessListClient.Count() == 0)
                    {
                        packageProcessDB.PackageType = null;
                        packageProcessDB.PackageProcessID = null;
                    }

                    //更新主流程，更新或插入泳道流
                    packageProcessDB.PackageType = (byte)PackageTypeEnum.MainProcess;
                    packageProcessDB.XmlContent = file.XmlContent;
                    packageProcessDB.LastUpdatedDateTime = System.DateTime.Now;
                    var processModelPackageDB = ProcessModelFactory.Create(session.Connection, packageProcessDB.ProcessGUID, packageProcessDB.Version, session.Transaction);
                    SetProcessStartEndType(processModelPackageDB, packageProcessDB, file.XmlContent);
                    Update(session.Connection, packageProcessDB, session.Transaction);

                    foreach (var pool in poolProcessListClient)
                    {
                        var child = GetByVersion(session.Connection, pool.ProcessGUID, pool.Version, false, session.Transaction);
                        if (child == null)
                        {
                            //新增泳道流程
                            pool.PackageType = (byte)PackageTypeEnum.PoolProcess;
                            pool.PackageProcessID = packageProcessDB.ID;
                            pool.XmlContent = file.XmlContent;
                            pool.CreatedDateTime = System.DateTime.Now;
                            var processModelPoolA = ProcessModelFactory.Create(session.Connection, pool.ProcessGUID, pool.Version, session.Transaction);
                            SetProcessStartEndType(processModelPoolA, pool, file.XmlContent);
                            Insert(session.Connection, pool, session.Transaction);
                        }
                        else
                        {
                            //更新泳道流程
                            child.ProcessName = pool.ProcessName;
                            child.ProcessCode = pool.ProcessCode;
                            child.Description = pool.Description;
                            child.PackageType = (byte)PackageTypeEnum.PoolProcess;
                            child.XmlContent = file.XmlContent;
                            child.LastUpdatedDateTime = System.DateTime.Now;
                            var processModelPoolChild = ProcessModelFactory.Create(session.Connection, child.ProcessGUID, child.Version, session.Transaction);
                            SetProcessStartEndType(processModelPoolChild, child, file.XmlContent);
                            Update(session.Connection, child, session.Transaction);
                        }
                    }
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
            var processFileEntity = FillProcessFileEntity(processEntity);
            return processFileEntity;
        }

        /// <summary>
        /// 根据ID获取流程文件实体
        /// </summary>
        /// <param name="id">流程ID</param>
        /// <param name="extStorage">外部存储</param>
        /// <returns></returns>
        internal ProcessFileEntity GetProcessFileByID(int id, IXPDLStorage extStorage = null)
        {
            var processEntity = Repository.GetById<ProcessEntity>(id);
            var processFileEntity = FillProcessFileEntity(processEntity, extStorage);
            return processFileEntity;
        }

        /// <summary>
        /// 转换流程文件实体
        /// </summary>
        /// <param name="processEntity">流程实体</param>
        /// <param name="extStorage">外部存储</param>
        /// <returns></returns>
        private ProcessFileEntity FillProcessFileEntity(ProcessEntity processEntity, IXPDLStorage extStorage = null)
        {
            //流程文件实体
            var processFileEntity = new ProcessFileEntity();
            processFileEntity.ProcessGUID = processEntity.ProcessGUID;
            processFileEntity.ProcessName = processEntity.ProcessName;
            processFileEntity.ProcessCode = processEntity.ProcessCode;
            processFileEntity.Version = processEntity.Version;
            processFileEntity.StartType = processEntity.StartType;
            processFileEntity.StartExpression = processEntity.StartExpression;
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
