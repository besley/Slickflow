using System.Collections.Generic;
using Slickflow.Engine.Business.Entity;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// 当前运行节点Map信息
    /// </summary>
    public class CurrentRunningNodeMap
    {
        public ProcessStateEnum ProcessState { get; set; }
        public ActivityInstanceEntity RunningNode { get; set; }
        public bool IsMine { get; set; }
        public ActivityInstanceEntity PreviousActivityInstance { get; set; }
        public IList<NodeView> NextActivityTree { get; set; }

        /// <summary>
        /// 创建实例方法
        /// </summary>
        /// <param name="isMine"></param>
        /// <param name="previousActivityInstance"></param>
        /// <param name="nextActivityTree"></param>
        /// <returns></returns>
        public static CurrentRunningNodeMap Instance(ActivityInstanceEntity runningNode,
            bool isMine,
            ActivityInstanceEntity previousActivityInstance,
            IList<NodeView> nextActivityTree)
        {
            var nodeMap = new CurrentRunningNodeMap();
            nodeMap.RunningNode = runningNode;
            nodeMap.IsMine = isMine;
            nodeMap.PreviousActivityInstance = previousActivityInstance;
            nodeMap.NextActivityTree = nextActivityTree;

            return nodeMap;
        }
    }

    /// <summary>
    /// 当前流程运行节点的Map信息
    /// </summary>
    public class CurrentRunningNodeMapComplex
    {
        public bool IsMine { get; set; }
        public IList<ActivityInstanceEntity> PreviousActivityInstance { get; set; }
        public IList<NodeView> NextActivityTree { get; set; }

        /// <summary>
        /// 创建实例
        /// </summary>
        /// <param name="taskView"></param>
        /// <param name="previousActivityInstance"></param>
        /// <param name="nextActivityTree"></param>
        /// <returns></returns>
        public static CurrentRunningNodeMapComplex Instance(TaskViewEntity taskView,
            IList<ActivityInstanceEntity> previousActivityInstance,
            IList<NodeView> nextActivityTree)
        {
            var runningNode = new CurrentRunningNodeMapComplex();

            runningNode.IsMine = false;
            runningNode.PreviousActivityInstance = previousActivityInstance;
            runningNode.NextActivityTree = nextActivityTree;

            return runningNode;
        }
    }
}
