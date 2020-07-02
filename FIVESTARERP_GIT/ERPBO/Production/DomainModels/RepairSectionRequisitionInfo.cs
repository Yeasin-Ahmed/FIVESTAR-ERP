using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.DomainModels
{
    [Table("tblRepairSectionRequisitionInfo")]
    public class RepairSectionRequisitionInfo
    {
        [Key]
        public long RSRInfoId { get; set; }
        [StringLength(100)]
        public string RequisitionCode { get; set; }
        public long?  ProductionFloorId { get; set; }
        [StringLength(100)]
        public string ProductionFloorName { get; set; }
        public long? RepairLineId { get; set; }
        [StringLength(100)]
        public string RepairLineName { get; set; }
        public long? DescriptionId { get; set; }
        [StringLength(100)]
        public string ModelName { get; set; }
        public long? WarehouseId { get; set; }
        [StringLength(100)]
        public string WarehouseName { get; set; }
        public int TotalUnitQty { get; set; }
        public int IssueUnitQty { get; set; }
        [StringLength(100)]
        public string StateStatus { get; set; }
        public long OrganizationId { get; set; }
        public long? ApprovedBy { get; set; }
        public Nullable<DateTime> ApprovedDate { get; set; }
        public long? RecheckedBy { get; set; }
        public Nullable<DateTime> RecheckedDate { get; set; }
        public long? RejectedBy { get; set; }
        public Nullable<DateTime> RejectedDate { get; set; }
        public long? CanceledBy { get; set; }
        public Nullable<DateTime> CanceledDate { get; set; }
        public long? ReceivedBy { get; set; }
        public Nullable<DateTime> ReceivedDate { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public ICollection<RepairSectionRequisitionDetail> RepairSectionRequisitionDetails { get; set; }

    }
}
