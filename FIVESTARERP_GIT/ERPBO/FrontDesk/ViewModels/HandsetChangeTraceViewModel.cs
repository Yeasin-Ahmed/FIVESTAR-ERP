using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.FrontDesk.ViewModels
{
   public class HandsetChangeTraceViewModel
    {
        public long HandsetChangeTraceId { get; set; }
        public long JobOrderId { get; set; }
        public string JobOrderCode { get; set; }
        public string JobStatus { get; set; }
        public long ModelId { get; set; }
        public string Type { get; set; }
        public string IMEI1 { get; set; }
        public string IMEI2 { get; set; }
        public string Color { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public long BranchId { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public string ModelName { get; set; }
    }
}
