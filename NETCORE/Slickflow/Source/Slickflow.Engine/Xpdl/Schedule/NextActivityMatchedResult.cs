using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Xpdl.Schedule
{
    /// <summary>
    /// 执行活动的可选类型
    /// </summary>
    public class NextActivityMatchedResult
    {
        #region 属性和构造函数
        internal static string Unkonwn = "未知原因";
        internal static string Exceptional = "未能获取下一步节点";
        internal static string Successed = "已经成功获取下一步节点";
        internal static string NoneTransitionAsBeingFiltered = "并行分支上的条件不满足，无路径可达后续节点";
        internal static string NoneWayMatchedToSplit = "该分支上的条件不成立，不能到后续节点";
        internal static string WaitingForOthersJoin = "要合并的分支上的条件不是全部成立，无法合并到后续节点";
        internal static string NotMadeItselfToJoin = "没有满足条件的路径，无法合并到后续节点";
        internal static string NoneTransitionFilteredByCondition = "无法获取下一步节点列表，因为条件表达式未被满足，请查看必填字段是否已经填写！";

        public string Message
        {
            get;
            set;
        }

        public NextActivityMatchedType MatchedType
        {
            get;
            set;
        }

        public NextActivityComponent Root
        {
            get;
            set;
        }

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
        /// <param name="scheduleStatus"></param>
        /// <param name="root"></param>
        /// <returns></returns>
        internal static NextActivityMatchedResult CreateNextActivityMatchedResultObject(NextActivityMatchedType matchedType,
            NextActivityComponent root)
        {
            NextActivityMatchedResult result = new NextActivityMatchedResult(matchedType, root);
            switch (matchedType)
            {
                case NextActivityMatchedType.Unknown:
                    result.Message = NextActivityMatchedResult.Unkonwn;
                    break;
                case NextActivityMatchedType.Failed:
                    result.Message = NextActivityMatchedResult.Exceptional;
                    break;
                case NextActivityMatchedType.Successed:
                    result.Message = NextActivityMatchedResult.Successed;
                    break;
                case NextActivityMatchedType.NoneTransitionFilteredByCondition:
                    result.Message = NextActivityMatchedResult.NoneTransitionFilteredByCondition;
                    break;
                case NextActivityMatchedType.WaitingForSplitting:
                    result.Message = NextActivityMatchedResult.NoneTransitionAsBeingFiltered;
                    break;
                case NextActivityMatchedType.NoneTransitionMatchedToSplit:
                    result.Message = NextActivityMatchedResult.NoneWayMatchedToSplit;
                    break;
                case NextActivityMatchedType.WaitingForOthersJoin:
                    result.Message = NextActivityMatchedResult.WaitingForOthersJoin;
                    break;
                case NextActivityMatchedType.NotMadeItselfToJoin:
                    result.Message = NextActivityMatchedResult.NotMadeItselfToJoin;
                    break;
            }
            return result;
        }
        #endregion
    }
}
