using System;
using Slickflow.Engine.Common;

namespace Slickflow.Engine.Business.Entity
{
    /// <summary>
    /// Rule set definition entity (wf_rule_set)
    /// </summary>
    [Table("wf_rule_set")]
    public class RuleSetEntity
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("rule_set_code")]
        public string RuleSetCode { get; set; }

        [Column("rule_set_name")]
        public string RuleSetName { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("rule_content")]
        public string RuleContent { get; set; }

        /// <summary>
        /// Persisted <c>wf_rule_set.mode</c> (<see cref="RuleSetModeHelper.PersistedRuleTypes"/> or <see cref="RuleSetModeHelper.PersistedBindingsJson"/>).
        /// </summary>
        [Column("mode")]
        public string Mode { get; set; }

        [Column("is_enabled")]
        public short IsEnabled { get; set; }

        [Column("created_datetime")]
        public DateTime CreatedDateTime { get; set; }

        [Column("updated_datetime")]
        public DateTime? UpdatedDateTime { get; set; }
    }
}

