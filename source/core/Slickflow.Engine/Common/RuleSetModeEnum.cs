using System;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// Values for <c>wf_rule_set.mode</c> (stored string matches <see cref="RuleSetModeHelper.PersistedRuleTypes"/> / <see cref="RuleSetModeHelper.PersistedBindingsJson"/>).
    /// </summary>
    public enum RuleSetModeEnum
    {
        /// <summary>NRules: JSON with a <c>ruleTypes</c> array.</summary>
        RuleTypes = 0,

        /// <summary>JSON DSL: <c>rules</c> / when / set in <c>rule_content</c>.</summary>
        BindingsJson = 1
    }

    /// <summary>Maps enum to/from persisted varchar in <c>wf_rule_set.mode</c>.</summary>
    public static class RuleSetModeHelper
    {
        public const string PersistedRuleTypes = "ruleTypes";
        public const string PersistedBindingsJson = "bindingsJson";

        /// <summary>Empty or whitespace is treated as <see cref="RuleSetModeEnum.RuleTypes"/>.</summary>
        public static bool TryParseFromPersisted(string persisted, out RuleSetModeEnum mode)
        {
            mode = RuleSetModeEnum.RuleTypes;
            if (string.IsNullOrWhiteSpace(persisted))
                return true;

            var s = persisted.Trim();
            if (string.Equals(s, PersistedBindingsJson, StringComparison.OrdinalIgnoreCase))
            {
                mode = RuleSetModeEnum.BindingsJson;
                return true;
            }

            if (string.Equals(s, PersistedRuleTypes, StringComparison.OrdinalIgnoreCase))
            {
                mode = RuleSetModeEnum.RuleTypes;
                return true;
            }

            return false;
        }

        public static string ToPersistedString(RuleSetModeEnum mode) =>
            mode == RuleSetModeEnum.BindingsJson ? PersistedBindingsJson : PersistedRuleTypes;
    }
}
