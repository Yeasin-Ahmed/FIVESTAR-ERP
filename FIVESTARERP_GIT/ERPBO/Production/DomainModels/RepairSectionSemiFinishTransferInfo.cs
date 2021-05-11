using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.DomainModels
{
    [Table("tblRepairSectionSemiFinishTransferInfo")]
   public class RepairSectionSemiFinishTransferInfo
    {
        [Key]
        public long TransferInfoId { get; set; }
        public string TransferCode { get; set; }
        public string StateStatus { get; set; }
        public long? FloorId { get; set; }
        public long? QCLineId { get; set; }
        public long? RepairLineId { get; set; }
        public long? AssemblyLineId { get; set; }
        public long? DescriptionId { get; set; }
        public long? WarehouseId { get; set; }
        public int Qty { get; set; }
        public string Remarks { get; set; }
        public string Flag { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public ICollection<RepairSectionSemiFinishTransferDetails> RepairSectionSemiFinishTransferDetails { get; set; }

    }
}
