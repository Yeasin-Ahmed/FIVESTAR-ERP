using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.DTOModel
{
   public class RequsitionInfoDTO
    {
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
        public long WarehouseId { get; set; }
        public long LineId { get; set; }
        public long DescriptionId { get; set; }
        public bool IsBundle { get; set; }
        public long? ItemTypeId { get; set; }
        public long? ItemId { get; set; }
        public int? ForQty { get; set; }
        public long? UnitId { get; set; }
        [StringLength(100)]
        public string RequisitionFor { get; set; }
        public long? AssemblyLineId { get; set; }
        public long? PackagingLineId { get; set; }
        [StringLength(100)]
        public string WarehouseName { get; set; }
        [StringLength(100)]
        public string LineNumber { get; set; }
        public int? Qty { get; set; }
        [StringLength(100)]
        public string ModelName { get; set; }
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
        [StringLength(100)]
        public string PackagingLineName { get; set; }

        public List<RequisitionItemInfoDTO> RequisitionItemInfos { get; set; }
        public List<RequsitionDetailDTO> RequisitionDetails { get; set; }

    }
}
