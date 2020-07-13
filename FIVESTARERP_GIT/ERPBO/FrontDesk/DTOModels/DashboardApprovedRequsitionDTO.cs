using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.FrontDesk.DTOModels
{
   public class DashboardApprovedRequsitionDTO
    {
        public long JodOrderId { get; set; }
        public string JobOrderCode { get; set; }
        public string RequsitionCode { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
    }
}
