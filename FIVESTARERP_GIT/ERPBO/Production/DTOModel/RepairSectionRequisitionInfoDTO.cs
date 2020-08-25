using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.DTOModel
{
    public class RepairSectionRequisitionInfoDTO
    {
        public long RSRInfoId { get; set; }
        [StringLength(100)]
        public string RequisitionCode { get; set; }
        public long? ProductionFloorId { get; set; }
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
        public long? CheckedBy { get; set; }
        public Nullable<DateTime> CheckedDate { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public long? HandOverId { get; set; }
        public Nullable<DateTime> HandOverDate { get; set; }
        public ICollection<RepairSectionRequisitionDetailDTO> RepairSectionRequisitionDetails { get; set; }

        // Newly Added //
        [StringLength(50)]
        public string ReqFor { get; set; }
        public long? PackagingLineId { get; set; }
        [StringLength(100)]
        public string PackagingLineName { get; set; }

        // Custom Properties
        [StringLength(100)]
        public string EntryUser { get; set; }
        [StringLength(100)]
        public string UpdateUser { get; set; }
        [StringLength(100)]
        public string ApproveUser { get; set; }
        [StringLength(100)]
        public string RecheckUser { get; set; }
        [StringLength(100)]
        public string RejectUser { get; set; }
        [StringLength(100)]
        public string CancelUser { get; set; }
        [StringLength(100)]
        public string ReceiveUser { get; set; }
        [StringLength(100)]
        public string CheckUser { get; set; }
        [StringLength(100)]
        public string HandOverUser { get; set; }
    }
}
