using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.FrontDesk.ViewModels
{
   public class JobOrderServiceViewModel
    {
        public long JobOrderServiceId { get; set; }
        public long ServiceId { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public long JobOrderId { get; set; }

        // Custom Properties
        public string ServiceName { get; set; }
        [StringLength(100)]
        public string EntryUser { get; set; }
        public string UpdateUser { get; set; }
    }
}
