using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;

namespace Slickflow.Graph.Model
{
    /// <summary>
    /// Process Factory
    /// 流程工厂类
    /// </summary>
    public class ProcessFactory
    {
        /// <summary>
        /// Create process by template name
        /// 创建流程
        /// </summary>
        public static ProcessEntity CreateProcessByTemplate(string templateName)
        {
            var xmlContent = ProcessTemplateFactory.CreateProcessByTemplate(templateName, out ProcessFileEntity fileEntity);
            var entity = InsertProcessEntity(fileEntity, xmlContent);
            return entity;
        }

        /// <summary>
        /// Insert process entity
        /// 插入流程实体
        /// </summary>
        private static ProcessEntity InsertProcessEntity(ProcessFileEntity fileEntity,
            string xmlContent = null)
        {
            var entity = new ProcessEntity
            {
                ProcessId = string.IsNullOrEmpty(fileEntity.ProcessId) ? Guid.NewGuid().ToString() : fileEntity.ProcessId,
                ProcessName = fileEntity.ProcessName,
                ProcessCode = fileEntity.ProcessCode,
                Version = string.IsNullOrEmpty(fileEntity.Version) ? "1" : fileEntity.Version,
                Status = fileEntity.Status,
                XmlContent = xmlContent,
                Description = fileEntity.Description,
                CreatedDateTime = System.DateTime.UtcNow,
                UpdatedDateTime = System.DateTime.UtcNow
            };

            var pm = new ProcessManager();
            var processId = pm.Insert(entity);
            entity.Id = processId;

            return entity;
        }
    }
}
