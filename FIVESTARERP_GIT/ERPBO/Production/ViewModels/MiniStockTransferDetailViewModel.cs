using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.ViewModels
{
    public class MiniStockTransferDetailViewModel
    {
        public long MSTDetailId { get; set; }
        [Range(1, long.MaxValue)]
        public long DescriptionId { get; set; }
        [Range(1, long.MaxValue)]
        public long WarehouseId { get; set; }
        [Range(1, long.MaxValue)]
        public long ItemTypeId { get; set; }
        [Range(1, long.MaxValue)]
        public long ItemId { get; set; }
        public long UnitId { get; set; }
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public long MSTInfoId { get; set; }

        // Custom Properties //
        [StringLength(100)]
        public string ModelName { get; set; }
        [StringLength(100)]
        public string WarehouseName { get; set; }
        [StringLength(100)]
        public string ItemTypeName { get; set; }
        [StringLength(100)]
        public string ItemName { get; set; }
        [StringLength(100)]
        public string UnitName { get; set; }
        [StringLength(100)]
        public string EntryUser { get; set; }
        [StringLength(100)]
        public string UpdateUser { get; set; }
    }
}
