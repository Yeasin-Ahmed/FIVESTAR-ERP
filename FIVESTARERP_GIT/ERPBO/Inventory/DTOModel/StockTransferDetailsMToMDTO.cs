using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Inventory.DTOModel
{
   public class StockTransferDetailsMToMDTO
    {
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
        public long STransferInfoId { get; set; }
        public string Warehouse { get; set; }
        public string FModelName { get; set; }
        public string TModelName { get; set; }
        public string ItemType { get; set; }
        public string Item { get; set; }
        public string Unit { get; set; }
        public string EntryUser { get; set; }
        public string UpdateUser { get; set; }
    }
}
