using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Inventory.ViewModels
{
    public class ItemPreparationDetailWithInfoViewModel
    {
        public long PreparationInfoId { get; set; }
        public long ItemIdSrc { get; set; }
        public long ItemTypeIdSrc { get; set; }
        public long WarehouseIdSrc { get; set; }
        [StringLength(100)]
        public string ItemNameSrc { get; set; }
        [StringLength(100)]
        public string ItemTypeNameSrc { get; set; }
        [StringLength(100)]
        public string WarehouseNameSrc { get; set; }
        public int Quantity { get; set; }
        public long ItemIdTgt { get; set; }
        public long ItemTypeIdTgt { get; set; }
        public long WarehouseIdTgt { get; set; }
        [StringLength(100)]
        public string ItemNameTgt { get; set; }
        [StringLength(100)]
        public string ItemTypeNameTgt { get; set; }
        [StringLength(100)]
        public string WarehouseNameTgt { get; set; }
        public long UnitId { get; set; }
        public string UnitName { get; set; }
    }
}
