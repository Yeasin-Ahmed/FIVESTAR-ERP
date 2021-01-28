using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Accounts.DTOModels
{
   public class JournalDTO
    {
        public long JournalId { get; set; }
        public string ReferenceNum { get; set; }
        public long AccountId { get; set; }
        public double Debit { get; set; }
        public double Credit { get; set; }
        public string Flag { get; set; }
        public string Remarks { get; set; }
        public string Narration { get; set; }
        public short Year { get; set; }
        public short Month { get; set; }
        public short Day { get; set; }
        public Nullable<DateTime> JournalDate { get; set; }
        public long OrganizationId { get; set; }
        public long? BranchId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public long? ApprovedBy { get; set; }
        public Nullable<DateTime> ApproveDate { get; set; }
        public string AccountName { get; set; }
        //
        public string PersonalCode { get; set; }
        public string VoucherNo { get; set; }
        public double? DueAmount { get; set; }
    }
}
