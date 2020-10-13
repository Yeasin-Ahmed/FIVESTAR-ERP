using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.FrontDesk.ViewModels
{
   public class JobOrderTransferDetailViewModel
    {
        public long JobOrderTransferDetailId { get; set; }
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
        [StringLength(100)]
        public string FromBranchName { get; set; }
        public string AccessoriesNames { get; set; }
        public string ModelColor { get; set; }
        public string ModelName { get; set; }
        public string ReceivedBy { get; set; }
        //
        public string CourierName { get; set; }
        public string CourierNumber { get; set; }
        public string ApproxBill { get; set; }
    }
}
