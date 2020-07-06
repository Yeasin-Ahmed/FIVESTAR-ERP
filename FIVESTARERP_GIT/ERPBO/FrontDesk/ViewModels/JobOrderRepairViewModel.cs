using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.FrontDesk.ViewModels
{
   public class JobOrderRepairViewModel
    {
        public long JobOrderRepairId { get; set; }
        public long RepairId { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public long JobOrderId { get; set; }
        // Custom Properties
        public string RepairName { get; set; }
        public string EntryUser { get; set; }
        public string UpdateUser { get; set; }
    }
}
