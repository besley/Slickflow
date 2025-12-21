
namespace Slickflow.Module.Resource
{
    /// <summary>
    /// Resource Service Factory
    /// 资源服务的创建类
    /// </summary>
    public class ResourceServiceFactory
    {
        /// <summary>
        /// Create function
        /// 创建方法
        /// </summary>
        /// <returns></returns>
        public static IResourceService Create()
        {
            var resService = new ResourceService();
            return resService;
        }
    }
}
