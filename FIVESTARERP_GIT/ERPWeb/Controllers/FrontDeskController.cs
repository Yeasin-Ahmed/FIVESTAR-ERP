using ERPBLL.Common;
using ERPBLL.Configuration.Interface;
using ERPBLL.ControlPanel.Interface;
using ERPBLL.FrontDesk.Interface;
using ERPBLL.Inventory.Interface;
using ERPBLL.ReportSS.Interface;
using ERPBO.Common;
using ERPBO.Configuration.DTOModels;
using ERPBO.Configuration.ViewModels;
using ERPBO.FrontDesk.DomainModels;
using ERPBO.FrontDesk.DTOModels;
using ERPBO.FrontDesk.ReportModels;
using ERPBO.FrontDesk.ViewModels;
using ERPWeb.Filters;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ERPWeb.Controllers
{
    [CustomAuthorize]
    public class FrontDeskController : BaseController
    {
        // Configuation
        private readonly IAccessoriesBusiness _accessoriesBusiness;
        private readonly IClientProblemBusiness _clientProblemBusiness;
        private readonly ITechnicalServiceBusiness _technicalServiceBusiness;
        private readonly ICustomerBusiness _customerBusiness;
        private readonly IServicesWarehouseBusiness _servicesWarehouseBusiness;
        private readonly IBranchBusiness _branchBusiness;
        private readonly IMobilePartBusiness _mobilePartBusiness;
        private readonly IMobilePartStockInfoBusiness _mobilePartStockInfoBusiness;
        private readonly IMobilePartStockDetailBusiness _mobilePartStockDetailBusiness;
        private readonly IFaultBusiness _faultBusiness;
        private readonly IServiceBusiness _serviceBusiness;
        private readonly IRepairBusiness _repairBusiness;
        private readonly ERPBLL.Configuration.Interface.IHandSetStockBusiness _handSetStockBusiness;
        private readonly IFaultyStockInfoBusiness _faultyStockInfoBusiness;
        private readonly IColorSSBusiness _colorSSBusiness;
        private readonly IDealerSSBusiness _dealerSSBusiness;
        // Warehouse
        private readonly IDescriptionBusiness _descriptionBusiness;
        //ControlPanel
        private readonly IRoleBusiness _roleBusiness;

        // Front-Desk
        private readonly IJobOrderBusiness _jobOrderBusiness;
        private readonly IRequsitionInfoForJobOrderBusiness _requsitionInfoForJobOrderBusiness;
        private readonly IRequsitionDetailForJobOrderBusiness _requsitionDetailForJobOrderBusiness;
        private readonly ITechnicalServicesStockBusiness _technicalServicesStockBusiness;
        private readonly IJobOrderAccessoriesBusiness _jobOrderAccessoriesBusiness;
        private readonly IJobOrderProblemBusiness _jobOrderProblemBusiness;
        private readonly IJobOrderFaultBusiness _jobOrderFaultBusiness;
        private readonly IJobOrderServiceBusiness _jobOrderServiceBusiness;
        private readonly IJobOrderRepairBusiness _jobOrderRepairBusiness;
        private readonly ITsStockReturnInfoBusiness _tsStockReturnInfoBusiness;
        private readonly ITsStockReturnDetailsBusiness _tsStockReturnDetailsBusiness;
        private readonly IInvoiceInfoBusiness _invoiceInfoBusiness;
        private readonly IInvoiceDetailBusiness _invoiceDetailBusiness;
        private readonly IJobOrderReportBusiness _jobOrderReportBusiness;
        private readonly IJobOrderTransferDetailBusiness _jobOrderTransferDetailBusiness;
        private readonly IJobOrderReturnDetailBusiness _jobOrderReturnDetailBusiness;
        private readonly IJobOrderTSBusiness _jobOrderTSBusiness;
        private readonly IHandsetChangeTraceBusiness _handsetChangeTraceBusiness;
        private readonly IModelSSBusiness _modelSSBusiness;

        public FrontDeskController(IAccessoriesBusiness accessoriesBusiness, IClientProblemBusiness clientProblemBusiness, IDescriptionBusiness descriptionBusiness, IJobOrderBusiness jobOrderBusiness, ITechnicalServiceBusiness technicalServiceBusiness,ICustomerBusiness customerBusiness, IRequsitionInfoForJobOrderBusiness requsitionInfoForJobOrderBusiness, IRequsitionDetailForJobOrderBusiness requsitionDetailForJobOrderBusiness, IServicesWarehouseBusiness servicesWarehouseBusiness, IBranchBusiness branchBusiness, IMobilePartBusiness mobilePartBusiness, IMobilePartStockInfoBusiness mobilePartStockInfoBusiness, IMobilePartStockDetailBusiness mobilePartStockDetailBusiness, ITechnicalServicesStockBusiness technicalServicesStockBusiness, IJobOrderAccessoriesBusiness jobOrderAccessoriesBusiness, IJobOrderProblemBusiness jobOrderProblemBusiness, IFaultBusiness faultBusiness, IServiceBusiness serviceBusiness, IJobOrderFaultBusiness jobOrderFaultBusiness, IJobOrderServiceBusiness jobOrderServiceBusiness, IJobOrderRepairBusiness jobOrderRepairBusiness, IRepairBusiness repairBusiness, IRoleBusiness roleBusiness, ITsStockReturnInfoBusiness tsStockReturnInfoBusiness, ITsStockReturnDetailsBusiness tsStockReturnDetailsBusiness, IInvoiceInfoBusiness invoiceInfoBusiness, IInvoiceDetailBusiness invoiceDetailBusiness, IJobOrderReportBusiness jobOrderReportBusiness, IJobOrderTransferDetailBusiness jobOrderTransferDetailBusiness, IJobOrderReturnDetailBusiness jobOrderReturnDetailBusiness, IJobOrderTSBusiness jobOrderTSBusiness, ERPBLL.Configuration.Interface.IHandSetStockBusiness handSetStockBusiness, IFaultyStockInfoBusiness faultyStockInfoBusiness, IHandsetChangeTraceBusiness handsetChangeTraceBusiness, IColorSSBusiness colorSSBusiness, IModelSSBusiness modelSSBusiness, IDealerSSBusiness dealerSSBusiness)
        {
            this._handSetStockBusiness = handSetStockBusiness;
            this._accessoriesBusiness = accessoriesBusiness;
            this._clientProblemBusiness = clientProblemBusiness;
            this._descriptionBusiness = descriptionBusiness;
            this._jobOrderBusiness = jobOrderBusiness;
            this._technicalServiceBusiness = technicalServiceBusiness;
            this._customerBusiness = customerBusiness;
            this._requsitionInfoForJobOrderBusiness = requsitionInfoForJobOrderBusiness;
            this._requsitionDetailForJobOrderBusiness = requsitionDetailForJobOrderBusiness;
            this._servicesWarehouseBusiness = servicesWarehouseBusiness;
            this._branchBusiness = branchBusiness;
            this._mobilePartBusiness = mobilePartBusiness;
            this._mobilePartStockInfoBusiness = mobilePartStockInfoBusiness;
            this._mobilePartStockDetailBusiness = mobilePartStockDetailBusiness;
            this._technicalServicesStockBusiness = technicalServicesStockBusiness;
            this._jobOrderAccessoriesBusiness = jobOrderAccessoriesBusiness;
            this._jobOrderProblemBusiness = jobOrderProblemBusiness;
            this._faultBusiness = faultBusiness;
            this._serviceBusiness = serviceBusiness;
            this._jobOrderFaultBusiness = jobOrderFaultBusiness;
            this._jobOrderServiceBusiness = jobOrderServiceBusiness;
            this._jobOrderRepairBusiness = jobOrderRepairBusiness;
            this._repairBusiness = repairBusiness;
            this._roleBusiness = roleBusiness;
            this._tsStockReturnInfoBusiness = tsStockReturnInfoBusiness;
            this._tsStockReturnDetailsBusiness = tsStockReturnDetailsBusiness;
            this._invoiceInfoBusiness = invoiceInfoBusiness;
            this._invoiceDetailBusiness = invoiceDetailBusiness;
            this._jobOrderReportBusiness = jobOrderReportBusiness;
            this._jobOrderTransferDetailBusiness = jobOrderTransferDetailBusiness;
            this._jobOrderReturnDetailBusiness = jobOrderReturnDetailBusiness;
            this._jobOrderTSBusiness = jobOrderTSBusiness;
            this._faultyStockInfoBusiness = faultyStockInfoBusiness;
            this._handsetChangeTraceBusiness = handsetChangeTraceBusiness;
            this._colorSSBusiness = colorSSBusiness;
            this._modelSSBusiness = modelSSBusiness;
            this._dealerSSBusiness = dealerSSBusiness;
        }

        #region JobOrder
        [HttpGet]
        public ActionResult GetJobOrders(string flag, string fromDate, string toDate, long? modelId, long? jobOrderId, string mobileNo = "", string status = "", string jobCode = "", string iMEI = "", string iMEI2 = "", string tab = "",string customerType="",string jobType="",string repairStatus="",string customer="", int page = 1)
        {
            ViewBag.UserPrivilege = UserPrivilege("FrontDesk", "GetJobOrders");
            if (string.IsNullOrEmpty(flag))
            {
                //ViewBag.ddlModelName = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId).Select(d => new SelectListItem { Text = d.DescriptionName, Value = d.DescriptionId.ToString() }).ToList();
                ViewBag.ddlModelName = _modelSSBusiness.GetAllModel(User.OrgId).Select(m => new SelectListItem { Text = m.ModelName, Value = m.ModelId.ToString() }).ToList();

                ViewBag.ddlStateStatus = Utility.ListOfJobOrderStatus().Select(r => new SelectListItem { Text = r.text, Value = r.value }).ToList();
                ViewBag.ddlCustomerType = Utility.ListOfCustomerType().Select(r => new SelectListItem { Text = r.text, Value = r.value }).ToList();
                ViewBag.ddlJobType = Utility.ListOfJobOrderType().Select(r => new SelectListItem { Text = r.text, Value = r.value }).ToList();

                return View();
            }
            else if (!string.IsNullOrEmpty(flag) && (flag == "view" || flag == "search" || flag == "Detail" || flag == "Assign" || flag=="TSWork"))
            {
                var dto = _jobOrderBusiness.GetJobOrders(mobileNo.Trim(), modelId, status.Trim(), jobOrderId, jobCode, iMEI.Trim(), iMEI2.Trim(), User.OrgId, User.BranchId, fromDate, toDate,customerType,jobType, repairStatus,customer);

                IEnumerable<JobOrderViewModel> viewModels = new List<JobOrderViewModel>();
               
                if (flag == "view" || flag == "search")
                {
                    // Pagination //
                    ViewBag.PagerData = GetPagerData(dto.Count(), 5, page);
                    dto = dto.Skip((page - 1) * 5).Take(5).ToList();
                    //-----------------//
                    AutoMapper.Mapper.Map(dto, viewModels);
                    return PartialView("_GetJobOrders", viewModels);
                }
                //if (flag == "TSWork")
                //{
                //    AutoMapper.Mapper.Map(dto, viewModels);
                //    return PartialView("_GetTSWorkDetails", viewModels.FirstOrDefault());
                //}
                else if (flag == "Detail")// Flag = Detail
                {
                    var oldHandset = _handsetChangeTraceBusiness.GetOneJobByOrgId(jobOrderId.Value,User.OrgId);
                    var handset = new HandsetChangeTraceViewModel();
                    if (oldHandset != null)
                    {
                        handset = new HandsetChangeTraceViewModel
                        {
                            IMEI1 = oldHandset.IMEI1,
                            IMEI2 = oldHandset.IMEI2,
                            ModelId = oldHandset.ModelId,
                            ModelName = (_modelSSBusiness.GetModelById(oldHandset.ModelId, User.OrgId).ModelName),
                            Color = oldHandset.Color,
                        };
                    }
                    ViewBag.OldHansetInformation = handset;

                    AutoMapper.Mapper.Map(dto, viewModels);
                    ViewBag.ddlJobOrderType = Utility.ListOfJobOrderType().Select(r => new SelectListItem { Text = r.text, Value = r.value }).ToList();
                    return PartialView("_GetJobOrderDetail", viewModels.FirstOrDefault());
                }
                else
                {
                    // Flag= Assign
                    ViewBag.ddlTSName = _technicalServiceBusiness.GetAllTechnicalServiceByOrgId(User.OrgId, User.BranchId).Select(r => new SelectListItem { Text = r.Name, Value = r.EngId.ToString() }).ToList();
                    return PartialView("_GetJobOrderAssing", viewModels.FirstOrDefault());
                }
            }
            return View();
        }
        public ActionResult GetJobOrderDetails(long jobOrderId)
        {
            var dto = _jobOrderBusiness.GetJobOrderDetails(jobOrderId, User.OrgId);
            JobOrderViewModel viewModels = new JobOrderViewModel();
            AutoMapper.Mapper.Map(dto, viewModels);
            return PartialView("_GetJobOrderDetail", viewModels);
        }
        [HttpGet]
        public ActionResult GetTsWorksDetails(string flag, string fromDate, string toDate, long? modelId, long? jobOrderId, string mobileNo = "", string status = "", string jobCode = "", string iMEI = "", string iMEI2 = "",string customerType="",string jobType="",string repairStatus="",string customer="", int page = 1)
        {
            var dto = _jobOrderBusiness.GetJobOrders(mobileNo.Trim(), modelId, status.Trim(), jobOrderId, jobCode, iMEI.Trim(), iMEI2.Trim(), User.OrgId, User.BranchId, fromDate, toDate,customerType,jobType, repairStatus,customer);

            IEnumerable<JobOrderViewModel> viewModels = new List<JobOrderViewModel>();
            AutoMapper.Mapper.Map(dto, viewModels);
            return PartialView("_GetTSWorkDetails", viewModels.FirstOrDefault());
        }

        [HttpGet]
        public ActionResult CreateJobOrder(long? jobOrderId)
        {
            //ViewBag.ddlDescriptions = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId).Select(d => new SelectListItem { Text = d.DescriptionName, Value = d.DescriptionId.ToString() }).ToList();

            ViewBag.ddlDescriptions= _modelSSBusiness.GetAllModel(User.OrgId).Select(m => new SelectListItem { Text = m.ModelName, Value = m.ModelId.ToString() }).ToList();


            ViewBag.ddlAccessories = _accessoriesBusiness.GetAllAccessoriesByOrgId(User.OrgId).Select(a => new SelectListItem { Text = a.AccessoriesName, Value = a.AccessoriesId.ToString() }).ToList();

            ViewBag.ddlProblems = _clientProblemBusiness.GetAllClientProblemByOrgId(User.OrgId).Select(p => new SelectListItem { Text = p.ProblemName, Value = p.ProblemId.ToString() }).ToList();

            //ViewBag.ddlModelColor = Utility.ListOfModelColor().Select(r => new SelectListItem { Text = r.text, Value = r.value }).ToList();

            ViewBag.ddlModelColor = _colorSSBusiness.GetAllColorsByOrgId(User.OrgId).Select(c => new SelectListItem { Text = c.ColorName, Value = c.ColorName }).ToList();

            ViewBag.ddlPhoneTypes = Utility.ListOfPhoneTypes().Select(r => new SelectListItem { Text = r.text, Value = r.value }).ToList();

            ViewBag.ddlJobOrderType = Utility.ListOfJobOrderType().Select(r => new SelectListItem { Text = r.text, Value = r.value }).ToList();

            ViewBag.ddlCustomerType = Utility.ListOfCustomerType().Select(r => new SelectListItem { Text = r.text, Value = r.value }).ToList();

            ViewBag.ddlCustomerSupport = Utility.ListOfCustomerSupport().Select(r => new SelectListItem { Text = r.text, Value = r.value }).ToList();

            long jobOrder = 0;
            if (jobOrderId != null && jobOrderId > 0)
            {
                var jobOrderInDb = _jobOrderBusiness.GetJobOrderById(jobOrderId.Value, User.OrgId);
                jobOrder = jobOrderInDb != null ? jobOrderInDb.JodOrderId : 0;
            }

            ViewBag.JobOrderId = jobOrder;
            return View();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveJobOrder(JobOrderViewModel jobOrder, List<JobOrderAccessoriesViewModel> jobOrderAccessories,List<JobOrderProblemViewModel> jobOrderProblems)
        {
            bool IsSuccess = false;
            string file = string.Empty;
            if (ModelState.IsValid && jobOrderProblems.Count > 0)
            {
                JobOrderDTO jobOrderDTO = new JobOrderDTO();
                List<JobOrderAccessoriesDTO> listJobOrderAccessoriesDTO = new List<JobOrderAccessoriesDTO>();
                List<JobOrderProblemDTO> listJobOrderProblemDTO = new List<JobOrderProblemDTO>();

                AutoMapper.Mapper.Map(jobOrder, jobOrderDTO);
                AutoMapper.Mapper.Map(jobOrderAccessories, listJobOrderAccessoriesDTO);
                AutoMapper.Mapper.Map(jobOrderProblems, listJobOrderProblemDTO);

                var exe = _jobOrderBusiness.SaveJobOrderWithReport(jobOrderDTO, listJobOrderAccessoriesDTO, listJobOrderProblemDTO, User.UserId, User.OrgId, User.BranchId);

                IsSuccess = exe.isSuccess;
                file = GetJobOrderReport(Convert.ToInt64(exe.text));
            }
            return Json(new { IsSuccess = IsSuccess, file = file });
        }

        private string GetJobOrderReport(long jobOrderId)
        {
            string file = string.Empty;
            IEnumerable<JobOrderDTO> jobOrderDetails = _jobOrderBusiness.GetJobCreateReceipt(jobOrderId, User.OrgId, User.BranchId);

            ServicesReportHead reportHead = _jobOrderReportBusiness.GetBranchInformation(User.OrgId, User.BranchId);
            reportHead.ReportImage = Utility.GetImageBytes(User.LogoPaths[0]);
            List<ServicesReportHead> servicesReportHeads = new List<ServicesReportHead>();
            servicesReportHeads.Add(reportHead);

            LocalReport localReport = new LocalReport();
            string reportPath = Server.MapPath("~/Reports/ServiceRpt/FrontDesk/rptJobOrderCreateReceipt.rdlc");
            if (System.IO.File.Exists(reportPath))
            {
                localReport.ReportPath = reportPath;
                ReportDataSource dataSource1 = new ReportDataSource("JobCreateReceipt", jobOrderDetails);
                ReportDataSource dataSource2 = new ReportDataSource("ServicesReportHead", servicesReportHeads);
                localReport.DataSources.Clear();
                localReport.DataSources.Add(dataSource1);
                localReport.DataSources.Add(dataSource2);
                localReport.Refresh();
                localReport.DisplayName = "Receipt";

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

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult GetJobOrderById(long jobOrderId)
        {
            JobOrder jobOrder = _jobOrderBusiness.GetJobOrderById(jobOrderId, User.OrgId);
            JobOrderDTO jDto = new JobOrderDTO
            {
                JodOrderId = jobOrder.JodOrderId,
                JobOrderCode = jobOrder.JobOrderCode,
                CustomerId = jobOrder.CustomerId,
                CustomerName = jobOrder.CustomerName,
                CustomerType=jobOrder.CustomerType,
                MobileNo = jobOrder.MobileNo,
                Address = jobOrder.Address,
                DescriptionId = jobOrder.DescriptionId,
                ModelName = (_modelSSBusiness.GetModelById(jobOrder.DescriptionId, User.OrgId).ModelName),
                IsWarrantyAvailable = jobOrder.IsWarrantyAvailable,
                IsWarrantyPaperEnclosed = jobOrder.IsWarrantyPaperEnclosed,
                StateStatus = jobOrder.StateStatus,
                IMEI = jobOrder.IMEI,
                IMEI2 = jobOrder.IMEI2,
                Type = jobOrder.Type,
                ModelColor = jobOrder.ModelColor,
                JobOrderType = jobOrder.JobOrderType,
                WarrantyDate = jobOrder.WarrantyDate,
                Remarks = jobOrder.Remarks,
                CourierName= jobOrder.CourierName,
                CourierNumber= jobOrder.CourierNumber,
                ApproxBill= jobOrder.ApproxBill,
                ReferenceNumber = jobOrder.ReferenceNumber,
                EntryDate = jobOrder.EntryDate,
                OrganizationId = jobOrder.OrganizationId,
                BranchId = jobOrder.BranchId
            };

            var jorderAccessories = _jobOrderAccessoriesBusiness.GetJobOrderAccessoriesByJobOrder(jobOrderId, User.OrgId).Select(s => s.AccessoriesId).ToArray();

            var jobOrderProblems = _jobOrderProblemBusiness.GetJobOrderProblemByJobOrderId(jobOrderId, User.OrgId).Select(p => p.ProblemId).ToArray();

            JobOrderViewModel jobOrderViewModel = new JobOrderViewModel();
            AutoMapper.Mapper.Map(jDto, jobOrderViewModel);
            return Json(new { jobOrder = jDto, jorderAccessories = jorderAccessories, jobOrderProblems = jobOrderProblems });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult UpdateJobOrderStatus(long jobOrderId, string status, string type)
        {
            bool IsSuccess = false;
            if (jobOrderId > 0 && !string.IsNullOrEmpty(status) && !string.IsNullOrEmpty(type))
            {
                IsSuccess = _jobOrderBusiness.UpdateJobOrderStatus(jobOrderId, status, type, User.UserId, User.OrgId, User.BranchId);
            }
            return Json(IsSuccess);
        }

        [HttpGet]
        public ActionResult CreateMultipleJobOrder()
        {
            ViewBag.ddlDealerName = _dealerSSBusiness.GetAllDealerForD(User.OrgId).Select(p => new SelectListItem { Text = p.Dealer, Value = p.DealerId.ToString() }).ToList();

            ViewBag.ddlDescriptions = _modelSSBusiness.GetAllModel(User.OrgId).Select(m => new SelectListItem { Text = m.ModelName, Value = m.ModelId.ToString() }).ToList();


            ViewBag.ddlAccessories = _accessoriesBusiness.GetAllAccessoriesByOrgId(User.OrgId).Select(a => new SelectListItem { Text = a.AccessoriesName, Value = a.AccessoriesId.ToString() }).ToList();

            ViewBag.ddlProblems = _clientProblemBusiness.GetAllClientProblemByOrgId(User.OrgId).Select(p => new SelectListItem { Text = p.ProblemName, Value = p.ProblemId.ToString() }).ToList();

            ViewBag.ddlModelColor = _colorSSBusiness.GetAllColorsByOrgId(User.OrgId).Select(c => new SelectListItem { Text = c.ColorName, Value = c.ColorName }).ToList();

            ViewBag.ddlPhoneTypes = Utility.ListOfPhoneTypes().Select(r => new SelectListItem { Text = r.text, Value = r.value }).ToList();

            ViewBag.ddlJobOrderType = Utility.ListOfJobOrderType().Select(r => new SelectListItem { Text = r.text, Value = r.value }).ToList();

            ViewBag.ddlJobSource = Utility.ListOfJobSource().Select(r => new SelectListItem { Text = r.text, Value = r.value }).ToList();

            return View();
        }

        #endregion

        #region Multiple Job Delivery
        public ActionResult GetJobOrderListForDelivery(string flag, string fromDate, string toDate, long? modelId, long? jobOrderId, string mobileNo = "", string status = "", string jobCode = "", string iMEI = "", string iMEI2 = "", string tab = "", string customerType = "", string jobType = "", string repairStatus = "", int page = 1)
        {
            if (string.IsNullOrEmpty(flag))
            {
                //ViewBag.ddlModelName = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId).Select(d => new SelectListItem { Text = d.DescriptionName, Value = d.DescriptionId.ToString() }).ToList();

                ViewBag.ddlModelName = _modelSSBusiness.GetAllModel(User.OrgId).Select(m => new SelectListItem { Text = m.ModelName, Value = m.ModelId.ToString() }).ToList();

                ViewBag.ddlStateStatus = Utility.ListOfJobOrderStatus().Select(r => new SelectListItem { Text = r.text, Value = r.value }).ToList();
                ViewBag.ddlCustomerType = Utility.ListOfCustomerType().Select(r => new SelectListItem { Text = r.text, Value = r.value }).ToList();
                ViewBag.ddlJobType = Utility.ListOfJobOrderType().Select(r => new SelectListItem { Text = r.text, Value = r.value }).ToList();

                return View();
            }
            else if (!string.IsNullOrEmpty(flag) && (flag == "view" || flag == "search" || flag == "Detail" || flag == "Assign" || flag == "TSWork"))
            {
                var dto = _jobOrderBusiness.GetJobOrderForDelivery(mobileNo.Trim(), modelId, status.Trim(), jobOrderId, jobCode, iMEI.Trim(), iMEI2.Trim(), User.OrgId, User.BranchId, fromDate, toDate, customerType, jobType, repairStatus);

                IEnumerable<JobOrderViewModel> viewModels = new List<JobOrderViewModel>();

                if (flag == "view" || flag == "search")
                {
                    AutoMapper.Mapper.Map(dto, viewModels);
                    return PartialView("_GetJobOrderListForDelivery", viewModels);
                }
            }
            return View();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveMultipleDelivery(long[] jobOrders)
        {
            ExecutionStateWithText executionState = new ExecutionStateWithText();
            if (jobOrders.Count() > 0)
            {
                executionState = _jobOrderBusiness.SaveJobOrderMDelivey(jobOrders, User.UserId, User.OrgId, User.BranchId);
                if (executionState.isSuccess)
                {
                    executionState.text = GetMultipleJobDelivery(executionState.text);
                }
                    
            }
            return Json(executionState);
        }
        private string GetMultipleJobDelivery(string deliveryCode)
        {
            string file = string.Empty;
            IEnumerable<JobOrderDTO> jobOrderDetails = _jobOrderBusiness.GetMultipleJobDeliveryChalan(deliveryCode,User.BranchId, User.OrgId);

            ServicesReportHead reportHead = _jobOrderReportBusiness.GetBranchInformation(User.OrgId, User.BranchId);
            reportHead.ReportImage = Utility.GetImageBytes(User.LogoPaths[0]);
            List<ServicesReportHead> servicesReportHeads = new List<ServicesReportHead>();
            servicesReportHeads.Add(reportHead);

            LocalReport localReport = new LocalReport();
            string reportPath = Server.MapPath("~/Reports/ServiceRpt/FrontDesk/rptMultipleJobOrderDelivery.rdlc");
            if (System.IO.File.Exists(reportPath))
            {
                localReport.ReportPath = reportPath;
                ReportDataSource dataSource1 = new ReportDataSource("JobOrder", jobOrderDetails);
                ReportDataSource dataSource2 = new ReportDataSource("ServicesReportHead", servicesReportHeads);
                localReport.DataSources.Clear();
                localReport.DataSources.Add(dataSource1);
                localReport.DataSources.Add(dataSource2);
                localReport.Refresh();
                localReport.DisplayName = "Receipt";

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

        #region JobTransfer And Receive
        [HttpGet]
        public ActionResult GetJobTransferList(string flag,string tab="",int page=1)
        {
            ViewBag.UserPrivilege = UserPrivilege("FrontDesk", "GetJobTransferList");
            if (string.IsNullOrEmpty(flag))
            {
                //Transfer Job
                ViewBag.ddlBranchName = _branchBusiness.GetBranchByOrgId(User.OrgId).Where(b => b.BranchId != User.BranchId).Select(branch => new SelectListItem { Text = branch.BranchName, Value = branch.BranchId.ToString() }).ToList();

                ViewBag.ddlTransferCondition = Utility.ListOfTransferCondition().Select(r => new SelectListItem { Text = r.text, Value = r.value }).ToList();
                //receive Job
                ViewBag.ddlBranchName2 = _branchBusiness.GetBranchByOrgId(User.OrgId).Where(b => b.BranchId != User.BranchId).Select(branch => new SelectListItem { Text = branch.BranchName, Value = branch.BranchId.ToString() }).ToList();
                ViewBag.ddlTransferStatus2 = Utility.ListOfTransferStatus().Select(r => new SelectListItem { Text = r.text, Value = r.value }).ToList();
                ViewBag.ddlReceiveCondition = Utility.ListOfReceiveCondition().Select(r => new SelectListItem { Text = r.text, Value = r.value }).ToList();
                //Return Job
                ViewBag.ddlBranchName3 = _branchBusiness.GetBranchByOrgId(User.OrgId).Where(b => b.BranchId != User.BranchId).Select(branch => new SelectListItem { Text = branch.BranchName, Value = branch.BranchId.ToString() }).ToList();
                //Receive Return Job
                ViewBag.ddlTransferStatus4 = Utility.ListOfTransferStatus().Select(r => new SelectListItem { Text = r.text, Value = r.value }).ToList();

                ViewBag.ddlBranchName4 = _branchBusiness.GetBranchByOrgId(User.OrgId).Where(b => b.BranchId != User.BranchId).Select(branch => new SelectListItem { Text = branch.BranchName, Value = branch.BranchId.ToString() }).ToList();
                return View();
            }
            else
            {
                var dto = _jobOrderBusiness.JobOrderTransfer(User.OrgId, User.BranchId);
                List<JobOrderViewModel> viewModels = new List<JobOrderViewModel>();
                // Pagination //
                ViewBag.PagerData = GetPagerData(dto.Count(), 5, page);
                dto = dto.Skip((page - 1) * 5).Take(5).ToList();
                //-----------------//
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetJobTransferList",viewModels);
            }
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveJobTransfer(long transferId, long[] jobOrders,string cName,string cNumber)
        {
            ExecutionStateWithText executionState = new ExecutionStateWithText();
            if (transferId > 0 && jobOrders.Count() > 0)
            {
                executionState = _jobOrderTransferDetailBusiness.SaveJobOrderTransferWithReport(transferId, jobOrders, User.UserId, User.OrgId, User.BranchId,cName,cNumber);
                if (executionState.isSuccess)
                {
                    // Report ..
                    executionState.text =  GetTransferDeliveryChalan(executionState.text);
                }
            }
            return Json(executionState);
        }
        private string GetTransferDeliveryChalan(string transferCode)
        {
            string file = string.Empty;
            IEnumerable<JobOrderDTO> jobOrderDetails = _jobOrderTransferDetailBusiness.GetTransferDeliveryChalan(transferCode, User.OrgId);

            ServicesReportHead reportHead = _jobOrderReportBusiness.GetBranchInformation(User.OrgId, User.BranchId);
            reportHead.ReportImage = Utility.GetImageBytes(User.LogoPaths[0]);
            List<ServicesReportHead> servicesReportHeads = new List<ServicesReportHead>();
            servicesReportHeads.Add(reportHead);

            LocalReport localReport = new LocalReport();
            string reportPath = Server.MapPath("~/Reports/ServiceRpt/FrontDesk/rptJobTransferDeliveryChalan.rdlc");
            if (System.IO.File.Exists(reportPath))
            {
                localReport.ReportPath = reportPath;
                ReportDataSource dataSource1 = new ReportDataSource("JobTransfer", jobOrderDetails);
                ReportDataSource dataSource2 = new ReportDataSource("ServicesReportHead", servicesReportHeads);
                localReport.DataSources.Clear();
                localReport.DataSources.Add(dataSource1);
                localReport.DataSources.Add(dataSource2);
                localReport.Refresh();
                localReport.DisplayName = "Receipt";

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
        public ActionResult ReceiveJobOrder(string flag,long? branchName, string fromDate, string tstatus, string toDate, string jobCode="",string transferCode="",int page=1)
        {
            if (string.IsNullOrEmpty(flag) && flag=="recive")
            {
                ViewBag.ddlBranchName = _branchBusiness.GetBranchByOrgId(User.OrgId).Where(b => b.BranchId != User.BranchId).Select(branch => new SelectListItem { Text = branch.BranchName, Value = branch.BranchId.ToString() }).ToList();

                ViewBag.ddlTransferStatus = Utility.ListOfTransferStatus().Select(r => new SelectListItem { Text = r.text, Value = r.value }).ToList();
                ViewBag.ddlReceiveCondition = Utility.ListOfReceiveCondition().Select(r => new SelectListItem { Text = r.text, Value = r.value }).ToList();
                return View();
            }
            else
            {
                var dto = _jobOrderTransferDetailBusiness.GetReceiveJob(User.OrgId, User.BranchId,branchName,jobCode,transferCode,fromDate,toDate,tstatus);
                List<JobOrderTransferDetailViewModel> viewModels = new List<JobOrderTransferDetailViewModel>();
                // Pagination //
                ViewBag.PagerData = GetPagerData(dto.Count(), 5, page);
                dto = dto.Skip((page - 1) * 5).Take(5).ToList();
                //-----------------//
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_ReceiveJobOrder", viewModels);
            }
        }
        public ActionResult UpdateReceiveJob(long transferId, long jobOrderId)
        {
            bool IsSuccess = false;
            if (transferId > 0 && jobOrderId > 0)
            {
                IsSuccess = _jobOrderTransferDetailBusiness.UpdateReceiveJobOrder(transferId, jobOrderId, User.UserId, User.OrgId, User.BranchId);
            }
            return Json(IsSuccess);
        }
        public ActionResult TransferReceiveJobOrder(string flag,long? branchName,int page=1)
        {
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlBranchName = _branchBusiness.GetBranchByOrgId(User.OrgId).Where(b => b.BranchId != User.BranchId).Select(branch => new SelectListItem { Text = branch.BranchName, Value = branch.BranchId.ToString() }).ToList();
                return View();
            }
            else
            {
                var dto = _jobOrderBusiness.TransferReceiveJobOrder(User.OrgId, User.BranchId,branchName);
                List<JobOrderViewModel> viewModels = new List<JobOrderViewModel>();
                // Pagination //
                ViewBag.PagerData = GetPagerData(dto.Count(), 5, page);
                dto = dto.Skip((page - 1) * 5).Take(5).ToList();
                //-----------------//
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_TransferReceiveJobOrder", viewModels);
            }
            
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveTransferReceiveJob(long transferId, long[] jobOrders)
        {
            bool IsSuccess = false;
            if (transferId > 0 && jobOrders.Count() > 0)
            {
                IsSuccess = _jobOrderBusiness.SaveTransferReceiveJob(transferId, jobOrders, User.UserId, User.OrgId, User.BranchId);
            }
            return Json(IsSuccess);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveJobReturn(long transferId, long[] jobOrders,string cName,string cNumber)
        {
            ExecutionStateWithText executionState = new ExecutionStateWithText();
            if (transferId > 0 && jobOrders.Count() > 0)
            {
                executionState = _jobOrderReturnDetailBusiness.SaveJobOrderReturnWithReport(transferId, jobOrders, User.UserId, User.OrgId, User.BranchId,cName,cNumber);
                if (executionState.isSuccess)
                {
                    executionState.text = GetReturnDeliveryChalan(executionState.text);
                }
            }
            return Json(executionState);
        }
        private string GetReturnDeliveryChalan(string transferCode)
        {
            string file = string.Empty;
            IEnumerable<JobOrderDTO> jobOrderDetails = _jobOrderReturnDetailBusiness.GetReturnDeliveryChalan(transferCode, User.OrgId);

            ServicesReportHead reportHead = _jobOrderReportBusiness.GetBranchInformation(User.OrgId, User.BranchId);
            reportHead.ReportImage = Utility.GetImageBytes(User.LogoPaths[0]);
            List<ServicesReportHead> servicesReportHeads = new List<ServicesReportHead>();
            servicesReportHeads.Add(reportHead);

            LocalReport localReport = new LocalReport();
            string reportPath = Server.MapPath("~/Reports/ServiceRpt/FrontDesk/rptJobReturnDeliveryChalan.rdlc");
            if (System.IO.File.Exists(reportPath))
            {
                localReport.ReportPath = reportPath;
                ReportDataSource dataSource1 = new ReportDataSource("JobReturn", jobOrderDetails);
                ReportDataSource dataSource2 = new ReportDataSource("ServicesReportHead", servicesReportHeads);
                localReport.DataSources.Clear();
                localReport.DataSources.Add(dataSource1);
                localReport.DataSources.Add(dataSource2);
                localReport.Refresh();
                localReport.DisplayName = "Receipt";

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
        public ActionResult ReceiveReturnJobOrder(string flag, long? branchName, string fromDate, string toDate, string tstatus, string jobCode = "", string transferCode = "",int page=1)
        {
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlBranchName = _branchBusiness.GetBranchByOrgId(User.OrgId).Where(b => b.BranchId != User.BranchId).Select(branch => new SelectListItem { Text = branch.BranchName, Value = branch.BranchId.ToString() }).ToList();

                ViewBag.ddlTransferStatus = Utility.ListOfTransferStatus().Select(r => new SelectListItem { Text = r.text, Value = r.value }).ToList();
                return View();
            }
            else
            {
                var dto = _jobOrderReturnDetailBusiness.GetReturnJobOrder(User.OrgId, User.BranchId, branchName, jobCode, transferCode,fromDate,toDate, tstatus);
                List<JobOrderReturnDetailViewModel> viewModels = new List<JobOrderReturnDetailViewModel>();
                // Pagination //
                ViewBag.PagerData = GetPagerData(dto.Count(), 5, page);
                dto = dto.Skip((page - 1) * 5).Take(5).ToList();
                //-----------------//
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_ReceiveReturnJobOrder", viewModels);
            }
        }
        public ActionResult UpdateReturnJob(long receiveId,long jobOrderId)
        {
            bool IsSuccess = false;
            if (receiveId > 0 )
            {
                IsSuccess = _jobOrderReturnDetailBusiness.UpdateReturnJobOrder(receiveId, jobOrderId, User.UserId, User.OrgId, User.BranchId);
            }
            return Json(IsSuccess);
        }
        #endregion

        #region JobOrderTS
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult AssignTSForJobOrder(long jobOrderId, long tsId)
        {
            bool IsSuccess = false;
            if (jobOrderId > 0 && tsId > 0)
            {
                IsSuccess = _jobOrderBusiness.AssignTSForJobOrder(jobOrderId, tsId, User.UserId, User.OrgId, User.BranchId);
            }
            return Json(IsSuccess);
        }
        [HttpGet]
        public ActionResult GetJobOrdersTS(string flag, long? modelId, long? jobOrderId, string mobileNo = "", string jobCode = "",int page=1)
        {
            if (string.IsNullOrEmpty(flag))
            {
                //ViewBag.ddlModelName = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId).Select(d => new SelectListItem { Text = d.DescriptionName, Value = d.DescriptionId.ToString() }).ToList();

                ViewBag.ddlModelName = _modelSSBusiness.GetAllModel(User.OrgId).Select(m => new SelectListItem { Text = m.ModelName, Value = m.ModelId.ToString() }).ToList();

                return View();
            }
            else if (!string.IsNullOrEmpty(flag) && (flag == "view" || flag == "search" || flag == "Detail" || flag == "Assign"))
            {
                var dto = _jobOrderBusiness.GetJobOrdersTS(User.RoleName, mobileNo.Trim(), modelId, jobOrderId, jobCode, User.UserId, User.OrgId, User.BranchId);

                IEnumerable<JobOrderViewModel> viewModels = new List<JobOrderViewModel>();
                // Pagination //
                ViewBag.PagerData = GetPagerData(dto.Count(), 5, page);
                dto = dto.Skip((page - 1) * 5).Take(5).ToList();
                //-----------------//
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetJobOrdersTS", viewModels);
            }
            return View();
        }
        #endregion

        #region JobOrderPush
        [HttpGet]
        public ActionResult GetJobOrdersPush(string flag, long? jobOrderId)
        {
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlTechnicalServicesName = _roleBusiness.GetRoleByTechnicalServicesId(string.Empty, User.OrgId, User.BranchId).Select(d => new SelectListItem { Text = d.UserName, Value = d.UserId.ToString() }).ToList();

                return View();
            }
            else if (!string.IsNullOrEmpty(flag) && (flag == "view" || flag == "search" || flag == "Detail" || flag == "Assign"))
            {
                var dto = _jobOrderBusiness.GetJobOrdersPush(jobOrderId, User.OrgId, User.BranchId);

                IEnumerable<JobOrderViewModel> viewModels = new List<JobOrderViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetJobOrdersPush", viewModels);
            }
            return View();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveJobOrderPushing(long ts, long[] jobOrders)
        {
            bool IsSuccess = false;
            if (ts > 0 && jobOrders.Count() > 0)
            {
                IsSuccess = _jobOrderBusiness.SaveJobOrderPushing(ts, jobOrders, User.UserId, User.OrgId, User.BranchId);
            }
            return Json(IsSuccess);
        }
        #endregion

        #region JobOrderPull
        [HttpGet]
        public ActionResult GetJobOrdersPull(string flag)
        {
            if (string.IsNullOrEmpty(flag))
            {
                return View();
            }
            else if (!string.IsNullOrEmpty(flag) && (flag == "view" || flag == "search"))
            {
                var dto = _jobOrderBusiness.GetJobOrdersPush(0, User.OrgId, User.BranchId);

                IEnumerable<JobOrderViewModel> viewModels = new List<JobOrderViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetJobOrdersPull", viewModels);
            }
            return View();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveJobOrderPulling(long jobOrder)
        {
            bool IsSuccess = false;
            if (jobOrder > 0)
            {
                IsSuccess = _jobOrderBusiness.SaveJobOrderPulling(jobOrder, User.UserId, User.OrgId, User.BranchId);
            }
            return Json(IsSuccess);
        }
        #endregion

        #region Parts Request
        public ActionResult RequsitionInfoForJobOrderList(long?  joborderId)
        {
            var jobOrder = _jobOrderBusiness.GetJobOrderById(joborderId.Value, User.OrgId);
            ViewBag.JobOrder = new JobOrderViewModel
            {
                JobOrderCode = jobOrder.JobOrderCode,
                JodOrderId = jobOrder.JodOrderId
            };
            ViewBag.ddlServicesWarehouse = _servicesWarehouseBusiness.GetAllServiceWarehouseByOrgId(User.OrgId, User.BranchId).Select(services => new SelectListItem { Text = services.ServicesWarehouseName, Value = services.SWarehouseId.ToString() }).ToList();

            ViewBag.ddlMobilePart = _mobilePartBusiness.GetAllMobilePartAndCode(User.OrgId).Select(mobile => new SelectListItem { Text = mobile.MobilePartName, Value = mobile.MobilePartId.ToString() }).ToList();
            return View();
        }

        public ActionResult RequsitionInfoForJobOrderPartialListList(long jobOrderId)
        {
            //IEnumerable<RequsitionInfoForJobOrderDTO> requsitionInfoForJobOrderDTO = _requsitionInfoForJobOrderBusiness.GetAllRequsitionInfoForJobOrder(jobOrderId, User.OrgId, User.BranchId).Select(info => new RequsitionInfoForJobOrderDTO
            //{
            //    RequsitionInfoForJobOrderId = info.RequsitionInfoForJobOrderId,
            //    RequsitionCode = info.RequsitionCode,
            //    SWarehouseId = info.SWarehouseId,
            //    SWarehouseName = (_servicesWarehouseBusiness.GetServiceWarehouseOneByOrgId(info.SWarehouseId.Value, User.OrgId, User.BranchId).ServicesWarehouseName),
            //    StateStatus = info.StateStatus,
            //    JobOrderId = info.JobOrderId,
            //    JobOrderCode = info.JobOrderCode,
            //    Remarks = info.Remarks,
            //    BranchId = info.BranchId,
            //    OrganizationId = info.OrganizationId,
            //    EUserId = info.EUserId,
            //    EntryDate = DateTime.Now,
            //}).ToList();
            var dto = _requsitionInfoForJobOrderBusiness.GetRequsitionInfoTSData(jobOrderId);
            List<RequsitionInfoForJobOrderViewModel> requsitionInfoForJobs = new List<RequsitionInfoForJobOrderViewModel>();
            AutoMapper.Mapper.Map(dto, requsitionInfoForJobs);

            return PartialView("RequsitionInfoForJobOrderPartialListList", requsitionInfoForJobs);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveRequisitionInfoForJobOrder(RequsitionInfoForJobOrderViewModel info, List<RequsitionDetailForJobOrderViewModel> details)
        {
            bool IsSuccess = false;
            if (ModelState.IsValid && details.Count() > 0)
            {
                RequsitionInfoForJobOrderDTO dtoInfo = new RequsitionInfoForJobOrderDTO();
                List<RequsitionDetailForJobOrderDTO> dtoDetail = new List<RequsitionDetailForJobOrderDTO>();
                AutoMapper.Mapper.Map(info, dtoInfo);
                AutoMapper.Mapper.Map(details, dtoDetail);
                IsSuccess = _requsitionInfoForJobOrderBusiness.SaveRequisitionInfoForJobOrder(dtoInfo, dtoDetail, User.UserId, User.OrgId, User.BranchId);
            }
            return Json(IsSuccess);
        }

        public ActionResult RequsitionForJobOrderDetails(long? requsitionInfoId)
        {
            var info = _requsitionInfoForJobOrderBusiness.GetAllRequsitionInfoForJobOrderId(requsitionInfoId.Value, User.OrgId);
            var jobOrderInfo = _jobOrderBusiness.GetJobOrdersByIdWithBranch(info.JobOrderId.Value, User.BranchId, User.OrgId);
            jobOrderInfo = jobOrderInfo == null ? _jobOrderBusiness.GetJobOrdersByIdWithTransferBranch(info.JobOrderId.Value, User.BranchId, User.OrgId) : jobOrderInfo;

            ViewBag.ReqInfoJobOrder = new RequsitionInfoForJobOrderViewModel
            {
                RequsitionCode = info.RequsitionCode,
                JobOrderCode = (jobOrderInfo.JobOrderCode),
                Type = (_jobOrderBusiness.GetJobOrdersByIdWithBranch(info.JobOrderId.Value, jobOrderInfo.BranchId.Value, User.OrgId).Type),
                ModelName = (_modelSSBusiness.GetModelById(jobOrderInfo.DescriptionId, User.OrgId).ModelName),
                EntryDate = info.EntryDate,
                SWarehouseName = (_servicesWarehouseBusiness.GetServiceWarehouseOneByOrgId(info.SWarehouseId.Value, User.OrgId, jobOrderInfo.BranchId.Value).ServicesWarehouseName),
            };

            IEnumerable<RequsitionDetailForJobOrderDTO> requsitionDetailsDto = _requsitionDetailForJobOrderBusiness.GetAllRequsitionDetailForJobOrderId(requsitionInfoId.Value, User.OrgId, jobOrderInfo.BranchId.Value).Select(s => new RequsitionDetailForJobOrderDTO
            {
                RequsitionDetailForJobOrderId = s.RequsitionDetailForJobOrderId,
                PartsId=s.PartsId,
                PartsName= (_mobilePartBusiness.GetMobilePartOneByOrgId(s.PartsId.Value, User.OrgId).MobilePartName),
                MobilePartCode= (_mobilePartBusiness.GetMobilePartOneByOrgId(s.PartsId.Value, User.OrgId).MobilePartCode),
                SellPrice =s.SellPrice,
                CostPrice=s.CostPrice,
                Quantity = s.Quantity,
                Remarks = s.Remarks
            }).ToList();

            
            IEnumerable<RequsitionDetailForJobOrderViewModel> itemReturnDetailViews = new List<RequsitionDetailForJobOrderViewModel>();
            ViewBag.RequisitionStatus = _requsitionInfoForJobOrderBusiness.GetAllRequsitionInfoForJobOrderId(requsitionInfoId.Value, User.OrgId).StateStatus;
            ViewBag.UserPrivilege = UserPrivilege("FrontDesk", "RequsitionForJobOrderDetails");

            AutoMapper.Mapper.Map(requsitionDetailsDto, itemReturnDetailViews);
            return PartialView("RequsitionForJobOrderDetails", itemReturnDetailViews);
        }

        //Services Warehouse Part//

        public ActionResult TSRequsitionInfoForJobOrderList()
        {
            ViewBag.ddlWarehouseName = _servicesWarehouseBusiness.GetAllServiceWarehouseByOrgId(User.OrgId,User.BranchId).Select(ware => new SelectListItem { Text = ware.ServicesWarehouseName, Value = ware.SWarehouseId.ToString() }).ToList();

            ViewBag.ddlStateStatus = Utility.ListOfReqStatus().Where(status =>status.value == RequisitionStatus.Current || status.value == RequisitionStatus.Pending || status.value == RequisitionStatus.Approved || status.value == RequisitionStatus.Rejected || status.value == RequisitionStatus.Void).Select(st => new SelectListItem
            {
                Text = st.text,
                Value = st.value
            }).ToList();
            ViewBag.ddlTechnicalServicesName = _roleBusiness.GetRoleByTechnicalServicesId(string.Empty, User.OrgId, User.BranchId).Select(d => new SelectListItem { Text = d.UserName, Value = d.UserId.ToString() }).ToList();
            //Other Branch
            ViewBag.ddlWarehouseName2 = _servicesWarehouseBusiness.GetAllServiceWarehouseByOrgId(User.OrgId, User.BranchId).Select(ware => new SelectListItem { Text = ware.ServicesWarehouseName, Value = ware.SWarehouseId.ToString() }).ToList();

            ViewBag.ddlStateStatus2 = Utility.ListOfReqStatus().Where(status => status.value == RequisitionStatus.Current || status.value == RequisitionStatus.Pending || status.value == RequisitionStatus.Approved || status.value == RequisitionStatus.Rejected || status.value == RequisitionStatus.Void).Select(st => new SelectListItem
            {
                Text = st.text,
                Value = st.value
            }).ToList();
            ViewBag.ddlTechnicalServicesName2 = _roleBusiness.GetRoleByTechnicalServicesId(string.Empty, User.OrgId, User.BranchId).Select(d => new SelectListItem { Text = d.UserName, Value = d.UserId.ToString() }).ToList();
            return View();
        }
        public ActionResult TSRequsitionInfoForJobOrderPartialListList(string reqCode, long? warehouseId,long? tsId, string status, string fromDate, string toDate,string jobCode="",int page=1)
        {
            //IEnumerable<RequsitionInfoForJobOrderDTO> requsitionInfoForJobOrderDTO = _requsitionInfoForJobOrderBusiness.GetAllRequsitionInfoForJob(User.OrgId, User.BranchId).Where(req =>
            //    (reqCode == null || reqCode.Trim() == "" || req.RequsitionCode.Contains(reqCode))
            //    &&
            //    (warehouseId == null || warehouseId <= 0 || req.SWarehouseId == warehouseId)
            //    &&
            //    (tsId == null || tsId <= 0 || req.EUserId == tsId)
            //    &&
            //    (status == null || status.Trim() == "" || req.StateStatus == status.Trim())
            //    &&
            //    (
            //        (fromDate == null && toDate == null)
            //        ||
            //         (fromDate == "" && toDate == "")
            //        ||
            //        (fromDate.Trim() != "" && toDate.Trim() != "" &&

            //            req.EntryDate.Value.Date >= Convert.ToDateTime(fromDate).Date &&
            //            req.EntryDate.Value.Date <= Convert.ToDateTime(toDate).Date)
            //        ||
            //        (fromDate.Trim() != "" && req.EntryDate.Value.Date == Convert.ToDateTime(fromDate).Date)
            //        ||
            //        (toDate.Trim() != "" && req.EntryDate.Value.Date == Convert.ToDateTime(toDate).Date)
            //    )).Select(info => new RequsitionInfoForJobOrderDTO
            //{
            //    RequsitionInfoForJobOrderId = info.RequsitionInfoForJobOrderId,
            //    RequsitionCode = info.RequsitionCode,
            //    SWarehouseId = info.SWarehouseId,
            //    SWarehouseName = (_servicesWarehouseBusiness.GetServiceWarehouseOneByOrgId(info.SWarehouseId.Value, User.OrgId, User.BranchId).ServicesWarehouseName),
            //    StateStatus = info.StateStatus,
            //    JobOrderId = info.JobOrderId,
            //    JobOrderCode = info.JobOrderCode,
            //    Remarks = info.Remarks,
            //    BranchId = info.BranchId,
            //    OrganizationId = info.OrganizationId,
            //    EUserId = info.EUserId,
            //    Requestby = UserForEachRecord(info.EUserId.Value).UserName,
            //    EntryDate = info.EntryDate
            //}).ToList();
            var dto = _requsitionInfoForJobOrderBusiness.GetRequsitionInfoData(reqCode, warehouseId, tsId, status, fromDate, toDate, User.OrgId, User.BranchId,jobCode);
            List<RequsitionInfoForJobOrderViewModel> requsitionInfoForJobs = new List<RequsitionInfoForJobOrderViewModel>();
            // Pagination //
            ViewBag.PagerData = GetPagerData(dto.Count(), 10, page);
            dto = dto.Skip((page - 1) * 10).Take(10).ToList();
            //-----------------//
            AutoMapper.Mapper.Map(dto, requsitionInfoForJobs);

            return PartialView("TSRequsitionInfoForJobOrderPartialListList", requsitionInfoForJobs);
        }
        public ActionResult TSRequsitionForJobOrderDetails(long? requsitionInfoId)
        {
            var info = _requsitionInfoForJobOrderBusiness.GetAllRequsitionInfoForJobOrderId(requsitionInfoId.Value, User.OrgId);
            var jobOrderInfo = _jobOrderBusiness.GetJobOrdersByIdWithBranch(info.JobOrderId.Value, User.BranchId, User.OrgId);
            jobOrderInfo= jobOrderInfo==null? _jobOrderBusiness.GetJobOrdersByIdWithTransferBranch(info.JobOrderId.Value, User.BranchId, User.OrgId) : jobOrderInfo;

            ViewBag.ReqInfoJobOrder = new RequsitionInfoForJobOrderViewModel
            {
                RequsitionCode = info.RequsitionCode,
                JobOrderCode = (jobOrderInfo.JobOrderCode),
                Type = (_jobOrderBusiness.GetJobOrdersByIdWithBranch(info.JobOrderId.Value, jobOrderInfo.BranchId.Value, User.OrgId).Type),
                ModelName = (_modelSSBusiness.GetModelById(jobOrderInfo.DescriptionId, User.OrgId).ModelName),
                ModelColor=jobOrderInfo.ModelColor,
                Requestby = UserForEachRecord(info.EUserId.Value).UserName,
                EntryDate = info.EntryDate,
                SWarehouseName = (_servicesWarehouseBusiness.GetServiceWarehouseOneByOrgId(info.SWarehouseId.Value, User.OrgId, User.BranchId).ServicesWarehouseName),
            };

            ViewBag.RequisitionStatus = _requsitionInfoForJobOrderBusiness.GetAllRequsitionInfoForJobOrderId(requsitionInfoId.Value, User.OrgId).StateStatus;
            ViewBag.UserPrivilege = UserPrivilege("FrontDesk", "TSRequsitionInfoForJobOrderList");

            //IEnumerable<RequsitionDetailForJobOrderDTO> requsitionDetailsDto = _requsitionDetailForJobOrderBusiness.GetAllRequsitionDetailForJobOrderId(requsitionInfoId.Value, User.OrgId,User.BranchId).Where(rqd => requsitionInfoId == null || requsitionInfoId == 0 || rqd.RequsitionInfoForJobOrderId == requsitionInfoId).Select(s => new RequsitionDetailForJobOrderDTO
            //{
            //    RequsitionDetailForJobOrderId = s.RequsitionDetailForJobOrderId,
            //    PartsId=s.PartsId,
            //    PartsName= (_mobilePartBusiness.GetMobilePartOneByOrgId(s.PartsId.Value, User.OrgId).MobilePartName),
            //    CostPrice=s.CostPrice,
            //    SellPrice=s.SellPrice,
            //    Quantity = s.Quantity,
            //    Remarks = s.Remarks
            //}).ToList();
            //IEnumerable<RequsitionDetailForJobOrderViewModel> itemReturnDetailViews = new List<RequsitionDetailForJobOrderViewModel>();
            //AutoMapper.Mapper.Map(requsitionDetailsDto, itemReturnDetailViews);
            //return PartialView("TSRequsitionForJobOrderDetails", itemReturnDetailViews);

            //Nishad//
            //IEnumerable<RequsitionDetailForJobOrderDTO> returnDTO = _requsitionDetailForJobOrderBusiness.GetAvailableQtyByRequsition(requsitionInfoId.Value, User.OrgId, User.BranchId);
            //Nishad//
            IEnumerable<RequsitionDetailForJobOrderDTO> returnDTO = _requsitionDetailForJobOrderBusiness.GetModelWiseAvailableQtyByRequsition(requsitionInfoId.Value, User.OrgId, jobOrderInfo.BranchId.Value, jobOrderInfo.DescriptionId);
            IEnumerable<RequsitionDetailForJobOrderViewModel> returnViewModels = new List<RequsitionDetailForJobOrderViewModel>();
            AutoMapper.Mapper.Map(returnDTO, returnViewModels);
            ViewBag.AvailableQtyByRequsition = returnViewModels;
            return View();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult TSSaveRequisitionStatus(long reqId, string status)
        {
            bool IsSuccess = false;
            if (reqId > 0 && !string.IsNullOrEmpty(status))
            {
                if (RequisitionStatus.Rejected == status)
                {
                    IsSuccess = _requsitionInfoForJobOrderBusiness.SaveRequisitionStatus(reqId, status, User.UserId, User.OrgId, User.BranchId);
                }
                if (RequisitionStatus.Pending == status)
                {
                    IsSuccess = _requsitionInfoForJobOrderBusiness.SaveRequisitionStatus(reqId, status, User.UserId, User.OrgId, User.BranchId);
                }
                else
                if (RequisitionStatus.Approved == status)
                {
                    //if (GetExecutionStockAvailableForRequisition(reqId).isSuccess == true)
                    //{

                    //}
                    IsSuccess = _mobilePartStockDetailBusiness.SaveMobilePartStockOutByReq(reqId, status, User.OrgId, User.BranchId, User.UserId);
                }
                
            }
            return Json(IsSuccess);
        }
        [NonAction]
        private ExecutionStateWithText GetExecutionStockAvailableForRequisition(long? reqInfoId)
        {
            ExecutionStateWithText stateWithText = new ExecutionStateWithText();
            var reqDetail = _requsitionDetailForJobOrderBusiness.GetAllRequsitionDetailForJobOrderId(reqInfoId.Value, User.OrgId,User.BranchId).ToArray();
            var warehouseStock = _mobilePartStockInfoBusiness.GetAllMobilePartStockInfoByOrgId(User.OrgId,User.BranchId).ToList();
            var parts = _mobilePartBusiness.GetAllMobilePartByOrgId(User.OrgId).ToList();
            stateWithText.isSuccess = true;

            for (int i = 0; i < reqDetail.Length; i++)
            {
                var w = warehouseStock.FirstOrDefault(wr => wr.MobilePartId == reqDetail[i].PartsId);
                if (w != null)
                {
                    if ((w.StockInQty - w.StockOutQty) < reqDetail[i].Quantity)
                    {
                        stateWithText.isSuccess = false;
                        stateWithText.text += parts.FirstOrDefault(it => it.MobilePartId == reqDetail[i].PartsId).MobilePartName + " does not have enough stock </br>";
                        break;
                    }
                }
                else
                {
                    stateWithText.isSuccess = false;
                    stateWithText.text += parts.FirstOrDefault(it => it.MobilePartId == reqDetail[i].PartsId).MobilePartName + " does not have enough stock </br>";
                    break;
                }
            }
            return stateWithText;
        }
        //AnotherBranchRequsition
        public ActionResult AnotherBranchRequsition()
        {
            ViewBag.ddlWarehouseName = _servicesWarehouseBusiness.GetAllServiceWarehouseByOrgId(User.OrgId, User.BranchId).Select(ware => new SelectListItem { Text = ware.ServicesWarehouseName, Value = ware.SWarehouseId.ToString() }).ToList();

            ViewBag.ddlStateStatus = Utility.ListOfReqStatus().Where(status => status.value == RequisitionStatus.Current || status.value == RequisitionStatus.Pending || status.value == RequisitionStatus.Approved || status.value == RequisitionStatus.Rejected || status.value == RequisitionStatus.Void).Select(st => new SelectListItem
            {
                Text = st.text,
                Value = st.value
            }).ToList();
            ViewBag.ddlTechnicalServicesName = _roleBusiness.GetRoleByTechnicalServicesId(string.Empty, User.OrgId, User.BranchId).Select(d => new SelectListItem { Text = d.UserName, Value = d.UserId.ToString() }).ToList();
            return View();
        }
        public ActionResult AnotherBranchRequsitionPartialList(string reqCode, long? warehouseId, long? tsId, string status, string fromDate, string toDate, int page = 1)
        {
            var dto = _requsitionInfoForJobOrderBusiness.GetRequsitionInfoOtherBranchData(reqCode, warehouseId, tsId, status, fromDate, toDate, User.OrgId, User.BranchId);
            List<RequsitionInfoForJobOrderViewModel> requsitionInfoForJobs = new List<RequsitionInfoForJobOrderViewModel>();
            // Pagination //
            ViewBag.PagerData = GetPagerData(dto.Count(), 10, page);
            dto = dto.Skip((page - 1) * 10).Take(10).ToList();
            //-----------------//
            AutoMapper.Mapper.Map(dto, requsitionInfoForJobs);

            return PartialView("AnotherBranchRequsitionPartialList", requsitionInfoForJobs);
        }
        public ActionResult AnotherBranchRequsitionDetails(long? requsitionInfoId)
        {
            var info = _requsitionInfoForJobOrderBusiness.GetAllRequsitionInfoForJobOrderId(requsitionInfoId.Value, User.OrgId);
            var jobOrderInfo = _jobOrderBusiness.GetJobOrdersByIdWithBranch(info.JobOrderId.Value, User.BranchId, User.OrgId);
            jobOrderInfo = jobOrderInfo == null ? _jobOrderBusiness.GetJobOrdersByIdWithTransferBranch(info.JobOrderId.Value, User.BranchId, User.OrgId) : jobOrderInfo;

            ViewBag.ReqInfoJobOrder = new RequsitionInfoForJobOrderViewModel
            {
                RequsitionCode = info.RequsitionCode,
                JobOrderCode = (jobOrderInfo.JobOrderCode),
                Type = (_jobOrderBusiness.GetJobOrdersByIdWithBranch(info.JobOrderId.Value, jobOrderInfo.BranchId.Value, User.OrgId).Type),
                ModelName = (_modelSSBusiness.GetModelById(jobOrderInfo.DescriptionId, User.OrgId).ModelName),
                ModelColor = jobOrderInfo.ModelColor,
                Requestby = UserForEachRecord(info.EUserId.Value).UserName,
                EntryDate = info.EntryDate,
                //SWarehouseName = (_servicesWarehouseBusiness.GetServiceWarehouseOneByOrgId(info.SWarehouseId.Value, User.OrgId, jobOrderInfo.BranchId.Value).ServicesWarehouseName),
            };

            ViewBag.RequisitionStatus = _requsitionInfoForJobOrderBusiness.GetAllRequsitionInfoForJobOrderId(requsitionInfoId.Value, User.OrgId).StateStatus;
            ViewBag.UserPrivilege = UserPrivilege("FrontDesk", "AnotherBranchRequsition");

            //Nishad//
            //IEnumerable<RequsitionDetailForJobOrderDTO> returnDTO = _requsitionDetailForJobOrderBusiness.GetAvailableQtyByRequsition(requsitionInfoId.Value, User.OrgId, User.BranchId);
            //Nishad//
            IEnumerable<RequsitionDetailForJobOrderDTO> returnDTO = _requsitionDetailForJobOrderBusiness.GetModelWiseAvailableQtyByRequsition(requsitionInfoId.Value, User.OrgId, jobOrderInfo.BranchId.Value,jobOrderInfo.DescriptionId);
            IEnumerable<RequsitionDetailForJobOrderViewModel> returnViewModels = new List<RequsitionDetailForJobOrderViewModel>();
            AutoMapper.Mapper.Map(returnDTO, returnViewModels);
            ViewBag.AvailableQtyByRequsition = returnViewModels;
            return View();
        }
        #endregion

        #region T.S Stock
        public ActionResult TechnicalSetvicesStockList(long id)
        {
            IEnumerable<TechnicalServicesStockDTO> stockDTO = _technicalServicesStockBusiness.GetAllTechnicalServicesStock(id,User.OrgId, User.BranchId).Select(stock => new TechnicalServicesStockDTO
            {
                TsStockId = stock.TsStockId,
                JobOrderId=stock.JobOrderId,
                RequsitionInfoForJobOrderId=stock.RequsitionInfoForJobOrderId,
                RequsitionCode= (_requsitionInfoForJobOrderBusiness.GetAllRequsitionInfoOneByOrgId(stock.RequsitionInfoForJobOrderId, User.OrgId,User.BranchId).RequsitionCode),
                SWarehouseId = stock.SWarehouseId,
                SWarehouseName= (_servicesWarehouseBusiness.GetServiceWarehouseOneByOrgId(stock.SWarehouseId.Value, User.OrgId, User.BranchId).ServicesWarehouseName),
                PartsId = stock.PartsId,
                PartsName= (_mobilePartBusiness.GetMobilePartOneByOrgId(stock.PartsId.Value, User.OrgId).MobilePartName),
                CostPrice=stock.CostPrice,
                SellPrice=stock.SellPrice,
                Quantity = stock.Quantity,
                UsedQty = stock.UsedQty,
                ReturnQty = stock.ReturnQty,
                Remarks = stock.Remarks,
                BranchId = stock.BranchId,
                OrganizationId = stock.OrganizationId,
                EUserId = stock.EUserId,
                EntryDate = DateTime.Now,
                UpUserId=stock.UpUserId,
                UpdateDate=DateTime.Now
            }).ToList();
            List<TechnicalServicesStockViewModel> viewModel = new List<TechnicalServicesStockViewModel>();
            AutoMapper.Mapper.Map(stockDTO, viewModel);
            return View(viewModel);
        }
        #endregion

        #region T.S Servicing Details
        [HttpGet]
        public ActionResult ServicesDetails(long? joborderId)
        {
            ViewBag.ddlFaultName = _faultBusiness.GetAllFaultByOrgId(User.OrgId).Select(fault => new SelectListItem { Text = fault.FaultName, Value = fault.FaultId.ToString() }).ToList();

            ViewBag.ddlServiceName = _serviceBusiness.GetAllServiceByOrgId(User.OrgId).Select(service => new SelectListItem { Text = service.ServiceName, Value = service.ServiceId.ToString() }).ToList();

            ViewBag.ddlRepairName = _repairBusiness.GetAllRepairByOrgId(User.OrgId).Select(re => new SelectListItem { Text = re.RepairName, Value = re.RepairId.ToString() }).ToList();

            ViewBag.ddlServicesWarehouse = _servicesWarehouseBusiness.GetAllServiceWarehouseByOrgId(User.OrgId, User.BranchId).Select(ware => new SelectListItem { Text = ware.ServicesWarehouseName, Value = ware.SWarehouseId.ToString() }).ToList();

            ViewBag.ddlMobilePart = _mobilePartBusiness.GetAllMobilePartAndCode(User.OrgId).Select(mobile => new SelectListItem { Text = mobile.MobilePartName, Value = mobile.MobilePartId.ToString() }).ToList();

            ViewBag.ddlSymptomName = _clientProblemBusiness.GetAllClientProblemByOrgId(User.OrgId).Select(mobile => new SelectListItem { Text = mobile.ProblemName, Value = mobile.ProblemId.ToString() }).ToList();
            //New Handset//
            //ViewBag.ddlModelName = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId).Select(mobile => new SelectListItem { Text = mobile.DescriptionName, Value = mobile.DescriptionId.ToString() }).ToList();

            ViewBag.ddlModelName = _modelSSBusiness.GetAllModel(User.OrgId).Select(m => new SelectListItem { Text = m.ModelName, Value = m.ModelId.ToString() }).ToList();

            //ViewBag.ddlModelColor = Utility.ListOfModelColor().Select(r => new SelectListItem { Text = r.text, Value = r.value }).ToList();

            ViewBag.ddlModelColor = _colorSSBusiness.GetAllColorsByOrgId(User.OrgId).Select(c => new SelectListItem { Text = c.ColorName, Value = c.ColorName }).ToList();


            var jobOrder = _jobOrderBusiness.GetJobOrderById(joborderId.Value, User.OrgId);
            ViewBag.JobOrder = new JobOrderViewModel
            {
                JodOrderId = jobOrder.JodOrderId,
                JobOrderCode = jobOrder.JobOrderCode,
                EntryDate=jobOrder.EntryDate,
                CustomerName=jobOrder.CustomerName,
                Address=jobOrder.Address,
                MobileNo=jobOrder.MobileNo,
                IMEI=jobOrder.IMEI,
                IMEI2=jobOrder.IMEI2,
                Type=jobOrder.Type,
                DescriptionId=jobOrder.DescriptionId,
                ModelName= (_modelSSBusiness.GetModelById(jobOrder.DescriptionId, User.OrgId).ModelName),
                ModelColor=jobOrder.ModelColor,
                Remarks=jobOrder.Remarks,
                TSRemarks=jobOrder.TSRemarks,
                CallCenterRemarks=jobOrder.CallCenterRemarks,
                CustomerApproval=jobOrder.CustomerApproval,
                QCStatus=jobOrder.QCStatus,
                QCRemarks=jobOrder.QCRemarks,
                JobOrderType=jobOrder.JobOrderType
                
            };
            IEnumerable<JobOrderProblemDTO> prblm = _jobOrderProblemBusiness.GetJobOrderProblemByJobOrderId(joborderId.Value, User.OrgId).Select(p => new JobOrderProblemDTO
            {
                JobOrderProblemId = p.JobOrderProblemId,
                ProblemId = p.ProblemId,
                ProblemName = (_clientProblemBusiness.GetClientProblemOneByOrgId(p.ProblemId, User.OrgId).ProblemName),
            }).ToList();
            List<JobOrderProblemViewModel> problemViewModels = new List<JobOrderProblemViewModel>();
            AutoMapper.Mapper.Map(prblm, problemViewModels);
            ViewBag.Problem = problemViewModels;

            IEnumerable<JobOrderFaultDTO> Faulty = _jobOrderFaultBusiness.GetJobOrderFaultByJobOrderId(joborderId.Value, User.OrgId).Select(f=>new JobOrderFaultDTO {
                JobOrderFaultId = f.JobOrderFaultId,
                FaultId = f.FaultId,
                FaultName = (_faultBusiness.GetFaultOneByOrgId(f.FaultId, User.OrgId).FaultName),
            }).ToList();
            List<JobOrderFaultViewModel> faultViewModels = new List<JobOrderFaultViewModel>();
            AutoMapper.Mapper.Map(Faulty, faultViewModels);
            ViewBag.faulty = faultViewModels;

            IEnumerable<JobOrderServiceDTO> services = _jobOrderServiceBusiness.GetJobOrderServiceByJobOrderId(joborderId.Value, User.OrgId).Select(s => new JobOrderServiceDTO
            {
                JobOrderServiceId = s.JobOrderServiceId,
                ServiceId = s.ServiceId,
                ServiceName = (_serviceBusiness.GetServiceOneByOrgId(s.ServiceId, User.OrgId).ServiceName),
            }).ToList();
            List<JobOrderServiceViewModel> serviceViewModels = new List<JobOrderServiceViewModel>();
            AutoMapper.Mapper.Map(services, serviceViewModels);
            ViewBag.services = serviceViewModels;

            var repair = _jobOrderRepairBusiness.GetJobOrderRepairByJobId(joborderId.Value, User.OrgId);
            var  jobrepair = new JobOrderRepairViewModel();
            if(repair != null)
            {
                jobrepair = new JobOrderRepairViewModel
                {
                    JobOrderRepairId = repair.JobOrderRepairId,
                    RepairId = repair.RepairId,
                    RepairName = (_repairBusiness.GetRepairOneByOrgId(repair.RepairId, User.OrgId).RepairName),
                };
            }
            ViewBag.jobrepair = jobrepair;

            //IEnumerable<JobOrderRepairDTO> repair = _jobOrderRepairBusiness.GetJobOrderRepairByJobOrderId(joborderId.Value, User.OrgId).Select(r => new JobOrderRepairDTO
            //{
            //    JobOrderRepairId = r.JobOrderRepairId,
            //    RepairId = r.RepairId,
            //    RepairName = (_repairBusiness.GetRepairOneByOrgId(r.RepairId, User.OrgId).RepairName),
            //}).ToList();
            //List<JobOrderRepairViewModel> repairViewModels = new List<JobOrderRepairViewModel>();
            //AutoMapper.Mapper.Map(repair, repairViewModels);
            //ViewBag.repair = repairViewModels;

            IEnumerable<MobilePartStockInfoDTO> wareStock = _mobilePartStockInfoBusiness.GetAllMobilePartStockInfoById(User.OrgId, User.BranchId).Select(stock => new MobilePartStockInfoDTO
            {
                MobilePartStockInfoId = stock.MobilePartStockInfoId,
                MobilePartId = stock.MobilePartId,
                MobilePartName = (_mobilePartBusiness.GetMobilePartOneByOrgId(stock.MobilePartId.Value, User.OrgId).MobilePartName),
                StockInQty = stock.StockInQty,
                StockOutQty = stock.StockOutQty,
            }).ToList();
            List<MobilePartStockInfoViewModel> stockInfoViewModels = new List<MobilePartStockInfoViewModel>();
            AutoMapper.Mapper.Map(wareStock, stockInfoViewModels);
            ViewBag.warehouseStock = stockInfoViewModels;

            IEnumerable<SparePartsAvailableAndReqQtyDTO> spareDTO = _jobOrderBusiness.SparePartsAvailableAndReqQty(User.OrgId, User.BranchId, joborderId.Value).Select(qty => new SparePartsAvailableAndReqQtyDTO
            {
                MobilePartId = qty.MobilePartId,
                MobilePartName = qty.MobilePartName,
                AvailableQty = qty.AvailableQty,
                RequistionQty = qty.RequistionQty
            }).ToList();
            IEnumerable<SparePartsAvailableAndReqQtyViewModel> qtyViewModels = new List<SparePartsAvailableAndReqQtyViewModel>();
            AutoMapper.Mapper.Map(spareDTO, qtyViewModels);
            ViewBag.SparePartsAvailableAndReqQty = qtyViewModels;
            //stock
            IEnumerable<TechnicalServicesStockDTO> stockDTO = _technicalServicesStockBusiness.GetAllTechnicalServicesStock(joborderId.Value, User.OrgId, User.BranchId).Select(stock => new TechnicalServicesStockDTO
            {
                TsStockId = stock.TsStockId,
                JobOrderId = stock.JobOrderId,
                RequsitionInfoForJobOrderId = stock.RequsitionInfoForJobOrderId,
                RequsitionCode = (_requsitionInfoForJobOrderBusiness.GetAllRequsitionInfoOneByOrgId(stock.RequsitionInfoForJobOrderId, User.OrgId, jobOrder.BranchId.Value).RequsitionCode),
                SWarehouseId = stock.SWarehouseId,
                SWarehouseName = (_servicesWarehouseBusiness.GetServiceWarehouseOneByOrgId(stock.SWarehouseId.Value, User.OrgId, jobOrder.BranchId.Value).ServicesWarehouseName),
                PartsId = stock.PartsId,
                PartsName = (_mobilePartBusiness.GetMobilePartOneByOrgId(stock.PartsId.Value, User.OrgId).MobilePartName),
                MobilePartCode= (_mobilePartBusiness.GetMobilePartOneByOrgId(stock.PartsId.Value, User.OrgId).MobilePartCode),
                CostPrice = stock.CostPrice,
                SellPrice = stock.SellPrice,
                Quantity = stock.Quantity,
                UsedQty = stock.UsedQty,
                ReturnQty = stock.ReturnQty,
                Remarks = stock.Remarks,
                BranchId = stock.BranchId,
                OrganizationId = stock.OrganizationId,
                EUserId = stock.EUserId,
                EntryDate = DateTime.Now,
                UpUserId = stock.UpUserId,
                UpdateDate = DateTime.Now
            }).ToList();
            IEnumerable<TechnicalServicesStockViewModel> viewModel = new List<TechnicalServicesStockViewModel>();
            AutoMapper.Mapper.Map(stockDTO, viewModel);
            ViewBag.TechnicalServicesStock = viewModel;

            return View();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveJobOrderFault( List<JobOrderFaultViewModel> jobOrderFaultViewModels)
        {
            bool IsSuccess = false;
            if (ModelState.IsValid && jobOrderFaultViewModels.Count > 0)
            {
                List<JobOrderFaultDTO> listjobOrderFaultDTO = new List<JobOrderFaultDTO>();

                AutoMapper.Mapper.Map(jobOrderFaultViewModels, listjobOrderFaultDTO);

                IsSuccess = _jobOrderFaultBusiness.SaveJobOrderFault( listjobOrderFaultDTO, User.UserId, User.OrgId);
            }
            return Json(IsSuccess);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveJobOrderService(List<JobOrderServiceViewModel> jobOrderServiceViewModels)
        {
            bool IsSuccess = false;
            if (ModelState.IsValid && jobOrderServiceViewModels.Count > 0)
            {
                List<JobOrderServiceDTO> listjobOrderServiceDTO = new List<JobOrderServiceDTO>();

                AutoMapper.Mapper.Map(jobOrderServiceViewModels, listjobOrderServiceDTO);

                IsSuccess = _jobOrderServiceBusiness.SaveJobOrderServicve(listjobOrderServiceDTO, User.UserId, User.OrgId);
            }
            return Json(IsSuccess);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveJobOrderRepair(List<JobOrderRepairViewModel> jobOrderRepairViewModels,long jobOrderId)
        {
            bool IsSuccess = false;
            if (ModelState.IsValid && jobOrderRepairViewModels.Count > 0)
            {
                List<JobOrderRepairDTO> listjobOrderRepairDTO = new List<JobOrderRepairDTO>();

                AutoMapper.Mapper.Map(jobOrderRepairViewModels, listjobOrderRepairDTO);

                IsSuccess = _jobOrderRepairBusiness.SaveJobOrderRepair(listjobOrderRepairDTO, jobOrderId, User.UserId, User.OrgId);
            }
            return Json(IsSuccess);
        }
        public ActionResult SaveJobOrderProblem(List<JobOrderProblemViewModel> jobOrderProblemViewModels)
        {
            bool IsSuccess = false;
            if (ModelState.IsValid && jobOrderProblemViewModels.Count > 0)
            {
                List<JobOrderProblemDTO> listjobOrderProblemDTO = new List<JobOrderProblemDTO>();

                AutoMapper.Mapper.Map(jobOrderProblemViewModels, listjobOrderProblemDTO);

                IsSuccess = _jobOrderProblemBusiness.SaveJobOrderProblem(listjobOrderProblemDTO, User.UserId, User.OrgId);
            }
            return Json(IsSuccess);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult UpdateJobOrderTsRemarks(long jobOrderId, string remarks)
        {
            bool IsSuccess = false;
            if (jobOrderId > 0 && !string.IsNullOrEmpty(remarks))
            {
                IsSuccess = _jobOrderBusiness.UpdateJobOrderTsRemarks(jobOrderId, remarks, User.UserId, User.OrgId, User.BranchId);
            }
            return Json(IsSuccess);
        }
        public ActionResult UpdateAndChangeJobOrder(long jobId,long modelId,string imei1,string imei2,string color)
        {
            bool IsSuccess = false;
            if (jobId > 0)
            {
                IsSuccess = _handsetChangeTraceBusiness.UpdateAndChangeJobOrder(jobId, imei1, imei2, modelId,color, User.OrgId, User.BranchId, User.UserId);
            }
            return Json(IsSuccess);
        }
        #endregion

        #region Stock Return Part And Job Sign-Out
        public ActionResult GetTSStockByRequsition(long? jobOrderId)
        {
            var repair = _jobOrderRepairBusiness.GetJobOrderRepairByJobId(jobOrderId.Value, User.OrgId);
            var jobrepair = new JobOrderRepairViewModel();
            if (repair != null)
            {
                jobrepair = new JobOrderRepairViewModel
                {
                    JobOrderRepairId = repair.JobOrderRepairId,
                    RepairId = repair.RepairId,
                    RepairName = (_repairBusiness.GetRepairOneByOrgId(repair.RepairId, User.OrgId).RepairName)
                };
            }
            ViewBag.jobrepair = jobrepair;
            var dto = _technicalServicesStockBusiness.GetStockByJobOrder(jobOrderId.Value, User.UserId, User.OrgId, User.BranchId, User.RoleName).ToList();
            IEnumerable<TSStockByRequsitionViewModel> viewModels = new List<TSStockByRequsitionViewModel>();
            AutoMapper.Mapper.Map(dto, viewModels);
            return PartialView("GetTSStockByRequsition", viewModels);
        }

        public ActionResult SaveReturnStockandTsStockOut(TSStockInfoViewModel servicesStockViewModel)
        {
            bool IsSuccess = false;
            // JobOrder : , JobOrderSTokdetail
            if (ModelState.IsValid)
            {
                TSStockInfoDTO servicesStockDTOs = new TSStockInfoDTO();
                AutoMapper.Mapper.Map(servicesStockViewModel, servicesStockDTOs);
                IsSuccess = _technicalServicesStockBusiness.SaveJobSignOutWithStock(servicesStockDTOs, User.UserId, User.OrgId, User.BranchId);
            }
            return Json(IsSuccess);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult JobOrderTsStock(long jobOrderId)
        {
            var jobOrder = _jobOrderBusiness.GetJobOrdersByIdWithBranch(jobOrderId, User.BranchId, User.OrgId);
            jobOrder = jobOrder == null ? _jobOrderBusiness.GetJobOrdersByIdWithTransferBranch(jobOrderId, User.BranchId, User.OrgId) : jobOrder;
            if (jobOrder != null)
            {

                JobOrderViewModel jobOrderViewModel = new JobOrderViewModel()
                {
                    JodOrderId = jobOrder.JodOrderId,
                    JobOrderCode = jobOrder.JobOrderCode,
                    TsRepairStatus = jobOrder.TsRepairStatus,
                    TSId = jobOrder.TSId
                };

                //var JobOrderTsStock = _tsStockReturnDetailsBusiness.GetAllTsStockReturn(User.OrgId, User.BranchId).Select(s=> new TechnicalServicesStockViewModel {
                //    PartsId = s.PartsId,
                //    PartsName = _mobilePartBusiness.GetMobilePartOneByOrgId(s.PartsId,User.OrgId).MobilePartName,
                //    RequsitionCode = s.RequsitionCode,
                //    RequsitionInfoForJobOrderId = s.ReqInfoId,
                //    Quantity = s.Quantity
                //});

                var JobOrderTsStock = _technicalServicesStockBusiness.GetStockByJobOrder(jobOrder.JodOrderId, User.UserId, User.OrgId,User.BranchId, User.RoleName).Select(s => new TechnicalServicesStockViewModel
                {
                    PartsId = s.PartsId,
                    PartsName = _mobilePartBusiness.GetMobilePartOneByOrgId(s.PartsId, User.OrgId).MobilePartName,
                    RequsitionCode = s.RequsitionCode,
                    RequsitionInfoForJobOrderId = s.RequsitionInfoForJobOrderId,
                    Quantity = s.Quantity,
                    UsedQty = 0
                });

                //IEnumerable<TechnicalServicesStockViewModel> servicesStockViewModel = new List<TechnicalServicesStockViewModel>();
                //AutoMapper.Mapper.Map(JobOrderTsStock, servicesStockViewModel);

                return Json(new { jobOrder = jobOrderViewModel, tsStock = JobOrderTsStock });
            }
            return Json(null);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult UpdateReturnStatus(long returnInfoId, string status)
        {
            bool IsSuccess = false;
            if (returnInfoId > 0 && !string.IsNullOrEmpty(status))
            {
                IsSuccess = _tsStockReturnInfoBusiness.UpdateReturnInfoStatus(returnInfoId, status, User.UserId, User.OrgId, User.BranchId);
            }
            return Json(IsSuccess);
        }
        #endregion

        #region Invoice
        public ActionResult GetPartsUsedStock(long jobOrderId)
        {
            IEnumerable<InvoiceUsedPartsDTO> dto = _invoiceDetailBusiness.GetUsedPartsDetails(jobOrderId, User.OrgId).Select(qty => new InvoiceUsedPartsDTO
            {
                PartsId = qty.PartsId,
                MobilePartName = qty.MobilePartName,
                UsedQty = qty.UsedQty,
                Price = qty.Price,
                Total = qty.Total
            }).ToList();
            List<InvoiceUsedPartsViewModel> viewModel = new List<InvoiceUsedPartsViewModel>();
            AutoMapper.Mapper.Map(dto, viewModel);
            return PartialView("GetPartsUsedStock", viewModel);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveInvoiceForJobOrder(InvoiceInfoViewModel info, List<InvoiceDetailViewModel> details)
        {
            bool IsSuccess = false;
            if (ModelState.IsValid)
            {
                InvoiceInfoDTO dtoInfo = new InvoiceInfoDTO();
                List<InvoiceDetailDTO> dtoDetail = new List<InvoiceDetailDTO>();
                AutoMapper.Mapper.Map(info, dtoInfo);
                AutoMapper.Mapper.Map(details, dtoDetail);
                IsSuccess = _invoiceInfoBusiness.SaveInvoiceForJobOrder(dtoInfo, dtoDetail, User.UserId, User.OrgId, User.BranchId);
            }
            return Json(IsSuccess);
        }
        #endregion

        #region Accessories Invoice
        public ActionResult AccessoriesSells( string flag,string fromDate,string toDate, string invoice = "",string mobileNo="", int page=1)
        {
            if (string.IsNullOrEmpty(flag))
            {
                return View();
            }
            else if (!string.IsNullOrEmpty(flag) && flag.Trim() == "sell")
            {
                var dto = _invoiceInfoBusiness.GetSellsAccessories(User.OrgId, User.BranchId, fromDate, toDate,invoice,mobileNo);
                List<InvoiceInfoViewModel> viewModels = new List<InvoiceInfoViewModel>();
                // Pagination //
                ViewBag.PagerData = GetPagerData(dto.Count(),10, page);
                dto = dto.Skip((page - 1) * 10).Take(10).ToList();
                //-----------------//
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_AccessoriesSells",viewModels);
            }
            return Content("");
        }
        public ActionResult CreateAccessoriesInvoice()
        {
            ViewBag.ddlMobileParts = _mobilePartStockInfoBusiness.GetAllGoodMobilePartsAndCode(User.OrgId).Select(mobile => new SelectListItem { Text = mobile.MobilePartName, Value = mobile.MobilePartId.ToString() }).ToList();

            ViewBag.ddlFaultyMobileParts = _faultyStockInfoBusiness.GetAllFaultyMobilePartsAndCode(User.OrgId).Select(s => new SelectListItem { Text = s.MobilePartName, Value = s.PartsId.ToString() }).ToList();

            ViewBag.ddlsellsPrice = _mobilePartStockInfoBusiness.GetAllMobilePartStockInfoByOrgId(User.OrgId, User.BranchId).Select(mobile => new SelectListItem { Text = mobile.SellPrice.ToString(), Value = mobile.MobilePartStockInfoId.ToString() }).ToList();

            ViewBag.ddlHandset = _handSetStockBusiness.GetAllHansetModelAndColor(User.OrgId).Select(s => new SelectListItem { Text = s.ModelName, Value = s.ModelId.ToString() }).ToList();

            //.ViewBag.ddlPartsModelName = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId).Select(mobile => new SelectListItem { Text = mobile.DescriptionName, Value = mobile.DescriptionId.ToString() }).ToList();

            ViewBag.ddlPartsModelName = _modelSSBusiness.GetAllModel(User.OrgId).Select(m => new SelectListItem { Text = m.ModelName, Value = m.ModelId.ToString() }).ToList();
            return View();
        }

        public ActionResult SaveInvoiceForAccessoriesSells(InvoiceInfoViewModel info, List<InvoiceDetailViewModel> details)
        {
            bool IsSuccess = false;
            if (ModelState.IsValid)
            {
                InvoiceInfoDTO dtoInfo = new InvoiceInfoDTO();
                List<InvoiceDetailDTO> dtoDetail = new List<InvoiceDetailDTO>();
                AutoMapper.Mapper.Map(info, dtoInfo);
                AutoMapper.Mapper.Map(details, dtoDetail);
                IsSuccess = _invoiceInfoBusiness.SaveInvoiceForAccessoriesSells(dtoInfo, dtoDetail, User.UserId, User.OrgId, User.BranchId);
            }
            return Json(IsSuccess);


        }
        #endregion

        #region Reports
        [HttpGet]
        public ActionResult GetJobOrderListReport()
        {
            //ViewBag.ddlModelName = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId).Select(d => new SelectListItem { Text = d.DescriptionName, Value = d.DescriptionId.ToString() }).ToList();
            ViewBag.ddlModelName = _modelSSBusiness.GetAllModel(User.OrgId).Select(m => new SelectListItem { Text = m.ModelName, Value = m.ModelId.ToString() }).ToList();

            ViewBag.ddlStateStatus = Utility.ListOfJobOrderStatus().Select(r => new SelectListItem { Text = r.text, Value = r.value }).ToList();
            ViewBag.ddlCustomerType = Utility.ListOfCustomerType().Select(r => new SelectListItem { Text = r.text, Value = r.value }).ToList();
            ViewBag.ddlJobType = Utility.ListOfJobOrderType().Select(r => new SelectListItem { Text = r.text, Value = r.value }).ToList();
            return View();
        }
        public ActionResult TSRequsitionListReport()
        {
            ViewBag.ddlWarehouseName = _servicesWarehouseBusiness.GetAllServiceWarehouseByOrgId(User.OrgId, User.BranchId).Select(ware => new SelectListItem { Text = ware.ServicesWarehouseName, Value = ware.SWarehouseId.ToString() }).ToList();

            ViewBag.ddlStateStatus = Utility.ListOfReqStatus().Where(status => status.value == RequisitionStatus.Current || status.value == RequisitionStatus.Pending || status.value == RequisitionStatus.Approved || status.value == RequisitionStatus.Rejected || status.value == RequisitionStatus.Void).Select(st => new SelectListItem
            {
                Text = st.text,
                Value = st.value
            }).ToList();

            ViewBag.ddlTechnicalServicesName = _roleBusiness.GetRoleByTechnicalServicesId(string.Empty, User.OrgId, User.BranchId).Select(d => new SelectListItem { Text = d.UserName, Value = d.UserId.ToString() }).ToList();
            /////////
            ViewBag.ddlWarehouseName2 = _servicesWarehouseBusiness.GetAllServiceWarehouseByOrgId(User.OrgId, User.BranchId).Select(ware => new SelectListItem { Text = ware.ServicesWarehouseName, Value = ware.SWarehouseId.ToString() }).ToList();

            ViewBag.ddlStateStatus2 = Utility.ListOfReqStatus().Where(status => status.value == RequisitionStatus.Current || status.value == RequisitionStatus.Pending || status.value == RequisitionStatus.Approved || status.value == RequisitionStatus.Rejected || status.value == RequisitionStatus.Void).Select(st => new SelectListItem
            {
                Text = st.text,
                Value = st.value
            }).ToList();
            ViewBag.ddlTechnicalServicesName2 = _roleBusiness.GetRoleByTechnicalServicesId(string.Empty, User.OrgId, User.BranchId).Select(d => new SelectListItem { Text = d.UserName, Value = d.UserId.ToString() }).ToList();
            return View();
        }

        [HttpGet]
        public ActionResult ReturnParts(string flag, long? mobilePartId,long? tsId,string jobCode, string fromDate, string toDate,int page=1)
        {
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlMobileParts = _mobilePartBusiness.GetAllMobilePartAndCode(User.OrgId).Select(mobile => new SelectListItem { Text = mobile.MobilePartName, Value = mobile.MobilePartId.ToString() }).ToList();

                ViewBag.ddlTechnicalServicesName = _roleBusiness.GetRoleByTechnicalServicesId(string.Empty, User.OrgId, User.BranchId).Select(d => new SelectListItem { Text = d.UserName, Value = d.UserId.ToString() }).ToList();
                return View();
            }
            else
            {
                //IEnumerable<TsStockReturnDetailDTO> dto = _tsStockReturnDetailsBusiness.GetAllTsStockReturn(User.OrgId, User.BranchId).Select(ret => new TsStockReturnDetailDTO
                //{
                //    RequsitionCode = ret.RequsitionCode,
                //    PartsId = ret.PartsId,
                //    PartsName = (_mobilePartBusiness.GetMobilePartOneByOrgId(ret.PartsId, User.OrgId).MobilePartName),
                //    PartsCode = (_mobilePartBusiness.GetMobilePartOneByOrgId(ret.PartsId, User.OrgId).MobilePartCode),
                //    Quantity = ret.Quantity,
                //    EntryDate = ret.EntryDate,
                //    EUserId=ret.EUserId,
                //   EntryUser= UserForEachRecord(ret.EUserId.Value).UserName,
                //}).AsEnumerable();
                //dto = dto.Where(f => 1 == 1 && (mobilePartId == null || mobilePartId <= 0 || f.PartsId == mobilePartId) && (tsId == null || tsId <= 0 || f.EUserId == tsId) &&
                //             (
                //                 (fromDate == null && toDate == null)
                //                 ||
                //                  (fromDate == "" && toDate == "")
                //                 ||
                //                 (fromDate.Trim() != "" && toDate.Trim() != "" &&

                //                     f.EntryDate.Value.Date >= Convert.ToDateTime(fromDate).Date &&
                //                     f.EntryDate.Value.Date <= Convert.ToDateTime(toDate).Date)
                //                 ||
                //                 (fromDate.Trim() != "" && f.EntryDate.Value.Date == Convert.ToDateTime(fromDate).Date)
                //                 ||
                //                 (toDate.Trim() != "" && f.EntryDate.Value.Date == Convert.ToDateTime(toDate).Date)
                //             )
                //         );
                var dto = _tsStockReturnDetailsBusiness.GetReturnParts(User.OrgId, User.BranchId, tsId, mobilePartId, jobCode, fromDate, toDate);
                List<TsStockReturnDetailViewModel> viewModel = new List<TsStockReturnDetailViewModel>();
                // Pagination //
                ViewBag.PagerData = GetPagerData(dto.Count(), 10, page);
                dto = dto.Skip((page - 1) * 10).Take(10).ToList();
                //-----------------//
                AutoMapper.Mapper.Map(dto, viewModel);
                return PartialView("_ReturnParts", viewModel);
            }
        }
        [HttpGet]
        public ActionResult GetUsedParts(string flag, long? partsId,long? tsId, string fromDate, string toDate, string jobCode, int page=1)
        {
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlMobileParts = _mobilePartBusiness.GetAllMobilePartAndCode(User.OrgId).Select(mobile => new SelectListItem { Text = mobile.MobilePartName, Value = mobile.MobilePartId.ToString() }).ToList();

                ViewBag.ddlTechnicalServicesName = _roleBusiness.GetRoleByTechnicalServicesId(string.Empty, User.OrgId, User.BranchId).Select(d => new SelectListItem { Text = d.UserName, Value = d.UserId.ToString() }).ToList();
                //
                ViewBag.ddlMobileParts2 = _mobilePartBusiness.GetAllMobilePartAndCode(User.OrgId).Select(mobile => new SelectListItem { Text = mobile.MobilePartName, Value = mobile.MobilePartId.ToString() }).ToList();

                ViewBag.ddlTechnicalServicesName2 = _roleBusiness.GetRoleByTechnicalServicesId(string.Empty, User.OrgId, User.BranchId).Select(d => new SelectListItem { Text = d.UserName, Value = d.UserId.ToString() }).ToList();
                return View();
            }
            else
            {
                var dto = _technicalServicesStockBusiness.GetUsedParts(partsId,tsId, User.OrgId, User.BranchId, fromDate, toDate,jobCode);
                List<TechnicalServicesStockViewModel> viewModels = new List<TechnicalServicesStockViewModel>();
                // Pagination //
                ViewBag.PagerData = GetPagerData(dto.Count(), 10, page);
                dto = dto.Skip((page - 1) * 10).Take(10).ToList();
                //-----------------//
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetUsedParts", viewModels);
            }
        }
    
        public ActionResult GetSellsReport(string flag, string fromDate, string toDate,string invoiceType,string invoice, int page=1)
        {
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlInvoiceTypeStatus = Utility.ListOfInvoiceTypeStatus().Select(i => new SelectListItem
                {
                    Text = i.text,
                    Value = i.value
                }).ToList();
                return View();
            }
            else
            {
                var dto = _invoiceInfoBusiness.GetSellsReport(User.OrgId, User.BranchId, fromDate, toDate, invoiceType, invoice);
                List<InvoiceInfoViewModel> viewModels = new List<InvoiceInfoViewModel>();
                // Pagination //
                ViewBag.PagerData = GetPagerData(dto.Count(), 10, page);
                dto = dto.Skip((page - 1) * 10).Take(10).ToList();
                //-----------------//
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetSellsReport", viewModels);
            }
        }
        public ActionResult SellsDetails(long invoiceId)
        {
            var sells = _invoiceInfoBusiness.GetAllInvoiceByOrgId(invoiceId, User.OrgId, User.BranchId);
            ViewBag.Sells = new InvoiceInfoViewModel
            {
                TotalSPAmount=sells.TotalSPAmount,
                LabourCharge=sells.LabourCharge,
                VAT=sells.VAT,
                Tax=sells.Tax,
                Discount=sells.Discount,
                NetAmount=sells.NetAmount,
            };
            return PartialView("_SellsDetails");
        }

        public ActionResult JobRepairOtherBranch(string flag, long? branchName, string fromDate, string toDate,int page=1)
        {
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlBranchName = _branchBusiness.GetBranchByOrgId(User.OrgId).Where(b => b.BranchId != User.BranchId).Select(branch => new SelectListItem { Text = branch.BranchName, Value = branch.BranchId.ToString() }).ToList();
                ViewBag.ddlBranchName2 = _branchBusiness.GetBranchByOrgId(User.OrgId).Where(b => b.BranchId != User.BranchId).Select(branch => new SelectListItem { Text = branch.BranchName, Value = branch.BranchId.ToString() }).ToList();
                return View();
            }
            else
            {
                var dto = _jobOrderReturnDetailBusiness.RepairOtherBranchJob(User.BranchId, branchName, User.OrgId, fromDate, toDate);
                List<JobOrderReturnDetailViewModel> viewModels = new List<JobOrderReturnDetailViewModel>();
                // Pagination //
                ViewBag.PagerData = GetPagerData(dto.Count(), 10, page);
                dto = dto.Skip((page - 1) * 10).Take(10).ToList();
                //-----------------//
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_JobRepairOtherBranch", viewModels);
            }
        }
        public ActionResult RepairedJobOfOtherBranch(string flag, long? branchName, string fromDate, string toDate,int page=1)
        {
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlBranchName2 = _branchBusiness.GetBranchByOrgId(User.OrgId).Where(b => b.BranchId != User.BranchId).Select(branch => new SelectListItem { Text = branch.BranchName, Value = branch.BranchId.ToString() }).ToList();
                return View();
            }
            else
            {
                var dto = _jobOrderReturnDetailBusiness.RepairedJobOfOtherBranch(User.BranchId, branchName, User.OrgId, fromDate, toDate);
                List<JobOrderReturnDetailViewModel> viewModels = new List<JobOrderReturnDetailViewModel>();
                // Pagination //
                ViewBag.PagerData = GetPagerData(dto.Count(), 10, page);
                dto = dto.Skip((page - 1) * 10).Take(10).ToList();
                //-----------------//
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_RepairedJobOfOtherBranch", viewModels);
            }
        }
        public ActionResult TSOtherBranchRequsitionReport(string flag, string reqCode, long? warehouseId, long? tsId, string rstatus, string fromDate, string toDate, int page = 1)
        {
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlWarehouseName2 = _servicesWarehouseBusiness.GetAllServiceWarehouseByOrgId(User.OrgId, User.BranchId).Select(ware => new SelectListItem { Text = ware.ServicesWarehouseName, Value = ware.SWarehouseId.ToString() }).ToList();

                ViewBag.ddlStateStatus2 = Utility.ListOfReqStatus().Where(status => status.value == RequisitionStatus.Current || status.value == RequisitionStatus.Pending || status.value == RequisitionStatus.Approved || status.value == RequisitionStatus.Rejected || status.value == RequisitionStatus.Void).Select(st => new SelectListItem
                {
                    Text = st.text,
                    Value = st.value
                }).ToList();
                ViewBag.ddlTechnicalServicesName2 = _roleBusiness.GetRoleByTechnicalServicesId(string.Empty, User.OrgId, User.BranchId).Select(d => new SelectListItem { Text = d.UserName, Value = d.UserId.ToString() }).ToList();
                return View();
            }
            else
            {
                var dto = _requsitionInfoForJobOrderBusiness.GetRequsitionInfoOtherBranchData(reqCode, warehouseId, tsId, rstatus, fromDate, toDate, User.OrgId, User.BranchId);
                List<RequsitionInfoForJobOrderViewModel> viewModels = new List<RequsitionInfoForJobOrderViewModel>();
                // Pagination //
                ViewBag.PagerData = GetPagerData(dto.Count(), 10, page);
                dto = dto.Skip((page - 1) * 10).Take(10).ToList();
                //-----------------//
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("AnotherBranchRequsitionPartialList", viewModels);
            }
        }
        public ActionResult ReceiveJobOrderReport(string flag, long? branchName, string fromDate, string tstatus, string toDate, string jobCode = "", string transferCode = "",int page=1)
        {
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlBranchName = _branchBusiness.GetBranchByOrgId(User.OrgId).Where(b => b.BranchId != User.BranchId).Select(branch => new SelectListItem { Text = branch.BranchName, Value = branch.BranchId.ToString() }).ToList();

                ViewBag.ddlTransferStatus = Utility.ListOfTransferStatus().Select(r => new SelectListItem { Text = r.text, Value = r.value }).ToList();
                /////
                ViewBag.ddlBranchName2 = _branchBusiness.GetBranchByOrgId(User.OrgId).Where(b => b.BranchId != User.BranchId).Select(branch => new SelectListItem { Text = branch.BranchName, Value = branch.BranchId.ToString() }).ToList();

                ViewBag.ddlTransferStatus2 = Utility.ListOfTransferStatus().Select(r => new SelectListItem { Text = r.text, Value = r.value }).ToList();
                return View();
            }
            else
            {
                var dto = _jobOrderTransferDetailBusiness.GetReceiveJob(User.OrgId, User.BranchId, branchName, jobCode, transferCode, fromDate, toDate, tstatus);
                List<JobOrderTransferDetailViewModel> viewModels = new List<JobOrderTransferDetailViewModel>();
                // Pagination //
                ViewBag.PagerData = GetPagerData(dto.Count(), 10, page);
                dto = dto.Skip((page - 1) * 10).Take(10).ToList();
                //-----------------//
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_ReceiveJobOrder", viewModels);
            }
        }
        public ActionResult ReceiveReturnJobOrderReport(string flag, long? branchName, string fromDate, string toDate, string tstatus, string jobCode = "", string transferCode = "",int page=1)
        {
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlBranchName = _branchBusiness.GetBranchByOrgId(User.OrgId).Where(b => b.BranchId != User.BranchId).Select(branch => new SelectListItem { Text = branch.BranchName, Value = branch.BranchId.ToString() }).ToList();

                ViewBag.ddlTransferStatus = Utility.ListOfTransferStatus().Select(r => new SelectListItem { Text = r.text, Value = r.value }).ToList();
                return View();
            }
            else
            {
                var dto = _jobOrderReturnDetailBusiness.GetReturnJobOrder(User.OrgId, User.BranchId, branchName, jobCode, transferCode, fromDate, toDate, tstatus);
                List<JobOrderReturnDetailViewModel> viewModels = new List<JobOrderReturnDetailViewModel>();
                // Pagination //
                ViewBag.PagerData = GetPagerData(dto.Count(), 10, page);
                dto = dto.Skip((page - 1) * 10).Take(10).ToList();
                //-----------------//
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_ReceiveReturnJobOrder", viewModels);
            }
        }
        public ActionResult JobSignInAndOut(string flag, long? tsId, string fromDate, string toDate, string jobCode="",int page=1)
        {
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlTechnicalServicesName = _roleBusiness.GetRoleByTechnicalServicesId(string.Empty, User.OrgId, User.BranchId).Select(d => new SelectListItem { Text = d.UserName, Value = d.UserId.ToString() }).ToList();
                return View();
            }
            else
            {
                var dto = _jobOrderTSBusiness.JobSignInAndOut(tsId,jobCode,User.OrgId, User.BranchId,fromDate,toDate);
                List<JobOrderTSViewModel> viewModels = new List<JobOrderTSViewModel>();
                // Pagination //
                ViewBag.PagerData = GetPagerData(dto.Count(), 10, page);
                dto = dto.Skip((page - 1) * 10).Take(10).ToList();
                //-----------------//
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_JobSignInAndOut", viewModels);
            }
        }

        public ActionResult DailySummaryReport(string flag, string fromDate, string toDate,int page=1)
        {
            if (string.IsNullOrEmpty(flag))
            {
                return View();
            }
            else
            {
                var dto = _jobOrderBusiness.DailySummaryReport(User.OrgId, User.BranchId,fromDate,toDate);
                List<DailySummaryReportViewModel> viewModels = new List<DailySummaryReportViewModel>();
                // Pagination //
                ViewBag.PagerData = GetPagerData(dto.Count(), 5, page);
                dto = dto.Skip((page - 1) * 5).Take(5).ToList();
                //-----------------//
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_DailySummaryReport", viewModels);
            }
        }

        public ActionResult AllBranchDailySummaryReport(string flag, string fromDate, string toDate, int page = 1)
        {
            if (string.IsNullOrEmpty(flag))
            {
                return View();
            }
            else
            {
                var dto = _jobOrderBusiness.AllBranchDailySummaryReport(User.OrgId, fromDate, toDate);
                List<DailySummaryReportViewModel> viewModels = new List<DailySummaryReportViewModel>();
                // Pagination //
                ViewBag.PagerData = GetPagerData(dto.Count(), 10, page);
                dto = dto.Skip((page - 1) * 10).Take(10).ToList();
                //-----------------//
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_AllBranchDailySummaryReport", viewModels);
            }
        }

        public ActionResult GetHandsetChangeInformationList(string flag, string fromDate, string toDate,int page=1)
        {
            if (string.IsNullOrEmpty(flag))
            {
                return View();
            }
            else
            {
                var dto = _handsetChangeTraceBusiness.GetHandsetChangeList(User.OrgId, User.BranchId, fromDate, toDate);
                List<HandsetChangeInformationViewModel> viewModels = new List<HandsetChangeInformationViewModel>();
                // Pagination //
                ViewBag.PagerData = GetPagerData(dto.Count(), 10, page);
                dto = dto.Skip((page - 1) * 10).Take(10).ToList();
                //-----------------//
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetHandsetChangeInformationList", viewModels);
            }
        }
        #endregion

        #region Call Center
        public ActionResult GetJobOrderListForCallCenter(string flag, string fromDate, string toDate, long? modelId, long? jobOrderId, string mobileNo = "", string status = "", string jobCode = "", string iMEI = "", string iMEI2 = "", string tab = "", string customerType = "", string jobType = "",string repairStatus="",string customer="", int page = 1)
        {
            ViewBag.UserPrivilege = UserPrivilege("FrontDesk", "GetJobOrders");
            if (string.IsNullOrEmpty(flag))
            {
                //ViewBag.ddlModelName = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId).Select(d => new SelectListItem { Text = d.DescriptionName, Value = d.DescriptionId.ToString() }).ToList();
                ViewBag.ddlModelName = _modelSSBusiness.GetAllModel(User.OrgId).Select(m => new SelectListItem { Text = m.ModelName, Value = m.ModelId.ToString() }).ToList();

                ViewBag.ddlStateStatus = Utility.ListOfJobOrderStatus().Select(r => new SelectListItem { Text = r.text, Value = r.value }).ToList();
                ViewBag.ddlCustomerType = Utility.ListOfCustomerType().Select(r => new SelectListItem { Text = r.text, Value = r.value }).ToList();
                ViewBag.ddlJobType = Utility.ListOfJobOrderType().Select(r => new SelectListItem { Text = r.text, Value = r.value }).ToList();

                ViewBag.ddlCallCenterApproval = Utility.ListOfCallCenterApproval().Select(r => new SelectListItem { Text = r.text, Value = r.value }).ToList();

                return View();
            }
            else if (!string.IsNullOrEmpty(flag) && (flag == "view" || flag == "search" || flag == "Detail" || flag == "Assign" || flag=="TSWork"))
            {
                var dto = _jobOrderBusiness.GetJobOrders(mobileNo.Trim(), modelId, status.Trim(), jobOrderId, jobCode, iMEI.Trim(), iMEI2.Trim(), User.OrgId, User.BranchId, fromDate, toDate,customerType,jobType, repairStatus, customer);

                IEnumerable<JobOrderViewModel> viewModels = new List<JobOrderViewModel>();
               
                if (flag == "view" || flag == "search")
                {
                    // Pagination //
                    ViewBag.PagerData = GetPagerData(dto.Count(), 5, page);
                    dto = dto.Skip((page - 1) * 5).Take(5).ToList();
                    //-----------------//
                    AutoMapper.Mapper.Map(dto, viewModels);
                    return PartialView("_GetJobOrderForCallCenter", viewModels);
                }
                //if (flag == "TSWork")
                //{
                //    AutoMapper.Mapper.Map(dto, viewModels);
                //    return PartialView("_GetTSWorkDetails", viewModels.FirstOrDefault());
                //}
                else if (flag == "Detail")// Flag = Detail
                {
                    var oldHandset = _handsetChangeTraceBusiness.GetOneJobByOrgId(jobOrderId.Value,User.OrgId);
                    var handset = new HandsetChangeTraceViewModel();
                    if (oldHandset != null)
                    {
                        handset = new HandsetChangeTraceViewModel
                        {
                            IMEI1 = oldHandset.IMEI1,
                            IMEI2 = oldHandset.IMEI2,
                            ModelId = oldHandset.ModelId,
                            ModelName = (_modelSSBusiness.GetModelById(oldHandset.ModelId, User.OrgId).ModelName),
                            Color = oldHandset.Color,
                        };
                    }
                    ViewBag.OldHansetInformation = handset;

                    AutoMapper.Mapper.Map(dto, viewModels);
                    ViewBag.ddlJobOrderType = Utility.ListOfJobOrderType().Select(r => new SelectListItem { Text = r.text, Value = r.value }).ToList();
                    return PartialView("_GetJobOrderDetailsForCallCenter", viewModels.FirstOrDefault());
                }
               
            }
            return View();
        }

        public ActionResult SaveCallCenterApproval(long jobId,string approval,string remarks)
        {
            bool IsSuccess = false;
            if (jobId > 0)
            {
                IsSuccess = _jobOrderBusiness.SaveCallCenterApproval(jobId, approval, remarks, User.UserId, User.OrgId);
            }
            return Json(IsSuccess);
        }
        #endregion

        #region QC
        public ActionResult GetJobOrderListForQc(string flag, string fromDate, string toDate, long? modelId, long? jobOrderId, string mobileNo = "", string status = "", string jobCode = "", string iMEI = "", string iMEI2 = "", string tab = "", string customerType = "", string jobType = "", string repairStatus = "", int page = 1)
        {
            ViewBag.UserPrivilege = UserPrivilege("FrontDesk", "GetJobOrderListForQc");
            if (string.IsNullOrEmpty(flag))
            {
                //ViewBag.ddlModelName = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId).Select(d => new SelectListItem { Text = d.DescriptionName, Value = d.DescriptionId.ToString() }).ToList();

                ViewBag.ddlModelName = _modelSSBusiness.GetAllModel(User.OrgId).Select(m => new SelectListItem { Text = m.ModelName, Value = m.ModelId.ToString() }).ToList();

                ViewBag.ddlStateStatus = Utility.ListOfJobOrderStatus().Select(r => new SelectListItem { Text = r.text, Value = r.value }).ToList();
                ViewBag.ddlCustomerType = Utility.ListOfCustomerType().Select(r => new SelectListItem { Text = r.text, Value = r.value }).ToList();
                ViewBag.ddlJobType = Utility.ListOfJobOrderType().Select(r => new SelectListItem { Text = r.text, Value = r.value }).ToList();

                ViewBag.ddlQCStatus = Utility.ListOfQCStatus().Select(r => new SelectListItem { Text = r.text, Value = r.value }).ToList();

                return View();
            }
            else if (!string.IsNullOrEmpty(flag) && (flag == "view" || flag == "search" || flag == "Detail" || flag == "Assign" || flag == "TSWork"))
            {
                var dto = _jobOrderBusiness.GetJobOrderForQc(mobileNo.Trim(), modelId, status.Trim(), jobOrderId, jobCode, iMEI.Trim(), iMEI2.Trim(), User.OrgId, User.BranchId, fromDate, toDate, customerType, jobType, repairStatus);

                IEnumerable<JobOrderViewModel> viewModels = new List<JobOrderViewModel>();

                if (flag == "view" || flag == "search")
                {
                    // Pagination //
                    ViewBag.PagerData = GetPagerData(dto.Count(), 5, page);
                    dto = dto.Skip((page - 1) * 5).Take(5).ToList();
                    //-----------------//
                    AutoMapper.Mapper.Map(dto, viewModels);
                    return PartialView("_GetJobOrderListForQc", viewModels);
                }
                //if (flag == "TSWork")
                //{
                //    AutoMapper.Mapper.Map(dto, viewModels);
                //    return PartialView("_GetTSWorkDetails", viewModels.FirstOrDefault());
                //}
                else if (flag == "Detail")// Flag = Detail
                {
                    var oldHandset = _handsetChangeTraceBusiness.GetOneJobByOrgId(jobOrderId.Value, User.OrgId);
                    var handset = new HandsetChangeTraceViewModel();
                    if (oldHandset != null)
                    {
                        handset = new HandsetChangeTraceViewModel
                        {
                            IMEI1 = oldHandset.IMEI1,
                            IMEI2 = oldHandset.IMEI2,
                            ModelId = oldHandset.ModelId,
                            ModelName = (_modelSSBusiness.GetModelById(oldHandset.ModelId, User.OrgId).ModelName),
                            Color = oldHandset.Color,
                        };
                    }
                    ViewBag.OldHansetInformation = handset;

                    AutoMapper.Mapper.Map(dto, viewModels);
                    ViewBag.ddlJobOrderType = Utility.ListOfJobOrderType().Select(r => new SelectListItem { Text = r.text, Value = r.value }).ToList();
                    return PartialView("_GetJobOrderDetailsForQC", viewModels.FirstOrDefault());
                }

            }
            return View();
        }

        public ActionResult SaveQCApproval(long jobId, string approval, string remarks)
        {
            bool IsSuccess = false;
            if (jobId > 0)
            {
                IsSuccess = _jobOrderBusiness.SaveQCApproval(jobId, approval, remarks, User.UserId, User.OrgId,User.BranchId);
                if (approval == "QC-Fail")
                {
                    IsSuccess = _jobOrderTSBusiness.UpdateJobOrderTsForQcFail(jobId, User.UserId, User.OrgId, User.BranchId);
                }
                
            }
            return Json(IsSuccess);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult UpdateQCTransferStatus(long jobOrderId)
        {
            bool IsSuccess = false;
            if (jobOrderId > 0)
            {
                IsSuccess = _jobOrderBusiness.UpdateQCTransferStatus(jobOrderId, User.OrgId, User.BranchId, User.UserId);
            }
            return Json(IsSuccess);
        }
        #endregion
    }
}