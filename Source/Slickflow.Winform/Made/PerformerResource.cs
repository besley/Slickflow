using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Winform
{
    /// <summary>
    /// 执行人员信息
    /// </summary>
    public class PerformerResource
    {
        public string UserID { get; set; }
        public string UserName { get; set; }

        public static IDictionary<string, string> Initialize()
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict["andun"] = "1";
            dict["yiran"] = "2";
            dict["limu"] = "3";
            dict["guangling"] = "4";

            return dict;
        }
    }
}
