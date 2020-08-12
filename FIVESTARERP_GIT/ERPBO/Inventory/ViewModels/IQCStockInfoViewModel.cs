using ERPBO.Inventory.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Inventory.ViewModels
{
    public class IQCStockInfoViewModel
    {
        public long StockInfoId { get; set; }
        public long? WarehouseId { get; set; }
        public long? DescriptionId { get; set; }
        public long? ItemTypeId { get; set; }
        public long? ItemId { get; set; }
        public long? UnitId { get; set; }
        public int? StockInQty { get; set; }
        public int? StockOutQty { get; set; }
        public string StockType { get; set; }
        public long? SupplierId { get; set; }
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        //Custome
        public string WarehouseName { get; set; }
        public string ModelName { get; set; }
        public string ItemTypeName { get; set; }
        public string ItemName { get; set; }
        public string UnitSymbol { get; set; }
        public string ReferenceNumber { get; set; }
        public string IQCName { get; set; }
    }
}
