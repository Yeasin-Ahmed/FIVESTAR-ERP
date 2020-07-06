using ERPBLL.Configuration.Interface;
using ERPBLL.FrontDesk.Interface;
using ERPBO.Common;
using ERPBO.Configuration.DTOModels;
using ERPBO.Configuration.ViewModels;
using ERPBO.FrontDesk.ViewModels;
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
        private readonly IMobilePartStockInfoBusiness _mobilePartStockInfoBusiness;
        private readonly IJobOrderBusiness _jobOrderBusiness;
        private readonly IFaultBusiness _faultBusiness;
        private readonly IServiceBusiness _serviceBusiness;

        public Common2Controller(IAccessoriesBusiness accessoriesBusiness, IClientProblemBusiness clientProblemBusiness, IMobilePartBusiness mobilePartBusiness, ICustomerBusiness customerBusiness, ITechnicalServiceBusiness technicalServiceBusiness, ICustomerServiceBusiness customerServiceBusiness,IBranchBusiness2 branchBusiness, IMobilePartStockInfoBusiness mobilePartStockInfoBusiness, IJobOrderBusiness jobOrderBusiness, IFaultBusiness faultBusiness, IServiceBusiness serviceBusiness)
        {
            this._accessoriesBusiness = accessoriesBusiness;
            this._clientProblemBusiness = clientProblemBusiness;
            this._mobilePartBusiness = mobilePartBusiness;
            this._customerBusiness = customerBusiness;
            this._technicalServiceBusiness = technicalServiceBusiness;
            this._customerServiceBusiness = customerServiceBusiness;
            this._branchBusiness = branchBusiness;
            this._mobilePartStockInfoBusiness = mobilePartStockInfoBusiness;
            this._jobOrderBusiness = jobOrderBusiness;
            this._faultBusiness = faultBusiness;
            this._serviceBusiness = serviceBusiness;
        }

        #region Configuration Module

        [HttpPost]
        public ActionResult GetCustomerByMobileNo(string mobileNo)
        {
            var customer =_customerBusiness.GetCustomerByMobileNo(mobileNo, User.OrgId,User.BranchId);
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
        [HttpPost]
        public ActionResult GetReferencesByIMEI(string imei)
        {
            var Refe = _jobOrderBusiness.GetReferencesNumberByIMEI(imei, User.OrgId,User.BranchId);
            JobOrderViewModel viewModel = new JobOrderViewModel();
            if (Refe != null)
            {
                viewModel.JodOrderId = Refe.JodOrderId;
                viewModel.IMEI = Refe.IMEI;
                viewModel.ReferenceNumber = Refe.JobOrderCode;
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
        public ActionResult IsDuplicateCustomerPhone(string customerPhone, long id)
        {
            bool isExist = _customerBusiness.IsDuplicateCustomerPhone(customerPhone, id, User.OrgId,User.BranchId);
            return Json(isExist);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult IsDuplicateTSName(string name, long id)
        {
            bool isExist = _technicalServiceBusiness.IsDuplicateTechnicalName(name, id, User.OrgId,User.BranchId);
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
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult IsDuplicateFaultName(string faultName, long id)
        {
            bool isExist = _faultBusiness.IsDuplicateFaultName(faultName, id, User.OrgId);
            return Json(isExist);
        }

        [HttpPost]
        public ActionResult GetCostPriceForDDL(long partsId)
        {
            var cost = _mobilePartStockInfoBusiness.GetAllMobilePartStockInfoByOrgId(User.OrgId,User.BranchId).AsEnumerable();
            var dropDown = cost.Where(i => i.MobilePartId == partsId).Select(i => new Dropdown { text = i.CostPrice.ToString(), value = i.CostPrice.ToString() }).ToList();
            return Json(dropDown);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult GetPartsStockByPrice(long warehouseId,long partsId,double cprice)
        {
            var stock = _mobilePartStockInfoBusiness.GetAllMobilePartStockInfoByInfoId(warehouseId, partsId, cprice, User.OrgId, User.BranchId);
            int total = 0;
            if(stock != null)
            {
                total = (stock.StockInQty - (stock.StockOutQty)).Value;
            }
            return Json(total);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult GetSellPriceByCostPrice(long warehouseId, long partsId, double sprice)
        {
            var price = _mobilePartStockInfoBusiness.GetAllMobilePartStockInfoBySellPrice(warehouseId, partsId, sprice, User.OrgId, User.BranchId);
            MobilePartStockDetailDTO detailDTO = new MobilePartStockDetailDTO();
            price.MobilePartStockInfoId = detailDTO.MobilePartStockDetailId;
            price.SellPrice = detailDTO.SellPrice;
            return Json(detailDTO);
        }

        #region Front-Desk Module

        #endregion
    }
}