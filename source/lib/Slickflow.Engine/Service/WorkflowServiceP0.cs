﻿using System;
using System.Xml;
using System.Collections.Generic;
using Slickflow.Data;
using Slickflow.Engine.Common;
using Slickflow.Engine.Storage;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Utility;
using Slickflow.Engine.Xpdl.Common;

namespace Slickflow.Engine.Service
{
    /// <summary>
    /// 流程定义数据服务类
    /// </summary>
    public partial class WorkflowService: IWorkflowService
    {
        #region 流程定义数据
        /// <summary>
        /// 流程定义数据读取
        /// </summary>
        /// <param name="processGUID">流程定义GUID</param>
        /// <param name="version">版本号</param>
        /// <returns>流程</returns>
        public ProcessEntity GetProcessByVersion(string processGUID, string version = null)
        {
            var pm = new ProcessManager();
            var entity = pm.GetByVersion(processGUID, version);

            return entity;
        }

        /// <summary>
        /// 流程定义数据读取
        /// </summary>
        /// <param name="processName">流程名称</param>
        /// <param name="version">版本号</param>
        /// <returns>流程</returns>
        public ProcessEntity GetProcessByName(string processName, string version = null)
        {
            var pm = new ProcessManager();
            var entity = pm.GetByName(processName, version);

            return entity;
        }

        /// <summary>
        /// 流程定义数据读取
        /// </summary>
        /// <param name="processCode">流程代码</param>
        /// <param name="version">版本号</param>
        /// <returns>流程</returns>
        public ProcessEntity GetProcessByCode(string processCode, string version)
        {
            var pm = new ProcessManager();
            var entity = pm.GetByCode(processCode, version);

            return entity;
        }

        /// <summary>
        /// 获取当前版本的流程定义记录
        /// </summary>
        /// <param name="processGUID">流程GUID</param>
        /// <returns>流程</returns>
        public ProcessEntity GetProcessUsing(string processGUID)
        {
            var pm = new ProcessManager();
            var entity = pm.GetVersionUsing(processGUID);
            return entity;
        }

        /// <summary>
        /// 获取流程定义记录
        /// </summary>
        /// <param name="processID">流程主键ID</param>
        /// <returns>流程</returns>
        public ProcessEntity GetProcessByID(int processID)
        {
            var pm = new ProcessManager();
            var entity = pm.GetByID(processID);
            return entity;
        }

        /// <summary>
        /// 获取流程定义文件
        /// </summary>
        /// <param name="id">流程ID</param>
        /// <returns></returns>
        public ProcessFileEntity GetProcessFileByID(int id)
        {
            var pm = new ProcessManager();
            var entity = pm.GetProcessFileByID(id, XPDLStorageFactory.CreateXPDLStorage());

            return entity;
        }

        /// <summary>
        /// 获取流程定义数据
        /// </summary>
        /// <returns></returns>
        public IList<ProcessEntity> GetProcessList()
        {
            var pm = new ProcessManager();
            var list = pm.GetAll();

            return list;
        }

        /// <summary>
        /// 获取流程定义数据（只包括基本属性）
        /// </summary>
        /// <returns></returns>
        public IList<ProcessEntity> GetProcessListSimple()
        {
            var pm = new ProcessManager();
            var list = pm.GetListSimple();
            return list;
        }


        /// <summary>
        /// 流程定义的XML文件获取
        /// </summary>
        /// <param name="processGUID">流程GUID</param>
        /// <param name="version">版本</param>
        /// <returns>流程文件</returns>
        public ProcessFileEntity GetProcessFile(string processGUID, string version)
        {
            var pm = new ProcessManager();
            var entity = pm.GetProcessFile(processGUID, version, XPDLStorageFactory.CreateXPDLStorage());

            return entity;
        }

        /// <summary>
        /// 保存流程定义的xml文件
        /// </summary>
        /// <param name="entity">流程文件实体</param>
        public void SaveProcessFile(ProcessFileEntity entity)
        {
            var pm = new ProcessManager();
            pm.SaveProcessFile(entity);
        }

        /// <summary>
        /// 创建流程定义记录
        /// </summary>
        /// <param name="entity">流程定义实体</param>
        /// <returns>新ID</returns>
        public int InsertProcess(ProcessEntity entity)
        {
            var pm = new ProcessManager();
            var processID = pm.Insert(entity);

            return processID;
        }

        /// <summary>
        /// 创建流程定义记录
        /// </summary>
        /// <param name="entity">流程定义实体</param>
        /// <returns>新ID</returns>
        public int CreateProcess(ProcessEntity entity)
        {
            var pm = new ProcessManager();
            var processID = pm.CreateProcess(entity);

            return processID;
        }

        /// <summary>
        /// 创建流程定义记录新版本
        /// </summary>
        /// <param name="entity">流程</param>
        public int CreateProcessVersion(ProcessEntity entity)
        {
            int newProcessID = 0;
            var pm = new ProcessManager();
            var entityDB = pm.GetByID(entity.ID);
            if (entity != null)
            {
                newProcessID = pm.Upgrade(entityDB.ProcessGUID, entityDB.Version, entity.Version);
            }
            return newProcessID;
        }
        /// <summary>
        /// 更新流程定义记录
        /// </summary>
        /// <param name="entity">流程</param>
        public void UpdateProcess(ProcessEntity entity)
        {
            var processManager = new ProcessManager();
            processManager.Update(entity);
        }

        /// <summary>
        /// 更新流程使用状态
        /// </summary>
        /// <param name="processGUID">流程GUID</param>
        /// <param name="version">版本</param>
        /// <param name="usingState">使用状态</param>
        public void UpdateProcessUsingState(string processGUID,
            string version,
            byte usingState)
        {
            var processManager = new ProcessManager();
            processManager.UpdateUsingState(processGUID, version, usingState);
        }

        /// <summary>
        /// 升级流程记录
        /// </summary>
        /// <param name="processGUID">流程GUID</param>
        /// <param name="version">流程版本</param>
        /// <param name="newVersion">新版本编号</param>
        public void UpgradeProcess(string processGUID, string version, string newVersion)
        {
            var processManager = new ProcessManager();
            processManager.Upgrade(processGUID, version, newVersion);
        }

        /// <summary>
        /// 删除流程定义记录
        /// </summary>
        /// <param name="processGUID">流程GUID</param>
        /// <param name="version">版本</param>
        public void DeleteProcess(string processGUID, string version)
        {
            var pm = new ProcessManager();
            pm.DeleteProcess(processGUID, version);
        }

        /// <summary>
        /// 删除流程定义记录
        /// </summary>
        /// <param name="processGUID">流程GUID</param>
        public void DeleteProcess(string processGUID)
        {
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();
                var processManager = new ProcessManager();
                processManager.Delete(session.Connection, processGUID, session.Transaction);
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
        /// 删除流程实例包括其关联数据
        /// </summary>
        /// <param name="processGUID">流程GUID</param>
        /// <param name="version">版本</param>
        /// <returns>是否删除</returns>
        public bool DeleteInstanceInt(string processGUID, string version)
        {
            var pim = new ProcessInstanceManager();
            return pim.Delete(processGUID, version);
        }

        /// <summary>
        /// 导入流程XML文件，并生成新流程
        /// </summary>
        /// <param name="xmlContent">流程XML文档</param>
        /// <returns>新流程ID</returns>
        public void ImportProcess(string xmlContent)
        {
            var pm = new ProcessManager();
            pm.ImportProcess(xmlContent);
        }

        /// <summary>
        /// 流程校验
        /// </summary>
        /// <param name="entity">流程校验实体</param>
        /// <returns>流程校验结果</returns>
        public ProcessValidateResult ValidateProcess(ProcessEntity entity)
        {
            var result = ProcessValidator.Validate(entity);
            return result;
        }

        /// <summary>
        /// 重置缓存中的流程定义信息
        /// </summary>
        /// <param name="processGUID">流程Guid编号</param>
        /// <param name="version">流程版本</param>
        public void ResetCache(string processGUID, string version = null)
        {
            ////获取流程信息
            //var procesEntity = GetProcessByVersion(processGUID, version);
            //var process = ProcessModelConvertor.ConvertProcessModelFromXML(procesEntity);
            //if (XPDLMemoryCachedHelper.GetXpdlCache(procesEntity.ProcessGUID, procesEntity.Version) == null)
            //{
            //    XPDLMemoryCachedHelper.SetXpdlCache(procesEntity.ProcessGUID, procesEntity.Version, process);
            //}
            //else
            //{
            //    XPDLMemoryCachedHelper.TryUpdate(procesEntity.ProcessGUID, procesEntity.Version, process);
            //}
            throw new NotImplementedException();
        }

        public void SetProcessTimerType(string processGUID, string version)
        {
            var pm = new ProcessManager();
            
        }

        #endregion
    }
}
