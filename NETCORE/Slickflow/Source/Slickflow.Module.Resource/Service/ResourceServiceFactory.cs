using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Module.Resource.Service
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
        public static ResourceService Create()
        {
            var resService = new ResourceService();
            return resService;
        }
    }
}
