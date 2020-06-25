using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.DomainModels
{
    [Table("tblQCItemStockInfo")]
    public class QCItemStockInfo
    {
        [Key]
        public long QCItemStockInfoId { get; set; }
        public long? ProductionFloorId { get; set; }
        public long? DescriptionId { get; set; }
        public long? QCId { get; set; }
        public long? WarehouseId { get; set; }
        public long? ItemTypeId { get; set; }
        public long? ItemId { get; set; }
        public int Quantity { get; set; }
        public int RepairQty { get; set; }
        public int PackagingQty { get; set; }
        public int LabQty { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
    }
}
