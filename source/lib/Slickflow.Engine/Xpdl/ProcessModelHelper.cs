using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Linq;
using Slickflow.Engine.Common;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Xpdl.Common;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Engine.Xpdl.Node;
using Slickflow.Engine.Utility;

namespace Slickflow.Engine.Xpdl
{
    /// <summary>
    /// Process Model Helper
    /// </summary>
    public class ProcessModelHelper
    {
        #region Activity Veiw Convertor
        /// <summary>
        /// Get start activity
        /// </summary>
        public static Activity GetStartActivity(Process process)
        {
            var activityList = process.ActivityList.ToList();
            var activity = activityList.SingleOrDefault<Activity>(a => a.ActivityType == ActivityTypeEnum.StartNode);
            return activity;
        }

        /// <summary>
        /// Get end activity
        /// </summary>
        public static Activity GetEndActivity(Process process)
        {
            var activityList = process.ActivityList.ToList();
            var activity = activityList.SingleOrDefault<Activity>(a => a.ActivityType == ActivityTypeEnum.EndNode);
            return activity;
        }

        /// <summary>
        /// Get activity
        /// </summary>
        public static Activity GetActivity(Process process, string activityID)
        {
            var activityList = process.ActivityList.ToList();
            var activity = activityList.SingleOrDefault<Activity>(a => a.ActivityID == activityID);
            return activity;
        }

        /// <summary>
        /// Get first activity
        /// </summary>
        public static Activity GetFirstActivity(Process process)
        {
            var activityList = process.ActivityList.ToList();
            var activity = activityList.SingleOrDefault<Activity>(a => a.ActivityType == ActivityTypeEnum.StartNode);

            var transitionList = process.TransitionList.ToList();
            var transition = transitionList.SingleOrDefault<Transition>(t=>t.FromActivityID == activity.ActivityID);
            var firstActivity = transition.ToActivity;

            return firstActivity;
        }

        /// <summary>
        /// Get Next Activity by from
        /// </summary>
        public static Activity GetNextActivity(Process process, string fromActivityID)
        {
            var transitionList = process.TransitionList.ToList();
            var transition = transitionList.SingleOrDefault<Transition>(t => t.FromActivityID == fromActivityID);
            var nextActivity = transition.ToActivity;

            return nextActivity;
        }

        /// <summary>
        /// Convert to activity view
        /// </summary>
        public static ActivityView ConvertFromActivityEntity(Activity entity)
        {
            var view = new ActivityView();
            view.ActivityID = entity.ActivityID;
            view.ActivityName = entity.ActivityName;
            view.ActivityCode = entity.ActivityCode;
            view.ActivityType = entity.ActivityType.ToString();
            if (entity.TriggerDetail != null)
            {
                view.TriggerType = entity.TriggerDetail.TriggerType.ToString();
                view.MessageDirection = entity.TriggerDetail.MessageDirection.ToString();
                view.Expression = entity.TriggerDetail.Expression;
            }
            return view;
        }
        #endregion

        #region Transition Convertor
        /// <summary>
        /// Get transition list
        /// </summary>
        public static IList<Transition> GetForwardTransitionList(Process process, string fromActivityID)
        {
            var transitionList = process.TransitionList.Where<Transition>(t => t.FromActivityID == fromActivityID).ToList();
            return transitionList;
        }

        /// <summary>
        /// Get transition
        /// </summary>
        public static Transition GetForwardTransition(Process process, string fromActivityID, string toActivityID)
        {
            var transiton = process.TransitionList.SingleOrDefault<Transition>(t => t.FromActivityID == fromActivityID
                && t.ToActivityID == toActivityID);
            return transiton;
        }

        /// <summary>
        /// Get backward transition list
        /// </summary>
        public static IList<Transition> GetBackwardTransitionList(Process process, string toActivityID)
        {
            var transitionList = process.TransitionList.Where<Transition>(t => t.ToActivityID == toActivityID).ToList();
            return transitionList;
        }
        #endregion

        #region Convert process property
        /// <summary>
        /// Convert to process file entity
        /// </summary>
        public static ProcessFileEntity ConvertFromXmlNodeProcess(XmlNode xmlNodeProcess)
        {
            var process = new ProcessFileEntity();
            process.ProcessID = XMLHelper.GetXmlAttribute(xmlNodeProcess, "id");
            process.ProcessName = XMLHelper.GetXmlAttribute(xmlNodeProcess, "name");
            if (string.IsNullOrEmpty(process.ProcessName)) process.ProcessName = process.ProcessID;

            process.ProcessCode = XMLHelper.GetXmlAttribute(xmlNodeProcess, "sf:code");
            process.Version = "1";
            process.IsUsing = 1;

            return process;
        }

        /// <summary>
        /// Convert to process
        /// </summary>
        public static Xpdl.Entity.Process ConvertToXPDLProcess(XmlDocument xmlDoc)
        {
            var root = xmlDoc.DocumentElement;
            var xmlNodeProcess = root.SelectSingleNode(XPDLDefinition.BPMN_StrXmlPath_Process,
                    XPDLHelper.GetSlickflowXmlNamespaceManager(xmlDoc));
            var process = ProcessModelConvertor.ConvertProcessModelFromXML(xmlNodeProcess);
            return process;
        }
        #endregion
    }
}
