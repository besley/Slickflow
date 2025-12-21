
namespace Slickflow.Module.Form
{
    /// <summary>
    /// Form service factory
    /// 表单服务的创建类
    /// </summary>
    public class FormServiceFactory
    {
        /// <summary>
        /// Create method
        /// 创建方法
        /// </summary>
        /// <returns></returns>
        public static IFormService Create()
        {
            var formService = new FormService();
            return formService;
        }
    }
}
