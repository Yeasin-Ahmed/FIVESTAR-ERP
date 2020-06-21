using ERPBO.Production.DomainModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.DomainModels
{
    [Table("tblPackagingLine")]
    public class PackagingLine
    {
        [Key]
        public long PackagingLineId { get; set; }
        [StringLength(50)]
        public string PackagingLineName { get; set; }
        public bool IsActive { get; set; }
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        [ForeignKey("ProductionLine")]
        public long ProductionLineId { get; set; }
        public ProductionLine ProductionLine { get; set; }
    }
}
