using Dapper;
using Slickflow.Data;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Common;

namespace Slickflow.Engine.Business.SqlProvider
{
    /// <summary>
    /// SQL Provider Factory
    /// </summary>
    public class DatabaseSqlProviderFactory
    {
        private static DatabaseTypeEnum _databaseType = Data.DatabaseTypeEnum.NONE;
        static DatabaseSqlProviderFactory()
        {
            _databaseType = Slickflow.Data.DBTypeExtenstions.GetDbType();
        }

        public static string GetProcessListSimple_SQL()
        {
            var sql = string.Empty;
            if (_databaseType == DatabaseTypeEnum.SQLSERVER)
            {
                sql = @"SELECT 
                        Id, 
                        ProcessId, 
                        ProcessName,
                        ProcessCode,
                        Version,
                        Status,
                        AppType,
                        PackageType,
                        PackageId,
                        PageUrl,
                        Icon,
                        StartType,
                        StartExpression,
                        EndType,
                        EndExpression,
                        CreatedDateTime,
                        UpdatedDateTime
                    FROM WfProcess
                    ORDER BY UpdatedDateTime DESC";
            }
            else
            {
                sql = @"SELECT 
                        id AS Id, 
                        process_id AS ProcessId, 
                        process_name AS ProcessName,
                        process_code AS ProcessCode,
                        version AS Version,
                        status AS Status,
                        app_type AS AppType,
                        package_type AS PackageType,
                        package_id AS PackageId,
                        page_url AS PageUrl,
                        icon AS Icon,
                        start_type AS StartType,
                        start_expression AS StartExpression,
                        end_type AS EndType,
                        end_expression AS EndExpression,
                        created_datetime AS CreatedDateTime,
                        updated_datetime As UpdatedDateTime
                    FROM wf_process
                    ORDER BY COALESCE(updated_datetime, '1970-01-01') DESC";
            }
            return sql;
        }

        public static string GetTaskListEMailUnSent_SQL()
        {
            //processState:2 -running 流程处于运行状态
            //activityType:4 -task type 表示“任务”类型的节点
            //activityState: 1-ready（准备）, 2-running（运行）
            //isEMailSent: 0-邮件未发送, 1-发送成功, -1-发送失败
            var sql = string.Empty;
            if (_databaseType == Data.DatabaseTypeEnum.SQLSERVER)
            {
                sql = @"SELECT * 
                             FROM vwWfActivityInstanceTasks 
                             WHERE ProcessState=2 
                                AND (ActivityType=4 OR WorkItemType=1)
                                AND ActivityState=1
                                AND IsEMailSent=0
							    AND TaskState<>32
                            ";
            }
            else
            {
                sql = @"SELECT * 
                             FROM vw_wf_task_details 
                             WHERE process_state=2 
                                AND (activity_type=4 OR work_item_type=1)
                                AND activity_state=1
                                AND is_email_sent=0
							    AND task_state<>32
                            ";
            }
            return sql;
        }

        public static string GetReadyTaskOfApp_SQL()
        {
            var sql = string.Empty;
            if (_databaseType == Data.DatabaseTypeEnum.SQLSERVER)
            {
                //processState:2 -running 流程处于运行状态
                //activityType:4 -task type 表示“任务”类型的节点
                //activityState: 1-ready（准备）
                sql = @"SELECT 
                                * 
                           FROM vwWfActivityInstanceTasks 
                           WHERE AppInstanceId=@appInstanceId 
                                AND ProcessId=@processId 
                                AND ProcessState=2 
                                AND (ActivityType=4 OR WorkItemType=1)
                                AND ActivityState=1
                           ORDER BY TaskId DESC";
            }
            else
            {
                sql = @"SELECT 
                                * 
                           FROM vw_wf_task_details 
                           WHERE app_instance_id=@appInstanceId 
                                AND process_id=@processId 
                                AND process_state=2 
                                AND (activity_type=4 OR work_item_type=1)
                                AND activity_state=1
                           ORDER BY id DESC";
            }
            return sql;
        }

        public static string GetTaskOfMine_SQL()
        {
            var sql = string.Empty;
            if (_databaseType == Data.DatabaseTypeEnum.SQLSERVER)
            {
                //processState:2 -running 流程处于运行状态
                //activityType:4 -task type 表示“任务”类型的节点
                //activityState: 1-ready（准备）, 2-running（）运行；
                //2015.09.10 besley
                //added ActivityType to WorkItemType
                //To handle multiple types of task nodes, including regular tasks, multiple instances, and subprocess nodes
                sql = @"SELECT 
                            TOP 1 * 
                       FROM vwWfActivityInstanceTasks 
                       WHERE AppInstanceId=@appInstanceId 
                            AND ProcessId=@processId 
                            AND AssignedUserId=@userId 
                            AND ProcessState=2 
                            AND (ActivityType=4 OR ActivityType=5 OR ActivityType=6 OR WorkItemType=1)
                            AND (ActivityState=1 OR ActivityState=2) 
                       ORDER BY TaskId DESC";
            }
            else
            {
                sql = @"SELECT 
                            * 
                       FROM vw_wf_task_details 
                       WHERE app_instance_id=@appInstanceId 
                            AND process_id=@processId 
                            AND assigned_user_id=@userId 
                            AND process_state=2 
                            AND (activity_type=4 OR activity_type=5 OR activity_type=6 OR work_item_type=1)
                            AND (activity_state=1 OR activity_state=2) 
                       ORDER BY id DESC
                       LIMIT 1";
            }
            return sql;
        }

        public static string GetTaskOfMineByActivityInstance_SQL()
        {
            var sql = string.Empty;
            if (_databaseType == Data.DatabaseTypeEnum.SQLSERVER)
            {
                //processState:2 -running 流程处于运行状态
                //activityType:4 -task type 表示“任务”类型的节点
                //activityState: 1-ready（准备）, 2-running（）运行；
                sql = @"SELECT 
                                TOP 1 *
                            FROM vwWfActivityInstanceTasks 
                            WHERE ActivityInstanceId=@activityInstanceId 
                                AND AssignedUserId=@userId 
                                AND ProcessState=2 
                                AND (ActivityType=4 OR ActivityType=5 OR ActivityType=6) 
                                AND (ActivityState=1 OR ActivityState=2) 
                            ORDER BY TaskId DESC";
            }
            else
            {
                sql = @"SELECT 
                            * 
                       FROM vw_wf_task_details 
                       WHERE activity_instance_id=@activityInstanceId 
                            AND assigned_user_id=@userId 
                            AND process_state=2 
                            AND (activity_type=4 OR activity_type=5 OR activity_type=6)
                            AND (activity_state=1 OR activity_state=2) 
                       ORDER BY id DESC
                       LIMIT 1";
            }
            return sql;
        }

        public static string GetTaskDoneListTop_SQL()
        {
            var sql = string.Empty;
            if (_databaseType == Data.DatabaseTypeEnum.SQLSERVER)
            {
                sql = @"SELECT
                                TOP 10 * 
                            FROM vwWfActivityInstanceTasks 
                            WHERE ProcessState=2 
                                AND (ActivityType=4 OR WorkItemType=1)
                                AND ActivityState=4
                                AND TaskState<>32
                            ORDER BY TaskId DESC";
            }
            else
            {
                sql = @"SELECT
                                * 
                            FROM vw_wf_task_details 
                            WHERE process_state=2 
                                AND (activity_type=4 OR work_item_type=1)
                                AND activity_state=4
                                AND task_state<>32
                            ORDER BY id DESC
                            LIMIT 10";
            }
            return sql;
        }

        public static string GetTaskToDoListTop_SQL()
        {
            var sql = string.Empty;
            if (_databaseType == Data.DatabaseTypeEnum.SQLSERVER)
            {
                sql = @"SELECT
                                TOP 10 * 
                            FROM vwWfActivityInstanceTasks 
                            WHERE ProcessState=2 
                                AND (ActivityType=4 OR WorkItemType=1)
                                AND ActivityState=1
                                AND TaskState<>32
                            ORDER BY TaskId DESC";
            }
            else
            {
                sql = @"SELECT
                                * 
                            FROM vw_wf_task_details 
                            WHERE process_state=2 
                                AND (activity_type=4 OR work_item_type=1)
                                AND activity_state=1
                                AND task_state<>32
                            ORDER BY id DESC
                            LIMIT 10";
            }
            return sql;
        }

        public static string GetFirstRunningTask_SQL()
        {
            var sql = string.Empty;
            if (_databaseType == Data.DatabaseTypeEnum.SQLSERVER)
            {
                sql = @"SELECT
                            * 
                         FROM vwWfActivityInstanceTasks 
                         WHERE ProcessState=2 
                            AND ActivityInstanceId=@activityInstanceId 
                            AND (ActivityType=4 OR WorkItemType=1)
                            AND (ActivityState=1 OR ActivityState=2)
                            AND (TaskState=1 OR TaskState=2)
                            ORDER BY TaskState DESC
                        ";
            }
            else
            {
                sql = @"SELECT
                            * 
                         FROM vw_wf_task_details 
                         WHERE process_state=2 
                            AND activity_instance_id=@activityInstanceId 
                            AND (activity_type=4 OR work_item_type=1)
                            AND (activity_state=1 OR activity_state=2)
                            AND (task_state=1 OR task_state=2)
                            ORDER BY task_state DESC
                        ";
            }
            return sql;
        }

        public static string GetTaskViewByActivity_SQL()
        {
            var sql = string.Empty;
            if (_databaseType == Data.DatabaseTypeEnum.SQLSERVER)
            {
                sql = @"SELECT
                            * 
                         FROM vwWfActivityInstanceTasks 
                         WHERE ActivityInstanceId=@activityInstanceId
                            AND ProcessInstanceId=@processInstanceId
                        ";
            }
            else
            {
                sql = @"SELECT
                            * 
                         FROM vw_wf_task_details 
                         WHERE activity_instance_id=@activityInstanceId
                            AND process_instance_id=@processInstanceId
                        ";
            }
            return sql;
        }

        public static string GetTasksPaged_SQL(TaskQuery query, int activityState, DynamicParameters parameters)
        {
            var sql = string.Empty;
            if (_databaseType == Data.DatabaseTypeEnum.SQLSERVER)
            {
                //processState:2 -running 流程处于运行状态
                //activityType:4 -task type 表示“任务”类型的节点
                //activityState: 1-ready（准备）, 2-running（运行）；
                sql = @"SELECT
                                * 
                            FROM vwWfActivityInstanceTasks 
                            WHERE ProcessState=2 
                                AND (ActivityType=4 OR WorkItemType=1)";
            }
            else
            {
                sql = @"SELECT
                                * 
                            FROM vw_wf_task_details 
                            WHERE process_state=2 
                                AND (activity_type=4 OR work_item_type=1)";
            }

            //sql context
            var sqlBuilder = new StringSQLBuilder(sql);

            if (activityState == (short)ActivityStateEnum.Ready
                || activityState == (short)ActivityStateEnum.Running)
            {
                sqlBuilder.And(DatabaseFieldMapper.MapField("ActivityState"), Data.Operator.Eq, "@activityState");
                sqlBuilder.And(DatabaseFieldMapper.MapField("TaskState"), Data.Operator.Nq, 32);
            }
            else if (activityState == (short)ActivityStateEnum.Completed)
            {
                //正常完成的情况
                //Normal completed
                sqlBuilder.And(DatabaseFieldMapper.MapField("TaskState"), Data.Operator.Eq, 4);
                //特殊情况下的close状态，认为是已经完成
                //The close status in special circumstances is considered completed
                //sqlBuilder.Or(DatabaseFieldMapper.MapField("TaskState"), Data.Operator.Eq, 32);
            }

            parameters.Add("@activityState", activityState);


            if (activityState == 1)
            {
                sqlBuilder.And(DatabaseFieldMapper.MapField("MainActivityState"), Data.Operator.Nq, 4);
            }

            if (!string.IsNullOrEmpty(query.AppInstanceId))
            {
                sqlBuilder.And(DatabaseFieldMapper.MapField("AppInstanceId"), Data.Operator.Eq, "@appInstanceId");
                parameters.Add("@appInstanceId", query.AppInstanceId);
            }

            if (!string.IsNullOrEmpty(query.ProcessId))
            {
                sqlBuilder.And(DatabaseFieldMapper.MapField("ProcessId"), Data.Operator.Eq, "@processId");
                parameters.Add("@processId", query.ProcessId);
            }

            if (!string.IsNullOrEmpty(query.Version))
            {
                sqlBuilder.And(DatabaseFieldMapper.MapField("Version"), Data.Operator.Eq, "@version");
                parameters.Add("@version", query.Version);
            }

            if (!string.IsNullOrEmpty(query.UserId))
            {
                sqlBuilder.And(DatabaseFieldMapper.MapField("AssignedUserId"), Data.Operator.Eq, "@assignedUserId");
                parameters.Add("@assignedUserId", query.UserId);
            }

            if (!string.IsNullOrEmpty(query.EndedUserId))
            {
                sqlBuilder.And(DatabaseFieldMapper.MapField("EndedUserId"), Data.Operator.Eq, "@endedUserId");
                parameters.Add("@endedUserId", query.EndedUserId);
            }

            if (!string.IsNullOrEmpty(query.AppName))
            {
                sqlBuilder.And(DatabaseFieldMapper.MapField("AppName"), Data.Operator.Like, "@appName");
                parameters.Add("@appName", string.Format("%{0}%", query.AppName));
            }
            sqlBuilder.OrderBy(DatabaseFieldMapper.MapField("TaskId"), true);

            var sqlWhere = sqlBuilder.GetSQL();
            return sqlWhere;
        }
    }
}
