﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slickflow.Data;

namespace Slickflow.Data.DataProvider
{
    /// <summary>
    /// ORACLE SQL 特定语句
    /// </summary>
    public class MySqlWfDataProvider : IWfDataProvider
    {
        public string GetSqlTaskPaged(string sql)
        {
            sql = @"SELECT
                            * 
                         FROM vwWfActivityInstanceTasks 
                         WHERE ProcessState=2 
                            AND (ActivityType=4 OR WorkItemType=1)
                            AND ActivityState=@activityState
							AND TaskState<>32 
                        LIMIT 100";
            return sql;
        }

        public string GetSqlTaskOfMineByAtcitivityInstance(string sql)
        {
            sql = @"SELECT 
                            * 
                        FROM vwWfActivityInstanceTasks 
                        WHERE ActivityInstanceID=@activityInstanceID 
                            AND AssignedToUserID=@userID 
                            AND ProcessState=2 
                            AND (ActivityType=4 OR ActivityType=5 OR ActivityType=6 OR WorkItemType=1)
                            AND (ActivityState=1 OR ActivityState=2) 
                        ORDER BY TASKID DESC
                        LIMIT 1";
            return sql;
        }

        public string GetSqlTaskOfMineByAppInstance(string sql)
        {
            sql = @"SELECT 
                            * 
                           FROM vwWfActivityInstanceTasks 
                           WHERE AppInstanceID=@appInstanceID 
                                AND ProcessGUID=@processGUID 
                                AND AssignedToUserID=@userID 
                                AND ProcessState=2 
                                AND (ActivityType=4 OR ActivityType=5 OR ActivityType=6 OR WorkItemType=1)
                                AND (ActivityState=1 OR ActivityState=2) 
                           ORDER BY TASKID DESC
                           LIMIT 1";
            return sql;
        }
    }
}
