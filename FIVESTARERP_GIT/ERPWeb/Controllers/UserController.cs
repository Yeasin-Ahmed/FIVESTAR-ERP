using ERPBLL.Common;
using ERPBLL.Configuration.Interface;
using ERPBLL.FrontDesk.Interface;
using ERPBLL.Inventory.Interface;
using ERPBLL.Production.Interface;
using ERPBO.Common;
using ERPBO.FrontDesk.DTOModels;
using ERPBO.FrontDesk.ViewModels;
using ERPBO.Production.DTOModel;
using ERPBO.Production.ViewModels;
using ERPWeb.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERPWeb.Controllers
{
    [CustomAuthorize]
    public class UserController : BaseController
    {
        // GET: User
        private readonly IProductionLineBusiness _productionLineBusiness;
        private readonly IRequsitionInfoBusiness _requsitionInfoBusiness;
        private readonly IFinishGoodsStockInfoBusiness _finishGoodsStockInfoBusiness;
        private readonly IFinishGoodsStockDetailBusiness _finishGoodsStockDetailBusiness;
        private readonly IItemReturnInfoBusiness _itemReturnInfoBusiness;
        private readonly IJobOrderBusiness _jobOrderBusiness;
        private readonly IJobOrderTSBusiness _jobOrderTSBusiness;
        private readonly IRequsitionInfoForJobOrderBusiness _requsitionInfoForJobOrderBusiness;
        private readonly ITsStockReturnInfoBusiness _tsStockReturnInfoBusiness;
        private readonly ITsStockReturnDetailsBusiness _tsStockReturnDetailsBusiness;
        private readonly IMobilePartBusiness _mobilePartBusiness;
        private readonly IDescriptionBusiness _descriptionBusiness;

        public UserController (IRequsitionInfoBusiness requsitionInfoBusiness, IFinishGoodsStockInfoBusiness finishGoodsStockInfoBusiness, IProductionLineBusiness productionLineBusiness, IFinishGoodsStockDetailBusiness finishGoodsStockDetailBusiness,IItemReturnInfoBusiness itemReturnInfoBusiness, IJobOrderBusiness jobOrderBusiness, IJobOrderTSBusiness jobOrderTSBusiness, IRequsitionInfoForJobOrderBusiness requsitionInfoForJobOrderBusiness, ITsStockReturnInfoBusiness tsStockReturnInfoBusiness, ITsStockReturnDetailsBusiness tsStockReturnDetailsBusiness, IMobilePartBusiness mobilePartBusiness, IDescriptionBusiness descriptionBusiness)
        {
            this._requsitionInfoBusiness = requsitionInfoBusiness;
            this._finishGoodsStockInfoBusiness = finishGoodsStockInfoBusiness;
            this._productionLineBusiness = productionLineBusiness;
            this._finishGoodsStockDetailBusiness = finishGoodsStockDetailBusiness;
            this._itemReturnInfoBusiness = itemReturnInfoBusiness;
            this._jobOrderBusiness = jobOrderBusiness;
            this._jobOrderTSBusiness = jobOrderTSBusiness;
            this._requsitionInfoForJobOrderBusiness=requsitionInfoForJobOrderBusiness;
            this._tsStockReturnInfoBusiness = tsStockReturnInfoBusiness;
            this._tsStockReturnDetailsBusiness = tsStockReturnDetailsBusiness;
            this._mobilePartBusiness = mobilePartBusiness;
            this._descriptionBusiness = descriptionBusiness;
        }
        public ActionResult Index()
        {
            if(User.AppType == ApplicationType.ERP)
            {
                // Requisition Summery
                IEnumerable<DashboardRequisitionSummeryDTO> dto = _requsitionInfoBusiness.DashboardRequisitionSummery(User.OrgId);
                IEnumerable<DashboardRequisitionSummeryViewModel> viewModel = new List<DashboardRequisitionSummeryViewModel>();
                AutoMapper.Mapper.Map(dto, viewModel);
                ViewBag.RequisitionSummery = viewModel;
                // Requisition Status // YSN

                var reqAccepted = dto.FirstOrDefault(req => req.StateStatus == RequisitionStatus.Accepted);
                ViewBag.RequisitionAccepted = (reqAccepted == null) ? new DashboardRequisitionSummeryViewModel { StateStatus = "Accepted", TotalCount = 0 } : new DashboardRequisitionSummeryViewModel { StateStatus = reqAccepted.StateStatus, TotalCount = reqAccepted.TotalCount };

                var reqApproved = dto.FirstOrDefault(req => req.StateStatus == RequisitionStatus.Approved);
                ViewBag.RequisitionApproved = (reqApproved == null) ? new DashboardRequisitionSummeryViewModel { StateStatus = "Approved", TotalCount = 0 } : new DashboardRequisitionSummeryViewModel { StateStatus = reqApproved.StateStatus, TotalCount = reqApproved.TotalCount };

                var reqPending = dto.FirstOrDefault(req => req.StateStatus == RequisitionStatus.Pending);
                ViewBag.RequisitionPending = (reqPending == null) ? new DashboardRequisitionSummeryViewModel { StateStatus = "Pending", TotalCount = 0 } : new DashboardRequisitionSummeryViewModel { StateStatus = reqPending.StateStatus, TotalCount = reqPending.TotalCount };

                var reqRecheck = dto.FirstOrDefault(req => req.StateStatus == RequisitionStatus.Rechecked);
                ViewBag.RequisitionRecheck = (reqRecheck == null) ? new DashboardRequisitionSummeryViewModel { StateStatus = "Rechecked", TotalCount = 0 } : new DashboardRequisitionSummeryViewModel { StateStatus = reqRecheck.StateStatus, TotalCount = reqRecheck.TotalCount };

                var reqCancel = dto.FirstOrDefault(req => req.StateStatus == RequisitionStatus.Canceled);
                ViewBag.RequisitionCancel = (reqCancel == null) ? new DashboardRequisitionSummeryViewModel { StateStatus = "Canceled", TotalCount = 0 } : new DashboardRequisitionSummeryViewModel { StateStatus = reqCancel.StateStatus, TotalCount = reqCancel.TotalCount };

                var reqReject = dto.FirstOrDefault(req => req.StateStatus == RequisitionStatus.Rejected);
                ViewBag.RequisitionReject = (reqReject == null) ? new DashboardRequisitionSummeryViewModel { StateStatus = "Rejected", TotalCount = 0 } : new DashboardRequisitionSummeryViewModel { StateStatus = reqReject.StateStatus, TotalCount = reqReject.TotalCount };

                //--------------------//
                // Line wise daily Production
                IEnumerable<DashboardLineWiseProductionDTO> dashboardLineWises = _finishGoodsStockDetailBusiness.DashboardLineWiseDailyProduction(User.OrgId);
                IEnumerable<DashboardLineWiseProductionViewModel> dashboardLines = new List<DashboardLineWiseProductionViewModel>();
                AutoMapper.Mapper.Map(dashboardLineWises, dashboardLines);
                ViewBag.DashboardLineWiseProductionViewModel = dashboardLines;

                // Line wise OverAll Production
                IEnumerable<DashboardLineWiseProductionDTO> overalldto = _finishGoodsStockDetailBusiness.DashboardLineWiseOverAllProduction(User.OrgId);
                IEnumerable<DashboardLineWiseProductionViewModel> overallViews = new List<DashboardLineWiseProductionViewModel>();
                AutoMapper.Mapper.Map(overalldto, overallViews);
                ViewBag.DashboardLineWiseOverallProductionViewModel = overallViews;

                // Faculty daily wise Production
                IEnumerable<DashboardFacultyWiseProductionDTO> dailyFacultyDTO = _itemReturnInfoBusiness.DashboardFacultyDayWiseProduction(User.OrgId);
                IEnumerable<DashboardFacultyWiseProductionViewModel> dailyFacultyViews = new List<DashboardFacultyWiseProductionViewModel>();
                AutoMapper.Mapper.Map(dailyFacultyDTO, dailyFacultyViews);
                ViewBag.DashboardFacultyWiseProductionViewModel = dailyFacultyViews;

                // Faculty wise OveAll Production
                IEnumerable<DashboardFacultyWiseProductionDTO> OverAllFacultyDTO = _itemReturnInfoBusiness.DashboardFacultyOverAllWiseProduction(User.OrgId);
                IEnumerable<DashboardFacultyWiseProductionViewModel> OverAllFacultyViews = new List<DashboardFacultyWiseProductionViewModel>();
                AutoMapper.Mapper.Map(OverAllFacultyDTO, OverAllFacultyViews);
                ViewBag.DashboardFacultyWiseOverAllProductionViewModel = OverAllFacultyViews;

                #region Assembly Line Progress

                var assemblyProgressDto =_requsitionInfoBusiness.GetDashBoardAssemblyProgresses(null, null, User.OrgId);

                List<DashBoardAssemblyProgressViewModel> assemblyProgressViewModel = new List<DashBoardAssemblyProgressViewModel>();

                AutoMapper.Mapper.Map(assemblyProgressDto, assemblyProgressViewModel);
                ViewBag.AssemblyProgress = assemblyProgressViewModel;

                #endregion

                return View();
            }
            else
            {
                IEnumerable<DashboardRequisitionSummeryDTO> dto = _jobOrderBusiness.DashboardJobOrderSummery(User.OrgId,User.BranchId);
                IEnumerable<DashboardRequisitionSummeryViewModel> viewModel = new List<DashboardRequisitionSummeryViewModel>();
                AutoMapper.Mapper.Map(dto, viewModel);
                ViewBag.JobOrderSummery = viewModel;

                var repairDone = dto.FirstOrDefault(req => req.StateStatus == JobOrderStatus.RepairDone);
                ViewBag.RepairDone = (repairDone == null) ? new DashboardRequisitionSummeryViewModel { StateStatus = JobOrderStatus.RepairDone, TotalCount = 0 } : new DashboardRequisitionSummeryViewModel { StateStatus = repairDone.StateStatus, TotalCount = repairDone.TotalCount };

                var jobOrderApproved = dto.FirstOrDefault(req => req.StateStatus == JobOrderStatus.JobInitiated);
                ViewBag.JobOrderApproved = (jobOrderApproved == null) ? new DashboardRequisitionSummeryViewModel { StateStatus = JobOrderStatus.JobInitiated, TotalCount = 0 } : new DashboardRequisitionSummeryViewModel { StateStatus = jobOrderApproved.StateStatus, TotalCount = jobOrderApproved.TotalCount };

                var deliveryDone = dto.FirstOrDefault(req => req.StateStatus == JobOrderStatus.DeliveryDone);
                ViewBag.DeliveryDone = (deliveryDone == null) ? new DashboardRequisitionSummeryViewModel { StateStatus = JobOrderStatus.DeliveryDone, TotalCount = 0 } : new DashboardRequisitionSummeryViewModel { StateStatus = deliveryDone.StateStatus, TotalCount = deliveryDone.TotalCount };

                var jobOrdeAssignToTS = dto.FirstOrDefault(req => req.StateStatus == JobOrderStatus.AssignToTS);
                ViewBag.JobOrdeAssignToTS = (jobOrdeAssignToTS == null) ? new DashboardRequisitionSummeryViewModel { StateStatus = JobOrderStatus.AssignToTS, TotalCount = 0 } : new DashboardRequisitionSummeryViewModel { StateStatus = jobOrdeAssignToTS.StateStatus, TotalCount = jobOrdeAssignToTS.TotalCount };

                // Daily Job Orders
                IEnumerable<DashboardDailyReceiveJobOrderDTO> dashboardJobOrders = _jobOrderBusiness.DashboardDailyJobOrder(User.OrgId,User.BranchId);
                IEnumerable<DashboardDailyReceiveJobOrderViewModel> dashboardJob = new List<DashboardDailyReceiveJobOrderViewModel>();
                AutoMapper.Mapper.Map(dashboardJobOrders, dashboardJob);
                ViewBag.DashboardDailyJobOrderViewModel = dashboardJob;

                // Daily Sells//25-10-2020
                IEnumerable<DashboardSellsDTO> dailysellDTO = _jobOrderBusiness.DashboardDailySells(User.OrgId, User.BranchId);
                IEnumerable<DashboardSellsViewModel> dailyView = new List<DashboardSellsViewModel>();
                AutoMapper.Mapper.Map(dailysellDTO, dailyView);
                ViewBag.DashboardDailySellsViewModel = dailyView;

                // Total Sells//25-10-2020
                IEnumerable<DashboardSellsDTO> totalDTO = _jobOrderBusiness.DashboardTotalSells(User.OrgId, User.BranchId);
                IEnumerable<DashboardSellsViewModel> totalView = new List<DashboardSellsViewModel>();
                AutoMapper.Mapper.Map(totalDTO, totalView);
                ViewBag.DashboardTotalSellsViewModel = totalView;

                // Daily Job Transfer//25-10-2020
                IEnumerable<DashboardDailyReceiveJobOrderDTO> dailyTransferDTO = _jobOrderBusiness.DashboardDailyTransferJob(User.OrgId, User.BranchId);
                IEnumerable<DashboardDailyReceiveJobOrderViewModel> dailyTransferView = new List<DashboardDailyReceiveJobOrderViewModel>();
                AutoMapper.Mapper.Map(dailyTransferDTO, dailyTransferView);
                ViewBag.DashboardTotalTransferViewModel = dailyTransferView;

                // Daily Job Receive//25-10-2020
                IEnumerable<DashboardDailyReceiveJobOrderDTO> receiveJobDTO = _jobOrderBusiness.DashboardDailyReceiveJob(User.OrgId, User.BranchId);
                IEnumerable<DashboardDailyReceiveJobOrderViewModel> receiveJOBView = new List<DashboardDailyReceiveJobOrderViewModel>();
                AutoMapper.Mapper.Map(receiveJobDTO, receiveJOBView);
                ViewBag.DashboardTotalReceiveViewModel = receiveJOBView;

                // Daily Warranty and Billing Job
                IEnumerable<DashboardDailyBillingAndWarrantyJobDTO> andWarrantyJobDTO = _jobOrderBusiness.DashboardDailyBillingAndWarrantyJob(User.OrgId, User.BranchId);
                IEnumerable<DashboardDailyBillingAndWarrantyJobViewModel> andWarrantyJobViewModels = new List<DashboardDailyBillingAndWarrantyJobViewModel>();
                AutoMapper.Mapper.Map(andWarrantyJobDTO, andWarrantyJobViewModels);
                ViewBag.DashboardDailyWarrantyandBillingViewModel = andWarrantyJobViewModels;

                // Daily singIn Out
                IEnumerable<DashboardDailySingInAndOutDTO> singInAndOutDTO = _jobOrderTSBusiness.DashboardDailySingInAndOuts(User.OrgId, User.BranchId);
                IEnumerable<DashboardDailySingInAndOutViewModel> inAndOutViewModels = new List<DashboardDailySingInAndOutViewModel>();
                AutoMapper.Mapper.Map(singInAndOutDTO, inAndOutViewModels);
                ViewBag.DashboardDailySingInOutViewModel = inAndOutViewModels;

                // Daily Spare Parts Request
                IEnumerable<DashboardRequestSparePartsDTO> sparePartsDTO = _requsitionInfoForJobOrderBusiness.DashboardRequestSpareParts(User.OrgId, User.BranchId);
                IEnumerable<DashboardRequestSparePartsViewModel> sparePartsViewModels = new List<DashboardRequestSparePartsViewModel>();
                AutoMapper.Mapper.Map(sparePartsDTO, sparePartsViewModels);
                ViewBag.DashboardDailySparePartsViewModel = sparePartsViewModels;

                // Requsition Pending
                IEnumerable<DashboardApprovedRequsitionDTO> pendingRequsitionDTO = _jobOrderBusiness.DashboardPendingRequsition(User.OrgId, User.BranchId);
                IEnumerable<DashboardApprovedRequsitionViewModel> pendingRequsitionViewModels = new List<DashboardApprovedRequsitionViewModel>();
                AutoMapper.Mapper.Map(pendingRequsitionDTO, pendingRequsitionViewModels);
                ViewBag.DashboardPendingViewModel = pendingRequsitionViewModels;

                // Requsition Current
                IEnumerable<DashboardApprovedRequsitionDTO> currentRequsitionDTO = _jobOrderBusiness.DashboardCurrentRequsition(User.OrgId, User.BranchId);
                IEnumerable<DashboardApprovedRequsitionViewModel> currentRequsitionViewModels = new List<DashboardApprovedRequsitionViewModel>();
                AutoMapper.Mapper.Map(currentRequsitionDTO, currentRequsitionViewModels);
                ViewBag.DashboardCurrentViewModel = currentRequsitionViewModels;

                // Requsition Another Branch
                IEnumerable<DashboardApprovedRequsitionDTO> anotherBranchReqDTO = _jobOrderBusiness.DashboardAnotherBranchRequsition(User.OrgId, User.BranchId);
                IEnumerable<DashboardApprovedRequsitionViewModel> anotherBranchReqViewModels = new List<DashboardApprovedRequsitionViewModel>();
                AutoMapper.Mapper.Map(anotherBranchReqDTO, anotherBranchReqViewModels);
                ViewBag.AnotherBranchRequsition = anotherBranchReqViewModels;

                // Return Spareparts
                IEnumerable<DashbordTsPartsReturnDTO> returnDTO = _tsStockReturnInfoBusiness.DashboardReturnParts(User.OrgId, User.BranchId);
                IEnumerable<DashbordTsPartsReturnViewModel> returnViewModels = new List<DashbordTsPartsReturnViewModel>();
                AutoMapper.Mapper.Map(returnDTO, returnViewModels);
                ViewBag.ReturnPartsViewModel = returnViewModels;

                return View("Index2");
            }
        }

        [HttpPost,ValidateJsonAntiForgeryToken]
        public ActionResult GetChartData()
        {
            var data =_finishGoodsStockDetailBusiness.GetDayAndLineProductionChartsData(User.OrgId);
            var lineNumber = (from ln in data
                         select ln.LineNumber).Distinct().ToArray();

            var months = (from ln in data
                          select ln.Date).Distinct().ToArray();
            List<List<int>> charts = new List<List<int>>();

            foreach (var item in lineNumber)
            {
                var qtys = data.Where(c => c.LineNumber == item).Select(c => c.Quantity).ToList();
                charts.Add(qtys);
            }

            // Chart-2 
            var data2 = _finishGoodsStockDetailBusiness.GetDayAndModelWiseProductionChart(User.OrgId);
            var modelNames = (from ln in data2
                              select ln.ModelName).Distinct().ToArray();

            var months2 = (from ln in data
                          select ln.Date).Distinct().ToArray();
            List<List<int>> charts2 = new List<List<int>>();

            foreach (var item in modelNames)
            {
                var qtys = data2.Where(c => c.ModelName == item).Select(c => c.Quantity).ToList();
                charts2.Add(qtys);
            }
            
            return Json(new {lines= lineNumber, months= months, charts= charts, models= modelNames, charts2=charts2,months2= months2 });

            
        }
        [HttpPost,ValidateAntiForgeryToken]
       
        public ActionResult ReturnStockDetails(long id)
        {
            
            var info = _tsStockReturnInfoBusiness.GetAllReturnId(id, User.OrgId,User.BranchId);
            var jobOrderInfo = _jobOrderBusiness.GetJobOrdersByIdWithBranch(info.JobOrderId, User.BranchId, User.OrgId);
            var jobOrder = new RequsitionInfoForJobOrderViewModel();
            if(jobOrderInfo != null)
            {
                jobOrder = new RequsitionInfoForJobOrderViewModel
                {
                    RequsitionCode = info.RequsitionCode,
                    JobOrderCode = (jobOrderInfo.JobOrderCode),
                    Type = (_jobOrderBusiness.GetJobOrdersByIdWithBranch(info.JobOrderId, User.BranchId, User.OrgId).Type),
                    ModelName = (_descriptionBusiness.GetDescriptionOneByOrdId(jobOrderInfo.DescriptionId, User.OrgId).DescriptionName),
                    ModelColor= jobOrderInfo.ModelColor,
                    Requestby = UserForEachRecord(info.EUserId.Value).UserName,
                    EntryDate = info.EntryDate,
                };
            }
            ViewBag.ReqInfoJobOrder = jobOrder;

            IEnumerable<TsStockReturnDetailDTO> tsStock = _tsStockReturnDetailsBusiness.GetAllTsStockReturn(User.OrgId, User.BranchId).Where(s=> s.ReturnInfoId == id).Select(detail => new TsStockReturnDetailDTO
            {
                ReqInfoId = detail.ReturnInfoId,
                ReturnDetailId = detail.ReturnDetailId,
                PartsId = detail.PartsId,
                PartsName = (_mobilePartBusiness.GetMobilePartOneByOrgId(detail.PartsId, User.OrgId).MobilePartName),
                PartsCode = (_mobilePartBusiness.GetMobilePartOneByOrgId(detail.PartsId, User.OrgId).MobilePartCode),
                Quantity = detail.Quantity,
            }).ToList();
            List<TsStockReturnDetailViewModel> detailViewModels = new List<TsStockReturnDetailViewModel>();
            AutoMapper.Mapper.Map(tsStock, detailViewModels);
            return PartialView("ReturnStockDetails", detailViewModels);
        }
        public ActionResult GetDailySellsChart()
        {
            var data = _jobOrderBusiness.DailySellsChart(string.Empty, string.Empty, User.OrgId, User.BranchId);


            var days = data.Select(s => s.EntryDate).ToArray();
            var charts = data.Select(s => s.Sells).ToArray();
            return Json(new { days = days, charts = charts });

        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}