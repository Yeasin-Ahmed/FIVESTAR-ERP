using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.DomainModels
{
    [Table("tblQRCodeProblem")]
    public class QRCodeProblem
    {
        public long QRProbId { get; set; }
        public long FloorId { get; set; }
        public long QCLineId { get; set; }
        public long RepairLineId { get; set; }
        [StringLength(100)]
        public string QRCode { get; set; }
        public long AssemblyLineId { get; set; }
        public long DescriptionId { get; set; }
        public long ProblemId { get; set; }
        [StringLength(250)]
        public string ProblemName { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        [ForeignKey("QRCodeTransferToRepairInfo")]
        public long QRTRInfoId { get; set; }
        public QRCodeTransferToRepairInfo QRCodeTransferToRepairInfo { get; set; }
    }
}
