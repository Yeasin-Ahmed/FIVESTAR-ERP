using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Accounts.DTOModels
{
   public class AccountDTO
    {
        public long AccountId { get; set; }
        public string AccountName { get; set; }
        public string AccountCode { get; set; }
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        public long? BranchId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public bool IsGroupHead { get; set; }
        public string AccountType { get; set; }
        public string AncestorId { get; set; }
    }
}
