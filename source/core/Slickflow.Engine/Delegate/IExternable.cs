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
}
