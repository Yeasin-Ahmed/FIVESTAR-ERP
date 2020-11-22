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
   public class FinanceYearBusiness: IFinanceYearBusiness
    {
        private readonly IAccountsUnitOfWork _accountsDb; // database
        private readonly FinanceYearRepository financeYearRepository; // repo
        public FinanceYearBusiness(IAccountsUnitOfWork accountsDb)
        {
            this._accountsDb = accountsDb;
            financeYearRepository = new FinanceYearRepository(this._accountsDb);
        }

        public IEnumerable<FinanceYearDTO> GetAllFinanceList(long orgId)
        {
            return this._accountsDb.Db.Database.SqlQuery<FinanceYearDTO>(
                string.Format(@"Select * from tblFinanceYear where OrganizationId={0}", orgId)).ToList();
        }

        public bool SaveYear(FinanceYearDTO yearDTO, long userId, long orgId)
        {
            if (yearDTO.FinanceYearId == 0)
            {
                FinanceYear financeYear = new FinanceYear();
                financeYear.Session = yearDTO.Session;
                financeYear.FromDate = yearDTO.FromDate;
                financeYear.ToDate = yearDTO.ToDate;
                financeYear.Remarks = yearDTO.Remarks;
                financeYear.OrganizationId = orgId;
                financeYear.BranchId = financeYear.BranchId;
                financeYear.EUserId = userId;
                financeYear.EntryDate = DateTime.Now;
                financeYearRepository.Insert(financeYear);
            }
            return financeYearRepository.Save();
        }
    }
}
