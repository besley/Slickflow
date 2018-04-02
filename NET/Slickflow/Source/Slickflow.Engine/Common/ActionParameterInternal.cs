using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// 调用外部API的参数封装类
    /// </summary>
    public class ActionParameterInternal
    {
        public object[] ConstructorParameters { get; set; }
        public object[] MethodParameters { get; set; }
    }
}
