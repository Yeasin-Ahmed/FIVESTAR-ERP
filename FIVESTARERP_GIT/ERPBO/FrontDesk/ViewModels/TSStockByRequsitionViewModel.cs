using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.FrontDesk.ViewModels
{
   public class TSStockByRequsitionViewModel
    {
        public long JodOrderId { get; set; }
        public long RequsitionInfoForJobOrderId { get; set; }
        public string JobOrderCode { get; set; }
        public string MobilePartName { get; set; }
        public string RequsitionCode { get; set; }
        public long EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long DescriptionId { get; set; }
        public long PartsId { get; set; }
        public int Quantity { get; set; }
        public string StateStatus { get; set; }
    }
}
