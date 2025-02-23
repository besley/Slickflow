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
        int ProcessInstanceID { get; set; }
        string ActivityID { get; set; }
        int GetProcessInstanceID();
        IDbSession GetSession();
        string GetVariable(ProcessVariableTypeEnum variableType, string name);
        string GetVariableThroughly(string name);
        void SaveVariable(ProcessVariableTypeEnum variableType, string name, string value);
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
