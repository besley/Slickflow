using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slickflow.Data;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Utility;


namespace Slickflow.Engine.Business.Manager
{
    /// <summary>
    /// 作业数据管理器
    /// </summary>
    public class JobInfoManager : ManagerBase
    {
        /// <summary>
        /// 获取作业信息
        /// </summary>
        /// <param name="topic"></param>
        /// <returns>作业实体</returns>
        public List<JobInfoEntity> GetJobSubscribedByTopic(string topic)
        {
            var sql = @"SELECT 
                            * 
                        FROM WfJobInfo
                        WHERE Topic=@topic
                            AND JobStatus=@jobStatus 
                        ORDER BY ID DESC";
            var list = Repository.Query<JobInfoEntity>(sql,
                    new
                    {
                        topic = topic,
                        jobStatus = "Subscribed"
                    }).ToList();
            return list;
        }
    }
}
