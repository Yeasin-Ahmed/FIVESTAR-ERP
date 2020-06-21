using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.DomainModels
{
    [Table("tblFinishGoodsInfo")]
    public class FinishGoodsInfo
    {
        [Key]
        public long FinishGoodsInfoId { get; set;}
        public long ProductionLineId { get; set; }
        public long PackagingLineId { get; set; }
        public long DescriptionId { get; set; }
        public long WarehouseId { get; set; }
        public long ItemTypeId { get; set; }
        public long ItemId { get; set; }
        public long UnitId { get; set; }
        public int Quanity { get; set; }
        public string ProductionType { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public ICollection<FinishGoodsRowMaterial> FinishGoodsRowMaterials { get; set; }
    }
}
