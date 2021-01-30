using ERPBO.Configuration.DTOModels;
using ERPDAL.AccountsDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Accounts.Interface
{
   public interface ICustomerBusiness
    {
        IEnumerable<CustomerDTO> GetAllCustomerList(long orgId);
    }
}
