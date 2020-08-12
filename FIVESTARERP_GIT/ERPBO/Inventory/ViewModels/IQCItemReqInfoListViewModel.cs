using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Inventory.ViewModels
{
    public class IQCItemReqInfoListViewModel
    {
        public long IQCItemReqInfoId { get; set; }
        public string IQCReqCode { get; set; }
        public long? IQCId { get; set; }
        public long? WarehouseId { get; set; }
        public long? DescriptionId { get; set; }
        public long? SupplierId { get; set; }
        public string Remarks { get; set; }
        public string StateStatus { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public string Warehouse { get; set; }
        public string ModelName { get; set; }
        public string IQCName { get; set; }
        public string Supplier { get; set; }
        public int? AvailableQty { get; set; }
        public string EntryUser { get; set; }
        public List<IQCItemReqDetailListViewModel> IQCItemReqDetails { get; set; }
    }
}
