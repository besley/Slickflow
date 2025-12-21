using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// Process reverse, withdraw, sendback task receiver
    /// 流程返签、撤销和退回接收人的实体对象
    /// </summary>
    public class WfBackwardTaskReceiver
    {
        /// <summary>
        /// User Id
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// User Name
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Activity Name
        /// 活动名称
        /// </summary>
        public string ActivityName { get; set; }

        /// <summary>
        /// Construct an instance of WfBackwardReceiver
        /// 构造WfBackwardReceiver实例
        /// </summary>
        public static WfBackwardTaskReceiver Instance(string backwardToActivityName,
            string userId,
            string userName)
        {
            var instance = new WfBackwardTaskReceiver();
            instance.ActivityName = backwardToActivityName;
            instance.UserId = userId;
            instance.UserName = userName;

            return instance;
        }
    }
}
