using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.ViewModels
{
   public class RequsitionInfoViewModel
    {
        public long ReqInfoId { get; set; }
        [StringLength(100)]
        public string ReqInfoCode { get; set; }
        [StringLength(100)]
        public string StateStatus { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        [Range(1, long.MaxValue)]
        public long WarehouseId { get; set; }
        [Range(1, long.MaxValue)]
        public long LineId { get; set; }
        [Range(1, long.MaxValue)]
        public long DescriptionId { get; set; }
        public bool IsBundle { get; set; }
        public long? ItemTypeId { get; set; }
        public long? ItemId { get; set; }
        public int? ForQty { get; set; }
        public long? UnitId { get; set; }
        [StringLength(100)]
        public string RequisitionFor { get; set; }
        [Range(1,long.MaxValue)]
        public long? AssemblyLineId { get; set; }

        //Custom
        [StringLength(100)]
        public string WarehouseName { get; set; }
        [StringLength(100)]
        public string LineNumber { get; set; }
        public int? Qty { get; set; }
        [StringLength(100)]
        public string ModelName { get; set; }
        [Required,StringLength(50)]
        public string RequisitionType { get; set; }
        public int? TotalReqCount { get; set; }
        public string EntryUser { get; set; }
        public string UpdateUser { get; set; }
        [StringLength(100)]
        public string ItemTypeName { get; set; }
        [StringLength(100)]
        public string ItemName { get; set; }
        [StringLength(100)]
        public string UnitName { get; set; }
        [StringLength(100)]
        public string AssemblyLineName { get; set; }
        public List<RequisitionItemInfoViewModel> RequisitionItemInfos { get; set; }
        public List<RequsitionDetailViewModel> RequisitionDetails { get; set; }
    }
}
