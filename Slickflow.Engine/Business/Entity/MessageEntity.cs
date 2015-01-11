using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Business.Entity
{
    [Table("WfMessage")]
    public class MessageEntity
    {
        internal long ID
        {
            get;
            set;
        }

        internal string Title
        {
            get;
            set;
        }

        internal string MessageContent
        {
            get;
            set;
        }

        internal long AppInstanceID
        {
            get;
            set;
        }

        internal long ProcessInstanceID
        {
            get;
            set;
        }

        internal long ActivityInstanceID
        {
            get;
            set;
        }

        internal long SendToUserID
        {
            get;
            set;
        }

        internal string SendToUserName
        {
            get;
            set;
        }
    }
}
