﻿using ERPBO.Accounts.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDAL.AccountsDAL
{
    public class AccountsHeadRepository : AccountsBaseRepository<Account>
    {
        public AccountsHeadRepository(IAccountsUnitOfWork accountsUnitOfWork) : base(accountsUnitOfWork) { }
    }
    public class JournalRepository : AccountsBaseRepository<Journal>
    {
        public JournalRepository(IAccountsUnitOfWork accountsUnitOfWork) : base(accountsUnitOfWork) { }
    }
    public class FinanceYearRepository : AccountsBaseRepository<FinanceYear>
    {
        public FinanceYearRepository(IAccountsUnitOfWork accountsUnitOfWork) : base(accountsUnitOfWork) { }
    }
    public class ChequeBookRepository : AccountsBaseRepository<ChequeBook>
    {
        public ChequeBookRepository(IAccountsUnitOfWork accountsUnitOfWork) : base(accountsUnitOfWork) { }
    }
    public class SupplierRepository : AccountsBaseRepository<Supplier>
    {
        public SupplierRepository(IAccountsUnitOfWork accountsUnitOfWork) : base(accountsUnitOfWork) { }
    }
    public class CustomerRepository : AccountsBaseRepository<Customer>
    {
        public CustomerRepository(IAccountsUnitOfWork accountsUnitOfWork) : base(accountsUnitOfWork) { }
    }
}
