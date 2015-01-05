using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// 流程执行人(业务应用的办理者)
    /// </summary>
    public class WfAppRunner
    {
        public string AppName { get; set; }
        public string AppInstanceID { get; set; }
        public string AppInstanceCode { get; set; }
        public string ProcessGUID { get; set; }
        public string FlowStatus { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public IDictionary<string, PerformerList> NextActivityPerformers { get; set; }
        public IDictionary<string, string> Conditions { get; set; }
        public Nullable<int> TaskID { get; set; }        //任务ID，区分当前用户ActivityInstance列表的唯一任务
        public string JumpbackActivityGUID { get; set; }     //回跳的节点GUID
        public string Other { get; set; }
    }

    /// <summary>
    /// 流程返签、撤销和退回接收人的实体对象
    /// </summary>
    public class WfBackwardTaskReciever
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string ActivityName { get; set; }

        /// <summary>
        /// 构造WfBackwardReciever实例
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="userName"></param>
        /// <param name="backwardToActivityName"></param>
        /// <returns></returns>
        public static WfBackwardTaskReciever Instance(string backwardToActivityName,
            string userID,
            string userName)
        {
            var instance = new WfBackwardTaskReciever();
            instance.ActivityName = backwardToActivityName;
            instance.UserID = userID;
            instance.UserName = userName;
            
            return instance;
        }
    }
}
