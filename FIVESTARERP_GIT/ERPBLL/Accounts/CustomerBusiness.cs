using ERPBLL.Accounts.Interface;
using ERPBO.Configuration.DTOModels;
using ERPDAL.AccountsDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Accounts
{
   public class CustomerBusiness: ICustomerBusiness
    {
        private readonly IAccountsUnitOfWork _accountsDb; // database
        private readonly CustomerRepository _customerRepository; // repo
        public CustomerBusiness(IAccountsUnitOfWork accountsDb)
        {
            this._accountsDb = accountsDb;
            _customerRepository = new CustomerRepository(this._accountsDb);
        }

        public IEnumerable<CustomerDTO> GetAllCustomerList(long orgId)
        {
            return this._accountsDb.Db.Database.SqlQuery<CustomerDTO>(
                string.Format(@" Select * From tblCustomers Where OrganizationId={0}", orgId)).ToList();
        }
    }
}
