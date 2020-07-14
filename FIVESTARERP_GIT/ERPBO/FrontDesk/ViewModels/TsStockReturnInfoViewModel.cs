using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.FrontDesk.ViewModels
{
   public class TsStockReturnInfoViewModel
    {
        public long ReturnInfoId { get; set; }
        public long ReqInfoId { get; set; }
        public long JobOrderId { get; set; }
        public string RequsitionCode { get; set; }
        public string StateStatus { get; set; }
        public long? BranchId { get; set; }
        public long? OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public List<TsStockReturnDetailViewModel> TsStockReturnDetails { get; set; }
    }
}
