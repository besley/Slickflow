using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Slickflow.Engine.Business.Entity
{
    /// <summary>
    /// 节点转移类
    /// </summary>
    [Table("WfTransitionInstance")]
    public class TransitionInstanceEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(Order = 0)]
        public int ID { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)", Order = 1)]
        [MaxLength(100)]
        public string TransitionGUID { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)", Order = 2)]
        [MaxLength(50)]
        public string AppName { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)", Order = 3)]
        [MaxLength(50)]
        public string AppInstanceID { get; set; }

        [Required]
        [Column(Order = 4)]
        public int ProcessInstanceID { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)", Order = 5)]
        [MaxLength(100)]
        public string ProcessGUID { get; set; }

        [Required]
        [Column(Order = 6)]
        public byte TransitionType { get; set; }

        [Required]
        [Column(Order = 7)]
        public byte FlyingType { get; set; }

        [Required]
        [Column(Order = 8)]
        public int FromActivityInstanceID { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)", Order = 9)]
        [MaxLength(100)]
        public string FromActivityGUID { get; set; }

        [Required]
        [Column(Order = 10)]
        public short FromActivityType { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)", Order = 11)]
        [MaxLength(50)]
        public string FromActivityName { get; set; }

        [Required]
        [Column(Order = 12)]
        public int ToActivityInstanceID { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)", Order = 13)]
        [MaxLength(100)]
        public string ToActivityGUID { get; set; }

        [Required]
        [Column(Order = 14)]
        public short ToActivityType { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)", Order = 15)]
        [MaxLength(50)]
        public string ToActivityName { get; set; }

        [Required]
        [Column(Order = 16)]
        public byte ConditionParseResult { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)", Order = 17)]
        [MaxLength(50)]
        public string CreatedByUserID { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)", Order = 18)]
        [MaxLength(50)]
        public string CreatedByUserName { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column(TypeName = "datetime", Order = 19)]
        public System.DateTime CreatedDateTime { get; set; }

        [Required]
        [Column(Order = 20)]
        public byte RecordStatusInvalid { get; set; }

        [Timestamp]
        [Column(TypeName = "timestamp", Order = 21)]
        public DateTime RowVersionID { get; set; }
    }
}
