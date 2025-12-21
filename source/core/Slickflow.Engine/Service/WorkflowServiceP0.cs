using MySqlX.XDevAPI;
using Slickflow.Data;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Common;
using Slickflow.Engine.Storage;
using Slickflow.Engine.Utility;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Xpdl.Common;
using System;
using System.Collections.Generic;
using System.Xml;

namespace Slickflow.Engine.Service
{
    /// <summary>
    /// Process Definition Data Service Class
    /// 流程定义数据服务类
    /// </summary>
    public partial class WorkflowService: IWorkflowService
    {
        #region Process Definition Data
        /// <summary>
        /// Get Process by Version
        /// 流程定义数据读取
        /// </summary>
        public ProcessEntity GetProcessByVersion(string processId, string version = null)
        {
            var pm = new ProcessManager();
            var entity = pm.GetByVersion(processId, version);

            return entity;
        }

        /// <summary>
        /// Get Process by Version
        /// 流程定义数据读取
        /// </summary>
        public ProcessEntity GetProcessByName(string processName, string version = null)
        {
            var pm = new ProcessManager();
            var entity = pm.GetByName(processName, version);

            return entity;
        }

        /// <summary>
        /// Get Process by Version
        /// 流程定义数据读取
        /// </summary>
        public ProcessEntity GetProcessByCode(string processCode, string version)
        {
            var pm = new ProcessManager();
            var entity = pm.GetByCode(processCode, version);

            return entity;
        }

        /// <summary>
        /// Retrieve the current using version of the process 
        /// 获取当前版本的流程定义记录
        /// </summary>
        public ProcessEntity GetProcessUsing(string processId)
        {
            var pm = new ProcessManager();
            var entity = pm.GetVersionUsing(processId);
            return entity;
        }

        /// <summary>
        /// Retrieve the process by id
        /// 获取流程定义记录
        /// </summary>
        public ProcessEntity GetProcessById(int processId)
        {
            var pm = new ProcessManager();
            var entity = pm.GetById(processId);
            return entity;
        }

        /// <summary>
        /// Retrieve the process file by id
        /// 获取流程定义文件
        /// </summary>
        public ProcessFileEntity GetProcessFileById(int id)
        {
            var pm = new ProcessManager();
            var entity = pm.GetProcessFileById(id, XPDLStorageFactory.CreateXPDLStorage());

            return entity;
        }

        /// <summary>
        /// Get process list
        /// 获取流程定义数据
        /// </summary>
        public IList<ProcessEntity> GetProcessList()
        {
            var pm = new ProcessManager();
            var list = pm.GetAll();

            return list;
        }

        /// <summary>
        /// Obtain process definition data (including only basic attributes)
        /// 获取流程定义数据（只包括基本属性）
        /// </summary>
        public IList<ProcessEntity> GetProcessListSimple()
        {
            var pm = new ProcessManager();
            var list = pm.GetListSimple();
            return list;
        }


        /// <summary>
        /// Get process file
        /// 流程定义的XML文件获取
        /// </summary>
        public ProcessFileEntity GetProcessFile(string processId, string version)
        {
            var pm = new ProcessManager();
            var entity = pm.GetProcessFile(processId, version, XPDLStorageFactory.CreateXPDLStorage());

            return entity;
        }

        /// <summary>
        /// Save process file
        /// 保存流程定义的xml文件
        /// </summary>
        public void SaveProcessFile(ProcessFileEntity entity)
        {
            var pm = new ProcessManager();
            pm.SaveProcessFile(entity);
        }

        /// <summary>
        /// Insert process
        /// 创建流程定义记录
        /// </summary>
        public int InsertProcess(ProcessEntity entity)
        {
            var pm = new ProcessManager();
            var processId = pm.Insert(entity);

            return processId;
        }

        /// <summary>
        /// Create process
        /// 创建流程定义记录
        /// </summary>
        public int CreateProcess(ProcessEntity entity)
        {
            var pm = new ProcessManager();
            var processId = pm.CreateProcess(entity);

            return processId;
        }

        /// <summary>
        /// Create process version
        /// 创建流程定义记录新版本
        /// </summary>
        public int CreateProcessVersion(ProcessEntity entity)
        {
            int newProcessId = 0;
            var pm = new ProcessManager();
            var entityDB = pm.GetById(entity.Id);
            if (entity != null)
            {
                newProcessId = pm.Upgrade(entityDB.ProcessId, entityDB.Version, entity.Version);
            }
            return newProcessId;
        }

        /// <summary>
        /// Create process by xml content
        /// 根据XML创建新流程
        /// </summary>
        public ProcessFileEntity CreateProcessByXML(string xmlContent)
        {
            var pm = new ProcessManager();
            var processFileEntity = pm.CreateProcessByXML(xmlContent);
            return processFileEntity;
        }

        /// <summary>
        /// Update process
        /// 更新流程定义记录
        /// </summary>
        public void UpdateProcess(ProcessEntity entity)
        {
            var processManager = new ProcessManager();
            processManager.Update(entity);
        }

        /// <summary>
        /// Update process state
        /// 更新流程使用状态
        /// </summary>
        public void UpdateProcessUsingState(string processId,
            string version,
            byte usingState)
        {
            var processManager = new ProcessManager();
            processManager.UpdateUsingState(processId, version, usingState);
        }

        /// <summary>
        /// Upgrade process
        /// 升级流程记录
        /// </summary>
        public void UpgradeProcess(string processId, string version, string newVersion)
        {
            var processManager = new ProcessManager();
            processManager.Upgrade(processId, version, newVersion);
        }

        /// <summary>
        /// Delete process
        /// 删除流程定义记录
        /// </summary>
        public void DeleteProcess(string processId, string version)
        {
            var pm = new ProcessManager();
            pm.DeleteProcess(processId, version);
        }

        /// <summary>
        /// Delete process
        /// 删除流程定义记录
        /// </summary>
        public void DeleteProcess(int id)
        {
            var pm = new ProcessManager();
            pm.Delete(id);
        }

        /// <summary>
        /// Delete instance internal
        /// 删除流程实例包括其关联数据
        /// </summary>
        public bool DeleteInstanceInt(string processId, string version)
        {
            var pim = new ProcessInstanceManager();
            return pim.Delete(processId, version);
        }

        /// <summary>
        /// Import process file and generate a new process
        /// 导入流程XML文件，并生成新流程
        /// </summary>
        public void ImportProcess(string xmlContent)
        {
            var pm = new ProcessManager();
            pm.ImportProcess(xmlContent);
        }

        /// <summary>
        /// Validate process
        /// 流程校验
        /// </summary>
        public ProcessValidateResult ValidateProcess(ProcessEntity entity)
        {
            var result = ProcessValidator.Validate(entity);
            return result;
        }

        /// <summary>
        /// Reset Cachae
        /// 重置缓存中的流程定义信息
        /// </summary>
        public void ResetCache(string processId, string version = null)
        {
            ////获取流程信息
            //var procesEntity = GetProcessByVersion(processId, version);
            //var process = ProcessModelConvertor.ConvertProcessModelFromXML(procesEntity);
            //if (XPDLMemoryCachedHelper.GetXpdlCache(procesEntity.ProcessId, procesEntity.Version) == null)
            //{
            //    XPDLMemoryCachedHelper.SetXpdlCache(procesEntity.ProcessId, procesEntity.Version, process);
            //}
            //else
            //{
            //    XPDLMemoryCachedHelper.TryUpdate(procesEntity.ProcessId, procesEntity.Version, process);
            //}
            throw new NotImplementedException();
        }
        #endregion
    }
}
