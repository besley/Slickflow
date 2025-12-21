using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.BizAppService.Entity
{
    /// <summary>
    /// Application flow record
    /// 流转记录实体对象
    /// </summary>
    [Table("biz_app_flow")]
    public class AppFlowEntity
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("app_name")]
        public string AppName { get; set; }

        [Column("app_instance_id")]
        public string AppInstanceId { get; set; }

        [Column("app_instance_code")]
        public string AppInstanceCode { get; set; }

        [Column("activity_name")]
        public string ActivityName { get; set; }

        [Column("remark")]
        public string Remark { get; set; }

        [Column("changed_time")]
        public DateTime ChangedTime { get; set; }

        [Column("changed_user_id")]
        public string ChangedUserId { get; set; }

        [Column("changed_user_name")]
        public string ChangedUserName { get; set; }
    }
}
