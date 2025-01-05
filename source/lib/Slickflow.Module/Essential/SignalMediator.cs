using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slickflow.Module.Essential.Entity;
using Slickflow.Module.Logging;

namespace Slickflow.Module.Essential
{
    /// <summary>
    /// Signal Mediator
    /// 信号处理器
    /// </summary>
    public class SignalMediator
    {
        /// <summary>
        /// Invoke from signal
        /// 激活信号服务
        /// </summary>
        /// <param name="consumeSignalFunction">signal function</param>
        /// <param name="topic">topic</param>
        /// <param name="line">content</param>
        /// <param name="jobName">job name</param>
        public void InvokeFromSignal(Func<SignalEntity, List<SignalConsumedResult>> consumeSignalFunction,
            string topic,
            string line,
            string jobName)
        {
            try
            {
                var resultList = consumeSignalFunction?.Invoke(new SignalEntity(topic, line, jobName));
                foreach (var result in resultList)
                {
                    if (result.Status == SignalConsumedStatus.Success)
                    {
                        LogService.RecordInfoLog(JobTypeEnum.Signal,
                            jobName,
                            topic,
                            string.Empty,
                            string.Empty,
                            1,
                            LogAdminDefine.ADMIN_MESSAGE_SUCCESS,
                            line);
                    }
                    else
                    {
                        LogService.RecordInfoLog(JobTypeEnum.Message,
                            jobName,
                            topic,
                            string.Empty,
                            string.Empty,
                            -1,
                            result.Message,
                            line);
                    }
                }
            }
            catch (Exception ex)
            {
                var stackTrace = ex.InnerException != null ? ex.InnerException.StackTrace : ex.StackTrace;
                LogService.RecordErrorLog(JobTypeEnum.Message,
                    jobName,
                    topic,
                    string.Empty,
                    string.Empty,
                    -1,
                    string.Format("Exception Message:{0}", ex.Message),
                    ex.StackTrace,
                    ex.InnerException != null ? ex.InnerException.StackTrace : null,
                    line);
                throw;
            }
        }
    }
}
