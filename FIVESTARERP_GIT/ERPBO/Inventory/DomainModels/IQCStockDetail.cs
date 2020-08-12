using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Inventory.DomainModels
{
    [Table("tblIQCStockDetails") ]
    public class IQCStockDetail
    {
        [Key]
        public long StockDetailId { get; set; }
        public long? IQCId { get; set; }
        public long? WarehouseId { get; set; }
        public long? DescriptionId { get; set; }
        public long? ItemTypeId { get; set; }
        public long? ItemId { get; set; }
        public long? UnitId { get; set; }
        public int Quantity { get; set; }
        [StringLength(150)]
        public string StockType { get; set; }
        [StringLength(150)]
        public string ReferenceNumber { get; set; }
        public long? SupplierId { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }

    }
}
