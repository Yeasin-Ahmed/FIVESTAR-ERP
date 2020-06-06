using ERPBLL.Configuration.Interface;
using ERPBO.Configuration.ViewModels;
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

        public Common2Controller(IAccessoriesBusiness accessoriesBusiness, IClientProblemBusiness clientProblemBusiness, IMobilePartBusiness mobilePartBusiness, ICustomerBusiness customerBusiness, ITechnicalServiceBusiness technicalServiceBusiness, ICustomerServiceBusiness customerServiceBusiness)
        {
            this._accessoriesBusiness = accessoriesBusiness;
            this._clientProblemBusiness = clientProblemBusiness;
            this._mobilePartBusiness = mobilePartBusiness;
            this._customerBusiness = customerBusiness;
            this._technicalServiceBusiness = technicalServiceBusiness;
            this._customerServiceBusiness = customerServiceBusiness;
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

        #region Front-Desk Module

        #endregion
    }
}