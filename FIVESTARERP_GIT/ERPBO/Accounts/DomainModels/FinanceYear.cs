using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Accounts.DomainModels
{
    [Table("tblFinanceYear")]
   public class FinanceYear
    {
        [Key]
        public long FinanceYearId { get; set; }
        public Nullable<DateTime> FromDate { get; set; }
        public Nullable<DateTime> ToDate { get; set; }
        public string Session { get; set; }
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        public long? BranchId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
    }
}
