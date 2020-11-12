using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.SalesAndDistribution.ViewModels
{
    public class StoreMasterStockViewModel
    {
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

        // Custom Properties
        [StringLength(150)]
        public string CategoryName { get; set; }
        [StringLength(150)]
        public string BrandName { get; set; }
        [StringLength(100)]
        public string ModelName { get; set; }
        [StringLength(200)]
        public string ColorName { get; set; }
        public string EntryUser { get; set; }
        public string UpdateUser { get; set; }
    }
}
