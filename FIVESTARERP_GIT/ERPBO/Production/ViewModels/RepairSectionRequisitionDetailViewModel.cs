using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.ViewModels
{
    public class RepairSectionRequisitionDetailViewModel
    {
        public long RSRDetailId { get; set; }
        [StringLength(100)]
        public string RequisitionCode { get; set; }
        //[Range(1, long.MaxValue)]
        public long? RepairLineId { get; set; }
        [StringLength(100)]
        public string RepairLineName { get; set; }
        public long? WarehouseId { get; set; }
        [StringLength(100)]
        public string WarehouseName { get; set; }
        [Range(1, long.MaxValue)]
        public long? ItemTypeId { get; set; }
        [StringLength(100)]
        public string ItemTypeName { get; set; }
        [Range(1, long.MaxValue)]
        public long? ItemId { get; set; }
        [StringLength(100)]
        public string ItemName { get; set; }
        [Range(1, long.MaxValue)]
        public long? UnitId { get; set; }
        [StringLength(100)]
        public string UnitName { get; set; }
        [Range(1, int.MaxValue)]
        public int RequestQty { get; set; }
        public int IssueQty { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public long RSRInfoId { get; set; }
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
        public int AvailableQty { get; set; }
    }
}
