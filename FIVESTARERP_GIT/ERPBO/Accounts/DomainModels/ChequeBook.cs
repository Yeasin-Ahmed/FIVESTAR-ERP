using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Accounts.DomainModels
{
    [Table("tblChequeBooks")]
   public class ChequeBook
    {
        [Key]
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
    }
}
