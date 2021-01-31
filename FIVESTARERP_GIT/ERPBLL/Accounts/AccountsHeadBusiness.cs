using ERPBLL.Accounts.Interface;
using ERPBO.Accounts.DomainModels;
using ERPBO.Accounts.DTOModels;
using ERPBO.Configuration.DomainModels;
using ERPBO.Configuration.DTOModels;
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
        public IEnumerable<AccountDTO> AccountList(long orgId)
        {
            return this._accountsDb.Db.Database.SqlQuery<AccountDTO>(
                string.Format(@"Select *from tblAccount Where OrganizationId={0}", orgId)).ToList();
        }

        public AccountDTO GetAccountName(long accountId, long orgId)
        {
            return this._accountsDb.Db.Database.SqlQuery<AccountDTO>(
                string.Format(@"select AccountName from tblAccount 
 where AccountId={0} and OrganizationId={1}", accountId, orgId)).FirstOrDefault();
        }

        public Account GetAccountOneByOrgId(long id, long orgId)
        {
            return accountsHeadRepository.GetOneByOrg(h => h.AccountId == id && h.OrganizationId == orgId);
        }

        public AccountDTO GetCashHeadId(long orgId)
        {
            return this._accountsDb.Db.Database.SqlQuery<AccountDTO>(
                string.Format(@"Select * from tblAccount Where AccountName='Cash In Hand' and OrganizationId={0}", orgId)).FirstOrDefault();
        }

        public AccountDTO GetCustomerAncestorId(long orgId)
        {
            return this._accountsDb.Db.Database.SqlQuery<AccountDTO>(
                string.Format(@"Select * from tblAccount Where AccountName='Bill Receable' and OrganizationId={0}", orgId)).FirstOrDefault();
        }

        public AccountDTO GetSupplierAncestorId(long orgId)
        {
            return this._accountsDb.Db.Database.SqlQuery<AccountDTO>(
               string.Format(@"Select * from tblAccount Where AccountName='Bill Payable' and OrganizationId={0}", orgId)).FirstOrDefault();
        }

        public bool IsDuplicateAaccountCode(string code, long id, long orgId)
        {
            return accountsHeadRepository.GetOneByOrg(h => h.AccountCode == code && h.AccountId != id && h.OrganizationId == orgId) != null ? true : false;
        }

        public bool SaveAccount(AccountDTO accountsHeadDTO, long userId, long orgId)
        {
            var mainHeadId = string.Empty;
            var ancestorId = string.Empty;
            var accountName = string.Empty;
            string mainAncestorIdfull = "";

            if (!accountsHeadDTO.IsGroupHead)
            {
                long aheadId = Convert.ToInt64(accountsHeadDTO.AncestorId);
                var data = accountsHeadRepository.GetOneByOrg(s => s.OrganizationId == orgId && s.AccountId == aheadId);
                mainHeadId = data != null ? data.AccountId.ToString() : "0";
                ancestorId = data != null ? (string.IsNullOrEmpty(data.AncestorId) ? "0": data.AncestorId) : "0";
                mainAncestorIdfull= ","+mainHeadId + "," + ancestorId +",";
                mainAncestorIdfull = mainAncestorIdfull.Replace(",,", ",");
                //accountName = GetAccountName(aheadId, orgId).AccountName;
            }
            

            Account head = new Account();
            if (accountsHeadDTO.AccountId == 0)
            {
                //if (!accountsHeadDTO.IsGroupHead)
                //{
                //    head.AccountName = accountName+"_"+ accountsHeadDTO.AccountName;
                //}
                //else
                //{
                //    head.AccountName = accountsHeadDTO.AccountName;
                //}
                head.AccountName = accountsHeadDTO.AccountName;
                head.AccountCode = accountsHeadDTO.AccountCode;
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
