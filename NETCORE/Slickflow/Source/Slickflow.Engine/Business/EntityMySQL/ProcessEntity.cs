using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Slickflow.Engine.Business.Entity
{
    /// <summary>
    /// 流程实体类
    /// </summary>
    [Table("WfProcess")]
    public class ProcessEntity
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
        [Column(TypeName = "varchar(50)", Order = 2)]
        [MaxLength(50)]
        public string ProcessName { get; set; }

        [Required]
        [Column(TypeName ="varchar(20)", Order = 3)]
        [MaxLength(20)]
        public string Version { get; set; }

        [Required]
        [Column(Order = 4)]
        public byte IsUsing { get; set; }

        [Column(TypeName = "varchar(20)", Order = 5)]
        [MaxLength(20)]
        public string AppType { get; set; }

        [Column(TypeName = "varchar(100)", Order = 6)]
        [MaxLength(100)]
        public string PageUrl { get; set; }

        [Column(TypeName = "varchar(50)", Order = 7)]
        [MaxLength(50)]
        public string XmlFileName { get; set; }

        [Column(TypeName = "varchar(50)", Order = 8)]
        [MaxLength(50)]
        public string XmlFilePath { get; set; }

        [Column(TypeName = "text", Order = 9)]
        public string XmlContent { get; set; }

        [Required]
        [Column(Order = 10)]
        public byte StartType { get; set; }

        [Column(TypeName = "varchar(100)", Order = 11)]
        [MaxLength(100)]
        public string StartExpression { get; set; }

        [Required]
        [Column(Order = 12)]
        public byte EndType { get; set; }

        [Column(TypeName = "varchar(100)", Order = 13)]
        [MaxLength(100)]
        public string EndExpression { get; set; }

        [Column(TypeName ="varchar(1000)", Order = 14)]
        [MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "datetime", Order = 15)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedDateTime { get; set; }

        [Column(TypeName = "datetime", Order = 16)]
        public Nullable<DateTime> LastUpdatedDateTime { get; set; }

        [Timestamp]
        [Column(TypeName = "timestamp", Order = 18)]
        public DateTime RowVersionID { get; set; }
    }
}
