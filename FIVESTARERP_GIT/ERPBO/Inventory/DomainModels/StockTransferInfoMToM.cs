using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Inventory.DomainModels
{
    [Table("tblStockTransferInfoMToM")]
   public class StockTransferInfoMToM
    {
        [Key]
        public long STransferInfoId { get; set; }
        public long? WarehouseId { get; set; }
        public long? FromDescriptionId { get; set; }
        public long? ToDescriptionId { get; set; }
        public string TransferCode { get; set; }
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public ICollection<StockTransferDetailsMToM> stockTransferDetailsMToM { get; set; }

    }
}
