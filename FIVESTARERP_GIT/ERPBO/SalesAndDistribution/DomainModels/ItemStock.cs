using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.SalesAndDistribution.DomainModels
{
    [Table("tblItemStock")]
    public class ItemStock
    {
        [Key]
        public long StockId { get; set; }
        public long? WarehouseId { get; set; }
        public long? CategoryId { get; set; }
        public long? BrandId { get; set; }
        public long ModelId { get; set; }
        public long? ItemTypeId { get; set; }
        public long? ItemId { get; set; }
        public long? ColorId { get; set; }
        [StringLength(100)]
        public string IMEI { get; set; }
        [StringLength(100)]
        public string AllIMEI { get; set; }
        [StringLength(100)]
        public string StockStatus { get; set; }
        [StringLength(100)]
        public string Remarks { get; set; }
        public long? BranchId { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
    }
}
