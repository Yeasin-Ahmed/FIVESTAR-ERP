using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.ViewModels
{
    public class AssemblyLineStockInfoViewModel
    {
        public long ALSInfo { get; set; }
        [Range(1,long.MaxValue)]
        public long? AssemblyLineId { get; set; }
        [Range(1, long.MaxValue)]
        public long? ProductionLineId { get; set; }
        [Range(1, long.MaxValue)]
        public long? DescriptionId { get; set; }
        [Range(1, long.MaxValue)]
        public long? WarehouseId { get; set; }
        [Range(1, long.MaxValue)]
        public long? ItemTypeId { get; set; }
        [Range(1, long.MaxValue)]
        public long? ItemId { get; set; }
        [Range(1, long.MaxValue)]
        public long? UnitId { get; set; }
        public int? StockInQty { get; set; }
        public int? StockOutQty { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }

        // Custom Property
        [StringLength(100)]
        public string AssemblyLineName { get; set; }
        [StringLength(100)]
        public string ProductionLineName { get; set; }
        [StringLength(200)]
        public string ModelName { get; set; }
        [StringLength(100)]
        public string WarehouseName { get; set; }
        [StringLength(100)]
        public string ItemTypeName { get; set; }
        [StringLength(100)]
        public string ItemName { get; set; }
        [StringLength(100)]
        public string UnitName { get; set; }
    }
}
