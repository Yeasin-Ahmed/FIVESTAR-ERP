using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERPBO.Production.DomainModels
{
    [Table("tblProductionToPackagingStockTransferDetail")]
    public class ProductionToPackagingStockTransferDetail
    {
        [Key]
        public long PTPSTDetailId { get; set; }
        [StringLength(100)]
        public string TransferCode { get; set; }
        public long FloorId { get; set; }
        public long PackagingLineId { get; set; }
        public long ModelId { get; set; }
        public long WarehouseId { get; set; }
        public long ItemTypeId { get; set; }
        public long ItemId { get; set; }
        public int Quantity { get; set; }
        [StringLength(100)]
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        [ForeignKey("ProductionToPackagingStockTransferInfo")]
        public long PTPSTInfoId { get; set; }
        public ProductionToPackagingStockTransferInfo ProductionToPackagingStockTransferInfo { get; set; }
    }
}
