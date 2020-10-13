﻿using ERPBLL.Configuration.Interface;
using ERPBLL.FrontDesk.Interface;
using ERPBLL.Inventory.Interface;
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
    [CustomAuthorize]
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
        private readonly IJobOrderProblemBusiness _jobOrderProblemBusiness;
        private readonly IJobOrderFaultBusiness _jobOrderFaultBusiness;
        private readonly IJobOrderServiceBusiness _jobOrderServiceBusiness;
        private readonly IDescriptionBusiness _descriptionBusiness;
        private readonly IInvoiceInfoBusiness _invoiceInfoBusiness;
        private readonly IInvoiceDetailBusiness _invoiceDetailBusiness;
        private readonly IServicesWarehouseBusiness _servicesWarehouseBusiness;

        public Common2Controller(IAccessoriesBusiness accessoriesBusiness, IClientProblemBusiness clientProblemBusiness, IMobilePartBusiness mobilePartBusiness, ICustomerBusiness customerBusiness, ITechnicalServiceBusiness technicalServiceBusiness, ICustomerServiceBusiness customerServiceBusiness,IBranchBusiness2 branchBusiness, IMobilePartStockInfoBusiness mobilePartStockInfoBusiness, IJobOrderBusiness jobOrderBusiness, IFaultBusiness faultBusiness, IServiceBusiness serviceBusiness, IJobOrderProblemBusiness jobOrderProblemBusiness, IJobOrderFaultBusiness jobOrderFaultBusiness, IJobOrderServiceBusiness jobOrderServiceBusiness, IDescriptionBusiness descriptionBusiness, IInvoiceInfoBusiness invoiceInfoBusiness, IInvoiceDetailBusiness invoiceDetailBusiness, IServicesWarehouseBusiness servicesWarehouseBusiness)
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
            this._jobOrderProblemBusiness = jobOrderProblemBusiness;
            this._jobOrderFaultBusiness = jobOrderFaultBusiness;
            this._jobOrderServiceBusiness = jobOrderServiceBusiness;
            this._descriptionBusiness = descriptionBusiness;
            this._invoiceInfoBusiness = invoiceInfoBusiness;
            this._invoiceDetailBusiness = invoiceDetailBusiness;
            this._servicesWarehouseBusiness = servicesWarehouseBusiness;
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
                viewModel.IMEI2 = Refe.IMEI2;
                viewModel.Type = Refe.Type;
                viewModel.ModelColor = Refe.ModelColor;
                viewModel.JobOrderType = Refe.JobOrderType;
                viewModel.Remarks = Refe.Remarks;
                viewModel.WarrantyDate = Refe.WarrantyDate;
                viewModel.DescriptionId = Refe.DescriptionId;
                viewModel.ModelName = (_descriptionBusiness.GetDescriptionOneByOrdId(Refe.DescriptionId,User.OrgId).DescriptionName);
                viewModel.CustomerName = Refe.CustomerName;
                viewModel.Address = Refe.Address;
                viewModel.MobileNo = Refe.MobileNo;
                viewModel.CustomerType = Refe.CustomerType;

                viewModel.CourierName = Refe.CourierName;
                viewModel.CourierNumber = Refe.CourierNumber;
                viewModel.ApproxBill = Refe.ApproxBill;

            }
            return Json(viewModel);
        }
        [HttpPost]
        public ActionResult GetReferencesByIMEI2(string imei2)
        {
            var Refe = _jobOrderBusiness.GetReferencesNumberByIMEI2(imei2, User.OrgId, User.BranchId);
            JobOrderViewModel viewModel = new JobOrderViewModel();
            if (Refe != null)
            {
                viewModel.JodOrderId = Refe.JodOrderId;
                viewModel.IMEI = Refe.IMEI;
                viewModel.ReferenceNumber = Refe.JobOrderCode;
                viewModel.IMEI2 = Refe.IMEI2;
                viewModel.Type = Refe.Type;
                viewModel.ModelColor = Refe.ModelColor;
                viewModel.JobOrderType = Refe.JobOrderType;
                viewModel.Remarks = Refe.Remarks;
                viewModel.WarrantyDate = Refe.WarrantyDate;
                viewModel.DescriptionId = Refe.DescriptionId;
                viewModel.ModelName = (_descriptionBusiness.GetDescriptionOneByOrdId(Refe.DescriptionId, User.OrgId).DescriptionName);
                viewModel.CustomerName = Refe.CustomerName;
                viewModel.Address = Refe.Address;
                viewModel.MobileNo = Refe.MobileNo;
                viewModel.CustomerType = Refe.CustomerType;

                viewModel.CourierName = Refe.CourierName;
                viewModel.CourierNumber = Refe.CourierNumber;
                viewModel.ApproxBill = Refe.ApproxBill;
            }
            return Json(viewModel);
        }
        [HttpPost]
        public ActionResult GetReferencesByMobileNumber(string mobileNumber)
        {
            var Refe = _jobOrderBusiness.GetReferencesNumberByMobileNumber(mobileNumber, User.OrgId, User.BranchId);
            JobOrderViewModel viewModel = new JobOrderViewModel();
            if (Refe != null)
            {
                viewModel.JodOrderId = Refe.JodOrderId;
                viewModel.IMEI = Refe.IMEI;
                viewModel.ReferenceNumber = Refe.JobOrderCode;
                viewModel.IMEI2 = Refe.IMEI2;
                viewModel.Type = Refe.Type;
                viewModel.ModelColor = Refe.ModelColor;
                viewModel.JobOrderType = Refe.JobOrderType;
                viewModel.Remarks = Refe.Remarks;
                viewModel.WarrantyDate = Refe.WarrantyDate;
                viewModel.DescriptionId = Refe.DescriptionId;
                viewModel.ModelName = (_descriptionBusiness.GetDescriptionOneByOrdId(Refe.DescriptionId, User.OrgId).DescriptionName);
                viewModel.CustomerName = Refe.CustomerName;
                viewModel.Address = Refe.Address;
                viewModel.MobileNo = Refe.MobileNo;
            }
            return Json(viewModel);
        }

        [HttpPost]
        public ActionResult GetReferencesByJobOrder(string jobOrder)
        {
            var Refe = _jobOrderBusiness.GetReferencesNumberByJobOrder(jobOrder, User.OrgId, User.BranchId);
            JobOrderViewModel viewModel = new JobOrderViewModel();
            if (Refe != null)
            {
                viewModel.JodOrderId = Refe.JodOrderId;
                viewModel.IMEI = Refe.IMEI;
                viewModel.ReferenceNumber = Refe.JobOrderCode;
                viewModel.IMEI2 = Refe.IMEI2;
                viewModel.Type = Refe.Type;
                viewModel.ModelColor = Refe.ModelColor;
                viewModel.JobOrderType = Refe.JobOrderType;
                viewModel.Remarks = Refe.Remarks;
                viewModel.WarrantyDate = Refe.WarrantyDate;
                viewModel.DescriptionId = Refe.DescriptionId;
                viewModel.ModelName = (_descriptionBusiness.GetDescriptionOneByOrdId(Refe.DescriptionId, User.OrgId).DescriptionName);
                viewModel.CustomerName = Refe.CustomerName;
                viewModel.Address = Refe.Address;
                viewModel.MobileNo = Refe.MobileNo;
                viewModel.CustomerType = Refe.CustomerType;

                viewModel.CourierName = Refe.CourierName;
                viewModel.CourierNumber = Refe.CourierNumber;
                viewModel.ApproxBill = Refe.ApproxBill;

            }
            return Json(viewModel);
        }

        #endregion

        #region Duplicate Check
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
        public ActionResult IsDuplicateMobilePartCode(string partsCode, long id)
        {
            bool isExist = _mobilePartBusiness.IsDuplicateMobilePartCode(partsCode, id, User.OrgId);
            return Json(isExist);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult IsDuplicateCustomerPhone(string customerPhone, long id)
        {
            bool isExist = _customerBusiness.IsDuplicateCustomerPhone(customerPhone, id, User.OrgId, User.BranchId);
            return Json(isExist);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult IsDuplicateTSName(string name, long id)
        {
            bool isExist = _technicalServiceBusiness.IsDuplicateTechnicalName(name, id, User.OrgId, User.BranchId);
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
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult IsDuplicateSymptomId(long id, long prblmId)
        {
            bool isExist = _jobOrderProblemBusiness.IsDuplicateSymptomName(id, prblmId, User.OrgId);
            return Json(isExist);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult IsDuplicateFaultId(long id, long faultId)
        {
            bool isExist = _jobOrderFaultBusiness.IsDuplicateFaultName(id, faultId, User.OrgId);
            return Json(isExist);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult IsDuplicateServicesId(long id, long servicesId)
        {
            bool isExist = _jobOrderServiceBusiness.IsDuplicateServicesName(id, servicesId, User.OrgId);
            return Json(isExist);
        }
        #endregion

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult IsIMEIExistWithRunningJobOrder(string iMEI,long jobOrdeId)
        {
            bool isExist = false;
            if (jobOrdeId == 0)
            {
                isExist = _jobOrderBusiness.IsIMEIExistWithRunningJobOrder(jobOrdeId, iMEI, User.OrgId, User.BranchId);
            }
            return Json(isExist);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult IsIMEI2ExistWithRunningJobOrder(string iMEI2, long jobOrdeId)
        {
            bool isExist = false;
            if (jobOrdeId == 0)
            {
                isExist = _jobOrderBusiness.IsIMEI2ExistWithRunningJobOrder(jobOrdeId, iMEI2, User.OrgId, User.BranchId);
            }
            return Json(isExist);
        }

        #region Front-Desk Module
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult GetPartsStockByParts(long partsId,long jobOrderId)
        {
            var jobOrder = _jobOrderBusiness.GetJobOrderById(jobOrderId,User.OrgId);
            var warehouse = _servicesWarehouseBusiness.GetWarehouseOneByOrgId(User.OrgId, jobOrder.BranchId.Value);
            var stock = _mobilePartStockInfoBusiness.GetAllMobilePartStockByParts(warehouse.SWarehouseId, partsId, User.OrgId, warehouse.BranchId).Select(s => s.StockInQty - s.StockOutQty).Sum();
            if (stock == null)
            {
                stock = 0;
            }
            return Json(stock);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult GetStockForAccessoriesSells(long partsId)
        {
            var warehouse = _servicesWarehouseBusiness.GetWarehouseOneByOrgId(User.OrgId, User.BranchId);
            var stock = _mobilePartStockInfoBusiness.GetAllMobilePartStockByParts(warehouse.SWarehouseId, partsId, User.OrgId, warehouse.BranchId).Select(s => s.StockInQty - s.StockOutQty).Sum();
            if (stock == null)
            {
                stock = 0;
            }
            return Json(stock);
        }

        [HttpPost]
        public ActionResult GetCostPriceForDDL(long partsId)
        {
            var cost = _mobilePartStockInfoBusiness.GetAllMobilePartStockInfoByOrgId(User.OrgId, User.BranchId).AsEnumerable();
            var dropDown = cost.Where(i => i.MobilePartId == partsId).Select(i => new Dropdown { text = i.CostPrice.ToString(), value = i.CostPrice.ToString() }).ToList();
            return Json(dropDown);
        }
        [HttpPost]
        public ActionResult GetSellPriceForDDL(long partsId)
        {
            var sell = _mobilePartStockInfoBusiness.GetAllMobilePartStockInfoByOrgId(User.OrgId, User.BranchId).AsEnumerable();
            var dropDown = sell.Where(i => i.MobilePartId == partsId).Select(i => new Dropdown { text = i.SellPrice.ToString(), value = i.SellPrice.ToString() }).ToList();
            return Json(dropDown);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult GetPartsStockByPrice(long warehouseId, long partsId, double cprice)
        {
            var stock = _mobilePartStockInfoBusiness.GetAllMobilePartStockInfoByInfoId(warehouseId, partsId, cprice, User.OrgId, User.BranchId);
            int total = 0;
            if (stock != null)
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
        #endregion
    }
}