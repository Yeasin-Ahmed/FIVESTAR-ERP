using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Inventory.DTOModel
{
    public class WarehouseStockDetailInfoListDTO
    {
        public long StockDetailId { get; set; }
        [StringLength(150)]
        public string WarehouseName { get; set; }
        [StringLength(150)]
        public string ModelName { get; set; }
        [StringLength(150)]
        public string ItemTypeName { get; set; }
        [StringLength(150)]
        public string ItemName { get; set; }
        [StringLength(150)]
        public string  UnitName { get; set; }
        public int Quantity { get; set; }
        [StringLength(50)]
        public string StockStatus { get; set; }
        [StringLength(150)]
        public string EntryDate { get; set; }
        [StringLength(150)]
        public string EntryUser { get; set; }
        [StringLength(150)]
        public string RefferenceNumber { get; set; }
        public string UpdateUser { get; set; }
        public int OrderQty{ get; set; }
        public string SupplierName { get; set; }
    }
}
