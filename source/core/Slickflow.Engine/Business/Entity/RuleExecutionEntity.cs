using System.Collections.Generic;

namespace Slickflow.Engine.Business.Entity
{
    public class RuleExecutionEntity
    {
        public string RuleSetCode { get; set; }
        public IDictionary<string, object> Variables { get; set; }
    }

    public class RuleExecutionResultEntity
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public IDictionary<string, object> OutputVariables { get; set; }
    }

    /// <summary>
    /// Fact passed to NRules.
    /// </summary>
    public class RuleInputFact
    {
        public IDictionary<string, object> Vars { get; set; } = new Dictionary<string, object>();
    }

    /// <summary>
    /// Fact for storing rule outputs.
    /// </summary>
    public class RuleOutputFact
    {
        public IDictionary<string, object> Vars { get; set; } = new Dictionary<string, object>();

        public void Set(string name, object value)
        {
            Vars[name] = value;
        }
    }
}

