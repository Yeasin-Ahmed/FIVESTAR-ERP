using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Accounts.DTOModels
{
   public class DetailsLedgerDTO
    {
        public double TotalDebit { get; set; }
        public double TotalCredit { get; set; }
        public double Debit { get; set; }
        public double Credit { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string AccountName { get; set; }
    }
}
