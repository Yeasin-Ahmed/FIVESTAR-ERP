using ERPBO.Accounts.DomainModels;
using ERPBO.Accounts.DTOModels;
using ERPBO.Configuration.DTOModels;
using ERPDAL.AccountsDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Accounts.Interface
{
   public interface ICustomersBusiness
    {
        IEnumerable<AccountsCustomerDTO> GetAllCustomerList(long orgId);
        bool SaveAccountsCustomers(AccountsCustomerDTO dto,long userId, long orgId);
        AccountsCustomer GetCustomerByOrgId(long cusId, long orgId);
    }
}
