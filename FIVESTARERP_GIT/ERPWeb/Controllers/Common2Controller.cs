using ERPBLL.Accounts.Interface;
using ERPBLL.Common;
using ERPBLL.Configuration.Interface;
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
        public readonly IRepairBusiness _repairBusiness;
        private readonly IHandsetChangeTraceBusiness _handsetChangeTraceBusiness;
        private readonly IJournalBusiness _journalBusiness;
        private readonly ICustomersBusiness _customersBusiness;
        private readonly ERPBLL.Accounts.Interface.ISupplierBusiness _suppliersBusiness;
        private readonly IDealerSSBusiness _dealerSSBusiness;
        private readonly IModelSSBusiness _modelSSBusiness;
        //Nishad
        private readonly ERPBLL.Configuration.Interface.IHandSetStockBusiness _handSetStockBusiness;
        private readonly IFaultyStockInfoBusiness _faultyStockInfoBusiness;

        public Common2Controller(IAccessoriesBusiness accessoriesBusiness, IClientProblemBusiness clientProblemBusiness, IMobilePartBusiness mobilePartBusiness, ICustomerBusiness customerBusiness, ITechnicalServiceBusiness technicalServiceBusiness, ICustomerServiceBusiness customerServiceBusiness,IBranchBusiness2 branchBusiness, IMobilePartStockInfoBusiness mobilePartStockInfoBusiness, IJobOrderBusiness jobOrderBusiness, IFaultBusiness faultBusiness, IServiceBusiness serviceBusiness, IJobOrderProblemBusiness jobOrderProblemBusiness, IJobOrderFaultBusiness jobOrderFaultBusiness, IJobOrderServiceBusiness jobOrderServiceBusiness, IDescriptionBusiness descriptionBusiness, IInvoiceInfoBusiness invoiceInfoBusiness, IInvoiceDetailBusiness invoiceDetailBusiness, IServicesWarehouseBusiness servicesWarehouseBusiness, IRepairBusiness repairBusiness, ERPBLL.Configuration.Interface.IHandSetStockBusiness handSetStockBusiness, IFaultyStockInfoBusiness faultyStockInfoBusiness, IHandsetChangeTraceBusiness handsetChangeTraceBusiness, IJournalBusiness journalBusiness, ICustomersBusiness customersBusiness, ERPBLL.Accounts.Interface.ISupplierBusiness suppliersBusiness, IDealerSSBusiness dealerSSBusiness, IModelSSBusiness modelSSBusiness)
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
            this._repairBusiness = repairBusiness;
            this._handSetStockBusiness = handSetStockBusiness;
            this._faultyStockInfoBusiness = faultyStockInfoBusiness;
            this._handsetChangeTraceBusiness = handsetChangeTraceBusiness;
            this._journalBusiness = journalBusiness;
            this._customersBusiness = customersBusiness;
            this._suppliersBusiness = suppliersBusiness;
            this._dealerSSBusiness = dealerSSBusiness;
            this._modelSSBusiness = modelSSBusiness;
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

            var refeCode = _jobOrderBusiness.GetRefeNumberCount(imei, User.BranchId, User.OrgId);
            JobOrderViewModel viewModel = new JobOrderViewModel();
            if (Refe != null)
            {
                viewModel.JodOrderId = Refe.JodOrderId;
                viewModel.IMEI = Refe.IMEI;
                viewModel.ReferenceNumber =Refe.JobOrderCode+"-"+ refeCode.Count();
                viewModel.IMEI2 = Refe.IMEI2;
                viewModel.Type = Refe.Type;
                viewModel.ModelColor = Refe.ModelColor;
                viewModel.JobOrderType = Refe.JobOrderType;
                viewModel.Remarks = Refe.Remarks;
                viewModel.WarrantyDate = Refe.WarrantyDate;
                viewModel.DescriptionId = Refe.DescriptionId;
                viewModel.ModelName = (_modelSSBusiness.GetModelById(Refe.DescriptionId,User.OrgId).ModelName);
                viewModel.CustomerName = Refe.CustomerName;
                viewModel.Address = Refe.Address;
                viewModel.MobileNo = Refe.MobileNo;
                viewModel.CustomerType = Refe.CustomerType;
                viewModel.CourierName = Refe.CourierName;
                viewModel.CourierNumber = Refe.CourierNumber;
                viewModel.ApproxBill = Refe.ApproxBill;
                viewModel.JobSource = Refe.JobSource;

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
                viewModel.ModelName = (_modelSSBusiness.GetModelById(Refe.DescriptionId, User.OrgId).ModelName);
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
        public ActionResult IsHandsetCustomerPending(string imei)
        {
            bool isExist = _handSetStockBusiness.IsHandsetCustomerPrndingIMEI(imei, "Customer-Pending", User.OrgId);
            return Json(isExist);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult IsHandsetStockCheck(string imei)
        {
            bool isExist = _handSetStockBusiness.IsHandsetStockIMEICheck(imei, User.OrgId);
            return Json(isExist);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult IsExistsInHandsetStock(string imei)
        {
            bool isExist = _handSetStockBusiness.IsExistsHandsetStockIMEI(imei, User.OrgId, StockStatus.StockIn);
            return Json(isExist);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult IsDuplicateHandsetStockIMEI(string imei, long id)
        {
            bool isExist = _handSetStockBusiness.IsDuplicateHandsetStockIMEI(imei, id, User.OrgId);
            return Json(isExist);
        }
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
        [HttpPost,ValidateJsonAntiForgeryToken]
        public ActionResult IsDuplicateFaultNameL(string faultName, long id)
        {
            bool isExist = _faultBusiness.IsDuplicateFaultName(faultName, id, User.OrgId);
            return Json(isExist);
        }
        [HttpPost,ValidateJsonAntiForgeryToken]
        public ActionResult IsDuplicateServicesNameC(string servicesName,long id)
        {
            bool isExist = _serviceBusiness.IsDuplicateServiceName(servicesName,id,User.OrgId);
            return Json(isExist);
        }
        [HttpPost,ValidateJsonAntiForgeryToken]
        public ActionResult IsDuplicateRepairNameC(string repairName,long id)
        {
            bool isExist = _repairBusiness.IsDuplicateRepairName(repairName,id,User.OrgId);
            return Json(isExist);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult ExistJobOrderForIMEI(long jobId)
        {
            bool isExist = _handsetChangeTraceBusiness.ExitJobOrderForIMEI(jobId, User.OrgId,User.BranchId);
            return Json(isExist);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult IsDuplicateIMEI1(long jobId,string imei1)
        {
            bool isExist = _handsetChangeTraceBusiness.IsDuplicateIMEI1(jobId, imei1, User.OrgId, User.BranchId);
            return Json(isExist);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult IsDuplicateIMEI2(long jobId, string imei2)
        {
            bool isExist = _handsetChangeTraceBusiness.IsDuplicateIMEI2(jobId, imei2, User.OrgId, User.BranchId);
            return Json(isExist);
        }
        #endregion

        [HttpPost]
        public ActionResult GetDealer(long dealerId)
        {
            var dealer = _dealerSSBusiness.GetDealerById(dealerId, User.OrgId);
            DealerSSViewModel viewModel = new DealerSSViewModel();
            if (dealer != null)
            {
                viewModel.DealerName = dealer.DealerName;
                viewModel.MobileNo = dealer.MobileNo;
                viewModel.Address = dealer.Address;
                viewModel.DealerId = dealer.DealerId;
            }
            return Json(viewModel);
        }

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
            var stock = _mobilePartStockInfoBusiness.GetAllMobilePartStockByParts(warehouse.SWarehouseId, partsId, User.OrgId, warehouse.BranchId, jobOrder.DescriptionId).Select(s => s.StockInQty - s.StockOutQty).Sum();
            if (stock == null)
            {
                stock = 0;
            }
            return Json(stock);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult GetStockForAccessoriesSells(long partsId,long modelId)
        {
           // var jobOrder = _jobOrderBusiness.GetJobOrderById(jobOrderId, User.OrgId);
            var warehouse = _servicesWarehouseBusiness.GetWarehouseOneByOrgId(User.OrgId, User.BranchId);
            var stock = _mobilePartStockInfoBusiness.GetAllMobilePartStockByPartsSales(warehouse.SWarehouseId, partsId, User.OrgId, warehouse.BranchId,modelId).Select(s => s.StockInQty - s.StockOutQty).Sum();
            if (stock == null)
            {
                stock = 0;
            }
            return Json(stock);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult GetFaultyStockForAccessoriesSells(long partsId,long modelId)
        {
            var warehouse = _servicesWarehouseBusiness.GetWarehouseOneByOrgId(User.OrgId, User.BranchId);
            var stock = _faultyStockInfoBusiness.GetAllFaultyMobilePartStockByParts(warehouse.SWarehouseId, partsId, User.OrgId, warehouse.BranchId,modelId).Select(s => s.StockInQty - s.StockOutQty).Sum();
            if (stock == 0)
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
        public ActionResult GetSellPriceForDDL(long partsId,long modelId)
        {
            var sell = _mobilePartStockInfoBusiness.GetAllMobilePartStockInfoByOrgId(User.OrgId, User.BranchId).AsEnumerable();
            var dropDown = sell.Where(i => i.MobilePartId == partsId && i.DescriptionId==modelId).Select(i => new Dropdown { text = i.SellPrice.ToString(), value = i.SellPrice.ToString() }).ToList();
            return Json(dropDown);
        }
        [HttpPost]
        public ActionResult GetFaultyStockSellPriceForDDL(long partsId, long modelId)
        {
            var sell = _faultyStockInfoBusiness.GetAllFaultyStockInfoByOrgId(User.OrgId, User.BranchId).AsEnumerable();
            var dropDown = sell.Where(i => i.PartsId == partsId && i.DescriptionId==modelId).Select(i => new Dropdown { text = i.SellPrice.ToString(), value = i.SellPrice.ToString() }).ToList();
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
        public ActionResult GetSellPriceByCostPrice(long warehouseId, long partsId, double cprice)
        {
            var price = _mobilePartStockInfoBusiness.GetAllMobilePartStockInfoBySellPrice(warehouseId, partsId, cprice, User.OrgId, User.BranchId);
            //MobilePartStockDetailDTO detailDTO = new MobilePartStockDetailDTO();
            //price.MobilePartStockInfoId = detailDTO.MobilePartStockDetailId;
            //price.SellPrice = detailDTO.SellPrice;
            return Json(price.SellPrice);
        }

        #endregion
        #region Accounts
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult GetDebitDueAmount(long accountId)
        {
            var acount = _journalBusiness.GetDebitDueAmount(accountId,User.OrgId);
            double amount = 0;
            if (acount != null)
            {
                 amount = acount.Sum(s => (s.Debit - s.Credit));
            }
            return Json(amount);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult GetCreditDueAmount(long accountId)
        {
            var acount = _journalBusiness.GetDebitDueAmount(accountId, User.OrgId);
            double amount = 0;
            if (acount != null)
            {
                amount = acount.Sum(s => (s.Credit - s.Debit)); 
            }
            return Json(amount);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult IsDuplicateCustomerMobile(string mobile, long id)
        {
            bool isExist = _customersBusiness.IsDuplicateCustomerMobile(mobile, id, User.OrgId);
            return Json(isExist);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult IsDuplicateSupplierMobile(string mobile, long id)
        {
            bool isExist = _suppliersBusiness.IsDuplicateSupplierMobile(mobile, id, User.OrgId);
            return Json(isExist);
        }
        #endregion
    }
}