using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Slickflow.Data;
using Slickflow.Engine.Common;

namespace Slickflow.Engine.Delegate
{
    /// <summary>
    /// Delegate Service Interface
    /// </summary>
    public interface IDelegateService
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
    /// Delegate Event List
    /// </summary>
    public class DelegateEventList
        : List<KeyValuePair<EventFireTypeEnum, Func<DelegateContext, IDelegateService, Boolean>>>
    {

    }
}
