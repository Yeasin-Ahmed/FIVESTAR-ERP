using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.DomainModels
{
    [Table("tblStockItemReturnInfo")]
    public class StockItemReturnInfo
    {
        [Key]
        public long SIRInfoId { get; set; }
        [StringLength(50)]
        public string ReturnCode { get; set; }
        public long DescriptionId { get; set; }
        public long ProductionFloorId { get; set; }
        public long? AssemblyLineId { get; set; }
        public long? RepairLineId { get; set; }
        public long? PackagingLineId { get; set; }
        public long WarehouseId { get; set; }
        [StringLength(50)]
        public string Flag { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }
        [StringLength(150)]
        public string StateStatus { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public ICollection<StockItemReturnDetail> StockItemReturnDetails { get; set; }
    }
}
