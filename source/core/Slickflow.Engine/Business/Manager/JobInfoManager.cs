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
    /// Job Info Manager
    /// 作业数据管理器
    /// </summary>
    public class JobInfoManager : ManagerBase
    {
        /// <summary>
        /// Retrieve job info by the topic
        /// 根据主题获取作业信息
        /// </summary>
        /// <param name="topic"></param>
        /// <returns></returns>
        public List<JobInfoEntity> GetJobSubscribedByTopic(string topic)
        {
            var sql = @"SELECT 
                            * 
                        FROM wf_job_info
                        WHERE topic=@topic
                            AND job_status=@jobStatus 
                        ORDER BY id DESC";
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
