using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Inventory.DomainModels
{
    [Table("tblItemPreparationInfo")]
    public class ItemPreparationInfo
    {
        [Key]
        public long PreparationInfoId { get; set; }
        [StringLength(100)]
        public string PreparationType { get; set; }
        public long WarehouseId { get; set; }
        public long ItemTypeId { get; set; }
        public long ItemId { get; set; }
        public long UnitId { get; set; }
        public long DescriptionId { get; set; }
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public ICollection<ItemPreparationDetail> ItemPreparationDetails { get; set; }
    }
}
