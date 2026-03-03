using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Slickflow.Data;
using Slickflow.Engine.Common;

namespace Slickflow.Engine.Event
{
    /// <summary>
    /// Event Service Interface
    /// 事件服务接口
    /// </summary>
    public interface IEventService
    {
        int GetProcessInstanceId();
        IDbSession GetSession();
        string GetVariable(ProcessVariableScopeEnum variableType, string name);
        string GetVariableByScopePriority(string name);
        void SaveVariable(ProcessVariableScopeEnum variableType, string name, string value);
        string GetCondition(string name);
        void SetCondition(string name, string value);
        T GetInstance<T>(int id) where T : class;
    }

    /// <summary>
    /// Event Subscription List
    /// 事件订阅列表
    /// </summary>
    public class EventSubscriptionList
        : List<KeyValuePair<EventFireTypeEnum, Func<EventContext, IEventService, Boolean>>>
    {

    }
}
