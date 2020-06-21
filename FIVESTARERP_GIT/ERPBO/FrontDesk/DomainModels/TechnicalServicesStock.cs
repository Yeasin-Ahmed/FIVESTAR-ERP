using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.FrontDesk.DomainModels
{
    [Table("tblTechnicalServicesStock")]
   public class TechnicalServicesStock
    {
        [Key]
        public long TsStockId { get; set; }
        public long? JobOrderId { get; set; }
        public long? SWarehouseId { get; set; }
        public long? PartsId { get; set; }
        public int Quantity { get; set; }
        public int UsedQty { get; set; }
        public int ReturnQty { get; set; }
        public string Remarks { get; set; }
        public long? BranchId { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
    }
}
