using System;
using System.Data;
using System.Xml;
using Slickflow.Data;
using Slickflow.Module.Localize;
using Slickflow.Engine.Common;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Xpdl.Node;
using Slickflow.Engine.Utility;

namespace Slickflow.Engine.Xpdl
{
    /// <summary>
    /// Process Model Factory Class
    /// 1.  Four types of process model creation (by process, by process instance, by task view, by subprocess node)
    /// 2.  Distinguish the creation mode of sub processes (handle the situation where sub processes are embedded into the main process)
    /// 流程模型工厂类
    /// 1. 流程模型创建的4种类型(按流程, 按流程实例, 按任务视图, 按子流程节点)
    /// 2. 区分出子流程的创建模式(子流程嵌入到主流程里面的情况处理)
    /// </summary>
    public class ProcessModelFactory
    {
        /// <summary>
        /// Create by Process
        /// </summary>
        public static IProcessModel CreateByProcess(ProcessEntity entity)
        {
            IProcessModel processModel = new ProcessModelBPMN(entity);
            return processModel;
        }

        /// <summary>
        /// Create by Process version
        /// </summary>
        public static IProcessModel CreateByProcess(string processGUID, string version)
        {
            using (var session = SessionFactory.CreateSession())
            {
                var processModel = CreateByProcess(session.Connection, processGUID, version, session.Transaction);
                return processModel;
            }
        }

        /// <summary>
        /// Create by Process version
        /// </summary>
        private static IProcessModel CreateByProcess(IDbConnection conn,
            string processGUID,
            string version,
            IDbTransaction trans)
        {
            if (string.IsNullOrEmpty(processGUID)
                || string.IsNullOrEmpty(version))
            {
                throw new WfXpdlException(LocalizeHelper.GetEngineMessage("processmodel.factory.processnullexception"));
            }

            var pm = new ProcessManager();
            var entity = pm.GetByVersion(conn, processGUID, version, false, trans);

            if (entity == null)
            {
                throw new WfXpdlException(LocalizeHelper.GetEngineMessage("processmodel.factory.processnullexception"));
            }

            IProcessModel processModel = new ProcessModelBPMN(entity);
            return processModel;
        }

        /// <summary>
        /// Create by Process instance
        /// </summary>
        public static IProcessModel CreateByProcessInstance(ProcessInstanceEntity processInstance)
        {
            using (var session = SessionFactory.CreateSession())
            {
                var processModel = CreateByProcessInstance(session.Connection, processInstance, session.Transaction);
                return processModel;
            }
        }

        /// <summary>
        /// Create by Process instance
        /// </summary>
        public static IProcessModel CreateByProcessInstance(IDbConnection conn,
            ProcessInstanceEntity processInstance,
            IDbTransaction trans)
        {
            IProcessModel processModel = null;
            if (processInstance.SubProcessType == null)
            {
                //Single process
                processModel = CreateByProcess(conn, processInstance.ProcessGUID, processInstance.Version, trans);
            }
            else if (processInstance.SubProcessType != null
                && !string.IsNullOrEmpty(processInstance.SubProcessGUID))
            {
                //Sub process
                processModel = CreateSubByProcessInstance(conn, processInstance, trans);
            }
            return processModel;
        }

        /// <summary>
        /// Create by task
        /// </summary>
        public static IProcessModel CreateByTask(TaskViewEntity taskView)
        {
            using (var session = SessionFactory.CreateSession())
            {
                var processModel = CreateByTask(session.Connection, taskView, session.Transaction);
                return processModel;
            }
        }

        /// <summary>
        /// Create by task
        /// </summary>
        public static IProcessModel CreateByTask(IDbConnection conn, TaskViewEntity taskView, IDbTransaction trans)
        {
            IProcessModel processModel = null;
            if (taskView.SubProcessType == null)
            {
                //Single process
                processModel = CreateByProcess(conn, taskView.ProcessGUID, taskView.Version, trans);
            }
            else if (taskView.SubProcessType != null
                && !string.IsNullOrEmpty(taskView.SubProcessGUID))
            {
                //Sub process
                processModel = CreateSubByTask(conn, taskView, trans);
            }
            return processModel;
        }

        /// <summary>
        /// Create by sub process node
        /// </summary>
        public static IProcessModel CreateSubByNode(IDbConnection conn,
            SubProcessNode subProcessNode,
            IDbTransaction trans)
        {
            ProcessEntity entity = null;
            if (subProcessNode != null)
            {
                if (subProcessNode.SubProcessNested == null)
                {
                    //Referenced from outer
                    var pm = new ProcessManager();
                    entity = pm.GetByID(subProcessNode.SubProcessID);
                }
                else
                {
                    //Nested sub process
                    entity = BuildProcessEntityFromXpdl(subProcessNode.SubProcessNested);
                }
            }

            if (entity == null)
            {
                throw new WfXpdlException(LocalizeHelper.GetEngineMessage("processmodel.factory.processnullexception"));
            }

            IProcessModel processModel = new ProcessModelBPMN(entity);
            return processModel;
        }

        /// <summary>
        /// Build process objects with nested subprocesses
        /// 构建内嵌子流程的流程对象
        /// </summary>
        private static ProcessEntity BuildProcessEntityFromXpdl(Xpdl.Entity.Process subProcess)
        {
            var processEntity = new ProcessEntity();
            processEntity.ProcessGUID = subProcess.ProcessGUID;
            processEntity.ProcessName = subProcess.Name;
            processEntity.ProcessCode = subProcess.Code;
            processEntity.Version = "1";
            processEntity.XmlContent = subProcess.XmlContent;
            return processEntity;
        }

        /// <summary>
        /// Create sub process by process instance
        /// </summary>
        private static IProcessModel CreateSubByProcessInstance(IDbConnection conn,
            ProcessInstanceEntity processInstance,
            IDbTransaction trans)
        {
            var subProcessType = SubProcessTypeEnum.None;
            if (processInstance.SubProcessType != null) subProcessType = EnumHelper.TryParseEnum<SubProcessTypeEnum>(
                processInstance.SubProcessType.ToString());
            var processModel = CreateSubInternal(conn, processInstance.ProcessGUID, processInstance.Version, subProcessType, processInstance.SubProcessID,
                processInstance.SubProcessGUID, trans);
            return processModel;
        }

        /// <summary>
        /// Create sub process by task
        /// </summary>
        private static IProcessModel CreateSubByTask(IDbConnection conn,
            TaskViewEntity taskView,
            IDbTransaction trans)
        {
            var subProcessType = SubProcessTypeEnum.None;
            if (taskView.SubProcessType != null) subProcessType = EnumHelper.TryParseEnum<SubProcessTypeEnum>(
                taskView.SubProcessType.ToString());
            var processModel = CreateSubInternal(conn, taskView.ProcessGUID, taskView.Version, subProcessType, taskView.SubProcessID,
                taskView.SubProcessGUID, trans);
            return processModel;
        }

        /// <summary>
        /// Create sub process internal
        /// </summary>
        private static IProcessModel CreateSubInternal(IDbConnection conn,
            string processGUID,
            string version,
            Nullable<SubProcessTypeEnum> subProcessType,
            int subProcessID,
            string subProcessGUID,
            IDbTransaction trans)
        {
            var pm = new ProcessManager();
            ProcessEntity processEntity = null;
            if (subProcessType.HasValue 
                && subProcessType.Value == SubProcessTypeEnum.Referenced)
            {
                processEntity = pm.GetByID(conn, subProcessID, trans);
            }
            else if (subProcessType.HasValue
                && subProcessType.Value == SubProcessTypeEnum.Nested)
            {
                processEntity = pm.GetByVersion(conn, processGUID, version, true, trans);
                //取出子流程节点对象
                //Retrieve sub process node objects
                var xmlContent = processEntity.XmlContent;
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlContent);

                //查找子流程节点
                //Find sub process nodes
                var strSubProcessPath = XPDLHelper.GetXmlPathOfProcess(true);
                XmlNode xmlSubProcessNode = xmlDoc.DocumentElement.SelectSingleNode(
                    string.Format("{0}[@sf:guid='" + subProcessGUID + "']", strSubProcessPath),
                    XPDLHelper.GetSlickflowXmlNamespaceManager(xmlDoc));

                processEntity.ProcessGUID = XMLHelper.GetXmlAttribute(xmlSubProcessNode, "guid");
                processEntity.ProcessName = XMLHelper.GetXmlAttribute(xmlSubProcessNode, "name");
                processEntity.ProcessCode = XMLHelper.GetXmlAttribute(xmlSubProcessNode, "code");
                processEntity.Version = "1";
                processEntity.XmlContent = xmlSubProcessNode.OuterXml;
            }
            IProcessModel processModel = new ProcessModelBPMN(processEntity);
            return processModel;
        }
    }
}
