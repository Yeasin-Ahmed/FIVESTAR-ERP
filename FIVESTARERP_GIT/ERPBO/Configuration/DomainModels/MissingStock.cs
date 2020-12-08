using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Configuration.DomainModels
{
    [Table("tblMissingStock")]
    public class MissingStock
    {
        [Key]
        public long MissingStockId { get; set; }
        public long DescriptionId { get; set; }
        public long ColorId { get; set; }
        public long PartsId { get; set; }
        public string StockType { get; set; }
        public string IMEI { get; set; }
        public int Quantity { get; set; }
        public long OrganizationId { get; set; }
        public long BranchId { get; set; }
        public string Remarks { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
    }
}
