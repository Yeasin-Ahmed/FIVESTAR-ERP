using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.FrontDesk.ViewModels
{
   public class DashboardApprovedRequsitionViewModel
    {
        public long RequsitionInfoForJobOrderId { get; set; }
        public string StateStatus { get; set; }
        public long JodOrderId { get; set; }
        public string JobOrderCode { get; set; }
        public string RequsitionCode { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
    }
}
