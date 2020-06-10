using ERPBLL.Configuration.Interface;
using ERPBO.Configuration.ViewModels;
using ERPWeb.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERPWeb.Controllers
{
    public class Common2Controller : BaseController
    {
        // This controller is only for Configuration & FrontDesk Modules
        // Purpose of this Controller is for Unique checking, 
        // validates data, dropdown data, autocomplete data, related data. 
        // It acts a service class.

        //Configuration
        private readonly IAccessoriesBusiness _accessoriesBusiness;
        private readonly IClientProblemBusiness _clientProblemBusiness;
        private readonly IMobilePartBusiness _mobilePartBusiness;
        private readonly ICustomerBusiness _customerBusiness;
        private readonly ITechnicalServiceBusiness _technicalServiceBusiness;
        private readonly ICustomerServiceBusiness _customerServiceBusiness;
        private readonly IBranchBusiness2 _branchBusiness;

        public Common2Controller(IAccessoriesBusiness accessoriesBusiness, IClientProblemBusiness clientProblemBusiness, IMobilePartBusiness mobilePartBusiness, ICustomerBusiness customerBusiness, ITechnicalServiceBusiness technicalServiceBusiness, ICustomerServiceBusiness customerServiceBusiness,IBranchBusiness2 branchBusiness)
        {
            this._accessoriesBusiness = accessoriesBusiness;
            this._clientProblemBusiness = clientProblemBusiness;
            this._mobilePartBusiness = mobilePartBusiness;
            this._customerBusiness = customerBusiness;
            this._technicalServiceBusiness = technicalServiceBusiness;
            this._customerServiceBusiness = customerServiceBusiness;
            this._branchBusiness = branchBusiness;
        }

        #region Configuration Module

        [HttpPost]
        public ActionResult GetCustomerByMobileNo(string mobileNo)
        {
            var customer =_customerBusiness.GetCustomerByMobileNo(mobileNo, User.OrgId);
            CustomerViewModel viewModel = new CustomerViewModel();
            if(customer != null)
            {
                viewModel.CustomerName = customer.CustomerName;
                viewModel.CustomerPhone = customer.CustomerPhone;
                viewModel.CustomerAddress = customer.CustomerAddress;
                viewModel.CustomerId = customer.CustomerId;
            }
            return Json(viewModel);
        }

        #endregion
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult IsDuplicateAccessoriesName(string accessoriesName, long id)
        {
            bool isExist = _accessoriesBusiness.IsDuplicateAccessoriesName(accessoriesName, id, User.OrgId);
            return Json(isExist);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult IsDuplicateProblemName(string problemName, long id)
        {
            bool isExist = _clientProblemBusiness.IsDuplicateProblemName(problemName, id, User.OrgId);
            return Json(isExist);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult IsDuplicateMobilePartName(string mobilePartName, long id)
        {
            bool isExist = _mobilePartBusiness.IsDuplicateMobilePart(mobilePartName, id, User.OrgId);
            return Json(isExist);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult IsDuplicateCustomer(string customerName, long id)
        {
            bool isExist = _customerBusiness.IsDuplicateCustomerName(customerName, id, User.OrgId);
            return Json(isExist);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult IsDuplicateTSName(string name, long id)
        {
            bool isExist = _technicalServiceBusiness.IsDuplicateTechnicalName(name, id, User.OrgId);
            return Json(isExist);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult IsDuplicateCsName(string name, long id)
        {
            bool isExist = _customerServiceBusiness.IsDuplicateCustomerServiceName(name, id, User.OrgId);
            return Json(isExist);
        }
        public ActionResult IsDuplicateBranchName(string branchName, long id)
        {
            bool isExist = _branchBusiness.IsDuplicateBranchName(branchName, id, User.OrgId);
            return Json(isExist);
        }
        #region Front-Desk Module

        #endregion
    }
}