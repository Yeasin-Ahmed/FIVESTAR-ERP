using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Inventory.DomainModels
{
    [Table("tblWarehouseFaultyStockDetails")]
    public class WarehouseFaultyStockDetail
    {
        [Key]
        public long RStockDetailId { get; set; }
        public long? WarehouseId { get; set; }
        public long? ItemTypeId { get; set; }
        public long? ItemId { get; set; }
        public long? UnitId { get; set; }
        public long? LineId { get; set; }
        public long? DescriptionId { get; set; }
        public int Quantity { get; set; }
        [StringLength(50)]
        public string ReturnType { get; set; }
        [StringLength(50)]
        public string FaultyCase { get; set; }
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
    }
}
