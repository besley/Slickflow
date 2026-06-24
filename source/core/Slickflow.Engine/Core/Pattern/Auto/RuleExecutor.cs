using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Slickflow.Data;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Common;
using Slickflow.Engine.Event;
using Slickflow.Engine.Executor;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.WebUtility;

namespace Slickflow.Engine.Core.Pattern.Auto
{
    internal static class RuleExecutor
    {
        internal static void ExecuteRule(RuleConfigDetail ruleConfig,
            Activity currentActivity,
            ActivityInstanceEntity currentActivityInstance,
            IEventService eventService)
        {
            if (ruleConfig == null || string.IsNullOrWhiteSpace(ruleConfig.RuleSetCode))
                return;

            var ruleSet = new RuleSetManager().GetByCode(ruleConfig.RuleSetCode);
            var inputs = BuildRuleInputs(eventService, currentActivityInstance, currentActivity);
            var result = new RuleSetExecutionManager().Execute(ruleSet, inputs);

            if (!result.IsSuccess)
                throw new WorkflowException($"RuleTask execution failed: {result.Message}");

            if (result.OutputVariables == null || result.OutputVariables.Count == 0)
                return;

            var processInstanceId = eventService.GetProcessInstanceId();
            var session = eventService.GetSession();
            var processInstance = new ProcessInstanceManager().GetById(session.Connection, processInstanceId, session.Transaction);
            var pvm = new ProcessVariableManager();

            foreach (var item in result.OutputVariables)
            {
                if (string.IsNullOrWhiteSpace(item.Key)) continue;
                var workflowVarName = item.Key.Trim();
                var value = item.Value == null ? string.Empty : Convert.ToString(item.Value, CultureInfo.InvariantCulture);

                var processVariable = new ProcessVariableEntity
                {
                    VariableScope = ProcessVariableScopeEnum.Activity.ToString(),
                    AppInstanceId = processInstance?.AppInstanceId,
                    ProcessId = processInstance?.ProcessId,
                    ProcessInstanceId = processInstanceId,
                    ActivityInstanceId = currentActivityInstance.Id,
                    ActivityId = currentActivity.ActivityId,
                    ActivityName = currentActivity.ActivityName,
                    Name = workflowVarName,
                    Value = value,
                    MediaType = MultiMediaTypeEnum.Text.ToString(),
                    UpdatedDateTime = DateTime.UtcNow
                };
                pvm.SaveVariable(session.Connection, processVariable, session.Transaction);
            }
        }

        /// <summary>
        /// Rule inputs: process-scoped vars; then <c>wf_variable</c> for this activity (Input rows —
        /// referenced inputs load from prior node <c>wf_process_variable</c> via <c>source_ref</c>/<c>source_variable_name</c>);
        /// if no <c>wf_variable</c> rows, merge all activity-scoped rows for this instance (legacy).
        /// Auto-run uses <see cref="AutoRunEventServiceAdapter.VariablesSnapshot"/>.
        /// </summary>
        private static IDictionary<string, object> BuildRuleInputs(IEventService eventService, ActivityInstanceEntity activityInstance, Activity currentActivity)
        {
            var dict = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

            if (eventService is AutoRunEventServiceAdapter autoRun)
            {
                var bag = autoRun.VariablesSnapshot;
                if (bag == null) return dict;
                foreach (var kv in bag)
                {
                    if (string.IsNullOrWhiteSpace(kv.Key)) continue;
                    dict[kv.Key.Trim()] = ParseTypedValue(kv.Value ?? string.Empty);
                }
                return dict;
            }

            var session = eventService.GetSession();
            if (session == null || activityInstance == null || currentActivity == null)
                return dict;

            var pvm = new ProcessVariableManager();
            var conn = session.Connection;
            var trans = session.Transaction;
            var processInstanceId = eventService.GetProcessInstanceId();
            var activityInstanceId = activityInstance.Id;
            var processScope = ProcessVariableScopeEnum.Process.ToString();
            var activityScope = ProcessVariableScopeEnum.Activity.ToString();

            var allForProcess = pvm.GetVariableList(processInstanceId);
            if (allForProcess != null)
            {
                foreach (var v in allForProcess)
                {
                    if (string.IsNullOrWhiteSpace(v?.Name)) continue;
                    if (!string.Equals(v.VariableScope, processScope, StringComparison.OrdinalIgnoreCase))
                        continue;
                    dict[v.Name.Trim()] = ParseTypedValue(v.Value ?? string.Empty);
                }
            }

            var processInstance = new ProcessInstanceManager().GetById(conn, processInstanceId, trans);
            if (processInstance != null
                && !string.IsNullOrWhiteSpace(processInstance.ProcessId)
                && !string.IsNullOrWhiteSpace(processInstance.Version)
                && !string.IsNullOrWhiteSpace(currentActivity.ActivityId))
            {
                var vm = new VariableManager();
                var defs = vm.GetList(processInstance.ProcessId, processInstance.Version, currentActivity.ActivityId);
                var inputDefs = defs?.Where(IsWfVariableInput).ToList() ?? new List<VariableEntity>();
                if (inputDefs.Count > 0)
                {
                    var aim = new ActivityInstanceManager();
                    foreach (var def in inputDefs)
                    {
                        var key = (def.Name ?? string.Empty).Trim();
                        if (string.IsNullOrEmpty(key)) continue;

                        string raw = null;
                        if (def.IsReferenced == 1
                            && !string.IsNullOrWhiteSpace(def.SourceRef)
                            && !string.IsNullOrWhiteSpace(def.SourceVariableName))
                        {
                            var sourceInst = ResolveSourceActivityInstance(activityInstance, def.SourceRef.Trim(), session, aim);
                            if (sourceInst != null)
                            {
                                var srcEntity = pvm.GetVariableByActivity(conn, processInstanceId, sourceInst.Id,
                                    def.SourceVariableName.Trim(), trans);
                                raw = srcEntity?.Value;
                            }
                        }
                        else
                        {
                            var local = pvm.GetVariableByActivity(conn, processInstanceId, activityInstanceId, key, trans);
                            raw = local?.Value;
                            if (raw == null && !string.IsNullOrEmpty(def.DefaultValue))
                                raw = def.DefaultValue;
                        }

                        // Keep declared input variables in the input bag even when source value is missing,
                        // so JSON DSL expressions (e.g. LeaveType == "Sick") can evaluate against empty string.
                        dict[key] = ParseTypedValue(raw ?? string.Empty);
                    }

                    return dict;
                }
            }

            var activityRows = pvm.GetVariableList(conn, processInstanceId, activityInstanceId, trans);
            if (activityRows != null)
            {
                foreach (var v in activityRows)
                {
                    if (string.IsNullOrWhiteSpace(v?.Name)) continue;
                    if (!string.Equals(v.VariableScope, activityScope, StringComparison.OrdinalIgnoreCase))
                        continue;
                    dict[v.Name.Trim()] = ParseTypedValue(v.Value ?? string.Empty);
                }
            }

            return dict;
        }

        private static bool IsWfVariableInput(VariableEntity e)
        {
            if (e == null) return false;
            if (string.IsNullOrWhiteSpace(e.Direction)) return true;
            return !string.Equals(e.Direction, "Output", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Prefer token-aware previous instance; else latest completed instance for <paramref name="sourceActivityId"/>.
        /// </summary>
        private static ActivityInstanceEntity ResolveSourceActivityInstance(
            ActivityInstanceEntity currentInstance,
            string sourceActivityId,
            IDbSession session,
            ActivityInstanceManager aim)
        {
            var prev = aim.GetPreviousActivityInstanceSimple(currentInstance, sourceActivityId, session);
            if (prev != null) return prev;

            var completed = aim.GetActivityInstanceListCompletedSimple(currentInstance.ProcessInstanceId, sourceActivityId, session);
            return completed != null && completed.Count > 0 ? completed[0] : null;
        }

        private static object ParseTypedValue(string raw)
        {
            if (bool.TryParse(raw, out var b)) return b;
            if (int.TryParse(raw, NumberStyles.Integer, CultureInfo.InvariantCulture, out var i)) return i;
            if (long.TryParse(raw, NumberStyles.Integer, CultureInfo.InvariantCulture, out var l)) return l;
            if (decimal.TryParse(raw, NumberStyles.Number, CultureInfo.InvariantCulture, out var d)) return d;
            if (double.TryParse(raw, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var f)) return f;
            return raw;
        }
    }
}

