using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.DTOModel
{
    public class FinishGoodsRowMaterialDTO
    {
        public long FGRMId { get; set; }
        public long WarehouseId { get; set; }
        public long ItemTypeId { get; set; }
        public long ItemId { get; set; }
        public long UnitId { get; set; }
        public int Quanity { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public long FinishGoodsInfoId { get; set; }
        //Custom Property
        public string WarehouseName { get; set; }
        public string ItemTypeName { get; set; }
        public string ItemName { get; set; }
        public string UnitName { get; set; }
    }
}
