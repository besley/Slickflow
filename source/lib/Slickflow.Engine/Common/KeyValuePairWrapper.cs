using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// 序列化Key-Value(WebAPI)
    /// </summary>
    public class KeyValuePairWrapper
    {
        public string ActivityGUID { get; set; }
        public PerformerList PerformerList { get; set; }

        //default constructor will be required for binding, the Web.MVC binder will invoke this and set the Key and Value accordingly.
        public KeyValuePairWrapper() { }

        //a convenience method which allows you to set the values while sorting
        public KeyValuePairWrapper(string activityGUID, PerformerList performerList)
        {
            ActivityGUID = activityGUID;
            PerformerList = performerList;
        }
    }
}
