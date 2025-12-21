using MySqlX.XDevAPI.CRUD;
using Slickflow.Engine.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Engine.Xpdl.Schedule;
using Slickflow.Engine.Xpdl;

namespace Slickflow.Engine.Core.Pattern
{
    /// <summary>
    /// Create Activity Resource
    /// </summary>
    internal class ActivityResourceFactory
    {
        internal static ActivityResource Create(WfAppRunner runner,
            IDictionary<string, PerformerList> nextActivityPerformers,
            IDictionary<string, string> conditionValuePair = null)
        {
            var activityResource = new ActivityResource(runner, nextActivityPerformers, conditionValuePair);

            return activityResource;
        }

        internal static ActivityResource Create(IProcessModel processModel,
            NextActivityComponent root, 
            WfAppRunner runner,
            IDictionary<string, string> conditionValuePair)
        {
            var nextActivityPerformers = GetActivityPerformerList(processModel, root, runner, conditionValuePair);
            var activityResource = new ActivityResource(runner, nextActivityPerformers, conditionValuePair);

            return activityResource;
        }

        private static IDictionary<string, PerformerList> GetActivityPerformerList(IProcessModel processModel,
            NextActivityComponent root,
            WfAppRunner runner,
            IDictionary<string, string> conditionValuePair)
        {
            IDictionary<string, PerformerList> nextActivityPerformers = new Dictionary<string, PerformerList>();
            foreach (NextActivityComponent comp in root)
            {
                IDictionary<string, PerformerList> itemActivityPerformers = new Dictionary<string, PerformerList>();
                if (comp.HasChildren)
                {
                    itemActivityPerformers = GetActivityPerformerList(processModel, comp, runner, conditionValuePair);
                }
                else
                {
                    itemActivityPerformers = processModel.GetActivityPerformers(comp.Activity.ActivityId);
                    foreach (var item in itemActivityPerformers)
                    {
                        if (item.Value.Count == 0)
                        {
                            item.Value.Add(new Performer(runner.UserId, runner.UserName));
                        }
                    }
                }
                Append(nextActivityPerformers, itemActivityPerformers);
            }
            return nextActivityPerformers;
        }

        private static void Append(IDictionary<string, PerformerList> orginalPerformerList,
            IDictionary<string, PerformerList> newPerformerList)
        {
            foreach (var item in newPerformerList)
            {
                if (!orginalPerformerList.ContainsKey(item.Key))
                {
                    orginalPerformerList[item.Key] = item.Value;
                }
            }
        }
    }
}
