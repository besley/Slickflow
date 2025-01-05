using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Delegate
{
    /// <summary>
    /// Externally executable interface
    /// 可外部执行接口
    /// </summary>
    public interface IExternable
    {
        void Executable(IDelegateService delegateService);
    }

    /// <summary>
    /// External Service Base
    /// 外部服务基类
    /// </summary>
    public abstract class ExternalServiceBase : IExternable
    {
        #region Property and Constructor
        public IDictionary<string, string> DynamicVariables { get; set; }
        protected IDelegateService DelegateService { get; set; }
        public abstract void Execute();
        #endregion

        /// <summary>
        /// Set Delegate Service
        /// 设置委托服务
        /// </summary>
        /// <param name="delegateService"></param>
        public void Executable(IDelegateService delegateService)
        {
            DelegateService = delegateService;
            Execute();
        }
    }
}
