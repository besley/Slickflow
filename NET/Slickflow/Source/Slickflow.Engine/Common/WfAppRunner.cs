/*
* Slickflow 软件遵循自有项目开源协议，也可联系作者获取企业版商业授权和技术支持；
* 除此之外的使用则视为不正当使用，请您务必避免由此带来的一切商业版权纠纷和损失。
* 
The Slickflow Open License (SfPL 1.0)
Copyright (C) 2014  .NET Workflow Engine Library

1. Slickflow software must be legally used, and should not be used in violation of law, 
   morality and other acts that endanger social interests;
2. Non-transferable, non-transferable and indivisible authorization of this software;
3. The source code can be modified to apply Slickflow components in their own projects 
   or products, but Slickflow source code can not be separately encapsulated for sale or 
   distributed to third-party users;
4. The intellectual property rights of Slickflow software shall be protected by law, and
   no documents such as technical data shall be made public or sold.
5. The enterprise, ultimate and universe version can be provided with commercial license, 
   technical support and upgrade service.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Engine.Delegate;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// 流程执行人(业务应用的办理者)
    /// </summary>
    public class WfAppRunner
    {
        #region 属性
        public string AppName { get; set; }
        public string AppInstanceID { get; set; }
        public string AppInstanceCode { get; set; }
        public string ProcessGUID { get; set; }
        public string ProcessCode { get; set; }
        public string Version { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }

        public Nullable<int> TaskID { get; set; }        //任务ID，区分当前用户ActivityInstance列表的唯一任务
        public IDictionary<string, string> Conditions = new Dictionary<string, string>();
        public IDictionary<string, string> DynamicVariables { get; set; }
        public IDictionary<string, PerformerList> NextActivityPerformers { get; set; }

        internal DelegateEventList DelegateEventList = new DelegateEventList();
        #endregion
    }

    /// <summary>
    /// 流程返签、撤销和退回接收人的实体对象
    /// </summary>
    public class WfBackwardTaskReceiver
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string ActivityName { get; set; }

        /// <summary>
        /// 构造WfBackwardReceiver实例
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="userName"></param>
        /// <param name="backwardToActivityName"></param>
        /// <returns></returns>
        public static WfBackwardTaskReceiver Instance(string backwardToActivityName,
            string userID,
            string userName)
        {
            var instance = new WfBackwardTaskReceiver();
            instance.ActivityName = backwardToActivityName;
            instance.UserID = userID;
            instance.UserName = userName;
            
            return instance;
        }
    }
}
