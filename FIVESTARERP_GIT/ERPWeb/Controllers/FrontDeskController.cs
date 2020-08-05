using ERPBLL.Common;
using ERPBLL.Configuration.Interface;
using ERPBLL.ControlPanel.Interface;
using ERPBLL.FrontDesk.Interface;
using ERPBLL.Inventory.Interface;
using ERPBO.Common;
using ERPBO.Configuration.DTOModels;
using ERPBO.Configuration.ViewModels;
using ERPBO.FrontDesk.DomainModels;
using ERPBO.FrontDesk.DTOModels;
using ERPBO.FrontDesk.ViewModels;
using ERPDAL.FrontDeskDAL;
using ERPWeb.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        public FrontDeskController(IAccessoriesBusiness accessoriesBusiness, IClientProblemBusiness clientProblemBusiness, IDescriptionBusiness descriptionBusiness, IJobOrderBusiness jobOrderBusiness, ITechnicalServiceBusiness technicalServiceBusiness,ICustomerBusiness customerBusiness, IRequsitionInfoForJobOrderBusiness requsitionInfoForJobOrderBusiness, IRequsitionDetailForJobOrderBusiness requsitionDetailForJobOrderBusiness, IServicesWarehouseBusiness servicesWarehouseBusiness, IBranchBusiness branchBusiness, IMobilePartBusiness mobilePartBusiness, IMobilePartStockInfoBusiness mobilePartStockInfoBusiness, IMobilePartStockDetailBusiness mobilePartStockDetailBusiness, ITechnicalServicesStockBusiness technicalServicesStockBusiness, IJobOrderAccessoriesBusiness jobOrderAccessoriesBusiness, IJobOrderProblemBusiness jobOrderProblemBusiness, IFaultBusiness faultBusiness, IServiceBusiness serviceBusiness, IJobOrderFaultBusiness jobOrderFaultBusiness, IJobOrderServiceBusiness jobOrderServiceBusiness, IJobOrderRepairBusiness jobOrderRepairBusiness, IRepairBusiness repairBusiness, IRoleBusiness roleBusiness, ITsStockReturnInfoBusiness tsStockReturnInfoBusiness, ITsStockReturnDetailsBusiness tsStockReturnDetailsBusiness, IInvoiceInfoBusiness invoiceInfoBusiness, IInvoiceDetailBusiness invoiceDetailBusiness)
        {
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
        }

        #region JobOrder
        [HttpGet]
        public ActionResult GetJobOrders(string flag, long? modelId, long? jobOrderId, string mobileNo = "", string status = "", string jobCode = "", string iMEI = "", string iMEI2 = "", int page = 1)
        {
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlModelName = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId).Select(d => new SelectListItem { Text = d.DescriptionName, Value = d.DescriptionId.ToString() }).ToList();

                ViewBag.ddlStateStatus = Utility.ListOfJobOrderStatus().Select(r => new SelectListItem { Text = r.text, Value = r.value }).ToList();

                return View();
            }
            else if (!string.IsNullOrEmpty(flag) && (flag == "view" || flag == "search" || flag == "Detail" || flag == "Assign"))
            {
                var dto = _jobOrderBusiness.GetJobOrders(mobileNo.Trim(), modelId, status.Trim(), jobOrderId, jobCode, iMEI.Trim(), iMEI2.Trim(), User.OrgId, User.BranchId);

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
                else if (flag == "Detail")// Flag = Detail
                {
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

        [HttpGet]
        public ActionResult CreateJobOrder(long? jobOrderId)
        {
            ViewBag.ddlDescriptions = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId).Select(d => new SelectListItem { Text = d.DescriptionName, Value = d.DescriptionId.ToString() }).ToList();

            ViewBag.ddlAccessories = _accessoriesBusiness.GetAllAccessoriesByOrgId(User.OrgId).Select(a => new SelectListItem { Text = a.AccessoriesName, Value = a.AccessoriesId.ToString() }).ToList();

            ViewBag.ddlProblems = _clientProblemBusiness.GetAllClientProblemByOrgId(User.OrgId).Select(p => new SelectListItem { Text = p.ProblemName, Value = p.ProblemId.ToString() }).ToList();

            ViewBag.ddlModelColor = Utility.ListOfModelColor().Select(r => new SelectListItem { Text = r.text, Value = r.value }).ToList();

            ViewBag.ddlPhoneTypes = Utility.ListOfPhoneTypes().Select(r => new SelectListItem { Text = r.text, Value = r.value }).ToList();

            ViewBag.ddlJobOrderType = Utility.ListOfJobOrderType().Select(r => new SelectListItem { Text = r.text, Value = r.value }).ToList();

            ViewBag.ddlCustomerType = Utility.ListOfCustomerType().Select(r => new SelectListItem { Text = r.text, Value = r.value }).ToList();

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
        public ActionResult SaveJobOrder(JobOrderViewModel jobOrder, List<JobOrderAccessoriesViewModel> jobOrderAccessories, List<JobOrderProblemViewModel> jobOrderProblems)
        {
            bool IsSuccess = false;
            if (ModelState.IsValid && jobOrderProblems.Count > 0)
            {
                JobOrderDTO jobOrderDTO = new JobOrderDTO();
                List<JobOrderAccessoriesDTO> listJobOrderAccessoriesDTO = new List<JobOrderAccessoriesDTO>();
                List<JobOrderProblemDTO> listJobOrderProblemDTO = new List<JobOrderProblemDTO>();

                AutoMapper.Mapper.Map(jobOrder, jobOrderDTO);
                AutoMapper.Mapper.Map(jobOrderAccessories, listJobOrderAccessoriesDTO);
                AutoMapper.Mapper.Map(jobOrderProblems, listJobOrderProblemDTO);

                IsSuccess = _jobOrderBusiness.SaveJobOrder(jobOrderDTO, listJobOrderAccessoriesDTO, listJobOrderProblemDTO, User.UserId, User.OrgId, User.BranchId);
            }
            return Json(IsSuccess);
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
                ModelName = (_descriptionBusiness.GetDescriptionOneByOrdId(jobOrder.DescriptionId, User.OrgId).DescriptionName),
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
        public ActionResult GetJobOrdersTS(string flag, long? modelId, long? jobOrderId, string mobileNo = "", string jobCode = "")
        {
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlModelName = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId).Select(d => new SelectListItem { Text = d.DescriptionName, Value = d.DescriptionId.ToString() }).ToList();

                return View();
            }
            else if (!string.IsNullOrEmpty(flag) && (flag == "view" || flag == "search" || flag == "Detail" || flag == "Assign"))
            {
                var dto = _jobOrderBusiness.GetJobOrdersTS(User.RoleName, mobileNo.Trim(), modelId, jobOrderId, jobCode, User.UserId, User.OrgId, User.BranchId);

                IEnumerable<JobOrderViewModel> viewModels = new List<JobOrderViewModel>();
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

            ViewBag.ddlMobilePart = _mobilePartBusiness.GetAllMobilePartByOrgId(User.OrgId).Select(mobile => new SelectListItem { Text = mobile.MobilePartName, Value = mobile.MobilePartId.ToString() }).ToList();
            return View();
        }

        public ActionResult RequsitionInfoForJobOrderPartialListList(long jobOrderId)
        {
            IEnumerable<RequsitionInfoForJobOrderDTO> requsitionInfoForJobOrderDTO = _requsitionInfoForJobOrderBusiness.GetAllRequsitionInfoForJobOrder(jobOrderId, User.OrgId, User.BranchId).Select(info => new RequsitionInfoForJobOrderDTO
            {
                RequsitionInfoForJobOrderId = info.RequsitionInfoForJobOrderId,
                RequsitionCode = info.RequsitionCode,
                SWarehouseId = info.SWarehouseId,
                SWarehouseName = (_servicesWarehouseBusiness.GetServiceWarehouseOneByOrgId(info.SWarehouseId.Value, User.OrgId, User.BranchId).ServicesWarehouseName),
                StateStatus = info.StateStatus,
                JobOrderId = info.JobOrderId,
                JobOrderCode = info.JobOrderCode,
                Remarks = info.Remarks,
                BranchId = info.BranchId,
                OrganizationId = info.OrganizationId,
                EUserId = info.EUserId,
                EntryDate = DateTime.Now,
            }).ToList();
            List<RequsitionInfoForJobOrderViewModel> requsitionInfoForJobs = new List<RequsitionInfoForJobOrderViewModel>();
            AutoMapper.Mapper.Map(requsitionInfoForJobOrderDTO, requsitionInfoForJobs);

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
            IEnumerable<RequsitionDetailForJobOrderDTO> requsitionDetailsDto = _requsitionDetailForJobOrderBusiness.GetAllRequsitionDetailForJobOrderId(requsitionInfoId.Value, User.OrgId,User.BranchId).Select(s => new RequsitionDetailForJobOrderDTO
            {
                RequsitionDetailForJobOrderId = s.RequsitionDetailForJobOrderId,
                PartsId=s.PartsId,
                PartsName= (_mobilePartBusiness.GetMobilePartOneByOrgId(s.PartsId.Value, User.OrgId).MobilePartName),
                SellPrice=s.SellPrice,
                CostPrice=s.CostPrice,
                Quantity = s.Quantity,
                Remarks = s.Remarks
            }).ToList();

            var info = _requsitionInfoForJobOrderBusiness.GetAllRequsitionInfoForJobOrderId(requsitionInfoId.Value, User.OrgId);
            var jobOrderInfo = _jobOrderBusiness.GetJobOrdersByIdWithBranch(info.JobOrderId.Value, User.BranchId, User.OrgId);
            ViewBag.ReqInfoJobOrder = new RequsitionInfoForJobOrderViewModel
            {
                RequsitionCode = info.RequsitionCode,
                JobOrderCode = (jobOrderInfo.JobOrderCode),
                Type = (_jobOrderBusiness.GetJobOrdersByIdWithBranch(info.JobOrderId.Value, User.BranchId, User.OrgId).Type),
                ModelName = (_descriptionBusiness.GetDescriptionOneByOrdId(jobOrderInfo.DescriptionId, User.OrgId).DescriptionName),
                EntryDate = info.EntryDate,
                SWarehouseName = (_servicesWarehouseBusiness.GetServiceWarehouseOneByOrgId(info.SWarehouseId.Value, User.OrgId, User.BranchId).ServicesWarehouseName),
            };
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
            return View();
        }
        public ActionResult TSRequsitionInfoForJobOrderPartialListList(string reqCode, long? warehouseId, string status, string fromDate, string toDate)
        {
            IEnumerable<RequsitionInfoForJobOrderDTO> requsitionInfoForJobOrderDTO = _requsitionInfoForJobOrderBusiness.GetAllRequsitionInfoForJob(User.OrgId, User.BranchId).Where(req =>
                (reqCode == null || reqCode.Trim() == "" || req.RequsitionCode.Contains(reqCode))
                &&
                (warehouseId == null || warehouseId <= 0 || req.SWarehouseId == warehouseId)
                &&
                (status == null || status.Trim() == "" || req.StateStatus == status.Trim())
                &&
                (
                    (fromDate == null && toDate == null)
                    ||
                     (fromDate == "" && toDate == "")
                    ||
                    (fromDate.Trim() != "" && toDate.Trim() != "" &&

                        req.EntryDate.Value.Date >= Convert.ToDateTime(fromDate).Date &&
                        req.EntryDate.Value.Date <= Convert.ToDateTime(toDate).Date)
                    ||
                    (fromDate.Trim() != "" && req.EntryDate.Value.Date == Convert.ToDateTime(fromDate).Date)
                    ||
                    (toDate.Trim() != "" && req.EntryDate.Value.Date == Convert.ToDateTime(toDate).Date)
                )).Select(info => new RequsitionInfoForJobOrderDTO
            {
                RequsitionInfoForJobOrderId = info.RequsitionInfoForJobOrderId,
                RequsitionCode = info.RequsitionCode,
                SWarehouseId = info.SWarehouseId,
                SWarehouseName = (_servicesWarehouseBusiness.GetServiceWarehouseOneByOrgId(info.SWarehouseId.Value, User.OrgId, User.BranchId).ServicesWarehouseName),
                StateStatus = info.StateStatus,
                JobOrderId = info.JobOrderId,
                JobOrderCode = info.JobOrderCode,
                Remarks = info.Remarks,
                BranchId = info.BranchId,
                OrganizationId = info.OrganizationId,
                EUserId = info.EUserId,
                EntryDate = info.EntryDate
            }).ToList();
            List<RequsitionInfoForJobOrderViewModel> requsitionInfoForJobs = new List<RequsitionInfoForJobOrderViewModel>();
            AutoMapper.Mapper.Map(requsitionInfoForJobOrderDTO, requsitionInfoForJobs);

            return PartialView("TSRequsitionInfoForJobOrderPartialListList", requsitionInfoForJobs);
        }
        public ActionResult TSRequsitionForJobOrderDetails(long? requsitionInfoId)
        {
            var info = _requsitionInfoForJobOrderBusiness.GetAllRequsitionInfoForJobOrderId(requsitionInfoId.Value, User.OrgId);
            var jobOrderInfo = _jobOrderBusiness.GetJobOrdersByIdWithBranch(info.JobOrderId.Value, User.BranchId, User.OrgId);
            ViewBag.ReqInfoJobOrder = new RequsitionInfoForJobOrderViewModel
            {
                RequsitionCode = info.RequsitionCode,
                JobOrderCode = (jobOrderInfo.JobOrderCode),
                Type = (_jobOrderBusiness.GetJobOrdersByIdWithBranch(info.JobOrderId.Value, User.BranchId, User.OrgId).Type),
                ModelName = (_descriptionBusiness.GetDescriptionOneByOrdId(jobOrderInfo.DescriptionId, User.OrgId).DescriptionName),

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
            IEnumerable<RequsitionDetailForJobOrderDTO> returnDTO = _requsitionDetailForJobOrderBusiness.GetAvailableQtyByRequsition(requsitionInfoId.Value, User.OrgId, User.BranchId);
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

            ViewBag.ddlMobilePart = _mobilePartBusiness.GetAllMobilePartByOrgId(User.OrgId).Select(mobile => new SelectListItem { Text = mobile.MobilePartName, Value = mobile.MobilePartId.ToString() }).ToList();

            ViewBag.ddlSymptomName = _clientProblemBusiness.GetAllClientProblemByOrgId(User.OrgId).Select(mobile => new SelectListItem { Text = mobile.ProblemName, Value = mobile.ProblemId.ToString() }).ToList();

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
                ModelName= (_descriptionBusiness.GetDescriptionOneByOrdId(jobOrder.DescriptionId, User.OrgId).DescriptionName),
                ModelColor=jobOrder.ModelColor,
                Remarks=jobOrder.Remarks,
                TSRemarks=jobOrder.TSRemarks
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
                RequsitionCode = (_requsitionInfoForJobOrderBusiness.GetAllRequsitionInfoOneByOrgId(stock.RequsitionInfoForJobOrderId, User.OrgId, User.BranchId).RequsitionCode),
                SWarehouseId = stock.SWarehouseId,
                SWarehouseName = (_servicesWarehouseBusiness.GetServiceWarehouseOneByOrgId(stock.SWarehouseId.Value, User.OrgId, User.BranchId).ServicesWarehouseName),
                PartsId = stock.PartsId,
                PartsName = (_mobilePartBusiness.GetMobilePartOneByOrgId(stock.PartsId.Value, User.OrgId).MobilePartName),
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
        #endregion

        #region Stock Return Part
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

                var JobOrderTsStock = _technicalServicesStockBusiness.GetStockByJobOrder(jobOrder.JodOrderId, User.UserId, User.OrgId, User.BranchId, User.RoleName).Select(s => new TechnicalServicesStockViewModel
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
            IEnumerable<InvoiceUsedPartsDTO> dto = _invoiceDetailBusiness.GetUsedPartsDetails(jobOrderId, User.OrgId, User.BranchId).Select(qty => new InvoiceUsedPartsDTO
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

    }
}