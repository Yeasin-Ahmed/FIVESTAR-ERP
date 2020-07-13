using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.DTOModel
{
    public class QCPassTransferDetailDTO
    {
        public long QPassDetailId { get; set; }
        public long ProductionFloorId { get; set; }
        [StringLength(100)]
        public string ProductionFloorName { get; set; }
        public long QCLineId { get; set; }
        [StringLength(100)]
        public string QCLineName { get; set; }
        public long DescriptionId { get; set; }
        [StringLength(100)]
        public string ModelName { get; set; }
        public long WarehouseId { get; set; }
        [StringLength(100)]
        public string WarehouseName { get; set; }
        public long ItemTypeId { get; set; }
        [StringLength(100)]
        public string ItemTypeName { get; set; }
        public long ItemId { get; set; }
        [StringLength(100)]
        public string ItemName { get; set; }
        public long UnitId { get; set; }
        [StringLength(100)]
        public string UnitName { get; set; }
        public int Quantity { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }       
        public long QPassId { get; set; }
    }

}
