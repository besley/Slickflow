using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Slickflow.Data;
using Slickflow.Engine.Business.Entity;

namespace Slickflow.Engine.Business.Manager
{
    /// <summary>
    /// 流程变量定义管理类（wf_variable 表）
    /// 对应 BPMN sf:variables 中 Variable 的维护
    /// </summary>
    public class VariableManager : ManagerBase
    {
        /// <summary>
        /// 根据流程与节点获取变量定义列表
        /// </summary>
        public IList<VariableEntity> GetList(string processId, string version, string activityId)
        {
            if (string.IsNullOrEmpty(processId) || string.IsNullOrEmpty(version) || string.IsNullOrEmpty(activityId))
                return new List<VariableEntity>();

            var sql = @"SELECT id, process_id, version, activity_id, name, type, direction, default_value,
                        is_required, is_referenced, source_ref, source_variable_name, sort_order, description,
                        created_datetime, updated_datetime
                        FROM wf_variable
                        WHERE process_id = @ProcessId AND version = @Version AND activity_id = @ActivityId
                        ORDER BY sort_order, id";
            var list = Repository.Query<VariableEntity>(sql, new { ProcessId = processId, Version = version, ActivityId = activityId });
            return list?.ToList() ?? new List<VariableEntity>();
        }

        /// <summary>
        /// 根据主键获取变量定义
        /// </summary>
        public VariableEntity GetById(int id)
        {
            return Repository.GetById<VariableEntity>(id);
        }

        /// <summary>
        /// 新增变量定义
        /// </summary>
        public int Insert(VariableEntity entity)
        {
            entity.CreatedDateTime = DateTime.UtcNow;
            var newId = Repository.Insert(entity);
            entity.Id = newId;
            return newId;
        }

        /// <summary>
        /// 新增变量定义（带事务）
        /// </summary>
        public int Insert(IDbConnection conn, VariableEntity entity, IDbTransaction trans)
        {
            entity.CreatedDateTime = DateTime.UtcNow;
            var newId = Repository.Insert(conn, entity, trans);
            entity.Id = newId;
            return newId;
        }

        /// <summary>
        /// 更新变量定义
        /// </summary>
        public void Update(VariableEntity entity)
        {
            entity.UpdatedDateTime = DateTime.UtcNow;
            Repository.Update(entity);
        }

        /// <summary>
        /// 更新变量定义（带事务）
        /// </summary>
        public void Update(IDbConnection conn, VariableEntity entity, IDbTransaction trans)
        {
            entity.UpdatedDateTime = DateTime.UtcNow;
            Repository.Update(conn, entity, trans);
        }

        /// <summary>
        /// 删除变量定义
        /// </summary>
        public void Delete(int id)
        {
            Repository.Delete<VariableEntity>(id);
        }

        /// <summary>
        /// 按流程+版本+节点删除该节点下全部变量定义（用于整表覆盖前先清空）
        /// </summary>
        public void DeleteByProcessActivity(string processId, string version, string activityId)
        {
            using (var session = SessionFactory.CreateSession())
            {
                DeleteByProcessActivity(session.Connection, processId, version, activityId, session.Transaction);
            }
        }

        /// <summary>
        /// 保存某节点变量定义列表：先按 process_id + version + activity_id 删除再批量插入
        /// </summary>
        public void SaveList(string processId, string version, string activityId, IList<VariableEntity> list)
        {
            using (var session = SessionFactory.CreateSession())
            {
                DeleteByProcessActivity(session.Connection, processId, version, activityId, session.Transaction);
                if (list != null)
                {
                    var order = 0;
                    foreach (var item in list)
                    {
                        item.ProcessId = processId;
                        item.Version = version;
                        item.ActivityId = activityId;
                        item.SortOrder = order++;
                        item.Id = 0;
                        Insert(session.Connection, item, session.Transaction);
                    }
                }
            }
        }

        private void DeleteByProcessActivity(IDbConnection conn, string processId, string version, string activityId, IDbTransaction trans)
        {
            var sql = "DELETE FROM wf_variable WHERE process_id = @ProcessId AND version = @Version AND activity_id = @ActivityId";
            Repository.Execute(conn, sql, new { ProcessId = processId, Version = version, ActivityId = activityId }, trans);
        }
    }
}
