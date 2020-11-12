using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.SalesAndDistribution.DomainModels
{
    [Table("tblStoreMasterStock")]
    public class StoreMasterStock
    {
        [Key]
        public long ID { get; set; }
        public long? CategoryId { get; set; }
        public long? BrandId { get; set; }
        public long? Model { get; set; }
        public long? ColorId { get; set; }
        [StringLength(100)]
        public string IMEI { get; set; }
        public double? CostPrice { get; set; }
        public double? SalePrice { get; set; }
        public long StockInQty { get; set; }
        public long StockOutQty { get; set; }
        [StringLength(100)]
        public string Remarks { get; set; }
        public long? BranchId { get; set; }
        public long OrganizationId { get; set; }
        public Nullable<DateTime> LastStockInTime { get; set; }
        public Nullable<DateTime> LastStockOutTime { get; set; }
        [StringLength(100)]
        public string StockTransactionReason { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
    }
}
