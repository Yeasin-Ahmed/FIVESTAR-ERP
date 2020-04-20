using ERPBLL.Production.Interface;
using ERPBO.Production.DTOModel;
using ERPBO.Production.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERPWeb.Controllers
{
    public class UserController : BaseController
    {
        // GET: User
        private readonly IProductionLineBusiness _productionLineBusiness;
        private readonly IRequsitionInfoBusiness _requsitionInfoBusiness;
        private readonly IFinishGoodsStockInfoBusiness _finishGoodsStockInfoBusiness;
        private readonly IFinishGoodsStockDetailBusiness _finishGoodsStockDetailBusiness;
        private readonly IItemReturnInfoBusiness _itemReturnInfoBusiness;

        private readonly long UserId = 1;
        private readonly long OrgId = 1;

        public UserController(IRequsitionInfoBusiness requsitionInfoBusiness, IFinishGoodsStockInfoBusiness finishGoodsStockInfoBusiness, IProductionLineBusiness productionLineBusiness, IFinishGoodsStockDetailBusiness finishGoodsStockDetailBusiness,IItemReturnInfoBusiness itemReturnInfoBusiness)
        {
            this._requsitionInfoBusiness = requsitionInfoBusiness;
            this._finishGoodsStockInfoBusiness = finishGoodsStockInfoBusiness;
            this._productionLineBusiness = productionLineBusiness;
            this._finishGoodsStockDetailBusiness = finishGoodsStockDetailBusiness;
            this._itemReturnInfoBusiness = itemReturnInfoBusiness;
        }
        public ActionResult Index()
        {
           // Requisition Summery
           IEnumerable<DashboardRequisitionSummeryDTO> dto = _requsitionInfoBusiness.DashboardRequisitionSummery(OrgId);
           IEnumerable<DashboardRequisitionSummeryViewModel> viewModel = new List<DashboardRequisitionSummeryViewModel>();
            AutoMapper.Mapper.Map(dto, viewModel);
            ViewBag.RequisitionSummery = viewModel;

            // Line wise daily Production
            IEnumerable<DashboardLineWiseProductionDTO> dashboardLineWises = _finishGoodsStockDetailBusiness.DashboardLineWiseDailyProduction(OrgId);
            IEnumerable<DashboardLineWiseProductionViewModel> dashboardLines = new List<DashboardLineWiseProductionViewModel>();
            AutoMapper.Mapper.Map(dashboardLineWises, dashboardLines);
            ViewBag.DashboardLineWiseProductionViewModel = dashboardLines;

            // Line wise OverAll Production
            IEnumerable<DashboardLineWiseProductionDTO> overalldto = _finishGoodsStockDetailBusiness.DashboardLineWiseOverAllProduction(OrgId);
            IEnumerable<DashboardLineWiseProductionViewModel> overallViews = new List<DashboardLineWiseProductionViewModel>();
            AutoMapper.Mapper.Map(overalldto, overallViews);
            ViewBag.DashboardLineWiseOverallProductionViewModel = overallViews;

            // Faculty daily wise Production
            IEnumerable<DashboardFacultyWiseProductionDTO> dailyFacultyDTO = _itemReturnInfoBusiness.DashboardFacultyDayWiseProduction(OrgId);
            IEnumerable<DashboardFacultyWiseProductionViewModel> dailyFacultyViews = new List<DashboardFacultyWiseProductionViewModel>();
            AutoMapper.Mapper.Map(dailyFacultyDTO, dailyFacultyViews);
            ViewBag.DashboardFacultyWiseProductionViewModel = dailyFacultyViews;

            // Faculty wise OveAll Production
            IEnumerable<DashboardFacultyWiseProductionDTO> OverAllFacultyDTO = _itemReturnInfoBusiness.DashboardFacultyOverAllWiseProduction(OrgId);
            IEnumerable<DashboardFacultyWiseProductionViewModel> OverAllFacultyViews = new List<DashboardFacultyWiseProductionViewModel>();
            AutoMapper.Mapper.Map(OverAllFacultyDTO, OverAllFacultyViews);
            ViewBag.DashboardFacultyWiseOverAllProductionViewModel = OverAllFacultyViews;

            return View();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}