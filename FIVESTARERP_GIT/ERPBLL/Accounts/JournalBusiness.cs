using ERPBLL.Accounts.Interface;
using ERPBLL.Common;
using ERPBO.Accounts.DomainModels;
using ERPBO.Accounts.DTOModels;
using ERPBO.Common;
using ERPBO.FrontDesk.ReportModels;
using ERPDAL.AccountsDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Accounts
{
   public class JournalBusiness:IJournalBusiness
    {
        private readonly IAccountsUnitOfWork _accountsDb; // database
        private readonly JournalRepository journalRepository; // repo
        private readonly IAccountsHeadBusiness _accountsHeadBusiness;
        public JournalBusiness(IAccountsUnitOfWork accountsDb, IAccountsHeadBusiness accountsHeadBusiness)
        {
            this._accountsDb = accountsDb;
            journalRepository = new JournalRepository(this._accountsDb);
            this._accountsHeadBusiness = accountsHeadBusiness;
        }

        public IEnumerable<JournalDTO> CashVoucherList(long orgId, string fromDate, string toDate)
        {
            fromDate = fromDate != null && fromDate.Trim() != "" ? Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd") : "";
            toDate = toDate != null && toDate.Trim() != "" ? Convert.ToDateTime(toDate).ToString("yyyy-MM-dd") : "";
            //fromDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
            string query = string.Format(@"Exec [dbo].[spCashbook] '{0}','{1}',{2}", fromDate, toDate, orgId);
            return this._accountsDb.Db.Database.SqlQuery<JournalDTO>(query).ToList();
        }
        private string QueryForCashBookList(long orgId, string fromDate, string toDate)
        {
            string query = string.Empty;
            string param = string.Empty;

            if (orgId > 0)
            {
                param += string.Format(@"and j.OrganizationId={0}", orgId);
            }
            if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "" && !string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(j.JournalDate as date) between '{0}' and '{1}'", fDate, tDate);
            }
            else if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(j.JournalDate as date)='{0}'", fDate);
            }
            else if (!string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(j.JournalDate as date)='{0}'", tDate);
            }

            query = string.Format(@"Select AccountName,j.ReferenceNum,j.Narration,j.JournalDate,j.Remarks,j.Debit,j.Credit From tblJournal j
 inner join tblAccount a on j.AccountId=a.AccountId
 Where j.ReferenceNum IN (Select ReferenceNum From tblJournal Where AccountId='14') and 1=1{0}", Utility.ParamChecker(param));
            return query;
        }

        public IEnumerable<JournalDTO> GetJournalList(long orgId, string fromDate, string toDate)
        {
            return this._accountsDb.Db.Database.SqlQuery<JournalDTO>(QueryForJournalList(orgId, fromDate,toDate)).ToList();
        }
        private string QueryForJournalList(long orgId, string fromDate, string toDate)
        {
            string query = string.Empty;
            string param = string.Empty;

            if (orgId > 0)
            {
                param += string.Format(@"and j.OrganizationId={0}", orgId);
            }
            if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "" && !string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(j.JournalDate as date) between '{0}' and '{1}'", fDate, tDate);
            }
            else if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(j.JournalDate as date)='{0}'", fDate);
            }
            else if (!string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(j.JournalDate as date)='{0}'", tDate);
            }

            query = string.Format(@"Select AccountName,j.ReferenceNum,j.Narration,j.JournalDate,j.Remarks,j.Debit,j.Credit from tblJournal j
inner join tblAccount a on j.AccountId=a.AccountId where Flag='Journal' and 1=1{0}", Utility.ParamChecker(param));
            return query;
        }

        public bool SaveCreditVouchar(List<JournalDTO> journalDTO, long userId, long orgId)
        {
            var refnumberCredit = ("CCV-" + DateTime.Now.ToString("yy")+"-"+ DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss"));
            var cashId = _accountsHeadBusiness.GetCashHeadId(orgId).AccountId;
            string sDate = journalDTO.FirstOrDefault().JournalDate.ToString();
            DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));

            short day = (short)datevalue.Day;
            short month = (short)datevalue.Month;
            short years = (short)datevalue.Year;


            List<Journal> journals = new List<Journal>();
            foreach (var item in journalDTO)
            {
                Journal journal = new Journal();
                journal.AccountId = item.AccountId;
                journal.Credit = item.Credit;
                journal.Narration = item.Narration;
                journal.Remarks = item.Remarks;
                journal.ReferenceNum = refnumberCredit;
                journal.JournalDate = item.JournalDate;
                journal.Flag = "Voucher";
                journal.Year = years;
                journal.Month = month;
                journal.Day = day;
                journal.OrganizationId = orgId;
                journal.EntryDate = DateTime.Now;
                journal.EUserId = userId;
                journals.Add(journal);
            }
            journalRepository.InsertAll(journals);
            var debit = journals.Sum(c => c.Credit);
            Journal jl = new Journal();
            jl.AccountId = cashId;
            jl.Credit = 0;
            jl.Debit = debit;
            jl.Narration = journalDTO.FirstOrDefault().Narration;
            jl.Remarks = journalDTO.FirstOrDefault().Remarks;
            jl.ReferenceNum = refnumberCredit;
            jl.JournalDate = journalDTO.FirstOrDefault().JournalDate;
            jl.Flag = "Voucher";
            jl.Year = years;
            jl.Month = month;
            jl.Day = day;
            jl.OrganizationId = orgId;
            jl.EntryDate = DateTime.Now;
            jl.EUserId = userId;
            journalRepository.Insert(jl);
            return journalRepository.Save();
        }

        public bool SaveDebitVouchar(List<JournalDTO> journalDTO, long userId, long orgId)
        {
            var refnumberDebit = ("CDV-" + DateTime.Now.ToString("yy") + "-" + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss"));
            var cashId = _accountsHeadBusiness.GetCashHeadId(orgId).AccountId;
            string sDate = journalDTO.FirstOrDefault().JournalDate.ToString();
            DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));

            short day = (short)datevalue.Day;
            short month = (short)datevalue.Month;
            short years = (short)datevalue.Year;

           
            List<Journal> journals = new List< Journal>();
            foreach (var item in journalDTO)
            {
                Journal journal = new Journal();
                journal.AccountId = item.AccountId;
                journal.Debit = item.Debit;
                journal.Narration = item.Narration;
                journal.Remarks = item.Remarks;
                journal.ReferenceNum = refnumberDebit;
                journal.JournalDate = item.JournalDate;
                journal.Flag = "Voucher";
                journal.Year = years;
                journal.Month = month;
                journal.Day = day;
                journal.OrganizationId = orgId;
                journal.EntryDate = DateTime.Now;
                journal.EUserId = userId;
                journals.Add(journal);
            }
            journalRepository.InsertAll(journals);
            var credit = journals.Sum(c => c.Debit);
            Journal jl = new Journal();
            jl.AccountId = cashId;
            jl.Debit = 0;
            jl.Credit = credit;
            jl.Narration = journalDTO.FirstOrDefault().Narration;
            jl.Remarks = journalDTO.FirstOrDefault().Remarks;
            jl.ReferenceNum = refnumberDebit;
            jl.JournalDate = journalDTO.FirstOrDefault().JournalDate;
            jl.Flag = "Voucher";
            jl.Year = years;
            jl.Month = month;
            jl.Day = day;
            jl.OrganizationId = orgId;
            jl.EntryDate = DateTime.Now;
            jl.EUserId = userId;
            journalRepository.Insert(jl);


           return journalRepository.Save();
        }

        public bool SaveJournalVouchar(List<JournalDTO> journalDTO, long userId, long orgId)
        {
            var refnumber= ("JV-" + DateTime.Now.ToString("yy") + "-" + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss")); ;
            string sDate = journalDTO.FirstOrDefault().JournalDate.ToString();
            DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));

            short day = (short)datevalue.Day;
            short month = (short)datevalue.Month;
            short years = (short)datevalue.Year;


            List<Journal> journals = new List<Journal>();
            foreach (var item in journalDTO)
            {
                Journal journal = new Journal();
                journal.AccountId = item.AccountId;
                journal.Debit = item.Debit;
                journal.Credit = item.Credit;
                journal.Narration = item.Narration;
                journal.Remarks = item.Remarks;
                journal.ReferenceNum = refnumber;
                journal.Flag = "Journal";
                journal.JournalDate = item.JournalDate;
                journal.Year = years;
                journal.Month = month;
                journal.Day = day;
                journal.OrganizationId = orgId;
                journal.EntryDate = DateTime.Now;
                journal.EUserId = userId;
                journals.Add(journal);
            }
            journalRepository.InsertAll(journals);
            return journalRepository.Save();
        }

        public IEnumerable<JournalDTO> LedgerList(long? accountId, long orgId, string fromDate, string toDate)
        {
            fromDate = fromDate != null && fromDate.Trim() != "" ? Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd") : "";
            toDate = toDate != null && toDate.Trim() != "" ? Convert.ToDateTime(toDate).ToString("yyyy-MM-dd") : "";
            //accountId = accountId.ToString() != null && accountId.ToString() != "" ? accountId.ToString() : "";
            //fromDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
            string query = string.Format(@"Exec [dbo].[spLadger] {0},'{1}','{2}',{3}", accountId.Value, fromDate, toDate, orgId);
            return this._accountsDb.Db.Database.SqlQuery<JournalDTO>(query).ToList();
        }

        public ExecutionStateWithText SaveDebitVoucharAndPrint(List<JournalDTO> journalDTO, long userId, long orgId)
        {
            ExecutionStateWithText executionState = new ExecutionStateWithText();
            var refnumberDebit = ("CDV-" + DateTime.Now.ToString("yy") + "-" + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss"));
            var cashId = _accountsHeadBusiness.GetCashHeadId(orgId).AccountId;
            string sDate = journalDTO.FirstOrDefault().JournalDate.ToString();
            DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));

            short day = (short)datevalue.Day;
            short month = (short)datevalue.Month;
            short years = (short)datevalue.Year;


            List<Journal> journals = new List<Journal>();
            foreach (var item in journalDTO)
            {
                Journal journal = new Journal();
                journal.AccountId = item.AccountId;
                journal.Debit = item.Debit;
                journal.Narration = item.Narration;
                journal.Remarks = item.Remarks;
                journal.ReferenceNum = refnumberDebit;
                journal.JournalDate = item.JournalDate;
                journal.Flag = "Voucher";
                journal.Year = years;
                journal.Month = month;
                journal.Day = day;
                journal.OrganizationId = orgId;
                journal.EntryDate = DateTime.Now;
                journal.EUserId = userId;
                journals.Add(journal);
            }
            journalRepository.InsertAll(journals);
            var credit = journals.Sum(c => c.Debit);
            Journal jl = new Journal();
            jl.AccountId = cashId;
            jl.Debit = 0;
            jl.Credit = credit;
            jl.Narration = journalDTO.FirstOrDefault().Narration;
            jl.Remarks = journalDTO.FirstOrDefault().Remarks;
            jl.ReferenceNum = refnumberDebit;
            jl.JournalDate = journalDTO.FirstOrDefault().JournalDate;
            jl.Flag = "Voucher";
            jl.Year = years;
            jl.Month = month;
            jl.Day = day;
            jl.OrganizationId = orgId;
            jl.EntryDate = DateTime.Now;
            jl.EUserId = userId;
            journalRepository.Insert(jl);


            if (journalRepository.Save()==true)
            {
                executionState.isSuccess = true;
                executionState.text = refnumberDebit;
            }
            return executionState;
        }
        public ServicesReportHead GetBranchInformation(long orgId, long branchId)
        {
            return this._accountsDb.Db.Database.SqlQuery<ServicesReportHead>(
                string.Format(@"select org.OrganizationName,bh.BranchName,bh.Address,org.OrgLogoPath,bh.PhoneNo,bh.MobileNo,bh.Fax,bh.Email from 
            [ControlPanel].dbo.tblBranch bh
            inner join [ControlPanel].dbo.tblOrganizations org
        on bh.OrganizationId=org.OrganizationId
        where bh.OrganizationId={0} and bh.BranchId={1}", orgId, branchId)).FirstOrDefault();
        }

        public IEnumerable<JournalDTO> GetDebitVoucherReport(string voucherNo, long orgId)
        {
            return this._accountsDb.Db.Database.SqlQuery<JournalDTO>(
                string.Format(@"Select * From tblJournal j
left join tblAccount a on j.AccountId=a.AccountId
where ReferenceNum='{0}' and Debit>0", voucherNo, orgId)).ToList();
        }
    }
}
