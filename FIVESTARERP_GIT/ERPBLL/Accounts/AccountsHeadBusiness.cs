using ERPBLL.Accounts.Interface;
using ERPBO.Accounts.DomainModels;
using ERPBO.Accounts.DTOModels;
using ERPDAL.AccountsDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Accounts
{
   public class AccountsHeadBusiness : IAccountsHeadBusiness

    {
        private readonly IAccountsUnitOfWork _accountsDb; // database
        private readonly AccountsHeadRepository accountsHeadRepository; // repo
        public AccountsHeadBusiness(IAccountsUnitOfWork accountsDb)
        {
            this._accountsDb = accountsDb;
            accountsHeadRepository = new AccountsHeadRepository(this._accountsDb);
        }
        public IEnumerable<AccountsHeadDTO> AccountsHeadList(long orgId)
        {
            return this._accountsDb.Db.Database.SqlQuery<AccountsHeadDTO>(
                string.Format(@"Select *from tblAccountsHead Where OrganizationId={0}", orgId)).ToList();
        }

        public AccountsHead GetAccountsHeadOneByOrgId(long id, long orgId)
        {
            return accountsHeadRepository.GetOneByOrg(h => h.AHeadId == id && h.OrganizationId == orgId);
        }

        public bool IsDuplicateAHeadCode(string code, long id, long orgId)
        {
            return accountsHeadRepository.GetOneByOrg(h => h.AHeadCode == code && h.AHeadId != id && h.OrganizationId == orgId) != null ? true : false;
        }

        public bool SaveAccountsHead(AccountsHeadDTO accountsHeadDTO, long userId, long orgId)
        {
            AccountsHead head = new AccountsHead(); ;
            if (accountsHeadDTO.AHeadId == 0)
            {
                head.AHeadName = accountsHeadDTO.AHeadName;
                head.AHeadCode = accountsHeadDTO.AHeadCode;
                head.Remarks = accountsHeadDTO.Remarks;
                head.OrganizationId = orgId;
                head.BranchId = accountsHeadDTO.BranchId;
                head.EUserId = userId;
                head.EntryDate = DateTime.Now;
                accountsHeadRepository.Insert(head);
            }
            else
            {
                head = GetAccountsHeadOneByOrgId(accountsHeadDTO.AHeadId,orgId);
                head.AHeadName = accountsHeadDTO.AHeadName;
                head.AHeadCode = accountsHeadDTO.AHeadCode;
                head.Remarks = accountsHeadDTO.Remarks;
                head.OrganizationId = orgId;
                head.BranchId = accountsHeadDTO.BranchId;
                head.UpUserId = userId;
                head.UpdateDate = DateTime.Now;
                accountsHeadRepository.Update(head);
            }
            return accountsHeadRepository.Save();
        }
    }
}
