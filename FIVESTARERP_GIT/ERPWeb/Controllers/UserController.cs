using ERPBLL.Common;
using ERPBLL.FrontDesk.Interface;
using ERPBLL.Production.Interface;
using ERPBO.Common;
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

        public UserController (IRequsitionInfoBusiness requsitionInfoBusiness, IFinishGoodsStockInfoBusiness finishGoodsStockInfoBusiness, IProductionLineBusiness productionLineBusiness, IFinishGoodsStockDetailBusiness finishGoodsStockDetailBusiness,IItemReturnInfoBusiness itemReturnInfoBusiness, IJobOrderBusiness jobOrderBusiness)
        {
            this._requsitionInfoBusiness = requsitionInfoBusiness;
            this._finishGoodsStockInfoBusiness = finishGoodsStockInfoBusiness;
            this._productionLineBusiness = productionLineBusiness;
            this._finishGoodsStockDetailBusiness = finishGoodsStockDetailBusiness;
            this._itemReturnInfoBusiness = itemReturnInfoBusiness;
            this._jobOrderBusiness = jobOrderBusiness;
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

                var reqRecheck = dto.FirstOrDefault(req => req.StateStatus == RequisitionStatus.Recheck);
                ViewBag.RequisitionRecheck = (reqRecheck == null) ? new DashboardRequisitionSummeryViewModel { StateStatus = "Recheck", TotalCount = 0 } : new DashboardRequisitionSummeryViewModel { StateStatus = reqRecheck.StateStatus, TotalCount = reqRecheck.TotalCount };

                var reqCancel = dto.FirstOrDefault(req => req.StateStatus == RequisitionStatus.Canceled);
                ViewBag.RequisitionCancel = (reqCancel == null) ? new DashboardRequisitionSummeryViewModel { StateStatus = "Cancel", TotalCount = 0 } : new DashboardRequisitionSummeryViewModel { StateStatus = reqCancel.StateStatus, TotalCount = reqCancel.TotalCount };

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

                return View();
            }
            else
            {
                IEnumerable<DashboardRequisitionSummeryDTO> dto = _jobOrderBusiness.DashboardJobOrderSummery(User.OrgId,User.BranchId);
                IEnumerable<DashboardRequisitionSummeryViewModel> viewModel = new List<DashboardRequisitionSummeryViewModel>();
                AutoMapper.Mapper.Map(dto, viewModel);
                ViewBag.JobOrderSummery = viewModel;

                var jobOrderPending = dto.FirstOrDefault(req => req.StateStatus == JobOrderStatus.PendingJobOrder);
                ViewBag.JobOrderPending = (jobOrderPending == null) ? new DashboardRequisitionSummeryViewModel { StateStatus = JobOrderStatus.PendingJobOrder, TotalCount = 0 } : new DashboardRequisitionSummeryViewModel { StateStatus = jobOrderPending.StateStatus, TotalCount = jobOrderPending.TotalCount };

                var jobOrderApproved = dto.FirstOrDefault(req => req.StateStatus == JobOrderStatus.CustomerApproved);
                ViewBag.JobOrderApproved = (jobOrderApproved == null) ? new DashboardRequisitionSummeryViewModel { StateStatus = JobOrderStatus.CustomerApproved, TotalCount = 0 } : new DashboardRequisitionSummeryViewModel { StateStatus = jobOrderApproved.StateStatus, TotalCount = jobOrderApproved.TotalCount };

                var jobOrderDisapproved = dto.FirstOrDefault(req => req.StateStatus == JobOrderStatus.CustomerDisapproved);
                ViewBag.JobOrderDisapproved = (jobOrderDisapproved == null) ? new DashboardRequisitionSummeryViewModel { StateStatus = JobOrderStatus.CustomerDisapproved, TotalCount = 0 } : new DashboardRequisitionSummeryViewModel { StateStatus = jobOrderDisapproved.StateStatus, TotalCount = jobOrderDisapproved.TotalCount };

                var jobOrdeAssignToTS = dto.FirstOrDefault(req => req.StateStatus == JobOrderStatus.AssignToTS);
                ViewBag.JobOrdeAssignToTS = (jobOrdeAssignToTS == null) ? new DashboardRequisitionSummeryViewModel { StateStatus = JobOrderStatus.AssignToTS, TotalCount = 0 } : new DashboardRequisitionSummeryViewModel { StateStatus = jobOrdeAssignToTS.StateStatus, TotalCount = jobOrdeAssignToTS.TotalCount };

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

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}