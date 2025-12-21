using Slickflow.Data;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Common;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Engine.Xpdl.Schedule;
using Slickflow.Module.Localize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Xpdl
{
    internal class ProcessModelBPMNCore
    {
        /// <summary>
        /// Obtain the next node list accompanied by runtime condition information
        /// 获取下一步节点列表，伴随运行时条件信息
        /// </summary>
        internal NextActivityMatchedResult GetNextActivityTreeListCore(IProcessModel processModel,
            string currentActivityId,
            Nullable<int> activityInstanceId,
            IDictionary<string, string> conditionKeyValuePair,
            IDbSession session)
        {
            try
            {
                NextActivityMatchedResult result = null;
                NextActivityMatchedType resultType = NextActivityMatchedType.Unknown;

                //创建“下一步节点”的根节点
                //Create the root node for the 'next step node'
                NextActivityComponent root = NextActivityComponentFactory.CreateNextActivityComponent();

                //开始正常情况下的路径查找
                //Start normal path search
                List<Transition> transitionList = GetForwardTransitionList(processModel.Process,
                    currentActivityId,
                    conditionKeyValuePair).ToList();

                if (transitionList.Count > 0)
                {
                    //遍历连线，获取下一步节点的列表
                    //Traverse the connection to obtain the list of next nodes
                    NextActivityComponent child = null;
                    foreach (Transition transition in transitionList)
                    {
                        if (XPDLHelper.IsSimpleComponentNode(transition.ToActivity.ActivityType) == true)
                        {
                            child = NextActivityComponentFactory.CreateNextActivityComponent(transition, transition.ToActivity);
                        }
                        else if (XPDLHelper.IsGatewayComponentNode(transition.ToActivity.ActivityType) == true)
                        {
                            NextActivityScheduleBase activitySchedule = NextActivityScheduleFactory.CreateActivitySchedule(processModel,
                                transition.ToActivity.GatewayDetail.SplitJoinType,
                                activityInstanceId);

                            //获取网关后面的节点
                            //Get the nodes behind the gateway
                            child = activitySchedule.GetNextActivityListFromGateway(transition,
                                transition.ToActivity,
                                conditionKeyValuePair,
                                session,
                                out resultType);
                        }
                        else if (XPDLHelper.IsCrossOverComponentNode(transition.ToActivity.ActivityType) == true)
                        {
                            //事件类型的特殊节点处理，跟网关类似
                            //Special node handling for event types, similar to gateways
                            NextActivityScheduleBase activitySchedule = NextActivityScheduleFactory.CreateActivityScheduleIntermediate(processModel);
                            child = activitySchedule.GetNextActivityListFromGateway(transition,
                                transition.ToActivity,
                                conditionKeyValuePair,
                                session,
                                out resultType);
                        }
                        else
                        {
                            var errMsg = string.Format("Unknown node type：{0}", transition.ToActivity.ActivityType.ToString());
                            LogManager.RecordLog(LocalizeHelper.GetEngineMessage("processmodel.getnextactivitylist.error"),
                                LogEventType.Exception,
                                LogPriority.Normal,
                                null,
                                new WfXpdlException(errMsg));
                            throw new XmlDefinitionException(errMsg);
                        }

                        if (child != null)
                        {
                            root.Add(child);
                            resultType = NextActivityMatchedType.Successed;
                        }
                    }
                }
                else
                {
                    resultType = NextActivityMatchedType.NoneTransitionFilteredByCondition;
                }
                result = NextActivityMatchedResult.CreateNextActivityMatchedResultObject(resultType, root);
                return result;
            }
            catch (System.Exception e)
            {
                LogManager.RecordLog(LocalizeHelper.GetEngineMessage("processmodel.getnextactivitylist.error"),
                    LogEventType.Exception,
                    LogPriority.Normal,
                    null,
                    new WfXpdlException(e.Message));
                throw new WfXpdlException(LocalizeHelper.GetEngineMessage("processmodel.getnextactivitylist.error", e.Message),
                    e);
            }
        }

        private IList<Transition> GetForwardTransitionList(Process process,
            string fromActivityId,
            IDictionary<string, string> conditionKeyValuePair)
        {
            var validTransitionList = new List<Transition>();
            var transitionList = ProcessModelHelper.GetForwardTransitionList(process, fromActivityId);
            foreach (var transition in transitionList)
            {
                bool isValidTranstion = IsValidTransition(transition, conditionKeyValuePair);
                if (isValidTranstion) validTransitionList.Add(transition);
            }
            return validTransitionList;
        }

        /// <summary>
        /// Is it a transition that meets the conditions? If the condition is empty, it defaults to being valid.
        /// 是否是满足条件的Transition，如果条件为空，默认是有效的。
        /// </summary>
        private bool IsValidTransition(Transition transition,
           IDictionary<string, string> conditionKeyValuePair)
        {
            bool isValid = false;

            if (transition.Condition != null && !string.IsNullOrEmpty(transition.Condition.ConditionText))
            {
                if (conditionKeyValuePair != null && conditionKeyValuePair.Count != 0)
                {
                    isValid = ParseCondition(transition, conditionKeyValuePair);
                }
            }
            else
            {
                //流程节点上定义的条件为空，则认为连线是可到达的
                //If the condition defined on the process node is empty, it is considered that the connection is reachable
                isValid = true;
            }
            return isValid;
        }

        /// <summary>
        /// Parse Condition by LINQ
        /// 用LINQ解析条件表达式
        /// </summary>
        private bool ParseCondition(Transition transition, IDictionary<string, string> conditionKeyValuePair)
        {
            Boolean result = false;
            try
            {
                string expression = transition.Condition.ConditionText;
                string expressionReplaced = ExpressionParser.ReplaceParameterToValue(expression, conditionKeyValuePair);
                result = ExpressionParser.Parse(expressionReplaced);
            }
            catch (System.Exception ex)
            {
                //throw new WfXpdlException(string.Format("条件表达式解析错误，请确认是否传入所有变量参数！内部错误描述：{0}", ex.Message),
                //    ex);
                ;
            }
            return result;
        }
    }
}
