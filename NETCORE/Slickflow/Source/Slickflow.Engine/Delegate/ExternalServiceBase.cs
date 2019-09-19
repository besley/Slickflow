using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Delegate
{
    /// <summary>
    /// 可外部执行接口
    /// </summary>
    public interface IExternable
    {
        void Executable(IDelegateService delegateService);
    }

    /// <summary>
    /// 外部服务基类
    /// </summary>
    public abstract class ExternalServiceBase : IExternable
    {
        #region 属性及抽象方法
        public IDictionary<string, string> DynamicVariables { get; set; }
        protected IDelegateService DelegateService { get; set; }
        public abstract void Execute();
        #endregion

        /// <summary>
        /// 设置委托服务
        /// </summary>
        /// <param name="delegateService">委托服务</param>
        public void Executable(IDelegateService delegateService)
        {
            DelegateService = delegateService;
            Execute();
        }
    }
}
