using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERPBO.Production.DomainModels
{
   [Table("tblRequsitionInfo")]
   public class RequsitionInfo
    {
        [Key]
        public long ReqInfoId { get; set; }
        [StringLength(100)]
        public string ReqInfoCode { get; set; }
        [StringLength(100)]
        public string StateStatus { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }
        [StringLength(50)]
        public string RequisitionType { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public ICollection<RequsitionDetail> RequsitionDetails { get; set; }
        [StringLength(100)]
        public string RequisitionFor { get; set; }
        public long WarehouseId { get; set; }
        public long LineId { get; set; }
        public long DescriptionId { get; set; }
        public bool IsBundle { get; set; }
        public long? ItemTypeId { get; set; }
        public long? ItemId { get; set; }
        public long? UnitId { get; set; }
        public int? ForQty { get; set; }
        public ICollection<RequisitionItemInfo> RequisitionItemInfos { get; set; }
        public long? AssemblyLineId { get; set; }
        [StringLength(50)]
        public string Flag { get; set; }
    }
}
