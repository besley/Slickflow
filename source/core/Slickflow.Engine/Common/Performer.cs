using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// The performer object of the task
    /// 任务的执行者对象
    /// </summary>
    public class Performer
    {
        /// <summary>
        /// Construct mehtod
        /// 构造函数
        /// </summary>
        public Performer(string userId, string userName)
        {
            UserId = userId;
            UserName = userName;
        }

        /// <summary>
        /// User Id
        /// 用户Id
        /// </summary>
        public string UserId
        {
            get;
            set;
        }

        /// <summary>
        /// User Name
        /// 用户名称
        /// </summary>
        public string UserName
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Performer List
    /// 执行者列表类
    /// </summary>
    public class PerformerList : List<Performer>
    {
        public PerformerList()
        {
        }
    }
}
