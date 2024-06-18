using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Module.Localize;

namespace Slickflow.Engine.Xpdl.Schedule
{
    /// <summary>
    /// 执行活动的可选类型
    /// </summary>
    public class NextActivityMatchedResult
    {
        #region 属性和构造函数
        /// <summary>
        /// 消息
        /// </summary>
        public string Message
        {
            get;
            set;
        }

        /// <summary>
        /// 匹配类型
        /// </summary>
        public NextActivityMatchedType MatchedType
        {
            get;
            set;
        }

        /// <summary>
        /// 根节点
        /// </summary>
        public NextActivityComponent Root
        {
            get;
            set;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="matchedType">匹配类型</param>
        /// <param name="root">根节点</param>
        private NextActivityMatchedResult(NextActivityMatchedType matchedType,
            NextActivityComponent root)
        {
            MatchedType = matchedType;
            Root = root;
        }

        #endregion

        #region 创建方法
        /// <summary>
        /// 创建方法
        /// </summary>
        /// <param name="matchedType">匹配类型</param>
        /// <param name="root">根节点</param>
        /// <returns>下一步匹配结果</returns>
        internal static NextActivityMatchedResult CreateNextActivityMatchedResultObject(NextActivityMatchedType matchedType,
            NextActivityComponent root)
        {
            NextActivityMatchedResult result = new NextActivityMatchedResult(matchedType, root);
            switch (matchedType)
            {
                case NextActivityMatchedType.Unknown:
                    result.Message = LocalizeHelper.GetEngineMessage("nextactivitymatchedresult.Unkonwn");
                    break;
                case NextActivityMatchedType.Failed:
                    result.Message = LocalizeHelper.GetEngineMessage("nextactivitymatchedresult.Exceptional"); 
                    break;
                case NextActivityMatchedType.Successed:
                    result.Message = LocalizeHelper.GetEngineMessage("nextactivitymatchedresult.Successed"); 
                    break;
                case NextActivityMatchedType.NoneTransitionFilteredByCondition:
                    result.Message = LocalizeHelper.GetEngineMessage("nextactivitymatchedresult.NoneTransitionFilteredByCondition"); 
                    break;
                case NextActivityMatchedType.WaitingForOtherSplitting:
                    result.Message = LocalizeHelper.GetEngineMessage("nextactivitymatchedresult.WaitingForOtherSplitting"); 
                    break;
                case NextActivityMatchedType.WaitingForAgreedOrRefused:
                    result.Message = LocalizeHelper.GetEngineMessage("nextactivitymatchedresult.WaitingForAgreedOrRefused");
                    break;
                case NextActivityMatchedType.NoneTransitionMatchedToSplit:
                    result.Message = LocalizeHelper.GetEngineMessage("nextactivitymatchedresult.NoneWayMatchedToSplit"); 
                    break;
                case NextActivityMatchedType.FailedPassRateOfMulitipleInstance:
                    result.Message = LocalizeHelper.GetEngineMessage("nextactivitymatchedresult.FailedPassRateOfMulitipleInstance"); 
                    break;
                case NextActivityMatchedType.WaitingForOthersJoin:
                    result.Message = LocalizeHelper.GetEngineMessage("nextactivitymatchedresult.WaitingForOthersJoin"); 
                    break;
                case NextActivityMatchedType.NotMadeItselfToJoin:
                    result.Message = LocalizeHelper.GetEngineMessage("nextactivitymatchedresult.NotMadeItselfToJoin"); 
                    break;
            }
            return result;
        }
        #endregion
    }
}
