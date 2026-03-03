using Slickflow.Engine.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.External
{
    public abstract class ExternalServiceBase : IExecutable
    {
        #region Property and abstract method
        public IDictionary<string, string> DynamicVariables { get; set; }
        protected IEventService EventService { get; set; }
        public abstract void Execute();
        #endregion

        /// <summary>
        /// 设置委托服务
        /// </summary>
        /// <param name="eventService">委托服务</param>
        public void Executable(IEventService eventService)
        {
            EventService = eventService;
            Execute();
        }

    }
}
