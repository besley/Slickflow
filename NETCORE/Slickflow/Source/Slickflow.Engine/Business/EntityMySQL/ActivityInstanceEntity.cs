using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Slickflow.Engine.Business.Entity
{
    /// <summary>
    /// 活动实例的实体对象
    /// </summary>
    [Table("WfActivityInstance")]
    public class ActivityInstanceEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(Order = 0)]
        public int ID { get; set; }

        [Required]
        [Column(Order = 1)]
        public int ProcessInstanceID { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)", Order = 2)]
        [MaxLength(50)]
        public string AppName { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)", Order = 3)]
        [MaxLength(50)]
        public string AppInstanceID { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)", Order = 4)]
        [MaxLength(100)]
        public string ProcessGUID { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)", Order = 5)]
        [MaxLength(100)]
        public string ActivityGUID { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)", Order = 6)]
        [MaxLength(50)]
        public string ActivityName { get; set; }

        [Required]
        [Column(Order = 7)]
        public short ActivityType { get; set; }

        [Required]
        [Column(Order = 8)]
        public short ActivityState { get; set; }

        [Required]
        [Column(Order = 9)]
        public short WorkItemType { get; set; }

        [Column(TypeName = "varchar(1000)", Order = 10)]
        [MaxLength(1000)]
        public string AssignedToUserIDs { get; set; }

        [Column(TypeName = "varchar(2000)", Order = 11)]
        [MaxLength(2000)]
        public string AssignedToUserNames { get; set; }

        [Column(Order = 12)]
        public short BackwardType { get; set; }

        [Column(Order = 13)]
        public Nullable<int> BackSrcActivityInstanceID { get; set; }

        [Column(Order = 14)]
        public Nullable<short> GatewayDirectionTypeID { get; set; }

        [Required]
        [Column(Order = 15)]
        public byte CanRenewInstance { get; set; }

        [Required]
        [Column(Order = 16)]
        public int TokensRequired { get; set; }

        [Required]
        [Column(Order = 17)]
        public int TokensHad { get; set; }

        [Column(Order = 18)]
        public Nullable<short> ComplexType { get; set; }

        [Column(Order = 19)]
        public Nullable<short> MergeType { get; set; }

        [Column(Order = 20)]
        public Nullable<int> MIHostActivityInstanceID { get; set; }

        [Column(Order = 21)]
        public Nullable<short> CompareType { get; set; }

        [Column(Order = 22)]
        public Nullable<double> CompleteOrder { get; set; }

        [Column(Order = 23)]
        public Nullable<short> SignForwardType { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)", Order = 24)]
        [MaxLength(50)]
        public string CreatedByUserID { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)", Order = 25)]
        [MaxLength(50)]
        public string CreatedByUserName { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column(TypeName = "datetime", Order = 26)]
        public System.DateTime CreatedDateTime { get; set; }

        [Column(TypeName = "varchar(50)", Order = 27)]
        [MaxLength(50)]
        public string LastUpdatedByUserID { get; set; }

        [Column(TypeName = "varchar(50)", Order = 28)]
        [MaxLength(50)]
        public string LastUpdatedByUserName { get; set; }

        [Column(TypeName = "datetime", Order = 29)]
        public Nullable<System.DateTime> LastUpdatedDateTime { get; set; }

        [Column(TypeName = "datetime", Order = 30)]
        public Nullable<System.DateTime> EndedDateTime { get; set; }

        [Column(TypeName = "varchar(50)", Order = 31)]
        [MaxLength(50)]
        public string EndedByUserID { get; set; }

        [Column(TypeName = "varchar(50)", Order = 32)]
        [MaxLength(50)]
        public string EndedByUserName { get; set; }

        [Required]
        [Column(Order = 33)]
        public byte RecordStatusInvalid { get; set; }

        [Timestamp]
        [Column(TypeName = "timestamp", Order = 34)]
        public DateTime RowVersionID { get; set; }
    }
}
