using System.Collections.Generic;
using Slickflow.Engine.Common;
using Slickflow.Engine.Xpdl;

namespace Slickflow.Graph
{
    /// <summary>
    /// 连线构造器
    /// </summary>
    public class LinkBuilder
    {
        #region 基本属性
        internal Link _link;
        #endregion

        #region 添加连线属性
        /// <summary>
        /// 添加接收者类型
        /// </summary>
        /// <param name="receiverType">接收者类型</param>
        /// <param name="candidates">人数</param>
        /// <returns>连线构造器</returns>
        public LinkBuilder AddReceiver(ReceiverTypeEnum receiverType, int candidates)
        {
            var receiver = new Receiver();
            receiver.ReceiverType = receiverType;
            receiver.Candidates = candidates;
            _link.Transition.Receiver = receiver;

            return this;
        }

        /// <summary>
        /// 添加条件
        /// </summary>
        /// <param name="conditionType">条件类型</param>
        /// <param name="conditionText">条件文本</param>
        /// <returns>连线构造器</returns>
        public LinkBuilder AddCondition(ConditionTypeEnum conditionType, string conditionText)
        {
            var condition = new ConditionEntity();
            condition.ConditionType = conditionType;
            condition.ConditionText = conditionText;
            _link.Transition.Condition = condition;

            return this;
        }
        #endregion

        #region 静态创建方法
        /// <summary>
        /// 创建转移
        /// </summary>
        /// <param name="description">描述</param>
        /// <returns>连线构造器</returns>
        public static LinkBuilder CreateTransition(string description)
        {
            var lb = new LinkBuilder();
            var link = new Link(description);

            lb._link = link;

            return lb;
        }
        #endregion
    }
}
