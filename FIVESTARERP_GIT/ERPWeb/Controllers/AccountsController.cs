using ERPBLL.Accounts.Interface;
using ERPBLL.Common;
using ERPBO.Accounts.DTOModels;
using ERPBO.Accounts.ViewModels;
using ERPBO.Common;
using ERPBO.FrontDesk.ReportModels;
using ERPWeb.Filters;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERPWeb.Controllers
{
    [CustomAuthorize]
    public class AccountsController : BaseController
    {
        private readonly IAccountsHeadBusiness _accountsHeadBusiness;
        private readonly IJournalBusiness _journalBusiness;
        private readonly IFinanceYearBusiness _financeYearBusiness;
        // GET: Accounts
        public AccountsController(IAccountsHeadBusiness accountsHeadBusiness, IJournalBusiness journalBusiness, IFinanceYearBusiness financeYearBusiness)
        {
            this._accountsHeadBusiness = accountsHeadBusiness;
            this._journalBusiness = journalBusiness;
            this._financeYearBusiness = financeYearBusiness;
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
        public ActionResult JournalList(string flag_, string fromDate, string toDate)
        {
            if (string.IsNullOrEmpty(flag_))
            {
                return View();
            }
            else
            {
                var dto = _journalBusiness.GetJournalList(User.OrgId, fromDate, toDate);
                List<JournalViewModel> viewModels = new List<JournalViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_JournalList", viewModels);
            }
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
    }
}