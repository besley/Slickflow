using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Slickflow.Engine.Business.Entity
{
    /// <summary>
    /// 日志记录实体
    /// </summary>
    [Table("WfLog")]
    public class LogEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(Order = 0)]
        public int ID { get; set; }

        [Required]
        [Column(Order = 1)]
        public int EventTypeID { get; set; }

        [Required]
        [Column(Order = 2)]
        public int Priority { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)", Order = 3)]
        [MaxLength(50)]
        public string Severity { get; set; }

        [Required]
        [Column(TypeName = "varchar(256)", Order = 4)]
        [MaxLength(256)]
        public string Title { get; set; }

        [Column(TypeName = "varchar(500)", Order = 5)]
        [MaxLength(500)]
        public string Message { get; set; }

        [Column(TypeName = "varchar(4000)", Order = 6)]
        [MaxLength(4000)]
        public string StackTrace { get; set; }

        [Column(TypeName = "varchar(4000)", Order = 7)]
        [MaxLength(4000)]
        public string InnerStackTrace { get; set; }

        [Column(TypeName = "varchar(2000)", Order = 8)]
        [MaxLength(2000)]
        public string RequestData { get; set; }

        [Required]
        [Column(TypeName = "datetime", Order = 2)]
        public DateTime Timestamp { get; set; }
    }
}
