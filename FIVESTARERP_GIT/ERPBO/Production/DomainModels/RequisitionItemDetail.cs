using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.DomainModels
{
    [Table("tblRequisitionItemDetail")]
    public class RequisitionItemDetail
    {
        [Key]
        public long ReqItemDetailId { get; set; }
        public long? WarehouseId { get; set; }
        public long? ItemId { get; set; }
        public long? ItemTypeId { get; set; }
        public int? ConsumptionQty { get; set; }
        public int? TotalQuantity { get; set; }
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        [ForeignKey("RequisitionItemInfo")]
        public long ReqItemInfoId { get; set; }
        public RequisitionItemInfo RequisitionItemInfo { get; set; }
    }
}
