using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Inventory.DomainModels
{
    [Table("tblStockTransferDetailsMToM")]
   public class StockTransferDetailsMToM
    {
        [Key]
        public long STransferDetailId { get; set; }
        public long? WarehouseId { get; set; }
        public long? FromDescriptionId { get; set; }
        public long? ToDescriptionId { get; set; }
        public long? ItemTypeId { get; set; }
        public long? ItemId { get; set; }
        public long? UnitId { get; set; }
        public string StockStatus { get; set; }
        public string RefferenceNumber { get; set; }
        public int Quantity { get; set; }
        public Nullable<DateTime> ExpireDate { get; set; }
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        [ForeignKey("StockTransferInfoMToM")]
        public long STransferInfoId { get; set; }
        public StockTransferInfoMToM StockTransferInfoMToM { get; set; }
    }
}
