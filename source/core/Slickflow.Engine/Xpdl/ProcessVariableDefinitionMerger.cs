using System.Collections.Generic;
using System.Linq;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Engine.Xpdl.Node;

namespace Slickflow.Engine.Xpdl
{
    /// <summary>
    /// Injects activity variable definitions from <c>wf_variable</c> after BPMN XML is parsed.
    /// Node variable metadata is no longer read from <c>sf:variables</c> in XML.
    /// </summary>
    public static class ProcessVariableDefinitionMerger
    {
        public static void ApplyFromDatabase(Process process, string processId, string version)
        {
            if (process == null || string.IsNullOrEmpty(processId))
                return;

            if (string.IsNullOrEmpty(version))
                version = "1";

            ApplyToActivities(process.ActivityList, processId, version);
        }

        private static void ApplyToActivities(IList<Activity> activities, string processId, string version)
        {
            if (activities == null)
                return;

            var vm = new VariableManager();

            foreach (var activity in activities)
            {
                var rows = vm.GetList(processId, version, activity.ActivityId);
                if (rows != null && rows.Count > 0)
                {
                    activity.VariableList = rows
                        .Select(VariableDefinitionMapper.ToVariableDetail)
                        .Where(v => v != null)
                        .ToList();
                }

                if (activity.Node is SubProcessNode spn && spn.SubProcessNested != null)
                {
                    var subProcessId = !string.IsNullOrEmpty(spn.SubProcessNested.ProcessId)
                        ? spn.SubProcessNested.ProcessId
                        : processId;

                    ApplyFromDatabase(spn.SubProcessNested, subProcessId, version);
                }
            }
        }
    }
}
