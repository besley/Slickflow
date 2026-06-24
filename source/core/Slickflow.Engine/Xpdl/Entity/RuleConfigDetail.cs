using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Common;

namespace Slickflow.Engine.Xpdl.Entity
{
    /// <summary>
    /// Rule task config: <see cref="RuleSetCode"/> from BPMN; rule body and mode from <see cref="MergeFrom"/> / <c>wf_rule_set</c>.
    /// Variable inputs for execution come from process/activity scope at runtime, not from BPMN.
    /// </summary>
    public class RuleConfigDetail
    {
        /// <summary>Bound rule set code from BPMN <c>sf:ruleConfig/@ruleSetCode</c>.</summary>
        public string RuleSetCode { get; set; }

        /// <summary><c>wf_rule_set.id</c> when resolved.</summary>
        public int? RuleSetId { get; set; }

        /// <summary><c>wf_rule_set.rule_set_name</c></summary>
        public string RuleSetName { get; set; }

        /// <summary><c>wf_rule_set.description</c></summary>
        public string Description { get; set; }

        /// <summary><c>wf_rule_set.rule_content</c></summary>
        public string RuleContent { get; set; }

        /// <summary><c>wf_rule_set.mode</c> (see <see cref="RuleSetModeEnum"/>).</summary>
        public string Mode { get; set; }

        /// <summary><c>wf_rule_set.is_enabled</c></summary>
        public short? IsEnabled { get; set; }

        /// <summary>True when <see cref="RuleSetCode"/> matched a row in <c>wf_rule_set</c>.</summary>
        public bool RuleSetResolved { get; set; }

        /// <summary>
        /// Copies fields from <paramref name="entity"/>; sets <see cref="RuleSetCode"/> to the canonical code from DB.
        /// </summary>
        public void MergeFrom(RuleSetEntity entity)
        {
            if (entity == null) return;
            RuleSetResolved = true;
            RuleSetId = entity.Id;
            RuleSetCode = entity.RuleSetCode;
            RuleSetName = entity.RuleSetName;
            Description = entity.Description;
            RuleContent = entity.RuleContent;
            Mode = entity.Mode;
            IsEnabled = entity.IsEnabled;
        }
    }
}

