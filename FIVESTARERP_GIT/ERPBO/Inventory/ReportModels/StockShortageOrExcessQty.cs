using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Inventory.ReportModels
{
    public class StockShortageOrExcessQty
    {
        public long WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public long DescriptionId { get; set; }
        public string DescriptionName { get; set; }
        public long ItemTypeId { get; set; }
        public string ItemTypeName { get; set; }
        public long ItemId { get; set; }
        public string ItemName { get; set; }
        public long UnitId { get; set; }
        public string UnitName { get; set; }
        public int OrderQty { get; set; }
        public int Quantity { get; set; }
        public int ShortageQty { get; set; }
        public int ExcessQty { get; set; }
        public string EntryDate { get; set; }
    }
}
