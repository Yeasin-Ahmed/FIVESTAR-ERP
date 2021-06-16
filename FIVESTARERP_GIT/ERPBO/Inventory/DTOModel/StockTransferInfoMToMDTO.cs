using ERPBO.Inventory.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Inventory.DTOModel
{
   public class StockTransferInfoMToMDTO
    {
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
        public string Warehouse { get; set; }

        public string FromModel { get; set; }
        public string ToModel { get; set; }
        public string EntryUser { get; set; }
        public string UpdateUser { get; set; }
        public ICollection<StockTransferDetailsMToM> stockTransferDetailsMToM { get; set; }
    }
}
