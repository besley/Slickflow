using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// 节点通过类型
    /// </summary>
    public class NodePassedResult
    {
        /// <summary>
        /// 节点通过类型熟悉
        /// </summary>
        public NodePassedTypeEnum NodePassedType { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="passedType">通过类型</param>
        public NodePassedResult(NodePassedTypeEnum passedType)
        {
            this.NodePassedType = passedType;
        }

        /// <summary>
        /// 创建方法
        /// </summary>
        /// <param name="passedType">通过类型</param>
        /// <returns></returns>
        public static NodePassedResult Create(NodePassedTypeEnum passedType)
        {
            var result = new NodePassedResult(passedType);
            return result;
        }

        /// <summary>
        /// 根据审批状态创建结果对象
        /// </summary>
        /// <param name="approvalStatus">审批状态</param>
        /// <returns></returns>
        public static NodePassedResult CreateByApprovalStatus(short approvalStatus)
        {
            var result = NodePassedResult.Create(NodePassedTypeEnum.Default);
            if (approvalStatus == (short)ApprovalStatusEnum.Agreed)
            {
                result = NodePassedResult.Create(NodePassedTypeEnum.Passed);
            }
            else if (approvalStatus == (short)ApprovalStatusEnum.Refused)
            {
                result = NodePassedResult.Create(NodePassedTypeEnum.NotPassed);
            }
            return result;
        }
    }
}
