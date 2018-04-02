using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Slickflow.Engine.Business.Entity
{
    /// <summary>
    /// 流程实例类
    /// </summary>
    [Table("WfProcessInstance")]
    public class ProcessInstanceEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(Order = 0)]
        public int ID { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)", Order = 1)]
        [MaxLength(100)]
        public string ProcessGUID { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)", Order = 2)]
        [MaxLength(50)]
        public string ProcessName { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(20)", Order = 3)]
        [MaxLength(20)]
        public string Version { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)", Order = 4)]
        [MaxLength(50)]
        public string AppName { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)", Order = 5)]
        [MaxLength(50)]
        public string AppInstanceID { get; set; }

        [Column(TypeName = "varchar(50)", Order = 6)]
        [MaxLength(50)]
        public string AppInstanceCode { get; set; }

        [Column(Order = 7)]
        public short ProcessState { get; set; }

        [Column(Order = 8)]
        public Nullable<int> ParentProcessInstanceID { get; set; }

        [Column(TypeName = "varchar(100)", Order = 9)]
        [MaxLength(100)]
        public string ParentProcessGUID { get; set; }

        [Column(Order = 10)]
        public int InvokedActivityInstanceID { get; set; }

        [Column(TypeName = "varchar(100)", Order = 11)]
        [MaxLength(100)]
        public string InvokedActivityGUID { get; set; }

        [Required]
        [Column(TypeName = "datetime2", Order = 12)]
        public DateTime CreatedDateTime { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)", Order = 13)]
        [MaxLength(50)]
        public string CreatedByUserID { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)", Order = 14)]
        [MaxLength(50)]
        public string CreatedByUserName { get; set; }

        [Column(TypeName = "datetime2", Order = 15)]
        public Nullable<DateTime> OverdueDateTime { get; set; }

        [Column(TypeName = "datetime2", Order = 16)]
        public Nullable<DateTime> OverdueTreatedDateTime { get; set; }

        [Column(TypeName = "datetime2", Order = 17)]
        public Nullable<DateTime> LastUpdatedDateTime { get; set; }

        [Column(TypeName = "varchar(50)", Order = 18)]
        [MaxLength(50)]
        public string LastUpdatedByUserID { get; set; }

        [Column(TypeName = "nvarchar(50)", Order = 19)]
        public string LastUpdatedByUserName { get; set; }

        [Column(TypeName = "datetime2", Order = 20)]
        public Nullable<DateTime> EndedDateTime { get; set; }

        [Column(TypeName = "varchar(50)", Order = 21)]
        [MaxLength(50)]
        public string EndedByUserID { get; set; }

        [Column(TypeName = "nvarchar(50)", Order = 22)]
        [MaxLength(50)]
        public string EndedByUserName { get; set; }

        [Required]
        [Column(Order = 23)]
        public byte RecordStatusInvalid { get; set; }

        [Timestamp]
        [Column(TypeName = "timestamp", Order = 24)]
        public byte[] RowVersionID { get; set; }
    }
}
