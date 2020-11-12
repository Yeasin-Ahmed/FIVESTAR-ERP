
using ERPBO.Accounts.DomainModels;
using ERPBO.Accounts.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Accounts.Interface
{
   public interface IAccountsHeadBusiness
    {
        IEnumerable<AccountsHeadDTO> AccountsHeadList(long orgId);
        bool SaveAccountsHead(AccountsHeadDTO accountsHeadDTO, long userId, long orgId);
        AccountsHead GetAccountsHeadOneByOrgId(long id, long orgId);
        bool IsDuplicateAHeadCode(string code, long id, long orgId);
    }
}
