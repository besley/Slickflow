using System;
using System.Collections.Generic;
using System.Linq;
using Slickflow.Data;
using Slickflow.Engine.Business.Entity;

namespace Slickflow.Engine.Business.Manager
{
    /// <summary>
    /// Rule set manager (wf_rule_set)
    /// </summary>
    public class RuleSetManager : ManagerBase
    {
        public IList<RuleSetEntity> GetAll()
        {
            var sql = @"SELECT id, rule_set_code, rule_set_name, description, rule_content, mode, is_enabled,
                        created_datetime, updated_datetime
                        FROM wf_rule_set
                        ORDER BY rule_set_code, id";
            var list = Repository.Query<RuleSetEntity>(sql, null);
            return list?.ToList() ?? new List<RuleSetEntity>();
        }

        public RuleSetEntity GetByCode(string ruleSetCode)
        {
            if (string.IsNullOrEmpty(ruleSetCode)) return null;
            var sql = @"SELECT id, rule_set_code, rule_set_name, description, rule_content, mode, is_enabled,
                        created_datetime, updated_datetime
                        FROM wf_rule_set
                        WHERE rule_set_code = @RuleSetCode";
            return Repository.GetFirst<RuleSetEntity>(sql, new { RuleSetCode = ruleSetCode });
        }

        public int Insert(RuleSetEntity entity)
        {
            entity.CreatedDateTime = DateTime.UtcNow;
            if (entity.IsEnabled == 0) { /* keep */ } else if (entity.IsEnabled != 1) entity.IsEnabled = 1;
            var newId = Repository.Insert(entity);
            entity.Id = newId;
            return newId;
        }

        public void Update(RuleSetEntity entity)
        {
            entity.UpdatedDateTime = DateTime.UtcNow;
            Repository.Update(entity);
        }

        public void DeleteByCode(string ruleSetCode)
        {
            if (string.IsNullOrEmpty(ruleSetCode)) return;
            using (var session = SessionFactory.CreateSession())
            {
                var sql = "DELETE FROM wf_rule_set WHERE rule_set_code = @RuleSetCode";
                Repository.Execute(session.Connection, sql, new { RuleSetCode = ruleSetCode }, session.Transaction);
            }
        }
    }
}

