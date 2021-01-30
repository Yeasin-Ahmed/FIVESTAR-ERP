using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Accounts.DTOModels
{
   public class ChequeBookDTO
    {
        public long ChequeBookId { get; set; }
        public string AccName { get; set; }
        public string AccountNumber { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string CheckNo { get; set; }
        public Nullable<DateTime> CheckDate { get; set; }
        public string CheckType { get; set; }
        public string PayType { get; set; }
        public string CheckApproval { get; set; }
        public double Amount { get; set; }
        public string PayOrReceive { get; set; }
        public string Flag { get; set; }
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        public long? BranchId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public string UserName { get; set; }
    }
}
