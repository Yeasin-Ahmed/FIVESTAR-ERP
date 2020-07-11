using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.ViewModels
{
   public class VmReqDetails
    {
        public long ReqDetailId { get; set; }
        [Range(1,long.MaxValue)]
        public long? ItemTypeId { get; set; }
        [StringLength(100)]
        public string ItemType { get; set; }
        [Range(1, long.MaxValue)]
        public long? ItemId { get; set; }
        [StringLength(100)]
        public string Item { get; set; }
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
        public int IssueQty { get; set; }
        //[Range(1, long.MaxValue)]
        public long? UnitId { get; set; }
        [StringLength(100)]
        public string UnitSymbol { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public long ReqInfoId { get; set; }
    }
    public class VmReqInfo
    {
        public long ReqInfoId { get; set; }
        [Range(1, long.MaxValue)]
        public long? LineId { get; set; }
        public string LineNumber { get; set; }
        [Range(1, long.MaxValue)]
        public long? WarehouseId { get; set; }
        [StringLength(100)]
        public string WarehouseName { get; set; }
        [Range(1, long.MaxValue)]
        public long DescriptionId { get; set; }
        [StringLength(100)]
        public string ModelName { get; set; }
        [Required,StringLength(50)]
        public string RequisitionType { get; set; }
        public List<VmReqDetails> ReqDetails { get; set; }
        public long? ItemTypeId { get; set; }
        public long? ItemId { get; set; }
        public int? ForQty  { get; set; }
        public bool IsBundle { get; set; }
        [Required, StringLength(100)]
        public string RequisitionFor { get; set; }
    }
}
