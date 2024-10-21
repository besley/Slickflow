
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
        public ProcessEntity GetByVersion(IDbConnection conn, 
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
        /// <param name="isNotThrownException">是否抛出异常</param>
        /// <returns>流程实体</returns>
        public ProcessEntity GetVersionUsing(string processGUID, bool isNotThrownException = true)
        {
            using (var session = SessionFactory.CreateSession())
            {
                return GetVersionUsing(session.Connection, processGUID, isNotThrownException, session.Transaction);
            }
        }

        /// <summary>
        /// 获取当前使用的流程版本
        /// </summary>
        /// <param name="conn">数据库连接</param>
        /// <param name="processGUID">流程GUID</param>
        /// <param name="isNotThrownException">是否抛出异常</param>
        /// <param name="trans">事务</param>
        /// <returns>流程实体</returns>
        public ProcessEntity GetVersionUsing(IDbConnection conn, 
            string processGUID, 
            bool isNotThrownException,
            IDbTransaction trans)
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
                if (isNotThrownException == false)
                {
                    throw new ApplicationException(LocalizeHelper.GetEngineMessage("processmanager.getbyversion.error",
                        string.Format("ProcessGUID: {0}", processGUID)
));
                }
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
        /// <param name="conn">链接</param>
        /// <param name="processGUID">流程GUID</param>
        /// <param name="version">版本</param>
        /// <param name="trans">事务</param>
        /// <returns>流程实体</returns>
        internal ProcessEntity GetByVersionInternal(IDbConnection conn,
            string processGUID, 
            string version,
            IDbTransaction trans)
        {
            ProcessEntity entity = null;
            string sql = @"SELECT 
                            * 
                        FROM WfProcess 
                        WHERE ProcessGUID=@processGUID 
                            AND VERSION=@version";
            var list = Repository.Query<ProcessEntity>(conn,
                sql, 
                new { 
                    processGUID = processGUID, 
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
            var entityExisted = GetByVersionInternal(conn, entity.ProcessGUID, entity.Version, trans);
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
                if (entityDB.PackageType == (short)PackageTypeEnum.Package)
                {
                    //更新主流程泳道流程的使用状态信息
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
        /// <param name="processGUID">流程GUID</param>
        /// <param name="version">版本</param>
        /// <param name="usingState">是否启用1-using;0-unusing</param>
        internal void UpdateUsingState(string processGUID, 
            string version,
            byte usingState)
        {
            var session = SessionFactory.CreateSession();

            try
            {
                //string strSql = @"UPDATE WfProcess 
                //                  SET IsUsing=0 
                //                  WHERE  ProcessGUID=@processGUID";
                //Repository.Execute(conn, strSql, new { processGUID = processGUID }, trans);
                session.BeginTrans();
                var entity = GetByVersion(session.Connection, processGUID, version, true, session.Transaction);
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

                if (entity.PackageType == (short)PackageTypeEnum.Package)
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
                            AND PackageID=@packageID";
            var list = Repository.Query<ProcessEntity>(conn,
                selSql,
                new { packageID = originProcessID },
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
        public void DeleteProcess(string processGUID, string version)
        {
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();
                var entity = GetByVersion(processGUID, version);
                if (entity.PackageType == (short)PackageTypeEnum.Package)
                {
                    //删除泳道流程
                    string strPoolSql = @"DELETE FROM WfProcess  
                                WHERE  PackageID=@packageID";

                    Repository.Execute(session.Connection, strPoolSql,
                        new { packageID = entity.ID },
                        session.Transaction);

                }

                //删除主流程
                string strSql = @"DELETE FROM WfProcess  
                                WHERE  ProcessGUID=@processGUID
                                    AND Version=@version";

                Repository.Execute(session.Connection, strSql, 
                    new { processGUID = processGUID, version = version },  
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

        #region 流程xml文件操作
        /// <summary>
        /// 流程定义的创建方法
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        internal int CreateProcess(ProcessEntity entity)
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
        /// 保存XML文件
        /// </summary>
        /// <param name="entity">实体</param>
        internal void SaveProcessFile(ProcessFileEntity entity)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(entity.XmlContent);
            var root = xmlDoc.DocumentElement;

            var xmlNodeCollaboration = root.SelectSingleNode(XPDLDefinition.BPMN2_StrXmlPath_Collaboration,
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
        /// 保存XML文件
        /// </summary>
        /// <param name="entity">实体</param>
        internal void SaveProcessFileSingle(ProcessFileEntity entity)
        {
            //检查流程名称是否为空
            if (string.IsNullOrEmpty(entity.ProcessName))
            {
                throw new ApplicationException(LocalizeHelper.GetEngineMessage("processmanager.saveprocessfile.noprocessname.error"));
            }

            //默认数据库存储
            var session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();

                var processEntity = GetByVersion(session.Connection, entity.ProcessGUID, entity.Version, false, session.Transaction);
                if (processEntity != null)
                {
                    // 删除泳道流程
                    DeletePoolProcess(session.Connection, processEntity.ID, session.Transaction);

                    //更新主流程信息
                    processEntity.ProcessName = entity.ProcessName;
                    processEntity.ProcessCode = entity.ProcessCode;
                    processEntity.XmlContent = entity.XmlContent;
                    processEntity.PackageType = null;

                    //数据库存储
                    processEntity.LastUpdatedDateTime = DateTime.Now;
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

                    //数据库存储
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
        /// 保存XML文件
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="xmlDoc">流程XML文档</param>
        /// <param name="xmlNodeCollaboration">xml节点</param>
        internal void SaveProcessFileWithPool(ProcessFileEntity entity,
            XmlDocument xmlDoc,
            XmlNode xmlNodeCollaboration)
        {
            var packageClient = CollaborationModelHelper.ConvertCollaborationFromXml(xmlDoc, xmlNodeCollaboration);
            //检查流程名称是否为空
            if (string.IsNullOrEmpty(packageClient.Name))
            {
                throw new ApplicationException(LocalizeHelper.GetEngineMessage("processmanager.saveprocessfile.noprocessname.error"));
            }

            var poolProcessListClient = packageClient.ParticipantList;
            var session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();
                var packageDB = GetByVersion(session.Connection, packageClient.CollaborationGUID, entity.Version, false, session.Transaction);
                if (packageDB == null)
                {
                    var processCollaboration = new ProcessEntity();
                    processCollaboration.ProcessName = packageClient.Name;
                    processCollaboration.ProcessGUID = packageClient.CollaborationGUID;
                    processCollaboration.ProcessCode = String.IsNullOrEmpty(packageClient.Code) ? string.Format("{0}_001", packageClient.Name)
                        : packageClient.Code;
                    processCollaboration.PackageType = (byte)PackageTypeEnum.Package;
                    processCollaboration.Version = "1";         //default version
                    processCollaboration.IsUsing = 1;
                    processCollaboration.XmlContent = entity.XmlContent;
                    processCollaboration.CreatedDateTime = DateTime.Now;

                    //插入Package流程
                    var newPackageID = Insert(session.Connection, processCollaboration, session.Transaction);

                    //插入泳道流程
                    foreach (var pool in poolProcessListClient)
                    {
                        var child = GetByVersion(session.Connection, pool.Process.ProcessGUID, entity.Version, false, session.Transaction);
                        if (child == null)
                        {
                            var processParticipant = new ProcessEntity();
                            processParticipant.PackageType = (byte)PackageTypeEnum.Pool;
                            processParticipant.PackageID = newPackageID;
                            processParticipant.ProcessName = pool.Name;
                            processParticipant.ProcessCode = String.IsNullOrEmpty(pool.Code) ? string.Format("{0}_001", pool.Name)
                                : pool.Code;
                            processParticipant.ProcessGUID = pool.Process.ProcessGUID;
                            processParticipant.ParticipantGUID = pool.ParticipantGUID;
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
                            child.ProcessGUID = pool.Process.ProcessGUID;
                            child.ParticipantGUID = pool.ParticipantGUID;
                            var childName = String.IsNullOrEmpty(pool.Name) ? pool.Process.Name : pool.Name;
                            if (string.IsNullOrEmpty(childName))
                            {
                                childName = pool.Process.ID;
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
                            var item = poolProcessListClient.Find(a=>a.ParticipantGUID == pool.ParticipantGUID);
                            if (item == null)
                            {
                                poolProcessIDs.Add(pool.ID);
                            }
                        }
                    }

                    //删除流程图中不含有的泳道流程
                    if (poolProcessIDs.Count() > 0)
                    {
                        DeletePoolProcess(session.Connection, packageDB.ID, poolProcessIDs, session.Transaction);
                    }

                    //如果是泳道流程变为单一流程，更新PackageType和PackageID
                    if (poolProcessListClient.Count() == 0)
                    {
                        packageDB.PackageType = null;
                        packageDB.PackageID = null;
                    }

                    //更新主流程，更新或插入泳道流
                    packageDB.PackageType = (byte)PackageTypeEnum.Package;
                    packageDB.ProcessName = entity.ProcessName;
                    packageDB.XmlContent = entity.XmlContent;
                    packageDB.LastUpdatedDateTime = DateTime.Now;
                    Update(session.Connection, packageDB, session.Transaction);

                    foreach (var pool in poolProcessListClient)
                    {
                        var child = GetByVersion(session.Connection, pool.Process.ProcessGUID, entity.Version, false, session.Transaction);
                        if (child == null)
                        {
                            //插入泳道流程
                            var processChild = new ProcessEntity();
                            processChild.ProcessGUID = pool.Process.ProcessGUID;
                            processChild.ParticipantGUID = pool.ParticipantGUID;
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
                            child.ProcessGUID = pool.Process.ProcessGUID;
                            child.ParticipantGUID = pool.ParticipantGUID;
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
       /// 更新流程定时器信息
       /// </summary>
       /// <param name="processGUID">流程GUID</param>
       /// <param name="version">流程版本</param>
        internal void SetProcessTimerType(string processGUID, string version)
        {
            var session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();
                var entity = GetByVersion(session.Connection, processGUID, version, true, session.Transaction);
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
        /// 或者主流程下的泳道流程列表
        /// </summary>
        /// <param name="conn">链接</param>
        /// <param name="packageID">主图形GUID</param>
        /// <param name="trans">事务</param>
        /// <returns>流程实体列表</returns>
        private List<ProcessEntity> GetPoolProcessList(IDbConnection conn, 
            int packageID, 
            IDbTransaction trans)
        {
            string sql = @"SELECT 
                            ID, ProcessGUID, Version, ProcessName, ProcessCode,  ParticipantGUID
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
        /// 导入XML文档生成流程记录
        /// </summary>
        /// <param name="xmlContent">XML文档</param>
        /// <returns>新流程ID</returns>
        internal void ImportProcess(string xmlContent)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlContent);
            var root = xmlDoc.DocumentElement;

            var xmlNodeCollaboration = root.SelectSingleNode(XPDLDefinition.BPMN2_StrXmlPath_Collaboration,
                XPDLHelper.GetSlickflowXmlNamespaceManager(xmlDoc));
            if (xmlNodeCollaboration != null)
            {
                var package = new ProcessFileEntity();
                package.XmlContent = xmlContent;
                package.Version = "1";
                
                package.ProcessGUID = XMLHelper.GetXmlAttribute(xmlNodeCollaboration, "sf:guid");
                var packageDB = GetByVersion(package.ProcessGUID, package.Version);
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
                var xmlNodeProcess = root.SelectSingleNode(XPDLDefinition.BPMN2_StrXmlPath_Process,
                    XPDLHelper.GetSlickflowXmlNamespaceManager(xmlDoc));
                if (xmlNodeProcess != null)
                {
                    var process = ProcessModelHelper.ConvertFromXmlNodeProcess(xmlNodeProcess);
                    process.XmlContent = xmlContent;
                    process.Version = "1";
                    var processDB = GetByVersion(process.ProcessGUID, process.Version, false);
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
        /// 删除泳道流程记录
        /// </summary>
        /// <param name="conn">数据库链接</param>
        /// <param name="packageID">主流程ID</param>
        /// <param name="poolProcessIDs">泳道流程ID列表</param>
        /// <param name="trans">交易</param>
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
        /// 删除泳道流程记录
        /// </summary>
        /// <param name="conn">数据库链接</param>
        /// <param name="packageID">主流程ID</param>
        /// <param name="trans">交易</param>
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
