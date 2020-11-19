
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
        IEnumerable<AccountDTO> AccountList(long orgId);
        bool SaveAccount(AccountDTO accountsHeadDTO, long userId, long orgId);
        Account GetAccountOneByOrgId(long id, long orgId);
        bool IsDuplicateAaccountCode(string code, long id, long orgId);
        AccountDTO GetCashHeadId(long orgId);
        AccountDTO GetAccountName(long accountId, long orgId);
    }
}
