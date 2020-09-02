using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.FrontDesk.DTOModels
{
   public class JobOrderReturnDetailDTO
    {
        public long JobOrderReturnDetailId { get; set; }
        public string TransferCode { get; set; }
        public long JobOrderId { get; set; }
        public string JobOrderCode { get; set; }
        public string JobStatus { get; set; }
        public string TransferStatus { get; set; }
        public long? FromBranch { get; set; }
        public long? ToBranch { get; set; }
        public long? BranchId { get; set; }
        public long? OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public string FromBranchName { get; set; }
        public string AccessoriesNames { get; set; }
        public string ModelColor { get; set; }
        public string ModelName { get; set; }
    }
}
