using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Inventory.DTOModel
{
    public class RepairStockDetailListDTO
    {
        public long RStockDetailId { get; set; }
        public string LineNumber { get; set; }
        public string ModelName { get; set; }
        public string WarehouseName { get; set; }
        public string ItemTypeName { get; set; }
        public string ItemName { get; set; }
        public string UnitName { get; set; }
        public int Quantity { get; set; }
        public string StockStatus { get; set; }
        public string EntryDate { get; set; }
        public string RefferenceNumber { get; set; }
        public string EntryUser { get; set; }
        public string UpdateUser { get; set; }
        public string ReturnType { get; set; }
        public string FaultyCase { get; set; }
    }
}
