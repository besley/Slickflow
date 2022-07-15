using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.HrsService.Entity
{
    /// <summary>
    /// 流转记录实体对象
    /// </summary>
    [Table("BizAppFlow")]
    public class AppFlowEntity
    {
        public int ID
        {
            get;
            set;
        }

        public string AppName
        {
            get;
            set;
        }

        public string AppInstanceID
        {
            get;
            set;
        }

        public string AppInstanceCode
        {
            get;
            set;
        }

        public string ActivityName
        {
            get;
            set;
        }

        public string Remark
        {
            get;
            set;
        }

        public DateTime ChangedTime
        {
            get;
            set;
        }

        public string ChangedUserID
        {
            get;
            set;
        }

        public string ChangedUserName
        {
            get;
            set;
        }
    }
}
