
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
using Slickflow.Engine.Xpdl.Common;
using Slickflow.Engine.Xpdl.Entity;
using System.Data.SqlTypes;

namespace Slickflow.Engine.Business.Manager
{
    /// <summary>
    /// Process Manager
    /// 流程定义管理类
    /// </summary>
    public class ProcessManager : ManagerBase
    {
        #region Obtain process data 获取流程数据
        /// <summary>
        /// Obtain process entity based on ID
        /// 根据ID获取流程实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ProcessEntity GetByID(int id)
        {
            var entity = Repository.GetById<ProcessEntity>(id);
            return entity;
        }

        /// <summary>
        /// Obtain process entity based on ID
        /// 根据ID获取流程实体
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="id"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public ProcessEntity GetByID(IDbConnection conn, int id, IDbTransaction trans)
        {
            var entity = Repository.GetById<ProcessEntity>(conn, id, trans);
            return entity;
        }

        /// <summary>
        /// Obtain the process based on the processID and version number
        /// Notes: throwException means whether to throw an exception if the query cannot be found
        /// 根据流程GUID和版本标识获取流程
        /// 说明:throwException 为如果查询不到，是否抛出异常
        /// </summary>
        public ProcessEntity GetByVersion(string processID, string version, bool throwException = true)
        {
            using (var session = SessionFactory.CreateSession())
            {
                return GetByVersion(session.Connection, processID, version, throwException, session.Transaction);
            }
        }

        /// <summary>
        /// Obtain the process based on the processID and version number
        /// Notes: throwException means whether to throw an exception if the query cannot be found
        /// 根据流程GUID和版本标识获取流程
        /// 说明:throwException 为如果查询不到，是否抛出异常
        /// </summary>
        public ProcessEntity GetByVersion(IDbConnection conn, 
            string processID, 
            string version, 
            bool throwException = true,
            IDbTransaction trans = null)
        {
            ProcessEntity entity = null;
            var sql = @"SELECT 
                            * 
                        FROM WfProcess 
                        WHERE ProcessID=@processID 
                            AND VERSION=@version";
            var list = Repository.Query<ProcessEntity>(conn, 
                sql,
                new
                {
                    processID = processID,
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
                        string.Format("ProcessID: {0}, Version: {1}", processID, version)
                    ));
                }
            }
            return entity;
        }

        /// <summary>
        /// Get the current version of the process being used
        /// 获取当前使用的流程版本
        /// </summary>
        public ProcessEntity GetVersionUsing(string processID, bool isNotThrownException = true)
        {
            using (var session = SessionFactory.CreateSession())
            {
                return GetVersionUsing(session.Connection, processID, isNotThrownException, session.Transaction);
            }
        }

        /// <summary>
        /// Get the current version of the process being used
        /// 获取当前使用的流程版本
        /// </summary>
        public ProcessEntity GetVersionUsing(IDbConnection conn, 
            string processID, 
            bool isNotThrownException,
            IDbTransaction trans)
        {
            ProcessEntity entity = null;
            var sql = @"SELECT 
                            * 
                        FROM WfProcess 
                        WHERE ProcessID=@processID 
                            AND IsUsing=1 
                        ORDER BY ID DESC";             //current using process definition record
            var list = Repository.Query<ProcessEntity>(conn,
                sql,
                new
                {
                    processID = processID
                },
                trans
                ).ToList<ProcessEntity>();

            if (list.Count() == 1)
            {
                entity = list[0];
            }
            else
            {
                if (isNotThrownException == false)
                {
                    throw new ApplicationException(LocalizeHelper.GetEngineMessage("processmanager.getbyversion.error",
                        string.Format("ProcessID: {0}", processID)
));
                }
            }
            return entity;
        }

        /// <summary>
        /// Obtain the process based on the process name and version identifier
        /// 根据流程名称和版本标识获取流程
        /// </summary>
        /// <param name="processName"></param>
        /// <param name="version"></param>
        /// <returns></returns>
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
        /// Obtain the process based on the process code and version identifier
        /// 根据流程代码和版本标识获取流程
        /// </summary>
        /// <param name="processCode"></param>
        /// <param name="version"></param>
        /// <returns></returns>
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
        /// Obtain process records according to version
        /// 按照版本获取流程记录
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="processID"></param>
        /// <param name="version"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        internal ProcessEntity GetByVersionInternal(IDbConnection conn,
            string processID, 
            string version,
            IDbTransaction trans)
        {
            ProcessEntity entity = null;
            string sql = @"SELECT 
                            * 
                        FROM WfProcess 
                        WHERE ProcessID=@processID 
                            AND VERSION=@version";
            var list = Repository.Query<ProcessEntity>(conn,
                sql, 
                new { 
                    processID = processID, 
                    version = version 
                },
                trans)
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
        /// Obtain process based on message subject
        /// 根据消息主题获取流程
        /// </summary>
        /// <param name="topic"></param>
        /// <returns></returns>
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
        /// Obtain process based on App type
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
        /// Get all process
        /// 获取所有流程记录
        /// </summary>
        /// <returns></returns>
        public List<ProcessEntity> GetAll()
        {
            var list = Repository.GetAll<ProcessEntity>().ToList<ProcessEntity>();
            return list;
        }

        /// <summary>
        /// List of process definition records for obtaining basic attributes
        /// 获取基本属性的流程定义记录列表
        /// </summary>
        /// <returns></returns>
        public List<ProcessEntity> GetListSimple()
        {
            var sql = @"SELECT 
                        ID, 
                        ProcessID, 
                        ProcessName,
                        ProcessCode,
                        Version,
                        IsUsing,
                        AppType,
                        PackageType,
                        PackageID,
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

        #region Addition, deletion, modification, and search of process records 新增、更新和删除流程数据
        /// <summary>
        /// Insert process
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
        /// Insert process
        /// 新增流程记录
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="entity"></param>
        /// <param name="trans"></param>
        public int Insert(IDbConnection conn, ProcessEntity entity, IDbTransaction trans)
        {
            var entityExisted = GetByVersionInternal(conn, entity.ProcessID, entity.Version, trans);
            if (entityExisted != null)
            {
                throw new ApplicationException(LocalizeHelper.GetEngineMessage("processmanager.insert.error"));
            }

            return Repository.Insert<ProcessEntity>(conn, entity, trans);
        }

        /// <summary>
        /// Update process
        /// 更新流程记录
        /// </summary>
        /// <param name="entity"></param>
        public void Update(ProcessEntity entity)
        {
            var entityDB = GetByVersion(entity.ProcessID, entity.Version);

            IDbSession session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();
                if (entityDB.PackageType == (short)PackageTypeEnum.Package)
                {
                    //更新主流程泳道流程的使用状态信息
                    //Update the usage status information of the main flow lane process
                    var sql = @"UPDATE WfProcess
                                SET IsUsing=@isUsing
                                WHERE PackageID=@packageID";
                    Repository.Execute(session.Connection,
                        sql, 
                        new { isUsing = entity.IsUsing, packageID = entity.ID },
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
        /// Update process
        /// 流程定义记录更新
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="entity"></param>
        /// <param name="trans"></param>
        public void Update(IDbConnection conn, ProcessEntity entity, IDbTransaction trans)
        {
            Repository.Update<ProcessEntity>(conn, entity, trans);
        }

        /// <summary>
        /// Update process version IsUsing=0
        /// 更新流程版本IsUsing=0
        /// </summary>
        internal void UpdateUsingState(string processID, 
            string version,
            byte usingState)
        {
            var session = SessionFactory.CreateSession();

            try
            {
                //string strSql = @"UPDATE WfProcess 
                //                  SET IsUsing=0 
                //                  WHERE  ProcessID=@processID";
                //Repository.Execute(conn, strSql, new { processID = processID }, trans);
                session.BeginTrans();
                var entity = GetByVersion(session.Connection, processID, version, true, session.Transaction);
                entity.IsUsing = usingState;
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
        /// Upgrade process version
        /// 升级流程记录
        /// </summary>
        public int Upgrade(string processID, string version, string newVersion)
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
                var entity = GetByVersion(session.Connection, processID, version, true, session.Transaction);
                var originProcessID = entity.ID;

                entity.Version = newVersion;
                entity.CreatedDateTime = System.DateTime.Now;
                //升级主流程版本
                //Upgrade package process version
                newProcessID = Repository.Insert<ProcessEntity>(session.Connection, entity, session.Transaction);

                if (entity.PackageType == (short)PackageTypeEnum.Package)
                {
                    //升级泳道流程版本
                    //Upgrade pool process version
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
        /// Upgrade pool process
        /// 升级泳道流程记录
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="originProcessID"></param>
        /// <param name="newProcessID"></param>
        /// <param name="newVersion"></param>
        /// <param name="trans"></param>
        public void UpgradePoolProcess(IDbConnection conn,
            int originProcessID,
            int newProcessID,
            string newVersion,
            IDbTransaction trans)
        {
            //升级泳道流程的使用状态信息
            //Upgrade the usage status information of the lane process
            var selSql = @"SELECT * FROM WfProcess
                        WHERE PackageType = 2
                            AND PackageID=@packageID";
            var list = Repository.Query<ProcessEntity>(conn,
                selSql,
                new { packageID = originProcessID },
                trans);
        }

        /// <summary>
        /// Delete process
        /// 删除流程记录
        /// </summary>
        public void Delete(string processID, string version)
        {
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                var entity = GetByVersion(processID, version);

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
        /// Delete process
        /// 删除流程记录
        /// </summary>
        public void Delete(IDbConnection conn, string processID, IDbTransaction trans)
        {
            string strSql = "DELETE FROM WfProcess  WHERE  ProcessID=@processID";
            Repository.Execute(conn, strSql, new { processID = processID }, trans);
        }

        /// <summary>
        /// Delete process
        /// 删除流程记录
        /// </summary>
        public void DeleteProcess(string processID, string version)
        {
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();
                var entity = GetByVersion(processID, version);
                if (entity.PackageType == (short)PackageTypeEnum.Package)
                {
                    //删除泳道流程
                    //Delete pool process
                    string strPoolSql = @"DELETE FROM WfProcess  
                                WHERE  PackageID=@packageID";

                    Repository.Execute(session.Connection, strPoolSql,
                        new { packageID = entity.ID },
                        session.Transaction);

                }

                //删除主流程
                //Delete package process
                string strSql = @"DELETE FROM WfProcess  
                                WHERE  ProcessID=@processID
                                    AND Version=@version";

                Repository.Execute(session.Connection, strSql, 
                    new { processID = processID, version = version },  
                    session.Transaction);

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

        #region Process XML file operation 流程xml文件操作
        /// <summary>
        /// Process XML file operation
        /// 流程定义的创建方法
        /// </summary>
        internal int CreateProcess(ProcessEntity entity)
        {
            int processID = 0;
            var session = SessionFactory.CreateSession();

            try
            {
                session.BeginTrans();
                if (string.IsNullOrEmpty(entity.ProcessID))
                {
                    entity.ProcessID = Guid.NewGuid().ToString();
                }
                
                if (String.IsNullOrEmpty(entity.Version))
                {
                    entity.Version = "1";     //default version value;
                }
                entity.IsUsing = 1;
                entity.CreatedDateTime = DateTime.Now;
                processID = Insert(session.Connection, entity, session.Transaction);
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
        /// Create process by xml content
        /// 根据XML创建新流程
        /// </summary>
        internal ProcessFileEntity CreateProcessByXML(string xmlContent)
        {
            var processFileEntity = new ProcessFileEntity();

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlContent);
            var root = xmlDoc.DocumentElement;

            //saving single process
            var xmlNodeProcess = root.SelectSingleNode(XPDLDefinition.BPMN_StrXmlPath_Process,
                XPDLHelper.GetSlickflowXmlNamespaceManager(xmlDoc));
            if (xmlNodeProcess != null)
            {
                var process = ProcessModelHelper.ConvertFromXmlNodeProcess(xmlNodeProcess);
                process.XmlContent = xmlContent;
                process.Version = "1";

                var processManager = new ProcessManager();
                var processDB = processManager.GetByVersion(process.ProcessID, process.Version, false);
                if (processDB != null)
                {
                    throw new ApplicationException(LocalizeHelper.GetEngineMessage("processmanager.import.sameprocessexist.error"));
                }
                else
                {
                    //saving single process file
                    var processEntity = new ProcessEntity();
                    processEntity.ProcessID = !string.IsNullOrEmpty(process.ProcessID) ? process.ProcessID : Guid.NewGuid().ToString();
                    processEntity.ProcessName = process.ProcessName;
                    processEntity.ProcessCode = !string.IsNullOrEmpty(process.ProcessCode) ? process.ProcessCode : process.ProcessID;
                    processEntity.Version = process.Version;
                    processEntity.IsUsing = process.IsUsing;
                    processEntity.XmlContent = process.XmlContent;
                    processEntity.CreatedDateTime = DateTime.Now;

                    //数据库存储
                    //Insert into database
                    processManager.Insert(processEntity);

                    //Set process file entity
                    processFileEntity.ProcessID = processEntity.ProcessID;
                    processFileEntity.ProcessCode = processEntity.ProcessCode;
                    processFileEntity.ProcessName = processEntity.ProcessName;
                    processFileEntity.Version = processEntity.Version;
                    processFileEntity.XmlContent = processEntity.XmlContent;
                    processFileEntity.IsUsing = processEntity.IsUsing;
                }
            }
            else
            {
                throw new ApplicationException(LocalizeHelper.GetEngineMessage("processmanager.import.emptyprocessnode.error"));
            }
            return processFileEntity;
        }



        /// <summary>
        /// Save process file
        /// 保存XML文件
        /// </summary>
        /// <param name="entity"></param>
        internal void SaveProcessFile(ProcessFileEntity entity)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(entity.XmlContent);
            var root = xmlDoc.DocumentElement;

            var xmlNodeCollaboration = root.SelectSingleNode(XPDLDefinition.BPMN_StrXmlPath_Collaboration,
                XPDLHelper.GetSlickflowXmlNamespaceManager(xmlDoc));
            if (xmlNodeCollaboration == null)
            {
                //saving single process
                SaveProcessFileSingle(entity);
            }
            else
            {
                //saving process by pool/swimlane
                SaveProcessFileWithPool(entity, xmlDoc, xmlNodeCollaboration);
            }
        }

        /// <summary>
        /// Save process file single
        /// 保存XML文件
        /// </summary>
        /// <param name="entity"></param>
        internal void SaveProcessFileSingle(ProcessFileEntity entity)
        {
            //检查流程名称是否为空
            //Check if the process name is empty
            if (string.IsNullOrEmpty(entity.ProcessName))
            {
                throw new ApplicationException(LocalizeHelper.GetEngineMessage("processmanager.saveprocessfile.noprocessname.error"));
            }

            var session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();

                var processEntity = GetByVersion(session.Connection, entity.ProcessID, entity.Version, false, session.Transaction);
                if (processEntity != null)
                {
                    //删除泳道流程
                    //Delete pool process
                    DeletePoolProcess(session.Connection, processEntity.ID, session.Transaction);

                    //更新主流程信息
                    //Update process info
                    processEntity.ProcessName = entity.ProcessName;
                    processEntity.ProcessCode = entity.ProcessCode;
                    processEntity.XmlContent = entity.XmlContent;
                    processEntity.PackageType = null;

                    //数据库存储
                    //Update to database
                    processEntity.LastUpdatedDateTime = DateTime.Now;
                    Repository.Update<ProcessEntity>(session.Connection, processEntity, session.Transaction);
                }
                else
                {
                    processEntity = new ProcessEntity();
                    processEntity.ProcessID = !string.IsNullOrEmpty(entity.ProcessID) ? entity.ProcessID : Guid.NewGuid().ToString();
                    processEntity.ProcessName = entity.ProcessName;
                    processEntity.ProcessCode = !string.IsNullOrEmpty(entity.ProcessCode) ? entity.ProcessCode : entity.ProcessName;
                    processEntity.Version = entity.Version;
                    processEntity.IsUsing = entity.IsUsing;
                    processEntity.XmlContent = entity.XmlContent;
                    processEntity.CreatedDateTime = DateTime.Now;

                    //数据库存储
                    //Insert into database
                    Repository.Insert<ProcessEntity>(session.Connection, processEntity, session.Transaction);
                }
                session.Commit();
            }
            catch (System.Exception ex)
            {
                LogManager.RecordLog(WfDefine.WF_XPDL_ERROR, LogEventType.Error, LogPriority.High, null, ex);
                session.Rollback();
                throw;
            }
            finally
            {
                session.Dispose();
            }
        }

        /// <summary>
        /// Save process file with pool
        /// 保存XML文件
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="xmlDoc"></param>
        /// <param name="xmlNodeCollaboration"></param>
        internal void SaveProcessFileWithPool(ProcessFileEntity entity,
            XmlDocument xmlDoc,
            XmlNode xmlNodeCollaboration)
        {
            var packageClient = CollaborationModelHelper.ConvertCollaborationFromXml(xmlDoc, xmlNodeCollaboration);
            //检查流程名称是否为空
            //Check if the process name is empty
            if (string.IsNullOrEmpty(packageClient.Name))
            {
                throw new ApplicationException(LocalizeHelper.GetEngineMessage("processmanager.saveprocessfile.noprocessname.error"));
            }

            var poolProcessListClient = packageClient.ParticipantList;
            var session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();
                var packageDB = GetByVersion(session.Connection, packageClient.CollaborationID, entity.Version, false, session.Transaction);
                if (packageDB == null)
                {
                    var processCollaboration = new ProcessEntity();
                    processCollaboration.ProcessName = packageClient.Name;
                    processCollaboration.ProcessID = packageClient.CollaborationID;
                    processCollaboration.ProcessCode = String.IsNullOrEmpty(packageClient.Code) ? string.Format("{0}_001", packageClient.Name)
                        : packageClient.Code;
                    processCollaboration.PackageType = (byte)PackageTypeEnum.Package;
                    processCollaboration.Version = "1";         //default version
                    processCollaboration.IsUsing = 1;
                    processCollaboration.XmlContent = entity.XmlContent;
                    processCollaboration.CreatedDateTime = DateTime.Now;

                    //插入Package流程
                    //Insert package process
                    var newPackageID = Insert(session.Connection, processCollaboration, session.Transaction);

                    //插入泳道流程
                    //Insert pool process
                    foreach (var pool in poolProcessListClient)
                    {
                        var child = GetByVersion(session.Connection, pool.Process.ProcessID, entity.Version, false, session.Transaction);
                        if (child == null)
                        {
                            var processParticipant = new ProcessEntity();
                            processParticipant.PackageType = (byte)PackageTypeEnum.Pool;
                            processParticipant.PackageID = newPackageID;
                            processParticipant.ProcessName = pool.Name;
                            processParticipant.ProcessCode = String.IsNullOrEmpty(pool.Code) ? string.Format("{0}_001", pool.Name)
                                : pool.Code;
                            processParticipant.ProcessID = pool.Process.ProcessID;
                            processParticipant.ParticipantGUID = pool.ParticipantID;
                            processParticipant.Version = "1";
                            processParticipant.IsUsing = 1;
                            processParticipant.XmlContent = entity.XmlContent;
                            processParticipant.CreatedDateTime = DateTime.Now;
                            Insert(session.Connection, processParticipant, session.Transaction);
                        }
                        else
                        {
                            child.PackageID = newPackageID;
                            child.PackageType = (byte)PackageTypeEnum.Pool;
                            child.ProcessID = pool.Process.ProcessID;
                            child.ParticipantGUID = pool.ParticipantID;
                            var childName = String.IsNullOrEmpty(pool.Name) ? pool.Process.Name : pool.Name;
                            if (string.IsNullOrEmpty(childName))
                            {
                                childName = pool.Process.ProcessID;
                            }
                            child.ProcessName = childName;
                            child.ProcessCode = String.IsNullOrEmpty(pool.Code) ? string.Format("{0}_001", childName)
                                : pool.Code;
                            child.XmlContent = entity.XmlContent;
                            child.LastUpdatedDateTime = DateTime.Now;
                            Update(session.Connection, child, session.Transaction);
                        }
                    }
                }
                else
                {
                    var poolProcessListDatabase = GetPoolProcessList(session.Connection, packageDB.ID, session.Transaction);
                    List<int> poolProcessIDs = new List<int>();
                    foreach (var pool in poolProcessListDatabase)
                    {
                        if (poolProcessListClient.Count() == 0)
                        {
                            poolProcessIDs.Add(pool.ID);
                        }
                        else
                        {
                            var item = poolProcessListClient.Find(a=>a.ParticipantID == pool.ParticipantGUID);
                            if (item == null)
                            {
                                poolProcessIDs.Add(pool.ID);
                            }
                        }
                    }

                    //删除流程图中不含有的泳道流程
                    //Delete lane processes that are not included in the flowchart
                    if (poolProcessIDs.Count() > 0)
                    {
                        DeletePoolProcess(session.Connection, packageDB.ID, poolProcessIDs, session.Transaction);
                    }

                    //如果是泳道流程变为单一流程，更新PackageType和PackageID
                    //If the lane process becomes a single process, update the PackageType and PackageID
                    if (poolProcessListClient.Count() == 0)
                    {
                        packageDB.PackageType = null;
                        packageDB.PackageID = null;
                    }

                    //更新主流程，更新或插入泳道流
                    //Update the main process, update or insert lane flow
                    packageDB.PackageType = (byte)PackageTypeEnum.Package;
                    packageDB.ProcessName = entity.ProcessName;
                    packageDB.XmlContent = entity.XmlContent;
                    packageDB.LastUpdatedDateTime = DateTime.Now;
                    Update(session.Connection, packageDB, session.Transaction);

                    foreach (var pool in poolProcessListClient)
                    {
                        var child = GetByVersion(session.Connection, pool.Process.ProcessID, entity.Version, false, session.Transaction);
                        if (child == null)
                        {
                            //插入泳道流程
                            //Insert pool process
                            var processChild = new ProcessEntity();
                            processChild.ProcessID = pool.Process.ProcessID;
                            processChild.ParticipantGUID = pool.ParticipantID;
                            processChild.PackageType = (byte)PackageTypeEnum.Pool;
                            processChild.ProcessName = pool.Name;
                            processChild.ProcessCode = String.IsNullOrEmpty(pool.Code) ? string.Format("{0}_001", pool.Name)
                            : pool.Code;
                            processChild.PackageID = packageDB.ID;
                            processChild.Version = packageDB.Version;
                            processChild.XmlContent = entity.XmlContent;
                            processChild.CreatedDateTime = DateTime.Now;
                            Insert(session.Connection, processChild, session.Transaction);
                        }
                        else
                        {
                            child.ProcessID = pool.Process.ProcessID;
                            child.ParticipantGUID = pool.ParticipantID;
                            child.PackageType = (byte)PackageTypeEnum.Pool;
                            child.ProcessName = pool.Name;
                            child.ProcessCode = String.IsNullOrEmpty(pool.Code) ? string.Format("{0}_001", pool.Name)
                                : pool.Code;
                            child.XmlContent = entity.XmlContent;
                            child.LastUpdatedDateTime = DateTime.Now;
                            Update(session.Connection, child, session.Transaction);
                        }
                    }
                }
                session.Commit();
            }
            catch(System.Exception ex)
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
       /// Set process timer 
       /// 更新流程定时器信息
       /// </summary>
        internal void SetProcessTimerType(string processID, string version)
        {
            var session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();
                var entity = GetByVersion(session.Connection, processID, version, true, session.Transaction);
                var processModel = ProcessModelFactory.CreateByProcess(entity);

                //StartNode
                var startNode = processModel.GetStartActivity();
                if (startNode != null)
                {
                    if (startNode.TriggerDetail != null)
                    {
                        if (startNode.TriggerDetail.TriggerType == TriggerTypeEnum.Timer)
                        {
                            entity.StartType = (byte)ProcessStartTypeEnum.Timer;
                            entity.StartExpression = startNode.TriggerDetail.Expression;
                        }
                        else if (startNode.TriggerDetail.TriggerType == TriggerTypeEnum.Message)
                        {
                            entity.StartType = (byte)ProcessStartTypeEnum.Message;
                            entity.StartExpression = startNode.TriggerDetail.Expression;
                        }
                    }
                }

                //EndNode
                var endNode = processModel.GetEndActivity();
                if (endNode != null)
                {
                    if (endNode.TriggerDetail != null)
                    {
                        if (endNode.TriggerDetail.TriggerType == TriggerTypeEnum.Timer)
                        {
                            entity.EndType = (byte)ProcessEndTypeEnum.Timer;
                            entity.EndExpression = endNode.TriggerDetail.Expression;
                        }
                        else if (endNode.TriggerDetail.TriggerType == TriggerTypeEnum.Message)
                        {
                            entity.EndType = (byte)ProcessEndTypeEnum.Message;
                            entity.EndExpression = endNode.TriggerDetail.Expression;
                        }
                    }
                }

                //更新流程定义实体
                //Update process entity
                Update(session.Connection, entity, session.Transaction);
                session.Commit();
            }
            catch (System.Exception ex)
            {
                session.Rollback();
                throw new ApplicationException(LocalizeHelper.GetEngineMessage("processmanager.setprocesstimertype.error", ex.Message));
            }
            finally
            {
                session.Dispose();
            }
        }

        /// <summary>
        /// Obtain the list of lane processes under the main flow
        /// 获取主流程下的泳道流程列表
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="packageID"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        private List<ProcessEntity> GetPoolProcessList(IDbConnection conn, 
            int packageID, 
            IDbTransaction trans)
        {
            string sql = @"SELECT 
                            ID, ProcessID, Version, ProcessName, ProcessCode,  ParticipantGUID
                        FROM WfProcess
                        WHERE PackageID=@packageID";
            var list = Repository.Query<ProcessEntity>(conn,
                sql,
                new
                {
                    packageID = packageID
                },
                trans).ToList();
            return list;
        }

        /// <summary>
        /// Import XML document to generate process records
        /// 导入XML文档生成流程记录
        /// </summary>
        internal void ImportProcess(string xmlContent)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlContent);
            var root = xmlDoc.DocumentElement;

            var xmlNodeCollaboration = root.SelectSingleNode(XPDLDefinition.BPMN_StrXmlPath_Collaboration,
                XPDLHelper.GetSlickflowXmlNamespaceManager(xmlDoc));
            if (xmlNodeCollaboration != null)
            {
                var package = new ProcessFileEntity();
                package.XmlContent = xmlContent;
                package.Version = "1";
                
                package.ProcessID = XMLHelper.GetXmlAttribute(xmlNodeCollaboration, "id");
                var packageDB = GetByVersion(package.ProcessID, package.Version);
                if (packageDB != null)
                {
                    throw new ApplicationException(LocalizeHelper.GetEngineMessage("processmanager.import.sameprocessexist.error"));
                }
                else
                {
                    //saving process by pool/swimlane
                    SaveProcessFileWithPool(package, xmlDoc, xmlNodeCollaboration);
                }
            }
            else
            {
                //saving single process
                var xmlNodeProcess = root.SelectSingleNode(XPDLDefinition.BPMN_StrXmlPath_Process,
                    XPDLHelper.GetSlickflowXmlNamespaceManager(xmlDoc));
                if (xmlNodeProcess != null)
                {
                    var process = ProcessModelHelper.ConvertFromXmlNodeProcess(xmlNodeProcess);
                    process.XmlContent = xmlContent;
                    process.Version = "1";
                    var processDB = GetByVersion(process.ProcessID, process.Version, false);
                    if (processDB != null)
                    {
                        throw new ApplicationException(LocalizeHelper.GetEngineMessage("processmanager.import.sameprocessexist.error"));
                    }
                    else
                    {
                        //saving single process file
                        SaveProcessFileSingle(process);
                    }
                }
                else
                {
                    throw new ApplicationException(LocalizeHelper.GetEngineMessage("processmanager.import.emptyprocessnode.error"));
                }
            }
        }

        /// <summary>
        /// Delete pool process
        /// 删除泳道流程记录
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="packageID"></param>
        /// <param name="poolProcessIDs"></param>
        /// <param name="trans"></param>
        private void DeletePoolProcess(IDbConnection conn, 
            int packageID, 
            List<int> poolProcessIDs, 
            IDbTransaction trans)
        {
            string sql = @"DELETE FROM WfProcess
                            WHERE PackageID=@packageID 
                                AND ID in @ids";
            Repository.Execute(conn, sql,
                new
                {
                    packageID = packageID,
                    ids = poolProcessIDs
                }, trans);
        }

        /// <summary>
        /// Delete pool process by package id
        /// 删除泳道流程记录
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="packageID"></param>
        /// <param name="trans"></param>
        private void DeletePoolProcess(IDbConnection conn,
            int packageID,
            IDbTransaction trans)
        {
            string sql = @"DELETE FROM WfProcess
                            WHERE PackageID=@packageID
                                AND PackageType=2";
            Repository.Execute(conn, sql,
                new
                {
                    packageID = packageID
                }, trans);
        }

        /// <summary>
        /// Read the content of the process XML file
        /// 读取流程XML文件内容
        /// </summary>
        internal ProcessFileEntity GetProcessFile(string processID, string version, IXPDLStorage extStorage = null)
        {
            var processEntity = GetByVersion(processID, version);
            var processFileEntity = FillProcessFileEntity(processEntity);
            return processFileEntity;
        }

        /// <summary>
        /// Obtain process file entity based on ID
        /// 根据ID获取流程文件实体
        /// </summary>
        /// <param name="id"></param>
        /// <param name="extStorage"></param>
        /// <returns></returns>
        internal ProcessFileEntity GetProcessFileByID(int id, IXPDLStorage extStorage = null)
        {
            var processEntity = Repository.GetById<ProcessEntity>(id);
            var processFileEntity = FillProcessFileEntity(processEntity, extStorage);
            return processFileEntity;
        }

        /// <summary>
        /// Convert process file entity
        /// 转换流程文件实体
        /// </summary>
        /// <param name="processEntity"></param>
        /// <param name="extStorage"></param>
        /// <returns></returns>
        private ProcessFileEntity FillProcessFileEntity(ProcessEntity processEntity, IXPDLStorage extStorage = null)
        {
            //流程文件实体
            //Process file entity
            var processFileEntity = new ProcessFileEntity();
            processFileEntity.ProcessID = processEntity.ProcessID;
            processFileEntity.ProcessName = processEntity.ProcessName;
            processFileEntity.ProcessCode = processEntity.ProcessCode;
            processFileEntity.Version = processEntity.Version;
            processFileEntity.StartType = processEntity.StartType;
            processFileEntity.StartExpression = processEntity.StartExpression;
            processFileEntity.Description = processEntity.Description;

            if (extStorage != null)
            {
                //扩展方式读取xml文件内容
                //Extension method for reading XML file content
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
        /// Read xml content
        /// 读取Xml文档
        /// </summary>
        internal XmlDocument GetProcessXmlDocument(string processID, string version, IXPDLStorage extStorage = null)
        {
            var processEntity = GetByVersion(processID, version);

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
