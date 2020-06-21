using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.ViewModels
{
    public class RepairLineStockDetailViewModel
    {
        public long RLStockDetailId { get; set; }
        public long? QCLineId { get; set; }
        public long? RepairLineId { get; set; }
        public long? ProductionLineId { get; set; }
        public long? DescriptionId { get; set; }
        public long? WarehouseId { get; set; }
        public long? ItemTypeId { get; set; }
        public long? ItemId { get; set; }
        public long? UnitId { get; set; }
        public int Quantity { get; set; }
        public Nullable<DateTime> ExpireDate { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        [StringLength(150)]
        public string StockStatus { get; set; }
        [StringLength(150)]
        public string RefferenceNumber { get; set; }

        // Custom Properties
        [StringLength(100)]
        public string QCLineName { get; set; }
        [StringLength(100)]
        public string RepairLineName { get; set; }
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
