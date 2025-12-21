using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// Node approval passed result
    /// 节点审批通过结果
    /// </summary>
    public class NodePassedResult
    {
        /// <summary>
        /// Node approval passed type
        /// 节点通过类型
        /// </summary>
        public NodePassedTypeEnum NodePassedType { get; set; }

        /// <summary>
        /// Constructor
        /// 构造函数
        /// </summary>
        /// <param name="passedType"></param>
        public NodePassedResult(NodePassedTypeEnum passedType)
        {
            this.NodePassedType = passedType;
        }

        /// <summary>
        /// Create method
        /// 创建方法
        /// </summary>
        /// <param name="passedType"></param>
        /// <returns></returns>
        public static NodePassedResult Create(NodePassedTypeEnum passedType)
        {
            var result = new NodePassedResult(passedType);
            return result;
        }

        /// <summary>
        /// Create result objects based on approval status
        /// 根据审批状态创建结果对象
        /// </summary>
        /// <param name="approvalStatus"></param>
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
