using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slickflow.Data;

namespace Slickflow.Engine.Business.SqlProvider
{
    /// <summary>
    /// Database Field Mapper
    /// </summary>
    public class DatabaseFieldMapper
    {
        private static readonly Dictionary<string, string> FieldMappings = new(StringComparer.OrdinalIgnoreCase)
        {
            ["TaskId"] = "id",
            ["ProcessId"] = "process_id",
            ["AppInstanceId"] = "app_instance_id",
            ["AssignedUserId"] = "assigned_user_id",
            ["EndedUserId"] = "ended_user_id",
            ["AppName"] = "app_name",
            ["ProcessState"] = "process_state",
            ["ActivityType"] = "activity_type",
            ["WorkItemType"] = "work_item_type",
            ["ActivityState"] = "activity_state",
            ["TaskState"] = "task_state",
            ["MainActivityState"] = "main_activity_state",
            ["Version"] = "version"
        };

        public static string MapField(string entityField)
        {
            var dbType = Slickflow.Data.DBTypeExtenstions.GetDbType();
            if (dbType == DatabaseTypeEnum.SQLSERVER)
                return entityField;

            if (FieldMappings.TryGetValue(entityField, out var dbField))
                return dbField;

            return ConvertToSnakeCase(entityField);
        }

        private static string ConvertToSnakeCase(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;  

            return string.Concat(input.Select((c, i)=>
                i > 0 && char.IsUpper(c) ? "_" + c.ToString() : c.ToString()));
        }
    }
}
