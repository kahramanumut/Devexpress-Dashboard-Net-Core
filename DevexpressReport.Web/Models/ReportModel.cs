using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevexpressReport.Web.Models
{
    [Table("_MR_Dashboards")]
    public partial class ReportModel
    {
        [Column("ID")]
        public int Id { get; set; }
        public byte[] Dashboard { get; set; }
        [StringLength(255)]
        public string Caption { get; set; }
    }
}
