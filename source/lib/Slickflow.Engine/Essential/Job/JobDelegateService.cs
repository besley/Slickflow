using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Text;
using Slickflow.Data;
using Slickflow.Engine.Common;
using Slickflow.Engine.Business.Entity;
using Slickflow.Module.Essential.Job;

namespace Slickflow.Engine.Essential.Job
{
    /// <summary>
    /// 作业代理服务
    /// </summary>
    public class JobDelegateService
    {
        ///// <summary>
        ///// 插入定时作业数据
        ///// </summary>
        ///// <param name="jobTimerName">作业名称</param>
        ///// <param name="processInstance">流程实例</param>
        ///// <param name="activityInstance">活动实例</param>
        ///// <param name="runner">运行用户</param>
        ///// <param name="session">数据会话</param>
        //internal void Insert(string jobTimerName, ProcessInstanceEntity processInstance,
        //    ActivityInstanceEntity activityInstance, WfAppRunner runner, IDbSession session)
        //{
        //    var entity = new JobTimerEntity();
        //    entity.JobInstanceType = (byte)JobInstanceTypeEnum.ActivityInstance;
        //    entity.JobTimerType = (byte)JobTimerTypeEnum.Timer;
        //    entity.JobTimerName = jobTimerName;
        //    entity.ProcessGUID = processInstance.ProcessGUID;
        //    entity.Version = processInstance.Version;
        //    entity.ProcessInstanceID = activityInstance.ProcessInstanceID;
        //    entity.ActivityInstanceID = activityInstance.ID;
        //    entity.AppName = activityInstance.AppName;
        //    entity.AppInstanceID = activityInstance.AppInstanceID;
        //    entity.AppInstanceCode = activityInstance.AppInstanceCode;
        //    entity.JsonAppRunner = JsonSerializer.SerializeToString(runner);
        //    entity.CreatedDateTime = System.DateTime.Now;
        //    entity.LastUpdatedDateTime = System.DateTime.Now;
        //    entity.Status = (byte)JobTimerStatusEnum.Ready;

        //    var jobService = new JobTimerService();
        //    jobService.Insert(entity, session);
        //}
    }
}
