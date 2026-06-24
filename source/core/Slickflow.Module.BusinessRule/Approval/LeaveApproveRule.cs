using NRules.Fluent.Dsl;
using Slickflow.Engine.Business.Entity;
using System.Globalization;

namespace Slickflow.Module.BusinessRule.Approval
{
    public class LeaveApproveRule : Rule
    {
        public override void Define()
        {
            RuleInputFact input = null!;
            RuleOutputFact output = null!;

            When()
                .Match(() => input)
                .Match(() => output);

            Then()
                .Do(_ => ApplyApproval(input, output));
        }

        private static void ApplyApproval(RuleInputFact input, RuleOutputFact output)
        {
            var days = 0;
            var leaveType = string.Empty;

            if (input.Vars.TryGetValue("LeaveDays", out var d) && d != null)
            {
                var rawDays = Convert.ToString(d, CultureInfo.InvariantCulture);
                if (!string.IsNullOrWhiteSpace(rawDays)
                    && int.TryParse(rawDays, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsedDays))
                {
                    days = parsedDays;
                }
            }

            if (input.Vars.TryGetValue("LeaveType", out var t) && t != null)
                leaveType = t.ToString() ?? string.Empty;

            if (days > 7 || leaveType.Equals("Sick", StringComparison.OrdinalIgnoreCase))
                output.Set("ApprovalLevel", "HR");
            else if (days > 3)
                output.Set("ApprovalLevel", "Manager");
            else
                output.Set("ApprovalLevel", "Leader");
        }

    }
}

