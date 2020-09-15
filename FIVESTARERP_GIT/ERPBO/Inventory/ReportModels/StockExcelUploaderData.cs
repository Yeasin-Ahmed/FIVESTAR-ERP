using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Inventory.ReportModels
{
    public class StockExcelUploaderData
    {
        public long WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public long ItemTypeId { get; set; }
        public string ItemTypeName { get; set; }
        public long ItemId { get; set; }
        public string ItemName { get; set; }
        public long UnitId { get; set; }
        public string UnitName { get; set; }
    }
}
