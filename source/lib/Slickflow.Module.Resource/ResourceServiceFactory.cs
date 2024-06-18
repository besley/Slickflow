
namespace Slickflow.Module.Resource
{
    /// <summary>
    /// 资源服务的创建类
    /// </summary>
    public class ResourceServiceFactory
    {
        /// <summary>
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
