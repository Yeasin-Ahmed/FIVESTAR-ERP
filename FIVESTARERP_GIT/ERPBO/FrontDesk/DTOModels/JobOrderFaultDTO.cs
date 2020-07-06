using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.FrontDesk.DTOModels
{
   public class JobOrderFaultDTO
    {
        public long JobOrderFaultId { get; set; }
        public long FaultId { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public long JobOrderId { get; set; }

        // Custom Properties
        public string FaultName { get; set; }
        public string EntryUser { get; set; }
        public string UpdateUser { get; set; }
    }
}
