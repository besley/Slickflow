using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Slickflow.Engine.Xpdl.Entity;
using System.Xml;
using Slickflow.Engine.Utility;
using IronPython.Compiler.Ast;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Xpdl.Common;

namespace Slickflow.Engine.Xpdl
{
    /// <summary>
    /// 流程校验器
    /// </summary>
    internal class ProcessValidator
    {
        /// <summary>
        /// 校验方法
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>校验结果</returns>
        internal static ProcessValidateResult Validate(ProcessEntity entity)
        {
            var result = new ProcessValidateResult();
            var xmlContent = entity.XmlContent;
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlContent);

            var root = xmlDoc.DocumentElement;
            var startEvent = XMLHelper.GetXmlNodeByXpath(xmlDoc, XPDLDefinition.BPMN2_StrXmlPath_StartEvent,
                XPDLHelper.GetSlickflowXmlNamespaceManager(xmlDoc));
            var endEvent = XMLHelper.GetXmlNodeByXpath(xmlDoc, XPDLDefinition.BPMN2_StrXmlPath_EndEvent,
                XPDLHelper.GetSlickflowXmlNamespaceManager(xmlDoc));

            //检验开始节点和结束节点
            if (startEvent == null)
            {
                result.ProcessValidatedResultType = ProcessValidateResultTypeEnum.WithoutStartEvent;
                return result;
            }
            else if (endEvent == null)
            {
                result.ProcessValidatedResultType = ProcessValidateResultTypeEnum.WithoutEndEvent;
                return result;
            }

            //检验开始到结束的关键路径是否存在
            var process = ProcessModelHelper.ConvertToXPDLProcess(xmlDoc);

            var startActivityGUID = XMLHelper.GetXmlAttribute(startEvent, "sf:guid");
            var activityList = process.ActivityList;
            var transitionList = process.TransitionList;
            activityList = activityList.Where(a => a.ActivityGUID != startActivityGUID).ToList();

            var isExistedStartEndPath = TranverseTransitonList(process, activityList, transitionList, startActivityGUID);
            if (isExistedStartEndPath == false)
            {
                result.ProcessValidatedResultType = ProcessValidateResultTypeEnum.WithoutStartEndPath;
                return result;
            }

            result.ProcessValidatedResultType = ProcessValidateResultTypeEnum.Successed;
            return result;
        }

        /// <summary>
        /// 迭代验证方法
        /// </summary>
        /// <param name="process">流程实体</param>
        /// <param name="activityList">活动列表</param>
        /// <param name="transitionList">转移列表</param>
        /// <param name="activityGUID">活动GUID</param>
        /// <returns>是否存在关键路径</returns>
        internal static Boolean TranverseTransitonList(Process process,
            IList<Activity> activityList,
            IList<Transition> transitionList,
            string activityGUID)
        {
            var list = transitionList.Where(t => t.FromActivityGUID == activityGUID).ToList();
            foreach (var t in list)
            {
                var toActivityGUID = t.ToActivityGUID;
                var activity = ProcessModelHelper.GetActivity(process, toActivityGUID);
                if (activity.ActivityType == Engine.Common.ActivityTypeEnum.EndNode)
                {
                    return true;
                }
                activityList = activityList.Where(a => a.ActivityGUID != toActivityGUID).ToList();
                var isStartEndExisted = TranverseTransitonList(process, activityList, transitionList, toActivityGUID);
                if (isStartEndExisted == true)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
