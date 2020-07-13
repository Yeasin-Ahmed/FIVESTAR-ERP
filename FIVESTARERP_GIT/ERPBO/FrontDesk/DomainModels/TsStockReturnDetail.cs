using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.FrontDesk.DomainModels
{
    [Table("tblTsStockReturnDetails")]
   public class TsStockReturnDetail
    {
        [Key]
        public long ReturnDetailId { get; set; }
        public long ReqInfoId { get; set; }
        public long JobOrderId { get; set; }
        public string RequsitionCode { get; set; }
        public long PartsId { get; set; }
        public int Quantity { get; set; }
        public long? BranchId { get; set; }
        public long? OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }

        [ForeignKey("TsStockReturnInfo")]
        public long ReturnInfoId { get; set; }
        public TsStockReturnInfo TsStockReturnInfo { get; set; }
    }
}
