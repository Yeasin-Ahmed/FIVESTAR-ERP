using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.DomainModels
{
    [Table("tblRepairItemProblem")]
    public class RepairItemProblem
    {
        [Key]
        public long RIProblemId { get; set; }
        [StringLength(100)]
        public string ReferenceNumber { get; set; }
        public long QRCodeId { get; set; }
        [StringLength(100)]
        public string QRCode { get; set; }
        public long ProblemId { get; set; }
        [StringLength(200)]
        public string Problem { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        [ForeignKey("RepairItem")]
        public long RepairItemId { get; set; }
        public RepairItem RepairItem { get; set; }
    }
}
