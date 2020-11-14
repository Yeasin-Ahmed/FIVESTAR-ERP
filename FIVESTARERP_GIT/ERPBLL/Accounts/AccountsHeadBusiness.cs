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
            var mainHeadId = string.Empty;
            var ancestorId = string.Empty;
            string mainAncestorIdfull = "";
            if (!accountsHeadDTO.IsGroupHead)
            {
                long aheadId = Convert.ToInt64(accountsHeadDTO.AncestorId);
                var data = accountsHeadRepository.GetOneByOrg(s => s.OrganizationId == orgId && s.AHeadId == aheadId);
                mainHeadId = data != null ? data.AHeadId.ToString() : "0";
                ancestorId = data != null ? (string.IsNullOrEmpty(data.AncestorId) ? "0": data.AncestorId) : "0";
                mainAncestorIdfull= ","+mainHeadId + "," + ancestorId +",";
                mainAncestorIdfull = mainAncestorIdfull.Replace(",,", ",");
            }
            
            AccountsHead head = new AccountsHead();
            if (accountsHeadDTO.AHeadId == 0)
            {
                head.AHeadName = accountsHeadDTO.AHeadName;
                head.AHeadCode = accountsHeadDTO.AHeadCode;
                head.AncestorId = mainAncestorIdfull;
                head.IsGroupHead = accountsHeadDTO.IsGroupHead;
                head.AccountType = accountsHeadDTO.AccountType;
                head.Remarks = accountsHeadDTO.Remarks;
                head.OrganizationId = orgId;
                head.BranchId = accountsHeadDTO.BranchId;
                head.EUserId = userId;
                head.EntryDate = DateTime.Now;
                accountsHeadRepository.Insert(head);
            }
            return accountsHeadRepository.Save();
        }
    }
}
