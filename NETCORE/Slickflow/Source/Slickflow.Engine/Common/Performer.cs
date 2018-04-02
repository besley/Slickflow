using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// 任务的执行者对象
    /// </summary>
    public class Performer
    {
        public Performer(string userID, string userName)
        {
            UserID = userID;
            UserName = userName;
        }

        public string UserID
        {
            get;
            set;
        }

        public string UserName
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 执行者列表类
    /// </summary>
    public class PerformerList : List<Performer>
    {
        public PerformerList()
        {
        }
    }
}
