using ERPBLL.Accounts.Interface;
using ERPBLL.Common;
using ERPBO.Configuration.DomainModels;
using ERPBO.Configuration.DTOModels;
using ERPBO.Configuration.ViewModels;
using ERPBO.Common;
using ERPBO.FrontDesk.ReportModels;
using ERPWeb.Filters;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ERPBO.Accounts.ViewModels;
using ERPBO.Accounts.DTOModels;

namespace ERPWeb.Controllers
{
    [CustomAuthorize]
    public class AccountsController : BaseController
    {
        private readonly IAccountsHeadBusiness _accountsHeadBusiness;
        private readonly IJournalBusiness _journalBusiness;
        private readonly IFinanceYearBusiness _financeYearBusiness;
        private readonly IChequeBookBusiness _chequeBookBusiness;
        private readonly ISupplierBusiness _supplierBusiness;
        private readonly ICustomersBusiness _customersBusiness;
        // GET: Accounts
        public AccountsController(IAccountsHeadBusiness accountsHeadBusiness, IJournalBusiness journalBusiness, IFinanceYearBusiness financeYearBusiness, IChequeBookBusiness chequeBookBusiness, ISupplierBusiness supplierBusiness, ICustomersBusiness customersBusiness)
        {
            this._accountsHeadBusiness = accountsHeadBusiness;
            this._journalBusiness = journalBusiness;
            this._financeYearBusiness = financeYearBusiness;
            this._chequeBookBusiness = chequeBookBusiness;
            this._supplierBusiness = supplierBusiness;
            this._customersBusiness = customersBusiness;
        }

        #region tblAccount
        [HttpGet]
        public ActionResult GetAccountList()
        {
            ViewBag.ddlAncestorName = _accountsHeadBusiness.AccountList(User.OrgId).Select(mobile => new SelectListItem { Text = mobile.AccountName, Value = mobile.AccountId.ToString() }).ToList();

            ViewBag.ddlAccountsType = Utility.ListOfAccountsType().Select(r => new SelectListItem { Text = r.text, Value = r.value }).ToList();

            var dto = _accountsHeadBusiness.AccountList(User.OrgId);
            List<AccountViewModel> viewModels = new List<AccountViewModel>();
            AutoMapper.Mapper.Map(dto, viewModels);
            return View(viewModels);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveAccount(AccountViewModel accountsHeadViewModel)
        {
            bool isSuccess = false;
            if (ModelState.IsValid)
            {
                try
                {
                    AccountDTO dto = new AccountDTO();
                    AutoMapper.Mapper.Map(accountsHeadViewModel, dto);
                    isSuccess = _accountsHeadBusiness.SaveAccount(dto, User.UserId, User.OrgId);
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                }
            }
            return Json(isSuccess);
        }
        #endregion

        #region Journal/Debit/Credit Entry
        public ActionResult GetJournalList()
        {
            ViewBag.UserPrivilege = UserPrivilege("Accounts", "GetJournalList");
            ViewBag.ddlDebitAccountName = _accountsHeadBusiness.AccountList(User.OrgId).Select(mobile => new SelectListItem { Text = mobile.AccountName, Value = mobile.AccountId.ToString() }).ToList();

            ViewBag.ddlCreditAccountName = _accountsHeadBusiness.AccountList(User.OrgId).Select(mobile => new SelectListItem { Text = mobile.AccountName, Value = mobile.AccountId.ToString() }).ToList();

            ViewBag.ddlJournalAccountName = _accountsHeadBusiness.AccountList(User.OrgId).Select(mobile => new SelectListItem { Text = mobile.AccountName, Value = mobile.AccountId.ToString() }).ToList();

            ViewBag.ddlJournalType = Utility.ListOfJournalType().Select(r => new SelectListItem { Text = r.text, Value = r.value }).ToList();

            return View(); 
        }
        [HttpPost]
        public ActionResult SaveDebitVoucher(List<JournalViewModel> models)
        {
            bool isSuccess = false;
            if (ModelState.IsValid && models.Count > 0)
            {
                try
                {
                    List<JournalDTO> dto = new List<JournalDTO>();
                    AutoMapper.Mapper.Map(models, dto);
                    isSuccess = _journalBusiness.SaveDebitVouchar(dto, User.UserId, User.OrgId);
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                }
            }
            return Json(isSuccess);
        }
        public ActionResult SaveDebitVoucherWithReport(List<JournalViewModel> models)
        {
            ExecutionStateWithText executionState = new ExecutionStateWithText();
            if (ModelState.IsValid && models.Count > 0)
            {
                List<JournalDTO> dto = new List<JournalDTO>();
                AutoMapper.Mapper.Map(models, dto);
                executionState = _journalBusiness.SaveDebitVoucharAndPrint(dto, User.UserId, User.OrgId);
                //executionState = _jobOrderBusiness.SaveJobOrderMDelivey(jobOrders, User.UserId, User.OrgId, User.BranchId);
                if (executionState.isSuccess)
                {
                    executionState.text = GetDebitVoucherReport(executionState.text);
                }

            }
            return Json(executionState);
        }
        private string GetDebitVoucherReport(string voucherNo)
        {
            string file = string.Empty;
            IEnumerable<JournalDTO> debit = _journalBusiness.GetDebitVoucherReport(voucherNo, User.OrgId);

           ServicesReportHead reportHead = _journalBusiness.GetBranchInformation(User.OrgId, User.BranchId);
           reportHead.ReportImage = Utility.GetImageBytes(User.LogoPaths[0]);
           List<ServicesReportHead> servicesReportHeads = new List<ServicesReportHead>();
            servicesReportHeads.Add(reportHead);

            LocalReport localReport = new LocalReport();
            string reportPath = Server.MapPath("~/Reports/Accounts/rptDebitVoucherReport.rdlc");
            if (System.IO.File.Exists(reportPath))
            {
                localReport.ReportPath = reportPath;
                ReportDataSource dataSource1 = new ReportDataSource("Debit", debit);
                ReportDataSource dataSource2 = new ReportDataSource("ServicesReportHead", servicesReportHeads);
                localReport.DataSources.Clear();
                localReport.DataSources.Add(dataSource1);
                localReport.DataSources.Add(dataSource2);
                localReport.Refresh();
                localReport.DisplayName = "DebitVoucher";

                string mimeType;
                string encoding;
                string fileNameExtension = ".pdf";
                Warning[] warnings;
                string[] streams;
                byte[] renderedBytes;

                renderedBytes = localReport.Render(
                    "Pdf",
                    "",
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings);
                var base64 = Convert.ToBase64String(renderedBytes);
                var fs = String.Format("data:application/pdf;base64,{0}", base64);

                file = fs;
            }

            return file;
        }
        [HttpPost]
        public ActionResult SaveCreditVoucher(List<JournalViewModel> models)
        {
            bool isSuccess = false;
            if (ModelState.IsValid && models.Count > 0)
            {
                try
                {
                    List<JournalDTO> dto = new List<JournalDTO>();
                    AutoMapper.Mapper.Map(models, dto);
                    isSuccess = _journalBusiness.SaveCreditVouchar(dto, User.UserId, User.OrgId);
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                }
            }
            return Json(isSuccess);
        }
        public ActionResult SaveCreditVoucherWithReport(List<JournalViewModel> models)
        {
            ExecutionStateWithText executionState = new ExecutionStateWithText();
            if (ModelState.IsValid && models.Count > 0)
            {
                List<JournalDTO> dto = new List<JournalDTO>();
                AutoMapper.Mapper.Map(models, dto);
                executionState = _journalBusiness.SaveCreditVoucharAndPrint(dto, User.UserId, User.OrgId);
                //executionState = _jobOrderBusiness.SaveJobOrderMDelivey(jobOrders, User.UserId, User.OrgId, User.BranchId);
                if (executionState.isSuccess)
                {
                    executionState.text = GetCreditVoucherReport(executionState.text);
                }

            }
            return Json(executionState);
        }
        private string GetCreditVoucherReport(string voucherNo)
        {
            string file = string.Empty;
            IEnumerable<JournalDTO> credit = _journalBusiness.GetCreditVoucherReport(voucherNo, User.OrgId);

            ServicesReportHead reportHead = _journalBusiness.GetBranchInformation(User.OrgId, User.BranchId);
            reportHead.ReportImage = Utility.GetImageBytes(User.LogoPaths[0]);
            List<ServicesReportHead> servicesReportHeads = new List<ServicesReportHead>();
            servicesReportHeads.Add(reportHead);

            LocalReport localReport = new LocalReport();
            string reportPath = Server.MapPath("~/Reports/Accounts/rptCreditVoucherReport.rdlc");
            if (System.IO.File.Exists(reportPath))
            {
                localReport.ReportPath = reportPath;
                ReportDataSource dataSource1 = new ReportDataSource("Credit", credit);
                ReportDataSource dataSource2 = new ReportDataSource("ServicesReportHead", servicesReportHeads);
                localReport.DataSources.Clear();
                localReport.DataSources.Add(dataSource1);
                localReport.DataSources.Add(dataSource2);
                localReport.Refresh();
                localReport.DisplayName = "CreditVoucher";

                string mimeType;
                string encoding;
                string fileNameExtension = ".pdf";
                Warning[] warnings;
                string[] streams;
                byte[] renderedBytes;

                renderedBytes = localReport.Render(
                    "Pdf",
                    "",
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings);
                var base64 = Convert.ToBase64String(renderedBytes);
                var fs = String.Format("data:application/pdf;base64,{0}", base64);

                file = fs;
            }

            return file;
        }
        [HttpPost]
        public ActionResult SaveJournalVoucher(List<JournalViewModel> models)
        {
            bool isSuccess = false;
            if (ModelState.IsValid && models.Count > 0)
            {
                try
                {
                    List<JournalDTO> dto = new List<JournalDTO>();
                    AutoMapper.Mapper.Map(models, dto);
                    isSuccess = _journalBusiness.SaveJournalVouchar(dto, User.UserId, User.OrgId);
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                }
            }
            return Json(isSuccess);
        }
        public ActionResult SaveJournalVoucherWithReport(List<JournalViewModel> models)
        {
            ExecutionStateWithText executionState = new ExecutionStateWithText();
            if (ModelState.IsValid && models.Count > 0)
            {
                List<JournalDTO> dto = new List<JournalDTO>();
                AutoMapper.Mapper.Map(models, dto);
                executionState = _journalBusiness.SaveJournalVoucharAndPrint(dto, User.UserId, User.OrgId);
                //executionState = _jobOrderBusiness.SaveJobOrderMDelivey(jobOrders, User.UserId, User.OrgId, User.BranchId);
                if (executionState.isSuccess)
                {
                    executionState.text = GetJournalVoucherReport(executionState.text);
                }

            }
            return Json(executionState);
        }
        private string GetJournalVoucherReport(string voucherNo)
        {
            string file = string.Empty;
            IEnumerable<JournalDTO> journal = _journalBusiness.GetJournalVoucherReport(voucherNo, User.OrgId);

            ServicesReportHead reportHead = _journalBusiness.GetBranchInformation(User.OrgId, User.BranchId);
            reportHead.ReportImage = Utility.GetImageBytes(User.LogoPaths[0]);
            List<ServicesReportHead> servicesReportHeads = new List<ServicesReportHead>();
            servicesReportHeads.Add(reportHead);

            LocalReport localReport = new LocalReport();
            string reportPath = Server.MapPath("~/Reports/Accounts/rptJournalVoucherReport.rdlc");
            if (System.IO.File.Exists(reportPath))
            {
                localReport.ReportPath = reportPath;
                ReportDataSource dataSource1 = new ReportDataSource("Journal", journal);
                ReportDataSource dataSource2 = new ReportDataSource("ServicesReportHead", servicesReportHeads);
                localReport.DataSources.Clear();
                localReport.DataSources.Add(dataSource1);
                localReport.DataSources.Add(dataSource2);
                localReport.Refresh();
                localReport.DisplayName = "CreditVoucher";

                string mimeType;
                string encoding;
                string fileNameExtension = ".pdf";
                Warning[] warnings;
                string[] streams;
                byte[] renderedBytes;

                renderedBytes = localReport.Render(
                    "Pdf",
                    "",
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings);
                var base64 = Convert.ToBase64String(renderedBytes);
                var fs = String.Format("data:application/pdf;base64,{0}", base64);

                file = fs;
            }

            return file;
        }
        #endregion

        #region Cashbook/Journalbook list
        public ActionResult JournalList(string voucherNo, string flag_, string fromDate, string toDate)
        {
            if (string.IsNullOrEmpty(flag_))
            {
                return View();
            }
            else
            {
                var dto = _journalBusiness.GetJournalList(voucherNo, User.OrgId, fromDate, toDate);
                List<JournalViewModel> viewModels = new List<JournalViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_JournalList", viewModels);
            }
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult DeleteVoucher(string voucherNo)
        {
            bool isSuccess = false;
            if (voucherNo !=null)
            {
                try
                {
                    isSuccess = _journalBusiness.DeleteJournalVoucher(voucherNo, User.OrgId);
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                }
            }
            return Json(isSuccess);
        }
        public ActionResult CashVoucherList(string flag_, string fromDate, string toDate)
        {
            if (string.IsNullOrEmpty(flag_) && flag_ == "cash")
            {
                return View();
            }
            else
            {
                var dto = _journalBusiness.CashVoucherList(User.OrgId, fromDate, toDate);
                List<JournalViewModel> viewModels = new List<JournalViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_CashVoucherList", viewModels);
            }
        }
        #endregion

        #region Ledger
        public ActionResult GetLadgerList(string _flag, long? accountId, string fromDate, string toDate)
        {
            if (string.IsNullOrEmpty(_flag))
            {
                ViewBag.ddlLedgerAccountName = _accountsHeadBusiness.AccountList(User.OrgId).Select(mobile => new SelectListItem { Text = mobile.AccountName, Value = mobile.AccountId.ToString() }).ToList();
                return View();
            }
            else
            {
                var dto = _journalBusiness.LedgerList(accountId, User.OrgId, fromDate, toDate);
                List<JournalViewModel> viewModels = new List<JournalViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetLadgerList", viewModels);
            }

        }
        #endregion

        #region FinanceYear
        public ActionResult GetFinanceYearList()
        {
            var dto = _financeYearBusiness.GetAllFinanceList(User.OrgId);
            List<FinanceYearViewModel> viewModels = new List<FinanceYearViewModel>();
            AutoMapper.Mapper.Map(dto, viewModels);
            return View( viewModels);
        }
        [HttpPost,ValidateJsonAntiForgeryToken]
        public ActionResult SaveFinanceYear(FinanceYearViewModel financeYearViewModel)
        {
            bool isSuccess = false;
            if (ModelState.IsValid)
            {
                try
                {
                    FinanceYearDTO dto = new FinanceYearDTO();
                    AutoMapper.Mapper.Map(financeYearViewModel, dto);
                    isSuccess = _financeYearBusiness.SaveYear(dto, User.UserId, User.OrgId);
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                }
            }
            return Json(isSuccess);
        }
        #endregion

        #region Reports
        public ActionResult GetJournalVoucherReports(string voucherNo, string fromDate, string toDate,string rptType)
        {
            string file = string.Empty;
            IEnumerable<JournalDTO> journal = _journalBusiness.GetJournalList(voucherNo,User.OrgId, fromDate, toDate);

            ServicesReportHead reportHead = _journalBusiness.GetBranchInformation(User.OrgId, User.BranchId);
            reportHead.ReportImage = Utility.GetImageBytes(User.LogoPaths[0]);
            List<ServicesReportHead> servicesReportHeads = new List<ServicesReportHead>();
            servicesReportHeads.Add(reportHead);

            LocalReport localReport = new LocalReport();
            string reportPath = Server.MapPath("~/Reports/Accounts/rptJournalVoucherReport.rdlc");

            if (System.IO.File.Exists(reportPath))
            {
                localReport.ReportPath = reportPath;
                ReportDataSource dataSource1 = new ReportDataSource("Journal", journal);
                ReportDataSource dataSource2 = new ReportDataSource("ServicesReportHead", servicesReportHeads);
                localReport.DataSources.Clear();
                localReport.DataSources.Add(dataSource1);
                localReport.DataSources.Add(dataSource2);
                localReport.Refresh();
                localReport.DisplayName = "Stock";

                string mimeType;
                string encoding;
                string fileNameExtension;
                Warning[] warnings;
                string[] streams;
                byte[] renderedBytes;

                renderedBytes = localReport.Render(
                    rptType,
                    "",
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings);
                return File(renderedBytes, mimeType);
            }
            return new EmptyResult();
        }

        public ActionResult GetLedgerReports(long ddlLedgerAccountName, string fromDate, string toDate, string rptType)
        {
            string file = string.Empty;
            List<DetailsLedgerDTO> detailslist = new List<DetailsLedgerDTO>();
            DetailsLedgerDTO details = new DetailsLedgerDTO();
            double totaldebit = 0;
            double totalcredit = 0;
            double debit = 0;
            double credit = 0;
            
            IEnumerable<JournalDTO> journal = _journalBusiness.LedgerList(ddlLedgerAccountName, User.OrgId, fromDate, toDate);
            double d = journal.Sum(a => (a.Debit));
            double c = journal.Sum(b => (b.Credit));
            double sumd = journal.Sum(s => (s.Debit - s.Credit));
            double sumc = journal.Sum(s => (s.Credit - s.Debit));
            string acname = journal.LastOrDefault().AccountName;
            if (sumd > 0)
            {
                credit = sumd;
            }
            else
            {
                credit = 0;
            }
            if (sumc > 0)
            {
                debit = sumc;
            }
            else
            {
                debit = 0;
            }
            if (sumd > 0)
            {
                totalcredit = c + sumd;
            }
            else
            {

                totalcredit = c;
            }
            if (sumc > 0)
            {
                totaldebit = d + sumc;
            }
            else
            {

                totaldebit = d;
            }
            details.TotalDebit = totaldebit;
            details.TotalCredit = totalcredit;
            details.Debit = debit;
            details.Credit = credit;
            details.FromDate = fromDate;
            details.ToDate = toDate;
            details.AccountName = acname;
            detailslist.Add(details);


            //IEnumerable<JournalDTO> journal = _journalBusiness.LedgerList(ddlLedgerAccountName, User.OrgId, fromDate, toDate);

            ServicesReportHead reportHead = _journalBusiness.GetBranchInformation(User.OrgId, User.BranchId);
            reportHead.ReportImage = Utility.GetImageBytes(User.LogoPaths[0]);
            List<ServicesReportHead> servicesReportHeads = new List<ServicesReportHead>();
            servicesReportHeads.Add(reportHead);

            LocalReport localReport = new LocalReport();
            string reportPath = Server.MapPath("~/Reports/Accounts/rptLedgerReport.rdlc");

            if (System.IO.File.Exists(reportPath))
            {
                localReport.ReportPath = reportPath;
                ReportDataSource dataSource1 = new ReportDataSource("Journal", journal);
                ReportDataSource dataSource2 = new ReportDataSource("ServicesReportHead", servicesReportHeads);
                ReportDataSource dataSource3 = new ReportDataSource("LedgerDetails", detailslist);
                localReport.DataSources.Clear();
                localReport.DataSources.Add(dataSource1);
                localReport.DataSources.Add(dataSource2);
                localReport.DataSources.Add(dataSource3);
                localReport.Refresh();
                localReport.DisplayName = "Ledger";

                string mimeType;
                string encoding;
                string fileNameExtension;
                Warning[] warnings;
                string[] streams;
                byte[] renderedBytes;

                renderedBytes = localReport.Render(
                    rptType,
                    "",
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings);
                return File(renderedBytes, mimeType);
            }
            return new EmptyResult();
        }

        public ActionResult CashbookReport(string fromDate, string toDate, string rptType)
        {
            string file = string.Empty;
            List<DetailsLedgerDTO> detailslist = new List<DetailsLedgerDTO>();
            DetailsLedgerDTO details = new DetailsLedgerDTO();
            double totaldebit = 0;
            double totalcredit = 0;
            double debit = 0;
            double credit = 0;

            IEnumerable<JournalDTO> cash = _journalBusiness.CashVoucherList(User.OrgId, fromDate, toDate);
            double d = cash.Sum(a => (a.Debit));
            double c = cash.Sum(b => (b.Credit));
            double sumd = cash.Sum(s => (s.Debit - s.Credit));
            double sumc = cash.Sum(s => (s.Credit - s.Debit));
            if (sumd > 0)
            {
                credit = sumd;
            }
            else
            {
                credit = 0;
            }
            if (sumc > 0)
            {
                debit = sumc;
            }
            else
            {
                debit = 0;
            }
            if (sumd > 0)
            {
                totalcredit = c + sumd;
            }
            else
            {

                totalcredit = c;
            }
            if (sumc > 0)
            {
                totaldebit = d + sumc;
            }
            else
            {

                totaldebit = d;
            }
            details.TotalDebit = totaldebit;
            details.TotalCredit = totalcredit;
            details.Debit = debit;
            details.Credit = credit;
            details.FromDate = fromDate;
            details.ToDate = toDate;
            detailslist.Add(details);

            ServicesReportHead reportHead = _journalBusiness.GetBranchInformation(User.OrgId, User.BranchId);
            reportHead.ReportImage = Utility.GetImageBytes(User.LogoPaths[0]);
            List<ServicesReportHead> accountsReportHead = new List<ServicesReportHead>();
            accountsReportHead.Add(reportHead);

            LocalReport localReport = new LocalReport();
            string reportPath = Server.MapPath("~/Reports/Accounts/rptCashbookReport.rdlc");

            if (System.IO.File.Exists(reportPath))
            {
                localReport.ReportPath = reportPath;
                ReportDataSource dataSource1 = new ReportDataSource("Cashbook", cash);
                ReportDataSource dataSource2 = new ReportDataSource("AccountsReportHead", accountsReportHead);
                ReportDataSource dataSource3 = new ReportDataSource("Cashbookdetails", detailslist);
                localReport.DataSources.Clear();
                localReport.DataSources.Add(dataSource1);
                localReport.DataSources.Add(dataSource2);
                localReport.DataSources.Add(dataSource3);
                localReport.Refresh();
                localReport.DisplayName = "Cashbook";

                string mimeType;
                string encoding;
                string fileNameExtension;
                Warning[] warnings;
                string[] streams;
                byte[] renderedBytes;

                renderedBytes = localReport.Render(
                    rptType,
                    "",
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings);
                return File(renderedBytes, mimeType);
            }
            return new EmptyResult();
        }
        #endregion

        #region CheckBook Entry
        [HttpGet]
        public ActionResult GetChequeBookList(string flag, string accName, string accNo, string chqType, string bankName, string chqNo, string fromDate, string toDate)
        {
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlAllBankName = _accountsHeadBusiness.GetAllBankName(User.OrgId).Select(acc => new SelectListItem { Text = acc.AccountName, Value = acc.AccountName }).ToList();
                ViewBag.ddlChequeType = Utility.ListOfChequeType().Select(r => new SelectListItem { Text = r.text, Value = r.value }).ToList();
                return View();
            }
            else
            {
                var dto = _chequeBookBusiness.GetChequeBookList(User.OrgId, accName, accNo, chqType, bankName, chqNo, fromDate, toDate);
                List<ChequeBookViewModel> viewModel = new List<ChequeBookViewModel>();
                AutoMapper.Mapper.Map(dto, viewModel);
                return PartialView("_GetChequeBookList", viewModel);
            }
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveChequeBook(ChequeBookViewModel chequeBookViewModel)
        {
            bool isSuccess = false;
            if (ModelState.IsValid)
            {
                try
                {
                    ChequeBookDTO dto = new ChequeBookDTO();
                    AutoMapper.Mapper.Map(chequeBookViewModel, dto);
                    isSuccess = _chequeBookBusiness.SaveChequeBook(dto, User.UserId, User.OrgId);
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                }
            }
            return Json(isSuccess);
        }
        #endregion

        #region Supplier & Customer List
        [HttpGet]
        public ActionResult GetAllCustomerAndSupplier(string flag)
        {
            if (string.IsNullOrEmpty(flag))
            {

            }
            else 
                if(!string.IsNullOrEmpty(flag) && flag == "supplier")
            {
                var dto = _supplierBusiness.GetAllSupplierList(User.OrgId);
                List<AccountsSupplierViewModel> supplierViews = new List<AccountsSupplierViewModel>(); 
                AutoMapper.Mapper.Map(dto, supplierViews);
                return PartialView("_Supplier", supplierViews);
            }
            else
                if(!string.IsNullOrEmpty(flag) && flag == "customer")
            {
                var dtos = _customersBusiness.GetAllCustomerList(User.OrgId);
                List<AccountsCustomerViewModel> customerViews = new List<AccountsCustomerViewModel>();
                AutoMapper.Mapper.Map(dtos, customerViews);
                return PartialView("_Customer", customerViews);
            }
            return View();
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveAccountsCustomer(AccountsCustomerViewModel viewModel)
        {
            bool isSuccess = false;
            if (ModelState.IsValid)
            {
                try
                {
                    AccountsCustomerDTO dto = new AccountsCustomerDTO();
                    AutoMapper.Mapper.Map(viewModel, dto);
                    isSuccess = _customersBusiness.SaveAccountsCustomers(dto, User.UserId, User.OrgId);
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                }
            }
            return Json(isSuccess);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveAccountsSupplier(AccountsSupplierViewModel viewModel)
        {
            bool isSuccess = false;
            if (ModelState.IsValid)
            {
                try
                {
                    AccountsSupplierDTO dto = new AccountsSupplierDTO();
                    AutoMapper.Mapper.Map(viewModel, dto);
                    isSuccess = _supplierBusiness.SaveAccountsSuppliers(dto, User.UserId, User.OrgId);
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                }
            }
            return Json(isSuccess);
        }
        #endregion
    }
}