using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Inventory.ViewModels
{
    public class IQCItemReqDetailListViewModel
    {
        public long IQCItemReqDetailId { get; set; }
        public long? ItemTypeId { get; set; }
        public long? ItemId { get; set; }
        public long? UnitId { get; set; }
        public decimal Quantity { get; set; }
        public decimal IssueQty { get; set; }
        public long OrganizationId { get; set; }
        public long IQCItemReqInfoId { get; set; }
        public long? IQCId { get; set; }
        public long? WarehouseId { get; set; }
        public int? AvailableQty { get; set; }
        public long? SupplierId { get; set; }
        public long? DescriptionId { get; set; }
        public string Remarks { get; set; }
        public string ItemType { get; set; }
        public string Item { get; set; }
        public string Unit { get; set; }
        public string EntryUser { get; set; }
        public string StateStatus { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
    }
}
