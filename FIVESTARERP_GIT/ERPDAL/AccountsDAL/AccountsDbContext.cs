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
        public DbSet<Account> tblAccount { get; set; }
        public DbSet<Journal> tblJournal { get; set; }
        public DbSet<FinanceYear> tblFinanceYear { get; set; }
        public DbSet<ChequeBook> tblChequeBooks { get; set; }
        public DbSet<Supplier> tblSuppliers { get; set; }
        public DbSet<Customer> tblCustomers { get; set; }
    }
}
