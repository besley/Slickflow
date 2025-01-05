using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// Key value pair wrapper for Webapi
    /// </summary>
    public class KeyValuePairWrapper
    {
        /// <summary>
        /// ActivityGUID
        /// </summary>
        public string ActivityGUID { get; set; }

        /// <summary>
        /// Performer List
        /// </summary>
        public PerformerList PerformerList { get; set; }

        /// <summary>
        /// default constructor will be required for binding, the Web.MVC binder will invoke this and set the Key and Value accordingly.
        /// </summary>
        public KeyValuePairWrapper() { }

        /// <summary>
        /// a convenience method which allows you to set the values while sorting
        /// </summary>
        /// <param name="activityGUID"></param>
        /// <param name="performerList"></param>
        public KeyValuePairWrapper(string activityGUID, PerformerList performerList)
        {
            ActivityGUID = activityGUID;
            PerformerList = performerList;
        }
    }
}
