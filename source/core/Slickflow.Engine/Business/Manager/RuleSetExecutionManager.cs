using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json.Linq;
using NRules;
using NRules.Fluent;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Common;
using Slickflow.Engine.Xpdl;

namespace Slickflow.Engine.Business.Manager
{
    /// <summary>
    /// Executes <see cref="RuleSetEntity"/> by <see cref="RuleSetModeEnum"/>.
    /// </summary>
    public class RuleSetExecutionManager
    {
        public RuleExecutionResultEntity Execute(RuleSetEntity ruleSet, IDictionary<string, object> variables)
        {
            if (ruleSet == null || string.IsNullOrWhiteSpace(ruleSet.RuleSetCode))
            {
                return new RuleExecutionResultEntity
                {
                    IsSuccess = false,
                    Message = "Rule set not found",
                    OutputVariables = new Dictionary<string, object>()
                };
            }

            if (!RuleSetModeHelper.TryParseFromPersisted(ruleSet.Mode, out var modeEnum))
            {
                return new RuleExecutionResultEntity
                {
                    IsSuccess = false,
                    Message = "wf_rule_set.mode must be \"" + RuleSetModeHelper.PersistedRuleTypes + "\" or \"" +
                              RuleSetModeHelper.PersistedBindingsJson + "\" (got: " + ruleSet.Mode + ")",
                    OutputVariables = new Dictionary<string, object>()
                };
            }

            switch (modeEnum)
            {
                case RuleSetModeEnum.BindingsJson:
                    return ExecuteJsonRuleContent(ruleSet, variables);
                default:
                    return ExecuteRuleTypesNRules(ruleSet, variables);
            }
        }

        /// <summary>Validate <c>rule_content</c> shape for the given mode before save.</summary>
        public bool ValidateRuleContent(string mode, string ruleContent, out string message)
        {
            message = null;
            if (!RuleSetModeHelper.TryParseFromPersisted(mode, out var modeEnum))
            {
                message = "mode must be \"" + RuleSetModeHelper.PersistedRuleTypes + "\" or \"" +
                          RuleSetModeHelper.PersistedBindingsJson + "\"";
                return false;
            }

            try
            {
                if (modeEnum == RuleSetModeEnum.BindingsJson)
                {
                    if (string.IsNullOrWhiteSpace(ruleContent))
                    {
                        message = "rule_content is required for bindingsJson mode";
                        return false;
                    }
                    var token = JToken.Parse(ruleContent);
                    var rules = token["rules"] as JArray;
                    if (rules == null || rules.Count == 0)
                    {
                        message = "bindingsJson mode requires rule_content with a non-empty \"rules\" array";
                        return false;
                    }
                    return true;
                }

                var types = ParseRuleTypes(ruleContent);
                if (types.Count == 0)
                {
                    message = "ruleTypes mode requires rule_content with a non-empty \"ruleTypes\" array";
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
        }

        private RuleExecutionResultEntity ExecuteRuleTypesNRules(RuleSetEntity ruleSet, IDictionary<string, object> variables)
        {
            var ruleTypes = ParseRuleTypes(ruleSet.RuleContent);
            if (ruleTypes.Count == 0)
            {
                return new RuleExecutionResultEntity
                {
                    IsSuccess = false,
                    Message = "No ruleTypes configured in rule_content",
                    OutputVariables = new Dictionary<string, object>()
                };
            }

            var loadedTypes = ResolveRuleTypes(ruleTypes);
            if (loadedTypes.Count == 0)
            {
                return new RuleExecutionResultEntity
                {
                    IsSuccess = false,
                    Message = "No rule types could be loaded",
                    OutputVariables = new Dictionary<string, object>()
                };
            }

            var repository = new RuleRepository();
            repository.Load(x => x.From(loadedTypes.ToArray()));
            var factory = repository.Compile();
            var session = factory.CreateSession();

            var inputFact = new RuleInputFact { Vars = variables ?? new Dictionary<string, object>() };
            var outputFact = new RuleOutputFact();

            session.Insert(inputFact);
            session.Insert(outputFact);
            session.Fire();

            return new RuleExecutionResultEntity
            {
                IsSuccess = true,
                Message = "OK",
                OutputVariables = outputFact.Vars ?? new Dictionary<string, object>()
            };
        }

        /// <summary>
        /// Evaluate JSON DSL in <c>rule_content</c>. Example:
        /// { "rules": [ { "when": "Amount &gt;= 1000", "set": { "ApprovalLevel": "2" } } ] }
        /// First rule whose <c>when</c> evaluates true (or missing/empty when) applies its <c>set</c>/<c>then</c>/<c>outputs</c>.
        /// </summary>
        private RuleExecutionResultEntity ExecuteJsonRuleContent(RuleSetEntity ruleSet, IDictionary<string, object> variables)
        {
            var outputs = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            if (string.IsNullOrWhiteSpace(ruleSet.RuleContent))
            {
                return new RuleExecutionResultEntity
                {
                    IsSuccess = false,
                    Message = "rule_content is empty",
                    OutputVariables = outputs
                };
            }

            JToken root;
            try
            {
                root = JToken.Parse(ruleSet.RuleContent);
            }
            catch (Exception ex)
            {
                return new RuleExecutionResultEntity
                {
                    IsSuccess = false,
                    Message = "Invalid rule_content JSON: " + ex.Message,
                    OutputVariables = outputs
                };
            }

            var stringDict = ToStringDictionary(variables);
            var rules = root["rules"] as JArray;
            if (rules == null || rules.Count == 0)
            {
                return new RuleExecutionResultEntity
                {
                    IsSuccess = false,
                    Message = "bindingsJson mode requires a \"rules\" array in rule_content",
                    OutputVariables = outputs
                };
            }

            var matched = false;
            foreach (var rule in rules)
            {
                var when = rule["when"]?.ToString();
                if (!string.IsNullOrWhiteSpace(when))
                {
                    if (!EvaluateWhenExpression(when, stringDict))
                        continue;
                }

                MergeRuleOutputs(rule, outputs);
                matched = true;
                break;
            }

            if (!matched && root["default"] is JObject defObj)
                MergeJObjectIntoOutputs(defObj, outputs);

            return new RuleExecutionResultEntity
            {
                IsSuccess = true,
                Message = "OK",
                OutputVariables = outputs
            };
        }

        private static void MergeRuleOutputs(JToken rule, IDictionary<string, object> outputs)
        {
            if (rule["set"] is JObject j1) MergeJObjectIntoOutputs(j1, outputs);
            if (rule["then"] is JObject j2) MergeJObjectIntoOutputs(j2, outputs);
            if (rule["outputs"] is JObject j3) MergeJObjectIntoOutputs(j3, outputs);
        }

        private static void MergeJObjectIntoOutputs(JObject jo, IDictionary<string, object> outputs)
        {
            foreach (var p in jo.Properties())
                outputs[p.Name] = JTokenToOutputValue(p.Value);
        }

        private static object JTokenToOutputValue(JToken t)
        {
            if (t == null || t.Type == JTokenType.Null)
                return string.Empty;
            switch (t.Type)
            {
                case JTokenType.Integer:
                    return t.Value<long>();
                case JTokenType.Float:
                    return t.Value<decimal>();
                case JTokenType.Boolean:
                    return t.Value<bool>();
                case JTokenType.String:
                    return t.Value<string>() ?? string.Empty;
                default:
                    return t.ToString();
            }
        }

        private static bool EvaluateWhenExpression(string when, IDictionary<string, string> stringDict)
        {
            try
            {
                var replaced = ExpressionParser.ReplaceParameterToValue(when, stringDict);
                return ExpressionParser.Parse(replaced);
            }
            catch
            {
                return false;
            }
        }

        private static IDictionary<string, string> ToStringDictionary(IDictionary<string, object> variables)
        {
            var d = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            if (variables == null) return d;
            foreach (var kv in variables)
                d[kv.Key] = kv.Value == null ? string.Empty : Convert.ToString(kv.Value, CultureInfo.InvariantCulture);
            return d;
        }

        private IList<string> ParseRuleTypes(string ruleContent)
        {
            if (string.IsNullOrWhiteSpace(ruleContent)) return new List<string>();
            try
            {
                var token = JToken.Parse(ruleContent);
                var arr = token["ruleTypes"] as JArray;
                if (arr == null) return new List<string>();
                return arr.Select(x => x?.ToString())
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .ToList();
            }
            catch
            {
                return new List<string>();
            }
        }

        private IList<Type> ResolveRuleTypes(IList<string> ruleTypeNames)
        {
            var list = new List<Type>();
            foreach (var typeName in ruleTypeNames)
            {
                var t = Type.GetType(typeName, false);
                if (t == null)
                {
                    t = AppDomain.CurrentDomain.GetAssemblies()
                        .Select(a => a.GetType(typeName, false))
                        .FirstOrDefault(x => x != null);
                }
                if (t != null)
                {
                    list.Add(t);
                }
            }
            return list;
        }
    }
}
