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
    /// Process Validator
    /// </summary>
    internal class ProcessValidator
    {
        /// <summary>
        /// Validate
        /// </summary>
        internal static ProcessValidateResult Validate(ProcessEntity entity)
        {
            var result = new ProcessValidateResult();
            var xmlContent = entity.XmlContent;
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlContent);

            var root = xmlDoc.DocumentElement;
            var startEvent = XMLHelper.GetXmlNodeByXpath(xmlDoc, XPDLDefinition.BPMN_StrXmlPath_StartEvent,
                XPDLHelper.GetSlickflowXmlNamespaceManager(xmlDoc));
            var endEvent = XMLHelper.GetXmlNodeByXpath(xmlDoc, XPDLDefinition.BPMN_StrXmlPath_EndEvent,
                XPDLHelper.GetSlickflowXmlNamespaceManager(xmlDoc));

            //检验开始节点和结束节点
            //Check the start and end nodes
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
            //Check if there is a critical path from start to finish
            var process = ProcessModelHelper.ConvertToXPDLProcess(xmlDoc);

            var startActivityId = XMLHelper.GetXmlAttribute(startEvent, "id");
            var activityList = process.ActivityList;
            var transitionList = process.TransitionList;
            activityList = activityList.Where(a => a.ActivityId != startActivityId).ToList();

            var isExistedStartEndPath = TranverseTransitonList(process, activityList, transitionList, startActivityId);
            if (isExistedStartEndPath == false)
            {
                result.ProcessValidatedResultType = ProcessValidateResultTypeEnum.WithoutStartEndPath;
                return result;
            }

            result.ProcessValidatedResultType = ProcessValidateResultTypeEnum.Successed;
            return result;
        }

        /// <summary>
        /// Iterative verification method
        /// 迭代验证方法
        /// </summary>
        internal static Boolean TranverseTransitonList(Process process,
            IList<Activity> activityList,
            IList<Transition> transitionList,
            string activityId)
        {
            var list = transitionList.Where(t => t.FromActivityId == activityId).ToList();
            foreach (var t in list)
            {
                var toActivityId = t.ToActivityId;
                var activity = ProcessModelHelper.GetActivity(process, toActivityId);
                if (activity.ActivityType == Engine.Common.ActivityTypeEnum.EndNode)
                {
                    return true;
                }
                activityList = activityList.Where(a => a.ActivityId != toActivityId).ToList();
                var isStartEndExisted = TranverseTransitonList(process, activityList, transitionList, toActivityId);
                if (isStartEndExisted == true)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
