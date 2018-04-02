using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Engine.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Slickflow.Engine.Business.Entity
{
    /// <summary>
    /// 任务实体对象
    /// </summary>
    [Table("WfTasks")]
    public class TaskEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(Order = 0)]
        public int ID { get; set; }

        [Required]
        [Column(Order = 1)]
        public int ActivityInstanceID { get; set; }

        [Required]
        [Column(Order = 2)]
        public int ProcessInstanceID { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)", Order = 3)]
        [MaxLength(50)]
        public string AppName { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)", Order = 4)]
        [MaxLength(50)]
        public string AppInstanceID { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)", Order = 5)]
        [MaxLength(100)]
        public string ProcessGUID { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)", Order = 6)]
        [MaxLength(100)]
        public string ActivityGUID { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)", Order = 7)]
        [MaxLength(50)]
        public string ActivityName { get; set; }

        [Required]
        [Column(Order = 8)]
        public short TaskType { get; set; }

        [Required]
        [Column(Order = 9)]
        public short TaskState { get; set; }

        [Column(Order = 10)]
        public Nullable<int> EntrustedTaskID { get; set; }        //被委托任务ID

        [Required]
        [Column(TypeName = "varchar(50)", Order = 11)]
        [MaxLength(50)]
        public string AssignedToUserID { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)", Order = 12)]
        [MaxLength(50)]
        public string AssignedToUserName { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)", Order = 13)]
        [MaxLength(50)]
        public string CreatedByUserID { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)", Order = 14)]
        [MaxLength(50)]
        public string CreatedByUserName { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column(TypeName = "datetime", Order = 15)]
        public System.DateTime CreatedDateTime { get; set; }

        [Column(TypeName = "datetime", Order = 16)]
        public Nullable<System.DateTime> LastUpdatedDateTime { get; set; }

        [Column(TypeName = "varchar(50)", Order = 17)]
        [MaxLength(50)]
        public string LastUpdatedByUserID { get; set; }

        [Column(TypeName = "varchar(50)", Order = 18)]
        [MaxLength(50)]
        public string LastUpdatedByUserName { get; set; }

        [Column(TypeName = "varchar(50)", Order = 19)]
        [MaxLength(50)]
        public string EndedByUserID { get; set; }

        [Column(TypeName = "varchar(50)", Order = 20)]
        [MaxLength(50)]
        public string EndedByUserName { get; set; }

        [Column(TypeName = "datetime", Order = 21)]
        public Nullable<System.DateTime> EndedDateTime { get; set; }

        [Required]
        [Column(Order = 22)]
        public byte RecordStatusInvalid { get; set; }

        [Timestamp]
        [Column(TypeName = "timestamp", Order = 23)]
        public DateTime RowVersionID { get; set; }
    }
}
