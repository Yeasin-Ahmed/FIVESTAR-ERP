using ERPBO.Accounts.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDAL.AccountsDAL
{
    public class AccountsHeadRepository : AccountsBaseRepository<AccountsHead>
    {
        public AccountsHeadRepository(IAccountsUnitOfWork accountsUnitOfWork) : base(accountsUnitOfWork) { }
    }
}
