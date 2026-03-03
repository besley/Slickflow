using Slickflow.Module.Resource;
using Slickflow.Module.Form;

namespace Slickflow.Engine.Service
{
    /// <summary>
    /// Workflow Service - Main Entry
    /// 工作流服务 - 主入口
    /// </summary>
    public partial class WorkflowService : IWorkflowService
    {
        #region Property and Constructor
        /// <summary>
        /// Role User Resource Service Interface
        /// 角色用户资源服务接口
        /// </summary>
        protected IResourceService ResourceService { get; private set; }

        /// <summary>
        /// Form Resource Service Interface
        /// 表单资源服务接口
        /// </summary>
        protected IFormService FormService { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public WorkflowService()
        {
            ResourceService = ResourceServiceFactory.Create();
            FormService = FormServiceFactory.Create();
        }
        #endregion
    }
}
