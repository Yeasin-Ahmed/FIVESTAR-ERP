using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.SalesAndDistribution.ViewModels
{
    public class ItemStockViewModel
    {
        public long StockId { get; set; }
        public long? WarehouseId { get; set; }
        public long? CategoryId { get; set; }
        public long? BrandId { get; set; }
        public long ModelId { get; set; }
        public long? ItemTypeId { get; set; }
        public long? ItemId { get; set; } // 
        public long? ColorId { get; set; }
        [StringLength(100)]
        public string IMEI { get; set; }
        [StringLength(100)]
        public string AllIMEI { get; set; }
        [StringLength(100)]
        public string StockStatus { get; set; }
        public long CartoonId { get; set; }
        [StringLength(100)]
        public string CartoonNo { get; set; }
        public Nullable<DateTime> SaleDate { get; set; }
        [StringLength(200)]
        public string ReferenceNumber { get; set; }
        [StringLength(100)]
        public string Remarks { get; set; }
        public long? BranchId { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        // Custom Properties
        [StringLength(150)]
        public string WarehouseName { get; set; }
        [StringLength(150)]
        public string ModelName { get; set; }
        [StringLength(150)]
        public string ItemTypeName { get; set; }
        [StringLength(150)]
        public string ItemName { get; set; }
        [StringLength(150)]
        public string ColorName { get; set; }
        [StringLength(150)]
        public string EntryUser { get; set; }
        [StringLength(150)]
        public string UpdateUser { get; set; }
        [StringLength(100)]
        public string CategoryName { get; set; }
        [StringLength(100)]
        public string BrandName { get; set; }
    }
}
