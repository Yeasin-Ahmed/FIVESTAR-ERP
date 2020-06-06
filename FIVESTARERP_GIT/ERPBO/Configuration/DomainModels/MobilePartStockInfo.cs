using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Configuration.DomainModels
{
    [Table("tblMobilePartStockInfo")]
   public class MobilePartStockInfo
    {
        [Key]
        public long MobilePartStockInfoId { get; set; }
        public long? MobilePartId { get; set; }
        public long? SWarehouseId { get; set; }
        public int? StockInQty { get; set; }
        public int? StockOutQty { get; set; }
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
    }
}
