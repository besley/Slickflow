using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Module.Essential.Entity
{
    /// <summary>
    /// 信号实体对象
    /// </summary>
    public class SignalEntity
    {
        public string Topic { get; set; }
        public string Line { get; set; }
        public string JobName { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="line"></param>
        /// <param name="jobName"></param>
        public SignalEntity(string topic, string line, string jobName)
        {
            Topic = topic;
            Line = line;
            JobName = jobName;
        }
    }
}
