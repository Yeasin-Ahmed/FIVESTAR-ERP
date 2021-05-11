using ERPBO.Production.DomainModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.ViewModels
{
   public class RepairSectionSemiFinishTransferInfoViewModel
    {
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

        // Custom Properties
        [StringLength(100)]
        public string FloorName { get; set; }
        [StringLength(100)]
        public string QCLineName { get; set; }
        [StringLength(100)]
        public string RepairLineName { get; set; }
        [StringLength(100)]
        public string AssemblyLineName { get; set; }
        [StringLength(100)]
        public string ModelName { get; set; }
        [StringLength(100)]
        public string WarehouseName { get; set; }
        [StringLength(100)]
        public string ItemTypeName { get; set; }
        [StringLength(100)]
        public string ItemName { get; set; }
        [StringLength(50)]
        public string EntryUser { get; set; }
        [StringLength(50)]
        public string UpdateUser { get; set; }
    }
}
