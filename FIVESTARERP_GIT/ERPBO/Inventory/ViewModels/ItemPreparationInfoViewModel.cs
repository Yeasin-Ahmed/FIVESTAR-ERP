using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Inventory.ViewModels
{
    public class ItemPreparationInfoViewModel
    {
        public long PreparationInfoId { get; set; }
        [Range(1, long.MaxValue)]
        public long WarehouseId { get; set; }
        [Range(1, long.MaxValue)]
        public long ItemTypeId { get; set; }
        [Range(1, long.MaxValue)]
        public long ItemId { get; set; }
        public long UnitId { get; set; }
        [Range(1, long.MaxValue)]
        public long DescriptionId { get; set; }
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }

        // Custom Property
        public string WarehouseName { get; set; }
        public string ItemTypeName { get; set; }
        public string ItemName { get; set; }
        public string UnitName { get; set; }
        public string ModelName { get; set; }
        public int ItemCount { get; set; }
        
    }
}
