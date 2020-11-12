using ERPBLL.Accounts.Interface;
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
        // GET: Accounts
        public AccountsController(IAccountsHeadBusiness accountsHeadBusiness)
        {
            this._accountsHeadBusiness = accountsHeadBusiness;
        }
        [HttpGet]
        public ActionResult GetAccountsHeadList()
        {
            var dto = _accountsHeadBusiness.AccountsHeadList(User.OrgId);
            List<AccountsHeadViewModel> viewModels = new List<AccountsHeadViewModel>();
            AutoMapper.Mapper.Map(dto, viewModels);
            return View(viewModels);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveAccountsHead(AccountsHeadViewModel accountsHeadViewModel)
        {
            bool isSuccess = false;
            if (ModelState.IsValid)
            {
                try
                {
                    AccountsHeadDTO dto = new AccountsHeadDTO();
                    AutoMapper.Mapper.Map(accountsHeadViewModel,dto);
                    isSuccess = _accountsHeadBusiness.SaveAccountsHead(dto, User.UserId, User.OrgId);
                }
                catch(Exception ex)
                {
                    isSuccess = false;
                }
            }
            return Json(isSuccess);
        }
    }
}