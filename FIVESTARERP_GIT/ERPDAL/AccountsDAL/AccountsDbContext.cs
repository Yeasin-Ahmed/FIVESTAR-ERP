using ERPBO.Accounts.DomainModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDAL.AccountsDAL
{
   public class AccountsDbContext:DbContext
    {
        public AccountsDbContext() : base("Accounts")
        {

        }
        public DbSet<AccountsHead> tblAccountsHeads { get; set; }
    }
}
