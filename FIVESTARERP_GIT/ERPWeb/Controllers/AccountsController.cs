using ERPBLL.Accounts.Interface;
using ERPBLL.Common;
using ERPBO.Accounts.DTOModels;
using ERPBO.Accounts.ViewModels;
using ERPWeb.Filters;
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
        // GET: Accounts
        public AccountsController(IAccountsHeadBusiness accountsHeadBusiness, IJournalBusiness journalBusiness)
        {
            this._accountsHeadBusiness = accountsHeadBusiness;
            this._journalBusiness = journalBusiness;
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
    }
}