using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDAL.AccountsDAL
{
   public class AccountsUnitOfWork:IAccountsUnitOfWork
    {
        private readonly AccountsDbContext _dbcontext;
        public AccountsUnitOfWork()
        {
            _dbcontext = new AccountsDbContext();
        }
        public DbContext Db { get { return _dbcontext; } }

        public void Dispose()
        {

        }
    }
}
