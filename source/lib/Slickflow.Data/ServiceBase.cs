
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Data
{
    /// <summary>
    /// 服务基类
    /// </summary>
    public class ServiceBase
    {
        /// <summary>
        /// Repository 属性
        /// </summary>
        private IRepository _quickRepository;
        public IRepository QuickRepository
        {
            get
            {
                if (_quickRepository == null)
                {
                    _quickRepository = new Repository();
                }
                return _quickRepository;
            }
        }
    }
}
